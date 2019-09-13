// Decompiled with JetBrains decompiler
// Type: SaveLoader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Ionic.Zlib;
using Klei;
using Klei.AI;
using Klei.CustomSettings;
using KMod;
using KSerialization;
using Newtonsoft.Json;
using ProcGenGame;
using STRINGS;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveLoader : KMonoBehaviour
{
  private bool compressSaveData = true;
  [MyCmpGet]
  private GridSettings gridSettings;
  private bool saveFileCorrupt;
  public bool saveAsText;
  public bool zipStreams;
  public const string MAINMENU_LEVELNAME = "launchscene";
  public const string FRONTEND_LEVELNAME = "frontend";
  public const string BACKEND_LEVELNAME = "backend";
  public const string SAVE_EXTENSION = ".sav";
  public const int MAX_AUTOSAVE_FILES = 10;
  [NonSerialized]
  public SaveManager saveManager;
  private const string CorruptFileSuffix = "_";
  private bool mustRestartOnFail;
  public WorldGen worldGen;
  public const string METRIC_SAVED_PREFAB_KEY = "SavedPrefabs";
  public const string METRIC_IS_AUTO_SAVE_KEY = "IsAutoSave";
  public const string METRIC_WAS_DEBUG_EVER_USED = "WasDebugEverUsed";
  public const string METRIC_RESOURCES_ACCESSIBLE_KEY = "ResourcesAccessible";
  public const string METRIC_DAILY_REPORT_KEY = "DailyReport";
  public const string METRIC_MINION_METRICS_KEY = "MinionMetrics";
  public const string METRIC_CUSTOM_GAME_SETTINGS = "CustomGameSettings";
  public const string METRIC_PERFORMANCE_MEASUREMENTS = "PerformanceMeasurements";
  public const string METRIC_FRAME_TIME = "AverageFrameTime";
  private static bool force_infinity;

  public bool loadedFromSave { get; private set; }

  public static void DestroyInstance()
  {
    SaveLoader.Instance = (SaveLoader) null;
  }

  public static SaveLoader Instance { get; private set; }

  public System.Action OnWorldGenComplete { get; set; }

  public SaveGame.Header LoadedHeader { get; private set; }

  public SaveGame.GameInfo GameInfo { get; private set; }

  protected override void OnPrefabInit()
  {
    SaveLoader.Instance = this;
    this.saveManager = this.GetComponent<SaveManager>();
  }

  private void MoveCorruptFile(string filename)
  {
    try
    {
    }
    catch
    {
      File.Replace(filename, filename + "_", filename + "_.bak", true);
    }
  }

  protected override void OnSpawn()
  {
    string activeSaveFilePath = SaveLoader.GetActiveSaveFilePath();
    if (WorldGen.CanLoad(activeSaveFilePath))
    {
      // ISSUE: reference to a compiler-generated field
      if (SaveLoader.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        SaveLoader.\u003C\u003Ef__mg\u0024cache0 = new Sim.GAME_MessageHandler(Sim.DLL_MessageHandler);
      }
      // ISSUE: reference to a compiler-generated field
      Sim.SIM_Initialize(SaveLoader.\u003C\u003Ef__mg\u0024cache0);
      SimMessages.CreateSimElementsTable(ElementLoader.elements);
      SimMessages.CreateDiseaseTable();
      this.loadedFromSave = true;
      this.loadedFromSave = this.Load(activeSaveFilePath);
      this.saveFileCorrupt = !this.loadedFromSave;
      if (!this.loadedFromSave)
      {
        SaveLoader.SetActiveSaveFilePath((string) null);
        if (this.mustRestartOnFail)
        {
          this.MoveCorruptFile(activeSaveFilePath);
          Sim.Shutdown();
          App.LoadScene("frontend");
          return;
        }
      }
    }
    if (this.loadedFromSave)
      return;
    Sim.Shutdown();
    if (!string.IsNullOrEmpty(activeSaveFilePath))
      DebugUtil.LogArgs((object) ("Couldn't load [" + activeSaveFilePath + "]"));
    if (this.saveFileCorrupt)
      this.MoveCorruptFile(activeSaveFilePath);
    bool flag = WorldGen.CanLoad(WorldGen.SIM_SAVE_FILENAME);
    if (flag && this.LoadFromWorldGen())
      return;
    DebugUtil.LogWarningArgs((object) "Couldn't start new game with current world gen, moving file");
    if (flag)
    {
      KMonoBehaviour.isLoadingScene = true;
      this.MoveCorruptFile(WorldGen.SIM_SAVE_FILENAME);
    }
    App.LoadScene("frontend");
  }

  public static byte[] CompressContents(byte[] uncompressed)
  {
    return SaveLoader.CompressContents(uncompressed, uncompressed.Length);
  }

  public static byte[] CompressContents(byte[] uncompressed, int length)
  {
    using (MemoryStream memoryStream = new MemoryStream(length))
    {
      using (ZlibStream zlibStream = new ZlibStream((Stream) memoryStream, CompressionMode.Compress, CompressionLevel.BestSpeed))
      {
        zlibStream.Write(uncompressed, 0, length);
        zlibStream.Flush();
      }
      memoryStream.Flush();
      return memoryStream.ToArray();
    }
  }

  private byte[] FloatToBytes(float[] floats)
  {
    byte[] numArray = new byte[floats.Length * 4];
    Buffer.BlockCopy((Array) floats, 0, (Array) numArray, 0, numArray.Length);
    return numArray;
  }

  public static byte[] DecompressContents(byte[] compressed)
  {
    return ZlibStream.UncompressBuffer(compressed);
  }

  private float[] BytesToFloat(byte[] bytes)
  {
    float[] numArray = new float[bytes.Length / 4];
    Buffer.BlockCopy((Array) bytes, 0, (Array) numArray, 0, bytes.Length);
    return numArray;
  }

  private SaveFileRoot PrepSaveFile()
  {
    SaveFileRoot saveFileRoot = new SaveFileRoot();
    saveFileRoot.WidthInCells = Grid.WidthInCells;
    saveFileRoot.HeightInCells = Grid.HeightInCells;
    using (MemoryStream memoryStream = new MemoryStream())
    {
      using (BinaryWriter writer = new BinaryWriter((Stream) memoryStream))
        Sim.Save(writer);
      if (this.zipStreams)
        saveFileRoot.streamed["SimBZ"] = SaveLoader.CompressContents(memoryStream.ToArray());
      else
        saveFileRoot.streamed["Sim"] = memoryStream.ToArray();
    }
    if (this.zipStreams)
    {
      saveFileRoot.streamed["GridVisibleBZ"] = SaveLoader.CompressContents(Grid.Visible);
      saveFileRoot.streamed["GridSpawnableBZ"] = SaveLoader.CompressContents(Grid.Spawnable);
      saveFileRoot.streamed["GridDamageBZ"] = SaveLoader.CompressContents(this.FloatToBytes(Grid.Damage));
    }
    else
    {
      saveFileRoot.streamed["GridVisible"] = Grid.Visible;
      saveFileRoot.streamed["GridSpawnable"] = Grid.Spawnable;
      saveFileRoot.streamed["GridDamage"] = this.FloatToBytes(Grid.Damage);
    }
    Global.Instance.modManager.SendMetricsEvent();
    saveFileRoot.active_mods = new List<KMod.Label>();
    foreach (KMod.Mod mod in Global.Instance.modManager.mods)
    {
      if (mod.enabled)
        saveFileRoot.active_mods.Add(mod.label);
    }
    string str = Game.worldID == null ? CustomGameSettings.Instance.GetCurrentQualitySetting((SettingConfig) CustomGameSettingConfigs.World).id : Game.worldID;
    saveFileRoot.worldID = str;
    using (MemoryStream memoryStream = new MemoryStream())
    {
      using (BinaryWriter writer = new BinaryWriter((Stream) memoryStream))
        Camera.main.transform.parent.GetComponent<CameraController>().Save(writer);
      saveFileRoot.streamed["Camera"] = memoryStream.ToArray();
    }
    return saveFileRoot;
  }

  private void Save(BinaryWriter writer)
  {
    writer.WriteKleiString("world");
    Serializer.Serialize((object) this.PrepSaveFile(), writer);
    Game.SaveSettings(writer);
    this.saveManager.Save(writer);
    Game.Instance.Save(writer);
  }

  private bool Load(IReader reader)
  {
    Debug.Assert(reader.ReadKleiString() == "world");
    Deserializer deserializer = new Deserializer(reader);
    SaveFileRoot saveFileRoot = new SaveFileRoot();
    deserializer.Deserialize((object) saveFileRoot);
    if ((this.GameInfo.saveMajorVersion == 7 || this.GameInfo.saveMinorVersion < 8) && saveFileRoot.requiredMods != null)
    {
      saveFileRoot.active_mods = new List<KMod.Label>();
      foreach (ModInfo requiredMod in saveFileRoot.requiredMods)
        saveFileRoot.active_mods.Add(new KMod.Label()
        {
          id = requiredMod.assetID,
          version = (long) requiredMod.lastModifiedTime,
          distribution_platform = KMod.Label.DistributionPlatform.Steam,
          title = requiredMod.description
        });
      saveFileRoot.requiredMods.Clear();
    }
    KMod.Manager modManager = Global.Instance.modManager;
    modManager.Load(Content.LayerableFiles);
    if (!modManager.MatchFootprint(saveFileRoot.active_mods, Content.LayerableFiles | Content.Strings | Content.DLL | Content.Translation | Content.Animation))
      DebugUtil.LogWarningArgs((object) "Mod footprint of save file doesn't match current mod configuration");
    Global.Instance.modManager.SendMetricsEvent();
    string worldName = saveFileRoot.worldID;
    if (worldName == null)
    {
      try
      {
        worldName = CustomGameSettings.Instance.GetCurrentQualitySetting((SettingConfig) CustomGameSettingConfigs.World).id;
      }
      catch
      {
        worldName = "worlds/SandstoneDefault";
      }
    }
    Game.worldID = worldName;
    this.worldGen = new WorldGen(worldName, (List<string>) null);
    Game.LoadSettings(deserializer);
    GridSettings.Reset(saveFileRoot.WidthInCells, saveFileRoot.HeightInCells);
    Singleton<KBatchedAnimUpdater>.Instance.InitializeGrid();
    // ISSUE: reference to a compiler-generated field
    if (SaveLoader.\u003C\u003Ef__mg\u0024cache1 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SaveLoader.\u003C\u003Ef__mg\u0024cache1 = new Sim.GAME_MessageHandler(Sim.DLL_MessageHandler);
    }
    // ISSUE: reference to a compiler-generated field
    Sim.SIM_Initialize(SaveLoader.\u003C\u003Ef__mg\u0024cache1);
    SimMessages.CreateSimElementsTable(ElementLoader.elements);
    SimMessages.CreateDiseaseTable();
    if (Sim.Load(new FastReader(saveFileRoot.streamed["Sim"])) != 0)
    {
      DebugUtil.LogWarningArgs((object) "\n--- Error loading save ---\nSimDLL found bad data\n");
      Sim.Shutdown();
      return false;
    }
    SceneInitializer.Instance.PostLoadPrefabs();
    this.mustRestartOnFail = true;
    if (!this.saveManager.Load(reader))
    {
      Sim.Shutdown();
      DebugUtil.LogWarningArgs((object) "\n--- Error loading save ---\n");
      SaveLoader.SetActiveSaveFilePath((string) null);
      return false;
    }
    Grid.Visible = saveFileRoot.streamed["GridVisible"];
    if (saveFileRoot.streamed.ContainsKey("GridSpawnable"))
      Grid.Spawnable = saveFileRoot.streamed["GridSpawnable"];
    Grid.Damage = this.BytesToFloat(saveFileRoot.streamed["GridDamage"]);
    Game.Instance.Load(deserializer);
    CameraSaveData.Load(new FastReader(saveFileRoot.streamed["Camera"]));
    return true;
  }

  public static string GetSavePrefix()
  {
    return System.IO.Path.Combine(Util.RootFolder(), "save_files/");
  }

  public static string GetSavePrefixAndCreateFolder()
  {
    string savePrefix = SaveLoader.GetSavePrefix();
    if (!System.IO.Directory.Exists(savePrefix))
      System.IO.Directory.CreateDirectory(savePrefix);
    return savePrefix;
  }

  public static string GetAutoSavePrefix()
  {
    string path = System.IO.Path.Combine(SaveLoader.GetSavePrefixAndCreateFolder(), "auto_save/");
    if (!System.IO.Directory.Exists(path))
      System.IO.Directory.CreateDirectory(path);
    return path;
  }

  public static void SetActiveSaveFilePath(string path)
  {
    KPlayerPrefs.SetString("SaveFilenameKey/", path);
  }

  public static string GetActiveSaveFilePath()
  {
    return KPlayerPrefs.GetString("SaveFilenameKey/");
  }

  public static string GetAutosaveFilePath()
  {
    return SaveLoader.GetAutoSavePrefix() + "AutoSave Cycle 1.sav";
  }

  public static string GetActiveSaveFolder()
  {
    string activeSaveFilePath = SaveLoader.GetActiveSaveFilePath();
    if (!string.IsNullOrEmpty(activeSaveFilePath))
      return System.IO.Path.GetDirectoryName(activeSaveFilePath);
    return (string) null;
  }

  public static List<string> GetSaveFiles(string save_dir)
  {
    List<string> stringList = new List<string>();
    try
    {
      if (!System.IO.Directory.Exists(save_dir))
        System.IO.Directory.CreateDirectory(save_dir);
      string[] files = System.IO.Directory.GetFiles(save_dir, "*.sav", SearchOption.AllDirectories);
      List<SaveLoader.SaveFileEntry> saveFileEntryList = new List<SaveLoader.SaveFileEntry>();
      foreach (string path in files)
      {
        try
        {
          System.DateTime lastWriteTime = File.GetLastWriteTime(path);
          SaveLoader.SaveFileEntry saveFileEntry = new SaveLoader.SaveFileEntry()
          {
            path = path,
            timeStamp = lastWriteTime
          };
          saveFileEntryList.Add(saveFileEntry);
        }
        catch (Exception ex)
        {
          Debug.LogWarning((object) ("Problem reading file: " + path + "\n" + ex.ToString()));
        }
      }
      saveFileEntryList.Sort((Comparison<SaveLoader.SaveFileEntry>) ((x, y) => y.timeStamp.CompareTo(x.timeStamp)));
      foreach (SaveLoader.SaveFileEntry saveFileEntry in saveFileEntryList)
        stringList.Add(saveFileEntry.path);
    }
    catch (Exception ex)
    {
      string text = (string) null;
      if (ex is UnauthorizedAccessException)
        text = string.Format((string) UI.FRONTEND.SUPPORTWARNINGS.SAVE_DIRECTORY_READ_ONLY, (object) save_dir);
      else if (ex is IOException)
        text = string.Format((string) UI.FRONTEND.SUPPORTWARNINGS.SAVE_DIRECTORY_INSUFFICIENT_SPACE, (object) save_dir);
      if (text == null)
        throw ex;
      Util.KInstantiateUI(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, !((UnityEngine.Object) FrontEndManager.Instance == (UnityEngine.Object) null) ? FrontEndManager.Instance.gameObject : GameScreenManager.Instance.ssOverlayCanvas, true).GetComponent<ConfirmDialogScreen>().PopupConfirmDialog(text, (System.Action) null, (System.Action) null, (string) null, (System.Action) null, (string) null, (string) null, (string) null, (Sprite) null, true);
    }
    return stringList;
  }

  public static List<string> GetAllFiles()
  {
    return SaveLoader.GetSaveFiles(SaveLoader.GetSavePrefixAndCreateFolder());
  }

  public static string GetLatestSaveFile()
  {
    List<string> allFiles = SaveLoader.GetAllFiles();
    if (allFiles.Count == 0)
      return (string) null;
    return allFiles[0];
  }

  public void InitialSave()
  {
    string filename = SaveLoader.GetActiveSaveFilePath();
    if (string.IsNullOrEmpty(filename))
      filename = SaveLoader.GetAutosaveFilePath();
    else if (!filename.Contains(".sav"))
      filename += ".sav";
    this.Save(filename, false, true);
  }

  public string Save(string filename, bool isAutoSave = false, bool updateSavePointer = true)
  {
    KSerialization.Manager.Clear();
    this.ReportSaveMetrics(isAutoSave);
    RetireColonyUtility.SaveColonySummaryData();
    if (isAutoSave && !GenericGameSettings.instance.keepAllAutosaves)
    {
      List<string> saveFiles = SaveLoader.GetSaveFiles(System.IO.Path.GetDirectoryName(filename));
      for (int index = saveFiles.Count - 1; index >= 9; --index)
      {
        string path1 = saveFiles[index];
        try
        {
          Debug.Log((object) ("Deleting old autosave: " + path1));
          File.Delete(path1);
        }
        catch (Exception ex)
        {
          Debug.LogWarning((object) ("Problem deleting autosave: " + path1 + "\n" + ex.ToString()));
        }
        string path2 = System.IO.Path.ChangeExtension(path1, ".png");
        try
        {
          if (File.Exists(path2))
            File.Delete(path2);
        }
        catch (Exception ex)
        {
          Debug.LogWarning((object) ("Problem deleting autosave screenshot: " + path2 + "\n" + ex.ToString()));
        }
      }
    }
    byte[] buffer = (byte[]) null;
    using (MemoryStream memoryStream = new MemoryStream())
    {
      using (BinaryWriter writer = new BinaryWriter((Stream) memoryStream))
      {
        this.Save(writer);
        buffer = !this.compressSaveData ? memoryStream.ToArray() : SaveLoader.CompressContents(memoryStream.GetBuffer(), (int) memoryStream.Length);
      }
    }
    try
    {
      using (BinaryWriter writer = new BinaryWriter((Stream) File.Open(filename, FileMode.Create)))
      {
        SaveGame.Header header;
        byte[] saveHeader = SaveGame.Instance.GetSaveHeader(isAutoSave, this.compressSaveData, out header);
        writer.Write(header.buildVersion);
        writer.Write(header.headerSize);
        writer.Write(header.headerVersion);
        writer.Write(header.compression);
        writer.Write(saveHeader);
        KSerialization.Manager.SerializeDirectory(writer);
        writer.Write(buffer);
        Stats.Print();
      }
    }
    catch (Exception ex)
    {
      if (ex is UnauthorizedAccessException)
      {
        DebugUtil.LogArgs((object) ("UnauthorizedAccessException for " + filename));
        ((ConfirmDialogScreen) GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay)).PopupConfirmDialog(string.Format((string) UI.CRASHSCREEN.SAVEFAILED, (object) "Unauthorized Access Exception"), (System.Action) null, (System.Action) null, (string) null, (System.Action) null, (string) null, (string) null, (string) null, (Sprite) null, true);
        return SaveLoader.GetActiveSaveFilePath();
      }
      if (!(ex is IOException))
        throw ex;
      DebugUtil.LogArgs((object) ("IOException (probably out of disk space) for " + filename));
      ((ConfirmDialogScreen) GameScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject, GameScreenManager.UIRenderTarget.ScreenSpaceOverlay)).PopupConfirmDialog(string.Format((string) UI.CRASHSCREEN.SAVEFAILED, (object) "IOException. You may not have enough free space!"), (System.Action) null, (System.Action) null, (string) null, (System.Action) null, (string) null, (string) null, (string) null, (Sprite) null, true);
      return SaveLoader.GetActiveSaveFilePath();
    }
    if (updateSavePointer)
      SaveLoader.SetActiveSaveFilePath(filename);
    Game.Instance.timelapser.SaveColonyPreview(filename);
    DebugUtil.LogArgs((object) "Saved to", (object) ("[" + filename + "]"));
    GC.Collect();
    return filename;
  }

  public static SaveGame.GameInfo LoadHeader(string filename, out SaveGame.Header header)
  {
    using (BinaryReader br = new BinaryReader((Stream) File.Open(filename, FileMode.Open)))
    {
      header = SaveGame.GetHeader(br);
      return SaveGame.GetGameInfo(br.ReadBytes(header.headerSize));
    }
  }

  public bool Load(string filename)
  {
    SaveLoader.SetActiveSaveFilePath(filename);
    try
    {
      KSerialization.Manager.Clear();
      byte[] bytes = File.ReadAllBytes(filename);
      IReader reader = (IReader) new FastReader(bytes);
      SaveGame.Header header;
      this.GameInfo = SaveGame.GetHeader(reader, out header);
      this.LoadedHeader = header;
      DebugUtil.LogArgs((object) string.Format("Loading save file: {4}\n headerVersion:{0}, buildVersion:{1}, headerSize:{2}, IsCompressed:{3}", (object) header.headerVersion, (object) header.buildVersion, (object) header.headerSize, (object) header.IsCompressed, (object) filename));
      DebugUtil.LogArgs((object) string.Format("GameInfo: numberOfCycles:{0}, numberOfDuplicants:{1}, baseName:{2}, isAutoSave:{3}, originalSaveName:{4}, saveVersion:{5}.{6}", (object) this.GameInfo.numberOfCycles, (object) this.GameInfo.numberOfDuplicants, (object) this.GameInfo.baseName, (object) this.GameInfo.isAutoSave, (object) this.GameInfo.originalSaveName, (object) this.GameInfo.saveMajorVersion, (object) this.GameInfo.saveMinorVersion));
      if (this.GameInfo.saveMajorVersion == 7 && this.GameInfo.saveMinorVersion < 4)
        Helper.SetTypeInfoMask(SerializationTypeInfo.VALUE_MASK | SerializationTypeInfo.IS_GENERIC_TYPE);
      KSerialization.Manager.DeserializeDirectory(reader);
      if (header.IsCompressed)
      {
        int length = bytes.Length - reader.Position;
        byte[] compressed = new byte[length];
        Array.Copy((Array) bytes, reader.Position, (Array) compressed, 0, length);
        this.Load((IReader) new FastReader(SaveLoader.DecompressContents(compressed)));
      }
      else
        this.Load(reader);
      if (this.GameInfo.isAutoSave)
      {
        if (!string.IsNullOrEmpty(this.GameInfo.originalSaveName))
          SaveLoader.SetActiveSaveFilePath(this.GameInfo.originalSaveName);
      }
    }
    catch (Exception ex)
    {
      DebugUtil.LogWarningArgs((object) ("\n--- Error loading save ---\n" + ex.Message + "\n" + ex.StackTrace));
      Sim.Shutdown();
      SaveLoader.SetActiveSaveFilePath((string) null);
      return false;
    }
    Stats.Print();
    DebugUtil.LogArgs((object) "Loaded", (object) ("[" + filename + "]"));
    DebugUtil.LogArgs((object) "World Seeds", (object) ("[" + (object) this.worldDetailSave.globalWorldSeed + "/" + (object) this.worldDetailSave.globalWorldLayoutSeed + "/" + (object) this.worldDetailSave.globalTerrainSeed + "/" + (object) this.worldDetailSave.globalNoiseSeed + "]"));
    GC.Collect();
    return true;
  }

  public bool LoadFromWorldGen()
  {
    DebugUtil.LogArgs((object) "Attempting to start a new game with current world gen");
    WorldGen.LoadSettings();
    string worldName;
    try
    {
      worldName = CustomGameSettings.Instance.GetCurrentQualitySetting((SettingConfig) CustomGameSettingConfigs.World).id;
    }
    catch
    {
      worldName = "worlds/SandstoneDefault";
    }
    this.worldGen = new WorldGen(worldName, (List<string>) null);
    SimSaveFileStructure saveFileStructure = this.worldGen.LoadWorldGenSim();
    if (saveFileStructure == null)
    {
      Debug.LogError((object) "Attempt failed");
      return false;
    }
    this.worldDetailSave = saveFileStructure.worldDetail;
    if (this.worldDetailSave == null)
      Debug.LogError((object) "Detail is null");
    GridSettings.Reset(saveFileStructure.WidthInCells, saveFileStructure.HeightInCells);
    Singleton<KBatchedAnimUpdater>.Instance.InitializeGrid();
    // ISSUE: reference to a compiler-generated field
    if (SaveLoader.\u003C\u003Ef__mg\u0024cache2 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SaveLoader.\u003C\u003Ef__mg\u0024cache2 = new Sim.GAME_MessageHandler(Sim.DLL_MessageHandler);
    }
    // ISSUE: reference to a compiler-generated field
    Sim.SIM_Initialize(SaveLoader.\u003C\u003Ef__mg\u0024cache2);
    SimMessages.CreateSimElementsTable(ElementLoader.elements);
    SimMessages.CreateDiseaseTable();
    try
    {
      if (Sim.Load(new FastReader(saveFileStructure.Sim)) != 0)
      {
        DebugUtil.LogWarningArgs((object) "\n--- Error loading save ---\nSimDLL found bad data\n");
        Sim.Shutdown();
        return false;
      }
    }
    catch (Exception ex)
    {
      Debug.LogWarning((object) ("--- Error loading Sim FROM NEW WORLDGEN ---" + ex.Message + "\n" + ex.StackTrace));
      Sim.Shutdown();
      return false;
    }
    Debug.Log((object) "Attempt success");
    SceneInitializer.Instance.PostLoadPrefabs();
    SceneInitializer.Instance.NewSaveGamePrefab();
    this.worldGen.ReplayGenerate(new WorldGen.ResetFunction(this.Reset));
    this.OnWorldGenComplete.Signal();
    ThreadedHttps<KleiMetrics>.Instance.StartNewGame();
    return true;
  }

  public GameSpawnData cachedGSD { get; private set; }

  public WorldDetailSave worldDetailSave { get; private set; }

  public void SetWorldDetail(WorldDetailSave worldDetail)
  {
    this.worldDetailSave = worldDetail;
  }

  private void Reset(GameSpawnData gsd)
  {
    this.cachedGSD = gsd;
  }

  private void ReportSaveMetrics(bool is_auto_save)
  {
    if (ThreadedHttps<KleiMetrics>.Instance == null || !ThreadedHttps<KleiMetrics>.Instance.enabled || (UnityEngine.Object) this.saveManager == (UnityEngine.Object) null)
      return;
    Dictionary<string, object> eventData = new Dictionary<string, object>();
    eventData[GameClock.NewCycleKey] = (object) (GameClock.Instance.GetCycle() + 1);
    eventData["WasDebugEverUsed"] = (object) Game.Instance.debugWasUsed;
    eventData["IsAutoSave"] = (object) is_auto_save;
    eventData["SavedPrefabs"] = (object) this.GetSavedPrefabMetrics();
    eventData["ResourcesAccessible"] = (object) this.GetWorldInventoryMetrics();
    eventData["MinionMetrics"] = (object) this.GetMinionMetrics();
    if (is_auto_save)
    {
      eventData["DailyReport"] = (object) this.GetDailyReportMetrics();
      eventData["PerformanceMeasurements"] = (object) this.GetPerformanceMeasurements();
      eventData["AverageFrameTime"] = (object) this.GetFrameTime();
    }
    eventData["CustomGameSettings"] = (object) CustomGameSettings.Instance.GetSettingsForMetrics();
    ThreadedHttps<KleiMetrics>.Instance.SendEvent(eventData);
  }

  private List<SaveLoader.MinionMetricsData> GetMinionMetrics()
  {
    List<SaveLoader.MinionMetricsData> minionMetricsDataList = new List<SaveLoader.MinionMetricsData>();
    foreach (MinionIdentity minionIdentity in Components.LiveMinionIdentities.Items)
    {
      if (!((UnityEngine.Object) minionIdentity == (UnityEngine.Object) null))
      {
        Amounts amounts = minionIdentity.gameObject.GetComponent<Modifiers>().amounts;
        List<SaveLoader.MinionAttrFloatData> minionAttrFloatDataList = new List<SaveLoader.MinionAttrFloatData>(amounts.Count);
        foreach (AmountInstance amountInstance in (Modifications<Amount, AmountInstance>) amounts)
        {
          float f = amountInstance.value;
          if (!float.IsNaN(f) && !float.IsInfinity(f))
            minionAttrFloatDataList.Add(new SaveLoader.MinionAttrFloatData()
            {
              Name = amountInstance.modifier.Id,
              Value = amountInstance.value
            });
        }
        MinionResume component = minionIdentity.gameObject.GetComponent<MinionResume>();
        float experienceGained = component.TotalExperienceGained;
        List<string> stringList = new List<string>();
        foreach (KeyValuePair<string, bool> keyValuePair in component.MasteryBySkillID)
        {
          if (keyValuePair.Value)
            stringList.Add(keyValuePair.Key);
        }
        minionMetricsDataList.Add(new SaveLoader.MinionMetricsData()
        {
          Name = minionIdentity.name,
          Modifiers = minionAttrFloatDataList,
          TotalExperienceGained = experienceGained,
          Skills = stringList
        });
      }
    }
    return minionMetricsDataList;
  }

  private List<SaveLoader.SavedPrefabMetricsData> GetSavedPrefabMetrics()
  {
    Dictionary<Tag, List<SaveLoadRoot>> lists = this.saveManager.GetLists();
    List<SaveLoader.SavedPrefabMetricsData> prefabMetricsDataList = new List<SaveLoader.SavedPrefabMetricsData>(lists.Count);
    foreach (KeyValuePair<Tag, List<SaveLoadRoot>> keyValuePair in lists)
    {
      Tag key = keyValuePair.Key;
      List<SaveLoadRoot> saveLoadRootList = keyValuePair.Value;
      if (saveLoadRootList.Count > 0)
        prefabMetricsDataList.Add(new SaveLoader.SavedPrefabMetricsData()
        {
          PrefabName = key.ToString(),
          Count = saveLoadRootList.Count
        });
    }
    return prefabMetricsDataList;
  }

  private List<SaveLoader.WorldInventoryMetricsData> GetWorldInventoryMetrics()
  {
    Dictionary<Tag, float> accessibleAmounts = WorldInventory.Instance.GetAccessibleAmounts();
    List<SaveLoader.WorldInventoryMetricsData> inventoryMetricsDataList = new List<SaveLoader.WorldInventoryMetricsData>(accessibleAmounts.Count);
    foreach (KeyValuePair<Tag, float> keyValuePair in accessibleAmounts)
    {
      float f = keyValuePair.Value;
      if (!float.IsInfinity(f) && !float.IsNaN(f))
        inventoryMetricsDataList.Add(new SaveLoader.WorldInventoryMetricsData()
        {
          Name = keyValuePair.Key.ToString(),
          Amount = f
        });
    }
    return inventoryMetricsDataList;
  }

  private List<SaveLoader.DailyReportMetricsData> GetDailyReportMetrics()
  {
    List<SaveLoader.DailyReportMetricsData> reportMetricsDataList = new List<SaveLoader.DailyReportMetricsData>();
    ReportManager.DailyReport report = ReportManager.Instance.FindReport(GameClock.Instance.GetCycle());
    if (report != null)
    {
      foreach (ReportManager.ReportEntry reportEntry in report.reportEntries)
      {
        SaveLoader.DailyReportMetricsData reportMetricsData = new SaveLoader.DailyReportMetricsData();
        reportMetricsData.Name = reportEntry.reportType.ToString();
        if (!float.IsInfinity(reportEntry.Net) && !float.IsNaN(reportEntry.Net))
          reportMetricsData.Net = new float?(reportEntry.Net);
        if (SaveLoader.force_infinity)
          reportMetricsData.Net = new float?();
        if (!float.IsInfinity(reportEntry.Positive) && !float.IsNaN(reportEntry.Positive))
          reportMetricsData.Positive = new float?(reportEntry.Positive);
        if (!float.IsInfinity(reportEntry.Negative) && !float.IsNaN(reportEntry.Negative))
          reportMetricsData.Negative = new float?(reportEntry.Negative);
        reportMetricsDataList.Add(reportMetricsData);
      }
      reportMetricsDataList.Add(new SaveLoader.DailyReportMetricsData()
      {
        Name = "MinionCount",
        Net = new float?((float) Components.LiveMinionIdentities.Count),
        Positive = new float?(0.0f),
        Negative = new float?(0.0f)
      });
    }
    return reportMetricsDataList;
  }

  private List<SaveLoader.PerformanceMeasurement> GetPerformanceMeasurements()
  {
    List<SaveLoader.PerformanceMeasurement> performanceMeasurementList = new List<SaveLoader.PerformanceMeasurement>();
    if ((UnityEngine.Object) Global.Instance != (UnityEngine.Object) null)
    {
      PerformanceMonitor component = Global.Instance.GetComponent<PerformanceMonitor>();
      performanceMeasurementList.Add(new SaveLoader.PerformanceMeasurement()
      {
        name = "FramesAbove30",
        value = (float) component.NumFramesAbove30
      });
      performanceMeasurementList.Add(new SaveLoader.PerformanceMeasurement()
      {
        name = "FramesBelow30",
        value = (float) component.NumFramesBelow30
      });
      component.Reset();
    }
    return performanceMeasurementList;
  }

  private float GetFrameTime()
  {
    PerformanceMonitor component = Global.Instance.GetComponent<PerformanceMonitor>();
    DebugUtil.LogArgs((object) "Average frame time:", (object) (float) (1.0 / (double) component.FPS));
    return 1f / component.FPS;
  }

  public class FlowUtilityNetworkInstance
  {
    public int id = -1;
    public SimHashes containedElement = SimHashes.Vacuum;
    public float containedMass;
    public float containedTemperature;
  }

  [SerializationConfig(KSerialization.MemberSerialization.OptOut)]
  public class FlowUtilityNetworkSaver : ISaveLoadable
  {
    public List<SaveLoader.FlowUtilityNetworkInstance> gas;
    public List<SaveLoader.FlowUtilityNetworkInstance> liquid;

    public FlowUtilityNetworkSaver()
    {
      this.gas = new List<SaveLoader.FlowUtilityNetworkInstance>();
      this.liquid = new List<SaveLoader.FlowUtilityNetworkInstance>();
    }
  }

  public struct SaveFileEntry
  {
    public string path;
    public System.DateTime timeStamp;
  }

  private struct MinionAttrFloatData
  {
    public string Name;
    public float Value;
  }

  private struct MinionMetricsData
  {
    public string Name;
    public List<SaveLoader.MinionAttrFloatData> Modifiers;
    public float TotalExperienceGained;
    public List<string> Skills;
  }

  private struct SavedPrefabMetricsData
  {
    public string PrefabName;
    public int Count;
  }

  private struct WorldInventoryMetricsData
  {
    public string Name;
    public float Amount;
  }

  private struct DailyReportMetricsData
  {
    public string Name;
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public float? Net;
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public float? Positive;
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public float? Negative;
  }

  private struct PerformanceMeasurement
  {
    public string name;
    public float value;
  }
}
