// Decompiled with JetBrains decompiler
// Type: FishFeederConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class FishFeederConfig : IBuildingConfig
{
  public const string ID = "FishFeeder";

  public override BuildingDef CreateBuildingDef()
  {
    string id = "FishFeeder";
    int width = 1;
    int height = 3;
    string anim = "fishfeeder_kanim";
    int hitpoints = 100;
    float construction_time = 120f;
    float[] tieR3 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
    string[] rawMetals = MATERIALS.RAW_METALS;
    float melting_point = 1600f;
    BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
    EffectorValues none = TUNING.NOISE_POLLUTION.NONE;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR3, rawMetals, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER2, none, 0.2f);
    buildingDef.AudioCategory = "Metal";
    buildingDef.Entombable = true;
    buildingDef.Floodable = true;
    buildingDef.ForegroundLayer = Grid.SceneLayer.TileMain;
    return buildingDef;
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    Prioritizable.AddRef(go);
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.CreatureFeeder, false);
    Storage storage1 = go.AddOrGet<Storage>();
    storage1.capacityKg = 200f;
    storage1.showInUI = true;
    storage1.showDescriptor = true;
    storage1.allowItemRemoval = false;
    storage1.allowSettingOnlyFetchMarkedItems = false;
    Storage storage2 = go.AddComponent<Storage>();
    storage2.capacityKg = 200f;
    storage2.showInUI = true;
    storage2.showDescriptor = true;
    storage2.allowItemRemoval = false;
    go.AddOrGet<StorageLocker>();
    Effect resource = new Effect("AteFromFeeder", (string) STRINGS.CREATURES.MODIFIERS.ATE_FROM_FEEDER.NAME, (string) STRINGS.CREATURES.MODIFIERS.ATE_FROM_FEEDER.TOOLTIP, 600f, true, false, false, (string) null, 0.0f, (string) null);
    resource.Add(new AttributeModifier(Db.Get().Amounts.Wildness.deltaAttribute.Id, -0.03333334f, (string) STRINGS.CREATURES.MODIFIERS.ATE_FROM_FEEDER.NAME, false, false, true));
    resource.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, 2f, (string) STRINGS.CREATURES.MODIFIERS.ATE_FROM_FEEDER.NAME, false, false, true));
    Db.Get().effects.Add(resource);
    go.AddOrGet<TreeFilterable>();
    go.AddOrGet<CreatureFeeder>().effectId = resource.Id;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.AddOrGetDef<StorageController.Def>();
    go.AddOrGetDef<FishFeeder.Def>();
    go.AddOrGetDef<MakeBaseSolid.Def>();
    SymbolOverrideControllerUtil.AddToPrefab(go);
  }

  public override void ConfigurePost(BuildingDef def)
  {
    List<Tag> tagList = new List<Tag>();
    Tag[] target_species = new Tag[1]
    {
      GameTags.Creatures.Species.PacuSpecies
    };
    foreach (KeyValuePair<Tag, Diet> collectDiet in DietManager.CollectDiets(target_species))
      tagList.Add(collectDiet.Key);
    def.BuildingComplete.GetComponent<Storage>().storageFilters = tagList;
  }
}
