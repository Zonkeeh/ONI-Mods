// Decompiled with JetBrains decompiler
// Type: Harmony.ILCopying.MethodBodyReader
// Assembly: 0Harmony, Version=1.2.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 1B235470-4299-4E96-B8B6-361DBE3791D9
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\0Harmony.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace Harmony.ILCopying
{
  public class MethodBodyReader
  {
    private static Dictionary<OpCode, OpCode> shortJumps = new Dictionary<OpCode, OpCode>()
    {
      {
        OpCodes.Leave_S,
        OpCodes.Leave
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
        OpCodes.Beq_S,
        OpCodes.Beq
      },
      {
        OpCodes.Bge_S,
        OpCodes.Bge
      },
      {
        OpCodes.Bgt_S,
        OpCodes.Bgt
      },
      {
        OpCodes.Ble_S,
        OpCodes.Ble
      },
      {
        OpCodes.Blt_S,
        OpCodes.Blt
      },
      {
        OpCodes.Bne_Un_S,
        OpCodes.Bne_Un
      },
      {
        OpCodes.Bge_Un_S,
        OpCodes.Bge_Un
      },
      {
        OpCodes.Bgt_Un_S,
        OpCodes.Bgt_Un
      },
      {
        OpCodes.Ble_Un_S,
        OpCodes.Ble_Un
      },
      {
        OpCodes.Br_S,
        OpCodes.Br
      },
      {
        OpCodes.Blt_Un_S,
        OpCodes.Blt_Un
      }
    };
    private static readonly OpCode[] one_byte_opcodes = new OpCode[225];
    private static readonly OpCode[] two_bytes_opcodes = new OpCode[31];
    private readonly ILGenerator generator;
    private readonly MethodBase method;
    private readonly Module module;
    private readonly Type[] typeArguments;
    private readonly Type[] methodArguments;
    private readonly ByteBuffer ilBytes;
    private readonly ParameterInfo this_parameter;
    private readonly ParameterInfo[] parameters;
    private readonly IList<LocalVariableInfo> locals;
    private readonly IList<ExceptionHandlingClause> exceptions;
    private List<ILInstruction> ilInstructions;
    private LocalBuilder[] variables;
    private static readonly Dictionary<Type, MethodInfo> emitMethods;

    public static List<ILInstruction> GetInstructions(
      ILGenerator generator,
      MethodBase method)
    {
      if (method == null)
        throw new ArgumentNullException("Method cannot be null");
      MethodBodyReader methodBodyReader = new MethodBodyReader(method, generator);
      methodBodyReader.DeclareVariables((LocalBuilder[]) null);
      methodBodyReader.ReadInstructions();
      return methodBodyReader.ilInstructions;
    }

    public MethodBodyReader(MethodBase method, ILGenerator generator)
    {
      this.generator = generator;
      this.method = method;
      this.module = method.Module;
      MethodBody methodBody = method.GetMethodBody();
      if (methodBody == null)
        throw new ArgumentException("Method " + method.FullDescription() + " has no body");
      byte[] ilAsByteArray = methodBody.GetILAsByteArray();
      if (ilAsByteArray == null)
        throw new ArgumentException("Can not get IL bytes of method " + method.FullDescription());
      this.ilBytes = new ByteBuffer(ilAsByteArray);
      this.ilInstructions = new List<ILInstruction>((ilAsByteArray.Length + 1) / 2);
      Type declaringType = method.DeclaringType;
      if (declaringType.IsGenericType)
      {
        try
        {
          this.typeArguments = declaringType.GetGenericArguments();
        }
        catch
        {
          this.typeArguments = (Type[]) null;
        }
      }
      if (method.IsGenericMethod)
      {
        try
        {
          this.methodArguments = method.GetGenericArguments();
        }
        catch
        {
          this.methodArguments = (Type[]) null;
        }
      }
      if (!method.IsStatic)
        this.this_parameter = (ParameterInfo) new MethodBodyReader.ThisParameter(method);
      this.parameters = method.GetParameters();
      this.locals = methodBody.LocalVariables;
      this.exceptions = methodBody.ExceptionHandlingClauses;
    }

    public void ReadInstructions()
    {
      while (this.ilBytes.position < this.ilBytes.buffer.Length)
      {
        int position = this.ilBytes.position;
        ILInstruction instruction = new ILInstruction(this.ReadOpCode(), (object) null)
        {
          offset = position
        };
        this.ReadOperand(instruction);
        this.ilInstructions.Add(instruction);
      }
      this.ResolveBranches();
      this.ParseExceptions();
    }

    public void DeclareVariables(LocalBuilder[] existingVariables)
    {
      if (this.generator == null)
        return;
      if (existingVariables != null)
        this.variables = existingVariables;
      else
        this.variables = this.locals.Select<LocalVariableInfo, LocalBuilder>((Func<LocalVariableInfo, LocalBuilder>) (lvi => this.generator.DeclareLocal(lvi.LocalType, lvi.IsPinned))).ToArray<LocalBuilder>();
    }

    private void ResolveBranches()
    {
      foreach (ILInstruction ilInstruction in this.ilInstructions)
      {
        switch (ilInstruction.opcode.OperandType)
        {
          case OperandType.InlineBrTarget:
          case OperandType.ShortInlineBrTarget:
            ilInstruction.operand = (object) this.GetInstruction((int) ilInstruction.operand, false);
            break;
          case OperandType.InlineSwitch:
            int[] operand = (int[]) ilInstruction.operand;
            ILInstruction[] ilInstructionArray = new ILInstruction[operand.Length];
            for (int index = 0; index < operand.Length; ++index)
              ilInstructionArray[index] = this.GetInstruction(operand[index], false);
            ilInstruction.operand = (object) ilInstructionArray;
            break;
        }
      }
    }

    private void ParseExceptions()
    {
      foreach (ExceptionHandlingClause exception in (IEnumerable<ExceptionHandlingClause>) this.exceptions)
      {
        int tryOffset = exception.TryOffset;
        int num = exception.TryOffset + exception.TryLength - 1;
        int handlerOffset = exception.HandlerOffset;
        int offset = exception.HandlerOffset + exception.HandlerLength - 1;
        this.GetInstruction(tryOffset, false).blocks.Add(new ExceptionBlock(ExceptionBlockType.BeginExceptionBlock, (Type) null));
        this.GetInstruction(offset, true).blocks.Add(new ExceptionBlock(ExceptionBlockType.EndExceptionBlock, (Type) null));
        switch (exception.Flags)
        {
          case ExceptionHandlingClauseOptions.Clause:
            this.GetInstruction(handlerOffset, false).blocks.Add(new ExceptionBlock(ExceptionBlockType.BeginCatchBlock, exception.CatchType));
            break;
          case ExceptionHandlingClauseOptions.Filter:
            this.GetInstruction(exception.FilterOffset, false).blocks.Add(new ExceptionBlock(ExceptionBlockType.BeginExceptFilterBlock, (Type) null));
            break;
          case ExceptionHandlingClauseOptions.Finally:
            this.GetInstruction(handlerOffset, false).blocks.Add(new ExceptionBlock(ExceptionBlockType.BeginFinallyBlock, (Type) null));
            break;
          case ExceptionHandlingClauseOptions.Fault:
            this.GetInstruction(handlerOffset, false).blocks.Add(new ExceptionBlock(ExceptionBlockType.BeginFaultBlock, (Type) null));
            break;
        }
      }
    }

    public void FinalizeILCodes(
      List<MethodInfo> transpilers,
      List<Label> endLabels,
      List<ExceptionBlock> endBlocks)
    {
      if (this.generator == null)
        return;
      foreach (ILInstruction ilInstruction1 in this.ilInstructions)
      {
        switch (ilInstruction1.opcode.OperandType)
        {
          case OperandType.InlineBrTarget:
          case OperandType.ShortInlineBrTarget:
            ILInstruction operand1 = ilInstruction1.operand as ILInstruction;
            if (operand1 != null)
            {
              Label label = this.generator.DefineLabel();
              operand1.labels.Add(label);
              ilInstruction1.argument = (object) label;
              break;
            }
            break;
          case OperandType.InlineSwitch:
            ILInstruction[] operand2 = ilInstruction1.operand as ILInstruction[];
            if (operand2 != null)
            {
              List<Label> labelList = new List<Label>();
              foreach (ILInstruction ilInstruction2 in operand2)
              {
                Label label = this.generator.DefineLabel();
                ilInstruction2.labels.Add(label);
                labelList.Add(label);
              }
              ilInstruction1.argument = (object) labelList.ToArray();
              break;
            }
            break;
        }
      }
      CodeTranspiler codeTranspiler = new CodeTranspiler(this.ilInstructions);
      transpilers.Do<MethodInfo>((Action<MethodInfo>) (transpiler => codeTranspiler.Add(transpiler)));
      List<CodeInstruction> result = codeTranspiler.GetResult(this.generator, this.method);
      while (true)
      {
        CodeInstruction codeInstruction = result.LastOrDefault<CodeInstruction>();
        if (codeInstruction != null && !(codeInstruction.opcode != OpCodes.Ret))
        {
          endLabels.AddRange((IEnumerable<Label>) codeInstruction.labels);
          result.RemoveAt(result.Count - 1);
        }
        else
          break;
      }
      int idx = 0;
      result.Do<CodeInstruction>((Action<CodeInstruction>) (codeInstruction =>
      {
        codeInstruction.labels.Do<Label>((Action<Label>) (label => Emitter.MarkLabel(this.generator, label)));
        Label? label1;
        codeInstruction.blocks.Do<ExceptionBlock>((Action<ExceptionBlock>) (block => Emitter.MarkBlockBefore(this.generator, block, out label1)));
        OpCode opCode1 = codeInstruction.opcode;
        object obj = codeInstruction.operand;
        if (opCode1 == OpCodes.Ret)
        {
          Label label2 = this.generator.DefineLabel();
          opCode1 = OpCodes.Br;
          obj = (object) label2;
          endLabels.Add(label2);
        }
        OpCode opCode2;
        if (MethodBodyReader.shortJumps.TryGetValue(opCode1, out opCode2))
          opCode1 = opCode2;
        if (true)
        {
          switch (opCode1.OperandType)
          {
            case OperandType.InlineNone:
              Emitter.Emit(this.generator, opCode1);
              break;
            case OperandType.InlineSig:
              if (obj == null)
                throw new Exception("Wrong null argument: " + (object) codeInstruction);
              if (!(obj is int))
                throw new Exception("Wrong Emit argument type " + (object) obj.GetType() + " in " + (object) codeInstruction);
              Emitter.Emit(this.generator, opCode1, (int) obj);
              break;
            default:
              if (obj == null)
                throw new Exception("Wrong null argument: " + (object) codeInstruction);
              MethodInfo methodInfo = this.EmitMethodForType(obj.GetType());
              if (methodInfo == null)
                throw new Exception("Unknown Emit argument type " + (object) obj.GetType() + " in " + (object) codeInstruction);
              if (HarmonyInstance.DEBUG)
                FileLog.LogBuffered(Emitter.CodePos(this.generator) + (object) opCode1 + " " + Emitter.FormatArgument(obj));
              methodInfo.Invoke((object) this.generator, new object[2]
              {
                (object) opCode1,
                obj
              });
              break;
          }
        }
        codeInstruction.blocks.Do<ExceptionBlock>((Action<ExceptionBlock>) (block => Emitter.MarkBlockAfter(this.generator, block)));
        ++idx;
      }));
    }

    private static void GetMemberInfoValue(MemberInfo info, out object result)
    {
      result = (object) null;
      switch (info.MemberType)
      {
        case MemberTypes.Constructor:
          result = (object) (ConstructorInfo) info;
          break;
        case MemberTypes.Event:
          result = (object) (EventInfo) info;
          break;
        case MemberTypes.Field:
          result = (object) (FieldInfo) info;
          break;
        case MemberTypes.Method:
          result = (object) (MethodInfo) info;
          break;
        case MemberTypes.Property:
          result = (object) (PropertyInfo) info;
          break;
        case MemberTypes.TypeInfo:
        case MemberTypes.NestedType:
          result = (object) (Type) info;
          break;
      }
    }

    private void ReadOperand(ILInstruction instruction)
    {
      switch (instruction.opcode.OperandType)
      {
        case OperandType.InlineBrTarget:
          int num1 = this.ilBytes.ReadInt32();
          instruction.operand = (object) (num1 + this.ilBytes.position);
          break;
        case OperandType.InlineField:
          int metadataToken1 = this.ilBytes.ReadInt32();
          instruction.operand = (object) this.module.ResolveField(metadataToken1, this.typeArguments, this.methodArguments);
          instruction.argument = (object) (FieldInfo) instruction.operand;
          break;
        case OperandType.InlineI:
          int num2 = this.ilBytes.ReadInt32();
          instruction.operand = (object) num2;
          instruction.argument = (object) (int) instruction.operand;
          break;
        case OperandType.InlineI8:
          long num3 = this.ilBytes.ReadInt64();
          instruction.operand = (object) num3;
          instruction.argument = (object) (long) instruction.operand;
          break;
        case OperandType.InlineMethod:
          int metadataToken2 = this.ilBytes.ReadInt32();
          instruction.operand = (object) this.module.ResolveMethod(metadataToken2, this.typeArguments, this.methodArguments);
          if (instruction.operand is ConstructorInfo)
          {
            instruction.argument = (object) (ConstructorInfo) instruction.operand;
            break;
          }
          instruction.argument = (object) (MethodInfo) instruction.operand;
          break;
        case OperandType.InlineNone:
          instruction.argument = (object) null;
          break;
        case OperandType.InlineR:
          double num4 = this.ilBytes.ReadDouble();
          instruction.operand = (object) num4;
          instruction.argument = (object) (double) instruction.operand;
          break;
        case OperandType.InlineSig:
          byte[] numArray1 = this.module.ResolveSignature(this.ilBytes.ReadInt32());
          instruction.operand = (object) numArray1;
          instruction.argument = (object) numArray1;
          Debugger.Log(0, "TEST", "METHOD " + this.method.FullDescription() + "\n");
          Debugger.Log(0, "TEST", "Signature = " + ((IEnumerable<byte>) numArray1).Select<byte, string>((Func<byte, string>) (b => string.Format("0x{0:x02}", (object) b))).Aggregate<string>((Func<string, string, string>) ((a, b) => a + " " + b)) + "\n");
          Debugger.Break();
          break;
        case OperandType.InlineString:
          int metadataToken3 = this.ilBytes.ReadInt32();
          instruction.operand = (object) this.module.ResolveString(metadataToken3);
          instruction.argument = (object) (string) instruction.operand;
          break;
        case OperandType.InlineSwitch:
          int length = this.ilBytes.ReadInt32();
          int num5 = this.ilBytes.position + 4 * length;
          int[] numArray2 = new int[length];
          for (int index = 0; index < length; ++index)
            numArray2[index] = this.ilBytes.ReadInt32() + num5;
          instruction.operand = (object) numArray2;
          break;
        case OperandType.InlineTok:
          int metadataToken4 = this.ilBytes.ReadInt32();
          instruction.operand = (object) this.module.ResolveMember(metadataToken4, this.typeArguments, this.methodArguments);
          MethodBodyReader.GetMemberInfoValue((MemberInfo) instruction.operand, out instruction.argument);
          break;
        case OperandType.InlineType:
          int metadataToken5 = this.ilBytes.ReadInt32();
          instruction.operand = (object) this.module.ResolveType(metadataToken5, this.typeArguments, this.methodArguments);
          instruction.argument = (object) (Type) instruction.operand;
          break;
        case OperandType.InlineVar:
          short num6 = this.ilBytes.ReadInt16();
          if (MethodBodyReader.TargetsLocalVariable(instruction.opcode))
          {
            LocalVariableInfo localVariable = this.GetLocalVariable((int) num6);
            if (localVariable == null)
            {
              instruction.argument = (object) num6;
              break;
            }
            instruction.operand = (object) localVariable;
            instruction.argument = (object) this.variables[localVariable.LocalIndex];
            break;
          }
          instruction.operand = (object) this.GetParameter((int) num6);
          instruction.argument = (object) num6;
          break;
        case OperandType.ShortInlineBrTarget:
          sbyte num7 = (sbyte) this.ilBytes.ReadByte();
          instruction.operand = (object) ((int) num7 + this.ilBytes.position);
          break;
        case OperandType.ShortInlineI:
          if (instruction.opcode == OpCodes.Ldc_I4_S)
          {
            sbyte num8 = (sbyte) this.ilBytes.ReadByte();
            instruction.operand = (object) num8;
            instruction.argument = (object) (sbyte) instruction.operand;
            break;
          }
          byte num9 = this.ilBytes.ReadByte();
          instruction.operand = (object) num9;
          instruction.argument = (object) (byte) instruction.operand;
          break;
        case OperandType.ShortInlineR:
          float num10 = this.ilBytes.ReadSingle();
          instruction.operand = (object) num10;
          instruction.argument = (object) (float) instruction.operand;
          break;
        case OperandType.ShortInlineVar:
          byte num11 = this.ilBytes.ReadByte();
          if (MethodBodyReader.TargetsLocalVariable(instruction.opcode))
          {
            LocalVariableInfo localVariable = this.GetLocalVariable((int) num11);
            if (localVariable == null)
            {
              instruction.argument = (object) num11;
              break;
            }
            instruction.operand = (object) localVariable;
            instruction.argument = (object) this.variables[localVariable.LocalIndex];
            break;
          }
          instruction.operand = (object) this.GetParameter((int) num11);
          instruction.argument = (object) num11;
          break;
        default:
          throw new NotSupportedException();
      }
    }

    private ILInstruction GetInstruction(int offset, bool isEndOfInstruction)
    {
      int index1 = this.ilInstructions.Count - 1;
      if (offset < 0 || offset > this.ilInstructions[index1].offset)
        throw new Exception("Instruction offset " + (object) offset + " is outside valid range 0 - " + (object) this.ilInstructions[index1].offset);
      int num1 = 0;
      int num2 = index1;
      while (num1 <= num2)
      {
        int index2 = num1 + (num2 - num1) / 2;
        ILInstruction ilInstruction = this.ilInstructions[index2];
        if (isEndOfInstruction)
        {
          if (offset == ilInstruction.offset + ilInstruction.GetSize() - 1)
            return ilInstruction;
        }
        else if (offset == ilInstruction.offset)
          return ilInstruction;
        if (offset < ilInstruction.offset)
          num2 = index2 - 1;
        else
          num1 = index2 + 1;
      }
      throw new Exception("Cannot find instruction for " + offset.ToString("X4"));
    }

    private static bool TargetsLocalVariable(OpCode opcode)
    {
      return opcode.Name.Contains("loc");
    }

    private LocalVariableInfo GetLocalVariable(int index)
    {
      return this.locals?[index];
    }

    private ParameterInfo GetParameter(int index)
    {
      if (index == 0)
        return this.this_parameter;
      return this.parameters[index - 1];
    }

    private OpCode ReadOpCode()
    {
      byte num = this.ilBytes.ReadByte();
      return num != (byte) 254 ? MethodBodyReader.one_byte_opcodes[(int) num] : MethodBodyReader.two_bytes_opcodes[(int) this.ilBytes.ReadByte()];
    }

    private MethodInfo EmitMethodForType(Type type)
    {
      foreach (KeyValuePair<Type, MethodInfo> emitMethod in MethodBodyReader.emitMethods)
      {
        if (emitMethod.Key == type)
          return emitMethod.Value;
      }
      foreach (KeyValuePair<Type, MethodInfo> emitMethod in MethodBodyReader.emitMethods)
      {
        if (emitMethod.Key.IsAssignableFrom(type))
          return emitMethod.Value;
      }
      return (MethodInfo) null;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    static MethodBodyReader()
    {
      foreach (FieldInfo field in typeof (OpCodes).GetFields(BindingFlags.Static | BindingFlags.Public))
      {
        OpCode opCode = (OpCode) field.GetValue((object) null);
        if (opCode.OpCodeType != OpCodeType.Nternal)
        {
          if (opCode.Size == 1)
            MethodBodyReader.one_byte_opcodes[(int) opCode.Value] = opCode;
          else
            MethodBodyReader.two_bytes_opcodes[(int) opCode.Value & (int) byte.MaxValue] = opCode;
        }
      }
      MethodBodyReader.emitMethods = new Dictionary<Type, MethodInfo>();
      ((IEnumerable<MethodInfo>) typeof (ILGenerator).GetMethods()).ToList<MethodInfo>().Do<MethodInfo>((Action<MethodInfo>) (method =>
      {
        if (method.Name != "Emit")
          return;
        ParameterInfo[] parameters = method.GetParameters();
        if (parameters.Length != 2)
          return;
        Type[] array = ((IEnumerable<ParameterInfo>) parameters).Select<ParameterInfo, Type>((Func<ParameterInfo, Type>) (p => p.ParameterType)).ToArray<Type>();
        if (array[0] != typeof (OpCode))
          return;
        MethodBodyReader.emitMethods[array[1]] = method;
      }));
    }

    private class ThisParameter : ParameterInfo
    {
      public ThisParameter(MethodBase method)
      {
        this.MemberImpl = (MemberInfo) method;
        this.ClassImpl = method.DeclaringType;
        this.NameImpl = "this";
        this.PositionImpl = -1;
      }
    }
  }
}
