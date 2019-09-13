// Decompiled with JetBrains decompiler
// Type: FishOvercrowingManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

public class FishOvercrowingManager : KMonoBehaviour, ISim1000ms
{
  private List<FishOvercrowdingMonitor.Instance> fishes = new List<FishOvercrowdingMonitor.Instance>();
  private Dictionary<int, FishOvercrowingManager.CavityInfo> cavityIdToCavityInfo = new Dictionary<int, FishOvercrowingManager.CavityInfo>();
  private Dictionary<int, int> cellToFishCount = new Dictionary<int, int>();
  private int versionCounter = 1;
  public static FishOvercrowingManager Instance;
  private FishOvercrowingManager.Cell[] cells;

  public static void DestroyInstance()
  {
    FishOvercrowingManager.Instance = (FishOvercrowingManager) null;
  }

  protected override void OnPrefabInit()
  {
    FishOvercrowingManager.Instance = this;
    this.cells = new FishOvercrowingManager.Cell[Grid.CellCount];
  }

  public void Add(FishOvercrowdingMonitor.Instance fish)
  {
    this.fishes.Add(fish);
  }

  public void Remove(FishOvercrowdingMonitor.Instance fish)
  {
    this.fishes.Remove(fish);
  }

  public void Sim1000ms(float dt)
  {
    int num1 = this.versionCounter++;
    int num2 = 1;
    this.cavityIdToCavityInfo.Clear();
    this.cellToFishCount.Clear();
    ListPool<FishOvercrowingManager.FishInfo, FishOvercrowingManager>.PooledList pooledList1 = ListPool<FishOvercrowingManager.FishInfo, FishOvercrowingManager>.Allocate();
    foreach (FishOvercrowdingMonitor.Instance fish in this.fishes)
    {
      int cell = Grid.PosToCell((StateMachine.Instance) fish);
      if (Grid.IsValidCell(cell))
      {
        FishOvercrowingManager.FishInfo fishInfo = new FishOvercrowingManager.FishInfo()
        {
          cell = cell,
          fish = fish
        };
        pooledList1.Add(fishInfo);
        int num3 = 0;
        this.cellToFishCount.TryGetValue(cell, out num3);
        int num4 = num3 + 1;
        this.cellToFishCount[cell] = num4;
      }
    }
    foreach (FishOvercrowingManager.FishInfo fishInfo in (List<FishOvercrowingManager.FishInfo>) pooledList1)
    {
      ListPool<int, FishOvercrowingManager>.PooledList pooledList2 = ListPool<int, FishOvercrowingManager>.Allocate();
      pooledList2.Add(fishInfo.cell);
      int num3 = 0;
      int key = num2++;
      while (num3 < pooledList2.Count)
      {
        int index = pooledList2[num3++];
        if (Grid.IsValidCell(index))
        {
          FishOvercrowingManager.Cell cell = this.cells[index];
          if (cell.version != num1 && Grid.IsLiquid(index))
          {
            cell.cavityId = key;
            cell.version = num1;
            int num4 = 0;
            this.cellToFishCount.TryGetValue(index, out num4);
            FishOvercrowingManager.CavityInfo cavityInfo = new FishOvercrowingManager.CavityInfo();
            if (!this.cavityIdToCavityInfo.TryGetValue(key, out cavityInfo))
              cavityInfo = new FishOvercrowingManager.CavityInfo();
            cavityInfo.fishCount += num4;
            ++cavityInfo.cellCount;
            this.cavityIdToCavityInfo[key] = cavityInfo;
            pooledList2.Add(Grid.CellLeft(index));
            pooledList2.Add(Grid.CellRight(index));
            pooledList2.Add(Grid.CellAbove(index));
            pooledList2.Add(Grid.CellBelow(index));
            this.cells[index] = cell;
          }
        }
      }
      pooledList2.Recycle();
    }
    foreach (FishOvercrowingManager.FishInfo fishInfo in (List<FishOvercrowingManager.FishInfo>) pooledList1)
    {
      FishOvercrowingManager.Cell cell = this.cells[fishInfo.cell];
      FishOvercrowingManager.CavityInfo cavityInfo = new FishOvercrowingManager.CavityInfo();
      this.cavityIdToCavityInfo.TryGetValue(cell.cavityId, out cavityInfo);
      fishInfo.fish.SetOvercrowdingInfo(cavityInfo.cellCount, cavityInfo.fishCount);
    }
    pooledList1.Recycle();
  }

  private struct Cell
  {
    public int version;
    public int cavityId;
  }

  private struct FishInfo
  {
    public int cell;
    public FishOvercrowdingMonitor.Instance fish;
  }

  private struct CavityInfo
  {
    public int fishCount;
    public int cellCount;
  }
}
