// Decompiled with JetBrains decompiler
// Type: LogicEventHandler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using UnityEngine;

internal class LogicEventHandler : ILogicEventReceiver, ILogicUIElement, ILogicNetworkConnection, IUniformGridObject
{
  private int cell;
  private int value;
  private System.Action<int> onValueChanged;
  private System.Action<int, bool> onConnectionChanged;
  private LogicPortSpriteType spriteType;

  public LogicEventHandler(
    int cell,
    System.Action<int> on_value_changed,
    System.Action<int, bool> on_connection_changed,
    LogicPortSpriteType sprite_type)
  {
    this.cell = cell;
    this.onValueChanged = on_value_changed;
    this.onConnectionChanged = on_connection_changed;
    this.spriteType = sprite_type;
  }

  public void ReceiveLogicEvent(int value)
  {
    this.TriggerAudio(value);
    this.value = value;
    this.onValueChanged(value);
  }

  public int Value
  {
    get
    {
      return this.value;
    }
  }

  public int GetLogicUICell()
  {
    return this.cell;
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

  public int GetLogicCell()
  {
    return this.cell;
  }

  private void TriggerAudio(int new_value)
  {
    LogicCircuitNetwork networkForCell = Game.Instance.logicCircuitManager.GetNetworkForCell(this.cell);
    SpeedControlScreen instance1 = SpeedControlScreen.Instance;
    if (networkForCell == null || new_value == this.value || (!((UnityEngine.Object) instance1 != (UnityEngine.Object) null) || instance1.IsPaused) || KPlayerPrefs.HasKey(AudioOptionsScreen.AlwaysPlayAutomation) && KPlayerPrefs.GetInt(AudioOptionsScreen.AlwaysPlayAutomation) != 1 && OverlayScreen.Instance.GetMode() != OverlayModes.Logic.ID)
      return;
    string name = "Logic_Building_Toggle";
    if (!CameraController.Instance.IsAudibleSound((Vector2) Grid.CellToPosCCC(this.cell, Grid.SceneLayer.BuildingFront)))
      return;
    EventInstance instance2 = KFMOD.BeginOneShot(GlobalAssets.GetSound(name, false), Grid.CellToPos(this.cell));
    int num1 = (int) instance2.setParameterValue("wireCount", (float) (networkForCell.Wires.Count % 24));
    int num2 = (int) instance2.setParameterValue("enabled", (float) new_value);
    KFMOD.EndOneShot(instance2);
  }

  public void OnLogicNetworkConnectionChanged(bool connected)
  {
    if (this.onConnectionChanged == null)
      return;
    this.onConnectionChanged(this.cell, connected);
  }
}
