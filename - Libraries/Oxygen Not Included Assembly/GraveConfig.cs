// Decompiled with JetBrains decompiler
// Type: GraveConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class GraveConfig : IBuildingConfig
{
  private static readonly HashedString[] STORAGE_WORK_ANIMS = new HashedString[1]
  {
    (HashedString) "working_pre"
  };
  private static readonly HashedString STORAGE_PST_ANIM = HashedString.Invalid;
  private static readonly List<Storage.StoredItemModifier> StorageModifiers = new List<Storage.StoredItemModifier>()
  {
    Storage.StoredItemModifier.Hide,
    Storage.StoredItemModifier.Preserve
  };
  public const string ID = "Grave";
  private static KAnimFile[] STORAGE_OVERRIDE_ANIM_FILES;

  public override BuildingDef CreateBuildingDef()
  {
    string id = "Grave";
    int width = 1;
    int height = 2;
    string anim = "gravestone_kanim";
    int hitpoints = 30;
    float construction_time = 120f;
    float[] tieR5 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
    string[] rawMinerals = MATERIALS.RAW_MINERALS;
    float melting_point = 1600f;
    BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
    EffectorValues none = NOISE_POLLUTION.NONE;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR5, rawMinerals, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER2, none, 0.2f);
    buildingDef.Overheatable = false;
    buildingDef.Floodable = false;
    buildingDef.AudioCategory = "Metal";
    buildingDef.BaseTimeUntilRepair = -1f;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    GraveConfig.STORAGE_OVERRIDE_ANIM_FILES = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_bury_dupe_kanim")
    };
    Storage storage = go.AddOrGet<Storage>();
    storage.showInUI = true;
    storage.SetDefaultStoredItemModifiers(GraveConfig.StorageModifiers);
    storage.overrideAnims = GraveConfig.STORAGE_OVERRIDE_ANIM_FILES;
    storage.workAnims = GraveConfig.STORAGE_WORK_ANIMS;
    storage.workingPstComplete = GraveConfig.STORAGE_PST_ANIM;
    storage.synchronizeAnims = false;
    storage.useGunForDelivery = false;
    storage.workAnimPlayMode = KAnim.PlayMode.Once;
    go.AddOrGet<Grave>();
    Prioritizable.AddRef(go);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
  }
}
