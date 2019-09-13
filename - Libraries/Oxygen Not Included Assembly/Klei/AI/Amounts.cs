// Decompiled with JetBrains decompiler
// Type: Klei.AI.Amounts
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace Klei.AI
{
  public class Amounts : Modifications<Amount, AmountInstance>
  {
    public Amounts(GameObject go)
      : base(go, (ResourceSet<Amount>) null)
    {
    }

    public float GetValue(string amount_id)
    {
      return this.Get(amount_id).value;
    }

    public void SetValue(string amount_id, float value)
    {
      this.Get(amount_id).value = value;
    }

    public override AmountInstance Add(AmountInstance instance)
    {
      instance.Activate();
      return base.Add(instance);
    }

    public override void Remove(AmountInstance instance)
    {
      instance.Deactivate();
      base.Remove(instance);
    }

    public void Cleanup()
    {
      for (int index = 0; index < this.Count; ++index)
        this[index].Deactivate();
    }
  }
}
