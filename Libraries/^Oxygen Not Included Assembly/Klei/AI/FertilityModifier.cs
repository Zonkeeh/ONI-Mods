// Decompiled with JetBrains decompiler
// Type: Klei.AI.FertilityModifier
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

namespace Klei.AI
{
  public class FertilityModifier : Resource
  {
    public string Description;
    public Tag TargetTag;
    public Func<string, string> TooltipCB;
    public FertilityModifier.FertilityModFn ApplyFunction;

    public FertilityModifier(
      string id,
      Tag targetTag,
      string name,
      string description,
      Func<string, string> tooltipCB,
      FertilityModifier.FertilityModFn applyFunction)
      : base(id, name)
    {
      this.Description = description;
      this.TargetTag = targetTag;
      this.TooltipCB = tooltipCB;
      this.ApplyFunction = applyFunction;
    }

    public string GetTooltip()
    {
      if (this.TooltipCB != null)
        return this.TooltipCB(this.Description);
      return this.Description;
    }

    public delegate void FertilityModFn(FertilityMonitor.Instance inst, Tag eggTag);
  }
}
