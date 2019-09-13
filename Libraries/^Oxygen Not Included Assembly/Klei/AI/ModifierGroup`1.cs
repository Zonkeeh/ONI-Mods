// Decompiled with JetBrains decompiler
// Type: Klei.AI.ModifierGroup`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

namespace Klei.AI
{
  public class ModifierGroup<T> : Resource
  {
    public List<T> modifiers = new List<T>();

    public ModifierGroup(string id, string name)
      : base(id, name)
    {
    }

    public IEnumerator<T> GetEnumerator()
    {
      return (IEnumerator<T>) this.modifiers.GetEnumerator();
    }

    public T this[int idx]
    {
      get
      {
        return this.modifiers[idx];
      }
    }

    public int Count
    {
      get
      {
        return this.modifiers.Count;
      }
    }

    public void Add(T modifier)
    {
      this.modifiers.Add(modifier);
    }
  }
}
