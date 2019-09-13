// Decompiled with JetBrains decompiler
// Type: Harmony.ILCopying.Emitter
// Assembly: 0Harmony, Version=1.2.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 1B235470-4299-4E96-B8B6-361DBE3791D9
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\0Harmony.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;

namespace Harmony.ILCopying
{
  public static class Emitter
  {
    private static readonly GetterHandler codeLenGetter = FastAccess.CreateFieldGetter(typeof (ILGenerator), "code_len", "m_length");
    private static readonly GetterHandler localsGetter = FastAccess.CreateFieldGetter(typeof (ILGenerator), "locals");
    private static readonly GetterHandler localCountGetter = FastAccess.CreateFieldGetter(typeof (ILGenerator), "m_localCount");

    public static string CodePos(ILGenerator il)
    {
      return string.Format("L_{0:x4}: ", (object) (int) Emitter.codeLenGetter((object) il));
    }

    public static void LogIL(ILGenerator il, OpCode opCode, object argument)
    {
      if (!HarmonyInstance.DEBUG)
        return;
      string str1 = Emitter.FormatArgument(argument);
      string str2 = str1.Length > 0 ? " " : "";
      FileLog.LogBuffered(string.Format("{0}{1}{2}{3}", (object) Emitter.CodePos(il), (object) opCode, (object) str2, (object) str1));
    }

    public static void LogLocalVariable(ILGenerator il, LocalBuilder variable)
    {
      if (!HarmonyInstance.DEBUG)
        return;
      LocalBuilder[] localBuilderArray = Emitter.localsGetter != null ? (LocalBuilder[]) Emitter.localsGetter((object) il) : (LocalBuilder[]) null;
      int num = localBuilderArray == null || (uint) localBuilderArray.Length <= 0U ? (int) Emitter.localCountGetter((object) il) : localBuilderArray.Length;
      FileLog.LogBuffered(string.Format("{0}Local var {1}: {2}{3}", (object) Emitter.CodePos(il), (object) (num - 1), (object) variable.LocalType.FullName, variable.IsPinned ? (object) "(pinned)" : (object) ""));
    }

    public static string FormatArgument(object argument)
    {
      if (argument == null)
        return "NULL";
      Type type = argument.GetType();
      if (type == typeof (string))
        return "\"" + argument + "\"";
      if (type == typeof (Label))
        return "Label" + (object) ((Label) argument).GetHashCode();
      if (type == typeof (Label[]))
        return "Labels" + string.Join(",", ((IEnumerable<Label>) (Label[]) argument).Select<Label, string>((Func<Label, string>) (l => l.GetHashCode().ToString())).ToArray<string>());
      if (type != typeof (LocalBuilder))
        return argument.ToString().Trim();
      return ((LocalVariableInfo) argument).LocalIndex.ToString() + " (" + (object) ((LocalVariableInfo) argument).LocalType + ")";
    }

    public static void MarkLabel(ILGenerator il, Label label)
    {
      if (HarmonyInstance.DEBUG)
        FileLog.LogBuffered(Emitter.CodePos(il) + Emitter.FormatArgument((object) label));
      il.MarkLabel(label);
    }

    public static void MarkBlockBefore(ILGenerator il, ExceptionBlock block, out Label? label)
    {
      label = new Label?();
      switch (block.blockType)
      {
        case ExceptionBlockType.BeginExceptionBlock:
          if (HarmonyInstance.DEBUG)
          {
            FileLog.LogBuffered(".try");
            FileLog.LogBuffered("{");
            FileLog.ChangeIndent(1);
          }
          label = new Label?(il.BeginExceptionBlock());
          break;
        case ExceptionBlockType.BeginCatchBlock:
          if (HarmonyInstance.DEBUG)
          {
            Emitter.LogIL(il, OpCodes.Leave, (object) new LeaveTry());
            FileLog.ChangeIndent(-1);
            FileLog.LogBuffered("} // end try");
            FileLog.LogBuffered(".catch " + (object) block.catchType);
            FileLog.LogBuffered("{");
            FileLog.ChangeIndent(1);
          }
          il.BeginCatchBlock(block.catchType);
          break;
        case ExceptionBlockType.BeginExceptFilterBlock:
          if (HarmonyInstance.DEBUG)
          {
            Emitter.LogIL(il, OpCodes.Leave, (object) new LeaveTry());
            FileLog.ChangeIndent(-1);
            FileLog.LogBuffered("} // end try");
            FileLog.LogBuffered(".filter");
            FileLog.LogBuffered("{");
            FileLog.ChangeIndent(1);
          }
          il.BeginExceptFilterBlock();
          break;
        case ExceptionBlockType.BeginFaultBlock:
          if (HarmonyInstance.DEBUG)
          {
            Emitter.LogIL(il, OpCodes.Leave, (object) new LeaveTry());
            FileLog.ChangeIndent(-1);
            FileLog.LogBuffered("} // end try");
            FileLog.LogBuffered(".fault");
            FileLog.LogBuffered("{");
            FileLog.ChangeIndent(1);
          }
          il.BeginFaultBlock();
          break;
        case ExceptionBlockType.BeginFinallyBlock:
          if (HarmonyInstance.DEBUG)
          {
            Emitter.LogIL(il, OpCodes.Leave, (object) new LeaveTry());
            FileLog.ChangeIndent(-1);
            FileLog.LogBuffered("} // end try");
            FileLog.LogBuffered(".finally");
            FileLog.LogBuffered("{");
            FileLog.ChangeIndent(1);
          }
          il.BeginFinallyBlock();
          break;
      }
    }

    public static void MarkBlockAfter(ILGenerator il, ExceptionBlock block)
    {
      if (block.blockType != ExceptionBlockType.EndExceptionBlock)
        return;
      if (HarmonyInstance.DEBUG)
      {
        Emitter.LogIL(il, OpCodes.Leave, (object) new LeaveTry());
        FileLog.ChangeIndent(-1);
        FileLog.LogBuffered("} // end handler");
      }
      il.EndExceptionBlock();
    }

    public static void Emit(ILGenerator il, OpCode opcode)
    {
      if (HarmonyInstance.DEBUG)
        FileLog.LogBuffered(Emitter.CodePos(il) + (object) opcode);
      il.Emit(opcode);
    }

    public static void Emit(ILGenerator il, OpCode opcode, LocalBuilder local)
    {
      Emitter.LogIL(il, opcode, (object) local);
      il.Emit(opcode, local);
    }

    public static void Emit(ILGenerator il, OpCode opcode, FieldInfo field)
    {
      Emitter.LogIL(il, opcode, (object) field);
      il.Emit(opcode, field);
    }

    public static void Emit(ILGenerator il, OpCode opcode, Label[] labels)
    {
      Emitter.LogIL(il, opcode, (object) labels);
      il.Emit(opcode, labels);
    }

    public static void Emit(ILGenerator il, OpCode opcode, Label label)
    {
      Emitter.LogIL(il, opcode, (object) label);
      il.Emit(opcode, label);
    }

    public static void Emit(ILGenerator il, OpCode opcode, string str)
    {
      Emitter.LogIL(il, opcode, (object) str);
      il.Emit(opcode, str);
    }

    public static void Emit(ILGenerator il, OpCode opcode, float arg)
    {
      Emitter.LogIL(il, opcode, (object) arg);
      il.Emit(opcode, arg);
    }

    public static void Emit(ILGenerator il, OpCode opcode, byte arg)
    {
      Emitter.LogIL(il, opcode, (object) arg);
      il.Emit(opcode, arg);
    }

    public static void Emit(ILGenerator il, OpCode opcode, sbyte arg)
    {
      Emitter.LogIL(il, opcode, (object) arg);
      il.Emit(opcode, arg);
    }

    public static void Emit(ILGenerator il, OpCode opcode, double arg)
    {
      Emitter.LogIL(il, opcode, (object) arg);
      il.Emit(opcode, arg);
    }

    public static void Emit(ILGenerator il, OpCode opcode, int arg)
    {
      Emitter.LogIL(il, opcode, (object) arg);
      il.Emit(opcode, arg);
    }

    public static void Emit(ILGenerator il, OpCode opcode, MethodInfo meth)
    {
      Emitter.LogIL(il, opcode, (object) meth);
      il.Emit(opcode, meth);
    }

    public static void Emit(ILGenerator il, OpCode opcode, short arg)
    {
      Emitter.LogIL(il, opcode, (object) arg);
      il.Emit(opcode, arg);
    }

    public static void Emit(ILGenerator il, OpCode opcode, SignatureHelper signature)
    {
      Emitter.LogIL(il, opcode, (object) signature);
      il.Emit(opcode, signature);
    }

    public static void Emit(ILGenerator il, OpCode opcode, ConstructorInfo con)
    {
      Emitter.LogIL(il, opcode, (object) con);
      il.Emit(opcode, con);
    }

    public static void Emit(ILGenerator il, OpCode opcode, Type cls)
    {
      Emitter.LogIL(il, opcode, (object) cls);
      il.Emit(opcode, cls);
    }

    public static void Emit(ILGenerator il, OpCode opcode, long arg)
    {
      Emitter.LogIL(il, opcode, (object) arg);
      il.Emit(opcode, arg);
    }

    public static void EmitCall(
      ILGenerator il,
      OpCode opcode,
      MethodInfo methodInfo,
      Type[] optionalParameterTypes)
    {
      if (HarmonyInstance.DEBUG)
        FileLog.LogBuffered(string.Format("{0}Call {1} {2} {3}", (object) Emitter.CodePos(il), (object) opcode, (object) methodInfo, (object) optionalParameterTypes));
      il.EmitCall(opcode, methodInfo, optionalParameterTypes);
    }

    public static void EmitCalli(
      ILGenerator il,
      OpCode opcode,
      CallingConvention unmanagedCallConv,
      Type returnType,
      Type[] parameterTypes)
    {
      if (HarmonyInstance.DEBUG)
        FileLog.LogBuffered(string.Format("{0}Calli {1} {2} {3} {4}", (object) Emitter.CodePos(il), (object) opcode, (object) unmanagedCallConv, (object) returnType, (object) parameterTypes));
      il.EmitCalli(opcode, unmanagedCallConv, returnType, parameterTypes);
    }

    public static void EmitCalli(
      ILGenerator il,
      OpCode opcode,
      CallingConventions callingConvention,
      Type returnType,
      Type[] parameterTypes,
      Type[] optionalParameterTypes)
    {
      if (HarmonyInstance.DEBUG)
        FileLog.LogBuffered(string.Format("{0}Calli {1} {2} {3} {4} {5}", (object) Emitter.CodePos(il), (object) opcode, (object) callingConvention, (object) returnType, (object) parameterTypes, (object) optionalParameterTypes));
      il.EmitCalli(opcode, callingConvention, returnType, parameterTypes, optionalParameterTypes);
    }
  }
}
