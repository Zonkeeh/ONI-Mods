// Decompiled with JetBrains decompiler
// Type: CavityInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class CavityInfo
{
  public List<KPrefabID> buildings = new List<KPrefabID>();
  public List<KPrefabID> plants = new List<KPrefabID>();
  public List<KPrefabID> creatures = new List<KPrefabID>();
  public List<KPrefabID> eggs = new List<KPrefabID>();
  public HandleVector<int>.Handle handle;
  public bool dirty;
  public int numCells;
  public int maxX;
  public int maxY;
  public int minX;
  public int minY;
  public Room room;

  public CavityInfo()
  {
    this.handle = HandleVector<int>.InvalidHandle;
    this.dirty = true;
  }

  public void AddBuilding(KPrefabID bc)
  {
    this.buildings.Add(bc);
    this.dirty = true;
  }

  public void AddPlants(KPrefabID plant)
  {
    this.plants.Add(plant);
    this.dirty = true;
  }

  public void OnEnter(object data)
  {
    foreach (KPrefabID building in this.buildings)
    {
      if ((Object) building != (Object) null)
        building.Trigger(-832141045, data);
    }
  }
}
