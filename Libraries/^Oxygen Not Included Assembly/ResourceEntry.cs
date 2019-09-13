// Decompiled with JetBrains decompiler
// Type: ResourceEntry
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ResourceEntry : KMonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
{
  private float currentQuantity = float.MinValue;
  public Tag Resource;
  public GameUtil.MeasureUnit Measure;
  public LocText NameLabel;
  public LocText QuantityLabel;
  public Image image;
  [SerializeField]
  private Color AvailableColor;
  [SerializeField]
  private Color UnavailableColor;
  [SerializeField]
  private Color OverdrawnColor;
  [SerializeField]
  private Color HighlightColor;
  [SerializeField]
  private Color BackgroundHoverColor;
  [SerializeField]
  private Image Background;
  [MyCmpGet]
  private ToolTip tooltip;
  [MyCmpReq]
  private UnityEngine.UI.Button button;
  private int selectionIdx;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.QuantityLabel.color = this.AvailableColor;
    this.NameLabel.color = this.AvailableColor;
    this.button.onClick.AddListener(new UnityAction(this.OnClick));
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.tooltip.OnToolTip = new Func<string>(this.OnToolTip);
  }

  private void OnClick()
  {
    List<Pickupable> pickupables = WorldInventory.Instance.GetPickupables(this.Resource);
    if (pickupables == null)
      return;
    Pickupable cmp = (Pickupable) null;
    for (int index1 = 0; index1 < pickupables.Count; ++index1)
    {
      ++this.selectionIdx;
      int index2 = this.selectionIdx % pickupables.Count;
      cmp = pickupables[index2];
      if ((UnityEngine.Object) cmp != (UnityEngine.Object) null && !cmp.HasTag(GameTags.StoredPrivate))
        break;
    }
    if (!((UnityEngine.Object) cmp != (UnityEngine.Object) null))
      return;
    Transform transform = cmp.transform;
    if ((UnityEngine.Object) cmp.storage != (UnityEngine.Object) null)
      transform = cmp.storage.transform;
    SelectTool.Instance.SelectAndFocus(transform.transform.GetPosition(), transform.GetComponent<KSelectable>(), Vector3.zero);
    for (int index = 0; index < pickupables.Count; ++index)
    {
      Pickupable pickupable = pickupables[index];
      if ((UnityEngine.Object) pickupable != (UnityEngine.Object) null)
      {
        KAnimControllerBase component = pickupable.GetComponent<KAnimControllerBase>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
          component.HighlightColour = (Color32) this.HighlightColor;
      }
    }
  }

  public void GetAmounts(
    EdiblesManager.FoodInfo food_info,
    bool doExtras,
    out float available,
    out float total,
    out float reserved)
  {
    available = WorldInventory.Instance.GetAmount(this.Resource);
    total = !doExtras ? 0.0f : WorldInventory.Instance.GetTotalAmount(this.Resource);
    reserved = !doExtras ? 0.0f : MaterialNeeds.Instance.GetAmount(this.Resource);
    if (food_info == null)
      return;
    available *= food_info.CaloriesPerUnit;
    total *= food_info.CaloriesPerUnit;
    reserved *= food_info.CaloriesPerUnit;
  }

  private void GetAmounts(bool doExtras, out float available, out float total, out float reserved)
  {
    this.GetAmounts(this.Measure != GameUtil.MeasureUnit.kcal ? (EdiblesManager.FoodInfo) null : Game.Instance.ediblesManager.GetFoodInfo(this.Resource.Name), doExtras, out available, out total, out reserved);
  }

  public void UpdateValue()
  {
    this.SetName(this.Resource.ProperName());
    float available;
    float total;
    float reserved;
    this.GetAmounts(GenericGameSettings.instance.allowInsufficientMaterialBuild, out available, out total, out reserved);
    if ((double) this.currentQuantity != (double) available)
    {
      this.currentQuantity = available;
      this.QuantityLabel.text = ResourceCategoryScreen.QuantityTextForMeasure(available, this.Measure);
    }
    Color color = this.AvailableColor;
    if ((double) reserved > (double) total)
      color = this.OverdrawnColor;
    else if ((double) available == 0.0)
      color = this.UnavailableColor;
    if (this.QuantityLabel.color != color)
      this.QuantityLabel.color = color;
    if (!(this.NameLabel.color != color))
      return;
    this.NameLabel.color = color;
  }

  private string OnToolTip()
  {
    float available;
    float total;
    float reserved;
    this.GetAmounts(true, out available, out total, out reserved);
    return this.NameLabel.text + "\n" + string.Format((string) STRINGS.UI.RESOURCESCREEN.AVAILABLE_TOOLTIP, (object) ResourceCategoryScreen.QuantityTextForMeasure(available, this.Measure), (object) ResourceCategoryScreen.QuantityTextForMeasure(reserved, this.Measure), (object) ResourceCategoryScreen.QuantityTextForMeasure(total, this.Measure));
  }

  public void SetName(string name)
  {
    this.NameLabel.text = name;
  }

  public void SetTag(Tag t, GameUtil.MeasureUnit measure)
  {
    this.Resource = t;
    this.Measure = measure;
  }

  private void Hover(bool is_hovering)
  {
    if ((UnityEngine.Object) WorldInventory.Instance == (UnityEngine.Object) null)
      return;
    if (is_hovering)
      this.Background.color = this.BackgroundHoverColor;
    else
      this.Background.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
    List<Pickupable> pickupables = WorldInventory.Instance.GetPickupables(this.Resource);
    if (pickupables == null)
      return;
    for (int index = 0; index < pickupables.Count; ++index)
    {
      if (!((UnityEngine.Object) pickupables[index] == (UnityEngine.Object) null))
      {
        KAnimControllerBase component = pickupables[index].GetComponent<KAnimControllerBase>();
        if (!((UnityEngine.Object) component == (UnityEngine.Object) null))
          component.HighlightColour = !is_hovering ? (Color32) Color.black : (Color32) this.HighlightColor;
      }
    }
  }

  public void OnPointerEnter(PointerEventData eventData)
  {
    this.Hover(true);
  }

  public void OnPointerExit(PointerEventData eventData)
  {
    this.Hover(false);
  }

  public void SetSprite(Tag t)
  {
    Element elementByName = ElementLoader.FindElementByName(this.Resource.Name);
    if (elementByName == null)
      return;
    Sprite fromMultiObjectAnim = Def.GetUISpriteFromMultiObjectAnim(elementByName.substance.anim, "ui", false, string.Empty);
    if (!((UnityEngine.Object) fromMultiObjectAnim != (UnityEngine.Object) null))
      return;
    this.image.sprite = fromMultiObjectAnim;
  }

  public void SetSprite(Sprite sprite)
  {
    this.image.sprite = sprite;
  }
}
