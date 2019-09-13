// Decompiled with JetBrains decompiler
// Type: EntombedItemManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class EntombedItemManager : KMonoBehaviour, ISim33ms
{
  [Serialize]
  private List<int> cells = new List<int>();
  [Serialize]
  private List<int> elementIds = new List<int>();
  [Serialize]
  private List<float> masses = new List<float>();
  [Serialize]
  private List<float> temperatures = new List<float>();
  [Serialize]
  private List<byte> diseaseIndices = new List<byte>();
  [Serialize]
  private List<int> diseaseCounts = new List<int>();
  private List<Pickupable> pickupables = new List<Pickupable>();

  [OnDeserialized]
  private void OnDeserialized()
  {
    this.SpawnUncoveredObjects();
    this.AddMassToWorlIfPossible();
    this.PopulateEntombedItemVisualizers();
  }

  public static bool CanEntomb(Pickupable pickupable)
  {
    if ((Object) pickupable == (Object) null || (Object) pickupable.storage != (Object) null)
      return false;
    int cell = Grid.PosToCell((KMonoBehaviour) pickupable);
    return Grid.IsValidCell(cell) && Grid.Solid[cell] && (!((Object) Grid.Objects[cell, 9] != (Object) null) && pickupable.GetComponent<PrimaryElement>().Element.IsSolid) && (Object) pickupable.GetComponent<ElementChunk>() != (Object) null;
  }

  public void Add(Pickupable pickupable)
  {
    this.pickupables.Add(pickupable);
  }

  public void Sim33ms(float dt)
  {
    EntombedItemVisualizer component1 = Game.Instance.GetComponent<EntombedItemVisualizer>();
    HashSetPool<Pickupable, EntombedItemManager>.PooledHashSet pooledHashSet = HashSetPool<Pickupable, EntombedItemManager>.Allocate();
    foreach (Pickupable pickupable in this.pickupables)
    {
      if (EntombedItemManager.CanEntomb(pickupable))
        pooledHashSet.Add(pickupable);
    }
    this.pickupables.Clear();
    foreach (Pickupable pickupable in (HashSet<Pickupable>) pooledHashSet)
    {
      int cell = Grid.PosToCell((KMonoBehaviour) pickupable);
      PrimaryElement component2 = pickupable.GetComponent<PrimaryElement>();
      SimHashes elementId = component2.ElementID;
      float mass = component2.Mass;
      float temperature = component2.Temperature;
      byte diseaseIdx = component2.DiseaseIdx;
      int diseaseCount = component2.DiseaseCount;
      Element element = Grid.Element[cell];
      if (elementId == element.id && (double) mass > 0.0100000007078052 && (double) Grid.Mass[cell] + (double) mass < (double) element.maxMass)
      {
        SimMessages.AddRemoveSubstance(cell, (int) ElementLoader.FindElementByHash(elementId).idx, CellEventLogger.Instance.ElementConsumerSimUpdate, mass, temperature, diseaseIdx, diseaseCount, true, -1);
      }
      else
      {
        component1.AddItem(cell);
        this.cells.Add(cell);
        this.elementIds.Add((int) elementId);
        this.masses.Add(mass);
        this.temperatures.Add(temperature);
        this.diseaseIndices.Add(diseaseIdx);
        this.diseaseCounts.Add(diseaseCount);
      }
      Util.KDestroyGameObject(pickupable.gameObject);
    }
    pooledHashSet.Recycle();
  }

  public void OnSolidChanged(List<int> solid_changed_cells)
  {
    ListPool<int, EntombedItemManager>.PooledList pooledList1 = ListPool<int, EntombedItemManager>.Allocate();
    foreach (int solidChangedCell in solid_changed_cells)
    {
      if (!Grid.Solid[solidChangedCell])
        pooledList1.Add(solidChangedCell);
    }
    ListPool<int, EntombedItemManager>.PooledList pooledList2 = ListPool<int, EntombedItemManager>.Allocate();
    for (int index = 0; index < this.cells.Count; ++index)
    {
      int cell = this.cells[index];
      foreach (int num in (List<int>) pooledList1)
      {
        if (cell == num)
        {
          pooledList2.Add(index);
          break;
        }
      }
    }
    pooledList1.Recycle();
    this.SpawnObjects((List<int>) pooledList2);
    pooledList2.Recycle();
  }

  private void SpawnUncoveredObjects()
  {
    ListPool<int, EntombedItemManager>.PooledList pooledList = ListPool<int, EntombedItemManager>.Allocate();
    for (int index = 0; index < this.cells.Count; ++index)
    {
      int cell = this.cells[index];
      if (!Grid.Solid[cell])
        pooledList.Add(index);
    }
    this.SpawnObjects((List<int>) pooledList);
    pooledList.Recycle();
  }

  private void AddMassToWorlIfPossible()
  {
    ListPool<int, EntombedItemManager>.PooledList pooledList = ListPool<int, EntombedItemManager>.Allocate();
    for (int index = 0; index < this.cells.Count; ++index)
    {
      int cell = this.cells[index];
      if (Grid.Solid[cell] && Grid.Element[cell].id == (SimHashes) this.elementIds[index])
        pooledList.Add(index);
    }
    pooledList.Sort();
    pooledList.Reverse();
    foreach (int item_idx in (List<int>) pooledList)
    {
      EntombedItemManager.Item obj = this.GetItem(item_idx);
      this.RemoveItem(item_idx);
      if ((double) obj.mass > 1.40129846432482E-45)
        SimMessages.AddRemoveSubstance(obj.cell, (int) ElementLoader.FindElementByHash((SimHashes) obj.elementId).idx, CellEventLogger.Instance.ElementConsumerSimUpdate, obj.mass, obj.temperature, obj.diseaseIdx, obj.diseaseCount, true, -1);
    }
    pooledList.Recycle();
  }

  private void RemoveItem(int item_idx)
  {
    this.cells.RemoveAt(item_idx);
    this.elementIds.RemoveAt(item_idx);
    this.masses.RemoveAt(item_idx);
    this.temperatures.RemoveAt(item_idx);
    this.diseaseIndices.RemoveAt(item_idx);
    this.diseaseCounts.RemoveAt(item_idx);
  }

  private EntombedItemManager.Item GetItem(int item_idx)
  {
    return new EntombedItemManager.Item()
    {
      cell = this.cells[item_idx],
      elementId = this.elementIds[item_idx],
      mass = this.masses[item_idx],
      temperature = this.temperatures[item_idx],
      diseaseIdx = this.diseaseIndices[item_idx],
      diseaseCount = this.diseaseCounts[item_idx]
    };
  }

  private void SpawnObjects(List<int> uncovered_item_indices)
  {
    uncovered_item_indices.Sort();
    uncovered_item_indices.Reverse();
    EntombedItemVisualizer component = Game.Instance.GetComponent<EntombedItemVisualizer>();
    foreach (int uncoveredItemIndex in uncovered_item_indices)
    {
      EntombedItemManager.Item obj = this.GetItem(uncoveredItemIndex);
      component.RemoveItem(obj.cell);
      this.RemoveItem(uncoveredItemIndex);
      ElementLoader.FindElementByHash((SimHashes) obj.elementId)?.substance.SpawnResource(Grid.CellToPosCCC(obj.cell, Grid.SceneLayer.Ore), obj.mass, obj.temperature, obj.diseaseIdx, obj.diseaseCount, false, false, false);
    }
  }

  private void PopulateEntombedItemVisualizers()
  {
    EntombedItemVisualizer component = Game.Instance.GetComponent<EntombedItemVisualizer>();
    foreach (int cell in this.cells)
      component.AddItem(cell);
  }

  private struct Item
  {
    public int cell;
    public int elementId;
    public float mass;
    public float temperature;
    public byte diseaseIdx;
    public int diseaseCount;
  }
}
