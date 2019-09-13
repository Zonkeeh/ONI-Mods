// Decompiled with JetBrains decompiler
// Type: SafetyConditions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

public class SafetyConditions
{
  public SafetyChecker.Condition IsNotLiquid;
  public SafetyChecker.Condition IsNotLadder;
  public SafetyChecker.Condition IsCorrectTemperature;
  public SafetyChecker.Condition IsWarming;
  public SafetyChecker.Condition IsCooling;
  public SafetyChecker.Condition HasSomeOxygen;
  public SafetyChecker.Condition IsClear;
  public SafetyChecker.Condition IsNotFoundation;
  public SafetyChecker.Condition IsNotDoor;
  public SafetyChecker.Condition IsNotLedge;
  public SafetyChecker.Condition IsNearby;
  public SafetyChecker WarmUpChecker;
  public SafetyChecker CoolDownChecker;
  public SafetyChecker RecoverBreathChecker;
  public SafetyChecker VomitCellChecker;
  public SafetyChecker SafeCellChecker;
  public SafetyChecker IdleCellChecker;

  public SafetyConditions()
  {
    int num1 = 1;
    int num2;
    this.IsNearby = new SafetyChecker.Condition(nameof (IsNearby), num2 = num1 * 2, (SafetyChecker.Condition.Callback) ((cell, cost, context) => cost > 5));
    int num3;
    this.IsNotLedge = new SafetyChecker.Condition(nameof (IsNotLedge), num3 = num2 * 2, (SafetyChecker.Condition.Callback) ((cell, cost, context) =>
    {
      int index1 = Grid.CellBelow(Grid.CellLeft(cell));
      if (Grid.Solid[index1])
        return false;
      int index2 = Grid.CellBelow(Grid.CellRight(cell));
      return Grid.Solid[index2];
    }));
    int num4;
    this.IsNotLiquid = new SafetyChecker.Condition(nameof (IsNotLiquid), num4 = num3 * 2, (SafetyChecker.Condition.Callback) ((cell, cost, context) => !Grid.Element[cell].IsLiquid));
    int num5;
    this.IsNotLadder = new SafetyChecker.Condition(nameof (IsNotLadder), num5 = num4 * 2, (SafetyChecker.Condition.Callback) ((cell, cost, context) =>
    {
      if (!context.navigator.NavGrid.NavTable.IsValid(cell, NavType.Ladder))
        return !context.navigator.NavGrid.NavTable.IsValid(cell, NavType.Pole);
      return false;
    }));
    int num6;
    this.IsNotDoor = new SafetyChecker.Condition(nameof (IsNotDoor), num6 = num5 * 2, (SafetyChecker.Condition.Callback) ((cell, cost, context) =>
    {
      int cell1 = Grid.CellAbove(cell);
      if (!Grid.HasDoor[cell] && Grid.IsValidCell(cell1))
        return !Grid.HasDoor[cell1];
      return false;
    }));
    int num7;
    this.IsCorrectTemperature = new SafetyChecker.Condition(nameof (IsCorrectTemperature), num7 = num6 * 2, (SafetyChecker.Condition.Callback) ((cell, cost, context) =>
    {
      if ((double) Grid.Temperature[cell] > 285.149993896484)
        return (double) Grid.Temperature[cell] < 303.149993896484;
      return false;
    }));
    int num8;
    this.IsWarming = new SafetyChecker.Condition(nameof (IsWarming), num8 = num7 * 2, (SafetyChecker.Condition.Callback) ((cell, cost, context) => true));
    int num9;
    this.IsCooling = new SafetyChecker.Condition(nameof (IsCooling), num9 = num8 * 2, (SafetyChecker.Condition.Callback) ((cell, cost, context) => false));
    int num10;
    this.HasSomeOxygen = new SafetyChecker.Condition(nameof (HasSomeOxygen), num10 = num9 * 2, (SafetyChecker.Condition.Callback) ((cell, cost, context) => context.oxygenBreather.IsBreathableElementAtCell(cell, (CellOffset[]) null)));
    int num11;
    this.IsClear = new SafetyChecker.Condition(nameof (IsClear), num11 = num10 * 2, (SafetyChecker.Condition.Callback) ((cell, cost, context) => context.minionBrain.IsCellClear(cell)));
    this.WarmUpChecker = new SafetyChecker(new List<SafetyChecker.Condition>()
    {
      this.IsWarming
    }.ToArray());
    this.CoolDownChecker = new SafetyChecker(new List<SafetyChecker.Condition>()
    {
      this.IsCooling
    }.ToArray());
    List<SafetyChecker.Condition> conditionList1 = new List<SafetyChecker.Condition>();
    conditionList1.Add(this.HasSomeOxygen);
    conditionList1.Add(this.IsNotDoor);
    this.RecoverBreathChecker = new SafetyChecker(conditionList1.ToArray());
    List<SafetyChecker.Condition> conditionList2 = new List<SafetyChecker.Condition>((IEnumerable<SafetyChecker.Condition>) conditionList1);
    conditionList2.Add(this.IsNotLiquid);
    conditionList2.Add(this.IsCorrectTemperature);
    this.SafeCellChecker = new SafetyChecker(conditionList2.ToArray());
    this.IdleCellChecker = new SafetyChecker(new List<SafetyChecker.Condition>((IEnumerable<SafetyChecker.Condition>) conditionList2)
    {
      this.IsClear,
      this.IsNotLadder
    }.ToArray());
    this.VomitCellChecker = new SafetyChecker(new List<SafetyChecker.Condition>()
    {
      this.IsNotLiquid,
      this.IsNotLedge,
      this.IsNearby
    }.ToArray());
  }
}
