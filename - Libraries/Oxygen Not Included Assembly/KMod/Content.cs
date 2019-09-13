// Decompiled with JetBrains decompiler
// Type: KMod.Content
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

namespace KMod
{
  [Flags]
  public enum Content : byte
  {
    LayerableFiles = 1,
    Strings = 2,
    DLL = 4,
    Translation = 8,
    Animation = 16, // 0x10
  }
}
