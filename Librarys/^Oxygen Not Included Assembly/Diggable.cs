// Decompiled with JetBrains decompiler
// Type: Diggable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TUNING;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class Diggable : Workable
{
  private static List<Tuple<string, Tag>> lasersForHardness = new List<Tuple<string, Tag>>()
  {
    new Tuple<string, Tag>("dig", (Tag) "fx_dig_splash"),
    new Tuple<string, Tag>("specialistdig", (Tag) "fx_dig_splash")
  };
  private static readonly EventSystem.IntraObjectHandler<Diggable> OnReachableChangedDelegate = new EventSystem.IntraObjectHandler<Diggable>((System.Action<Diggable, object>) ((component, data) => component.OnReachableChanged(data)));
  private static readonly EventSystem.IntraObjectHandler<Diggable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Diggable>((System.Action<Diggable, object>) ((component, data) => component.OnRefreshUserMenu(data)));
  private HandleVector<int>.Handle partitionerEntry;
  private HandleVector<int>.Handle unstableEntry;
  private MeshRenderer childRenderer;
  private bool isReachable;
  private Element originalDigElement;
  [MyCmpAdd]
  private Prioritizable prioritizable;
  [SerializeField]
  public HashedString choreTypeIdHash;
  [SerializeField]
  public Material[] materials;
  [SerializeField]
  public MeshRenderer materialDisplay;
  private bool isDigComplete;
  private int handle;
  public Chore chore;

  private Diggable()
  {
    this.SetOffsetTable(OffsetGroups.InvertedStandardTableWithCorners);
  }

  public bool Reachable
  {
    get
    {
      return this.isReachable;
    }
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.workerStatusItem = Db.Get().DuplicantStatusItems.Digging;
    this.readyForSkillWorkStatusItem = Db.Get().BuildingStatusItems.DigRequiresSkillPerk;
    this.faceTargetWhenWorking = true;
    this.Subscribe<Diggable>(-1432940121, Diggable.OnReachableChangedDelegate);
    this.attributeConverter = Db.Get().AttributeConverters.DiggingSpeed;
    this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
    this.skillExperienceSkillGroup = Db.Get().SkillGroups.Mining.Id;
    this.skillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
    this.multitoolContext = (HashedString) "dig";
    this.multitoolHitEffectTag = (Tag) "fx_dig_splash";
    this.workingPstComplete = HashedString.Invalid;
    this.workingPstFailed = HashedString.Invalid;
    Prioritizable.AddRef(this.gameObject);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    int cell = Grid.PosToCell((KMonoBehaviour) this);
    this.originalDigElement = Grid.Element[cell];
    this.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().MiscStatusItems.WaitingForDig, (object) null);
    this.UpdateColor(this.isReachable);
    Grid.Objects[cell, 7] = this.gameObject;
    ChoreType chore_type = Db.Get().ChoreTypes.Dig;
    if (this.choreTypeIdHash.IsValid)
      chore_type = Db.Get().ChoreTypes.GetByHash(this.choreTypeIdHash);
    this.chore = (Chore) new WorkChore<Diggable>(chore_type, (IStateMachineTarget) this, (ChoreProvider) null, true, (System.Action<Chore>) null, (System.Action<Chore>) null, (System.Action<Chore>) null, true, (ScheduleBlockType) null, false, true, (KAnimFile) null, true, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
    this.SetWorkTime(float.PositiveInfinity);
    this.partitionerEntry = GameScenePartitioner.Instance.Add("Diggable.OnSpawn", (object) this.gameObject, Grid.PosToCell((KMonoBehaviour) this), GameScenePartitioner.Instance.solidChangedLayer, new System.Action<object>(this.OnSolidChanged));
    this.OnSolidChanged((object) null);
    new ReachabilityMonitor.Instance((Workable) this).StartSM();
    this.Subscribe<Diggable>(493375141, Diggable.OnRefreshUserMenuDelegate);
    this.handle = Game.Instance.Subscribe(-1523247426, new System.Action<object>(((Workable) this).UpdateStatusItem));
    Components.Diggables.Add(this);
  }

  public override Workable.AnimInfo GetAnim(Worker worker)
  {
    Workable.AnimInfo animInfo = new Workable.AnimInfo();
    if (this.overrideAnims != null && this.overrideAnims.Length > 0)
      animInfo.overrideAnims = this.overrideAnims;
    if (this.multitoolContext.IsValid && this.multitoolHitEffectTag.IsValid)
      animInfo.smi = (StateMachine.Instance) new MultitoolController.Instance((Workable) this, worker, this.multitoolContext, Assets.GetPrefab(this.multitoolHitEffectTag));
    return animInfo;
  }

  private static bool IsCellBuildable(int cell)
  {
    bool flag = false;
    GameObject gameObject = Grid.Objects[cell, 1];
    if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null && (UnityEngine.Object) gameObject.GetComponent<Constructable>() != (UnityEngine.Object) null)
      flag = true;
    return flag;
  }

  [DebuggerHidden]
  private IEnumerator PeriodicUnstableFallingRecheck()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new Diggable.\u003CPeriodicUnstableFallingRecheck\u003Ec__Iterator0()
    {
      \u0024this = this
    };
  }

  private void OnSolidChanged(object data)
  {
    if ((UnityEngine.Object) this == (UnityEngine.Object) null || (UnityEngine.Object) this.gameObject == (UnityEngine.Object) null)
      return;
    GameScenePartitioner.Instance.Free(ref this.unstableEntry);
    int cell = Grid.PosToCell((KMonoBehaviour) this);
    int num = -1;
    this.UpdateColor(this.isReachable);
    if (Grid.Element[cell].hardness >= (byte) 200)
    {
      bool flag = false;
      foreach (Chore.PreconditionInstance precondition in this.chore.GetPreconditions())
      {
        if (precondition.id == ChorePreconditions.instance.HasSkillPerk.id)
        {
          flag = true;
          break;
        }
      }
      if (!flag)
        this.chore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, (object) Db.Get().SkillPerks.CanDigSupersuperhard);
      this.requiredSkillPerk = Db.Get().SkillPerks.CanDigSupersuperhard.Id;
      this.materialDisplay.sharedMaterial = this.materials[3];
    }
    else if (Grid.Element[cell].hardness >= (byte) 150)
    {
      bool flag = false;
      foreach (Chore.PreconditionInstance precondition in this.chore.GetPreconditions())
      {
        if (precondition.id == ChorePreconditions.instance.HasSkillPerk.id)
        {
          flag = true;
          break;
        }
      }
      if (!flag)
        this.chore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, (object) Db.Get().SkillPerks.CanDigNearlyImpenetrable);
      this.requiredSkillPerk = Db.Get().SkillPerks.CanDigNearlyImpenetrable.Id;
      this.materialDisplay.sharedMaterial = this.materials[2];
    }
    else if (Grid.Element[cell].hardness >= (byte) 50)
    {
      bool flag = false;
      foreach (Chore.PreconditionInstance precondition in this.chore.GetPreconditions())
      {
        if (precondition.id == ChorePreconditions.instance.HasSkillPerk.id)
        {
          flag = true;
          break;
        }
      }
      if (!flag)
        this.chore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, (object) Db.Get().SkillPerks.CanDigVeryFirm);
      this.requiredSkillPerk = Db.Get().SkillPerks.CanDigVeryFirm.Id;
      this.materialDisplay.sharedMaterial = this.materials[1];
    }
    else
    {
      this.requiredSkillPerk = (string) null;
      this.chore.GetPreconditions().Remove(this.chore.GetPreconditions().Find((Predicate<Chore.PreconditionInstance>) (o => o.id == ChorePreconditions.instance.HasSkillPerk.id)));
    }
    this.UpdateStatusItem((object) null);
    bool flag1 = false;
    if (!Grid.Solid[cell])
    {
      num = Diggable.GetUnstableCellAbove(cell);
      if (num == -1)
        flag1 = true;
      else
        this.StartCoroutine("PeriodicUnstableFallingRecheck");
    }
    else if (Grid.Foundation[cell])
      flag1 = true;
    if (flag1)
    {
      this.isDigComplete = true;
      if (this.chore == null || !this.chore.InProgress())
        Util.KDestroyGameObject(this.gameObject);
      else
        this.GetComponentInChildren<MeshRenderer>().enabled = false;
    }
    else
    {
      if (num == -1)
        return;
      Extents extents = new Extents();
      Grid.CellToXY(cell, out extents.x, out extents.y);
      extents.width = 1;
      extents.height = (num - cell + Grid.WidthInCells - 1) / Grid.WidthInCells + 1;
      this.unstableEntry = GameScenePartitioner.Instance.Add("Diggable.OnSolidChanged", (object) this.gameObject, extents, GameScenePartitioner.Instance.solidChangedLayer, new System.Action<object>(this.OnSolidChanged));
    }
  }

  public Element GetTargetElement()
  {
    int cell = Grid.PosToCell(this.transform.GetPosition());
    return Grid.Element[cell];
  }

  public override string GetConversationTopic()
  {
    return this.originalDigElement.tag.Name;
  }

  protected override bool OnWorkTick(Worker worker, float dt)
  {
    Diggable.DoDigTick(Grid.PosToCell((KMonoBehaviour) this), dt);
    return this.isDigComplete;
  }

  protected override void OnStopWork(Worker worker)
  {
    if (!this.isDigComplete)
      return;
    Util.KDestroyGameObject(this.gameObject);
  }

  public static void DoDigTick(int cell, float dt)
  {
    float hardness = (float) Grid.Element[cell].hardness;
    if ((double) hardness == (double) byte.MaxValue)
      return;
    Element elementByHash = ElementLoader.FindElementByHash(SimHashes.Ice);
    float num1 = hardness / (float) elementByHash.hardness;
    float num2 = 4f * (Mathf.Min(Grid.Mass[cell], 400f) / 400f);
    float num3 = num2 + num1 * num2;
    float amount = dt / num3;
    double num4 = (double) WorldDamage.Instance.ApplyDamage(cell, amount, -1, -1, (string) null, (string) null);
  }

  public static Diggable GetDiggable(int cell)
  {
    GameObject gameObject = Grid.Objects[cell, 7];
    if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
      return gameObject.GetComponent<Diggable>();
    return (Diggable) null;
  }

  public static bool IsDiggable(int cell)
  {
    if (Grid.Solid[cell])
      return !Grid.Foundation[cell];
    return Diggable.GetUnstableCellAbove(cell) != Grid.InvalidCell;
  }

  private static int GetUnstableCellAbove(int cell)
  {
    List<int> containingFallingAbove = World.Instance.GetComponent<UnstableGroundManager>().GetCellsContainingFallingAbove(Grid.CellToXY(cell));
    if (containingFallingAbove.Contains(cell))
      return cell;
    for (int cell1 = Grid.CellAbove(cell); Grid.IsValidCell(cell1) && !Grid.Foundation[cell1]; cell1 = Grid.CellAbove(cell1))
    {
      if (Grid.Solid[cell1])
      {
        if (Grid.Element[cell1].IsUnstable)
          return cell1;
        return Grid.InvalidCell;
      }
      if (containingFallingAbove.Contains(cell1))
        return cell1;
    }
    return Grid.InvalidCell;
  }

  public static bool RequiresTool(Element e)
  {
    return false;
  }

  public static bool Undiggable(Element e)
  {
    return e.id == SimHashes.Unobtanium;
  }

  private void OnReachableChanged(object data)
  {
    if ((UnityEngine.Object) this.childRenderer == (UnityEngine.Object) null)
      this.childRenderer = this.GetComponentInChildren<MeshRenderer>();
    Material material = this.childRenderer.material;
    this.isReachable = (bool) data;
    if (material.color == Game.Instance.uiColours.Dig.invalidLocation)
      return;
    this.UpdateColor(this.isReachable);
    KSelectable component = this.GetComponent<KSelectable>();
    if (this.isReachable)
    {
      component.RemoveStatusItem(Db.Get().BuildingStatusItems.DigUnreachable, false);
    }
    else
    {
      component.AddStatusItem(Db.Get().BuildingStatusItems.DigUnreachable, (object) this);
      GameScheduler.Instance.Schedule("Locomotion Tutorial", 2f, (System.Action<object>) (obj => Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Locomotion, true)), (object) null, (SchedulerGroup) null);
    }
  }

  private void UpdateColor(bool reachable)
  {
    if (!((UnityEngine.Object) this.childRenderer != (UnityEngine.Object) null))
      return;
    Material material = this.childRenderer.material;
    if (Diggable.RequiresTool(Grid.Element[Grid.PosToCell(this.gameObject)]) || Diggable.Undiggable(Grid.Element[Grid.PosToCell(this.gameObject)]))
      material.color = Game.Instance.uiColours.Dig.invalidLocation;
    else if (Grid.Element[Grid.PosToCell(this.gameObject)].hardness >= (byte) 50)
    {
      material.color = !reachable ? Game.Instance.uiColours.Dig.unreachable : Game.Instance.uiColours.Dig.validLocation;
      this.multitoolContext = (HashedString) Diggable.lasersForHardness[1].first;
      this.multitoolHitEffectTag = Diggable.lasersForHardness[1].second;
    }
    else
    {
      material.color = !reachable ? Game.Instance.uiColours.Dig.unreachable : Game.Instance.uiColours.Dig.validLocation;
      this.multitoolContext = (HashedString) Diggable.lasersForHardness[0].first;
      this.multitoolHitEffectTag = Diggable.lasersForHardness[0].second;
    }
  }

  public override float GetPercentComplete()
  {
    return Grid.Damage[Grid.PosToCell((KMonoBehaviour) this)];
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
    GameScenePartitioner.Instance.Free(ref this.unstableEntry);
    Game.Instance.Unsubscribe(this.handle);
    GameScenePartitioner.Instance.TriggerEvent(Grid.PosToCell((KMonoBehaviour) this), GameScenePartitioner.Instance.digDestroyedLayer, (object) null);
    Components.Diggables.Remove(this);
  }

  private void OnCancel()
  {
    DetailsScreen.Instance.Show(false);
    this.gameObject.Trigger(2127324410, (object) null);
  }

  private void OnRefreshUserMenu(object data)
  {
    Game.Instance.userMenu.AddButton(this.gameObject, new KIconButtonMenu.ButtonInfo("icon_cancel", (string) UI.USERMENUACTIONS.CANCELDIG.NAME, new System.Action(this.OnCancel), Action.NumActions, (System.Action<GameObject>) null, (System.Action<KIconButtonMenu.ButtonInfo>) null, (Texture) null, (string) UI.USERMENUACTIONS.CANCELDIG.TOOLTIP, true), 1f);
  }
}
