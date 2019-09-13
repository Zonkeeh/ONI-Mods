// Decompiled with JetBrains decompiler
// Type: ComplexRecipe
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

public class ComplexRecipe
{
  public string id;
  public ComplexRecipe.RecipeElement[] ingredients;
  public ComplexRecipe.RecipeElement[] results;
  public float time;
  public GameObject FabricationVisualizer;
  public ComplexRecipe.RecipeNameDisplay nameDisplay;
  public string description;
  public List<Tag> fabricators;
  public int sortOrder;
  public string requiredTech;

  public ComplexRecipe(
    string id,
    ComplexRecipe.RecipeElement[] ingredients,
    ComplexRecipe.RecipeElement[] results)
  {
    this.id = id;
    this.ingredients = ingredients;
    this.results = results;
    ComplexRecipeManager.Get().Add(this);
  }

  public Tag FirstResult
  {
    get
    {
      return this.results[0].material;
    }
  }

  public float TotalResultUnits()
  {
    float num = 0.0f;
    foreach (ComplexRecipe.RecipeElement result in this.results)
      num += result.amount;
    return num;
  }

  public bool RequiresTechUnlock()
  {
    return !string.IsNullOrEmpty(this.requiredTech);
  }

  public bool IsRequiredTechUnlocked()
  {
    if (string.IsNullOrEmpty(this.requiredTech))
      return true;
    return Db.Get().Techs.Get(this.requiredTech).IsComplete();
  }

  public Sprite GetUIIcon()
  {
    Sprite sprite = (Sprite) null;
    KBatchedAnimController component = Assets.GetPrefab(this.nameDisplay != ComplexRecipe.RecipeNameDisplay.Ingredient ? this.results[0].material : this.ingredients[0].material).GetComponent<KBatchedAnimController>();
    if ((Object) component != (Object) null)
      sprite = Def.GetUISpriteFromMultiObjectAnim(component.AnimFiles[0], "ui", false, string.Empty);
    return sprite;
  }

  public Color GetUIColor()
  {
    return Color.white;
  }

  public string GetUIName()
  {
    switch (this.nameDisplay)
    {
      case ComplexRecipe.RecipeNameDisplay.Result:
        return this.results[0].material.ProperName();
      case ComplexRecipe.RecipeNameDisplay.IngredientToResult:
        return string.Format((string) UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_FROM_TO, (object) this.ingredients[0].material.ProperName(), (object) this.results[0].material.ProperName());
      case ComplexRecipe.RecipeNameDisplay.ResultWithIngredient:
        return string.Format((string) UI.UISIDESCREENS.REFINERYSIDESCREEN.RECIPE_WITH, (object) this.ingredients[0].material.ProperName(), (object) this.results[0].material.ProperName());
      default:
        return this.ingredients[0].material.ProperName();
    }
  }

  public enum RecipeNameDisplay
  {
    Ingredient,
    Result,
    IngredientToResult,
    ResultWithIngredient,
  }

  public class RecipeElement
  {
    public Tag material;

    public RecipeElement(Tag material, float amount)
    {
      this.material = material;
      this.amount = amount;
    }

    public float amount { get; private set; }
  }
}
