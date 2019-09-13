// Decompiled with JetBrains decompiler
// Type: FlowOffsetRenderer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class FlowOffsetRenderer : KMonoBehaviour
{
  public RenderTexture[] OffsetTextures = new RenderTexture[2];
  private float GasPhase0;
  private float GasPhase1;
  public float PhaseMultiplier;
  public float NoiseInfluence;
  public float NoiseScale;
  public float OffsetSpeed;
  public string OffsetTextureName;
  public string ParametersName;
  public Vector2 MinFlow0;
  public Vector2 MinFlow1;
  public Vector2 LiquidGasMask;
  [SerializeField]
  private Material FlowMaterial;
  [SerializeField]
  private bool forceUpdate;
  private TextureLerper FlowLerper;
  private int OffsetIdx;
  private float CurrentTime;

  protected override void OnSpawn()
  {
    this.FlowMaterial = new Material(Shader.Find("Klei/Flow"));
    ScreenResize.Instance.OnResize += new System.Action(this.OnResize);
    this.OnResize();
    this.DoUpdate(0.1f);
  }

  private void OnResize()
  {
    for (int index = 0; index < this.OffsetTextures.Length; ++index)
    {
      if ((UnityEngine.Object) this.OffsetTextures[index] != (UnityEngine.Object) null)
        this.OffsetTextures[index].DestroyRenderTexture();
      this.OffsetTextures[index] = new RenderTexture(Screen.width / 2, Screen.height / 2, 0, RenderTextureFormat.ARGBHalf);
      this.OffsetTextures[index].filterMode = FilterMode.Bilinear;
      this.OffsetTextures[index].name = "FlowOffsetTexture";
    }
  }

  private void LateUpdate()
  {
    if (((double) Time.deltaTime <= 0.0 || (double) Time.timeScale <= 0.0) && !this.forceUpdate)
      return;
    float num = Time.deltaTime / Time.timeScale;
    this.DoUpdate((float) ((double) num * (double) Time.timeScale / 4.0 + (double) num * 0.5));
  }

  private void DoUpdate(float dt)
  {
    this.CurrentTime += dt;
    float num1 = this.CurrentTime * this.PhaseMultiplier;
    float num2 = num1 - (float) (int) num1;
    float num3 = num2 - (float) (int) num2;
    float y = 1f;
    if ((double) num3 <= (double) this.GasPhase0)
      y = 0.0f;
    this.GasPhase0 = num3;
    float z = 1f;
    float num4 = num2 + 0.5f - (float) (int) ((double) num2 + 0.5);
    if ((double) num4 <= (double) this.GasPhase1)
      z = 0.0f;
    this.GasPhase1 = num4;
    Shader.SetGlobalVector(this.ParametersName, new Vector4(this.GasPhase0, 0.0f, 0.0f, 0.0f));
    Shader.SetGlobalVector("_NoiseParameters", new Vector4(this.NoiseInfluence, this.NoiseScale, 0.0f, 0.0f));
    RenderTexture offsetTexture1 = this.OffsetTextures[this.OffsetIdx];
    this.OffsetIdx = (this.OffsetIdx + 1) % 2;
    RenderTexture offsetTexture2 = this.OffsetTextures[this.OffsetIdx];
    Material flowMaterial = this.FlowMaterial;
    flowMaterial.SetTexture("_PreviousOffsetTex", (Texture) offsetTexture1);
    flowMaterial.SetVector("_FlowParameters", new Vector4(Time.deltaTime * this.OffsetSpeed, y, z, 0.0f));
    flowMaterial.SetVector("_MinFlow", new Vector4(this.MinFlow0.x, this.MinFlow0.y, this.MinFlow1.x, this.MinFlow1.y));
    flowMaterial.SetVector("_VisibleArea", new Vector4(0.0f, 0.0f, (float) Grid.WidthInCells, (float) Grid.HeightInCells));
    flowMaterial.SetVector("_LiquidGasMask", new Vector4(this.LiquidGasMask.x, this.LiquidGasMask.y, 0.0f, 0.0f));
    Graphics.Blit((Texture) offsetTexture1, offsetTexture2, flowMaterial);
    Shader.SetGlobalTexture(this.OffsetTextureName, (Texture) offsetTexture2);
  }
}
