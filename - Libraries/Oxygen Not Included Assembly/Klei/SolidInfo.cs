// Decompiled with JetBrains decompiler
// Type: Klei.SolidInfo
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

namespace Klei
{
  public struct SolidInfo
  {
    public int cellIdx;
    public bool isSolid;

    public SolidInfo(int cellIdx, bool isSolid)
    {
      this.cellIdx = cellIdx;
      this.isSolid = isSolid;
    }
  }
}
