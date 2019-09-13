// Decompiled with JetBrains decompiler
// Type: Ownables
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class Ownables : Assignables
{
  protected override void OnSpawn()
  {
    base.OnSpawn();
  }

  public void UnassignAll()
  {
    foreach (AssignableSlotInstance slot in this.slots)
    {
      if ((Object) slot.assignable != (Object) null)
        slot.assignable.Unassign();
    }
  }
}
