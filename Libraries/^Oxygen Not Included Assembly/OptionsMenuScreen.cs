// Decompiled with JetBrains decompiler
// Type: OptionsMenuScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OptionsMenuScreen : KModalButtonMenu
{
  [SerializeField]
  private GameOptionsScreen gameOptionsScreenPrefab;
  [SerializeField]
  private AudioOptionsScreen audioOptionsScreenPrefab;
  [SerializeField]
  private GraphicsOptionsScreen graphicsOptionsScreenPrefab;
  [SerializeField]
  private CreditsScreen creditsScreenPrefab;
  [SerializeField]
  private KButton closeButton;
  [SerializeField]
  private MetricsOptionsScreen metricsScreenPrefab;
  [SerializeField]
  private LocText title;
  [SerializeField]
  private KButton backButton;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.keepMenuOpen = true;
    this.buttons = (IList<KButtonMenu.ButtonInfo>) new List<KButtonMenu.ButtonInfo>()
    {
      new KButtonMenu.ButtonInfo((string) UI.FRONTEND.OPTIONS_SCREEN.GRAPHICS, Action.NumActions, new UnityAction(this.OnGraphicsOptions), (KButtonMenu.ButtonInfo.HoverCallback) null, (object) null),
      new KButtonMenu.ButtonInfo((string) UI.FRONTEND.OPTIONS_SCREEN.AUDIO, Action.NumActions, new UnityAction(this.OnAudioOptions), (KButtonMenu.ButtonInfo.HoverCallback) null, (object) null),
      new KButtonMenu.ButtonInfo((string) UI.FRONTEND.OPTIONS_SCREEN.GAME, Action.NumActions, new UnityAction(this.OnGameOptions), (KButtonMenu.ButtonInfo.HoverCallback) null, (object) null),
      new KButtonMenu.ButtonInfo((string) UI.FRONTEND.OPTIONS_SCREEN.METRICS, Action.NumActions, new UnityAction(this.OnMetrics), (KButtonMenu.ButtonInfo.HoverCallback) null, (object) null),
      new KButtonMenu.ButtonInfo((string) UI.FRONTEND.OPTIONS_SCREEN.CREDITS, Action.NumActions, new UnityAction(this.OnCredits), (KButtonMenu.ButtonInfo.HoverCallback) null, (object) null)
    };
    this.closeButton.onClick += new System.Action(((KScreen) this).Deactivate);
    this.backButton.onClick += new System.Action(((KScreen) this).Deactivate);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.title.SetText((string) UI.FRONTEND.OPTIONS_SCREEN.TITLE);
    this.backButton.transform.SetAsLastSibling();
  }

  protected override void OnActivate()
  {
    base.OnActivate();
    foreach (GameObject buttonObject in this.buttonObjects)
      ;
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (e.TryConsume(Action.Escape) || e.TryConsume(Action.MouseRight))
      this.Deactivate();
    else
      base.OnKeyDown(e);
  }

  private void OnGraphicsOptions()
  {
    this.ActivateChildScreen(this.graphicsOptionsScreenPrefab.gameObject);
  }

  private void OnAudioOptions()
  {
    this.ActivateChildScreen(this.audioOptionsScreenPrefab.gameObject);
  }

  private void OnGameOptions()
  {
    this.ActivateChildScreen(this.gameOptionsScreenPrefab.gameObject);
  }

  private void OnMetrics()
  {
    this.ActivateChildScreen(this.metricsScreenPrefab.gameObject);
  }

  private void OnCredits()
  {
    this.ActivateChildScreen(this.creditsScreenPrefab.gameObject);
  }

  private void Update()
  {
    Debug.developerConsoleVisible = false;
  }
}
