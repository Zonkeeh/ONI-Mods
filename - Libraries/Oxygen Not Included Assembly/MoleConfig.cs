// Decompiled with JetBrains decompiler
// Type: MoleConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System.Collections.Generic;
using UnityEngine;

public class MoleConfig : IEntityConfig
{
  private static float MIN_POOP_SIZE_IN_CALORIES = 2400000f;
  private static float CALORIES_PER_KG_OF_DIRT = 1000f;
  public static int EGG_SORT_ORDER = 800;
  public const string ID = "Mole";
  public const string BASE_TRAIT_ID = "MoleBaseTrait";
  public const string EGG_ID = "MoleEgg";

  public static GameObject CreateMole(
    string id,
    string name,
    string desc,
    string anim_file,
    bool is_baby = false)
  {
    GameObject gameObject = BaseMoleConfig.BaseMole(id, name, (string) STRINGS.CREATURES.SPECIES.MOLE.DESC, "MoleBaseTrait", anim_file, is_baby);
    gameObject.AddTag(GameTags.Creatures.Digger);
    EntityTemplates.ExtendEntityToWildCreature(gameObject, MoleTuning.PEN_SIZE_PER_CREATURE, 100f);
    Trait trait = Db.Get().CreateTrait("MoleBaseTrait", name, name, (string) null, false, (ChoreGroup[]) null, true, true);
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, MoleTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, (float) (-(double) MoleTuning.STANDARD_CALORIES_PER_CYCLE / 600.0), name, false, false, true));
    trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name, false, false, true));
    Diet diet = new Diet(BaseMoleConfig.SimpleOreDiet(new List<Tag>()
    {
      SimHashes.Regolith.CreateTag(),
      SimHashes.Dirt.CreateTag(),
      SimHashes.IronOre.CreateTag()
    }, MoleConfig.CALORIES_PER_KG_OF_DIRT, TUNING.CREATURES.CONVERSION_EFFICIENCY.NORMAL).ToArray());
    CreatureCalorieMonitor.Def def = gameObject.AddOrGetDef<CreatureCalorieMonitor.Def>();
    def.diet = diet;
    def.minPoopSizeInCalories = MoleConfig.MIN_POOP_SIZE_IN_CALORIES;
    gameObject.AddOrGetDef<SolidConsumerMonitor.Def>().diet = diet;
    gameObject.AddOrGetDef<OvercrowdingMonitor.Def>().spaceRequiredPerCreature = 0;
    gameObject.AddOrGet<LoopingSounds>();
    return gameObject;
  }

  public GameObject CreatePrefab()
  {
    GameObject mole = MoleConfig.CreateMole("Mole", (string) STRINGS.CREATURES.SPECIES.MOLE.NAME, (string) STRINGS.CREATURES.SPECIES.MOLE.DESC, "driller_kanim", false);
    string eggId = "MoleEgg";
    string eggName = (string) STRINGS.CREATURES.SPECIES.MOLE.EGG_NAME;
    string desc = (string) STRINGS.CREATURES.SPECIES.MOLE.DESC;
    string egg_anim = "egg_driller_kanim";
    float eggMass = MoleTuning.EGG_MASS;
    string baby_id = "MoleBaby";
    float fertility_cycles = 60f;
    float incubation_cycles = 20f;
    int eggSortOrder = MoleConfig.EGG_SORT_ORDER;
    return EntityTemplates.ExtendEntityToFertileCreature(mole, eggId, eggName, desc, egg_anim, eggMass, baby_id, fertility_cycles, incubation_cycles, MoleTuning.EGG_CHANCES_BASE, eggSortOrder, true, false, true, 1f);
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
    MoleConfig.SetSpawnNavType(inst);
  }

  public static void SetSpawnNavType(GameObject inst)
  {
    int cell = Grid.PosToCell(inst);
    Navigator component = inst.GetComponent<Navigator>();
    if (!((Object) component != (Object) null))
      return;
    if (Grid.IsSolidCell(cell))
    {
      component.SetCurrentNavType(NavType.Solid);
      inst.transform.SetPosition(Grid.CellToPosCBC(cell, Grid.SceneLayer.FXFront));
      inst.GetComponent<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.FXFront);
    }
    else
      inst.GetComponent<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.Creatures);
  }
}
