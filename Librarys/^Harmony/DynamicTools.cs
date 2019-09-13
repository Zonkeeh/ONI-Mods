// Decompiled with JetBrains decompiler
// Type: Harmony.DynamicTools
// Assembly: 0Harmony, Version=1.2.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 1B235470-4299-4E96-B8B6-361DBE3791D9
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\0Harmony.dll

using Harmony.ILCopying;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace Harmony
{
  public static class DynamicTools
  {
    public static DynamicMethod CreateDynamicMethod(MethodBase original, string suffix)
    {
      if (original == null)
        throw new ArgumentNullException("original cannot be null");
      string name = (original.Name + suffix).Replace("<>", "");
      ParameterInfo[] parameters = original.GetParameters();
      List<Type> list = ((IEnumerable<Type>) parameters.Types()).ToList<Type>();
      if (!original.IsStatic)
        list.Insert(0, typeof (object));
      Type[] array = list.ToArray();
      Type returnedType = AccessTools.GetReturnedType(original);
      if (returnedType == null || returnedType.IsByRef)
        return (DynamicMethod) null;
      DynamicMethod dynamicMethod;
      try
      {
        dynamicMethod = new DynamicMethod(name, MethodAttributes.Public | MethodAttributes.Static, CallingConventions.Standard, returnedType, array, original.DeclaringType, true);
      }
      catch (Exception ex)
      {
        return (DynamicMethod) null;
      }
      for (int index = 0; index < parameters.Length; ++index)
        dynamicMethod.DefineParameter(index + 1, parameters[index].Attributes, parameters[index].Name);
      return dynamicMethod;
    }

    public static ILGenerator CreateSaveableMethod(
      MethodBase original,
      string suffix,
      out AssemblyBuilder assemblyBuilder,
      out TypeBuilder typeBuilder)
    {
      AssemblyName name1 = new AssemblyName("DebugAssembly");
      string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
      assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(name1, AssemblyBuilderAccess.RunAndSave, folderPath);
      ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(name1.Name, name1.Name + ".dll");
      typeBuilder = moduleBuilder.DefineType("Debug" + original.DeclaringType.Name, TypeAttributes.Public);
      if (original == null)
        throw new ArgumentNullException("original cannot be null");
      string name2 = (original.Name + suffix).Replace("<>", "");
      List<Type> list = ((IEnumerable<Type>) original.GetParameters().Types()).ToList<Type>();
      if (!original.IsStatic)
        list.Insert(0, typeof (object));
      Type[] array = list.ToArray();
      return typeBuilder.DefineMethod(name2, MethodAttributes.Public | MethodAttributes.Static, CallingConventions.Standard, AccessTools.GetReturnedType(original), array).GetILGenerator();
    }

    public static void SaveMethod(AssemblyBuilder assemblyBuilder, TypeBuilder typeBuilder)
    {
      typeBuilder.CreateType();
      assemblyBuilder.Save("HarmonyDebugAssembly.dll");
    }

    public static LocalBuilder[] DeclareLocalVariables(
      MethodBase original,
      ILGenerator il,
      bool logOutput = true)
    {
      IList<LocalVariableInfo> localVariables = original.GetMethodBody()?.LocalVariables;
      if (localVariables == null)
        return new LocalBuilder[0];
      return localVariables.Select<LocalVariableInfo, LocalBuilder>((Func<LocalVariableInfo, LocalBuilder>) (lvi =>
      {
        LocalBuilder variable = il.DeclareLocal(lvi.LocalType, lvi.IsPinned);
        if (logOutput)
          Emitter.LogLocalVariable(il, variable);
        return variable;
      })).ToArray<LocalBuilder>();
    }

    public static LocalBuilder DeclareLocalVariable(ILGenerator il, Type type)
    {
      if (type.IsByRef)
        type = type.GetElementType();
      if (AccessTools.IsClass(type))
      {
        LocalBuilder localBuilder = il.DeclareLocal(type);
        Emitter.LogLocalVariable(il, localBuilder);
        Emitter.Emit(il, OpCodes.Ldnull);
        Emitter.Emit(il, OpCodes.Stloc, localBuilder);
        return localBuilder;
      }
      if (AccessTools.IsStruct(type))
      {
        LocalBuilder localBuilder = il.DeclareLocal(type);
        Emitter.LogLocalVariable(il, localBuilder);
        Emitter.Emit(il, OpCodes.Ldloca, localBuilder);
        Emitter.Emit(il, OpCodes.Initobj, type);
        return localBuilder;
      }
      if (!AccessTools.IsValue(type))
        return (LocalBuilder) null;
      LocalBuilder localBuilder1 = il.DeclareLocal(type);
      Emitter.LogLocalVariable(il, localBuilder1);
      if (type == typeof (float))
        Emitter.Emit(il, OpCodes.Ldc_R4, 0.0f);
      else if (type == typeof (double))
        Emitter.Emit(il, OpCodes.Ldc_R8, 0.0);
      else if (type == typeof (long))
        Emitter.Emit(il, OpCodes.Ldc_I8, 0L);
      else
        Emitter.Emit(il, OpCodes.Ldc_I4, 0);
      Emitter.Emit(il, OpCodes.Stloc, localBuilder1);
      return localBuilder1;
    }

    public static void PrepareDynamicMethod(DynamicMethod method)
    {
      BindingFlags bindingAttr1 = BindingFlags.Instance | BindingFlags.NonPublic;
      BindingFlags bindingAttr2 = BindingFlags.Static | BindingFlags.NonPublic;
      MethodInfo method1 = typeof (DynamicMethod).GetMethod("CreateDynMethod", bindingAttr1);
      if (method1 != null)
      {
        method1.Invoke((object) method, new object[0]);
      }
      else
      {
        MethodInfo method2 = typeof (RuntimeHelpers).GetMethod("_CompileMethod", bindingAttr2);
        RuntimeMethodHandle runtimeMethodHandle = (RuntimeMethodHandle) typeof (DynamicMethod).GetMethod("GetMethodDescriptor", bindingAttr1).Invoke((object) method, new object[0]);
        MethodInfo method3 = typeof (RuntimeMethodHandle).GetMethod("GetMethodInfo", bindingAttr1);
        if (method3 != null)
        {
          object obj = method3.Invoke((object) runtimeMethodHandle, new object[0]);
          try
          {
            method2.Invoke((object) null, new object[1]
            {
              obj
            });
            return;
          }
          catch (Exception ex)
          {
          }
        }
        if (method2.GetParameters()[0].ParameterType.IsAssignableFrom(runtimeMethodHandle.Value.GetType()))
        {
          method2.Invoke((object) null, new object[1]
          {
            (object) runtimeMethodHandle.Value
          });
        }
        else
        {
          if (!method2.GetParameters()[0].ParameterType.IsAssignableFrom(runtimeMethodHandle.GetType()))
            return;
          method2.Invoke((object) null, new object[1]
          {
            (object) runtimeMethodHandle
          });
        }
      }
    }
  }
}
