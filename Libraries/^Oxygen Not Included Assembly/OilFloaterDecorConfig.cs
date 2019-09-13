// Decompiled with JetBrains decompiler
// Type: OilFloaterDecorConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using UnityEngine;

public class OilFloaterDecorConfig : IEntityConfig
{
  private static float KG_ORE_EATEN_PER_CYCLE = 30f;
  private static float CALORIES_PER_KG_OF_ORE = OilFloaterTuning.STANDARD_CALORIES_PER_CYCLE / OilFloaterDecorConfig.KG_ORE_EATEN_PER_CYCLE;
  public static int EGG_SORT_ORDER = OilFloaterConfig.EGG_SORT_ORDER + 2;
  public const string ID = "OilfloaterDecor";
  public const string BASE_TRAIT_ID = "OilfloaterDecorBaseTrait";
  public const string EGG_ID = "OilfloaterDecorEgg";
  public const SimHashes CONSUME_ELEMENT = SimHashes.Oxygen;

  public static GameObject CreateOilFloater(
    string id,
    string name,
    string desc,
    string anim_file,
    bool is_baby)
  {
    GameObject gameObject = BaseOilFloaterConfig.BaseOilFloater(id, name, desc, anim_file, "OilfloaterDecorBaseTrait", 283.15f, 343.15f, is_baby, "oxy_");
    gameObject.AddOrGet<DecorProvider>().SetValues(TUNING.DECOR.BONUS.TIER6);
    EntityTemplates.ExtendEntityToWildCreature(gameObject, OilFloaterTuning.PEN_SIZE_PER_CREATURE, 150f);
    Trait trait = Db.Get().CreateTrait("OilfloaterDecorBaseTrait", name, name, (string) null, false, (ChoreGroup[]) null, true, true);
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.maxAttribute.Id, OilFloaterTuning.STANDARD_STOMACH_SIZE, name, false, false, true));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, (float) (-(double) OilFloaterTuning.STANDARD_CALORIES_PER_CYCLE / 600.0), name, false, false, true));
    trait.Add(new AttributeModifier(Db.Get().Amounts.HitPoints.maxAttribute.Id, 25f, name, false, false, true));
    trait.Add(new AttributeModifier(Db.Get().Amounts.Age.maxAttribute.Id, 150f, name, false, false, true));
    return BaseOilFloaterConfig.SetupDiet(gameObject, SimHashes.Oxygen.CreateTag(), Tag.Invalid, OilFloaterDecorConfig.CALORIES_PER_KG_OF_ORE, 0.0f, (string) null, 0.0f, 0.0f);
  }

  public GameObject CreatePrefab()
  {
    GameObject oilFloater = OilFloaterDecorConfig.CreateOilFloater("OilfloaterDecor", (string) STRINGS.CREATURES.SPECIES.OILFLOATER.VARIANT_DECOR.NAME, (string) STRINGS.CREATURES.SPECIES.OILFLOATER.VARIANT_DECOR.DESC, "oilfloater_kanim", false);
    EntityTemplates.ExtendEntityToFertileCreature(oilFloater, "OilfloaterDecorEgg", (string) STRINGS.CREATURES.SPECIES.OILFLOATER.VARIANT_DECOR.EGG_NAME, (string) STRINGS.CREATURES.SPECIES.OILFLOATER.VARIANT_DECOR.DESC, "egg_oilfloater_kanim", OilFloaterTuning.EGG_MASS, "OilfloaterDecorBaby", 90f, 30f, OilFloaterTuning.EGG_CHANCES_DECOR, OilFloaterDecorConfig.EGG_SORT_ORDER, true, false, true, 1f);
    return oilFloater;
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
  }
}
