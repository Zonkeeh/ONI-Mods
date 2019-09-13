// Decompiled with JetBrains decompiler
// Type: OreScrubberConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class OreScrubberConfig : IBuildingConfig
{
  public const string ID = "OreScrubber";
  private const float MASS_PER_USE = 0.07f;
  private const int DISEASE_REMOVAL_COUNT = 480000;
  private const SimHashes CONSUMED_ELEMENT = SimHashes.ChlorineGas;

  public override BuildingDef CreateBuildingDef()
  {
    string id = "OreScrubber";
    int width = 3;
    int height = 3;
    string anim = "orescrubber_kanim";
    int hitpoints = 30;
    float construction_time = 30f;
    string[] construction_materials = new string[1]
    {
      "Metal"
    };
    EffectorValues none = NOISE_POLLUTION.NONE;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, new float[1]
    {
      BUILDINGS.CONSTRUCTION_MASS_KG.TIER3[0]
    }, construction_materials, 1600f, BuildLocationRule.OnFloor, BUILDINGS.DECOR.BONUS.TIER1, none, 0.2f);
    buildingDef.UtilityInputOffset = new CellOffset(1, 1);
    buildingDef.ForegroundLayer = Grid.SceneLayer.BuildingFront;
    buildingDef.InputConduitType = ConduitType.Gas;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
    OreScrubber oreScrubber = go.AddOrGet<OreScrubber>();
    oreScrubber.massConsumedPerUse = 0.07f;
    oreScrubber.consumedElement = SimHashes.ChlorineGas;
    oreScrubber.diseaseRemovalCount = 480000;
    ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
    conduitConsumer.conduitType = ConduitType.Liquid;
    conduitConsumer.consumptionRate = 1f;
    conduitConsumer.capacityKG = 10f;
    conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
    conduitConsumer.capacityTag = ElementLoader.FindElementByHash(SimHashes.ChlorineGas).tag;
    go.AddOrGet<DirectionControl>();
    OreScrubber.Work work = go.AddOrGet<OreScrubber.Work>();
    work.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_ore_scrubber_kanim")
    };
    work.workTime = 10.2f;
    work.trackUses = true;
    work.workLayer = Grid.SceneLayer.BuildingUse;
    go.AddOrGet<Storage>().SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
