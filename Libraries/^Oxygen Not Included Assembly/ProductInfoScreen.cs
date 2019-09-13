// Decompiled with JetBrains decompiler
// Type: ProductInfoScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ProductInfoScreen : KScreen
{
  private Dictionary<string, GameObject> descLabels = new Dictionary<string, GameObject>();
  private bool expandedInfo = true;
  public TitleBar titleBar;
  public GameObject ProductDescriptionPane;
  public LocText productDescriptionText;
  public DescriptorPanel ProductRequirementsPane;
  public DescriptorPanel ProductEffectsPane;
  public GameObject ProductFlavourPane;
  public LocText productFlavourText;
  public RectTransform BGPanel;
  public MaterialSelectionPanel materialSelectionPanelPrefab;
  public MultiToggle sandboxInstantBuildToggle;
  [NonSerialized]
  public MaterialSelectionPanel materialSelectionPanel;
  [NonSerialized]
  public BuildingDef currentDef;
  public System.Action onElementsFullySelected;
  private bool configuring;

  private void RefreshScreen()
  {
    if ((UnityEngine.Object) this.currentDef != (UnityEngine.Object) null)
      this.SetTitle(this.currentDef);
    else
      this.ClearProduct(true);
  }

  public void ClearProduct(bool deactivateTool = true)
  {
    this.currentDef = (BuildingDef) null;
    this.materialSelectionPanel.ClearMaterialToggles();
    if ((UnityEngine.Object) PlayerController.Instance.ActiveTool == (UnityEngine.Object) BuildTool.Instance && deactivateTool)
      BuildTool.Instance.Deactivate();
    if ((UnityEngine.Object) PlayerController.Instance.ActiveTool == (UnityEngine.Object) UtilityBuildTool.Instance || (UnityEngine.Object) PlayerController.Instance.ActiveTool == (UnityEngine.Object) WireBuildTool.Instance)
      ToolMenu.Instance.ClearSelection();
    this.ClearLabels();
    this.Show(false);
  }

  public new void Awake()
  {
    base.Awake();
    this.materialSelectionPanel = Util.KInstantiateUI<MaterialSelectionPanel>(this.materialSelectionPanelPrefab.gameObject, this.gameObject, false);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    if ((UnityEngine.Object) BuildingGroupScreen.Instance != (UnityEngine.Object) null)
    {
      BuildingGroupScreen instance1 = BuildingGroupScreen.Instance;
      instance1.pointerEnterActions = instance1.pointerEnterActions + new KScreen.PointerEnterActions(this.CheckMouseOver);
      BuildingGroupScreen instance2 = BuildingGroupScreen.Instance;
      instance2.pointerExitActions = instance2.pointerExitActions + new KScreen.PointerExitActions(this.CheckMouseOver);
    }
    if ((UnityEngine.Object) PlanScreen.Instance != (UnityEngine.Object) null)
    {
      PlanScreen instance1 = PlanScreen.Instance;
      instance1.pointerEnterActions = instance1.pointerEnterActions + new KScreen.PointerEnterActions(this.CheckMouseOver);
      PlanScreen instance2 = PlanScreen.Instance;
      instance2.pointerExitActions = instance2.pointerExitActions + new KScreen.PointerExitActions(this.CheckMouseOver);
    }
    if ((UnityEngine.Object) BuildMenu.Instance != (UnityEngine.Object) null)
    {
      BuildMenu instance1 = BuildMenu.Instance;
      instance1.pointerEnterActions = instance1.pointerEnterActions + new KScreen.PointerEnterActions(this.CheckMouseOver);
      BuildMenu instance2 = BuildMenu.Instance;
      instance2.pointerExitActions = instance2.pointerExitActions + new KScreen.PointerExitActions(this.CheckMouseOver);
    }
    ProductInfoScreen productInfoScreen1 = this;
    productInfoScreen1.pointerEnterActions = productInfoScreen1.pointerEnterActions + new KScreen.PointerEnterActions(this.CheckMouseOver);
    ProductInfoScreen productInfoScreen2 = this;
    productInfoScreen2.pointerExitActions = productInfoScreen2.pointerExitActions + new KScreen.PointerExitActions(this.CheckMouseOver);
    this.ConsumeMouseScroll = true;
    this.sandboxInstantBuildToggle.ChangeState(!SandboxToolParameterMenu.instance.settings.InstantBuild ? 0 : 1);
    this.sandboxInstantBuildToggle.onClick += (System.Action) (() =>
    {
      SandboxToolParameterMenu.instance.settings.InstantBuild = !SandboxToolParameterMenu.instance.settings.InstantBuild;
      this.sandboxInstantBuildToggle.ChangeState(!SandboxToolParameterMenu.instance.settings.InstantBuild ? 0 : 1);
    });
    this.sandboxInstantBuildToggle.gameObject.SetActive(Game.Instance.SandboxModeActive);
    Game.Instance.Subscribe(-1948169901, (System.Action<object>) (data => this.sandboxInstantBuildToggle.gameObject.SetActive(Game.Instance.SandboxModeActive)));
  }

  public void ConfigureScreen(BuildingDef def)
  {
    this.configuring = true;
    this.currentDef = def;
    this.SetTitle(def);
    this.SetDescription(def);
    this.SetEffects(def);
    this.SetMaterials(def);
    this.configuring = false;
  }

  private void ExpandInfo(PointerEventData data)
  {
    this.ToggleExpandedInfo(true);
  }

  private void CollapseInfo(PointerEventData data)
  {
    this.ToggleExpandedInfo(false);
  }

  public void ToggleExpandedInfo(bool state)
  {
    this.expandedInfo = state;
    if ((UnityEngine.Object) this.ProductDescriptionPane != (UnityEngine.Object) null)
      this.ProductDescriptionPane.SetActive(this.expandedInfo);
    if ((UnityEngine.Object) this.ProductRequirementsPane != (UnityEngine.Object) null)
      this.ProductRequirementsPane.gameObject.SetActive(this.expandedInfo && this.ProductRequirementsPane.HasDescriptors());
    if ((UnityEngine.Object) this.ProductEffectsPane != (UnityEngine.Object) null)
      this.ProductEffectsPane.gameObject.SetActive(this.expandedInfo && this.ProductEffectsPane.HasDescriptors());
    if ((UnityEngine.Object) this.ProductFlavourPane != (UnityEngine.Object) null)
      this.ProductFlavourPane.SetActive(this.expandedInfo);
    if (!((UnityEngine.Object) this.materialSelectionPanel != (UnityEngine.Object) null) || !(this.materialSelectionPanel.CurrentSelectedElement != (Tag) ((string) null)))
      return;
    this.materialSelectionPanel.ToggleShowDescriptorPanels(this.expandedInfo);
  }

  private void CheckMouseOver(PointerEventData data)
  {
    this.ToggleExpandedInfo(this.GetMouseOver || (UnityEngine.Object) PlanScreen.Instance != (UnityEngine.Object) null && (PlanScreen.Instance.isActiveAndEnabled && PlanScreen.Instance.GetMouseOver || BuildingGroupScreen.Instance.GetMouseOver) || (UnityEngine.Object) BuildMenu.Instance != (UnityEngine.Object) null && BuildMenu.Instance.isActiveAndEnabled && BuildMenu.Instance.GetMouseOver);
  }

  private void Update()
  {
    if (DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive || (!((UnityEngine.Object) this.currentDef != (UnityEngine.Object) null) || !(this.materialSelectionPanel.CurrentSelectedElement != (Tag) ((string) null))) || (MaterialSelector.AllowInsufficientMaterialBuild() || (double) this.currentDef.Mass[0] <= (double) WorldInventory.Instance.GetAmount(this.materialSelectionPanel.CurrentSelectedElement)))
      return;
    this.materialSelectionPanel.AutoSelectAvailableMaterial();
  }

  private void SetTitle(BuildingDef def)
  {
    this.titleBar.SetTitle(def.Name);
    this.titleBar.GetComponentInChildren<KImage>().ColorState = (!((UnityEngine.Object) PlanScreen.Instance != (UnityEngine.Object) null) || !PlanScreen.Instance.isActiveAndEnabled || PlanScreen.Instance.BuildableState(def) != PlanScreen.RequirementsState.Complete) && (!((UnityEngine.Object) BuildMenu.Instance != (UnityEngine.Object) null) || !BuildMenu.Instance.isActiveAndEnabled || BuildMenu.Instance.BuildableState(def) != PlanScreen.RequirementsState.Complete) ? KImage.ColorSelector.Disabled : KImage.ColorSelector.Active;
  }

  private void SetDescription(BuildingDef def)
  {
    if ((UnityEngine.Object) def == (UnityEngine.Object) null || (UnityEngine.Object) this.productFlavourText == (UnityEngine.Object) null)
      return;
    string str1 = def.Desc;
    Dictionary<Klei.AI.Attribute, float> dictionary1 = new Dictionary<Klei.AI.Attribute, float>();
    Dictionary<Klei.AI.Attribute, float> dictionary2 = new Dictionary<Klei.AI.Attribute, float>();
    foreach (Klei.AI.Attribute attribute in def.attributes)
    {
      if (!dictionary1.ContainsKey(attribute))
        dictionary1[attribute] = 0.0f;
    }
    foreach (AttributeModifier attributeModifier in def.attributeModifiers)
    {
      float num = 0.0f;
      Klei.AI.Attribute key = Db.Get().BuildingAttributes.Get(attributeModifier.AttributeId);
      dictionary1.TryGetValue(key, out num);
      num += attributeModifier.Value;
      dictionary1[key] = num;
    }
    if (this.materialSelectionPanel.CurrentSelectedElement != (Tag) ((string) null))
    {
      Element element = ElementLoader.GetElement(this.materialSelectionPanel.CurrentSelectedElement);
      if (element != null)
      {
        foreach (AttributeModifier attributeModifier in element.attributeModifiers)
        {
          float num = 0.0f;
          Klei.AI.Attribute key = Db.Get().BuildingAttributes.Get(attributeModifier.AttributeId);
          dictionary2.TryGetValue(key, out num);
          num += attributeModifier.Value;
          dictionary2[key] = num;
        }
      }
      else
      {
        PrefabAttributeModifiers component = Assets.TryGetPrefab(this.materialSelectionPanel.CurrentSelectedElement).GetComponent<PrefabAttributeModifiers>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        {
          foreach (AttributeModifier descriptor in component.descriptors)
          {
            float num = 0.0f;
            Klei.AI.Attribute key = Db.Get().BuildingAttributes.Get(descriptor.AttributeId);
            dictionary2.TryGetValue(key, out num);
            num += descriptor.Value;
            dictionary2[key] = num;
          }
        }
      }
    }
    if (dictionary1.Count > 0)
    {
      str1 += "\n\n";
      foreach (KeyValuePair<Klei.AI.Attribute, float> keyValuePair in dictionary1)
      {
        float num1 = 0.0f;
        dictionary1.TryGetValue(keyValuePair.Key, out num1);
        float num2 = 0.0f;
        string str2 = string.Empty;
        if (dictionary2.TryGetValue(keyValuePair.Key, out num2))
        {
          num2 = Mathf.Abs(num1 * num2);
          str2 = "(+" + (object) num2 + ")";
        }
        str1 = str1 + "\n" + keyValuePair.Key.Name + ": " + (object) (float) ((double) num1 + (double) num2) + str2;
      }
    }
    this.productFlavourText.text = str1;
  }

  private void SetEffects(BuildingDef def)
  {
    if (this.productDescriptionText.text != null)
      this.productDescriptionText.text = string.Format("{0}", (object) def.Effect);
    List<Descriptor> allDescriptors = GameUtil.GetAllDescriptors(def);
    List<Descriptor> requirementDescriptors = GameUtil.GetRequirementDescriptors(allDescriptors);
    if (requirementDescriptors.Count > 0)
    {
      Descriptor descriptor = new Descriptor();
      descriptor.SetupDescriptor((string) UI.BUILDINGEFFECTS.OPERATIONREQUIREMENTS, (string) UI.BUILDINGEFFECTS.TOOLTIPS.OPERATIONREQUIREMENTS, Descriptor.DescriptorType.Effect);
      requirementDescriptors.Insert(0, descriptor);
      this.ProductRequirementsPane.gameObject.SetActive(true);
    }
    else
      this.ProductRequirementsPane.gameObject.SetActive(false);
    this.ProductRequirementsPane.SetDescriptors((IList<Descriptor>) requirementDescriptors);
    List<Descriptor> effectDescriptors = GameUtil.GetEffectDescriptors(allDescriptors);
    if (effectDescriptors.Count > 0)
    {
      Descriptor descriptor = new Descriptor();
      descriptor.SetupDescriptor((string) UI.BUILDINGEFFECTS.OPERATIONEFFECTS, (string) UI.BUILDINGEFFECTS.TOOLTIPS.OPERATIONEFFECTS, Descriptor.DescriptorType.Effect);
      effectDescriptors.Insert(0, descriptor);
      this.ProductEffectsPane.gameObject.SetActive(true);
    }
    else
      this.ProductEffectsPane.gameObject.SetActive(false);
    this.ProductEffectsPane.SetDescriptors((IList<Descriptor>) effectDescriptors);
  }

  public void ClearLabels()
  {
    List<string> stringList = new List<string>((IEnumerable<string>) this.descLabels.Keys);
    if (stringList.Count <= 0)
      return;
    foreach (string key in stringList)
    {
      GameObject descLabel = this.descLabels[key];
      if ((UnityEngine.Object) descLabel != (UnityEngine.Object) null)
        UnityEngine.Object.Destroy((UnityEngine.Object) descLabel);
      this.descLabels.Remove(key);
    }
  }

  public void SetMaterials(BuildingDef def)
  {
    this.materialSelectionPanel.gameObject.SetActive(true);
    Recipe craftRecipe = def.CraftRecipe;
    this.materialSelectionPanel.ClearSelectActions();
    this.materialSelectionPanel.ConfigureScreen(craftRecipe);
    this.materialSelectionPanel.ToggleShowDescriptorPanels(false);
    this.materialSelectionPanel.AddSelectAction(new MaterialSelector.SelectMaterialActions(this.RefreshScreen));
    this.materialSelectionPanel.AddSelectAction(new MaterialSelector.SelectMaterialActions(this.onMenuMaterialChanged));
    this.materialSelectionPanel.AutoSelectAvailableMaterial();
    this.ActivateAppropriateTool(def);
  }

  private bool BuildRequirementsMet(BuildingDef def)
  {
    return DebugHandler.InstantBuildMode || Game.Instance.SandboxModeActive || this.materialSelectionPanel.CanBuild(def.CraftRecipe) && Db.Get().TechItems.IsTechItemComplete(def.PrefabID);
  }

  private void onMenuMaterialChanged()
  {
    if ((UnityEngine.Object) this.currentDef == (UnityEngine.Object) null)
      return;
    this.ActivateAppropriateTool(this.currentDef);
    this.SetDescription(this.currentDef);
  }

  private void ActivateAppropriateTool(BuildingDef def)
  {
    Debug.Assert((UnityEngine.Object) def != (UnityEngine.Object) null, (object) "def was null");
    if (this.materialSelectionPanel.AllSelectorsSelected() && this.BuildRequirementsMet(def))
    {
      this.onElementsFullySelected.Signal();
    }
    else
    {
      if (MaterialSelector.AllowInsufficientMaterialBuild() || DebugHandler.InstantBuildMode)
        return;
      if ((UnityEngine.Object) PlayerController.Instance.ActiveTool == (UnityEngine.Object) BuildTool.Instance)
        BuildTool.Instance.Deactivate();
      if ((UnityEngine.Object) PlanScreen.Instance != (UnityEngine.Object) null)
        PrebuildTool.Instance.Activate(def, PlanScreen.Instance.BuildableState(def));
      if (!((UnityEngine.Object) BuildMenu.Instance != (UnityEngine.Object) null))
        return;
      PrebuildTool.Instance.Activate(def, BuildMenu.Instance.BuildableState(def));
    }
  }

  public static bool MaterialsMet(Recipe recipe)
  {
    if (recipe == null)
    {
      Debug.LogError((object) "Trying to verify the materials on a null recipe!");
      return false;
    }
    if (recipe.Ingredients == null || recipe.Ingredients.Count == 0)
    {
      Debug.LogError((object) "Trying to verify the materials on a recipe with no MaterialCategoryTags!");
      return false;
    }
    for (int index = 0; index < recipe.Ingredients.Count; ++index)
    {
      if ((double) MaterialSelectionPanel.Filter(recipe.Ingredients[index].tag).kgAvailable < (double) recipe.Ingredients[index].amount)
        return false;
    }
    return true;
  }

  public void Close()
  {
    if (this.configuring)
      return;
    this.ClearProduct(true);
    this.Show(false);
  }
}
