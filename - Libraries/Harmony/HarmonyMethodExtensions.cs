// Decompiled with JetBrains decompiler
// Type: Harmony.HarmonyMethodExtensions
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
  public static class HarmonyMethodExtensions
  {
    public static void CopyTo(this HarmonyMethod from, HarmonyMethod to)
    {
      if (to == null)
        return;
      Traverse fromTrv = Traverse.Create((object) from);
      Traverse toTrv = Traverse.Create((object) to);
      HarmonyMethod.HarmonyFields().ForEach((Action<string>) (f =>
      {
        object obj = fromTrv.Field(f).GetValue();
        if (obj == null)
          return;
        toTrv.Field(f).SetValue(obj);
      }));
    }

    public static HarmonyMethod Clone(this HarmonyMethod original)
    {
      HarmonyMethod to = new HarmonyMethod();
      original.CopyTo(to);
      return to;
    }

    public static HarmonyMethod Merge(this HarmonyMethod master, HarmonyMethod detail)
    {
      if (detail == null)
        return master;
      HarmonyMethod harmonyMethod = new HarmonyMethod();
      Traverse resultTrv = Traverse.Create((object) harmonyMethod);
      Traverse masterTrv = Traverse.Create((object) master);
      Traverse detailTrv = Traverse.Create((object) detail);
      HarmonyMethod.HarmonyFields().ForEach((Action<string>) (f =>
      {
        object obj1 = masterTrv.Field(f).GetValue();
        object obj2 = detailTrv.Field(f).GetValue();
        resultTrv.Field(f).SetValue(obj2 ?? obj1);
      }));
      return harmonyMethod;
    }

    public static List<HarmonyMethod> GetHarmonyMethods(this Type type)
    {
      return ((IEnumerable<object>) type.GetCustomAttributes(true)).Where<object>((Func<object, bool>) (attr => attr is HarmonyAttribute)).Cast<HarmonyAttribute>().Select<HarmonyAttribute, HarmonyMethod>((Func<HarmonyAttribute, HarmonyMethod>) (attr => attr.info)).ToList<HarmonyMethod>();
    }

    public static List<HarmonyMethod> GetHarmonyMethods(this MethodBase method)
    {
      if (method is DynamicMethod)
        return new List<HarmonyMethod>();
      return ((IEnumerable<object>) method.GetCustomAttributes(true)).Where<object>((Func<object, bool>) (attr => attr is HarmonyAttribute)).Cast<HarmonyAttribute>().Select<HarmonyAttribute, HarmonyMethod>((Func<HarmonyAttribute, HarmonyMethod>) (attr => attr.info)).ToList<HarmonyMethod>();
    }
  }
}
