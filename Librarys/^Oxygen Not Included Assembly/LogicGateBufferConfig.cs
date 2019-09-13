// Decompiled with JetBrains decompiler
// Type: LogicGateBufferConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class LogicGateBufferConfig : LogicGateBaseConfig
{
  public const string ID = "LogicGateBUFFER";

  protected override LogicGateBase.Op GetLogicOp()
  {
    return LogicGateBase.Op.CustomSingle;
  }

  protected override LogicGate.LogicGateDescriptions GetDescriptions()
  {
    return new LogicGate.LogicGateDescriptions()
    {
      output = new LogicGate.LogicGateDescriptions.Description()
      {
        name = (string) BUILDINGS.PREFABS.LOGICGATEBUFFER.OUTPUT_NAME,
        active = (string) BUILDINGS.PREFABS.LOGICGATEBUFFER.OUTPUT_ACTIVE,
        inactive = (string) BUILDINGS.PREFABS.LOGICGATEBUFFER.OUTPUT_INACTIVE
      }
    };
  }

  public override BuildingDef CreateBuildingDef()
  {
    return this.CreateBuildingDef("LogicGateBUFFER", "logic_buffer_kanim", 2, 1);
  }

  public override void DoPostConfigureComplete(GameObject go)
  {
    go.AddComponent<LogicGateBuffer>().op = this.GetLogicOp();
    go.GetComponent<KPrefabID>().prefabInitFn += (KPrefabID.PrefabFn) (game_object => game_object.GetComponent<LogicGateBuffer>().SetPortDescriptions(this.GetDescriptions()));
  }
}
