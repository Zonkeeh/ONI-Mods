// Decompiled with JetBrains decompiler
// Type: Harmony.PatchFunctions
// Assembly: 0Harmony, Version=1.2.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 1B235470-4299-4E96-B8B6-361DBE3791D9
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\0Harmony.dll

using Harmony.ILCopying;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Harmony
{
  public static class PatchFunctions
  {
    public static void AddPrefix(PatchInfo patchInfo, string owner, HarmonyMethod info)
    {
      if (info == null || info.method == null)
        return;
      int priority = info.prioritiy == -1 ? 400 : info.prioritiy;
      string[] before = info.before ?? new string[0];
      string[] after = info.after ?? new string[0];
      patchInfo.AddPrefix(info.method, owner, priority, before, after);
    }

    public static void RemovePrefix(PatchInfo patchInfo, string owner)
    {
      patchInfo.RemovePrefix(owner);
    }

    public static void AddPostfix(PatchInfo patchInfo, string owner, HarmonyMethod info)
    {
      if (info == null || info.method == null)
        return;
      int priority = info.prioritiy == -1 ? 400 : info.prioritiy;
      string[] before = info.before ?? new string[0];
      string[] after = info.after ?? new string[0];
      patchInfo.AddPostfix(info.method, owner, priority, before, after);
    }

    public static void RemovePostfix(PatchInfo patchInfo, string owner)
    {
      patchInfo.RemovePostfix(owner);
    }

    public static void AddTranspiler(PatchInfo patchInfo, string owner, HarmonyMethod info)
    {
      if (info == null || info.method == null)
        return;
      int priority = info.prioritiy == -1 ? 400 : info.prioritiy;
      string[] before = info.before ?? new string[0];
      string[] after = info.after ?? new string[0];
      patchInfo.AddTranspiler(info.method, owner, priority, before, after);
    }

    public static void RemoveTranspiler(PatchInfo patchInfo, string owner)
    {
      patchInfo.RemoveTranspiler(owner);
    }

    public static void RemovePatch(PatchInfo patchInfo, MethodInfo patch)
    {
      patchInfo.RemovePatch(patch);
    }

    public static List<ILInstruction> GetInstructions(
      ILGenerator generator,
      MethodBase method)
    {
      return MethodBodyReader.GetInstructions(generator, method);
    }

    public static List<MethodInfo> GetSortedPatchMethods(
      MethodBase original,
      Patch[] patches)
    {
      return ((IEnumerable<Patch>) patches).Where<Patch>((Func<Patch, bool>) (p => p.patch != null)).OrderBy<Patch, Patch>((Func<Patch, Patch>) (p => p)).Select<Patch, MethodInfo>((Func<Patch, MethodInfo>) (p => p.GetMethod(original))).ToList<MethodInfo>();
    }

    public static DynamicMethod UpdateWrapper(
      MethodBase original,
      PatchInfo patchInfo,
      string instanceID)
    {
      List<MethodInfo> sortedPatchMethods1 = PatchFunctions.GetSortedPatchMethods(original, patchInfo.prefixes);
      List<MethodInfo> sortedPatchMethods2 = PatchFunctions.GetSortedPatchMethods(original, patchInfo.postfixes);
      List<MethodInfo> sortedPatchMethods3 = PatchFunctions.GetSortedPatchMethods(original, patchInfo.transpilers);
      DynamicMethod patchedMethod = MethodPatcher.CreatePatchedMethod(original, instanceID, sortedPatchMethods1, sortedPatchMethods2, sortedPatchMethods3);
      if (patchedMethod == null)
        throw new MissingMethodException("Cannot create dynamic replacement for " + original.FullDescription());
      string str = Memory.DetourMethod(original, (MethodBase) patchedMethod);
      if (str != null)
        throw new FormatException("Method " + original.FullDescription() + " cannot be patched. Reason: " + str);
      PatchTools.RememberObject((object) original, (object) patchedMethod);
      return patchedMethod;
    }
  }
}
