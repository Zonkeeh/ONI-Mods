// Decompiled with JetBrains decompiler
// Type: SaveUpgradeWarning
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using Klei.CustomSettings;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;

public class SaveUpgradeWarning : KMonoBehaviour
{
  [MyCmpReq]
  private Game game;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.game.OnLoad += new System.Action<Game.GameSaveData>(this.OnLoad);
  }

  protected override void OnCleanUp()
  {
    this.game.OnLoad -= new System.Action<Game.GameSaveData>(this.OnLoad);
    base.OnCleanUp();
  }

  private void OnLoad(Game.GameSaveData data)
  {
    foreach (SaveUpgradeWarning.Upgrade upgrade in new List<SaveUpgradeWarning.Upgrade>()
    {
      new SaveUpgradeWarning.Upgrade(7, 5, new System.Action(this.SuddenMoraleHelper))
    })
    {
      if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(upgrade.major, upgrade.minor))
        upgrade.action();
    }
  }

  private void SuddenMoraleHelper()
  {
    Effect morale_effect = Db.Get().effects.Get(nameof (SuddenMoraleHelper));
    CustomizableDialogScreen screen = Util.KInstantiateUI<CustomizableDialogScreen>(ScreenPrefabs.Instance.CustomizableDialogScreen.gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject, true);
    screen.AddOption((string) UI.FRONTEND.SAVEUPGRADEWARNINGS.SUDDENMORALEHELPER_BUFF, (System.Action) (() =>
    {
      foreach (Component component in Components.LiveMinionIdentities.Items)
        component.GetComponent<Effects>().Add(morale_effect, true);
      screen.Deactivate();
    }));
    screen.AddOption((string) UI.FRONTEND.SAVEUPGRADEWARNINGS.SUDDENMORALEHELPER_DISABLE, (System.Action) (() =>
    {
      SettingConfig morale = CustomGameSettingConfigs.Morale;
      CustomGameSettings.Instance.customGameMode = CustomGameSettings.CustomGameMode.Custom;
      CustomGameSettings.Instance.SetQualitySetting(morale, morale.GetLevel("Disabled").id);
      screen.Deactivate();
    }));
    screen.PopupConfirmDialog(string.Format((string) UI.FRONTEND.SAVEUPGRADEWARNINGS.SUDDENMORALEHELPER, (object) Mathf.RoundToInt(morale_effect.duration / 600f)), (string) UI.FRONTEND.SAVEUPGRADEWARNINGS.SUDDENMORALEHELPER_TITLE, (Sprite) null);
  }

  private struct Upgrade
  {
    public int major;
    public int minor;
    public System.Action action;

    public Upgrade(int major, int minor, System.Action action)
    {
      this.major = major;
      this.minor = minor;
      this.action = action;
    }
  }
}
