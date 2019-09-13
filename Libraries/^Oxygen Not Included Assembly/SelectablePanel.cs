// Decompiled with JetBrains decompiler
// Type: SelectablePanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;

public class SelectablePanel : MonoBehaviour, IDeselectHandler, IEventSystemHandler
{
  public void OnDeselect(BaseEventData evt)
  {
    this.gameObject.SetActive(false);
  }
}
