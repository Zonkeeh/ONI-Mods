// Decompiled with JetBrains decompiler
// Type: Klei.AI.PeriodicEmoteSickness
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

namespace Klei.AI
{
  public class PeriodicEmoteSickness : Sickness.SicknessComponent
  {
    private HashedString[] anims;
    private float cooldown;

    public PeriodicEmoteSickness(HashedString kanim, HashedString[] anims, float cooldown)
    {
      this.anims = anims;
      this.cooldown = cooldown;
    }

    public override object OnInfect(GameObject go, SicknessInstance diseaseInstance)
    {
      PeriodicEmoteSickness.StatesInstance statesInstance = new PeriodicEmoteSickness.StatesInstance(diseaseInstance, this);
      statesInstance.StartSM();
      return (object) statesInstance;
    }

    public override void OnCure(GameObject go, object instance_data)
    {
      ((StateMachine.Instance) instance_data).StopSM("Cured");
    }

    public class StatesInstance : GameStateMachine<PeriodicEmoteSickness.States, PeriodicEmoteSickness.StatesInstance, SicknessInstance, object>.GameInstance
    {
      public PeriodicEmoteSickness periodicEmoteSickness;

      public StatesInstance(SicknessInstance master, PeriodicEmoteSickness periodicEmoteSickness)
        : base(master)
      {
        this.periodicEmoteSickness = periodicEmoteSickness;
      }

      public Reactable GetReactable()
      {
        SelfEmoteReactable selfEmoteReactable = new SelfEmoteReactable(this.master.gameObject, (HashedString) nameof (PeriodicEmoteSickness), Db.Get().ChoreTypes.Emote, (HashedString) "anim_sneeze_kanim", 0.0f, this.periodicEmoteSickness.cooldown, float.PositiveInfinity);
        foreach (HashedString anim in this.periodicEmoteSickness.anims)
          selfEmoteReactable.AddStep(new EmoteReactable.EmoteStep()
          {
            anim = anim
          });
        return (Reactable) selfEmoteReactable;
      }
    }

    public class States : GameStateMachine<PeriodicEmoteSickness.States, PeriodicEmoteSickness.StatesInstance, SicknessInstance>
    {
      public override void InitializeStates(out StateMachine.BaseState default_state)
      {
        default_state = (StateMachine.BaseState) this.root;
        this.root.ToggleReactable((Func<PeriodicEmoteSickness.StatesInstance, Reactable>) (smi => smi.GetReactable()));
      }
    }
  }
}
