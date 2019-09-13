// Decompiled with JetBrains decompiler
// Type: HandSanitizer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

public class HandSanitizer : StateMachineComponent<HandSanitizer.SMInstance>, IEffectDescriptor
{
  private static readonly EventSystem.IntraObjectHandler<HandSanitizer> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<HandSanitizer>((System.Action<HandSanitizer, object>) ((component, data) => component.OnStorageChange(data)));
  public float massConsumedPerUse = 1f;
  public SimHashes consumedElement = SimHashes.BleachStone;
  public int diseaseRemovalCount = 10000;
  public int maxUses = 10;
  public SimHashes outputElement = SimHashes.Vacuum;
  public bool dumpWhenFull;
  private WorkableReactable reactable;
  private MeterController cleanMeter;
  private MeterController dirtyMeter;
  public Meter.Offset cleanMeterOffset;
  public Meter.Offset dirtyMeterOffset;
  [Serialize]
  public int maxPossiblyRemoved;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.gameObject.FindOrAddComponent<Workable>();
  }

  private void RefreshMeters()
  {
    float percent_full1 = 0.0f;
    PrimaryElement primaryElement1 = this.GetComponent<Storage>().FindPrimaryElement(this.consumedElement);
    float num = (float) this.maxUses * this.massConsumedPerUse;
    ConduitConsumer component = this.GetComponent<ConduitConsumer>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      num = component.capacityKG;
    if ((UnityEngine.Object) primaryElement1 != (UnityEngine.Object) null)
      percent_full1 = Mathf.Clamp01(primaryElement1.Mass / num);
    float percent_full2 = 0.0f;
    PrimaryElement primaryElement2 = this.GetComponent<Storage>().FindPrimaryElement(this.outputElement);
    if ((UnityEngine.Object) primaryElement2 != (UnityEngine.Object) null)
      percent_full2 = Mathf.Clamp01(primaryElement2.Mass / ((float) this.maxUses * this.massConsumedPerUse));
    this.cleanMeter.SetPositionPercent(percent_full1);
    this.dirtyMeter.SetPositionPercent(percent_full2);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi.StartSM();
    this.cleanMeter = new MeterController((KAnimControllerBase) this.GetComponent<KBatchedAnimController>(), "meter_clean_target", "meter_clean", this.cleanMeterOffset, Grid.SceneLayer.NoLayer, new string[1]
    {
      "meter_clean_target"
    });
    this.dirtyMeter = new MeterController((KAnimControllerBase) this.GetComponent<KBatchedAnimController>(), "meter_dirty_target", "meter_dirty", this.dirtyMeterOffset, Grid.SceneLayer.NoLayer, new string[1]
    {
      "meter_dirty_target"
    });
    this.RefreshMeters();
    Components.HandSanitizers.Add(this);
    this.Subscribe<HandSanitizer>(-1697596308, HandSanitizer.OnStorageChangeDelegate);
    this.GetComponent<DirectionControl>().onDirectionChanged += new System.Action<WorkableReactable.AllowedDirection>(this.OnDirectionChanged);
    this.OnDirectionChanged(this.GetComponent<DirectionControl>().allowedDirection);
  }

  protected override void OnCleanUp()
  {
    Components.HandSanitizers.Remove(this);
    base.OnCleanUp();
  }

  private void OnDirectionChanged(
    WorkableReactable.AllowedDirection allowed_direction)
  {
    if (this.reactable == null)
      return;
    this.reactable.allowedDirection = allowed_direction;
  }

  public List<Descriptor> RequirementDescriptors(BuildingDef def)
  {
    return new List<Descriptor>()
    {
      new Descriptor(string.Format((string) UI.BUILDINGEFFECTS.ELEMENTCONSUMEDPERUSE, (object) ElementLoader.FindElementByHash(this.consumedElement).name, (object) GameUtil.GetFormattedMass(this.massConsumedPerUse, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMEDPERUSE, (object) ElementLoader.FindElementByHash(this.consumedElement).name, (object) GameUtil.GetFormattedMass(this.massConsumedPerUse, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), Descriptor.DescriptorType.Requirement, false)
    };
  }

  public List<Descriptor> EffectDescriptors(BuildingDef def)
  {
    List<Descriptor> descriptorList = new List<Descriptor>();
    if (this.outputElement != SimHashes.Vacuum)
      descriptorList.Add(new Descriptor(string.Format((string) UI.BUILDINGEFFECTS.ELEMENTEMITTEDPERUSE, (object) ElementLoader.FindElementByHash(this.outputElement).name, (object) GameUtil.GetFormattedMass(this.massConsumedPerUse, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTEMITTEDPERUSE, (object) ElementLoader.FindElementByHash(this.outputElement).name, (object) GameUtil.GetFormattedMass(this.massConsumedPerUse, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), Descriptor.DescriptorType.Effect, false));
    descriptorList.Add(new Descriptor(string.Format((string) UI.BUILDINGEFFECTS.DISEASECONSUMEDPERUSE, (object) GameUtil.GetFormattedDiseaseAmount(this.diseaseRemovalCount)), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.DISEASECONSUMEDPERUSE, (object) GameUtil.GetFormattedDiseaseAmount(this.diseaseRemovalCount)), Descriptor.DescriptorType.Effect, false));
    return descriptorList;
  }

  public List<Descriptor> GetDescriptors(BuildingDef def)
  {
    List<Descriptor> descriptorList = new List<Descriptor>();
    descriptorList.AddRange((IEnumerable<Descriptor>) this.RequirementDescriptors(def));
    descriptorList.AddRange((IEnumerable<Descriptor>) this.EffectDescriptors(def));
    return descriptorList;
  }

  private void OnStorageChange(object data)
  {
    if (this.dumpWhenFull && this.smi.OutputFull())
      this.smi.DumpOutput();
    this.RefreshMeters();
  }

  private class WashHandsReactable : WorkableReactable
  {
    public WashHandsReactable(
      Workable workable,
      ChoreType chore_type,
      WorkableReactable.AllowedDirection allowed_direction = WorkableReactable.AllowedDirection.Any)
      : base(workable, (HashedString) "WashHands", chore_type, allowed_direction)
    {
    }

    public override bool InternalCanBegin(
      GameObject new_reactor,
      Navigator.ActiveTransition transition)
    {
      if (base.InternalCanBegin(new_reactor, transition))
      {
        PrimaryElement component = new_reactor.GetComponent<PrimaryElement>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
          return component.DiseaseIdx != byte.MaxValue;
      }
      return false;
    }
  }

  public class SMInstance : GameStateMachine<HandSanitizer.States, HandSanitizer.SMInstance, HandSanitizer, object>.GameInstance
  {
    public SMInstance(HandSanitizer master)
      : base(master)
    {
    }

    private bool HasSufficientMass()
    {
      bool flag = false;
      PrimaryElement primaryElement = this.GetComponent<Storage>().FindPrimaryElement(this.master.consumedElement);
      if ((UnityEngine.Object) primaryElement != (UnityEngine.Object) null)
        flag = (double) primaryElement.Mass > 0.0;
      return flag;
    }

    public bool OutputFull()
    {
      PrimaryElement primaryElement = this.GetComponent<Storage>().FindPrimaryElement(this.master.outputElement);
      if ((UnityEngine.Object) primaryElement != (UnityEngine.Object) null)
        return (double) primaryElement.Mass >= (double) this.master.maxUses * (double) this.master.massConsumedPerUse;
      return false;
    }

    public bool IsReady()
    {
      return this.HasSufficientMass() && !this.OutputFull();
    }

    public void OnCompleteWork(Worker worker)
    {
    }

    public void DumpOutput()
    {
      Storage component = this.master.GetComponent<Storage>();
      if (this.master.outputElement == SimHashes.Vacuum)
        return;
      component.Drop(ElementLoader.FindElementByHash(this.master.outputElement).tag);
    }
  }

  public class States : GameStateMachine<HandSanitizer.States, HandSanitizer.SMInstance, HandSanitizer>
  {
    public GameStateMachine<HandSanitizer.States, HandSanitizer.SMInstance, HandSanitizer, object>.State notready;
    public HandSanitizer.States.ReadyStates ready;
    public GameStateMachine<HandSanitizer.States, HandSanitizer.SMInstance, HandSanitizer, object>.State notoperational;
    public GameStateMachine<HandSanitizer.States, HandSanitizer.SMInstance, HandSanitizer, object>.State full;
    public GameStateMachine<HandSanitizer.States, HandSanitizer.SMInstance, HandSanitizer, object>.State empty;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.notready;
      this.root.Update(new System.Action<HandSanitizer.SMInstance, float>(this.UpdateStatusItems), UpdateRate.SIM_200ms, false);
      this.notoperational.PlayAnim("off").TagTransition(GameTags.Operational, this.notready, false);
      this.notready.PlayAnim("off").EventTransition(GameHashes.OnStorageChange, (GameStateMachine<HandSanitizer.States, HandSanitizer.SMInstance, HandSanitizer, object>.State) this.ready, (StateMachine<HandSanitizer.States, HandSanitizer.SMInstance, HandSanitizer, object>.Transition.ConditionCallback) (smi => smi.IsReady())).TagTransition(GameTags.Operational, this.notoperational, true);
      this.ready.DefaultState(this.ready.free).ToggleReactable((Func<HandSanitizer.SMInstance, Reactable>) (smi => (Reactable) (smi.master.reactable = (WorkableReactable) new HandSanitizer.WashHandsReactable((Workable) smi.master.GetComponent<HandSanitizer.Work>(), Db.Get().ChoreTypes.WashHands, smi.master.GetComponent<DirectionControl>().allowedDirection)))).TagTransition(GameTags.Operational, this.notoperational, true);
      this.ready.free.PlayAnim("on").WorkableStartTransition((Func<HandSanitizer.SMInstance, Workable>) (smi => (Workable) smi.GetComponent<HandSanitizer.Work>()), this.ready.occupied);
      this.ready.occupied.PlayAnim("working_pre").QueueAnim("working_loop", true, (Func<HandSanitizer.SMInstance, string>) null).WorkableStopTransition((Func<HandSanitizer.SMInstance, Workable>) (smi => (Workable) smi.GetComponent<HandSanitizer.Work>()), this.notready);
    }

    private void UpdateStatusItems(HandSanitizer.SMInstance smi, float dt)
    {
      if (smi.OutputFull())
        smi.master.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.OutputPipeFull, (object) this);
      else
        smi.master.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.OutputPipeFull, false);
    }

    public class ReadyStates : GameStateMachine<HandSanitizer.States, HandSanitizer.SMInstance, HandSanitizer, object>.State
    {
      public GameStateMachine<HandSanitizer.States, HandSanitizer.SMInstance, HandSanitizer, object>.State free;
      public GameStateMachine<HandSanitizer.States, HandSanitizer.SMInstance, HandSanitizer, object>.State occupied;
    }
  }

  public class Work : Workable, IGameObjectEffectDescriptor
  {
    private int diseaseRemoved;

    protected override void OnPrefabInit()
    {
      base.OnPrefabInit();
      this.resetProgressOnStop = true;
      this.shouldTransferDiseaseWithWorker = false;
      GameScheduler.Instance.Schedule("WaterFetchingTutorial", 2f, (System.Action<object>) (obj => Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_FetchingWater, true)), (object) null, (SchedulerGroup) null);
    }

    protected override void OnStartWork(Worker worker)
    {
      base.OnStartWork(worker);
      this.diseaseRemoved = 0;
    }

    protected override bool OnWorkTick(Worker worker, float dt)
    {
      base.OnWorkTick(worker, dt);
      HandSanitizer component1 = this.GetComponent<HandSanitizer>();
      Storage component2 = this.GetComponent<Storage>();
      float massAvailable = component2.GetMassAvailable(component1.consumedElement);
      if ((double) massAvailable == 0.0)
        return true;
      PrimaryElement component3 = worker.GetComponent<PrimaryElement>();
      float num1 = Mathf.Min(component1.massConsumedPerUse * dt / this.workTime, massAvailable);
      int num2 = Math.Min((int) ((double) dt / (double) this.workTime * (double) component1.diseaseRemovalCount), component3.DiseaseCount);
      this.diseaseRemoved += num2;
      SimUtil.DiseaseInfo invalid = SimUtil.DiseaseInfo.Invalid;
      invalid.idx = component3.DiseaseIdx;
      invalid.count = num2;
      component3.ModifyDiseaseCount(-num2, "HandSanitizer.OnWorkTick");
      component1.maxPossiblyRemoved += num2;
      SimUtil.DiseaseInfo disease_info = SimUtil.DiseaseInfo.Invalid;
      float aggregate_temperature;
      component2.ConsumeAndGetDisease(ElementLoader.FindElementByHash(component1.consumedElement).tag, num1, out disease_info, out aggregate_temperature);
      if (component1.outputElement != SimHashes.Vacuum)
      {
        SimUtil.DiseaseInfo finalDiseaseInfo = SimUtil.CalculateFinalDiseaseInfo(invalid, disease_info);
        component2.AddLiquid(component1.outputElement, num1, aggregate_temperature, finalDiseaseInfo.idx, finalDiseaseInfo.count, false, true);
      }
      return this.diseaseRemoved > component1.diseaseRemovalCount;
    }

    protected override void OnCompleteWork(Worker worker)
    {
      base.OnCompleteWork(worker);
    }
  }
}
