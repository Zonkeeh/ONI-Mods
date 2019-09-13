// Decompiled with JetBrains decompiler
// Type: ResourceCategoryHeader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ResourceCategoryHeader : KMonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
{
  public Dictionary<Tag, ResourceEntry> ResourcesDiscovered = new Dictionary<Tag, ResourceEntry>();
  private float cachedAvailable = float.MinValue;
  private float cachedTotal = float.MinValue;
  private float cachedReserved = float.MinValue;
  public GameObject Prefab_ResourceEntry;
  public Transform EntryContainer;
  public Tag ResourceCategoryTag;
  public GameUtil.MeasureUnit Measure;
  public bool IsOpen;
  public ImageToggleState expandArrow;
  private UnityEngine.UI.Button mButton;
  public ResourceCategoryHeader.ElementReferences elements;
  public Color TextColor_Interactable;
  public Color TextColor_NonInteractable;
  private string quantityString;
  private float currentQuantity;
  private bool anyDiscovered;
  [MyCmpGet]
  private ToolTip tooltip;
  [SerializeField]
  private int minimizedFontSize;
  [SerializeField]
  private int maximizedFontSize;
  [SerializeField]
  private Color highlightColour;
  [SerializeField]
  private Color BackgroundHoverColor;
  [SerializeField]
  private Image Background;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.EntryContainer.SetParent(this.transform.parent);
    this.EntryContainer.SetSiblingIndex(this.transform.GetSiblingIndex() + 1);
    this.EntryContainer.localScale = Vector3.one;
    this.mButton = this.GetComponent<UnityEngine.UI.Button>();
    this.mButton.onClick.AddListener((UnityAction) (() => this.ToggleOpen(true)));
    this.SetInteractable(this.anyDiscovered);
    this.SetActiveColor(false);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.tooltip.OnToolTip = new Func<string>(this.OnTooltip);
    this.UpdateContents();
  }

  private void SetInteractable(bool state)
  {
    if (state)
    {
      if (!this.IsOpen)
        this.expandArrow.SetInactive();
      else
        this.expandArrow.SetActive();
    }
    else
    {
      this.SetOpen(false);
      this.expandArrow.SetDisabled();
    }
  }

  private void SetActiveColor(bool state)
  {
    if (state)
    {
      this.elements.QuantityText.color = this.TextColor_Interactable;
      this.elements.LabelText.color = this.TextColor_Interactable;
      this.expandArrow.ActiveColour = this.TextColor_Interactable;
      this.expandArrow.InactiveColour = this.TextColor_Interactable;
      this.expandArrow.TargetImage.color = this.TextColor_Interactable;
    }
    else
    {
      this.elements.LabelText.color = this.TextColor_NonInteractable;
      this.elements.QuantityText.color = this.TextColor_NonInteractable;
      this.expandArrow.ActiveColour = this.TextColor_NonInteractable;
      this.expandArrow.InactiveColour = this.TextColor_NonInteractable;
      this.expandArrow.TargetImage.color = this.TextColor_NonInteractable;
    }
  }

  public void SetTag(Tag t, GameUtil.MeasureUnit measure)
  {
    this.ResourceCategoryTag = t;
    this.Measure = measure;
    this.elements.LabelText.text = t.ProperName();
    if (!SaveGame.Instance.expandedResourceTags.Contains(this.ResourceCategoryTag))
      return;
    this.anyDiscovered = true;
    this.ToggleOpen(false);
  }

  private void ToggleOpen(bool play_sound)
  {
    if (!this.anyDiscovered)
    {
      if (!play_sound)
        return;
      KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative", false));
    }
    else if (!this.IsOpen)
    {
      if (play_sound)
        KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Open", false));
      this.SetOpen(true);
      this.elements.LabelText.fontSize = (float) this.maximizedFontSize;
      this.elements.QuantityText.fontSize = (float) this.maximizedFontSize;
    }
    else
    {
      if (play_sound)
        KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Close", false));
      this.SetOpen(false);
      this.elements.LabelText.fontSize = (float) this.minimizedFontSize;
      this.elements.QuantityText.fontSize = (float) this.minimizedFontSize;
    }
  }

  private void Hover(bool is_hovering)
  {
    this.Background.color = !is_hovering ? new Color(0.0f, 0.0f, 0.0f, 0.0f) : this.BackgroundHoverColor;
    List<Pickupable> pickupableList = (List<Pickupable>) null;
    if ((UnityEngine.Object) WorldInventory.Instance != (UnityEngine.Object) null)
      pickupableList = WorldInventory.Instance.GetPickupables(this.ResourceCategoryTag);
    if (pickupableList == null)
      return;
    for (int index = 0; index < pickupableList.Count; ++index)
    {
      if (!((UnityEngine.Object) pickupableList[index] == (UnityEngine.Object) null))
      {
        KAnimControllerBase component = pickupableList[index].GetComponent<KAnimControllerBase>();
        if (!((UnityEngine.Object) component == (UnityEngine.Object) null))
          component.HighlightColour = (Color32) (!is_hovering ? Color.black : this.highlightColour);
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

  public void SetOpen(bool open)
  {
    this.IsOpen = open;
    if (open)
    {
      this.expandArrow.SetActive();
      if (!SaveGame.Instance.expandedResourceTags.Contains(this.ResourceCategoryTag))
        SaveGame.Instance.expandedResourceTags.Add(this.ResourceCategoryTag);
    }
    else
    {
      this.expandArrow.SetInactive();
      SaveGame.Instance.expandedResourceTags.Remove(this.ResourceCategoryTag);
    }
    this.EntryContainer.gameObject.SetActive(this.IsOpen);
  }

  private void GetAmounts(bool doExtras, out float available, out float total, out float reserved)
  {
    available = 0.0f;
    total = 0.0f;
    reserved = 0.0f;
    HashSet<Tag> resources = (HashSet<Tag>) null;
    if (!WorldInventory.Instance.TryGetDiscoveredResourcesFromTag(this.ResourceCategoryTag, out resources))
      return;
    ListPool<Tag, ResourceCategoryHeader>.PooledList pooledList = ListPool<Tag, ResourceCategoryHeader>.Allocate();
    foreach (Tag tag in resources)
    {
      EdiblesManager.FoodInfo food_info = (EdiblesManager.FoodInfo) null;
      if (this.Measure == GameUtil.MeasureUnit.kcal)
      {
        food_info = Game.Instance.ediblesManager.GetFoodInfo(tag.Name);
        if (food_info == null)
        {
          pooledList.Add(tag);
          continue;
        }
      }
      this.anyDiscovered = true;
      ResourceEntry resourceEntry = (ResourceEntry) null;
      if (!this.ResourcesDiscovered.TryGetValue(tag, out resourceEntry))
      {
        resourceEntry = this.NewResourceEntry(tag, this.Measure);
        this.ResourcesDiscovered.Add(tag, resourceEntry);
      }
      float available1;
      float total1;
      float reserved1;
      resourceEntry.GetAmounts(food_info, doExtras, out available1, out total1, out reserved1);
      available += available1;
      total += total1;
      reserved += reserved1;
    }
    foreach (Tag tag in (List<Tag>) pooledList)
      resources.Remove(tag);
    pooledList.Recycle();
  }

  public void UpdateContents()
  {
    float available;
    float total;
    float reserved;
    this.GetAmounts(false, out available, out total, out reserved);
    if ((double) available != (double) this.cachedAvailable || (double) total != (double) this.cachedTotal || (double) reserved != (double) this.cachedReserved)
    {
      if (this.quantityString == null || (double) this.currentQuantity != (double) available)
      {
        switch (this.Measure)
        {
          case GameUtil.MeasureUnit.mass:
            this.quantityString = GameUtil.GetFormattedMass(available, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}");
            break;
          case GameUtil.MeasureUnit.kcal:
            this.quantityString = GameUtil.GetFormattedCalories(available, GameUtil.TimeSlice.None, true);
            break;
          case GameUtil.MeasureUnit.quantity:
            this.quantityString = available.ToString();
            break;
        }
        this.elements.QuantityText.text = this.quantityString;
        this.currentQuantity = available;
      }
      this.cachedAvailable = available;
      this.cachedTotal = total;
      this.cachedReserved = reserved;
    }
    foreach (KeyValuePair<Tag, ResourceEntry> keyValuePair in this.ResourcesDiscovered)
      keyValuePair.Value.UpdateValue();
    this.SetActiveColor((double) available > 0.0);
    this.SetInteractable(this.anyDiscovered);
  }

  private string OnTooltip()
  {
    float available;
    float total;
    float reserved;
    this.GetAmounts(true, out available, out total, out reserved);
    return this.elements.LabelText.text + "\n" + string.Format((string) STRINGS.UI.RESOURCESCREEN.AVAILABLE_TOOLTIP, (object) ResourceCategoryScreen.QuantityTextForMeasure(available, this.Measure), (object) ResourceCategoryScreen.QuantityTextForMeasure(reserved, this.Measure), (object) ResourceCategoryScreen.QuantityTextForMeasure(total, this.Measure));
  }

  private ResourceEntry NewResourceEntry(Tag resourceTag, GameUtil.MeasureUnit measure)
  {
    ResourceEntry component = Util.KInstantiateUI(this.Prefab_ResourceEntry, this.EntryContainer.gameObject, true).GetComponent<ResourceEntry>();
    component.SetTag(resourceTag, measure);
    return component;
  }

  [Serializable]
  public struct ElementReferences
  {
    public LocText LabelText;
    public LocText QuantityText;
  }
}
