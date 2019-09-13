// Decompiled with JetBrains decompiler
// Type: LogicPowerRelayConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class LogicPowerRelayConfig : IBuildingConfig
{
  public static string ID = "LogicPowerRelay";
  private static readonly LogicPorts.Port[] INPUT_PORTS = new LogicPorts.Port[1]
  {
    LogicPorts.Port.InputPort(LogicOperationalController.PORT_ID, new CellOffset(0, 0), (string) STRINGS.BUILDINGS.PREFABS.LOGICPOWERRELAY.LOGIC_PORT, (string) STRINGS.BUILDINGS.PREFABS.LOGICPOWERRELAY.LOGIC_PORT_ACTIVE, (string) STRINGS.BUILDINGS.PREFABS.LOGICPOWERRELAY.LOGIC_PORT_INACTIVE, true, false)
  };

  public override BuildingDef CreateBuildingDef()
  {
    string id = LogicPowerRelayConfig.ID;
    int width = 1;
    int height = 1;
    string anim = "switchpowershutoff_kanim";
    int hitpoints = 10;
    float construction_time = 30f;
    float[] tieR2 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
    string[] allMetals = MATERIALS.ALL_METALS;
    float melting_point = 1600f;
    BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
    EffectorValues none = TUNING.NOISE_POLLUTION.NONE;
    BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR2, allMetals, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.NONE, none, 0.2f);
    buildingDef.Overheatable = false;
    buildingDef.Floodable = false;
    buildingDef.Entombable = false;
    buildingDef.ViewMode = OverlayModes.Power.ID;
    buildingDef.AudioCategory = "Metal";
    buildingDef.SceneLayer = Grid.SceneLayer.Building;
    SoundEventVolumeCache.instance.AddVolume("switchpower_kanim", "PowerSwitch_on", TUNING.NOISE_POLLUTION.NOISY.TIER3);
    SoundEventVolumeCache.instance.AddVolume("switchpower_kanim", "PowerSwitch_off", TUNING.NOISE_POLLUTION.NOISY.TIER3);
    GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, LogicPowerRelayConfig.ID);
    return buildingDef;
  }

  public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, LogicPowerRelayConfig.INPUT_PORTS);
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, LogicPowerRelayConfig.INPUT_PORTS);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    Object.DestroyImmediate((Object) go.GetComponent<BuildingEnabledButton>());
    GeneratedBuildings.RegisterLogicPorts(go, LogicPowerRelayConfig.INPUT_PORTS);
    go.AddOrGet<LogicOperationalController>();
    go.AddOrGet<OperationalControlledSwitch>().objectLayer = ObjectLayer.Wire;
  }
}
