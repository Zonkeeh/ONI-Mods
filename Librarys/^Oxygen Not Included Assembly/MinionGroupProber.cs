// Decompiled with JetBrains decompiler
// Type: MinionGroupProber
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

public class MinionGroupProber : KMonoBehaviour, IGroupProber
{
  private Dictionary<object, KeyValuePair<int, int>> valid_serial_nos = new Dictionary<object, KeyValuePair<int, int>>();
  private List<object> pending_removals = new List<object>();
  private readonly object access = new object();
  private static MinionGroupProber Instance;
  private Dictionary<object, int>[] cells;

  public static void DestroyInstance()
  {
    MinionGroupProber.Instance = (MinionGroupProber) null;
  }

  public static MinionGroupProber Get()
  {
    return MinionGroupProber.Instance;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    MinionGroupProber.Instance = this;
    this.cells = new Dictionary<object, int>[Grid.CellCount];
  }

  private bool IsReachable_AssumeLock(int cell)
  {
    Dictionary<object, int> cell1 = this.cells[cell];
    if (cell1 == null)
      return false;
    bool flag = false;
    foreach (KeyValuePair<object, int> keyValuePair1 in cell1)
    {
      object key = keyValuePair1.Key;
      int num = keyValuePair1.Value;
      KeyValuePair<int, int> keyValuePair2;
      if (this.valid_serial_nos.TryGetValue(key, out keyValuePair2) && (num == keyValuePair2.Key || num == keyValuePair2.Value))
      {
        flag = true;
        break;
      }
      this.pending_removals.Add(key);
    }
    foreach (object pendingRemoval in this.pending_removals)
    {
      cell1.Remove(pendingRemoval);
      if (cell1.Count == 0)
        this.cells[cell] = (Dictionary<object, int>) null;
    }
    this.pending_removals.Clear();
    return flag;
  }

  public bool IsReachable(int cell)
  {
    if (!Grid.IsValidCell(cell))
      return false;
    lock (this.access)
      return this.IsReachable_AssumeLock(cell);
  }

  public bool IsReachable(int cell, CellOffset[] offsets)
  {
    if (!Grid.IsValidCell(cell))
      return false;
    bool flag = false;
    lock (this.access)
    {
      foreach (CellOffset offset in offsets)
      {
        if (this.IsReachable_AssumeLock(Grid.OffsetCell(cell, offset)))
        {
          flag = true;
          break;
        }
      }
    }
    return flag;
  }

  public bool IsAllReachable(int cell, CellOffset[] offsets)
  {
    if (!Grid.IsValidCell(cell))
      return false;
    bool flag = false;
    lock (this.access)
    {
      if (this.IsReachable_AssumeLock(cell))
      {
        flag = true;
      }
      else
      {
        foreach (CellOffset offset in offsets)
        {
          if (this.IsReachable_AssumeLock(Grid.OffsetCell(cell, offset)))
          {
            flag = true;
            break;
          }
        }
      }
    }
    return flag;
  }

  public bool IsReachable(Workable workable)
  {
    return this.IsReachable(Grid.PosToCell((KMonoBehaviour) workable), workable.GetOffsets());
  }

  public void Occupy(object prober, int serial_no, IEnumerable<int> cells)
  {
    lock (this.access)
    {
      foreach (int cell in cells)
      {
        if (this.cells[cell] == null)
          this.cells[cell] = new Dictionary<object, int>();
        this.cells[cell][prober] = serial_no;
      }
    }
  }

  public void SetValidSerialNos(object prober, int previous_serial_no, int serial_no)
  {
    lock (this.access)
      this.valid_serial_nos[prober] = new KeyValuePair<int, int>(previous_serial_no, serial_no);
  }

  public bool ReleaseProber(object prober)
  {
    lock (this.access)
      return this.valid_serial_nos.Remove(prober);
  }
}
