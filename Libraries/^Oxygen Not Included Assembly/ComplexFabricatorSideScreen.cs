// Decompiled with JetBrains decompiler
// Type: ComplexFabricatorSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using FMODUnity;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComplexFabricatorSideScreen : SideScreenContent
{
  private Dictionary<ComplexFabricator, int> selectedRecipeFabricatorMap = new Dictionary<ComplexFabricator, int>();
  private List<GameObject> recipeToggles = new List<GameObject>();
  private int targetOrdersUpdatedSubHandle = -1;
  [Header("Recipe List")]
  [SerializeField]
  private GameObject recipeGrid;
  [Header("Recipe button variants")]
  [SerializeField]
  private GameObject recipeButton;
  [SerializeField]
  private GameObject recipeButtonMultiple;
  [SerializeField]
  private GameObject recipeButtonQueueHybrid;
  [SerializeField]
  private Sprite buttonSelectedBG;
  [SerializeField]
  private Sprite buttonNormalBG;
  [SerializeField]
  private Sprite elementPlaceholderSpr;
  private KToggle selectedToggle;
  public LayoutElement buttonScrollContainer;
  public RectTransform buttonContentContainer;
  [SerializeField]
  private GameObject elementContainer;
  [SerializeField]
  private LocText currentOrderLabel;
  [SerializeField]
  private LocText nextOrderLabel;
  [EventRef]
  public string createOrderSound;
  [SerializeField]
  private RectTransform content;
  [SerializeField]
  private LocText subtitleLabel;
  [SerializeField]
  private LocText noRecipesDiscoveredLabel;
  public ScriptableObject styleTooltipHeader;
  public ScriptableObject styleTooltipBody;
  private ComplexFabricator targetFab;
  private ComplexRecipe selectedRecipe;
  private Dictionary<GameObject, ComplexRecipe> recipeMap;
  public SelectedRecipeQueueScreen recipeScreenPrefab;
  private SelectedRecipeQueueScreen recipeScreen;

  public override string GetTitle()
  {
    if ((UnityEngine.Object) this.targetFab == (UnityEngine.Object) null)
      return Strings.Get(this.titleKey).ToString().Replace("{0}", string.Empty);
    return string.Format((string) Strings.Get(this.titleKey), (object) this.targetFab.GetProperName());
  }

  public override bool IsValidForTarget(GameObject target)
  {
    return (UnityEngine.Object) target.GetComponent<ComplexFabricator>() != (UnityEngine.Object) null;
  }

  public override void SetTarget(GameObject target)
  {
    ComplexFabricator component = target.GetComponent<ComplexFabricator>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "The object selected doesn't have a ComplexFabricator!");
    }
    else
    {
      if (this.targetOrdersUpdatedSubHandle != -1)
        this.Unsubscribe(this.targetOrdersUpdatedSubHandle);
      this.Initialize(component);
      this.targetOrdersUpdatedSubHandle = this.targetFab.Subscribe(1721324763, new System.Action<object>(this.UpdateQueueCountLabels));
      this.UpdateQueueCountLabels((object) null);
    }
  }

  private void UpdateQueueCountLabels(object data = null)
  {
    foreach (ComplexRecipe recipe in this.targetFab.GetRecipes())
    {
      ComplexRecipe r = recipe;
      ComplexFabricatorSideScreen fabricatorSideScreen = this;
      GameObject entryGO = this.recipeToggles.Find((Predicate<GameObject>) (match => fabricatorSideScreen.recipeMap[match] == r));
      if ((UnityEngine.Object) entryGO != (UnityEngine.Object) null)
        this.RefreshQueueCountDisplay(entryGO, this.targetFab);
    }
    if (this.targetFab.CurrentWorkingOrder != null)
      this.currentOrderLabel.text = string.Format((string) STRINGS.UI.UISIDESCREENS.FABRICATORSIDESCREEN.CURRENT_ORDER, (object) this.targetFab.CurrentWorkingOrder.GetUIName());
    else
      this.currentOrderLabel.text = string.Format((string) STRINGS.UI.UISIDESCREENS.FABRICATORSIDESCREEN.CURRENT_ORDER, (object) STRINGS.UI.UISIDESCREENS.FABRICATORSIDESCREEN.NO_WORKABLE_ORDER);
    if (this.targetFab.NextOrder != null)
      this.nextOrderLabel.text = string.Format((string) STRINGS.UI.UISIDESCREENS.FABRICATORSIDESCREEN.NEXT_ORDER, (object) this.targetFab.NextOrder.GetUIName());
    else
      this.nextOrderLabel.text = string.Format((string) STRINGS.UI.UISIDESCREENS.FABRICATORSIDESCREEN.NEXT_ORDER, (object) STRINGS.UI.UISIDESCREENS.FABRICATORSIDESCREEN.NO_WORKABLE_ORDER);
  }

  protected override void OnShow(bool show)
  {
    if (show)
    {
      AudioMixer.instance.Start(AudioMixerSnapshots.Get().FabricatorSideScreenOpenSnapshot);
    }
    else
    {
      AudioMixer.instance.Stop((HashedString) AudioMixerSnapshots.Get().FabricatorSideScreenOpenSnapshot, STOP_MODE.ALLOWFADEOUT);
      DetailsScreen.Instance.ClearSecondarySideScreen();
      this.selectedRecipe = (ComplexRecipe) null;
      this.selectedToggle = (KToggle) null;
    }
    base.OnShow(show);
  }

  public void Initialize(ComplexFabricator target)
  {
    if ((UnityEngine.Object) target == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "ComplexFabricator provided was null.");
    }
    else
    {
      this.targetFab = target;
      this.gameObject.SetActive(true);
      this.recipeMap = new Dictionary<GameObject, ComplexRecipe>();
      this.recipeToggles.ForEach((System.Action<GameObject>) (rbi => UnityEngine.Object.Destroy((UnityEngine.Object) rbi.gameObject)));
      this.recipeToggles.Clear();
      GridLayoutGroup component1 = this.recipeGrid.GetComponent<GridLayoutGroup>();
      switch (this.targetFab.sideScreenStyle)
      {
        case ComplexFabricatorSideScreen.StyleSetting.ListResult:
        case ComplexFabricatorSideScreen.StyleSetting.ListInput:
        case ComplexFabricatorSideScreen.StyleSetting.ListInputOutput:
          component1.constraintCount = 1;
          component1.cellSize = new Vector2(262f, component1.cellSize.y);
          break;
        case ComplexFabricatorSideScreen.StyleSetting.ClassicFabricator:
          component1.constraintCount = 128;
          component1.cellSize = new Vector2(78f, 96f);
          this.buttonScrollContainer.minHeight = 100f;
          break;
        case ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid:
          component1.constraintCount = 1;
          component1.cellSize = new Vector2(264f, 64f);
          this.buttonScrollContainer.minHeight = 66f;
          break;
        default:
          component1.constraintCount = 3;
          component1.cellSize = new Vector2(116f, component1.cellSize.y);
          break;
      }
      int num = 0;
      foreach (ComplexRecipe recipe1 in this.targetFab.GetRecipes())
      {
        ComplexRecipe recipe = recipe1;
        bool flag1 = false;
        if (DebugHandler.InstantBuildMode)
          flag1 = true;
        else if (recipe.RequiresTechUnlock() && recipe.IsRequiredTechUnlocked())
          flag1 = true;
        else if (target.GetRecipeQueueCount(recipe) != 0)
          flag1 = true;
        else if (this.AnyRecipeRequirementsDiscovered(recipe))
          flag1 = true;
        else if (this.HasAnyRecipeRequirements(recipe))
          flag1 = true;
        if (flag1)
        {
          ++num;
          Tuple<Sprite, Color> uiSprite1 = Def.GetUISprite((object) recipe.ingredients[0].material, "ui", false);
          Tuple<Sprite, Color> uiSprite2 = Def.GetUISprite((object) recipe.results[0].material, "ui", false);
          KToggle newToggle = (KToggle) null;
          GameObject entryGO;
          switch (target.sideScreenStyle)
          {
            case ComplexFabricatorSideScreen.StyleSetting.ListInputOutput:
            case ComplexFabricatorSideScreen.StyleSetting.GridInputOutput:
              newToggle = Util.KInstantiateUI<KToggle>(this.recipeButtonMultiple, this.recipeGrid, false);
              entryGO = newToggle.gameObject;
              HierarchyReferences component2 = newToggle.GetComponent<HierarchyReferences>();
              foreach (ComplexRecipe.RecipeElement ingredient in recipe.ingredients)
              {
                GameObject gameObject = Util.KInstantiateUI(component2.GetReference("FromIconPrefab").gameObject, component2.GetReference("FromIcons").gameObject, true);
                gameObject.GetComponent<Image>().sprite = Def.GetUISprite((object) ingredient.material, "ui", false).first;
                gameObject.GetComponent<Image>().color = Def.GetUISprite((object) ingredient.material, "ui", false).second;
                gameObject.gameObject.name = ingredient.material.Name;
              }
              foreach (ComplexRecipe.RecipeElement result in recipe.results)
              {
                GameObject gameObject = Util.KInstantiateUI(component2.GetReference("ToIconPrefab").gameObject, component2.GetReference("ToIcons").gameObject, true);
                gameObject.GetComponent<Image>().sprite = Def.GetUISprite((object) result.material, "ui", false).first;
                gameObject.GetComponent<Image>().color = Def.GetUISprite((object) result.material, "ui", false).second;
                gameObject.gameObject.name = result.material.Name;
              }
              break;
            case ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid:
              newToggle = Util.KInstantiateUI<KToggle>(this.recipeButtonQueueHybrid, this.recipeGrid, false);
              entryGO = newToggle.gameObject;
              this.recipeMap.Add(entryGO, recipe);
              Image image = entryGO.GetComponentsInChildrenOnly<Image>()[2];
              if (recipe.nameDisplay == ComplexRecipe.RecipeNameDisplay.Ingredient)
              {
                image.sprite = uiSprite1.first;
                image.color = uiSprite1.second;
              }
              else
              {
                image.sprite = uiSprite2.first;
                image.color = uiSprite2.second;
              }
              entryGO.GetComponentInChildren<LocText>().text = recipe.GetUIName();
              bool flag2 = this.HasAllRecipeRequirements(recipe);
              image.material = !flag2 ? Assets.UIPrefabs.TableScreenWidgets.DesaturatedUIMaterial : Assets.UIPrefabs.TableScreenWidgets.DefaultUIMaterial;
              this.RefreshQueueCountDisplay(entryGO, this.targetFab);
              entryGO.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("DecrementButton").onClick = (System.Action) (() =>
              {
                target.DecrementRecipeQueueCount(recipe, false);
                this.RefreshQueueCountDisplay(entryGO, target);
              });
              entryGO.GetComponent<HierarchyReferences>().GetReference<MultiToggle>("IncrementButton").onClick = (System.Action) (() =>
              {
                target.IncrementRecipeQueueCount(recipe);
                this.RefreshQueueCountDisplay(entryGO, target);
              });
              entryGO.gameObject.SetActive(true);
              break;
            default:
              newToggle = Util.KInstantiateUI<KToggle>(this.recipeButton, this.recipeGrid, false);
              entryGO = newToggle.gameObject;
              Image componentInChildrenOnly = newToggle.gameObject.GetComponentInChildrenOnly<Image>();
              if (target.sideScreenStyle == ComplexFabricatorSideScreen.StyleSetting.GridInput || target.sideScreenStyle == ComplexFabricatorSideScreen.StyleSetting.ListInput)
              {
                componentInChildrenOnly.sprite = uiSprite1.first;
                componentInChildrenOnly.color = uiSprite1.second;
                break;
              }
              componentInChildrenOnly.sprite = uiSprite2.first;
              componentInChildrenOnly.color = uiSprite2.second;
              break;
          }
          if (this.targetFab.sideScreenStyle == ComplexFabricatorSideScreen.StyleSetting.ClassicFabricator)
            newToggle.GetComponentInChildren<LocText>().text = recipe.results[0].material.ProperName();
          else if (this.targetFab.sideScreenStyle != ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid)
            newToggle.GetComponentInChildren<LocText>().text = string.Format((string) STRINGS.UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_FROM_TO_WITH_NEWLINES, (object) recipe.ingredients[0].material.ProperName(), (object) recipe.results[0].material.ProperName());
          ToolTip component3 = entryGO.GetComponent<ToolTip>();
          component3.toolTipPosition = ToolTip.TooltipPosition.Custom;
          component3.parentPositionAnchor = new Vector2(0.0f, 0.5f);
          component3.tooltipPivot = new Vector2(1f, 1f);
          component3.tooltipPositionOffset = new Vector2(-24f, 20f);
          component3.ClearMultiStringTooltip();
          component3.AddMultiStringTooltip(recipe.GetUIName(), this.styleTooltipHeader);
          component3.AddMultiStringTooltip(recipe.description, this.styleTooltipBody);
          newToggle.onClick += (System.Action) (() => this.ToggleClicked(newToggle));
          entryGO.SetActive(true);
          this.recipeToggles.Add(entryGO);
        }
      }
      if (this.recipeToggles.Count > 0)
      {
        this.buttonScrollContainer.GetComponent<LayoutElement>().minHeight = Mathf.Min(451f, (float) (2.0 + (double) num * (double) this.recipeButtonQueueHybrid.rectTransform().sizeDelta.y));
        this.subtitleLabel.SetText((string) STRINGS.UI.UISIDESCREENS.FABRICATORSIDESCREEN.SUBTITLE);
        this.noRecipesDiscoveredLabel.gameObject.SetActive(false);
      }
      else
      {
        this.subtitleLabel.SetText((string) STRINGS.UI.UISIDESCREENS.FABRICATORSIDESCREEN.NORECIPEDISCOVERED);
        this.noRecipesDiscoveredLabel.SetText((string) STRINGS.UI.UISIDESCREENS.FABRICATORSIDESCREEN.NORECIPEDISCOVERED_BODY);
        this.noRecipesDiscoveredLabel.gameObject.SetActive(true);
        this.buttonScrollContainer.GetComponent<LayoutElement>().minHeight = this.noRecipesDiscoveredLabel.rectTransform.sizeDelta.y + 10f;
      }
      this.RefreshIngredientAvailabilityVis();
    }
  }

  public void RefreshQueueCountDisplayForRecipe(ComplexRecipe recipe, ComplexFabricator fabricator)
  {
    GameObject entryGO = this.recipeToggles.Find((Predicate<GameObject>) (match => this.recipeMap[match] == recipe));
    if (!((UnityEngine.Object) entryGO != (UnityEngine.Object) null))
      return;
    this.RefreshQueueCountDisplay(entryGO, fabricator);
  }

  private void RefreshQueueCountDisplay(GameObject entryGO, ComplexFabricator fabricator)
  {
    HierarchyReferences component = entryGO.GetComponent<HierarchyReferences>();
    bool flag = fabricator.GetRecipeQueueCount(this.recipeMap[entryGO]) == ComplexFabricator.QUEUE_INFINITE;
    component.GetReference<LocText>("CountLabel").text = !flag ? fabricator.GetRecipeQueueCount(this.recipeMap[entryGO]).ToString() : string.Empty;
    component.GetReference<RectTransform>("InfiniteIcon").gameObject.SetActive(flag);
  }

  private void ToggleClicked(KToggle toggle)
  {
    if (!this.recipeMap.ContainsKey(toggle.gameObject))
    {
      Debug.LogError((object) "Recipe not found on recipe list.");
    }
    else
    {
      if ((UnityEngine.Object) this.selectedToggle == (UnityEngine.Object) toggle)
      {
        this.selectedToggle.isOn = false;
        this.selectedToggle = (KToggle) null;
        this.selectedRecipe = (ComplexRecipe) null;
      }
      else
      {
        this.selectedToggle = toggle;
        this.selectedToggle.isOn = true;
        this.selectedRecipe = this.recipeMap[toggle.gameObject];
        this.selectedRecipeFabricatorMap[this.targetFab] = this.recipeToggles.IndexOf(toggle.gameObject);
      }
      this.RefreshIngredientAvailabilityVis();
      if (toggle.isOn)
      {
        this.recipeScreen = (SelectedRecipeQueueScreen) DetailsScreen.Instance.SetSecondarySideScreen((KScreen) this.recipeScreenPrefab, (string) STRINGS.UI.UISIDESCREENS.FABRICATORSIDESCREEN.RECIPE_DETAILS);
        this.recipeScreen.SetRecipe(this, this.targetFab, this.selectedRecipe);
      }
      else
        DetailsScreen.Instance.ClearSecondarySideScreen();
    }
  }

  private bool HasAnyRecipeRequirements(ComplexRecipe recipe)
  {
    foreach (ComplexRecipe.RecipeElement ingredient in recipe.ingredients)
    {
      if ((double) WorldInventory.Instance.GetAmount(ingredient.material) + (double) this.targetFab.inStorage.GetAmountAvailable(ingredient.material) + (double) this.targetFab.buildStorage.GetAmountAvailable(ingredient.material) >= (double) ingredient.amount)
        return true;
    }
    return false;
  }

  private bool HasAllRecipeRequirements(ComplexRecipe recipe)
  {
    bool flag = true;
    foreach (ComplexRecipe.RecipeElement ingredient in recipe.ingredients)
    {
      if ((double) WorldInventory.Instance.GetAmount(ingredient.material) + (double) this.targetFab.inStorage.GetAmountAvailable(ingredient.material) + (double) this.targetFab.buildStorage.GetAmountAvailable(ingredient.material) < (double) ingredient.amount)
        flag = false;
    }
    return flag;
  }

  private bool AnyRecipeRequirementsDiscovered(ComplexRecipe recipe)
  {
    foreach (ComplexRecipe.RecipeElement ingredient in recipe.ingredients)
    {
      if (WorldInventory.Instance.IsDiscovered(ingredient.material))
        return true;
    }
    return false;
  }

  private void Update()
  {
    this.RefreshIngredientAvailabilityVis();
  }

  private void RefreshIngredientAvailabilityVis()
  {
    foreach (KeyValuePair<GameObject, ComplexRecipe> recipe in this.recipeMap)
    {
      HierarchyReferences component1 = recipe.Key.GetComponent<HierarchyReferences>();
      bool flag = this.HasAllRecipeRequirements(recipe.Value);
      KToggle component2 = recipe.Key.GetComponent<KToggle>();
      if (flag)
      {
        if (this.selectedRecipe == recipe.Value)
          component2.ActivateFlourish(true, ImageToggleState.State.Active);
        else
          component2.ActivateFlourish(false, ImageToggleState.State.Inactive);
      }
      else if (this.selectedRecipe == recipe.Value)
        component2.ActivateFlourish(true, ImageToggleState.State.DisabledActive);
      else
        component2.ActivateFlourish(false, ImageToggleState.State.Disabled);
      component1.GetReference<LocText>("Label").color = !flag ? new Color(0.22f, 0.22f, 0.22f, 1f) : Color.black;
    }
  }

  private Element[] GetRecipeElements(Recipe recipe)
  {
    Element[] elementArray = new Element[recipe.Ingredients.Count];
    for (int index = 0; index < recipe.Ingredients.Count; ++index)
    {
      Tag tag = recipe.Ingredients[index].tag;
      foreach (Element element in ElementLoader.elements)
      {
        if (GameTagExtensions.Create(element.id) == tag)
        {
          elementArray[index] = element;
          break;
        }
      }
    }
    return elementArray;
  }

  public enum StyleSetting
  {
    GridResult,
    ListResult,
    GridInput,
    ListInput,
    ListInputOutput,
    GridInputOutput,
    ClassicFabricator,
    ListQueueHybrid,
  }
}
