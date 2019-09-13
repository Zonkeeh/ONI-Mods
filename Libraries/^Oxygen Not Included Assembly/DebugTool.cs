// Decompiled with JetBrains decompiler
// Type: DebugTool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class DebugTool : DragTool
{
  public static DebugTool Instance;
  public DebugTool.Type type;

  public static void DestroyInstance()
  {
    DebugTool.Instance = (DebugTool) null;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    DebugTool.Instance = this;
  }

  public void Activate()
  {
    PlayerController.Instance.ActivateTool((InterfaceTool) this);
  }

  public void Activate(DebugTool.Type type)
  {
    this.type = type;
    this.Activate();
  }

  protected override void OnDeactivateTool(InterfaceTool new_tool)
  {
    base.OnDeactivateTool(new_tool);
    PlayerController.Instance.ToolDeactivated((InterfaceTool) this);
  }

  protected override void OnDragTool(int cell, int distFromOrigin)
  {
    if (!Grid.IsValidCell(cell))
      return;
    switch (this.type)
    {
      case DebugTool.Type.Dig:
        SimMessages.Dig(cell, -1);
        break;
      case DebugTool.Type.Heat:
        SimMessages.ModifyEnergy(cell, 10000f, 10000f, SimMessages.EnergySourceID.DebugHeat);
        break;
      case DebugTool.Type.Cool:
        SimMessages.ModifyEnergy(cell, -10000f, 10000f, SimMessages.EnergySourceID.DebugCool);
        break;
      case DebugTool.Type.ReplaceSubstance:
        this.DoReplaceSubstance(cell);
        break;
      case DebugTool.Type.FillReplaceSubstance:
        GameUtil.FloodFillNext.Clear();
        GameUtil.FloodFillVisited.Clear();
        SimHashes elem_hash = Grid.Element[cell].id;
        GameUtil.FloodFillConditional(cell, (Func<int, bool>) (check_cell =>
        {
          bool flag = false;
          if (Grid.Element[check_cell].id == elem_hash)
          {
            flag = true;
            this.DoReplaceSubstance(check_cell);
          }
          return flag;
        }), (ICollection<int>) GameUtil.FloodFillVisited, (ICollection<int>) null);
        break;
      case DebugTool.Type.AddPressure:
        SimMessages.ModifyMass(cell, 10000f, byte.MaxValue, 0, CellEventLogger.Instance.DebugToolModifyMass, 293f, SimHashes.Oxygen);
        break;
      case DebugTool.Type.RemovePressure:
        SimMessages.ModifyMass(cell, -10000f, byte.MaxValue, 0, CellEventLogger.Instance.DebugToolModifyMass, 0.0f, SimHashes.Oxygen);
        break;
      case DebugTool.Type.Clear:
        this.ClearCell(cell);
        break;
      case DebugTool.Type.AddSelection:
        DebugBaseTemplateButton.Instance.AddToSelection(cell);
        break;
      case DebugTool.Type.RemoveSelection:
        DebugBaseTemplateButton.Instance.RemoveFromSelection(cell);
        break;
      case DebugTool.Type.Deconstruct:
        this.DeconstructCell(cell);
        break;
      case DebugTool.Type.Destroy:
        this.DestroyCell(cell);
        break;
      case DebugTool.Type.Sample:
        DebugPaintElementScreen.Instance.SampleCell(cell);
        break;
    }
  }

  public void DoReplaceSubstance(int cell)
  {
    if (!Grid.IsValidBuildingCell(cell))
      return;
    Element element = (!DebugPaintElementScreen.Instance.paintElement.isOn ? ElementLoader.elements[(int) Grid.ElementIdx[cell]] : ElementLoader.FindElementByHash(DebugPaintElementScreen.Instance.element)) ?? ElementLoader.FindElementByHash(SimHashes.Vacuum);
    byte num1 = !DebugPaintElementScreen.Instance.paintDisease.isOn ? Grid.DiseaseIdx[cell] : DebugPaintElementScreen.Instance.diseaseIdx;
    float temperature = !DebugPaintElementScreen.Instance.paintTemperature.isOn ? Grid.Temperature[cell] : DebugPaintElementScreen.Instance.temperature;
    float mass = !DebugPaintElementScreen.Instance.paintMass.isOn ? Grid.Mass[cell] : DebugPaintElementScreen.Instance.mass;
    int num2 = !DebugPaintElementScreen.Instance.paintDiseaseCount.isOn ? Grid.DiseaseCount[cell] : DebugPaintElementScreen.Instance.diseaseCount;
    if ((double) temperature == -1.0)
      temperature = element.defaultValues.temperature;
    if ((double) mass == -1.0)
      mass = element.defaultValues.mass;
    if (DebugPaintElementScreen.Instance.affectCells.isOn)
    {
      SimMessages.ReplaceElement(cell, element.id, CellEventLogger.Instance.DebugTool, mass, temperature, num1, num2, -1);
      if (DebugPaintElementScreen.Instance.set_prevent_fow_reveal)
      {
        Grid.Visible[cell] = (byte) 0;
        Grid.PreventFogOfWarReveal[cell] = true;
      }
      else if (DebugPaintElementScreen.Instance.set_allow_fow_reveal && Grid.PreventFogOfWarReveal[cell])
        Grid.PreventFogOfWarReveal[cell] = false;
    }
    if (!DebugPaintElementScreen.Instance.affectBuildings.isOn)
      return;
    foreach (GameObject gameObject in new List<GameObject>()
    {
      Grid.Objects[cell, 1],
      Grid.Objects[cell, 2],
      Grid.Objects[cell, 9],
      Grid.Objects[cell, 16],
      Grid.Objects[cell, 12],
      Grid.Objects[cell, 16],
      Grid.Objects[cell, 26]
    })
    {
      if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
      {
        PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
        if ((double) temperature > 0.0)
          component.Temperature = temperature;
        if (num2 > 0 && num1 != byte.MaxValue)
        {
          component.ModifyDiseaseCount(int.MinValue, "DebugTool.DoReplaceSubstance");
          component.AddDisease(num1, num2, "DebugTool.DoReplaceSubstance");
        }
      }
    }
  }

  public void DeconstructCell(int cell)
  {
    bool instantBuildMode = DebugHandler.InstantBuildMode;
    DebugHandler.InstantBuildMode = true;
    DeconstructTool.Instance.DeconstructCell(cell);
    if (instantBuildMode)
      return;
    DebugHandler.InstantBuildMode = false;
  }

  public void DestroyCell(int cell)
  {
    foreach (GameObject gameObject in new List<GameObject>()
    {
      Grid.Objects[cell, 2],
      Grid.Objects[cell, 1],
      Grid.Objects[cell, 12],
      Grid.Objects[cell, 16],
      Grid.Objects[cell, 0],
      Grid.Objects[cell, 26]
    })
    {
      if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
        UnityEngine.Object.Destroy((UnityEngine.Object) gameObject);
    }
    this.ClearCell(cell);
    if (ElementLoader.elements[(int) Grid.ElementIdx[cell]].id == SimHashes.Void)
      SimMessages.ReplaceElement(cell, SimHashes.Void, CellEventLogger.Instance.DebugTool, 0.0f, 0.0f, byte.MaxValue, 0, -1);
    else
      SimMessages.ReplaceElement(cell, SimHashes.Vacuum, CellEventLogger.Instance.DebugTool, 0.0f, 0.0f, byte.MaxValue, 0, -1);
  }

  public void ClearCell(int cell)
  {
    Vector2I xy = Grid.CellToXY(cell);
    ListPool<ScenePartitionerEntry, DebugTool>.PooledList pooledList = ListPool<ScenePartitionerEntry, DebugTool>.Allocate();
    GameScenePartitioner.Instance.GatherEntries(xy.x, xy.y, 1, 1, GameScenePartitioner.Instance.pickupablesLayer, (List<ScenePartitionerEntry>) pooledList);
    for (int index = 0; index < pooledList.Count; ++index)
    {
      Pickupable pickupable = pooledList[index].obj as Pickupable;
      if ((UnityEngine.Object) pickupable != (UnityEngine.Object) null && (UnityEngine.Object) pickupable.GetComponent<MinionBrain>() == (UnityEngine.Object) null)
        Util.KDestroyGameObject(pickupable.gameObject);
    }
    pooledList.Recycle();
  }

  public enum Type
  {
    Dig,
    Heat,
    Cool,
    ReplaceSubstance,
    FillReplaceSubstance,
    AddPressure,
    RemovePressure,
    PaintPlant,
    Clear,
    AddSelection,
    RemoveSelection,
    Deconstruct,
    Destroy,
    Sample,
  }
}
