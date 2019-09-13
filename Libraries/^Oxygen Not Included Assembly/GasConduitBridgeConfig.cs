// Decompiled with JetBrains decompiler
// Type: GasConduitBridgeConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class GasConduitBridgeConfig : IBuildingConfig
{
  public const string ID = "GasConduitBridge";
  private const ConduitType CONDUIT_TYPE = ConduitType.Gas;

  public override BuildingDef CreateBuildingDef()
  {
    string id = "GasConduitBridge";
    int width = 3;
    int height = 1;
    string anim = "utilitygasbridge_kanim";
    int hitpoints = 10;
    float construction_time = 3f;
    float[] tieR1 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
    string[] rawMinerals = MATERIALS.RAW_MINERALS;
    float melting_point = 1600f;
    BuildLocationRule build_location_rule = BuildLocationRule.Conduit;
    EffectorValues none = NOISE_POLLUTION.NONE;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR1, rawMinerals, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, none, 0.2f);
    buildingDef.ObjectLayer = ObjectLayer.GasConduitConnection;
    buildingDef.SceneLayer = Grid.SceneLayer.GasConduitBridges;
    buildingDef.InputConduitType = ConduitType.Gas;
    buildingDef.OutputConduitType = ConduitType.Gas;
    buildingDef.Floodable = false;
    buildingDef.Entombable = false;
    buildingDef.Overheatable = false;
    buildingDef.ViewMode = OverlayModes.GasConduits.ID;
    buildingDef.AudioCategory = "Metal";
    buildingDef.AudioSize = "small";
    buildingDef.BaseTimeUntilRepair = -1f;
    buildingDef.PermittedRotations = PermittedRotations.R360;
    buildingDef.UtilityInputOffset = new CellOffset(-1, 0);
    buildingDef.UtilityOutputOffset = new CellOffset(1, 0);
    GeneratedBuildings.RegisterWithOverlay(OverlayScreen.GasVentIDs, buildingDef.PrefabID);
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    GeneratedBuildings.MakeBuildingAlwaysOperational(go);
    BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof (RequiresFoundation), prefab_tag);
    go.AddOrGet<ConduitBridge>().type = ConduitType.Gas;
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    Object.DestroyImmediate((Object) go.GetComponent<RequireInputs>());
    Object.DestroyImmediate((Object) go.GetComponent<ConduitConsumer>());
    Object.DestroyImmediate((Object) go.GetComponent<ConduitDispenser>());
  }
}
