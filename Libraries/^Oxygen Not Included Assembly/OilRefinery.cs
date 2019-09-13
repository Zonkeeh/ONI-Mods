// Decompiled with JetBrains decompiler
// Type: OilRefinery
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using TUNING;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class OilRefinery : StateMachineComponent<OilRefinery.StatesInstance>
{
  private static readonly EventSystem.IntraObjectHandler<OilRefinery> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<OilRefinery>((System.Action<OilRefinery, object>) ((component, data) => component.OnStorageChanged(data)));
  [SerializeField]
  public float overpressureWarningMass = 4.5f;
  [SerializeField]
  public float overpressureMass = 5f;
  private bool wasOverPressure;
  private float maxSrcMass;
  private float envPressure;
  private float cellCount;
  [MyCmpGet]
  private Storage storage;
  [MyCmpReq]
  private Operational operational;
  [MyCmpAdd]
  private OilRefinery.WorkableTarget workable;
  [MyCmpReq]
  private OccupyArea occupyArea;
  private const bool hasMeter = true;
  private MeterController meter;

  protected override void OnSpawn()
  {
    this.Subscribe<OilRefinery>(-1697596308, OilRefinery.OnStorageChangedDelegate);
    this.meter = new MeterController((KAnimControllerBase) this.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Vector3.zero, (string[]) null);
    this.smi.StartSM();
    this.maxSrcMass = this.GetComponent<ConduitConsumer>().capacityKG;
  }

  private void OnStorageChanged(object data)
  {
    this.meter.SetPositionPercent(Mathf.Clamp01(this.storage.GetMassAvailable(SimHashes.CrudeOil) / this.maxSrcMass));
  }

  private static bool UpdateStateCb(int cell, object data)
  {
    OilRefinery oilRefinery = data as OilRefinery;
    if (Grid.Element[cell].IsGas)
    {
      ++oilRefinery.cellCount;
      oilRefinery.envPressure += Grid.Mass[cell];
    }
    return true;
  }

  private void TestAreaPressure()
  {
    this.envPressure = 0.0f;
    this.cellCount = 0.0f;
    if (!((UnityEngine.Object) this.occupyArea != (UnityEngine.Object) null) || !((UnityEngine.Object) this.gameObject != (UnityEngine.Object) null))
      return;
    OccupyArea occupyArea = this.occupyArea;
    int cell = Grid.PosToCell(this.gameObject);
    // ISSUE: reference to a compiler-generated field
    if (OilRefinery.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      OilRefinery.\u003C\u003Ef__mg\u0024cache0 = new Func<int, object, bool>(OilRefinery.UpdateStateCb);
    }
    // ISSUE: reference to a compiler-generated field
    Func<int, object, bool> fMgCache0 = OilRefinery.\u003C\u003Ef__mg\u0024cache0;
    occupyArea.TestArea(cell, (object) this, fMgCache0);
    this.envPressure /= this.cellCount;
  }

  private bool IsOverPressure()
  {
    return (double) this.envPressure >= (double) this.overpressureMass;
  }

  private bool IsOverWarningPressure()
  {
    return (double) this.envPressure >= (double) this.overpressureWarningMass;
  }

  public class StatesInstance : GameStateMachine<OilRefinery.States, OilRefinery.StatesInstance, OilRefinery, object>.GameInstance
  {
    public StatesInstance(OilRefinery smi)
      : base(smi)
    {
    }

    public void TestAreaPressure()
    {
      this.smi.master.TestAreaPressure();
      bool flag1 = this.smi.master.IsOverPressure();
      bool flag2 = this.smi.master.IsOverWarningPressure();
      if (flag1)
      {
        this.smi.master.wasOverPressure = true;
        this.sm.isOverPressure.Set(true, this);
      }
      else
      {
        if (!this.smi.master.wasOverPressure || flag2)
          return;
        this.sm.isOverPressure.Set(false, this);
      }
    }
  }

  public class States : GameStateMachine<OilRefinery.States, OilRefinery.StatesInstance, OilRefinery>
  {
    public StateMachine<OilRefinery.States, OilRefinery.StatesInstance, OilRefinery, object>.BoolParameter isOverPressure;
    public StateMachine<OilRefinery.States, OilRefinery.StatesInstance, OilRefinery, object>.BoolParameter isOverPressureWarning;
    public GameStateMachine<OilRefinery.States, OilRefinery.StatesInstance, OilRefinery, object>.State disabled;
    public GameStateMachine<OilRefinery.States, OilRefinery.StatesInstance, OilRefinery, object>.State overpressure;
    public GameStateMachine<OilRefinery.States, OilRefinery.StatesInstance, OilRefinery, object>.State needResources;
    public GameStateMachine<OilRefinery.States, OilRefinery.StatesInstance, OilRefinery, object>.State ready;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.disabled;
      this.root.EventTransition(GameHashes.OperationalChanged, this.disabled, (StateMachine<OilRefinery.States, OilRefinery.StatesInstance, OilRefinery, object>.Transition.ConditionCallback) (smi => !smi.master.operational.IsOperational));
      this.disabled.EventTransition(GameHashes.OperationalChanged, this.needResources, (StateMachine<OilRefinery.States, OilRefinery.StatesInstance, OilRefinery, object>.Transition.ConditionCallback) (smi => smi.master.operational.IsOperational));
      this.needResources.EventTransition(GameHashes.OnStorageChange, this.ready, (StateMachine<OilRefinery.States, OilRefinery.StatesInstance, OilRefinery, object>.Transition.ConditionCallback) (smi => smi.master.GetComponent<ElementConverter>().HasEnoughMassToStartConverting()));
      this.ready.Update("Test Pressure Update", (System.Action<OilRefinery.StatesInstance, float>) ((smi, dt) => smi.TestAreaPressure()), UpdateRate.SIM_1000ms, false).ParamTransition<bool>((StateMachine<OilRefinery.States, OilRefinery.StatesInstance, OilRefinery, object>.Parameter<bool>) this.isOverPressure, this.overpressure, GameStateMachine<OilRefinery.States, OilRefinery.StatesInstance, OilRefinery, object>.IsTrue).Transition(this.needResources, (StateMachine<OilRefinery.States, OilRefinery.StatesInstance, OilRefinery, object>.Transition.ConditionCallback) (smi => !smi.master.GetComponent<ElementConverter>().HasEnoughMassToStartConverting()), UpdateRate.SIM_200ms).ToggleChore((Func<OilRefinery.StatesInstance, Chore>) (smi => (Chore) new WorkChore<OilRefinery.WorkableTarget>(Db.Get().ChoreTypes.Fabricate, (IStateMachineTarget) smi.master.workable, (ChoreProvider) null, true, (System.Action<Chore>) null, (System.Action<Chore>) null, (System.Action<Chore>) null, true, (ScheduleBlockType) null, false, true, (KAnimFile) null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true)), this.needResources);
      this.overpressure.Update("Test Pressure Update", (System.Action<OilRefinery.StatesInstance, float>) ((smi, dt) => smi.TestAreaPressure()), UpdateRate.SIM_1000ms, false).ParamTransition<bool>((StateMachine<OilRefinery.States, OilRefinery.StatesInstance, OilRefinery, object>.Parameter<bool>) this.isOverPressure, this.ready, GameStateMachine<OilRefinery.States, OilRefinery.StatesInstance, OilRefinery, object>.IsFalse).ToggleStatusItem(Db.Get().BuildingStatusItems.PressureOk, (object) null);
    }
  }

  public class WorkableTarget : Workable
  {
    [MyCmpGet]
    public Operational operational;

    protected override void OnPrefabInit()
    {
      base.OnPrefabInit();
      this.showProgressBar = false;
      this.workerStatusItem = (StatusItem) null;
      this.skillExperienceSkillGroup = Db.Get().SkillGroups.Technicals.Id;
      this.skillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
      this.overrideAnims = new KAnimFile[1]
      {
        Assets.GetAnim((HashedString) "anim_interacts_oilrefinery_kanim")
      };
    }

    protected override void OnSpawn()
    {
      base.OnSpawn();
      this.SetWorkTime(float.PositiveInfinity);
    }

    protected override void OnStartWork(Worker worker)
    {
      this.operational.SetActive(true, false);
    }

    protected override void OnStopWork(Worker worker)
    {
      this.operational.SetActive(false, false);
    }

    protected override void OnCompleteWork(Worker worker)
    {
      this.operational.SetActive(false, false);
    }
  }
}
