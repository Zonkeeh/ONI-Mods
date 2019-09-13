// Decompiled with JetBrains decompiler
// Type: Klei.SimSaveFileStructure
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

namespace Klei
{
  public class SimSaveFileStructure
  {
    public int WidthInCells;
    public int HeightInCells;
    public byte[] Sim;
    public WorldDetailSave worldDetail;

    public SimSaveFileStructure()
    {
      this.worldDetail = new WorldDetailSave();
    }
  }
}
