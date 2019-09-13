// Decompiled with JetBrains decompiler
// Type: PowerTransformerConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class PowerTransformerConfig : IBuildingConfig
{
  public const string ID = "PowerTransformer";

  public override BuildingDef CreateBuildingDef()
  {
    string id = "PowerTransformer";
    int width = 3;
    int height = 2;
    string anim = "transformer_kanim";
    int hitpoints = 30;
    float construction_time = 30f;
    float[] tieR3 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
    string[] refinedMetals = MATERIALS.REFINED_METALS;
    float melting_point = 800f;
    BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
    EffectorValues tieR5 = NOISE_POLLUTION.NOISY.TIER5;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR3, refinedMetals, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, tieR5, 0.2f);
    buildingDef.RequiresPowerInput = true;
    buildingDef.UseWhitePowerOutputConnectorColour = true;
    buildingDef.PowerInputOffset = new CellOffset(-1, 1);
    buildingDef.PowerOutputOffset = new CellOffset(1, 0);
    buildingDef.ElectricalArrowOffset = new CellOffset(1, 0);
    buildingDef.ExhaustKilowattsWhenActive = 0.25f;
    buildingDef.SelfHeatKilowattsWhenActive = 1f;
    buildingDef.ViewMode = OverlayModes.Power.ID;
    buildingDef.AudioCategory = "Metal";
    buildingDef.ExhaustKilowattsWhenActive = 0.0f;
    buildingDef.SelfHeatKilowattsWhenActive = 1f;
    buildingDef.Entombable = true;
    buildingDef.GeneratorWattageRating = 4000f;
    buildingDef.GeneratorBaseCapacity = 4000f;
    buildingDef.PermittedRotations = PermittedRotations.FlipH;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
    go.AddComponent<RequireInputs>();
    BuildingDef def = go.GetComponent<Building>().Def;
    Battery battery = go.AddOrGet<Battery>();
    battery.powerSortOrder = 1000;
    battery.capacity = def.GeneratorWattageRating;
    battery.chargeWattage = def.GeneratorWattageRating;
    go.AddComponent<PowerTransformer>().powerDistributionOrder = 9;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    Object.DestroyImmediate((Object) go.GetComponent<EnergyConsumer>());
    go.AddOrGetDef<PoweredActiveController.Def>();
  }
}
