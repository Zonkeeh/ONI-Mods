// Decompiled with JetBrains decompiler
// Type: LightBuffer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class LightBuffer : MonoBehaviour
{
  private Mesh Mesh;
  private Camera Camera;
  [NonSerialized]
  public Material Material;
  [NonSerialized]
  public Material CircleMaterial;
  [NonSerialized]
  public Material ConeMaterial;
  private int ColorRangeTag;
  private int LightPosTag;
  private int LightDirectionAngleTag;
  private int TintColorTag;
  private int Layer;
  public RenderTexture Texture;
  public UnityEngine.Texture WorldLight;
  public static LightBuffer Instance;
  private const RenderTextureFormat RTFormat = RenderTextureFormat.ARGBHalf;

  private void Awake()
  {
    LightBuffer.Instance = this;
    this.ColorRangeTag = Shader.PropertyToID("_ColorRange");
    this.LightPosTag = Shader.PropertyToID("_LightPos");
    this.LightDirectionAngleTag = Shader.PropertyToID("_LightDirectionAngle");
    this.TintColorTag = Shader.PropertyToID("_TintColor");
    this.Camera = this.GetComponent<Camera>();
    this.Layer = LayerMask.NameToLayer("Lights");
    this.Mesh = new Mesh();
    this.Mesh.name = "Light Mesh";
    this.Mesh.vertices = new Vector3[4]
    {
      new Vector3(-1f, -1f, 0.0f),
      new Vector3(-1f, 1f, 0.0f),
      new Vector3(1f, -1f, 0.0f),
      new Vector3(1f, 1f, 0.0f)
    };
    this.Mesh.uv = new Vector2[4]
    {
      new Vector2(0.0f, 0.0f),
      new Vector2(0.0f, 1f),
      new Vector2(1f, 0.0f),
      new Vector2(1f, 1f)
    };
    this.Mesh.triangles = new int[6]{ 0, 1, 2, 2, 1, 3 };
    this.Mesh.bounds = new Bounds(Vector3.zero, new Vector3(float.MaxValue, float.MaxValue, float.MaxValue));
    this.Texture = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGBHalf);
    this.Texture.name = nameof (LightBuffer);
    this.Camera.targetTexture = this.Texture;
  }

  private void LateUpdate()
  {
    if ((UnityEngine.Object) PropertyTextures.instance == (UnityEngine.Object) null)
      return;
    if (this.Texture.width != Screen.width || this.Texture.height != Screen.height)
    {
      this.Texture.DestroyRenderTexture();
      this.Texture = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGBHalf);
      this.Texture.name = nameof (LightBuffer);
      this.Camera.targetTexture = this.Texture;
    }
    Matrix4x4 matrix = new Matrix4x4();
    this.WorldLight = PropertyTextures.instance.GetTexture(PropertyTextures.Property.WorldLight);
    this.Material.SetTexture("_PropertyWorldLight", this.WorldLight);
    this.CircleMaterial.SetTexture("_PropertyWorldLight", this.WorldLight);
    this.ConeMaterial.SetTexture("_PropertyWorldLight", this.WorldLight);
    foreach (Light2D light2D in Components.Light2Ds.Items)
    {
      if (!((UnityEngine.Object) light2D == (UnityEngine.Object) null) && light2D.enabled)
      {
        MaterialPropertyBlock materialPropertyBlock = light2D.materialPropertyBlock;
        materialPropertyBlock.SetVector(this.ColorRangeTag, new Vector4(light2D.Color.r * light2D.IntensityAnimation, light2D.Color.g * light2D.IntensityAnimation, light2D.Color.b * light2D.IntensityAnimation, light2D.Range));
        Vector3 position = light2D.transform.GetPosition();
        position.x += light2D.Offset.x;
        position.y += light2D.Offset.y;
        materialPropertyBlock.SetVector(this.LightPosTag, new Vector4(position.x, position.y, 0.0f, 0.0f));
        Vector2 normalized = light2D.Direction.normalized;
        materialPropertyBlock.SetVector(this.LightDirectionAngleTag, new Vector4(normalized.x, normalized.y, 0.0f, light2D.Angle));
        Graphics.DrawMesh(this.Mesh, Vector3.zero, Quaternion.identity, this.Material, this.Layer, this.Camera, 0, materialPropertyBlock, false, false);
        if (light2D.drawOverlay)
        {
          materialPropertyBlock.SetColor(this.TintColorTag, light2D.overlayColour);
          switch (light2D.shape)
          {
            case LightShape.Circle:
              matrix.SetTRS(position, Quaternion.identity, Vector3.one * light2D.Range);
              Graphics.DrawMesh(this.Mesh, matrix, this.CircleMaterial, this.Layer, this.Camera, 0, materialPropertyBlock);
              continue;
            case LightShape.Cone:
              matrix.SetTRS(position - Vector3.up * (light2D.Range * 0.5f), Quaternion.identity, new Vector3(1f, 0.5f, 1f) * light2D.Range);
              Graphics.DrawMesh(this.Mesh, matrix, this.ConeMaterial, this.Layer, this.Camera, 0, materialPropertyBlock);
              continue;
            default:
              continue;
          }
        }
      }
    }
  }

  private void OnDestroy()
  {
    LightBuffer.Instance = (LightBuffer) null;
  }
}
