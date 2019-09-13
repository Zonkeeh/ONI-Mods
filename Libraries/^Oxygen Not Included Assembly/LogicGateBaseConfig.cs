// Decompiled with JetBrains decompiler
// Type: LogicGateBaseConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public abstract class LogicGateBaseConfig : IBuildingConfig
{
  protected BuildingDef CreateBuildingDef(
    string ID,
    string anim,
    int width = 2,
    int height = 2)
  {
    string id = ID;
    int width1 = width;
    int height1 = height;
    string anim1 = anim;
    int hitpoints = 10;
    float construction_time = 3f;
    float[] tieR0 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
    string[] refinedMetals = MATERIALS.REFINED_METALS;
    float melting_point = 1600f;
    BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
    EffectorValues none = NOISE_POLLUTION.NONE;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width1, height1, anim1, hitpoints, construction_time, tieR0, refinedMetals, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER0, none, 0.2f);
    buildingDef.ViewMode = OverlayModes.Logic.ID;
    buildingDef.ObjectLayer = ObjectLayer.LogicGates;
    buildingDef.SceneLayer = Grid.SceneLayer.LogicGates;
    buildingDef.ThermalConductivity = 0.05f;
    buildingDef.Floodable = false;
    buildingDef.Overheatable = false;
    buildingDef.Entombable = false;
    buildingDef.AudioCategory = "Metal";
    buildingDef.AudioSize = "small";
    buildingDef.BaseTimeUntilRepair = -1f;
    buildingDef.PermittedRotations = PermittedRotations.R360;
    buildingDef.DragBuild = true;
    LogicGateBase.uiSrcData = Assets.instance.logicModeUIData;
    GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, ID);
    return buildingDef;
  }

  protected abstract LogicGateBase.Op GetLogicOp();

  protected abstract LogicGate.LogicGateDescriptions GetDescriptions();

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    GeneratedBuildings.MakeBuildingAlwaysOperational(go);
    BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof (RequiresFoundation), prefab_tag);
  }

  public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
  {
    base.DoPostConfigurePreview(def, go);
    go.AddComponent<MoveableLogicGateVisualizer>().op = this.GetLogicOp();
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
    base.DoPostConfigureUnderConstruction(go);
    go.AddComponent<LogicGateVisualizer>().op = this.GetLogicOp();
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.AddComponent<LogicGate>().op = this.GetLogicOp();
    go.GetComponent<KPrefabID>().prefabInitFn += (KPrefabID.PrefabFn) (game_object => game_object.GetComponent<LogicGate>().SetPortDescriptions(this.GetDescriptions()));
  }
}
