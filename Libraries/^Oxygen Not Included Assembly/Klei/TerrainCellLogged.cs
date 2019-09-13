// Decompiled with JetBrains decompiler
// Type: Klei.TerrainCellLogged
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using ProcGenGame;
using VoronoiTree;

namespace Klei
{
  public class TerrainCellLogged : TerrainCell
  {
    public TerrainCellLogged()
    {
    }

    public TerrainCellLogged(ProcGen.Node node, Diagram.Site site)
      : base(node, site)
    {
    }

    public override void LogInfo(string evt, string param, float value)
    {
    }
  }
}
