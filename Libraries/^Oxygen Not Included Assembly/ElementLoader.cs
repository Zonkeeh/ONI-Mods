// Decompiled with JetBrains decompiler
// Type: ElementLoader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using ProcGenGame;
using STRINGS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ElementLoader
{
  private static string path = Application.streamingAssetsPath + "/elements/";
  private static readonly Color noColour = new Color(0.0f, 0.0f, 0.0f, 0.0f);
  public static List<Element> elements;
  public static Dictionary<int, Element> elementTable;

  public static List<ElementLoader.ElementEntry> CollectElementsFromYAML()
  {
    List<ElementLoader.ElementEntry> elementEntryList = new List<ElementLoader.ElementEntry>();
    ListPool<FileHandle, ElementLoader>.PooledList pooledList = ListPool<FileHandle, ElementLoader>.Allocate();
    FileSystem.GetFiles(FileSystem.Normalize(ElementLoader.path), "*.yaml", (ICollection<FileHandle>) pooledList);
    ListPool<YamlIO.Error, ElementLoader>.PooledList errors = ListPool<YamlIO.Error, ElementLoader>.Allocate();
    foreach (FileHandle fileHandle in (List<FileHandle>) pooledList)
    {
      FileHandle file = fileHandle;
      ElementLoader.ElementEntryCollection elementEntryCollection = YamlIO.LoadFile<ElementLoader.ElementEntryCollection>(file.full_path, (YamlIO.ErrorHandler) ((error, force_log_as_warning) =>
      {
        error.file = file;
        errors.Add(error);
      }), (List<Tuple<string, System.Type>>) null);
      if (elementEntryCollection != null)
        elementEntryList.AddRange((IEnumerable<ElementLoader.ElementEntry>) elementEntryCollection.elements);
    }
    pooledList.Recycle();
    if ((UnityEngine.Object) Global.Instance != (UnityEngine.Object) null && Global.Instance.modManager != null)
      Global.Instance.modManager.HandleErrors((List<YamlIO.Error>) errors);
    errors.Recycle();
    return elementEntryList;
  }

  public static void Load(ref Hashtable substanceList, SubstanceTable substanceTable)
  {
    ElementLoader.elements = new List<Element>();
    ElementLoader.elementTable = new Dictionary<int, Element>();
    foreach (ElementLoader.ElementEntry entry in ElementLoader.CollectElementsFromYAML())
    {
      int key = Hash.SDBMLower(entry.elementId);
      if (!ElementLoader.elementTable.ContainsKey(key))
      {
        Element elem = new Element()
        {
          id = (SimHashes) key,
          name = (string) Strings.Get(entry.localizationID)
        };
        elem.nameUpperCase = elem.name.ToUpper();
        elem.description = (string) Strings.Get(entry.description);
        elem.tag = TagManager.Create(entry.elementId, elem.name);
        ElementLoader.CopyEntryToElement(entry, elem);
        ElementLoader.elements.Add(elem);
        ElementLoader.elementTable[key] = elem;
      }
    }
    foreach (Element element in ElementLoader.elements)
    {
      if (!ElementLoader.ManifestSubstanceForElement(element, ref substanceList, substanceTable))
        Debug.LogWarning((object) ("Missing substance for element: " + element.id.ToString()));
    }
    ElementLoader.FinaliseElementsTable(ref substanceList, substanceTable);
    WorldGen.SetupDefaultElements();
  }

  private static void CopyEntryToElement(ElementLoader.ElementEntry entry, Element elem)
  {
    Hash.SDBMLower(entry.elementId);
    elem.tag = TagManager.Create(entry.elementId.ToString());
    elem.specificHeatCapacity = entry.specificHeatCapacity;
    elem.thermalConductivity = entry.thermalConductivity;
    elem.molarMass = entry.molarMass;
    elem.strength = entry.strength;
    elem.disabled = entry.isDisabled;
    elem.flow = entry.flow;
    elem.maxMass = entry.maxMass;
    elem.maxCompression = entry.liquidCompression;
    elem.viscosity = entry.speed;
    elem.minHorizontalFlow = entry.minHorizontalFlow;
    elem.minVerticalFlow = entry.minVerticalFlow;
    elem.maxMass = entry.maxMass;
    elem.solidSurfaceAreaMultiplier = entry.solidSurfaceAreaMultiplier;
    elem.liquidSurfaceAreaMultiplier = entry.liquidSurfaceAreaMultiplier;
    elem.gasSurfaceAreaMultiplier = entry.gasSurfaceAreaMultiplier;
    elem.state = entry.state;
    elem.hardness = entry.hardness;
    elem.lowTemp = entry.lowTemp;
    elem.lowTempTransitionTarget = (SimHashes) Hash.SDBMLower(entry.lowTempTransitionTarget);
    elem.highTemp = entry.highTemp;
    elem.highTempTransitionTarget = (SimHashes) Hash.SDBMLower(entry.highTempTransitionTarget);
    elem.highTempTransitionOreID = (SimHashes) Hash.SDBMLower(entry.highTempTransitionOreId);
    elem.highTempTransitionOreMassConversion = entry.highTempTransitionOreMassConversion;
    elem.lowTempTransitionOreID = (SimHashes) Hash.SDBMLower(entry.lowTempTransitionOreId);
    elem.lowTempTransitionOreMassConversion = entry.lowTempTransitionOreMassConversion;
    elem.sublimateId = (SimHashes) Hash.SDBMLower(entry.sublimateId);
    elem.convertId = (SimHashes) Hash.SDBMLower(entry.convertId);
    elem.sublimateFX = (SpawnFXHashes) Hash.SDBMLower(entry.sublimateFx);
    elem.lightAbsorptionFactor = entry.lightAbsorptionFactor;
    elem.toxicity = entry.toxicity;
    Tag phaseTag = TagManager.Create(entry.state.ToString());
    elem.materialCategory = ElementLoader.CreateMaterialCategoryTag(elem.id, phaseTag, entry.materialCategory);
    elem.oreTags = ElementLoader.CreateOreTags(elem.materialCategory, phaseTag, entry.tags);
    elem.buildMenuSort = entry.buildMenuSort;
    Sim.PhysicsData physicsData = new Sim.PhysicsData();
    physicsData.temperature = entry.defaultTemperature;
    physicsData.mass = entry.defaultMass;
    physicsData.pressure = entry.defaultPressure;
    switch (entry.state)
    {
      case Element.State.Gas:
        GameTags.GasElements.Add(elem.tag);
        physicsData.mass = 1f;
        elem.maxMass = 1.8f;
        break;
      case Element.State.Liquid:
        GameTags.LiquidElements.Add(elem.tag);
        break;
      case Element.State.Solid:
        GameTags.SolidElements.Add(elem.tag);
        break;
    }
    elem.defaultValues = physicsData;
  }

  private static bool ManifestSubstanceForElement(
    Element elem,
    ref Hashtable substanceList,
    SubstanceTable substanceTable)
  {
    elem.substance = (Substance) null;
    if (substanceList.ContainsKey((object) elem.id))
    {
      elem.substance = substanceList[(object) elem.id] as Substance;
      return false;
    }
    if ((UnityEngine.Object) substanceTable != (UnityEngine.Object) null)
      elem.substance = substanceTable.GetSubstance(elem.id);
    if (elem.substance == null)
    {
      elem.substance = new Substance();
      substanceTable.GetList().Add(elem.substance);
    }
    elem.substance.elementID = elem.id;
    elem.substance.renderedByWorld = elem.IsSolid;
    elem.substance.idx = substanceList.Count;
    if ((Color) elem.substance.uiColour == ElementLoader.noColour)
    {
      int count = ElementLoader.elements.Count;
      int idx = elem.substance.idx;
      elem.substance.uiColour = (Color32) Color.HSVToRGB((float) idx / (float) count, 1f, 1f);
    }
    string tag_string = UI.StripLinkFormatting(elem.name);
    elem.substance.name = tag_string;
    elem.substance.nameTag = Array.IndexOf<SimHashes>((SimHashes[]) Enum.GetValues(typeof (SimHashes)), elem.id) < 0 ? (tag_string == null ? Tag.Invalid : TagManager.Create(tag_string)) : GameTagExtensions.Create(elem.id);
    elem.substance.audioConfig = ElementsAudio.Instance.GetConfigForElement(elem.id);
    substanceList.Add((object) elem.id, (object) elem.substance);
    return true;
  }

  public static Element FindElementByName(string name)
  {
    try
    {
      return ElementLoader.FindElementByHash((SimHashes) Enum.Parse(typeof (SimHashes), name));
    }
    catch
    {
      return ElementLoader.FindElementByHash((SimHashes) Hash.SDBMLower(name));
    }
  }

  public static Element FindElementByHash(SimHashes hash)
  {
    Element element = (Element) null;
    ElementLoader.elementTable.TryGetValue((int) hash, out element);
    return element;
  }

  public static int GetElementIndex(SimHashes hash)
  {
    for (int index = 0; index != ElementLoader.elements.Count; ++index)
    {
      if (ElementLoader.elements[index].id == hash)
        return index;
    }
    return -1;
  }

  public static byte GetElementIndex(Tag element_tag)
  {
    byte num = byte.MaxValue;
    for (int index = 0; index < ElementLoader.elements.Count; ++index)
    {
      Element element = ElementLoader.elements[index];
      if (element_tag == element.tag)
      {
        num = (byte) index;
        break;
      }
    }
    return num;
  }

  public static Element GetElement(Tag tag)
  {
    for (int index = 0; index < ElementLoader.elements.Count; ++index)
    {
      Element element = ElementLoader.elements[index];
      if (tag == element.tag)
        return element;
    }
    return (Element) null;
  }

  public static SimHashes GetElementID(Tag tag)
  {
    for (int index = 0; index < ElementLoader.elements.Count; ++index)
    {
      Element element = ElementLoader.elements[index];
      if (tag == element.tag)
        return element.id;
    }
    return SimHashes.Vacuum;
  }

  private static SimHashes GetID(
    int column,
    int row,
    string[,] grid,
    SimHashes defaultValue = SimHashes.Vacuum)
  {
    if (column >= grid.GetLength(0) || row > grid.GetLength(1))
    {
      Debug.LogError((object) string.Format("Could not find element at loc [{0},{1}] grid is only [{2},{3}]", (object) column, (object) row, (object) grid.GetLength(0), (object) grid.GetLength(1)));
      return defaultValue;
    }
    string str = grid[column, row];
    if (str == null || str == string.Empty)
      return defaultValue;
    object obj;
    try
    {
      obj = Enum.Parse(typeof (SimHashes), str);
    }
    catch (Exception ex)
    {
      Debug.LogError((object) string.Format("Could not find element {0}: {1}", (object) str, (object) ex.ToString()));
      return defaultValue;
    }
    return (SimHashes) obj;
  }

  private static SpawnFXHashes GetSpawnFX(int column, int row, string[,] grid)
  {
    if (column >= grid.GetLength(0) || row > grid.GetLength(1))
    {
      Debug.LogError((object) string.Format("Could not find SpawnFXHashes at loc [{0},{1}] grid is only [{2},{3}]", (object) column, (object) row, (object) grid.GetLength(0), (object) grid.GetLength(1)));
      return SpawnFXHashes.None;
    }
    string str = grid[column, row];
    if (str == null || str == string.Empty)
      return SpawnFXHashes.None;
    object obj;
    try
    {
      obj = Enum.Parse(typeof (SpawnFXHashes), str);
    }
    catch (Exception ex)
    {
      Debug.LogError((object) string.Format("Could not find FX {0}: {1}", (object) str, (object) ex.ToString()));
      return SpawnFXHashes.None;
    }
    return (SpawnFXHashes) obj;
  }

  private static Tag CreateMaterialCategoryTag(
    SimHashes element_id,
    Tag phaseTag,
    string materialCategoryField)
  {
    if (string.IsNullOrEmpty(materialCategoryField))
      return phaseTag;
    Tag tag = TagManager.Create(materialCategoryField);
    if (!GameTags.MaterialCategories.Contains(tag) && !GameTags.IgnoredMaterialCategories.Contains(tag))
      Debug.LogWarningFormat("Element {0} has category {1}, but that isn't in GameTags.MaterialCategores!", (object) element_id, (object) materialCategoryField);
    return tag;
  }

  private static Tag[] CreateOreTags(
    Tag materialCategory,
    Tag phaseTag,
    string[] ore_tags_split)
  {
    List<Tag> tagList = new List<Tag>();
    if (ore_tags_split != null)
    {
      foreach (string tag_string in ore_tags_split)
      {
        if (!string.IsNullOrEmpty(tag_string))
          tagList.Add(TagManager.Create(tag_string));
      }
    }
    tagList.Add(phaseTag);
    if (materialCategory.IsValid && !tagList.Contains(materialCategory))
      tagList.Add(materialCategory);
    return tagList.ToArray();
  }

  private static void FinaliseElementsTable(
    ref Hashtable substanceList,
    SubstanceTable substanceTable)
  {
    foreach (Element element in ElementLoader.elements)
    {
      if (element != null)
      {
        if (element.substance == null)
        {
          if ((UnityEngine.Object) substanceTable == (UnityEngine.Object) null)
            element.substance = new Substance();
          else
            ElementLoader.ManifestSubstanceForElement(element, ref substanceList, substanceTable);
        }
        Debug.Assert(element.substance.nameTag.IsValid);
        if ((double) element.thermalConductivity == 0.0)
          element.state |= Element.State.TemperatureInsulated;
        if ((double) element.strength == 0.0)
          element.state |= Element.State.Unbreakable;
        if (element.IsSolid)
        {
          Element elementByHash = ElementLoader.FindElementByHash(element.highTempTransitionTarget);
          if (elementByHash != null)
            element.highTempTransition = elementByHash;
        }
        else if (element.IsLiquid)
        {
          Element elementByHash1 = ElementLoader.FindElementByHash(element.highTempTransitionTarget);
          if (elementByHash1 != null)
            element.highTempTransition = elementByHash1;
          Element elementByHash2 = ElementLoader.FindElementByHash(element.lowTempTransitionTarget);
          if (elementByHash2 != null)
            element.lowTempTransition = elementByHash2;
        }
        else if (element.IsGas)
        {
          Element elementByHash = ElementLoader.FindElementByHash(element.lowTempTransitionTarget);
          if (elementByHash != null)
            element.lowTempTransition = elementByHash;
        }
      }
    }
    ElementLoader.elements = ElementLoader.elements.OrderByDescending<Element, int>((Func<Element, int>) (e => (int) (e.state & Element.State.Solid))).ThenBy<Element, SimHashes>((Func<Element, SimHashes>) (e => e.id)).ToList<Element>();
    for (int index = 0; index < ElementLoader.elements.Count; ++index)
    {
      if (ElementLoader.elements[index].substance != null)
        ElementLoader.elements[index].substance.idx = index;
      ElementLoader.elements[index].idx = (byte) index;
    }
  }

  public class ElementEntryCollection
  {
    public ElementLoader.ElementEntry[] elements { get; set; }
  }

  public class ElementEntry
  {
    private string description_backing;

    public ElementEntry()
    {
      this.lowTemp = 0.0f;
      this.highTemp = 10000f;
    }

    public string elementId { get; set; }

    public float specificHeatCapacity { get; set; }

    public float thermalConductivity { get; set; }

    public float solidSurfaceAreaMultiplier { get; set; }

    public float liquidSurfaceAreaMultiplier { get; set; }

    public float gasSurfaceAreaMultiplier { get; set; }

    public float defaultMass { get; set; }

    public float defaultTemperature { get; set; }

    public float defaultPressure { get; set; }

    public float molarMass { get; set; }

    public float lightAbsorptionFactor { get; set; }

    public string lowTempTransitionTarget { get; set; }

    public float lowTemp { get; set; }

    public string highTempTransitionTarget { get; set; }

    public float highTemp { get; set; }

    public string lowTempTransitionOreId { get; set; }

    public float lowTempTransitionOreMassConversion { get; set; }

    public string highTempTransitionOreId { get; set; }

    public float highTempTransitionOreMassConversion { get; set; }

    public string sublimateId { get; set; }

    public string sublimateFx { get; set; }

    public string materialCategory { get; set; }

    public string[] tags { get; set; }

    public bool isDisabled { get; set; }

    public float strength { get; set; }

    public float maxMass { get; set; }

    public byte hardness { get; set; }

    public float toxicity { get; set; }

    public float liquidCompression { get; set; }

    public float speed { get; set; }

    public float minHorizontalFlow { get; set; }

    public float minVerticalFlow { get; set; }

    public string convertId { get; set; }

    public float flow { get; set; }

    public int buildMenuSort { get; set; }

    public Element.State state { get; set; }

    public string localizationID { get; set; }

    public string description
    {
      get
      {
        return this.description_backing ?? "STRINGS.ELEMENTS." + this.elementId.ToString().ToUpper() + ".DESC";
      }
      set
      {
        this.description_backing = value;
      }
    }
  }
}
