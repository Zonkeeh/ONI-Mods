// Decompiled with JetBrains decompiler
// Type: Klei.WorldGenSave
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

namespace Klei
{
  public class WorldGenSave
  {
    public Vector2I version;
    public Dictionary<string, object> stats;
    public Data data;

    public WorldGenSave()
    {
      this.data = new Data();
      this.stats = new Dictionary<string, object>();
    }
  }
}
