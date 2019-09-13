// Decompiled with JetBrains decompiler
// Type: Tween
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tween : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
{
  private static float Scale = 1.025f;
  private static float ScaleSpeed = 0.5f;
  private float Direction = -1f;
  private Selectable Selectable;

  private void Awake()
  {
    this.Selectable = this.GetComponent<Selectable>();
  }

  public void OnPointerEnter(PointerEventData data)
  {
    this.Direction = 1f;
  }

  public void OnPointerExit(PointerEventData data)
  {
    this.Direction = -1f;
  }

  private void Update()
  {
    if (!this.Selectable.interactable)
      return;
    float x = this.transform.localScale.x;
    float num = Mathf.Max(Mathf.Min(x + this.Direction * Time.unscaledDeltaTime * Tween.ScaleSpeed, Tween.Scale), 1f);
    if ((double) num == (double) x)
      return;
    this.transform.localScale = new Vector3(num, num, 1f);
  }
}
