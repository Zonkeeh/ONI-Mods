// Decompiled with JetBrains decompiler
// Type: Slideshow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Slideshow : KMonoBehaviour
{
  public float timePerSlide = 1f;
  public float timeFactorForLastSlide = 3f;
  [SerializeField]
  private bool transparentIfEmpty = true;
  public RawImage imageTarget;
  private string[] files;
  private Sprite currentSlideImage;
  private Sprite[] sprites;
  private int currentSlide;
  private float timeUntilNextSlide;
  private bool paused;
  public bool playInThumbnail;
  public SlideshowUpdateType updateType;
  [SerializeField]
  private bool isExpandable;
  [SerializeField]
  private KButton button;
  [SerializeField]
  private KButton closeButton;
  [SerializeField]
  private KButton prevButton;
  [SerializeField]
  private KButton nextButton;
  [SerializeField]
  private KButton pauseButton;
  [SerializeField]
  private Image pauseIcon;
  [SerializeField]
  private Image unpauseIcon;
  public Slideshow.onBeforeAndEndPlayDelegate onBeforePlay;
  public Slideshow.onBeforeAndEndPlayDelegate onEndingPlay;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.timeUntilNextSlide = this.timePerSlide;
    if (this.transparentIfEmpty && this.sprites != null && this.sprites.Length == 0)
      this.imageTarget.color = Color.clear;
    if (this.isExpandable)
    {
      this.button = this.GetComponent<KButton>();
      this.button.onClick += (System.Action) (() =>
      {
        if (this.onBeforePlay != null)
          this.onBeforePlay();
        switch (this.updateType)
        {
          case SlideshowUpdateType.preloadedSprites:
            VideoScreen.Instance.PlaySlideShow(this.sprites);
            break;
          case SlideshowUpdateType.loadOnDemand:
            VideoScreen.Instance.PlaySlideShow(this.files);
            break;
        }
      });
    }
    if ((UnityEngine.Object) this.nextButton != (UnityEngine.Object) null)
      this.nextButton.onClick += (System.Action) (() => this.nextSlide());
    if ((UnityEngine.Object) this.prevButton != (UnityEngine.Object) null)
      this.prevButton.onClick += (System.Action) (() => this.prevSlide());
    if ((UnityEngine.Object) this.pauseButton != (UnityEngine.Object) null)
      this.pauseButton.onClick += (System.Action) (() => this.SetPaused(!this.paused));
    if (!((UnityEngine.Object) this.closeButton != (UnityEngine.Object) null))
      return;
    this.closeButton.onClick += (System.Action) (() =>
    {
      VideoScreen.Instance.Stop();
      if (this.onEndingPlay == null)
        return;
      this.onEndingPlay();
    });
  }

  public void SetPaused(bool state)
  {
    this.paused = state;
    if ((UnityEngine.Object) this.pauseIcon != (UnityEngine.Object) null)
      this.pauseIcon.gameObject.SetActive(!this.paused);
    if ((UnityEngine.Object) this.unpauseIcon != (UnityEngine.Object) null)
      this.unpauseIcon.gameObject.SetActive(this.paused);
    if ((UnityEngine.Object) this.prevButton != (UnityEngine.Object) null)
      this.prevButton.gameObject.SetActive(this.paused);
    if (!((UnityEngine.Object) this.nextButton != (UnityEngine.Object) null))
      return;
    this.nextButton.gameObject.SetActive(this.paused);
  }

  private void resetSlide(bool enable)
  {
    this.timeUntilNextSlide = this.timePerSlide;
    this.currentSlide = 0;
    if (enable)
    {
      this.imageTarget.color = Color.white;
    }
    else
    {
      if (!this.transparentIfEmpty)
        return;
      this.imageTarget.color = Color.clear;
    }
  }

  private Sprite loadSlide(string file)
  {
    float realtimeSinceStartup = Time.realtimeSinceStartup;
    Texture2D texture2D = new Texture2D(512, 768);
    texture2D.filterMode = FilterMode.Point;
    texture2D.LoadImage(File.ReadAllBytes(file));
    return Sprite.Create(texture2D, new Rect(Vector2.zero, new Vector2((float) texture2D.width, (float) texture2D.height)), new Vector2(0.5f, 0.5f), 100f, 0U, SpriteMeshType.FullRect);
  }

  public void SetFiles(string[] files, int loadFrame = -1)
  {
    if (files == null)
      return;
    this.files = files;
    bool enable = files.Length > 0 && files[0] != null;
    this.resetSlide(enable);
    if (!enable)
      return;
    int index = loadFrame == -1 ? files.Length - 1 : loadFrame;
    Sprite slide = this.loadSlide(files[index]);
    this.setSlide(slide);
    this.currentSlideImage = slide;
  }

  public void updateSize(Sprite sprite)
  {
    this.GetComponent<RectTransform>().sizeDelta = this.GetFittedSize(sprite, 960f, 960f);
  }

  public void SetSprites(Sprite[] sprites)
  {
    if (sprites == null)
      return;
    this.sprites = sprites;
    this.resetSlide(sprites.Length > 0 && (UnityEngine.Object) sprites[0] != (UnityEngine.Object) null);
    if (sprites.Length <= 0 || !((UnityEngine.Object) sprites[0] != (UnityEngine.Object) null))
      return;
    this.setSlide(sprites[0]);
  }

  public Vector2 GetFittedSize(Sprite sprite, float maxWidth, float maxHeight)
  {
    if ((UnityEngine.Object) sprite == (UnityEngine.Object) null || (UnityEngine.Object) sprite.texture == (UnityEngine.Object) null)
      return Vector2.zero;
    int width = sprite.texture.width;
    int height = sprite.texture.height;
    float num1 = maxWidth / (float) width;
    float num2 = maxHeight / (float) height;
    if ((double) num1 < (double) num2)
      return new Vector2((float) width * num1, (float) height * num1);
    return new Vector2((float) width * num2, (float) height * num2);
  }

  public void setSlide(Sprite slide)
  {
    if ((UnityEngine.Object) slide == (UnityEngine.Object) null)
      return;
    this.imageTarget.texture = (Texture) slide.texture;
    this.updateSize(slide);
  }

  public void nextSlide()
  {
    this.setSlideIndex(this.currentSlide + 1);
  }

  public void prevSlide()
  {
    this.setSlideIndex(this.currentSlide - 1);
  }

  private void setSlideIndex(int slideIndex)
  {
    this.timeUntilNextSlide = this.timePerSlide;
    switch (this.updateType)
    {
      case SlideshowUpdateType.preloadedSprites:
        if (slideIndex < 0)
          slideIndex = this.sprites.Length + slideIndex;
        this.currentSlide = slideIndex % this.sprites.Length;
        if (this.currentSlide == this.sprites.Length - 1)
          this.timeUntilNextSlide *= this.timeFactorForLastSlide;
        if (!this.playInThumbnail)
          break;
        this.setSlide(this.sprites[this.currentSlide]);
        break;
      case SlideshowUpdateType.loadOnDemand:
        if (slideIndex < 0)
          slideIndex = this.files.Length + slideIndex;
        this.currentSlide = slideIndex % this.files.Length;
        if (this.currentSlide == this.files.Length - 1)
          this.timeUntilNextSlide *= this.timeFactorForLastSlide;
        if (!this.playInThumbnail)
          break;
        if ((UnityEngine.Object) this.currentSlideImage != (UnityEngine.Object) null)
        {
          UnityEngine.Object.Destroy((UnityEngine.Object) this.currentSlideImage.texture);
          UnityEngine.Object.Destroy((UnityEngine.Object) this.currentSlideImage);
          GC.Collect();
        }
        this.currentSlideImage = this.loadSlide(this.files[this.currentSlide]);
        this.setSlide(this.currentSlideImage);
        break;
    }
  }

  private void Update()
  {
    if (this.updateType == SlideshowUpdateType.preloadedSprites && (this.sprites == null || this.sprites.Length <= 0) || this.updateType == SlideshowUpdateType.loadOnDemand && (this.files == null || this.files.Length <= 0) || this.paused)
      return;
    this.timeUntilNextSlide -= Time.unscaledDeltaTime;
    if ((double) this.timeUntilNextSlide > 0.0)
      return;
    this.nextSlide();
  }

  public delegate void onBeforeAndEndPlayDelegate();
}
