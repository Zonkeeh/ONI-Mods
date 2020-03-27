using KSerialization;
using UnityEngine;

namespace Trashcans
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public abstract class TrashcanAnim : StateMachineComponent<TrashcanAnim.SMInstance>
    {
        private static readonly EventSystem.IntraObjectHandler<TrashcanAnim> OnStorageChangedDelegate = new EventSystem.IntraObjectHandler<TrashcanAnim>(((component, data) => component.OnStorageChanged(data)));
        [MyCmpReq]
        private Operational operational;
        [MyCmpAdd]
        private Storage storage;
        private MeterController meter;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            this.meter = new MeterController(this, Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[0]);
            this.Subscribe<TrashcanAnim>(-1697596308, TrashcanAnim.OnStorageChangedDelegate);
            this.UpdateMeter();
            this.smi.StartSM();
        }

        protected override void OnCleanUp()
        {
            base.OnCleanUp();
        }

        private void OnStorageChanged(object data)
        {
            this.UpdateMeter();
        }

        private void UpdateMeter()
        {
            this.meter.SetPositionPercent(Mathf.Clamp01(this.storage.MassStored() / this.storage.capacityKg));
        }

        protected abstract bool IsConsuming();

        private void UpdateConsuming()
        {
            this.smi.sm.consuming.Set(this.IsConsuming(), this.smi);
        }

        public class SMInstance : GameStateMachine<TrashcanAnim.States, TrashcanAnim.SMInstance, TrashcanAnim, object>.GameInstance
        {
            public SMInstance(TrashcanAnim master)
              : base(master)
            {
            }
        }

        public class States : GameStateMachine<TrashcanAnim.States, TrashcanAnim.SMInstance, TrashcanAnim>
        {
            public StateMachine<TrashcanAnim.States, TrashcanAnim.SMInstance, TrashcanAnim, object>.BoolParameter consuming;
            public GameStateMachine<TrashcanAnim.States, TrashcanAnim.SMInstance, TrashcanAnim, object>.State off;
            public GameStateMachine<TrashcanAnim.States, TrashcanAnim.SMInstance, TrashcanAnim, object>.State idle;
            public GameStateMachine<TrashcanAnim.States, TrashcanAnim.SMInstance, TrashcanAnim, object>.State working;
            public GameStateMachine<TrashcanAnim.States, TrashcanAnim.SMInstance, TrashcanAnim, object>.State post;

            public override void InitializeStates(out StateMachine.BaseState default_state)
            {
                default_state = (StateMachine.BaseState)this.off;
                this.root.Update("RefreshConsuming", ((smi, dt) => smi.master.UpdateConsuming()), UpdateRate.SIM_1000ms, false);
                this.off
                    .PlayAnim("off")
                    .EventTransition(GameHashes.OperationalChanged, this.idle, (smi => smi.GetComponent<Operational>().IsOperational));
                this.idle
                    .PlayAnim("on")
                    .EventTransition(GameHashes.OperationalChanged, this.off, (smi => !smi.GetComponent<Operational>().IsOperational))
                    .ParamTransition<bool>(this.consuming, this.working, IsTrue);             
                this.working
                    .PlayAnim("working_pre")
                    .PlayAnim("working_loop", KAnim.PlayMode.Loop)
                    .EventTransition(GameHashes.OperationalChanged, this.off, (smi => !smi.GetComponent<Operational>().IsOperational))
                    .ParamTransition<bool>(this.consuming, this.post, IsFalse);
                this.post
                    .PlayAnim("working_pst")
                    .OnAnimQueueComplete(this.idle);
            }
        }
    }
}
