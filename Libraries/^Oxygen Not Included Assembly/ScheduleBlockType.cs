// Decompiled with JetBrains decompiler
// Type: ScheduleBlockType
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Diagnostics;
using UnityEngine;

[DebuggerDisplay("{Id}")]
public class ScheduleBlockType : Resource
{
  public ScheduleBlockType(
    string id,
    ResourceSet parent,
    string name,
    string description,
    Color color)
    : base(id, parent, name)
  {
    this.color = color;
    this.description = description;
  }

  public Color color { get; private set; }

  public string description { get; private set; }
}
