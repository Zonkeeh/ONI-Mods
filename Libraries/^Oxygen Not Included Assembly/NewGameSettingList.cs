// Decompiled with JetBrains decompiler
// Type: NewGameSettingList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.CustomSettings;
using UnityEngine;
using UnityEngine.UI;

public class NewGameSettingList : NewGameSettingWidget
{
  [SerializeField]
  private LocText Label;
  [SerializeField]
  private ToolTip ToolTip;
  [SerializeField]
  private LocText ValueLabel;
  [SerializeField]
  private ToolTip ValueToolTip;
  [SerializeField]
  private KButton CycleLeft;
  [SerializeField]
  private KButton CycleRight;
  [SerializeField]
  private Image BG;
  private ListSettingConfig config;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.CycleLeft.onClick += new System.Action(this.DoCycleLeft);
    this.CycleRight.onClick += new System.Action(this.DoCycleRight);
  }

  public void Initialize(ListSettingConfig config)
  {
    this.config = config;
    this.Label.text = config.label;
    this.ToolTip.toolTip = config.tooltip;
  }

  public override void Refresh()
  {
    SettingLevel currentQualitySetting = CustomGameSettings.Instance.GetCurrentQualitySetting((SettingConfig) this.config);
    this.ValueLabel.text = currentQualitySetting.label;
    this.ValueToolTip.toolTip = currentQualitySetting.tooltip;
    this.CycleLeft.isInteractable = !this.config.IsFirstLevel(currentQualitySetting.id);
    this.CycleRight.isInteractable = !this.config.IsLastLevel(currentQualitySetting.id);
  }

  private void DoCycleLeft()
  {
    CustomGameSettings.Instance.CycleSettingLevel(this.config, -1);
    this.Refresh();
  }

  private void DoCycleRight()
  {
    CustomGameSettings.Instance.CycleSettingLevel(this.config, 1);
    this.Refresh();
  }
}
