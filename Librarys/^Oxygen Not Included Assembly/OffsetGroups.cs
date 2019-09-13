// Decompiled with JetBrains decompiler
// Type: OffsetGroups
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public static class OffsetGroups
{
  public static CellOffset[] Use = new CellOffset[1]
  {
    new CellOffset()
  };
  public static CellOffset[] Chat = new CellOffset[6]
  {
    new CellOffset(1, 0),
    new CellOffset(-1, 0),
    new CellOffset(1, 1),
    new CellOffset(1, -1),
    new CellOffset(-1, 1),
    new CellOffset(-1, -1)
  };
  public static CellOffset[] LeftOnly = new CellOffset[1]
  {
    new CellOffset(-1, 0)
  };
  public static CellOffset[] RightOnly = new CellOffset[1]
  {
    new CellOffset(1, 0)
  };
  public static CellOffset[] Standard = OffsetGroups.InitGrid(-2, 2, -3, 3);
  public static CellOffset[] LiquidSource = new CellOffset[11]
  {
    new CellOffset(0, 0),
    new CellOffset(1, 0),
    new CellOffset(-1, 0),
    new CellOffset(0, 1),
    new CellOffset(0, -1),
    new CellOffset(1, 1),
    new CellOffset(1, -1),
    new CellOffset(-1, 1),
    new CellOffset(-1, -1),
    new CellOffset(2, 0),
    new CellOffset(-2, 0)
  };
  public static CellOffset[][] InvertedStandardTable = OffsetTable.Mirror(new CellOffset[28][]
  {
    new CellOffset[1]{ new CellOffset(0, 0) },
    new CellOffset[1]{ new CellOffset(0, 1) },
    new CellOffset[2]
    {
      new CellOffset(0, 2),
      new CellOffset(0, 1)
    },
    new CellOffset[3]
    {
      new CellOffset(0, 3),
      new CellOffset(0, 1),
      new CellOffset(0, 2)
    },
    new CellOffset[1]{ new CellOffset(0, -1) },
    new CellOffset[1]{ new CellOffset(0, -2) },
    new CellOffset[3]
    {
      new CellOffset(0, -3),
      new CellOffset(0, -2),
      new CellOffset(0, -1)
    },
    new CellOffset[1]{ new CellOffset(1, 0) },
    new CellOffset[2]
    {
      new CellOffset(1, 1),
      new CellOffset(0, 1)
    },
    new CellOffset[2]
    {
      new CellOffset(1, 1),
      new CellOffset(1, 0)
    },
    new CellOffset[3]
    {
      new CellOffset(1, 2),
      new CellOffset(1, 0),
      new CellOffset(1, 1)
    },
    new CellOffset[3]
    {
      new CellOffset(1, 2),
      new CellOffset(0, 1),
      new CellOffset(0, 2)
    },
    new CellOffset[3]
    {
      new CellOffset(1, 3),
      new CellOffset(1, 2),
      new CellOffset(1, 1)
    },
    new CellOffset[4]
    {
      new CellOffset(1, 3),
      new CellOffset(0, 1),
      new CellOffset(0, 2),
      new CellOffset(0, 3)
    },
    new CellOffset[1]{ new CellOffset(1, -1) },
    new CellOffset[3]
    {
      new CellOffset(1, -2),
      new CellOffset(1, 0),
      new CellOffset(1, -1)
    },
    new CellOffset[3]
    {
      new CellOffset(1, -2),
      new CellOffset(1, -1),
      new CellOffset(0, -1)
    },
    new CellOffset[3]
    {
      new CellOffset(1, -3),
      new CellOffset(1, 0),
      new CellOffset(1, -1)
    },
    new CellOffset[3]
    {
      new CellOffset(1, -3),
      new CellOffset(0, -1),
      new CellOffset(0, -2)
    },
    new CellOffset[3]
    {
      new CellOffset(1, -3),
      new CellOffset(0, -1),
      new CellOffset(-1, -1)
    },
    new CellOffset[2]
    {
      new CellOffset(2, 0),
      new CellOffset(1, 0)
    },
    new CellOffset[3]
    {
      new CellOffset(2, 1),
      new CellOffset(1, 1),
      new CellOffset(0, 1)
    },
    new CellOffset[3]
    {
      new CellOffset(2, 1),
      new CellOffset(1, 1),
      new CellOffset(1, 0)
    },
    new CellOffset[3]
    {
      new CellOffset(2, 2),
      new CellOffset(1, 2),
      new CellOffset(1, 1)
    },
    new CellOffset[4]
    {
      new CellOffset(2, 3),
      new CellOffset(1, 1),
      new CellOffset(1, 2),
      new CellOffset(1, 3)
    },
    new CellOffset[3]
    {
      new CellOffset(2, -1),
      new CellOffset(2, 0),
      new CellOffset(1, 0)
    },
    new CellOffset[4]
    {
      new CellOffset(2, -2),
      new CellOffset(1, 0),
      new CellOffset(1, -1),
      new CellOffset(2, -1)
    },
    new CellOffset[4]
    {
      new CellOffset(2, -3),
      new CellOffset(1, 0),
      new CellOffset(1, -1),
      new CellOffset(1, -2)
    }
  });
  public static CellOffset[][] InvertedStandardTableWithCorners = OffsetTable.Mirror(new CellOffset[26][]
  {
    new CellOffset[1]{ new CellOffset(0, 0) },
    new CellOffset[1]{ new CellOffset(0, 1) },
    new CellOffset[2]
    {
      new CellOffset(0, 2),
      new CellOffset(0, 1)
    },
    new CellOffset[3]
    {
      new CellOffset(0, 3),
      new CellOffset(0, 1),
      new CellOffset(0, 2)
    },
    new CellOffset[1]{ new CellOffset(0, -1) },
    new CellOffset[1]{ new CellOffset(0, -2) },
    new CellOffset[3]
    {
      new CellOffset(0, -3),
      new CellOffset(0, -2),
      new CellOffset(0, -1)
    },
    new CellOffset[1]{ new CellOffset(1, 0) },
    new CellOffset[1]{ new CellOffset(1, 1) },
    new CellOffset[2]
    {
      new CellOffset(1, 1),
      new CellOffset(1, 0)
    },
    new CellOffset[3]
    {
      new CellOffset(1, 2),
      new CellOffset(1, 0),
      new CellOffset(1, 1)
    },
    new CellOffset[3]
    {
      new CellOffset(1, 2),
      new CellOffset(0, 1),
      new CellOffset(0, 2)
    },
    new CellOffset[3]
    {
      new CellOffset(1, 3),
      new CellOffset(1, 2),
      new CellOffset(1, 1)
    },
    new CellOffset[4]
    {
      new CellOffset(1, 3),
      new CellOffset(0, 1),
      new CellOffset(0, 2),
      new CellOffset(0, 3)
    },
    new CellOffset[1]{ new CellOffset(1, -1) },
    new CellOffset[3]
    {
      new CellOffset(1, -2),
      new CellOffset(1, 0),
      new CellOffset(1, -1)
    },
    new CellOffset[2]
    {
      new CellOffset(1, -2),
      new CellOffset(1, -1)
    },
    new CellOffset[4]
    {
      new CellOffset(1, -3),
      new CellOffset(1, 0),
      new CellOffset(1, -1),
      new CellOffset(1, -2)
    },
    new CellOffset[3]
    {
      new CellOffset(1, -3),
      new CellOffset(1, -2),
      new CellOffset(1, -1)
    },
    new CellOffset[2]
    {
      new CellOffset(2, 0),
      new CellOffset(1, 0)
    },
    new CellOffset[2]
    {
      new CellOffset(2, 1),
      new CellOffset(1, 1)
    },
    new CellOffset[3]
    {
      new CellOffset(2, 2),
      new CellOffset(1, 2),
      new CellOffset(1, 1)
    },
    new CellOffset[4]
    {
      new CellOffset(2, 3),
      new CellOffset(1, 1),
      new CellOffset(1, 2),
      new CellOffset(1, 3)
    },
    new CellOffset[3]
    {
      new CellOffset(2, -1),
      new CellOffset(2, 0),
      new CellOffset(1, 0)
    },
    new CellOffset[4]
    {
      new CellOffset(2, -2),
      new CellOffset(1, 0),
      new CellOffset(1, -1),
      new CellOffset(2, -1)
    },
    new CellOffset[4]
    {
      new CellOffset(2, -3),
      new CellOffset(1, 0),
      new CellOffset(1, -1),
      new CellOffset(1, -2)
    }
  });
  private static Dictionary<CellOffset[], Dictionary<CellOffset[][], Dictionary<CellOffset[], CellOffset[][]>>> reachabilityTableCache = new Dictionary<CellOffset[], Dictionary<CellOffset[][], Dictionary<CellOffset[], CellOffset[][]>>>();
  private static readonly CellOffset[] nullFilter = new CellOffset[0];

  public static CellOffset[] InitGrid(int x0, int x1, int y0, int y1)
  {
    List<CellOffset> cellOffsetList = new List<CellOffset>();
    for (int y = y0; y <= y1; ++y)
    {
      for (int x = x0; x <= x1; ++x)
        cellOffsetList.Add(new CellOffset(x, y));
    }
    CellOffset[] array = cellOffsetList.ToArray();
    Array.Sort<CellOffset>(array, 0, array.Length, (IComparer<CellOffset>) new OffsetGroups.CellOffsetComparer());
    return array;
  }

  public static CellOffset[][] BuildReachabilityTable(
    CellOffset[] area_offsets,
    CellOffset[][] table,
    CellOffset[] filter)
  {
    Dictionary<CellOffset[][], Dictionary<CellOffset[], CellOffset[][]>> dictionary1 = (Dictionary<CellOffset[][], Dictionary<CellOffset[], CellOffset[][]>>) null;
    Dictionary<CellOffset[], CellOffset[][]> dictionary2 = (Dictionary<CellOffset[], CellOffset[][]>) null;
    CellOffset[][] cellOffsetArray1 = (CellOffset[][]) null;
    if (OffsetGroups.reachabilityTableCache.TryGetValue(area_offsets, out dictionary1) && dictionary1.TryGetValue(table, out dictionary2) && dictionary2.TryGetValue(filter != null ? filter : OffsetGroups.nullFilter, out cellOffsetArray1))
      return cellOffsetArray1;
    HashSet<CellOffset> cellOffsetSet = new HashSet<CellOffset>();
    foreach (CellOffset areaOffset in area_offsets)
    {
      foreach (CellOffset[] cellOffsetArray2 in table)
      {
        if (filter == null || Array.IndexOf<CellOffset>(filter, cellOffsetArray2[0]) == -1)
        {
          CellOffset cellOffset = areaOffset + cellOffsetArray2[0];
          cellOffsetSet.Add(cellOffset);
        }
      }
    }
    List<CellOffset[]> cellOffsetArrayList = new List<CellOffset[]>();
    foreach (CellOffset cellOffset1 in cellOffsetSet)
    {
      CellOffset cellOffset2 = area_offsets[0];
      foreach (CellOffset areaOffset in area_offsets)
      {
        if ((cellOffset1 - cellOffset2).GetOffsetDistance() > (cellOffset1 - areaOffset).GetOffsetDistance())
          cellOffset2 = areaOffset;
      }
      foreach (CellOffset[] cellOffsetArray2 in table)
      {
        if ((filter == null || Array.IndexOf<CellOffset>(filter, cellOffsetArray2[0]) == -1) && cellOffsetArray2[0] + cellOffset2 == cellOffset1)
        {
          CellOffset[] cellOffsetArray3 = new CellOffset[cellOffsetArray2.Length];
          for (int index = 0; index < cellOffsetArray2.Length; ++index)
            cellOffsetArray3[index] = cellOffsetArray2[index] + cellOffset2;
          cellOffsetArrayList.Add(cellOffsetArray3);
        }
      }
    }
    CellOffset[][] array = cellOffsetArrayList.ToArray();
    Array.Sort<CellOffset[]>(array, (Comparison<CellOffset[]>) ((x, y) => x[0].GetOffsetDistance().CompareTo(y[0].GetOffsetDistance())));
    if (dictionary1 == null)
    {
      dictionary1 = new Dictionary<CellOffset[][], Dictionary<CellOffset[], CellOffset[][]>>();
      OffsetGroups.reachabilityTableCache.Add(area_offsets, dictionary1);
    }
    if (dictionary2 == null)
    {
      dictionary2 = new Dictionary<CellOffset[], CellOffset[][]>();
      dictionary1.Add(table, dictionary2);
    }
    dictionary2.Add(filter != null ? filter : OffsetGroups.nullFilter, array);
    return array;
  }

  private class CellOffsetComparer : IComparer<CellOffset>
  {
    public int Compare(CellOffset a, CellOffset b)
    {
      return (Math.Abs(a.x) + Math.Abs(a.y)).CompareTo(Math.Abs(b.x) + Math.Abs(b.y));
    }
  }
}
