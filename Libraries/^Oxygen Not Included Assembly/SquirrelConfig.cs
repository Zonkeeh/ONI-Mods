// Decompiled with JetBrains decompiler
// Type: SquirrelConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using UnityEngine;

public class SquirrelConfig : IEntityConfig
{
  private static float DAYS_PLANT_GROWTH_EATEN_PER_CYCLE = 0.4f;
  private static float CALORIES_PER_DAY_OF_PLANT_EATEN = SquirrelTuning.STANDARD_CALORIES_PER_CYCLE / SquirrelConfig.DAYS_PLANT_GROWTH_EATEN_PER_CYCLE;
  private static float KG_POOP_PER_DAY_OF_PLANT = 50f;
  private static float MIN_POOP_SIZE_KG = 40f;
  public static int EGG_SORT_ORDER = 0;
  public const string ID = "Squirrel";
  public const string BASE_TRAIT_ID = "SquirrelBaseTrait";
  public const string EGG_ID = "SquirrelEgg";
  public const float OXYGEN_RATE = 0.0234375f;
  public const float BABY_OXYGEN_RATE = 0.01171875f;
  private const SimHashes EMIT_ELEMENT = SimHashes.Dirt;

  public static GameObject CreateSquirrel(
    string id,
    string name,
    string desc,
    string anim_file,
    bool is_baby)
  {
    GameObject wildCreature = EntityTemplates.ExtendEntityToWildCreature(BaseSquirrelConfig.BaseSquirrel(id, name, desc, anim_file, "SquirrelBaseTrait", is_baby, (string) null), SquirrelTuning.PEN_SIZE_PER_CREATURE, 100f);
    Trait trait = Db.Get().CreateTrait("SquirrelBaseTrait", name, name, (string) null, false, (ChoreGroup[]) null, true, true);
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, SquirrelTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, (float) (-(double) SquirrelTuning.STANDARD_CALORIES_PER_CYCLE / 600.0), name, false, false, true));
    trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 100f, name, false, false, true));
    Diet.Info[] diet_infos = BaseSquirrelConfig.BasicWoodDiet(SimHashes.Dirt.CreateTag(), SquirrelConfig.CALORIES_PER_DAY_OF_PLANT_EATEN, SquirrelConfig.KG_POOP_PER_DAY_OF_PLANT, (string) null, 0.0f);
    return BaseSquirrelConfig.SetupDiet(wildCreature, diet_infos, SquirrelConfig.MIN_POOP_SIZE_KG);
  }

  public GameObject CreatePrefab()
  {
    return EntityTemplates.ExtendEntityToFertileCreature(SquirrelConfig.CreateSquirrel("Squirrel", (string) CREATURES.SPECIES.SQUIRREL.NAME, (string) CREATURES.SPECIES.SQUIRREL.DESC, "squirrel_kanim", false), "SquirrelEgg", (string) CREATURES.SPECIES.SQUIRREL.EGG_NAME, (string) CREATURES.SPECIES.SQUIRREL.DESC, "egg_squirrel_kanim", SquirrelTuning.EGG_MASS, "SquirrelBaby", 60f, 20f, SquirrelTuning.EGG_CHANCES_BASE, SquirrelConfig.EGG_SORT_ORDER, true, false, true, 1f);
  }

  public void OnPrefabInit(GameObject prefab)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
