// Decompiled with JetBrains decompiler
// Type: EntombedItemVisualizer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class EntombedItemVisualizer : KMonoBehaviour
{
  private static readonly string[] EntombedVisualizerAnims = new string[4]
  {
    "idle1",
    "idle2",
    "idle3",
    "idle4"
  };
  private Dictionary<int, EntombedItemVisualizer.Data> cellEntombedCounts = new Dictionary<int, EntombedItemVisualizer.Data>();
  [SerializeField]
  private GameObject entombedItemPrefab;
  private ObjectPool entombedItemPool;

  public void Clear()
  {
    this.cellEntombedCounts.Clear();
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.entombedItemPool = new ObjectPool(new Func<GameObject>(this.InstantiateEntombedObject), 32);
  }

  public bool AddItem(int cell)
  {
    bool flag = false;
    if ((UnityEngine.Object) Grid.Objects[cell, 9] == (UnityEngine.Object) null)
    {
      flag = true;
      EntombedItemVisualizer.Data data;
      this.cellEntombedCounts.TryGetValue(cell, out data);
      if (data.refCount == 0)
      {
        GameObject instance = this.entombedItemPool.GetInstance();
        instance.transform.SetPosition(Grid.CellToPosCCC(cell, Grid.SceneLayer.FXFront));
        instance.transform.rotation = Quaternion.Euler(0.0f, 0.0f, UnityEngine.Random.value * 360f);
        KBatchedAnimController component = instance.GetComponent<KBatchedAnimController>();
        int index = UnityEngine.Random.Range(0, EntombedItemVisualizer.EntombedVisualizerAnims.Length);
        string entombedVisualizerAnim = EntombedItemVisualizer.EntombedVisualizerAnims[index];
        component.initialAnim = entombedVisualizerAnim;
        instance.SetActive(true);
        component.Play((HashedString) entombedVisualizerAnim, KAnim.PlayMode.Once, 1f, 0.0f);
        data.controller = component;
      }
      ++data.refCount;
      this.cellEntombedCounts[cell] = data;
    }
    return flag;
  }

  public void RemoveItem(int cell)
  {
    EntombedItemVisualizer.Data data;
    if (!this.cellEntombedCounts.TryGetValue(cell, out data))
      return;
    --data.refCount;
    if (data.refCount == 0)
      this.ReleaseVisualizer(cell, data);
    else
      this.cellEntombedCounts[cell] = data;
  }

  public void ForceClear(int cell)
  {
    EntombedItemVisualizer.Data data;
    if (!this.cellEntombedCounts.TryGetValue(cell, out data))
      return;
    this.ReleaseVisualizer(cell, data);
  }

  private void ReleaseVisualizer(int cell, EntombedItemVisualizer.Data data)
  {
    if ((UnityEngine.Object) data.controller != (UnityEngine.Object) null)
    {
      data.controller.gameObject.SetActive(false);
      this.entombedItemPool.ReleaseInstance(data.controller.gameObject);
    }
    this.cellEntombedCounts.Remove(cell);
  }

  public bool IsEntombedItem(int cell)
  {
    if (this.cellEntombedCounts.ContainsKey(cell))
      return this.cellEntombedCounts[cell].refCount > 0;
    return false;
  }

  private GameObject InstantiateEntombedObject()
  {
    GameObject gameObject = GameUtil.KInstantiate(this.entombedItemPrefab, Grid.SceneLayer.FXFront, (string) null, 0);
    gameObject.SetActive(false);
    return gameObject;
  }

  private struct Data
  {
    public int refCount;
    public KBatchedAnimController controller;
  }
}
