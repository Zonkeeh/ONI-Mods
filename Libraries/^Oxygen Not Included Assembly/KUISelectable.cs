// Decompiled with JetBrains decompiler
// Type: KUISelectable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class KUISelectable : KMonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
{
  private GameObject target;

  protected override void OnPrefabInit()
  {
  }

  protected override void OnSpawn()
  {
    this.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(new UnityAction(this.OnClick));
  }

  public void SetTarget(GameObject target)
  {
    this.target = target;
  }

  public void OnPointerEnter(PointerEventData eventData)
  {
    if (!((Object) this.target != (Object) null))
      return;
    SelectTool.Instance.SetHoverOverride(this.target.GetComponent<KSelectable>());
  }

  public void OnPointerExit(PointerEventData eventData)
  {
    SelectTool.Instance.SetHoverOverride((KSelectable) null);
  }

  private void OnClick()
  {
    if (!((Object) this.target != (Object) null))
      return;
    SelectTool.Instance.Select(this.target.GetComponent<KSelectable>(), false);
  }

  protected override void OnCmpDisable()
  {
    if (!((Object) SelectTool.Instance != (Object) null))
      return;
    SelectTool.Instance.SetHoverOverride((KSelectable) null);
  }
}
