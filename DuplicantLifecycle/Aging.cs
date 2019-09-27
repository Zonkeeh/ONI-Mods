using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;

namespace DuplicantLifecycle
{
    [SkipSaveFileSerialization]
    public class Aging : StateMachineComponent<Aging.StatesInstance>
    {
        private List<AttributeModifier> attributeModifiers;
        private int maxAge;
        private int middleAge;
        private int elderlyAge;
        private int dyingAge;
        private float youthAttributePercentage;
        private float elderlyAttributePercentage;
        private float dyingAttributePercentage;

        protected override void OnSpawn()
        {
            this.maxAge = 600;
            this.middleAge = 200;
            this.elderlyAge = 500;
            this.dyingAge = 550;
            this.youthAttributePercentage = 1.2f;
            this.elderlyAttributePercentage = 0.7f;
            this.dyingAttributePercentage = 0.2f;

            this.attributeModifiers = new List<AttributeModifier>
            {
                new AttributeModifier("Construction", 0,(string) DuplicantLifecycleStrings.NAME, false, false, true),
                new AttributeModifier("Digging", 0,     (string) DuplicantLifecycleStrings.NAME, false, false, true),
                new AttributeModifier("Machinery", 0,   (string) DuplicantLifecycleStrings.NAME, false, false, true),
                new AttributeModifier("Athletics", 0,   (string) DuplicantLifecycleStrings.NAME, false, false, true),
                new AttributeModifier("Learning", 0,    (string) DuplicantLifecycleStrings.NAME, false, false, true),
                new AttributeModifier("Cooking", 0,     (string) DuplicantLifecycleStrings.NAME, false, false, true),
                new AttributeModifier("Art", 0,         (string) DuplicantLifecycleStrings.NAME, false, false, true),
                new AttributeModifier("Strength", 0,    (string) DuplicantLifecycleStrings.NAME, false, false, true),
                new AttributeModifier("Caring", 0,      (string) DuplicantLifecycleStrings.NAME, false, false, true),
                new AttributeModifier("Botanist", 0,    (string) DuplicantLifecycleStrings.NAME, false, false, true),
                new AttributeModifier("Ranching", 0,    (string) DuplicantLifecycleStrings.NAME, false, false, true)
            };

            this.smi.StartSM();
        }

        private float CurrentMinionAge()
        {
            float arrivalCycle = this.smi.master.GetComponent<MinionIdentity>().arrivalTime;
            float currentCycle = GameClock.Instance.GetCycle();
            return currentCycle - arrivalCycle;
        }

        protected bool IsYouthful() => CurrentMinionAge() <= this.middleAge;
        protected void ApplyYouthfulModifiers() => this.ApplyModifiers(this.youthAttributePercentage);
        protected bool IsElderly() => CurrentMinionAge() >= this.elderlyAge && CurrentMinionAge() < this.dyingAge;
        protected void ApplyElderlyModifiers() => this.ApplyModifiers(this.elderlyAttributePercentage);
        protected bool IsDying() => CurrentMinionAge() >= this.dyingAge;
        protected void ApplyDyingModifiers() => this.ApplyModifiers(this.dyingAttributePercentage);

        protected bool TimeToDie()
        {
            float minionAge = this.CurrentMinionAge();

            if (minionAge >= this.maxAge)
                return true;

            float cyclesTillMax = (this.maxAge) - minionAge;
            Random gen = new Random();
            int prob = gen.Next((int) cyclesTillMax);
            return prob <= 1;
        }

        private void ApplyModifiers(float percentage)
        {
            Attributes attributes = this.gameObject.GetAttributes();
            List<AttributeModifier> tempList = new List<AttributeModifier>(this.attributeModifiers);

            foreach (AttributeModifier am in tempList)
            {
                am.SetValue((am.Value * percentage) - am.Value);
                attributes.Add(am);
            }
        }

        //POSSIBLE NULL ERROR AS CREATING NEW LIST;s
        public void RemoveModifiers()
        {
            Attributes attributes = this.gameObject.GetAttributes();
            foreach (AttributeModifier am in this.attributeModifiers)
                attributes.Remove(am);
        }

        public class StatesInstance : GameStateMachine<Aging.States, Aging.StatesInstance, Aging, object>.GameInstance
        {
            public StatesInstance(Aging master)
              : base(master)
            {
            }

            public void KillDuplicant()
            {
                Health minionHealth = this.smi.master.GetComponent<Health>();
                minionHealth.Damage(minionHealth.maxHitPoints);
                this.smi.master.enabled = false;
            }
        }

        public class States : GameStateMachine<Aging.States, Aging.StatesInstance, Aging>
        {
            public GameStateMachine<Aging.States, Aging.StatesInstance, Aging, object>.State noage;
            public GameStateMachine<Aging.States, Aging.StatesInstance, Aging, object>.State youthful;
            public GameStateMachine<Aging.States, Aging.StatesInstance, Aging, object>.State middleaged;
            public GameStateMachine<Aging.States, Aging.StatesInstance, Aging, object>.State elderly;
            public GameStateMachine<Aging.States, Aging.StatesInstance, Aging, object>.State dying;

            public override void InitializeStates(out StateMachine.BaseState default_state)
            {
                default_state = this.noage;
                this.noage.Transition(this.youthful, (smi => smi.master.IsYouthful()), UpdateRate.SIM_200ms).Transition(this.middleaged, (smi => !smi.master.IsYouthful() && !smi.master.IsElderly() && !smi.master.IsDying()), UpdateRate.SIM_200ms).Transition(this.elderly, (smi => smi.master.IsElderly()), UpdateRate.SIM_200ms).Transition(this.dying, (smi => smi.master.IsDying()), UpdateRate.SIM_200ms);
                this.youthful.Enter("Youthful", (smi => smi.master.ApplyYouthfulModifiers())).Exit("NotYouthful", (smi => smi.master.RemoveModifiers())).ToggleStatusItem(DuplicantLifecycleStrings.AgingYouth, null).ToggleExpression(Db.Get().Expressions.Happy, null).Transition(this.middleaged, (smi => !smi.master.IsDying() && !smi.master.IsElderly() && !smi.master.IsYouthful()), UpdateRate.SIM_4000ms);
                this.middleaged.Enter("MiddleAged", null).Exit("NotMiddleAged", null).ToggleStatusItem(DuplicantLifecycleStrings.AgingMiddle, null).Transition(this.elderly, (smi => !smi.master.IsDying() && smi.master.IsElderly() && !smi.master.IsYouthful()), UpdateRate.SIM_4000ms);
                this.elderly.Enter("Elderly", (smi => smi.master.ApplyElderlyModifiers())).Exit("NotElderly", (smi => smi.master.RemoveModifiers())).ToggleStatusItem(DuplicantLifecycleStrings.AgingElderly, null).ToggleExpression(Db.Get().Expressions.Tired, null).Transition(this.dying, (smi => smi.master.IsDying() && !smi.master.IsElderly() && !smi.master.IsYouthful()), UpdateRate.SIM_4000ms);
                this.dying.Enter("Dying", (smi => smi.master.ApplyDyingModifiers())).Exit("Dead", (smi => smi.KillDuplicant())).ToggleStatusItem(DuplicantLifecycleStrings.AgingDying, null).ToggleExpression(Db.Get().Expressions.Sick, null).Transition(this.noage, (smi => smi.master.TimeToDie()));
            }
        }
    }
}
