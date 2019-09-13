// Decompiled with JetBrains decompiler
// Type: GameOptionsScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class GameOptionsScreen : KModalButtonMenu
{
  [SerializeField]
  private SaveConfigurationScreen saveConfiguration;
  [SerializeField]
  private UnitConfigurationScreen unitConfiguration;
  [SerializeField]
  private KButton resetTutorialButton;
  [SerializeField]
  private KButton controlsButton;
  [SerializeField]
  private KButton sandboxButton;
  [SerializeField]
  private KButton doneButton;
  [SerializeField]
  private KButton closeButton;
  [SerializeField]
  private GameObject savePanel;
  [SerializeField]
  private InputBindingsScreen inputBindingsScreenPrefab;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.unitConfiguration.Init();
    if ((UnityEngine.Object) SaveGame.Instance != (UnityEngine.Object) null)
    {
      this.saveConfiguration.ToggleDisabledContent(true);
      this.saveConfiguration.Init();
      this.SetSandboxModeActive(SaveGame.Instance.sandboxEnabled);
    }
    else
      this.saveConfiguration.ToggleDisabledContent(false);
    this.resetTutorialButton.onClick += new System.Action(this.OnTutorialReset);
    this.controlsButton.onClick += new System.Action(this.OnKeyBindings);
    this.sandboxButton.onClick += new System.Action(this.OnUnlockSandboxMode);
    this.doneButton.onClick += new System.Action(((KScreen) this).Deactivate);
    this.closeButton.onClick += new System.Action(((KScreen) this).Deactivate);
  }

  protected override void OnShow(bool show)
  {
    base.OnShow(show);
    if ((UnityEngine.Object) SaveGame.Instance != (UnityEngine.Object) null)
    {
      this.savePanel.SetActive(true);
      this.saveConfiguration.Show(show);
      this.SetSandboxModeActive(SaveGame.Instance.sandboxEnabled);
    }
    else
      this.savePanel.SetActive(false);
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (e.TryConsume(Action.Escape) || e.TryConsume(Action.MouseRight))
      this.Deactivate();
    else
      base.OnKeyDown(e);
  }

  private void OnTutorialReset()
  {
    ConfirmDialogScreen component = this.ActivateChildScreen(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject).GetComponent<ConfirmDialogScreen>();
    component.PopupConfirmDialog((string) UI.FRONTEND.OPTIONS_SCREEN.RESET_TUTORIAL_WARNING, (System.Action) (() => Tutorial.ResetHiddenTutorialMessages()), (System.Action) (() => {}), (string) null, (System.Action) null, (string) null, (string) null, (string) null, (Sprite) null, true);
    component.Activate();
  }

  private void OnUnlockSandboxMode()
  {
    ConfirmDialogScreen component = this.ActivateChildScreen(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject).GetComponent<ConfirmDialogScreen>();
    ConfirmDialogScreen confirmDialogScreen = component;
    string unlockSandboxWarning = (string) UI.FRONTEND.OPTIONS_SCREEN.TOGGLE_SANDBOX_SCREEN.UNLOCK_SANDBOX_WARNING;
    System.Action action1 = (System.Action) (() =>
    {
      SaveGame.Instance.sandboxEnabled = true;
      this.SetSandboxModeActive(SaveGame.Instance.sandboxEnabled);
      TopLeftControlScreen.Instance.UpdateSandboxToggleState();
      this.Deactivate();
    });
    System.Action action2 = (System.Action) (() =>
    {
      SaveLoader.Instance.Save(SaveLoader.GetSavePrefixAndCreateFolder() + "\\" + SaveGame.Instance.BaseName + (string) UI.FRONTEND.OPTIONS_SCREEN.TOGGLE_SANDBOX_SCREEN.BACKUP_SAVE_GAME_APPEND + ".sav", false, false);
      this.SetSandboxModeActive(SaveGame.Instance.sandboxEnabled);
      TopLeftControlScreen.Instance.UpdateSandboxToggleState();
      this.Deactivate();
    });
    string cancel = (string) UI.FRONTEND.OPTIONS_SCREEN.TOGGLE_SANDBOX_SCREEN.CANCEL;
    System.Action action3 = (System.Action) (() => {});
    string confirmSaveBackup = (string) UI.FRONTEND.OPTIONS_SCREEN.TOGGLE_SANDBOX_SCREEN.CONFIRM_SAVE_BACKUP;
    string text = unlockSandboxWarning;
    System.Action on_confirm = action1;
    System.Action on_cancel = action2;
    string configurable_text = cancel;
    System.Action on_configurable_clicked = action3;
    string confirm = (string) UI.FRONTEND.OPTIONS_SCREEN.TOGGLE_SANDBOX_SCREEN.CONFIRM;
    string cancel_text = confirmSaveBackup;
    confirmDialogScreen.PopupConfirmDialog(text, on_confirm, on_cancel, configurable_text, on_configurable_clicked, (string) null, confirm, cancel_text, (Sprite) null, true);
    component.Activate();
  }

  private void OnKeyBindings()
  {
    this.ActivateChildScreen(this.inputBindingsScreenPrefab.gameObject);
  }

  private void SetSandboxModeActive(bool active)
  {
    this.sandboxButton.GetComponent<HierarchyReferences>().GetReference("Checkmark").gameObject.SetActive(active);
    this.sandboxButton.isInteractable = !active;
    this.sandboxButton.gameObject.GetComponentInParent<CanvasGroup>().alpha = !active ? 1f : 0.5f;
  }
}
