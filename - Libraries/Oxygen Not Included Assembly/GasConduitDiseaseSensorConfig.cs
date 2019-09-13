// Decompiled with JetBrains decompiler
// Type: GasConduitDiseaseSensorConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class GasConduitDiseaseSensorConfig : ConduitSensorConfig
{
  public static string ID = "GasConduitDiseaseSensor";
  public static readonly LogicPorts.Port OUTPUT_PORT = LogicPorts.Port.OutputPort(LogicSwitch.PORT_ID, new CellOffset(0, 0), (string) STRINGS.BUILDINGS.PREFABS.GASCONDUITDISEASESENSOR.LOGIC_PORT, (string) STRINGS.BUILDINGS.PREFABS.GASCONDUITDISEASESENSOR.LOGIC_PORT_ACTIVE, (string) STRINGS.BUILDINGS.PREFABS.GASCONDUITDISEASESENSOR.LOGIC_PORT_INACTIVE, true, false);

  protected override ConduitType ConduitType
  {
    get
    {
      return ConduitType.Gas;
    }
  }

  public override BuildingDef CreateBuildingDef()
  {
    return this.CreateBuildingDef(GasConduitDiseaseSensorConfig.ID, "gas_germs_sensor_kanim", new float[2]
    {
      TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER0[0],
      TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER1[0]
    }, new string[2]{ "RefinedMetal", "Plastic" });
  }

  public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, GasConduitDiseaseSensorConfig.OUTPUT_PORT);
  }

  public override void DoPostConfigureUnderConstruction(GameObject go)
  {
    GeneratedBuildings.RegisterLogicPorts(go, GasConduitDiseaseSensorConfig.OUTPUT_PORT);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    base.DoPostConfigureComplete(go);
    GeneratedBuildings.RegisterLogicPorts(go, GasConduitDiseaseSensorConfig.OUTPUT_PORT);
    ConduitDiseaseSensor conduitDiseaseSensor = go.AddComponent<ConduitDiseaseSensor>();
    conduitDiseaseSensor.conduitType = this.ConduitType;
    conduitDiseaseSensor.Threshold = 0.0f;
    conduitDiseaseSensor.ActivateAboveThreshold = true;
    conduitDiseaseSensor.manuallyControlled = false;
    conduitDiseaseSensor.defaultState = false;
  }
}
