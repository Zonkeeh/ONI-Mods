// Decompiled with JetBrains decompiler
// Type: Rendering.World.Brush
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace Rendering.World
{
  public class Brush
  {
    private HashSet<int> tiles = new HashSet<int>();
    private bool dirty;
    private Material material;
    private int layer;
    private List<Brush> activeBrushes;
    private List<Brush> dirtyBrushes;
    private int widthInTiles;
    private Mask mask;
    private DynamicMesh mesh;
    private MaterialPropertyBlock propertyBlock;

    public Brush(
      int id,
      string name,
      Material material,
      Mask mask,
      List<Brush> active_brushes,
      List<Brush> dirty_brushes,
      int width_in_tiles,
      MaterialPropertyBlock property_block)
    {
      this.Id = id;
      this.material = material;
      this.mask = mask;
      this.mesh = new DynamicMesh(name, new Bounds(Vector3.zero, new Vector3(float.MaxValue, float.MaxValue, 0.0f)));
      this.activeBrushes = active_brushes;
      this.dirtyBrushes = dirty_brushes;
      this.layer = LayerMask.NameToLayer("World");
      this.widthInTiles = width_in_tiles;
      this.propertyBlock = property_block;
    }

    public int Id { get; private set; }

    public void Add(int tile_idx)
    {
      this.tiles.Add(tile_idx);
      if (this.dirty)
        return;
      this.dirtyBrushes.Add(this);
      this.dirty = true;
    }

    public void Remove(int tile_idx)
    {
      this.tiles.Remove(tile_idx);
      if (this.dirty)
        return;
      this.dirtyBrushes.Add(this);
      this.dirty = true;
    }

    public void SetMaskOffset(int offset)
    {
      this.mask.SetOffset(offset);
    }

    public void Refresh()
    {
      bool flag = this.mesh.Meshes.Length > 0;
      int count = this.tiles.Count;
      this.mesh.Reserve(count * 4, count * 6);
      if (this.mesh.SetTriangles)
      {
        int triangle = 0;
        for (int index = 0; index < count; ++index)
        {
          this.mesh.AddTriangle(triangle);
          this.mesh.AddTriangle(2 + triangle);
          this.mesh.AddTriangle(1 + triangle);
          this.mesh.AddTriangle(1 + triangle);
          this.mesh.AddTriangle(2 + triangle);
          this.mesh.AddTriangle(3 + triangle);
          triangle += 4;
        }
      }
      foreach (int tile in this.tiles)
      {
        float num1 = (float) (tile % this.widthInTiles);
        float num2 = (float) (tile / this.widthInTiles);
        float z = 0.0f;
        this.mesh.AddVertex(new Vector3(num1 - 0.5f, num2 - 0.5f, z));
        this.mesh.AddVertex(new Vector3(num1 + 0.5f, num2 - 0.5f, z));
        this.mesh.AddVertex(new Vector3(num1 - 0.5f, num2 + 0.5f, z));
        this.mesh.AddVertex(new Vector3(num1 + 0.5f, num2 + 0.5f, z));
      }
      if (this.mesh.SetUVs)
      {
        for (int index = 0; index < count; ++index)
        {
          this.mesh.AddUV(this.mask.UV0);
          this.mesh.AddUV(this.mask.UV1);
          this.mesh.AddUV(this.mask.UV2);
          this.mesh.AddUV(this.mask.UV3);
        }
      }
      this.dirty = false;
      this.mesh.Commit();
      if (this.mesh.Meshes.Length > 0)
      {
        if (flag)
          return;
        this.activeBrushes.Add(this);
      }
      else
      {
        if (!flag)
          return;
        this.activeBrushes.Remove(this);
      }
    }

    public void Render()
    {
      this.mesh.Render(new Vector3(0.0f, 0.0f, Grid.GetLayerZ(Grid.SceneLayer.Ground)), Quaternion.identity, this.material, this.layer, this.propertyBlock);
    }

    public void SetMaterial(Material material, MaterialPropertyBlock property_block)
    {
      this.material = material;
      this.propertyBlock = property_block;
    }
  }
}
