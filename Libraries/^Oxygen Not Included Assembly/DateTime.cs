// Decompiled with JetBrains decompiler
// Type: DateTime
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using UnityEngine;

public class DateTime : KScreen
{
  private int displayedDayCount = -1;
  public static DateTime Instance;
  public LocText day;
  [SerializeField]
  private LocText text;
  [SerializeField]
  private ToolTip tooltip;
  [SerializeField]
  private TextStyleSetting tooltipstyle_Days;
  [SerializeField]
  private TextStyleSetting tooltipstyle_Playtime;
  [SerializeField]
  public KToggle scheduleToggle;

  public static void DestroyInstance()
  {
    DateTime.Instance = (DateTime) null;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    DateTime.Instance = this;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.tooltip.OnToolTip = new Func<string>(this.OnToolTip);
  }

  private void Update()
  {
    if (!((UnityEngine.Object) GameClock.Instance != (UnityEngine.Object) null) || this.displayedDayCount == GameUtil.GetCurrentCycle())
      return;
    this.text.text = this.Days();
    this.displayedDayCount = GameUtil.GetCurrentCycle();
  }

  private string Days()
  {
    return GameUtil.GetCurrentCycle().ToString();
  }

  private string OnToolTip()
  {
    if ((UnityEngine.Object) GameClock.Instance != (UnityEngine.Object) null)
    {
      this.tooltip.ClearMultiStringTooltip();
      this.tooltip.AddMultiStringTooltip(string.Format((string) UI.ASTEROIDCLOCK.CYCLES_OLD, (object) this.Days()), (ScriptableObject) this.tooltipstyle_Days);
      this.tooltip.AddMultiStringTooltip(string.Format((string) UI.ASTEROIDCLOCK.TIME_PLAYED, (object) (GameClock.Instance.GetTimePlayedInSeconds() / 3600f).ToString("0.00")), (ScriptableObject) this.tooltipstyle_Playtime);
    }
    return string.Empty;
  }
}
