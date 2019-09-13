// Decompiled with JetBrains decompiler
// Type: SubworldZoneRenderData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Delaunay.Geo;
using Klei;
using ProcGen;
using System;
using UnityEngine;

public class SubworldZoneRenderData : KMonoBehaviour
{
  [SerializeField]
  [HideInInspector]
  public Color32[] zoneColours = new Color32[11]
  {
    new Color32((byte) 145, (byte) 198, (byte) 213, (byte) 0),
    new Color32((byte) 135, (byte) 82, (byte) 160, (byte) 1),
    new Color32((byte) 123, (byte) 151, (byte) 75, (byte) 2),
    new Color32((byte) 236, (byte) 189, (byte) 89, (byte) 3),
    new Color32((byte) 201, (byte) 152, (byte) 181, (byte) 4),
    new Color32((byte) 222, (byte) 90, (byte) 59, (byte) 5),
    new Color32((byte) 201, (byte) 152, (byte) 181, (byte) 6),
    new Color32(byte.MaxValue, (byte) 0, (byte) 0, (byte) 7),
    new Color32((byte) 201, (byte) 201, (byte) 151, (byte) 8),
    new Color32((byte) 236, (byte) 90, (byte) 110, (byte) 9),
    new Color32((byte) 110, (byte) 236, (byte) 110, (byte) 10)
  };
  [SerializeField]
  private Texture2D colourTex;
  [SerializeField]
  private Texture2D indexTex;
  [HideInInspector]
  public SubWorld.ZoneType[] worldZoneTypes;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.GenerateTexture();
  }

  public void GenerateTexture()
  {
    this.colourTex = new Texture2D(Grid.WidthInCells, Grid.HeightInCells, TextureFormat.RGB24, false);
    this.colourTex.name = "SubworldRegionColourData";
    this.colourTex.filterMode = FilterMode.Bilinear;
    this.colourTex.wrapMode = TextureWrapMode.Clamp;
    this.colourTex.anisoLevel = 0;
    this.indexTex = new Texture2D(Grid.WidthInCells, Grid.HeightInCells, TextureFormat.Alpha8, false);
    this.indexTex.name = "SubworldRegionIndexData";
    this.indexTex.filterMode = FilterMode.Point;
    this.indexTex.wrapMode = TextureWrapMode.Clamp;
    this.indexTex.anisoLevel = 0;
    byte[] data = new byte[Grid.WidthInCells * Grid.HeightInCells * 3];
    byte[] numArray = new byte[Grid.WidthInCells * Grid.HeightInCells];
    this.worldZoneTypes = new SubWorld.ZoneType[Grid.CellCount];
    WorldDetailSave worldDetailSave = SaveLoader.Instance.worldDetailSave;
    Vector2 zero = Vector2.zero;
    for (int index1 = 0; index1 < worldDetailSave.overworldCells.Count; ++index1)
    {
      WorldDetailSave.OverworldCell overworldCell = worldDetailSave.overworldCells[index1];
      Polygon poly = overworldCell.poly;
      for (zero.y = (float) (int) Mathf.Floor(poly.bounds.yMin); (double) zero.y < (double) Mathf.Ceil(poly.bounds.yMax); ++zero.y)
      {
        for (zero.x = (float) (int) Mathf.Floor(poly.bounds.xMin); (double) zero.x < (double) Mathf.Ceil(poly.bounds.xMax); ++zero.x)
        {
          if (poly.Contains(zero))
          {
            int index2 = (int) ((double) zero.x + (double) zero.y * (double) Grid.WidthInCells);
            numArray[index2] = overworldCell.zoneType != SubWorld.ZoneType.Space ? (byte) overworldCell.zoneType : byte.MaxValue;
            Color32 zoneColour = this.zoneColours[(int) overworldCell.zoneType];
            data[index2 * 3] = zoneColour.r;
            data[index2 * 3 + 1] = zoneColour.g;
            data[index2 * 3 + 2] = zoneColour.b;
            int cell = Grid.XYToCell((int) zero.x, (int) zero.y);
            if (Grid.IsValidCell(cell))
              this.worldZoneTypes[cell] = overworldCell.zoneType;
          }
        }
      }
    }
    this.colourTex.LoadRawTextureData(data);
    this.indexTex.LoadRawTextureData(numArray);
    this.colourTex.Apply();
    this.indexTex.Apply();
    this.OnShadersReloaded();
    ShaderReloader.Register(new System.Action(this.OnShadersReloaded));
    this.InitSimZones(numArray);
  }

  private void OnShadersReloaded()
  {
    Shader.SetGlobalTexture("_WorldZoneTex", (Texture) this.colourTex);
    Shader.SetGlobalTexture("_WorldZoneIndexTex", (Texture) this.indexTex);
  }

  public SubWorld.ZoneType GetSubWorldZoneType(int cell)
  {
    if (cell >= 0 && cell < this.worldZoneTypes.Length)
      return this.worldZoneTypes[cell];
    return SubWorld.ZoneType.Sandstone;
  }

  private SubWorld.ZoneType GetSubWorldZoneType(Vector2I pos)
  {
    WorldDetailSave worldDetailSave = SaveLoader.Instance.worldDetailSave;
    if (worldDetailSave != null)
    {
      for (int index = 0; index < worldDetailSave.overworldCells.Count; ++index)
      {
        if (worldDetailSave.overworldCells[index].poly.Contains((Vector2) pos))
          return worldDetailSave.overworldCells[index].zoneType;
      }
    }
    return SubWorld.ZoneType.Sandstone;
  }

  private Color32 GetZoneColor(SubWorld.ZoneType zone_type)
  {
    Color32 color32 = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, (byte) 3);
    Debug.Assert((zone_type < (SubWorld.ZoneType) this.zoneColours.Length ? 1 : 0) != 0, (object) ("Need to add more colours to handle this zone" + (object) (int) zone_type + "<" + (object) this.zoneColours.Length));
    return color32;
  }

  private unsafe void InitSimZones(byte[] bytes)
  {
    // ISSUE: cast to a reference type
    // ISSUE: explicit reference operation
    fixed (byte* msg = &^(bytes == null || bytes.Length == 0 ? (byte&) IntPtr.Zero : ref bytes[0]))
      Sim.SIM_HandleMessage(-457308393, bytes.Length, msg);
  }
}
