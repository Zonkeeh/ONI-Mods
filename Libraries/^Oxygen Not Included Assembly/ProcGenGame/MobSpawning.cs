// Decompiled with JetBrains decompiler
// Type: ProcGenGame.MobSpawning
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using ProcGen;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProcGenGame
{
  public static class MobSpawning
  {
    public static Dictionary<TerrainCell, List<HashSet<int>>> NaturalCavities = new Dictionary<TerrainCell, List<HashSet<int>>>();
    public static HashSet<int> allNaturalCavityCells = new HashSet<int>();

    public static Dictionary<int, string> PlaceFeatureAmbientMobs(
      WorldGenSettings settings,
      TerrainCell tc,
      SeededRandom rnd,
      Sim.Cell[] cells,
      float[] bgTemp,
      Sim.DiseaseCell[] dc,
      HashSet<int> avoidCells,
      bool isDebug)
    {
      Dictionary<int, string> spawnedMobs = new Dictionary<int, string>();
      ProcGen.Node node = tc.node;
      HashSet<int> alreadyOccupiedCells = new HashSet<int>();
      FeatureSettings featureSettings = (FeatureSettings) null;
      foreach (Tag featureSpecificTag in node.featureSpecificTags)
      {
        if (settings.HasFeature(featureSpecificTag.Name))
        {
          featureSettings = settings.GetFeature(featureSpecificTag.Name);
          break;
        }
      }
      if (featureSettings == null || featureSettings.internalMobs == null || featureSettings.internalMobs.Count == 0)
        return spawnedMobs;
      List<int> spawnCellsFeature = tc.GetAvailableSpawnCellsFeature();
      tc.LogInfo(nameof (PlaceFeatureAmbientMobs), "possibleSpawnPoints", (float) spawnCellsFeature.Count);
      for (int index1 = spawnCellsFeature.Count - 1; index1 > 0; --index1)
      {
        int index2 = spawnCellsFeature[index1];
        if (ElementLoader.elements[(int) cells[index2].elementIdx].id == SimHashes.Katairite || ElementLoader.elements[(int) cells[index2].elementIdx].id == SimHashes.Unobtanium || avoidCells.Contains(index2))
          spawnCellsFeature.RemoveAt(index1);
      }
      tc.LogInfo("mob spawns", "Id:" + (object) node.node.Id + " possible cells", (float) spawnCellsFeature.Count);
      if (spawnCellsFeature.Count == 0)
      {
        if (isDebug)
          Debug.LogWarning((object) ("No where to put mobs possibleSpawnPoints [" + (object) tc.node.node.Id + "]"));
        return (Dictionary<int, string>) null;
      }
      foreach (MobReference internalMob in featureSettings.internalMobs)
      {
        Mob mob = settings.GetMob(internalMob.type);
        if (mob == null)
        {
          Debug.LogError((object) ("Missing mob description for internal mob [" + internalMob.type + "]"));
        }
        else
        {
          List<int> possibleSpawnPoints = MobSpawning.GetMobPossibleSpawnPoints(mob, spawnCellsFeature, cells, alreadyOccupiedCells, rnd);
          if (possibleSpawnPoints.Count == 0)
          {
            if (!isDebug)
              ;
          }
          else
          {
            tc.LogInfo("\t\tpossible", internalMob.type + " mps: " + (object) possibleSpawnPoints.Count + " ps:", (float) spawnCellsFeature.Count);
            int count = Mathf.RoundToInt(internalMob.count.GetRandomValueWithinRange(rnd));
            tc.LogInfo("\t\tcount", internalMob.type, (float) count);
            Tag mobPrefab = mob.prefabName != null ? new Tag(mob.prefabName) : new Tag(internalMob.type);
            MobSpawning.SpawnCountMobs(mob, mobPrefab, count, possibleSpawnPoints, tc, ref spawnedMobs, ref alreadyOccupiedCells);
          }
        }
      }
      return spawnedMobs;
    }

    public static Dictionary<int, string> PlaceBiomeAmbientMobs(
      WorldGenSettings settings,
      TerrainCell tc,
      SeededRandom rnd,
      Sim.Cell[] cells,
      float[] bgTemp,
      Sim.DiseaseCell[] dc,
      HashSet<int> avoidCells,
      bool isDebug)
    {
      Dictionary<int, string> spawnedMobs = new Dictionary<int, string>();
      ProcGen.Node node = tc.node;
      HashSet<int> alreadyOccupiedCells = new HashSet<int>();
      List<Tag> list = new List<Tag>();
      if (node.biomeSpecificTags == null)
      {
        tc.LogInfo(nameof (PlaceBiomeAmbientMobs), "No tags", (float) node.node.Id);
        return (Dictionary<int, string>) null;
      }
      foreach (Tag biomeSpecificTag in node.biomeSpecificTags)
      {
        if (settings.HasMob(biomeSpecificTag.Name) && settings.GetMob(biomeSpecificTag.Name) != null)
          list.Add(biomeSpecificTag);
      }
      if (list.Count <= 0)
      {
        tc.LogInfo(nameof (PlaceBiomeAmbientMobs), "No biome MOBS", (float) node.node.Id);
        return (Dictionary<int, string>) null;
      }
      List<int> possibleSpawnPoints1 = !node.tags.Contains(WorldGenTags.PreventAmbientMobsInFeature) ? tc.GetAvailableSpawnCellsAll() : tc.GetAvailableSpawnCellsBiome();
      tc.LogInfo("PlaceBiomAmbientMobs", "possibleSpawnPoints", (float) possibleSpawnPoints1.Count);
      for (int index1 = possibleSpawnPoints1.Count - 1; index1 > 0; --index1)
      {
        int index2 = possibleSpawnPoints1[index1];
        if (ElementLoader.elements[(int) cells[index2].elementIdx].id == SimHashes.Katairite || ElementLoader.elements[(int) cells[index2].elementIdx].id == SimHashes.Unobtanium || avoidCells.Contains(index2))
          possibleSpawnPoints1.RemoveAt(index1);
      }
      tc.LogInfo("mob spawns", "Id:" + (object) node.node.Id + " possible cells", (float) possibleSpawnPoints1.Count);
      if (possibleSpawnPoints1.Count == 0)
      {
        if (isDebug)
          Debug.LogWarning((object) ("No where to put mobs possibleSpawnPoints [" + (object) tc.node.node.Id + "]"));
        return (Dictionary<int, string>) null;
      }
      list.ShuffleSeeded<Tag>(rnd.RandomSource());
      for (int index = 0; index < list.Count; ++index)
      {
        Mob mob = settings.GetMob(list[index].Name);
        if (mob == null)
        {
          Debug.LogError((object) ("Missing sample description for tag [" + list[index].Name + "]"));
        }
        else
        {
          List<int> possibleSpawnPoints2 = MobSpawning.GetMobPossibleSpawnPoints(mob, possibleSpawnPoints1, cells, alreadyOccupiedCells, rnd);
          if (possibleSpawnPoints2.Count == 0)
          {
            if (!isDebug)
              ;
          }
          else
          {
            tc.LogInfo("\t\tpossible", list[index].ToString() + " mps: " + (object) possibleSpawnPoints2.Count + " ps:", (float) possibleSpawnPoints1.Count);
            float num = mob.density.GetRandomValueWithinRange(rnd) * MobSettings.AmbientMobDensity;
            if ((double) num > 1.0)
            {
              if (isDebug)
                Debug.LogWarning((object) ("Got a mob density greater than 1.0 for " + list[index].Name + ". Probably using density as spacing!"));
              num = 1f;
            }
            tc.LogInfo("\t\tdensity:", string.Empty, num);
            int count = Mathf.RoundToInt((float) possibleSpawnPoints2.Count * num);
            tc.LogInfo("\t\tcount", list[index].ToString(), (float) count);
            Tag mobPrefab = mob.prefabName != null ? new Tag(mob.prefabName) : list[index];
            MobSpawning.SpawnCountMobs(mob, mobPrefab, count, possibleSpawnPoints2, tc, ref spawnedMobs, ref alreadyOccupiedCells);
          }
        }
      }
      return spawnedMobs;
    }

    private static List<int> GetMobPossibleSpawnPoints(
      Mob mob,
      List<int> possibleSpawnPoints,
      Sim.Cell[] cells,
      HashSet<int> alreadyOccupiedCells,
      SeededRandom rnd)
    {
      List<int> all = possibleSpawnPoints.FindAll((Predicate<int>) (cell => MobSpawning.IsSuitableMobSpawnPoint(cell, mob, cells, ref alreadyOccupiedCells)));
      all.ShuffleSeeded<int>(rnd.RandomSource());
      return all;
    }

    public static void SpawnCountMobs(
      Mob mobData,
      Tag mobPrefab,
      int count,
      List<int> mobPossibleSpawnPoints,
      TerrainCell tc,
      ref Dictionary<int, string> spawnedMobs,
      ref HashSet<int> alreadyOccupiedCells)
    {
      for (int index1 = 0; index1 < count && index1 < mobPossibleSpawnPoints.Count; ++index1)
      {
        int possibleSpawnPoint = mobPossibleSpawnPoints[index1];
        for (int widthIterator = 0; widthIterator < mobData.width; ++widthIterator)
        {
          for (int index2 = 0; index2 < mobData.height; ++index2)
          {
            int num = MobSpawning.MobWidthOffset(possibleSpawnPoint, widthIterator);
            alreadyOccupiedCells.Add(num);
          }
        }
        tc.AddMob(new KeyValuePair<int, Tag>(possibleSpawnPoint, mobPrefab));
        spawnedMobs.Add(possibleSpawnPoint, mobPrefab.Name);
      }
    }

    public static int MobWidthOffset(int occupiedCell, int widthIterator)
    {
      return Grid.OffsetCell(occupiedCell, widthIterator % 2 != 0 ? widthIterator / 2 + widthIterator % 2 : -(widthIterator / 2), 0);
    }

    private static bool IsSuitableMobSpawnPoint(
      int cell,
      Mob mob,
      Sim.Cell[] cells,
      ref HashSet<int> alreadyOccupiedCells)
    {
      for (int widthIterator = 0; widthIterator < mob.width; ++widthIterator)
      {
        for (int index = 0; index < mob.height; ++index)
        {
          int cell1 = MobSpawning.MobWidthOffset(cell, widthIterator);
          if (!Grid.IsValidCell(cell1) || !Grid.IsValidCell(Grid.CellAbove(cell1)) || (!Grid.IsValidCell(Grid.CellBelow(cell1)) || alreadyOccupiedCells.Contains(cell1)))
            return false;
        }
      }
      switch (mob.location)
      {
        case Mob.Location.Floor:
          if (MobSpawning.isNaturalCavity(cell) && !Grid.Solid[cell] && (!Grid.Solid[Grid.CellAbove(cell)] && Grid.Solid[Grid.CellBelow(cell)]))
            return !Grid.IsLiquid(cell);
          return false;
        case Mob.Location.Ceiling:
          if (MobSpawning.isNaturalCavity(cell) && !Grid.Solid[cell] && (Grid.Solid[Grid.CellAbove(cell)] && !Grid.Solid[Grid.CellBelow(cell)]))
            return !Grid.IsLiquid(cell);
          return false;
        case Mob.Location.Air:
          if (!Grid.Solid[cell] && !Grid.Solid[Grid.CellAbove(cell)])
            return !Grid.IsLiquid(cell);
          return false;
        case Mob.Location.Solid:
          if (!MobSpawning.isNaturalCavity(cell))
            return Grid.Solid[cell];
          return false;
        case Mob.Location.Water:
          if (Grid.Element[cell].id != SimHashes.Water && Grid.Element[cell].id != SimHashes.DirtyWater)
            return false;
          if (Grid.Element[Grid.CellAbove(cell)].id != SimHashes.Water)
            return Grid.Element[Grid.CellAbove(cell)].id == SimHashes.DirtyWater;
          return true;
        case Mob.Location.Surface:
          bool flag = true;
          for (int widthIterator = 0; widthIterator < mob.width; ++widthIterator)
          {
            int cell1 = MobSpawning.MobWidthOffset(cell, widthIterator);
            flag = flag && Grid.Element[cell1].id == SimHashes.Vacuum && Grid.Solid[Grid.CellBelow(cell1)];
          }
          return flag;
        case Mob.Location.LiquidFloor:
          if (MobSpawning.isNaturalCavity(cell) && !Grid.Solid[cell] && (!Grid.Solid[Grid.CellAbove(cell)] && Grid.Solid[Grid.CellBelow(cell)]))
            return Grid.IsLiquid(cell);
          return false;
        case Mob.Location.AnyFloor:
          if (MobSpawning.isNaturalCavity(cell) && !Grid.Solid[cell] && !Grid.Solid[Grid.CellAbove(cell)])
            return Grid.Solid[Grid.CellBelow(cell)];
          return false;
        default:
          if (MobSpawning.isNaturalCavity(cell))
            return !Grid.Solid[cell];
          return false;
      }
    }

    public static bool isNaturalCavity(int cell)
    {
      return MobSpawning.NaturalCavities != null && MobSpawning.allNaturalCavityCells.Contains(cell);
    }

    public static void DetectNaturalCavities(
      List<TerrainCell> terrainCells,
      WorldGen.OfflineCallbackFunction updateProgressFn)
    {
      int num1 = updateProgressFn(UI.WORLDGEN.ANALYZINGWORLD.key, 0.8f, WorldGenProgressStages.Stages.DetectNaturalCavities) ? 1 : 0;
      HashSet<int> invalidCells = new HashSet<int>();
      for (int index1 = 0; index1 < terrainCells.Count; ++index1)
      {
        TerrainCell terrainCell = terrainCells[index1];
        float completePercent = (float) ((double) index1 / (double) terrainCells.Count * 100.0);
        int num2 = updateProgressFn(UI.WORLDGEN.ANALYZINGWORLDCOMPLETE.key, completePercent, WorldGenProgressStages.Stages.DetectNaturalCavities) ? 1 : 0;
        MobSpawning.NaturalCavities.Add(terrainCell, new List<HashSet<int>>());
        invalidCells.Clear();
        List<int> allCells = terrainCell.GetAllCells();
        for (int index2 = 0; index2 < allCells.Count; ++index2)
        {
          int start_cell = allCells[index2];
          if (!Grid.Solid[start_cell] && !invalidCells.Contains(start_cell))
          {
            HashSet<int> intSet = GameUtil.FloodCollectCells(start_cell, (Func<int, bool>) (checkCell =>
            {
              if (!invalidCells.Contains(checkCell))
                return !Grid.Solid[checkCell];
              return false;
            }), 300, invalidCells, true);
            if (intSet != null && intSet.Count > 0)
            {
              MobSpawning.NaturalCavities[terrainCell].Add(intSet);
              MobSpawning.allNaturalCavityCells.UnionWith((IEnumerable<int>) intSet);
            }
          }
        }
      }
      int num3 = updateProgressFn(UI.WORLDGEN.ANALYZINGWORLDCOMPLETE.key, 100f, WorldGenProgressStages.Stages.DetectNaturalCavities) ? 1 : 0;
    }
  }
}
