// Decompiled with JetBrains decompiler
// Type: WireBridgeConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class WireBridgeConfig : IBuildingConfig
{
  public const string ID = "WireBridge";

  protected virtual string GetID()
  {
    return "WireBridge";
  }

  public override BuildingDef CreateBuildingDef()
  {
    string id = this.GetID();
    int width = 3;
    int height = 1;
    string anim = "utilityelectricbridge_kanim";
    int hitpoints = 30;
    float construction_time = 3f;
    float[] tieR0 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
    string[] allMetals = MATERIALS.ALL_METALS;
    float melting_point = 1600f;
    BuildLocationRule build_location_rule = BuildLocationRule.WireBridge;
    EffectorValues none = NOISE_POLLUTION.NONE;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR0, allMetals, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER0, none, 0.2f);
    buildingDef.Overheatable = false;
    buildingDef.Floodable = false;
    buildingDef.Entombable = false;
    buildingDef.ViewMode = OverlayModes.Power.ID;
    buildingDef.ObjectLayer = ObjectLayer.WireConnectors;
    buildingDef.SceneLayer = Grid.SceneLayer.WireBridges;
    buildingDef.AudioCategory = "Metal";
    buildingDef.AudioSize = "small";
    buildingDef.BaseTimeUntilRepair = -1f;
    buildingDef.PermittedRotations = PermittedRotations.R360;
    buildingDef.UtilityInputOffset = new CellOffset(0, 0);
    buildingDef.UtilityOutputOffset = new CellOffset(0, 2);
    GeneratedBuildings.RegisterWithOverlay(OverlayScreen.WireIDs, "WireBridge");
    return buildingDef;
  }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    GeneratedBuildings.MakeBuildingAlwaysOperational(go);
  }

  public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
  {
    base.DoPostConfigurePreview(def, go);
    this.AddNetworkLink(go).visualizeOnly = true;
    go.AddOrGet<BuildingCellVisualizer>();
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
    base.DoPostConfigureUnderConstruction(go);
    this.AddNetworkLink(go).visualizeOnly = true;
    go.AddOrGet<BuildingCellVisualizer>();
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    this.AddNetworkLink(go).visualizeOnly = false;
    go.AddOrGet<BuildingCellVisualizer>();
  }

  protected virtual WireUtilityNetworkLink AddNetworkLink(GameObject go)
  {
    WireUtilityNetworkLink utilityNetworkLink = go.AddOrGet<WireUtilityNetworkLink>();
    utilityNetworkLink.maxWattageRating = Wire.WattageRating.Max1000;
    utilityNetworkLink.link1 = new CellOffset(-1, 0);
    utilityNetworkLink.link2 = new CellOffset(1, 0);
    return utilityNetworkLink;
  }
}
