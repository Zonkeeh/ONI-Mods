// Decompiled with JetBrains decompiler
// Type: Schedule
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using System.Collections.Generic;

[SerializationConfig(MemberSerialization.OptIn)]
public class Schedule : ISaveLoadable, IListableOption
{
  [Serialize]
  public bool alarmActivated = true;
  [Serialize]
  private List<ScheduleBlock> blocks;
  [Serialize]
  private List<Ref<Schedulable>> assigned;
  [Serialize]
  public string name;
  [Serialize]
  private int[] tones;
  public System.Action<Schedule> onChanged;

  public Schedule(string name, List<ScheduleGroup> defaultGroups, bool alarmActivated)
  {
    this.name = name;
    this.alarmActivated = alarmActivated;
    this.blocks = new List<ScheduleBlock>(24);
    this.assigned = new List<Ref<Schedulable>>();
    this.tones = this.GenerateTones();
    this.SetBlocksToGroupDefaults(defaultGroups);
  }

  public static int GetBlockIdx()
  {
    return Math.Min((int) ((double) GameClock.Instance.GetCurrentCycleAsPercentage() * 24.0), 23);
  }

  public static int GetLastBlockIdx()
  {
    return (Schedule.GetBlockIdx() + 24 - 1) % 24;
  }

  public void ClearNullReferences()
  {
    this.assigned.RemoveAll((Predicate<Ref<Schedulable>>) (x => (UnityEngine.Object) x.Get() == (UnityEngine.Object) null));
  }

  public void SetBlocksToGroupDefaults(List<ScheduleGroup> defaultGroups)
  {
    this.blocks.Clear();
    int num = 0;
    for (int index1 = 0; index1 < defaultGroups.Count; ++index1)
    {
      ScheduleGroup defaultGroup = defaultGroups[index1];
      for (int index2 = 0; index2 < defaultGroup.defaultSegments; ++index2)
      {
        this.blocks.Add(new ScheduleBlock(defaultGroup.Name, defaultGroup.allowedTypes, defaultGroup.Id));
        ++num;
      }
    }
    Debug.Assert(num == 24);
    this.Changed();
  }

  public void Tick()
  {
    ScheduleBlock block1 = this.GetBlock(Schedule.GetBlockIdx());
    ScheduleBlock block2 = this.GetBlock(Schedule.GetLastBlockIdx());
    if (!Schedule.AreScheduleTypesIdentical(block1.allowed_types, block2.allowed_types))
    {
      ScheduleGroup forScheduleTypes1 = Db.Get().ScheduleGroups.FindGroupForScheduleTypes(block1.allowed_types);
      ScheduleGroup forScheduleTypes2 = Db.Get().ScheduleGroups.FindGroupForScheduleTypes(block2.allowed_types);
      if (this.alarmActivated && forScheduleTypes2.alarm != forScheduleTypes1.alarm)
        ScheduleManager.Instance.PlayScheduleAlarm(this, block1, forScheduleTypes1.alarm);
      foreach (Ref<Schedulable> @ref in this.GetAssigned())
        @ref.Get().OnScheduleBlocksChanged(this);
    }
    foreach (Ref<Schedulable> @ref in this.GetAssigned())
      @ref.Get().OnScheduleBlocksTick(this);
  }

  string IListableOption.GetProperName()
  {
    return this.name;
  }

  public int[] GenerateTones()
  {
    int minToneIndex = TuningData<ScheduleManager.Tuning>.Get().minToneIndex;
    int maxToneIndex = TuningData<ScheduleManager.Tuning>.Get().maxToneIndex;
    int firstLastToneSpacing = TuningData<ScheduleManager.Tuning>.Get().firstLastToneSpacing;
    int[] numArray = new int[4]
    {
      UnityEngine.Random.Range(minToneIndex, maxToneIndex - firstLastToneSpacing + 1),
      UnityEngine.Random.Range(minToneIndex, maxToneIndex + 1),
      UnityEngine.Random.Range(minToneIndex, maxToneIndex + 1),
      0
    };
    numArray[3] = UnityEngine.Random.Range(numArray[0] + firstLastToneSpacing, maxToneIndex + 1);
    return numArray;
  }

  public List<Ref<Schedulable>> GetAssigned()
  {
    if (this.assigned == null)
      this.assigned = new List<Ref<Schedulable>>();
    return this.assigned;
  }

  public int[] GetTones()
  {
    if (this.tones == null)
      this.tones = this.GenerateTones();
    return this.tones;
  }

  public void SetGroup(int idx, ScheduleGroup group)
  {
    if (0 > idx || idx >= this.blocks.Count)
      return;
    this.blocks[idx] = new ScheduleBlock(group.Name, group.allowedTypes, group.Id);
    this.Changed();
  }

  private void Changed()
  {
    foreach (Ref<Schedulable> @ref in this.GetAssigned())
      @ref.Get().OnScheduleChanged(this);
    if (this.onChanged == null)
      return;
    this.onChanged(this);
  }

  public List<ScheduleBlock> GetBlocks()
  {
    return this.blocks;
  }

  public ScheduleBlock GetBlock(int idx)
  {
    return this.blocks[idx];
  }

  public void Assign(Schedulable schedulable)
  {
    if (!this.IsAssigned(schedulable))
      this.GetAssigned().Add(new Ref<Schedulable>(schedulable));
    this.Changed();
  }

  public void Unassign(Schedulable schedulable)
  {
    for (int index = 0; index < this.GetAssigned().Count; ++index)
    {
      if ((UnityEngine.Object) this.GetAssigned()[index].Get() == (UnityEngine.Object) schedulable)
      {
        this.GetAssigned().RemoveAt(index);
        break;
      }
    }
    this.Changed();
  }

  public bool IsAssigned(Schedulable schedulable)
  {
    foreach (Ref<Schedulable> @ref in this.GetAssigned())
    {
      if ((UnityEngine.Object) @ref.Get() == (UnityEngine.Object) schedulable)
        return true;
    }
    return false;
  }

  public static bool AreScheduleTypesIdentical(List<ScheduleBlockType> a, List<ScheduleBlockType> b)
  {
    if (a.Count != b.Count)
      return false;
    foreach (ScheduleBlockType scheduleBlockType1 in a)
    {
      bool flag = false;
      foreach (ScheduleBlockType scheduleBlockType2 in b)
      {
        if (scheduleBlockType1.IdHash == scheduleBlockType2.IdHash)
        {
          flag = true;
          break;
        }
      }
      if (!flag)
        return false;
    }
    return true;
  }
}
