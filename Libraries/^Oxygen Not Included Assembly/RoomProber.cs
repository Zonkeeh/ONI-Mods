// Decompiled with JetBrains decompiler
// Type: RoomProber
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class RoomProber : ISim1000ms
{
  public List<Room> rooms = new List<Room>();
  private KCompactedVector<CavityInfo> cavityInfos = new KCompactedVector<CavityInfo>(1024);
  private bool dirty = true;
  private HashSet<int> solidChanges = new HashSet<int>();
  private HashSet<int> visitedCells = new HashSet<int>();
  private HashSet<int> floodFillSet = new HashSet<int>();
  private HashSet<HandleVector<int>.Handle> releasedIDs = new HashSet<HandleVector<int>.Handle>();
  private HandleVector<int>.Handle[] CellCavityID;
  private RoomProber.CavityFloodFiller floodFiller;

  public RoomProber()
  {
    this.CellCavityID = new HandleVector<int>.Handle[Grid.CellCount];
    this.floodFiller = new RoomProber.CavityFloodFiller(this.CellCavityID);
    for (int index = 0; index < this.CellCavityID.Length; ++index)
      this.solidChanges.Add(index);
    this.ProcessSolidChanges();
    this.RefreshRooms();
    World.Instance.OnSolidChanged += new System.Action<int>(this.SolidChangedEvent);
    GameScenePartitioner.Instance.AddGlobalLayerListener(GameScenePartitioner.Instance.objectLayers[1], new System.Action<int, object>(this.OnBuildingsChanged));
  }

  private void SolidChangedEvent(int cell)
  {
    this.SolidChangedEvent(cell, true);
  }

  private void OnBuildingsChanged(int cell, object building)
  {
    if (this.GetCavityForCell(cell) == null)
      return;
    this.solidChanges.Add(cell);
    this.dirty = true;
  }

  public void SolidChangedEvent(int cell, bool ignoreDoors)
  {
    if (ignoreDoors && Grid.HasDoor[cell])
      return;
    this.solidChanges.Add(cell);
    this.dirty = true;
  }

  private CavityInfo CreateNewCavity()
  {
    CavityInfo initial_data = new CavityInfo();
    initial_data.handle = this.cavityInfos.Allocate(initial_data);
    return initial_data;
  }

  private unsafe void ProcessSolidChanges()
  {
    // ISSUE: untyped stack allocation
    int* numPtr = (int*) __untypedstackalloc((int) checked (5U * 4U));
    *numPtr = 0;
    numPtr[1] = -Grid.WidthInCells;
    numPtr[2] = -1;
    numPtr[3] = 1;
    numPtr[4] = Grid.WidthInCells;
    foreach (int solidChange in this.solidChanges)
    {
      for (int index = 0; index < 5; ++index)
      {
        int cell = solidChange + numPtr[index];
        if (Grid.IsValidCell(cell))
        {
          this.floodFillSet.Add(cell);
          HandleVector<int>.Handle handle = this.CellCavityID[cell];
          if (handle.IsValid())
          {
            this.CellCavityID[cell] = HandleVector<int>.InvalidHandle;
            this.releasedIDs.Add(handle);
          }
        }
      }
    }
    CavityInfo newCavity = this.CreateNewCavity();
    foreach (int floodFill in this.floodFillSet)
    {
      if (!this.visitedCells.Contains(floodFill) && !this.CellCavityID[floodFill].IsValid())
      {
        CavityInfo cavityInfo = newCavity;
        this.floodFiller.Reset(cavityInfo.handle);
        GameUtil.FloodFillConditional(floodFill, new Func<int, bool>(this.floodFiller.ShouldContinue), (ICollection<int>) this.visitedCells, (ICollection<int>) null);
        if (this.floodFiller.NumCells > 0)
        {
          cavityInfo.numCells = this.floodFiller.NumCells;
          cavityInfo.minX = this.floodFiller.MinX;
          cavityInfo.minY = this.floodFiller.MinY;
          cavityInfo.maxX = this.floodFiller.MaxX;
          cavityInfo.maxY = this.floodFiller.MaxY;
          newCavity = this.CreateNewCavity();
        }
      }
    }
    if (newCavity.numCells == 0)
      this.releasedIDs.Add(newCavity.handle);
    foreach (HandleVector<int>.Handle releasedId in this.releasedIDs)
    {
      CavityInfo data = this.cavityInfos.GetData(releasedId);
      if (data.room != null)
        this.ClearRoom(data.room);
      this.cavityInfos.Free(releasedId);
    }
    this.RebuildDirtyCavities((ICollection<int>) this.visitedCells);
    this.releasedIDs.Clear();
    this.visitedCells.Clear();
    this.solidChanges.Clear();
    this.floodFillSet.Clear();
  }

  private void RebuildDirtyCavities(ICollection<int> visited_cells)
  {
    int maxRoomSize = TuningData<RoomProber.Tuning>.Get().maxRoomSize;
    foreach (int visitedCell in (IEnumerable<int>) visited_cells)
    {
      HandleVector<int>.Handle handle = this.CellCavityID[visitedCell];
      if (handle.IsValid())
      {
        CavityInfo data = this.cavityInfos.GetData(handle);
        if (0 < data.numCells && data.numCells <= maxRoomSize)
        {
          GameObject gameObject = Grid.Objects[visitedCell, 1];
          if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
          {
            KPrefabID component = gameObject.GetComponent<KPrefabID>();
            bool flag = false;
            foreach (KPrefabID building in data.buildings)
            {
              if (component.InstanceID == building.InstanceID)
              {
                flag = true;
                break;
              }
            }
            foreach (KPrefabID plant in data.plants)
            {
              if (component.InstanceID == plant.InstanceID)
              {
                flag = true;
                break;
              }
            }
            if (!flag)
            {
              if ((bool) ((UnityEngine.Object) component.GetComponent<Deconstructable>()))
                data.AddBuilding(component);
              else if (component.HasTag(GameTags.Plant) && !component.HasTag("ForestTreeBranch".ToTag()))
                data.AddPlants(component);
            }
          }
        }
      }
    }
    visited_cells.Clear();
  }

  public void Sim1000ms(float dt)
  {
    if (!this.dirty)
      return;
    this.ProcessSolidChanges();
    this.RefreshRooms();
  }

  private void CreateRoom(CavityInfo cavity)
  {
    Debug.Assert(cavity.room == null);
    Room room = new Room();
    room.cavity = cavity;
    cavity.room = room;
    this.rooms.Add(room);
    room.roomType = Db.Get().RoomTypes.GetRoomType(room);
    this.AssignBuildingsToRoom(room);
  }

  private void ClearRoom(Room room)
  {
    this.UnassignBuildingsToRoom(room);
    room.CleanUp();
    this.rooms.Remove(room);
  }

  private void RefreshRooms()
  {
    int maxRoomSize = TuningData<RoomProber.Tuning>.Get().maxRoomSize;
    foreach (CavityInfo data in this.cavityInfos.GetDataList())
    {
      if (data.dirty)
      {
        Debug.Assert(data.room == null, (object) "I expected info.room to always be null by this point");
        if (data.numCells > 0)
        {
          if (data.numCells <= maxRoomSize)
            this.CreateRoom(data);
          foreach (KMonoBehaviour building in data.buildings)
            building.Trigger(144050788, (object) data.room);
          foreach (KMonoBehaviour plant in data.plants)
            plant.Trigger(144050788, (object) data.room);
        }
        data.dirty = false;
      }
    }
    this.dirty = false;
  }

  private void AssignBuildingsToRoom(Room room)
  {
    Debug.Assert(room != null);
    RoomType roomType = room.roomType;
    if (roomType == Db.Get().RoomTypes.Neutral)
      return;
    foreach (KPrefabID building in room.buildings)
    {
      if (!((UnityEngine.Object) building == (UnityEngine.Object) null) && !building.HasTag(GameTags.NotRoomAssignable))
      {
        Assignable component = building.GetComponent<Assignable>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null && (roomType.primary_constraint == null || !roomType.primary_constraint.building_criteria(building.GetComponent<KPrefabID>())))
          component.Assign((IAssignableIdentity) room);
      }
    }
  }

  private void UnassignKPrefabIDs(Room room, List<KPrefabID> list)
  {
    foreach (KPrefabID kprefabId in list)
    {
      if (!((UnityEngine.Object) kprefabId == (UnityEngine.Object) null))
      {
        kprefabId.Trigger(144050788, (object) null);
        Assignable component = kprefabId.GetComponent<Assignable>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.assignee == room)
          component.Unassign();
      }
    }
  }

  private void UnassignBuildingsToRoom(Room room)
  {
    Debug.Assert(room != null);
    this.UnassignKPrefabIDs(room, room.buildings);
    this.UnassignKPrefabIDs(room, room.plants);
  }

  public void UpdateRoom(CavityInfo cavity)
  {
    if (cavity == null)
      return;
    if (cavity.room != null)
    {
      this.ClearRoom(cavity.room);
      cavity.room = (Room) null;
    }
    this.CreateRoom(cavity);
    foreach (KMonoBehaviour building in cavity.buildings)
      building.Trigger(144050788, (object) cavity.room);
    foreach (KMonoBehaviour plant in cavity.plants)
      plant.Trigger(144050788, (object) cavity.room);
  }

  public Room GetRoomOfGameObject(GameObject go)
  {
    if ((UnityEngine.Object) go == (UnityEngine.Object) null)
      return (Room) null;
    int cell = Grid.PosToCell(go);
    if (!Grid.IsValidCell(cell))
      return (Room) null;
    return this.GetCavityForCell(cell)?.room;
  }

  public bool IsInRoomType(GameObject go, RoomType checkType)
  {
    Room roomOfGameObject = this.GetRoomOfGameObject(go);
    if (roomOfGameObject == null)
      return false;
    RoomType roomType = roomOfGameObject.roomType;
    return checkType == roomType;
  }

  private CavityInfo GetCavityInfo(HandleVector<int>.Handle id)
  {
    CavityInfo cavityInfo = (CavityInfo) null;
    if (id.IsValid())
      cavityInfo = this.cavityInfos.GetData(id);
    return cavityInfo;
  }

  public CavityInfo GetCavityForCell(int cell)
  {
    if (!Grid.IsValidCell(cell))
      return (CavityInfo) null;
    return this.GetCavityInfo(this.CellCavityID[cell]);
  }

  public class Tuning : TuningData<RoomProber.Tuning>
  {
    public int maxRoomSize;
  }

  private class CavityFloodFiller
  {
    private HandleVector<int>.Handle[] grid;
    private HandleVector<int>.Handle cavityID;
    private int numCells;
    private int minX;
    private int minY;
    private int maxX;
    private int maxY;

    public CavityFloodFiller(HandleVector<int>.Handle[] grid)
    {
      this.grid = grid;
    }

    public void Reset(HandleVector<int>.Handle search_id)
    {
      this.cavityID = search_id;
      this.numCells = 0;
      this.minX = int.MaxValue;
      this.minY = int.MaxValue;
      this.maxX = 0;
      this.maxY = 0;
    }

    private static bool IsWall(int cell)
    {
      if ((Grid.BuildMasks[cell] & (Grid.BuildFlags.Solid | Grid.BuildFlags.Foundation)) == (Grid.BuildFlags) 0)
        return Grid.HasDoor[cell];
      return true;
    }

    public bool ShouldContinue(int flood_cell)
    {
      bool flag = false;
      if (!RoomProber.CavityFloodFiller.IsWall(flood_cell))
      {
        flag = true;
        this.grid[flood_cell] = this.cavityID;
        int x = 0;
        int y = 0;
        Grid.CellToXY(flood_cell, out x, out y);
        this.minX = Math.Min(x, this.minX);
        this.minY = Math.Min(y, this.minY);
        this.maxX = Math.Max(x, this.maxX);
        this.maxY = Math.Max(y, this.maxY);
        ++this.numCells;
      }
      else
        this.grid[flood_cell] = HandleVector<int>.InvalidHandle;
      return flag;
    }

    public int NumCells
    {
      get
      {
        return this.numCells;
      }
    }

    public int MinX
    {
      get
      {
        return this.minX;
      }
    }

    public int MinY
    {
      get
      {
        return this.minY;
      }
    }

    public int MaxX
    {
      get
      {
        return this.maxX;
      }
    }

    public int MaxY
    {
      get
      {
        return this.maxY;
      }
    }
  }
}
