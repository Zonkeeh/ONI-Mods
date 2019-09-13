// Decompiled with JetBrains decompiler
// Type: NoiseSplat
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class NoiseSplat : IUniformGridObject
{
  private List<Pair<int, float>> decibels = new List<Pair<int, float>>();
  public const float noiseFalloff = 0.05f;
  private IPolluter provider;
  private Vector2 position;
  private int radius;
  private Extents effectExtents;
  private Extents baseExtents;
  private HandleVector<int>.Handle partitionerEntry;
  private HandleVector<int>.Handle solidChangedPartitionerEntry;

  public NoiseSplat(NoisePolluter setProvider, float death_time = 0.0f)
  {
    this.deathTime = death_time;
    this.dB = 0;
    this.radius = 5;
    if (setProvider.dB != null)
      this.dB = (int) setProvider.dB.GetTotalValue();
    int cell = Grid.PosToCell(setProvider.gameObject);
    if (!NoisePolluter.IsNoiseableCell(cell))
      this.dB = 0;
    if (this.dB == 0)
      return;
    setProvider.Clear();
    OccupyArea occupyArea = setProvider.occupyArea;
    this.baseExtents = occupyArea.GetExtents();
    this.provider = (IPolluter) setProvider;
    this.position = (Vector2) setProvider.transform.GetPosition();
    if (setProvider.dBRadius != null)
      this.radius = (int) setProvider.dBRadius.GetTotalValue();
    if (this.radius == 0)
      return;
    int x = 0;
    int y = 0;
    Grid.CellToXY(cell, out x, out y);
    int widthInCells = occupyArea.GetWidthInCells();
    int heightInCells = occupyArea.GetHeightInCells();
    Vector2I v1 = new Vector2I(x - this.radius, y - this.radius);
    Vector2I v2 = v1 + new Vector2I(this.radius * 2 + widthInCells, this.radius * 2 + heightInCells);
    Vector2I vector2I1 = Vector2I.Max(v1, Vector2I.zero);
    Vector2I vector2I2 = Vector2I.Min(v2, new Vector2I(Grid.WidthInCells - 1, Grid.HeightInCells - 1));
    this.effectExtents = new Extents(vector2I1.x, vector2I1.y, vector2I2.x - vector2I1.x, vector2I2.y - vector2I1.y);
    this.partitionerEntry = GameScenePartitioner.Instance.Add("NoiseSplat.SplatCollectNoisePolluters", (object) setProvider.gameObject, this.effectExtents, GameScenePartitioner.Instance.noisePolluterLayer, setProvider.onCollectNoisePollutersCallback);
    this.solidChangedPartitionerEntry = GameScenePartitioner.Instance.Add("NoiseSplat.SplatSolidCheck", (object) setProvider.gameObject, this.effectExtents, GameScenePartitioner.Instance.solidChangedLayer, setProvider.refreshPartionerCallback);
  }

  public NoiseSplat(IPolluter setProvider, float death_time = 0.0f)
  {
    this.deathTime = death_time;
    this.provider = setProvider;
    this.provider.Clear();
    this.position = this.provider.GetPosition();
    this.dB = this.provider.GetNoise();
    int cell = Grid.PosToCell(this.position);
    if (!NoisePolluter.IsNoiseableCell(cell))
      this.dB = 0;
    if (this.dB == 0)
      return;
    this.radius = this.provider.GetRadius();
    if (this.radius == 0)
      return;
    int x = 0;
    int y = 0;
    Grid.CellToXY(cell, out x, out y);
    Vector2I v1 = new Vector2I(x - this.radius, y - this.radius);
    Vector2I v2 = v1 + new Vector2I(this.radius * 2, this.radius * 2);
    Vector2I vector2I1 = Vector2I.Max(v1, Vector2I.zero);
    Vector2I vector2I2 = Vector2I.Min(v2, new Vector2I(Grid.WidthInCells - 1, Grid.HeightInCells - 1));
    this.effectExtents = new Extents(vector2I1.x, vector2I1.y, vector2I2.x - vector2I1.x, vector2I2.y - vector2I1.y);
    this.baseExtents = new Extents(x, y, 1, 1);
    this.AddNoise();
  }

  public int dB { get; private set; }

  public float deathTime { get; private set; }

  public string GetName()
  {
    return this.provider.GetName();
  }

  public IPolluter GetProvider()
  {
    return this.provider;
  }

  public Vector2 PosMin()
  {
    return new Vector2(this.position.x - (float) this.radius, this.position.y - (float) this.radius);
  }

  public Vector2 PosMax()
  {
    return new Vector2(this.position.x + (float) this.radius, this.position.y + (float) this.radius);
  }

  public void Clear()
  {
    GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
    GameScenePartitioner.Instance.Free(ref this.solidChangedPartitionerEntry);
    this.RemoveNoise();
  }

  private void AddNoise()
  {
    int cell1 = Grid.PosToCell(this.position);
    int val1_1 = this.effectExtents.x + this.effectExtents.width;
    int val1_2 = this.effectExtents.y + this.effectExtents.height;
    int x1 = this.effectExtents.x;
    int y1 = this.effectExtents.y;
    int x2 = 0;
    int y2 = 0;
    Grid.CellToXY(cell1, out x2, out y2);
    int num1 = Math.Min(val1_1, Grid.WidthInCells);
    int num2 = Math.Min(val1_2, Grid.HeightInCells);
    int num3 = Math.Max(0, x1);
    for (int index1 = Math.Max(0, y1); index1 < num2; ++index1)
    {
      for (int index2 = num3; index2 < num1; ++index2)
      {
        if (Grid.VisibilityTest(x2, y2, index2, index1, false))
        {
          int cell2 = Grid.XYToCell(index2, index1);
          float dbForCell = this.GetDBForCell(cell2);
          if ((double) dbForCell > 0.0)
          {
            float loudness = AudioEventManager.DBToLoudness(dbForCell);
            Grid.Loudness[cell2] += loudness;
            this.decibels.Add(new Pair<int, float>(cell2, loudness));
          }
        }
      }
    }
  }

  public float GetDBForCell(int cell)
  {
    Vector2 pos2D = (Vector2) Grid.CellToPos2D(cell);
    float num = Mathf.Floor(Vector2.Distance(this.position, pos2D));
    if ((double) pos2D.x >= (double) this.baseExtents.x && (double) pos2D.x < (double) (this.baseExtents.x + this.baseExtents.width) && ((double) pos2D.y >= (double) this.baseExtents.y && (double) pos2D.y < (double) (this.baseExtents.y + this.baseExtents.height)))
      num = 0.0f;
    return Mathf.Round((float) this.dB - (float) ((double) this.dB * (double) num * 0.0500000007450581));
  }

  private void RemoveNoise()
  {
    for (int index = 0; index < this.decibels.Count; ++index)
    {
      Pair<int, float> decibel = this.decibels[index];
      float num = Math.Max(0.0f, Grid.Loudness[decibel.first] - decibel.second);
      Grid.Loudness[decibel.first] = (double) num >= 1.0 ? num : 0.0f;
    }
    this.decibels.Clear();
  }

  public float GetLoudness(int cell)
  {
    float num = 0.0f;
    for (int index = 0; index < this.decibels.Count; ++index)
    {
      Pair<int, float> decibel = this.decibels[index];
      if (decibel.first == cell)
      {
        num = decibel.second;
        break;
      }
    }
    return num;
  }
}
