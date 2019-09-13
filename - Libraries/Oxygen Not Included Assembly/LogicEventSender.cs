// Decompiled with JetBrains decompiler
// Type: LogicEventSender
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

internal class LogicEventSender : ILogicEventSender, ILogicUIElement, ILogicNetworkConnection, IUniformGridObject
{
  private HashedString id;
  private int cell;
  private int logicValue;
  private System.Action<int> onValueChanged;
  private System.Action<int, bool> onConnectionChanged;
  private LogicPortSpriteType spriteType;

  public LogicEventSender(
    HashedString id,
    int cell,
    System.Action<int> on_value_changed,
    System.Action<int, bool> on_connection_changed,
    LogicPortSpriteType sprite_type)
  {
    this.id = id;
    this.cell = cell;
    this.onValueChanged = on_value_changed;
    this.onConnectionChanged = on_connection_changed;
    this.spriteType = sprite_type;
  }

  public HashedString ID
  {
    get
    {
      return this.id;
    }
  }

  public int GetLogicCell()
  {
    return this.cell;
  }

  public int GetLogicValue()
  {
    return this.logicValue;
  }

  public int GetLogicUICell()
  {
    return this.GetLogicCell();
  }

  public LogicPortSpriteType GetLogicPortSpriteType()
  {
    return this.spriteType;
  }

  public Vector2 PosMin()
  {
    return (Vector2) Grid.CellToPos2D(this.cell);
  }

  public Vector2 PosMax()
  {
    return (Vector2) Grid.CellToPos2D(this.cell);
  }

  public void SetValue(int value)
  {
    this.logicValue = value;
    this.onValueChanged(value);
  }

  public void LogicTick()
  {
  }

  public void OnLogicNetworkConnectionChanged(bool connected)
  {
    if (this.onConnectionChanged == null)
      return;
    this.onConnectionChanged(this.cell, connected);
  }
}
