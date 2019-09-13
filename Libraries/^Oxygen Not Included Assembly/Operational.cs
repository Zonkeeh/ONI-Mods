// Decompiled with JetBrains decompiler
// Type: Operational
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class Operational : KMonoBehaviour
{
  private static readonly EventSystem.IntraObjectHandler<Operational> OnNewBuildingDelegate = new EventSystem.IntraObjectHandler<Operational>((System.Action<Operational, object>) ((component, data) => component.OnNewBuilding(data)));
  [Serialize]
  private List<Operational.TimeEntry> activeTimes = new List<Operational.TimeEntry>();
  [Serialize]
  private List<Operational.TimeEntry> inactiveTimes = new List<Operational.TimeEntry>();
  public Dictionary<Operational.Flag, bool> Flags = new Dictionary<Operational.Flag, bool>();
  [Serialize]
  public float inactiveStartTime;
  [Serialize]
  public float activeStartTime;

  public bool IsOperational { get; private set; }

  public bool IsFunctional { get; private set; }

  public bool IsActive { get; private set; }

  [OnSerializing]
  private void OnSerializing()
  {
    float startingTime = !this.IsActive ? this.inactiveStartTime : this.activeStartTime;
    List<Operational.TimeEntry> timeEntries = !this.IsActive ? this.inactiveTimes : this.activeTimes;
    float time = GameClock.Instance.GetTime();
    this.AddTimeEntry(timeEntries, startingTime, time);
    this.activeStartTime = GameClock.Instance.GetTime();
    this.inactiveStartTime = GameClock.Instance.GetTime();
  }

  protected override void OnPrefabInit()
  {
    this.UpdateFunctional();
    this.UpdateOperational();
    this.Subscribe<Operational>(-1661515756, Operational.OnNewBuildingDelegate);
  }

  public void OnNewBuilding(object data)
  {
    BuildingComplete component = this.GetComponent<BuildingComplete>();
    if ((double) component.creationTime <= 0.0)
      return;
    this.inactiveStartTime = component.creationTime;
    this.activeStartTime = component.creationTime;
    this.activeTimes.Clear();
    this.inactiveTimes.Clear();
  }

  public bool IsOperationalType(Operational.Flag.Type type)
  {
    if (type == Operational.Flag.Type.Functional)
      return this.IsFunctional;
    return this.IsOperational;
  }

  public void SetFlag(Operational.Flag flag, bool value)
  {
    bool flag1 = false;
    if (this.Flags.TryGetValue(flag, out flag1))
    {
      if (flag1 != value)
      {
        this.Flags[flag] = value;
        this.Trigger(187661686, (object) flag);
      }
    }
    else
    {
      this.Flags[flag] = value;
      this.Trigger(187661686, (object) flag);
    }
    if (flag.FlagType == Operational.Flag.Type.Functional && value != this.IsFunctional)
      this.UpdateFunctional();
    if (value == this.IsOperational)
      return;
    this.UpdateOperational();
  }

  public bool GetFlag(Operational.Flag flag)
  {
    bool flag1 = false;
    this.Flags.TryGetValue(flag, out flag1);
    return flag1;
  }

  private void UpdateFunctional()
  {
    bool flag1 = true;
    foreach (KeyValuePair<Operational.Flag, bool> flag2 in this.Flags)
    {
      if (flag2.Key.FlagType == Operational.Flag.Type.Functional && !flag2.Value)
      {
        flag1 = false;
        break;
      }
    }
    this.IsFunctional = flag1;
    this.Trigger(-1852328367, (object) this.IsFunctional);
  }

  private void UpdateOperational()
  {
    Dictionary<Operational.Flag, bool>.Enumerator enumerator = this.Flags.GetEnumerator();
    bool flag = true;
    while (enumerator.MoveNext())
    {
      if (!enumerator.Current.Value)
      {
        flag = false;
        break;
      }
    }
    if (flag == this.IsOperational)
      return;
    this.IsOperational = flag;
    if (!this.IsOperational)
      this.SetActive(false, false);
    if (this.IsOperational)
      this.GetComponent<KPrefabID>().AddTag(GameTags.Operational, false);
    else
      this.GetComponent<KPrefabID>().RemoveTag(GameTags.Operational);
    this.Trigger(-592767678, (object) this.IsOperational);
    Game.Instance.Trigger(-809948329, (object) this.gameObject);
  }

  public void SetActive(bool value, bool force_ignore = false)
  {
    if (this.IsActive == value)
      return;
    float startingTime = !this.IsActive ? this.inactiveStartTime : this.activeStartTime;
    List<Operational.TimeEntry> timeEntries = !this.IsActive ? this.inactiveTimes : this.activeTimes;
    float time = GameClock.Instance.GetTime();
    this.AddTimeEntry(timeEntries, startingTime, time);
    this.IsActive = value;
    if (this.IsActive)
      this.activeStartTime = time;
    else
      this.inactiveStartTime = time;
    this.Trigger(824508782, (object) this);
    Game.Instance.Trigger(-809948329, (object) this.gameObject);
  }

  private void AddTimeEntry(
    List<Operational.TimeEntry> timeEntries,
    float startingTime,
    float endingTime)
  {
    if ((double) startingTime == (double) endingTime)
      return;
    timeEntries.Add(new Operational.TimeEntry(startingTime, endingTime));
  }

  private void ValidateTimeEntries(List<Operational.TimeEntry> timeEntries)
  {
    if (timeEntries.Count <= 2)
      return;
    for (int index1 = timeEntries.Count - 1; index1 > 0; --index1)
    {
      Operational.TimeEntry timeEntry1 = timeEntries[index1];
      for (int index2 = index1 - 1; index2 >= 0; --index2)
      {
        Operational.TimeEntry timeEntry2 = timeEntries[index2];
        if ((double) timeEntry1.startTime < (double) timeEntry2.endTime || (double) timeEntry1.startTime == (double) timeEntry2.startTime)
          Debug.Assert(false, (object) "ENTRY TIMES OVERLAP!");
      }
    }
  }

  public float GetUptimeForTimeSpan(float duration = 600f)
  {
    float num1 = this.SumTimesForTimeSpawn(this.activeTimes, duration);
    float num2 = this.SumTimesForTimeSpawn(this.inactiveTimes, duration);
    float b = GameClock.Instance.GetTime() - duration;
    if (this.IsActive)
      num1 += GameClock.Instance.GetTime() - Mathf.Max(this.activeStartTime, b);
    else
      num2 += GameClock.Instance.GetTime() - Mathf.Max(this.inactiveStartTime, b);
    float num3 = num1 + num2;
    Debug.Assert((double) num3 <= (double) duration, (object) "totalTime is greater than allowed duration!");
    if ((double) num1 == 0.0 || (double) num3 == 0.0)
      return 0.0f;
    if ((double) num2 == 0.0)
      return 1f;
    return num1 / num3;
  }

  private float SumTimesForTimeSpawn(List<Operational.TimeEntry> times, float duration)
  {
    float num1 = GameClock.Instance.GetTime() - duration;
    float num2 = 0.0f;
    foreach (Operational.TimeEntry time in times)
    {
      if ((double) time.startTime >= (double) num1 || (double) time.endTime >= (double) num1)
      {
        if ((double) time.startTime < (double) num1 && (double) time.endTime >= (double) num1)
          num2 += time.endTime - num1;
        else
          num2 += time.endTime - time.startTime;
      }
    }
    return num2;
  }

  public class Flag
  {
    public string Name;
    public Operational.Flag.Type FlagType;

    public Flag(string name, Operational.Flag.Type type)
    {
      this.Name = name;
      this.FlagType = type;
    }

    public enum Type
    {
      Requirement,
      Functional,
    }
  }

  public struct TimeEntry
  {
    public float startTime;
    public float endTime;

    public TimeEntry(float start, float end)
    {
      this.startTime = start;
      this.endTime = end;
    }
  }
}
