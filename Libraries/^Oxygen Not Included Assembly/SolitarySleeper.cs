// Decompiled with JetBrains decompiler
// Type: SolitarySleeper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

[SkipSaveFileSerialization]
public class SolitarySleeper : StateMachineComponent<SolitarySleeper.StatesInstance>
{
  protected override void OnSpawn()
  {
    this.smi.StartSM();
  }

  protected bool IsUncomfortable()
  {
    if (!this.gameObject.GetSMI<StaminaMonitor.Instance>().IsSleeping())
      return false;
    int num = 5;
    bool flag1 = true;
    bool flag2 = true;
    int cell = Grid.PosToCell(this.gameObject);
    for (int x = 1; x < num; ++x)
    {
      int index1 = Grid.OffsetCell(cell, x, 0);
      int index2 = Grid.OffsetCell(cell, -x, 0);
      if (Grid.Solid[index2])
        flag1 = false;
      if (Grid.Solid[index1])
        flag2 = false;
      foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.Items)
      {
        if (flag1 && Grid.PosToCell(minionIdentity.gameObject) == index2 || flag2 && Grid.PosToCell(minionIdentity.gameObject) == index1)
          return true;
      }
    }
    return false;
  }

  public class StatesInstance : GameStateMachine<SolitarySleeper.States, SolitarySleeper.StatesInstance, SolitarySleeper, object>.GameInstance
  {
    public StatesInstance(SolitarySleeper master)
      : base(master)
    {
    }
  }

  public class States : GameStateMachine<SolitarySleeper.States, SolitarySleeper.StatesInstance, SolitarySleeper>
  {
    public GameStateMachine<SolitarySleeper.States, SolitarySleeper.StatesInstance, SolitarySleeper, object>.State satisfied;
    public GameStateMachine<SolitarySleeper.States, SolitarySleeper.StatesInstance, SolitarySleeper, object>.State suffering;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.satisfied;
      this.root.EventTransition(GameHashes.Died, (GameStateMachine<SolitarySleeper.States, SolitarySleeper.StatesInstance, SolitarySleeper, object>.State) null, (StateMachine<SolitarySleeper.States, SolitarySleeper.StatesInstance, SolitarySleeper, object>.Transition.ConditionCallback) (smi => smi.gameObject.GetSMI<DeathMonitor.Instance>().IsDead())).EventTransition(GameHashes.NewDay, this.satisfied, (StateMachine<SolitarySleeper.States, SolitarySleeper.StatesInstance, SolitarySleeper, object>.Transition.ConditionCallback) null).Update("SolitarySleeperCheck", (System.Action<SolitarySleeper.StatesInstance, float>) ((smi, dt) =>
      {
        if (smi.master.IsUncomfortable())
        {
          if (smi.GetCurrentState() == this.suffering)
            return;
          smi.GoTo((StateMachine.BaseState) this.suffering);
        }
        else
        {
          if (smi.GetCurrentState() == this.satisfied)
            return;
          smi.GoTo((StateMachine.BaseState) this.satisfied);
        }
      }), UpdateRate.SIM_4000ms, false);
      this.suffering.AddEffect("PeopleTooCloseWhileSleeping").ToggleExpression(Db.Get().Expressions.Uncomfortable, (Func<SolitarySleeper.StatesInstance, bool>) null).Update("PeopleTooCloseSleepFail", (System.Action<SolitarySleeper.StatesInstance, float>) ((smi, dt) => smi.master.gameObject.Trigger(1338475637, (object) this)), UpdateRate.SIM_1000ms, false);
      this.satisfied.DoNothing();
    }
  }
}
