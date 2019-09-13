// Decompiled with JetBrains decompiler
// Type: NewGameSettingsPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.CustomSettings;
using KMod;
using ProcGen;
using ProcGenGame;
using System.Collections.Generic;
using UnityEngine;

public class NewGameSettingsPanel : KMonoBehaviour
{
  [SerializeField]
  private Transform content;
  [SerializeField]
  private KButton closeButton;
  [SerializeField]
  private KButton background;
  [UnityEngine.Header("Prefab UI Refs")]
  [SerializeField]
  private GameObject prefab_cycle_setting;
  [SerializeField]
  private GameObject prefab_slider_setting;
  [SerializeField]
  private GameObject prefab_checkbox_setting;
  [SerializeField]
  private GameObject prefab_seed_input_setting;
  private CustomGameSettings settings;
  private List<NewGameSettingWidget> widgets;

  public void SetCloseAction(System.Action onClose)
  {
    if ((UnityEngine.Object) this.closeButton != (UnityEngine.Object) null)
      this.closeButton.onClick += onClose;
    if (!((UnityEngine.Object) this.background != (UnityEngine.Object) null))
      return;
    this.background.onClick += onClose;
  }

  public void Init()
  {
    Global.Instance.modManager.Load(Content.LayerableFiles);
    SettingsCache.Clear();
    WorldGen.LoadSettings();
    CustomGameSettings.Instance.LoadWorlds();
    Global.Instance.modManager.Report(this.gameObject);
    this.settings = CustomGameSettings.Instance;
    this.widgets = new List<NewGameSettingWidget>();
    foreach (KeyValuePair<string, SettingConfig> qualitySetting in this.settings.QualitySettings)
    {
      if (!qualitySetting.Value.debug_only || DebugHandler.enabled)
      {
        ListSettingConfig config1 = qualitySetting.Value as ListSettingConfig;
        if (config1 != null)
        {
          NewGameSettingList newGameSettingList = Util.KInstantiateUI<NewGameSettingList>(this.prefab_cycle_setting, this.content.gameObject, true);
          newGameSettingList.Initialize(config1);
          this.widgets.Add((NewGameSettingWidget) newGameSettingList);
        }
        else
        {
          ToggleSettingConfig config2 = qualitySetting.Value as ToggleSettingConfig;
          if (config2 != null)
          {
            NewGameSettingToggle gameSettingToggle = Util.KInstantiateUI<NewGameSettingToggle>(this.prefab_checkbox_setting, this.content.gameObject, true);
            gameSettingToggle.Initialize(config2);
            this.widgets.Add((NewGameSettingWidget) gameSettingToggle);
          }
          else
          {
            SeedSettingConfig config3 = qualitySetting.Value as SeedSettingConfig;
            if (config3 != null)
            {
              NewGameSettingSeed newGameSettingSeed = Util.KInstantiateUI<NewGameSettingSeed>(this.prefab_seed_input_setting, this.content.gameObject, true);
              newGameSettingSeed.Initialize(config3);
              this.widgets.Add((NewGameSettingWidget) newGameSettingSeed);
            }
          }
        }
      }
    }
    this.Refresh();
  }

  public void Refresh()
  {
    foreach (NewGameSettingWidget widget in this.widgets)
      widget.Refresh();
  }

  public void SetSetting(SettingConfig setting, string level)
  {
    this.settings.SetQualitySetting(setting, level);
  }

  public string GetSetting(SettingConfig setting)
  {
    return this.settings.GetCurrentQualitySetting(setting).id;
  }

  public void Cancel()
  {
    Global.Instance.modManager.Unload(Content.LayerableFiles);
    SettingsCache.Clear();
  }
}
