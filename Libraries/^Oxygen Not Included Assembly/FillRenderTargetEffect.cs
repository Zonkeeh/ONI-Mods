// Decompiled with JetBrains decompiler
// Type: FillRenderTargetEffect
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class FillRenderTargetEffect : MonoBehaviour
{
  private Texture fillTexture;

  public void SetFillTexture(Texture tex)
  {
    this.fillTexture = tex;
  }

  private void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    Graphics.Blit(this.fillTexture, (RenderTexture) null);
  }
}
