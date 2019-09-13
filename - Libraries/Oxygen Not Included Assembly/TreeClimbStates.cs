// Decompiled with JetBrains decompiler
// Type: TreeClimbStates
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using UnityEngine;

public class TreeClimbStates : GameStateMachine<TreeClimbStates, TreeClimbStates.Instance, IStateMachineTarget, TreeClimbStates.Def>
{
  public GameStateMachine<TreeClimbStates, TreeClimbStates.Instance, IStateMachineTarget, TreeClimbStates.Def>.ApproachSubState<Uprootable> moving;
  public TreeClimbStates.ClimbState climbing;
  public GameStateMachine<TreeClimbStates, TreeClimbStates.Instance, IStateMachineTarget, TreeClimbStates.Def>.State behaviourcomplete;
  public StateMachine<TreeClimbStates, TreeClimbStates.Instance, IStateMachineTarget, TreeClimbStates.Def>.TargetParameter target;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.moving;
    GameStateMachine<TreeClimbStates, TreeClimbStates.Instance, IStateMachineTarget, TreeClimbStates.Def>.State root = this.root;
    // ISSUE: reference to a compiler-generated field
    if (TreeClimbStates.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      TreeClimbStates.\u003C\u003Ef__mg\u0024cache0 = new StateMachine<TreeClimbStates, TreeClimbStates.Instance, IStateMachineTarget, TreeClimbStates.Def>.State.Callback(TreeClimbStates.SetTarget);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<TreeClimbStates, TreeClimbStates.Instance, IStateMachineTarget, TreeClimbStates.Def>.State.Callback fMgCache0 = TreeClimbStates.\u003C\u003Ef__mg\u0024cache0;
    GameStateMachine<TreeClimbStates, TreeClimbStates.Instance, IStateMachineTarget, TreeClimbStates.Def>.State state1 = root.Enter(fMgCache0).Enter((StateMachine<TreeClimbStates, TreeClimbStates.Instance, IStateMachineTarget, TreeClimbStates.Def>.State.Callback) (smi =>
    {
      if (TreeClimbStates.ReserveClimbable(smi))
        return;
      smi.GoTo((StateMachine.BaseState) this.behaviourcomplete);
    }));
    // ISSUE: reference to a compiler-generated field
    if (TreeClimbStates.\u003C\u003Ef__mg\u0024cache1 == null)
    {
      // ISSUE: reference to a compiler-generated field
      TreeClimbStates.\u003C\u003Ef__mg\u0024cache1 = new StateMachine<TreeClimbStates, TreeClimbStates.Instance, IStateMachineTarget, TreeClimbStates.Def>.State.Callback(TreeClimbStates.UnreserveClimbable);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<TreeClimbStates, TreeClimbStates.Instance, IStateMachineTarget, TreeClimbStates.Def>.State.Callback fMgCache1 = TreeClimbStates.\u003C\u003Ef__mg\u0024cache1;
    GameStateMachine<TreeClimbStates, TreeClimbStates.Instance, IStateMachineTarget, TreeClimbStates.Def>.State state2 = state1.Exit(fMgCache1);
    string name1 = (string) CREATURES.STATUSITEMS.RUMMAGINGSEED.NAME;
    string tooltip1 = (string) CREATURES.STATUSITEMS.RUMMAGINGSEED.TOOLTIP;
    StatusItemCategory main = Db.Get().StatusItemCategories.Main;
    string name2 = name1;
    string tooltip2 = tooltip1;
    string empty = string.Empty;
    HashedString render_overlay = new HashedString();
    StatusItemCategory category = main;
    state2.ToggleStatusItem(name2, tooltip2, empty, StatusItem.IconType.Info, (NotificationType) 0, false, render_overlay, 0, (Func<string, TreeClimbStates.Instance, string>) null, (Func<string, TreeClimbStates.Instance, string>) null, category);
    GameStateMachine<TreeClimbStates, TreeClimbStates.Instance, IStateMachineTarget, TreeClimbStates.Def>.ApproachSubState<Uprootable> moving = this.moving;
    // ISSUE: reference to a compiler-generated field
    if (TreeClimbStates.\u003C\u003Ef__mg\u0024cache2 == null)
    {
      // ISSUE: reference to a compiler-generated field
      TreeClimbStates.\u003C\u003Ef__mg\u0024cache2 = new Func<TreeClimbStates.Instance, int>(TreeClimbStates.GetClimbableCell);
    }
    // ISSUE: reference to a compiler-generated field
    Func<TreeClimbStates.Instance, int> fMgCache2 = TreeClimbStates.\u003C\u003Ef__mg\u0024cache2;
    TreeClimbStates.ClimbState climbing = this.climbing;
    moving.MoveTo(fMgCache2, (GameStateMachine<TreeClimbStates, TreeClimbStates.Instance, IStateMachineTarget, TreeClimbStates.Def>.State) climbing, (GameStateMachine<TreeClimbStates, TreeClimbStates.Instance, IStateMachineTarget, TreeClimbStates.Def>.State) null, false);
    this.climbing.DefaultState(this.climbing.pre);
    this.climbing.pre.PlayAnim("rummage_pre").OnAnimQueueComplete(this.climbing.loop);
    GameStateMachine<TreeClimbStates, TreeClimbStates.Instance, IStateMachineTarget, TreeClimbStates.Def>.State state3 = this.climbing.loop.QueueAnim("rummage_loop", true, (Func<TreeClimbStates.Instance, string>) null).ScheduleGoTo(3.5f, (StateMachine.BaseState) this.climbing.pst);
    // ISSUE: reference to a compiler-generated field
    if (TreeClimbStates.\u003C\u003Ef__mg\u0024cache3 == null)
    {
      // ISSUE: reference to a compiler-generated field
      TreeClimbStates.\u003C\u003Ef__mg\u0024cache3 = new System.Action<TreeClimbStates.Instance, float>(TreeClimbStates.Rummage);
    }
    // ISSUE: reference to a compiler-generated field
    System.Action<TreeClimbStates.Instance, float> fMgCache3 = TreeClimbStates.\u003C\u003Ef__mg\u0024cache3;
    state3.Update(fMgCache3, UpdateRate.SIM_1000ms, false);
    this.climbing.pst.QueueAnim("rummage_pst", false, (Func<TreeClimbStates.Instance, string>) null).OnAnimQueueComplete(this.behaviourcomplete);
    this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.WantsToClimbTree, false);
  }

  private static void SetTarget(TreeClimbStates.Instance smi)
  {
    smi.sm.target.Set(smi.GetSMI<ClimbableTreeMonitor.Instance>().climbTarget, smi);
  }

  private static bool ReserveClimbable(TreeClimbStates.Instance smi)
  {
    GameObject go = smi.sm.target.Get(smi);
    if (!((UnityEngine.Object) go != (UnityEngine.Object) null) || go.HasTag(GameTags.Creatures.ReservedByCreature))
      return false;
    go.AddTag(GameTags.Creatures.ReservedByCreature);
    return true;
  }

  private static void UnreserveClimbable(TreeClimbStates.Instance smi)
  {
    GameObject go = smi.sm.target.Get(smi);
    if (!((UnityEngine.Object) go != (UnityEngine.Object) null))
      return;
    go.RemoveTag(GameTags.Creatures.ReservedByCreature);
  }

  private static void Rummage(TreeClimbStates.Instance smi, float dt)
  {
    GameObject gameObject1 = smi.sm.target.Get(smi);
    if (!((UnityEngine.Object) gameObject1 != (UnityEngine.Object) null))
      return;
    BuddingTrunk component1 = gameObject1.GetComponent<BuddingTrunk>();
    if ((bool) ((UnityEngine.Object) component1))
    {
      component1.ExtractExtraSeed();
    }
    else
    {
      Storage component2 = gameObject1.GetComponent<Storage>();
      if (!(bool) ((UnityEngine.Object) component2) || component2.items.Count <= 0)
        return;
      int index = UnityEngine.Random.Range(0, component2.items.Count - 1);
      GameObject gameObject2 = component2.items[index];
      Pickupable pu = !(bool) ((UnityEngine.Object) gameObject2) ? (Pickupable) null : gameObject2.GetComponent<Pickupable>();
      if (!(bool) ((UnityEngine.Object) pu) || (double) pu.UnreservedAmount <= 0.00999999977648258)
        return;
      smi.Toss(pu);
    }
  }

  private static int GetClimbableCell(TreeClimbStates.Instance smi)
  {
    return Grid.PosToCell(smi.sm.target.Get(smi));
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public class Instance : GameStateMachine<TreeClimbStates, TreeClimbStates.Instance, IStateMachineTarget, TreeClimbStates.Def>.GameInstance
  {
    private static readonly Vector2 VEL_MIN = new Vector2(-1f, 2f);
    private static readonly Vector2 VEL_MAX = new Vector2(1f, 4f);
    private Storage storage;

    public Instance(Chore<TreeClimbStates.Instance> chore, TreeClimbStates.Def def)
      : base((IStateMachineTarget) chore, def)
    {
      chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, (object) GameTags.Creatures.WantsToClimbTree);
      this.storage = this.GetComponent<Storage>();
    }

    public void Toss(Pickupable pu)
    {
      Pickupable pickupable = pu.Take(Mathf.Min(1f, pu.UnreservedAmount));
      if (!((UnityEngine.Object) pickupable != (UnityEngine.Object) null))
        return;
      this.storage.Store(pickupable.gameObject, true, false, true, false);
      this.storage.Drop(pickupable.gameObject, true);
      this.Throw(pickupable.gameObject);
    }

    private void Throw(GameObject ore_go)
    {
      Vector3 position = this.transform.GetPosition();
      position.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
      int cell1 = Grid.PosToCell(position);
      int cell2 = Grid.CellAbove(cell1);
      Vector2 initial_velocity;
      if (Grid.IsValidCell(cell1) && Grid.Solid[cell1] || Grid.IsValidCell(cell2) && Grid.Solid[cell2])
      {
        initial_velocity = Vector2.zero;
      }
      else
      {
        position.y += 0.5f;
        initial_velocity = new Vector2(UnityEngine.Random.Range(TreeClimbStates.Instance.VEL_MIN.x, TreeClimbStates.Instance.VEL_MAX.x), UnityEngine.Random.Range(TreeClimbStates.Instance.VEL_MIN.y, TreeClimbStates.Instance.VEL_MAX.y));
      }
      ore_go.transform.SetPosition(position);
      if (GameComps.Fallers.Has((object) ore_go))
        GameComps.Fallers.Remove(ore_go);
      GameComps.Fallers.Add(ore_go, initial_velocity);
    }
  }

  public class ClimbState : GameStateMachine<TreeClimbStates, TreeClimbStates.Instance, IStateMachineTarget, TreeClimbStates.Def>.State
  {
    public GameStateMachine<TreeClimbStates, TreeClimbStates.Instance, IStateMachineTarget, TreeClimbStates.Def>.State pre;
    public GameStateMachine<TreeClimbStates, TreeClimbStates.Instance, IStateMachineTarget, TreeClimbStates.Def>.State loop;
    public GameStateMachine<TreeClimbStates, TreeClimbStates.Instance, IStateMachineTarget, TreeClimbStates.Def>.State pst;
  }
}
