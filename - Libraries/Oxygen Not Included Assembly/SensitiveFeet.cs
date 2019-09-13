// Decompiled with JetBrains decompiler
// Type: SensitiveFeet
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

[SkipSaveFileSerialization]
public class SensitiveFeet : StateMachineComponent<SensitiveFeet.StatesInstance>
{
  protected override void OnSpawn()
  {
    this.smi.StartSM();
  }

  protected bool IsUncomfortable()
  {
    int cell = Grid.CellBelow(Grid.PosToCell(this.gameObject));
    return Grid.IsValidCell(cell) && Grid.Solid[cell] && (UnityEngine.Object) Grid.Objects[cell, 9] == (UnityEngine.Object) null;
  }

  public class StatesInstance : GameStateMachine<SensitiveFeet.States, SensitiveFeet.StatesInstance, SensitiveFeet, object>.GameInstance
  {
    public StatesInstance(SensitiveFeet master)
      : base(master)
    {
    }
  }

  public class States : GameStateMachine<SensitiveFeet.States, SensitiveFeet.StatesInstance, SensitiveFeet>
  {
    public GameStateMachine<SensitiveFeet.States, SensitiveFeet.StatesInstance, SensitiveFeet, object>.State satisfied;
    public GameStateMachine<SensitiveFeet.States, SensitiveFeet.StatesInstance, SensitiveFeet, object>.State suffering;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.satisfied;
      this.root.Update("SensitiveFeetCheck", (System.Action<SensitiveFeet.StatesInstance, float>) ((smi, dt) =>
      {
        if (smi.master.IsUncomfortable())
          smi.GoTo((StateMachine.BaseState) this.suffering);
        else
          smi.GoTo((StateMachine.BaseState) this.satisfied);
      }), UpdateRate.SIM_1000ms, false);
      this.suffering.AddEffect("UncomfortableFeet").ToggleExpression(Db.Get().Expressions.Uncomfortable, (Func<SensitiveFeet.StatesInstance, bool>) null);
      this.satisfied.DoNothing();
    }
  }
}
