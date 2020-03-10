using KSerialization;
using System;
using Zolibrary.Logging;

namespace AdvancedSpaceScanner
{
    public class AdvancedSpaceScanner : GameStateMachine<AdvancedSpaceScanner, AdvancedSpaceScanner.Instance, IStateMachineTarget, AdvancedSpaceScanner.Def>
    {
        public GameStateMachine<AdvancedSpaceScanner, AdvancedSpaceScanner.Instance, IStateMachineTarget, AdvancedSpaceScanner.Def>.State off;
        public AdvancedSpaceScanner.OnStates on;

        public override void InitializeStates(out StateMachine.BaseState default_state)
        {
            default_state = (StateMachine.BaseState)this.off;
            this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, (GameStateMachine<AdvancedSpaceScanner, AdvancedSpaceScanner.Instance, IStateMachineTarget, AdvancedSpaceScanner.Def>.State)this.on, (StateMachine<AdvancedSpaceScanner, AdvancedSpaceScanner.Instance, IStateMachineTarget, AdvancedSpaceScanner.Def>.Transition.ConditionCallback)(smi => smi.GetComponent<Operational>().IsOperational));
            this.on.DefaultState(this.on.pre).ToggleStatusItem(Db.Get().BuildingStatusItems.DetectorScanning, (object)null).Enter("ToggleActive", (StateMachine<AdvancedSpaceScanner, AdvancedSpaceScanner.Instance, IStateMachineTarget, AdvancedSpaceScanner.Def>.State.Callback)(smi => smi.GetComponent<Operational>().SetActive(true, false))).Exit("ToggleActive", (StateMachine<AdvancedSpaceScanner, AdvancedSpaceScanner.Instance, IStateMachineTarget, AdvancedSpaceScanner.Def>.State.Callback)(smi => smi.GetComponent<Operational>().SetActive(false, false))).Update("Scan Sky", (System.Action<AdvancedSpaceScanner.Instance, float>)((smi, dt) => smi.ScanSky()), UpdateRate.SIM_200ms, false);
            this.on.pre.PlayAnim("on_pre").OnAnimQueueComplete(this.on.loop);
            this.on.loop.PlayAnim("on", KAnim.PlayMode.Loop).EventTransition(GameHashes.OperationalChanged, this.on.pst, (StateMachine<AdvancedSpaceScanner, AdvancedSpaceScanner.Instance, IStateMachineTarget, AdvancedSpaceScanner.Def>.Transition.ConditionCallback)(smi => !smi.GetComponent<Operational>().IsOperational)).TagTransition(GameTags.Detecting, (GameStateMachine<AdvancedSpaceScanner, AdvancedSpaceScanner.Instance, IStateMachineTarget, AdvancedSpaceScanner.Def>.State)this.on.working, false);
            this.on.pst.PlayAnim("on_pst").OnAnimQueueComplete(this.off);
            this.on.working.DefaultState(this.on.working.pre).ToggleStatusItem(Db.Get().BuildingStatusItems.IncomingMeteors, (object)null).Enter("ToggleActive", (StateMachine<AdvancedSpaceScanner, AdvancedSpaceScanner.Instance, IStateMachineTarget, AdvancedSpaceScanner.Def>.State.Callback)(smi =>
            {
                smi.GetComponent<Operational>().SetActive(true, false);
                smi.SetLogicSignal(true);
            })).Exit("ToggleActive", (StateMachine<AdvancedSpaceScanner, AdvancedSpaceScanner.Instance, IStateMachineTarget, AdvancedSpaceScanner.Def>.State.Callback)(smi =>
            {
                smi.GetComponent<Operational>().SetActive(false, false);
                smi.SetLogicSignal(false);
            }));
            this.on.working.pre.PlayAnim("detect_pre").OnAnimQueueComplete(this.on.working.loop);
            this.on.working.loop.PlayAnim("detect_loop", KAnim.PlayMode.Loop).EventTransition(GameHashes.OperationalChanged, this.on.working.pst, (StateMachine<AdvancedSpaceScanner, AdvancedSpaceScanner.Instance, IStateMachineTarget, AdvancedSpaceScanner.Def>.Transition.ConditionCallback)(smi => !smi.GetComponent<Operational>().IsOperational)).EventTransition(GameHashes.ActiveChanged, this.on.working.pst, (StateMachine<AdvancedSpaceScanner, AdvancedSpaceScanner.Instance, IStateMachineTarget, AdvancedSpaceScanner.Def>.Transition.ConditionCallback)(smi => !smi.GetComponent<Operational>().IsActive)).TagTransition(GameTags.Detecting, this.on.working.pst, true);
            this.on.working.pst.PlayAnim("detect_pst").OnAnimQueueComplete(this.on.loop).Enter("Reroll", (StateMachine<AdvancedSpaceScanner, AdvancedSpaceScanner.Instance, IStateMachineTarget, AdvancedSpaceScanner.Def>.State.Callback)(smi => smi.RerollAccuracy()));
        }

        public class Def : StateMachine.BaseDef
        {
        }

        public class OnStates : GameStateMachine<AdvancedSpaceScanner, AdvancedSpaceScanner.Instance, IStateMachineTarget, AdvancedSpaceScanner.Def>.State
        {
            public GameStateMachine<AdvancedSpaceScanner, AdvancedSpaceScanner.Instance, IStateMachineTarget, AdvancedSpaceScanner.Def>.State pre;
            public GameStateMachine<AdvancedSpaceScanner, AdvancedSpaceScanner.Instance, IStateMachineTarget, AdvancedSpaceScanner.Def>.State loop;
            public AdvancedSpaceScanner.WorkingStates working;
            public GameStateMachine<AdvancedSpaceScanner, AdvancedSpaceScanner.Instance, IStateMachineTarget, AdvancedSpaceScanner.Def>.State pst;
        }

        public class WorkingStates : GameStateMachine<AdvancedSpaceScanner, AdvancedSpaceScanner.Instance, IStateMachineTarget, AdvancedSpaceScanner.Def>.State
        {
            public GameStateMachine<AdvancedSpaceScanner, AdvancedSpaceScanner.Instance, IStateMachineTarget, AdvancedSpaceScanner.Def>.State pre;
            public GameStateMachine<AdvancedSpaceScanner, AdvancedSpaceScanner.Instance, IStateMachineTarget, AdvancedSpaceScanner.Def>.State loop;
            public GameStateMachine<AdvancedSpaceScanner, AdvancedSpaceScanner.Instance, IStateMachineTarget, AdvancedSpaceScanner.Def>.State pst;
        }

        public class Instance : GameStateMachine<AdvancedSpaceScanner, AdvancedSpaceScanner.Instance, IStateMachineTarget, AdvancedSpaceScanner.Def>.GameInstance
        {
            public bool ShowWorkingStatus;
            private float BEST_WARNING_TIME = 300f;
            private float WORST_WARNING_TIME = 30f;
            private float VARIANCE = 50f;
            private int MAX_DISH_COUNT = 6;
            private int INTERFERENCE_RADIUS = 10;
            [Serialize]
            private float nextAccuracy;
            [Serialize]
            private Ref<LaunchConditionManager> targetCraft;
            private DetectorNetwork.Def detectorNetworkDef;
            private DetectorNetwork.Instance detectorNetwork;

            private void CheckValidConfigVars()
            {
                if (AdvancedSpaceScannerPatches.Config.MaximumWarningTime >= AdvancedSpaceScannerPatches.Config.MinimumWarningTime)
                    BEST_WARNING_TIME = AdvancedSpaceScannerPatches.Config.MaximumWarningTime;
                else
                    LogManager.LogException("MaximumWarningTime was set to an invalid time, either less than 1 or less than the Minimum: " + AdvancedSpaceScannerPatches.Config.MaximumWarningTime, new ArgumentOutOfRangeException("MaximumWarningTime:" + AdvancedSpaceScannerPatches.Config.MaximumWarningTime));

                if (AdvancedSpaceScannerPatches.Config.MinimumWarningTime >= 1 && AdvancedSpaceScannerPatches.Config.MinimumWarningTime <= AdvancedSpaceScannerPatches.Config.MaximumWarningTime)
                    WORST_WARNING_TIME = AdvancedSpaceScannerPatches.Config.MinimumWarningTime;
                else
                    LogManager.LogException("MiniumWarningTime was set to an invalid time, either less than 1 or more than the Maximum: " + AdvancedSpaceScannerPatches.Config.MinimumWarningTime, new ArgumentOutOfRangeException("MinimumWarningTime:" + AdvancedSpaceScannerPatches.Config.MinimumWarningTime));

                if (AdvancedSpaceScannerPatches.Config.InterferenceRadius >= 1)
                    INTERFERENCE_RADIUS = AdvancedSpaceScannerPatches.Config.InterferenceRadius;
                else
                    LogManager.LogException("InterferenceRadius was set to an invalid integer: " + AdvancedSpaceScannerPatches.Config.InterferenceRadius, new ArgumentOutOfRangeException("InterferenceRadius:" + AdvancedSpaceScannerPatches.Config.InterferenceRadius));
            }

            public Instance(IStateMachineTarget master, AdvancedSpaceScanner.Def def)
              : base(master, def)
            {
                CheckValidConfigVars();

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
                        component.AddTag(GameTags.Detecting, false);
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
}
