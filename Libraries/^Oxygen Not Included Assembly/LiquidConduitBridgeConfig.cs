// Decompiled with JetBrains decompiler
// Type: LiquidConduitBridgeConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class LiquidConduitBridgeConfig : IBuildingConfig
{
  public const string ID = "LiquidConduitBridge";
  private const ConduitType CONDUIT_TYPE = ConduitType.Liquid;

  public override BuildingDef CreateBuildingDef()
  {
    string id = "LiquidConduitBridge";
    int width = 3;
    int height = 1;
    string anim = "utilityliquidbridge_kanim";
    int hitpoints = 10;
    float construction_time = 3f;
    float[] tieR2 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
    string[] rawMinerals = MATERIALS.RAW_MINERALS;
    float melting_point = 1600f;
    BuildLocationRule build_location_rule = BuildLocationRule.Conduit;
    EffectorValues none = NOISE_POLLUTION.NONE;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR2, rawMinerals, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, none, 0.2f);
    buildingDef.ObjectLayer = ObjectLayer.LiquidConduitConnection;
    buildingDef.SceneLayer = Grid.SceneLayer.LiquidConduitBridges;
    buildingDef.InputConduitType = ConduitType.Liquid;
    buildingDef.OutputConduitType = ConduitType.Liquid;
    buildingDef.Floodable = false;
    buildingDef.Entombable = false;
    buildingDef.Overheatable = false;
    buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
    buildingDef.AudioCategory = "Metal";
    buildingDef.AudioSize = "small";
    buildingDef.BaseTimeUntilRepair = -1f;
    buildingDef.PermittedRotations = PermittedRotations.R360;
    buildingDef.UtilityInputOffset = new CellOffset(-1, 0);
    buildingDef.UtilityOutputOffset = new CellOffset(1, 0);
    GeneratedBuildings.RegisterWithOverlay(OverlayScreen.LiquidVentIDs, "LiquidConduitBridge");
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    GeneratedBuildings.MakeBuildingAlwaysOperational(go);
    BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof (RequiresFoundation), prefab_tag);
    go.AddOrGet<ConduitBridge>().type = ConduitType.Liquid;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    Object.DestroyImmediate((Object) go.GetComponent<RequireInputs>());
    Object.DestroyImmediate((Object) go.GetComponent<ConduitConsumer>());
    Object.DestroyImmediate((Object) go.GetComponent<ConduitDispenser>());
  }
}
