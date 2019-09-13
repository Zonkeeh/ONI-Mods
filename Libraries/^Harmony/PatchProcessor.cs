// Decompiled with JetBrains decompiler
// Type: Harmony.PatchProcessor
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
  public class PatchProcessor
  {
    private static object locker = new object();
    private List<MethodBase> originals = new List<MethodBase>();
    private readonly HarmonyInstance instance;
    private readonly Type container;
    private readonly HarmonyMethod containerAttributes;
    private HarmonyMethod prefix;
    private HarmonyMethod postfix;
    private HarmonyMethod transpiler;

    public PatchProcessor(HarmonyInstance instance, Type type, HarmonyMethod attributes)
    {
      this.instance = instance;
      this.container = type;
      this.containerAttributes = attributes ?? new HarmonyMethod((MethodInfo) null);
      this.prefix = this.containerAttributes.Clone();
      this.postfix = this.containerAttributes.Clone();
      this.transpiler = this.containerAttributes.Clone();
      this.PrepareType();
    }

    public PatchProcessor(
      HarmonyInstance instance,
      List<MethodBase> originals,
      HarmonyMethod prefix = null,
      HarmonyMethod postfix = null,
      HarmonyMethod transpiler = null)
    {
      this.instance = instance;
      this.originals = originals;
      this.prefix = prefix ?? new HarmonyMethod((MethodInfo) null);
      this.postfix = postfix ?? new HarmonyMethod((MethodInfo) null);
      this.transpiler = transpiler ?? new HarmonyMethod((MethodInfo) null);
    }

    public static Patches GetPatchInfo(MethodBase method)
    {
      lock (PatchProcessor.locker)
      {
        PatchInfo patchInfo = HarmonySharedState.GetPatchInfo(method);
        if (patchInfo == null)
          return (Patches) null;
        return new Patches(patchInfo.prefixes, patchInfo.postfixes, patchInfo.transpilers);
      }
    }

    public static IEnumerable<MethodBase> AllPatchedMethods()
    {
      lock (PatchProcessor.locker)
        return HarmonySharedState.GetPatchedMethods();
    }

    public List<DynamicMethod> Patch()
    {
      lock (PatchProcessor.locker)
      {
        List<DynamicMethod> dynamicMethodList = new List<DynamicMethod>();
        foreach (MethodBase original in this.originals)
        {
          if (original == null)
            throw new NullReferenceException("original");
          if (this.RunMethod<HarmonyPrepare, bool>(true, (object) original))
          {
            PatchInfo patchInfo = HarmonySharedState.GetPatchInfo(original) ?? new PatchInfo();
            PatchFunctions.AddPrefix(patchInfo, this.instance.Id, this.prefix);
            PatchFunctions.AddPostfix(patchInfo, this.instance.Id, this.postfix);
            PatchFunctions.AddTranspiler(patchInfo, this.instance.Id, this.transpiler);
            dynamicMethodList.Add(PatchFunctions.UpdateWrapper(original, patchInfo, this.instance.Id));
            HarmonySharedState.UpdatePatchInfo(original, patchInfo);
            this.RunMethod<HarmonyCleanup>((object) original);
          }
        }
        return dynamicMethodList;
      }
    }

    public void Unpatch(HarmonyPatchType type, string harmonyID)
    {
      lock (PatchProcessor.locker)
      {
        foreach (MethodBase original in this.originals)
        {
          PatchInfo patchInfo = HarmonySharedState.GetPatchInfo(original) ?? new PatchInfo();
          if (type == HarmonyPatchType.All || type == HarmonyPatchType.Prefix)
            PatchFunctions.RemovePrefix(patchInfo, harmonyID);
          if (type == HarmonyPatchType.All || type == HarmonyPatchType.Postfix)
            PatchFunctions.RemovePostfix(patchInfo, harmonyID);
          if (type == HarmonyPatchType.All || type == HarmonyPatchType.Transpiler)
            PatchFunctions.RemoveTranspiler(patchInfo, harmonyID);
          PatchFunctions.UpdateWrapper(original, patchInfo, this.instance.Id);
          HarmonySharedState.UpdatePatchInfo(original, patchInfo);
        }
      }
    }

    public void Unpatch(MethodInfo patch)
    {
      lock (PatchProcessor.locker)
      {
        foreach (MethodBase original in this.originals)
        {
          PatchInfo patchInfo = HarmonySharedState.GetPatchInfo(original) ?? new PatchInfo();
          PatchFunctions.RemovePatch(patchInfo, patch);
          PatchFunctions.UpdateWrapper(original, patchInfo, this.instance.Id);
          HarmonySharedState.UpdatePatchInfo(original, patchInfo);
        }
      }
    }

    private void PrepareType()
    {
      if (!this.RunMethod<HarmonyPrepare, bool>(true))
        return;
      IEnumerable<MethodBase> source = this.RunMethod<HarmonyTargetMethods, IEnumerable<MethodBase>>((IEnumerable<MethodBase>) null);
      if (source != null)
      {
        this.originals = source.ToList<MethodBase>();
      }
      else
      {
        MethodType? methodType = this.containerAttributes.methodType;
        if (!this.containerAttributes.methodType.HasValue)
          this.containerAttributes.methodType = new MethodType?(MethodType.Normal);
        if (Attribute.GetCustomAttribute((MemberInfo) this.container, typeof (HarmonyPatchAll)) != null)
        {
          Type declaringType = this.containerAttributes.declaringType;
          this.originals.AddRange(AccessTools.GetDeclaredConstructors(declaringType).Cast<MethodBase>());
          this.originals.AddRange(AccessTools.GetDeclaredMethods(declaringType).Cast<MethodBase>());
        }
        else
        {
          MethodBase methodBase = this.RunMethod<HarmonyTargetMethod, MethodBase>((MethodBase) null) ?? this.GetOriginalMethod();
          if (methodBase == null)
            throw new ArgumentException("No target method specified for class " + this.container.FullName + " " + ("(" + "declaringType=" + (object) this.containerAttributes.declaringType + ", " + "methodName =" + this.containerAttributes.methodName + ", " + "methodType=" + (object) methodType + ", " + "argumentTypes=" + this.containerAttributes.argumentTypes.Description() + ")"));
          this.originals.Add(methodBase);
        }
      }
      PatchTools.GetPatches(this.container, out this.prefix.method, out this.postfix.method, out this.transpiler.method);
      if (this.prefix.method != null)
      {
        if (!this.prefix.method.IsStatic)
          throw new ArgumentException("Patch method " + this.prefix.method.FullDescription() + " must be static");
        this.containerAttributes.Merge(HarmonyMethod.Merge(this.prefix.method.GetHarmonyMethods())).CopyTo(this.prefix);
      }
      if (this.postfix.method != null)
      {
        if (!this.postfix.method.IsStatic)
          throw new ArgumentException("Patch method " + this.postfix.method.FullDescription() + " must be static");
        this.containerAttributes.Merge(HarmonyMethod.Merge(this.postfix.method.GetHarmonyMethods())).CopyTo(this.postfix);
      }
      if (this.transpiler.method == null)
        return;
      if (!this.transpiler.method.IsStatic)
        throw new ArgumentException("Patch method " + this.transpiler.method.FullDescription() + " must be static");
      this.containerAttributes.Merge(HarmonyMethod.Merge(this.transpiler.method.GetHarmonyMethods())).CopyTo(this.transpiler);
    }

    private MethodBase GetOriginalMethod()
    {
      HarmonyMethod containerAttributes = this.containerAttributes;
      if (containerAttributes.declaringType == null)
        return (MethodBase) null;
      MethodType? methodType = containerAttributes.methodType;
      if (methodType.HasValue)
      {
        switch (methodType.GetValueOrDefault())
        {
          case MethodType.Normal:
            if (containerAttributes.methodName == null)
              return (MethodBase) null;
            return (MethodBase) AccessTools.DeclaredMethod(containerAttributes.declaringType, containerAttributes.methodName, containerAttributes.argumentTypes, (Type[]) null);
          case MethodType.Getter:
            if (containerAttributes.methodName == null)
              return (MethodBase) null;
            return (MethodBase) AccessTools.DeclaredProperty(containerAttributes.declaringType, containerAttributes.methodName).GetGetMethod(true);
          case MethodType.Setter:
            if (containerAttributes.methodName == null)
              return (MethodBase) null;
            return (MethodBase) AccessTools.DeclaredProperty(containerAttributes.declaringType, containerAttributes.methodName).GetSetMethod(true);
          case MethodType.Constructor:
            return (MethodBase) AccessTools.DeclaredConstructor(containerAttributes.declaringType, containerAttributes.argumentTypes);
          case MethodType.StaticConstructor:
            return (MethodBase) AccessTools.GetDeclaredConstructors(containerAttributes.declaringType).Where<ConstructorInfo>((Func<ConstructorInfo, bool>) (c => c.IsStatic)).FirstOrDefault<ConstructorInfo>();
        }
      }
      return (MethodBase) null;
    }

    private T RunMethod<S, T>(T defaultIfNotExisting, params object[] parameters)
    {
      if (this.container == null)
        return defaultIfNotExisting;
      string name = typeof (S).Name.Replace("Harmony", "");
      List<object> objectList = new List<object>()
      {
        (object) this.instance
      };
      objectList.AddRange((IEnumerable<object>) parameters);
      Type[] types = AccessTools.GetTypes(objectList.ToArray());
      MethodInfo patchMethod1 = PatchTools.GetPatchMethod<S>(this.container, name, types);
      if (patchMethod1 != null && typeof (T).IsAssignableFrom(patchMethod1.ReturnType))
        return (T) patchMethod1.Invoke((object) null, objectList.ToArray());
      MethodInfo patchMethod2 = PatchTools.GetPatchMethod<S>(this.container, name, new Type[1]
      {
        typeof (HarmonyInstance)
      });
      if (patchMethod2 != null && typeof (T).IsAssignableFrom(patchMethod2.ReturnType))
        return (T) patchMethod2.Invoke((object) null, new object[1]
        {
          (object) this.instance
        });
      MethodInfo patchMethod3 = PatchTools.GetPatchMethod<S>(this.container, name, Type.EmptyTypes);
      if (patchMethod3 == null)
        return defaultIfNotExisting;
      if (typeof (T).IsAssignableFrom(patchMethod3.ReturnType))
        return (T) patchMethod3.Invoke((object) null, (object[]) Type.EmptyTypes);
      patchMethod3.Invoke((object) null, (object[]) Type.EmptyTypes);
      return defaultIfNotExisting;
    }

    private void RunMethod<S>(params object[] parameters)
    {
      if (this.container == null)
        return;
      string name = typeof (S).Name.Replace("Harmony", "");
      List<object> objectList = new List<object>()
      {
        (object) this.instance
      };
      objectList.AddRange((IEnumerable<object>) parameters);
      Type[] types = AccessTools.GetTypes(objectList.ToArray());
      MethodInfo patchMethod1 = PatchTools.GetPatchMethod<S>(this.container, name, types);
      if (patchMethod1 != null)
      {
        patchMethod1.Invoke((object) null, objectList.ToArray());
      }
      else
      {
        MethodInfo patchMethod2 = PatchTools.GetPatchMethod<S>(this.container, name, new Type[1]
        {
          typeof (HarmonyInstance)
        });
        if (patchMethod2 != null)
        {
          patchMethod2.Invoke((object) null, new object[1]
          {
            (object) this.instance
          });
        }
        else
        {
          MethodInfo patchMethod3 = PatchTools.GetPatchMethod<S>(this.container, name, Type.EmptyTypes);
          if (patchMethod3 == null)
            return;
          patchMethod3.Invoke((object) null, (object[]) Type.EmptyTypes);
        }
      }
    }
  }
}
