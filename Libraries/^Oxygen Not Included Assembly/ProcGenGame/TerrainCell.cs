// Decompiled with JetBrains decompiler
// Type: ProcGenGame.TerrainCell
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Delaunay.Geo;
using KSerialization;
using ProcGen;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using VoronoiTree;

namespace ProcGenGame
{
  [SerializationConfig(MemberSerialization.OptIn)]
  public class TerrainCell
  {
    private static HashSet<int> claimedCells = new HashSet<int>();
    private static HashSet<int> highPriorityClaims = new HashSet<int>();
    private static readonly Tag[] noFeatureSpawnTags = new Tag[5]
    {
      WorldGenTags.StartLocation,
      WorldGenTags.AtStart,
      WorldGenTags.NearStartLocation,
      WorldGenTags.POI,
      WorldGenTags.Feature
    };
    private static readonly TagSet noFeatureSpawnTagSet = new TagSet(TerrainCell.noFeatureSpawnTags);
    private static readonly Tag[] noPOISpawnTags = new Tag[6]
    {
      WorldGenTags.StartLocation,
      WorldGenTags.AtStart,
      WorldGenTags.NearStartLocation,
      WorldGenTags.POI,
      WorldGenTags.AtEdge,
      WorldGenTags.AtDepths
    };
    private static readonly TagSet noPOISpawnTagSet = new TagSet(TerrainCell.noPOISpawnTags);
    private static readonly Tag[] noPOINeighborSpawnTags = new Tag[1]
    {
      WorldGenTags.POI
    };
    private static readonly TagSet noPOINeighborSpawnTagSet = new TagSet(TerrainCell.noPOINeighborSpawnTags);
    public List<uint> terrain_neighbors_idx = new List<uint>();
    public List<KeyValuePair<int, Tag>> terrainPositions;
    public List<KeyValuePair<int, Tag>> poi;
    private float finalSize;
    private bool debugMode;
    private List<int> allCells;
    private HashSet<Vector2I> availableTerrainPoints;
    private HashSet<Vector2I> featureSpawnPoints;
    private HashSet<Vector2I> availableSpawnPoints;
    public const int DONT_SET_TEMPERATURE_DEFAULTS = -1;

    protected TerrainCell()
    {
    }

    protected TerrainCell(ProcGen.Node node, Diagram.Site site)
    {
      this.node = node;
      this.site = site;
      this.node.SetPosition(site.position);
    }

    public Polygon poly
    {
      get
      {
        return this.site.poly;
      }
    }

    [Serialize]
    public ProcGen.Node node { get; private set; }

    public void SetNode(ProcGen.Node newNode)
    {
      this.node = newNode;
    }

    [Serialize]
    public Diagram.Site site { get; private set; }

    public bool HasMobs
    {
      get
      {
        if (this.mobs != null)
          return this.mobs.Count > 0;
        return false;
      }
    }

    public List<KeyValuePair<int, Tag>> mobs { get; private set; }

    public virtual void LogInfo(string evt, string param, float value)
    {
      Debug.Log((object) (evt + ":" + param + "=" + (object) value));
    }

    public static HashSet<int> GetClaimedCells()
    {
      return TerrainCell.claimedCells;
    }

    public static HashSet<int> GetHighPriorityClaimCells()
    {
      return TerrainCell.highPriorityClaims;
    }

    public static void ClearClaimedCells()
    {
      TerrainCell.claimedCells.Clear();
      TerrainCell.highPriorityClaims.Clear();
    }

    public void InitializeCells()
    {
      if (this.allCells != null)
        return;
      this.allCells = new List<int>();
      this.availableTerrainPoints = new HashSet<Vector2I>();
      this.availableSpawnPoints = new HashSet<Vector2I>();
      for (int y = 0; y < Grid.HeightInCells; ++y)
      {
        for (int x = 0; x < Grid.WidthInCells; ++x)
        {
          if (this.poly.Contains(new Vector2((float) x, (float) y)))
          {
            int cell = Grid.XYToCell(x, y);
            this.availableTerrainPoints.Add(Grid.CellToXY(cell));
            this.availableSpawnPoints.Add(Grid.CellToXY(cell));
            if (TerrainCell.claimedCells.Add(cell))
              this.allCells.Add(cell);
          }
        }
      }
      this.LogInfo("Initialise cells", string.Empty, (float) this.allCells.Count);
    }

    public List<int> GetAllCells()
    {
      return new List<int>((IEnumerable<int>) this.allCells);
    }

    public List<int> GetAvailableSpawnCellsAll()
    {
      List<int> intList = new List<int>();
      foreach (Vector2I availableSpawnPoint in this.availableSpawnPoints)
        intList.Add(Grid.XYToCell(availableSpawnPoint.x, availableSpawnPoint.y));
      return intList;
    }

    public List<int> GetAvailableSpawnCellsFeature()
    {
      List<int> intList = new List<int>();
      HashSet<Vector2I> vector2ISet = new HashSet<Vector2I>((IEnumerable<Vector2I>) this.availableSpawnPoints);
      vector2ISet.ExceptWith((IEnumerable<Vector2I>) this.availableTerrainPoints);
      foreach (Vector2I vector2I in vector2ISet)
        intList.Add(Grid.XYToCell(vector2I.x, vector2I.y));
      return intList;
    }

    public List<int> GetAvailableSpawnCellsBiome()
    {
      List<int> intList = new List<int>();
      HashSet<Vector2I> vector2ISet = new HashSet<Vector2I>((IEnumerable<Vector2I>) this.availableSpawnPoints);
      vector2ISet.ExceptWith((IEnumerable<Vector2I>) this.featureSpawnPoints);
      foreach (Vector2I vector2I in vector2ISet)
        intList.Add(Grid.XYToCell(vector2I.x, vector2I.y));
      return intList;
    }

    public List<int> GetAvailableTerrainCells()
    {
      List<int> intList = new List<int>();
      foreach (Vector2I availableTerrainPoint in this.availableTerrainPoints)
        intList.Add(Grid.XYToCell(availableTerrainPoint.x, availableTerrainPoint.y));
      return intList;
    }

    private void AddHighPriorityCells(HashSet<Vector2I> cells)
    {
      foreach (Vector2I cell1 in cells)
      {
        int cell2 = Grid.XYToCell(cell1.x, cell1.y);
        TerrainCell.highPriorityClaims.Add(cell2);
      }
    }

    private bool RemoveFromAvailableSpawnCells(int cell)
    {
      int x;
      int y;
      Grid.CellToXY(cell, out x, out y);
      return this.availableSpawnPoints.Remove(new Vector2I(x, y));
    }

    public void AddMobs(IEnumerable<KeyValuePair<int, Tag>> newMobs)
    {
      foreach (KeyValuePair<int, Tag> newMob in newMobs)
        this.AddMob(newMob);
    }

    private void AddMob(int cellIdx, string tag)
    {
      this.AddMob(new KeyValuePair<int, Tag>(cellIdx, new Tag(tag)));
    }

    public void AddMob(KeyValuePair<int, Tag> mob)
    {
      if (this.mobs == null)
        this.mobs = new List<KeyValuePair<int, Tag>>();
      this.mobs.Add(mob);
      bool flag = this.RemoveFromAvailableSpawnCells(mob.Key);
      this.LogInfo("\t\t\tRemoveFromAvailableCells", mob.Value.Name + ": " + (!flag ? "failed" : "success"), (float) mob.Key);
      if (flag)
        return;
      if (!this.allCells.Contains(mob.Key))
        Debug.Assert(false, (object) ("Couldnt find cell [" + (object) mob.Key + "] we dont own, to remove for mob [" + mob.Value.Name + "]"));
      else
        Debug.Assert(false, (object) ("Couldnt find cell [" + (object) mob.Key + "] to remove for mob [" + mob.Value.Name + "]"));
    }

    protected string GetSubWorldType(WorldGen worldGen)
    {
      Vector2I pos = new Vector2I((int) this.site.poly.Centroid().x, (int) this.site.poly.Centroid().y);
      return worldGen.GetSubWorldType(pos);
    }

    protected ProcGen.Temperature.Range GetTemperatureRange(WorldGen worldGen)
    {
      string subWorldType = this.GetSubWorldType(worldGen);
      if (subWorldType == null || !worldGen.Settings.HasSubworld(subWorldType))
        return ProcGen.Temperature.Range.Mild;
      return worldGen.Settings.GetSubWorld(subWorldType).temperatureRange;
    }

    protected void GetTemperatureRange(WorldGen worldGen, ref float min, ref float range)
    {
      ProcGen.Temperature.Range temperatureRange = this.GetTemperatureRange(worldGen);
      min = SettingsCache.temperatures[temperatureRange].min;
      range = SettingsCache.temperatures[temperatureRange].max - min;
    }

    protected float GetDensityMassForCell(Chunk world, int cellIdx, float mass)
    {
      if (!Grid.IsValidCell(cellIdx))
        return 0.0f;
      Debug.Assert((double) world.density[cellIdx] >= 0.0 && (double) world.density[cellIdx] <= 1.0, (object) ("Density [" + (object) world.density[cellIdx] + "] out of range [0-1]"));
      float num1 = world.density[cellIdx] - 0.5f;
      float num2 = mass + mass * num1;
      if ((double) num2 > 10000.0)
        num2 = 10000f;
      return num2;
    }

    private void HandleSprinkleOfElement(
      WorldGenSettings settings,
      Tag targetTag,
      Chunk world,
      TerrainCell.SetValuesFunction SetValues,
      float temperatureMin,
      float temperatureRange,
      SeededRandom rnd)
    {
      Element elementByName = ElementLoader.FindElementByName(settings.GetFeature(targetTag.Name).GetOneWeightedSimHash("SprinkleOfElementChoices", rnd).element);
      ProcGen.Room room = (ProcGen.Room) null;
      SettingsCache.rooms.TryGetValue(targetTag.Name, out room);
      SampleDescriber sampleDescriber = (SampleDescriber) room;
      Sim.PhysicsData defaultValues = elementByName.defaultValues;
      Sim.DiseaseCell invalid = Sim.DiseaseCell.Invalid;
      for (int index1 = 0; index1 < this.terrainPositions.Count; ++index1)
      {
        if (!(this.terrainPositions[index1].Value != targetTag))
        {
          float radius = rnd.RandomRange(sampleDescriber.blobSize.min, sampleDescriber.blobSize.max);
          List<Vector2I> filledCircle = ProcGen.Util.GetFilledCircle((Vector2) Grid.CellToPos2D(this.terrainPositions[index1].Key), radius);
          for (int index2 = 0; index2 < filledCircle.Count; ++index2)
          {
            int cell = Grid.XYToCell(filledCircle[index2].x, filledCircle[index2].y);
            if (Grid.IsValidCell(cell))
            {
              defaultValues.mass = this.GetDensityMassForCell(world, cell, elementByName.defaultValues.mass);
              defaultValues.temperature = temperatureMin + world.heatOffset[cell] * temperatureRange;
              SetValues(cell, (object) elementByName, defaultValues, invalid);
            }
          }
        }
      }
    }

    private HashSet<Vector2I> DigFeature(
      ProcGen.Room.Shape shape,
      float size,
      List<int> bordersWidths,
      SeededRandom rnd,
      out List<Vector2I> featureCenterPoints,
      out List<List<Vector2I>> featureBorders)
    {
      HashSet<Vector2I> sourcePoints = new HashSet<Vector2I>();
      featureCenterPoints = new List<Vector2I>();
      featureBorders = new List<List<Vector2I>>();
      if ((double) size < 1.0)
        return sourcePoints;
      Vector2 center = this.site.poly.Centroid();
      this.finalSize = size;
      switch (shape)
      {
        case ProcGen.Room.Shape.Circle:
          featureCenterPoints = ProcGen.Util.GetFilledCircle(center, this.finalSize);
          break;
        case ProcGen.Room.Shape.Blob:
          featureCenterPoints = ProcGen.Util.GetBlob(center, this.finalSize, rnd.RandomSource());
          break;
        case ProcGen.Room.Shape.Square:
          featureCenterPoints = ProcGen.Util.GetFilledRectangle(center, this.finalSize, this.finalSize, rnd, 2f, 2f);
          break;
        case ProcGen.Room.Shape.TallThin:
          featureCenterPoints = ProcGen.Util.GetFilledRectangle(center, this.finalSize / 4f, this.finalSize, rnd, 2f, 2f);
          break;
        case ProcGen.Room.Shape.ShortWide:
          featureCenterPoints = ProcGen.Util.GetFilledRectangle(center, this.finalSize, this.finalSize / 4f, rnd, 2f, 2f);
          break;
        case ProcGen.Room.Shape.Splat:
          featureCenterPoints = ProcGen.Util.GetSplat(center, this.finalSize, rnd.RandomSource());
          break;
      }
      sourcePoints.UnionWith((IEnumerable<Vector2I>) featureCenterPoints);
      if (featureCenterPoints.Count == 0)
        Debug.LogWarning((object) ("Room has no centerpoints. Terrain Cell [ shape: " + shape.ToString() + " size: " + (object) this.finalSize + "] [" + (object) this.node.node.Id + " " + this.node.type + " " + (object) this.node.position + "]"));
      else if (bordersWidths != null && bordersWidths.Count > 0 && bordersWidths[0] > 0)
      {
        for (int index = 0; index < bordersWidths.Count && bordersWidths[index] > 0; ++index)
        {
          featureBorders.Add(ProcGen.Util.GetBorder(sourcePoints, bordersWidths[index]));
          sourcePoints.UnionWith((IEnumerable<Vector2I>) featureBorders[index]);
        }
      }
      return sourcePoints;
    }

    public static TerrainCell.ElementOverride GetElementOverride(
      string element,
      SampleDescriber.Override overrides)
    {
      Debug.Assert(element != null && element.Length > 0);
      TerrainCell.ElementOverride elementOverride = new TerrainCell.ElementOverride();
      elementOverride.element = ElementLoader.FindElementByName(element);
      Debug.Assert(elementOverride.element != null, (object) ("Couldn't find an element called " + element));
      elementOverride.pdelement = elementOverride.element.defaultValues;
      elementOverride.dc = Sim.DiseaseCell.Invalid;
      elementOverride.mass = elementOverride.element.defaultValues.mass;
      elementOverride.temperature = elementOverride.element.defaultValues.temperature;
      if (overrides == null)
        return elementOverride;
      elementOverride.overrideMass = false;
      elementOverride.overrideTemperature = false;
      elementOverride.overrideDiseaseIdx = false;
      elementOverride.overrideDiseaseAmount = false;
      if (overrides.massOverride.HasValue)
      {
        elementOverride.mass = overrides.massOverride.Value;
        elementOverride.overrideMass = true;
      }
      if (overrides.massMultiplier.HasValue)
      {
        elementOverride.mass *= overrides.massMultiplier.Value;
        elementOverride.overrideMass = true;
      }
      if (overrides.temperatureOverride.HasValue)
      {
        elementOverride.temperature = overrides.temperatureOverride.Value;
        elementOverride.overrideTemperature = true;
      }
      if (overrides.temperatureMultiplier.HasValue)
      {
        elementOverride.temperature *= overrides.temperatureMultiplier.Value;
        elementOverride.overrideTemperature = true;
      }
      if (overrides.diseaseOverride != null)
      {
        elementOverride.diseaseIdx = (byte) WorldGen.GetDiseaseIdx(overrides.diseaseOverride);
        elementOverride.overrideDiseaseIdx = true;
      }
      if (overrides.diseaseAmountOverride.HasValue)
      {
        elementOverride.diseaseAmount = overrides.diseaseAmountOverride.Value;
        elementOverride.overrideDiseaseAmount = true;
      }
      if (elementOverride.overrideTemperature)
        elementOverride.pdelement.temperature = elementOverride.temperature;
      if (elementOverride.overrideMass)
        elementOverride.pdelement.mass = elementOverride.mass;
      if (elementOverride.overrideDiseaseIdx)
        elementOverride.dc.diseaseIdx = elementOverride.diseaseIdx;
      if (elementOverride.overrideDiseaseAmount)
        elementOverride.dc.elementCount = elementOverride.diseaseAmount;
      return elementOverride;
    }

    private void ApplyPlaceElementForRoom(
      FeatureSettings feature,
      string group,
      List<Vector2I> cells,
      Chunk world,
      TerrainCell.SetValuesFunction SetValues,
      float temperatureMin,
      float temperatureRange,
      SeededRandom rnd)
    {
      if (cells == null || cells.Count == 0 || !feature.HasGroup(group))
        return;
      switch (feature.ElementChoiceGroups[group].selectionMethod)
      {
        case ProcGen.Room.Selection.Weighted:
        case ProcGen.Room.Selection.WeightedResample:
          for (int index = 0; index < cells.Count; ++index)
          {
            int cell = Grid.XYToCell(cells[index].x, cells[index].y);
            if (Grid.IsValidCell(cell) && !TerrainCell.highPriorityClaims.Contains(cell))
            {
              WeightedSimHash oneWeightedSimHash = feature.GetOneWeightedSimHash(group, rnd);
              TerrainCell.ElementOverride elementOverride = TerrainCell.GetElementOverride(oneWeightedSimHash.element, oneWeightedSimHash.overrides);
              if (!elementOverride.overrideTemperature)
                elementOverride.pdelement.temperature = temperatureMin + world.heatOffset[cell] * temperatureRange;
              if (!elementOverride.overrideMass)
                elementOverride.pdelement.mass = this.GetDensityMassForCell(world, cell, elementOverride.mass);
              SetValues(cell, (object) elementOverride.element, elementOverride.pdelement, elementOverride.dc);
            }
          }
          break;
        default:
          WeightedSimHash oneWeightedSimHash1 = feature.GetOneWeightedSimHash(group, rnd);
          DebugUtil.LogArgs((object) "Picked one: ", (object) oneWeightedSimHash1.element);
          for (int index = 0; index < cells.Count; ++index)
          {
            int cell = Grid.XYToCell(cells[index].x, cells[index].y);
            if (Grid.IsValidCell(cell) && !TerrainCell.highPriorityClaims.Contains(cell))
            {
              TerrainCell.ElementOverride elementOverride = TerrainCell.GetElementOverride(oneWeightedSimHash1.element, oneWeightedSimHash1.overrides);
              if (!elementOverride.overrideTemperature)
                elementOverride.pdelement.temperature = temperatureMin + world.heatOffset[cell] * temperatureRange;
              if (!elementOverride.overrideMass)
                elementOverride.pdelement.mass = this.GetDensityMassForCell(world, cell, elementOverride.mass);
              SetValues(cell, (object) elementOverride.element, elementOverride.pdelement, elementOverride.dc);
            }
          }
          break;
      }
    }

    private int GetIndexForLocation(List<Vector2I> points, Mob.Location location, SeededRandom rnd)
    {
      int index1 = -1;
      if (points == null || points.Count == 0)
        return index1;
      if (location == Mob.Location.Air || location == Mob.Location.Solid)
        return rnd.RandomRange(0, points.Count);
      for (int index2 = 0; index2 < points.Count; ++index2)
      {
        if (Grid.IsValidCell(Grid.XYToCell(points[index2].x, points[index2].y)))
        {
          if (index1 == -1)
          {
            index1 = index2;
          }
          else
          {
            switch (location)
            {
              case Mob.Location.Floor:
                if (points[index2].y < points[index1].y)
                {
                  index1 = index2;
                  continue;
                }
                continue;
              case Mob.Location.Ceiling:
                if (points[index2].y > points[index1].y)
                {
                  index1 = index2;
                  continue;
                }
                continue;
              default:
                continue;
            }
          }
        }
      }
      return index1;
    }

    private void PlaceMobsInRoom(
      WorldGenSettings settings,
      List<MobReference> mobTags,
      List<Vector2I> points,
      SeededRandom rnd)
    {
      if (points == null)
        return;
      if (this.mobs == null)
        this.mobs = new List<KeyValuePair<int, Tag>>();
      for (int index1 = 0; index1 < mobTags.Count; ++index1)
      {
        if (!settings.HasMob(mobTags[index1].type))
        {
          Debug.LogError((object) ("Missing sample description for tag [" + mobTags[index1].type + "]"));
        }
        else
        {
          Mob mob = settings.GetMob(mobTags[index1].type);
          int num = Mathf.RoundToInt(mobTags[index1].count.GetRandomValueWithinRange(rnd));
          for (int index2 = 0; index2 < num; ++index2)
          {
            int indexForLocation = this.GetIndexForLocation(points, mob.location, rnd);
            if (indexForLocation != -1)
            {
              if (points.Count <= indexForLocation)
                return;
              int cell = Grid.XYToCell(points[indexForLocation].x, points[indexForLocation].y);
              points.RemoveAt(indexForLocation);
              this.AddMob(cell, mobTags[index1].type);
            }
            else
              break;
          }
        }
      }
    }

    private int[] ConvertNoiseToPoints(float[] basenoise, float minThreshold = 0.9f, float maxThreshold = 1f)
    {
      if (basenoise == null)
        return (int[]) null;
      List<int> intList = new List<int>();
      float width = this.site.poly.bounds.width;
      float height = this.site.poly.bounds.height;
      for (float y = this.site.position.y - height / 2f; (double) y < (double) this.site.position.y + (double) height / 2.0; ++y)
      {
        for (float x = this.site.position.x - width / 2f; (double) x < (double) this.site.position.x + (double) width / 2.0; ++x)
        {
          int cell = Grid.PosToCell(new Vector2(x, y));
          if (this.site.poly.Contains(new Vector2(x, y)))
          {
            float num = (float) (int) basenoise[cell];
            if ((double) num >= (double) minThreshold && (double) num <= (double) maxThreshold && !intList.Contains(cell))
              intList.Add(Grid.PosToCell(new Vector2(x, y)));
          }
        }
      }
      return intList.ToArray();
    }

    private void ApplyForeground(
      WorldGenSettings settings,
      Chunk world,
      TerrainCell.SetValuesFunction SetValues,
      float temperatureMin,
      float temperatureRange,
      SeededRandom rnd)
    {
      this.LogInfo("Apply foregreound", (this.node.tags != null).ToString(), this.node.tags == null ? 0.0f : (float) this.node.tags.Count);
      if (this.node.tags == null)
        return;
      FeatureSettings feature = settings.TryGetFeature(this.node.type);
      this.LogInfo("\tFeature?", (feature != null).ToString(), 0.0f);
      if (feature == null && this.node.tags != null)
      {
        List<Tag> tagList = new List<Tag>();
        foreach (Tag tag in this.node.tags)
        {
          if (settings.HasFeature(tag.Name))
            tagList.Add(tag);
        }
        this.LogInfo("\tNo feature, checking possible feature tags, found", string.Empty, (float) tagList.Count);
        if (tagList.Count > 0)
        {
          Tag tag = tagList[rnd.RandomSource().Next(tagList.Count)];
          feature = settings.GetFeature(tag.Name);
          this.LogInfo("\tPicked feature", tag.Name, 0.0f);
        }
      }
      if (feature == null)
        return;
      this.LogInfo("APPLY FOREGROUND", this.node.type, 0.0f);
      float size = feature.blobSize.GetRandomValueWithinRange(rnd);
      float closestEdge = this.poly.DistanceToClosestEdge(new Vector2?());
      if (!this.node.tags.Contains(WorldGenTags.AllowExceedNodeBorders) && (double) closestEdge < (double) size)
      {
        if (this.debugMode)
          Debug.LogWarning((object) (this.node.type + " " + (object) feature.shape + "  blob size too large to fit in node. Size reduced. " + (object) size + "->" + (closestEdge - 6f).ToString()));
        size = closestEdge - 6f;
      }
      if ((double) size <= 0.0)
        return;
      List<Vector2I> featureCenterPoints;
      List<List<Vector2I>> featureBorders;
      this.featureSpawnPoints = this.DigFeature(feature.shape, size, feature.borders, rnd, out featureCenterPoints, out featureBorders);
      this.LogInfo("\t\t", "claimed points", (float) this.featureSpawnPoints.Count);
      this.availableTerrainPoints.ExceptWith((IEnumerable<Vector2I>) this.featureSpawnPoints);
      this.ApplyPlaceElementForRoom(feature, "RoomCenterElements", featureCenterPoints, world, SetValues, temperatureMin, temperatureRange, rnd);
      if (featureBorders != null)
      {
        for (int index = 0; index < featureBorders.Count; ++index)
          this.ApplyPlaceElementForRoom(feature, "RoomBorderChoices" + (object) index, featureBorders[index], world, SetValues, temperatureMin, temperatureRange, rnd);
      }
      if (!feature.tags.Contains(WorldGenTags.HighPriorityFeature.Name))
        return;
      this.AddHighPriorityCells(this.featureSpawnPoints);
    }

    private void ApplyBackground(
      WorldGen worldGen,
      Chunk world,
      TerrainCell.SetValuesFunction SetValues,
      float temperatureMin,
      float temperatureRange,
      SeededRandom rnd)
    {
      this.LogInfo("Apply Background", this.node.type, 0.0f);
      float floatSetting1 = worldGen.Settings.GetFloatSetting("CaveOverrideMaxValue");
      float floatSetting2 = worldGen.Settings.GetFloatSetting("CaveOverrideSliverValue");
      Leaf leafForTerrainCell = worldGen.GetLeafForTerrainCell(this);
      bool flag1 = leafForTerrainCell.tags.Contains(WorldGenTags.IgnoreCaveOverride);
      bool flag2 = leafForTerrainCell.tags.Contains(WorldGenTags.CaveVoidSliver);
      bool flag3 = leafForTerrainCell.tags.Contains(WorldGenTags.ErodePointToCentroid);
      bool flag4 = leafForTerrainCell.tags.Contains(WorldGenTags.ErodePointToCentroidInv);
      bool flag5 = leafForTerrainCell.tags.Contains(WorldGenTags.ErodePointToEdge);
      bool flag6 = leafForTerrainCell.tags.Contains(WorldGenTags.ErodePointToEdgeInv);
      bool flag7 = leafForTerrainCell.tags.Contains(WorldGenTags.ErodePointToBorder);
      bool flag8 = leafForTerrainCell.tags.Contains(WorldGenTags.ErodePointToBorderInv);
      bool flag9 = leafForTerrainCell.tags.Contains(WorldGenTags.ErodePointToWorldTop);
      bool flag10 = leafForTerrainCell.tags.Contains(WorldGenTags.DistFunctionPointCentroid);
      bool flag11 = leafForTerrainCell.tags.Contains(WorldGenTags.DistFunctionPointEdge);
      Sim.DiseaseCell diseaseCell = new Sim.DiseaseCell();
      diseaseCell.diseaseIdx = byte.MaxValue;
      if (this.node.tags.Contains(WorldGenTags.Infected))
      {
        diseaseCell.diseaseIdx = (byte) rnd.RandomRange(0, WorldGen.diseaseIds.Count);
        this.node.tags.Add(new Tag("Infected:" + WorldGen.diseaseIds[(int) diseaseCell.diseaseIdx]));
        diseaseCell.elementCount = rnd.RandomRange(10000, 1000000);
      }
      this.LogInfo("Getting Element Bands", this.node.type, 0.0f);
      ElementBandConfiguration elementBandForBiome1 = worldGen.Settings.GetElementBandForBiome(this.node.type);
      if (elementBandForBiome1 == null && this.node.biomeSpecificTags != null)
      {
        this.LogInfo("\tType is not a biome, checking tags", string.Empty, (float) this.node.tags.Count);
        List<ElementBandConfiguration> bandConfigurationList = new List<ElementBandConfiguration>();
        foreach (Tag biomeSpecificTag in this.node.biomeSpecificTags)
        {
          ElementBandConfiguration elementBandForBiome2 = worldGen.Settings.GetElementBandForBiome(biomeSpecificTag.Name);
          if (elementBandForBiome2 != null)
          {
            bandConfigurationList.Add(elementBandForBiome2);
            this.LogInfo("\tFound biome", biomeSpecificTag.Name, 0.0f);
          }
        }
        if (bandConfigurationList.Count > 0)
        {
          int index = rnd.RandomSource().Next(bandConfigurationList.Count);
          elementBandForBiome1 = bandConfigurationList[index];
          this.LogInfo("\tPicked biome", string.Empty, (float) index);
        }
      }
      DebugUtil.Assert(elementBandForBiome1 != null, "A node didn't get assigned a biome! ", this.node.type);
      foreach (Vector2I availableTerrainPoint in this.availableTerrainPoints)
      {
        int cell = Grid.XYToCell(availableTerrainPoint.x, availableTerrainPoint.y);
        if (!TerrainCell.highPriorityClaims.Contains(cell))
        {
          float num1 = world.overrides[cell];
          if (!flag1 && (double) num1 >= 100.0)
          {
            if ((double) num1 >= 300.0)
              SetValues(cell, (object) WorldGen.voidElement, WorldGen.voidElement.defaultValues, Sim.DiseaseCell.Invalid);
            else if ((double) num1 >= 200.0)
              SetValues(cell, (object) WorldGen.unobtaniumElement, WorldGen.unobtaniumElement.defaultValues, Sim.DiseaseCell.Invalid);
            else
              SetValues(cell, (object) WorldGen.katairiteElement, WorldGen.katairiteElement.defaultValues, Sim.DiseaseCell.Invalid);
          }
          else
          {
            float erode = 1f;
            Vector2 vector2 = new Vector2((float) availableTerrainPoint.x, (float) availableTerrainPoint.y);
            if (flag3 || flag4)
            {
              float num2 = 15f;
              if (flag11)
              {
                float timeOnEdge = 0.0f;
                MathUtil.Pair<Vector2, Vector2> closestEdge = this.poly.GetClosestEdge(vector2, ref timeOnEdge);
                num2 = Vector2.Distance(closestEdge.First + (closestEdge.Second - closestEdge.First) * timeOnEdge, vector2);
              }
              erode = Mathf.Max(0.0f, Mathf.Min(1f, Vector2.Distance(this.poly.Centroid(), vector2) / num2));
              if (flag4)
                erode = 1f - erode;
            }
            if (flag6 || flag5)
            {
              float timeOnEdge = 0.0f;
              MathUtil.Pair<Vector2, Vector2> closestEdge = this.poly.GetClosestEdge(vector2, ref timeOnEdge);
              Vector2 a = closestEdge.First + (closestEdge.Second - closestEdge.First) * timeOnEdge;
              float num2 = 15f;
              if (flag10)
                num2 = Vector2.Distance(this.poly.Centroid(), vector2);
              erode = Mathf.Max(0.0f, Mathf.Min(1f, Vector2.Distance(a, vector2) / num2));
              if (flag6)
                erode = 1f - erode;
            }
            if (flag8 || flag7)
            {
              List<ProcGen.Map.Edge> edgesWithTag = worldGen.WorldLayout.overworldGraph.GetEdgesWithTag(WorldGenTags.EdgeClosed);
              float b = float.MaxValue;
              foreach (ProcGen.Map.Edge edge in edgesWithTag)
              {
                MathUtil.Pair<Vector2, Vector2> segment = new MathUtil.Pair<Vector2, Vector2>(edge.corner0.position, edge.corner1.position);
                float closest_point = 0.0f;
                b = Mathf.Min(Mathf.Abs(MathUtil.GetClosestPointBetweenPointAndLineSegment(segment, vector2, ref closest_point)), b);
              }
              float num2 = 7f;
              if (flag10)
                num2 = Vector2.Distance(this.poly.Centroid(), vector2);
              erode = Mathf.Max(0.0f, Mathf.Min(1f, b / num2));
              if (flag8)
                erode = 1f - erode;
            }
            if (flag9)
            {
              int y = worldGen.WorldSize.y;
              float num2 = 38f;
              float num3 = 58f;
              float num4 = (float) y - vector2.y;
              erode = (double) num4 >= (double) num2 ? ((double) num4 >= (double) num3 ? 1f : Mathf.Clamp01((float) (((double) num4 - (double) num2) / ((double) num3 - (double) num2)))) : 0.0f;
            }
            Element element;
            Sim.PhysicsData pd;
            Sim.DiseaseCell dc;
            worldGen.GetElementForBiomePoint(world, elementBandForBiome1, availableTerrainPoint, out element, out pd, out dc, erode);
            if (!element.IsVacuum && element.id != SimHashes.Katairite && element.id != SimHashes.Unobtanium)
            {
              if (element.lowTempTransition != null && (double) temperatureMin < (double) element.lowTemp)
                temperatureMin = element.lowTemp + 20f;
              pd.temperature = temperatureMin + world.heatOffset[cell] * temperatureRange;
            }
            if (element.IsSolid && !flag1 && ((double) num1 > (double) floatSetting1 && (double) num1 < 100.0))
            {
              element = !flag2 || (double) num1 <= (double) floatSetting2 ? WorldGen.vacuumElement : WorldGen.voidElement;
              pd = element.defaultValues;
            }
            if (dc.diseaseIdx == byte.MaxValue)
              dc = diseaseCell;
            SetValues(cell, (object) element, pd, dc);
          }
        }
      }
      if (this.node.tags.Contains(WorldGenTags.SprinkleOfOxyRock))
        this.HandleSprinkleOfElement(worldGen.Settings, WorldGenTags.SprinkleOfOxyRock, world, SetValues, temperatureMin, temperatureRange, rnd);
      if (!this.node.tags.Contains(WorldGenTags.SprinkleOfMetal))
        return;
      this.HandleSprinkleOfElement(worldGen.Settings, WorldGenTags.SprinkleOfMetal, world, SetValues, temperatureMin, temperatureRange, rnd);
    }

    private void GenerateActionCells(
      WorldGenSettings settings,
      Tag tag,
      HashSet<Vector2I> possiblePoints,
      SeededRandom rnd)
    {
      ProcGen.Room room = (ProcGen.Room) null;
      SettingsCache.rooms.TryGetValue(tag.Name, out room);
      SampleDescriber sampleDescriber = (SampleDescriber) room;
      if (sampleDescriber == null && settings.HasMob(tag.Name))
        sampleDescriber = (SampleDescriber) settings.GetMob(tag.Name);
      if (sampleDescriber == null)
        return;
      HashSet<Vector2I> vector2ISet = new HashSet<Vector2I>();
      float valueWithinRange = sampleDescriber.density.GetRandomValueWithinRange(rnd);
      List<Vector2> vector2List;
      switch (sampleDescriber.selectMethod)
      {
        case SampleDescriber.PointSelectionMethod.RandomPoints:
          vector2List = PointGenerator.GetRandomPoints(this.poly, valueWithinRange, 0.0f, (List<Vector2>) null, sampleDescriber.sampleBehaviour, true, rnd, true, true);
          break;
        default:
          vector2List = new List<Vector2>();
          vector2List.Add(this.node.position);
          break;
      }
      foreach (Vector2 vector2 in vector2List)
      {
        Vector2I vector2I = new Vector2I((int) vector2.x, (int) vector2.y);
        if (possiblePoints.Contains(vector2I))
          vector2ISet.Add(vector2I);
      }
      if (room == null || room.mobselection != ProcGen.Room.Selection.None)
        return;
      if (this.terrainPositions == null)
        this.terrainPositions = new List<KeyValuePair<int, Tag>>();
      foreach (Vector2I vector2I in vector2ISet)
      {
        int cell = Grid.XYToCell(vector2I.x, vector2I.y);
        if (Grid.IsValidCell(cell))
          this.terrainPositions.Add(new KeyValuePair<int, Tag>(cell, tag));
      }
    }

    private void DoProcess(
      WorldGen worldGen,
      Chunk world,
      TerrainCell.SetValuesFunction SetValues,
      SeededRandom rnd)
    {
      float min = 265f;
      float range = 30f;
      this.InitializeCells();
      this.GetTemperatureRange(worldGen, ref min, ref range);
      this.ApplyForeground(worldGen.Settings, world, SetValues, min, range, rnd);
      for (int index = 0; index < this.node.tags.Count; ++index)
        this.GenerateActionCells(worldGen.Settings, this.node.tags[index], this.availableTerrainPoints, rnd);
      this.ApplyBackground(worldGen, world, SetValues, min, range, rnd);
    }

    public void Process(
      WorldGen worldGen,
      Sim.Cell[] cells,
      float[] bgTemp,
      Sim.DiseaseCell[] dcs,
      Chunk world,
      SeededRandom rnd)
    {
      TerrainCell.SetValuesFunction SetValues = (TerrainCell.SetValuesFunction) ((index, elem, pd, dc) =>
      {
        if (Grid.IsValidCell(index))
        {
          if ((double) pd.temperature == 0.0 || (elem as Element).HasTag(GameTags.Special))
            bgTemp[index] = -1f;
          cells[index].SetValues(elem as Element, pd, ElementLoader.elements);
          dcs[index] = dc;
        }
        else
          Debug.LogError((object) ("Process::SetValuesFunction Index [" + (object) index + "] is not valid. cells.Length [" + (object) cells.Length + "]"));
      });
      this.DoProcess(worldGen, world, SetValues, rnd);
    }

    public void Process(WorldGen worldGen, Chunk world, SeededRandom rnd)
    {
      TerrainCell.SetValuesFunction SetValues = (TerrainCell.SetValuesFunction) ((index, elem, pd, dc) => SimMessages.ModifyCell(index, ElementLoader.GetElementIndex((elem as Element).id), pd.temperature, pd.mass, dc.diseaseIdx, dc.elementCount, SimMessages.ReplaceType.Replace, false, -1));
      this.DoProcess(worldGen, world, SetValues, rnd);
    }

    [OnDeserializing]
    internal void OnDeserializingMethod()
    {
      this.node = new ProcGen.Node();
      this.site = new Diagram.Site();
    }

    public bool IsSafeToSpawnFeatureTemplate(Tag additionalTag)
    {
      if (!this.node.tags.Contains(additionalTag))
        return !this.node.tags.ContainsOne(TerrainCell.noFeatureSpawnTagSet);
      return false;
    }

    public bool IsSafeToSpawnFeatureTemplate()
    {
      return !this.node.tags.ContainsOne(TerrainCell.noFeatureSpawnTagSet);
    }

    public bool IsSafeToSpawnPOI(List<TerrainCell> allCells)
    {
      foreach (uint num in this.terrain_neighbors_idx)
      {
        uint neighbor_idx = num;
        if (allCells.Find((Predicate<TerrainCell>) (cell => (int) cell.site.id == (int) neighbor_idx)).node.tags.ContainsOne(TerrainCell.noPOINeighborSpawnTagSet))
          return false;
      }
      return !this.node.tags.ContainsOne(TerrainCell.noPOISpawnTagSet);
    }

    public delegate void SetValuesFunction(
      int index,
      object elem,
      Sim.PhysicsData pd,
      Sim.DiseaseCell dc);

    public struct ElementOverride
    {
      public Element element;
      public Sim.PhysicsData pdelement;
      public Sim.DiseaseCell dc;
      public float mass;
      public float temperature;
      public byte diseaseIdx;
      public int diseaseAmount;
      public bool overrideMass;
      public bool overrideTemperature;
      public bool overrideDiseaseIdx;
      public bool overrideDiseaseAmount;
    }
  }
}
