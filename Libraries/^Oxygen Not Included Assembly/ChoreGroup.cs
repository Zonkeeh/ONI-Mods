// Decompiled with JetBrains decompiler
// Type: ChoreGroup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System.Collections.Generic;
using System.Diagnostics;

[DebuggerDisplay("{IdHash}")]
public class ChoreGroup : Resource
{
  public List<ChoreType> choreTypes = new List<ChoreType>();
  public Attribute attribute;
  public string description;
  public string sprite;
  private int defaultPersonalPriority;

  public ChoreGroup(
    string id,
    string name,
    Attribute attribute,
    string sprite,
    int default_personal_priority)
    : base(id, name)
  {
    this.attribute = attribute;
    this.description = Strings.Get("STRINGS.DUPLICANTS.CHOREGROUPS." + id.ToUpper() + ".DESC").String;
    this.sprite = sprite;
    this.defaultPersonalPriority = default_personal_priority;
  }

  public int DefaultPersonalPriority
  {
    get
    {
      return this.defaultPersonalPriority;
    }
  }
}
