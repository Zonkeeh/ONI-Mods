// Decompiled with JetBrains decompiler
// Type: GasMiniPumpConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class GasMiniPumpConfig : IBuildingConfig
{
  public const string ID = "GasMiniPump";

  public override BuildingDef CreateBuildingDef()
  {
    string id = "GasMiniPump";
    int width = 1;
    int height = 2;
    string anim = "minigaspump_kanim";
    int hitpoints = 30;
    float construction_time = 60f;
    float[] tieR1 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
    string[] plastics = MATERIALS.PLASTICS;
    float melting_point = 1600f;
    BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
    EffectorValues tieR2 = NOISE_POLLUTION.NOISY.TIER2;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR1, plastics, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, tieR2, 0.2f);
    buildingDef.PermittedRotations = PermittedRotations.R360;
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 60f;
    buildingDef.ExhaustKilowattsWhenActive = 0.0f;
    buildingDef.SelfHeatKilowattsWhenActive = 0.0f;
    buildingDef.OutputConduitType = ConduitType.Gas;
    buildingDef.Floodable = true;
    buildingDef.ViewMode = OverlayModes.GasConduits.ID;
    buildingDef.AudioCategory = "Metal";
    buildingDef.PowerInputOffset = new CellOffset(0, 0);
    buildingDef.UtilityOutputOffset = new CellOffset(0, 1);
    return buildingDef;
  }

  public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, LogicOperationalController.INPUT_PORTS_0_1);
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, LogicOperationalController.INPUT_PORTS_0_1);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, LogicOperationalController.INPUT_PORTS_0_1);
    go.AddOrGet<LogicOperationalController>();
    go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
    go.AddOrGet<LoopingSounds>();
    go.AddOrGet<EnergyConsumer>();
    go.AddOrGet<Pump>();
    go.AddOrGet<Storage>().capacityKg = 0.1f;
    ElementConsumer elementConsumer = go.AddOrGet<ElementConsumer>();
    elementConsumer.configuration = ElementConsumer.Configuration.AllGas;
    elementConsumer.consumptionRate = 0.05f;
    elementConsumer.storeOnConsume = true;
    elementConsumer.showInStatusPanel = false;
    elementConsumer.consumptionRadius = (byte) 2;
    ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
    conduitDispenser.conduitType = ConduitType.Gas;
    conduitDispenser.alwaysDispense = true;
    conduitDispenser.elementFilter = (SimHashes[]) null;
    go.AddOrGetDef<OperationalController.Def>();
  }
}
