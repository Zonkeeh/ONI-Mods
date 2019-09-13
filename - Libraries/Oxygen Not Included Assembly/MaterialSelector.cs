// Decompiled with JetBrains decompiler
// Type: MaterialSelector
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaterialSelector : KScreen
{
  public Dictionary<Tag, KToggle> ElementToggles = new Dictionary<Tag, KToggle>();
  public Tag CurrentSelectedElement;
  public int selectorIndex;
  public MaterialSelector.SelectMaterialActions selectMaterialActions;
  public MaterialSelector.SelectMaterialActions deselectMaterialActions;
  private ToggleGroup toggleGroup;
  public GameObject TogglePrefab;
  public GameObject LayoutContainer;
  public KScrollRect ScrollRect;
  public GameObject Scrollbar;
  public GameObject Headerbar;
  public GameObject BadBG;
  public LocText NoMaterialDiscovered;
  public GameObject MaterialDescriptionPane;
  public LocText MaterialDescriptionText;
  public DescriptorPanel MaterialEffectsPane;
  public GameObject DescriptorsPanel;
  private KToggle selectedToggle;
  private Recipe.Ingredient activeIngredient;
  private Recipe activeRecipe;
  private float activeMass;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.toggleGroup = this.GetComponent<ToggleGroup>();
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (e.Consumed)
      return;
    base.OnKeyDown(e);
  }

  public void ClearMaterialToggles()
  {
    this.CurrentSelectedElement = (Tag) ((string) null);
    this.NoMaterialDiscovered.gameObject.SetActive(false);
    foreach (KeyValuePair<Tag, KToggle> elementToggle in this.ElementToggles)
    {
      elementToggle.Value.gameObject.SetActive(false);
      Util.KDestroyGameObject(elementToggle.Value.gameObject);
    }
    this.ElementToggles.Clear();
  }

  public void ConfigureScreen(Recipe.Ingredient ingredient, Recipe recipe)
  {
    this.ClearMaterialToggles();
    this.activeIngredient = ingredient;
    this.activeRecipe = recipe;
    this.activeMass = ingredient.amount;
    List<Tag> tagList = new List<Tag>();
    foreach (Element element in ElementLoader.elements)
    {
      if (element.IsSolid && (element.tag == ingredient.tag || element.HasTag(ingredient.tag)))
        tagList.Add(element.tag);
    }
    foreach (Tag materialBuildingElement in GameTags.MaterialBuildingElements)
    {
      if (materialBuildingElement == ingredient.tag)
      {
        foreach (GameObject gameObject in Assets.GetPrefabsWithTag(materialBuildingElement))
        {
          KPrefabID component = gameObject.GetComponent<KPrefabID>();
          if ((UnityEngine.Object) component != (UnityEngine.Object) null && !tagList.Contains(component.PrefabTag))
            tagList.Add(component.PrefabTag);
        }
      }
    }
    foreach (Tag tag in tagList)
    {
      if (!this.ElementToggles.ContainsKey(tag))
      {
        GameObject gameObject = Util.KInstantiate(this.TogglePrefab, this.LayoutContainer, "MaterialSelection_" + tag.ProperName());
        gameObject.transform.localScale = Vector3.one;
        gameObject.SetActive(true);
        KToggle component = gameObject.GetComponent<KToggle>();
        this.ElementToggles.Add(tag, component);
        component.group = this.toggleGroup;
        gameObject.gameObject.GetComponent<ToolTip>().toolTip = tag.ProperName();
      }
    }
    this.RefreshToggleContents();
  }

  private void SetToggleBGImage(KToggle toggle, Tag elem)
  {
    if ((UnityEngine.Object) toggle == (UnityEngine.Object) this.selectedToggle)
    {
      toggle.GetComponentsInChildren<Image>()[1].material = GlobalResources.Instance().AnimUIMaterial;
      toggle.GetComponent<ImageToggleState>().SetActive();
    }
    else if ((double) WorldInventory.Instance.GetAmount(elem) >= (double) this.activeMass || DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive)
    {
      toggle.GetComponentsInChildren<Image>()[1].material = GlobalResources.Instance().AnimUIMaterial;
      toggle.GetComponentsInChildren<Image>()[1].color = Color.white;
      toggle.GetComponent<ImageToggleState>().SetInactive();
    }
    else
    {
      toggle.GetComponentsInChildren<Image>()[1].material = GlobalResources.Instance().AnimMaterialUIDesaturated;
      toggle.GetComponentsInChildren<Image>()[1].color = new Color(1f, 1f, 1f, 0.6f);
      if (MaterialSelector.AllowInsufficientMaterialBuild())
        return;
      toggle.GetComponent<ImageToggleState>().SetDisabled();
    }
  }

  public void OnSelectMaterial(Tag elem, Recipe recipe, bool focusScrollRect = false)
  {
    KToggle elementToggle1 = this.ElementToggles[elem];
    if ((UnityEngine.Object) elementToggle1 != (UnityEngine.Object) this.selectedToggle)
    {
      this.selectedToggle = elementToggle1;
      if (recipe != null)
        SaveGame.Instance.materialSelectorSerializer.SetSelectedElement(this.selectorIndex, recipe.Result, elem);
      this.CurrentSelectedElement = elem;
      if (this.selectMaterialActions != null)
        this.selectMaterialActions();
      this.UpdateHeader();
      this.SetDescription(elem);
      this.SetEffects(elem);
      if (!this.MaterialDescriptionPane.gameObject.activeSelf && !this.MaterialEffectsPane.gameObject.activeSelf)
        this.DescriptorsPanel.SetActive(false);
      else
        this.DescriptorsPanel.SetActive(true);
    }
    if (focusScrollRect && this.ElementToggles.Count > 1)
    {
      List<Tag> tagList = new List<Tag>();
      foreach (KeyValuePair<Tag, KToggle> elementToggle2 in this.ElementToggles)
        tagList.Add(elementToggle2.Key);
      tagList.Sort(new Comparison<Tag>(this.ElementSorter));
      this.ScrollRect.normalizedPosition = new Vector2((float) tagList.IndexOf(elem) / (float) (tagList.Count - 1), 0.0f);
    }
    this.RefreshToggleContents();
  }

  public void RefreshToggleContents()
  {
    foreach (KeyValuePair<Tag, KToggle> elementToggle in this.ElementToggles)
    {
      KToggle ktoggle = elementToggle.Value;
      Tag elem = elementToggle.Key;
      GameObject gameObject = ktoggle.gameObject;
      LocText[] componentsInChildren = gameObject.GetComponentsInChildren<LocText>();
      LocText locText1 = componentsInChildren[0];
      LocText locText2 = componentsInChildren[1];
      Image componentsInChild = gameObject.GetComponentsInChildren<Image>()[1];
      locText2.text = Util.FormatWholeNumber(WorldInventory.Instance.GetAmount(elem));
      locText1.text = Util.FormatWholeNumber(this.activeMass);
      GameObject prefab = Assets.TryGetPrefab(elementToggle.Key);
      if ((UnityEngine.Object) prefab != (UnityEngine.Object) null)
      {
        KBatchedAnimController component = prefab.GetComponent<KBatchedAnimController>();
        componentsInChild.sprite = Def.GetUISpriteFromMultiObjectAnim(component.AnimFiles[0], "ui", false, string.Empty);
      }
      gameObject.SetActive(WorldInventory.Instance.IsDiscovered(elem) || DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive);
      this.SetToggleBGImage(elementToggle.Value, elementToggle.Key);
      ktoggle.soundPlayer.AcceptClickCondition = (Func<bool>) (() => this.IsEnoughMass(elem));
      ktoggle.ClearOnClick();
      if (this.IsEnoughMass(elem))
        ktoggle.onClick += (System.Action) (() => this.OnSelectMaterial(elem, this.activeRecipe, false));
    }
    this.SortElementToggles();
    this.UpdateMaterialTooltips();
    this.UpdateHeader();
  }

  private bool IsEnoughMass(Tag t)
  {
    if ((double) WorldInventory.Instance.GetAmount(t) < (double) this.activeMass && !DebugHandler.InstantBuildMode && !Game.Instance.SandboxModeActive)
      return MaterialSelector.AllowInsufficientMaterialBuild();
    return true;
  }

  public bool AutoSelectAvailableMaterial()
  {
    if (this.activeRecipe == null || this.ElementToggles.Count == 0)
      return false;
    Tag previousElement = SaveGame.Instance.materialSelectorSerializer.GetPreviousElement(this.selectorIndex, this.activeRecipe.Result);
    if (previousElement != (Tag) ((string) null))
    {
      KToggle ktoggle;
      this.ElementToggles.TryGetValue(previousElement, out ktoggle);
      if ((UnityEngine.Object) ktoggle != (UnityEngine.Object) null && (DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive || (double) WorldInventory.Instance.GetAmount(previousElement) >= (double) this.activeMass))
      {
        this.OnSelectMaterial(previousElement, this.activeRecipe, true);
        return true;
      }
    }
    float num = -1f;
    List<Tag> tagList = new List<Tag>();
    foreach (KeyValuePair<Tag, KToggle> elementToggle in this.ElementToggles)
      tagList.Add(elementToggle.Key);
    tagList.Sort(new Comparison<Tag>(this.ElementSorter));
    if (DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive)
    {
      this.OnSelectMaterial(tagList[0], this.activeRecipe, true);
      return true;
    }
    Tag elem = (Tag) ((string) null);
    foreach (Tag tag in tagList)
    {
      float amount = WorldInventory.Instance.GetAmount(tag);
      if ((double) amount >= (double) this.activeMass && (double) amount > (double) num)
      {
        num = amount;
        elem = tag;
      }
    }
    if (!(elem != (Tag) ((string) null)))
      return false;
    this.OnSelectMaterial(elem, this.activeRecipe, true);
    return true;
  }

  private void SortElementToggles()
  {
    List<Tag> tagList = new List<Tag>();
    foreach (KeyValuePair<Tag, KToggle> elementToggle in this.ElementToggles)
      tagList.Add(elementToggle.Key);
    tagList.Sort(new Comparison<Tag>(this.ElementSorter));
    foreach (Tag index in tagList)
      this.ElementToggles[index].transform.SetAsLastSibling();
    this.UpdateScrollBar();
  }

  private void UpdateMaterialTooltips()
  {
    foreach (KeyValuePair<Tag, KToggle> elementToggle in this.ElementToggles)
    {
      ToolTip component = elementToggle.Value.gameObject.GetComponent<ToolTip>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        component.toolTip = GameUtil.GetMaterialTooltips(elementToggle.Key);
    }
  }

  private void UpdateScrollBar()
  {
    int num = 0;
    foreach (KeyValuePair<Tag, KToggle> elementToggle in this.ElementToggles)
    {
      if (elementToggle.Value.gameObject.activeSelf)
        ++num;
    }
    this.Scrollbar.SetActive(num > 5);
  }

  private void UpdateHeader()
  {
    if (this.activeIngredient == null)
      return;
    int num = 0;
    foreach (KeyValuePair<Tag, KToggle> elementToggle in this.ElementToggles)
    {
      if (elementToggle.Value.gameObject.activeSelf)
        ++num;
    }
    LocText componentInChildren = this.Headerbar.GetComponentInChildren<LocText>();
    if (num == 0)
    {
      componentInChildren.text = string.Format((string) STRINGS.UI.PRODUCTINFO_MISSINGRESOURCES_TITLE, (object) this.activeIngredient.tag.ProperName(), (object) GameUtil.GetFormattedMass(this.activeIngredient.amount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
      this.NoMaterialDiscovered.text = string.Format((string) STRINGS.UI.PRODUCTINFO_MISSINGRESOURCES_DESC, (object) this.activeIngredient.tag.ProperName());
      this.NoMaterialDiscovered.gameObject.SetActive(true);
      this.NoMaterialDiscovered.color = Constants.NEGATIVE_COLOR;
      this.BadBG.SetActive(true);
      this.Scrollbar.SetActive(false);
      this.LayoutContainer.SetActive(false);
    }
    else
    {
      componentInChildren.text = string.Format((string) STRINGS.UI.PRODUCTINFO_SELECTMATERIAL, (object) this.activeIngredient.tag.ProperName());
      this.NoMaterialDiscovered.gameObject.SetActive(false);
      this.BadBG.SetActive(false);
      this.LayoutContainer.SetActive(true);
      this.UpdateScrollBar();
    }
  }

  public void ToggleShowDescriptorsPanel(bool show)
  {
    this.DescriptorsPanel.gameObject.SetActive(show);
  }

  private void SetDescription(Tag element)
  {
    StringEntry result = (StringEntry) null;
    if (Strings.TryGet(new StringKey("STRINGS.ELEMENTS." + element.ToString().ToUpper() + ".BUILD_DESC"), out result))
    {
      this.MaterialDescriptionText.text = result.ToString();
      this.MaterialDescriptionPane.SetActive(true);
    }
    else
      this.MaterialDescriptionPane.SetActive(false);
  }

  private void SetEffects(Tag element)
  {
    List<Descriptor> materialDescriptors = GameUtil.GetMaterialDescriptors(element);
    if (materialDescriptors.Count > 0)
    {
      Descriptor descriptor = new Descriptor();
      descriptor.SetupDescriptor((string) ELEMENTS.MATERIAL_MODIFIERS.EFFECTS_HEADER, (string) ELEMENTS.MATERIAL_MODIFIERS.TOOLTIP.EFFECTS_HEADER, Descriptor.DescriptorType.Effect);
      materialDescriptors.Insert(0, descriptor);
      this.MaterialEffectsPane.gameObject.SetActive(true);
      this.MaterialEffectsPane.SetDescriptors((IList<Descriptor>) materialDescriptors);
    }
    else
      this.MaterialEffectsPane.gameObject.SetActive(false);
  }

  public static bool AllowInsufficientMaterialBuild()
  {
    return GenericGameSettings.instance.allowInsufficientMaterialBuild;
  }

  private int ElementSorter(Tag at, Tag bt)
  {
    GameObject prefab1 = Assets.TryGetPrefab(at);
    IHasSortOrder hasSortOrder1 = !((UnityEngine.Object) prefab1 != (UnityEngine.Object) null) ? (IHasSortOrder) null : prefab1.GetComponent<IHasSortOrder>();
    GameObject prefab2 = Assets.TryGetPrefab(bt);
    IHasSortOrder hasSortOrder2 = !((UnityEngine.Object) prefab2 != (UnityEngine.Object) null) ? (IHasSortOrder) null : prefab2.GetComponent<IHasSortOrder>();
    if (hasSortOrder1 == null || hasSortOrder2 == null)
      return 0;
    Element element1 = ElementLoader.GetElement(at);
    Element element2 = ElementLoader.GetElement(bt);
    if (element1 != null && element2 != null && element1.buildMenuSort == element2.buildMenuSort)
      return element1.idx.CompareTo(element2.idx);
    return hasSortOrder1.sortOrder.CompareTo(hasSortOrder2.sortOrder);
  }

  public delegate void SelectMaterialActions();
}
