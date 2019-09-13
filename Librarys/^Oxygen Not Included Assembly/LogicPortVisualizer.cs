// Decompiled with JetBrains decompiler
// Type: LogicPortVisualizer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class LogicPortVisualizer : ILogicUIElement, IUniformGridObject
{
  private int cell;
  private LogicPortSpriteType spriteType;

  public LogicPortVisualizer(int cell, LogicPortSpriteType sprite_type)
  {
    this.cell = cell;
    this.spriteType = sprite_type;
  }

  public int GetLogicUICell()
  {
    return this.cell;
  }

  public Vector2 PosMin()
  {
    return (Vector2) Grid.CellToPos2D(this.cell);
  }

  public Vector2 PosMax()
  {
    return (Vector2) Grid.CellToPos2D(this.cell);
  }

  public LogicPortSpriteType GetLogicPortSpriteType()
  {
    return this.spriteType;
  }
}
