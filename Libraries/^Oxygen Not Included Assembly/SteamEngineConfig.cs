// Decompiled with JetBrains decompiler
// Type: SteamEngineConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class SteamEngineConfig : IBuildingConfig
{
  public const string ID = "SteamEngine";

  public override BuildingDef CreateBuildingDef()
  {
    string id = "SteamEngine";
    int width = 7;
    int height = 5;
    string anim = "rocket_steam_engine_kanim";
    int hitpoints = 1000;
    float construction_time = 480f;
    float[] tieR7 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER7;
    string[] construction_materials = new string[1]
    {
      SimHashes.Steel.ToString()
    };
    float melting_point = 9999f;
    BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
    EffectorValues tieR2 = NOISE_POLLUTION.NOISY.TIER2;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR7, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tieR2, 0.2f);
    BuildingTemplates.CreateRocketBuildingDef(buildingDef);
    buildingDef.SceneLayer = Grid.SceneLayer.BuildingFront;
    buildingDef.OverheatTemperature = 2273.15f;
    buildingDef.Floodable = false;
    buildingDef.AttachmentSlotTag = GameTags.Rocket;
    buildingDef.ObjectLayer = ObjectLayer.Building;
    buildingDef.attachablePosition = new CellOffset(0, 0);
    buildingDef.UtilityInputOffset = new CellOffset(2, 3);
    buildingDef.InputConduitType = ConduitType.Gas;
    buildingDef.RequiresPowerInput = false;
    buildingDef.CanMove = true;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof (RequiresFoundation), prefab_tag);
    go.AddOrGet<LoopingSounds>();
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
    go.AddOrGet<BuildingAttachPoint>().points = new BuildingAttachPoint.HardPoint[1]
    {
      new BuildingAttachPoint.HardPoint(new CellOffset(0, 5), GameTags.Rocket, (AttachableBuilding) null)
    };
  }

  public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
  {
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    RocketEngine rocketEngine = go.AddOrGet<RocketEngine>();
    rocketEngine.fuelTag = ElementLoader.FindElementByHash(SimHashes.Steam).tag;
    rocketEngine.efficiency = ROCKETRY.ENGINE_EFFICIENCY.WEAK;
    rocketEngine.explosionEffectHash = SpawnFXHashes.MeteorImpactDust;
    rocketEngine.requireOxidizer = false;
    rocketEngine.exhaustElement = SimHashes.Steam;
    rocketEngine.exhaustTemperature = ElementLoader.FindElementByHash(SimHashes.Steam).lowTemp + 50f;
    FuelTank fuelTank = go.AddOrGet<FuelTank>();
    fuelTank.capacityKg = fuelTank.minimumLaunchMass;
    fuelTank.FuelType = ElementLoader.FindElementByHash(SimHashes.Steam).tag;
    fuelTank.SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>()
    {
      Storage.StoredItemModifier.Hide,
      Storage.StoredItemModifier.Seal,
      Storage.StoredItemModifier.Insulate
    });
    ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
    conduitConsumer.conduitType = ConduitType.Gas;
    conduitConsumer.consumptionRate = 10f;
    conduitConsumer.capacityTag = fuelTank.FuelType;
    conduitConsumer.capacityKG = fuelTank.capacityKg;
    conduitConsumer.forceAlwaysSatisfied = true;
    conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
    go.AddOrGet<RocketModule>().SetBGKAnim(Assets.GetAnim((HashedString) "rocket_steam_engine_bg_kanim"));
    EntityTemplates.ExtendBuildingToRocketModule(go);
  }
}
