// Decompiled with JetBrains decompiler
// Type: GroundMasks
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class GroundMasks : ScriptableObject
{
  public TextureAtlas maskAtlas;
  [NonSerialized]
  public Dictionary<string, GroundMasks.BiomeMaskData> biomeMasks;

  public void Initialize()
  {
    if ((UnityEngine.Object) this.maskAtlas == (UnityEngine.Object) null || this.maskAtlas.items == null)
      return;
    this.biomeMasks = new Dictionary<string, GroundMasks.BiomeMaskData>();
    foreach (TextureAtlas.Item obj in this.maskAtlas.items)
    {
      string name = obj.name;
      int length = name.IndexOf('/');
      string str1 = name.Substring(0, length);
      string str2 = name.Substring(length + 1, 4);
      string index = str1.ToLower();
      for (int startIndex = index.IndexOf('_'); startIndex != -1; startIndex = index.IndexOf('_'))
        index = index.Remove(startIndex, 1);
      GroundMasks.BiomeMaskData biomeMaskData = (GroundMasks.BiomeMaskData) null;
      if (!this.biomeMasks.TryGetValue(index, out biomeMaskData))
      {
        biomeMaskData = new GroundMasks.BiomeMaskData(index);
        this.biomeMasks[index] = biomeMaskData;
      }
      int int32 = Convert.ToInt32(str2, 2);
      GroundMasks.Tile tile = biomeMaskData.tiles[int32];
      if (tile.variationUVs == null)
      {
        tile.isSource = true;
        tile.variationUVs = new GroundMasks.UVData[1];
      }
      else
      {
        GroundMasks.UVData[] uvDataArray = new GroundMasks.UVData[tile.variationUVs.Length + 1];
        Array.Copy((Array) tile.variationUVs, (Array) uvDataArray, tile.variationUVs.Length);
        tile.variationUVs = uvDataArray;
      }
      Vector4 vector4 = new Vector4(obj.uvBox.x, obj.uvBox.w, obj.uvBox.z, obj.uvBox.y);
      GroundMasks.UVData uvData = new GroundMasks.UVData(new Vector2(vector4.x, vector4.y), new Vector2(vector4.z, vector4.y), new Vector2(vector4.x, vector4.w), new Vector2(vector4.z, vector4.w));
      tile.variationUVs[tile.variationUVs.Length - 1] = uvData;
      biomeMaskData.tiles[int32] = tile;
    }
    foreach (KeyValuePair<string, GroundMasks.BiomeMaskData> biomeMask in this.biomeMasks)
    {
      biomeMask.Value.GenerateRotations();
      biomeMask.Value.Validate();
    }
  }

  [ContextMenu("Regenerate")]
  private void Regenerate()
  {
    this.Initialize();
    foreach (KeyValuePair<string, GroundMasks.BiomeMaskData> biomeMask in this.biomeMasks)
    {
      GroundMasks.BiomeMaskData biomeMaskData = biomeMask.Value;
      DebugUtil.LogArgs((object) biomeMaskData.name);
      for (int index = 1; index < biomeMaskData.tiles.Length; ++index)
      {
        GroundMasks.Tile tile = biomeMaskData.tiles[index];
        DebugUtil.LogArgs((object) "Tile", (object) index, (object) "has", (object) tile.variationUVs.Length, (object) "variations");
      }
    }
  }

  public struct UVData
  {
    public Vector2 bl;
    public Vector2 br;
    public Vector2 tl;
    public Vector2 tr;

    public UVData(Vector2 bl, Vector2 br, Vector2 tl, Vector2 tr)
    {
      this.bl = bl;
      this.br = br;
      this.tl = tl;
      this.tr = tr;
    }
  }

  public struct Tile
  {
    public bool isSource;
    public GroundMasks.UVData[] variationUVs;
  }

  public class BiomeMaskData
  {
    public string name;
    public GroundMasks.Tile[] tiles;

    public BiomeMaskData(string name)
    {
      this.name = name;
      this.tiles = new GroundMasks.Tile[16];
    }

    public void GenerateRotations()
    {
      for (int dest_mask = 1; dest_mask < 15; ++dest_mask)
      {
        if (!this.tiles[dest_mask].isSource)
        {
          GroundMasks.Tile tile = this.tiles[dest_mask];
          tile.variationUVs = this.GetNonNullRotationUVs(dest_mask);
          this.tiles[dest_mask] = tile;
        }
      }
    }

    public GroundMasks.UVData[] GetNonNullRotationUVs(int dest_mask)
    {
      GroundMasks.UVData[] uvDataArray = (GroundMasks.UVData[]) null;
      int num1 = dest_mask;
      for (int index1 = 0; index1 < 3; ++index1)
      {
        int num2 = (num1 & 1) >> 0;
        int num3 = (num1 & 2) >> 1;
        int num4 = (num1 & 4) >> 2;
        int index2 = (num1 & 8) >> 3 << 2 | num4 << 0 | num3 << 3 | num2 << 1;
        if (this.tiles[index2].isSource)
        {
          uvDataArray = new GroundMasks.UVData[this.tiles[index2].variationUVs.Length];
          for (int index3 = 0; index3 < this.tiles[index2].variationUVs.Length; ++index3)
          {
            GroundMasks.UVData variationUv = this.tiles[index2].variationUVs[index3];
            GroundMasks.UVData uvData = variationUv;
            switch (index1)
            {
              case 0:
                uvData = new GroundMasks.UVData(variationUv.tl, variationUv.bl, variationUv.tr, variationUv.br);
                break;
              case 1:
                uvData = new GroundMasks.UVData(variationUv.tr, variationUv.tl, variationUv.br, variationUv.bl);
                break;
              case 2:
                uvData = new GroundMasks.UVData(variationUv.br, variationUv.tr, variationUv.bl, variationUv.tl);
                break;
              default:
                Debug.LogError((object) "Unhandled rotation case");
                break;
            }
            uvDataArray[index3] = uvData;
          }
          break;
        }
        num1 = index2;
      }
      return uvDataArray;
    }

    public void Validate()
    {
      for (int index = 1; index < this.tiles.Length; ++index)
      {
        if (this.tiles[index].variationUVs == null)
          DebugUtil.LogErrorArgs((object) this.name, (object) "has invalid tile at index", (object) index);
      }
    }
  }
}
