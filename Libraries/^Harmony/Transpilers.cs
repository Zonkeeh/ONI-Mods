// Decompiled with JetBrains decompiler
// Type: Harmony.Transpilers
// Assembly: 0Harmony, Version=1.2.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 1B235470-4299-4E96-B8B6-361DBE3791D9
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\0Harmony.dll

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace Harmony
{
  public static class Transpilers
  {
    public static IEnumerable<CodeInstruction> MethodReplacer(
      this IEnumerable<CodeInstruction> instructions,
      MethodBase from,
      MethodBase to)
    {
      if (from == null)
        throw new ArgumentException("Unexpected null argument", nameof (from));
      if (to == null)
        throw new ArgumentException("Unexpected null argument", nameof (to));
      foreach (CodeInstruction instruction1 in instructions)
      {
        CodeInstruction instruction = instruction1;
        MethodBase method = instruction.operand as MethodBase;
        if (method == from)
        {
          instruction.opcode = to.IsConstructor ? OpCodes.Newobj : OpCodes.Call;
          instruction.operand = (object) to;
        }
        yield return instruction;
        method = (MethodBase) null;
        instruction = (CodeInstruction) null;
      }
    }

    public static IEnumerable<CodeInstruction> DebugLogger(
      this IEnumerable<CodeInstruction> instructions,
      string text)
    {
      yield return new CodeInstruction(OpCodes.Ldstr, (object) text);
      yield return new CodeInstruction(OpCodes.Call, (object) AccessTools.Method(typeof (FileLog), "Log", (Type[]) null, (Type[]) null));
      foreach (CodeInstruction instruction1 in instructions)
      {
        CodeInstruction instruction = instruction1;
        yield return instruction;
        instruction = (CodeInstruction) null;
      }
    }
  }
}
