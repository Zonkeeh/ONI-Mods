// Decompiled with JetBrains decompiler
// Type: StorageLockerConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class StorageLockerConfig : IBuildingConfig
{
  public const string ID = "StorageLocker";

  public override BuildingDef CreateBuildingDef()
  {
    string id = "StorageLocker";
    int width = 1;
    int height = 2;
    string anim = "storagelocker_kanim";
    int hitpoints = 30;
    float construction_time = 10f;
    float[] tieR4 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
    string[] rawMinerals = MATERIALS.RAW_MINERALS;
    float melting_point = 1600f;
    BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
    EffectorValues none = NOISE_POLLUTION.NONE;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR4, rawMinerals, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, none, 0.2f);
    buildingDef.Floodable = false;
    buildingDef.AudioCategory = "Metal";
    buildingDef.Overheatable = false;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    SoundEventVolumeCache.instance.AddVolume("storagelocker_kanim", "StorageLocker_Hit_metallic_low", NOISE_POLLUTION.NOISY.TIER1);
    Prioritizable.AddRef(go);
    Storage storage = go.AddOrGet<Storage>();
    storage.showInUI = true;
    storage.allowItemRemoval = true;
    storage.showDescriptor = true;
    storage.storageFilters = STORAGEFILTERS.NOT_EDIBLE_SOLIDS;
    storage.storageFullMargin = STORAGE.STORAGE_LOCKER_FILLED_MARGIN;
    storage.fetchCategory = Storage.FetchCategory.GeneralStorage;
    go.AddOrGet<CopyBuildingSettings>().copyGroupTag = GameTags.StorageLocker;
    go.AddOrGet<StorageLocker>();
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.AddOrGetDef<StorageController.Def>();
  }
}
