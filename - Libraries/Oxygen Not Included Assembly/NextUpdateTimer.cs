// Decompiled with JetBrains decompiler
// Type: NextUpdateTimer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using UnityEngine;

public class NextUpdateTimer : KMonoBehaviour
{
  public LocText TimerText;
  public KBatchedAnimController UpdateAnimController;
  public KBatchedAnimController UpdateAnimMeterController;
  public float initialAnimScale;
  public System.DateTime nextReleaseDate;
  public System.DateTime currentReleaseDate;
  private string m_releaseTextOverride;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.initialAnimScale = this.UpdateAnimController.animScale;
    ScreenResize.Instance.OnResize += new System.Action(this.RefreshScale);
  }

  protected override void OnCleanUp()
  {
    ScreenResize.Instance.OnResize -= new System.Action(this.RefreshScale);
    base.OnCleanUp();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.RefreshReleaseTimes();
  }

  public void UpdateReleaseTimes(string lastUpdateTime, string nextUpdateTime, string textOverride)
  {
    if (!System.DateTime.TryParse(lastUpdateTime, out this.currentReleaseDate))
      Debug.LogWarning((object) ("Failed to parse last_update_time: " + lastUpdateTime));
    if (!System.DateTime.TryParse(nextUpdateTime, out this.nextReleaseDate))
      Debug.LogWarning((object) ("Failed to parse next_update_time: " + nextUpdateTime));
    this.m_releaseTextOverride = textOverride;
    this.RefreshReleaseTimes();
    this.RefreshScale();
  }

  private void RefreshReleaseTimes()
  {
    TimeSpan timeSpan1 = this.nextReleaseDate - this.currentReleaseDate;
    TimeSpan timeSpan2 = this.nextReleaseDate - System.DateTime.UtcNow;
    TimeSpan timeSpan3 = System.DateTime.UtcNow - this.currentReleaseDate;
    string empty = string.Empty;
    string str1 = "4";
    string str2;
    if (!string.IsNullOrEmpty(this.m_releaseTextOverride))
      str2 = this.m_releaseTextOverride;
    else if (timeSpan2.TotalHours < 8.0)
    {
      str2 = (string) UI.DEVELOPMENTBUILDS.UPDATES.TWENTY_FOUR_HOURS;
      str1 = "4";
    }
    else if (timeSpan2.TotalDays < 1.0)
    {
      str2 = string.Format((string) UI.DEVELOPMENTBUILDS.UPDATES.FINAL_WEEK, (object) 1);
      str1 = "3";
    }
    else
    {
      int num1 = timeSpan2.Days % 7;
      int num2 = (timeSpan2.Days - num1) / 7;
      if (num2 <= 0)
      {
        str2 = string.Format((string) UI.DEVELOPMENTBUILDS.UPDATES.FINAL_WEEK, (object) num1);
        str1 = "2";
      }
      else
      {
        str2 = string.Format((string) UI.DEVELOPMENTBUILDS.UPDATES.BIGGER_TIMES, (object) num1, (object) num2);
        str1 = "1";
      }
    }
    this.TimerText.text = str2;
    this.UpdateAnimController.Play((HashedString) str1, KAnim.PlayMode.Loop, 1f, 0.0f);
    this.UpdateAnimMeterController.SetPositionPercent(Mathf.Clamp01((float) (timeSpan3.TotalSeconds / timeSpan1.TotalSeconds)));
  }

  private void RefreshScale()
  {
    float canvasScale = this.GetComponentInParent<KCanvasScaler>().GetCanvasScale();
    if ((UnityEngine.Object) this.UpdateAnimController != (UnityEngine.Object) null)
      this.UpdateAnimController.animScale = this.initialAnimScale * (1f / canvasScale);
    if (!((UnityEngine.Object) this.UpdateAnimMeterController != (UnityEngine.Object) null))
      return;
    this.UpdateAnimMeterController.animScale = this.initialAnimScale * (1f / canvasScale);
  }
}
