// Decompiled with JetBrains decompiler
// Type: KModalScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

public class KModalScreen : KScreen
{
  public bool pause = true;
  private bool shown;
  public const float SCREEN_SORT_KEY = 100f;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.ConsumeMouseScroll = true;
    this.activateOnSpawn = true;
  }

  protected override void OnCmpEnable()
  {
    base.OnCmpEnable();
    if (!((Object) CameraController.Instance != (Object) null))
      return;
    CameraController.Instance.DisableUserCameraControl = true;
  }

  protected override void OnCmpDisable()
  {
    base.OnCmpDisable();
    if ((Object) CameraController.Instance != (Object) null)
      CameraController.Instance.DisableUserCameraControl = false;
    this.Trigger(476357528, (object) null);
  }

  public override bool IsModal()
  {
    return true;
  }

  public override float GetSortKey()
  {
    return 100f;
  }

  protected override void OnActivate()
  {
    this.OnShow(true);
  }

  protected override void OnDeactivate()
  {
    this.OnShow(false);
  }

  protected override void OnShow(bool show)
  {
    base.OnShow(show);
    if (!this.pause || !((Object) SpeedControlScreen.Instance != (Object) null))
      return;
    if (show && !this.shown)
      SpeedControlScreen.Instance.Pause(false);
    else if (!show && this.shown)
      SpeedControlScreen.Instance.Unpause(false);
    this.shown = show;
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if ((Object) Game.Instance != (Object) null && (e.TryConsume(Action.TogglePause) || e.TryConsume(Action.CycleSpeed)))
      KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative", false));
    if (!e.Consumed && e.TryConsume(Action.Escape))
      this.Deactivate();
    if (!e.Consumed)
    {
      KScrollRect componentInChildren = this.GetComponentInChildren<KScrollRect>();
      if ((Object) componentInChildren != (Object) null)
        componentInChildren.OnKeyDown(e);
    }
    e.Consumed = true;
  }

  public override void OnKeyUp(KButtonEvent e)
  {
    if (!e.Consumed)
    {
      KScrollRect componentInChildren = this.GetComponentInChildren<KScrollRect>();
      if ((Object) componentInChildren != (Object) null)
        componentInChildren.OnKeyUp(e);
    }
    e.Consumed = true;
  }

  public void SetBackgroundActive(bool active)
  {
    this.GetComponent<Image>().color = (Color) new Color32((byte) 0, (byte) 0, (byte) 0, !active ? (byte) 0 : (byte) 190);
  }
}
