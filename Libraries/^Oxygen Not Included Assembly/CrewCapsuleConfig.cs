// Decompiled with JetBrains decompiler
// Type: CrewCapsuleConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class CrewCapsuleConfig : IBuildingConfig
{
  public const string ID = "CrewCapsule";

  public override BuildingDef CreateBuildingDef()
  {
    string id = "CrewCapsule";
    int width = 5;
    int height = 19;
    string anim = "rocket_small_steam_kanim";
    int hitpoints = 1000;
    float construction_time = 480f;
    float[] tieR7 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER7;
    string[] construction_materials = new string[1]
    {
      SimHashes.Steel.ToString()
    };
    float melting_point = 1600f;
    BuildLocationRule build_location_rule = BuildLocationRule.BuildingAttachPoint;
    EffectorValues tieR2 = NOISE_POLLUTION.NOISY.TIER2;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR7, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tieR2, 0.2f);
    buildingDef.SceneLayer = Grid.SceneLayer.BuildingFront;
    buildingDef.OverheatTemperature = 2273.15f;
    buildingDef.Floodable = false;
    buildingDef.AttachmentSlotTag = GameTags.Rocket;
    buildingDef.ObjectLayer = ObjectLayer.Building;
    buildingDef.UtilityInputOffset = new CellOffset(2, 6);
    buildingDef.InputConduitType = ConduitType.Gas;
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 10f;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<LoopingSounds>();
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
    go.AddOrGet<LaunchConditionManager>();
  }

  public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
  {
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.AddComponent<Storage>();
    ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
    conduitConsumer.conduitType = ConduitType.Gas;
    conduitConsumer.consumptionRate = 1f;
    conduitConsumer.capacityTag = ElementLoader.FindElementByHash(SimHashes.Oxygen).tag;
    conduitConsumer.capacityKG = 10f;
    conduitConsumer.forceAlwaysSatisfied = true;
    conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
  }
}
