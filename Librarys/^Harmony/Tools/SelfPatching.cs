// Decompiled with JetBrains decompiler
// Type: Harmony.Tools.SelfPatching
// Assembly: 0Harmony, Version=1.2.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 1B235470-4299-4E96-B8B6-361DBE3791D9
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\0Harmony.dll

using Harmony.ILCopying;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Harmony.Tools
{
  internal class SelfPatching
  {
    private static readonly int upgradeToLatestVersionFullNameHash = typeof (UpgradeToLatestVersion).FullName.GetHashCode();

    [UpgradeToLatestVersion(1)]
    private static int GetVersion(MethodBase method)
    {
      object root = ((IEnumerable<object>) method.GetCustomAttributes(false)).Where<object>((Func<object, bool>) (attr => attr.GetType().FullName.GetHashCode() == SelfPatching.upgradeToLatestVersionFullNameHash)).FirstOrDefault<object>();
      if (root == null)
        return -1;
      return Traverse.Create(root).Field("version").GetValue<int>();
    }

    [UpgradeToLatestVersion(1)]
    private static string MethodKey(MethodBase method)
    {
      return method.FullDescription();
    }

    [UpgradeToLatestVersion(1)]
    private static bool IsHarmonyAssembly(Assembly assembly)
    {
      try
      {
        return !assembly.ReflectionOnly && assembly.GetType(typeof (HarmonyInstance).FullName) != null;
      }
      catch (Exception ex)
      {
        return false;
      }
    }

    private static List<MethodBase> GetAllMethods(Assembly assembly)
    {
      Type[] types = assembly.GetTypes();
      return ((IEnumerable<Type>) types).SelectMany<Type, MethodBase>((Func<Type, IEnumerable<MethodBase>>) (type => type.GetMethods(AccessTools.all).Cast<MethodBase>())).Concat<MethodBase>(((IEnumerable<Type>) types).SelectMany<Type, ConstructorInfo>((Func<Type, IEnumerable<ConstructorInfo>>) (type => (IEnumerable<ConstructorInfo>) type.GetConstructors(AccessTools.all))).Cast<MethodBase>()).Concat<MethodBase>(((IEnumerable<Type>) types).SelectMany<Type, PropertyInfo>((Func<Type, IEnumerable<PropertyInfo>>) (type => (IEnumerable<PropertyInfo>) type.GetProperties(AccessTools.all))).Select<PropertyInfo, MethodInfo>((Func<PropertyInfo, MethodInfo>) (prop => prop.GetGetMethod())).Cast<MethodBase>()).Concat<MethodBase>(((IEnumerable<Type>) types).SelectMany<Type, PropertyInfo>((Func<Type, IEnumerable<PropertyInfo>>) (type => (IEnumerable<PropertyInfo>) type.GetProperties(AccessTools.all))).Select<PropertyInfo, MethodInfo>((Func<PropertyInfo, MethodInfo>) (prop => prop.GetSetMethod())).Cast<MethodBase>()).Where<MethodBase>((Func<MethodBase, bool>) (method =>
      {
        if (method != null)
          return method.DeclaringType.Assembly == assembly;
        return false;
      })).OrderBy<MethodBase, string>((Func<MethodBase, string>) (method => method.FullDescription())).ToList<MethodBase>();
    }

    private static string AssemblyInfo(Assembly assembly)
    {
      Version version = assembly.GetName().Version;
      string str = assembly.Location;
      if (str == null || str == "")
        str = new Uri(assembly.CodeBase).LocalPath;
      return str + "(v" + (object) version + (assembly.GlobalAssemblyCache ? (object) ", cached" : (object) "") + ")";
    }

    [UpgradeToLatestVersion(1)]
    public static void PatchOldHarmonyMethods()
    {
      Stopwatch stopwatch = new Stopwatch();
      stopwatch.Start();
      Assembly ourAssembly = new StackTrace(true).GetFrame(1).GetMethod().DeclaringType.Assembly;
      if (HarmonyInstance.DEBUG)
      {
        Version version1 = ourAssembly.GetName().Version;
        Version version2 = typeof (SelfPatching).Assembly.GetName().Version;
        if (version2 > version1)
        {
          FileLog.Log("### Harmony v" + (object) version1 + " started");
          FileLog.Log("### Self-patching unnecessary because we are already patched by v" + (object) version2);
          FileLog.Log("### At " + DateTime.Now.ToString("yyyy-MM-dd hh.mm.ss"));
          return;
        }
        FileLog.Log("Self-patching started (v" + (object) version1 + ")");
      }
      Dictionary<string, MethodBase> potentialMethodsToUpgrade = new Dictionary<string, MethodBase>();
      SelfPatching.GetAllMethods(ourAssembly).Where<MethodBase>((Func<MethodBase, bool>) (method =>
      {
        if (method != null)
          return ((IEnumerable<object>) method.GetCustomAttributes(false)).Any<object>((Func<object, bool>) (attr => attr is UpgradeToLatestVersion));
        return false;
      })).Do<MethodBase>((Action<MethodBase>) (method => potentialMethodsToUpgrade.Add(SelfPatching.MethodKey(method), method)));
      List<Assembly> list = ((IEnumerable<Assembly>) AppDomain.CurrentDomain.GetAssemblies()).Where<Assembly>((Func<Assembly, bool>) (assembly =>
      {
        if (SelfPatching.IsHarmonyAssembly(assembly))
          return assembly != ourAssembly;
        return false;
      })).ToList<Assembly>();
      if (HarmonyInstance.DEBUG)
      {
        list.Do<Assembly>((Action<Assembly>) (assembly => FileLog.Log("Found Harmony " + SelfPatching.AssemblyInfo(assembly))));
        FileLog.Log("Potential methods to upgrade:");
        potentialMethodsToUpgrade.Values.OrderBy<MethodBase, string>((Func<MethodBase, string>) (method => method.FullDescription())).Do<MethodBase>((Action<MethodBase>) (method => FileLog.Log("- " + method.FullDescription())));
      }
      int num1 = 0;
      int num2 = 0;
      int num3 = 0;
      foreach (Assembly assembly in list)
      {
        foreach (MethodBase allMethod in SelfPatching.GetAllMethods(assembly))
        {
          ++num1;
          MethodBase methodBase;
          if (potentialMethodsToUpgrade.TryGetValue(SelfPatching.MethodKey(allMethod), out methodBase))
          {
            int version = SelfPatching.GetVersion(methodBase);
            ++num2;
            if (SelfPatching.GetVersion(allMethod) < version)
            {
              if (HarmonyInstance.DEBUG)
                FileLog.Log("Self-patching " + allMethod.FullDescription() + " in " + SelfPatching.AssemblyInfo(assembly));
              ++num3;
              Memory.DetourMethod(allMethod, methodBase);
            }
          }
        }
      }
      if (!HarmonyInstance.DEBUG)
        return;
      FileLog.Log("Self-patched " + (object) num3 + " out of " + (object) num1 + " methods (" + (object) (num2 - num3) + " skipped) in " + (object) stopwatch.ElapsedMilliseconds + "ms");
    }
  }
}
