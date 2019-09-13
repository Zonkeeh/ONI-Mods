// Decompiled with JetBrains decompiler
// Type: Infrared
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class Infrared : MonoBehaviour
{
  private RenderTexture minionTexture;
  private RenderTexture cameraTexture;
  private Infrared.Mode mode;
  public static int temperatureParametersId;
  public static Infrared Instance;

  public static void DestroyInstance()
  {
    Infrared.Instance = (Infrared) null;
  }

  private void Awake()
  {
    Infrared.temperatureParametersId = Shader.PropertyToID("_TemperatureParameters");
    Infrared.Instance = this;
    this.OnResize();
    this.UpdateState();
  }

  private void OnRenderImage(RenderTexture source, RenderTexture dest)
  {
    Graphics.Blit((Texture) source, this.minionTexture);
    Graphics.Blit((Texture) source, dest);
  }

  private void OnResize()
  {
    if ((Object) this.minionTexture != (Object) null)
      this.minionTexture.DestroyRenderTexture();
    if ((Object) this.cameraTexture != (Object) null)
      this.cameraTexture.DestroyRenderTexture();
    int num = 2;
    this.minionTexture = new RenderTexture(Screen.width / num, Screen.height / num, 0, RenderTextureFormat.ARGB32);
    this.cameraTexture = new RenderTexture(Screen.width / num, Screen.height / num, 0, RenderTextureFormat.ARGB32);
    this.GetComponent<Camera>().targetTexture = this.cameraTexture;
  }

  public void SetMode(Infrared.Mode mode)
  {
    Vector4 vector4;
    switch (mode)
    {
      case Infrared.Mode.Disabled:
        vector4 = Vector4.zero;
        break;
      case Infrared.Mode.Disease:
        vector4 = new Vector4(1f, 0.0f, 0.0f, 0.0f);
        GameComps.InfraredVisualizers.ClearOverlayColour();
        break;
      default:
        vector4 = new Vector4(1f, 0.0f, 0.0f, 0.0f);
        break;
    }
    Shader.SetGlobalVector("_ColouredOverlayParameters", vector4);
    this.mode = mode;
    this.UpdateState();
  }

  private void UpdateState()
  {
    this.enabled = this.mode != Infrared.Mode.Disabled;
    if (!this.enabled)
      return;
    this.Update();
  }

  private void Update()
  {
    switch (this.mode)
    {
      case Infrared.Mode.Infrared:
        GameComps.InfraredVisualizers.UpdateTemperature();
        break;
      case Infrared.Mode.Disease:
        GameComps.DiseaseContainers.UpdateOverlayColours();
        break;
    }
  }

  public enum Mode
  {
    Disabled,
    Infrared,
    Disease,
  }
}
