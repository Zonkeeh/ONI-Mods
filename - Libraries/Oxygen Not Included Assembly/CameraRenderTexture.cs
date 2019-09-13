// Decompiled with JetBrains decompiler
// Type: CameraRenderTexture
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class CameraRenderTexture : MonoBehaviour
{
  public string TextureName;
  private RenderTexture resultTexture;
  private Material material;

  private void Awake()
  {
    this.material = new Material(Shader.Find("Klei/PostFX/CameraRenderTexture"));
  }

  private void Start()
  {
    ScreenResize.Instance.OnResize += new System.Action(this.OnResize);
    this.OnResize();
  }

  private void OnResize()
  {
    if ((UnityEngine.Object) this.resultTexture != (UnityEngine.Object) null)
      this.resultTexture.DestroyRenderTexture();
    this.resultTexture = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGB32);
    this.resultTexture.name = this.name;
    this.resultTexture.filterMode = FilterMode.Point;
    this.resultTexture.autoGenerateMips = false;
    if (!(this.TextureName != string.Empty))
      return;
    Shader.SetGlobalTexture(this.TextureName, (Texture) this.resultTexture);
  }

  private void OnRenderImage(RenderTexture source, RenderTexture dest)
  {
    Graphics.Blit((Texture) source, this.resultTexture, this.material);
  }

  public RenderTexture GetTexture()
  {
    return this.resultTexture;
  }

  public bool ShouldFlip()
  {
    return false;
  }
}
