// Decompiled with JetBrains decompiler
// Type: RefrigeratorConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class RefrigeratorConfig : IBuildingConfig
{
  private static readonly LogicPorts.Port OUTPUT_PORT = LogicPorts.Port.OutputPort(FilteredStorage.FULL_PORT_ID, new CellOffset(0, 1), (string) STRINGS.BUILDINGS.PREFABS.REFRIGERATOR.LOGIC_PORT, (string) STRINGS.BUILDINGS.PREFABS.REFRIGERATOR.LOGIC_PORT_ACTIVE, (string) STRINGS.BUILDINGS.PREFABS.REFRIGERATOR.LOGIC_PORT_INACTIVE, false, false);
  public const string ID = "Refrigerator";

  public override BuildingDef CreateBuildingDef()
  {
    string id = "Refrigerator";
    int width = 1;
    int height = 2;
    string anim = "fridge_kanim";
    int hitpoints = 30;
    float construction_time = 10f;
    float[] tieR4 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
    string[] rawMinerals = MATERIALS.RAW_MINERALS;
    float melting_point = 800f;
    BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
    EffectorValues tieR0 = TUNING.NOISE_POLLUTION.NOISY.TIER0;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR4, rawMinerals, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.BONUS.TIER1, tieR0, 0.2f);
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 120f;
    buildingDef.ExhaustKilowattsWhenActive = 0.5f;
    buildingDef.Floodable = false;
    buildingDef.ViewMode = OverlayModes.Power.ID;
    buildingDef.AudioCategory = "Metal";
    SoundEventVolumeCache.instance.AddVolume("fridge_kanim", "Refrigerator_open", TUNING.NOISE_POLLUTION.NOISY.TIER1);
    SoundEventVolumeCache.instance.AddVolume("fridge_kanim", "Refrigerator_close", TUNING.NOISE_POLLUTION.NOISY.TIER1);
    return buildingDef;
  }

  public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, RefrigeratorConfig.OUTPUT_PORT);
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, RefrigeratorConfig.OUTPUT_PORT);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, RefrigeratorConfig.OUTPUT_PORT);
    Storage storage = go.AddOrGet<Storage>();
    storage.showInUI = true;
    storage.showDescriptor = true;
    storage.storageFilters = STORAGEFILTERS.FOOD;
    storage.allowItemRemoval = true;
    storage.capacityKg = 100f;
    storage.storageFullMargin = TUNING.STORAGE.STORAGE_LOCKER_FILLED_MARGIN;
    storage.fetchCategory = Storage.FetchCategory.GeneralStorage;
    Prioritizable.AddRef(go);
    go.AddOrGet<TreeFilterable>();
    go.AddOrGet<Refrigerator>();
    go.AddOrGet<DropAllWorkable>();
    go.AddOrGetDef<StorageController.Def>();
  }
}
