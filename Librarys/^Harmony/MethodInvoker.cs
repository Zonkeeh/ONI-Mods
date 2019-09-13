// Decompiled with JetBrains decompiler
// Type: Harmony.MethodInvoker
// Assembly: 0Harmony, Version=1.2.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 1B235470-4299-4E96-B8B6-361DBE3791D9
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\0Harmony.dll

using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Harmony
{
  public class MethodInvoker
  {
    public static FastInvokeHandler GetHandler(
      DynamicMethod methodInfo,
      Module module)
    {
      return MethodInvoker.Handler((MethodInfo) methodInfo, module, false);
    }

    public static FastInvokeHandler GetHandler(MethodInfo methodInfo)
    {
      return MethodInvoker.Handler(methodInfo, methodInfo.DeclaringType.Module, false);
    }

    private static FastInvokeHandler Handler(
      MethodInfo methodInfo,
      Module module,
      bool directBoxValueAccess = false)
    {
      DynamicMethod dynamicMethod = new DynamicMethod("FastInvoke_" + methodInfo.Name + "_" + (directBoxValueAccess ? "direct" : "indirect"), typeof (object), new Type[2]
      {
        typeof (object),
        typeof (object[])
      }, module, true);
      ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
      if (!methodInfo.IsStatic)
      {
        ilGenerator.Emit(OpCodes.Ldarg_0);
        MethodInvoker.EmitUnboxIfNeeded(ilGenerator, methodInfo.DeclaringType);
      }
      bool flag = true;
      ParameterInfo[] parameters = methodInfo.GetParameters();
      for (int index = 0; index < parameters.Length; ++index)
      {
        Type cls = parameters[index].ParameterType;
        bool isByRef = cls.IsByRef;
        if (isByRef)
          cls = cls.GetElementType();
        bool isValueType = cls.IsValueType;
        if (isByRef & isValueType && !directBoxValueAccess)
        {
          ilGenerator.Emit(OpCodes.Ldarg_1);
          MethodInvoker.EmitFastInt(ilGenerator, index);
        }
        ilGenerator.Emit(OpCodes.Ldarg_1);
        MethodInvoker.EmitFastInt(ilGenerator, index);
        if (isByRef && !isValueType)
        {
          ilGenerator.Emit(OpCodes.Ldelema, typeof (object));
        }
        else
        {
          ilGenerator.Emit(OpCodes.Ldelem_Ref);
          if (isValueType)
          {
            if (!isByRef || !directBoxValueAccess)
            {
              ilGenerator.Emit(OpCodes.Unbox_Any, cls);
              if (isByRef)
              {
                ilGenerator.Emit(OpCodes.Box, cls);
                ilGenerator.Emit(OpCodes.Dup);
                ilGenerator.Emit(OpCodes.Unbox, cls);
                if (flag)
                {
                  flag = false;
                  ilGenerator.DeclareLocal(typeof (void*), true);
                }
                ilGenerator.Emit(OpCodes.Stloc_0);
                ilGenerator.Emit(OpCodes.Stelem_Ref);
                ilGenerator.Emit(OpCodes.Ldloc_0);
              }
            }
            else
              ilGenerator.Emit(OpCodes.Unbox, cls);
          }
        }
      }
      if (methodInfo.IsStatic)
        ilGenerator.EmitCall(OpCodes.Call, methodInfo, (Type[]) null);
      else
        ilGenerator.EmitCall(OpCodes.Callvirt, methodInfo, (Type[]) null);
      if (methodInfo.ReturnType == typeof (void))
        ilGenerator.Emit(OpCodes.Ldnull);
      else
        MethodInvoker.EmitBoxIfNeeded(ilGenerator, methodInfo.ReturnType);
      ilGenerator.Emit(OpCodes.Ret);
      return (FastInvokeHandler) dynamicMethod.CreateDelegate(typeof (FastInvokeHandler));
    }

    private static void EmitCastToReference(ILGenerator il, Type type)
    {
      if (type.IsValueType)
        il.Emit(OpCodes.Unbox_Any, type);
      else
        il.Emit(OpCodes.Castclass, type);
    }

    private static void EmitUnboxIfNeeded(ILGenerator il, Type type)
    {
      if (!type.IsValueType)
        return;
      il.Emit(OpCodes.Unbox_Any, type);
    }

    private static void EmitBoxIfNeeded(ILGenerator il, Type type)
    {
      if (!type.IsValueType)
        return;
      il.Emit(OpCodes.Box, type);
    }

    private static void EmitFastInt(ILGenerator il, int value)
    {
      switch (value)
      {
        case -1:
          il.Emit(OpCodes.Ldc_I4_M1);
          break;
        case 0:
          il.Emit(OpCodes.Ldc_I4_0);
          break;
        case 1:
          il.Emit(OpCodes.Ldc_I4_1);
          break;
        case 2:
          il.Emit(OpCodes.Ldc_I4_2);
          break;
        case 3:
          il.Emit(OpCodes.Ldc_I4_3);
          break;
        case 4:
          il.Emit(OpCodes.Ldc_I4_4);
          break;
        case 5:
          il.Emit(OpCodes.Ldc_I4_5);
          break;
        case 6:
          il.Emit(OpCodes.Ldc_I4_6);
          break;
        case 7:
          il.Emit(OpCodes.Ldc_I4_7);
          break;
        case 8:
          il.Emit(OpCodes.Ldc_I4_8);
          break;
        default:
          if (value > -129 && value < 128)
          {
            il.Emit(OpCodes.Ldc_I4_S, (sbyte) value);
            break;
          }
          il.Emit(OpCodes.Ldc_I4, value);
          break;
      }
    }
  }
}
