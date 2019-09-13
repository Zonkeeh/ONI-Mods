// Decompiled with JetBrains decompiler
// Type: Harmony.FastAccess
// Assembly: 0Harmony, Version=1.2.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 1B235470-4299-4E96-B8B6-361DBE3791D9
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\0Harmony.dll

using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Harmony
{
  public class FastAccess
  {
    public static InstantiationHandler CreateInstantiationHandler(Type type)
    {
      ConstructorInfo constructor = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, (System.Reflection.Binder) null, new Type[0], (ParameterModifier[]) null);
      if (constructor == null)
        throw new ApplicationException(string.Format("The type {0} must declare an empty constructor (the constructor may be private, internal, protected, protected internal, or public).", (object) type));
      DynamicMethod dynamicMethod = new DynamicMethod("InstantiateObject_" + type.Name, MethodAttributes.Public | MethodAttributes.Static, CallingConventions.Standard, typeof (object), (Type[]) null, type, true);
      ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
      ilGenerator.Emit(OpCodes.Newobj, constructor);
      ilGenerator.Emit(OpCodes.Ret);
      return (InstantiationHandler) dynamicMethod.CreateDelegate(typeof (InstantiationHandler));
    }

    public static GetterHandler CreateGetterHandler(PropertyInfo propertyInfo)
    {
      MethodInfo getMethod = propertyInfo.GetGetMethod(true);
      DynamicMethod getDynamicMethod = FastAccess.CreateGetDynamicMethod(propertyInfo.DeclaringType);
      ILGenerator ilGenerator = getDynamicMethod.GetILGenerator();
      ilGenerator.Emit(OpCodes.Ldarg_0);
      ilGenerator.Emit(OpCodes.Call, getMethod);
      FastAccess.BoxIfNeeded(getMethod.ReturnType, ilGenerator);
      ilGenerator.Emit(OpCodes.Ret);
      return (GetterHandler) getDynamicMethod.CreateDelegate(typeof (GetterHandler));
    }

    public static GetterHandler CreateGetterHandler(FieldInfo fieldInfo)
    {
      DynamicMethod getDynamicMethod = FastAccess.CreateGetDynamicMethod(fieldInfo.DeclaringType);
      ILGenerator ilGenerator = getDynamicMethod.GetILGenerator();
      ilGenerator.Emit(OpCodes.Ldarg_0);
      ilGenerator.Emit(OpCodes.Ldfld, fieldInfo);
      FastAccess.BoxIfNeeded(fieldInfo.FieldType, ilGenerator);
      ilGenerator.Emit(OpCodes.Ret);
      return (GetterHandler) getDynamicMethod.CreateDelegate(typeof (GetterHandler));
    }

    public static GetterHandler CreateFieldGetter(Type type, params string[] names)
    {
      foreach (string name in names)
      {
        if (AccessTools.Field(typeof (ILGenerator), name) != null)
          return FastAccess.CreateGetterHandler(AccessTools.Field(type, name));
        if (AccessTools.Property(typeof (ILGenerator), name) != null)
          return FastAccess.CreateGetterHandler(AccessTools.Property(type, name));
      }
      return (GetterHandler) null;
    }

    public static SetterHandler CreateSetterHandler(PropertyInfo propertyInfo)
    {
      MethodInfo setMethod = propertyInfo.GetSetMethod(true);
      DynamicMethod setDynamicMethod = FastAccess.CreateSetDynamicMethod(propertyInfo.DeclaringType);
      ILGenerator ilGenerator = setDynamicMethod.GetILGenerator();
      ilGenerator.Emit(OpCodes.Ldarg_0);
      ilGenerator.Emit(OpCodes.Ldarg_1);
      FastAccess.UnboxIfNeeded(setMethod.GetParameters()[0].ParameterType, ilGenerator);
      ilGenerator.Emit(OpCodes.Call, setMethod);
      ilGenerator.Emit(OpCodes.Ret);
      return (SetterHandler) setDynamicMethod.CreateDelegate(typeof (SetterHandler));
    }

    public static SetterHandler CreateSetterHandler(FieldInfo fieldInfo)
    {
      DynamicMethod setDynamicMethod = FastAccess.CreateSetDynamicMethod(fieldInfo.DeclaringType);
      ILGenerator ilGenerator = setDynamicMethod.GetILGenerator();
      ilGenerator.Emit(OpCodes.Ldarg_0);
      ilGenerator.Emit(OpCodes.Ldarg_1);
      FastAccess.UnboxIfNeeded(fieldInfo.FieldType, ilGenerator);
      ilGenerator.Emit(OpCodes.Stfld, fieldInfo);
      ilGenerator.Emit(OpCodes.Ret);
      return (SetterHandler) setDynamicMethod.CreateDelegate(typeof (SetterHandler));
    }

    private static DynamicMethod CreateGetDynamicMethod(Type type)
    {
      return new DynamicMethod("DynamicGet_" + type.Name, typeof (object), new Type[1]
      {
        typeof (object)
      }, type, true);
    }

    private static DynamicMethod CreateSetDynamicMethod(Type type)
    {
      return new DynamicMethod("DynamicSet_" + type.Name, typeof (void), new Type[2]
      {
        typeof (object),
        typeof (object)
      }, type, true);
    }

    private static void BoxIfNeeded(Type type, ILGenerator generator)
    {
      if (!type.IsValueType)
        return;
      generator.Emit(OpCodes.Box, type);
    }

    private static void UnboxIfNeeded(Type type, ILGenerator generator)
    {
      if (!type.IsValueType)
        return;
      generator.Emit(OpCodes.Unbox_Any, type);
    }
  }
}
