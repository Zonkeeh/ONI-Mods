// Decompiled with JetBrains decompiler
// Type: Lighting
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

[ExecuteInEditMode]
public class Lighting : MonoBehaviour
{
  public LightingSettings Settings;
  public static Lighting Instance;
  [NonSerialized]
  public bool disableLighting;

  private void Awake()
  {
    Lighting.Instance = this;
  }

  private void OnDestroy()
  {
    Lighting.Instance = (Lighting) null;
  }

  private Color PremultiplyAlpha(Color c)
  {
    return c * c.a;
  }

  private void Update()
  {
    Shader.SetGlobalInt("_LiquidZ", -28);
    Shader.SetGlobalInt("_SceneLayerMax", 34);
    Shader.SetGlobalColor("_StateTransitionColour", this.Settings.StateTransitionColor);
    Shader.SetGlobalVector("_DigMapMapParameters", new Vector4(this.Settings.DigMapColour.r, this.Settings.DigMapColour.g, this.Settings.DigMapColour.b, this.Settings.DigMapScale));
    Shader.SetGlobalTexture("_DigDamageMap", (Texture) this.Settings.DigDamageMap);
    Shader.SetGlobalTexture("_StateTransitionMap", (Texture) this.Settings.StateTransitionMap);
    Shader.SetGlobalColor("_StateTransitionColor", this.Settings.StateTransitionColor);
    Shader.SetGlobalVector("_StateTransitionParameters", new Vector4(1f / this.Settings.StateTransitionUVScale, this.Settings.StateTransitionUVOffsetRate.x, this.Settings.StateTransitionUVOffsetRate.y, 0.0f));
    Shader.SetGlobalColor("_WaterTrimColor", this.Settings.WaterTrimColor);
    Shader.SetGlobalVector("_WaterParameters2", new Vector4(this.Settings.WaterTrimSize, this.Settings.WaterAlphaTrimSize, 0.0f, this.Settings.WaterAlphaThreshold));
    Shader.SetGlobalVector("_WaterWaveParameters", new Vector4(this.Settings.WaterWaveAmplitude, this.Settings.WaterWaveFrequency, this.Settings.WaterWaveSpeed, 0.0f));
    Shader.SetGlobalVector("_WaterWaveParameters2", new Vector4(this.Settings.WaterWaveAmplitude2, this.Settings.WaterWaveFrequency2, this.Settings.WaterWaveSpeed2, 0.0f));
    Shader.SetGlobalVector("_WaterDetailParameters", new Vector4(this.Settings.WaterCubeMapScale, this.Settings.WaterDetailTiling, this.Settings.WaterColorScale, this.Settings.WaterDetailTiling2));
    Shader.SetGlobalVector("_WaterDistortionParameters", new Vector4(this.Settings.WaterDistortionScaleStart, this.Settings.WaterDistortionScaleEnd, this.Settings.WaterDepthColorOpacityStart, this.Settings.WaterDepthColorOpacityEnd));
    Shader.SetGlobalVector("_BloomParameters", new Vector4(this.Settings.BloomScale, 0.0f, 0.0f, 0.0f));
    Shader.SetGlobalVector("_LiquidParameters2", new Vector4(this.Settings.LiquidMin, this.Settings.LiquidMax, this.Settings.LiquidCutoff, this.Settings.LiquidTransparency));
    Shader.SetGlobalVector("_GridParameters", new Vector4(this.Settings.GridLineWidth, this.Settings.GridSize, this.Settings.GridMinIntensity, this.Settings.GridMaxIntensity));
    Shader.SetGlobalColor("_GridColor", this.Settings.GridColor);
    Shader.SetGlobalVector("_EdgeGlowParameters", new Vector4(this.Settings.EdgeGlowCutoffStart, this.Settings.EdgeGlowCutoffEnd, this.Settings.EdgeGlowIntensity, 0.0f));
    if (this.disableLighting)
    {
      Shader.SetGlobalVector("_SubstanceParameters", new Vector4(1f, 1f, 1f, 1f));
      Shader.SetGlobalVector("_TileEdgeParameters", new Vector4(1f, 1f, 1f, 1f));
    }
    else
    {
      Shader.SetGlobalVector("_SubstanceParameters", new Vector4(this.Settings.substanceEdgeParameters.intensity, this.Settings.substanceEdgeParameters.edgeIntensity, this.Settings.substanceEdgeParameters.directSunlightScale, this.Settings.substanceEdgeParameters.power));
      Shader.SetGlobalVector("_TileEdgeParameters", new Vector4(this.Settings.tileEdgeParameters.intensity, this.Settings.tileEdgeParameters.edgeIntensity, this.Settings.tileEdgeParameters.directSunlightScale, this.Settings.tileEdgeParameters.power));
    }
    float w = !((UnityEngine.Object) SimDebugView.Instance != (UnityEngine.Object) null) || !(SimDebugView.Instance.GetMode() == OverlayModes.Disease.ID) ? 0.0f : 1f;
    if (this.disableLighting)
      Shader.SetGlobalVector("_AnimParameters", new Vector4(1f, this.Settings.WorldZoneAnimBlend, 0.0f, w));
    else
      Shader.SetGlobalVector("_AnimParameters", new Vector4(this.Settings.AnimIntensity, this.Settings.WorldZoneAnimBlend, 0.0f, w));
    Shader.SetGlobalVector("_GasOpacity", new Vector4(this.Settings.GasMinOpacity, this.Settings.GasMaxOpacity, 0.0f, 0.0f));
    Shader.SetGlobalColor("_DarkenTintBackground", this.Settings.DarkenTints[0]);
    Shader.SetGlobalColor("_DarkenTintMidground", this.Settings.DarkenTints[1]);
    Shader.SetGlobalColor("_DarkenTintForeground", this.Settings.DarkenTints[2]);
    Shader.SetGlobalColor("_BrightenOverlay", this.Settings.BrightenOverlayColour);
    Shader.SetGlobalColor("_ColdFG", this.PremultiplyAlpha(this.Settings.ColdColours[2]));
    Shader.SetGlobalColor("_ColdMG", this.PremultiplyAlpha(this.Settings.ColdColours[1]));
    Shader.SetGlobalColor("_ColdBG", this.PremultiplyAlpha(this.Settings.ColdColours[0]));
    Shader.SetGlobalColor("_HotFG", this.PremultiplyAlpha(this.Settings.HotColours[2]));
    Shader.SetGlobalColor("_HotMG", this.PremultiplyAlpha(this.Settings.HotColours[1]));
    Shader.SetGlobalColor("_HotBG", this.PremultiplyAlpha(this.Settings.HotColours[0]));
    Shader.SetGlobalVector("_TemperatureParallax", this.Settings.TemperatureParallax);
    Shader.SetGlobalVector("_ColdUVOffset1", new Vector4(this.Settings.ColdBGUVOffset.x, this.Settings.ColdBGUVOffset.y, this.Settings.ColdMGUVOffset.x, this.Settings.ColdMGUVOffset.y));
    Shader.SetGlobalVector("_ColdUVOffset2", new Vector4(this.Settings.ColdFGUVOffset.x, this.Settings.ColdFGUVOffset.y, 0.0f, 0.0f));
    Shader.SetGlobalVector("_HotUVOffset1", new Vector4(this.Settings.HotBGUVOffset.x, this.Settings.HotBGUVOffset.y, this.Settings.HotMGUVOffset.x, this.Settings.HotMGUVOffset.y));
    Shader.SetGlobalVector("_HotUVOffset2", new Vector4(this.Settings.HotFGUVOffset.x, this.Settings.HotFGUVOffset.y, 0.0f, 0.0f));
    Shader.SetGlobalColor("_DustColour", this.PremultiplyAlpha(this.Settings.DustColour));
    Shader.SetGlobalVector("_DustInfo", new Vector4(this.Settings.DustScale, this.Settings.DustMovement.x, this.Settings.DustMovement.y, this.Settings.DustMovement.z));
    Shader.SetGlobalTexture("_DustTex", (Texture) this.Settings.DustTex);
    Shader.SetGlobalVector("_DebugShowInfo", new Vector4(this.Settings.ShowDust, this.Settings.ShowGas, this.Settings.ShowShadow, this.Settings.ShowTemperature));
    Shader.SetGlobalVector("_HeatHazeParameters", this.Settings.HeatHazeParameters);
    Shader.SetGlobalTexture("_HeatHazeTexture", (Texture) this.Settings.HeatHazeTexture);
    Shader.SetGlobalVector("_ShineParams", new Vector4(this.Settings.ShineCenter.x, this.Settings.ShineCenter.y, this.Settings.ShineRange.x, this.Settings.ShineRange.y));
    Shader.SetGlobalVector("_ShineParams2", new Vector4(this.Settings.ShineZoomSpeed, 0.0f, 0.0f, 0.0f));
    Shader.SetGlobalFloat("_WorldZoneGasBlend", this.Settings.WorldZoneGasBlend);
    Shader.SetGlobalFloat("_WorldZoneLiquidBlend", this.Settings.WorldZoneLiquidBlend);
    Shader.SetGlobalFloat("_WorldZoneForegroundBlend", this.Settings.WorldZoneForegroundBlend);
    Shader.SetGlobalFloat("_WorldZoneSimpleAnimBlend", this.Settings.WorldZoneSimpleAnimBlend);
    Shader.SetGlobalColor("_CharacterLitColour", (Color) this.Settings.characterLighting.litColour);
    Shader.SetGlobalColor("_CharacterUnlitColour", (Color) this.Settings.characterLighting.unlitColour);
    Shader.SetGlobalTexture("_BuildingDamagedTex", (Texture) this.Settings.BuildingDamagedTex);
    Shader.SetGlobalVector("_BuildingDamagedUVParameters", this.Settings.BuildingDamagedUVParameters);
    Shader.SetGlobalTexture("_DiseaseOverlayTex", (Texture) this.Settings.DiseaseOverlayTex);
    Shader.SetGlobalVector("_DiseaseOverlayTexInfo", this.Settings.DiseaseOverlayTexInfo);
    if (!((UnityEngine.Object) LightBuffer.Instance != (UnityEngine.Object) null) || !((UnityEngine.Object) LightBuffer.Instance.Texture != (UnityEngine.Object) null))
      return;
    Shader.SetGlobalTexture("_LightBufferTex", (Texture) LightBuffer.Instance.Texture);
  }
}
