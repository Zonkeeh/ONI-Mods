// Decompiled with JetBrains decompiler
// Type: Harmony.ILCopying.ExceptionBlock
// Assembly: 0Harmony, Version=1.2.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 1B235470-4299-4E96-B8B6-361DBE3791D9
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\0Harmony.dll

using System;

namespace Harmony.ILCopying
{
  public class ExceptionBlock
  {
    public ExceptionBlockType blockType;
    public Type catchType;

    public ExceptionBlock(ExceptionBlockType blockType, Type catchType)
    {
      this.blockType = blockType;
      this.catchType = catchType;
    }
  }
}
