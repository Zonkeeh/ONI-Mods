// Decompiled with JetBrains decompiler
// Type: LiquidFuelTankConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class LiquidFuelTankConfig : IBuildingConfig
{
  public const string ID = "LiquidFuelTank";
  public const float FuelCapacity = 900f;

  public override BuildingDef CreateBuildingDef()
  {
    string id = "LiquidFuelTank";
    int width = 5;
    int height = 5;
    string anim = "rocket_liquid_fuel_tank_kanim";
    int hitpoints = 1000;
    float construction_time = 60f;
    float[] fuelTankDryMass = BUILDINGS.ROCKETRY_MASS_KG.FUEL_TANK_DRY_MASS;
    string[] construction_materials = new string[1]
    {
      SimHashes.Steel.ToString()
    };
    float melting_point = 9999f;
    BuildLocationRule build_location_rule = BuildLocationRule.BuildingAttachPoint;
    EffectorValues tieR2 = NOISE_POLLUTION.NOISY.TIER2;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, fuelTankDryMass, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tieR2, 0.2f);
    BuildingTemplates.CreateRocketBuildingDef(buildingDef);
    buildingDef.SceneLayer = Grid.SceneLayer.BuildingFront;
    buildingDef.OverheatTemperature = 2273.15f;
    buildingDef.Floodable = false;
    buildingDef.AttachmentSlotTag = GameTags.Rocket;
    buildingDef.ObjectLayer = ObjectLayer.Building;
    buildingDef.UtilityInputOffset = new CellOffset(2, 3);
    buildingDef.InputConduitType = ConduitType.Liquid;
    buildingDef.RequiresPowerInput = false;
    buildingDef.attachablePosition = new CellOffset(0, 0);
    buildingDef.CanMove = true;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof (RequiresFoundation), prefab_tag);
    go.AddOrGet<BuildingAttachPoint>().points = new BuildingAttachPoint.HardPoint[1]
    {
      new BuildingAttachPoint.HardPoint(new CellOffset(0, 5), GameTags.Rocket, (AttachableBuilding) null)
    };
    go.AddOrGet<LoopingSounds>();
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
  }

  public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
  {
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    FuelTank fuelTank = go.AddOrGet<FuelTank>();
    fuelTank.capacityKg = fuelTank.minimumLaunchMass;
    fuelTank.SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>()
    {
      Storage.StoredItemModifier.Hide,
      Storage.StoredItemModifier.Seal,
      Storage.StoredItemModifier.Insulate
    });
    go.AddOrGet<DropToUserCapacity>();
    ManualDeliveryKG manualDeliveryKg = go.AddOrGet<ManualDeliveryKG>();
    manualDeliveryKg.SetStorage((Storage) fuelTank);
    manualDeliveryKg.refillMass = fuelTank.capacityKg;
    manualDeliveryKg.capacity = fuelTank.capacityKg;
    manualDeliveryKg.operationalRequirement = FetchOrder2.OperationalRequirement.None;
    manualDeliveryKg.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
    ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
    conduitConsumer.conduitType = ConduitType.Liquid;
    conduitConsumer.consumptionRate = 10f;
    conduitConsumer.capacityTag = GameTags.Liquid;
    conduitConsumer.capacityKG = fuelTank.capacityKg;
    conduitConsumer.forceAlwaysSatisfied = true;
    conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Store;
    go.AddOrGet<RocketModule>().SetBGKAnim(Assets.GetAnim((HashedString) "rocket_liquid_fuel_tank_bg_kanim"));
    EntityTemplates.ExtendBuildingToRocketModule(go);
  }
}
