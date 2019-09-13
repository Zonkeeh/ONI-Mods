// Decompiled with JetBrains decompiler
// Type: Harmony.DelegateTypeFactory
// Assembly: 0Harmony, Version=1.2.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 1B235470-4299-4E96-B8B6-361DBE3791D9
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\0Harmony.dll

using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Harmony
{
  public class DelegateTypeFactory
  {
    private readonly ModuleBuilder module;
    private static int counter;

    public DelegateTypeFactory()
    {
      ++DelegateTypeFactory.counter;
      this.module = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName("HarmonyDTFAssembly" + (object) DelegateTypeFactory.counter), AssemblyBuilderAccess.Run).DefineDynamicModule("HarmonyDTFModule" + (object) DelegateTypeFactory.counter);
    }

    public Type CreateDelegateType(MethodInfo method)
    {
      TypeAttributes attr = TypeAttributes.Public | TypeAttributes.Sealed;
      TypeBuilder typeBuilder = this.module.DefineType("HarmonyDTFType" + (object) DelegateTypeFactory.counter, attr, typeof (MulticastDelegate));
      typeBuilder.DefineConstructor(MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.RTSpecialName, CallingConventions.Standard, new Type[2]
      {
        typeof (object),
        typeof (IntPtr)
      }).SetImplementationFlags(MethodImplAttributes.CodeTypeMask);
      ParameterInfo[] parameters = method.GetParameters();
      MethodBuilder methodBuilder = typeBuilder.DefineMethod("Invoke", MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig, method.ReturnType, parameters.Types());
      methodBuilder.SetImplementationFlags(MethodImplAttributes.CodeTypeMask);
      for (int index = 0; index < parameters.Length; ++index)
        methodBuilder.DefineParameter(index + 1, ParameterAttributes.None, parameters[index].Name);
      return typeBuilder.CreateType();
    }
  }
}
