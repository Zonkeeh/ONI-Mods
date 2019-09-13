// Decompiled with JetBrains decompiler
// Type: LogicGateVisualizer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

[SkipSaveFileSerialization]
public class LogicGateVisualizer : LogicGateBase
{
  private List<LogicGateVisualizer.IOVisualizer> visChildren = new List<LogicGateVisualizer.IOVisualizer>();

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Register();
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    this.Unregister();
  }

  private void Register()
  {
    this.Unregister();
    this.visChildren.Add(new LogicGateVisualizer.IOVisualizer(this.OutputCell, false));
    this.visChildren.Add(new LogicGateVisualizer.IOVisualizer(this.InputCellOne, true));
    if (this.RequiresTwoInputs)
      this.visChildren.Add(new LogicGateVisualizer.IOVisualizer(this.InputCellTwo, true));
    LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
    foreach (LogicGateVisualizer.IOVisualizer visChild in this.visChildren)
      logicCircuitManager.AddVisElem((ILogicUIElement) visChild);
  }

  private void Unregister()
  {
    LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
    foreach (LogicGateVisualizer.IOVisualizer visChild in this.visChildren)
      logicCircuitManager.RemoveVisElem((ILogicUIElement) visChild);
    this.visChildren.Clear();
  }

  private class IOVisualizer : ILogicUIElement, IUniformGridObject
  {
    private int cell;
    private bool input;

    public IOVisualizer(int cell, bool input)
    {
      this.cell = cell;
      this.input = input;
    }

    public int GetLogicUICell()
    {
      return this.cell;
    }

    public LogicPortSpriteType GetLogicPortSpriteType()
    {
      return this.input ? LogicPortSpriteType.Input : LogicPortSpriteType.Output;
    }

    public Vector2 PosMin()
    {
      return (Vector2) Grid.CellToPos2D(this.cell);
    }

    public Vector2 PosMax()
    {
      return this.PosMin();
    }
  }
}
