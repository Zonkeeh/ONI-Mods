// Decompiled with JetBrains decompiler
// Type: Harmony.ILCopying.ILInstruction
// Assembly: 0Harmony, Version=1.2.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 1B235470-4299-4E96-B8B6-361DBE3791D9
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\0Harmony.dll

using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Harmony.ILCopying
{
  public class ILInstruction
  {
    public List<Label> labels = new List<Label>();
    public List<ExceptionBlock> blocks = new List<ExceptionBlock>();
    public int offset;
    public OpCode opcode;
    public object operand;
    public object argument;

    public ILInstruction(OpCode opcode, object operand = null)
    {
      this.opcode = opcode;
      this.operand = operand;
      this.argument = operand;
    }

    public CodeInstruction GetCodeInstruction()
    {
      CodeInstruction codeInstruction = new CodeInstruction(this.opcode, this.argument);
      if (this.opcode.OperandType == OperandType.InlineNone)
        codeInstruction.operand = (object) null;
      codeInstruction.labels = this.labels;
      codeInstruction.blocks = this.blocks;
      return codeInstruction;
    }

    public int GetSize()
    {
      int size = this.opcode.Size;
      switch (this.opcode.OperandType)
      {
        case OperandType.InlineBrTarget:
        case OperandType.InlineField:
        case OperandType.InlineI:
        case OperandType.InlineMethod:
        case OperandType.InlineSig:
        case OperandType.InlineString:
        case OperandType.InlineTok:
        case OperandType.InlineType:
        case OperandType.ShortInlineR:
          size += 4;
          break;
        case OperandType.InlineI8:
        case OperandType.InlineR:
          size += 8;
          break;
        case OperandType.InlineSwitch:
          size += (1 + ((Array) this.operand).Length) * 4;
          break;
        case OperandType.InlineVar:
          size += 2;
          break;
        case OperandType.ShortInlineBrTarget:
        case OperandType.ShortInlineI:
        case OperandType.ShortInlineVar:
          ++size;
          break;
      }
      return size;
    }

    public override string ToString()
    {
      string str1 = "";
      ILInstruction.AppendLabel(ref str1, (object) this);
      string str2 = str1 + ": " + this.opcode.Name;
      if (this.operand == null)
        return str2;
      string str3 = str2 + " ";
      switch (this.opcode.OperandType)
      {
        case OperandType.InlineBrTarget:
        case OperandType.ShortInlineBrTarget:
          ILInstruction.AppendLabel(ref str3, this.operand);
          break;
        case OperandType.InlineString:
          str3 = str3 + "\"" + this.operand + "\"";
          break;
        case OperandType.InlineSwitch:
          ILInstruction[] operand = (ILInstruction[]) this.operand;
          for (int index = 0; index < operand.Length; ++index)
          {
            if (index > 0)
              str3 += ",";
            ILInstruction.AppendLabel(ref str3, (object) operand[index]);
          }
          break;
        default:
          str3 += (string) this.operand;
          break;
      }
      return str3;
    }

    private static void AppendLabel(ref string str, object argument)
    {
      ILInstruction ilInstruction = argument as ILInstruction;
      if (ilInstruction != null)
        str = str + "IL_" + ilInstruction.offset.ToString("X4");
      else
        str = str + "IL_" + argument;
    }
  }
}
