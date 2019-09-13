// Decompiled with JetBrains decompiler
// Type: GameClock
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using KSerialization;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class GameClock : KMonoBehaviour, ISaveLoadable, ISim33ms, IRender1000ms
{
  public static readonly string NewCycleKey = "NewCycle";
  private Dictionary<string, object> newDayMetric = new Dictionary<string, object>()
  {
    {
      GameClock.NewCycleKey,
      (object) null
    }
  };
  public static GameClock Instance;
  [Serialize]
  private int frame;
  [Serialize]
  private float time;
  [Serialize]
  private float timeSinceStartOfCycle;
  [Serialize]
  private int cycle;
  [Serialize]
  private float timePlayed;
  [Serialize]
  private bool isNight;

  public static void DestroyInstance()
  {
    GameClock.Instance = (GameClock) null;
  }

  protected override void OnPrefabInit()
  {
    GameClock.Instance = this;
    this.timeSinceStartOfCycle = 50f;
  }

  [OnDeserialized]
  private void OnDeserialized()
  {
    if ((double) this.time == 0.0)
      return;
    this.cycle = (int) ((double) this.time / 600.0);
    this.timeSinceStartOfCycle = Mathf.Max(this.time - (float) this.cycle * 600f, 0.0f);
    this.time = 0.0f;
  }

  public void Sim33ms(float dt)
  {
    this.AddTime(dt);
  }

  public void Render1000ms(float dt)
  {
    this.timePlayed += dt;
  }

  private void LateUpdate()
  {
    ++this.frame;
  }

  private void AddTime(float dt)
  {
    this.timeSinceStartOfCycle += dt;
    bool flag = false;
    while ((double) this.timeSinceStartOfCycle >= 600.0)
    {
      ++this.cycle;
      this.timeSinceStartOfCycle -= 600f;
      this.Trigger(631075836, (object) null);
      flag = true;
    }
    if (!this.isNight && this.IsNighttime())
    {
      this.isNight = true;
      this.Trigger(-722330267, (object) null);
    }
    if (this.isNight && !this.IsNighttime())
      this.isNight = false;
    if (!flag || SaveGame.Instance.AutoSaveCycleInterval <= 0 || this.cycle % SaveGame.Instance.AutoSaveCycleInterval != 0)
      return;
    this.DoAutoSave(this.cycle);
  }

  public float GetTimeSinceStartOfReport()
  {
    if (this.IsNighttime())
      return 525f - this.GetTimeSinceStartOfCycle();
    return this.GetTimeSinceStartOfCycle() + 75f;
  }

  public float GetTimeSinceStartOfCycle()
  {
    return this.timeSinceStartOfCycle;
  }

  public float GetCurrentCycleAsPercentage()
  {
    return this.timeSinceStartOfCycle / 600f;
  }

  public float GetTime()
  {
    return this.timeSinceStartOfCycle + (float) this.cycle * 600f;
  }

  public int GetFrame()
  {
    return this.frame;
  }

  public int GetCycle()
  {
    return this.cycle;
  }

  public bool IsNighttime()
  {
    return (double) GameClock.Instance.GetCurrentCycleAsPercentage() >= 0.875;
  }

  public void SetTime(float new_time)
  {
    this.AddTime(Mathf.Max(new_time - this.GetTime(), 0.0f));
  }

  public float GetTimePlayedInSeconds()
  {
    return this.timePlayed;
  }

  private void DoAutoSave(int day)
  {
    if (GenericGameSettings.instance.disableAutosave)
      return;
    ++day;
    this.newDayMetric[GameClock.NewCycleKey] = (object) day;
    ThreadedHttps<KleiMetrics>.Instance.SendEvent(this.newDayMetric);
    string str1 = SaveLoader.GetActiveSaveFilePath() ?? SaveLoader.GetAutosaveFilePath();
    int startIndex = str1.LastIndexOf("\\");
    if (startIndex > 0)
    {
      int length = str1.IndexOf(" Cycle ", startIndex);
      if (length > 0)
        str1 = str1.Substring(0, length);
    }
    string str2 = System.IO.Path.Combine(SaveLoader.GetAutoSavePrefix(), System.IO.Path.GetFileName(SaveScreen.GetValidSaveFilename(str1.Replace(".sav", string.Empty) + " Cycle " + day.ToString())));
    string str3 = str2;
    int num = 1;
    while (File.Exists(str2))
    {
      str3.Replace(".sav", string.Empty);
      str2 = SaveScreen.GetValidSaveFilename(str3 + " (" + (object) num + ")");
      ++num;
    }
    Game.Instance.StartDelayedSave(str2, true, false);
  }
}
