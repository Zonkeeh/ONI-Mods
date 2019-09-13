// Decompiled with JetBrains decompiler
// Type: Harmony.HarmonySharedState
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
  public static class HarmonySharedState
  {
    private static readonly string name = nameof (HarmonySharedState);
    internal static readonly int internalVersion = 100;
    internal static int actualVersion = -1;

    private static Dictionary<MethodBase, byte[]> GetState()
    {
      lock (HarmonySharedState.name)
      {
        Assembly assembly = HarmonySharedState.SharedStateAssembly();
        if (assembly == null)
        {
          ModuleBuilder moduleBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName(HarmonySharedState.name), AssemblyBuilderAccess.Run).DefineDynamicModule(HarmonySharedState.name);
          TypeAttributes attr = TypeAttributes.Public | TypeAttributes.Abstract | TypeAttributes.Sealed;
          TypeBuilder typeBuilder = moduleBuilder.DefineType(HarmonySharedState.name, attr);
          typeBuilder.DefineField("state", typeof (Dictionary<MethodBase, byte[]>), FieldAttributes.Public | FieldAttributes.Static);
          typeBuilder.DefineField("version", typeof (int), FieldAttributes.Public | FieldAttributes.Static).SetConstant((object) HarmonySharedState.internalVersion);
          typeBuilder.CreateType();
          assembly = HarmonySharedState.SharedStateAssembly();
          if (assembly == null)
            throw new Exception("Cannot find or create harmony shared state");
        }
        FieldInfo field1 = assembly.GetType(HarmonySharedState.name).GetField("version");
        if (field1 == null)
          throw new Exception("Cannot find harmony state version field");
        HarmonySharedState.actualVersion = (int) field1.GetValue((object) null);
        FieldInfo field2 = assembly.GetType(HarmonySharedState.name).GetField("state");
        if (field2 == null)
          throw new Exception("Cannot find harmony state field");
        if (field2.GetValue((object) null) == null)
          field2.SetValue((object) null, (object) new Dictionary<MethodBase, byte[]>());
        return (Dictionary<MethodBase, byte[]>) field2.GetValue((object) null);
      }
    }

    private static Assembly SharedStateAssembly()
    {
      return ((IEnumerable<Assembly>) AppDomain.CurrentDomain.GetAssemblies()).FirstOrDefault<Assembly>((Func<Assembly, bool>) (a => a.GetName().Name.Contains(HarmonySharedState.name)));
    }

    internal static PatchInfo GetPatchInfo(MethodBase method)
    {
      byte[] valueSafe = HarmonySharedState.GetState().GetValueSafe<MethodBase, byte[]>(method);
      if (valueSafe == null)
        return (PatchInfo) null;
      return PatchInfoSerialization.Deserialize(valueSafe);
    }

    internal static IEnumerable<MethodBase> GetPatchedMethods()
    {
      return HarmonySharedState.GetState().Keys.AsEnumerable<MethodBase>();
    }

    internal static void UpdatePatchInfo(MethodBase method, PatchInfo patchInfo)
    {
      HarmonySharedState.GetState()[method] = patchInfo.Serialize();
    }
  }
}
