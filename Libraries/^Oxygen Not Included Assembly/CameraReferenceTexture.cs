// Decompiled with JetBrains decompiler
// Type: CameraReferenceTexture
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class CameraReferenceTexture : MonoBehaviour
{
  public Camera referenceCamera;
  private FullScreenQuad quad;

  private void OnPreCull()
  {
    if (this.quad == null)
      this.quad = new FullScreenQuad(nameof (CameraReferenceTexture), this.GetComponent<Camera>(), this.referenceCamera.GetComponent<CameraRenderTexture>().ShouldFlip());
    if (!((Object) this.referenceCamera != (Object) null))
      return;
    this.quad.Draw((Texture) this.referenceCamera.GetComponent<CameraRenderTexture>().GetTexture());
  }
}
