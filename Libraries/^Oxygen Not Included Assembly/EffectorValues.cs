// Decompiled with JetBrains decompiler
// Type: EffectorValues
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

[Serializable]
public struct EffectorValues
{
  public int amount;
  public int radius;

  public EffectorValues(int amt, int rad)
  {
    this.amount = amt;
    this.radius = rad;
  }

  public override bool Equals(object obj)
  {
    if (obj is EffectorValues)
      return this.Equals((EffectorValues) obj);
    return false;
  }

  public bool Equals(EffectorValues p)
  {
    if (object.ReferenceEquals((object) p, (object) null))
      return false;
    if (object.ReferenceEquals((object) this, (object) p))
      return true;
    if (this.GetType() != p.GetType() || this.amount != p.amount)
      return false;
    return this.radius == p.radius;
  }

  public override int GetHashCode()
  {
    return this.amount ^ this.radius;
  }

  public static bool operator ==(EffectorValues lhs, EffectorValues rhs)
  {
    if (!object.ReferenceEquals((object) lhs, (object) null))
      return lhs.Equals(rhs);
    return object.ReferenceEquals((object) rhs, (object) null);
  }

  public static bool operator !=(EffectorValues lhs, EffectorValues rhs)
  {
    return !(lhs == rhs);
  }
}
