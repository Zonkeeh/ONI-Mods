// Decompiled with JetBrains decompiler
// Type: FishTrapConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class FishTrapConfig : IBuildingConfig
{
  private static readonly List<Storage.StoredItemModifier> StoredItemModifiers = new List<Storage.StoredItemModifier>();
  public const string ID = "FishTrap";

  public override BuildingDef CreateBuildingDef()
  {
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("FishTrap", 1, 2, "fishtrap_kanim", 10, 10f, BUILDINGS.CONSTRUCTION_MASS_KG.TIER3, MATERIALS.PLASTICS, 1600f, BuildLocationRule.Anywhere, BUILDINGS.DECOR.PENALTY.TIER2, NOISE_POLLUTION.NOISY.TIER0, 0.2f);
    buildingDef.AudioCategory = "Metal";
    buildingDef.Floodable = false;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    Storage storage = go.AddOrGet<Storage>();
    storage.allowItemRemoval = true;
    storage.SetDefaultStoredItemModifiers(FishTrapConfig.StoredItemModifiers);
    storage.sendOnStoreOnSpawn = true;
    Trap trap = go.AddOrGet<Trap>();
    trap.trappableCreatures = new Tag[1]
    {
      GameTags.Creatures.Swimmer
    };
    trap.trappedOffset = new Vector2(0.0f, 1f);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    Lure.Def def = go.AddOrGetDef<Lure.Def>();
    def.lurePoints = new CellOffset[1]
    {
      new CellOffset(0, 0)
    };
    def.radius = 32;
    def.initialLures = new Tag[1]
    {
      GameTags.Creatures.FishTrapLure
    };
  }
}
