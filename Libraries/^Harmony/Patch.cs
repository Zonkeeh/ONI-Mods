// Decompiled with JetBrains decompiler
// Type: Harmony.Patch
// Assembly: 0Harmony, Version=1.2.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 1B235470-4299-4E96-B8B6-361DBE3791D9
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\0Harmony.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Harmony
{
  [Serializable]
  public class Patch : IComparable
  {
    public readonly int index;
    public readonly string owner;
    public readonly int priority;
    public readonly string[] before;
    public readonly string[] after;
    public readonly MethodInfo patch;

    public Patch(
      MethodInfo patch,
      int index,
      string owner,
      int priority,
      string[] before,
      string[] after)
    {
      if (patch is DynamicMethod)
        throw new Exception("Cannot directly reference dynamic method \"" + patch.FullDescription() + "\" in Harmony. Use a factory method instead that will return the dynamic method.");
      this.index = index;
      this.owner = owner;
      this.priority = priority;
      this.before = before;
      this.after = after;
      this.patch = patch;
    }

    public MethodInfo GetMethod(MethodBase original)
    {
      if (this.patch.ReturnType != typeof (DynamicMethod) || !this.patch.IsStatic)
        return this.patch;
      ParameterInfo[] parameters = this.patch.GetParameters();
      if (((IEnumerable<ParameterInfo>) parameters).Count<ParameterInfo>() != 1 || parameters[0].ParameterType != typeof (MethodBase))
        return this.patch;
      return (MethodInfo) (this.patch.Invoke((object) null, new object[1]
      {
        (object) original
      }) as DynamicMethod);
    }

    public override bool Equals(object obj)
    {
      return obj != null && obj is Patch && this.patch == ((Patch) obj).patch;
    }

    public int CompareTo(object obj)
    {
      return PatchInfoSerialization.PriorityComparer(obj, this.index, this.priority, this.before, this.after);
    }

    public override int GetHashCode()
    {
      return this.patch.GetHashCode();
    }
  }
}
