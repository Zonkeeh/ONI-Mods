// Decompiled with JetBrains decompiler
// Type: CometDetector
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;

public class CometDetector : GameStateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>
{
  public GameStateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.State off;
  public CometDetector.OnStates on;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.off;
    this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, (GameStateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.State) this.on, (StateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.Transition.ConditionCallback) (smi => smi.GetComponent<Operational>().IsOperational));
    this.on.DefaultState(this.on.pre).ToggleStatusItem(Db.Get().BuildingStatusItems.DetectorScanning, (object) null).Enter("ToggleActive", (StateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.State.Callback) (smi => smi.GetComponent<Operational>().SetActive(true, false))).Exit("ToggleActive", (StateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.State.Callback) (smi => smi.GetComponent<Operational>().SetActive(false, false))).Update("Scan Sky", (System.Action<CometDetector.Instance, float>) ((smi, dt) => smi.ScanSky()), UpdateRate.SIM_200ms, false);
    this.on.pre.PlayAnim("on_pre").OnAnimQueueComplete(this.on.loop);
    this.on.loop.PlayAnim("on", KAnim.PlayMode.Loop).EventTransition(GameHashes.OperationalChanged, this.on.pst, (StateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.Transition.ConditionCallback) (smi => !smi.GetComponent<Operational>().IsOperational)).TagTransition(GameTags.Detecting, (GameStateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.State) this.on.working, false);
    this.on.pst.PlayAnim("on_pst").OnAnimQueueComplete(this.off);
    this.on.working.DefaultState(this.on.working.pre).ToggleStatusItem(Db.Get().BuildingStatusItems.IncomingMeteors, (object) null).Enter("ToggleActive", (StateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.State.Callback) (smi =>
    {
      smi.GetComponent<Operational>().SetActive(true, false);
      smi.SetLogicSignal(true);
    })).Exit("ToggleActive", (StateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.State.Callback) (smi =>
    {
      smi.GetComponent<Operational>().SetActive(false, false);
      smi.SetLogicSignal(false);
    }));
    this.on.working.pre.PlayAnim("detect_pre").OnAnimQueueComplete(this.on.working.loop);
    this.on.working.loop.PlayAnim("detect_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.OperationalChanged, this.on.working.pst, (StateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.Transition.ConditionCallback) (smi => !smi.GetComponent<Operational>().IsOperational)).EventTransition(GameHashes.ActiveChanged, this.on.working.pst, (StateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.Transition.ConditionCallback) (smi => !smi.GetComponent<Operational>().IsActive)).TagTransition(GameTags.Detecting, this.on.working.pst, true);
    this.on.working.pst.PlayAnim("detect_pst").OnAnimQueueComplete(this.on.loop).Enter("Reroll", (StateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.State.Callback) (smi => smi.RerollAccuracy()));
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public class OnStates : GameStateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.State
  {
    public GameStateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.State pre;
    public GameStateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.State loop;
    public CometDetector.WorkingStates working;
    public GameStateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.State pst;
  }

  public class WorkingStates : GameStateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.State
  {
    public GameStateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.State pre;
    public GameStateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.State loop;
    public GameStateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.State pst;
  }

  public class Instance : GameStateMachine<CometDetector, CometDetector.Instance, IStateMachineTarget, CometDetector.Def>.GameInstance
  {
    public bool ShowWorkingStatus;
    private const float BEST_WARNING_TIME = 200f;
    private const float WORST_WARNING_TIME = 1f;
    private const float VARIANCE = 50f;
    private const int MAX_DISH_COUNT = 6;
    private const int INTERFERENCE_RADIUS = 15;
    [Serialize]
    private float nextAccuracy;
    [Serialize]
    private Ref<LaunchConditionManager> targetCraft;
    private DetectorNetwork.Def detectorNetworkDef;
    private DetectorNetwork.Instance detectorNetwork;

    public Instance(IStateMachineTarget master, CometDetector.Def def)
      : base(master, def)
    {
      this.detectorNetworkDef = new DetectorNetwork.Def();
      this.detectorNetworkDef.interferenceRadius = 15;
      this.detectorNetworkDef.worstWarningTime = 1f;
      this.detectorNetworkDef.bestWarningTime = 200f;
      this.detectorNetworkDef.bestNetworkSize = 6;
      this.targetCraft = new Ref<LaunchConditionManager>();
      this.RerollAccuracy();
    }

    public override void StartSM()
    {
      if (this.detectorNetwork == null)
        this.detectorNetwork = (DetectorNetwork.Instance) this.detectorNetworkDef.CreateSMI(this.master);
      this.detectorNetwork.StartSM();
      base.StartSM();
    }

    public override void StopSM(string reason)
    {
      base.StopSM(reason);
      this.detectorNetwork.StopSM(reason);
    }

    public void ScanSky()
    {
      float detectTime = this.GetDetectTime();
      KPrefabID component = this.GetComponent<KPrefabID>();
      if ((UnityEngine.Object) this.targetCraft.Get() == (UnityEngine.Object) null)
      {
        if ((double) SaveGame.Instance.GetComponent<SeasonManager>().TimeUntilNextBombardment() <= (double) detectTime)
          component.AddTag(GameTags.Detecting, false);
        else
          component.RemoveTag(GameTags.Detecting);
      }
      else
      {
        Spacecraft conditionManager = SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this.targetCraft.Get());
        if (conditionManager.state == Spacecraft.MissionState.Destroyed)
        {
          this.targetCraft.Set((LaunchConditionManager) null);
          component.RemoveTag(GameTags.Detecting);
        }
        else if (conditionManager.state == Spacecraft.MissionState.Launching || conditionManager.state == Spacecraft.MissionState.WaitingToLand || conditionManager.state == Spacecraft.MissionState.Landing || conditionManager.state == Spacecraft.MissionState.Underway && (double) conditionManager.GetTimeLeft() <= (double) detectTime)
          component.AddTag(GameTags.Detecting, false);
        else
          component.RemoveTag(GameTags.Detecting);
      }
    }

    public void RerollAccuracy()
    {
      this.nextAccuracy = UnityEngine.Random.value;
    }

    public void SetLogicSignal(bool on)
    {
      this.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, !on ? 0 : 1);
    }

    public float GetDetectTime()
    {
      return this.detectorNetwork.GetDetectTimeRange().Lerp(this.nextAccuracy);
    }

    public void SetTargetCraft(LaunchConditionManager target)
    {
      this.targetCraft.Set(target);
    }

    public LaunchConditionManager GetTargetCraft()
    {
      return this.targetCraft.Get();
    }
  }
}
