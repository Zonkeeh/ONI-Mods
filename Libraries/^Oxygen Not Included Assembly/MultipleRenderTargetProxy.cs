// Decompiled with JetBrains decompiler
// Type: MultipleRenderTargetProxy
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class MultipleRenderTargetProxy : MonoBehaviour
{
  public RenderTexture[] Textures = new RenderTexture[3];
  private bool colouredOverlayBufferEnabled;

  private void Start()
  {
    ScreenResize.Instance.OnResize += new System.Action(this.OnResize);
    this.CreateRenderTarget();
    ShaderReloader.Register(new System.Action(this.OnShadersReloaded));
  }

  public void ToggleColouredOverlayView(bool enabled)
  {
    this.colouredOverlayBufferEnabled = enabled;
    this.CreateRenderTarget();
  }

  private void CreateRenderTarget()
  {
    RenderBuffer[] colorBuffer = new RenderBuffer[!this.colouredOverlayBufferEnabled ? 2 : 3];
    this.Textures[0] = this.RecreateRT(this.Textures[0], 24, RenderTextureFormat.ARGB32);
    this.Textures[0].filterMode = FilterMode.Point;
    this.Textures[0].name = "MRT0";
    this.Textures[1] = this.RecreateRT(this.Textures[1], 0, RenderTextureFormat.ARGB32);
    this.Textures[1].filterMode = FilterMode.Point;
    this.Textures[1].name = "MRT1";
    colorBuffer[0] = this.Textures[0].colorBuffer;
    colorBuffer[1] = this.Textures[1].colorBuffer;
    if (this.colouredOverlayBufferEnabled)
    {
      this.Textures[2] = this.RecreateRT(this.Textures[2], 0, RenderTextureFormat.ARGB32);
      this.Textures[2].filterMode = FilterMode.Bilinear;
      this.Textures[2].name = "MRT2";
      colorBuffer[2] = this.Textures[2].colorBuffer;
    }
    this.GetComponent<Camera>().SetTargetBuffers(colorBuffer, this.Textures[0].depthBuffer);
    this.OnShadersReloaded();
  }

  private RenderTexture RecreateRT(
    RenderTexture rt,
    int depth,
    RenderTextureFormat format)
  {
    RenderTexture renderTexture = rt;
    if ((UnityEngine.Object) rt == (UnityEngine.Object) null || rt.width != Screen.width || (rt.height != Screen.height || rt.format != format))
    {
      if ((UnityEngine.Object) rt != (UnityEngine.Object) null)
        rt.DestroyRenderTexture();
      renderTexture = new RenderTexture(Screen.width, Screen.height, depth, format);
    }
    return renderTexture;
  }

  private void OnResize()
  {
    this.CreateRenderTarget();
  }

  private void Update()
  {
    if (this.Textures[0].IsCreated())
      return;
    this.CreateRenderTarget();
  }

  private void OnShadersReloaded()
  {
    Shader.SetGlobalTexture("_MRT0", (Texture) this.Textures[0]);
    Shader.SetGlobalTexture("_MRT1", (Texture) this.Textures[1]);
    if (!this.colouredOverlayBufferEnabled)
      return;
    Shader.SetGlobalTexture("_MRT2", (Texture) this.Textures[2]);
  }
}
