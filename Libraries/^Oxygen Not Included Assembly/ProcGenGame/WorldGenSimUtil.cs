// Decompiled with JetBrains decompiler
// Type: ProcGenGame.WorldGenSimUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using KSerialization;
using ProcGen;
using STRINGS;
using System;
using System.Collections.Generic;
using System.IO;

namespace ProcGenGame
{
  public static class WorldGenSimUtil
  {
    public static unsafe bool DoSettleSim(
      WorldGenSettings settings,
      Sim.Cell[] cells,
      float[] bgTemp,
      Sim.DiseaseCell[] dcs,
      WorldGen.OfflineCallbackFunction updateProgressFn,
      Klei.Data data,
      List<KeyValuePair<Vector2I, TemplateContainer>> templateSpawnTargets,
      System.Action<OfflineWorldGen.ErrorInfo> error_cb,
      System.Action<Sim.Cell[], float[], Sim.DiseaseCell[]> onSettleComplete)
    {
      // ISSUE: reference to a compiler-generated field
      if (WorldGenSimUtil.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        WorldGenSimUtil.\u003C\u003Ef__mg\u0024cache0 = new Sim.GAME_MessageHandler(Sim.DLL_MessageHandler);
      }
      // ISSUE: reference to a compiler-generated field
      Sim.SIM_Initialize(WorldGenSimUtil.\u003C\u003Ef__mg\u0024cache0);
      SimMessages.CreateSimElementsTable(ElementLoader.elements);
      SimMessages.CreateWorldGenHACKDiseaseTable(WorldGen.diseaseIds);
      Sim.DiseaseCell[] dc = new Sim.DiseaseCell[dcs.Length];
      SimMessages.SimDataInitializeFromCells(Grid.WidthInCells, Grid.HeightInCells, cells, bgTemp, dc);
      int num1 = 500;
      int num2 = updateProgressFn(UI.WORLDGEN.SETTLESIM.key, 0.0f, WorldGenProgressStages.Stages.SettleSim) ? 1 : 0;
      Vector2I min = new Vector2I(0, 0);
      Vector2I max = new Vector2I(Grid.WidthInCells, Grid.HeightInCells);
      byte[] bytes = (byte[]) null;
      using (MemoryStream memoryStream = new MemoryStream())
      {
        using (BinaryWriter writer = new BinaryWriter((Stream) memoryStream))
        {
          try
          {
            Sim.Save(writer);
          }
          catch (Exception ex)
          {
            WorldGenLogger.LogException(ex.Message, ex.StackTrace);
            return updateProgressFn(new StringKey("Exception in Sim Save"), -1f, WorldGenProgressStages.Stages.Failure);
          }
        }
        bytes = memoryStream.ToArray();
      }
      if (Sim.Load(new FastReader(bytes)) != 0)
      {
        int num3 = updateProgressFn(UI.WORLDGEN.FAILED.key, -1f, WorldGenProgressStages.Stages.Failure) ? 1 : 0;
        return true;
      }
      byte[] msg = new byte[Grid.CellCount];
      for (int index = 0; index < Grid.CellCount; ++index)
        msg[index] = byte.MaxValue;
      for (int index1 = 0; index1 < num1; ++index1)
      {
        SimMessages.NewGameFrame(0.2f, min, max);
        IntPtr num3 = Sim.HandleMessage(SimMessageHashes.PrepareGameData, msg.Length, msg);
        int num4 = updateProgressFn(UI.WORLDGEN.SETTLESIM.key, (float) ((double) index1 / (double) num1 * 100.0), WorldGenProgressStages.Stages.SettleSim) ? 1 : 0;
        if (!(num3 == IntPtr.Zero))
        {
          Sim.GameDataUpdate* gameDataUpdatePtr = (Sim.GameDataUpdate*) (void*) num3;
          for (int index2 = 0; index2 < gameDataUpdatePtr->numSubstanceChangeInfo; ++index2)
          {
            int cellIdx = gameDataUpdatePtr->substanceChangeInfo[index2].cellIdx;
            cells[cellIdx].elementIdx = gameDataUpdatePtr->elementIdx[cellIdx];
            cells[cellIdx].insulation = gameDataUpdatePtr->insulation[cellIdx];
            cells[cellIdx].properties = gameDataUpdatePtr->properties[cellIdx];
            cells[cellIdx].temperature = gameDataUpdatePtr->temperature[cellIdx];
            cells[cellIdx].mass = gameDataUpdatePtr->mass[cellIdx];
            cells[cellIdx].strengthInfo = gameDataUpdatePtr->strengthInfo[cellIdx];
          }
          foreach (KeyValuePair<Vector2I, TemplateContainer> templateSpawnTarget in templateSpawnTargets)
          {
            for (int index2 = 0; index2 < templateSpawnTarget.Value.cells.Count; ++index2)
            {
              TemplateClasses.Cell templateCellData = templateSpawnTarget.Value.cells[index2];
              int cell = Grid.OffsetCell(Grid.XYToCell(templateSpawnTarget.Key.x, templateSpawnTarget.Key.y), templateCellData.location_x, templateCellData.location_y);
              if (Grid.IsValidCell(cell))
              {
                cells[cell].elementIdx = (byte) ElementLoader.GetElementIndex(templateCellData.element);
                cells[cell].temperature = templateCellData.temperature;
                cells[cell].mass = templateCellData.mass;
                dcs[cell].diseaseIdx = (byte) WorldGen.diseaseIds.FindIndex((Predicate<string>) (name => name == templateCellData.diseaseName));
                dcs[cell].elementCount = templateCellData.diseaseCount;
              }
            }
          }
        }
      }
      for (int gameCell = 0; gameCell < Grid.CellCount; ++gameCell)
      {
        int callbackIdx = gameCell != Grid.CellCount - 1 ? -1 : 2147481337;
        SimMessages.ModifyCell(gameCell, (int) cells[gameCell].elementIdx, cells[gameCell].temperature, cells[gameCell].mass, dcs[gameCell].diseaseIdx, dcs[gameCell].elementCount, SimMessages.ReplaceType.Replace, false, callbackIdx);
      }
      bool flag1 = false;
      while (!flag1)
      {
        SimMessages.NewGameFrame(0.2f, min, max);
        IntPtr num3 = Sim.HandleMessage(SimMessageHashes.PrepareGameData, msg.Length, msg);
        if (!(num3 == IntPtr.Zero))
        {
          Sim.GameDataUpdate* gameDataUpdatePtr = (Sim.GameDataUpdate*) (void*) num3;
          for (int index = 0; index < gameDataUpdatePtr->numCallbackInfo; ++index)
          {
            if (gameDataUpdatePtr->callbackInfo[index].callbackIdx == 2147481337)
            {
              flag1 = true;
              break;
            }
          }
        }
      }
      Sim.HandleMessage(SimMessageHashes.SettleWorldGen, 0, (byte[]) null);
      bool flag2 = WorldGenSimUtil.SaveSim(settings, data, error_cb);
      onSettleComplete(cells, bgTemp, dcs);
      Sim.Shutdown();
      return flag2;
    }

    private static bool SaveSim(
      WorldGenSettings settings,
      Klei.Data data,
      System.Action<OfflineWorldGen.ErrorInfo> error_cb)
    {
      try
      {
        Manager.Clear();
        SimSaveFileStructure saveFileStructure = new SimSaveFileStructure();
        for (int index = 0; index < data.overworldCells.Count; ++index)
          saveFileStructure.worldDetail.overworldCells.Add(new WorldDetailSave.OverworldCell(SettingsCache.GetCachedSubWorld(data.overworldCells[index].node.type).zoneType, data.overworldCells[index]));
        saveFileStructure.worldDetail.globalWorldSeed = data.globalWorldSeed;
        saveFileStructure.worldDetail.globalWorldLayoutSeed = data.globalWorldLayoutSeed;
        saveFileStructure.worldDetail.globalTerrainSeed = data.globalTerrainSeed;
        saveFileStructure.worldDetail.globalNoiseSeed = data.globalNoiseSeed;
        saveFileStructure.WidthInCells = Grid.WidthInCells;
        saveFileStructure.HeightInCells = Grid.HeightInCells;
        using (MemoryStream memoryStream = new MemoryStream())
        {
          using (BinaryWriter writer = new BinaryWriter((Stream) memoryStream))
            Sim.Save(writer);
          saveFileStructure.Sim = memoryStream.ToArray();
        }
        using (MemoryStream memoryStream = new MemoryStream())
        {
          using (BinaryWriter writer = new BinaryWriter((Stream) memoryStream))
          {
            try
            {
              Serializer.Serialize((object) saveFileStructure, writer);
            }
            catch (Exception ex)
            {
              DebugUtil.LogErrorArgs((object) "Couldn't serialize", (object) ex.Message, (object) ex.StackTrace);
            }
          }
          using (BinaryWriter writer = new BinaryWriter((Stream) File.Open(WorldGen.SIM_SAVE_FILENAME, FileMode.Create)))
          {
            Manager.SerializeDirectory(writer);
            writer.Write(memoryStream.ToArray());
          }
        }
        return true;
      }
      catch (Exception ex)
      {
        error_cb(new OfflineWorldGen.ErrorInfo()
        {
          errorDesc = string.Format((string) UI.FRONTEND.SUPPORTWARNINGS.SAVE_DIRECTORY_READ_ONLY, (object) WorldGen.SIM_SAVE_FILENAME),
          exception = ex
        });
        DebugUtil.LogErrorArgs((object) "Couldn't write", (object) ex.Message, (object) ex.StackTrace);
        return false;
      }
    }
  }
}
