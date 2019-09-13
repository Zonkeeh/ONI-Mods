// Decompiled with JetBrains decompiler
// Type: EggConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class EggConfig
{
  public static GameObject CreateEgg(
    string id,
    string name,
    string desc,
    Tag creature_id,
    string anim,
    float mass,
    int egg_sort_order,
    float base_incubation_rate)
  {
    GameObject looseEntity = EntityTemplates.CreateLooseEntity(id, name, desc, mass, true, Assets.GetAnim((HashedString) anim), "idle", Grid.SceneLayer.Ore, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.8f, true, 0, SimHashes.Creature, (List<Tag>) null);
    looseEntity.AddOrGet<KBoxCollider2D>().offset = (Vector2) new Vector2f(0.0f, 0.36f);
    looseEntity.AddOrGet<Pickupable>().sortOrder = SORTORDER.EGGS + egg_sort_order;
    looseEntity.AddOrGet<Effects>();
    KPrefabID kprefabId = looseEntity.AddOrGet<KPrefabID>();
    kprefabId.AddTag(GameTags.Egg, false);
    kprefabId.AddTag(GameTags.IncubatableEgg, false);
    kprefabId.AddTag(GameTags.PedestalDisplayable, false);
    IncubationMonitor.Def def = looseEntity.AddOrGetDef<IncubationMonitor.Def>();
    def.spawnedCreature = creature_id;
    def.baseIncubationRate = base_incubation_rate;
    looseEntity.AddOrGetDef<OvercrowdingMonitor.Def>().spaceRequiredPerCreature = 0;
    Object.Destroy((Object) looseEntity.GetComponent<EntitySplitter>());
    Assets.AddPrefab(looseEntity.GetComponent<KPrefabID>());
    string str1 = string.Format((string) STRINGS.BUILDINGS.PREFABS.EGGCRACKER.RESULT_DESCRIPTION, (object) name);
    ComplexRecipe.RecipeElement[] ingredients = new ComplexRecipe.RecipeElement[1]
    {
      new ComplexRecipe.RecipeElement((Tag) id, 1f)
    };
    ComplexRecipe.RecipeElement[] results = new ComplexRecipe.RecipeElement[2]
    {
      new ComplexRecipe.RecipeElement((Tag) "RawEgg", 0.5f * mass),
      new ComplexRecipe.RecipeElement((Tag) "EggShell", 0.5f * mass)
    };
    string obsolete_id = ComplexRecipeManager.MakeObsoleteRecipeID(id, (Tag) "RawEgg");
    string str2 = ComplexRecipeManager.MakeRecipeID(id, (IList<ComplexRecipe.RecipeElement>) ingredients, (IList<ComplexRecipe.RecipeElement>) results);
    ComplexRecipe complexRecipe = new ComplexRecipe(str2, ingredients, results)
    {
      description = string.Format((string) STRINGS.BUILDINGS.PREFABS.EGGCRACKER.RECIPE_DESCRIPTION, (object) name, (object) str1),
      fabricators = new List<Tag>() { (Tag) "EggCracker" },
      time = 5f
    };
    ComplexRecipeManager.Get().AddObsoleteIDMapping(obsolete_id, str2);
    return looseEntity;
  }
}
