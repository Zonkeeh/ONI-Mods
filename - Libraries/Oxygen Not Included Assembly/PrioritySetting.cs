// Decompiled with JetBrains decompiler
// Type: PrioritySetting
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

public struct PrioritySetting : IComparable<PrioritySetting>
{
  public PriorityScreen.PriorityClass priority_class;
  public int priority_value;

  public PrioritySetting(PriorityScreen.PriorityClass priority_class, int priority_value)
  {
    this.priority_class = priority_class;
    this.priority_value = priority_value;
  }

  public override int GetHashCode()
  {
    return ((int) this.priority_class << 28).GetHashCode() ^ this.priority_value.GetHashCode();
  }

  public static bool operator ==(PrioritySetting lhs, PrioritySetting rhs)
  {
    return lhs.Equals((object) rhs);
  }

  public static bool operator !=(PrioritySetting lhs, PrioritySetting rhs)
  {
    return !lhs.Equals((object) rhs);
  }

  public static bool operator <=(PrioritySetting lhs, PrioritySetting rhs)
  {
    return lhs.CompareTo(rhs) <= 0;
  }

  public static bool operator >=(PrioritySetting lhs, PrioritySetting rhs)
  {
    return lhs.CompareTo(rhs) >= 0;
  }

  public static bool operator <(PrioritySetting lhs, PrioritySetting rhs)
  {
    return lhs.CompareTo(rhs) < 0;
  }

  public static bool operator >(PrioritySetting lhs, PrioritySetting rhs)
  {
    return lhs.CompareTo(rhs) > 0;
  }

  public override bool Equals(object obj)
  {
    if (!(obj is PrioritySetting) || ((PrioritySetting) obj).priority_class != this.priority_class)
      return false;
    return ((PrioritySetting) obj).priority_value == this.priority_value;
  }

  public int CompareTo(PrioritySetting other)
  {
    if (this.priority_class > other.priority_class)
      return 1;
    if (this.priority_class < other.priority_class)
      return -1;
    if (this.priority_value > other.priority_value)
      return 1;
    return this.priority_value < other.priority_value ? -1 : 0;
  }
}
