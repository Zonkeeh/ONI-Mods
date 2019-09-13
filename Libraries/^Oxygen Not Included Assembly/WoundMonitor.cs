// Decompiled with JetBrains decompiler
// Type: WoundMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class WoundMonitor : GameStateMachine<WoundMonitor, WoundMonitor.Instance>
{
  public GameStateMachine<WoundMonitor, WoundMonitor.Instance, IStateMachineTarget, object>.State healthy;
  public WoundMonitor.Wounded wounded;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.healthy;
    this.root.ToggleAnims("anim_hits_kanim", 0.0f).EventHandler(GameHashes.HealthChanged, (GameStateMachine<WoundMonitor, WoundMonitor.Instance, IStateMachineTarget, object>.GameEvent.Callback) ((smi, data) => smi.OnHealthChanged(data)));
    this.healthy.EventTransition(GameHashes.HealthChanged, (GameStateMachine<WoundMonitor, WoundMonitor.Instance, IStateMachineTarget, object>.State) this.wounded, (StateMachine<WoundMonitor, WoundMonitor.Instance, IStateMachineTarget, object>.Transition.ConditionCallback) (smi => smi.health.State != Health.HealthState.Perfect));
    this.wounded.ToggleUrge(Db.Get().Urges.Heal).Enter((StateMachine<WoundMonitor, WoundMonitor.Instance, IStateMachineTarget, object>.State.Callback) (smi =>
    {
      switch (smi.health.State)
      {
        case Health.HealthState.Scuffed:
          smi.GoTo((StateMachine.BaseState) this.wounded.light);
          break;
        case Health.HealthState.Injured:
          smi.GoTo((StateMachine.BaseState) this.wounded.medium);
          break;
        case Health.HealthState.Critical:
          smi.GoTo((StateMachine.BaseState) this.wounded.heavy);
          break;
      }
    })).EventHandler(GameHashes.HealthChanged, (StateMachine<WoundMonitor, WoundMonitor.Instance, IStateMachineTarget, object>.State.Callback) (smi => smi.GoToProperHeathState()));
    this.wounded.medium.ToggleAnims("anim_loco_wounded_kanim", 1f);
    this.wounded.heavy.ToggleAnims("anim_loco_wounded_kanim", 3f);
  }

  public class Wounded : GameStateMachine<WoundMonitor, WoundMonitor.Instance, IStateMachineTarget, object>.State
  {
    public GameStateMachine<WoundMonitor, WoundMonitor.Instance, IStateMachineTarget, object>.State light;
    public GameStateMachine<WoundMonitor, WoundMonitor.Instance, IStateMachineTarget, object>.State medium;
    public GameStateMachine<WoundMonitor, WoundMonitor.Instance, IStateMachineTarget, object>.State heavy;
  }

  public class Instance : GameStateMachine<WoundMonitor, WoundMonitor.Instance, IStateMachineTarget, object>.GameInstance
  {
    public Health health;
    private Worker worker;

    public Instance(IStateMachineTarget master)
      : base(master)
    {
      this.health = master.GetComponent<Health>();
      this.worker = master.GetComponent<Worker>();
    }

    public void OnHealthChanged(object data)
    {
      float num = (float) data;
      if ((double) this.health.hitPoints == 0.0 || (double) num >= 0.0)
        return;
      this.PlayHitAnimation();
    }

    private void PlayHitAnimation()
    {
      string str1 = (string) null;
      KBatchedAnimController kbatchedAnimController = this.smi.Get<KBatchedAnimController>();
      if (kbatchedAnimController.CurrentAnim != null)
        str1 = kbatchedAnimController.CurrentAnim.name;
      KAnim.PlayMode playMode = kbatchedAnimController.PlayMode;
      if (str1 != null && (str1.Contains("hit") || str1.Contains("2_0") || (str1.Contains("2_1") || str1.Contains("2_-1")) || (str1.Contains("2_-2") || str1.Contains("1_-1") || (str1.Contains("1_-2") || str1.Contains("1_1"))) || (str1.Contains("1_2") || str1.Contains("breathe_") || str1.Contains("death_"))))
        return;
      string str2 = "hit";
      AttackChore.StatesInstance smi = this.gameObject.GetSMI<AttackChore.StatesInstance>();
      if ((object) smi != null && smi.GetCurrentState() == smi.sm.attack)
        str2 = smi.master.GetHitAnim();
      if (this.worker.GetComponent<Navigator>().CurrentNavType == NavType.Ladder)
        str2 = "hit_ladder";
      else if (this.worker.GetComponent<Navigator>().CurrentNavType == NavType.Pole)
        str2 = "hit_pole";
      kbatchedAnimController.Play((HashedString) str2, KAnim.PlayMode.Once, 1f, 0.0f);
      if (str1 == null)
        return;
      kbatchedAnimController.Queue((HashedString) str1, playMode, 1f, 0.0f);
    }

    public void GoToProperHeathState()
    {
      switch (this.smi.health.State)
      {
        case Health.HealthState.Perfect:
          this.smi.GoTo((StateMachine.BaseState) this.sm.healthy);
          break;
        case Health.HealthState.Scuffed:
          this.smi.GoTo((StateMachine.BaseState) this.sm.wounded.light);
          break;
        case Health.HealthState.Injured:
          this.smi.GoTo((StateMachine.BaseState) this.sm.wounded.medium);
          break;
        case Health.HealthState.Critical:
          this.smi.GoTo((StateMachine.BaseState) this.sm.wounded.heavy);
          break;
      }
    }

    public bool ShouldExitInfirmary()
    {
      return this.health.State == Health.HealthState.Perfect;
    }
  }
}
