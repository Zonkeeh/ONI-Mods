using Harmony;
using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using Zolibrary.Logging;

namespace DuplicantLifecycles
{
    public class Immortal : StateMachineComponent<Immortal.StatesInstance>
    {
        protected override void OnSpawn()
        {
            this.smi.StartSM();
        }

        protected bool IsImmortal() => this.gameObject.GetComponent<Worker>().GetComponent<Traits>().HasTrait(DuplicantLifecycleStrings.ImmortalID);

        

        public class StatesInstance : GameStateMachine<Immortal.States, Immortal.StatesInstance, Immortal, object>.GameInstance
        {
            public StatesInstance(Immortal master)
              : base(master)
            {
            }
        }

        public class States : GameStateMachine<Immortal.States, Immortal.StatesInstance, Immortal>
        {
            public GameStateMachine<Immortal.States, Immortal.StatesInstance, Immortal, object>.State immortal;
            public GameStateMachine<Immortal.States, Immortal.StatesInstance, Immortal, object>.State dead;

            public override void InitializeStates(out StateMachine.BaseState default_state)
            {
                default_state = this.immortal;
                this.immortal.TagTransition(GameTags.Dead, this.dead);
                this.dead.Enter("Dead", (smi => smi.StopSM("Death")));
            }
        }
    }
}
