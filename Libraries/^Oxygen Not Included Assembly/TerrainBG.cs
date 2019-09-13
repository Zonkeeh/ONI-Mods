// Decompiled with JetBrains decompiler
// Type: TerrainBG
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class TerrainBG : KMonoBehaviour
{
  public bool doDraw = true;
  public Material starsMaterial;
  public Material backgroundMaterial;
  public Material gasMaterial;
  [SerializeField]
  private Texture3D noiseVolume;
  private Mesh starsPlane;
  private Mesh worldPlane;
  private Mesh gasPlane;
  private int layer;
  private MaterialPropertyBlock[] propertyBlocks;

  protected override void OnSpawn()
  {
    this.layer = LayerMask.NameToLayer("Default");
    this.noiseVolume = this.CreateTexture3D(32);
    this.starsPlane = this.CreateStarsPlane("StarsPlane");
    this.worldPlane = this.CreateWorldPlane("WorldPlane");
    this.gasPlane = this.CreateGasPlane("GasPlane");
    this.propertyBlocks = new MaterialPropertyBlock[Lighting.Instance.Settings.BackgroundLayers];
    for (int index = 0; index < this.propertyBlocks.Length; ++index)
      this.propertyBlocks[index] = new MaterialPropertyBlock();
  }

  private Texture3D CreateTexture3D(int size)
  {
    Color32[] colors = new Color32[size * size * size];
    Texture3D texture3D = new Texture3D(size, size, size, TextureFormat.RGBA32, true);
    for (int index1 = 0; index1 < size; ++index1)
    {
      for (int index2 = 0; index2 < size; ++index2)
      {
        for (int index3 = 0; index3 < size; ++index3)
        {
          Color32 color32 = new Color32((byte) Random.Range(0, (int) byte.MaxValue), (byte) Random.Range(0, (int) byte.MaxValue), (byte) Random.Range(0, (int) byte.MaxValue), (byte) Random.Range(0, (int) byte.MaxValue));
          colors[index1 + index2 * size + index3 * size * size] = color32;
        }
      }
    }
    texture3D.SetPixels32(colors);
    texture3D.Apply();
    return texture3D;
  }

  public Mesh CreateGasPlane(string name)
  {
    Mesh mesh = new Mesh();
    mesh.name = name;
    int length = 4;
    Vector3[] vector3Array1 = new Vector3[length];
    Vector2[] vector2Array1 = new Vector2[length];
    int[] numArray1 = new int[6];
    Vector3[] vector3Array2 = new Vector3[4]
    {
      new Vector3(0.0f, 0.0f, 0.0f),
      new Vector3((float) Grid.WidthInCells, 0.0f, 0.0f),
      new Vector3(0.0f, Grid.HeightInMeters, 0.0f),
      new Vector3(Grid.WidthInMeters, Grid.HeightInMeters, 0.0f)
    };
    Vector2[] vector2Array2 = new Vector2[4]
    {
      new Vector2(0.0f, 0.0f),
      new Vector2(1f, 0.0f),
      new Vector2(0.0f, 1f),
      new Vector2(1f, 1f)
    };
    int[] numArray2 = new int[6]{ 0, 2, 1, 1, 2, 3 };
    mesh.vertices = vector3Array2;
    mesh.uv = vector2Array2;
    mesh.triangles = numArray2;
    mesh.bounds = new Bounds(new Vector3((float) Grid.WidthInCells * 0.5f, (float) Grid.HeightInCells * 0.5f, 0.0f), new Vector3((float) Grid.WidthInCells, (float) Grid.HeightInCells, 0.0f));
    return mesh;
  }

  public Mesh CreateWorldPlane(string name)
  {
    Mesh mesh = new Mesh();
    mesh.name = name;
    int length = 4;
    Vector3[] vector3Array1 = new Vector3[length];
    Vector2[] vector2Array1 = new Vector2[length];
    int[] numArray1 = new int[6];
    Vector3[] vector3Array2 = new Vector3[4]
    {
      new Vector3((float) -Grid.WidthInCells, (float) -Grid.HeightInCells, 0.0f),
      new Vector3((float) Grid.WidthInCells * 2f, (float) -Grid.HeightInCells, 0.0f),
      new Vector3((float) -Grid.WidthInCells, Grid.HeightInMeters * 2f, 0.0f),
      new Vector3(Grid.WidthInMeters * 2f, Grid.HeightInMeters * 2f, 0.0f)
    };
    Vector2[] vector2Array2 = new Vector2[4]
    {
      new Vector2(0.0f, 0.0f),
      new Vector2(1f, 0.0f),
      new Vector2(0.0f, 1f),
      new Vector2(1f, 1f)
    };
    int[] numArray2 = new int[6]{ 0, 2, 1, 1, 2, 3 };
    mesh.vertices = vector3Array2;
    mesh.uv = vector2Array2;
    mesh.triangles = numArray2;
    mesh.bounds = new Bounds(new Vector3((float) Grid.WidthInCells * 0.5f, (float) Grid.HeightInCells * 0.5f, 0.0f), new Vector3((float) Grid.WidthInCells, (float) Grid.HeightInCells, 0.0f));
    return mesh;
  }

  public Mesh CreateStarsPlane(string name)
  {
    Mesh mesh = new Mesh();
    mesh.name = name;
    int length = 4;
    Vector3[] vector3Array1 = new Vector3[length];
    Vector2[] vector2Array1 = new Vector2[length];
    int[] numArray1 = new int[6];
    Vector3[] vector3Array2 = new Vector3[4]
    {
      new Vector3((float) -Grid.WidthInCells, (float) -Grid.HeightInCells, 0.0f),
      new Vector3((float) Grid.WidthInCells * 2f, (float) -Grid.HeightInCells, 0.0f),
      new Vector3((float) -Grid.WidthInCells, Grid.HeightInMeters * 2f, 0.0f),
      new Vector3(Grid.WidthInMeters * 2f, Grid.HeightInMeters * 2f, 0.0f)
    };
    Vector2[] vector2Array2 = new Vector2[4]
    {
      new Vector2(0.0f, 0.0f),
      new Vector2(1f, 0.0f),
      new Vector2(0.0f, 1f),
      new Vector2(1f, 1f)
    };
    int[] numArray2 = new int[6]{ 0, 2, 1, 1, 2, 3 };
    mesh.vertices = vector3Array2;
    mesh.uv = vector2Array2;
    mesh.triangles = numArray2;
    Vector2 vector2 = new Vector2((float) Grid.WidthInCells, 2f * (float) Grid.HeightInCells);
    mesh.bounds = new Bounds(new Vector3(0.5f * vector2.x, 0.5f * vector2.y, 0.0f), new Vector3(vector2.x, vector2.y, 0.0f));
    return mesh;
  }

  private void LateUpdate()
  {
    if (!this.doDraw)
      return;
    this.starsMaterial.renderQueue = RenderQueues.Stars;
    this.starsMaterial.SetTexture("_NoiseVolume", (Texture) this.noiseVolume);
    Graphics.DrawMesh(this.starsPlane, new Vector3(0.0f, 0.0f, Grid.GetLayerZ(Grid.SceneLayer.Background) + 1f), Quaternion.identity, this.starsMaterial, this.layer);
    this.backgroundMaterial.renderQueue = RenderQueues.Backwall;
    for (int index = 0; index < Lighting.Instance.Settings.BackgroundLayers; ++index)
    {
      if (index >= Lighting.Instance.Settings.BackgroundLayers - 1)
      {
        float t = (float) index / (float) (Lighting.Instance.Settings.BackgroundLayers - 1);
        float x = Mathf.Lerp(1f, Lighting.Instance.Settings.BackgroundDarkening, t);
        float z = Mathf.Lerp(1f, Lighting.Instance.Settings.BackgroundUVScale, t);
        float w = 1f;
        if (index == Lighting.Instance.Settings.BackgroundLayers - 1)
          w = 0.0f;
        MaterialPropertyBlock propertyBlock = this.propertyBlocks[index];
        propertyBlock.SetVector("_BackWallParameters", new Vector4(x, Lighting.Instance.Settings.BackgroundClip, z, w));
        Graphics.DrawMesh(this.worldPlane, new Vector3(0.0f, 0.0f, Grid.GetLayerZ(Grid.SceneLayer.Background)), Quaternion.identity, this.backgroundMaterial, this.layer, (Camera) null, 0, propertyBlock);
      }
    }
    this.gasMaterial.renderQueue = RenderQueues.Gas;
    Graphics.DrawMesh(this.gasPlane, new Vector3(0.0f, 0.0f, Grid.GetLayerZ(Grid.SceneLayer.Gas)), Quaternion.identity, this.gasMaterial, this.layer);
    Graphics.DrawMesh(this.gasPlane, new Vector3(0.0f, 0.0f, Grid.GetLayerZ(Grid.SceneLayer.GasFront)), Quaternion.identity, this.gasMaterial, this.layer);
  }
}
