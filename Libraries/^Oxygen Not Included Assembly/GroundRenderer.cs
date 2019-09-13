// Decompiled with JetBrains decompiler
// Type: GroundRenderer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using ProcGen;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GroundRenderer : KMonoBehaviour
{
  private Dictionary<SimHashes, GroundRenderer.Materials> elementMaterials = new Dictionary<SimHashes, GroundRenderer.Materials>();
  [SerializeField]
  private GroundMasks masks;
  private GroundMasks.BiomeMaskData[] biomeMasks;
  private bool[,] dirtyChunks;
  private GroundRenderer.WorldChunk[,] worldChunks;
  private const int ChunkEdgeSize = 16;
  private Vector2I size;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    ShaderReloader.Register(new System.Action(this.OnShadersReloaded));
    this.OnShadersReloaded();
    this.masks.Initialize();
    SubWorld.ZoneType[] values = (SubWorld.ZoneType[]) Enum.GetValues(typeof (SubWorld.ZoneType));
    this.biomeMasks = new GroundMasks.BiomeMaskData[values.Length];
    for (int index = 0; index < values.Length; ++index)
    {
      SubWorld.ZoneType zone_type = values[index];
      this.biomeMasks[index] = this.GetBiomeMask(zone_type);
    }
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.size = new Vector2I((Grid.WidthInCells + 16 - 1) / 16, (Grid.HeightInCells + 16 - 1) / 16);
    this.dirtyChunks = new bool[this.size.x, this.size.y];
    this.worldChunks = new GroundRenderer.WorldChunk[this.size.x, this.size.y];
    for (int y = 0; y < this.size.y; ++y)
    {
      for (int x = 0; x < this.size.x; ++x)
      {
        this.worldChunks[x, y] = new GroundRenderer.WorldChunk(x, y);
        this.dirtyChunks[x, y] = true;
      }
    }
  }

  public void Render(Vector2I vis_min, Vector2I vis_max, bool forceVisibleRebuild = false)
  {
    if (!this.enabled)
      return;
    int layer = LayerMask.NameToLayer("World");
    Vector2I vector2I1 = new Vector2I(vis_min.x / 16, vis_min.y / 16);
    Vector2I vector2I2 = new Vector2I((vis_max.x + 16 - 1) / 16, (vis_max.y + 16 - 1) / 16);
    for (int y = vector2I1.y; y < vector2I2.y; ++y)
    {
      for (int x = vector2I1.x; x < vector2I2.x; ++x)
      {
        GroundRenderer.WorldChunk worldChunk = this.worldChunks[x, y];
        if (this.dirtyChunks[x, y] || forceVisibleRebuild)
        {
          this.dirtyChunks[x, y] = false;
          worldChunk.Rebuild(this.biomeMasks, this.elementMaterials);
        }
        worldChunk.Render(layer);
      }
    }
    this.RebuildDirtyChunks();
  }

  public void RenderAll()
  {
    this.Render(new Vector2I(0, 0), new Vector2I(this.worldChunks.GetLength(0) * 16, this.worldChunks.GetLength(1) * 16), true);
  }

  private void RebuildDirtyChunks()
  {
    for (int index1 = 0; index1 < this.dirtyChunks.GetLength(1); ++index1)
    {
      for (int index2 = 0; index2 < this.dirtyChunks.GetLength(0); ++index2)
      {
        if (this.dirtyChunks[index2, index1])
        {
          this.dirtyChunks[index2, index1] = false;
          this.worldChunks[index2, index1].Rebuild(this.biomeMasks, this.elementMaterials);
        }
      }
    }
  }

  public void MarkDirty(int cell)
  {
    Vector2I xy = Grid.CellToXY(cell);
    Vector2I vector2I = new Vector2I(xy.x / 16, xy.y / 16);
    this.dirtyChunks[vector2I.x, vector2I.y] = true;
    bool flag1 = xy.x % 16 == 0 && vector2I.x > 0;
    bool flag2 = xy.x % 16 == 15 && vector2I.x < this.size.x - 1;
    bool flag3 = xy.y % 16 == 0 && vector2I.y > 0;
    bool flag4 = xy.y % 16 == 15 && vector2I.y < this.size.y - 1;
    if (flag1)
    {
      this.dirtyChunks[vector2I.x - 1, vector2I.y] = true;
      if (flag3)
        this.dirtyChunks[vector2I.x - 1, vector2I.y - 1] = true;
      if (flag4)
        this.dirtyChunks[vector2I.x - 1, vector2I.y + 1] = true;
    }
    if (flag3)
      this.dirtyChunks[vector2I.x, vector2I.y - 1] = true;
    if (flag4)
      this.dirtyChunks[vector2I.x, vector2I.y + 1] = true;
    if (!flag2)
      return;
    this.dirtyChunks[vector2I.x + 1, vector2I.y] = true;
    if (flag3)
      this.dirtyChunks[vector2I.x + 1, vector2I.y - 1] = true;
    if (!flag4)
      return;
    this.dirtyChunks[vector2I.x + 1, vector2I.y + 1] = true;
  }

  private Vector2I GetChunkIdx(int cell)
  {
    Vector2I xy = Grid.CellToXY(cell);
    return new Vector2I(xy.x / 16, xy.y / 16);
  }

  private GroundMasks.BiomeMaskData GetBiomeMask(SubWorld.ZoneType zone_type)
  {
    GroundMasks.BiomeMaskData biomeMaskData = (GroundMasks.BiomeMaskData) null;
    string lower = zone_type.ToString().ToLower();
    foreach (KeyValuePair<string, GroundMasks.BiomeMaskData> biomeMask in this.masks.biomeMasks)
    {
      string key = biomeMask.Key;
      if (lower == key)
      {
        biomeMaskData = biomeMask.Value;
        break;
      }
    }
    return biomeMaskData;
  }

  private void InitOpaqueMaterial(Material material, Element element)
  {
    material.name = element.id.ToString() + "_opaque";
    material.renderQueue = RenderQueues.WorldOpaque;
    material.EnableKeyword("OPAQUE");
    material.DisableKeyword("ALPHA");
    this.ConfigureMaterialShine(material);
    material.SetInt("_SrcAlpha", 1);
    material.SetInt("_DstAlpha", 0);
    material.SetInt("_ZWrite", 1);
    material.SetTexture("_AlphaTestMap", (Texture) Texture2D.whiteTexture);
  }

  private void InitAlphaMaterial(Material material, Element element)
  {
    material.name = element.id.ToString() + "_alpha";
    material.renderQueue = RenderQueues.WorldTransparent;
    material.EnableKeyword("ALPHA");
    material.DisableKeyword("OPAQUE");
    this.ConfigureMaterialShine(material);
    material.SetTexture("_AlphaTestMap", (Texture) this.masks.maskAtlas.texture);
    material.SetInt("_SrcAlpha", 5);
    material.SetInt("_DstAlpha", 10);
    material.SetInt("_ZWrite", 0);
  }

  private void ConfigureMaterialShine(Material material)
  {
    if ((UnityEngine.Object) material.GetTexture("_ShineMask") != (UnityEngine.Object) null)
    {
      material.DisableKeyword("MATTE");
      material.EnableKeyword("SHINY");
    }
    else
    {
      material.EnableKeyword("MATTE");
      material.DisableKeyword("SHINY");
    }
  }

  [ContextMenu("Reload Shaders")]
  public void OnShadersReloaded()
  {
    this.FreeMaterials();
    foreach (Element element in ElementLoader.elements)
    {
      if (element.IsSolid)
      {
        if ((UnityEngine.Object) element.substance.material == (UnityEngine.Object) null)
          DebugUtil.LogErrorArgs((object) element.name, (object) "must have material associated with it in the substance table");
        Material material1 = new Material(element.substance.material);
        this.InitOpaqueMaterial(material1, element);
        Material material2 = new Material(material1);
        this.InitAlphaMaterial(material2, element);
        GroundRenderer.Materials materials = new GroundRenderer.Materials(material1, material2);
        this.elementMaterials[element.id] = materials;
      }
    }
    if (this.worldChunks == null)
      return;
    for (int index1 = 0; index1 < this.dirtyChunks.GetLength(1); ++index1)
    {
      for (int index2 = 0; index2 < this.dirtyChunks.GetLength(0); ++index2)
        this.dirtyChunks[index2, index1] = true;
    }
    GroundRenderer.WorldChunk[,] worldChunks = this.worldChunks;
    int length1 = worldChunks.GetLength(0);
    int length2 = worldChunks.GetLength(1);
    for (int index1 = 0; index1 < length1; ++index1)
    {
      for (int index2 = 0; index2 < length2; ++index2)
      {
        GroundRenderer.WorldChunk worldChunk = worldChunks[index1, index2];
        worldChunk.Clear();
        worldChunk.Rebuild(this.biomeMasks, this.elementMaterials);
      }
    }
  }

  public void FreeResources()
  {
    this.FreeMaterials();
    this.elementMaterials.Clear();
    this.elementMaterials = (Dictionary<SimHashes, GroundRenderer.Materials>) null;
    if (this.worldChunks == null)
      return;
    GroundRenderer.WorldChunk[,] worldChunks = this.worldChunks;
    int length1 = worldChunks.GetLength(0);
    int length2 = worldChunks.GetLength(1);
    for (int index1 = 0; index1 < length1; ++index1)
    {
      for (int index2 = 0; index2 < length2; ++index2)
        worldChunks[index1, index2].FreeResources();
    }
    this.worldChunks = (GroundRenderer.WorldChunk[,]) null;
  }

  private void FreeMaterials()
  {
    foreach (GroundRenderer.Materials materials in this.elementMaterials.Values)
    {
      UnityEngine.Object.Destroy((UnityEngine.Object) materials.opaque);
      UnityEngine.Object.Destroy((UnityEngine.Object) materials.alpha);
    }
    this.elementMaterials.Clear();
  }

  [Serializable]
  private struct Materials
  {
    public Material opaque;
    public Material alpha;

    public Materials(Material opaque, Material alpha)
    {
      this.opaque = opaque;
      this.alpha = alpha;
    }
  }

  private class ElementChunk
  {
    public SimHashes element;
    private GroundRenderer.ElementChunk.RenderData alpha;
    private GroundRenderer.ElementChunk.RenderData opaque;
    public int tileCount;

    public ElementChunk(
      SimHashes element,
      Dictionary<SimHashes, GroundRenderer.Materials> materials)
    {
      this.element = element;
      GroundRenderer.Materials material = materials[element];
      this.alpha = new GroundRenderer.ElementChunk.RenderData(material.alpha);
      this.opaque = new GroundRenderer.ElementChunk.RenderData(material.opaque);
      this.Clear();
    }

    public void Clear()
    {
      this.opaque.Clear();
      this.alpha.Clear();
      this.tileCount = 0;
    }

    public void AddOpaqueQuad(int x, int y, GroundMasks.UVData uvs)
    {
      this.opaque.AddQuad(x, y, uvs);
      ++this.tileCount;
    }

    public void AddAlphaQuad(int x, int y, GroundMasks.UVData uvs)
    {
      this.alpha.AddQuad(x, y, uvs);
      ++this.tileCount;
    }

    public void Build()
    {
      this.opaque.Build();
      this.alpha.Build();
    }

    public void Render(int layer, int element_idx)
    {
      float z = Grid.GetLayerZ(Grid.SceneLayer.Ground) - 0.0001f * (float) element_idx;
      this.opaque.Render(new Vector3(0.0f, 0.0f, z), layer);
      this.alpha.Render(new Vector3(0.0f, 0.0f, z), layer);
    }

    public void FreeResources()
    {
      this.alpha.FreeResources();
      this.opaque.FreeResources();
      this.alpha = (GroundRenderer.ElementChunk.RenderData) null;
      this.opaque = (GroundRenderer.ElementChunk.RenderData) null;
    }

    private class RenderData
    {
      public Material material;
      public Mesh mesh;
      public List<Vector3> pos;
      public List<Vector2> uv;
      public List<int> indices;

      public RenderData(Material material)
      {
        this.material = material;
        this.mesh = new Mesh();
        this.mesh.MarkDynamic();
        this.mesh.name = nameof (ElementChunk);
        this.pos = new List<Vector3>();
        this.uv = new List<Vector2>();
        this.indices = new List<int>();
      }

      public void ClearMesh()
      {
        if (!((UnityEngine.Object) this.mesh != (UnityEngine.Object) null))
          return;
        this.mesh.Clear();
        UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.mesh);
        this.mesh = (Mesh) null;
      }

      public void Clear()
      {
        if ((UnityEngine.Object) this.mesh != (UnityEngine.Object) null)
          this.mesh.Clear();
        if (this.pos != null)
          this.pos.Clear();
        if (this.uv != null)
          this.uv.Clear();
        if (this.indices == null)
          return;
        this.indices.Clear();
      }

      public void FreeResources()
      {
        this.ClearMesh();
        this.Clear();
        this.pos = (List<Vector3>) null;
        this.uv = (List<Vector2>) null;
        this.indices = (List<int>) null;
        this.material = (Material) null;
      }

      public void Build()
      {
        this.mesh.SetVertices(this.pos);
        this.mesh.SetUVs(0, this.uv);
        this.mesh.SetTriangles(this.indices, 0);
      }

      public void AddQuad(int x, int y, GroundMasks.UVData uvs)
      {
        int count = this.pos.Count;
        this.indices.Add(count);
        this.indices.Add(count + 1);
        this.indices.Add(count + 3);
        this.indices.Add(count);
        this.indices.Add(count + 3);
        this.indices.Add(count + 2);
        this.pos.Add(new Vector3((float) x - 0.5f, (float) y - 0.5f, 0.0f));
        this.pos.Add(new Vector3((float) ((double) x + 1.0 - 0.5), (float) y - 0.5f, 0.0f));
        this.pos.Add(new Vector3((float) x - 0.5f, (float) ((double) y + 1.0 - 0.5), 0.0f));
        this.pos.Add(new Vector3((float) ((double) x + 1.0 - 0.5), (float) ((double) y + 1.0 - 0.5), 0.0f));
        this.uv.Add(uvs.bl);
        this.uv.Add(uvs.br);
        this.uv.Add(uvs.tl);
        this.uv.Add(uvs.tr);
      }

      public void Render(Vector3 position, int layer)
      {
        if (this.pos.Count == 0)
          return;
        Graphics.DrawMesh(this.mesh, position, Quaternion.identity, this.material, layer, (Camera) null, 0, (MaterialPropertyBlock) null, ShadowCastingMode.Off, false, (Transform) null, false);
      }
    }
  }

  private struct WorldChunk
  {
    private static Element[] elements = new Element[4];
    private static Element[] uniqueElements = new Element[4];
    private static int[] substances = new int[4];
    private static Vector2 NoiseScale = (Vector2) new Vector3(1f, 1f);
    public readonly int chunkX;
    public readonly int chunkY;
    private List<GroundRenderer.ElementChunk> elementChunks;

    public WorldChunk(int x, int y)
    {
      this.chunkX = x;
      this.chunkY = y;
      this.elementChunks = new List<GroundRenderer.ElementChunk>();
    }

    public void Clear()
    {
      this.elementChunks.Clear();
    }

    private static void InsertSorted(Element element, Element[] array, int size)
    {
      int id = (int) element.id;
      for (int index = 0; index < size; ++index)
      {
        Element element1 = array[index];
        if (element1.id > (SimHashes) id)
        {
          array[index] = element;
          element = element1;
          id = (int) element1.id;
        }
      }
      array[size] = element;
    }

    public void Rebuild(
      GroundMasks.BiomeMaskData[] biomeMasks,
      Dictionary<SimHashes, GroundRenderer.Materials> materials)
    {
      foreach (GroundRenderer.ElementChunk elementChunk in this.elementChunks)
        elementChunk.Clear();
      Vector2I vector2I1 = new Vector2I(this.chunkX * 16, this.chunkY * 16);
      Vector2I vector2I2 = new Vector2I(Math.Min(Grid.WidthInCells, (this.chunkX + 1) * 16), Math.Min(Grid.HeightInCells, (this.chunkY + 1) * 16));
      for (int y = vector2I1.y; y < vector2I2.y; ++y)
      {
        int num1 = Math.Max(0, y - 1);
        int num2 = y;
        for (int x = vector2I1.x; x < vector2I2.x; ++x)
        {
          int num3 = Math.Max(0, x - 1);
          int num4 = x;
          int index1 = num1 * Grid.WidthInCells + num3;
          int index2 = num1 * Grid.WidthInCells + num4;
          int index3 = num2 * Grid.WidthInCells + num3;
          int index4 = num2 * Grid.WidthInCells + num4;
          GroundRenderer.WorldChunk.elements[0] = Grid.Element[index1];
          GroundRenderer.WorldChunk.elements[1] = Grid.Element[index2];
          GroundRenderer.WorldChunk.elements[2] = Grid.Element[index3];
          GroundRenderer.WorldChunk.elements[3] = Grid.Element[index4];
          GroundRenderer.WorldChunk.substances[0] = !Grid.RenderedByWorld[index1] || !GroundRenderer.WorldChunk.elements[0].IsSolid ? -1 : GroundRenderer.WorldChunk.elements[0].substance.idx;
          GroundRenderer.WorldChunk.substances[1] = !Grid.RenderedByWorld[index2] || !GroundRenderer.WorldChunk.elements[1].IsSolid ? -1 : GroundRenderer.WorldChunk.elements[1].substance.idx;
          GroundRenderer.WorldChunk.substances[2] = !Grid.RenderedByWorld[index3] || !GroundRenderer.WorldChunk.elements[2].IsSolid ? -1 : GroundRenderer.WorldChunk.elements[2].substance.idx;
          GroundRenderer.WorldChunk.substances[3] = !Grid.RenderedByWorld[index4] || !GroundRenderer.WorldChunk.elements[3].IsSolid ? -1 : GroundRenderer.WorldChunk.elements[3].substance.idx;
          GroundRenderer.WorldChunk.uniqueElements[0] = GroundRenderer.WorldChunk.elements[0];
          GroundRenderer.WorldChunk.InsertSorted(GroundRenderer.WorldChunk.elements[1], GroundRenderer.WorldChunk.uniqueElements, 1);
          GroundRenderer.WorldChunk.InsertSorted(GroundRenderer.WorldChunk.elements[2], GroundRenderer.WorldChunk.uniqueElements, 2);
          GroundRenderer.WorldChunk.InsertSorted(GroundRenderer.WorldChunk.elements[3], GroundRenderer.WorldChunk.uniqueElements, 3);
          int num5 = -1;
          int biomeIdx = GroundRenderer.WorldChunk.GetBiomeIdx(y * Grid.WidthInCells + x);
          GroundMasks.BiomeMaskData biomeMask = biomeMasks[biomeIdx];
          for (int index5 = 0; index5 < GroundRenderer.WorldChunk.uniqueElements.Length; ++index5)
          {
            Element uniqueElement = GroundRenderer.WorldChunk.uniqueElements[index5];
            if (uniqueElement.IsSolid)
            {
              int idx = uniqueElement.substance.idx;
              if (idx != num5)
              {
                num5 = idx;
                int index6 = (GroundRenderer.WorldChunk.substances[2] < idx ? 0 : 1) << 3 | (GroundRenderer.WorldChunk.substances[3] < idx ? 0 : 1) << 2 | (GroundRenderer.WorldChunk.substances[0] < idx ? 0 : 1) << 1 | (GroundRenderer.WorldChunk.substances[1] < idx ? 0 : 1) << 0;
                if (index6 > 0)
                {
                  GroundMasks.UVData[] variationUvs = biomeMask.tiles[index6].variationUVs;
                  float staticRandom = GroundRenderer.WorldChunk.GetStaticRandom(x, y);
                  int num6 = Mathf.Min(variationUvs.Length - 1, (int) ((double) variationUvs.Length * (double) staticRandom));
                  GroundMasks.UVData uvs = variationUvs[num6 % variationUvs.Length];
                  GroundRenderer.ElementChunk elementChunk = this.GetElementChunk(uniqueElement.id, materials);
                  if (index6 == 15)
                    elementChunk.AddOpaqueQuad(x, y, uvs);
                  else
                    elementChunk.AddAlphaQuad(x, y, uvs);
                }
              }
            }
          }
        }
      }
      foreach (GroundRenderer.ElementChunk elementChunk in this.elementChunks)
        elementChunk.Build();
      for (int index1 = this.elementChunks.Count - 1; index1 >= 0; --index1)
      {
        if (this.elementChunks[index1].tileCount == 0)
        {
          int index2 = this.elementChunks.Count - 1;
          this.elementChunks[index1] = this.elementChunks[index2];
          this.elementChunks.RemoveAt(index2);
        }
      }
    }

    private GroundRenderer.ElementChunk GetElementChunk(
      SimHashes elementID,
      Dictionary<SimHashes, GroundRenderer.Materials> materials)
    {
      GroundRenderer.ElementChunk elementChunk = (GroundRenderer.ElementChunk) null;
      for (int index = 0; index < this.elementChunks.Count; ++index)
      {
        if (this.elementChunks[index].element == elementID)
        {
          elementChunk = this.elementChunks[index];
          break;
        }
      }
      if (elementChunk == null)
      {
        elementChunk = new GroundRenderer.ElementChunk(elementID, materials);
        this.elementChunks.Add(elementChunk);
      }
      return elementChunk;
    }

    private static int GetBiomeIdx(int cell)
    {
      if (!Grid.IsValidCell(cell))
        return 0;
      return (int) World.Instance.zoneRenderData.GetSubWorldZoneType(cell);
    }

    private static float GetStaticRandom(int x, int y)
    {
      return PerlinSimplexNoise.noise((float) x * GroundRenderer.WorldChunk.NoiseScale.x, (float) y * GroundRenderer.WorldChunk.NoiseScale.y);
    }

    public void Render(int layer)
    {
      for (int index = 0; index < this.elementChunks.Count; ++index)
      {
        GroundRenderer.ElementChunk elementChunk = this.elementChunks[index];
        elementChunk.Render(layer, ElementLoader.FindElementByHash(elementChunk.element).substance.idx);
      }
    }

    public void FreeResources()
    {
      foreach (GroundRenderer.ElementChunk elementChunk in this.elementChunks)
        elementChunk.FreeResources();
      this.elementChunks.Clear();
      this.elementChunks = (List<GroundRenderer.ElementChunk>) null;
    }
  }
}
