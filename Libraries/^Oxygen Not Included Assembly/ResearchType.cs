// Decompiled with JetBrains decompiler
// Type: ResearchType
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class ResearchType
{
  private string _id;
  private string _name;
  private string _description;
  private Recipe _recipe;
  private Sprite _sprite;
  private Color _color;

  public ResearchType(
    string id,
    string name,
    string description,
    Sprite sprite,
    Color color,
    Recipe.Ingredient[] fabricationIngredients,
    float fabricationTime,
    HashedString kAnim_ID,
    string[] fabricators,
    string recipeDescription)
  {
    this._id = id;
    this._name = name;
    this._description = description;
    this._sprite = sprite;
    this._color = color;
    this.CreatePrefab(fabricationIngredients, fabricationTime, kAnim_ID, fabricators, recipeDescription, color);
  }

  public GameObject CreatePrefab(
    Recipe.Ingredient[] fabricationIngredients,
    float fabricationTime,
    HashedString kAnim_ID,
    string[] fabricators,
    string recipeDescription,
    Color color)
  {
    GameObject basicEntity = EntityTemplates.CreateBasicEntity(this.id, this.name, this.description, 1f, true, Assets.GetAnim(kAnim_ID), "ui", Grid.SceneLayer.BuildingFront, SimHashes.Creature, (List<Tag>) null, 293f);
    basicEntity.AddOrGet<ResearchPointObject>().TypeID = this.id;
    this._recipe = new Recipe(this.id, 1f, (SimHashes) 0, this.name, recipeDescription, 0);
    this._recipe.SetFabricators(fabricators, fabricationTime);
    this._recipe.SetIcon(Assets.GetSprite((HashedString) "research_type_icon"), color);
    if (fabricationIngredients != null)
    {
      foreach (Recipe.Ingredient fabricationIngredient in fabricationIngredients)
        this._recipe.AddIngredient(fabricationIngredient);
    }
    return basicEntity;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }

  public string id
  {
    get
    {
      return this._id;
    }
  }

  public string name
  {
    get
    {
      return this._name;
    }
  }

  public string description
  {
    get
    {
      return this._description;
    }
  }

  public string recipe
  {
    get
    {
      return this.recipe;
    }
  }

  public Color color
  {
    get
    {
      return this._color;
    }
  }

  public Sprite sprite
  {
    get
    {
      return this._sprite;
    }
  }
}
