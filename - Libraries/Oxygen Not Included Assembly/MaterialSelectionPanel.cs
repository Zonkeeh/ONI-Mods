// Decompiled with JetBrains decompiler
// Type: MaterialSelectionPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MaterialSelectionPanel : KScreen
{
  private static Dictionary<Tag, List<Tag>> elementsWithTag = new Dictionary<Tag, List<Tag>>();
  public Dictionary<KToggle, Tag> ElementToggles = new Dictionary<KToggle, Tag>();
  private List<MaterialSelector> MaterialSelectors = new List<MaterialSelector>();
  private List<Tag> currentSelectedElements = new List<Tag>();
  [SerializeField]
  protected PriorityScreen priorityScreenPrefab;
  [SerializeField]
  protected GameObject priorityScreenParent;
  private PriorityScreen priorityScreen;
  public GameObject MaterialSelectorTemplate;
  public GameObject ResearchRequired;
  private Recipe activeRecipe;

  public static void ClearStatics()
  {
    MaterialSelectionPanel.elementsWithTag.Clear();
  }

  public Tag CurrentSelectedElement
  {
    get
    {
      return this.MaterialSelectors[0].CurrentSelectedElement;
    }
  }

  public IList<Tag> GetSelectedElementAsList
  {
    get
    {
      this.currentSelectedElements.Clear();
      foreach (MaterialSelector materialSelector in this.MaterialSelectors)
      {
        if (materialSelector.gameObject.activeSelf)
        {
          Debug.Assert(materialSelector.CurrentSelectedElement != (Tag) ((string) null));
          this.currentSelectedElements.Add(materialSelector.CurrentSelectedElement);
        }
      }
      return (IList<Tag>) this.currentSelectedElements;
    }
  }

  public PriorityScreen PriorityScreen
  {
    get
    {
      return this.priorityScreen;
    }
  }

  protected override void OnPrefabInit()
  {
    MaterialSelectionPanel.elementsWithTag.Clear();
    base.OnPrefabInit();
    this.ConsumeMouseScroll = true;
    for (int index = 0; index < 3; ++index)
    {
      MaterialSelector materialSelector = Util.KInstantiateUI<MaterialSelector>(this.MaterialSelectorTemplate, this.gameObject, false);
      materialSelector.selectorIndex = index;
      this.MaterialSelectors.Add(materialSelector);
    }
    this.MaterialSelectors[0].gameObject.SetActive(true);
    this.MaterialSelectorTemplate.SetActive(false);
    this.ResearchRequired.SetActive(false);
    this.priorityScreen = Util.KInstantiateUI<PriorityScreen>(this.priorityScreenPrefab.gameObject, this.priorityScreenParent, false);
    this.priorityScreen.InstantiateButtons(new System.Action<PrioritySetting>(this.OnPriorityClicked), true);
    Game.Instance.Subscribe(-107300940, (System.Action<object>) (d => this.RefreshSelectors()));
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.activateOnSpawn = true;
  }

  public void AddSelectAction(MaterialSelector.SelectMaterialActions action)
  {
    this.MaterialSelectors.ForEach((System.Action<MaterialSelector>) (selector => selector.selectMaterialActions += action));
  }

  public void ClearSelectActions()
  {
    this.MaterialSelectors.ForEach((System.Action<MaterialSelector>) (selector => selector.selectMaterialActions = (MaterialSelector.SelectMaterialActions) null));
  }

  public void ClearMaterialToggles()
  {
    this.MaterialSelectors.ForEach((System.Action<MaterialSelector>) (selector => selector.ClearMaterialToggles()));
  }

  public void ConfigureScreen(Recipe recipe)
  {
    this.activeRecipe = recipe;
    this.RefreshSelectors();
  }

  public bool AllSelectorsSelected()
  {
    foreach (MaterialSelector materialSelector in this.MaterialSelectors)
    {
      if (materialSelector.gameObject.activeInHierarchy && materialSelector.CurrentSelectedElement == (Tag) ((string) null))
        return false;
    }
    return true;
  }

  public void RefreshSelectors()
  {
    if (this.activeRecipe == null)
      return;
    this.MaterialSelectors.ForEach((System.Action<MaterialSelector>) (selector => selector.gameObject.SetActive(false)));
    TechItem techItem = Db.Get().TechItems.TryGet(this.activeRecipe.GetBuildingDef().PrefabID);
    if (!DebugHandler.InstantBuildMode && !Game.Instance.SandboxModeActive && techItem != null && !techItem.IsComplete())
    {
      this.ResearchRequired.SetActive(true);
      LocText[] componentsInChildren = this.ResearchRequired.GetComponentsInChildren<LocText>();
      componentsInChildren[0].text = (string) UI.PRODUCTINFO_RESEARCHREQUIRED;
      componentsInChildren[1].text = string.Format((string) UI.PRODUCTINFO_REQUIRESRESEARCHDESC, (object) techItem.parentTech.Name);
      componentsInChildren[1].color = Constants.NEGATIVE_COLOR;
      this.priorityScreen.gameObject.SetActive(false);
    }
    else
    {
      this.ResearchRequired.SetActive(false);
      for (int index = 0; index < this.activeRecipe.Ingredients.Count; ++index)
      {
        this.MaterialSelectors[index].gameObject.SetActive(true);
        this.MaterialSelectors[index].ConfigureScreen(this.activeRecipe.Ingredients[index], this.activeRecipe);
      }
      this.priorityScreen.gameObject.SetActive(true);
      this.priorityScreen.gameObject.transform.SetAsLastSibling();
    }
  }

  public void UpdateResourceToggleValues()
  {
    this.MaterialSelectors.ForEach((System.Action<MaterialSelector>) (selector =>
    {
      if (!selector.gameObject.activeSelf)
        return;
      selector.RefreshToggleContents();
    }));
  }

  public bool AutoSelectAvailableMaterial()
  {
    bool flag = true;
    for (int index = 0; index < this.MaterialSelectors.Count; ++index)
    {
      if (!this.MaterialSelectors[index].AutoSelectAvailableMaterial())
        flag = false;
    }
    return flag;
  }

  public void SelectSourcesMaterials(Building building)
  {
    Tag[] tagArray = (Tag[]) null;
    Deconstructable component1 = building.gameObject.GetComponent<Deconstructable>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
      tagArray = component1.constructionElements;
    Constructable component2 = building.GetComponent<Constructable>();
    if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
      tagArray = component2.SelectedElementsTags.ToArray<Tag>();
    if (tagArray == null)
      return;
    for (int index = 0; index < Mathf.Min(tagArray.Length, this.MaterialSelectors.Count); ++index)
    {
      if (this.MaterialSelectors[index].ElementToggles.ContainsKey(tagArray[index]))
        this.MaterialSelectors[index].OnSelectMaterial(tagArray[index], this.activeRecipe, false);
    }
  }

  public bool CanBuild(Recipe recipe)
  {
    foreach (MaterialSelector materialSelector in this.MaterialSelectors)
    {
      if (materialSelector.gameObject.activeSelf && materialSelector.CurrentSelectedElement == (Tag) ((string) null))
        return false;
    }
    return true;
  }

  public static MaterialSelectionPanel.SelectedElemInfo Filter(
    Tag materialCategoryTag)
  {
    MaterialSelectionPanel.SelectedElemInfo selectedElemInfo = new MaterialSelectionPanel.SelectedElemInfo();
    selectedElemInfo.element = (Tag) ((string) null);
    selectedElemInfo.kgAvailable = 0.0f;
    if ((UnityEngine.Object) WorldInventory.Instance == (UnityEngine.Object) null || ElementLoader.elements == null || ElementLoader.elements.Count == 0)
      return selectedElemInfo;
    List<Tag> tagList = (List<Tag>) null;
    if (!MaterialSelectionPanel.elementsWithTag.TryGetValue(materialCategoryTag, out tagList))
    {
      tagList = new List<Tag>();
      foreach (Element element in ElementLoader.elements)
      {
        if (element.tag == materialCategoryTag || element.HasTag(materialCategoryTag))
          tagList.Add(element.tag);
      }
      foreach (Tag materialBuildingElement in GameTags.MaterialBuildingElements)
      {
        if (materialBuildingElement == materialCategoryTag)
        {
          foreach (GameObject gameObject in Assets.GetPrefabsWithTag(materialBuildingElement))
          {
            KPrefabID component = gameObject.GetComponent<KPrefabID>();
            if ((UnityEngine.Object) component != (UnityEngine.Object) null && !tagList.Contains(component.PrefabTag))
              tagList.Add(component.PrefabTag);
          }
        }
      }
      MaterialSelectionPanel.elementsWithTag[materialCategoryTag] = tagList;
    }
    foreach (Tag tag in tagList)
    {
      float amount = WorldInventory.Instance.GetAmount(tag);
      if ((double) amount > (double) selectedElemInfo.kgAvailable)
      {
        selectedElemInfo.kgAvailable = amount;
        selectedElemInfo.element = tag;
      }
    }
    return selectedElemInfo;
  }

  public void ToggleShowDescriptorPanels(bool show)
  {
    for (int index = 0; index < this.MaterialSelectors.Count; ++index)
    {
      if ((UnityEngine.Object) this.MaterialSelectors[index] != (UnityEngine.Object) null)
        this.MaterialSelectors[index].ToggleShowDescriptorsPanel(show);
    }
  }

  private void OnPriorityClicked(PrioritySetting priority)
  {
    this.priorityScreen.SetScreenPriority(priority, false);
  }

  public delegate void SelectElement(Element element, float kgAvailable, float recipe_amount);

  public struct SelectedElemInfo
  {
    public Tag element;
    public float kgAvailable;
  }
}
