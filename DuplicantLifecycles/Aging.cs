using Harmony;
using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using Zolibrary.Logging;

namespace DuplicantLifecycles
{
    public class Aging : StateMachineComponent<Aging.StatesInstance>
    {
        private float maxAge;
        private float middleAge;
        private float elderlyAge;
        private float dyingAge;

        private bool usePercentage;
        private float customBase;
        private float probabilityDecrease;
        private int timeToDieCycleCheck;


        private float immortalAttributeMultiplier;
        private List<AttributeModifier> immortalAttributeModifiers;
        private float youthAttributeMultiplier;
        private List<AttributeModifier> youthAttributeModifiers;
        private float middleAttributeMultiplier;
        private List<AttributeModifier> middleAttributeModifiers;
        private float elderlyAttributeMultiplier;
        private List<AttributeModifier> elderlyAttributeModifiers;
        private float dyingAttributeMultiplier;
        private List<AttributeModifier> dyingAttributeModifiers;

        protected override void OnSpawn()
        {
            this.timeToDieCycleCheck = 0;

            this.maxAge = DuplicantLifecycleConfigChecker.MaxAge;
            this.middleAge = DuplicantLifecycleConfigChecker.MiddleAge;
            this.elderlyAge = DuplicantLifecycleConfigChecker.ElderlyAge;
            this.dyingAge = DuplicantLifecycleConfigChecker.DyingAge;
            this.immortalAttributeMultiplier = DuplicantLifecycleConfigChecker.ImmortalAttributeMultiplier;
            this.youthAttributeMultiplier = DuplicantLifecycleConfigChecker.YouthAttributeMultiplier;
            this.middleAttributeMultiplier = DuplicantLifecycleConfigChecker.MiddleAttributeMultiplier;
            this.elderlyAttributeMultiplier = DuplicantLifecycleConfigChecker.ElderlyAttributeMultiplier;
            this.dyingAttributeMultiplier = DuplicantLifecycleConfigChecker.DyingAttributeMultiplier; 
            this.usePercentage = DuplicantLifecycleConfigChecker.UsePercentage;
            this.customBase = DuplicantLifecycleConfigChecker.CustomBase;
            this.probabilityDecrease = DuplicantLifecycleConfigChecker.ProbabilityDecrease;

            this.SetupModifierLists();

            this.smi.StartSM();
        }

        private void SetupModifierLists()
        {
            this.immortalAttributeModifiers = this.CalculateAgeModifiers(immortalAttributeMultiplier);
            this.youthAttributeModifiers = this.CalculateAgeModifiers(youthAttributeMultiplier);
            this.middleAttributeModifiers = this.CalculateAgeModifiers(middleAttributeMultiplier);
            this.elderlyAttributeModifiers = this.CalculateAgeModifiers(elderlyAttributeMultiplier);
            this.dyingAttributeModifiers = this.CalculateAgeModifiers(dyingAttributeMultiplier);
        }

        private List<AttributeModifier> CalculateAgeModifiers(float multiplier)
        {
            return new List<AttributeModifier>
            {
                new AttributeModifier("Construction",   this.GetModifierDelta("Construction", multiplier),(string) DuplicantLifecycleStrings.AgingNAME, false, false, false),
                new AttributeModifier("Digging",        this.GetModifierDelta("Digging", multiplier),     (string) DuplicantLifecycleStrings.AgingNAME, false, false, false),
                new AttributeModifier("Machinery",      this.GetModifierDelta("Machinery", multiplier),   (string) DuplicantLifecycleStrings.AgingNAME, false, false, false),
                new AttributeModifier("Athletics",      this.GetModifierDelta("Athletics", multiplier),   (string) DuplicantLifecycleStrings.AgingNAME, false, false, false),
                new AttributeModifier("Learning",       this.GetModifierDelta("Learning", multiplier),    (string) DuplicantLifecycleStrings.AgingNAME, false, false, false),
                new AttributeModifier("Cooking",        this.GetModifierDelta("Cooking", multiplier),     (string) DuplicantLifecycleStrings.AgingNAME, false, false, false),
                new AttributeModifier("Art",            this.GetModifierDelta("Art", multiplier),         (string) DuplicantLifecycleStrings.AgingNAME, false, false, false),
                new AttributeModifier("Strength",       this.GetModifierDelta("Strength", multiplier),    (string) DuplicantLifecycleStrings.AgingNAME, false, false, false),
                new AttributeModifier("Caring",         this.GetModifierDelta("Caring", multiplier),      (string) DuplicantLifecycleStrings.AgingNAME, false, false, false),
                new AttributeModifier("Botanist",       this.GetModifierDelta("Botanist", multiplier),    (string) DuplicantLifecycleStrings.AgingNAME, false, false, false),
                new AttributeModifier("Ranching",       this.GetModifierDelta("Ranching", multiplier),    (string) DuplicantLifecycleStrings.AgingNAME, false, false, false)
            };
        }

        private float GetModifierDelta(string id, float multiplier)
        {
            float base_value = this.customBase;

            if (this.usePercentage)
            {
                AttributeInstance i = this.gameObject.GetAttributes().Get(id);

                if (i == null)
                    LogManager.LogException("Attribute id does not exist.", new ArgumentNullException("Attribute ID: " + id));
                else if (i.GetTotalValue() > 0)
                    base_value = i.GetTotalValue();
                else
                    base_value = 0;
            }
            float delta = (Math.Abs(base_value) * multiplier);
            #if DEBUG
                LogManager.LogDebug(this.name + " : " + (multiplier * 100) + "% : " + id + " : " + delta);
            #endif
            return delta;
        }

        private float CurrentMinionAge()
        {
            float arrivalCycle = this.smi.master.GetComponent<MinionIdentity>().arrivalTime;
            float currentCycle = GameClock.Instance.GetCycle();
            return currentCycle - arrivalCycle;
            
        }

        protected bool IsImmortal() => this.GetComponent<Worker>().GetComponent<Traits>().HasTrait(DuplicantLifecycleStrings.ImmortalID);
        protected void ApplyImmortalModifiers() => this.ApplyModifiers(this.immortalAttributeModifiers);
        protected bool IsYouthful() => CurrentMinionAge() <= this.middleAge;
        protected void ApplyYouthfulModifiers() => this.ApplyModifiers(this.youthAttributeModifiers);
        protected bool IsMiddleAged() => CurrentMinionAge() >= this.middleAge && CurrentMinionAge() < this.elderlyAge;
        protected void ApplyMiddleAgedModifiers() => this.ApplyModifiers(this.middleAttributeModifiers);
        protected bool IsElderly() => CurrentMinionAge() >= this.elderlyAge && CurrentMinionAge() < this.dyingAge;
        protected void ApplyElderlyModifiers() => this.ApplyModifiers(this.elderlyAttributeModifiers);
        protected bool IsDying() => CurrentMinionAge() >= this.dyingAge;
        protected bool IsDead() => this.gameObject.HasTag(GameTags.Dead);
        protected void ApplyDyingModifiers() => this.ApplyModifiers(this.dyingAttributeModifiers);

        protected bool TimeToDie()
        {
            if (!DuplicantLifecycleConfigChecker.EnableDeath)
                return false;

            if (this.timeToDieCycleCheck == GameClock.Instance.GetCycle())
                return false;
            else
                this.timeToDieCycleCheck = GameClock.Instance.GetCycle();

            float minionAge = this.CurrentMinionAge();

            if (minionAge >= this.maxAge)
                return true;

            float cyclesTillMax = (this.maxAge) - minionAge;
            Random gen = new Random();
            int prob = gen.Next((int) cyclesTillMax + (int) probabilityDecrease);

            if (prob <= 1)
                this.KillDuplicant();

            return prob <= 1;
        }

        private void ApplyModifiers(List<AttributeModifier> toAdd)
        {
            Attributes attributes = this.gameObject.GetAttributes();

            foreach (AttributeModifier am in toAdd)
                attributes.Add(am);
        }

        protected void RemoveModifiers(List<AttributeModifier> toRemove)
        {
            Attributes attributes = this.gameObject.GetAttributes();
            foreach (AttributeModifier am in toRemove)
                attributes.Remove(am);
        }

        protected void KillDuplicant()
        {
            this.RemoveModifiers(this.dyingAttributeModifiers);
            this.gameObject.GetSMI<DeathMonitor.Instance>().Kill(DuplicantLifeCyclePatches.oldAgeDeath);
        }

        public class StatesInstance : GameStateMachine<Aging.States, Aging.StatesInstance, Aging, object>.GameInstance
        {
            public StatesInstance(Aging master)
              : base(master)
            {
            }
        }

        public class States : GameStateMachine<Aging.States, Aging.StatesInstance, Aging>
        {
            public GameStateMachine<Aging.States, Aging.StatesInstance, Aging, object>.State noage;
            public GameStateMachine<Aging.States, Aging.StatesInstance, Aging, object>.State immortal;
            public GameStateMachine<Aging.States, Aging.StatesInstance, Aging, object>.State youthful;
            public GameStateMachine<Aging.States, Aging.StatesInstance, Aging, object>.State middleaged;
            public GameStateMachine<Aging.States, Aging.StatesInstance, Aging, object>.State elderly;
            public GameStateMachine<Aging.States, Aging.StatesInstance, Aging, object>.State dying;
            public GameStateMachine<Aging.States, Aging.StatesInstance, Aging, object>.State dead;

            public override void InitializeStates(out StateMachine.BaseState default_state)
            {
                default_state = this.noage;
                this.noage.Transition(this.immortal, (smi => smi.master.IsImmortal()), UpdateRate.SIM_200ms).Transition(this.youthful, (smi => smi.master.IsYouthful()), UpdateRate.SIM_200ms).Transition(this.middleaged, (smi => !smi.master.IsYouthful() && !smi.master.IsElderly() && !smi.master.IsDying()), UpdateRate.SIM_200ms).Transition(this.elderly, (smi => smi.master.IsElderly()), UpdateRate.SIM_200ms).Transition(this.dying, (smi => smi.master.IsDying()), UpdateRate.SIM_200ms).Transition(this.dead, (smi => smi.master.IsDead()), UpdateRate.SIM_200ms);
                this.immortal.Enter("Immortal", (smi => smi.master.ApplyImmortalModifiers())).Exit("NotImmortal", (smi => smi.master.RemoveModifiers(smi.master.immortalAttributeModifiers))).ToggleStatusItem(DuplicantLifecycleStrings.Immortal, null).ToggleExpression(Db.Get().Expressions.Relief, null).TagTransition(GameTags.Dead, this.dead);
                this.youthful.Enter("Youthful", (smi => smi.master.ApplyYouthfulModifiers())).Exit("NotYouthful", (smi => smi.master.RemoveModifiers(smi.master.youthAttributeModifiers))).ToggleStatusItem(DuplicantLifecycleStrings.AgingYouth, null).ToggleExpression(Db.Get().Expressions.Happy, null).Transition(this.middleaged, (smi => smi.master.IsMiddleAged()), UpdateRate.SIM_4000ms).Transition(this.immortal, (smi => smi.master.IsImmortal()), UpdateRate.SIM_1000ms).TagTransition(GameTags.Dead, this.dead);
                this.middleaged.Enter("MiddleAged", (smi => smi.master.ApplyMiddleAgedModifiers())).Exit("NotMiddleAged", (smi => smi.master.RemoveModifiers(smi.master.middleAttributeModifiers))).ToggleStatusItem(DuplicantLifecycleStrings.AgingMiddle, null).Transition(this.elderly, (smi => smi.master.IsElderly()), UpdateRate.SIM_4000ms).Transition(this.immortal, (smi => smi.master.IsImmortal()), UpdateRate.SIM_1000ms).TagTransition(GameTags.Dead, this.dead);
                this.elderly.Enter("Elderly", (smi => smi.master.ApplyElderlyModifiers())).Exit("NotElderly", (smi => smi.master.RemoveModifiers(smi.master.elderlyAttributeModifiers))).ToggleStatusItem(DuplicantLifecycleStrings.AgingElderly, null).ToggleExpression(Db.Get().Expressions.Tired, null).Transition(this.dying, (smi => smi.master.IsDying()), UpdateRate.SIM_4000ms).Transition(this.immortal, (smi => smi.master.IsImmortal()), UpdateRate.SIM_1000ms).TagTransition(GameTags.Dead, this.dead);
                this.dying.Enter("Dying", (smi => smi.master.ApplyDyingModifiers())).ToggleStatusItem(DuplicantLifecycleStrings.AgingDying, null).ToggleExpression(Db.Get().Expressions.Sick, null).Transition(this.dead, (smi => smi.master.TimeToDie()), UpdateRate.SIM_4000ms).Transition(this.immortal, (smi => smi.master.IsImmortal()), UpdateRate.SIM_1000ms).TagTransition(GameTags.Dead, this.dead);
                this.dead.Enter("Dead", (smi => smi.StopSM("Death")));
            }
        }
    }
}
