// Decompiled with JetBrains decompiler
// Type: FogOfWarPostFX
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class FogOfWarPostFX : MonoBehaviour
{
  [SerializeField]
  private Shader shader;
  private Material material;
  private Camera myCamera;

  private void Awake()
  {
    this.enabled = SystemInfo.supportsImageEffects;
    if (!((Object) this.shader != (Object) null))
      return;
    this.material = new Material(this.shader);
  }

  private void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    this.SetupUVs();
    Graphics.Blit((Texture) source, destination, this.material, 0);
  }

  private void SetupUVs()
  {
    if ((Object) this.myCamera == (Object) null)
    {
      this.myCamera = this.GetComponent<Camera>();
      if ((Object) this.myCamera == (Object) null)
        return;
    }
    Ray ray1 = this.myCamera.ViewportPointToRay(Vector3.zero);
    float distance1 = Mathf.Abs(ray1.origin.z / ray1.direction.z);
    Vector3 point1 = ray1.GetPoint(distance1);
    Vector4 vector4;
    vector4.x = point1.x / Grid.WidthInMeters;
    vector4.y = point1.y / Grid.HeightInMeters;
    Ray ray2 = this.myCamera.ViewportPointToRay(Vector3.one);
    float distance2 = Mathf.Abs(ray2.origin.z / ray2.direction.z);
    Vector3 point2 = ray2.GetPoint(distance2);
    vector4.z = point2.x / Grid.WidthInMeters - vector4.x;
    vector4.w = point2.y / Grid.HeightInMeters - vector4.y;
    this.material.SetVector("_UVOffsetScale", vector4);
  }
}
