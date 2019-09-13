// Decompiled with JetBrains decompiler
// Type: VideoWidget
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoWidget : KMonoBehaviour
{
  [SerializeField]
  private VideoClip clip;
  [SerializeField]
  private VideoPlayer thumbnailPlayer;
  [SerializeField]
  private KButton button;
  [SerializeField]
  private string overlayName;
  [SerializeField]
  private List<string> texts;
  private RenderTexture renderTexture;
  private RawImage rawImage;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.button.onClick += new System.Action(this.Clicked);
    this.rawImage = this.thumbnailPlayer.GetComponent<RawImage>();
  }

  private void Clicked()
  {
    VideoScreen.Instance.PlayVideo(this.clip, false, string.Empty, false);
    if (string.IsNullOrEmpty(this.overlayName))
      return;
    VideoScreen.Instance.SetOverlayText(this.overlayName, this.texts);
  }

  public void SetClip(VideoClip clip, string overlayName = null, List<string> texts = null)
  {
    if ((UnityEngine.Object) clip == (UnityEngine.Object) null)
    {
      Debug.LogWarning((object) "Tried to assign null video clip to VideoWidget");
    }
    else
    {
      this.clip = clip;
      this.overlayName = overlayName;
      this.texts = texts;
      this.renderTexture = new RenderTexture(Convert.ToInt32(clip.width), Convert.ToInt32(clip.height), 16);
      this.thumbnailPlayer.targetTexture = this.renderTexture;
      this.rawImage.texture = (Texture) this.renderTexture;
      this.StartCoroutine(this.ConfigureThumbnail());
    }
  }

  [DebuggerHidden]
  private IEnumerator ConfigureThumbnail()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new VideoWidget.\u003CConfigureThumbnail\u003Ec__Iterator0()
    {
      \u0024this = this
    };
  }

  private void Update()
  {
    if (!this.thumbnailPlayer.isPlaying || this.thumbnailPlayer.time <= 2.0)
      return;
    this.thumbnailPlayer.Pause();
  }
}
