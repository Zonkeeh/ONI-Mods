// Decompiled with JetBrains decompiler
// Type: Harmony.Traverse
// Assembly: 0Harmony, Version=1.2.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 1B235470-4299-4E96-B8B6-361DBE3791D9
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\0Harmony.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Harmony
{
  public class Traverse
  {
    public static Action<Traverse, Traverse> CopyFields = (Action<Traverse, Traverse>) ((from, to) => to.SetValue(from.GetValue()));
    private static AccessCache Cache;
    private Type _type;
    private object _root;
    private MemberInfo _info;
    private MethodBase _method;
    private object[] _params;

    [MethodImpl(MethodImplOptions.Synchronized)]
    static Traverse()
    {
      if (Traverse.Cache != null)
        return;
      Traverse.Cache = new AccessCache();
    }

    public static Traverse Create(Type type)
    {
      return new Traverse(type);
    }

    public static Traverse Create<T>()
    {
      return Traverse.Create(typeof (T));
    }

    public static Traverse Create(object root)
    {
      return new Traverse(root);
    }

    public static Traverse CreateWithType(string name)
    {
      return new Traverse(AccessTools.TypeByName(name));
    }

    private Traverse()
    {
    }

    public Traverse(Type type)
    {
      this._type = type;
    }

    public Traverse(object root)
    {
      this._root = root;
      this._type = root?.GetType();
    }

    private Traverse(object root, MemberInfo info, object[] index)
    {
      this._root = root;
      this._type = root?.GetType();
      this._info = info;
      this._params = index;
    }

    private Traverse(object root, MethodInfo method, object[] parameter)
    {
      this._root = root;
      this._type = method.ReturnType;
      this._method = (MethodBase) method;
      this._params = parameter;
    }

    public object GetValue()
    {
      if (this._info is FieldInfo)
        return ((FieldInfo) this._info).GetValue(this._root);
      if (this._info is PropertyInfo)
        return ((PropertyInfo) this._info).GetValue(this._root, AccessTools.all, (System.Reflection.Binder) null, this._params, CultureInfo.CurrentCulture);
      if (this._method != null)
        return this._method.Invoke(this._root, this._params);
      if (this._root == null && this._type != null)
        return (object) this._type;
      return this._root;
    }

    public T GetValue<T>()
    {
      object obj = this.GetValue();
      if (obj == null)
        return default (T);
      return (T) obj;
    }

    public object GetValue(params object[] arguments)
    {
      if (this._method == null)
        throw new Exception("cannot get method value without method");
      return this._method.Invoke(this._root, arguments);
    }

    public T GetValue<T>(params object[] arguments)
    {
      if (this._method == null)
        throw new Exception("cannot get method value without method");
      return (T) this._method.Invoke(this._root, arguments);
    }

    public Traverse SetValue(object value)
    {
      if (this._info is FieldInfo)
        ((FieldInfo) this._info).SetValue(this._root, value, AccessTools.all, (System.Reflection.Binder) null, CultureInfo.CurrentCulture);
      if (this._info is PropertyInfo)
        ((PropertyInfo) this._info).SetValue(this._root, value, AccessTools.all, (System.Reflection.Binder) null, this._params, CultureInfo.CurrentCulture);
      if (this._method != null)
        throw new Exception("cannot set value of method " + this._method.FullDescription());
      return this;
    }

    public Type GetValueType()
    {
      if (this._info is FieldInfo)
        return ((FieldInfo) this._info).FieldType;
      if (this._info is PropertyInfo)
        return ((PropertyInfo) this._info).PropertyType;
      return (Type) null;
    }

    private Traverse Resolve()
    {
      if (this._root == null && this._type != null)
        return this;
      return new Traverse(this.GetValue());
    }

    public Traverse Type(string name)
    {
      if (name == null)
        throw new ArgumentNullException("name cannot be null");
      if (this._type == null)
        return new Traverse();
      Type type = AccessTools.Inner(this._type, name);
      if (type == null)
        return new Traverse();
      return new Traverse(type);
    }

    public Traverse Field(string name)
    {
      if (name == null)
        throw new ArgumentNullException("name cannot be null");
      Traverse traverse = this.Resolve();
      if (traverse._type == null)
        return new Traverse();
      FieldInfo fieldInfo = Traverse.Cache.GetFieldInfo(traverse._type, name);
      if (fieldInfo == null)
        return new Traverse();
      if (!fieldInfo.IsStatic && traverse._root == null)
        return new Traverse();
      return new Traverse(traverse._root, (MemberInfo) fieldInfo, (object[]) null);
    }

    public Traverse<T> Field<T>(string name)
    {
      return new Traverse<T>(this.Field(name));
    }

    public List<string> Fields()
    {
      return AccessTools.GetFieldNames(this.Resolve()._type);
    }

    public Traverse Property(string name, object[] index = null)
    {
      if (name == null)
        throw new ArgumentNullException("name cannot be null");
      Traverse traverse = this.Resolve();
      if (traverse._root == null || traverse._type == null)
        return new Traverse();
      PropertyInfo propertyInfo = Traverse.Cache.GetPropertyInfo(traverse._type, name);
      if (propertyInfo == null)
        return new Traverse();
      return new Traverse(traverse._root, (MemberInfo) propertyInfo, index);
    }

    public Traverse<T> Property<T>(string name, object[] index = null)
    {
      return new Traverse<T>(this.Property(name, index));
    }

    public List<string> Properties()
    {
      return AccessTools.GetPropertyNames(this.Resolve()._type);
    }

    public Traverse Method(string name, params object[] arguments)
    {
      if (name == null)
        throw new ArgumentNullException("name cannot be null");
      Traverse traverse = this.Resolve();
      if (traverse._type == null)
        return new Traverse();
      Type[] types = AccessTools.GetTypes(arguments);
      MethodBase methodInfo = Traverse.Cache.GetMethodInfo(traverse._type, name, types);
      if (methodInfo == null)
        return new Traverse();
      return new Traverse(traverse._root, (MethodInfo) methodInfo, arguments);
    }

    public Traverse Method(string name, Type[] paramTypes, object[] arguments = null)
    {
      if (name == null)
        throw new ArgumentNullException("name cannot be null");
      Traverse traverse = this.Resolve();
      if (traverse._type == null)
        return new Traverse();
      MethodBase methodInfo = Traverse.Cache.GetMethodInfo(traverse._type, name, paramTypes);
      if (methodInfo == null)
        return new Traverse();
      return new Traverse(traverse._root, (MethodInfo) methodInfo, arguments);
    }

    public List<string> Methods()
    {
      return AccessTools.GetMethodNames(this.Resolve()._type);
    }

    public bool FieldExists()
    {
      return this._info != null;
    }

    public bool MethodExists()
    {
      return this._method != null;
    }

    public bool TypeExists()
    {
      return this._type != null;
    }

    public static void IterateFields(object source, Action<Traverse> action)
    {
      Traverse sourceTrv = Traverse.Create(source);
      AccessTools.GetFieldNames(source).ForEach((Action<string>) (f => action(sourceTrv.Field(f))));
    }

    public static void IterateFields(
      object source,
      object target,
      Action<Traverse, Traverse> action)
    {
      Traverse sourceTrv = Traverse.Create(source);
      Traverse targetTrv = Traverse.Create(target);
      AccessTools.GetFieldNames(source).ForEach((Action<string>) (f => action(sourceTrv.Field(f), targetTrv.Field(f))));
    }

    public static void IterateFields(
      object source,
      object target,
      Action<string, Traverse, Traverse> action)
    {
      Traverse sourceTrv = Traverse.Create(source);
      Traverse targetTrv = Traverse.Create(target);
      AccessTools.GetFieldNames(source).ForEach((Action<string>) (f => action(f, sourceTrv.Field(f), targetTrv.Field(f))));
    }

    public static void IterateProperties(object source, Action<Traverse> action)
    {
      Traverse sourceTrv = Traverse.Create(source);
      AccessTools.GetPropertyNames(source).ForEach((Action<string>) (f => action(sourceTrv.Property(f, (object[]) null))));
    }

    public static void IterateProperties(
      object source,
      object target,
      Action<Traverse, Traverse> action)
    {
      Traverse sourceTrv = Traverse.Create(source);
      Traverse targetTrv = Traverse.Create(target);
      AccessTools.GetPropertyNames(source).ForEach((Action<string>) (f => action(sourceTrv.Property(f, (object[]) null), targetTrv.Property(f, (object[]) null))));
    }

    public static void IterateProperties(
      object source,
      object target,
      Action<string, Traverse, Traverse> action)
    {
      Traverse sourceTrv = Traverse.Create(source);
      Traverse targetTrv = Traverse.Create(target);
      AccessTools.GetPropertyNames(source).ForEach((Action<string>) (f => action(f, sourceTrv.Property(f, (object[]) null), targetTrv.Property(f, (object[]) null))));
    }

    public override string ToString()
    {
      return ((object) this._method ?? this.GetValue())?.ToString();
    }
  }
}
