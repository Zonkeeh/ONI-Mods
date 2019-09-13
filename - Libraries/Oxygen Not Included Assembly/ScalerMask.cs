// Decompiled with JetBrains decompiler
// Type: ScalerMask
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScalerMask : KMonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
{
  private bool queuedSizeUpdate = true;
  public RectTransform SourceTransform;
  private RectTransform _thisTransform;
  private LayoutElement _thisLayoutElement;
  public GameObject hoverIndicator;
  public bool hoverLock;
  private bool grandparentIsHovered;
  private bool isHovered;
  public float topPadding;
  public float bottomPadding;

  private RectTransform ThisTransform
  {
    get
    {
      if ((Object) this._thisTransform == (Object) null)
        this._thisTransform = this.GetComponent<RectTransform>();
      return this._thisTransform;
    }
  }

  private LayoutElement ThisLayoutElement
  {
    get
    {
      if ((Object) this._thisLayoutElement == (Object) null)
        this._thisLayoutElement = this.GetComponent<LayoutElement>();
      return this._thisLayoutElement;
    }
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    DetailsScreen componentInParent = this.GetComponentInParent<DetailsScreen>();
    if (!(bool) ((Object) componentInParent))
      return;
    DetailsScreen detailsScreen1 = componentInParent;
    detailsScreen1.pointerEnterActions = detailsScreen1.pointerEnterActions + new KScreen.PointerEnterActions(this.OnPointerEnterGrandparent);
    DetailsScreen detailsScreen2 = componentInParent;
    detailsScreen2.pointerExitActions = detailsScreen2.pointerExitActions + new KScreen.PointerExitActions(this.OnPointerExitGrandparent);
  }

  protected override void OnCleanUp()
  {
    DetailsScreen componentInParent = this.GetComponentInParent<DetailsScreen>();
    if ((bool) ((Object) componentInParent))
    {
      DetailsScreen detailsScreen1 = componentInParent;
      detailsScreen1.pointerEnterActions = detailsScreen1.pointerEnterActions - new KScreen.PointerEnterActions(this.OnPointerEnterGrandparent);
      DetailsScreen detailsScreen2 = componentInParent;
      detailsScreen2.pointerExitActions = detailsScreen2.pointerExitActions - new KScreen.PointerExitActions(this.OnPointerExitGrandparent);
    }
    base.OnCleanUp();
  }

  private void Update()
  {
    if ((Object) this.SourceTransform != (Object) null)
      this.SourceTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this.ThisTransform.rect.width);
    if ((Object) this.SourceTransform != (Object) null && (!this.hoverLock || !this.grandparentIsHovered || (this.isHovered || this.queuedSizeUpdate)))
    {
      this.ThisLayoutElement.minHeight = this.SourceTransform.rect.height + this.topPadding + this.bottomPadding;
      this.SourceTransform.anchoredPosition = new Vector2(0.0f, -this.topPadding);
      this.queuedSizeUpdate = false;
    }
    if (!((Object) this.hoverIndicator != (Object) null))
      return;
    if ((Object) this.SourceTransform != (Object) null && (double) this.SourceTransform.rect.height > (double) this.ThisTransform.rect.height)
      this.hoverIndicator.SetActive(true);
    else
      this.hoverIndicator.SetActive(false);
  }

  public void UpdateSize()
  {
    this.queuedSizeUpdate = true;
  }

  public void OnPointerEnterGrandparent(PointerEventData eventData)
  {
    this.grandparentIsHovered = true;
  }

  public void OnPointerExitGrandparent(PointerEventData eventData)
  {
    this.grandparentIsHovered = false;
  }

  public void OnPointerEnter(PointerEventData eventData)
  {
    this.isHovered = true;
  }

  public void OnPointerExit(PointerEventData eventData)
  {
    this.isHovered = false;
  }
}
