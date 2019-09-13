// Decompiled with JetBrains decompiler
// Type: StoryMessageScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class StoryMessageScreen : KScreen
{
  public bool restoreInterfaceOnClose = true;
  private const float ALPHA_SPEED = 0.01f;
  [SerializeField]
  private Image bg;
  [SerializeField]
  private GameObject dialog;
  [SerializeField]
  private KButton button;
  [SerializeField]
  [EventRef]
  private string dialogSound;
  [SerializeField]
  private LocText titleLabel;
  [SerializeField]
  private LocText bodyLabel;
  private const float expandedHeight = 300f;
  [SerializeField]
  private GameObject content;
  public System.Action OnClose;
  private bool startFade;

  public string title
  {
    set
    {
      this.titleLabel.SetText(value);
    }
  }

  public string body
  {
    set
    {
      this.bodyLabel.SetText(value);
    }
  }

  public override float GetSortKey()
  {
    return 8f;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    StoryMessageScreen.HideInterface(true);
    CameraController.Instance.FadeOut(0.5f, 1f);
  }

  [DebuggerHidden]
  private IEnumerator ExpandPanel()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new StoryMessageScreen.\u003CExpandPanel\u003Ec__Iterator0()
    {
      \u0024this = this
    };
  }

  [DebuggerHidden]
  private IEnumerator CollapsePanel()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new StoryMessageScreen.\u003CCollapsePanel\u003Ec__Iterator1()
    {
      \u0024this = this
    };
  }

  public static void HideInterface(bool hide)
  {
    NotificationScreen.Instance.Show(!hide);
    OverlayMenu.Instance.Show(!hide);
    if ((UnityEngine.Object) PlanScreen.Instance != (UnityEngine.Object) null)
      PlanScreen.Instance.Show(!hide);
    if ((UnityEngine.Object) BuildMenu.Instance != (UnityEngine.Object) null)
      BuildMenu.Instance.Show(!hide);
    ManagementMenu.Instance.Show(!hide);
    ToolMenu.Instance.Show(!hide);
    ToolMenu.Instance.PriorityScreen.Show(!hide);
    ResourceCategoryScreen.Instance.Show(!hide);
    TopLeftControlScreen.Instance.Show(!hide);
    DateTime.Instance.Show(!hide);
    BuildWatermark.Instance.Show(!hide);
    PopFXManager.Instance.Show(!hide);
  }

  public void Update()
  {
    if (!this.startFade)
      return;
    Color color = this.bg.color;
    color.a -= 0.01f;
    if ((double) color.a <= 0.0)
      color.a = 0.0f;
    this.bg.color = color;
  }

  protected override void OnActivate()
  {
    base.OnActivate();
    SelectTool.Instance.Select((KSelectable) null, false);
    this.button.onClick += (System.Action) (() => this.StartCoroutine(this.CollapsePanel()));
    this.dialog.GetComponent<KScreen>().Show(false);
    this.startFade = false;
    CameraController.Instance.DisableUserCameraControl = true;
    KFMOD.PlayOneShot(this.dialogSound);
    this.dialog.GetComponent<KScreen>().Activate();
    this.dialog.GetComponent<KScreen>().SetShouldFadeIn(true);
    this.dialog.GetComponent<KScreen>().Show(true);
    MusicManager.instance.PlaySong("Music_Victory_01_Message", false);
    this.StartCoroutine(this.ExpandPanel());
  }

  protected override void OnDeactivate()
  {
    if (!this.IsActive())
      ;
    base.OnDeactivate();
    MusicManager.instance.StopSong("Music_Victory_01_Message", true, STOP_MODE.ALLOWFADEOUT);
    if (!this.restoreInterfaceOnClose)
      return;
    CameraController.Instance.DisableUserCameraControl = false;
    CameraController.Instance.FadeIn(0.0f, 1f);
    StoryMessageScreen.HideInterface(false);
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (e.TryConsume(Action.Escape))
      this.StartCoroutine(this.CollapsePanel());
    e.Consumed = true;
  }

  public override void OnKeyUp(KButtonEvent e)
  {
    e.Consumed = true;
  }
}
