using Klei.AI;

namespace DuplicantLifecycles
{
    public class ImmortalTrait : StateMachineComponent<ImmortalTrait.StatesInstance>
    {
        protected override void OnSpawn() => this.smi.StartSM();

        protected bool IsImmortal() => this.gameObject.GetComponent<Worker>().GetComponent<Traits>().HasTrait(DuplicantLifecycleStrings.ImmortalID);

        public class StatesInstance : GameStateMachine<ImmortalTrait.States, ImmortalTrait.StatesInstance, ImmortalTrait, object>.GameInstance
        {
            public StatesInstance(ImmortalTrait master) : base(master) { }
        }

        public class States : GameStateMachine<ImmortalTrait.States, ImmortalTrait.StatesInstance, ImmortalTrait>
        {
            public GameStateMachine<ImmortalTrait.States, ImmortalTrait.StatesInstance, ImmortalTrait, object>.State immortal;
            public GameStateMachine<ImmortalTrait.States, ImmortalTrait.StatesInstance, ImmortalTrait, object>.State dead;

            public override void InitializeStates(out StateMachine.BaseState default_state)
            {
                default_state = this.immortal;
                this.immortal.TagTransition(GameTags.Dead, this.dead);
                this.dead.Enter("Dead", (smi => smi.StopSM("Death")));
            }
        }
    }
}
