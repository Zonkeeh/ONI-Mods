// Decompiled with JetBrains decompiler
// Type: LightBufferCompositor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class LightBufferCompositor : MonoBehaviour
{
  private bool particlesEnabled = true;
  [SerializeField]
  private Material material;
  [SerializeField]
  private Material blurMaterial;

  private void Start()
  {
    this.material = new Material(Shader.Find("Klei/PostFX/LightBufferCompositor"));
    this.material.SetTexture("_InvalidTex", (Texture) Assets.instance.invalidAreaTex);
    this.blurMaterial = new Material(Shader.Find("Klei/PostFX/Blur"));
    this.OnShadersReloaded();
    ShaderReloader.Register(new System.Action(this.OnShadersReloaded));
  }

  private void OnEnable()
  {
    this.OnShadersReloaded();
  }

  private void DownSample4x(Texture source, RenderTexture dest)
  {
    float num = 1f;
    Graphics.BlitMultiTap(source, dest, this.blurMaterial, new Vector2(-num, -num), new Vector2(-num, num), new Vector2(num, num), new Vector2(num, -num));
  }

  [ContextMenu("ToggleParticles")]
  private void ToggleParticles()
  {
    this.particlesEnabled = !this.particlesEnabled;
    this.UpdateMaterialState();
  }

  public void SetParticlesEnabled(bool enabled)
  {
    this.particlesEnabled = enabled;
    this.UpdateMaterialState();
  }

  private void UpdateMaterialState()
  {
    if (this.particlesEnabled)
      this.material.DisableKeyword("DISABLE_TEMPERATURE_PARTICLES");
    else
      this.material.EnableKeyword("DISABLE_TEMPERATURE_PARTICLES");
  }

  private void OnRenderImage(RenderTexture src, RenderTexture dest)
  {
    if ((UnityEngine.Object) PropertyTextures.instance == (UnityEngine.Object) null)
      return;
    Texture texture = PropertyTextures.instance.GetTexture(PropertyTextures.Property.Temperature);
    texture.name = "temperature_tex";
    RenderTexture temporary = RenderTexture.GetTemporary(Screen.width / 8, Screen.height / 8);
    temporary.filterMode = FilterMode.Bilinear;
    Graphics.Blit(texture, temporary, this.blurMaterial);
    Shader.SetGlobalTexture("_BlurredTemperature", (Texture) temporary);
    this.material.SetTexture("_LightBufferTex", (Texture) LightBuffer.Instance.Texture);
    Graphics.Blit((Texture) src, dest, this.material);
    RenderTexture.ReleaseTemporary(temporary);
  }

  private void OnShadersReloaded()
  {
    if (!((UnityEngine.Object) this.material != (UnityEngine.Object) null) || !((UnityEngine.Object) Lighting.Instance != (UnityEngine.Object) null))
      return;
    this.material.SetTexture("_EmberTex", (Texture) Lighting.Instance.Settings.EmberTex);
    this.material.SetTexture("_FrostTex", (Texture) Lighting.Instance.Settings.FrostTex);
    this.material.SetTexture("_Thermal1Tex", (Texture) Lighting.Instance.Settings.Thermal1Tex);
    this.material.SetTexture("_Thermal2Tex", (Texture) Lighting.Instance.Settings.Thermal2Tex);
  }
}
