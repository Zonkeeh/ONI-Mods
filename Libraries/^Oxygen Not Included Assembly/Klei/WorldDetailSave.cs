// Decompiled with JetBrains decompiler
// Type: Klei.WorldDetailSave
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Delaunay.Geo;
using KSerialization;
using ProcGen;
using ProcGenGame;
using System.Collections.Generic;

namespace Klei
{
  public class WorldDetailSave
  {
    public List<WorldDetailSave.OverworldCell> overworldCells;
    public int globalWorldSeed;
    public int globalWorldLayoutSeed;
    public int globalTerrainSeed;
    public int globalNoiseSeed;

    public WorldDetailSave()
    {
      this.overworldCells = new List<WorldDetailSave.OverworldCell>();
    }

    [SerializationConfig(MemberSerialization.OptOut)]
    public class OverworldCell
    {
      public Polygon poly;
      public TagSet tags;
      public SubWorld.ZoneType zoneType;

      public OverworldCell()
      {
      }

      public OverworldCell(SubWorld.ZoneType zoneType, TerrainCell tc)
      {
        this.poly = tc.poly;
        this.tags = tc.node.tags;
        this.zoneType = zoneType;
      }
    }
  }
}
