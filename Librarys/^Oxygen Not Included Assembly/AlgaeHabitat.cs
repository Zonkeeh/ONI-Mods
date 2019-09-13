// Decompiled with JetBrains decompiler
// Type: AlgaeHabitat
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class AlgaeHabitat : StateMachineComponent<AlgaeHabitat.SMInstance>
{
  [SerializeField]
  public float lightBonusMultiplier = 1.1f;
  public CellOffset pressureSampleOffset = CellOffset.none;
  [MyCmpGet]
  private Operational operational;
  private Storage pollutedWaterStorage;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi.StartSM();
    GameScheduler.Instance.Schedule("WaterFetchingTutorial", 2f, (System.Action<object>) (obj => Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_FetchingWater, true)), (object) null, (SchedulerGroup) null);
    this.ConfigurePollutedWaterOutput();
    Tutorial.Instance.oxygenGenerators.Add(this.gameObject);
  }

  protected override void OnCleanUp()
  {
    Tutorial.Instance.oxygenGenerators.Remove(this.gameObject);
    base.OnCleanUp();
  }

  private void ConfigurePollutedWaterOutput()
  {
    Storage storage = (Storage) null;
    Tag tag = ElementLoader.FindElementByHash(SimHashes.DirtyWater).tag;
    foreach (Storage component in this.GetComponents<Storage>())
    {
      if (component.storageFilters.Contains(tag))
      {
        storage = component;
        break;
      }
    }
    foreach (ElementConverter component in this.GetComponents<ElementConverter>())
    {
      foreach (ElementConverter.OutputElement outputElement in component.outputElements)
      {
        if (outputElement.elementHash == SimHashes.DirtyWater)
        {
          component.SetStorage(storage);
          break;
        }
      }
    }
    this.pollutedWaterStorage = storage;
  }

  public class SMInstance : GameStateMachine<AlgaeHabitat.States, AlgaeHabitat.SMInstance, AlgaeHabitat, object>.GameInstance
  {
    public ElementConverter converter;
    public Chore emptyChore;

    public SMInstance(AlgaeHabitat master)
      : base(master)
    {
      this.converter = master.GetComponent<ElementConverter>();
    }

    public bool HasEnoughMass(Tag tag)
    {
      return this.converter.HasEnoughMass(tag);
    }

    public bool NeedsEmptying()
    {
      return (double) this.smi.master.pollutedWaterStorage.RemainingCapacity() <= 0.0;
    }

    public void CreateEmptyChore()
    {
      if (this.emptyChore != null)
        this.emptyChore.Cancel("dupe");
      this.emptyChore = (Chore) new WorkChore<AlgaeHabitatEmpty>(Db.Get().ChoreTypes.EmptyStorage, (IStateMachineTarget) this.master.GetComponent<AlgaeHabitatEmpty>(), (ChoreProvider) null, true, new System.Action<Chore>(this.OnEmptyComplete), (System.Action<Chore>) null, (System.Action<Chore>) null, true, (ScheduleBlockType) null, false, true, (KAnimFile) null, false, true, true, PriorityScreen.PriorityClass.basic, 5, true, true);
    }

    public void CancelEmptyChore()
    {
      if (this.emptyChore == null)
        return;
      this.emptyChore.Cancel("Cancelled");
      this.emptyChore = (Chore) null;
    }

    private void OnEmptyComplete(Chore chore)
    {
      this.emptyChore = (Chore) null;
      this.master.pollutedWaterStorage.DropAll(true, false, new Vector3(), true);
    }
  }

  public class States : GameStateMachine<AlgaeHabitat.States, AlgaeHabitat.SMInstance, AlgaeHabitat>
  {
    public GameStateMachine<AlgaeHabitat.States, AlgaeHabitat.SMInstance, AlgaeHabitat, object>.State generatingOxygen;
    public GameStateMachine<AlgaeHabitat.States, AlgaeHabitat.SMInstance, AlgaeHabitat, object>.State stoppedGeneratingOxygen;
    public GameStateMachine<AlgaeHabitat.States, AlgaeHabitat.SMInstance, AlgaeHabitat, object>.State stoppedGeneratingOxygenTransition;
    public GameStateMachine<AlgaeHabitat.States, AlgaeHabitat.SMInstance, AlgaeHabitat, object>.State noWater;
    public GameStateMachine<AlgaeHabitat.States, AlgaeHabitat.SMInstance, AlgaeHabitat, object>.State noAlgae;
    public GameStateMachine<AlgaeHabitat.States, AlgaeHabitat.SMInstance, AlgaeHabitat, object>.State needsEmptying;
    public GameStateMachine<AlgaeHabitat.States, AlgaeHabitat.SMInstance, AlgaeHabitat, object>.State gotAlgae;
    public GameStateMachine<AlgaeHabitat.States, AlgaeHabitat.SMInstance, AlgaeHabitat, object>.State gotWater;
    public GameStateMachine<AlgaeHabitat.States, AlgaeHabitat.SMInstance, AlgaeHabitat, object>.State gotEmptied;
    public GameStateMachine<AlgaeHabitat.States, AlgaeHabitat.SMInstance, AlgaeHabitat, object>.State lostAlgae;
    public GameStateMachine<AlgaeHabitat.States, AlgaeHabitat.SMInstance, AlgaeHabitat, object>.State notoperational;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.noAlgae;
      this.root.EventTransition(GameHashes.OperationalChanged, this.notoperational, (StateMachine<AlgaeHabitat.States, AlgaeHabitat.SMInstance, AlgaeHabitat, object>.Transition.ConditionCallback) (smi => !smi.master.operational.IsOperational)).EventTransition(GameHashes.OperationalChanged, this.noAlgae, (StateMachine<AlgaeHabitat.States, AlgaeHabitat.SMInstance, AlgaeHabitat, object>.Transition.ConditionCallback) (smi => smi.master.operational.IsOperational));
      this.notoperational.QueueAnim("off", false, (Func<AlgaeHabitat.SMInstance, string>) null);
      this.gotAlgae.PlayAnim("on_pre").OnAnimQueueComplete(this.noWater);
      this.gotEmptied.PlayAnim("on_pre").OnAnimQueueComplete(this.generatingOxygen);
      this.lostAlgae.PlayAnim("on_pst").OnAnimQueueComplete(this.noAlgae);
      this.noAlgae.QueueAnim("off", false, (Func<AlgaeHabitat.SMInstance, string>) null).EventTransition(GameHashes.OnStorageChange, this.gotAlgae, (StateMachine<AlgaeHabitat.States, AlgaeHabitat.SMInstance, AlgaeHabitat, object>.Transition.ConditionCallback) (smi => smi.HasEnoughMass(GameTags.Algae))).Enter((StateMachine<AlgaeHabitat.States, AlgaeHabitat.SMInstance, AlgaeHabitat, object>.State.Callback) (smi => smi.master.operational.SetActive(false, false)));
      this.noWater.QueueAnim("on", false, (Func<AlgaeHabitat.SMInstance, string>) null).Enter((StateMachine<AlgaeHabitat.States, AlgaeHabitat.SMInstance, AlgaeHabitat, object>.State.Callback) (smi => smi.master.GetComponent<PassiveElementConsumer>().EnableConsumption(true))).EventTransition(GameHashes.OnStorageChange, this.lostAlgae, (StateMachine<AlgaeHabitat.States, AlgaeHabitat.SMInstance, AlgaeHabitat, object>.Transition.ConditionCallback) (smi => !smi.HasEnoughMass(GameTags.Algae))).EventTransition(GameHashes.OnStorageChange, this.gotWater, (StateMachine<AlgaeHabitat.States, AlgaeHabitat.SMInstance, AlgaeHabitat, object>.Transition.ConditionCallback) (smi =>
      {
        if (smi.HasEnoughMass(GameTags.Algae))
          return smi.HasEnoughMass(GameTags.Water);
        return false;
      }));
      this.needsEmptying.QueueAnim("off", false, (Func<AlgaeHabitat.SMInstance, string>) null).Enter((StateMachine<AlgaeHabitat.States, AlgaeHabitat.SMInstance, AlgaeHabitat, object>.State.Callback) (smi => smi.CreateEmptyChore())).Exit((StateMachine<AlgaeHabitat.States, AlgaeHabitat.SMInstance, AlgaeHabitat, object>.State.Callback) (smi => smi.CancelEmptyChore())).ToggleStatusItem(Db.Get().BuildingStatusItems.HabitatNeedsEmptying, (object) null).EventTransition(GameHashes.OnStorageChange, this.noAlgae, (StateMachine<AlgaeHabitat.States, AlgaeHabitat.SMInstance, AlgaeHabitat, object>.Transition.ConditionCallback) (smi =>
      {
        if (smi.HasEnoughMass(GameTags.Algae))
          return !smi.HasEnoughMass(GameTags.Water);
        return true;
      })).EventTransition(GameHashes.OnStorageChange, this.gotEmptied, (StateMachine<AlgaeHabitat.States, AlgaeHabitat.SMInstance, AlgaeHabitat, object>.Transition.ConditionCallback) (smi =>
      {
        if (smi.HasEnoughMass(GameTags.Algae) && smi.HasEnoughMass(GameTags.Water))
          return !smi.NeedsEmptying();
        return false;
      }));
      this.gotWater.PlayAnim("working_pre").OnAnimQueueComplete(this.needsEmptying);
      this.generatingOxygen.Enter((StateMachine<AlgaeHabitat.States, AlgaeHabitat.SMInstance, AlgaeHabitat, object>.State.Callback) (smi => smi.master.operational.SetActive(true, false))).Exit((StateMachine<AlgaeHabitat.States, AlgaeHabitat.SMInstance, AlgaeHabitat, object>.State.Callback) (smi => smi.master.operational.SetActive(false, false))).Update("GeneratingOxygen", (System.Action<AlgaeHabitat.SMInstance, float>) ((smi, dt) =>
      {
        int cell = Grid.PosToCell(smi.master.transform.GetPosition());
        smi.converter.OutputMultiplier = Grid.LightCount[cell] <= 0 ? 1f : smi.master.lightBonusMultiplier;
      }), UpdateRate.SIM_200ms, false).QueueAnim("working_loop", true, (Func<AlgaeHabitat.SMInstance, string>) null).EventTransition(GameHashes.OnStorageChange, this.stoppedGeneratingOxygen, (StateMachine<AlgaeHabitat.States, AlgaeHabitat.SMInstance, AlgaeHabitat, object>.Transition.ConditionCallback) (smi =>
      {
        if (smi.HasEnoughMass(GameTags.Water) && smi.HasEnoughMass(GameTags.Algae))
          return smi.NeedsEmptying();
        return true;
      }));
      this.stoppedGeneratingOxygen.PlayAnim("working_pst").OnAnimQueueComplete(this.stoppedGeneratingOxygenTransition);
      this.stoppedGeneratingOxygenTransition.EventTransition(GameHashes.OnStorageChange, this.needsEmptying, (StateMachine<AlgaeHabitat.States, AlgaeHabitat.SMInstance, AlgaeHabitat, object>.Transition.ConditionCallback) (smi => smi.NeedsEmptying())).EventTransition(GameHashes.OnStorageChange, this.noWater, (StateMachine<AlgaeHabitat.States, AlgaeHabitat.SMInstance, AlgaeHabitat, object>.Transition.ConditionCallback) (smi => !smi.HasEnoughMass(GameTags.Water))).EventTransition(GameHashes.OnStorageChange, this.lostAlgae, (StateMachine<AlgaeHabitat.States, AlgaeHabitat.SMInstance, AlgaeHabitat, object>.Transition.ConditionCallback) (smi => !smi.HasEnoughMass(GameTags.Algae))).EventTransition(GameHashes.OnStorageChange, this.gotWater, (StateMachine<AlgaeHabitat.States, AlgaeHabitat.SMInstance, AlgaeHabitat, object>.Transition.ConditionCallback) (smi =>
      {
        if (smi.HasEnoughMass(GameTags.Water))
          return smi.HasEnoughMass(GameTags.Algae);
        return false;
      }));
    }
  }
}
