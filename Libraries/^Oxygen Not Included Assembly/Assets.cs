// Decompiled with JetBrains decompiler
// Type: Assets
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Assets : KMonoBehaviour, ISerializationCallbackReceiver
{
  public static List<KAnimFile> ModLoadedKAnims = new List<KAnimFile>();
  public static List<KPrefabID> Prefabs = new List<KPrefabID>();
  private static HashSet<Tag> CountableTags = new HashSet<Tag>();
  private static Dictionary<Tag, KPrefabID> PrefabsByTag = new Dictionary<Tag, KPrefabID>();
  private static Dictionary<Tag, List<KPrefabID>> PrefabsByAdditionalTags = new Dictionary<Tag, List<KPrefabID>>();
  private static Dictionary<HashedString, KAnimFile> AnimTable = new Dictionary<HashedString, KAnimFile>();
  private static Dictionary<string, string> simpleSoundEventNames = new Dictionary<string, string>();
  public List<KPrefabID> PrefabAssets = new List<KPrefabID>();
  private static System.Action<KPrefabID> OnAddPrefab;
  public static List<BuildingDef> BuildingDefs;
  public List<Sprite> SpriteAssets;
  public static Dictionary<HashedString, Sprite> Sprites;
  public List<string> videoClipNames;
  private const string VIDEO_ASSET_PATH = "video";
  public List<TintedSprite> TintedSpriteAssets;
  public static List<TintedSprite> TintedSprites;
  public List<Texture2D> TextureAssets;
  public static List<Texture2D> Textures;
  public static List<TextureAtlas> TextureAtlases;
  public List<TextureAtlas> TextureAtlasAssets;
  public static List<Material> Materials;
  public List<Material> MaterialAssets;
  public static List<Shader> Shaders;
  public List<Shader> ShaderAssets;
  public static List<BlockTileDecorInfo> BlockTileDecorInfos;
  public List<BlockTileDecorInfo> BlockTileDecorInfoAssets;
  public Material AnimMaterialAsset;
  public static Material AnimMaterial;
  public DiseaseVisualization DiseaseVisualization;
  public Sprite LegendColourBox;
  public Texture2D invalidAreaTex;
  public Assets.UIPrefabData UIPrefabAssets;
  public static Assets.UIPrefabData UIPrefabs;
  public List<KAnimFile> AnimAssets;
  public static List<KAnimFile> Anims;
  public Font DebugFontAsset;
  public static Font DebugFont;
  public SubstanceTable substanceTable;
  public static SubstanceTable SubstanceTable;
  [SerializeField]
  public TextAsset elementAudio;
  [SerializeField]
  public TextAsset personalitiesFile;
  public LogicModeUI logicModeUIData;
  public CommonPlacerConfig.CommonPlacerAssets commonPlacerAssets;
  public DigPlacerConfig.DigPlacerAssets digPlacerAssets;
  public MopPlacerConfig.MopPlacerAssets mopPlacerAssets;
  public ComicData[] comics;
  public static Assets instance;

  protected override void OnPrefabInit()
  {
    Assets.instance = this;
    if (KPlayerPrefs.HasKey("TemperatureUnit"))
      GameUtil.temperatureUnit = (GameUtil.TemperatureUnit) KPlayerPrefs.GetInt("TemperatureUnit");
    if (KPlayerPrefs.HasKey("MassUnit"))
      GameUtil.massUnit = (GameUtil.MassUnit) KPlayerPrefs.GetInt("MassUnit");
    RecipeManager.DestroyInstance();
    RecipeManager.Get();
    Assets.AnimMaterial = this.AnimMaterialAsset;
    Assets.Prefabs = new List<KPrefabID>(this.PrefabAssets.Where<KPrefabID>((Func<KPrefabID, bool>) (x => (UnityEngine.Object) x != (UnityEngine.Object) null)));
    Assets.PrefabsByTag.Clear();
    Assets.PrefabsByAdditionalTags.Clear();
    Assets.CountableTags.Clear();
    Assets.Sprites = new Dictionary<HashedString, Sprite>();
    foreach (Sprite spriteAsset in this.SpriteAssets)
    {
      if (!((UnityEngine.Object) spriteAsset == (UnityEngine.Object) null))
      {
        HashedString key = new HashedString(spriteAsset.name);
        Assets.Sprites.Add(key, spriteAsset);
      }
    }
    Assets.TintedSprites = this.TintedSpriteAssets.Where<TintedSprite>((Func<TintedSprite, bool>) (x =>
    {
      if (x != null)
        return (UnityEngine.Object) x.sprite != (UnityEngine.Object) null;
      return false;
    })).ToList<TintedSprite>();
    Assets.Materials = this.MaterialAssets.Where<Material>((Func<Material, bool>) (x => (UnityEngine.Object) x != (UnityEngine.Object) null)).ToList<Material>();
    Assets.Textures = this.TextureAssets.Where<Texture2D>((Func<Texture2D, bool>) (x => (UnityEngine.Object) x != (UnityEngine.Object) null)).ToList<Texture2D>();
    Assets.TextureAtlases = this.TextureAtlasAssets.Where<TextureAtlas>((Func<TextureAtlas, bool>) (x => (UnityEngine.Object) x != (UnityEngine.Object) null)).ToList<TextureAtlas>();
    Assets.BlockTileDecorInfos = this.BlockTileDecorInfoAssets.Where<BlockTileDecorInfo>((Func<BlockTileDecorInfo, bool>) (x => (UnityEngine.Object) x != (UnityEngine.Object) null)).ToList<BlockTileDecorInfo>();
    Assets.Anims = this.AnimAssets.Where<KAnimFile>((Func<KAnimFile, bool>) (x => (UnityEngine.Object) x != (UnityEngine.Object) null)).ToList<KAnimFile>();
    Assets.Anims.AddRange((IEnumerable<KAnimFile>) Assets.ModLoadedKAnims);
    Assets.UIPrefabs = this.UIPrefabAssets;
    Assets.DebugFont = this.DebugFontAsset;
    AsyncLoadManager<IGlobalAsyncLoader>.Run();
    GameAudioSheets.Get().Initialize();
    this.SubstanceListHookup();
    Assets.BuildingDefs = new List<BuildingDef>();
    foreach (KPrefabID prefabAsset in this.PrefabAssets)
    {
      if (!((UnityEngine.Object) prefabAsset == (UnityEngine.Object) null))
        Assets.AddPrefab(prefabAsset);
    }
    Assets.AnimTable.Clear();
    foreach (KAnimFile anim in Assets.Anims)
    {
      if ((UnityEngine.Object) anim != (UnityEngine.Object) null)
      {
        HashedString name = (HashedString) anim.name;
        Assets.AnimTable[name] = anim;
      }
    }
    this.CreatePrefabs();
  }

  private void CreatePrefabs()
  {
    LegacyModMain.Load();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    Db.Get();
  }

  private static void TryAddCountableTag(KPrefabID prefab)
  {
    foreach (Tag displayAsUnit in GameTags.DisplayAsUnits)
    {
      if (prefab.HasTag(displayAsUnit))
      {
        Assets.AddCountableTag(prefab.PrefabTag);
        break;
      }
    }
  }

  public static void AddCountableTag(Tag tag)
  {
    Assets.CountableTags.Add(tag);
  }

  public static bool IsTagCountable(Tag tag)
  {
    return Assets.CountableTags.Contains(tag);
  }

  private void SubstanceListHookup()
  {
    Hashtable substanceList = new Hashtable();
    ElementsAudio.Instance.LoadData(AsyncLoadManager<IGlobalAsyncLoader>.AsyncLoader<ElementAudioFileLoader>.Get().entries);
    ElementLoader.Load(ref substanceList, this.substanceTable);
    Assets.SubstanceTable = this.substanceTable;
  }

  public static string GetSimpleSoundEventName(string path)
  {
    string str = (string) null;
    if (!Assets.simpleSoundEventNames.TryGetValue(path, out str))
    {
      int num = path.LastIndexOf('/');
      str = num == -1 ? path : path.Substring(num + 1);
      Assets.simpleSoundEventNames[path] = str;
    }
    return str;
  }

  private static BuildingDef GetDef(IList<BuildingDef> defs, string prefab_id)
  {
    int count = defs.Count;
    for (int index = 0; index < count; ++index)
    {
      if (defs[index].PrefabID == prefab_id)
        return defs[index];
    }
    return (BuildingDef) null;
  }

  public static BuildingDef GetBuildingDef(string prefab_id)
  {
    return Assets.GetDef((IList<BuildingDef>) Assets.BuildingDefs, prefab_id);
  }

  public static TintedSprite GetTintedSprite(string name)
  {
    TintedSprite tintedSprite = (TintedSprite) null;
    if (Assets.TintedSprites != null)
    {
      for (int index = 0; index < Assets.TintedSprites.Count; ++index)
      {
        if (Assets.TintedSprites[index].sprite.name == name)
        {
          tintedSprite = Assets.TintedSprites[index];
          break;
        }
      }
    }
    return tintedSprite;
  }

  public static Sprite GetSprite(HashedString name)
  {
    Sprite sprite = (Sprite) null;
    if (Assets.Sprites != null)
      Assets.Sprites.TryGetValue(name, out sprite);
    return sprite;
  }

  public static VideoClip GetVideo(string name)
  {
    return UnityEngine.Resources.Load<VideoClip>("video/" + name);
  }

  public static Texture2D GetTexture(string name)
  {
    Texture2D texture2D = (Texture2D) null;
    if (Assets.Textures != null)
    {
      for (int index = 0; index < Assets.Textures.Count; ++index)
      {
        if (Assets.Textures[index].name == name)
        {
          texture2D = Assets.Textures[index];
          break;
        }
      }
    }
    return texture2D;
  }

  public static ComicData GetComic(string id)
  {
    foreach (ComicData comic in Assets.instance.comics)
    {
      if (comic.name == id)
        return comic;
    }
    return (ComicData) null;
  }

  public static void AddPrefab(KPrefabID prefab)
  {
    if ((UnityEngine.Object) prefab == (UnityEngine.Object) null)
      return;
    prefab.UpdateSaveLoadTag();
    if (Assets.PrefabsByTag.ContainsKey(prefab.PrefabTag))
      Debug.LogWarning((object) ("Tried loading prefab with duplicate tag, ignoring: " + (object) prefab.PrefabTag));
    Assets.PrefabsByTag[prefab.PrefabTag] = prefab;
    foreach (Tag tag in prefab.Tags)
    {
      if (!Assets.PrefabsByAdditionalTags.ContainsKey(tag))
        Assets.PrefabsByAdditionalTags[tag] = new List<KPrefabID>();
      Assets.PrefabsByAdditionalTags[tag].Add(prefab);
    }
    Assets.Prefabs.Add(prefab);
    Assets.TryAddCountableTag(prefab);
    if (Assets.OnAddPrefab == null)
      return;
    Assets.OnAddPrefab(prefab);
  }

  public static void RegisterOnAddPrefab(System.Action<KPrefabID> on_add)
  {
    Assets.OnAddPrefab += on_add;
    foreach (KPrefabID prefab in Assets.Prefabs)
      on_add(prefab);
  }

  public static void UnregisterOnAddPrefab(System.Action<KPrefabID> on_add)
  {
    Assets.OnAddPrefab -= on_add;
  }

  public static void ClearOnAddPrefab()
  {
    Assets.OnAddPrefab = (System.Action<KPrefabID>) null;
  }

  public static GameObject GetPrefab(Tag tag)
  {
    GameObject prefab = Assets.TryGetPrefab(tag);
    if ((UnityEngine.Object) prefab == (UnityEngine.Object) null)
      Debug.LogWarning((object) ("Missing prefab: " + (object) tag));
    return prefab;
  }

  public static GameObject TryGetPrefab(Tag tag)
  {
    KPrefabID kprefabId = (KPrefabID) null;
    Assets.PrefabsByTag.TryGetValue(tag, out kprefabId);
    if ((UnityEngine.Object) kprefabId != (UnityEngine.Object) null)
      return kprefabId.gameObject;
    return (GameObject) null;
  }

  public static List<GameObject> GetPrefabsWithTag(Tag tag)
  {
    List<GameObject> gameObjectList = new List<GameObject>();
    if (Assets.PrefabsByAdditionalTags.ContainsKey(tag))
    {
      for (int index = 0; index < Assets.PrefabsByAdditionalTags[tag].Count; ++index)
        gameObjectList.Add(Assets.PrefabsByAdditionalTags[tag][index].gameObject);
    }
    return gameObjectList;
  }

  public static List<GameObject> GetPrefabsWithComponent<Type>()
  {
    List<GameObject> gameObjectList = new List<GameObject>();
    for (int index = 0; index < Assets.Prefabs.Count; ++index)
    {
      if ((object) Assets.Prefabs[index].GetComponent<Type>() != null)
        gameObjectList.Add(Assets.Prefabs[index].gameObject);
    }
    return gameObjectList;
  }

  public static List<Tag> GetPrefabTagsWithComponent<Type>()
  {
    List<Tag> tagList = new List<Tag>();
    for (int index = 0; index < Assets.Prefabs.Count; ++index)
    {
      if ((object) Assets.Prefabs[index].GetComponent<Type>() != null)
        tagList.Add(Assets.Prefabs[index].PrefabID());
    }
    return tagList;
  }

  public static Assets GetInstanceEditorOnly()
  {
    Assets[] objectsOfTypeAll = (Assets[]) UnityEngine.Resources.FindObjectsOfTypeAll(typeof (Assets));
    if (objectsOfTypeAll == null || objectsOfTypeAll.Length != 0)
      ;
    return objectsOfTypeAll[0];
  }

  public static TextureAtlas GetTextureAtlas(string name)
  {
    foreach (TextureAtlas textureAtlase in Assets.TextureAtlases)
    {
      if (textureAtlase.name == name)
        return textureAtlase;
    }
    return (TextureAtlas) null;
  }

  public static Material GetMaterial(string name)
  {
    foreach (Material material in Assets.Materials)
    {
      if (material.name == name)
        return material;
    }
    return (Material) null;
  }

  public static BlockTileDecorInfo GetBlockTileDecorInfo(string name)
  {
    foreach (BlockTileDecorInfo blockTileDecorInfo in Assets.BlockTileDecorInfos)
    {
      if (blockTileDecorInfo.name == name)
        return blockTileDecorInfo;
    }
    Debug.LogError((object) ("Could not find BlockTileDecorInfo named [" + name + "]"));
    return (BlockTileDecorInfo) null;
  }

  public static KAnimFile GetAnim(HashedString name)
  {
    if (!name.IsValid)
    {
      Debug.LogWarning((object) "Invalid hash name");
      return (KAnimFile) null;
    }
    KAnimFile kanimFile = (KAnimFile) null;
    Assets.AnimTable.TryGetValue(name, out kanimFile);
    if ((UnityEngine.Object) kanimFile == (UnityEngine.Object) null)
      Debug.LogWarning((object) ("Missing Anim: [" + name.ToString() + "]. You may have to run Collect Anim on the Assets prefab"));
    return kanimFile;
  }

  public void OnAfterDeserialize()
  {
    this.TintedSpriteAssets = this.TintedSpriteAssets.Where<TintedSprite>((Func<TintedSprite, bool>) (x =>
    {
      if (x != null)
        return (UnityEngine.Object) x.sprite != (UnityEngine.Object) null;
      return false;
    })).ToList<TintedSprite>();
    this.TintedSpriteAssets.Sort((Comparison<TintedSprite>) ((a, b) => a.name.CompareTo(b.name)));
  }

  public void OnBeforeSerialize()
  {
  }

  public static void AddBuildingDef(BuildingDef def)
  {
    Assets.BuildingDefs = Assets.BuildingDefs.Where<BuildingDef>((Func<BuildingDef, bool>) (x => x.PrefabID != def.PrefabID)).ToList<BuildingDef>();
    Assets.BuildingDefs.Add(def);
  }

  [Serializable]
  public struct UIPrefabData
  {
    public ProgressBar ProgressBar;
    public HealthBar HealthBar;
    public GameObject ResourceVisualizer;
    public Image RegionCellBlocked;
    public RectTransform PriorityOverlayIcon;
    public RectTransform HarvestWhenReadyOverlayIcon;
    public Assets.TableScreenAssets TableScreenWidgets;
  }

  [Serializable]
  public struct TableScreenAssets
  {
    public Material DefaultUIMaterial;
    public Material DesaturatedUIMaterial;
    public GameObject MinionPortrait;
    public GameObject GenericPortrait;
    public GameObject TogglePortrait;
    public GameObject ButtonLabel;
    public GameObject ButtonLabelWhite;
    public GameObject Label;
    public GameObject LabelHeader;
    public GameObject Checkbox;
    public GameObject BlankCell;
    public GameObject SuperCheckbox_Horizontal;
    public GameObject SuperCheckbox_Vertical;
    public GameObject Spacer;
    public GameObject NumericDropDown;
    public GameObject DropDownHeader;
    public GameObject PriorityGroupSelector;
    public GameObject PriorityGroupSelectorHeader;
    public GameObject PrioritizeRowWidget;
    public GameObject PrioritizeRowHeaderWidget;
  }
}
