// Decompiled with JetBrains decompiler
// Type: Harmony.HarmonyInstance
// Assembly: 0Harmony, Version=1.2.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 1B235470-4299-4E96-B8B6-361DBE3791D9
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\0Harmony.dll

using Harmony.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Harmony
{
  public class HarmonyInstance
  {
    public static bool DEBUG = false;
    private static bool selfPatchingDone = false;
    private readonly string id;

    public string Id
    {
      get
      {
        return this.id;
      }
    }

    private HarmonyInstance(string id)
    {
      if (HarmonyInstance.DEBUG)
      {
        Assembly assembly1 = typeof (HarmonyInstance).Assembly;
        Version version = assembly1.GetName().Version;
        string str1 = assembly1.Location;
        if (str1 == null || str1 == "")
          str1 = new Uri(assembly1.CodeBase).LocalPath;
        FileLog.Log("### Harmony id=" + id + ", version=" + (object) version + ", location=" + str1);
        MethodBase outsideCaller = this.GetOutsideCaller();
        Assembly assembly2 = outsideCaller.DeclaringType.Assembly;
        string str2 = assembly2.Location;
        if (str2 == null || str2 == "")
          str2 = new Uri(assembly2.CodeBase).LocalPath;
        FileLog.Log("### Started from " + outsideCaller.FullDescription() + ", location " + str2);
        FileLog.Log("### At " + DateTime.Now.ToString("yyyy-MM-dd hh.mm.ss"));
      }
      this.id = id;
      if (HarmonyInstance.selfPatchingDone)
        return;
      HarmonyInstance.selfPatchingDone = true;
      SelfPatching.PatchOldHarmonyMethods();
    }

    public static HarmonyInstance Create(string id)
    {
      if (id == null)
        throw new Exception("id cannot be null");
      return new HarmonyInstance(id);
    }

    private MethodBase GetOutsideCaller()
    {
      foreach (StackFrame frame in new StackTrace(true).GetFrames())
      {
        MethodBase method = frame.GetMethod();
        if (method.DeclaringType.Namespace != typeof (HarmonyInstance).Namespace)
          return method;
      }
      throw new Exception("Unexpected end of stack trace");
    }

    public void PatchAll()
    {
      this.PatchAll(new StackTrace().GetFrame(1).GetMethod().ReflectedType.Assembly);
    }

    public void PatchAll(Assembly assembly)
    {
      ((IEnumerable<Type>) assembly.GetTypes()).Do<Type>((Action<Type>) (type =>
      {
        List<HarmonyMethod> harmonyMethods = type.GetHarmonyMethods();
        if (harmonyMethods == null || harmonyMethods.Count<HarmonyMethod>() <= 0)
          return;
        HarmonyMethod attributes = HarmonyMethod.Merge(harmonyMethods);
        new PatchProcessor(this, type, attributes).Patch();
      }));
    }

    public DynamicMethod Patch(
      MethodBase original,
      HarmonyMethod prefix = null,
      HarmonyMethod postfix = null,
      HarmonyMethod transpiler = null)
    {
      return new PatchProcessor(this, new List<MethodBase>()
      {
        original
      }, prefix, postfix, transpiler).Patch().FirstOrDefault<DynamicMethod>();
    }

    public void UnpatchAll(string harmonyID = null)
    {
      foreach (MethodBase methodBase in this.GetPatchedMethods().ToList<MethodBase>())
      {
        MethodBase original = methodBase;
        Patches patchInfo1 = this.GetPatchInfo(original);
        patchInfo1.Prefixes.DoIf<Patch>(new Func<Patch, bool>(IDCheck), (Action<Patch>) (patchInfo => this.Unpatch(original, patchInfo.patch)));
        patchInfo1.Postfixes.DoIf<Patch>(new Func<Patch, bool>(IDCheck), (Action<Patch>) (patchInfo => this.Unpatch(original, patchInfo.patch)));
        patchInfo1.Transpilers.DoIf<Patch>(new Func<Patch, bool>(IDCheck), (Action<Patch>) (patchInfo => this.Unpatch(original, patchInfo.patch)));
      }

      bool IDCheck(Patch patchInfo)
      {
        if (harmonyID != null)
          return patchInfo.owner == harmonyID;
        return true;
      }
    }

    public void Unpatch(MethodBase original, HarmonyPatchType type, string harmonyID = null)
    {
      new PatchProcessor(this, new List<MethodBase>()
      {
        original
      }, (HarmonyMethod) null, (HarmonyMethod) null, (HarmonyMethod) null).Unpatch(type, harmonyID);
    }

    public void Unpatch(MethodBase original, MethodInfo patch)
    {
      new PatchProcessor(this, new List<MethodBase>()
      {
        original
      }, (HarmonyMethod) null, (HarmonyMethod) null, (HarmonyMethod) null).Unpatch(patch);
    }

    public bool HasAnyPatches(string harmonyID)
    {
      return this.GetPatchedMethods().Select<MethodBase, Patches>((Func<MethodBase, Patches>) (original => this.GetPatchInfo(original))).Any<Patches>((Func<Patches, bool>) (info => info.Owners.Contains(harmonyID)));
    }

    public Patches GetPatchInfo(MethodBase method)
    {
      return PatchProcessor.GetPatchInfo(method);
    }

    public IEnumerable<MethodBase> GetPatchedMethods()
    {
      return HarmonySharedState.GetPatchedMethods();
    }

    public Dictionary<string, Version> VersionInfo(out Version currentVersion)
    {
      currentVersion = typeof (HarmonyInstance).Assembly.GetName().Version;
      Dictionary<string, Assembly> assemblies = new Dictionary<string, Assembly>();
      this.GetPatchedMethods().Do<MethodBase>((Action<MethodBase>) (method =>
      {
        PatchInfo patchInfo = HarmonySharedState.GetPatchInfo(method);
        ((IEnumerable<Patch>) patchInfo.prefixes).Do<Patch>((Action<Patch>) (fix => assemblies[fix.owner] = fix.patch.DeclaringType.Assembly));
        ((IEnumerable<Patch>) patchInfo.postfixes).Do<Patch>((Action<Patch>) (fix => assemblies[fix.owner] = fix.patch.DeclaringType.Assembly));
        ((IEnumerable<Patch>) patchInfo.transpilers).Do<Patch>((Action<Patch>) (fix => assemblies[fix.owner] = fix.patch.DeclaringType.Assembly));
      }));
      Dictionary<string, Version> result = new Dictionary<string, Version>();
      assemblies.Do<KeyValuePair<string, Assembly>>((Action<KeyValuePair<string, Assembly>>) (info =>
      {
        AssemblyName assemblyName = ((IEnumerable<AssemblyName>) info.Value.GetReferencedAssemblies()).FirstOrDefault<AssemblyName>((Func<AssemblyName, bool>) (a => a.FullName.StartsWith("0Harmony, Version")));
        if (assemblyName == null)
          return;
        result[info.Key] = assemblyName.Version;
      }));
      return result;
    }
  }
}
