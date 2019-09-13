// Decompiled with JetBrains decompiler
// Type: Harmony.AccessCache
// Assembly: 0Harmony, Version=1.2.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 1B235470-4299-4E96-B8B6-361DBE3791D9
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\0Harmony.dll

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Harmony
{
  public class AccessCache
  {
    private Dictionary<Type, Dictionary<string, FieldInfo>> fields = new Dictionary<Type, Dictionary<string, FieldInfo>>();
    private Dictionary<Type, Dictionary<string, PropertyInfo>> properties = new Dictionary<Type, Dictionary<string, PropertyInfo>>();
    private readonly Dictionary<Type, Dictionary<string, Dictionary<int, MethodBase>>> methods = new Dictionary<Type, Dictionary<string, Dictionary<int, MethodBase>>>();

    [UpgradeToLatestVersion(1)]
    public FieldInfo GetFieldInfo(Type type, string name)
    {
      Dictionary<string, FieldInfo> dictionary = (Dictionary<string, FieldInfo>) null;
      if (!this.fields.TryGetValue(type, out dictionary))
      {
        dictionary = new Dictionary<string, FieldInfo>();
        this.fields.Add(type, dictionary);
      }
      FieldInfo fieldInfo = (FieldInfo) null;
      if (!dictionary.TryGetValue(name, out fieldInfo))
      {
        fieldInfo = AccessTools.Field(type, name);
        dictionary.Add(name, fieldInfo);
      }
      return fieldInfo;
    }

    public PropertyInfo GetPropertyInfo(Type type, string name)
    {
      Dictionary<string, PropertyInfo> dictionary = (Dictionary<string, PropertyInfo>) null;
      if (!this.properties.TryGetValue(type, out dictionary))
      {
        dictionary = new Dictionary<string, PropertyInfo>();
        this.properties.Add(type, dictionary);
      }
      PropertyInfo propertyInfo = (PropertyInfo) null;
      if (!dictionary.TryGetValue(name, out propertyInfo))
      {
        propertyInfo = AccessTools.Property(type, name);
        dictionary.Add(name, propertyInfo);
      }
      return propertyInfo;
    }

    private static int CombinedHashCode(IEnumerable<object> objects)
    {
      int num1 = 352654597;
      int num2 = num1;
      int num3 = 0;
      foreach (object obj in objects)
      {
        if (num3 % 2 == 0)
          num1 = (num1 << 5) + num1 + (num1 >> 27) ^ obj.GetHashCode();
        else
          num2 = (num2 << 5) + num2 + (num2 >> 27) ^ obj.GetHashCode();
        ++num3;
      }
      return num1 + num2 * 1566083941;
    }

    public MethodBase GetMethodInfo(Type type, string name, Type[] arguments)
    {
      Dictionary<string, Dictionary<int, MethodBase>> dictionary1 = (Dictionary<string, Dictionary<int, MethodBase>>) null;
      this.methods.TryGetValue(type, out dictionary1);
      if (dictionary1 == null)
      {
        dictionary1 = new Dictionary<string, Dictionary<int, MethodBase>>();
        this.methods.Add(type, dictionary1);
      }
      Dictionary<int, MethodBase> dictionary2 = (Dictionary<int, MethodBase>) null;
      dictionary1.TryGetValue(name, out dictionary2);
      if (dictionary2 == null)
      {
        dictionary2 = new Dictionary<int, MethodBase>();
        dictionary1.Add(name, dictionary2);
      }
      MethodBase methodBase = (MethodBase) null;
      int key = AccessCache.CombinedHashCode((IEnumerable<object>) arguments);
      dictionary2.TryGetValue(key, out methodBase);
      if (methodBase == null)
      {
        methodBase = (MethodBase) AccessTools.Method(type, name, arguments, (Type[]) null);
        dictionary2.Add(key, methodBase);
      }
      return methodBase;
    }
  }
}
