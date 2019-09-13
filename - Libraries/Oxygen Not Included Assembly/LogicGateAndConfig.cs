// Decompiled with JetBrains decompiler
// Type: LogicGateAndConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;

public class LogicGateAndConfig : LogicGateBaseConfig
{
  public const string ID = "LogicGateAND";

  protected override LogicGateBase.Op GetLogicOp()
  {
    return LogicGateBase.Op.And;
  }

  protected override LogicGate.LogicGateDescriptions GetDescriptions()
  {
    return new LogicGate.LogicGateDescriptions()
    {
      output = new LogicGate.LogicGateDescriptions.Description()
      {
        name = (string) BUILDINGS.PREFABS.LOGICGATEAND.OUTPUT_NAME,
        active = (string) BUILDINGS.PREFABS.LOGICGATEAND.OUTPUT_ACTIVE,
        inactive = (string) BUILDINGS.PREFABS.LOGICGATEAND.OUTPUT_INACTIVE
      }
    };
  }

  public override BuildingDef CreateBuildingDef()
  {
    return this.CreateBuildingDef("LogicGateAND", "logic_and_kanim", 2, 2);
  }
}
