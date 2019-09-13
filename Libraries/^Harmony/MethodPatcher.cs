// Decompiled with JetBrains decompiler
// Type: Harmony.MethodPatcher
// Assembly: 0Harmony, Version=1.2.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 1B235470-4299-4E96-B8B6-361DBE3791D9
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\0Harmony.dll

using Harmony.ILCopying;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Harmony
{
  public static class MethodPatcher
  {
    public static string INSTANCE_PARAM = "__instance";
    public static string ORIGINAL_METHOD_PARAM = "__originalMethod";
    public static string RESULT_VAR = "__result";
    public static string STATE_VAR = "__state";
    public static string PARAM_INDEX_PREFIX = "__";
    public static string INSTANCE_FIELD_PREFIX = "___";
    private static readonly bool DEBUG_METHOD_GENERATION_BY_DLL_CREATION = false;
    private static MethodInfo getMethodMethod = typeof (MethodBase).GetMethod("GetMethodFromHandle", new Type[1]
    {
      typeof (RuntimeMethodHandle)
    });

    [UpgradeToLatestVersion(1)]
    public static DynamicMethod CreatePatchedMethod(
      MethodBase original,
      List<MethodInfo> prefixes,
      List<MethodInfo> postfixes,
      List<MethodInfo> transpilers)
    {
      return MethodPatcher.CreatePatchedMethod(original, "HARMONY_PATCH_1.1.1", prefixes, postfixes, transpilers);
    }

    public static DynamicMethod CreatePatchedMethod(
      MethodBase original,
      string harmonyInstanceID,
      List<MethodInfo> prefixes,
      List<MethodInfo> postfixes,
      List<MethodInfo> transpilers)
    {
      try
      {
        if (HarmonyInstance.DEBUG)
          FileLog.LogBuffered("### Patch " + (object) original.DeclaringType + ", " + (object) original);
        int num = prefixes.Count<MethodInfo>() + postfixes.Count<MethodInfo>();
        DynamicMethod dynamicMethod = DynamicTools.CreateDynamicMethod(original, "_Patch" + (object) num);
        if (dynamicMethod == null)
          return (DynamicMethod) null;
        ILGenerator il = dynamicMethod.GetILGenerator();
        AssemblyBuilder assemblyBuilder = (AssemblyBuilder) null;
        TypeBuilder typeBuilder = (TypeBuilder) null;
        if (MethodPatcher.DEBUG_METHOD_GENERATION_BY_DLL_CREATION)
          il = DynamicTools.CreateSaveableMethod(original, "_Patch" + (object) num, out assemblyBuilder, out typeBuilder);
        LocalBuilder[] existingVariables = DynamicTools.DeclareLocalVariables(original, il, true);
        Dictionary<string, LocalBuilder> privateVars = new Dictionary<string, LocalBuilder>();
        LocalBuilder local = (LocalBuilder) null;
        if (num > 0)
        {
          local = DynamicTools.DeclareLocalVariable(il, AccessTools.GetReturnedType(original));
          privateVars[MethodPatcher.RESULT_VAR] = local;
        }
        prefixes.ForEach((Action<MethodInfo>) (prefix => ((IEnumerable<ParameterInfo>) prefix.GetParameters()).Where<ParameterInfo>((Func<ParameterInfo, bool>) (patchParam => patchParam.Name == MethodPatcher.STATE_VAR)).Do<ParameterInfo>((Action<ParameterInfo>) (patchParam => privateVars[prefix.DeclaringType.FullName] = DynamicTools.DeclareLocalVariable(il, patchParam.ParameterType)))));
        Label label1 = il.DefineLabel();
        bool flag = MethodPatcher.AddPrefixes(il, original, prefixes, privateVars, label1);
        MethodCopier methodCopier = new MethodCopier(original, il, existingVariables);
        foreach (MethodInfo transpiler in transpilers)
          methodCopier.AddTranspiler(transpiler);
        List<Label> endLabels = new List<Label>();
        List<ExceptionBlock> endBlocks = new List<ExceptionBlock>();
        methodCopier.Finalize(endLabels, endBlocks);
        foreach (Label label2 in endLabels)
          Emitter.MarkLabel(il, label2);
        foreach (ExceptionBlock block in endBlocks)
          Emitter.MarkBlockAfter(il, block);
        if (local != null)
          Emitter.Emit(il, OpCodes.Stloc, local);
        if (flag)
          Emitter.MarkLabel(il, label1);
        MethodPatcher.AddPostfixes(il, original, postfixes, privateVars, false);
        if (local != null)
          Emitter.Emit(il, OpCodes.Ldloc, local);
        MethodPatcher.AddPostfixes(il, original, postfixes, privateVars, true);
        Emitter.Emit(il, OpCodes.Ret);
        if (HarmonyInstance.DEBUG)
        {
          FileLog.LogBuffered("DONE");
          FileLog.LogBuffered("");
          FileLog.FlushBuffer();
        }
        if (MethodPatcher.DEBUG_METHOD_GENERATION_BY_DLL_CREATION)
        {
          DynamicTools.SaveMethod(assemblyBuilder, typeBuilder);
          return (DynamicMethod) null;
        }
        DynamicTools.PrepareDynamicMethod(dynamicMethod);
        return dynamicMethod;
      }
      catch (Exception ex)
      {
        throw new Exception("Exception from HarmonyInstance \"" + harmonyInstanceID + "\"", ex);
      }
      finally
      {
        if (HarmonyInstance.DEBUG)
          FileLog.FlushBuffer();
      }
    }

    private static OpCode LoadIndOpCodeFor(Type type)
    {
      if (type.IsEnum)
        return OpCodes.Ldind_I4;
      if (type == typeof (float))
        return OpCodes.Ldind_R4;
      if (type == typeof (double))
        return OpCodes.Ldind_R8;
      if (type == typeof (byte))
        return OpCodes.Ldind_U1;
      if (type == typeof (ushort))
        return OpCodes.Ldind_U2;
      if (type == typeof (uint))
        return OpCodes.Ldind_U4;
      if (type == typeof (ulong))
        return OpCodes.Ldind_I8;
      if (type == typeof (sbyte))
        return OpCodes.Ldind_I1;
      if (type == typeof (short))
        return OpCodes.Ldind_I2;
      if (type == typeof (int))
        return OpCodes.Ldind_I4;
      if (type == typeof (long))
        return OpCodes.Ldind_I8;
      return OpCodes.Ldind_Ref;
    }

    private static HarmonyArgument GetArgumentAttribute(this ParameterInfo parameter)
    {
      return ((IEnumerable<object>) parameter.GetCustomAttributes(false)).FirstOrDefault<object>((Func<object, bool>) (attr => attr is HarmonyArgument)) as HarmonyArgument;
    }

    private static HarmonyArgument[] GetArgumentAttributes(this MethodInfo method)
    {
      return ((IEnumerable<object>) method.GetCustomAttributes(false)).Where<object>((Func<object, bool>) (attr => attr is HarmonyArgument)).Cast<HarmonyArgument>().ToArray<HarmonyArgument>();
    }

    private static HarmonyArgument[] GetArgumentAttributes(this Type type)
    {
      return ((IEnumerable<object>) type.GetCustomAttributes(false)).Where<object>((Func<object, bool>) (attr => attr is HarmonyArgument)).Cast<HarmonyArgument>().ToArray<HarmonyArgument>();
    }

    private static string GetOriginalArgumentName(
      this ParameterInfo parameter,
      string[] originalParameterNames)
    {
      HarmonyArgument argumentAttribute = parameter.GetArgumentAttribute();
      if (argumentAttribute == null)
        return (string) null;
      if (!string.IsNullOrEmpty(argumentAttribute.OriginalName))
        return argumentAttribute.OriginalName;
      if (argumentAttribute.Index >= 0 && argumentAttribute.Index < originalParameterNames.Length)
        return originalParameterNames[argumentAttribute.Index];
      return (string) null;
    }

    private static string GetOriginalArgumentName(
      HarmonyArgument[] attributes,
      string name,
      string[] originalParameterNames)
    {
      if (attributes.Length == 0)
        return (string) null;
      HarmonyArgument harmonyArgument = ((IEnumerable<HarmonyArgument>) attributes).SingleOrDefault<HarmonyArgument>((Func<HarmonyArgument, bool>) (p => p.NewName == name));
      if (harmonyArgument == null)
        return (string) null;
      if (!string.IsNullOrEmpty(harmonyArgument.OriginalName))
        return harmonyArgument.OriginalName;
      if (harmonyArgument.Index >= 0 && harmonyArgument.Index < originalParameterNames.Length)
        return originalParameterNames[harmonyArgument.Index];
      return (string) null;
    }

    private static string GetOriginalArgumentName(
      this MethodInfo method,
      string[] originalParameterNames,
      string name)
    {
      string originalArgumentName1 = MethodPatcher.GetOriginalArgumentName(method.GetArgumentAttributes(), name, originalParameterNames);
      if (originalArgumentName1 != null)
        return originalArgumentName1;
      string originalArgumentName2 = MethodPatcher.GetOriginalArgumentName(method.DeclaringType.GetArgumentAttributes(), name, originalParameterNames);
      if (originalArgumentName2 != null)
        return originalArgumentName2;
      return name;
    }

    private static int GetArgumentIndex(
      MethodInfo patch,
      string[] originalParameterNames,
      ParameterInfo patchParam)
    {
      string originalArgumentName1 = patchParam.GetOriginalArgumentName(originalParameterNames);
      if (originalArgumentName1 != null)
        return Array.IndexOf<string>(originalParameterNames, originalArgumentName1);
      string name = patchParam.Name;
      string originalArgumentName2 = patch.GetOriginalArgumentName(originalParameterNames, name);
      if (originalArgumentName2 != null)
        return Array.IndexOf<string>(originalParameterNames, originalArgumentName2);
      return -1;
    }

    private static void EmitCallParameter(
      ILGenerator il,
      MethodBase original,
      MethodInfo patch,
      Dictionary<string, LocalBuilder> variables,
      bool allowFirsParamPassthrough)
    {
      bool flag1 = !original.IsStatic;
      ParameterInfo[] parameters = original.GetParameters();
      string[] array = ((IEnumerable<ParameterInfo>) parameters).Select<ParameterInfo, string>((Func<ParameterInfo, string>) (p => p.Name)).ToArray<string>();
      List<ParameterInfo> list = ((IEnumerable<ParameterInfo>) patch.GetParameters()).ToList<ParameterInfo>();
      if (allowFirsParamPassthrough && patch.ReturnType != typeof (void) && list.Count > 0 && list[0].ParameterType == patch.ReturnType)
        list.RemoveRange(0, 1);
      foreach (ParameterInfo patchParam in list)
      {
        if (patchParam.Name == MethodPatcher.ORIGINAL_METHOD_PARAM)
        {
          ConstructorInfo con = original as ConstructorInfo;
          if (con != null)
          {
            Emitter.Emit(il, OpCodes.Ldtoken, con);
            Emitter.Emit(il, OpCodes.Call, MethodPatcher.getMethodMethod);
          }
          else
          {
            MethodInfo meth = original as MethodInfo;
            if (meth != null)
            {
              Emitter.Emit(il, OpCodes.Ldtoken, meth);
              Emitter.Emit(il, OpCodes.Call, MethodPatcher.getMethodMethod);
            }
            else
              Emitter.Emit(il, OpCodes.Ldnull);
          }
        }
        else if (patchParam.Name == MethodPatcher.INSTANCE_PARAM)
        {
          if (original.IsStatic)
            Emitter.Emit(il, OpCodes.Ldnull);
          else if (patchParam.ParameterType.IsByRef)
            Emitter.Emit(il, OpCodes.Ldarga, 0);
          else
            Emitter.Emit(il, OpCodes.Ldarg_0);
        }
        else if (patchParam.Name.StartsWith(MethodPatcher.INSTANCE_FIELD_PREFIX))
        {
          string str = patchParam.Name.Substring(MethodPatcher.INSTANCE_FIELD_PREFIX.Length);
          FieldInfo field;
          if (str.All<char>(new Func<char, bool>(char.IsDigit)))
          {
            field = AccessTools.Field(original.DeclaringType, int.Parse(str));
            if (field == null)
              throw new ArgumentException("No field found at given index in class " + original.DeclaringType.FullName, str);
          }
          else
          {
            field = AccessTools.Field(original.DeclaringType, str);
            if (field == null)
              throw new ArgumentException("No such field defined in class " + original.DeclaringType.FullName, str);
          }
          if (field.IsStatic)
          {
            if (patchParam.ParameterType.IsByRef)
              Emitter.Emit(il, OpCodes.Ldsflda, field);
            else
              Emitter.Emit(il, OpCodes.Ldsfld, field);
          }
          else if (patchParam.ParameterType.IsByRef)
          {
            Emitter.Emit(il, OpCodes.Ldarg_0);
            Emitter.Emit(il, OpCodes.Ldflda, field);
          }
          else
          {
            Emitter.Emit(il, OpCodes.Ldarg_0);
            Emitter.Emit(il, OpCodes.Ldfld, field);
          }
        }
        else if (patchParam.Name == MethodPatcher.STATE_VAR)
        {
          OpCode opcode = patchParam.ParameterType.IsByRef ? OpCodes.Ldloca : OpCodes.Ldloc;
          Emitter.Emit(il, opcode, variables[patch.DeclaringType.FullName]);
        }
        else if (patchParam.Name == MethodPatcher.RESULT_VAR)
        {
          if (AccessTools.GetReturnedType(original) == typeof (void))
            throw new Exception("Cannot get result from void method " + original.FullDescription());
          OpCode opcode = patchParam.ParameterType.IsByRef ? OpCodes.Ldloca : OpCodes.Ldloc;
          Emitter.Emit(il, opcode, variables[MethodPatcher.RESULT_VAR]);
        }
        else
        {
          int result;
          if (patchParam.Name.StartsWith(MethodPatcher.PARAM_INDEX_PREFIX))
          {
            if (!int.TryParse(patchParam.Name.Substring(MethodPatcher.PARAM_INDEX_PREFIX.Length), out result))
              throw new Exception("Parameter " + patchParam.Name + " does not contain a valid index");
            if (result < 0 || result >= parameters.Length)
              throw new Exception("No parameter found at index " + (object) result);
          }
          else
          {
            result = MethodPatcher.GetArgumentIndex(patch, array, patchParam);
            if (result == -1)
              throw new Exception("Parameter \"" + patchParam.Name + "\" not found in method " + original.FullDescription());
          }
          bool flag2 = !parameters[result].IsOut && !parameters[result].ParameterType.IsByRef;
          bool flag3 = !patchParam.IsOut && !patchParam.ParameterType.IsByRef;
          int num = result + (flag1 ? 1 : 0);
          if (flag2 == flag3)
            Emitter.Emit(il, OpCodes.Ldarg, num);
          else if (flag2 && !flag3)
          {
            Emitter.Emit(il, OpCodes.Ldarga, num);
          }
          else
          {
            Emitter.Emit(il, OpCodes.Ldarg, num);
            Emitter.Emit(il, MethodPatcher.LoadIndOpCodeFor(parameters[result].ParameterType));
          }
        }
      }
    }

    private static bool AddPrefixes(
      ILGenerator il,
      MethodBase original,
      List<MethodInfo> prefixes,
      Dictionary<string, LocalBuilder> variables,
      Label label)
    {
      bool canHaveJump = false;
      prefixes.ForEach((Action<MethodInfo>) (fix =>
      {
        MethodPatcher.EmitCallParameter(il, original, fix, variables, false);
        Emitter.Emit(il, OpCodes.Call, fix);
        if (fix.ReturnType == typeof (void))
          return;
        if (fix.ReturnType != typeof (bool))
          throw new Exception("Prefix patch " + (object) fix + " has not \"bool\" or \"void\" return type: " + (object) fix.ReturnType);
        Emitter.Emit(il, OpCodes.Brfalse, label);
        canHaveJump = true;
      }));
      return canHaveJump;
    }

    private static void AddPostfixes(
      ILGenerator il,
      MethodBase original,
      List<MethodInfo> postfixes,
      Dictionary<string, LocalBuilder> variables,
      bool passthroughPatches)
    {
      postfixes.Where<MethodInfo>((Func<MethodInfo, bool>) (fix => passthroughPatches == (fix.ReturnType != typeof (void)))).Do<MethodInfo>((Action<MethodInfo>) (fix =>
      {
        MethodPatcher.EmitCallParameter(il, original, fix, variables, true);
        Emitter.Emit(il, OpCodes.Call, fix);
        if (fix.ReturnType == typeof (void))
          return;
        ParameterInfo parameterInfo = ((IEnumerable<ParameterInfo>) fix.GetParameters()).FirstOrDefault<ParameterInfo>();
        if (parameterInfo != null && fix.ReturnType == parameterInfo.ParameterType)
          return;
        if (parameterInfo != null)
          throw new Exception("Return type of postfix patch " + (object) fix + " does match type of its first parameter");
        throw new Exception("Postfix patch " + (object) fix + " must have a \"void\" return type");
      }));
    }
  }
}
