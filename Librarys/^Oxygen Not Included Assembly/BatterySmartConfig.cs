// Decompiled with JetBrains decompiler
// Type: BatterySmartConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class BatterySmartConfig : BaseBatteryConfig
{
  private static readonly LogicPorts.Port[] OUTPUT_PORTS = new LogicPorts.Port[1]
  {
    LogicPorts.Port.OutputPort(BatterySmart.PORT_ID, new CellOffset(0, 0), (string) STRINGS.BUILDINGS.PREFABS.BATTERYSMART.LOGIC_PORT, (string) STRINGS.BUILDINGS.PREFABS.BATTERYSMART.LOGIC_PORT_ACTIVE, (string) STRINGS.BUILDINGS.PREFABS.BATTERYSMART.LOGIC_PORT_INACTIVE, true, false)
  };
  public const string ID = "BatterySmart";

  public override BuildingDef CreateBuildingDef()
  {
    string id = "BatterySmart";
    int width = 2;
    int height = 2;
    int hitpoints = 30;
    string anim = "smartbattery_kanim";
    float construction_time = 60f;
    float[] tieR3 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
    string[] refinedMetals = MATERIALS.REFINED_METALS;
    float melting_point = 800f;
    float exhaust_temperature_active = 0.0f;
    float self_heat_kilowatts_active = 0.5f;
    EffectorValues tieR1 = TUNING.NOISE_POLLUTION.NOISY.TIER1;
    BuildingDef buildingDef = this.CreateBuildingDef(id, width, height, hitpoints, anim, construction_time, tieR3, refinedMetals, melting_point, exhaust_temperature_active, self_heat_kilowatts_active, TUNING.BUILDINGS.DECOR.PENALTY.TIER2, tieR1);
    SoundEventVolumeCache.instance.AddVolume("batterymed_kanim", "Battery_med_rattle", TUNING.NOISE_POLLUTION.NOISY.TIER2);
    return buildingDef;
  }

  public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, (LogicPorts.Port[]) null, BatterySmartConfig.OUTPUT_PORTS);
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, (LogicPorts.Port[]) null, BatterySmartConfig.OUTPUT_PORTS);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    BatterySmart batterySmart = go.AddOrGet<BatterySmart>();
    batterySmart.capacity = 20000f;
    batterySmart.joulesLostPerSecond = 0.6666667f;
    batterySmart.powerSortOrder = 1000;
    GeneratedBuildings.RegisterLogicPorts(go, (LogicPorts.Port[]) null, BatterySmartConfig.OUTPUT_PORTS);
    base.DoPostConfigureComplete(go);
  }
}
