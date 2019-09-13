// Decompiled with JetBrains decompiler
// Type: OreScrubber
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

public class OreScrubber : StateMachineComponent<OreScrubber.SMInstance>, IEffectDescriptor
{
  private static readonly EventSystem.IntraObjectHandler<OreScrubber> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<OreScrubber>((System.Action<OreScrubber, object>) ((component, data) => component.OnStorageChange(data)));
  public float massConsumedPerUse = 1f;
  public SimHashes consumedElement = SimHashes.BleachStone;
  public int diseaseRemovalCount = 10000;
  public SimHashes outputElement = SimHashes.Vacuum;
  private WorkableReactable reactable;
  private MeterController cleanMeter;
  [Serialize]
  public int maxPossiblyRemoved;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.gameObject.FindOrAddComponent<Workable>();
  }

  private void RefreshMeters()
  {
    float percent_full = 0.0f;
    PrimaryElement primaryElement = this.GetComponent<Storage>().FindPrimaryElement(this.consumedElement);
    if ((UnityEngine.Object) primaryElement != (UnityEngine.Object) null)
      percent_full = Mathf.Clamp01(primaryElement.Mass / this.GetComponent<ConduitConsumer>().capacityKG);
    this.cleanMeter.SetPositionPercent(percent_full);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi.StartSM();
    this.cleanMeter = new MeterController((KAnimControllerBase) this.GetComponent<KBatchedAnimController>(), "meter_clean_target", "meter_clean", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[1]
    {
      "meter_clean_target"
    });
    this.RefreshMeters();
    this.Subscribe<OreScrubber>(-1697596308, OreScrubber.OnStorageChangeDelegate);
    this.GetComponent<DirectionControl>().onDirectionChanged += new System.Action<WorkableReactable.AllowedDirection>(this.OnDirectionChanged);
    this.OnDirectionChanged(this.GetComponent<DirectionControl>().allowedDirection);
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
    List<Descriptor> descriptorList = new List<Descriptor>();
    string name = ElementLoader.FindElementByHash(this.consumedElement).name;
    descriptorList.Add(new Descriptor(string.Format((string) UI.BUILDINGEFFECTS.ELEMENTCONSUMEDPERUSE, (object) name, (object) GameUtil.GetFormattedMass(this.massConsumedPerUse, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMEDPERUSE, (object) name, (object) GameUtil.GetFormattedMass(this.massConsumedPerUse, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), Descriptor.DescriptorType.Requirement, false));
    return descriptorList;
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
    this.RefreshMeters();
  }

  private static PrimaryElement GetFirstInfected(Storage storage)
  {
    foreach (GameObject go in storage.items)
    {
      if (!((UnityEngine.Object) go == (UnityEngine.Object) null))
      {
        PrimaryElement component = go.GetComponent<PrimaryElement>();
        if (component.DiseaseIdx != byte.MaxValue && !go.HasTag(GameTags.Edible))
          return component;
      }
    }
    return (PrimaryElement) null;
  }

  private class ScrubOreReactable : WorkableReactable
  {
    public ScrubOreReactable(
      Workable workable,
      ChoreType chore_type,
      WorkableReactable.AllowedDirection allowed_direction = WorkableReactable.AllowedDirection.Any)
      : base(workable, (HashedString) "ScrubOre", chore_type, allowed_direction)
    {
    }

    public override bool InternalCanBegin(
      GameObject new_reactor,
      Navigator.ActiveTransition transition)
    {
      if (base.InternalCanBegin(new_reactor, transition))
      {
        Storage component = new_reactor.GetComponent<Storage>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null && (UnityEngine.Object) OreScrubber.GetFirstInfected(component) != (UnityEngine.Object) null)
          return true;
      }
      return false;
    }
  }

  public class SMInstance : GameStateMachine<OreScrubber.States, OreScrubber.SMInstance, OreScrubber, object>.GameInstance
  {
    public SMInstance(OreScrubber master)
      : base(master)
    {
    }

    public bool HasSufficientMass()
    {
      bool flag = false;
      PrimaryElement primaryElement = this.GetComponent<Storage>().FindPrimaryElement(this.master.consumedElement);
      if ((UnityEngine.Object) primaryElement != (UnityEngine.Object) null)
        flag = (double) primaryElement.Mass > 0.0;
      return flag;
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

  public class States : GameStateMachine<OreScrubber.States, OreScrubber.SMInstance, OreScrubber>
  {
    public GameStateMachine<OreScrubber.States, OreScrubber.SMInstance, OreScrubber, object>.State notready;
    public OreScrubber.States.ReadyStates ready;
    public GameStateMachine<OreScrubber.States, OreScrubber.SMInstance, OreScrubber, object>.State notoperational;
    public GameStateMachine<OreScrubber.States, OreScrubber.SMInstance, OreScrubber, object>.State full;
    public GameStateMachine<OreScrubber.States, OreScrubber.SMInstance, OreScrubber, object>.State empty;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.notready;
      this.serializable = true;
      this.notoperational.PlayAnim("off").TagTransition(GameTags.Operational, this.notready, false);
      this.notready.PlayAnim("off").EventTransition(GameHashes.OnStorageChange, (GameStateMachine<OreScrubber.States, OreScrubber.SMInstance, OreScrubber, object>.State) this.ready, (StateMachine<OreScrubber.States, OreScrubber.SMInstance, OreScrubber, object>.Transition.ConditionCallback) (smi => smi.HasSufficientMass())).TagTransition(GameTags.Operational, this.notoperational, true);
      this.ready.DefaultState(this.ready.free).ToggleReactable((Func<OreScrubber.SMInstance, Reactable>) (smi => (Reactable) (smi.master.reactable = (WorkableReactable) new OreScrubber.ScrubOreReactable((Workable) smi.master.GetComponent<OreScrubber.Work>(), Db.Get().ChoreTypes.ScrubOre, smi.master.GetComponent<DirectionControl>().allowedDirection)))).EventTransition(GameHashes.OnStorageChange, this.notready, (StateMachine<OreScrubber.States, OreScrubber.SMInstance, OreScrubber, object>.Transition.ConditionCallback) (smi => !smi.HasSufficientMass())).TagTransition(GameTags.Operational, this.notoperational, true);
      this.ready.free.PlayAnim("on").WorkableStartTransition((Func<OreScrubber.SMInstance, Workable>) (smi => (Workable) smi.GetComponent<OreScrubber.Work>()), this.ready.occupied);
      this.ready.occupied.PlayAnim("working_pre").QueueAnim("working_loop", true, (Func<OreScrubber.SMInstance, string>) null).WorkableStopTransition((Func<OreScrubber.SMInstance, Workable>) (smi => (Workable) smi.GetComponent<OreScrubber.Work>()), (GameStateMachine<OreScrubber.States, OreScrubber.SMInstance, OreScrubber, object>.State) this.ready);
    }

    public class ReadyStates : GameStateMachine<OreScrubber.States, OreScrubber.SMInstance, OreScrubber, object>.State
    {
      public GameStateMachine<OreScrubber.States, OreScrubber.SMInstance, OreScrubber, object>.State free;
      public GameStateMachine<OreScrubber.States, OreScrubber.SMInstance, OreScrubber, object>.State occupied;
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
    }

    protected override void OnStartWork(Worker worker)
    {
      base.OnStartWork(worker);
      this.diseaseRemoved = 0;
    }

    protected override bool OnWorkTick(Worker worker, float dt)
    {
      base.OnWorkTick(worker, dt);
      OreScrubber component1 = this.GetComponent<OreScrubber>();
      Storage component2 = this.GetComponent<Storage>();
      PrimaryElement firstInfected = OreScrubber.GetFirstInfected(worker.GetComponent<Storage>());
      int num1 = 0;
      SimUtil.DiseaseInfo invalid = SimUtil.DiseaseInfo.Invalid;
      if ((UnityEngine.Object) firstInfected != (UnityEngine.Object) null)
      {
        num1 = Math.Min((int) ((double) dt / (double) this.workTime * (double) component1.diseaseRemovalCount), firstInfected.DiseaseCount);
        this.diseaseRemoved += num1;
        invalid.idx = firstInfected.DiseaseIdx;
        invalid.count = num1;
        firstInfected.ModifyDiseaseCount(-num1, "OreScrubber.OnWorkTick");
      }
      component1.maxPossiblyRemoved += num1;
      float num2 = component1.massConsumedPerUse * dt / this.workTime;
      SimUtil.DiseaseInfo disease_info = SimUtil.DiseaseInfo.Invalid;
      float aggregate_temperature;
      component2.ConsumeAndGetDisease(ElementLoader.FindElementByHash(component1.consumedElement).tag, num2, out disease_info, out aggregate_temperature);
      if (component1.outputElement != SimHashes.Vacuum)
      {
        disease_info = SimUtil.CalculateFinalDiseaseInfo(invalid, disease_info);
        component2.AddLiquid(component1.outputElement, num2, aggregate_temperature, disease_info.idx, disease_info.count, false, true);
      }
      return this.diseaseRemoved > component1.diseaseRemovalCount;
    }

    protected override void OnCompleteWork(Worker worker)
    {
      base.OnCompleteWork(worker);
    }
  }
}
