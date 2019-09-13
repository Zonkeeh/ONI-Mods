// Decompiled with JetBrains decompiler
// Type: LogicGateNotConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;

public class LogicGateNotConfig : LogicGateBaseConfig
{
  public const string ID = "LogicGateNOT";

  protected override LogicGateBase.Op GetLogicOp()
  {
    return LogicGateBase.Op.Not;
  }

  protected override LogicGate.LogicGateDescriptions GetDescriptions()
  {
    return new LogicGate.LogicGateDescriptions()
    {
      output = new LogicGate.LogicGateDescriptions.Description()
      {
        name = (string) BUILDINGS.PREFABS.LOGICGATENOT.OUTPUT_NAME,
        active = (string) BUILDINGS.PREFABS.LOGICGATENOT.OUTPUT_ACTIVE,
        inactive = (string) BUILDINGS.PREFABS.LOGICGATENOT.OUTPUT_INACTIVE
      }
    };
  }

  public override BuildingDef CreateBuildingDef()
  {
    return this.CreateBuildingDef("LogicGateNOT", "logic_not_kanim", 2, 1);
  }
}
