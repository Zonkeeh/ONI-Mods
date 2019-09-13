// Decompiled with JetBrains decompiler
// Type: IAttributeFormatter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using UnityEngine;

public interface IAttributeFormatter
{
  GameUtil.TimeSlice DeltaTimeSlice { get; set; }

  string GetFormattedAttribute(AttributeInstance instance);

  string GetFormattedModifier(AttributeModifier modifier, GameObject parent_instance);

  string GetFormattedValue(float value, GameUtil.TimeSlice timeSlice, GameObject parent_instance);

  string GetTooltip(Attribute master, AttributeInstance instance);
}
