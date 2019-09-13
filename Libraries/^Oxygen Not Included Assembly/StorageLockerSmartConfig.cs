// Decompiled with JetBrains decompiler
// Type: StorageLockerSmartConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class StorageLockerSmartConfig : IBuildingConfig
{
  private static readonly LogicPorts.Port OUTPUT_PORT = LogicPorts.Port.OutputPort(FilteredStorage.FULL_PORT_ID, new CellOffset(0, 1), (string) STRINGS.BUILDINGS.PREFABS.STORAGELOCKERSMART.LOGIC_PORT, (string) STRINGS.BUILDINGS.PREFABS.STORAGELOCKERSMART.LOGIC_PORT_ACTIVE, (string) STRINGS.BUILDINGS.PREFABS.STORAGELOCKERSMART.LOGIC_PORT_INACTIVE, true, false);
  public const string ID = "StorageLockerSmart";

  public override BuildingDef CreateBuildingDef()
  {
    string id = "StorageLockerSmart";
    int width = 1;
    int height = 2;
    string anim = "smartstoragelocker_kanim";
    int hitpoints = 30;
    float construction_time = 60f;
    float[] tieR3 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
    string[] refinedMetals = MATERIALS.REFINED_METALS;
    float melting_point = 1600f;
    BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
    EffectorValues none = TUNING.NOISE_POLLUTION.NONE;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR3, refinedMetals, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER1, none, 0.2f);
    buildingDef.Floodable = false;
    buildingDef.AudioCategory = "Metal";
    buildingDef.Overheatable = false;
    buildingDef.ViewMode = OverlayModes.Logic.ID;
    buildingDef.RequiresPowerInput = true;
    buildingDef.EnergyConsumptionWhenActive = 60f;
    buildingDef.ExhaustKilowattsWhenActive = 0.125f;
    return buildingDef;
  }

  public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, StorageLockerSmartConfig.OUTPUT_PORT);
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, StorageLockerSmartConfig.OUTPUT_PORT);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, StorageLockerSmartConfig.OUTPUT_PORT);
    SoundEventVolumeCache.instance.AddVolume("storagelocker_kanim", "StorageLocker_Hit_metallic_low", TUNING.NOISE_POLLUTION.NOISY.TIER1);
    Prioritizable.AddRef(go);
    Storage storage = go.AddOrGet<Storage>();
    storage.showInUI = true;
    storage.allowItemRemoval = true;
    storage.showDescriptor = true;
    storage.storageFilters = STORAGEFILTERS.NOT_EDIBLE_SOLIDS;
    storage.storageFullMargin = TUNING.STORAGE.STORAGE_LOCKER_FILLED_MARGIN;
    storage.fetchCategory = Storage.FetchCategory.GeneralStorage;
    go.AddOrGet<CopyBuildingSettings>().copyGroupTag = GameTags.StorageLocker;
    go.AddOrGet<StorageLockerSmart>();
    go.AddOrGetDef<StorageController.Def>();
  }
}
