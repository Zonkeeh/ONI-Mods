// Decompiled with JetBrains decompiler
// Type: SplashMessageScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class SplashMessageScreen : KMonoBehaviour
{
  public KButton confirmButton;
  public LayoutElement bodyText;
  public bool previewInEditor;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.confirmButton.onClick += (System.Action) (() =>
    {
      this.gameObject.SetActive(false);
      AudioMixer.instance.Stop((HashedString) AudioMixerSnapshots.Get().FrontEndWelcomeScreenSnapshot, STOP_MODE.ALLOWFADEOUT);
    });
  }

  private void OnEnable()
  {
    LayoutElement component = this.confirmButton.GetComponent<LayoutElement>();
    LocText componentInChildren = this.confirmButton.GetComponentInChildren<LocText>();
    if (Screen.width > 2560)
    {
      component.minWidth = 720f;
      component.minHeight = 128f;
      this.bodyText.minWidth = 840f;
      componentInChildren.fontSizeMax = 24f;
    }
    else if (Screen.width > 1920)
    {
      component.minWidth = 720f;
      component.minHeight = 128f;
      this.bodyText.minWidth = 700f;
      componentInChildren.fontSizeMax = 24f;
    }
    else if (Screen.width > 1280)
    {
      component.minWidth = 440f;
      component.minHeight = 64f;
      this.bodyText.minWidth = 480f;
      componentInChildren.fontSizeMax = 18f;
    }
    else
    {
      component.minWidth = 300f;
      component.minHeight = 48f;
      this.bodyText.minWidth = 300f;
      componentInChildren.fontSizeMax = 16f;
    }
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    AudioMixer.instance.Start(AudioMixerSnapshots.Get().FrontEndWelcomeScreenSnapshot);
    this.StartCoroutine(this.ShowMessage());
  }

  [DebuggerHidden]
  private IEnumerator ShowMessage()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new SplashMessageScreen.\u003CShowMessage\u003Ec__Iterator0()
    {
      \u0024this = this
    };
  }
}
