// Decompiled with JetBrains decompiler
// Type: CreatureHelpers
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public static class CreatureHelpers
{
  public static bool isClear(int cell)
  {
    return Grid.IsValidCell(cell) && !Grid.Solid[cell] && !Grid.IsSubstantialLiquid(cell, 0.9f) && (!Grid.IsValidCell(Grid.CellBelow(cell)) || !Grid.IsLiquid(cell) || !Grid.IsLiquid(Grid.CellBelow(cell)));
  }

  public static int FindNearbyBreathableCell(int currentLocation, SimHashes breathableElement)
  {
    return currentLocation;
  }

  public static bool cellsAreClear(int[] cells)
  {
    for (int index = 0; index < cells.Length; ++index)
    {
      if (!Grid.IsValidCell(cells[index]) || !CreatureHelpers.isClear(cells[index]))
        return false;
    }
    return true;
  }

  public static Vector3 PositionOfCurrentCell(Vector3 transformPosition)
  {
    return Grid.CellToPos(Grid.PosToCell(transformPosition));
  }

  public static Vector3 CenterPositionOfCell(int cell)
  {
    return Grid.CellToPos(cell) + new Vector3(0.5f, 0.5f, -2f);
  }

  public static void DeselectCreature(GameObject creature)
  {
    KSelectable component = creature.GetComponent<KSelectable>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || !((UnityEngine.Object) SelectTool.Instance.selected == (UnityEngine.Object) component))
      return;
    SelectTool.Instance.Select((KSelectable) null, false);
  }

  public static bool isSwimmable(int cell)
  {
    return Grid.IsValidCell(cell) && !Grid.Solid[cell] && Grid.IsSubstantialLiquid(cell, 0.35f);
  }

  public static bool isSolidGround(int cell)
  {
    return Grid.IsValidCell(cell) && Grid.Solid[cell];
  }

  public static void FlipAnim(KAnimControllerBase anim, Vector3 heading)
  {
    if ((double) heading.x < 0.0)
    {
      anim.FlipX = true;
    }
    else
    {
      if ((double) heading.x <= 0.0)
        return;
      anim.FlipX = false;
    }
  }

  public static void FlipAnim(KBatchedAnimController anim, Vector3 heading)
  {
    if ((double) heading.x < 0.0)
    {
      anim.FlipX = true;
    }
    else
    {
      if ((double) heading.x <= 0.0)
        return;
      anim.FlipX = false;
    }
  }

  public static Vector3 GetWalkMoveTarget(Transform transform, Vector2 Heading)
  {
    int cell = Grid.PosToCell(transform.GetPosition());
    if ((double) Heading.x == 1.0)
    {
      if (CreatureHelpers.isClear(Grid.CellRight(cell)) && CreatureHelpers.isClear(Grid.CellDownRight(cell)) && (CreatureHelpers.isClear(Grid.CellRight(Grid.CellRight(cell))) && !CreatureHelpers.isClear(Grid.PosToCell(transform.GetPosition() + Vector3.right * 2f + Vector3.down))))
        return transform.GetPosition() + Vector3.right * 2f;
      if (CreatureHelpers.cellsAreClear(new int[2]
      {
        Grid.CellRight(cell),
        Grid.CellDownRight(cell)
      }) && !CreatureHelpers.isClear(Grid.CellBelow(Grid.CellDownRight(cell))))
        return transform.GetPosition() + Vector3.right + Vector3.down;
      if (CreatureHelpers.cellsAreClear(new int[3]
      {
        Grid.OffsetCell(cell, 1, 0),
        Grid.OffsetCell(cell, 1, -1),
        Grid.OffsetCell(cell, 1, -2)
      }) && !CreatureHelpers.isClear(Grid.OffsetCell(cell, 1, -3)))
        return transform.GetPosition() + Vector3.right + Vector3.down + Vector3.down;
      if (CreatureHelpers.cellsAreClear(new int[4]
      {
        Grid.OffsetCell(cell, 1, 0),
        Grid.OffsetCell(cell, 1, -1),
        Grid.OffsetCell(cell, 1, -2),
        Grid.OffsetCell(cell, 1, -3)
      }))
        return transform.GetPosition();
      if (CreatureHelpers.isClear(Grid.CellRight(cell)))
        return transform.GetPosition() + Vector3.right;
      if (CreatureHelpers.isClear(Grid.CellUpRight(cell)) && !Grid.Solid[Grid.CellAbove(cell)] && Grid.Solid[Grid.CellRight(cell)])
        return transform.GetPosition() + Vector3.up + Vector3.right;
      if (!Grid.Solid[Grid.CellAbove(cell)] && !Grid.Solid[Grid.CellAbove(Grid.CellAbove(cell))] && (Grid.Solid[Grid.CellAbove(Grid.CellRight(cell))] && CreatureHelpers.isClear(Grid.CellRight(Grid.CellAbove(Grid.CellAbove(cell))))))
        return transform.GetPosition() + Vector3.up + Vector3.up + Vector3.right;
    }
    if ((double) Heading.x == -1.0)
    {
      if (CreatureHelpers.isClear(Grid.CellLeft(cell)) && CreatureHelpers.isClear(Grid.CellDownLeft(cell)) && (CreatureHelpers.isClear(Grid.CellLeft(Grid.CellLeft(cell))) && !CreatureHelpers.isClear(Grid.PosToCell(transform.GetPosition() + Vector3.left * 2f + Vector3.down))))
        return transform.GetPosition() + Vector3.left * 2f;
      if (CreatureHelpers.cellsAreClear(new int[2]
      {
        Grid.CellLeft(cell),
        Grid.CellDownLeft(cell)
      }) && !CreatureHelpers.isClear(Grid.CellBelow(Grid.CellDownLeft(cell))))
        return transform.GetPosition() + Vector3.left + Vector3.down;
      if (CreatureHelpers.cellsAreClear(new int[3]
      {
        Grid.OffsetCell(cell, -1, 0),
        Grid.OffsetCell(cell, -1, -1),
        Grid.OffsetCell(cell, -1, -2)
      }) && !CreatureHelpers.isClear(Grid.OffsetCell(cell, -1, -3)))
        return transform.GetPosition() + Vector3.left + Vector3.down + Vector3.down;
      if (CreatureHelpers.cellsAreClear(new int[4]
      {
        Grid.OffsetCell(cell, -1, 0),
        Grid.OffsetCell(cell, -1, -1),
        Grid.OffsetCell(cell, -1, -2),
        Grid.OffsetCell(cell, -1, -3)
      }))
        return transform.GetPosition();
      if (CreatureHelpers.isClear(Grid.CellLeft(Grid.PosToCell(transform.GetPosition()))))
        return transform.GetPosition() + Vector3.left;
      if (CreatureHelpers.isClear(Grid.CellUpLeft(cell)) && !Grid.Solid[Grid.CellAbove(cell)] && Grid.Solid[Grid.CellLeft(cell)])
        return transform.GetPosition() + Vector3.up + Vector3.left;
      if (!Grid.Solid[Grid.CellAbove(cell)] && !Grid.Solid[Grid.CellAbove(Grid.CellAbove(cell))] && (Grid.Solid[Grid.CellAbove(Grid.CellLeft(cell))] && CreatureHelpers.isClear(Grid.CellLeft(Grid.CellAbove(Grid.CellAbove(cell))))))
        return transform.GetPosition() + Vector3.up + Vector3.up + Vector3.left;
    }
    return transform.GetPosition();
  }

  public static bool CrewNearby(Transform transform, int range = 6)
  {
    int cell = Grid.PosToCell(transform.gameObject);
    for (int x = 1; x < range; ++x)
    {
      int index1 = Grid.OffsetCell(cell, x, 0);
      int index2 = Grid.OffsetCell(cell, -x, 0);
      if ((UnityEngine.Object) Grid.Objects[index1, 0] != (UnityEngine.Object) null || (UnityEngine.Object) Grid.Objects[index2, 0] != (UnityEngine.Object) null)
        return true;
    }
    return false;
  }

  public static bool CheckHorizontalClear(Vector3 startPosition, Vector3 endPosition)
  {
    int cell = Grid.PosToCell(startPosition);
    int num1 = 1;
    if ((double) endPosition.x < (double) startPosition.x)
      num1 = -1;
    float num2 = Mathf.Abs(endPosition.x - startPosition.x);
    for (int index1 = 0; (double) index1 < (double) num2; ++index1)
    {
      int index2 = Grid.OffsetCell(cell, index1 * num1, 0);
      if (Grid.Solid[index2])
        return false;
    }
    return true;
  }

  public static GameObject GetFleeTargetLocatorObject(GameObject self, GameObject threat)
  {
    if ((UnityEngine.Object) threat == (UnityEngine.Object) null)
    {
      Debug.LogWarning((object) (self.name + " is trying to flee, bus has no threats"));
      return (GameObject) null;
    }
    int cell1 = Grid.PosToCell(threat);
    int cell2 = Grid.PosToCell(self);
    Navigator nav = self.GetComponent<Navigator>();
    if ((UnityEngine.Object) nav == (UnityEngine.Object) null)
    {
      Debug.LogWarning((object) (self.name + " is trying to flee, bus has no navigator component attached."));
      return (GameObject) null;
    }
    HashSet<int> intSet = GameUtil.FloodCollectCells(Grid.PosToCell(self), (Func<int, bool>) (cell => CreatureHelpers.CanFleeTo(cell, nav)), 300, (HashSet<int>) null, true);
    int cell3 = -1;
    int num1 = -1;
    foreach (int num2 in intSet)
    {
      if (nav.CanReach(num2) && num2 != cell2)
      {
        int num3 = Grid.GetCellDistance(num2, cell1) - 1;
        if (CreatureHelpers.isInFavoredFleeDirection(num2, cell1, self))
          num3 += 2;
        if (num3 > num1)
        {
          num1 = num3;
          cell3 = num2;
        }
      }
    }
    if (cell3 != -1)
      return ChoreHelpers.CreateLocator("GoToLocator", Grid.CellToPos(cell3));
    return (GameObject) null;
  }

  private static bool isInFavoredFleeDirection(int targetFleeCell, int threatCell, GameObject self)
  {
    return (double) Grid.CellToPos(threatCell).x < (double) self.transform.GetPosition().x == (double) Grid.CellToPos(threatCell).x < (double) Grid.CellToPos(targetFleeCell).x;
  }

  private static bool CanFleeTo(int cell, Navigator nav)
  {
    if (!nav.CanReach(cell) && !nav.CanReach(Grid.OffsetCell(cell, -1, -1)) && (!nav.CanReach(Grid.OffsetCell(cell, 1, -1)) && !nav.CanReach(Grid.OffsetCell(cell, -1, 1))))
      return nav.CanReach(Grid.OffsetCell(cell, 1, 1));
    return true;
  }
}
