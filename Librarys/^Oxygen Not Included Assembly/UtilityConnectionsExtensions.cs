// Decompiled with JetBrains decompiler
// Type: UtilityConnectionsExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

public static class UtilityConnectionsExtensions
{
  public static UtilityConnections InverseDirection(
    this UtilityConnections direction)
  {
    switch (direction)
    {
      case UtilityConnections.Left:
        return UtilityConnections.Right;
      case UtilityConnections.Right:
        return UtilityConnections.Left;
      case UtilityConnections.Up:
        return UtilityConnections.Down;
      case UtilityConnections.Down:
        return UtilityConnections.Up;
      default:
        throw new ArgumentException("Unexpected enum value: " + (object) direction, nameof (direction));
    }
  }

  public static UtilityConnections LeftDirection(this UtilityConnections direction)
  {
    switch (direction)
    {
      case UtilityConnections.Left:
        return UtilityConnections.Down;
      case UtilityConnections.Right:
        return UtilityConnections.Up;
      case UtilityConnections.Up:
        return UtilityConnections.Left;
      case UtilityConnections.Down:
        return UtilityConnections.Right;
      default:
        throw new ArgumentException("Unexpected enum value: " + (object) direction, nameof (direction));
    }
  }

  public static UtilityConnections RightDirection(
    this UtilityConnections direction)
  {
    switch (direction)
    {
      case UtilityConnections.Left:
        return UtilityConnections.Up;
      case UtilityConnections.Right:
        return UtilityConnections.Down;
      case UtilityConnections.Up:
        return UtilityConnections.Right;
      case UtilityConnections.Down:
        return UtilityConnections.Left;
      default:
        throw new ArgumentException("Unexpected enum value: " + (object) direction, nameof (direction));
    }
  }

  public static int CellInDirection(this UtilityConnections direction, int from_cell)
  {
    switch (direction)
    {
      case UtilityConnections.Left:
        return from_cell - 1;
      case UtilityConnections.Right:
        return from_cell + 1;
      case UtilityConnections.Up:
        return from_cell + Grid.WidthInCells;
      case UtilityConnections.Down:
        return from_cell - Grid.WidthInCells;
      default:
        throw new ArgumentException("Unexpected enum value: " + (object) direction, nameof (direction));
    }
  }

  public static UtilityConnections DirectionFromToCell(int from_cell, int to_cell)
  {
    if (to_cell == from_cell - 1)
      return UtilityConnections.Left;
    if (to_cell == from_cell + 1)
      return UtilityConnections.Right;
    if (to_cell == from_cell + Grid.WidthInCells)
      return UtilityConnections.Up;
    return to_cell == from_cell - Grid.WidthInCells ? UtilityConnections.Down : (UtilityConnections) 0;
  }
}
