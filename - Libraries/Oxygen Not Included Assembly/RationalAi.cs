// Decompiled with JetBrains decompiler
// Type: RationalAi
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class RationalAi : GameStateMachine<RationalAi, RationalAi.Instance>
{
  public GameStateMachine<RationalAi, RationalAi.Instance, IStateMachineTarget, object>.State alive;
  public GameStateMachine<RationalAi, RationalAi.Instance, IStateMachineTarget, object>.State dead;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.alive;
    this.serializable = true;
    this.root.ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new DeathMonitor.Instance(smi.master, new DeathMonitor.Def())));
    this.alive.TagTransition(GameTags.Dead, this.dead, false).ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new ThoughtGraph.Instance(smi.master))).ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new StaminaMonitor.Instance(smi.master))).ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new StressMonitor.Instance(smi.master))).ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new EmoteMonitor.Instance(smi.master))).ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new SneezeMonitor.Instance(smi.master))).ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new DecorMonitor.Instance(smi.master))).ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new IncapacitationMonitor.Instance(smi.master))).ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new IdleMonitor.Instance(smi.master))).ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new RationMonitor.Instance(smi.master))).ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new CalorieMonitor.Instance(smi.master))).ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new DoctorMonitor.Instance(smi.master))).ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new SicknessMonitor.Instance(smi.master))).ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new GermExposureMonitor.Instance(smi.master))).ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new BreathMonitor.Instance(smi.master))).ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new RoomMonitor.Instance(smi.master))).ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new TemperatureMonitor.Instance(smi.master))).ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new ExternalTemperatureMonitor.Instance(smi.master))).ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new BladderMonitor.Instance(smi.master))).ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new SteppedInMonitor.Instance(smi.master))).ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new LightMonitor.Instance(smi.master))).ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new RedAlertMonitor.Instance(smi.master))).ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new CringeMonitor.Instance(smi.master))).ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new HygieneMonitor.Instance(smi.master))).ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new FallMonitor.Instance(smi.master))).ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new ThreatMonitor.Instance(smi.master, new ThreatMonitor.Def()))).ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new WoundMonitor.Instance(smi.master))).ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new TiredMonitor.Instance(smi.master))).ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new MoveToLocationMonitor.Instance(smi.master))).ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new ReactionMonitor.Instance(smi.master))).ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new SuitWearer.Instance(smi.master))).ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new TubeTraveller.Instance(smi.master))).ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new MingleMonitor.Instance(smi.master))).ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new MournMonitor.Instance(smi.master)));
    this.dead.ToggleStateMachine((Func<RationalAi.Instance, StateMachine.Instance>) (smi => (StateMachine.Instance) new FallWhenDeadMonitor.Instance(smi.master))).ToggleBrain("dead").Enter("RefreshUserMenu", (StateMachine<RationalAi, RationalAi.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.RefreshUserMenu())).Enter("DropStorage", (StateMachine<RationalAi, RationalAi.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.GetComponent<Storage>().DropAll(false, false, new Vector3(), true)));
  }

  public class Instance : GameStateMachine<RationalAi, RationalAi.Instance, IStateMachineTarget, object>.GameInstance
  {
    public Instance(IStateMachineTarget master)
      : base(master)
    {
      ChoreConsumer component = this.GetComponent<ChoreConsumer>();
      component.AddUrge(Db.Get().Urges.EmoteHighPriority);
      component.AddUrge(Db.Get().Urges.EmoteIdle);
    }

    public void RefreshUserMenu()
    {
      Game.Instance.userMenu.Refresh(this.master.gameObject);
    }
  }
}
