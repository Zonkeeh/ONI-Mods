// Decompiled with JetBrains decompiler
// Type: Harmony.CodeInstruction
// Assembly: 0Harmony, Version=1.2.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 1B235470-4299-4E96-B8B6-361DBE3791D9
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\0Harmony.dll

using Harmony.ILCopying;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace Harmony
{
  public class CodeInstruction
  {
    public List<Label> labels = new List<Label>();
    public List<ExceptionBlock> blocks = new List<ExceptionBlock>();
    public OpCode opcode;
    public object operand;

    public CodeInstruction(OpCode opcode, object operand = null)
    {
      this.opcode = opcode;
      this.operand = operand;
    }

    public CodeInstruction(CodeInstruction instruction)
    {
      this.opcode = instruction.opcode;
      this.operand = instruction.operand;
      this.labels = ((IEnumerable<Label>) instruction.labels.ToArray()).ToList<Label>();
    }

    public CodeInstruction Clone()
    {
      return new CodeInstruction(this)
      {
        labels = new List<Label>()
      };
    }

    public CodeInstruction Clone(OpCode opcode)
    {
      CodeInstruction codeInstruction = new CodeInstruction(this)
      {
        labels = new List<Label>()
      };
      codeInstruction.opcode = opcode;
      return codeInstruction;
    }

    public CodeInstruction Clone(OpCode opcode, object operand)
    {
      CodeInstruction codeInstruction = new CodeInstruction(this)
      {
        labels = new List<Label>()
      };
      codeInstruction.opcode = opcode;
      codeInstruction.operand = operand;
      return codeInstruction;
    }

    public override string ToString()
    {
      List<string> stringList = new List<string>();
      foreach (Label label in this.labels)
        stringList.Add("Label" + (object) label.GetHashCode());
      foreach (ExceptionBlock block in this.blocks)
        stringList.Add("EX_" + block.blockType.ToString().Replace("Block", ""));
      string str1 = stringList.Count > 0 ? " [" + string.Join(", ", stringList.ToArray()) + "]" : "";
      string str2 = Emitter.FormatArgument(this.operand);
      if (str2 != "")
        str2 = " " + str2;
      return this.opcode.ToString() + str2 + str1;
    }
  }
}
