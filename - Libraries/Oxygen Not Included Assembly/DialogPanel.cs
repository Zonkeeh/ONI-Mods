// Decompiled with JetBrains decompiler
// Type: DialogPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class DialogPanel : MonoBehaviour, IDeselectHandler, IEventSystemHandler
{
  public bool destroyOnDeselect = true;

  public void OnDeselect(BaseEventData eventData)
  {
    if (this.destroyOnDeselect)
    {
      IEnumerator enumerator = this.transform.GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
          Util.KDestroyGameObject(((Component) enumerator.Current).gameObject);
      }
      finally
      {
        if (enumerator is IDisposable disposable)
          disposable.Dispose();
      }
    }
    this.gameObject.SetActive(false);
  }
}
