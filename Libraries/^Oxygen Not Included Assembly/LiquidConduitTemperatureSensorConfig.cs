// Decompiled with JetBrains decompiler
// Type: LiquidConduitTemperatureSensorConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class LiquidConduitTemperatureSensorConfig : ConduitSensorConfig
{
  public static string ID = "LiquidConduitTemperatureSensor";
  public static readonly LogicPorts.Port OUTPUT_PORT = LogicPorts.Port.OutputPort(LogicSwitch.PORT_ID, new CellOffset(0, 0), (string) STRINGS.BUILDINGS.PREFABS.LIQUIDCONDUITTEMPERATURESENSOR.LOGIC_PORT, (string) STRINGS.BUILDINGS.PREFABS.LIQUIDCONDUITTEMPERATURESENSOR.LOGIC_PORT_ACTIVE, (string) STRINGS.BUILDINGS.PREFABS.LIQUIDCONDUITTEMPERATURESENSOR.LOGIC_PORT_INACTIVE, true, false);

  protected override ConduitType ConduitType
  {
    get
    {
      return ConduitType.Liquid;
    }
  }

  public override BuildingDef CreateBuildingDef()
  {
    return this.CreateBuildingDef(LiquidConduitTemperatureSensorConfig.ID, "liquid_temperature_sensor_kanim", TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER0, MATERIALS.REFINED_METALS);
  }

  public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, LiquidConduitTemperatureSensorConfig.OUTPUT_PORT);
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, LiquidConduitTemperatureSensorConfig.OUTPUT_PORT);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    base.DoPostConfigureComplete(go);
    GeneratedBuildings.RegisterLogicPorts(go, LiquidConduitTemperatureSensorConfig.OUTPUT_PORT);
    ConduitTemperatureSensor temperatureSensor = go.AddComponent<ConduitTemperatureSensor>();
    temperatureSensor.conduitType = this.ConduitType;
    temperatureSensor.Threshold = 280f;
    temperatureSensor.ActivateAboveThreshold = true;
    temperatureSensor.manuallyControlled = false;
    temperatureSensor.rangeMin = 0.0f;
    temperatureSensor.rangeMax = 9999f;
    temperatureSensor.defaultState = false;
  }
}
