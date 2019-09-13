// Decompiled with JetBrains decompiler
// Type: NewGameSettingSeed
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.CustomSettings;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NewGameSettingSeed : NewGameSettingWidget
{
  [SerializeField]
  private LocText Label;
  [SerializeField]
  private ToolTip ToolTip;
  [SerializeField]
  private TMP_InputField Input;
  [SerializeField]
  private KButton RandomizeButton;
  [SerializeField]
  private Image BG;
  private const int MAX_VALID_SEED = 2147483647;
  private SeedSettingConfig config;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Input.onEndEdit.AddListener(new UnityAction<string>(this.OnEndEdit));
    this.Input.onValueChanged.AddListener(new UnityAction<string>(this.OnValueChanged));
    this.RandomizeButton.onClick += new System.Action(this.GetNewRandomSeed);
  }

  public void Initialize(SeedSettingConfig config)
  {
    this.config = config;
    this.Label.text = config.label;
    this.ToolTip.toolTip = config.tooltip;
    this.GetNewRandomSeed();
  }

  public override void Refresh()
  {
    string qualitySettingLevelId = CustomGameSettings.Instance.GetCurrentQualitySettingLevelId((SettingConfig) this.config);
    this.Input.text = qualitySettingLevelId;
    DebugUtil.LogArgs((object) "Set worldgen seed to", (object) int.Parse(qualitySettingLevelId));
  }

  private char ValidateInput(string text, int charIndex, char addedChar)
  {
    if ('0' <= addedChar && addedChar <= '9')
      return addedChar;
    return char.MinValue;
  }

  private void OnEndEdit(string text)
  {
    int seed;
    try
    {
      seed = Convert.ToInt32(text);
    }
    catch
    {
      seed = 0;
    }
    this.SetSeed(seed);
  }

  public void SetSeed(int seed)
  {
    seed = Mathf.Min(seed, int.MaxValue);
    CustomGameSettings.Instance.SetQualitySetting((SettingConfig) this.config, seed.ToString());
    this.Refresh();
  }

  private void OnValueChanged(string text)
  {
    int num = 0;
    try
    {
      num = Convert.ToInt32(text);
    }
    catch
    {
      this.Input.text = text.Length <= 0 ? string.Empty : text.Substring(0, text.Length - 1);
    }
    if (num <= int.MaxValue)
      return;
    this.Input.text = text.Substring(0, text.Length - 1);
  }

  private void GetNewRandomSeed()
  {
    this.SetSeed(UnityEngine.Random.Range(0, int.MaxValue));
  }
}
