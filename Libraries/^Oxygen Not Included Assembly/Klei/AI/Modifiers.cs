// Decompiled with JetBrains decompiler
// Type: Klei.AI.Modifiers
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System.Collections.Generic;
using System.IO;

namespace Klei.AI
{
  [SerializationConfig(MemberSerialization.OptIn)]
  public class Modifiers : KMonoBehaviour, ISaveLoadableDetails
  {
    public List<string> initialAmounts = new List<string>();
    public Amounts amounts;
    public Attributes attributes;
    public Sicknesses sicknesses;
    public string[] initialTraits;

    protected override void OnPrefabInit()
    {
      base.OnPrefabInit();
      this.amounts = new Amounts(this.gameObject);
      this.sicknesses = new Sicknesses(this.gameObject);
      this.attributes = new Attributes(this.gameObject);
      foreach (string initialAmount in this.initialAmounts)
        this.amounts.Add(new AmountInstance(Db.Get().Amounts.Get(initialAmount), this.gameObject));
      Traits component = this.GetComponent<Traits>();
      if (this.initialTraits == null)
        return;
      foreach (string initialTrait in this.initialTraits)
      {
        Trait trait = Db.Get().traits.Get(initialTrait);
        component.Add(trait);
      }
    }

    public void Serialize(BinaryWriter writer)
    {
      this.OnSerialize(writer);
    }

    public void Deserialize(IReader reader)
    {
      this.OnDeserialize(reader);
    }

    public virtual void OnSerialize(BinaryWriter writer)
    {
      this.amounts.Serialize(writer);
      this.sicknesses.Serialize(writer);
    }

    public virtual void OnDeserialize(IReader reader)
    {
      this.amounts.Deserialize(reader);
      this.sicknesses.Deserialize(reader);
    }

    protected override void OnCleanUp()
    {
      base.OnCleanUp();
      if (this.amounts == null)
        return;
      this.amounts.Cleanup();
    }
  }
}
