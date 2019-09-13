// Decompiled with JetBrains decompiler
// Type: Klei.AI.SlimeSickness
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Klei.AI
{
  public class SlimeSickness : Sickness
  {
    private const float COUGH_FREQUENCY = 20f;
    private const float COUGH_MASS = 0.1f;
    private const int DISEASE_AMOUNT = 1000;
    public const string ID = "SlimeSickness";
    public const string RECOVERY_ID = "SlimeSicknessRecovery";

    public SlimeSickness()
      : base(nameof (SlimeSickness), Sickness.SicknessType.Pathogen, Sickness.Severity.Minor, 0.00025f, new List<Sickness.InfectionVector>()
      {
        Sickness.InfectionVector.Inhalation
      }, 2220f, "SlimeSicknessRecovery")
    {
      this.AddSicknessComponent((Sickness.SicknessComponent) new CommonSickEffectSickness());
      this.AddSicknessComponent((Sickness.SicknessComponent) new AttributeModifierSickness(new AttributeModifier[2]
      {
        new AttributeModifier("BreathDelta", -1.136364f, (string) DUPLICANTS.DISEASES.SLIMESICKNESS.NAME, false, false, true),
        new AttributeModifier("Athletics", -3f, (string) DUPLICANTS.DISEASES.SLIMESICKNESS.NAME, false, false, true)
      }));
      this.AddSicknessComponent((Sickness.SicknessComponent) new AnimatedSickness(new HashedString[1]
      {
        (HashedString) "anim_idle_sick_kanim"
      }, Db.Get().Expressions.Sick));
      this.AddSicknessComponent((Sickness.SicknessComponent) new PeriodicEmoteSickness((HashedString) "anim_idle_sick_kanim", new HashedString[3]
      {
        (HashedString) "idle_pre",
        (HashedString) "idle_default",
        (HashedString) "idle_pst"
      }, 50f));
      this.AddSicknessComponent((Sickness.SicknessComponent) new SlimeSickness.SlimeLungComponent());
    }

    public class SlimeLungComponent : Sickness.SicknessComponent
    {
      public override object OnInfect(GameObject go, SicknessInstance diseaseInstance)
      {
        SlimeSickness.SlimeLungComponent.StatesInstance statesInstance = new SlimeSickness.SlimeLungComponent.StatesInstance(diseaseInstance);
        statesInstance.StartSM();
        return (object) statesInstance;
      }

      public override void OnCure(GameObject go, object instance_data)
      {
        ((StateMachine.Instance) instance_data).StopSM("Cured");
      }

      public override List<Descriptor> GetSymptoms()
      {
        return new List<Descriptor>()
        {
          new Descriptor((string) DUPLICANTS.DISEASES.SLIMESICKNESS.COUGH_SYMPTOM, (string) DUPLICANTS.DISEASES.SLIMESICKNESS.COUGH_SYMPTOM_TOOLTIP, Descriptor.DescriptorType.SymptomAidable, false)
        };
      }

      public class StatesInstance : GameStateMachine<SlimeSickness.SlimeLungComponent.States, SlimeSickness.SlimeLungComponent.StatesInstance, SicknessInstance, object>.GameInstance
      {
        public float lastCoughTime;

        public StatesInstance(SicknessInstance master)
          : base(master)
        {
        }

        public Reactable GetReactable()
        {
          return (Reactable) new SelfEmoteReactable(this.master.gameObject, (HashedString) "SlimeLungCough", Db.Get().ChoreTypes.Cough, (HashedString) "anim_slimelungcough_kanim", 0.0f, 0.0f, float.PositiveInfinity).AddStep(new EmoteReactable.EmoteStep()
          {
            anim = (HashedString) "react",
            finishcb = new System.Action<GameObject>(this.ProduceSlime)
          }).AddStep(new EmoteReactable.EmoteStep()
          {
            startcb = new System.Action<GameObject>(this.FinishedCoughing)
          });
        }

        private void ProduceSlime(GameObject cougher)
        {
          AmountInstance amountInstance = Db.Get().Amounts.Temperature.Lookup(cougher);
          int cell = Grid.PosToCell(cougher);
          string id = Db.Get().Diseases.SlimeGerms.Id;
          Equippable equippable = this.master.gameObject.GetComponent<SuitEquipper>().IsWearingAirtightSuit();
          if ((UnityEngine.Object) equippable != (UnityEngine.Object) null)
            equippable.GetComponent<Storage>().AddGasChunk(SimHashes.ContaminatedOxygen, 0.1f, amountInstance.value, Db.Get().Diseases.GetIndex((HashedString) id), 1000, false, true);
          else
            SimMessages.AddRemoveSubstance(cell, SimHashes.ContaminatedOxygen, CellEventLogger.Instance.Cough, 0.1f, amountInstance.value, Db.Get().Diseases.GetIndex((HashedString) id), 1000, true, -1);
          PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, string.Format((string) DUPLICANTS.DISEASES.ADDED_POPFX, (object) this.master.modifier.Name, (object) 1000), cougher.transform, 1.5f, false);
        }

        private void FinishedCoughing(GameObject cougher)
        {
          this.sm.coughFinished.Trigger(this);
        }
      }

      public class States : GameStateMachine<SlimeSickness.SlimeLungComponent.States, SlimeSickness.SlimeLungComponent.StatesInstance, SicknessInstance>
      {
        public StateMachine<SlimeSickness.SlimeLungComponent.States, SlimeSickness.SlimeLungComponent.StatesInstance, SicknessInstance, object>.Signal coughFinished;
        public SlimeSickness.SlimeLungComponent.States.BreathingStates breathing;
        public GameStateMachine<SlimeSickness.SlimeLungComponent.States, SlimeSickness.SlimeLungComponent.StatesInstance, SicknessInstance, object>.State notbreathing;

        public override void InitializeStates(out StateMachine.BaseState default_state)
        {
          default_state = (StateMachine.BaseState) this.breathing;
          this.breathing.DefaultState(this.breathing.normal).TagTransition(GameTags.NoOxygen, this.notbreathing, false);
          this.breathing.normal.Enter("SetCoughTime", (StateMachine<SlimeSickness.SlimeLungComponent.States, SlimeSickness.SlimeLungComponent.StatesInstance, SicknessInstance, object>.State.Callback) (smi =>
          {
            if ((double) smi.lastCoughTime >= (double) Time.time)
              return;
            smi.lastCoughTime = Time.time;
          })).Update("Cough", (System.Action<SlimeSickness.SlimeLungComponent.StatesInstance, float>) ((smi, dt) =>
          {
            if (smi.master.IsDoctored || (double) Time.time - (double) smi.lastCoughTime <= 20.0)
              return;
            smi.GoTo((StateMachine.BaseState) this.breathing.cough);
          }), UpdateRate.SIM_4000ms, false);
          this.breathing.cough.ToggleReactable((Func<SlimeSickness.SlimeLungComponent.StatesInstance, Reactable>) (smi => smi.GetReactable())).OnSignal(this.coughFinished, this.breathing.normal);
          this.notbreathing.TagTransition(new Tag[1]
          {
            GameTags.NoOxygen
          }, (GameStateMachine<SlimeSickness.SlimeLungComponent.States, SlimeSickness.SlimeLungComponent.StatesInstance, SicknessInstance, object>.State) this.breathing, true);
        }

        public class BreathingStates : GameStateMachine<SlimeSickness.SlimeLungComponent.States, SlimeSickness.SlimeLungComponent.StatesInstance, SicknessInstance, object>.State
        {
          public GameStateMachine<SlimeSickness.SlimeLungComponent.States, SlimeSickness.SlimeLungComponent.StatesInstance, SicknessInstance, object>.State normal;
          public GameStateMachine<SlimeSickness.SlimeLungComponent.States, SlimeSickness.SlimeLungComponent.StatesInstance, SicknessInstance, object>.State cough;
        }
      }
    }
  }
}
