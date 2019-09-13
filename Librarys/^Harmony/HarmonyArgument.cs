// Decompiled with JetBrains decompiler
// Type: Harmony.HarmonyArgument
// Assembly: 0Harmony, Version=1.2.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 1B235470-4299-4E96-B8B6-361DBE3791D9
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\0Harmony.dll

using System;

namespace Harmony
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Parameter, AllowMultiple = true)]
  public class HarmonyArgument : Attribute
  {
    public string OriginalName { get; private set; }

    public int Index { get; private set; }

    public string NewName { get; private set; }

    public HarmonyArgument(string originalName)
      : this(originalName, (string) null)
    {
    }

    public HarmonyArgument(int index)
      : this(index, (string) null)
    {
    }

    public HarmonyArgument(string originalName, string newName)
    {
      this.OriginalName = originalName;
      this.Index = -1;
      this.NewName = newName;
    }

    public HarmonyArgument(int index, string name)
    {
      this.OriginalName = (string) null;
      this.Index = index;
      this.NewName = name;
    }
  }
}
