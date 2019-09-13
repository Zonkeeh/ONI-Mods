// Decompiled with JetBrains decompiler
// Type: ComplexRecipeManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Text;

public class ComplexRecipeManager
{
  public List<ComplexRecipe> recipes = new List<ComplexRecipe>();
  private Dictionary<string, string> obsoleteIDMapping = new Dictionary<string, string>();
  private static ComplexRecipeManager _Instance;

  public static ComplexRecipeManager Get()
  {
    if (ComplexRecipeManager._Instance == null)
      ComplexRecipeManager._Instance = new ComplexRecipeManager();
    return ComplexRecipeManager._Instance;
  }

  public static void DestroyInstance()
  {
    ComplexRecipeManager._Instance = (ComplexRecipeManager) null;
  }

  public static string MakeObsoleteRecipeID(string fabricator, Tag signatureElement)
  {
    return fabricator + "_" + (object) signatureElement;
  }

  public static string MakeRecipeID(
    string fabricator,
    IList<ComplexRecipe.RecipeElement> inputs,
    IList<ComplexRecipe.RecipeElement> outputs)
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(fabricator);
    stringBuilder.Append("_I");
    foreach (ComplexRecipe.RecipeElement input in (IEnumerable<ComplexRecipe.RecipeElement>) inputs)
    {
      stringBuilder.Append("_");
      stringBuilder.Append(input.material.ToString());
    }
    stringBuilder.Append("_O");
    foreach (ComplexRecipe.RecipeElement output in (IEnumerable<ComplexRecipe.RecipeElement>) outputs)
    {
      stringBuilder.Append("_");
      stringBuilder.Append(output.material.ToString());
    }
    return stringBuilder.ToString();
  }

  public void Add(ComplexRecipe recipe)
  {
    foreach (ComplexRecipe recipe1 in this.recipes)
    {
      if (recipe1.id == recipe.id)
        Debug.LogError((object) string.Format("DUPLICATE RECIPE ID! '{0}' is being added to the recipe manager multiple times. This will result in the failure to save/load certain queued recipes at fabricators.", (object) recipe.id));
    }
    this.recipes.Add(recipe);
    if (!((UnityEngine.Object) recipe.FabricationVisualizer != (UnityEngine.Object) null))
      return;
    UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) recipe.FabricationVisualizer);
  }

  public ComplexRecipe GetRecipe(string id)
  {
    if (string.IsNullOrEmpty(id))
      return (ComplexRecipe) null;
    return this.recipes.Find((Predicate<ComplexRecipe>) (r => r.id == id));
  }

  public void AddObsoleteIDMapping(string obsolete_id, string new_id)
  {
    this.obsoleteIDMapping[obsolete_id] = new_id;
  }

  public ComplexRecipe GetObsoleteRecipe(string id)
  {
    if (string.IsNullOrEmpty(id))
      return (ComplexRecipe) null;
    ComplexRecipe complexRecipe = (ComplexRecipe) null;
    string id1 = (string) null;
    if (this.obsoleteIDMapping.TryGetValue(id, out id1))
      complexRecipe = this.GetRecipe(id1);
    return complexRecipe;
  }
}
