using KSerialization;

namespace AdvancedSpaceScanner
{
    public class AdvancedSpaceScannerController : GameStateMachine<AdvancedSpaceScannerController, AdvancedSpaceScannerController.Instance, IStateMachineTarget, AdvancedSpaceScannerController.Def>
    {
        public GameStateMachine<AdvancedSpaceScannerController, AdvancedSpaceScannerController.Instance, IStateMachineTarget, AdvancedSpaceScannerController.Def>.State off;
        public AdvancedSpaceScannerController.OnStates on;

        public override void InitializeStates(out StateMachine.BaseState default_state)
        {
            default_state = (StateMachine.BaseState)this.off;
            this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, (GameStateMachine<AdvancedSpaceScannerController, AdvancedSpaceScannerController.Instance, IStateMachineTarget, AdvancedSpaceScannerController.Def>.State)this.on, (StateMachine<AdvancedSpaceScannerController, AdvancedSpaceScannerController.Instance, IStateMachineTarget, AdvancedSpaceScannerController.Def>.Transition.ConditionCallback)(smi => smi.GetComponent<Operational>().IsOperational));
            this.on.DefaultState(this.on.pre).ToggleStatusItem(Db.Get().BuildingStatusItems.DetectorScanning).Enter("ToggleActive", (StateMachine<AdvancedSpaceScannerController, AdvancedSpaceScannerController.Instance, IStateMachineTarget, AdvancedSpaceScannerController.Def>.State.Callback)(smi => smi.GetComponent<Operational>().SetActive(true))).Exit("ToggleActive", (StateMachine<AdvancedSpaceScannerController, AdvancedSpaceScannerController.Instance, IStateMachineTarget, AdvancedSpaceScannerController.Def>.State.Callback)(smi => smi.GetComponent<Operational>().SetActive(false))).Update("Scan Sky", (System.Action<AdvancedSpaceScannerController.Instance, float>)((smi, dt) => smi.ScanSky()));
            this.on.pre.PlayAnim("on_pre").OnAnimQueueComplete(this.on.loop);
            this.on.loop.PlayAnim("on", KAnim.PlayMode.Loop).EventTransition(GameHashes.OperationalChanged, this.on.pst, (StateMachine<AdvancedSpaceScannerController, AdvancedSpaceScannerController.Instance, IStateMachineTarget, AdvancedSpaceScannerController.Def>.Transition.ConditionCallback)(smi => !smi.GetComponent<Operational>().IsOperational)).TagTransition(GameTags.Detecting, (GameStateMachine<AdvancedSpaceScannerController, AdvancedSpaceScannerController.Instance, IStateMachineTarget, AdvancedSpaceScannerController.Def>.State)this.on.working);
            this.on.pst.PlayAnim("on_pst").OnAnimQueueComplete(this.off);
            this.on.working.DefaultState(this.on.working.pre).ToggleStatusItem(Db.Get().BuildingStatusItems.IncomingMeteors).Enter("ToggleActive", (StateMachine<AdvancedSpaceScannerController, AdvancedSpaceScannerController.Instance, IStateMachineTarget, AdvancedSpaceScannerController.Def>.State.Callback)(smi =>
            {
                smi.GetComponent<Operational>().SetActive(true);
                smi.SetLogicSignal(true);
            })).Exit("ToggleActive", (StateMachine<AdvancedSpaceScannerController, AdvancedSpaceScannerController.Instance, IStateMachineTarget, AdvancedSpaceScannerController.Def>.State.Callback)(smi =>
            {
                smi.GetComponent<Operational>().SetActive(false);
                smi.SetLogicSignal(false);
            }));
            this.on.working.pre.PlayAnim("detect_pre").OnAnimQueueComplete(this.on.working.loop);
            this.on.working.loop.PlayAnim("detect_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.OperationalChanged, this.on.working.pst, (StateMachine<AdvancedSpaceScannerController, AdvancedSpaceScannerController.Instance, IStateMachineTarget, AdvancedSpaceScannerController.Def>.Transition.ConditionCallback)(smi => !smi.GetComponent<Operational>().IsOperational)).EventTransition(GameHashes.ActiveChanged, this.on.working.pst, (StateMachine<AdvancedSpaceScannerController, AdvancedSpaceScannerController.Instance, IStateMachineTarget, AdvancedSpaceScannerController.Def>.Transition.ConditionCallback)(smi => !smi.GetComponent<Operational>().IsActive)).TagTransition(GameTags.Detecting, this.on.working.pst, true);
            this.on.working.pst.PlayAnim("detect_pst").OnAnimQueueComplete(this.on.loop).Enter("Reroll", (StateMachine<AdvancedSpaceScannerController, AdvancedSpaceScannerController.Instance, IStateMachineTarget, AdvancedSpaceScannerController.Def>.State.Callback)(smi => smi.RerollAccuracy()));
        }

        public class Def : StateMachine.BaseDef
        {
        }

        public class OnStates : GameStateMachine<AdvancedSpaceScannerController, AdvancedSpaceScannerController.Instance, IStateMachineTarget, AdvancedSpaceScannerController.Def>.State
        {
            public GameStateMachine<AdvancedSpaceScannerController, AdvancedSpaceScannerController.Instance, IStateMachineTarget, AdvancedSpaceScannerController.Def>.State pre;
            public GameStateMachine<AdvancedSpaceScannerController, AdvancedSpaceScannerController.Instance, IStateMachineTarget, AdvancedSpaceScannerController.Def>.State loop;
            public AdvancedSpaceScannerController.WorkingStates working;
            public GameStateMachine<AdvancedSpaceScannerController, AdvancedSpaceScannerController.Instance, IStateMachineTarget, AdvancedSpaceScannerController.Def>.State pst;
        }

        public class WorkingStates : GameStateMachine<AdvancedSpaceScannerController, AdvancedSpaceScannerController.Instance, IStateMachineTarget, AdvancedSpaceScannerController.Def>.State
        {
            public GameStateMachine<AdvancedSpaceScannerController, AdvancedSpaceScannerController.Instance, IStateMachineTarget, AdvancedSpaceScannerController.Def>.State pre;
            public GameStateMachine<AdvancedSpaceScannerController, AdvancedSpaceScannerController.Instance, IStateMachineTarget, AdvancedSpaceScannerController.Def>.State loop;
            public GameStateMachine<AdvancedSpaceScannerController, AdvancedSpaceScannerController.Instance, IStateMachineTarget, AdvancedSpaceScannerController.Def>.State pst;
        }

        public new class Instance : GameStateMachine<AdvancedSpaceScannerController, AdvancedSpaceScannerController.Instance, IStateMachineTarget, AdvancedSpaceScannerController.Def>.GameInstance
        {
            public bool ShowWorkingStatus;
            private float BEST_WARNING_TIME = AdvancedSpaceScannerPatches.Config.MaximumWarningTime;
            private float WORST_WARNING_TIME = AdvancedSpaceScannerPatches.Config.MinimumWarningTime;
            private int MAX_DISH_COUNT = 6;
            private int INTERFERENCE_RADIUS = AdvancedSpaceScannerPatches.Config.InterferenceRadius;
            [Serialize]
            private float nextAccuracy;
            [Serialize]
            private Ref<LaunchConditionManager> targetCraft;
            private DetectorNetwork.Def detectorNetworkDef;
            private DetectorNetwork.Instance detectorNetwork;

            public Instance(IStateMachineTarget master, AdvancedSpaceScannerController.Def def)
              : base(master, def)
            {
                this.detectorNetworkDef = new DetectorNetwork.Def();
                this.detectorNetworkDef.interferenceRadius = INTERFERENCE_RADIUS;
                this.detectorNetworkDef.worstWarningTime = WORST_WARNING_TIME;
                this.detectorNetworkDef.bestWarningTime = BEST_WARNING_TIME;
                this.detectorNetworkDef.bestNetworkSize = MAX_DISH_COUNT;
                this.targetCraft = new Ref<LaunchConditionManager>();
                this.RerollAccuracy();
            }

            public override void StartSM()
            {
                if (this.detectorNetwork == null)
                    this.detectorNetwork = (DetectorNetwork.Instance)this.detectorNetworkDef.CreateSMI(this.master);
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
                if ((UnityEngine.Object)this.targetCraft.Get() == (UnityEngine.Object)null)
                {
                    if ((double)SaveGame.Instance.GetComponent<SeasonManager>().TimeUntilNextBombardment() <= (double)detectTime)
                        component.AddTag(GameTags.Detecting);
                    else
                        component.RemoveTag(GameTags.Detecting);
                }
                else
                {
                    Spacecraft conditionManager = SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this.targetCraft.Get());
                    if (conditionManager.state == Spacecraft.MissionState.Destroyed)
                    {
                        this.targetCraft.Set((LaunchConditionManager)null);
                        component.RemoveTag(GameTags.Detecting);
                    }
                    else if (conditionManager.state == Spacecraft.MissionState.Launching || conditionManager.state == Spacecraft.MissionState.WaitingToLand || conditionManager.state == Spacecraft.MissionState.Landing || conditionManager.state == Spacecraft.MissionState.Underway && (double)conditionManager.GetTimeLeft() <= (double)detectTime)
                        component.AddTag(GameTags.Detecting);
                    else
                        component.RemoveTag(GameTags.Detecting);
                }
            }

            public void RerollAccuracy() => this.nextAccuracy = UnityEngine.Random.value;

            public void SetLogicSignal(bool on) => this.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, on ? 1 : 0);

            public float GetDetectTime() => this.detectorNetwork.GetDetectTimeRange().Lerp(this.nextAccuracy);

            public void SetTargetCraft(LaunchConditionManager target) => this.targetCraft.Set(target);

            public LaunchConditionManager GetTargetCraft() => this.targetCraft.Get();
        }
    }
}
