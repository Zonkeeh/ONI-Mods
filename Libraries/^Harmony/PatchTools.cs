// Decompiled with JetBrains decompiler
// Type: Harmony.PatchTools
// Assembly: 0Harmony, Version=1.2.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 1B235470-4299-4E96-B8B6-361DBE3791D9
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\0Harmony.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Harmony
{
  public static class PatchTools
  {
    private static Dictionary<object, object> objectReferences = new Dictionary<object, object>();

    public static void RememberObject(object key, object value)
    {
      PatchTools.objectReferences[key] = value;
    }

    public static MethodInfo GetPatchMethod<T>(
      Type patchType,
      string name,
      Type[] parameters = null)
    {
      return ((IEnumerable<MethodInfo>) patchType.GetMethods(AccessTools.all)).FirstOrDefault<MethodInfo>((Func<MethodInfo, bool>) (m => ((IEnumerable<object>) m.GetCustomAttributes(typeof (T), true)).Any<object>())) ?? AccessTools.Method(patchType, name, parameters, (Type[]) null);
    }

    public static void GetPatches(
      Type patchType,
      out MethodInfo prefix,
      out MethodInfo postfix,
      out MethodInfo transpiler)
    {
      prefix = PatchTools.GetPatchMethod<HarmonyPrefix>(patchType, "Prefix", (Type[]) null);
      postfix = PatchTools.GetPatchMethod<HarmonyPostfix>(patchType, "Postfix", (Type[]) null);
      transpiler = PatchTools.GetPatchMethod<HarmonyTranspiler>(patchType, "Transpiler", (Type[]) null);
    }
  }
}
