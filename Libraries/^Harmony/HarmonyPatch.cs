// Decompiled with JetBrains decompiler
// Type: Harmony.HarmonyPatch
// Assembly: 0Harmony, Version=1.2.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 1B235470-4299-4E96-B8B6-361DBE3791D9
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\0Harmony.dll

using System;
using System.Collections.Generic;

namespace Harmony
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
  public class HarmonyPatch : HarmonyAttribute
  {
    public HarmonyPatch()
    {
    }

    public HarmonyPatch(Type declaringType)
    {
      this.info.declaringType = declaringType;
    }

    public HarmonyPatch(Type declaringType, Type[] argumentTypes)
    {
      this.info.declaringType = declaringType;
      this.info.argumentTypes = argumentTypes;
    }

    public HarmonyPatch(Type declaringType, string methodName)
    {
      this.info.declaringType = declaringType;
      this.info.methodName = methodName;
    }

    public HarmonyPatch(Type declaringType, string methodName, params Type[] argumentTypes)
    {
      this.info.declaringType = declaringType;
      this.info.methodName = methodName;
      this.info.argumentTypes = argumentTypes;
    }

    public HarmonyPatch(
      Type declaringType,
      string methodName,
      Type[] argumentTypes,
      ArgumentType[] argumentVariations)
    {
      this.info.declaringType = declaringType;
      this.info.methodName = methodName;
      this.ParseSpecialArguments(argumentTypes, argumentVariations);
    }

    public HarmonyPatch(Type declaringType, MethodType methodType)
    {
      this.info.declaringType = declaringType;
      this.info.methodType = new MethodType?(methodType);
    }

    public HarmonyPatch(Type declaringType, MethodType methodType, params Type[] argumentTypes)
    {
      this.info.declaringType = declaringType;
      this.info.methodType = new MethodType?(methodType);
      this.info.argumentTypes = argumentTypes;
    }

    public HarmonyPatch(
      Type declaringType,
      MethodType methodType,
      Type[] argumentTypes,
      ArgumentType[] argumentVariations)
    {
      this.info.declaringType = declaringType;
      this.info.methodType = new MethodType?(methodType);
      this.ParseSpecialArguments(argumentTypes, argumentVariations);
    }

    public HarmonyPatch(Type declaringType, string propertyName, MethodType methodType)
    {
      this.info.declaringType = declaringType;
      this.info.methodName = propertyName;
      this.info.methodType = new MethodType?(methodType);
    }

    public HarmonyPatch(string methodName)
    {
      this.info.methodName = methodName;
    }

    public HarmonyPatch(string methodName, params Type[] argumentTypes)
    {
      this.info.methodName = methodName;
      this.info.argumentTypes = argumentTypes;
    }

    public HarmonyPatch(string methodName, Type[] argumentTypes, ArgumentType[] argumentVariations)
    {
      this.info.methodName = methodName;
      this.ParseSpecialArguments(argumentTypes, argumentVariations);
    }

    public HarmonyPatch(string propertyName, MethodType methodType)
    {
      this.info.methodName = propertyName;
      this.info.methodType = new MethodType?(methodType);
    }

    public HarmonyPatch(MethodType methodType)
    {
      this.info.methodType = new MethodType?(methodType);
    }

    public HarmonyPatch(MethodType methodType, params Type[] argumentTypes)
    {
      this.info.methodType = new MethodType?(methodType);
      this.info.argumentTypes = argumentTypes;
    }

    public HarmonyPatch(
      MethodType methodType,
      Type[] argumentTypes,
      ArgumentType[] argumentVariations)
    {
      this.info.methodType = new MethodType?(methodType);
      this.ParseSpecialArguments(argumentTypes, argumentVariations);
    }

    public HarmonyPatch(Type[] argumentTypes)
    {
      this.info.argumentTypes = argumentTypes;
    }

    public HarmonyPatch(Type[] argumentTypes, ArgumentType[] argumentVariations)
    {
      this.ParseSpecialArguments(argumentTypes, argumentVariations);
    }

    [Obsolete("This attribute will be removed in the next major version. Use HarmonyPatch together with MethodType.Getter or MethodType.Setter instead")]
    public HarmonyPatch(string propertyName, PropertyMethod type)
    {
      this.info.methodName = propertyName;
      this.info.methodType = new MethodType?(type == PropertyMethod.Getter ? MethodType.Getter : MethodType.Setter);
    }

    private void ParseSpecialArguments(Type[] argumentTypes, ArgumentType[] argumentVariations)
    {
      if (argumentVariations == null || argumentVariations.Length == 0)
      {
        this.info.argumentTypes = argumentTypes;
      }
      else
      {
        if (argumentTypes.Length < argumentVariations.Length)
          throw new ArgumentException("argumentVariations contains more elements than argumentTypes", nameof (argumentVariations));
        List<Type> typeList = new List<Type>();
        for (int index = 0; index < argumentTypes.Length; ++index)
        {
          Type type = argumentTypes[index];
          switch (argumentVariations[index])
          {
            case ArgumentType.Ref:
            case ArgumentType.Out:
              type = type.MakeByRefType();
              break;
            case ArgumentType.Pointer:
              type = type.MakePointerType();
              break;
          }
          typeList.Add(type);
        }
        this.info.argumentTypes = typeList.ToArray();
      }
    }
  }
}
