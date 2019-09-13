// Decompiled with JetBrains decompiler
// Type: SaveGame
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using Newtonsoft.Json;
using ProcGenGame;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

[SerializationConfig(KSerialization.MemberSerialization.OptIn)]
public class SaveGame : KMonoBehaviour, ISaveLoadable
{
  [Serialize]
  public List<Tag> expandedResourceTags = new List<Tag>();
  [Serialize]
  public int minGermCountForDisinfect = 10000;
  [Serialize]
  public bool enableAutoDisinfect = true;
  [Serialize]
  private int autoSaveCycleInterval = 1;
  [Serialize]
  private Vector2I timelapseResolution = new Vector2I(512, 768);
  [Serialize]
  private int speed;
  [Serialize]
  public bool sandboxEnabled;
  private string baseName;
  public static SaveGame Instance;
  public EntombedItemManager entombedItemManager;
  public WorldGenSpawner worldGenSpawner;
  [MyCmpReq]
  public MaterialSelectorSerializer materialSelectorSerializer;
  public WorldGen worldGen;

  public int AutoSaveCycleInterval
  {
    get
    {
      return this.autoSaveCycleInterval;
    }
    set
    {
      this.autoSaveCycleInterval = value;
    }
  }

  public Vector2I TimelapseResolution
  {
    get
    {
      return this.timelapseResolution;
    }
    set
    {
      this.timelapseResolution = value;
    }
  }

  public string BaseName
  {
    get
    {
      return this.baseName;
    }
  }

  public static void DestroyInstance()
  {
    SaveGame.Instance = (SaveGame) null;
  }

  protected override void OnPrefabInit()
  {
    SaveGame.Instance = this;
    new ColonyRationMonitor.Instance((IStateMachineTarget) this).StartSM();
    new VignetteManager.Instance((IStateMachineTarget) this).StartSM();
    this.entombedItemManager = this.gameObject.AddComponent<EntombedItemManager>();
    this.worldGen = SaveLoader.Instance.worldGen;
    this.worldGenSpawner = this.gameObject.AddComponent<WorldGenSpawner>();
  }

  [OnSerializing]
  private void OnSerialize()
  {
    this.speed = SpeedControlScreen.Instance.GetSpeed();
  }

  [OnDeserializing]
  private void OnDeserialize()
  {
    this.baseName = SaveLoader.Instance.GameInfo.baseName;
  }

  public int GetSpeed()
  {
    return this.speed;
  }

  public byte[] GetSaveHeader(bool isAutoSave, bool isCompressed, out SaveGame.Header header)
  {
    byte[] bytes = Encoding.UTF8.GetBytes(!isAutoSave ? JsonConvert.SerializeObject((object) new SaveGame.GameInfo(GameClock.Instance.GetCycle(), Components.LiveMinionIdentities.Count, this.baseName, false)) : JsonConvert.SerializeObject((object) new SaveGame.GameInfo(GameClock.Instance.GetCycle(), Components.LiveMinionIdentities.Count, this.baseName, true, SaveLoader.GetActiveSaveFilePath(), false)));
    header = new SaveGame.Header();
    header.buildVersion = 366134U;
    header.headerSize = bytes.Length;
    header.headerVersion = 1U;
    header.compression = !isCompressed ? 0 : 1;
    return bytes;
  }

  public static SaveGame.Header GetHeader(BinaryReader br)
  {
    SaveGame.Header header = new SaveGame.Header();
    header.buildVersion = br.ReadUInt32();
    header.headerSize = br.ReadInt32();
    header.headerVersion = br.ReadUInt32();
    if (1U <= header.headerVersion)
      header.compression = br.ReadInt32();
    return header;
  }

  public static SaveGame.GameInfo GetHeader(IReader br, out SaveGame.Header header)
  {
    header = new SaveGame.Header();
    header.buildVersion = br.ReadUInt32();
    header.headerSize = br.ReadInt32();
    header.headerVersion = br.ReadUInt32();
    if (1U <= header.headerVersion)
      header.compression = br.ReadInt32();
    return SaveGame.GetGameInfo(br.ReadBytes(header.headerSize));
  }

  public static SaveGame.GameInfo GetGameInfo(byte[] data)
  {
    return JsonConvert.DeserializeObject<SaveGame.GameInfo>(Encoding.UTF8.GetString(data));
  }

  public void SetBaseName(string newBaseName)
  {
    if (string.IsNullOrEmpty(newBaseName))
      UnityEngine.Debug.LogWarning((object) "Cannot give the base an empty name");
    else
      this.baseName = newBaseName;
  }

  protected override void OnSpawn()
  {
    ThreadedHttps<KleiMetrics>.Instance.SendProfileStats();
    Game.Instance.Trigger(-1917495436, (object) null);
  }

  public struct Header
  {
    public uint buildVersion;
    public int headerSize;
    public uint headerVersion;
    public int compression;

    public bool IsCompressed
    {
      get
      {
        return 0 != this.compression;
      }
    }
  }

  public struct GameInfo
  {
    public int numberOfCycles;
    public int numberOfDuplicants;
    public string baseName;
    public bool isAutoSave;
    public string originalSaveName;
    public int saveMajorVersion;
    public int saveMinorVersion;

    public GameInfo(
      int numberOfCycles,
      int numberOfDuplicants,
      string baseName,
      bool isAutoSave,
      string originalSaveName,
      bool sandboxEnabled = false)
    {
      this.numberOfCycles = numberOfCycles;
      this.numberOfDuplicants = numberOfDuplicants;
      this.baseName = baseName;
      this.isAutoSave = isAutoSave;
      this.originalSaveName = originalSaveName;
      this.saveMajorVersion = 7;
      this.saveMinorVersion = 11;
    }

    public GameInfo(
      int numberOfCycles,
      int numberOfDuplicants,
      string baseName,
      bool sandboxEnabled = false)
    {
      this.numberOfCycles = numberOfCycles;
      this.numberOfDuplicants = numberOfDuplicants;
      this.baseName = baseName;
      this.isAutoSave = false;
      this.originalSaveName = string.Empty;
      this.saveMajorVersion = 7;
      this.saveMinorVersion = 11;
    }

    public bool IsVersionOlderThan(int major, int minor)
    {
      if (this.saveMajorVersion < major)
        return true;
      if (this.saveMajorVersion == major)
        return this.saveMinorVersion < minor;
      return false;
    }

    public bool IsVersionExactly(int major, int minor)
    {
      if (this.saveMajorVersion == major)
        return this.saveMinorVersion == minor;
      return false;
    }
  }
}
