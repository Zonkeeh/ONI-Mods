// Decompiled with JetBrains decompiler
// Type: Harmony.Patches
// Assembly: 0Harmony, Version=1.2.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 1B235470-4299-4E96-B8B6-361DBE3791D9
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\0Harmony.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Harmony
{
  public class Patches
  {
    public readonly ReadOnlyCollection<Patch> Prefixes;
    public readonly ReadOnlyCollection<Patch> Postfixes;
    public readonly ReadOnlyCollection<Patch> Transpilers;

    public ReadOnlyCollection<string> Owners
    {
      get
      {
        HashSet<string> source = new HashSet<string>();
        source.UnionWith(this.Prefixes.Select<Patch, string>((Func<Patch, string>) (p => p.owner)));
        source.UnionWith(this.Postfixes.Select<Patch, string>((Func<Patch, string>) (p => p.owner)));
        source.UnionWith(this.Transpilers.Select<Patch, string>((Func<Patch, string>) (p => p.owner)));
        return source.ToList<string>().AsReadOnly();
      }
    }

    public Patches(Patch[] prefixes, Patch[] postfixes, Patch[] transpilers)
    {
      if (prefixes == null)
        prefixes = new Patch[0];
      if (postfixes == null)
        postfixes = new Patch[0];
      if (transpilers == null)
        transpilers = new Patch[0];
      this.Prefixes = ((IEnumerable<Patch>) prefixes).ToList<Patch>().AsReadOnly();
      this.Postfixes = ((IEnumerable<Patch>) postfixes).ToList<Patch>().AsReadOnly();
      this.Transpilers = ((IEnumerable<Patch>) transpilers).ToList<Patch>().AsReadOnly();
    }
  }
}
