// Decompiled with JetBrains decompiler
// Type: TouristModule
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using KSerialization;
using System;
using System.Collections.Generic;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class TouristModule : StateMachineComponent<TouristModule.StatesInstance>, IEffectDescriptor
{
  private static readonly EventSystem.IntraObjectHandler<TouristModule> OnSuspendDelegate = new EventSystem.IntraObjectHandler<TouristModule>((System.Action<TouristModule, object>) ((component, data) => component.OnSuspend(data)));
  private static readonly EventSystem.IntraObjectHandler<TouristModule> OnAssigneeChangedDelegate = new EventSystem.IntraObjectHandler<TouristModule>((System.Action<TouristModule, object>) ((component, data) => component.OnAssigneeChanged(data)));
  public Storage storage;
  [Serialize]
  private bool isSuspended;
  private bool releasingAstronaut;
  private const Sim.Cell.Properties floorCellProperties = Sim.Cell.Properties.GasImpermeable | Sim.Cell.Properties.LiquidImpermeable | Sim.Cell.Properties.SolidImpermeable | Sim.Cell.Properties.Opaque;
  public Assignable assignable;
  private HandleVector<int>.Handle partitionerEntry;

  public bool IsSuspended
  {
    get
    {
      return this.isSuspended;
    }
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
  }

  public void SetSuspended(bool state)
  {
    this.isSuspended = state;
  }

  public void ReleaseAstronaut(object data, bool applyBuff = false)
  {
    if (this.releasingAstronaut)
      return;
    this.releasingAstronaut = true;
    MinionStorage component = this.GetComponent<MinionStorage>();
    List<MinionStorage.Info> storedMinionInfo = component.GetStoredMinionInfo();
    for (int index = storedMinionInfo.Count - 1; index >= 0; --index)
    {
      MinionStorage.Info info = storedMinionInfo[index];
      GameObject gameObject = component.DeserializeMinion(info.id, Grid.CellToPos(Grid.PosToCell(this.smi.master.transform.GetPosition())));
      if (Grid.FakeFloor[Grid.OffsetCell(Grid.PosToCell(this.smi.master.gameObject), 0, -1)])
      {
        gameObject.GetComponent<Navigator>().SetCurrentNavType(NavType.Floor);
        if (applyBuff)
          gameObject.GetComponent<Effects>().Add(Db.Get().effects.Get("SpaceTourist"), true);
      }
    }
    this.releasingAstronaut = false;
  }

  public void OnSuspend(object data)
  {
    Storage component = this.GetComponent<Storage>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
    {
      component.capacityKg = component.MassStored();
      component.allowItemRemoval = false;
    }
    if ((UnityEngine.Object) this.GetComponent<ManualDeliveryKG>() != (UnityEngine.Object) null)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.GetComponent<ManualDeliveryKG>());
    this.SetSuspended(true);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.storage = this.GetComponent<Storage>();
    this.assignable = this.GetComponent<Assignable>();
    this.smi.StartSM();
    this.partitionerEntry = GameScenePartitioner.Instance.Add("TouristModule.gantryChanged", (object) this.gameObject, Grid.PosToCell(this.gameObject), GameScenePartitioner.Instance.validNavCellChangedLayer, new System.Action<object>(this.OnGantryChanged));
    this.OnGantryChanged((object) null);
    this.Subscribe<TouristModule>(-1056989049, TouristModule.OnSuspendDelegate);
    this.Subscribe<TouristModule>(684616645, TouristModule.OnAssigneeChangedDelegate);
  }

  private void OnGantryChanged(object data)
  {
    if (!((UnityEngine.Object) this.gameObject != (UnityEngine.Object) null))
      return;
    KSelectable component = this.GetComponent<KSelectable>();
    component.RemoveStatusItem(Db.Get().BuildingStatusItems.HasGantry, false);
    component.RemoveStatusItem(Db.Get().BuildingStatusItems.MissingGantry, false);
    int index = Grid.OffsetCell(Grid.PosToCell(this.smi.master.gameObject), 0, -1);
    if (Grid.FakeFloor[index])
      component.AddStatusItem(Db.Get().BuildingStatusItems.HasGantry, (object) null);
    else
      component.AddStatusItem(Db.Get().BuildingStatusItems.MissingGantry, (object) null);
  }

  private Chore CreateWorkChore()
  {
    WorkChore<CommandModuleWorkable> workChore = new WorkChore<CommandModuleWorkable>(Db.Get().ChoreTypes.Astronaut, (IStateMachineTarget) this, (ChoreProvider) null, true, (System.Action<Chore>) null, (System.Action<Chore>) null, (System.Action<Chore>) null, false, (ScheduleBlockType) null, false, true, Assets.GetAnim((HashedString) "anim_hat_kanim"), false, true, false, PriorityScreen.PriorityClass.personalNeeds, 5, false, true);
    workChore.AddPrecondition(ChorePreconditions.instance.IsAssignedtoMe, (object) this.assignable);
    return (Chore) workChore;
  }

  private void OnAssigneeChanged(object data)
  {
    if (this.GetComponent<MinionStorage>().GetStoredMinionInfo().Count <= 0)
      return;
    this.ReleaseAstronaut((object) null, false);
    Game.Instance.userMenu.Refresh(this.gameObject);
  }

  protected override void OnCleanUp()
  {
    GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
    this.partitionerEntry.Clear();
    this.ReleaseAstronaut((object) null, false);
    GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
    this.smi.StopSM("cleanup");
  }

  public List<Descriptor> GetDescriptors(BuildingDef def)
  {
    return (List<Descriptor>) null;
  }

  public class StatesInstance : GameStateMachine<TouristModule.States, TouristModule.StatesInstance, TouristModule, object>.GameInstance
  {
    public StatesInstance(TouristModule smi)
      : base(smi)
    {
      smi.gameObject.Subscribe(238242047, (System.Action<object>) (data =>
      {
        smi.SetSuspended(false);
        smi.ReleaseAstronaut((object) null, true);
        smi.assignable.Unassign();
      }));
    }
  }

  public class States : GameStateMachine<TouristModule.States, TouristModule.StatesInstance, TouristModule>
  {
    public GameStateMachine<TouristModule.States, TouristModule.StatesInstance, TouristModule, object>.State idle;
    public GameStateMachine<TouristModule.States, TouristModule.StatesInstance, TouristModule, object>.State awaitingTourist;
    public GameStateMachine<TouristModule.States, TouristModule.StatesInstance, TouristModule, object>.State hasTourist;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.idle;
      this.idle.PlayAnim("grounded", KAnim.PlayMode.Loop).GoTo(this.awaitingTourist);
      this.awaitingTourist.PlayAnim("grounded", KAnim.PlayMode.Loop).ToggleChore((Func<TouristModule.StatesInstance, Chore>) (smi => smi.master.CreateWorkChore()), this.hasTourist);
      this.hasTourist.PlayAnim("grounded", KAnim.PlayMode.Loop).EventTransition(GameHashes.LandRocket, this.idle, (StateMachine<TouristModule.States, TouristModule.StatesInstance, TouristModule, object>.Transition.ConditionCallback) null).EventTransition(GameHashes.AssigneeChanged, this.idle, (StateMachine<TouristModule.States, TouristModule.StatesInstance, TouristModule, object>.Transition.ConditionCallback) null);
    }
  }
}
