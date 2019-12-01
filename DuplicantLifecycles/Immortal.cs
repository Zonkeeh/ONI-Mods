using Harmony;
using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using Zolibrary.Logging;

namespace DuplicantLifecycles
{
    [SkipSaveFileSerialization]
    public class Immortal : StateMachineComponent<Immortal.StatesInstance>
    {
        private bool usePercentage;
        private float customBase;

        private float immortalAttributeMultiplier;
        private List<AttributeModifier> immortalAttributeModifiers;

        protected override void OnSpawn()
        {
            this.immortalAttributeMultiplier = DuplicantLifecycleConfigChecker.ImmortalAttributeMultiplier; 
            this.usePercentage = DuplicantLifecycleConfigChecker.UsePercentage;
            this.customBase = DuplicantLifecycleConfigChecker.CustomBase;

            this.SetupModifierLists();

            this.smi.StartSM();
        }

        private void SetupModifierLists()
        {
            this.immortalAttributeModifiers = this.CalculateAgeModifiers(immortalAttributeMultiplier);
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

        protected bool IsImmortal() => this.gameObject.GetComponent<Worker>().GetComponent<Traits>().HasTrait(DuplicantLifecycleStrings.ImmortalID);

        protected void ApplyImmortalModifiers()
        {
            Traits traits_component = this.smi.master.GetComponent<Traits>();

            if (traits_component.HasTrait(DuplicantLifecycleStrings.AgingID))
                traits_component.Remove(Db.Get().traits.TryGet(DuplicantLifecycleStrings.AgingID));

            Attributes attributes = this.gameObject.GetAttributes();

            foreach (AttributeModifier am in this.immortalAttributeModifiers)
                attributes.Add(am);
        }

        protected void RemoveImmortalModifiers()
        {
            Attributes attributes = this.gameObject.GetAttributes();
            foreach (AttributeModifier am in this.immortalAttributeModifiers)
                attributes.Remove(am);
        }

        public class StatesInstance : GameStateMachine<Immortal.States, Immortal.StatesInstance, Immortal, object>.GameInstance
        {
            public StatesInstance(Immortal master)
              : base(master)
            {
            }

            public void RemoveImmortal()
            {
                this.smi.master.RemoveImmortalModifiers();

                Traits traits_component = this.smi.master.GetComponent<Traits>();

                if (traits_component.HasTrait(DuplicantLifecycleStrings.ImmortalID))
                    traits_component.Remove(Db.Get().traits.TryGet(DuplicantLifecycleStrings.ImmortalID));

                if (!traits_component.HasTrait(DuplicantLifecycleStrings.AgingID))
                    traits_component.Add(Db.Get().traits.TryGet(DuplicantLifecycleStrings.AgingID));

                this.smi.master.enabled = false;
                UnityEngine.Component.Destroy(this.gameObject.GetComponent<Immortal>());
            }
        }

        public class States : GameStateMachine<Immortal.States, Immortal.StatesInstance, Immortal>
        {
            public GameStateMachine<Immortal.States, Immortal.StatesInstance, Immortal, object>.State immortal;
            public GameStateMachine<Immortal.States, Immortal.StatesInstance, Immortal, object>.State not_immortal;

            public override void InitializeStates(out StateMachine.BaseState default_state)
            {
                default_state = this.not_immortal;
                this.not_immortal.Transition(this.immortal, (smi => smi.master.IsImmortal()), UpdateRate.SIM_200ms);
                this.immortal.Enter("Immortal", (smi => smi.master.ApplyImmortalModifiers())).Exit("Mortal", (smi => smi.RemoveImmortal())).ToggleStatusItem(DuplicantLifecycleStrings.Immortal, null).ToggleExpression(Db.Get().Expressions.Relief, null).Transition(this.not_immortal, (smi => !smi.master.IsImmortal()), UpdateRate.SIM_4000ms);
            }
        }
    }
}
