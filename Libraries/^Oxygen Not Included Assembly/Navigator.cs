// Decompiled with JetBrains decompiler
// Type: Navigator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Navigator : StateMachineComponent<Navigator.StatesInstance>, ISaveLoadableDetails, ISim4000ms
{
  private static readonly EventSystem.IntraObjectHandler<Navigator> OnDefeatedDelegate = new EventSystem.IntraObjectHandler<Navigator>((System.Action<Navigator, object>) ((component, data) => component.OnDefeated(data)));
  private static readonly EventSystem.IntraObjectHandler<Navigator> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Navigator>((System.Action<Navigator, object>) ((component, data) => component.OnRefreshUserMenu(data)));
  private static readonly EventSystem.IntraObjectHandler<Navigator> OnSelectObjectDelegate = new EventSystem.IntraObjectHandler<Navigator>((System.Action<Navigator, object>) ((component, data) => component.OnSelectObject(data)));
  private static readonly EventSystem.IntraObjectHandler<Navigator> OnStoreDelegate = new EventSystem.IntraObjectHandler<Navigator>((System.Action<Navigator, object>) ((component, data) => component.OnStore(data)));
  public float defaultSpeed = 1f;
  public Grid.SceneLayer sceneLayer = Grid.SceneLayer.Move;
  private int reservedCell = NavigationReservations.InvalidReservation;
  public bool DebugDrawPath;
  [MyCmpAdd]
  public PathProber PathProber;
  [MyCmpAdd]
  private Facing facing;
  public TransitionDriver transitionDriver;
  public string NavGridName;
  public bool updateProber;
  public int maxProbingRadius;
  public PathFinder.PotentialPath.Flags flags;
  private LoggerFS log;
  public Dictionary<NavType, int> distanceTravelledByNavType;
  private PathFinderAbilities abilities;
  [MyCmpReq]
  private KSelectable selectable;
  [NonSerialized]
  public PathFinder.Path path;
  public NavType CurrentNavType;
  private int AnchorCell;
  private KPrefabID targetLocator;
  private NavTactic tactic;
  public Navigator.PathProbeTask pathProbeTask;
  public bool executePathProbeTaskAsync;

  public KMonoBehaviour target { get; set; }

  public CellOffset[] targetOffsets { get; private set; }

  public NavGrid NavGrid { get; private set; }

  public void Serialize(BinaryWriter writer)
  {
    byte currentNavType = (byte) this.CurrentNavType;
    writer.Write(currentNavType);
    writer.Write(this.distanceTravelledByNavType.Count);
    foreach (KeyValuePair<NavType, int> keyValuePair in this.distanceTravelledByNavType)
    {
      byte key = (byte) keyValuePair.Key;
      writer.Write(key);
      writer.Write(keyValuePair.Value);
    }
  }

  public void Deserialize(IReader reader)
  {
    NavType navType = (NavType) reader.ReadByte();
    if (!SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 11))
    {
      int num1 = reader.ReadInt32();
      for (int index = 0; index < num1; ++index)
      {
        NavType key = (NavType) reader.ReadByte();
        int num2 = reader.ReadInt32();
        if (this.distanceTravelledByNavType.ContainsKey(key))
          this.distanceTravelledByNavType[key] = num2;
      }
    }
    bool flag = false;
    foreach (NavType validNavType in this.NavGrid.ValidNavTypes)
    {
      if (validNavType == navType)
      {
        flag = true;
        break;
      }
    }
    if (!flag)
      return;
    this.CurrentNavType = navType;
  }

  protected override void OnPrefabInit()
  {
    this.transitionDriver = new TransitionDriver(this);
    this.targetLocator = Util.KInstantiate(Assets.GetPrefab((Tag) TargetLocator.ID), (GameObject) null, (string) null).GetComponent<KPrefabID>();
    this.targetLocator.gameObject.SetActive(true);
    this.log = new LoggerFS(nameof (Navigator), 35);
    this.simRenderLoadBalance = true;
    this.autoRegisterSimRender = false;
    this.NavGrid = Pathfinding.Instance.GetNavGrid(this.NavGridName);
    this.GetComponent<PathProber>().SetValidNavTypes(this.NavGrid.ValidNavTypes, this.maxProbingRadius);
    this.distanceTravelledByNavType = new Dictionary<NavType, int>();
    for (int index = 0; index < 10; ++index)
      this.distanceTravelledByNavType.Add((NavType) index, 0);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Subscribe<Navigator>(1623392196, Navigator.OnDefeatedDelegate);
    this.Subscribe<Navigator>(-1506500077, Navigator.OnDefeatedDelegate);
    this.Subscribe<Navigator>(493375141, Navigator.OnRefreshUserMenuDelegate);
    this.Subscribe<Navigator>(-1503271301, Navigator.OnSelectObjectDelegate);
    this.Subscribe<Navigator>(856640610, Navigator.OnStoreDelegate);
    if (this.updateProber)
      SimAndRenderScheduler.instance.Add((object) this, false);
    this.pathProbeTask = new Navigator.PathProbeTask(this);
    this.SetCurrentNavType(this.CurrentNavType);
  }

  public bool IsMoving()
  {
    return this.smi.IsInsideState((StateMachine.BaseState) this.smi.sm.moving);
  }

  public bool GoTo(int cell, CellOffset[] offsets = null)
  {
    if (offsets == null)
      offsets = new CellOffset[1]{ new CellOffset() };
    this.targetLocator.transform.SetPosition(Grid.CellToPosCBC(cell, Grid.SceneLayer.Move));
    return this.GoTo((KMonoBehaviour) this.targetLocator, offsets, NavigationTactics.ReduceTravelDistance);
  }

  public bool GoTo(int cell, CellOffset[] offsets, NavTactic tactic)
  {
    if (offsets == null)
      offsets = new CellOffset[1]{ new CellOffset() };
    this.targetLocator.transform.SetPosition(Grid.CellToPosCBC(cell, Grid.SceneLayer.Move));
    return this.GoTo((KMonoBehaviour) this.targetLocator, offsets, tactic);
  }

  public void UpdateTarget(int cell)
  {
    this.targetLocator.transform.SetPosition(Grid.CellToPosCBC(cell, Grid.SceneLayer.Move));
  }

  public bool GoTo(KMonoBehaviour target, CellOffset[] offsets, NavTactic tactic)
  {
    if (tactic == null)
      tactic = NavigationTactics.ReduceTravelDistance;
    this.smi.GoTo((StateMachine.BaseState) this.smi.sm.moving);
    this.smi.sm.moveTarget.Set(target.gameObject, this.smi);
    this.tactic = tactic;
    this.target = target;
    this.targetOffsets = offsets;
    this.ClearReservedCell();
    this.AdvancePath(true);
    return this.IsMoving();
  }

  public void BeginTransition(NavGrid.Transition transition)
  {
    this.transitionDriver.EndTransition();
    this.smi.GoTo((StateMachine.BaseState) this.smi.sm.moving);
    this.transitionDriver.BeginTransition(this, new Navigator.ActiveTransition(transition, this.defaultSpeed));
  }

  private bool ValidatePath(ref PathFinder.Path path)
  {
    return PathFinder.ValidatePath(this.NavGrid, this.GetCurrentAbilities(), ref path);
  }

  public void AdvancePath(bool trigger_advance = true)
  {
    int cell1 = Grid.PosToCell((KMonoBehaviour) this);
    if ((UnityEngine.Object) this.target == (UnityEngine.Object) null)
    {
      this.Trigger(-766531887, (object) null);
      this.Stop(false);
    }
    else if (cell1 == this.reservedCell && this.CurrentNavType != NavType.Tube)
    {
      this.Stop(true);
    }
    else
    {
      int cell2 = Grid.PosToCell(this.target);
      bool flag;
      if (this.reservedCell == NavigationReservations.InvalidReservation)
        flag = true;
      else if (!this.CanReach(this.reservedCell))
        flag = true;
      else if (!Grid.IsCellOffsetOf(this.reservedCell, cell2, this.targetOffsets))
        flag = true;
      else if (this.path.IsValid())
      {
        if (cell1 == this.path.nodes[0].cell && this.CurrentNavType == this.path.nodes[0].navType)
          flag = !this.ValidatePath(ref this.path);
        else if (cell1 == this.path.nodes[1].cell && this.CurrentNavType == this.path.nodes[1].navType)
        {
          this.path.nodes.RemoveAt(0);
          flag = !this.ValidatePath(ref this.path);
        }
        else
          flag = true;
      }
      else
        flag = true;
      if (flag)
      {
        this.SetReservedCell(this.tactic.GetCellPreferences(cell2, this.targetOffsets, this));
        if (this.reservedCell == NavigationReservations.InvalidReservation)
          this.Stop(false);
        else
          PathFinder.UpdatePath(this.NavGrid, this.GetCurrentAbilities(), new PathFinder.PotentialPath(cell1, this.CurrentNavType, this.flags), (PathFinderQuery) PathFinderQueries.cellQuery.Reset(this.reservedCell), ref this.path);
      }
      if (this.path.IsValid())
      {
        this.BeginTransition(this.NavGrid.transitions[this.path.nodes[1].transitionId]);
        this.distanceTravelledByNavType[this.CurrentNavType] = Mathf.Max(this.distanceTravelledByNavType[this.CurrentNavType] + 1, this.distanceTravelledByNavType[this.CurrentNavType]);
      }
      else if (this.path.HasArrived())
      {
        this.Stop(true);
      }
      else
      {
        this.ClearReservedCell();
        this.Stop(false);
      }
    }
    if (!trigger_advance)
      return;
    this.Trigger(1347184327, (object) null);
  }

  public NavGrid.Transition GetNextTransition()
  {
    return this.NavGrid.transitions[this.path.nodes[1].transitionId];
  }

  public void Stop(bool arrived_at_destination = false)
  {
    this.target = (KMonoBehaviour) null;
    this.targetOffsets = (CellOffset[]) null;
    this.path.Clear();
    this.smi.sm.moveTarget.Set((KMonoBehaviour) null, this.smi);
    this.transitionDriver.EndTransition();
    this.GetComponent<KAnimControllerBase>().Play(this.NavGrid.GetIdleAnim(this.CurrentNavType), KAnim.PlayMode.Loop, 1f, 0.0f);
    if (arrived_at_destination)
    {
      this.smi.GoTo((StateMachine.BaseState) this.smi.sm.arrived);
    }
    else
    {
      if (this.smi.GetCurrentState() != this.smi.sm.moving)
        return;
      this.ClearReservedCell();
      this.smi.GoTo((StateMachine.BaseState) this.smi.sm.failed);
    }
  }

  private void Sim33ms(float dt)
  {
    if (!this.IsMoving())
      return;
    this.transitionDriver.UpdateTransition(dt);
  }

  public void Sim4000ms(float dt)
  {
    this.UpdateProbe(true);
  }

  public void UpdateProbe(bool forceUpdate = false)
  {
    if (!forceUpdate && this.executePathProbeTaskAsync)
      return;
    this.pathProbeTask.Update();
    this.pathProbeTask.Run((object) null);
  }

  public void DrawPath()
  {
    if (!this.gameObject.activeInHierarchy || !this.IsMoving())
      return;
    NavPathDrawer.Instance.DrawPath(this.GetComponent<KAnimControllerBase>().GetPivotSymbolPosition(), this.path);
  }

  private void OnDefeated(object data)
  {
    this.ClearReservedCell();
    this.Stop(false);
  }

  private void ClearReservedCell()
  {
    if (this.reservedCell == NavigationReservations.InvalidReservation)
      return;
    NavigationReservations.Instance.RemoveOccupancy(this.reservedCell);
    this.reservedCell = NavigationReservations.InvalidReservation;
  }

  private void SetReservedCell(int cell)
  {
    this.ClearReservedCell();
    this.reservedCell = cell;
    NavigationReservations.Instance.AddOccupancy(cell);
  }

  public int GetReservedCell()
  {
    return this.reservedCell;
  }

  public int GetAnchorCell()
  {
    return this.AnchorCell;
  }

  public bool IsValidNavType(NavType nav_type)
  {
    return this.NavGrid.HasNavTypeData(nav_type);
  }

  public void SetCurrentNavType(NavType nav_type)
  {
    this.CurrentNavType = nav_type;
    this.AnchorCell = NavTypeHelper.GetAnchorCell(nav_type, Grid.PosToCell((KMonoBehaviour) this));
    NavGrid.NavTypeData navTypeData = this.NavGrid.GetNavTypeData(this.CurrentNavType);
    KBatchedAnimController component = this.GetComponent<KBatchedAnimController>();
    Vector2 one = Vector2.one;
    if (navTypeData.flipX)
      one.x = -1f;
    if (navTypeData.flipY)
      one.y = -1f;
    component.navMatrix = Matrix2x3.Translate(navTypeData.animControllerOffset * 200f) * Matrix2x3.Rotate(navTypeData.rotation) * Matrix2x3.Scale(one);
  }

  private void OnRefreshUserMenu(object data)
  {
    if (this.gameObject.HasTag(GameTags.Dead))
      return;
    Game.Instance.userMenu.AddButton(this.gameObject, !((UnityEngine.Object) NavPathDrawer.Instance.GetNavigator() != (UnityEngine.Object) this) ? new KIconButtonMenu.ButtonInfo("action_navigable_regions", (string) UI.USERMENUACTIONS.DRAWPATHS.NAME_OFF, new System.Action(this.OnDrawPaths), Action.NumActions, (System.Action<GameObject>) null, (System.Action<KIconButtonMenu.ButtonInfo>) null, (Texture) null, (string) UI.USERMENUACTIONS.DRAWPATHS.TOOLTIP_OFF, true) : new KIconButtonMenu.ButtonInfo("action_navigable_regions", (string) UI.USERMENUACTIONS.DRAWPATHS.NAME, new System.Action(this.OnDrawPaths), Action.NumActions, (System.Action<GameObject>) null, (System.Action<KIconButtonMenu.ButtonInfo>) null, (Texture) null, (string) UI.USERMENUACTIONS.DRAWPATHS.TOOLTIP, true), 0.1f);
    Game.Instance.userMenu.AddButton(this.gameObject, new KIconButtonMenu.ButtonInfo("action_follow_cam", (string) UI.USERMENUACTIONS.FOLLOWCAM.NAME, new System.Action(this.OnFollowCam), Action.NumActions, (System.Action<GameObject>) null, (System.Action<KIconButtonMenu.ButtonInfo>) null, (Texture) null, (string) UI.USERMENUACTIONS.FOLLOWCAM.TOOLTIP, true), 0.3f);
  }

  private void OnFollowCam()
  {
    if ((UnityEngine.Object) CameraController.Instance.followTarget == (UnityEngine.Object) this.transform)
      CameraController.Instance.ClearFollowTarget();
    else
      CameraController.Instance.SetFollowTarget(this.transform);
  }

  private void OnDrawPaths()
  {
    if ((UnityEngine.Object) NavPathDrawer.Instance.GetNavigator() != (UnityEngine.Object) this)
      NavPathDrawer.Instance.SetNavigator(this);
    else
      NavPathDrawer.Instance.ClearNavigator();
  }

  private void OnSelectObject(object data)
  {
    NavPathDrawer.Instance.ClearNavigator();
  }

  public void OnStore(object data)
  {
    if (!(data is Storage) && (data == null || !(bool) data))
      return;
    this.Stop(false);
  }

  public PathFinderAbilities GetCurrentAbilities()
  {
    this.abilities.Refresh();
    return this.abilities;
  }

  public void SetAbilities(PathFinderAbilities abilities)
  {
    this.abilities = abilities;
  }

  public bool CanReach(IApproachable approachable)
  {
    return this.CanReach(approachable.GetCell(), approachable.GetOffsets());
  }

  public bool CanReach(int cell, CellOffset[] offsets)
  {
    foreach (CellOffset offset in offsets)
    {
      if (this.CanReach(Grid.OffsetCell(cell, offset)))
        return true;
    }
    return false;
  }

  public bool CanReach(int cell)
  {
    return this.GetNavigationCost(cell) != -1;
  }

  public int GetNavigationCost(int cell)
  {
    if (Grid.IsValidCell(cell))
      return this.PathProber.GetCost(cell);
    return -1;
  }

  public int GetNavigationCostIgnoreProberOffset(int cell, CellOffset[] offsets)
  {
    return this.PathProber.GetNavigationCostIgnoreProberOffset(cell, offsets);
  }

  public int GetNavigationCost(int cell, CellOffset[] offsets)
  {
    int num = -1;
    foreach (CellOffset offset in offsets)
    {
      int navigationCost = this.GetNavigationCost(Grid.OffsetCell(cell, offset));
      if (num == -1)
        num = navigationCost;
      else if (navigationCost != -1 && navigationCost < num)
        num = navigationCost;
    }
    return num;
  }

  public int GetNavigationCost(IApproachable approachable)
  {
    return this.GetNavigationCost(approachable.GetCell(), approachable.GetOffsets());
  }

  public void RunQuery(PathFinderQuery query)
  {
    PathFinder.Run(this.NavGrid, this.GetCurrentAbilities(), new PathFinder.PotentialPath(Grid.PosToCell((KMonoBehaviour) this), this.CurrentNavType, this.flags), query);
  }

  public void SetFlags(PathFinder.PotentialPath.Flags new_flags)
  {
    this.flags |= new_flags;
  }

  public void ClearFlags(PathFinder.PotentialPath.Flags new_flags)
  {
    this.flags &= ~new_flags;
  }

  public class ActiveTransition
  {
    public float animSpeed = 1f;
    public int x;
    public int y;
    public bool isLooping;
    public NavType start;
    public NavType end;
    public HashedString preAnim;
    public HashedString anim;
    public float speed;
    public Func<bool> isCompleteCB;
    public NavGrid.Transition navGridTransition;

    public ActiveTransition(NavGrid.Transition transition, float default_speed)
    {
      this.x = (int) transition.x;
      this.y = (int) transition.y;
      this.isLooping = transition.isLooping;
      this.start = transition.start;
      this.end = transition.end;
      this.preAnim = (HashedString) transition.preAnim;
      this.anim = (HashedString) transition.anim;
      this.speed = default_speed;
      this.navGridTransition = transition;
    }
  }

  public class StatesInstance : GameStateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.GameInstance
  {
    public StatesInstance(Navigator master)
      : base(master)
    {
    }
  }

  public class States : GameStateMachine<Navigator.States, Navigator.StatesInstance, Navigator>
  {
    public StateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.TargetParameter moveTarget;
    public GameStateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.State moving;
    public GameStateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.State arrived;
    public GameStateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.State failed;
    public GameStateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.State stopped;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.stopped;
      this.saveHistory = true;
      this.moving.Enter((StateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.State.Callback) (smi => smi.Trigger(1027377649, (object) GameHashes.ObjectMovementWakeUp))).Update("UpdateNavigator", (System.Action<Navigator.StatesInstance, float>) ((smi, dt) => smi.master.Sim33ms(dt)), UpdateRate.SIM_33ms, true).Exit((StateMachine<Navigator.States, Navigator.StatesInstance, Navigator, object>.State.Callback) (smi => smi.Trigger(1027377649, (object) GameHashes.ObjectMovementSleep)));
      this.arrived.TriggerOnEnter(GameHashes.DestinationReached, (Func<Navigator.StatesInstance, object>) null).GoTo(this.stopped);
      this.failed.TriggerOnEnter(GameHashes.NavigationFailed, (Func<Navigator.StatesInstance, object>) null).GoTo(this.stopped);
      this.stopped.DoNothing();
    }
  }

  public struct PathProbeTask : IWorkItem<object>
  {
    private int cell;
    private Navigator navigator;

    public PathProbeTask(Navigator navigator)
    {
      this.navigator = navigator;
      this.cell = -1;
    }

    public void Update()
    {
      this.cell = Grid.PosToCell((KMonoBehaviour) this.navigator);
      this.navigator.abilities.Refresh();
    }

    public void Run(object sharedData)
    {
      this.navigator.PathProber.UpdateProbe(this.navigator.NavGrid, this.cell, this.navigator.CurrentNavType, this.navigator.abilities, this.navigator.flags);
    }
  }
}
