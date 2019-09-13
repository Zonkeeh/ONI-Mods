// Decompiled with JetBrains decompiler
// Type: CommandModule
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using KSerialization;
using System;
using System.Collections.Generic;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class CommandModule : StateMachineComponent<CommandModule.StatesInstance>, IEffectDescriptor
{
  public Storage storage;
  public RocketStats rocketStats;
  private bool releasingAstronaut;
  private const Sim.Cell.Properties floorCellProperties = Sim.Cell.Properties.GasImpermeable | Sim.Cell.Properties.LiquidImpermeable | Sim.Cell.Properties.SolidImpermeable | Sim.Cell.Properties.Opaque;
  public ConditionDestinationReachable reachable;
  public ConditionHasAstronaut hasAstronaut;
  public ConditionHasAtmoSuit hasSuit;
  public CargoBayIsEmpty cargoEmpty;
  public ConditionHasMinimumMass destHasResources;
  public ConditionFlightPathIsClear flightPathIsClear;
  public Assignable assignable;
  private CharacterOverlay characterOverlay;
  private HandleVector<int>.Handle partitionerEntry;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.characterOverlay = this.gameObject.AddComponent<CharacterOverlay>();
    this.characterOverlay.Register();
    this.rocketStats = new RocketStats(this);
  }

  public void ReleaseAstronaut(bool fill_bladder)
  {
    if (this.releasingAstronaut)
      return;
    this.releasingAstronaut = true;
    MinionStorage component = this.GetComponent<MinionStorage>();
    List<MinionStorage.Info> storedMinionInfo = component.GetStoredMinionInfo();
    for (int index = storedMinionInfo.Count - 1; index >= 0; --index)
    {
      MinionStorage.Info info = storedMinionInfo[index];
      GameObject go = component.DeserializeMinion(info.id, Grid.CellToPos(Grid.PosToCell(this.smi.master.transform.GetPosition())));
      if (!((UnityEngine.Object) go == (UnityEngine.Object) null))
      {
        if (Grid.FakeFloor[Grid.OffsetCell(Grid.PosToCell(this.smi.master.gameObject), 0, -1)])
          go.GetComponent<Navigator>().SetCurrentNavType(NavType.Floor);
        if (fill_bladder)
        {
          AmountInstance amountInstance = Db.Get().Amounts.Bladder.Lookup(go);
          if (amountInstance != null)
            amountInstance.value = amountInstance.GetMax();
        }
      }
    }
    this.releasingAstronaut = false;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.storage = this.GetComponent<Storage>();
    this.assignable = this.GetComponent<Assignable>();
    this.assignable.AddAssignPrecondition(new Func<MinionAssignablesProxy, bool>(this.CanAssignTo));
    this.smi.StartSM();
    this.partitionerEntry = GameScenePartitioner.Instance.Add("CommandModule.gantryChanged", (object) this.gameObject, Grid.PosToCell(this.gameObject), GameScenePartitioner.Instance.validNavCellChangedLayer, new System.Action<object>(this.OnGantryChanged));
    this.OnGantryChanged((object) null);
    RocketModule component = this.GetComponent<RocketModule>();
    this.reachable = (ConditionDestinationReachable) component.AddLaunchCondition((RocketLaunchCondition) new ConditionDestinationReachable(this));
    this.hasAstronaut = (ConditionHasAstronaut) component.AddLaunchCondition((RocketLaunchCondition) new ConditionHasAstronaut(this));
    this.hasSuit = (ConditionHasAtmoSuit) component.AddLaunchCondition((RocketLaunchCondition) new ConditionHasAtmoSuit(this));
    this.cargoEmpty = (CargoBayIsEmpty) component.AddLaunchCondition((RocketLaunchCondition) new CargoBayIsEmpty(this));
    this.destHasResources = (ConditionHasMinimumMass) component.AddLaunchCondition((RocketLaunchCondition) new ConditionHasMinimumMass(this));
    this.flightPathIsClear = (ConditionFlightPathIsClear) component.AddFlightCondition((RocketFlightCondition) new ConditionFlightPathIsClear(this.gameObject, 1));
  }

  private bool CanAssignTo(MinionAssignablesProxy worker)
  {
    if (worker.target is MinionIdentity)
      return (worker.target as KMonoBehaviour).GetComponent<MinionResume>().HasPerk(Db.Get().SkillPerks.CanUseRockets);
    if (worker.target is StoredMinionIdentity)
      return (worker.target as StoredMinionIdentity).HasPerk(Db.Get().SkillPerks.CanUseRockets);
    return false;
  }

  private static bool HasValidGantry(GameObject go)
  {
    int index = Grid.OffsetCell(Grid.PosToCell(go), 0, -1);
    return Grid.FakeFloor[index];
  }

  private void OnGantryChanged(object data)
  {
    if (!((UnityEngine.Object) this.gameObject != (UnityEngine.Object) null))
      return;
    KSelectable component = this.GetComponent<KSelectable>();
    component.RemoveStatusItem(Db.Get().BuildingStatusItems.HasGantry, false);
    component.RemoveStatusItem(Db.Get().BuildingStatusItems.MissingGantry, false);
    if (CommandModule.HasValidGantry(this.smi.master.gameObject))
      component.AddStatusItem(Db.Get().BuildingStatusItems.HasGantry, (object) null);
    else
      component.AddStatusItem(Db.Get().BuildingStatusItems.MissingGantry, (object) null);
    this.smi.sm.gantryChanged.Trigger(this.smi);
  }

  private Chore CreateWorkChore()
  {
    WorkChore<CommandModuleWorkable> workChore = new WorkChore<CommandModuleWorkable>(Db.Get().ChoreTypes.Astronaut, (IStateMachineTarget) this, (ChoreProvider) null, true, (System.Action<Chore>) null, (System.Action<Chore>) null, (System.Action<Chore>) null, false, (ScheduleBlockType) null, false, true, Assets.GetAnim((HashedString) "anim_hat_kanim"), false, true, false, PriorityScreen.PriorityClass.personalNeeds, 5, false, true);
    workChore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, (object) Db.Get().SkillPerks.CanUseRockets);
    workChore.AddPrecondition(ChorePreconditions.instance.IsAssignedtoMe, (object) this.assignable);
    return (Chore) workChore;
  }

  protected override void OnCleanUp()
  {
    GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
    this.partitionerEntry.Clear();
    this.ReleaseAstronaut(false);
    this.smi.StopSM("cleanup");
  }

  public List<Descriptor> GetDescriptors(BuildingDef def)
  {
    return (List<Descriptor>) null;
  }

  public class StatesInstance : GameStateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.GameInstance
  {
    public StatesInstance(CommandModule master)
      : base(master)
    {
    }

    public void SetSuspended(bool suspended)
    {
      Storage component1 = this.GetComponent<Storage>();
      if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
        component1.allowItemRemoval = !suspended;
      ManualDeliveryKG component2 = this.GetComponent<ManualDeliveryKG>();
      if (!((UnityEngine.Object) component2 != (UnityEngine.Object) null))
        return;
      component2.Pause(suspended, "Rocket is suspended");
    }

    public bool CheckStoredMinionIsAssignee()
    {
      foreach (MinionStorage.Info info in this.GetComponent<MinionStorage>().GetStoredMinionInfo())
      {
        if (info.serializedMinion != null)
        {
          KPrefabID kprefabId = info.serializedMinion.Get();
          if (!((UnityEngine.Object) kprefabId == (UnityEngine.Object) null) && this.GetComponent<Assignable>().assignee == kprefabId.GetComponent<StoredMinionIdentity>().assignableProxy.Get())
            return true;
        }
      }
      return false;
    }
  }

  public class States : GameStateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule>
  {
    public StateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.Signal gantryChanged;
    public StateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.BoolParameter accumulatedPee;
    public CommandModule.States.GroundedStates grounded;
    public CommandModule.States.SpaceborneStates spaceborne;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.grounded;
      this.grounded.PlayAnim("grounded", KAnim.PlayMode.Loop).DefaultState(this.grounded.awaitingAstronaut).TagTransition(GameTags.RocketNotOnGround, (GameStateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.State) this.spaceborne, false);
      this.grounded.awaitingAstronaut.Enter((StateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.State.Callback) (smi =>
      {
        if (smi.CheckStoredMinionIsAssignee())
          smi.GoTo((StateMachine.BaseState) this.grounded.hasAstronaut);
        Game.Instance.userMenu.Refresh(smi.gameObject);
      })).EventHandler(GameHashes.AssigneeChanged, (StateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.State.Callback) (smi =>
      {
        if (smi.CheckStoredMinionIsAssignee())
          smi.GoTo((StateMachine.BaseState) this.grounded.hasAstronaut);
        Game.Instance.userMenu.Refresh(smi.gameObject);
      })).ToggleChore((Func<CommandModule.StatesInstance, Chore>) (smi => smi.master.CreateWorkChore()), this.grounded.hasAstronaut);
      this.grounded.hasAstronaut.EventHandler(GameHashes.AssigneeChanged, (StateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.State.Callback) (smi =>
      {
        if (smi.CheckStoredMinionIsAssignee())
          return;
        smi.GoTo((StateMachine.BaseState) this.grounded.waitingToRelease);
      }));
      this.grounded.waitingToRelease.ToggleStatusItem(Db.Get().BuildingStatusItems.DisembarkingDuplicant, (object) null).OnSignal(this.gantryChanged, this.grounded.awaitingAstronaut, (Func<CommandModule.StatesInstance, bool>) (smi =>
      {
        if (!CommandModule.HasValidGantry(smi.gameObject))
          return false;
        smi.master.ReleaseAstronaut(this.accumulatedPee.Get(smi));
        this.accumulatedPee.Set(false, smi);
        Game.Instance.userMenu.Refresh(smi.gameObject);
        return true;
      }));
      this.spaceborne.DefaultState(this.spaceborne.launch);
      this.spaceborne.launch.Enter((StateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.State.Callback) (smi => smi.SetSuspended(true))).GoTo(this.spaceborne.idle);
      this.spaceborne.idle.TagTransition(GameTags.RocketNotOnGround, this.spaceborne.land, true);
      this.spaceborne.land.Enter((StateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.State.Callback) (smi =>
      {
        smi.SetSuspended(false);
        Game.Instance.userMenu.Refresh(smi.gameObject);
        this.accumulatedPee.Set(true, smi);
      })).GoTo(this.grounded.waitingToRelease);
    }

    public class GroundedStates : GameStateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.State
    {
      public GameStateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.State awaitingAstronaut;
      public GameStateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.State hasAstronaut;
      public GameStateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.State waitingToRelease;
    }

    public class SpaceborneStates : GameStateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.State
    {
      public GameStateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.State launch;
      public GameStateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.State idle;
      public GameStateMachine<CommandModule.States, CommandModule.StatesInstance, CommandModule, object>.State land;
    }
  }
}
