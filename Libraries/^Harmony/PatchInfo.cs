// Decompiled with JetBrains decompiler
// Type: Harmony.PatchInfo
// Assembly: 0Harmony, Version=1.2.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 1B235470-4299-4E96-B8B6-361DBE3791D9
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\0Harmony.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Harmony
{
  [Serializable]
  public class PatchInfo
  {
    public Patch[] prefixes;
    public Patch[] postfixes;
    public Patch[] transpilers;

    public PatchInfo()
    {
      this.prefixes = new Patch[0];
      this.postfixes = new Patch[0];
      this.transpilers = new Patch[0];
    }

    public void AddPrefix(
      MethodInfo patch,
      string owner,
      int priority,
      string[] before,
      string[] after)
    {
      List<Patch> list = ((IEnumerable<Patch>) this.prefixes).ToList<Patch>();
      list.Add(new Patch(patch, ((IEnumerable<Patch>) this.prefixes).Count<Patch>() + 1, owner, priority, before, after));
      this.prefixes = list.ToArray();
    }

    public void RemovePrefix(string owner)
    {
      if (owner == "*")
        this.prefixes = new Patch[0];
      else
        this.prefixes = ((IEnumerable<Patch>) this.prefixes).Where<Patch>((Func<Patch, bool>) (patch => patch.owner != owner)).ToArray<Patch>();
    }

    public void AddPostfix(
      MethodInfo patch,
      string owner,
      int priority,
      string[] before,
      string[] after)
    {
      List<Patch> list = ((IEnumerable<Patch>) this.postfixes).ToList<Patch>();
      list.Add(new Patch(patch, ((IEnumerable<Patch>) this.postfixes).Count<Patch>() + 1, owner, priority, before, after));
      this.postfixes = list.ToArray();
    }

    public void RemovePostfix(string owner)
    {
      if (owner == "*")
        this.postfixes = new Patch[0];
      else
        this.postfixes = ((IEnumerable<Patch>) this.postfixes).Where<Patch>((Func<Patch, bool>) (patch => patch.owner != owner)).ToArray<Patch>();
    }

    public void AddTranspiler(
      MethodInfo patch,
      string owner,
      int priority,
      string[] before,
      string[] after)
    {
      List<Patch> list = ((IEnumerable<Patch>) this.transpilers).ToList<Patch>();
      list.Add(new Patch(patch, ((IEnumerable<Patch>) this.transpilers).Count<Patch>() + 1, owner, priority, before, after));
      this.transpilers = list.ToArray();
    }

    public void RemoveTranspiler(string owner)
    {
      if (owner == "*")
        this.transpilers = new Patch[0];
      else
        this.transpilers = ((IEnumerable<Patch>) this.transpilers).Where<Patch>((Func<Patch, bool>) (patch => patch.owner != owner)).ToArray<Patch>();
    }

    public void RemovePatch(MethodInfo patch)
    {
      this.prefixes = ((IEnumerable<Patch>) this.prefixes).Where<Patch>((Func<Patch, bool>) (p => p.patch != patch)).ToArray<Patch>();
      this.postfixes = ((IEnumerable<Patch>) this.postfixes).Where<Patch>((Func<Patch, bool>) (p => p.patch != patch)).ToArray<Patch>();
      this.transpilers = ((IEnumerable<Patch>) this.transpilers).Where<Patch>((Func<Patch, bool>) (p => p.patch != patch)).ToArray<Patch>();
    }
  }
}
