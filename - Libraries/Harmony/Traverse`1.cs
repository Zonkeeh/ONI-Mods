// Decompiled with JetBrains decompiler
// Type: Harmony.Traverse`1
// Assembly: 0Harmony, Version=1.2.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 1B235470-4299-4E96-B8B6-361DBE3791D9
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\0Harmony.dll

namespace Harmony
{
  public class Traverse<T>
  {
    private Traverse traverse;

    private Traverse()
    {
    }

    public Traverse(Traverse traverse)
    {
      this.traverse = traverse;
    }

    public T Value
    {
      get
      {
        return this.traverse.GetValue<T>();
      }
      set
      {
        this.traverse.SetValue((object) value);
      }
    }
  }
}
