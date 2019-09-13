// Decompiled with JetBrains decompiler
// Type: Claustrophobic
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

[SkipSaveFileSerialization]
public class Claustrophobic : StateMachineComponent<Claustrophobic.StatesInstance>
{
  protected override void OnSpawn()
  {
    this.smi.StartSM();
  }

  protected bool IsUncomfortable()
  {
    int num = 4;
    int cell1 = Grid.PosToCell(this.gameObject);
    for (int y = 0; y < num - 1; ++y)
    {
      int cell2 = Grid.OffsetCell(cell1, 0, y);
      if (Grid.IsValidCell(cell2) && Grid.Solid[cell2] || Grid.IsValidCell(Grid.CellRight(cell1)) && Grid.IsValidCell(Grid.CellLeft(cell1)) && (Grid.Solid[Grid.CellRight(cell1)] && Grid.Solid[Grid.CellLeft(cell1)]))
        return true;
    }
    return false;
  }

  public class StatesInstance : GameStateMachine<Claustrophobic.States, Claustrophobic.StatesInstance, Claustrophobic, object>.GameInstance
  {
    public StatesInstance(Claustrophobic master)
      : base(master)
    {
    }
  }

  public class States : GameStateMachine<Claustrophobic.States, Claustrophobic.StatesInstance, Claustrophobic>
  {
    public GameStateMachine<Claustrophobic.States, Claustrophobic.StatesInstance, Claustrophobic, object>.State satisfied;
    public GameStateMachine<Claustrophobic.States, Claustrophobic.StatesInstance, Claustrophobic, object>.State suffering;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.satisfied;
      this.root.Update("ClaustrophobicCheck", (System.Action<Claustrophobic.StatesInstance, float>) ((smi, dt) =>
      {
        if (smi.master.IsUncomfortable())
          smi.GoTo((StateMachine.BaseState) this.suffering);
        else
          smi.GoTo((StateMachine.BaseState) this.satisfied);
      }), UpdateRate.SIM_1000ms, false);
      this.suffering.AddEffect(nameof (Claustrophobic)).ToggleExpression(Db.Get().Expressions.Uncomfortable, (Func<Claustrophobic.StatesInstance, bool>) null);
      this.satisfied.DoNothing();
    }
  }
}
