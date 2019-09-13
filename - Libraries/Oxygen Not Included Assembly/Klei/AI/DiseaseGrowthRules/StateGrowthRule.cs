// Decompiled with JetBrains decompiler
// Type: Klei.AI.DiseaseGrowthRules.StateGrowthRule
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

namespace Klei.AI.DiseaseGrowthRules
{
  public class StateGrowthRule : GrowthRule
  {
    public Element.State state;

    public StateGrowthRule(Element.State state)
    {
      this.state = state;
    }

    public override bool Test(Element e)
    {
      return e.IsState(this.state);
    }

    public override string Name()
    {
      return Element.GetStateString(this.state);
    }
  }
}
