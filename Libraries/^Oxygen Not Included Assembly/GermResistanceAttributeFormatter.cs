// Decompiled with JetBrains decompiler
// Type: GermResistanceAttributeFormatter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using UnityEngine;

public class GermResistanceAttributeFormatter : StandardAttributeFormatter
{
  public GermResistanceAttributeFormatter()
    : base(GameUtil.UnitClass.SimpleFloat, GameUtil.TimeSlice.None)
  {
  }

  public override string GetFormattedModifier(
    AttributeModifier modifier,
    GameObject parent_instance)
  {
    return GameUtil.GetGermResistanceModifierString(modifier.Value, false);
  }
}
