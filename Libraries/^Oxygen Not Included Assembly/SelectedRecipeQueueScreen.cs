// Decompiled with JetBrains decompiler
// Type: SelectedRecipeQueueScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedRecipeQueueScreen : KScreen
{
  public Image recipeIcon;
  public LocText recipeName;
  public DescriptorPanel IngredientsDescriptorPanel;
  public DescriptorPanel EffectsDescriptorPanel;
  public KNumberInputField QueueCount;
  public MultiToggle DecrementButton;
  public MultiToggle IncrementButton;
  public KButton InfiniteButton;
  public GameObject InfiniteIcon;
  private ComplexFabricator target;
  private ComplexFabricatorSideScreen ownerScreen;
  private ComplexRecipe selectedRecipe;
  private bool isEditing;

  public override float GetSortKey()
  {
    if (this.isEditing)
      return 100f;
    return base.GetSortKey();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.DecrementButton.onClick = (System.Action) (() =>
    {
      this.target.DecrementRecipeQueueCount(this.selectedRecipe, false);
      this.RefreshQueueCountDisplay();
      this.ownerScreen.RefreshQueueCountDisplayForRecipe(this.selectedRecipe, this.target);
    });
    this.IncrementButton.onClick = (System.Action) (() =>
    {
      this.target.IncrementRecipeQueueCount(this.selectedRecipe);
      this.RefreshQueueCountDisplay();
      this.ownerScreen.RefreshQueueCountDisplayForRecipe(this.selectedRecipe, this.target);
    });
    this.InfiniteButton.onClick += (System.Action) (() =>
    {
      if (this.target.GetRecipeQueueCount(this.selectedRecipe) != ComplexFabricator.QUEUE_INFINITE)
        this.target.SetRecipeQueueCount(this.selectedRecipe, ComplexFabricator.QUEUE_INFINITE);
      else
        this.target.SetRecipeQueueCount(this.selectedRecipe, 0);
      this.RefreshQueueCountDisplay();
      this.ownerScreen.RefreshQueueCountDisplayForRecipe(this.selectedRecipe, this.target);
    });
    this.QueueCount.onEndEdit += (System.Action) (() =>
    {
      this.isEditing = false;
      this.target.SetRecipeQueueCount(this.selectedRecipe, Mathf.RoundToInt(this.QueueCount.currentValue));
      this.RefreshQueueCountDisplay();
      this.ownerScreen.RefreshQueueCountDisplayForRecipe(this.selectedRecipe, this.target);
    });
    this.QueueCount.onStartEdit += (System.Action) (() =>
    {
      this.isEditing = true;
      KScreenManager.Instance.RefreshStack();
    });
  }

  public void SetRecipe(
    ComplexFabricatorSideScreen owner,
    ComplexFabricator target,
    ComplexRecipe recipe)
  {
    this.ownerScreen = owner;
    this.target = target;
    this.selectedRecipe = recipe;
    this.recipeName.text = recipe.GetUIName();
    Tuple<Sprite, Color> uiSprite = Def.GetUISprite((object) (recipe.nameDisplay != ComplexRecipe.RecipeNameDisplay.Ingredient ? recipe.results[0].material : recipe.ingredients[0].material), "ui", false);
    this.recipeIcon.sprite = uiSprite.first;
    this.recipeIcon.color = uiSprite.second;
    this.RefreshIngredientDescriptors();
    this.RefreshResultDescriptors();
    this.RefreshQueueCountDisplay();
  }

  private void RefreshQueueCountDisplay()
  {
    bool flag = this.target.GetRecipeQueueCount(this.selectedRecipe) == ComplexFabricator.QUEUE_INFINITE;
    if (!flag)
      this.QueueCount.SetAmount((float) this.target.GetRecipeQueueCount(this.selectedRecipe));
    else
      this.QueueCount.SetDisplayValue(string.Empty);
    this.InfiniteIcon.gameObject.SetActive(flag);
  }

  private void RefreshResultDescriptors()
  {
    List<Descriptor> list = new List<Descriptor>();
    list.AddRange((IEnumerable<Descriptor>) this.GetResultDescriptions(this.selectedRecipe));
    list.AddRange((IEnumerable<Descriptor>) this.target.AdditionalEffectsForRecipe(this.selectedRecipe));
    if (list.Count <= 0)
      return;
    GameUtil.IndentListOfDescriptors(list, 1);
    list.Insert(0, new Descriptor((string) STRINGS.UI.UISIDESCREENS.FABRICATORSIDESCREEN.RESULTEFFECTS, (string) STRINGS.UI.UISIDESCREENS.FABRICATORSIDESCREEN.RESULTEFFECTS, Descriptor.DescriptorType.Effect, false));
    this.EffectsDescriptorPanel.gameObject.SetActive(true);
    this.EffectsDescriptorPanel.SetDescriptors((IList<Descriptor>) list);
  }

  public List<Descriptor> GetResultDescriptions(ComplexRecipe recipe)
  {
    List<Descriptor> descriptorList = new List<Descriptor>();
    foreach (ComplexRecipe.RecipeElement result in recipe.results)
    {
      GameObject prefab = Assets.GetPrefab(result.material);
      string formattedByTag = GameUtil.GetFormattedByTag(result.material, result.amount, GameUtil.TimeSlice.None);
      descriptorList.Add(new Descriptor(string.Format((string) STRINGS.UI.UISIDESCREENS.FABRICATORSIDESCREEN.RECIPEPRODUCT, (object) prefab.GetProperName(), (object) formattedByTag), string.Format((string) STRINGS.UI.UISIDESCREENS.FABRICATORSIDESCREEN.TOOLTIPS.RECIPEPRODUCT, (object) prefab.GetProperName(), (object) formattedByTag), Descriptor.DescriptorType.Requirement, false));
      Element element = ElementLoader.GetElement(result.material);
      if (element != null)
      {
        List<Descriptor> materialDescriptors = GameUtil.GetMaterialDescriptors(element);
        GameUtil.IndentListOfDescriptors(materialDescriptors, 1);
        descriptorList.AddRange((IEnumerable<Descriptor>) materialDescriptors);
      }
      else
      {
        List<Descriptor> effectDescriptors = GameUtil.GetEffectDescriptors(GameUtil.GetAllDescriptors(prefab, false));
        GameUtil.IndentListOfDescriptors(effectDescriptors, 1);
        descriptorList.AddRange((IEnumerable<Descriptor>) effectDescriptors);
      }
    }
    return descriptorList;
  }

  private void RefreshIngredientDescriptors()
  {
    List<Descriptor> descriptorList = new List<Descriptor>();
    descriptorList.Add(new Descriptor((string) STRINGS.UI.UISIDESCREENS.FABRICATORSIDESCREEN.COST, (string) STRINGS.UI.UISIDESCREENS.FABRICATORSIDESCREEN.COST, Descriptor.DescriptorType.Requirement, false));
    List<Descriptor> ingredientDescriptions = this.GetIngredientDescriptions(this.selectedRecipe);
    GameUtil.IndentListOfDescriptors(ingredientDescriptions, 1);
    descriptorList.AddRange((IEnumerable<Descriptor>) ingredientDescriptions);
    this.IngredientsDescriptorPanel.gameObject.SetActive(true);
    this.IngredientsDescriptorPanel.SetDescriptors((IList<Descriptor>) descriptorList);
  }

  public List<Descriptor> GetIngredientDescriptions(ComplexRecipe recipe)
  {
    List<Descriptor> descriptorList = new List<Descriptor>();
    foreach (ComplexRecipe.RecipeElement ingredient in recipe.ingredients)
    {
      GameObject prefab = Assets.GetPrefab(ingredient.material);
      string formattedByTag1 = GameUtil.GetFormattedByTag(ingredient.material, ingredient.amount, GameUtil.TimeSlice.None);
      string formattedByTag2 = GameUtil.GetFormattedByTag(ingredient.material, WorldInventory.Instance.GetAmount(ingredient.material), GameUtil.TimeSlice.None);
      string str = (double) WorldInventory.Instance.GetAmount(ingredient.material) < (double) ingredient.amount ? "<color=#F44A47>" + string.Format((string) STRINGS.UI.UISIDESCREENS.FABRICATORSIDESCREEN.RECIPERQUIREMENT, (object) prefab.GetProperName(), (object) formattedByTag1, (object) formattedByTag2) + "</color>" : string.Format((string) STRINGS.UI.UISIDESCREENS.FABRICATORSIDESCREEN.RECIPERQUIREMENT, (object) prefab.GetProperName(), (object) formattedByTag1, (object) formattedByTag2);
      descriptorList.Add(new Descriptor(str, str, Descriptor.DescriptorType.Requirement, false));
    }
    return descriptorList;
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (this.isEditing)
      e.Consumed = true;
    else
      base.OnKeyDown(e);
  }
}
