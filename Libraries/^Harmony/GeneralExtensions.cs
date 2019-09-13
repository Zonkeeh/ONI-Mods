// Decompiled with JetBrains decompiler
// Type: Harmony.GeneralExtensions
// Assembly: 0Harmony, Version=1.2.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 1B235470-4299-4E96-B8B6-361DBE3791D9
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\0Harmony.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Harmony
{
  public static class GeneralExtensions
  {
    public static string Join<T>(
      this IEnumerable<T> enumeration,
      Func<T, string> converter = null,
      string delimiter = ", ")
    {
      if (converter == null)
        converter = (Func<T, string>) (t => t.ToString());
      return enumeration.Aggregate<T, string>("", (Func<string, T, string>) ((prev, curr) => prev + (prev != "" ? delimiter : "") + converter(curr)));
    }

    public static string Description(this Type[] parameters)
    {
      if (parameters == null)
        return "NULL";
      string pattern = ", \\w+, Version=[0-9.]+, Culture=neutral, PublicKeyToken=[0-9a-f]+";
      return "(" + ((IEnumerable<Type>) parameters).Join<Type>((Func<Type, string>) (p =>
      {
        if (p != null && p.FullName != null)
          return Regex.Replace(p.FullName, pattern, "");
        return "null";
      }), ", ") + ")";
    }

    public static string FullDescription(this MethodBase method)
    {
      Type[] array = ((IEnumerable<ParameterInfo>) method.GetParameters()).Select<ParameterInfo, Type>((Func<ParameterInfo, Type>) (p => p.ParameterType)).ToArray<Type>();
      return method.DeclaringType.FullName + "." + method.Name + array.Description();
    }

    public static Type[] Types(this ParameterInfo[] pinfo)
    {
      return ((IEnumerable<ParameterInfo>) pinfo).Select<ParameterInfo, Type>((Func<ParameterInfo, Type>) (pi => pi.ParameterType)).ToArray<Type>();
    }

    public static T GetValueSafe<S, T>(this Dictionary<S, T> dictionary, S key)
    {
      T obj;
      if (dictionary.TryGetValue(key, out obj))
        return obj;
      return default (T);
    }

    public static T GetTypedValue<T>(this Dictionary<string, object> dictionary, string key)
    {
      object obj;
      if (dictionary.TryGetValue(key, out obj) && obj is T)
        return (T) obj;
      return default (T);
    }
  }
}
