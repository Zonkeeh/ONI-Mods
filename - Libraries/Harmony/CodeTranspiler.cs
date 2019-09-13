// Decompiled with JetBrains decompiler
// Type: Harmony.CodeTranspiler
// Assembly: 0Harmony, Version=1.2.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 1B235470-4299-4E96-B8B6-361DBE3791D9
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\0Harmony.dll

using Harmony.ILCopying;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Harmony
{
  public class CodeTranspiler
  {
    private static readonly Dictionary<OpCode, OpCode> allJumpCodes = new Dictionary<OpCode, OpCode>()
    {
      {
        OpCodes.Beq_S,
        OpCodes.Beq
      },
      {
        OpCodes.Bge_S,
        OpCodes.Bge
      },
      {
        OpCodes.Bge_Un_S,
        OpCodes.Bge_Un
      },
      {
        OpCodes.Bgt_S,
        OpCodes.Bgt
      },
      {
        OpCodes.Bgt_Un_S,
        OpCodes.Bgt_Un
      },
      {
        OpCodes.Ble_S,
        OpCodes.Ble
      },
      {
        OpCodes.Ble_Un_S,
        OpCodes.Ble_Un
      },
      {
        OpCodes.Blt_S,
        OpCodes.Blt
      },
      {
        OpCodes.Blt_Un_S,
        OpCodes.Blt_Un
      },
      {
        OpCodes.Bne_Un_S,
        OpCodes.Bne_Un
      },
      {
        OpCodes.Brfalse_S,
        OpCodes.Brfalse
      },
      {
        OpCodes.Brtrue_S,
        OpCodes.Brtrue
      },
      {
        OpCodes.Br_S,
        OpCodes.Br
      },
      {
        OpCodes.Leave_S,
        OpCodes.Leave
      }
    };
    private List<MethodInfo> transpilers = new List<MethodInfo>();
    private IEnumerable<CodeInstruction> codeInstructions;

    public CodeTranspiler(List<ILInstruction> ilInstructions)
    {
      this.codeInstructions = ilInstructions.Select<ILInstruction, CodeInstruction>((Func<ILInstruction, CodeInstruction>) (ilInstruction => ilInstruction.GetCodeInstruction())).ToList<CodeInstruction>().AsEnumerable<CodeInstruction>();
    }

    public void Add(MethodInfo transpiler)
    {
      this.transpilers.Add(transpiler);
    }

    [UpgradeToLatestVersion(1)]
    public static object ConvertInstruction(
      Type type,
      object op,
      out Dictionary<string, object> unassigned)
    {
      Dictionary<string, object> nonExisting = new Dictionary<string, object>();
      object obj1 = AccessTools.MakeDeepCopy(op, type, (Func<string, Traverse, Traverse, object>) ((namePath, trvSrc, trvDest) =>
      {
        object obj2 = trvSrc.GetValue();
        if (!trvDest.FieldExists())
        {
          nonExisting[namePath] = obj2;
          return (object) null;
        }
        if (namePath == "opcode")
          return (object) CodeTranspiler.ReplaceShortJumps((OpCode) obj2);
        return obj2;
      }), "");
      unassigned = nonExisting;
      return obj1;
    }

    public static bool ShouldAddExceptionInfo(
      object op,
      int opIndex,
      List<object> originalInstructions,
      List<object> newInstructions,
      Dictionary<object, Dictionary<string, object>> unassignedValues)
    {
      int count = originalInstructions.IndexOf(op);
      if (count == -1)
        return false;
      Dictionary<string, object> unassigned = (Dictionary<string, object>) null;
      object blocksObject;
      if (!unassignedValues.TryGetValue(op, out unassigned) || !unassigned.TryGetValue("blocks", out blocksObject))
        return false;
      List<ExceptionBlock> blocks = blocksObject as List<ExceptionBlock>;
      if (newInstructions.Count<object>((Func<object, bool>) (instr => instr == op)) <= 1)
        return true;
      ExceptionBlock exceptionBlock1 = blocks.FirstOrDefault<ExceptionBlock>((Func<ExceptionBlock, bool>) (block => block.blockType != ExceptionBlockType.EndExceptionBlock));
      ExceptionBlock exceptionBlock2 = blocks.FirstOrDefault<ExceptionBlock>((Func<ExceptionBlock, bool>) (block => block.blockType == ExceptionBlockType.EndExceptionBlock));
      if (exceptionBlock1 != null && exceptionBlock2 == null)
      {
        object obj1 = originalInstructions.Skip<object>(count + 1).FirstOrDefault<object>((Func<object, bool>) (instr =>
        {
          if (!unassignedValues.TryGetValue(instr, out unassigned) || !unassigned.TryGetValue("blocks", out blocksObject))
            return false;
          blocks = blocksObject as List<ExceptionBlock>;
          return blocks.Count<ExceptionBlock>() > 0;
        }));
        if (obj1 != null)
        {
          int num1 = count + 1;
          int num2 = num1 + originalInstructions.Skip<object>(num1).ToList<object>().IndexOf(obj1) - 1;
          IEnumerable<object> first = originalInstructions.GetRange(num1, num2 - num1).Intersect<object>((IEnumerable<object>) newInstructions);
          object obj2 = newInstructions.Skip<object>(opIndex + 1).FirstOrDefault<object>((Func<object, bool>) (instr =>
          {
            if (!unassignedValues.TryGetValue(instr, out unassigned) || !unassigned.TryGetValue("blocks", out blocksObject))
              return false;
            blocks = blocksObject as List<ExceptionBlock>;
            return blocks.Count<ExceptionBlock>() > 0;
          }));
          if (obj2 != null)
          {
            int index = opIndex + 1;
            int num3 = index + newInstructions.Skip<object>(opIndex + 1).ToList<object>().IndexOf(obj2) - 1;
            List<object> range = newInstructions.GetRange(index, num3 - index);
            return first.Except<object>((IEnumerable<object>) range).ToList<object>().Count<object>() == 0;
          }
        }
      }
      if (exceptionBlock1 == null && exceptionBlock2 != null)
      {
        object obj1 = originalInstructions.GetRange(0, count).LastOrDefault<object>((Func<object, bool>) (instr =>
        {
          if (!unassignedValues.TryGetValue(instr, out unassigned) || !unassigned.TryGetValue("blocks", out blocksObject))
            return false;
          blocks = blocksObject as List<ExceptionBlock>;
          return blocks.Count<ExceptionBlock>() > 0;
        }));
        if (obj1 != null)
        {
          int index1 = originalInstructions.GetRange(0, count).LastIndexOf(obj1);
          int num1 = count;
          IEnumerable<object> first = originalInstructions.GetRange(index1, num1 - index1).Intersect<object>((IEnumerable<object>) newInstructions);
          object obj2 = newInstructions.GetRange(0, opIndex).LastOrDefault<object>((Func<object, bool>) (instr =>
          {
            if (!unassignedValues.TryGetValue(instr, out unassigned) || !unassigned.TryGetValue("blocks", out blocksObject))
              return false;
            blocks = blocksObject as List<ExceptionBlock>;
            return blocks.Count<ExceptionBlock>() > 0;
          }));
          if (obj2 != null)
          {
            int index2 = newInstructions.GetRange(0, opIndex).LastIndexOf(obj2);
            int num2 = opIndex;
            List<object> range = newInstructions.GetRange(index2, num2 - index2);
            return first.Except<object>((IEnumerable<object>) range).Count<object>() == 0;
          }
        }
      }
      return true;
    }

    public static IEnumerable ConvertInstructionsAndUnassignedValues(
      Type type,
      IEnumerable enumerable,
      out Dictionary<object, Dictionary<string, object>> unassignedValues)
    {
      Assembly assembly = type.GetGenericTypeDefinition().Assembly;
      Type type1 = assembly.GetType(typeof (List<>).FullName);
      Type genericArgument = type.GetGenericArguments()[0];
      object instance = Activator.CreateInstance(assembly.GetType(type1.MakeGenericType(genericArgument).FullName));
      MethodInfo method = instance.GetType().GetMethod("Add");
      unassignedValues = new Dictionary<object, Dictionary<string, object>>();
      foreach (object op in enumerable)
      {
        Dictionary<string, object> unassigned;
        object key = CodeTranspiler.ConvertInstruction(genericArgument, op, out unassigned);
        unassignedValues.Add(key, unassigned);
        method.Invoke(instance, new object[1]{ key });
      }
      return instance as IEnumerable;
    }

    [UpgradeToLatestVersion(1)]
    public static IEnumerable ConvertToOurInstructions(
      IEnumerable instructions,
      List<object> originalInstructions,
      Dictionary<object, Dictionary<string, object>> unassignedValues)
    {
      Type codeInstructionType = ((IEnumerable<StackFrame>) new StackTrace().GetFrames()).Select<StackFrame, MethodBase>((Func<StackFrame, MethodBase>) (frame => frame.GetMethod())).OfType<MethodInfo>().Select<MethodInfo, Type>((Func<MethodInfo, Type>) (method =>
      {
        Type returnType = method.ReturnType;
        if (!returnType.IsGenericType)
          return (Type) null;
        Type[] genericArguments = returnType.GetGenericArguments();
        if (genericArguments.Length != 1)
          return (Type) null;
        Type type = genericArguments[0];
        return type.FullName == typeof (CodeInstruction).FullName ? type : (Type) null;
      })).Where<Type>((Func<Type, bool>) (type => type != null)).First<Type>();
      List<object> newInstructions = instructions.Cast<object>().ToList<object>();
      int index = -1;
      foreach (object obj in newInstructions)
      {
        object op = obj;
        ++index;
        object elementTo = AccessTools.MakeDeepCopy(op, codeInstructionType, (Func<string, Traverse, Traverse, object>) null, "");
        Dictionary<string, object> fields;
        if (unassignedValues.TryGetValue(op, out fields))
        {
          bool addExceptionInfo = CodeTranspiler.ShouldAddExceptionInfo(op, index, originalInstructions, newInstructions, unassignedValues);
          Traverse trv = Traverse.Create(elementTo);
          foreach (KeyValuePair<string, object> keyValuePair in fields)
          {
            KeyValuePair<string, object> field = keyValuePair;
            if (addExceptionInfo || field.Key != "blocks")
              trv.Field(field.Key).SetValue(field.Value);
            field = new KeyValuePair<string, object>();
          }
          trv = (Traverse) null;
        }
        yield return elementTo;
        elementTo = (object) null;
        fields = (Dictionary<string, object>) null;
        op = (object) null;
      }
    }

    public static IEnumerable ConvertToGeneralInstructions(
      MethodInfo transpiler,
      IEnumerable enumerable,
      out Dictionary<object, Dictionary<string, object>> unassignedValues)
    {
      return CodeTranspiler.ConvertInstructionsAndUnassignedValues(((IEnumerable<ParameterInfo>) transpiler.GetParameters()).Select<ParameterInfo, Type>((Func<ParameterInfo, Type>) (p => p.ParameterType)).FirstOrDefault<Type>((Func<Type, bool>) (t =>
      {
        if (t.IsGenericType)
          return t.GetGenericTypeDefinition().Name.StartsWith("IEnumerable");
        return false;
      })), enumerable, out unassignedValues);
    }

    public static List<object> GetTranspilerCallParameters(
      ILGenerator generator,
      MethodInfo transpiler,
      MethodBase method,
      IEnumerable instructions)
    {
      List<object> parameter = new List<object>();
      ((IEnumerable<ParameterInfo>) transpiler.GetParameters()).Select<ParameterInfo, Type>((Func<ParameterInfo, Type>) (param => param.ParameterType)).Do<Type>((Action<Type>) (type =>
      {
        if (type.IsAssignableFrom(typeof (ILGenerator)))
          parameter.Add((object) generator);
        else if (type.IsAssignableFrom(typeof (MethodBase)))
          parameter.Add((object) method);
        else
          parameter.Add((object) instructions);
      }));
      return parameter;
    }

    public List<CodeInstruction> GetResult(
      ILGenerator generator,
      MethodBase method)
    {
      IEnumerable instructions = (IEnumerable) this.codeInstructions;
      this.transpilers.ForEach((Action<MethodInfo>) (transpiler =>
      {
        Dictionary<object, Dictionary<string, object>> unassignedValues;
        instructions = CodeTranspiler.ConvertToGeneralInstructions(transpiler, instructions, out unassignedValues);
        List<object> originalInstructions = new List<object>();
        originalInstructions.AddRange(instructions.Cast<object>());
        List<object> transpilerCallParameters = CodeTranspiler.GetTranspilerCallParameters(generator, transpiler, method, instructions);
        instructions = transpiler.Invoke((object) null, transpilerCallParameters.ToArray()) as IEnumerable;
        instructions = CodeTranspiler.ConvertToOurInstructions(instructions, originalInstructions, unassignedValues);
      }));
      return instructions.Cast<CodeInstruction>().ToList<CodeInstruction>();
    }

    private static OpCode ReplaceShortJumps(OpCode opcode)
    {
      foreach (KeyValuePair<OpCode, OpCode> allJumpCode in CodeTranspiler.allJumpCodes)
      {
        if (opcode == allJumpCode.Key)
          return allJumpCode.Value;
      }
      return opcode;
    }
  }
}
