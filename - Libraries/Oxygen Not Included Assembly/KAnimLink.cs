// Decompiled with JetBrains decompiler
// Type: KAnimLink
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class KAnimLink
{
  private KAnimControllerBase master;
  private KAnimControllerBase slave;

  public KAnimLink(KAnimControllerBase master, KAnimControllerBase slave)
  {
    this.slave = slave;
    this.master = master;
    this.Register();
  }

  private void Register()
  {
    this.master.OnOverlayColourChanged += new System.Action<Color32>(this.OnOverlayColourChanged);
    this.master.OnTintChanged += new System.Action<Color>(this.OnTintColourChanged);
    this.master.OnHighlightChanged += new System.Action<Color>(this.OnHighlightColourChanged);
    this.master.onLayerChanged += new System.Action<int>(this.slave.SetLayer);
  }

  public void Unregister()
  {
    if (!((UnityEngine.Object) this.master != (UnityEngine.Object) null))
      return;
    this.master.OnOverlayColourChanged -= new System.Action<Color32>(this.OnOverlayColourChanged);
    this.master.OnTintChanged -= new System.Action<Color>(this.OnTintColourChanged);
    this.master.OnHighlightChanged -= new System.Action<Color>(this.OnHighlightColourChanged);
    if (!((UnityEngine.Object) this.slave != (UnityEngine.Object) null))
      return;
    this.master.onLayerChanged -= new System.Action<int>(this.slave.SetLayer);
  }

  private void OnOverlayColourChanged(Color32 c)
  {
    if (!((UnityEngine.Object) this.slave != (UnityEngine.Object) null))
      return;
    this.slave.OverlayColour = (Color) c;
  }

  private void OnTintColourChanged(Color c)
  {
    if (!((UnityEngine.Object) this.slave != (UnityEngine.Object) null))
      return;
    this.slave.TintColour = (Color32) c;
  }

  private void OnHighlightColourChanged(Color c)
  {
    if (!((UnityEngine.Object) this.slave != (UnityEngine.Object) null))
      return;
    this.slave.HighlightColour = (Color32) c;
  }
}
