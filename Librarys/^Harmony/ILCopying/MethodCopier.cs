// Decompiled with JetBrains decompiler
// Type: Harmony.ILCopying.MethodCopier
// Assembly: 0Harmony, Version=1.2.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 1B235470-4299-4E96-B8B6-361DBE3791D9
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\0Harmony.dll

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace Harmony.ILCopying
{
  public class MethodCopier
  {
    private readonly List<MethodInfo> transpilers = new List<MethodInfo>();
    private readonly MethodBodyReader reader;

    public MethodCopier(
      MethodBase fromMethod,
      ILGenerator toILGenerator,
      LocalBuilder[] existingVariables = null)
    {
      if (fromMethod == null)
        throw new ArgumentNullException("Method cannot be null");
      this.reader = new MethodBodyReader(fromMethod, toILGenerator);
      this.reader.DeclareVariables(existingVariables);
      this.reader.ReadInstructions();
    }

    public void AddTranspiler(MethodInfo transpiler)
    {
      this.transpilers.Add(transpiler);
    }

    public void Finalize(List<Label> endLabels, List<ExceptionBlock> endBlocks)
    {
      this.reader.FinalizeILCodes(this.transpilers, endLabels, endBlocks);
    }
  }
}
