// Decompiled with JetBrains decompiler
// Type: Harmony.AccessTools
// Assembly: 0Harmony, Version=1.2.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 1B235470-4299-4E96-B8B6-361DBE3791D9
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\0Harmony.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.Serialization;

namespace Harmony
{
  public static class AccessTools
  {
    public static BindingFlags all = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.GetProperty | BindingFlags.SetProperty;

    public static Type TypeByName(string name)
    {
      return (Type.GetType(name, false) ?? ((IEnumerable<Assembly>) AppDomain.CurrentDomain.GetAssemblies()).SelectMany<Assembly, Type>((Func<Assembly, IEnumerable<Type>>) (x => (IEnumerable<Type>) x.GetTypes())).FirstOrDefault<Type>((Func<Type, bool>) (x => x.FullName == name))) ?? ((IEnumerable<Assembly>) AppDomain.CurrentDomain.GetAssemblies()).SelectMany<Assembly, Type>((Func<Assembly, IEnumerable<Type>>) (x => (IEnumerable<Type>) x.GetTypes())).FirstOrDefault<Type>((Func<Type, bool>) (x => x.Name == name));
    }

    public static T FindIncludingBaseTypes<T>(Type type, Func<Type, T> action)
    {
      T obj;
      while (true)
      {
        obj = action(type);
        if ((object) obj == null)
        {
          if (type != typeof (object))
            type = type.BaseType;
          else
            goto label_3;
        }
        else
          break;
      }
      return obj;
label_3:
      return default (T);
    }

    public static T FindIncludingInnerTypes<T>(Type type, Func<Type, T> action)
    {
      T obj = action(type);
      if ((object) obj != null)
        return obj;
      foreach (Type nestedType in type.GetNestedTypes(AccessTools.all))
      {
        obj = AccessTools.FindIncludingInnerTypes<T>(nestedType, action);
        if ((object) obj != null)
          break;
      }
      return obj;
    }

    public static FieldInfo Field(Type type, string name)
    {
      if (type == null || name == null)
        return (FieldInfo) null;
      return AccessTools.FindIncludingBaseTypes<FieldInfo>(type, (Func<Type, FieldInfo>) (t => t.GetField(name, AccessTools.all)));
    }

    public static FieldInfo Field(Type type, int idx)
    {
      return AccessTools.GetDeclaredFields(type).ElementAtOrDefault<FieldInfo>(idx);
    }

    public static PropertyInfo DeclaredProperty(Type type, string name)
    {
      if (type == null || name == null)
        return (PropertyInfo) null;
      return type.GetProperty(name, AccessTools.all);
    }

    public static PropertyInfo Property(Type type, string name)
    {
      if (type == null || name == null)
        return (PropertyInfo) null;
      return AccessTools.FindIncludingBaseTypes<PropertyInfo>(type, (Func<Type, PropertyInfo>) (t => t.GetProperty(name, AccessTools.all)));
    }

    public static MethodInfo DeclaredMethod(
      Type type,
      string name,
      Type[] parameters = null,
      Type[] generics = null)
    {
      if (type == null || name == null)
        return (MethodInfo) null;
      ParameterModifier[] modifiers = new ParameterModifier[0];
      MethodInfo methodInfo = parameters != null ? type.GetMethod(name, AccessTools.all, (System.Reflection.Binder) null, parameters, modifiers) : type.GetMethod(name, AccessTools.all);
      if (methodInfo == null)
        return (MethodInfo) null;
      if (generics != null)
        methodInfo = methodInfo.MakeGenericMethod(generics);
      return methodInfo;
    }

    public static MethodInfo Method(
      Type type,
      string name,
      Type[] parameters = null,
      Type[] generics = null)
    {
      if (type == null || name == null)
        return (MethodInfo) null;
      ParameterModifier[] modifiers = new ParameterModifier[0];
      MethodInfo methodInfo;
      if (parameters == null)
      {
        try
        {
          methodInfo = AccessTools.FindIncludingBaseTypes<MethodInfo>(type, (Func<Type, MethodInfo>) (t => t.GetMethod(name, AccessTools.all)));
        }
        catch (AmbiguousMatchException ex)
        {
          methodInfo = AccessTools.FindIncludingBaseTypes<MethodInfo>(type, (Func<Type, MethodInfo>) (t => t.GetMethod(name, AccessTools.all, (System.Reflection.Binder) null, new Type[0], modifiers)));
        }
      }
      else
        methodInfo = AccessTools.FindIncludingBaseTypes<MethodInfo>(type, (Func<Type, MethodInfo>) (t => t.GetMethod(name, AccessTools.all, (System.Reflection.Binder) null, parameters, modifiers)));
      if (methodInfo == null)
        return (MethodInfo) null;
      if (generics != null)
        methodInfo = methodInfo.MakeGenericMethod(generics);
      return methodInfo;
    }

    public static MethodInfo Method(
      string typeColonMethodname,
      Type[] parameters = null,
      Type[] generics = null)
    {
      if (typeColonMethodname == null)
        return (MethodInfo) null;
      string[] strArray = typeColonMethodname.Split(':');
      if (strArray.Length != 2)
        throw new ArgumentException("Method must be specified as 'Namespace.Type1.Type2:MethodName", nameof (typeColonMethodname));
      return AccessTools.Method(AccessTools.TypeByName(strArray[0]), strArray[1], parameters, generics);
    }

    public static List<string> GetMethodNames(Type type)
    {
      if (type == null)
        return new List<string>();
      return ((IEnumerable<MethodInfo>) type.GetMethods(AccessTools.all)).Select<MethodInfo, string>((Func<MethodInfo, string>) (m => m.Name)).ToList<string>();
    }

    public static List<string> GetMethodNames(object instance)
    {
      if (instance == null)
        return new List<string>();
      return AccessTools.GetMethodNames(instance.GetType());
    }

    public static ConstructorInfo DeclaredConstructor(Type type, Type[] parameters = null)
    {
      if (type == null)
        return (ConstructorInfo) null;
      if (parameters == null)
        parameters = new Type[0];
      return type.GetConstructor(AccessTools.all, (System.Reflection.Binder) null, parameters, new ParameterModifier[0]);
    }

    public static ConstructorInfo Constructor(Type type, Type[] parameters = null)
    {
      if (type == null)
        return (ConstructorInfo) null;
      if (parameters == null)
        parameters = new Type[0];
      return AccessTools.FindIncludingBaseTypes<ConstructorInfo>(type, (Func<Type, ConstructorInfo>) (t => t.GetConstructor(AccessTools.all, (System.Reflection.Binder) null, parameters, new ParameterModifier[0])));
    }

    public static List<ConstructorInfo> GetDeclaredConstructors(Type type)
    {
      return ((IEnumerable<ConstructorInfo>) type.GetConstructors(AccessTools.all)).Where<ConstructorInfo>((Func<ConstructorInfo, bool>) (method => method.DeclaringType == type)).ToList<ConstructorInfo>();
    }

    public static List<MethodInfo> GetDeclaredMethods(Type type)
    {
      return ((IEnumerable<MethodInfo>) type.GetMethods(AccessTools.all)).Where<MethodInfo>((Func<MethodInfo, bool>) (method => method.DeclaringType == type)).ToList<MethodInfo>();
    }

    public static List<PropertyInfo> GetDeclaredProperties(Type type)
    {
      return ((IEnumerable<PropertyInfo>) type.GetProperties(AccessTools.all)).Where<PropertyInfo>((Func<PropertyInfo, bool>) (property => property.DeclaringType == type)).ToList<PropertyInfo>();
    }

    public static List<FieldInfo> GetDeclaredFields(Type type)
    {
      return ((IEnumerable<FieldInfo>) type.GetFields(AccessTools.all)).Where<FieldInfo>((Func<FieldInfo, bool>) (field => field.DeclaringType == type)).ToList<FieldInfo>();
    }

    public static Type GetReturnedType(MethodBase method)
    {
      if (method is ConstructorInfo)
        return typeof (void);
      return ((MethodInfo) method).ReturnType;
    }

    public static Type Inner(Type type, string name)
    {
      if (type == null || name == null)
        return (Type) null;
      return AccessTools.FindIncludingBaseTypes<Type>(type, (Func<Type, Type>) (t => t.GetNestedType(name, AccessTools.all)));
    }

    public static Type FirstInner(Type type, Func<Type, bool> predicate)
    {
      if (type == null || predicate == null)
        return (Type) null;
      return ((IEnumerable<Type>) type.GetNestedTypes(AccessTools.all)).FirstOrDefault<Type>((Func<Type, bool>) (subType => predicate(subType)));
    }

    public static MethodInfo FirstMethod(Type type, Func<MethodInfo, bool> predicate)
    {
      if (type == null || predicate == null)
        return (MethodInfo) null;
      return ((IEnumerable<MethodInfo>) type.GetMethods(AccessTools.all)).FirstOrDefault<MethodInfo>((Func<MethodInfo, bool>) (method => predicate(method)));
    }

    public static ConstructorInfo FirstConstructor(
      Type type,
      Func<ConstructorInfo, bool> predicate)
    {
      if (type == null || predicate == null)
        return (ConstructorInfo) null;
      return ((IEnumerable<ConstructorInfo>) type.GetConstructors(AccessTools.all)).FirstOrDefault<ConstructorInfo>((Func<ConstructorInfo, bool>) (constructor => predicate(constructor)));
    }

    public static PropertyInfo FirstProperty(
      Type type,
      Func<PropertyInfo, bool> predicate)
    {
      if (type == null || predicate == null)
        return (PropertyInfo) null;
      return ((IEnumerable<PropertyInfo>) type.GetProperties(AccessTools.all)).FirstOrDefault<PropertyInfo>((Func<PropertyInfo, bool>) (property => predicate(property)));
    }

    public static Type[] GetTypes(object[] parameters)
    {
      if (parameters == null)
        return new Type[0];
      return ((IEnumerable<object>) parameters).Select<object, Type>((Func<object, Type>) (p =>
      {
        if (p != null)
          return p.GetType();
        return typeof (object);
      })).ToArray<Type>();
    }

    public static List<string> GetFieldNames(Type type)
    {
      if (type == null)
        return new List<string>();
      return ((IEnumerable<FieldInfo>) type.GetFields(AccessTools.all)).Select<FieldInfo, string>((Func<FieldInfo, string>) (f => f.Name)).ToList<string>();
    }

    public static List<string> GetFieldNames(object instance)
    {
      if (instance == null)
        return new List<string>();
      return AccessTools.GetFieldNames(instance.GetType());
    }

    public static List<string> GetPropertyNames(Type type)
    {
      if (type == null)
        return new List<string>();
      return ((IEnumerable<PropertyInfo>) type.GetProperties(AccessTools.all)).Select<PropertyInfo, string>((Func<PropertyInfo, string>) (f => f.Name)).ToList<string>();
    }

    public static List<string> GetPropertyNames(object instance)
    {
      if (instance == null)
        return new List<string>();
      return AccessTools.GetPropertyNames(instance.GetType());
    }

    public static AccessTools.FieldRef<T, U> FieldRefAccess<T, U>(string fieldName)
    {
      FieldInfo field = typeof (T).GetField(fieldName, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.NonPublic);
      if (field == null)
        throw new MissingFieldException(typeof (T).Name, fieldName);
      DynamicMethod dynamicMethod = new DynamicMethod("__refget_" + typeof (T).Name + "_fi_" + field.Name, typeof (U), new Type[1]
      {
        typeof (T)
      }, typeof (T), true);
      Traverse traverse = Traverse.Create((object) dynamicMethod);
      traverse.Field("returnType").SetValue((object) typeof (U).MakeByRefType());
      traverse.Field("m_returnType").SetValue((object) typeof (U).MakeByRefType());
      ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
      ilGenerator.Emit(OpCodes.Ldarg_0);
      ilGenerator.Emit(OpCodes.Ldflda, field);
      ilGenerator.Emit(OpCodes.Ret);
      return (AccessTools.FieldRef<T, U>) dynamicMethod.CreateDelegate(typeof (AccessTools.FieldRef<T, U>));
    }

    public static ref U FieldRefAccess<T, U>(T instance, string fieldName)
    {
      return AccessTools.FieldRefAccess<T, U>(fieldName)(instance);
    }

    public static void ThrowMissingMemberException(Type type, params string[] names)
    {
      string str1 = string.Join(",", AccessTools.GetFieldNames(type).ToArray());
      string str2 = string.Join(",", AccessTools.GetPropertyNames(type).ToArray());
      throw new MissingMemberException(string.Join(",", names) + "; available fields: " + str1 + "; available properties: " + str2);
    }

    public static object GetDefaultValue(Type type)
    {
      if (type == null || type == typeof (void) || !type.IsValueType)
        return (object) null;
      return Activator.CreateInstance(type);
    }

    public static object CreateInstance(Type type)
    {
      if (type == null)
        throw new NullReferenceException("Cannot create instance for NULL type");
      if (type.GetConstructor(BindingFlags.Instance | BindingFlags.Public, (System.Reflection.Binder) null, CallingConventions.Any, new Type[0], (ParameterModifier[]) null) != null)
        return Activator.CreateInstance(type);
      return FormatterServices.GetUninitializedObject(type);
    }

    public static object MakeDeepCopy(
      object source,
      Type resultType,
      Func<string, Traverse, Traverse, object> processor = null,
      string pathRoot = "")
    {
      if (source == null)
        return (object) null;
      Type type = source.GetType();
      if (type.IsPrimitive)
        return source;
      if (type.IsEnum)
        return Enum.ToObject(resultType, (int) source);
      if (type.IsGenericType && resultType.IsGenericType)
      {
        MethodInfo methodInfo = AccessTools.FirstMethod(resultType, (Func<MethodInfo, bool>) (m =>
        {
          if (m.Name == "Add")
            return ((IEnumerable<ParameterInfo>) m.GetParameters()).Count<ParameterInfo>() == 1;
          return false;
        }));
        if (methodInfo != null)
        {
          object instance = Activator.CreateInstance(resultType);
          FastInvokeHandler handler = MethodInvoker.GetHandler(methodInfo);
          Type genericArgument = resultType.GetGenericArguments()[0];
          int num = 0;
          foreach (object source1 in source as IEnumerable)
          {
            string str = num++.ToString();
            string pathRoot1 = pathRoot.Length > 0 ? pathRoot + "." + str : str;
            object obj1 = AccessTools.MakeDeepCopy(source1, genericArgument, processor, pathRoot1);
            object obj2 = handler(instance, new object[1]
            {
              obj1
            });
          }
          return instance;
        }
      }
      if (type.IsArray && resultType.IsArray)
      {
        Type elementType = resultType.GetElementType();
        int length = ((Array) source).Length;
        object[] instance = Activator.CreateInstance(resultType, (object) length) as object[];
        object[] objArray = source as object[];
        for (int index = 0; index < length; ++index)
        {
          string str = index.ToString();
          string pathRoot1 = pathRoot.Length > 0 ? pathRoot + "." + str : str;
          instance[index] = AccessTools.MakeDeepCopy(objArray[index], elementType, processor, pathRoot1);
        }
        return (object) instance;
      }
      string str1 = type.Namespace;
      if (str1 == "System" || str1 != null && str1.StartsWith("System."))
        return source;
      object instance1 = AccessTools.CreateInstance(resultType);
      Traverse.IterateFields(source, instance1, (Action<string, Traverse, Traverse>) ((name, src, dst) =>
      {
        string pathRoot1 = pathRoot.Length > 0 ? pathRoot + "." + name : name;
        object source1 = processor != null ? processor(pathRoot1, src, dst) : src.GetValue();
        dst.SetValue(AccessTools.MakeDeepCopy(source1, dst.GetValueType(), processor, pathRoot1));
      }));
      return instance1;
    }

    public static void MakeDeepCopy<T>(
      object source,
      out T result,
      Func<string, Traverse, Traverse, object> processor = null,
      string pathRoot = "")
    {
      result = (T) AccessTools.MakeDeepCopy(source, typeof (T), processor, pathRoot);
    }

    public static bool IsStruct(Type type)
    {
      return type.IsValueType && !AccessTools.IsValue(type) && !AccessTools.IsVoid(type);
    }

    public static bool IsClass(Type type)
    {
      return !type.IsValueType;
    }

    public static bool IsValue(Type type)
    {
      return type.IsPrimitive || type.IsEnum;
    }

    public static bool IsVoid(Type type)
    {
      return type == typeof (void);
    }

    public delegate ref U FieldRef<T, U>(T obj);
  }
}
