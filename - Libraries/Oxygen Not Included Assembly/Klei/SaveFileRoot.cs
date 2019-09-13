// Decompiled with JetBrains decompiler
// Type: Klei.SaveFileRoot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

namespace Klei
{
  internal class SaveFileRoot
  {
    public int WidthInCells;
    public int HeightInCells;
    public Dictionary<string, byte[]> streamed;
    public string worldID;
    public List<ModInfo> requiredMods;
    public List<KMod.Label> active_mods;

    public SaveFileRoot()
    {
      this.streamed = new Dictionary<string, byte[]>();
    }
  }
}
