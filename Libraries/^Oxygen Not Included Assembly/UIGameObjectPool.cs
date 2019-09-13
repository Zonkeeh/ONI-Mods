// Decompiled with JetBrains decompiler
// Type: UIGameObjectPool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class UIGameObjectPool
{
  private List<GameObject> freeElements = new List<GameObject>();
  private List<GameObject> activeElements = new List<GameObject>();
  private GameObject prefab;
  public Transform disabledElementParent;

  public UIGameObjectPool(GameObject prefab)
  {
    this.prefab = prefab;
    this.freeElements = new List<GameObject>();
    this.activeElements = new List<GameObject>();
  }

  public int ActiveElementsCount
  {
    get
    {
      return this.activeElements.Count;
    }
  }

  public int FreeElementsCount
  {
    get
    {
      return this.freeElements.Count;
    }
  }

  public int TotalElementsCount
  {
    get
    {
      return this.ActiveElementsCount + this.FreeElementsCount;
    }
  }

  public GameObject GetFreeElement(GameObject instantiateParent = null, bool forceActive = false)
  {
    if (this.freeElements.Count == 0)
    {
      this.activeElements.Add(Util.KInstantiateUI(this.prefab.gameObject, instantiateParent, false));
    }
    else
    {
      GameObject freeElement = this.freeElements[0];
      this.activeElements.Add(freeElement);
      if ((UnityEngine.Object) freeElement.transform.parent != (UnityEngine.Object) instantiateParent)
        freeElement.transform.SetParent(instantiateParent.transform);
      this.freeElements.RemoveAt(0);
    }
    GameObject activeElement = this.activeElements[this.activeElements.Count - 1];
    if (activeElement.gameObject.activeInHierarchy != forceActive)
      activeElement.gameObject.SetActive(forceActive);
    return activeElement;
  }

  public void ClearElement(GameObject element)
  {
    if (!this.activeElements.Contains(element))
    {
      string str = !this.freeElements.Contains(element) ? element.name + ": The element provided does not belong to this pool" : element.name + ": The element provided is already inactive";
      element.SetActive(false);
      if ((UnityEngine.Object) this.disabledElementParent != (UnityEngine.Object) null)
        element.transform.SetParent(this.disabledElementParent);
      Debug.LogError((object) str);
    }
    else
    {
      if ((UnityEngine.Object) this.disabledElementParent != (UnityEngine.Object) null)
        element.transform.SetParent(this.disabledElementParent);
      element.SetActive(false);
      this.freeElements.Add(element);
      this.activeElements.Remove(element);
    }
  }

  public void ClearAll()
  {
    while (this.activeElements.Count > 0)
    {
      if ((UnityEngine.Object) this.disabledElementParent != (UnityEngine.Object) null)
        this.activeElements[0].transform.SetParent(this.disabledElementParent);
      this.activeElements[0].SetActive(false);
      this.freeElements.Add(this.activeElements[0]);
      this.activeElements.RemoveAt(0);
    }
  }

  public void DestroyAll()
  {
    this.DestroyAllActive();
    this.DestroyAllFree();
  }

  public void DestroyAllActive()
  {
    this.activeElements.ForEach((System.Action<GameObject>) (ae => UnityEngine.Object.Destroy((UnityEngine.Object) ae)));
    this.activeElements.Clear();
  }

  public void DestroyAllFree()
  {
    this.freeElements.ForEach((System.Action<GameObject>) (ae => UnityEngine.Object.Destroy((UnityEngine.Object) ae)));
    this.freeElements.Clear();
  }

  public void ForEachActiveElement(System.Action<GameObject> predicate)
  {
    this.activeElements.ForEach(predicate);
  }

  public void ForEachFreeElement(System.Action<GameObject> predicate)
  {
    this.freeElements.ForEach(predicate);
  }
}
