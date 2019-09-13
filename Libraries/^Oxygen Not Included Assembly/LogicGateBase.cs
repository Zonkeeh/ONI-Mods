// Decompiled with JetBrains decompiler
// Type: LogicGateBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class LogicGateBase : KMonoBehaviour
{
  public static CellOffset[] portOffsets = new CellOffset[3]
  {
    CellOffset.none,
    new CellOffset(0, 1),
    new CellOffset(1, 0)
  };
  public static LogicModeUI uiSrcData;
  [SerializeField]
  public LogicGateBase.Op op;

  private int GetActualCell(CellOffset offset)
  {
    Rotatable component = this.GetComponent<Rotatable>();
    if ((Object) component != (Object) null)
      offset = component.GetRotatedCellOffset(offset);
    return Grid.OffsetCell(Grid.PosToCell(this.transform.GetPosition()), offset);
  }

  public int InputCellOne
  {
    get
    {
      return this.GetActualCell(LogicGateBase.portOffsets[0]);
    }
  }

  public int InputCellTwo
  {
    get
    {
      return this.GetActualCell(LogicGateBase.portOffsets[1]);
    }
  }

  public int OutputCell
  {
    get
    {
      return this.GetActualCell(LogicGateBase.portOffsets[2]);
    }
  }

  public int PortCell(LogicGateBase.PortId port)
  {
    switch (port)
    {
      case LogicGateBase.PortId.InputOne:
        return this.InputCellOne;
      case LogicGateBase.PortId.InputTwo:
        return this.InputCellTwo;
      default:
        return this.OutputCell;
    }
  }

  public bool TryGetPortAtCell(int cell, out LogicGateBase.PortId port)
  {
    if (cell == this.InputCellOne)
    {
      port = LogicGateBase.PortId.InputOne;
      return true;
    }
    if (cell == this.InputCellTwo && this.RequiresTwoInputs)
    {
      port = LogicGateBase.PortId.InputTwo;
      return true;
    }
    if (cell == this.OutputCell)
    {
      port = LogicGateBase.PortId.Output;
      return true;
    }
    port = LogicGateBase.PortId.InputOne;
    return false;
  }

  public bool RequiresTwoInputs
  {
    get
    {
      return LogicGateBase.OpRequiresTwoInputs(this.op);
    }
  }

  public static bool OpRequiresTwoInputs(LogicGateBase.Op op)
  {
    return op != LogicGateBase.Op.Not && op != LogicGateBase.Op.CustomSingle;
  }

  public enum PortId
  {
    InputOne,
    InputTwo,
    Output,
  }

  public enum Op
  {
    And,
    Or,
    Not,
    Xor,
    CustomSingle,
  }
}
