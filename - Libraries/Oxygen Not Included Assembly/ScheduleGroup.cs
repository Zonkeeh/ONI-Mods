// Decompiled with JetBrains decompiler
// Type: ScheduleGroup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using System.Diagnostics;

[DebuggerDisplay("{Id}")]
public class ScheduleGroup : Resource
{
  public ScheduleGroup(
    string id,
    ResourceSet parent,
    int defaultSegments,
    string name,
    string description,
    string notificationTooltip,
    List<ScheduleBlockType> allowedTypes,
    bool alarm = false)
    : base(id, parent, name)
  {
    this.defaultSegments = defaultSegments;
    this.description = description;
    this.notificationTooltip = notificationTooltip;
    this.allowedTypes = allowedTypes;
    this.alarm = alarm;
  }

  public int defaultSegments { get; private set; }

  public string description { get; private set; }

  public string notificationTooltip { get; private set; }

  public List<ScheduleBlockType> allowedTypes { get; private set; }

  public bool alarm { get; private set; }

  public bool Allowed(ScheduleBlockType type)
  {
    return this.allowedTypes.Contains(type);
  }

  public string GetTooltip()
  {
    return string.Format((string) UI.SCHEDULEGROUPS.TOOLTIP_FORMAT, (object) this.Name, (object) this.description);
  }
}
