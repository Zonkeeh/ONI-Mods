// Decompiled with JetBrains decompiler
// Type: UIPool`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class UIPool<T> where T : MonoBehaviour
{
  private List<T> freeElements = new List<T>();
  private List<T> activeElements = new List<T>();
  private T prefab;
  public Transform disabledElementParent;

  public UIPool(T prefab)
  {
    this.prefab = prefab;
    this.freeElements = new List<T>();
    this.activeElements = new List<T>();
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

  public T GetFreeElement(GameObject instantiateParent = null, bool forceActive = false)
  {
    if (this.freeElements.Count == 0)
    {
      this.activeElements.Add(Util.KInstantiateUI<T>(this.prefab.gameObject, instantiateParent, false));
    }
    else
    {
      T freeElement = this.freeElements[0];
      this.activeElements.Add(freeElement);
      if ((UnityEngine.Object) freeElement.transform.parent != (UnityEngine.Object) instantiateParent)
        freeElement.transform.SetParent(instantiateParent.transform);
      this.freeElements.RemoveAt(0);
    }
    T activeElement = this.activeElements[this.activeElements.Count - 1];
    if (activeElement.gameObject.activeInHierarchy != forceActive)
      activeElement.gameObject.SetActive(forceActive);
    return activeElement;
  }

  public void ClearElement(T element)
  {
    if (!this.activeElements.Contains(element))
    {
      Debug.LogError(!this.freeElements.Contains(element) ? (object) "The element provided does not belong to this pool" : (object) "The element provided is already inactive");
    }
    else
    {
      if ((UnityEngine.Object) this.disabledElementParent != (UnityEngine.Object) null)
        element.gameObject.transform.SetParent(this.disabledElementParent);
      element.gameObject.SetActive(false);
      this.freeElements.Add(element);
      this.activeElements.Remove(element);
    }
  }

  public void ClearAll()
  {
    while (this.activeElements.Count > 0)
    {
      if ((UnityEngine.Object) this.disabledElementParent != (UnityEngine.Object) null)
        this.activeElements[0].gameObject.transform.SetParent(this.disabledElementParent);
      this.activeElements[0].gameObject.SetActive(false);
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
    this.activeElements.ForEach((System.Action<T>) (ae => UnityEngine.Object.Destroy((UnityEngine.Object) ae.gameObject)));
    this.activeElements.Clear();
  }

  public void DestroyAllFree()
  {
    this.freeElements.ForEach((System.Action<T>) (ae => UnityEngine.Object.Destroy((UnityEngine.Object) ae.gameObject)));
    this.freeElements.Clear();
  }

  public void ForEachActiveElement(System.Action<T> predicate)
  {
    this.activeElements.ForEach(predicate);
  }

  public void ForEachFreeElement(System.Action<T> predicate)
  {
    this.freeElements.ForEach(predicate);
  }
}
