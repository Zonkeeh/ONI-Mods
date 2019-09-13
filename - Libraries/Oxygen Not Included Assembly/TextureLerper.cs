// Decompiled with JetBrains decompiler
// Type: TextureLerper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.Rendering;

public class TextureLerper
{
  private RenderTexture[] BlendTextures = new RenderTexture[2];
  public float Speed = 1f;
  private static int offsetCounter;
  public string name;
  private float BlendDt;
  private float BlendTime;
  private int BlendIdx;
  private Material Material;
  private Mesh mesh;
  private RenderTexture source;
  private RenderTexture dest;
  private GameObject meshGO;
  private GameObject cameraGO;
  private Camera textureCam;
  private float blend;

  public TextureLerper(
    Texture target_texture,
    string name,
    FilterMode filter_mode = FilterMode.Bilinear,
    TextureFormat texture_format = TextureFormat.ARGB32)
  {
    this.name = name;
    this.Init(target_texture.width, target_texture.height, name, filter_mode, texture_format);
    this.Material.SetTexture("_TargetTex", target_texture);
  }

  private void Init(
    int width,
    int height,
    string name,
    FilterMode filter_mode,
    TextureFormat texture_format)
  {
    for (int index = 0; index < 2; ++index)
    {
      this.BlendTextures[index] = new RenderTexture(width, height, 0, TextureUtil.GetRenderTextureFormat(texture_format));
      this.BlendTextures[index].filterMode = filter_mode;
      this.BlendTextures[index].name = name;
    }
    this.Material = new Material(Shader.Find("Klei/LerpEffect"));
    this.Material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.None;
    this.mesh = new Mesh();
    this.mesh.name = "LerpEffect";
    this.mesh.vertices = new Vector3[4]
    {
      new Vector3(0.0f, 0.0f, 0.0f),
      new Vector3(1f, 1f, 0.0f),
      new Vector3(0.0f, 1f, 0.0f),
      new Vector3(1f, 0.0f, 0.0f)
    };
    this.mesh.triangles = new int[6]{ 0, 1, 2, 0, 3, 1 };
    this.mesh.uv = new Vector2[4]
    {
      new Vector2(0.0f, 0.0f),
      new Vector2(1f, 1f),
      new Vector2(0.0f, 1f),
      new Vector2(1f, 0.0f)
    };
    int layer = LayerMask.NameToLayer("RTT");
    int mask = LayerMask.GetMask("RTT");
    this.cameraGO = new GameObject();
    this.cameraGO.name = "TextureLerper_" + name;
    this.textureCam = this.cameraGO.AddComponent<Camera>();
    this.textureCam.transform.SetPosition(new Vector3((float) TextureLerper.offsetCounter + 0.5f, 0.5f, 0.0f));
    this.textureCam.clearFlags = CameraClearFlags.Nothing;
    this.textureCam.depth = -100f;
    this.textureCam.allowHDR = false;
    this.textureCam.orthographic = true;
    this.textureCam.orthographicSize = 0.5f;
    this.textureCam.cullingMask = mask;
    this.textureCam.targetTexture = this.dest;
    this.textureCam.nearClipPlane = -5f;
    this.textureCam.farClipPlane = 5f;
    this.textureCam.useOcclusionCulling = false;
    this.textureCam.aspect = 1f;
    this.textureCam.rect = new Rect(0.0f, 0.0f, 1f, 1f);
    this.meshGO = new GameObject();
    this.meshGO.name = "mesh";
    this.meshGO.transform.parent = this.cameraGO.transform;
    this.meshGO.transform.SetLocalPosition(new Vector3(-0.5f, -0.5f, 0.0f));
    this.meshGO.isStatic = true;
    MeshRenderer meshRenderer = this.meshGO.AddComponent<MeshRenderer>();
    meshRenderer.receiveShadows = false;
    meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
    meshRenderer.lightProbeUsage = LightProbeUsage.Off;
    meshRenderer.reflectionProbeUsage = ReflectionProbeUsage.Off;
    this.meshGO.AddComponent<MeshFilter>().mesh = this.mesh;
    meshRenderer.sharedMaterial = this.Material;
    this.cameraGO.SetLayerRecursively(layer);
    ++TextureLerper.offsetCounter;
  }

  public void LongUpdate(float dt)
  {
    this.BlendDt = dt;
    this.BlendTime = 0.0f;
  }

  public Texture Update()
  {
    float num1 = Time.deltaTime * this.Speed;
    if ((double) Time.deltaTime == 0.0)
      num1 = Time.unscaledDeltaTime * this.Speed;
    float num2 = Mathf.Min(num1 / Mathf.Max(this.BlendDt - this.BlendTime, 0.0f), 1f);
    this.BlendTime += num1;
    if (GameUtil.IsCapturingTimeLapse())
      num2 = 1f;
    this.source = this.BlendTextures[this.BlendIdx];
    this.BlendIdx = (this.BlendIdx + 1) % 2;
    this.dest = this.BlendTextures[this.BlendIdx];
    Vector4 vector4 = this.GetVisibleCellRange();
    vector4 = new Vector4(0.0f, 0.0f, (float) Grid.WidthInCells, (float) Grid.HeightInCells);
    this.Material.SetFloat("_Lerp", num2);
    this.Material.SetTexture("_SourceTex", (Texture) this.source);
    this.Material.SetVector("_MeshParams", vector4);
    this.textureCam.targetTexture = this.dest;
    return (Texture) this.dest;
  }

  private Vector4 GetVisibleCellRange()
  {
    Camera main = Camera.main;
    float cellSizeInMeters = Grid.CellSizeInMeters;
    Ray ray1 = main.ViewportPointToRay(Vector3.zero);
    float distance1 = Mathf.Abs(ray1.origin.z / ray1.direction.z);
    int cell = Grid.PosToCell(ray1.GetPoint(distance1));
    float num1 = -Grid.HalfCellSizeInMeters;
    Vector3 pos = Grid.CellToPos(cell, num1, num1, num1);
    int num2 = Math.Max(0, (int) ((double) pos.x / (double) cellSizeInMeters));
    int num3 = Math.Max(0, (int) ((double) pos.y / (double) cellSizeInMeters));
    Ray ray2 = main.ViewportPointToRay(Vector3.one);
    float distance2 = Mathf.Abs(ray2.origin.z / ray2.direction.z);
    Vector3 point = ray2.GetPoint(distance2);
    int a1 = Mathf.CeilToInt(point.x / cellSizeInMeters);
    int a2 = Mathf.CeilToInt(point.y / cellSizeInMeters);
    int num4 = Mathf.Min(a1, Grid.WidthInCells - 1);
    int num5 = Mathf.Min(a2, Grid.HeightInCells - 1);
    return new Vector4((float) num2, (float) num3, (float) num4, (float) num5);
  }
}
