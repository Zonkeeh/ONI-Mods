// Decompiled with JetBrains decompiler
// Type: ShadowRect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

public class ShadowRect : MonoBehaviour
{
  [SerializeField]
  protected Color shadowColor = new Color(0.0f, 0.0f, 0.0f, 0.6f);
  [SerializeField]
  protected Vector2 ShadowOffset = new Vector2(1.5f, -1.5f);
  public RectTransform RectMain;
  public RectTransform RectShadow;
  private LayoutElement shadowLayoutElement;

  private void OnEnable()
  {
    if ((Object) this.RectShadow != (Object) null)
    {
      this.RectShadow.name = "Shadow_" + this.RectMain.name;
      this.MatchRect();
    }
    else
      Debug.LogWarning((object) ("Shadowrect is missing rectshadow: " + this.gameObject.name));
  }

  private void Update()
  {
    this.MatchRect();
  }

  protected virtual void MatchRect()
  {
    if ((Object) this.RectShadow == (Object) null || (Object) this.RectMain == (Object) null)
      return;
    if ((Object) this.shadowLayoutElement == (Object) null)
      this.shadowLayoutElement = this.RectShadow.GetComponent<LayoutElement>();
    if ((Object) this.shadowLayoutElement != (Object) null && !this.shadowLayoutElement.ignoreLayout)
      this.shadowLayoutElement.ignoreLayout = true;
    if ((Object) this.RectShadow.transform.parent != (Object) this.RectMain.transform.parent)
      this.RectShadow.transform.SetParent(this.RectMain.transform.parent);
    if (this.RectShadow.GetSiblingIndex() >= this.RectMain.GetSiblingIndex())
      this.RectShadow.SetAsFirstSibling();
    this.RectShadow.transform.localScale = Vector3.one;
    if (this.RectShadow.pivot != this.RectMain.pivot)
      this.RectShadow.pivot = this.RectMain.pivot;
    if (this.RectShadow.anchorMax != this.RectMain.anchorMax)
      this.RectShadow.anchorMax = this.RectMain.anchorMax;
    if (this.RectShadow.anchorMin != this.RectMain.anchorMin)
      this.RectShadow.anchorMin = this.RectMain.anchorMin;
    if (this.RectShadow.sizeDelta != this.RectMain.sizeDelta)
      this.RectShadow.sizeDelta = this.RectMain.sizeDelta;
    if (this.RectShadow.anchoredPosition != this.RectMain.anchoredPosition + this.ShadowOffset)
      this.RectShadow.anchoredPosition = this.RectMain.anchoredPosition + this.ShadowOffset;
    if (this.RectMain.gameObject.activeInHierarchy == this.RectShadow.gameObject.activeInHierarchy)
      return;
    this.RectShadow.gameObject.SetActive(this.RectMain.gameObject.activeInHierarchy);
  }
}
