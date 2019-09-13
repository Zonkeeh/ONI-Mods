// Decompiled with JetBrains decompiler
// Type: FlushToilet
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FlushToilet : StateMachineComponent<FlushToilet.SMInstance>, IUsable, IEffectDescriptor
{
  public static readonly Tag WaterTag = GameTagExtensions.Create(SimHashes.Water);
  [SerializeField]
  public float massConsumedPerUse = 5f;
  [SerializeField]
  public float massEmittedPerUse = 5f;
  private MeterController fillMeter;
  private MeterController contaminationMeter;
  [SerializeField]
  public float newPeeTemperature;
  [SerializeField]
  public string diseaseId;
  [SerializeField]
  public int diseasePerFlush;
  [SerializeField]
  public int diseaseOnDupePerFlush;
  [MyCmpGet]
  private ConduitConsumer conduitConsumer;
  [MyCmpGet]
  private Storage storage;
  private int inputCell;
  private int outputCell;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    Building component1 = this.GetComponent<Building>();
    this.inputCell = component1.GetUtilityInputCell();
    this.outputCell = component1.GetUtilityOutputCell();
    ConduitFlow liquidConduitFlow = Game.Instance.liquidConduitFlow;
    liquidConduitFlow.onConduitsRebuilt += new System.Action(this.OnConduitsRebuilt);
    liquidConduitFlow.AddConduitUpdater(new System.Action<float>(this.OnConduitUpdate), ConduitFlowPriority.Default);
    KBatchedAnimController component2 = this.GetComponent<KBatchedAnimController>();
    this.fillMeter = new MeterController((KAnimControllerBase) component2, "meter_target", "meter", Meter.Offset.Behind, Grid.SceneLayer.NoLayer, new Vector3(0.4f, 3.2f, 0.1f), new string[0]);
    this.contaminationMeter = new MeterController((KAnimControllerBase) component2, "meter_target", "meter_dirty", Meter.Offset.Behind, Grid.SceneLayer.NoLayer, new Vector3(0.4f, 3.2f, 0.1f), new string[0]);
    Components.Toilets.Add((IUsable) this);
    this.smi.StartSM();
    this.smi.ShowFillMeter();
  }

  protected override void OnCleanUp()
  {
    Game.Instance.liquidConduitFlow.onConduitsRebuilt -= new System.Action(this.OnConduitsRebuilt);
    Components.Toilets.Remove((IUsable) this);
    base.OnCleanUp();
  }

  private void OnConduitsRebuilt()
  {
    this.Trigger(-2094018600, (object) null);
  }

  public bool IsUsable()
  {
    return this.smi.HasTag(GameTags.Usable);
  }

  private void Flush(Worker worker)
  {
    ListPool<GameObject, Storage>.PooledList pooledList = ListPool<GameObject, Storage>.Allocate();
    this.storage.Find(FlushToilet.WaterTag, (List<GameObject>) pooledList);
    float num1 = 0.0f;
    float massConsumedPerUse = this.massConsumedPerUse;
    foreach (GameObject gameObject in (List<GameObject>) pooledList)
    {
      PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
      float num2 = Mathf.Min(component.Mass, massConsumedPerUse);
      component.Mass -= num2;
      massConsumedPerUse -= num2;
      num1 += num2 * component.Temperature;
    }
    pooledList.Recycle();
    float num3 = this.massEmittedPerUse - this.massConsumedPerUse;
    float temperature = (num1 + num3 * this.newPeeTemperature) / this.massEmittedPerUse;
    byte index = Db.Get().Diseases.GetIndex((HashedString) this.diseaseId);
    this.storage.AddLiquid(SimHashes.DirtyWater, this.massEmittedPerUse, temperature, index, this.diseasePerFlush, false, true);
    if ((UnityEngine.Object) worker != (UnityEngine.Object) null)
    {
      worker.GetComponent<PrimaryElement>().AddDisease(index, this.diseaseOnDupePerFlush, "FlushToilet.Flush");
      PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, string.Format((string) DUPLICANTS.DISEASES.ADDED_POPFX, (object) Db.Get().Diseases[(int) index].Name, (object) (this.diseasePerFlush + this.diseaseOnDupePerFlush)), this.transform, Vector3.up, 1.5f, false, false);
      Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_LotsOfGerms, true);
    }
    else
      DebugUtil.LogWarningArgs((object) "Tried to add disease on toilet use but worker was null");
  }

  public List<Descriptor> RequirementDescriptors(BuildingDef def)
  {
    List<Descriptor> descriptorList = new List<Descriptor>();
    string str = ElementLoader.FindElementByHash(SimHashes.Water).tag.ProperName();
    descriptorList.Add(new Descriptor(string.Format((string) UI.BUILDINGEFFECTS.ELEMENTCONSUMEDPERUSE, (object) str, (object) GameUtil.GetFormattedMass(this.massConsumedPerUse, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMEDPERUSE, (object) str, (object) GameUtil.GetFormattedMass(this.massConsumedPerUse, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), Descriptor.DescriptorType.Requirement, false));
    return descriptorList;
  }

  public List<Descriptor> EffectDescriptors(BuildingDef def)
  {
    List<Descriptor> descriptorList = new List<Descriptor>();
    string str = ElementLoader.FindElementByHash(SimHashes.DirtyWater).tag.ProperName();
    descriptorList.Add(new Descriptor(string.Format((string) UI.BUILDINGEFFECTS.ELEMENTEMITTED_TOILET, (object) str, (object) GameUtil.GetFormattedMass(this.massEmittedPerUse, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}"), (object) GameUtil.GetFormattedTemperature(this.newPeeTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTEMITTED_TOILET, (object) str, (object) GameUtil.GetFormattedMass(this.massEmittedPerUse, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}"), (object) GameUtil.GetFormattedTemperature(this.newPeeTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), Descriptor.DescriptorType.Effect, false));
    Klei.AI.Disease disease = Db.Get().Diseases.Get(this.diseaseId);
    int units = this.diseasePerFlush + this.diseaseOnDupePerFlush;
    descriptorList.Add(new Descriptor(string.Format((string) UI.BUILDINGEFFECTS.DISEASEEMITTEDPERUSE, (object) disease.Name, (object) GameUtil.GetFormattedDiseaseAmount(units)), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.DISEASEEMITTEDPERUSE, (object) disease.Name, (object) GameUtil.GetFormattedDiseaseAmount(units)), Descriptor.DescriptorType.DiseaseSource, false));
    return descriptorList;
  }

  public List<Descriptor> GetDescriptors(BuildingDef def)
  {
    List<Descriptor> descriptorList = new List<Descriptor>();
    descriptorList.AddRange((IEnumerable<Descriptor>) this.RequirementDescriptors(def));
    descriptorList.AddRange((IEnumerable<Descriptor>) this.EffectDescriptors(def));
    return descriptorList;
  }

  private void OnConduitUpdate(float dt)
  {
    if (this.GetSMI() == null)
      return;
    this.smi.sm.outputBlocked.Set((double) Game.Instance.liquidConduitFlow.GetContents(this.outputCell).mass > 0.0 && this.smi.HasContaminatedMass(), this.smi);
  }

  Transform IUsable.get_transform()
  {
    return this.transform;
  }

  public class SMInstance : GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.GameInstance
  {
    public List<Chore> activeUseChores;

    public SMInstance(FlushToilet master)
      : base(master)
    {
      this.activeUseChores = new List<Chore>();
      this.UpdateFullnessState();
      this.UpdateDirtyState();
    }

    public bool HasValidConnections()
    {
      return Game.Instance.liquidConduitFlow.HasConduit(this.master.inputCell) && Game.Instance.liquidConduitFlow.HasConduit(this.master.outputCell);
    }

    public bool UpdateFullnessState()
    {
      float num = 0.0f;
      ListPool<GameObject, FlushToilet>.PooledList pooledList = ListPool<GameObject, FlushToilet>.Allocate();
      this.master.storage.Find(FlushToilet.WaterTag, (List<GameObject>) pooledList);
      foreach (GameObject gameObject in (List<GameObject>) pooledList)
      {
        PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
        num += component.Mass;
      }
      pooledList.Recycle();
      bool flag = (double) num >= (double) this.master.massConsumedPerUse;
      this.master.conduitConsumer.enabled = !flag;
      this.master.fillMeter.SetPositionPercent(Mathf.Clamp01(num / this.master.massConsumedPerUse));
      return flag;
    }

    public void UpdateDirtyState()
    {
      this.master.contaminationMeter.SetPositionPercent(this.GetComponent<ToiletWorkableUse>().GetPercentComplete());
    }

    public void Flush()
    {
      this.master.fillMeter.SetPositionPercent(0.0f);
      this.master.contaminationMeter.SetPositionPercent(1f);
      this.smi.ShowFillMeter();
      this.master.Flush(this.master.GetComponent<ToiletWorkableUse>().worker);
    }

    public void ShowFillMeter()
    {
      this.master.fillMeter.gameObject.SetActive(true);
      this.master.contaminationMeter.gameObject.SetActive(false);
    }

    public bool HasContaminatedMass()
    {
      foreach (GameObject gameObject in this.GetComponent<Storage>().items)
      {
        PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
        if (!((UnityEngine.Object) component == (UnityEngine.Object) null) && component.ElementID == SimHashes.DirtyWater && (double) component.Mass > 0.0)
          return true;
      }
      return false;
    }

    public void ShowContaminatedMeter()
    {
      this.master.fillMeter.gameObject.SetActive(false);
      this.master.contaminationMeter.gameObject.SetActive(true);
    }
  }

  public class States : GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet>
  {
    public GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.State disconnected;
    public GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.State backedup;
    public FlushToilet.States.ReadyStates ready;
    public GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.State fillingInactive;
    public GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.State filling;
    public GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.State flushing;
    public GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.State flushed;
    public StateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.BoolParameter outputBlocked;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.disconnected;
      this.disconnected.PlayAnim("off").EventTransition(GameHashes.ConduitConnectionChanged, this.backedup, (StateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.Transition.ConditionCallback) (smi => smi.HasValidConnections())).Enter((StateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.State.Callback) (smi => smi.GetComponent<Operational>().SetActive(false, false)));
      this.backedup.PlayAnim("off").ToggleStatusItem(Db.Get().BuildingStatusItems.OutputPipeFull, (object) null).EventTransition(GameHashes.ConduitConnectionChanged, this.disconnected, (StateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.Transition.ConditionCallback) (smi => !smi.HasValidConnections())).ParamTransition<bool>((StateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.Parameter<bool>) this.outputBlocked, this.fillingInactive, GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.IsFalse).Enter((StateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.State.Callback) (smi => smi.GetComponent<Operational>().SetActive(false, false)));
      this.filling.PlayAnim("off").Enter((StateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.State.Callback) (smi => smi.GetComponent<Operational>().SetActive(true, false))).EventTransition(GameHashes.ConduitConnectionChanged, this.disconnected, (StateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.Transition.ConditionCallback) (smi => !smi.HasValidConnections())).ParamTransition<bool>((StateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.Parameter<bool>) this.outputBlocked, this.backedup, GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.IsTrue).EventTransition(GameHashes.OnStorageChange, (GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.State) this.ready, (StateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.Transition.ConditionCallback) (smi => smi.UpdateFullnessState())).EventTransition(GameHashes.OperationalChanged, this.fillingInactive, (StateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.Transition.ConditionCallback) (smi => !smi.GetComponent<Operational>().IsOperational));
      this.fillingInactive.PlayAnim("off").Enter((StateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.State.Callback) (smi => smi.GetComponent<Operational>().SetActive(false, false))).EventTransition(GameHashes.OperationalChanged, this.filling, (StateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.Transition.ConditionCallback) (smi => smi.GetComponent<Operational>().IsOperational)).ParamTransition<bool>((StateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.Parameter<bool>) this.outputBlocked, this.backedup, GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.IsTrue);
      this.ready.DefaultState(this.ready.idle).Enter((StateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.State.Callback) (smi =>
      {
        smi.master.fillMeter.SetPositionPercent(1f);
        smi.master.contaminationMeter.SetPositionPercent(0.0f);
      })).PlayAnim("off").EventTransition(GameHashes.ConduitConnectionChanged, this.disconnected, (StateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.Transition.ConditionCallback) (smi => !smi.HasValidConnections())).ParamTransition<bool>((StateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.Parameter<bool>) this.outputBlocked, this.backedup, GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.IsTrue).ToggleChore(new Func<FlushToilet.SMInstance, Chore>(this.CreateUrgentUseChore), this.flushing).ToggleChore(new Func<FlushToilet.SMInstance, Chore>(this.CreateBreakUseChore), this.flushing);
      this.ready.idle.Enter((StateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.State.Callback) (smi => smi.GetComponent<Operational>().SetActive(false, false))).ToggleMainStatusItem(Db.Get().BuildingStatusItems.FlushToilet).WorkableStartTransition((Func<FlushToilet.SMInstance, Workable>) (smi => (Workable) smi.master.GetComponent<ToiletWorkableUse>()), this.ready.inuse);
      this.ready.inuse.Enter((StateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.State.Callback) (smi => smi.ShowContaminatedMeter())).ToggleMainStatusItem(Db.Get().BuildingStatusItems.FlushToiletInUse).Update((System.Action<FlushToilet.SMInstance, float>) ((smi, dt) => smi.UpdateDirtyState()), UpdateRate.SIM_200ms, false).WorkableCompleteTransition((Func<FlushToilet.SMInstance, Workable>) (smi => (Workable) smi.master.GetComponent<ToiletWorkableUse>()), this.flushing).WorkableStopTransition((Func<FlushToilet.SMInstance, Workable>) (smi => (Workable) smi.master.GetComponent<ToiletWorkableUse>()), this.flushed);
      this.flushing.Enter((StateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.State.Callback) (smi => smi.Flush())).GoTo(this.flushed);
      this.flushed.EventTransition(GameHashes.OnStorageChange, this.fillingInactive, (StateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.Transition.ConditionCallback) (smi => !smi.HasContaminatedMass())).ParamTransition<bool>((StateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.Parameter<bool>) this.outputBlocked, this.backedup, GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.IsTrue);
    }

    private Chore CreateUrgentUseChore(FlushToilet.SMInstance smi)
    {
      Chore useChore = this.CreateUseChore(smi, Db.Get().ChoreTypes.Pee);
      useChore.AddPrecondition(ChorePreconditions.instance.IsBladderFull, (object) null);
      useChore.AddPrecondition(ChorePreconditions.instance.NotCurrentlyPeeing, (object) null);
      return useChore;
    }

    private Chore CreateBreakUseChore(FlushToilet.SMInstance smi)
    {
      Chore useChore = this.CreateUseChore(smi, Db.Get().ChoreTypes.BreakPee);
      useChore.AddPrecondition(ChorePreconditions.instance.IsBladderNotFull, (object) null);
      return useChore;
    }

    private Chore CreateUseChore(FlushToilet.SMInstance smi, ChoreType choreType)
    {
      WorkChore<ToiletWorkableUse> workChore1 = new WorkChore<ToiletWorkableUse>(choreType, (IStateMachineTarget) smi.master, (ChoreProvider) null, true, (System.Action<Chore>) null, (System.Action<Chore>) null, (System.Action<Chore>) null, false, (ScheduleBlockType) null, true, true, (KAnimFile) null, false, true, false, PriorityScreen.PriorityClass.personalNeeds, 5, false, false);
      smi.activeUseChores.Add((Chore) workChore1);
      WorkChore<ToiletWorkableUse> workChore2 = workChore1;
      workChore2.onExit = workChore2.onExit + (System.Action<Chore>) (exiting_chore => smi.activeUseChores.Remove(exiting_chore));
      workChore1.AddPrecondition(ChorePreconditions.instance.IsPreferredAssignableOrUrgentBladder, (object) smi.master.GetComponent<Assignable>());
      workChore1.AddPrecondition(ChorePreconditions.instance.IsExclusivelyAvailableWithOtherChores, (object) smi.activeUseChores);
      return (Chore) workChore1;
    }

    public class ReadyStates : GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.State
    {
      public GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.State idle;
      public GameStateMachine<FlushToilet.States, FlushToilet.SMInstance, FlushToilet, object>.State inuse;
    }
  }
}
