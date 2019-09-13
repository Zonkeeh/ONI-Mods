// Decompiled with JetBrains decompiler
// Type: Harmony.ILCopying.Protection
// Assembly: 0Harmony, Version=1.2.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 1B235470-4299-4E96-B8B6-361DBE3791D9
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\0Harmony.dll

using System;

namespace Harmony.ILCopying
{
  [Flags]
  public enum Protection
  {
    PAGE_NOACCESS = 1,
    PAGE_READONLY = 2,
    PAGE_READWRITE = 4,
    PAGE_WRITECOPY = 8,
    PAGE_EXECUTE = 16, // 0x00000010
    PAGE_EXECUTE_READ = 32, // 0x00000020
    PAGE_EXECUTE_READWRITE = 64, // 0x00000040
    PAGE_EXECUTE_WRITECOPY = 128, // 0x00000080
    PAGE_GUARD = 256, // 0x00000100
    PAGE_NOCACHE = 512, // 0x00000200
    PAGE_WRITECOMBINE = 1024, // 0x00000400
  }
}
