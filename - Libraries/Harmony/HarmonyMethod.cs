// Decompiled with JetBrains decompiler
// Type: Harmony.HarmonyMethod
// Assembly: 0Harmony, Version=1.2.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 1B235470-4299-4E96-B8B6-361DBE3791D9
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\0Harmony.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Harmony
{
  public class HarmonyMethod
  {
    public int prioritiy = -1;
    public MethodInfo method;
    public Type declaringType;
    public string methodName;
    public MethodType? methodType;
    public Type[] argumentTypes;
    public string[] before;
    public string[] after;

    public HarmonyMethod()
    {
    }

    private void ImportMethod(MethodInfo theMethod)
    {
      this.method = theMethod;
      if (this.method == null)
        return;
      List<HarmonyMethod> harmonyMethods = this.method.GetHarmonyMethods();
      if (harmonyMethods != null)
        HarmonyMethod.Merge(harmonyMethods).CopyTo(this);
    }

    public HarmonyMethod(MethodInfo method)
    {
      this.ImportMethod(method);
    }

    public HarmonyMethod(Type type, string name, Type[] parameters = null)
    {
      this.ImportMethod(AccessTools.Method(type, name, parameters, (Type[]) null));
    }

    public static List<string> HarmonyFields()
    {
      return AccessTools.GetFieldNames(typeof (HarmonyMethod)).Where<string>((Func<string, bool>) (s => s != "method")).ToList<string>();
    }

    public static HarmonyMethod Merge(List<HarmonyMethod> attributes)
    {
      HarmonyMethod harmonyMethod = new HarmonyMethod();
      if (attributes == null)
        return harmonyMethod;
      Traverse resultTrv = Traverse.Create((object) harmonyMethod);
      attributes.ForEach((Action<HarmonyMethod>) (attribute =>
      {
        Traverse trv = Traverse.Create((object) attribute);
        HarmonyMethod.HarmonyFields().ForEach((Action<string>) (f =>
        {
          object obj = trv.Field(f).GetValue();
          if (obj == null)
            return;
          resultTrv.Field(f).SetValue(obj);
        }));
      }));
      return harmonyMethod;
    }

    public override string ToString()
    {
      string result = "HarmonyMethod[";
      Traverse trv = Traverse.Create((object) this);
      HarmonyMethod.HarmonyFields().ForEach((Action<string>) (f => result = result + f + "=" + trv.Field(f).GetValue()));
      return result + "]";
    }
  }
}
