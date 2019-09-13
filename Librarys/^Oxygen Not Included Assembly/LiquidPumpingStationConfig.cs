// Decompiled with JetBrains decompiler
// Type: LiquidPumpingStationConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class LiquidPumpingStationConfig : IBuildingConfig
{
  public const string ID = "LiquidPumpingStation";

  public override BuildingDef CreateBuildingDef()
  {
    string id = "LiquidPumpingStation";
    int width = 2;
    int height = 4;
    string anim = "waterpump_kanim";
    int hitpoints = 100;
    float construction_time = 10f;
    float[] tieR4 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
    string[] rawMinerals = MATERIALS.RAW_MINERALS;
    float melting_point = 1600f;
    BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
    EffectorValues none = NOISE_POLLUTION.NONE;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR4, rawMinerals, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, none, 0.2f);
    buildingDef.Floodable = false;
    buildingDef.Entombable = true;
    buildingDef.AudioCategory = "Metal";
    buildingDef.AudioSize = "large";
    buildingDef.UtilityInputOffset = new CellOffset(0, 0);
    buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
    buildingDef.DefaultAnimState = "on";
    buildingDef.ShowInBuildMenu = true;
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<LoopingSounds>();
    go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
    go.AddOrGet<LiquidPumpingStation>().overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_waterpump_kanim")
    };
    Storage storage = go.AddOrGet<Storage>();
    storage.showInUI = false;
    storage.allowItemRemoval = true;
    storage.showDescriptor = true;
    storage.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
  }

  private static void AddGuide(GameObject go, bool occupy_tiles)
  {
    GameObject gameObject = new GameObject();
    gameObject.transform.parent = go.transform;
    gameObject.transform.SetLocalPosition(Vector3.zero);
    KBatchedAnimController kbatchedAnimController = gameObject.AddComponent<KBatchedAnimController>();
    kbatchedAnimController.Offset = go.GetComponent<Building>().Def.GetVisualizerOffset();
    kbatchedAnimController.AnimFiles = new KAnimFile[1]
    {
      Assets.GetAnim(new HashedString("waterpump_kanim"))
    };
    kbatchedAnimController.initialAnim = "place_guide";
    kbatchedAnimController.visibilityType = KAnimControllerBase.VisibilityType.OffscreenUpdate;
    kbatchedAnimController.isMovable = true;
    PumpingStationGuide pumpingStationGuide = gameObject.AddComponent<PumpingStationGuide>();
    pumpingStationGuide.parent = go;
    pumpingStationGuide.occupyTiles = occupy_tiles;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    LiquidPumpingStationConfig.AddGuide(go.GetComponent<Building>().Def.BuildingPreview, false);
    LiquidPumpingStationConfig.AddGuide(go.GetComponent<Building>().Def.BuildingUnderConstruction, true);
  }
}
