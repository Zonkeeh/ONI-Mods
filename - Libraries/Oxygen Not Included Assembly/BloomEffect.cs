// Decompiled with JetBrains decompiler
// Type: BloomEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public class BloomEffect : MonoBehaviour
{
  public int iterations = 3;
  public float blurSpread = 0.6f;
  private Material BloomMaskMaterial;
  private Material BloomCompositeMaterial;
  public Shader blurShader;
  private Material m_Material;

  protected Material material
  {
    get
    {
      if ((UnityEngine.Object) this.m_Material == (UnityEngine.Object) null)
      {
        this.m_Material = new Material(this.blurShader);
        this.m_Material.hideFlags = HideFlags.DontSave;
      }
      return this.m_Material;
    }
  }

  protected void OnDisable()
  {
    if (!(bool) ((UnityEngine.Object) this.m_Material))
      return;
    UnityEngine.Object.DestroyImmediate((UnityEngine.Object) this.m_Material);
  }

  protected void Start()
  {
    if (!SystemInfo.supportsImageEffects)
      this.enabled = false;
    else if (!(bool) ((UnityEngine.Object) this.blurShader) || !this.material.shader.isSupported)
    {
      this.enabled = false;
    }
    else
    {
      this.BloomMaskMaterial = new Material(Shader.Find("Klei/PostFX/BloomMask"));
      this.BloomCompositeMaterial = new Material(Shader.Find("Klei/PostFX/BloomComposite"));
    }
  }

  public void FourTapCone(RenderTexture source, RenderTexture dest, int iteration)
  {
    float num = (float) (0.5 + (double) iteration * (double) this.blurSpread);
    Graphics.BlitMultiTap((Texture) source, dest, this.material, new Vector2(-num, -num), new Vector2(-num, num), new Vector2(num, num), new Vector2(num, -num));
  }

  private void DownSample4x(RenderTexture source, RenderTexture dest)
  {
    float num = 1f;
    Graphics.BlitMultiTap((Texture) source, dest, this.material, new Vector2(-num, -num), new Vector2(-num, num), new Vector2(num, num), new Vector2(num, -num));
  }

  private void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    RenderTexture temporary1 = RenderTexture.GetTemporary(source.width, source.height, 0);
    temporary1.name = "bloom_source";
    Graphics.Blit((Texture) source, temporary1, this.BloomMaskMaterial);
    int width = Math.Max(source.width / 4, 4);
    int height = Math.Max(source.height / 4, 4);
    RenderTexture renderTexture = RenderTexture.GetTemporary(width, height, 0);
    renderTexture.name = "bloom_downsampled";
    this.DownSample4x(temporary1, renderTexture);
    RenderTexture.ReleaseTemporary(temporary1);
    for (int iteration = 0; iteration < this.iterations; ++iteration)
    {
      RenderTexture temporary2 = RenderTexture.GetTemporary(width, height, 0);
      temporary2.name = "bloom_blurred";
      this.FourTapCone(renderTexture, temporary2, iteration);
      RenderTexture.ReleaseTemporary(renderTexture);
      renderTexture = temporary2;
    }
    this.BloomCompositeMaterial.SetTexture("_BloomTex", (Texture) renderTexture);
    Graphics.Blit((Texture) source, destination, this.BloomCompositeMaterial);
    RenderTexture.ReleaseTemporary(renderTexture);
  }
}
