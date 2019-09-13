// Decompiled with JetBrains decompiler
// Type: SafeCellQuery
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class SafeCellQuery : PathFinderQuery
{
  private MinionBrain brain;
  private int targetCell;
  private int targetCost;
  public SafeCellQuery.SafeFlags targetCellFlags;
  private bool avoid_light;

  public SafeCellQuery Reset(MinionBrain brain, bool avoid_light)
  {
    this.brain = brain;
    this.targetCell = PathFinder.InvalidCell;
    this.targetCost = int.MaxValue;
    this.targetCellFlags = (SafeCellQuery.SafeFlags) 0;
    this.avoid_light = avoid_light;
    return this;
  }

  public static SafeCellQuery.SafeFlags GetFlags(
    int cell,
    MinionBrain brain,
    bool avoid_light = false)
  {
    int cell1 = Grid.CellAbove(cell);
    if (!Grid.IsValidCell(cell1) || (Grid.Solid[cell] || Grid.Solid[cell1]) || (Grid.IsTileUnderConstruction[cell] || Grid.IsTileUnderConstruction[cell1]))
      return (SafeCellQuery.SafeFlags) 0;
    bool flag1 = brain.IsCellClear(cell);
    bool flag2 = !Grid.Element[cell].IsLiquid;
    bool flag3 = !Grid.Element[cell1].IsLiquid;
    bool flag4 = (double) Grid.Temperature[cell] > 285.149993896484 && (double) Grid.Temperature[cell] < 303.149993896484;
    bool flag5 = brain.OxygenBreather.IsBreathableElementAtCell(cell, Grid.DefaultOffset);
    bool flag6 = !brain.Navigator.NavGrid.NavTable.IsValid(cell, NavType.Ladder) && !brain.Navigator.NavGrid.NavTable.IsValid(cell, NavType.Pole);
    bool flag7 = !brain.Navigator.NavGrid.NavTable.IsValid(cell, NavType.Tube);
    bool flag8 = !avoid_light || SleepChore.IsLightLevelOk(cell);
    if (cell == Grid.PosToCell((KMonoBehaviour) brain))
      flag5 = !brain.OxygenBreather.IsSuffocating;
    SafeCellQuery.SafeFlags safeFlags = (SafeCellQuery.SafeFlags) 0;
    if (flag1)
      safeFlags |= SafeCellQuery.SafeFlags.IsClear;
    if (flag4)
      safeFlags |= SafeCellQuery.SafeFlags.CorrectTemperature;
    if (flag5)
      safeFlags |= SafeCellQuery.SafeFlags.IsBreathable;
    if (flag6)
      safeFlags |= SafeCellQuery.SafeFlags.IsNotLadder;
    if (flag7)
      safeFlags |= SafeCellQuery.SafeFlags.IsNotTube;
    if (flag2)
      safeFlags |= SafeCellQuery.SafeFlags.IsNotLiquid;
    if (flag3)
      safeFlags |= SafeCellQuery.SafeFlags.IsNotLiquidOnMyFace;
    if (flag8)
      safeFlags |= SafeCellQuery.SafeFlags.IsLightOk;
    return safeFlags;
  }

  public override bool IsMatch(int cell, int parent_cell, int cost)
  {
    SafeCellQuery.SafeFlags flags = SafeCellQuery.GetFlags(cell, this.brain, this.avoid_light);
    bool flag1 = flags > this.targetCellFlags;
    bool flag2 = flags == this.targetCellFlags && cost < this.targetCost;
    if (flag1 || flag2)
    {
      this.targetCellFlags = flags;
      this.targetCost = cost;
      this.targetCell = cell;
    }
    return false;
  }

  public override int GetResultCell()
  {
    return this.targetCell;
  }

  public enum SafeFlags
  {
    IsClear = 1,
    IsLightOk = 2,
    IsNotLadder = 4,
    IsNotTube = 8,
    CorrectTemperature = 16, // 0x00000010
    IsBreathable = 32, // 0x00000020
    IsNotLiquidOnMyFace = 64, // 0x00000040
    IsNotLiquid = 128, // 0x00000080
  }
}
