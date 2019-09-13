// Decompiled with JetBrains decompiler
// Type: Workaholic
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

[SkipSaveFileSerialization]
public class Workaholic : StateMachineComponent<Workaholic.StatesInstance>
{
  protected override void OnSpawn()
  {
    this.smi.StartSM();
  }

  protected bool IsUncomfortable()
  {
    return this.smi.master.GetComponent<ChoreDriver>().GetCurrentChore() is IdleChore;
  }

  public class StatesInstance : GameStateMachine<Workaholic.States, Workaholic.StatesInstance, Workaholic, object>.GameInstance
  {
    public StatesInstance(Workaholic master)
      : base(master)
    {
    }
  }

  public class States : GameStateMachine<Workaholic.States, Workaholic.StatesInstance, Workaholic>
  {
    public GameStateMachine<Workaholic.States, Workaholic.StatesInstance, Workaholic, object>.State satisfied;
    public GameStateMachine<Workaholic.States, Workaholic.StatesInstance, Workaholic, object>.State suffering;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.satisfied;
      this.root.Update("WorkaholicCheck", (System.Action<Workaholic.StatesInstance, float>) ((smi, dt) =>
      {
        if (smi.master.IsUncomfortable())
          smi.GoTo((StateMachine.BaseState) this.suffering);
        else
          smi.GoTo((StateMachine.BaseState) this.satisfied);
      }), UpdateRate.SIM_1000ms, false);
      this.suffering.AddEffect("Restless").ToggleExpression(Db.Get().Expressions.Uncomfortable, (Func<Workaholic.StatesInstance, bool>) null);
      this.satisfied.DoNothing();
    }
  }
}
