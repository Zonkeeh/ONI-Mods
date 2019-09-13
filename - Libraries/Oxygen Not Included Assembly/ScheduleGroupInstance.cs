// Decompiled with JetBrains decompiler
// Type: ScheduleGroupInstance
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;

[SerializationConfig(MemberSerialization.OptIn)]
public class ScheduleGroupInstance
{
  [Serialize]
  private string scheduleGroupID;
  [Serialize]
  public int segments;

  public ScheduleGroupInstance(ScheduleGroup scheduleGroup)
  {
    this.scheduleGroup = scheduleGroup;
    this.segments = scheduleGroup.defaultSegments;
  }

  public ScheduleGroup scheduleGroup
  {
    get
    {
      return Db.Get().ScheduleGroups.Get(this.scheduleGroupID);
    }
    set
    {
      this.scheduleGroupID = value.Id;
    }
  }
}
