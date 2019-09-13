// Decompiled with JetBrains decompiler
// Type: PopFX
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

public class PopFX : KMonoBehaviour
{
  private float Speed = 2f;
  private Sprite icon;
  private string text;
  private Transform targetTransform;
  private Vector3 offset;
  public Image IconDisplay;
  public LocText TextDisplay;
  public CanvasGroup canvasGroup;
  private Camera uiCamera;
  private float lifetime;
  private float lifeElapsed;
  private bool trackTarget;
  private Vector3 startPos;
  private bool isLive;

  public void Recycle()
  {
    this.icon = (Sprite) null;
    this.text = string.Empty;
    this.targetTransform = (Transform) null;
    this.lifeElapsed = 0.0f;
    this.trackTarget = false;
    this.startPos = Vector3.zero;
    this.IconDisplay.color = Color.white;
    this.TextDisplay.color = Color.white;
    PopFXManager.Instance.RecycleFX(this);
    this.canvasGroup.alpha = 0.0f;
    this.gameObject.SetActive(false);
    this.isLive = false;
  }

  public void Spawn(
    Sprite Icon,
    string Text,
    Transform TargetTransform,
    Vector3 Offset,
    float LifeTime = 1.5f,
    bool TrackTarget = false)
  {
    this.icon = Icon;
    this.text = Text;
    this.targetTransform = TargetTransform;
    this.trackTarget = TrackTarget;
    this.lifetime = LifeTime;
    this.offset = Offset;
    if ((Object) this.targetTransform != (Object) null)
    {
      this.startPos = this.targetTransform.GetPosition();
      int x;
      int y;
      Grid.PosToXY(this.startPos, out x, out y);
      if (y % 2 != 0)
        this.startPos.x += 0.5f;
    }
    this.TextDisplay.text = this.text;
    this.IconDisplay.sprite = this.icon;
    this.canvasGroup.alpha = 1f;
    this.isLive = true;
    this.Update();
  }

  private void Update()
  {
    if (!this.isLive || !PopFXManager.Instance.Ready())
      return;
    this.lifeElapsed += Time.unscaledDeltaTime;
    if ((double) this.lifeElapsed >= (double) this.lifetime)
      this.Recycle();
    if (this.trackTarget && (Object) this.targetTransform != (Object) null)
    {
      Vector3 screen = PopFXManager.Instance.WorldToScreen(this.targetTransform.GetPosition() + this.offset + Vector3.up * this.lifeElapsed * (this.Speed * this.lifeElapsed));
      screen.z = 0.0f;
      this.gameObject.rectTransform().anchoredPosition = (Vector2) screen;
    }
    else
    {
      Vector3 screen = PopFXManager.Instance.WorldToScreen(this.startPos + this.offset + Vector3.up * this.lifeElapsed * (this.Speed * (this.lifeElapsed / 2f)));
      screen.z = 0.0f;
      this.gameObject.rectTransform().anchoredPosition = (Vector2) screen;
    }
    this.canvasGroup.alpha = (float) (1.5 * (((double) this.lifetime - (double) this.lifeElapsed) / (double) this.lifetime));
  }
}
