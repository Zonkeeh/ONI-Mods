// Decompiled with JetBrains decompiler
// Type: TopLeftControlScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class TopLeftControlScreen : KScreen
{
  public static TopLeftControlScreen Instance;
  [SerializeField]
  private MultiToggle SandboxToggle;
  [SerializeField]
  private LocText locText;

  public static void DestroyInstance()
  {
    TopLeftControlScreen.Instance = (TopLeftControlScreen) null;
  }

  protected override void OnActivate()
  {
    base.OnActivate();
    TopLeftControlScreen.Instance = this;
    this.RefreshName();
    this.UpdateSandboxToggleState();
    this.SandboxToggle.onClick += new System.Action(this.OnClickSandboxToggle);
    Game.Instance.Subscribe(-1948169901, (System.Action<object>) (data => this.UpdateSandboxToggleState()));
  }

  public void RefreshName()
  {
    if (!((UnityEngine.Object) SaveGame.Instance != (UnityEngine.Object) null))
      return;
    this.locText.text = SaveGame.Instance.BaseName;
  }

  public void UpdateSandboxToggleState()
  {
    if (this.CheckSandboxModeLocked())
    {
      this.SandboxToggle.GetComponent<ToolTip>().SetSimpleTooltip(GameUtil.ReplaceHotkeyString((string) UI.SANDBOX_TOGGLE.TOOLTIP_LOCKED, Action.ToggleSandboxTools));
      this.SandboxToggle.ChangeState(0);
    }
    else
    {
      this.SandboxToggle.GetComponent<ToolTip>().SetSimpleTooltip(GameUtil.ReplaceHotkeyString((string) UI.SANDBOX_TOGGLE.TOOLTIP_UNLOCKED, Action.ToggleSandboxTools));
      this.SandboxToggle.ChangeState(!Game.Instance.SandboxModeActive ? 1 : 2);
    }
    this.SandboxToggle.gameObject.SetActive(SaveGame.Instance.sandboxEnabled);
  }

  private void OnClickSandboxToggle()
  {
    if (this.CheckSandboxModeLocked())
    {
      KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative", false));
    }
    else
    {
      KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click", false));
      Game.Instance.SandboxModeActive = !Game.Instance.SandboxModeActive;
    }
    this.UpdateSandboxToggleState();
  }

  private bool CheckSandboxModeLocked()
  {
    return !SaveGame.Instance.sandboxEnabled;
  }
}
