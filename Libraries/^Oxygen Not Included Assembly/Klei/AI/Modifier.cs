// Decompiled with JetBrains decompiler
// Type: Klei.AI.Modifier
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

namespace Klei.AI
{
  public class Modifier : Resource
  {
    public List<AttributeModifier> SelfModifiers = new List<AttributeModifier>();
    public string description;

    public Modifier(string id, string name, string description)
      : base(id, name)
    {
      this.description = description;
    }

    public void Add(AttributeModifier modifier)
    {
      if (!(modifier.AttributeId != string.Empty))
        return;
      this.SelfModifiers.Add(modifier);
    }

    public virtual void AddTo(Attributes attributes)
    {
      foreach (AttributeModifier selfModifier in this.SelfModifiers)
        attributes.Add(selfModifier);
    }

    public virtual void RemoveFrom(Attributes attributes)
    {
      foreach (AttributeModifier selfModifier in this.SelfModifiers)
        attributes.Remove(selfModifier);
    }
  }
}
