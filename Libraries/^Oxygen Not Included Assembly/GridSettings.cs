// Decompiled with JetBrains decompiler
// Type: GridSettings
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class GridSettings : KMonoBehaviour
{
  public int SimChunkEdgeSize = 32;
  public int GridWidthInCells;
  public int GridHeightInCells;
  public const float CellSizeInMeters = 1f;

  public static void Reset(int width, int height)
  {
    Grid.WidthInCells = width;
    Grid.HeightInCells = height;
    Grid.CellCount = width * height;
    Grid.WidthInMeters = 1f * (float) width;
    Grid.HeightInMeters = 1f * (float) height;
    Grid.CellSizeInMeters = 1f;
    Grid.HalfCellSizeInMeters = 0.5f;
    Grid.Element = new Element[Grid.CellCount];
    Grid.VisMasks = new Grid.VisFlags[Grid.CellCount];
    Grid.Visible = new byte[Grid.CellCount];
    Grid.Spawnable = new byte[Grid.CellCount];
    Grid.BuildMasks = new Grid.BuildFlags[Grid.CellCount];
    Grid.LightCount = new int[Grid.CellCount];
    Grid.RadiationCount = new int[Grid.CellCount];
    Grid.Damage = new float[Grid.CellCount];
    Grid.NavMasks = new Grid.NavFlags[Grid.CellCount];
    Grid.NavValidatorMasks = new Grid.NavValidatorFlags[Grid.CellCount];
    Grid.Decor = new float[Grid.CellCount];
    Grid.Loudness = new float[Grid.CellCount];
    Grid.GravitasFacility = new bool[Grid.CellCount];
    Grid.ObjectLayers = new Dictionary<int, GameObject>[39];
    for (int index = 0; index < Grid.ObjectLayers.Length; ++index)
      Grid.ObjectLayers[index] = new Dictionary<int, GameObject>();
    for (int index = 0; index < Grid.CellCount; ++index)
      Grid.Loudness[index] = 0.0f;
    if ((UnityEngine.Object) Game.Instance != (UnityEngine.Object) null)
    {
      Game.Instance.gasConduitSystem.Initialize(Grid.WidthInCells, Grid.HeightInCells);
      Game.Instance.liquidConduitSystem.Initialize(Grid.WidthInCells, Grid.HeightInCells);
      Game.Instance.electricalConduitSystem.Initialize(Grid.WidthInCells, Grid.HeightInCells);
      Game.Instance.travelTubeSystem.Initialize(Grid.WidthInCells, Grid.HeightInCells);
      Game.Instance.gasConduitFlow.Initialize(Grid.CellCount);
      Game.Instance.liquidConduitFlow.Initialize(Grid.CellCount);
    }
    Grid.OnReveal = (System.Action<int>) null;
  }

  public static void ClearGrid()
  {
    Grid.WidthInCells = 0;
    Grid.HeightInCells = 0;
    Grid.CellCount = 0;
    Grid.WidthInMeters = 0.0f;
    Grid.HeightInMeters = 0.0f;
    Grid.CellSizeInMeters = 0.0f;
    Grid.HalfCellSizeInMeters = 0.0f;
    Grid.Element = (Element[]) null;
    Grid.VisMasks = (Grid.VisFlags[]) null;
    Grid.Visible = (byte[]) null;
    Grid.Spawnable = (byte[]) null;
    Grid.BuildMasks = (Grid.BuildFlags[]) null;
    Grid.NavValidatorMasks = (Grid.NavValidatorFlags[]) null;
    Grid.LightCount = (int[]) null;
    Grid.RadiationCount = (int[]) null;
    Grid.Damage = (float[]) null;
    Grid.Decor = (float[]) null;
    Grid.Loudness = (float[]) null;
    Grid.GravitasFacility = (bool[]) null;
    Grid.ObjectLayers = (Dictionary<int, GameObject>[]) null;
    Grid.ResetNavMasksAndDetails();
  }
}
