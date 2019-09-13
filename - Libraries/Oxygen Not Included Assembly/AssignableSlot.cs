// Decompiled with JetBrains decompiler
// Type: AssignableSlot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Diagnostics;
using UnityEngine;

[DebuggerDisplay("{Id}")]
[Serializable]
public class AssignableSlot : Resource
{
  public bool showInUI = true;

  public AssignableSlot(string id, string name, bool showInUI = true)
    : base(id, name)
  {
    this.showInUI = showInUI;
  }

  public AssignableSlotInstance Lookup(GameObject go)
  {
    Assignables component = go.GetComponent<Assignables>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      return component.GetSlot(this);
    return (AssignableSlotInstance) null;
  }
}
