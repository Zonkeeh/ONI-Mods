// Decompiled with JetBrains decompiler
// Type: DisplayNodeEditor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using LibNoiseDotNet.Graphics.Tools.Noise;
using LibNoiseDotNet.Graphics.Tools.Noise.Builder;
using NodeEditorFramework;
using ProcGen;
using ProcGen.Noise;
using ProcGenGame;
using UnityEngine;

[NodeEditorFramework.Node(false, "Noise/Display", new System.Type[] {typeof (NoiseNodeCanvas)})]
public class DisplayNodeEditor : BaseNodeEditor
{
  private WorldGenSettings worldGenSettings;
  private const string Id = "displayNodeEditor";
  [SerializeField]
  public DisplayNodeEditor.DisplayType displayType;
  private const int width = 256;
  private const int height = 256;
  private Texture2D texture;
  private ElementBandConfiguration biome;
  private string[] biomeOptions;
  private string[] featureOptions;

  public override string GetID
  {
    get
    {
      return "displayNodeEditor";
    }
  }

  public override System.Type GetObjectType
  {
    get
    {
      return typeof (DisplayNodeEditor);
    }
  }

  public override NoiseBase GetTarget()
  {
    return (NoiseBase) null;
  }

  public override NodeEditorFramework.Node Create(Vector2 pos)
  {
    DisplayNodeEditor instance = ScriptableObject.CreateInstance<DisplayNodeEditor>();
    instance.rect = new Rect(pos.x, pos.y, 266f, 301f);
    instance.name = "Noise Display Node";
    instance.CreateInput("Source Node", "IModule3D", NodeSide.Left, 40f);
    return (NodeEditorFramework.Node) instance;
  }

  public override bool Calculate()
  {
    if (!this.allInputsReady() || this.settings == null)
      return false;
    IModule3D module3D = this.Inputs[0].GetValue<IModule3D>();
    if (module3D == null)
      return false;
    this.InitSettings();
    Vector2f lowerBound = this.settings.lowerBound;
    Vector2f upperBound = this.settings.upperBound;
    NoiseMapBuilderPlane nmbp = new NoiseMapBuilderPlane(lowerBound.x, upperBound.x, lowerBound.y, upperBound.y, this.settings.seamless);
    nmbp.SetSize(256, 256);
    nmbp.SourceModule = (IModule) module3D;
    float[] noise = WorldGen.GenerateNoise(Vector2.zero, this.settings.zoom, nmbp, 256, 256, (NoiseMapBuilderCallback) null);
    if (this.settings.normalise)
      WorldGen.Normalise(noise);
    GetColourDelegate getColourCall = (GetColourDelegate) null;
    switch (this.displayType)
    {
      case DisplayNodeEditor.DisplayType.DefaultColour:
        getColourCall = (GetColourDelegate) (cell => Color.HSVToRGB((float) ((40.0 + 320.0 * (double) noise[cell]) / 360.0), 1f, 1f));
        break;
      case DisplayNodeEditor.DisplayType.ElementColourBiome:
      case DisplayNodeEditor.DisplayType.ElementColourFeature:
        getColourCall = (GetColourDelegate) (cell =>
        {
          if (this.biome == null)
            return Color.black;
          float num = noise[cell];
          Element elementByName = ElementLoader.FindElementByName(this.biome[this.biome.Count - 1].content);
          for (int index = 0; index < this.biome.Count; ++index)
          {
            if ((double) num < (double) this.biome[index].maxValue)
            {
              elementByName = ElementLoader.FindElementByName(this.biome[index].content);
              break;
            }
          }
          return (Color) elementByName.substance.uiColour;
        });
        break;
    }
    if (getColourCall != null)
      this.SetColours(getColourCall);
    return true;
  }

  private void SetColours(GetColourDelegate getColourCall)
  {
    byte[] textureBytes;
    this.texture = SimDebugView.CreateTexture(out textureBytes, 256, 256);
    for (int cell = 0; cell < 65536; ++cell)
    {
      Color color = getColourCall(cell);
      int index = cell * 4;
      textureBytes[index] = (byte) ((double) Mathf.Min(color.r, 1f) * (double) byte.MaxValue);
      textureBytes[index + 1] = (byte) ((double) Mathf.Min(color.g, 1f) * (double) byte.MaxValue);
      textureBytes[index + 2] = (byte) ((double) Mathf.Min(color.b, 1f) * (double) byte.MaxValue);
      textureBytes[index + 3] = byte.MaxValue;
    }
    this.texture.LoadRawTextureData(textureBytes);
    this.texture.Apply();
  }

  private void InitSettings()
  {
    if (this.worldGenSettings != null)
      return;
    this.worldGenSettings = SaveGame.Instance.worldGen.Settings;
  }

  private void GetBiomeOptions()
  {
    if (this.biomeOptions != null)
      return;
    this.InitSettings();
    this.biomeOptions = SettingsCache.biomes.GetNames();
  }

  private void GetFeatureOptions()
  {
    if (this.featureOptions != null)
      return;
    this.InitSettings();
    this.featureOptions = SettingsCache.GetCachedFeatureNames().ToArray();
  }

  protected override void NodeGUI()
  {
    base.NodeGUI();
  }

  public enum DisplayType
  {
    DefaultColour,
    ElementColourBiome,
    ElementColourFeature,
  }
}
