// Decompiled with JetBrains decompiler
// Type: FullScreenQuad
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class FullScreenQuad
{
  private Mesh Mesh;
  private Camera Camera;
  private Material Material;
  private int Layer;

  public FullScreenQuad(string name, Camera camera, bool invert = false)
  {
    this.Camera = camera;
    this.Layer = LayerMask.NameToLayer("ForceDraw");
    this.Mesh = new Mesh();
    this.Mesh.name = name;
    this.Mesh.vertices = new Vector3[4]
    {
      new Vector3(-1f, -1f, 0.0f),
      new Vector3(-1f, 1f, 0.0f),
      new Vector3(1f, -1f, 0.0f),
      new Vector3(1f, 1f, 0.0f)
    };
    float y1 = 1f;
    float y2 = 0.0f;
    if (invert)
    {
      y1 = 0.0f;
      y2 = 1f;
    }
    this.Mesh.uv = new Vector2[4]
    {
      new Vector2(0.0f, y2),
      new Vector2(0.0f, y1),
      new Vector2(1f, y2),
      new Vector2(1f, y1)
    };
    this.Mesh.triangles = new int[6]{ 0, 1, 2, 2, 1, 3 };
    this.Mesh.bounds = new Bounds(Vector3.zero, new Vector3(float.MaxValue, float.MaxValue, float.MaxValue));
    this.Material = new Material(Shader.Find("Klei/PostFX/FullScreen"));
    this.Camera.cullingMask |= LayerMask.GetMask("ForceDraw");
  }

  public void Draw(Texture texture)
  {
    this.Material.mainTexture = texture;
    Graphics.DrawMesh(this.Mesh, Vector3.zero, Quaternion.identity, this.Material, this.Layer, this.Camera, 0, (MaterialPropertyBlock) null, false, false);
  }
}
