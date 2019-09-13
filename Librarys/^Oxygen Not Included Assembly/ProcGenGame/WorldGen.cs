// Decompiled with JetBrains decompiler
// Type: ProcGenGame.WorldGen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Delaunay.Geo;
using Klei;
using KSerialization;
using LibNoiseDotNet.Graphics.Tools.Noise;
using LibNoiseDotNet.Graphics.Tools.Noise.Builder;
using ProcGen;
using STRINGS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;
using VoronoiTree;

namespace ProcGenGame
{
  [Serializable]
  public class WorldGen
  {
    public static List<string> diseaseIds = new List<string>()
    {
      "FoodPoisoning",
      "SlimeLung"
    };
    private bool generateNoiseData = true;
    private bool running = true;
    public int polyIndex = -1;
    private const string _SIM_SAVE_FILENAME = "WorldGenSimSave.dat";
    private const string _WORLDGEN_SAVE_FILENAME = "WorldGenDataSave.dat";
    private const int heatScale = 2;
    private const int UNPASSABLE_EDGE_COUNT = 4;
    private const string heat_noise_name = "noise/Heat";
    private const string base_noise_name = "noise/Default";
    private const string cave_noise_name = "noise/DefaultCave";
    private const string density_noise_name = "noise/DefaultDensity";
    public const int WORLDGEN_SAVE_MAJOR_VERSION = 1;
    public const int WORLDGEN_SAVE_MINOR_VERSION = 1;
    public static Element voidElement;
    public static Element vacuumElement;
    public static Element katairiteElement;
    public static Element unobtaniumElement;
    public bool isRunningDebugGen;
    private Klei.Data data;
    private WorldGen.OfflineCallbackFunction successCallbackFn;
    private Thread generateThread;
    private Thread renderThread;
    private System.Action<OfflineWorldGen.ErrorInfo> errorCallback;
    private SeededRandom myRandom;
    private NoiseMapBuilderPlane heatSource;
    private bool wasLoaded;
    [EnumFlags]
    public WorldGen.DebugFlags drawOptions;

    public WorldGen(string worldName = "worlds/SandstoneDefault", List<string> chosenTraits = null)
    {
      WorldGen.LoadSettings();
      this.Settings = new WorldGenSettings(worldName, chosenTraits);
      this.data = new Klei.Data();
      this.data.chunkEdgeSize = this.Settings.GetIntSetting(nameof (ChunkEdgeSize));
      this.data.subWorldSize = new Vector2I(this.Settings.GetIntSetting("SubWorldWidth"), this.Settings.GetIntSetting("SubWorldHeight"));
      this.stats = new Dictionary<string, object>();
    }

    public static string SIM_SAVE_FILENAME
    {
      get
      {
        return System.IO.Path.Combine(Util.RootFolder(), "WorldGenSimSave.dat");
      }
    }

    public static string WORLDGEN_SAVE_FILENAME
    {
      get
      {
        return System.IO.Path.Combine(Util.RootFolder(), "WorldGenDataSave.dat");
      }
    }

    public int BaseLeft
    {
      get
      {
        return this.Settings.GetBaseLocation().left;
      }
    }

    public int BaseRight
    {
      get
      {
        return this.Settings.GetBaseLocation().right;
      }
    }

    public int BaseTop
    {
      get
      {
        return this.Settings.GetBaseLocation().top;
      }
    }

    public int BaseBot
    {
      get
      {
        return this.Settings.GetBaseLocation().bottom;
      }
    }

    public Dictionary<string, object> stats { get; private set; }

    public bool HasData
    {
      get
      {
        return this.data != null;
      }
    }

    public bool HasNoiseData
    {
      get
      {
        if (this.HasData)
          return this.data.world != null;
        return false;
      }
    }

    public float[] DensityMap
    {
      get
      {
        return this.data.world.density;
      }
    }

    public float[] HeatMap
    {
      get
      {
        return this.data.world.heatOffset;
      }
    }

    public float[] OverrideMap
    {
      get
      {
        return this.data.world.overrides;
      }
    }

    public float[] BaseNoiseMap
    {
      get
      {
        return this.data.world.data;
      }
    }

    public float[] DefaultTendMap
    {
      get
      {
        return this.data.world.defaultTemp;
      }
    }

    public Chunk World
    {
      get
      {
        return this.data.world;
      }
    }

    public Vector2I WorldSize
    {
      get
      {
        return this.data.world.size;
      }
    }

    public Vector2I SubWorldSize
    {
      get
      {
        return this.data.subWorldSize;
      }
    }

    public WorldLayout WorldLayout
    {
      get
      {
        return this.data.worldLayout;
      }
    }

    public List<TerrainCell> OverworldCells
    {
      get
      {
        return this.data.overworldCells;
      }
    }

    public List<TerrainCell> TerrainCells
    {
      get
      {
        return this.data.terrainCells;
      }
    }

    public List<ProcGen.River> Rivers
    {
      get
      {
        return this.data.rivers;
      }
    }

    public GameSpawnData SpawnData
    {
      get
      {
        return this.data.gameSpawnData;
      }
    }

    public int ChunkEdgeSize
    {
      get
      {
        return this.data.chunkEdgeSize;
      }
    }

    public WorldGenSettings Settings { get; private set; }

    public static void SetupDefaultElements()
    {
      WorldGen.voidElement = ElementLoader.FindElementByHash(SimHashes.Void);
      WorldGen.vacuumElement = ElementLoader.FindElementByHash(SimHashes.Vacuum);
      WorldGen.katairiteElement = ElementLoader.FindElementByHash(SimHashes.Katairite);
      WorldGen.unobtaniumElement = ElementLoader.FindElementByHash(SimHashes.Unobtanium);
    }

    public void Reset()
    {
      this.wasLoaded = false;
    }

    public static void LoadSettings()
    {
      ListPool<YamlIO.Error, WorldGen>.PooledList pooledList = ListPool<YamlIO.Error, WorldGen>.Allocate();
      if (SettingsCache.LoadFiles((List<YamlIO.Error>) pooledList))
        TemplateCache.Init();
      if (!((UnityEngine.Object) CustomGameSettings.Instance != (UnityEngine.Object) null))
        ;
      if (Application.isPlaying)
      {
        Global.Instance.modManager.HandleErrors((List<YamlIO.Error>) pooledList);
      }
      else
      {
        foreach (YamlIO.Error error in (List<YamlIO.Error>) pooledList)
          YamlIO.LogError(error, false);
      }
      pooledList.Recycle();
    }

    public void SaveSettings(string newpath = null)
    {
      SettingsCache.Save(newpath != null ? newpath : SettingsCache.GetPath());
    }

    public void InitRandom(int worldSeed, int layoutSeed, int terrainSeed, int noiseSeed)
    {
      this.data.globalWorldSeed = worldSeed;
      this.data.globalWorldLayoutSeed = layoutSeed;
      this.data.globalTerrainSeed = terrainSeed;
      this.data.globalNoiseSeed = noiseSeed;
      Console.WriteLine(string.Format("Seeds are [{0}/{1}/{2}/{3}]", (object) worldSeed, (object) layoutSeed, (object) terrainSeed, (object) noiseSeed));
      this.myRandom = new SeededRandom(worldSeed);
    }

    public void Initialise(
      WorldGen.OfflineCallbackFunction callbackFn,
      System.Action<OfflineWorldGen.ErrorInfo> error_cb,
      int worldSeed = -1,
      int layoutSeed = -1,
      int terrainSeed = -1,
      int noiseSeed = -1)
    {
      if (this.wasLoaded)
      {
        Debug.LogError((object) "Initialise called after load");
      }
      else
      {
        this.successCallbackFn = callbackFn;
        this.errorCallback = error_cb;
        Debug.Assert(this.successCallbackFn != null);
        this.isRunningDebugGen = false;
        this.running = false;
        int num1 = UnityEngine.Random.Range(0, int.MaxValue);
        if (worldSeed == -1)
          worldSeed = num1;
        if (layoutSeed == -1)
          layoutSeed = num1;
        if (terrainSeed == -1)
          terrainSeed = num1;
        if (noiseSeed == -1)
          noiseSeed = num1;
        this.data.gameSpawnData = new GameSpawnData();
        DebugUtil.LogArgs((object) string.Format("World seeds: [{0}/{1}/{2}/{3}]", (object) worldSeed, (object) layoutSeed, (object) terrainSeed, (object) noiseSeed));
        this.InitRandom(worldSeed, layoutSeed, terrainSeed, noiseSeed);
        TerrainCell.ClearClaimedCells();
        int num2 = this.successCallbackFn(UI.WORLDGEN.COMPLETE.key, 0.0f, WorldGenProgressStages.Stages.Failure) ? 1 : 0;
        this.stats["GenerateTime"] = (object) 0;
        this.stats["GenerateNoiseTime"] = (object) 0;
        this.stats["GenerateLayoutTime"] = (object) 0;
        this.stats["ConvertVoroToMapTime"] = (object) 0;
        WorldLayout.SetLayerGradient(SettingsCache.layers.LevelLayers);
      }
    }

    public void DontGenerateNoiseData()
    {
      this.generateNoiseData = false;
    }

    public void GenerateOfflineThreaded()
    {
      if (this.wasLoaded)
      {
        Debug.LogError((object) "GenerateOfflineThreaded called after load");
      }
      else
      {
        Debug.Assert(this.Settings.world != null, (object) "You need to set a world");
        if (this.Settings.world == null)
          return;
        this.running = true;
        this.generateThread = new Thread(new ThreadStart(this.GenerateOffline));
        Util.ApplyInvariantCultureToThread(this.generateThread);
        this.generateThread.Start();
      }
    }

    public void RenderWorldThreaded()
    {
      if (this.wasLoaded)
      {
        Debug.LogError((object) "RenderWorldThreaded called after load");
      }
      else
      {
        this.running = true;
        this.renderThread = new Thread(new ThreadStart(this.RenderOfflineThreadFn));
        Util.ApplyInvariantCultureToThread(this.renderThread);
        this.renderThread.Start();
      }
    }

    public void Quit()
    {
      if (this.generateThread != null && this.generateThread.IsAlive)
        this.generateThread.Abort();
      if (this.renderThread != null && this.renderThread.IsAlive)
        this.renderThread.Abort();
      this.running = false;
    }

    public bool IsGenerateComplete()
    {
      if (this.generateThread != null)
        return !this.generateThread.IsAlive;
      return false;
    }

    public bool IsRenderComplete()
    {
      if (this.renderThread != null)
        return !this.renderThread.IsAlive;
      return false;
    }

    public void GenerateOffline()
    {
      for (int index = 0; index < 3 && !this.GenerateWorldData(); ++index)
      {
        int num = this.successCallbackFn(UI.WORLDGEN.RETRYCOUNT.key, (float) index, WorldGenProgressStages.Stages.Failure) ? 1 : 0;
      }
    }

    private void PlaceTemplateSpawners(Vector2I position, TemplateContainer template)
    {
      this.data.gameSpawnData.AddTemplate(template, position);
    }

    private void RenderOfflineThreadFn()
    {
      Sim.DiseaseCell[] dc = (Sim.DiseaseCell[]) null;
      this.RenderOffline(true, ref dc);
    }

    public static int GetDiseaseIdx(string disease)
    {
      for (int index = 0; index < WorldGen.diseaseIds.Count; ++index)
      {
        if (disease == WorldGen.diseaseIds[index])
          return index;
      }
      return (int) byte.MaxValue;
    }

    public Sim.Cell[] RenderOffline(bool doSettle, ref Sim.DiseaseCell[] dc)
    {
      Sim.Cell[] cells = (Sim.Cell[]) null;
      float[] bgTemp = (float[]) null;
      dc = (Sim.DiseaseCell[]) null;
      HashSet<int> borderCells = new HashSet<int>();
      this.WriteOverWorldNoise(this.successCallbackFn);
      if (!this.RenderToMap(this.successCallbackFn, ref cells, ref bgTemp, ref dc, ref borderCells))
      {
        int num = this.successCallbackFn(UI.WORLDGEN.FAILED.key, -100f, WorldGenProgressStages.Stages.Failure) ? 1 : 0;
        return (Sim.Cell[]) null;
      }
      this.EnsureEnoughAlgaeInStartingBiome(cells);
      List<KeyValuePair<Vector2I, TemplateContainer>> templateSpawnTargets = new List<KeyValuePair<Vector2I, TemplateContainer>>();
      foreach (TerrainCell terrainCell in this.GetTerrainCellsForTag(WorldGenTags.StartLocation))
      {
        TemplateContainer startingBaseTemplate = TemplateCache.GetStartingBaseTemplate(this.Settings.world.startingBaseTemplate);
        templateSpawnTargets.Add(new KeyValuePair<Vector2I, TemplateContainer>(new Vector2I((int) terrainCell.poly.Centroid().x, (int) terrainCell.poly.Centroid().y), startingBaseTemplate));
      }
      List<TemplateContainer> templateContainerList1 = TemplateCache.CollectBaseTemplateAssets("poi");
      foreach (WeightedName subworldFile in this.Settings.world.subworldFiles)
      {
        SubWorld subWorld = this.Settings.GetSubWorld(subworldFile.name);
        if (subWorld.pointsOfInterest != null)
        {
          foreach (KeyValuePair<string, string[]> keyValuePair in subWorld.pointsOfInterest)
          {
            List<TerrainCell> terrainCellsForTag = this.GetTerrainCellsForTag(subWorld.name.ToTag());
            for (int index = terrainCellsForTag.Count - 1; index >= 0; --index)
            {
              if (!terrainCellsForTag[index].IsSafeToSpawnPOI(this.data.terrainCells))
                terrainCellsForTag.Remove(terrainCellsForTag[index]);
            }
            if (terrainCellsForTag.Count > 0)
            {
              string template = (string) null;
              TemplateContainer templateContainer = (TemplateContainer) null;
              for (int index = 0; templateContainer == null && index < keyValuePair.Value.Length; ++index)
              {
                template = keyValuePair.Value[this.myRandom.RandomRange(0, keyValuePair.Value.Length)];
                templateContainer = templateContainerList1.Find((Predicate<TemplateContainer>) (value => value.name == template));
              }
              if (templateContainer != null)
              {
                templateContainerList1.Remove(templateContainer);
                for (int index = 0; index < terrainCellsForTag.Count; ++index)
                {
                  TerrainCell terrainCell = terrainCellsForTag[this.myRandom.RandomRange(0, terrainCellsForTag.Count)];
                  if (!terrainCell.node.tags.Contains(WorldGenTags.POI))
                  {
                    if ((double) templateContainer.info.size.Y > (double) terrainCell.poly.MaxY - (double) terrainCell.poly.MinY)
                    {
                      float num1 = templateContainer.info.size.Y - (terrainCell.poly.MaxY - terrainCell.poly.MinY);
                      float num2 = templateContainer.info.size.X - (terrainCell.poly.MaxX - terrainCell.poly.MinX);
                      if ((double) terrainCell.poly.MaxY + (double) num1 < (double) Grid.HeightInCells && (double) terrainCell.poly.MinY - (double) num1 > 0.0 && ((double) terrainCell.poly.MaxX + (double) num2 < (double) Grid.WidthInCells && (double) terrainCell.poly.MinX - (double) num2 > 0.0))
                      {
                        templateSpawnTargets.Add(new KeyValuePair<Vector2I, TemplateContainer>(new Vector2I((int) terrainCell.poly.Centroid().x, (int) terrainCell.poly.Centroid().y), templateContainer));
                        terrainCell.node.tags.Add(template.ToTag());
                        terrainCell.node.tags.Add(WorldGenTags.POI);
                        break;
                      }
                    }
                    else
                    {
                      templateSpawnTargets.Add(new KeyValuePair<Vector2I, TemplateContainer>(new Vector2I((int) terrainCell.poly.Centroid().x, (int) terrainCell.poly.Centroid().y), templateContainer));
                      terrainCell.node.tags.Add(template.ToTag());
                      terrainCell.node.tags.Add(WorldGenTags.POI);
                      break;
                    }
                  }
                }
              }
            }
          }
        }
      }
      List<TemplateContainer> templateContainerList2 = TemplateCache.CollectBaseTemplateAssets("features");
      foreach (WeightedName subworldFile in this.Settings.world.subworldFiles)
      {
        SubWorld subWorld = this.Settings.GetSubWorld(subworldFile.name);
        if (subWorld.featureTemplates != null && subWorld.featureTemplates.Count > 0)
        {
          List<string> list = new List<string>();
          foreach (KeyValuePair<string, int> featureTemplate in subWorld.featureTemplates)
          {
            for (int index = 0; index < featureTemplate.Value; ++index)
              list.Add(featureTemplate.Key);
          }
          list.ShuffleSeeded<string>(this.myRandom.RandomSource());
          List<TerrainCell> terrainCellsForTag = this.GetTerrainCellsForTag(subWorld.name.ToTag());
          terrainCellsForTag.ShuffleSeeded<TerrainCell>(this.myRandom.RandomSource());
          foreach (TerrainCell terrainCell in terrainCellsForTag)
          {
            if (list.Count != 0)
            {
              if (terrainCell.IsSafeToSpawnFeatureTemplate())
              {
                string template = list[list.Count - 1];
                list.RemoveAt(list.Count - 1);
                TemplateContainer templateContainer = templateContainerList2.Find((Predicate<TemplateContainer>) (value => value.name == template));
                if (templateContainer != null)
                {
                  templateSpawnTargets.Add(new KeyValuePair<Vector2I, TemplateContainer>(new Vector2I((int) terrainCell.poly.Centroid().x, (int) terrainCell.poly.Centroid().y), templateContainer));
                  terrainCell.node.tags.Add(template.ToTag());
                  terrainCell.node.tags.Add(WorldGenTags.POI);
                }
              }
            }
            else
              break;
          }
        }
      }
      if (this.Settings.world.globalFeatureTemplates != null && this.Settings.world.globalFeatureTemplates.Count > 0)
      {
        List<TerrainCell> list1 = new List<TerrainCell>();
        foreach (TerrainCell terrainCell in this.data.terrainCells)
        {
          if (terrainCell.IsSafeToSpawnFeatureTemplate(WorldGenTags.NoGlobalFeatureSpawning))
            list1.Add(terrainCell);
        }
        list1.ShuffleSeeded<TerrainCell>(this.myRandom.RandomSource());
        List<string> list2 = new List<string>();
        foreach (KeyValuePair<string, int> globalFeatureTemplate in this.Settings.world.globalFeatureTemplates)
        {
          for (int index = 0; index < globalFeatureTemplate.Value; ++index)
            list2.Add(globalFeatureTemplate.Key);
        }
        list2.ShuffleSeeded<string>(this.myRandom.RandomSource());
        foreach (string str in list2)
        {
          string template = str;
          if (list1.Count != 0)
          {
            TerrainCell terrainCell = list1[list1.Count - 1];
            list1.RemoveAt(list1.Count - 1);
            TemplateContainer templateContainer = templateContainerList2.Find((Predicate<TemplateContainer>) (value => value.name == template));
            if (templateContainer != null)
            {
              templateSpawnTargets.Add(new KeyValuePair<Vector2I, TemplateContainer>(new Vector2I((int) terrainCell.poly.Centroid().x, (int) terrainCell.poly.Centroid().y), templateContainer));
              terrainCell.node.tags.Add(template.ToTag());
              terrainCell.node.tags.Add(WorldGenTags.Feature);
            }
          }
          else
            break;
        }
      }
      List<TerrainCell> terrainCellsForTag1 = this.GetTerrainCellsForTag(WorldGenTags.StartWorld);
      foreach (TerrainCell overworldCell in this.OverworldCells)
      {
        foreach (TerrainCell terrainCell in terrainCellsForTag1)
        {
          if (overworldCell.poly.PointInPolygon(terrainCell.poly.Centroid()))
          {
            overworldCell.node.tags.Add(WorldGenTags.StartWorld);
            break;
          }
        }
      }
      foreach (int index in borderCells)
        cells[index].SetValues(WorldGen.unobtaniumElement, ElementLoader.elements);
      if (doSettle)
        this.running = WorldGenSimUtil.DoSettleSim(this.Settings, cells, bgTemp, dc, this.successCallbackFn, this.data, templateSpawnTargets, this.errorCallback, (System.Action<Sim.Cell[], float[], Sim.DiseaseCell[]>) ((updatedCells, updatedBGTemp, updatedDisease) => this.SpawnMobsAndTemplates(updatedCells, updatedBGTemp, updatedDisease, borderCells)));
      foreach (KeyValuePair<Vector2I, TemplateContainer> keyValuePair in templateSpawnTargets)
        this.PlaceTemplateSpawners(keyValuePair.Key, keyValuePair.Value);
      for (int index = this.data.gameSpawnData.buildings.Count - 1; index >= 0; --index)
      {
        int cell = Grid.XYToCell(this.data.gameSpawnData.buildings[index].location_x, this.data.gameSpawnData.buildings[index].location_y);
        if (borderCells.Contains(cell))
          this.data.gameSpawnData.buildings.RemoveAt(index);
      }
      for (int index = this.data.gameSpawnData.elementalOres.Count - 1; index >= 0; --index)
      {
        int cell = Grid.XYToCell(this.data.gameSpawnData.elementalOres[index].location_x, this.data.gameSpawnData.elementalOres[index].location_y);
        if (borderCells.Contains(cell))
          this.data.gameSpawnData.elementalOres.RemoveAt(index);
      }
      for (int index = this.data.gameSpawnData.otherEntities.Count - 1; index >= 0; --index)
      {
        int cell = Grid.XYToCell(this.data.gameSpawnData.otherEntities[index].location_x, this.data.gameSpawnData.otherEntities[index].location_y);
        if (borderCells.Contains(cell))
          this.data.gameSpawnData.otherEntities.RemoveAt(index);
      }
      for (int index = this.data.gameSpawnData.pickupables.Count - 1; index >= 0; --index)
      {
        int cell = Grid.XYToCell(this.data.gameSpawnData.pickupables[index].location_x, this.data.gameSpawnData.pickupables[index].location_y);
        if (borderCells.Contains(cell))
          this.data.gameSpawnData.pickupables.RemoveAt(index);
      }
      this.SaveWorldGen();
      int num3 = this.successCallbackFn(UI.WORLDGEN.COMPLETE.key, 101f, WorldGenProgressStages.Stages.Complete) ? 1 : 0;
      this.running = false;
      return cells;
    }

    private void SpawnMobsAndTemplates(
      Sim.Cell[] cells,
      float[] bgTemp,
      Sim.DiseaseCell[] dc,
      HashSet<int> borderCells)
    {
      MobSpawning.DetectNaturalCavities(this.TerrainCells, this.successCallbackFn);
      SeededRandom rnd = new SeededRandom(this.data.globalTerrainSeed);
      for (int index = 0; index < this.TerrainCells.Count; ++index)
      {
        float completePercent = (float) ((double) index / (double) this.TerrainCells.Count * 100.0);
        int num = this.successCallbackFn(UI.WORLDGEN.PLACINGCREATURES.key, completePercent, WorldGenProgressStages.Stages.PlacingCreatures) ? 1 : 0;
        TerrainCell terrainCell = this.TerrainCells[index];
        Dictionary<int, string> dictionary1 = MobSpawning.PlaceFeatureAmbientMobs(this.Settings, terrainCell, rnd, cells, bgTemp, dc, borderCells, this.isRunningDebugGen);
        if (dictionary1 != null)
          this.data.gameSpawnData.AddRange((IEnumerable<KeyValuePair<int, string>>) dictionary1);
        Dictionary<int, string> dictionary2 = MobSpawning.PlaceBiomeAmbientMobs(this.Settings, terrainCell, rnd, cells, bgTemp, dc, borderCells, this.isRunningDebugGen);
        if (dictionary2 != null)
          this.data.gameSpawnData.AddRange((IEnumerable<KeyValuePair<int, string>>) dictionary2);
      }
      int num1 = this.successCallbackFn(UI.WORLDGEN.PLACINGCREATURES.key, 100f, WorldGenProgressStages.Stages.PlacingCreatures) ? 1 : 0;
    }

    public void SetWorldSize(int width, int height)
    {
      this.data.world = new Chunk(0, 0, width, height);
    }

    public bool GenerateNoiseData(WorldGen.OfflineCallbackFunction updateProgressFn)
    {
      this.stats["GenerateNoiseTime"] = (object) System.DateTime.Now.Ticks;
      try
      {
        this.running = updateProgressFn(UI.WORLDGEN.SETUPNOISE.key, 0.0f, WorldGenProgressStages.Stages.SetupNoise);
        if (!this.running)
        {
          this.stats["GenerateNoiseTime"] = (object) 0;
          return false;
        }
        this.SetupNoise(updateProgressFn);
        this.running = updateProgressFn(UI.WORLDGEN.SETUPNOISE.key, 100f, WorldGenProgressStages.Stages.SetupNoise);
        if (!this.running)
        {
          this.stats["GenerateNoiseTime"] = (object) 0;
          return false;
        }
        this.GenerateUnChunkedNoise(updateProgressFn);
      }
      catch (Exception ex)
      {
        WorldGenLogger.LogException(ex.Message, ex.StackTrace);
        this.running = this.successCallbackFn(new StringKey("Exception in GenerateNoiseData"), -1f, WorldGenProgressStages.Stages.Failure);
        return false;
      }
      this.stats["GenerateNoiseTime"] = (object) (System.DateTime.Now.Ticks - (long) this.stats["GenerateNoiseTime"]);
      return true;
    }

    public bool GenerateLayout(WorldGen.OfflineCallbackFunction updateProgressFn)
    {
      this.stats["GenerateLayoutTime"] = (object) System.DateTime.Now.Ticks;
      try
      {
        this.running = updateProgressFn(UI.WORLDGEN.WORLDLAYOUT.key, 0.0f, WorldGenProgressStages.Stages.WorldLayout);
        if (!this.running)
          return false;
        Debug.Assert(this.data.world.size.x != 0 && this.data.world.size.y != 0, (object) "Map size has not been set");
        this.data.worldLayout = new WorldLayout(this, this.data.world.size.x, this.data.world.size.y, this.data.globalWorldLayoutSeed);
        this.running = updateProgressFn(UI.WORLDGEN.WORLDLAYOUT.key, 5f, WorldGenProgressStages.Stages.WorldLayout);
        this.data.voronoiTree = (VoronoiTree.Tree) null;
        try
        {
          this.data.voronoiTree = this.WorldLayout.GenerateOverworld(this.Settings.world.layoutMethod == ProcGen.World.LayoutMethod.PowerTree);
          this.WorldLayout.PopulateSubworlds();
          this.CompleteLayout(updateProgressFn);
        }
        catch (Exception ex)
        {
          WorldGenLogger.LogException(ex.Message, ex.StackTrace);
          this.running = updateProgressFn(new StringKey("Exception in InitVoronoiTree"), -1f, WorldGenProgressStages.Stages.Failure);
          return false;
        }
        this.data.overworldCells = new List<TerrainCell>(40);
        for (int childIndex = 0; childIndex < this.data.voronoiTree.ChildCount(); ++childIndex)
        {
          VoronoiTree.Tree child = this.data.voronoiTree.GetChild(childIndex) as VoronoiTree.Tree;
          this.data.overworldCells.Add((TerrainCell) new TerrainCellLogged(this.data.worldLayout.overworldGraph.FindNodeByID(child.site.id), child.site));
        }
        this.running = updateProgressFn(UI.WORLDGEN.WORLDLAYOUT.key, 100f, WorldGenProgressStages.Stages.WorldLayout);
      }
      catch (Exception ex)
      {
        WorldGenLogger.LogException(ex.Message, ex.StackTrace);
        int num = this.successCallbackFn(new StringKey("Exception in GenerateLayout"), -1f, WorldGenProgressStages.Stages.Failure) ? 1 : 0;
        return false;
      }
      this.stats["GenerateLayoutTime"] = (object) (System.DateTime.Now.Ticks - (long) this.stats["GenerateLayoutTime"]);
      return true;
    }

    public bool CompleteLayout(WorldGen.OfflineCallbackFunction updateProgressFn)
    {
      long ticks = System.DateTime.Now.Ticks;
      try
      {
        this.running = updateProgressFn(UI.WORLDGEN.COMPLETELAYOUT.key, 0.0f, WorldGenProgressStages.Stages.CompleteLayout);
        if (!this.running)
          return false;
        this.data.terrainCells = (List<TerrainCell>) null;
        this.running = updateProgressFn(UI.WORLDGEN.COMPLETELAYOUT.key, 65f, WorldGenProgressStages.Stages.CompleteLayout);
        if (!this.running)
          return false;
        this.running = updateProgressFn(UI.WORLDGEN.COMPLETELAYOUT.key, 75f, WorldGenProgressStages.Stages.CompleteLayout);
        if (!this.running)
          return false;
        this.data.terrainCells = new List<TerrainCell>(4000);
        List<VoronoiTree.Node> nodes = new List<VoronoiTree.Node>();
        this.data.voronoiTree.ForceLowestToLeaf();
        this.ApplyStartNode();
        this.data.voronoiTree.VisitAll(new System.Action<VoronoiTree.Node>(this.UpdateVoronoiNodeTags));
        this.data.voronoiTree.GetLeafNodes(nodes, (VoronoiTree.Tree.LeafNodeTest) null);
        for (int index = 0; index < nodes.Count; ++index)
        {
          VoronoiTree.Node node = nodes[index];
          ProcGen.Node tn = this.data.worldLayout.localGraph.FindNodeByID(node.site.id);
          if (tn != null)
          {
            TerrainCell terrainCell = this.data.terrainCells.Find((Predicate<TerrainCell>) (c => c.node == tn));
            if (terrainCell == null)
              this.data.terrainCells.Add((TerrainCell) new TerrainCellLogged(tn, node.site));
            else
              Debug.LogWarning((object) ("Duplicate cell found" + (object) terrainCell.node.node.Id));
          }
        }
        for (int index1 = 0; index1 < this.data.terrainCells.Count; ++index1)
        {
          TerrainCell terrainCell = this.data.terrainCells[index1];
          foreach (KeyValuePair<uint, int> neighbour in terrainCell.site.neighbours)
          {
            for (int index2 = 0; index2 < this.data.terrainCells.Count; ++index2)
            {
              if ((int) this.data.terrainCells[index2].site.id == (int) neighbour.Key)
                terrainCell.terrain_neighbors_idx.Add(neighbour.Key);
            }
          }
        }
        this.running = updateProgressFn(UI.WORLDGEN.COMPLETELAYOUT.key, 100f, WorldGenProgressStages.Stages.CompleteLayout);
      }
      catch (Exception ex)
      {
        WorldGenLogger.LogException(ex.Message, ex.StackTrace);
        int num = this.successCallbackFn(new StringKey("Exception in CompleteLayout"), -1f, WorldGenProgressStages.Stages.Failure) ? 1 : 0;
        return false;
      }
      this.stats["GenerateLayoutTime"] = (object) ((long) this.stats["GenerateLayoutTime"] + (System.DateTime.Now.Ticks - ticks));
      return true;
    }

    public void UpdateVoronoiNodeTags(VoronoiTree.Node node)
    {
      (!node.tags.Contains(WorldGenTags.Overworld) ? this.WorldLayout.localGraph.FindNodeByID(node.site.id) : this.WorldLayout.overworldGraph.FindNodeByID(node.site.id))?.tags.Union(node.tags);
    }

    public bool GenerateWorldData()
    {
      this.stats["GenerateDataTime"] = (object) System.DateTime.Now.Ticks;
      Vector2I worldsize = this.Settings.world.worldsize;
      this.SetWorldSize(worldsize.x, worldsize.y);
      if (this.generateNoiseData && !this.GenerateNoiseData(this.successCallbackFn) || !this.GenerateLayout(this.successCallbackFn))
        return false;
      this.stats["GenerateDataTime"] = (object) (System.DateTime.Now.Ticks - (long) this.stats["GenerateDataTime"]);
      return true;
    }

    public void EnsureEnoughAlgaeInStartingBiome(Sim.Cell[] cells)
    {
      List<TerrainCell> terrainCellsForTag = this.GetTerrainCellsForTag(WorldGenTags.StartWorld);
      float num1 = 8200f;
      float num2 = 0.0f;
      int num3 = 0;
      foreach (TerrainCell terrainCell in terrainCellsForTag)
      {
        foreach (int allCell in terrainCell.GetAllCells())
        {
          if (ElementLoader.GetElementIndex(SimHashes.Algae) == (int) cells[allCell].elementIdx)
          {
            ++num3;
            num2 += cells[allCell].mass;
          }
        }
      }
      if ((double) num2 >= (double) num1)
        return;
      float num4 = (num1 - num2) / (float) num3;
      foreach (TerrainCell terrainCell in terrainCellsForTag)
      {
        foreach (int allCell in terrainCell.GetAllCells())
        {
          if (ElementLoader.GetElementIndex(SimHashes.Algae) == (int) cells[allCell].elementIdx)
            cells[allCell].mass += num4;
        }
      }
    }

    public bool RenderToMap(
      WorldGen.OfflineCallbackFunction updateProgressFn,
      ref Sim.Cell[] cells,
      ref float[] bgTemp,
      ref Sim.DiseaseCell[] dcs,
      ref HashSet<int> borderCells)
    {
      Debug.Assert(Grid.WidthInCells == this.Settings.world.worldsize.x);
      Debug.Assert(Grid.HeightInCells == this.Settings.world.worldsize.y);
      Debug.Assert(Grid.CellCount == Grid.WidthInCells * Grid.HeightInCells);
      Debug.Assert((double) Grid.CellSizeInMeters != 0.0);
      borderCells = new HashSet<int>();
      cells = new Sim.Cell[Grid.CellCount];
      bgTemp = new float[Grid.CellCount];
      dcs = new Sim.DiseaseCell[Grid.CellCount];
      this.running = updateProgressFn(UI.WORLDGEN.CLEARINGLEVEL.key, 0.0f, WorldGenProgressStages.Stages.ClearingLevel);
      if (!this.running)
        return false;
      for (int index = 0; index < cells.Length; ++index)
      {
        cells[index].SetValues(WorldGen.katairiteElement, ElementLoader.elements);
        bgTemp[index] = -1f;
        dcs[index] = new Sim.DiseaseCell();
        dcs[index].diseaseIdx = byte.MaxValue;
        this.running = updateProgressFn(UI.WORLDGEN.CLEARINGLEVEL.key, (float) (100.0 * ((double) index / (double) Grid.CellCount)), WorldGenProgressStages.Stages.ClearingLevel);
        if (!this.running)
          return false;
      }
      int num1 = updateProgressFn(UI.WORLDGEN.CLEARINGLEVEL.key, 100f, WorldGenProgressStages.Stages.ClearingLevel) ? 1 : 0;
      try
      {
        this.ProcessByTerrainCell(cells, bgTemp, dcs, updateProgressFn);
      }
      catch (Exception ex)
      {
        WorldGenLogger.LogException(ex.Message, ex.StackTrace);
        this.running = updateProgressFn(new StringKey("Exception in ProcessByTerrainCell"), -1f, WorldGenProgressStages.Stages.Failure);
        return false;
      }
      if (this.Settings.GetBoolSetting("DrawWorldBorder"))
      {
        SeededRandom rnd = new SeededRandom(0);
        this.DrawWorldBorder(cells, this.data.world, rnd, borderCells, updateProgressFn);
        int num2 = updateProgressFn(UI.WORLDGEN.DRAWWORLDBORDER.key, 100f, WorldGenProgressStages.Stages.DrawWorldBorder) ? 1 : 0;
      }
      this.data.gameSpawnData.baseStartPos = this.data.worldLayout.GetStartLocation();
      return true;
    }

    public SubWorld GetSubWorldForNode(VoronoiTree.Tree node)
    {
      ProcGen.Node nodeById = this.WorldLayout.overworldGraph.FindNodeByID(node.site.id);
      if (nodeById == null)
        return (SubWorld) null;
      if (!this.Settings.HasSubworld(nodeById.type))
        return (SubWorld) null;
      return this.Settings.GetSubWorld(nodeById.type);
    }

    public VoronoiTree.Tree GetOverworldForNode(Leaf leaf)
    {
      if (leaf == null)
        return (VoronoiTree.Tree) null;
      return this.data.worldLayout.GetVoronoiTree().GetChildContainingLeaf(leaf);
    }

    public Leaf GetLeafForTerrainCell(TerrainCell cell)
    {
      if (cell == null)
        return (Leaf) null;
      return this.data.worldLayout.GetVoronoiTree().GetNodeForSite(cell.site) as Leaf;
    }

    public List<TerrainCell> GetTerrainCellsForTag(Tag tag)
    {
      List<TerrainCell> terrainCellList = new List<TerrainCell>();
      List<VoronoiTree.Node> leafNodesWithTag = this.WorldLayout.GetLeafNodesWithTag(tag);
      for (int index = 0; index < leafNodesWithTag.Count; ++index)
      {
        VoronoiTree.Node node = leafNodesWithTag[index];
        TerrainCell terrainCell = this.data.terrainCells.Find((Predicate<TerrainCell>) (cell => (int) cell.site.id == (int) node.site.id));
        if (terrainCell != null)
          terrainCellList.Add(terrainCell);
      }
      return terrainCellList;
    }

    private void GetStartCells(out int baseX, out int baseY)
    {
      Vector2I vector2I = new Vector2I(this.data.world.size.x / 2, (int) ((double) this.data.world.size.y * 0.699999988079071));
      if (this.data.worldLayout != null)
        vector2I = this.data.worldLayout.GetStartLocation();
      baseX = vector2I.x;
      baseY = vector2I.y;
    }

    public void ChooseBaseLocation(VoronoiTree.Node startNode)
    {
      TagSet other = new TagSet();
      other.Add(WorldGenTags.StartLocation);
      List<VoronoiTree.Node> startNodes = this.WorldLayout.GetStartNodes();
      for (int index = 0; index < startNodes.Count; ++index)
      {
        if (startNodes[index] != startNode)
          startNodes[index].tags.Remove(other);
      }
    }

    private void SwitchNodes(VoronoiTree.Node n1, VoronoiTree.Node n2)
    {
      if (n1 is VoronoiTree.Tree || n2 is VoronoiTree.Tree)
      {
        Debug.Log((object) "WorldGen::SwitchNodes() Skipping tree node");
      }
      else
      {
        ProcGen.Node nodeById1 = this.data.worldLayout.localGraph.FindNodeByID(n1.site.id);
        ProcGen.Node nodeById2 = this.data.worldLayout.localGraph.FindNodeByID(n2.site.id);
        Diagram.Site site = n1.site;
        n1.site = n2.site;
        n2.site = site;
        string type = nodeById1.type;
        nodeById1.SetType(nodeById2.type);
        nodeById2.SetType(type);
      }
    }

    public void ApplyStartNode()
    {
      VoronoiTree.Node node1 = this.data.worldLayout.GetLeafNodesWithTag(WorldGenTags.StartLocation)[0];
      VoronoiTree.Tree parent = node1.parent;
      node1.parent.AddTagToChildren(WorldGenTags.IgnoreCaveOverride);
      node1.parent.tags.Remove(WorldGenTags.StartLocation);
      List<VoronoiTree.Node> siblings = node1.GetSiblings();
      List<VoronoiTree.Node> neighbors = node1.GetNeighbors();
      List<VoronoiTree.Node> list = new List<VoronoiTree.Node>();
      for (int childIndex = 0; childIndex < parent.ChildCount(); ++childIndex)
      {
        VoronoiTree.Node child = parent.GetChild(childIndex);
        if (child != node1)
          list.Add(child);
      }
      siblings.RemoveAll((Predicate<VoronoiTree.Node>) (node => neighbors.Contains(node)));
      if (list.Count <= 0)
        return;
      list.ShuffleSeeded<VoronoiTree.Node>(this.myRandom.RandomSource());
      List<VoronoiTree.Node> nodeList1 = new List<VoronoiTree.Node>();
      List<VoronoiTree.Node> nodeList2 = new List<VoronoiTree.Node>();
      for (int index = 0; index < list.Count; ++index)
      {
        VoronoiTree.Node n1 = list[index];
        bool flag1 = !n1.tags.Contains(WorldGenTags.Wet);
        bool flag2 = (double) n1.site.poly.Centroid().y > (double) node1.site.poly.Centroid().y;
        if (!flag1 && flag2)
        {
          if (nodeList2.Count > 0)
          {
            this.SwitchNodes(n1, nodeList2[0]);
            nodeList2.RemoveAt(0);
          }
          else
            nodeList1.Add(n1);
        }
        else if (flag1 && !flag2)
        {
          if (nodeList1.Count > 0)
          {
            this.SwitchNodes(n1, nodeList1[0]);
            nodeList1.RemoveAt(0);
          }
          else
            nodeList2.Add(n1);
        }
      }
      if (nodeList2.Count <= 0)
        return;
      for (int index = 0; index < nodeList1.Count && nodeList2.Count > 0; ++index)
      {
        this.SwitchNodes(nodeList1[index], nodeList2[0]);
        nodeList2.RemoveAt(0);
      }
    }

    public void ReplayGenerate(WorldGen.ResetFunction Reset)
    {
      Reset(this.data.gameSpawnData);
    }

    public void GetElementForBiomePoint(
      Chunk chunk,
      ElementBandConfiguration elementBands,
      Vector2I pos,
      out Element element,
      out Sim.PhysicsData pd,
      out Sim.DiseaseCell dc,
      float erode)
    {
      TerrainCell.GetElementOverride(WorldGen.voidElement.tag.ToString(), (SampleDescriber.Override) null);
      TerrainCell.ElementOverride biomeElementTable = this.GetElementFromBiomeElementTable(chunk, pos, (List<ElementGradient>) elementBands, erode);
      element = biomeElementTable.element;
      pd = biomeElementTable.pdelement;
      dc = biomeElementTable.dc;
    }

    private bool ConvertTerrainCellsToEdges(WorldGen.OfflineCallbackFunction updateProgress)
    {
      for (int index1 = 0; index1 < this.data.overworldCells.Count; ++index1)
      {
        this.running = updateProgress(UI.WORLDGEN.CONVERTTERRAINCELLSTOEDGES.key, (float) index1 / (float) this.data.overworldCells.Count, WorldGenProgressStages.Stages.ConvertCellsToEdges);
        if (!this.running)
          return this.running;
        List<Vector2> vertices = this.data.overworldCells[index1].poly.Vertices;
        for (int index2 = 0; index2 < vertices.Count; ++index2)
        {
          if (index2 < vertices.Count - 1)
            this.ConvertIntersectingCellsToType(new MathUtil.Pair<Vector2, Vector2>(vertices[index2], vertices[index2 + 1]), "EDGE");
          else
            this.ConvertIntersectingCellsToType(new MathUtil.Pair<Vector2, Vector2>(vertices[index2], vertices[0]), vertices.Count <= 4 ? "EDGE" : "UNPASSABLE");
        }
      }
      return true;
    }

    public void ConvertIntersectingCellsToType(MathUtil.Pair<Vector2, Vector2> segment, string type)
    {
      List<Vector2I> line = ProcGen.Util.GetLine(segment.First, segment.Second);
      for (int index1 = 0; index1 < this.data.terrainCells.Count; ++index1)
      {
        if (this.data.terrainCells[index1].node.type != type)
        {
          for (int index2 = 0; index2 < line.Count; ++index2)
          {
            if (this.data.terrainCells[index1].poly.Contains((Vector2) line[index2]))
              this.data.terrainCells[index1].node.SetType(type);
          }
        }
      }
    }

    public string GetSubWorldType(Vector2I pos)
    {
      for (int index = 0; index < this.data.overworldCells.Count; ++index)
      {
        if (this.data.overworldCells[index].poly.Contains((Vector2) pos))
          return this.data.overworldCells[index].node.type;
      }
      return (string) null;
    }

    private List<Polygon> GetOverworldPolygons()
    {
      List<Polygon> polygonList = new List<Polygon>();
      for (int index = 0; index < this.data.overworldCells.Count; ++index)
        polygonList.Add(this.data.overworldCells[index].poly);
      return polygonList;
    }

    private List<Border> GetBorders(List<TerrainCell> cells)
    {
      List<Border> borderList = new List<Border>();
      HashSet<TerrainCell> terrainCellSet = new HashSet<TerrainCell>();
      for (int index = 0; index < cells.Count; ++index)
      {
        TerrainCell cell = cells[index];
        terrainCellSet.Add(cell);
        HashSet<KeyValuePair<uint, int>>.Enumerator enumerator = cell.site.neighbours.GetEnumerator();
        int num = 0;
        while (enumerator.MoveNext())
        {
          KeyValuePair<uint, int> neighborId = enumerator.Current;
          TerrainCell terrainCell = cells.Find((Predicate<TerrainCell>) (n => (int) n.site.id == (int) neighborId.Key));
          if (terrainCell == null || !terrainCellSet.Contains(terrainCell))
            ;
          ++num;
        }
      }
      return borderList;
    }

    private void ProcessByTerrainCell(
      Sim.Cell[] map_cells,
      float[] bgTemp,
      Sim.DiseaseCell[] dcs,
      WorldGen.OfflineCallbackFunction updateProgressFn)
    {
      int num1 = updateProgressFn(UI.WORLDGEN.PROCESSING.key, 0.0f, WorldGenProgressStages.Stages.Processing) ? 1 : 0;
      SeededRandom rnd = new SeededRandom(this.data.globalTerrainSeed);
      try
      {
        for (int index = 0; index < this.data.terrainCells.Count; ++index)
        {
          int num2 = updateProgressFn(UI.WORLDGEN.PROCESSING.key, (float) (100.0 * ((double) index / (double) this.data.terrainCells.Count)), WorldGenProgressStages.Stages.Processing) ? 1 : 0;
          this.data.terrainCells[index].Process(this, map_cells, bgTemp, dcs, this.data.world, rnd);
        }
      }
      catch (Exception ex)
      {
        string message = ex.Message;
        string stackTrace = ex.StackTrace;
        int num2 = updateProgressFn(new StringKey("Exception in TerrainCell.Process"), -1f, WorldGenProgressStages.Stages.Failure) ? 1 : 0;
        Debug.LogError((object) ("Error:" + message + "\n" + stackTrace));
      }
      List<Border> borderList = new List<Border>();
      int num3 = updateProgressFn(UI.WORLDGEN.BORDERS.key, 0.0f, WorldGenProgressStages.Stages.Borders) ? 1 : 0;
      try
      {
        List<ProcGen.Map.Edge> edgesWithTag1 = this.data.worldLayout.overworldGraph.GetEdgesWithTag(WorldGenTags.EdgeUnpassable);
        for (int index = 0; index < edgesWithTag1.Count; ++index)
        {
          ProcGen.Map.Edge edge = edgesWithTag1[index];
          if (edge.site0 != edge.site1)
          {
            TerrainCell a = this.data.overworldCells.Find((Predicate<TerrainCell>) (c => c.node.node == edge.site0.node));
            TerrainCell b = this.data.overworldCells.Find((Predicate<TerrainCell>) (c => c.node.node == edge.site1.node));
            Debug.Assert(a != null && b != null, (object) "NULL Terrainell nodes with EdgeUnpassable");
            borderList.Add(new Border(new Neighbors(a, b), edge.corner0.position, edge.corner1.position)
            {
              element = SettingsCache.borders["impenetrable"],
              width = (float) rnd.RandomRange(2, 3)
            });
          }
        }
        List<ProcGen.Map.Edge> edgesWithTag2 = this.data.worldLayout.overworldGraph.GetEdgesWithTag(WorldGenTags.EdgeClosed);
        for (int index1 = 0; index1 < edgesWithTag2.Count; ++index1)
        {
          ProcGen.Map.Edge edge = edgesWithTag2[index1];
          if (edge.site0 != edge.site1 && !edgesWithTag1.Contains(edge))
          {
            TerrainCell a = this.data.overworldCells.Find((Predicate<TerrainCell>) (c => c.node.node == edge.site0.node));
            TerrainCell b = this.data.overworldCells.Find((Predicate<TerrainCell>) (c => c.node.node == edge.site1.node));
            Debug.Assert(a != null && b != null, (object) "NULL Terraincell nodes with EdgeClosed");
            string borderOverride1 = this.Settings.GetSubWorld(a.node.type).borderOverride;
            string borderOverride2 = this.Settings.GetSubWorld(b.node.type).borderOverride;
            string index2 = string.IsNullOrEmpty(borderOverride2) || string.IsNullOrEmpty(borderOverride1) ? (!string.IsNullOrEmpty(borderOverride2) || !string.IsNullOrEmpty(borderOverride1) ? (string.IsNullOrEmpty(borderOverride2) ? borderOverride1 : borderOverride2) : "hardToDig") : ((double) rnd.RandomValue() <= 0.5 ? borderOverride1 : borderOverride2);
            borderList.Add(new Border(new Neighbors(a, b), edge.corner0.position, edge.corner1.position)
            {
              element = SettingsCache.borders[index2],
              width = rnd.RandomRange(1f, 2.5f)
            });
          }
        }
      }
      catch (Exception ex)
      {
        string message = ex.Message;
        string stackTrace = ex.StackTrace;
        int num2 = updateProgressFn(new StringKey("Exception in Border creation"), -1f, WorldGenProgressStages.Stages.Failure) ? 1 : 0;
        Debug.LogError((object) ("Error:" + message + " " + stackTrace));
      }
      try
      {
        if (this.data.world.defaultTemp == null)
          this.data.world.defaultTemp = new float[this.data.world.density.Length];
        for (int index = 0; index < this.data.world.defaultTemp.Length; ++index)
          this.data.world.defaultTemp[index] = bgTemp[index];
      }
      catch (Exception ex)
      {
        string message = ex.Message;
        string stackTrace = ex.StackTrace;
        int num2 = updateProgressFn(new StringKey("Exception in border.defaultTemp"), -1f, WorldGenProgressStages.Stages.Failure) ? 1 : 0;
        Debug.LogError((object) ("Error:" + message + " " + stackTrace));
      }
      try
      {
        TerrainCell.SetValuesFunction SetValues = (TerrainCell.SetValuesFunction) ((index, elem, pd, dc) =>
        {
          if (Grid.IsValidCell(index))
          {
            if (TerrainCell.GetHighPriorityClaimCells().Contains(index))
              return;
            if ((elem as Element).HasTag(GameTags.Special))
              pd = (elem as Element).defaultValues;
            map_cells[index].SetValues(elem as Element, pd, ElementLoader.elements);
            dcs[index] = dc;
          }
          else
            Debug.LogError((object) ("Process::SetValuesFunction Index [" + (object) index + "] is not valid. cells.Length [" + (object) map_cells.Length + "]"));
        });
        for (int index = 0; index < borderList.Count; ++index)
        {
          Border border = borderList[index];
          SubWorld subWorld1 = this.Settings.GetSubWorld(border.neighbors.n0.node.type);
          SubWorld subWorld2 = this.Settings.GetSubWorld(border.neighbors.n1.node.type);
          float temperatureMin = Mathf.Min(SettingsCache.temperatures[subWorld1.temperatureRange].min, SettingsCache.temperatures[subWorld2.temperatureRange].min);
          float temperatureRange = Mathf.Max(SettingsCache.temperatures[subWorld1.temperatureRange].max, SettingsCache.temperatures[subWorld2.temperatureRange].max) - temperatureMin;
          border.Stagger(rnd, (float) rnd.RandomRange(8, 13), (float) rnd.RandomRange(2, 5));
          border.ConvertToMap(this.data.world, SetValues, temperatureMin, temperatureRange, rnd);
        }
      }
      catch (Exception ex)
      {
        string message = ex.Message;
        string stackTrace = ex.StackTrace;
        int num2 = updateProgressFn(new StringKey("Exception in border.ConvertToMap"), -1f, WorldGenProgressStages.Stages.Failure) ? 1 : 0;
        Debug.LogError((object) ("Error:" + message + " " + stackTrace));
      }
    }

    private void DrawBorder(Chunk chunk, int thickness, int range, SeededRandom rnd)
    {
      if (Mathf.Abs(chunk.offset.x % this.SubWorldSize.x) == 0)
      {
        int num = 0;
        for (int index1 = this.ChunkEdgeSize - 1; index1 >= 0; --index1)
        {
          num = Mathf.Max(-range, Mathf.Min(num + rnd.RandomRange(-2, 2), range));
          for (int index2 = 0; index2 < thickness + num; ++index2)
            chunk.overrides[index2 + this.ChunkEdgeSize * index1] = 100f;
        }
      }
      if (Mathf.Abs((chunk.offset.x + this.ChunkEdgeSize) % this.SubWorldSize.x) == 0)
      {
        int num = 0;
        for (int index1 = this.ChunkEdgeSize - 1; index1 >= 0; --index1)
        {
          num = Mathf.Max(-range, Mathf.Min(num + rnd.RandomRange(-2, 2), range));
          for (int index2 = 0; index2 < thickness + num; ++index2)
            chunk.overrides[this.ChunkEdgeSize - 1 - index2 + this.ChunkEdgeSize * index1] = 100f;
        }
      }
      if (Mathf.Abs(chunk.offset.y % this.SubWorldSize.y) == 0)
      {
        int num = 0;
        for (int index1 = 0; index1 < this.ChunkEdgeSize; ++index1)
        {
          num = Mathf.Max(-range, Mathf.Min(num + rnd.RandomRange(-2, 2), range));
          for (int index2 = 0; index2 < thickness + num; ++index2)
            chunk.overrides[index1 + this.ChunkEdgeSize * index2] = 100f;
        }
      }
      if (Mathf.Abs((chunk.offset.y + this.ChunkEdgeSize) % this.SubWorldSize.y) != 0)
        return;
      int num1 = 0;
      for (int index1 = 0; index1 < this.ChunkEdgeSize; ++index1)
      {
        num1 = Mathf.Max(-range, Mathf.Min(num1 + rnd.RandomRange(-2, 2), range));
        for (int index2 = 0; index2 < thickness + num1; ++index2)
          chunk.overrides[index1 + this.ChunkEdgeSize * (this.ChunkEdgeSize - 1 - index2)] = 100f;
      }
    }

    private void DrawWorldBorder(
      Sim.Cell[] cells,
      Chunk world,
      SeededRandom rnd,
      HashSet<int> borderCells,
      WorldGen.OfflineCallbackFunction updateProgressFn)
    {
      bool boolSetting = this.Settings.GetBoolSetting("DrawWorldBorderTop");
      int intSetting1 = this.Settings.GetIntSetting("WorldBorderThickness");
      int intSetting2 = this.Settings.GetIntSetting("WorldBorderRange");
      byte new_elem_idx = (byte) ElementLoader.elements.IndexOf(WorldGen.unobtaniumElement);
      float temperature = WorldGen.unobtaniumElement.defaultValues.temperature;
      float mass = WorldGen.unobtaniumElement.defaultValues.mass;
      int num1 = 0;
      int num2 = 0;
      int num3 = updateProgressFn(UI.WORLDGEN.DRAWWORLDBORDER.key, 0.0f, WorldGenProgressStages.Stages.DrawWorldBorder) ? 1 : 0;
      int num4 = world.size.y - 32;
      if (!boolSetting)
      {
        num4 = Math.Max(0, num4 - intSetting1 - 2 * intSetting2);
        num1 = -intSetting2;
        num2 = -intSetting2;
      }
      for (int y = num4; y >= 0; --y)
      {
        int num5 = updateProgressFn(UI.WORLDGEN.DRAWWORLDBORDER.key, (float) ((double) y / (double) num4 * 0.330000013113022), WorldGenProgressStages.Stages.DrawWorldBorder) ? 1 : 0;
        num1 = Mathf.Max(-intSetting2, Mathf.Min(num1 + rnd.RandomRange(-2, 2), intSetting2));
        for (int x = 0; x < intSetting1 + num1; ++x)
        {
          int cell = Grid.XYToCell(x, y);
          borderCells.Add(cell);
          cells[cell].SetValues(new_elem_idx, temperature, mass);
        }
        num2 = Mathf.Max(-intSetting2, Mathf.Min(num2 + rnd.RandomRange(-2, 2), intSetting2));
        for (int index = 0; index < intSetting1 + num2; ++index)
        {
          int cell = Grid.XYToCell(world.size.x - 1 - index, y);
          borderCells.Add(cell);
          cells[cell].SetValues(new_elem_idx, temperature, mass);
        }
      }
      int num6 = 0;
      int num7 = 0;
      for (int x = 0; x < world.size.x; ++x)
      {
        int num5 = updateProgressFn(UI.WORLDGEN.DRAWWORLDBORDER.key, (float) ((double) x / (double) world.size.x * 0.660000026226044 + 0.330000013113022), WorldGenProgressStages.Stages.DrawWorldBorder) ? 1 : 0;
        num6 = Mathf.Max(-intSetting2, Mathf.Min(num6 + rnd.RandomRange(-2, 2), intSetting2));
        for (int y = 0; y < intSetting1 + num6; ++y)
        {
          int cell = Grid.XYToCell(x, y);
          borderCells.Add(cell);
          cells[cell].SetValues(new_elem_idx, temperature, mass);
        }
        if (boolSetting)
        {
          num7 = Mathf.Max(-intSetting2, Mathf.Min(num7 + rnd.RandomRange(-2, 2), intSetting2));
          for (int index = 0; index < intSetting1 + num7; ++index)
          {
            int cell = Grid.XYToCell(x, world.size.y - 1 - index);
            borderCells.Add(cell);
            cells[cell].SetValues(new_elem_idx, temperature, mass);
          }
        }
      }
    }

    private void SetupNoise(WorldGen.OfflineCallbackFunction updateProgressFn)
    {
      int num1 = updateProgressFn(UI.WORLDGEN.BUILDNOISESOURCE.key, 0.0f, WorldGenProgressStages.Stages.SetupNoise) ? 1 : 0;
      this.heatSource = this.BuildNoiseSource(this.data.world.size.x, this.data.world.size.y, "noise/Heat");
      int num2 = updateProgressFn(UI.WORLDGEN.BUILDNOISESOURCE.key, 100f, WorldGenProgressStages.Stages.SetupNoise) ? 1 : 0;
    }

    public NoiseMapBuilderPlane BuildNoiseSource(
      int width,
      int height,
      string name)
    {
      ProcGen.Noise.Tree tree = SettingsCache.noise.GetTree(name, SettingsCache.GetPath());
      Debug.Assert(tree != null, (object) name);
      return this.BuildNoiseSource(width, height, tree);
    }

    public NoiseMapBuilderPlane BuildNoiseSource(
      int width,
      int height,
      ProcGen.Noise.Tree tree)
    {
      Vector2f lowerBound = tree.settings.lowerBound;
      Vector2f upperBound = tree.settings.upperBound;
      Debug.Assert(((double) lowerBound.x < (double) upperBound.x ? 1 : 0) != 0, (object) ("BuildNoiseSource X range broken [l: " + (object) lowerBound.x + " h: " + (object) upperBound.x + "]"));
      Debug.Assert(((double) lowerBound.y < (double) upperBound.y ? 1 : 0) != 0, (object) ("BuildNoiseSource Y range broken [l: " + (object) lowerBound.y + " h: " + (object) upperBound.y + "]"));
      Debug.Assert(width > 0, (object) ("BuildNoiseSource width <=0: [" + (object) width + "]"));
      Debug.Assert(height > 0, (object) ("BuildNoiseSource height <=0: [" + (object) height + "]"));
      NoiseMapBuilderPlane noiseMapBuilderPlane = new NoiseMapBuilderPlane(lowerBound.x, upperBound.x, lowerBound.y, upperBound.y, false);
      noiseMapBuilderPlane.SetSize(width, height);
      noiseMapBuilderPlane.SourceModule = (IModule) tree.BuildFinalModule(this.data.globalNoiseSeed);
      return noiseMapBuilderPlane;
    }

    private void GetMinMaxDataValues(float[] data, int width, int height)
    {
    }

    public static NoiseMap BuildNoiseMap(
      Vector2 offset,
      float zoom,
      NoiseMapBuilderPlane nmbp,
      int width,
      int height,
      NoiseMapBuilderCallback cb = null)
    {
      double x = (double) offset.x;
      double y = (double) offset.y;
      if ((double) zoom == 0.0)
        zoom = 0.01f;
      double num1 = x * (double) zoom;
      double num2 = (x + (double) width) * (double) zoom;
      double num3 = y * (double) zoom;
      double num4 = (y + (double) height) * (double) zoom;
      NoiseMap noiseMap = new NoiseMap(width, height);
      nmbp.NoiseMap = (IMap2D<float>) noiseMap;
      nmbp.SetBounds((float) num1, (float) num2, (float) num3, (float) num4);
      nmbp.CallBack = cb;
      nmbp.Build();
      return noiseMap;
    }

    public static float[] GenerateNoise(
      Vector2 offset,
      float zoom,
      NoiseMapBuilderPlane nmbp,
      int width,
      int height,
      NoiseMapBuilderCallback cb = null)
    {
      NoiseMap noiseMap = WorldGen.BuildNoiseMap(offset, zoom, nmbp, width, height, cb);
      float[] buffer = new float[noiseMap.Width * noiseMap.Height];
      noiseMap.CopyTo(ref buffer);
      return buffer;
    }

    public static void Normalise(float[] data)
    {
      Debug.Assert(data != null && data.Length > 0, (object) "MISSING DATA FOR NORMALIZE");
      float b1 = float.MaxValue;
      float b2 = float.MinValue;
      for (int index = 0; index < data.Length; ++index)
      {
        b1 = Mathf.Min(data[index], b1);
        b2 = Mathf.Max(data[index], b2);
      }
      float num = b2 - b1;
      for (int index = 0; index < data.Length; ++index)
        data[index] = (data[index] - b1) / num;
    }

    private void GenerateUnChunkedNoise(WorldGen.OfflineCallbackFunction updateProgressFn)
    {
      Vector2 offset = new Vector2(0.0f, 0.0f);
      int num1 = updateProgressFn(UI.WORLDGEN.GENERATENOISE.key, 0.0f, WorldGenProgressStages.Stages.GenerateNoise) ? 1 : 0;
      int num2;
      NoiseMapBuilderCallback mapBuilderCallback = (NoiseMapBuilderCallback) (line => num2 = updateProgressFn(UI.WORLDGEN.GENERATENOISE.key, (float) (int) (25.0 * ((double) line / (double) this.data.world.size.y)), WorldGenProgressStages.Stages.GenerateNoise) ? 1 : 0);
      int num3;
      NoiseMapBuilderCallback cb = (NoiseMapBuilderCallback) (line => num3 = updateProgressFn(UI.WORLDGEN.GENERATENOISE.key, (float) (int) (25.0 + 25.0 * ((double) line / (double) this.data.world.size.y)), WorldGenProgressStages.Stages.GenerateNoise) ? 1 : 0);
      if (cb == null)
        Debug.LogError((object) "nupd is null");
      this.data.world.heatOffset = WorldGen.GenerateNoise(offset, SettingsCache.noise.GetZoomForTree("noise/Heat"), this.heatSource, this.data.world.size.x, this.data.world.size.y, cb);
      this.data.world.data = new float[this.data.world.heatOffset.Length];
      this.data.world.density = new float[this.data.world.heatOffset.Length];
      this.data.world.overrides = new float[this.data.world.heatOffset.Length];
      int num4 = updateProgressFn(UI.WORLDGEN.NORMALISENOISE.key, 50f, WorldGenProgressStages.Stages.GenerateNoise) ? 1 : 0;
      if (SettingsCache.noise.ShouldNormaliseTree("noise/Heat"))
        WorldGen.Normalise(this.data.world.heatOffset);
      int num5 = updateProgressFn(UI.WORLDGEN.NORMALISENOISE.key, 100f, WorldGenProgressStages.Stages.GenerateNoise) ? 1 : 0;
    }

    public void WriteOverWorldNoise(WorldGen.OfflineCallbackFunction updateProgressFn)
    {
      float perCell = 100f / (float) this.OverworldCells.Count;
      float currentProgress = 0.0f;
      foreach (TerrainCell overworldCell in this.OverworldCells)
      {
        ProcGen.Noise.Tree tree1 = SettingsCache.noise.GetTree("noise/Default", SettingsCache.GetPath());
        ProcGen.Noise.Tree tree2 = SettingsCache.noise.GetTree("noise/DefaultCave", SettingsCache.GetPath());
        ProcGen.Noise.Tree tree3 = SettingsCache.noise.GetTree("noise/DefaultDensity", SettingsCache.GetPath());
        SubWorld subWorld = this.Settings.GetSubWorld(overworldCell.node.type);
        if (subWorld == null)
        {
          Debug.Log((object) ("Couldnt find Subworld for overworld node [" + overworldCell.node.type + "] using defaults"));
        }
        else
        {
          if (subWorld.biomeNoise != null)
          {
            ProcGen.Noise.Tree tree4 = SettingsCache.noise.GetTree(subWorld.biomeNoise);
            if (tree4 != null)
              tree1 = tree4;
          }
          if (subWorld.overrideNoise != null)
          {
            ProcGen.Noise.Tree tree4 = SettingsCache.noise.GetTree(subWorld.overrideNoise);
            if (tree4 != null)
              tree2 = tree4;
          }
          if (subWorld.densityNoise != null)
          {
            ProcGen.Noise.Tree tree4 = SettingsCache.noise.GetTree(subWorld.densityNoise);
            if (tree4 != null)
              tree3 = tree4;
          }
        }
        int width = (int) Mathf.Ceil(overworldCell.poly.bounds.width + 1f);
        int height = (int) Mathf.Ceil(overworldCell.poly.bounds.height + 1f);
        int num1 = (int) Mathf.Floor(overworldCell.poly.bounds.xMin - 1f);
        int num2 = (int) Mathf.Floor(overworldCell.poly.bounds.yMin - 1f);
        Vector2 offset = new Vector2((float) num1, (float) num2);
        Vector2 point = offset;
        int num3;
        NoiseMapBuilderCallback cb = (NoiseMapBuilderCallback) (line => num3 = updateProgressFn(UI.WORLDGEN.GENERATENOISE.key, (float) (int) ((double) currentProgress + (double) perCell * ((double) line / (double) height)), WorldGenProgressStages.Stages.NoiseMapBuilder) ? 1 : 0);
        NoiseMapBuilderPlane nmbp1 = this.BuildNoiseSource(width, height, tree1);
        NoiseMap noiseMap1 = WorldGen.BuildNoiseMap(offset, tree1.settings.zoom, nmbp1, width, height, cb);
        NoiseMapBuilderPlane nmbp2 = this.BuildNoiseSource(width, height, tree2);
        NoiseMap noiseMap2 = WorldGen.BuildNoiseMap(offset, tree2.settings.zoom, nmbp2, width, height, cb);
        NoiseMapBuilderPlane nmbp3 = this.BuildNoiseSource(width, height, tree3);
        NoiseMap noiseMap3 = WorldGen.BuildNoiseMap(offset, tree3.settings.zoom, nmbp3, width, height, cb);
        float b1 = float.MaxValue;
        float b2 = float.MinValue;
        float b3 = float.MaxValue;
        float b4 = float.MinValue;
        float b5 = float.MaxValue;
        float b6 = float.MinValue;
        List<int> intList = new List<int>();
        for (point.x = (float) (int) Mathf.Floor(overworldCell.poly.bounds.xMin); (double) point.x <= (double) (int) Mathf.Ceil(overworldCell.poly.bounds.xMax); ++point.x)
        {
          for (point.y = (float) (int) Mathf.Floor(overworldCell.poly.bounds.yMin); (double) point.y <= (double) (int) Mathf.Ceil(overworldCell.poly.bounds.yMax); ++point.y)
          {
            if (overworldCell.poly.PointInPolygon(point))
            {
              int cell = Grid.XYToCell((int) point.x, (int) point.y);
              intList.Add(cell);
              int x = (int) point.x - num1;
              int y = (int) point.y - num2;
              this.BaseNoiseMap[cell] = noiseMap1.GetValue(x, y);
              this.OverrideMap[cell] = noiseMap2.GetValue(x, y);
              this.DensityMap[cell] = noiseMap3.GetValue(x, y);
              b1 = Mathf.Min(this.BaseNoiseMap[cell], b1);
              b2 = Mathf.Max(this.BaseNoiseMap[cell], b2);
              b3 = Mathf.Min(this.OverrideMap[cell], b3);
              b4 = Mathf.Max(this.OverrideMap[cell], b4);
              b5 = Mathf.Min(this.DensityMap[cell], b5);
              b6 = Mathf.Max(this.DensityMap[cell], b6);
            }
          }
        }
        if (tree1.settings.normalise)
        {
          float num4 = b2 - b1;
          for (int index = 0; index < intList.Count; ++index)
            this.BaseNoiseMap[intList[index]] = (this.BaseNoiseMap[intList[index]] - b1) / num4;
        }
        if (tree2.settings.normalise)
        {
          float num4 = b4 - b3;
          for (int index = 0; index < intList.Count; ++index)
            this.OverrideMap[intList[index]] = (this.OverrideMap[intList[index]] - b3) / num4;
        }
        if (tree3.settings.normalise)
        {
          float num4 = b6 - b5;
          for (int index = 0; index < intList.Count; ++index)
            this.DensityMap[intList[index]] = (this.DensityMap[intList[index]] - b5) / num4;
        }
        currentProgress += perCell;
      }
    }

    private float GetValue(Chunk chunk, Vector2I pos)
    {
      int index = pos.x + this.data.world.size.x * pos.y;
      if (index < 0 || index >= chunk.data.Length)
        throw new ArgumentOutOfRangeException("chunkDataIndex [" + (object) index + "]", "chunk data length [" + (object) chunk.data.Length + "]");
      return chunk.data[index];
    }

    public bool InChunkRange(Chunk chunk, Vector2I pos)
    {
      int num = pos.x + this.data.world.size.x * pos.y;
      return num >= 0 && num < chunk.data.Length;
    }

    private TerrainCell.ElementOverride GetElementFromBiomeElementTable(
      Chunk chunk,
      Vector2I pos,
      List<ElementGradient> table,
      float erode)
    {
      float num = this.GetValue(chunk, pos) * erode;
      TerrainCell.ElementOverride elementOverride = TerrainCell.GetElementOverride(WorldGen.voidElement.tag.ToString(), (SampleDescriber.Override) null);
      if (table.Count == 0)
        return elementOverride;
      for (int index = 0; index < table.Count; ++index)
      {
        Debug.Assert(table[index].content != null, (object) index.ToString());
        if ((double) num < (double) table[index].maxValue)
          return TerrainCell.GetElementOverride(table[index].content, table[index].overrides);
      }
      return TerrainCell.GetElementOverride(table[table.Count - 1].content, table[table.Count - 1].overrides);
    }

    public static bool CanLoad(string fileName)
    {
      if (fileName != null)
      {
        if (!(fileName == string.Empty))
        {
          try
          {
            if (!File.Exists(fileName))
              return false;
            using (BinaryReader binaryReader = new BinaryReader((Stream) File.Open(fileName, FileMode.Open)))
              return binaryReader.BaseStream.CanRead;
          }
          catch (FileNotFoundException ex)
          {
            return false;
          }
          catch (Exception ex)
          {
            DebugUtil.LogWarningArgs((object) ("Failed to read " + fileName + "\n" + ex.ToString()));
            return false;
          }
        }
      }
      return false;
    }

    public void SaveWorldGen()
    {
      try
      {
        Manager.Clear();
        WorldGenSave worldGenSave = new WorldGenSave();
        worldGenSave.version = new Vector2I(1, 1);
        worldGenSave.stats = this.stats;
        worldGenSave.data = this.data;
        using (MemoryStream memoryStream = new MemoryStream())
        {
          using (BinaryWriter writer = new BinaryWriter((Stream) memoryStream))
          {
            try
            {
              Serializer.Serialize((object) worldGenSave, writer);
            }
            catch (Exception ex)
            {
              DebugUtil.LogErrorArgs((object) "Couldn't serialize", (object) ex.Message, (object) ex.StackTrace);
            }
          }
          using (BinaryWriter writer = new BinaryWriter((Stream) File.Open(WorldGen.WORLDGEN_SAVE_FILENAME, FileMode.Create)))
          {
            Manager.SerializeDirectory(writer);
            writer.Write(memoryStream.ToArray());
          }
        }
      }
      catch (Exception ex)
      {
        DebugUtil.LogErrorArgs((object) "Couldn't write", (object) ex.Message, (object) ex.StackTrace);
      }
    }

    public bool LoadWorldGen()
    {
      try
      {
        WorldGenSave worldGenSave = new WorldGenSave();
        FastReader fastReader = new FastReader(File.ReadAllBytes(WorldGen.WORLDGEN_SAVE_FILENAME));
        Manager.DeserializeDirectory((IReader) fastReader);
        Deserializer.Deserialize((object) worldGenSave, (IReader) fastReader);
        this.stats = worldGenSave.stats;
        this.data = worldGenSave.data;
        if (worldGenSave.version.x != 1 || worldGenSave.version.y > 1)
        {
          Debug.LogError((object) ("LoadWorldGenSim Error! Wrong save version Current: [" + (object) 1 + "." + (object) 1 + "] File: [" + (object) worldGenSave.version.x + "." + (object) worldGenSave.version.y + "]"));
          this.wasLoaded = false;
        }
        else
          this.wasLoaded = true;
      }
      catch (Exception ex)
      {
        DebugUtil.LogErrorArgs((object) "LoadWorldGenSim Error!\n", (object) ex.Message, (object) ex.StackTrace);
        this.wasLoaded = false;
      }
      return this.wasLoaded;
    }

    public SimSaveFileStructure LoadWorldGenSim()
    {
      this.LoadWorldGen();
      SimSaveFileStructure saveFileStructure = new SimSaveFileStructure();
      try
      {
        FastReader fastReader = new FastReader(File.ReadAllBytes(WorldGen.SIM_SAVE_FILENAME));
        Manager.DeserializeDirectory((IReader) fastReader);
        Deserializer.Deserialize((object) saveFileStructure, (IReader) fastReader);
      }
      catch (Exception ex)
      {
        DebugUtil.LogErrorArgs((object) "LoadWorldGenSim Error!\n", (object) ex.Message, (object) ex.StackTrace);
        this.wasLoaded = false;
        return (SimSaveFileStructure) null;
      }
      if (saveFileStructure.worldDetail == null)
        Debug.LogError((object) "Detail is null");
      else
        SaveLoader.Instance.SetWorldDetail(saveFileStructure.worldDetail);
      return saveFileStructure;
    }

    public void DrawDebug()
    {
    }

    public delegate void ResetFunction(GameSpawnData gsd);

    public delegate bool OfflineCallbackFunction(
      StringKey stringKeyRoot,
      float completePercent,
      WorldGenProgressStages.Stages stage);

    public enum GenerateSection
    {
      SolarSystem,
      WorldNoise,
      WorldLayout,
      RenderToMap,
      CollectSpawners,
    }

    [System.Flags]
    public enum DebugFlags
    {
      SitePoly = 4,
    }
  }
}
