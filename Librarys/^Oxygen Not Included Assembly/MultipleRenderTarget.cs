// Decompiled with JetBrains decompiler
// Type: MultipleRenderTarget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class MultipleRenderTarget : MonoBehaviour
{
  private MultipleRenderTargetProxy renderProxy;
  private FullScreenQuad quad;
  public bool isFrontEnd;

  public event System.Action<Camera> onSetupComplete;

  private void Start()
  {
    this.StartCoroutine(this.SetupProxy());
  }

  [DebuggerHidden]
  private IEnumerator SetupProxy()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new MultipleRenderTarget.\u003CSetupProxy\u003Ec__Iterator0()
    {
      \u0024this = this
    };
  }

  private void OnPreCull()
  {
    if (!((UnityEngine.Object) this.renderProxy != (UnityEngine.Object) null))
      return;
    this.quad.Draw((Texture) this.renderProxy.Textures[0]);
  }

  public void ToggleColouredOverlayView(bool enabled)
  {
    if (!((UnityEngine.Object) this.renderProxy != (UnityEngine.Object) null))
      return;
    this.renderProxy.ToggleColouredOverlayView(enabled);
  }
}
