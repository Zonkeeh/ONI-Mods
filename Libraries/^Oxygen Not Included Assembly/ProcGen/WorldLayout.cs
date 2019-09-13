// Decompiled with JetBrains decompiler
// Type: ProcGen.WorldLayout
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Delaunay.Geo;
using KSerialization;
using ObjectCloner;
using ProcGen.Map;
using ProcGenGame;
using Satsuma;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using VoronoiTree;

namespace ProcGen
{
  [SerializationConfig(MemberSerialization.OptIn)]
  public class WorldLayout
  {
    private Tree voronoiTree;
    [Serialize]
    public MapGraph localGraph;
    [Serialize]
    public MapGraph overworldGraph;
    [EnumFlags]
    public static WorldLayout.DebugFlags drawOptions;
    private LineSegment topEdge;
    private LineSegment bottomEdge;
    private LineSegment leftEdge;
    private LineSegment rightEdge;
    private SeededRandom myRandom;
    private WorldGen worldGen;
    [Serialize]
    private WorldLayout.ExtraIO extra;

    public WorldLayout(WorldGen worldGen, int seed)
    {
      this.worldGen = worldGen;
      this.localGraph = new MapGraph(seed);
      this.overworldGraph = new MapGraph(seed);
      this.SetSeed(seed);
    }

    public WorldLayout(WorldGen worldGen, int width, int height, int seed)
      : this(worldGen, seed)
    {
      this.mapWidth = width;
      this.mapHeight = height;
    }

    [Serialize]
    public int mapWidth { get; private set; }

    [Serialize]
    public int mapHeight { get; private set; }

    public bool layoutOK { get; private set; }

    public static LevelLayer levelLayerGradient { get; private set; }

    public void SetSeed(int seed)
    {
      this.myRandom = new SeededRandom(seed);
      this.localGraph.SetSeed(seed);
      this.overworldGraph.SetSeed(seed);
    }

    public Tree GetVoronoiTree()
    {
      return this.voronoiTree;
    }

    public static void SetLayerGradient(LevelLayer newGradient)
    {
      WorldLayout.levelLayerGradient = newGradient;
    }

    public static string GetNodeTypeFromLayers(Vector2 point, float mapHeight, SeededRandom rnd)
    {
      string name = WorldGenTags.TheVoid.Name;
      string str = WorldLayout.levelLayerGradient[WorldLayout.levelLayerGradient.Count - 1].content[rnd.RandomRange(0, WorldLayout.levelLayerGradient[WorldLayout.levelLayerGradient.Count - 1].content.Count)];
      for (int index1 = 0; index1 < WorldLayout.levelLayerGradient.Count; ++index1)
      {
        if ((double) point.y < (double) WorldLayout.levelLayerGradient[index1].maxValue * (double) mapHeight)
        {
          int index2 = rnd.RandomRange(0, WorldLayout.levelLayerGradient[index1].content.Count);
          str = WorldLayout.levelLayerGradient[index1].content[index2];
          break;
        }
      }
      return str;
    }

    public Tree GenerateOverworld(bool usePD)
    {
      Debug.Assert(this.mapWidth != 0 && this.mapHeight != 0, (object) "Map size has not been set");
      Debug.Assert(this.worldGen.Settings.world != null, (object) "You need to set a world");
      Diagram.Site site = new Diagram.Site(0U, new Vector2((float) (this.mapWidth / 2), (float) (this.mapHeight / 2)), 1f);
      this.topEdge = new LineSegment(new Vector2?(new Vector2(0.0f, (float) (this.mapHeight - 5))), new Vector2?(new Vector2((float) this.mapWidth, (float) (this.mapHeight - 5))));
      this.bottomEdge = new LineSegment(new Vector2?(new Vector2(0.0f, 5f)), new Vector2?(new Vector2((float) this.mapWidth, 5f)));
      this.leftEdge = new LineSegment(new Vector2?(new Vector2(5f, 0.0f)), new Vector2?(new Vector2(5f, (float) this.mapHeight)));
      this.rightEdge = new LineSegment(new Vector2?(new Vector2((float) (this.mapWidth - 5), 0.0f)), new Vector2?(new Vector2((float) (this.mapWidth - 5), (float) this.mapHeight)));
      site.poly = new Polygon(new Rect(0.0f, 0.0f, (float) this.mapWidth, (float) this.mapHeight));
      this.voronoiTree = new Tree(site, (Tree) null, this.myRandom.seed);
      VoronoiTree.Node.maxIndex = 0U;
      float density = this.myRandom.RandomRange(this.worldGen.Settings.GetFloatSetting("OverworldDensityMin"), this.worldGen.Settings.GetFloatSetting("OverworldDensityMax"));
      float floatSetting = this.worldGen.Settings.GetFloatSetting("OverworldAvoidRadius");
      PointGenerator.SampleBehaviour enumSetting = this.worldGen.Settings.GetEnumSetting<PointGenerator.SampleBehaviour>("OverworldSampleBehaviour");
      Debug.Log((object) string.Format("Generating overworld points using {0}, density {1}", (object) enumSetting.ToString(), (object) density));
      string startSubworldName = this.worldGen.Settings.world.startSubworldName;
      SubWorld subWorld = this.worldGen.Settings.GetSubWorld(startSubworldName);
      Vector2 newPos = new Vector2((float) this.mapWidth * this.worldGen.Settings.world.startingBasePositionHorizontal.GetRandomValueWithinRange(this.myRandom), (float) this.mapHeight * this.worldGen.Settings.world.startingBasePositionVertical.GetRandomValueWithinRange(this.myRandom));
      Debug.Log((object) ("Start node position is " + (object) newPos));
      Node node1 = this.overworldGraph.AddNode(startSubworldName);
      node1.SetPosition(newPos);
      VoronoiTree.Node vn = this.voronoiTree.AddSite(new Diagram.Site((uint) node1.node.Id, node1.position, subWorld.pdWeight), VoronoiTree.Node.NodeType.Internal);
      this.ApplySubworldToNode(vn, subWorld);
      List<Vector2> randomPoints = PointGenerator.GetRandomPoints(site.poly, density, floatSetting, new List<Vector2>()
      {
        node1.position
      }, enumSetting, false, this.myRandom, false, true);
      Debug.Log((object) string.Format(" -> Generated {0} points", (object) randomPoints.Count));
      int intSetting = this.worldGen.Settings.GetIntSetting("OverworldMaxNodes");
      if (randomPoints.Count > intSetting)
      {
        randomPoints.ShuffleSeeded<Vector2>(this.myRandom.RandomSource());
        randomPoints.RemoveRange(intSetting, randomPoints.Count - intSetting);
      }
      for (int index = 0; index < randomPoints.Count; ++index)
      {
        Node node2 = this.overworldGraph.AddNode(WorldGenTags.UnassignedNode.Name);
        node2.SetPosition(randomPoints[index]);
        this.voronoiTree.AddSite(new Diagram.Site((uint) node2.node.Id, node2.position, 1f), VoronoiTree.Node.NodeType.Internal).tags.Add(WorldGenTags.UnassignedNode);
        node2.tags.Add(WorldGenTags.UnassignedNode);
      }
      if (usePD)
      {
        List<Diagram.Site> diagramSites = new List<Diagram.Site>();
        for (int childIndex = 0; childIndex < this.voronoiTree.ChildCount(); ++childIndex)
          diagramSites.Add(this.voronoiTree.GetChild(childIndex).site);
        this.voronoiTree.ComputeNode(diagramSites);
        this.voronoiTree.ComputeNodePD(diagramSites, 500, 0.2f);
      }
      else
        this.voronoiTree.ComputeChildren(this.myRandom.seed + 1, false, false);
      this.voronoiTree.AddTagToChildren(WorldGenTags.Overworld);
      vn.AddTag(WorldGenTags.AtStart);
      this.TagTopAndBottomSites(WorldGenTags.AtSurface, WorldGenTags.AtDepths);
      this.TagEdgeSites(WorldGenTags.AtEdge, WorldGenTags.AtEdge);
      for (int childIndex = 0; childIndex < this.voronoiTree.ChildCount(); ++childIndex)
      {
        VoronoiTree.Node child = this.voronoiTree.GetChild(childIndex);
        Node nodeById1 = this.overworldGraph.FindNodeByID(child.site.id);
        nodeById1.tags.Union(child.tags);
        nodeById1.SetPosition(child.site.position);
        List<VoronoiTree.Node> neighbors = child.GetNeighbors();
        for (int index = 0; index < neighbors.Count; ++index)
        {
          Node nodeById2 = this.overworldGraph.FindNodeByID(neighbors[index].site.id);
          this.overworldGraph.AddArc(nodeById1, nodeById2, "Neighbor");
        }
      }
      this.PropagateDistanceTags(this.voronoiTree, WorldGenTags.DistanceTags);
      this.ConvertUnknownCells();
      this.voronoiTree.RelaxRecursive(0, this.worldGen.Settings.GetIntSetting("OverworldRelaxIterations"), this.worldGen.Settings.GetFloatSetting("OverworldRelaxEnergyMin"), usePD);
      if (this.worldGen.Settings.GetOverworldAddTags() != null)
      {
        foreach (string overworldAddTag in this.worldGen.Settings.GetOverworldAddTags())
          this.voronoiTree.GetChild(this.myRandom.RandomSource().Next(this.voronoiTree.ChildCount())).AddTag(new Tag(overworldAddTag));
      }
      this.FlattenOverworld();
      return this.voronoiTree;
    }

    public void PopulateSubworlds()
    {
      this.AddSubworldChildren();
      this.GetStartLocation();
      this.PropagateStartTag();
    }

    private void PropagateDistanceTags(Tree tree, TagSet tags)
    {
      foreach (Tag tag in tags)
      {
        Dictionary<uint, int> distanceToTag = this.overworldGraph.GetDistanceToTag(tag);
        if (distanceToTag != null)
        {
          int num = 0;
          for (int childIndex = 0; childIndex < tree.ChildCount(); ++childIndex)
          {
            VoronoiTree.Node child = tree.GetChild(childIndex);
            uint id = child.site.id;
            if (distanceToTag.ContainsKey(id))
            {
              child.minDistanceToTag.Add(tag, distanceToTag[id]);
              ++num;
              if (distanceToTag[id] > 0)
                child.AddTag(new Tag(tag.Name + "_Distance" + (object) distanceToTag[id]));
            }
          }
        }
      }
    }

    private char ConvertSignToCmp(int val)
    {
      if (val > 0)
        return '>';
      return val < 0 ? '<' : '=';
    }

    private HashSet<WeightedSubWorld> GetNameFilterSet(
      VoronoiTree.Node vn,
      World.AllowedCellsFilter filter,
      List<WeightedSubWorld> subworlds)
    {
      HashSet<WeightedSubWorld> weightedSubWorldSet = new HashSet<WeightedSubWorld>();
      switch (filter.tagcommand)
      {
        case World.AllowedCellsFilter.TagCommand.Default:
          for (int i = 0; i < filter.subworldNames.Count; ++i)
            weightedSubWorldSet.UnionWith((IEnumerable<WeightedSubWorld>) subworlds.FindAll((Predicate<WeightedSubWorld>) (f => f.subWorld.name == filter.subworldNames[i])));
          break;
        case World.AllowedCellsFilter.TagCommand.AtTag:
          if (vn.tags.Contains((Tag) filter.tag))
          {
            for (int i = 0; i < filter.subworldNames.Count; ++i)
              weightedSubWorldSet.UnionWith((IEnumerable<WeightedSubWorld>) subworlds.FindAll((Predicate<WeightedSubWorld>) (f => f.subWorld.name == filter.subworldNames[i])));
            break;
          }
          break;
        case World.AllowedCellsFilter.TagCommand.DistanceFromTag:
          Debug.Assert(vn.minDistanceToTag.ContainsKey(filter.tag.ToTag()), (object) filter.tag);
          if (vn.minDistanceToTag[filter.tag.ToTag()] >= filter.minDistance && vn.minDistanceToTag[filter.tag.ToTag()] <= filter.maxDistance)
          {
            for (int i = 0; i < filter.subworldNames.Count; ++i)
              weightedSubWorldSet.UnionWith((IEnumerable<WeightedSubWorld>) subworlds.FindAll((Predicate<WeightedSubWorld>) (f => f.subWorld.name == filter.subworldNames[i])));
            break;
          }
          break;
      }
      return weightedSubWorldSet;
    }

    private HashSet<WeightedSubWorld> GetZoneTypeFilterSet(
      VoronoiTree.Node vn,
      World.AllowedCellsFilter filter,
      Dictionary<string, List<WeightedSubWorld>> subworldsByZoneType)
    {
      HashSet<WeightedSubWorld> weightedSubWorldSet = new HashSet<WeightedSubWorld>();
      switch (filter.tagcommand)
      {
        case World.AllowedCellsFilter.TagCommand.Default:
          for (int index = 0; index < filter.zoneTypes.Count; ++index)
            weightedSubWorldSet.UnionWith((IEnumerable<WeightedSubWorld>) subworldsByZoneType[filter.zoneTypes[index].ToString()]);
          break;
        case World.AllowedCellsFilter.TagCommand.AtTag:
          if (vn.tags.Contains((Tag) filter.tag))
          {
            for (int index = 0; index < filter.zoneTypes.Count; ++index)
              weightedSubWorldSet.UnionWith((IEnumerable<WeightedSubWorld>) subworldsByZoneType[filter.zoneTypes[index].ToString()]);
            break;
          }
          break;
        case World.AllowedCellsFilter.TagCommand.DistanceFromTag:
          Debug.Assert(vn.minDistanceToTag.ContainsKey(filter.tag.ToTag()), (object) filter.tag);
          if (vn.minDistanceToTag[filter.tag.ToTag()] >= filter.minDistance && vn.minDistanceToTag[filter.tag.ToTag()] <= filter.maxDistance)
          {
            for (int index = 0; index < filter.zoneTypes.Count; ++index)
              weightedSubWorldSet.UnionWith((IEnumerable<WeightedSubWorld>) subworldsByZoneType[filter.zoneTypes[index].ToString()]);
            break;
          }
          break;
      }
      return weightedSubWorldSet;
    }

    private HashSet<WeightedSubWorld> GetTemperatureFilterSet(
      VoronoiTree.Node vn,
      World.AllowedCellsFilter filter,
      Dictionary<string, List<WeightedSubWorld>> subworldsByTemperature)
    {
      HashSet<WeightedSubWorld> weightedSubWorldSet = new HashSet<WeightedSubWorld>();
      switch (filter.tagcommand)
      {
        case World.AllowedCellsFilter.TagCommand.Default:
          for (int index = 0; index < filter.temperatureRanges.Count; ++index)
            weightedSubWorldSet.UnionWith((IEnumerable<WeightedSubWorld>) subworldsByTemperature[filter.temperatureRanges[index].ToString()]);
          break;
        case World.AllowedCellsFilter.TagCommand.AtTag:
          if (vn.tags.Contains((Tag) filter.tag))
          {
            for (int index = 0; index < filter.temperatureRanges.Count; ++index)
              weightedSubWorldSet.UnionWith((IEnumerable<WeightedSubWorld>) subworldsByTemperature[filter.temperatureRanges[index].ToString()]);
            break;
          }
          break;
        case World.AllowedCellsFilter.TagCommand.DistanceFromTag:
          Debug.Assert(vn.minDistanceToTag.ContainsKey(filter.tag.ToTag()), (object) filter.tag);
          if (vn.minDistanceToTag[filter.tag.ToTag()] >= filter.minDistance && vn.minDistanceToTag[filter.tag.ToTag()] <= filter.maxDistance)
          {
            for (int index = 0; index < filter.temperatureRanges.Count; ++index)
              weightedSubWorldSet.UnionWith((IEnumerable<WeightedSubWorld>) subworldsByTemperature[filter.temperatureRanges[index].ToString()]);
            break;
          }
          break;
      }
      return weightedSubWorldSet;
    }

    private void RunFilterClearCommand(
      VoronoiTree.Node vn,
      World.AllowedCellsFilter filter,
      HashSet<WeightedSubWorld> allowedSubworldsSet)
    {
      switch (filter.tagcommand)
      {
        case World.AllowedCellsFilter.TagCommand.Default:
          allowedSubworldsSet.Clear();
          break;
        case World.AllowedCellsFilter.TagCommand.AtTag:
          if (!vn.tags.Contains((Tag) filter.tag))
            break;
          allowedSubworldsSet.Clear();
          break;
        case World.AllowedCellsFilter.TagCommand.DistanceFromTag:
          Debug.Assert(vn.minDistanceToTag.ContainsKey(filter.tag.ToTag()), (object) filter.tag);
          if (vn.minDistanceToTag[filter.tag.ToTag()] < filter.minDistance || vn.minDistanceToTag[filter.tag.ToTag()] > filter.maxDistance)
            break;
          allowedSubworldsSet.Clear();
          break;
      }
    }

    private HashSet<WeightedSubWorld> Filter(
      VoronoiTree.Node vn,
      List<WeightedSubWorld> allSubWorlds,
      Dictionary<string, List<WeightedSubWorld>> subworldsByTemperature,
      Dictionary<string, List<WeightedSubWorld>> subworldsByZoneType)
    {
      HashSet<WeightedSubWorld> allowedSubworldsSet = new HashSet<WeightedSubWorld>();
      World world = this.worldGen.Settings.world;
      string str = string.Empty;
      foreach (KeyValuePair<Tag, int> keyValuePair in vn.minDistanceToTag)
        str = str + keyValuePair.Key.Name + ":" + keyValuePair.Value.ToString() + ", ";
      foreach (World.AllowedCellsFilter cellsAllowedSubworld in world.unknownCellsAllowedSubworlds)
      {
        HashSet<WeightedSubWorld> weightedSubWorldSet = new HashSet<WeightedSubWorld>();
        if (cellsAllowedSubworld.subworldNames != null && cellsAllowedSubworld.subworldNames.Count > 0)
          weightedSubWorldSet.UnionWith((IEnumerable<WeightedSubWorld>) this.GetNameFilterSet(vn, cellsAllowedSubworld, allSubWorlds));
        if (cellsAllowedSubworld.temperatureRanges != null && cellsAllowedSubworld.temperatureRanges.Count > 0)
          weightedSubWorldSet.UnionWith((IEnumerable<WeightedSubWorld>) this.GetTemperatureFilterSet(vn, cellsAllowedSubworld, subworldsByTemperature));
        if (cellsAllowedSubworld.zoneTypes != null && cellsAllowedSubworld.zoneTypes.Count > 0)
          weightedSubWorldSet.UnionWith((IEnumerable<WeightedSubWorld>) this.GetZoneTypeFilterSet(vn, cellsAllowedSubworld, subworldsByZoneType));
        switch (cellsAllowedSubworld.command)
        {
          case World.AllowedCellsFilter.Command.Clear:
            this.RunFilterClearCommand(vn, cellsAllowedSubworld, allowedSubworldsSet);
            continue;
          case World.AllowedCellsFilter.Command.Replace:
            if (weightedSubWorldSet.Count > 0)
            {
              allowedSubworldsSet.Clear();
              allowedSubworldsSet.UnionWith((IEnumerable<WeightedSubWorld>) weightedSubWorldSet);
              continue;
            }
            continue;
          case World.AllowedCellsFilter.Command.UnionWith:
            allowedSubworldsSet.UnionWith((IEnumerable<WeightedSubWorld>) weightedSubWorldSet);
            continue;
          case World.AllowedCellsFilter.Command.IntersectWith:
            allowedSubworldsSet.IntersectWith((IEnumerable<WeightedSubWorld>) weightedSubWorldSet);
            continue;
          case World.AllowedCellsFilter.Command.ExceptWith:
            allowedSubworldsSet.ExceptWith((IEnumerable<WeightedSubWorld>) weightedSubWorldSet);
            continue;
          case World.AllowedCellsFilter.Command.SymmetricExceptWith:
            allowedSubworldsSet.SymmetricExceptWith((IEnumerable<WeightedSubWorld>) weightedSubWorldSet);
            continue;
          default:
            continue;
        }
      }
      return allowedSubworldsSet;
    }

    private void ConvertUnknownCells()
    {
      List<VoronoiTree.Node> nodes = new List<VoronoiTree.Node>();
      this.voronoiTree.GetNodesWithTag(WorldGenTags.UnassignedNode, nodes);
      List<WeightedName> subworldList = new List<WeightedName>((IEnumerable<WeightedName>) this.worldGen.Settings.world.subworldFiles);
      subworldList.RemoveAll((Predicate<WeightedName>) (s => s.name == this.worldGen.Settings.world.startSubworldName));
      List<WeightedSubWorld> subworldsForWorld = this.worldGen.Settings.GetSubworldsForWorld(subworldList);
      Dictionary<string, List<WeightedSubWorld>> subworldsByTemperature = new Dictionary<string, List<WeightedSubWorld>>();
      IEnumerator enumerator1 = Enum.GetValues(typeof (Temperature.Range)).GetEnumerator();
      try
      {
        while (enumerator1.MoveNext())
        {
          Temperature.Range range = (Temperature.Range) enumerator1.Current;
          subworldsByTemperature.Add(range.ToString(), subworldsForWorld.FindAll((Predicate<WeightedSubWorld>) (sw => sw.subWorld.temperatureRange == range)));
        }
      }
      finally
      {
        if (enumerator1 is IDisposable disposable)
          disposable.Dispose();
      }
      Dictionary<string, List<WeightedSubWorld>> subworldsByZoneType = new Dictionary<string, List<WeightedSubWorld>>();
      IEnumerator enumerator2 = Enum.GetValues(typeof (SubWorld.ZoneType)).GetEnumerator();
      try
      {
        while (enumerator2.MoveNext())
        {
          SubWorld.ZoneType zt = (SubWorld.ZoneType) enumerator2.Current;
          subworldsByZoneType.Add(zt.ToString(), subworldsForWorld.FindAll((Predicate<WeightedSubWorld>) (sw => sw.subWorld.zoneType == zt)));
        }
      }
      finally
      {
        if (enumerator2 is IDisposable disposable)
          disposable.Dispose();
      }
      foreach (VoronoiTree.Node vn in nodes)
      {
        Node nodeById = this.overworldGraph.FindNodeByID(vn.site.id);
        vn.tags.Remove(WorldGenTags.UnassignedNode);
        nodeById.tags.Remove(WorldGenTags.UnassignedNode);
        WeightedSubWorld weightedSubWorld = WeightedRandom.Choose<WeightedSubWorld>(new List<WeightedSubWorld>((IEnumerable<WeightedSubWorld>) this.Filter(vn, subworldsForWorld, subworldsByTemperature, subworldsByZoneType)), this.myRandom);
        if (weightedSubWorld != null)
        {
          SubWorld subWorld = weightedSubWorld.subWorld;
          this.ApplySubworldToNode(vn, subWorld);
        }
        else
        {
          string str = string.Empty;
          foreach (KeyValuePair<Tag, int> keyValuePair in vn.minDistanceToTag)
            str = str + keyValuePair.Key.Name + ":" + keyValuePair.Value.ToString() + ", ";
          DebugUtil.LogWarningArgs((object) "No allowed Subworld types. Using default. ", (object) nodeById.tags.ToString(), (object) "Distances:", (object) str);
          nodeById.SetType("Default");
        }
      }
    }

    private Node ApplySubworldToNode(VoronoiTree.Node vn, SubWorld subWorld)
    {
      Node nodeById = this.overworldGraph.FindNodeByID(vn.site.id);
      nodeById.SetType(subWorld.name);
      vn.site.weight = subWorld.pdWeight;
      foreach (string tag in subWorld.tags)
        vn.AddTag(new Tag(tag));
      return nodeById;
    }

    private void FlattenOverworld()
    {
      try
      {
        for (int childIndex = 0; childIndex < this.voronoiTree.ChildCount(); ++childIndex)
        {
          VoronoiTree.Node child = this.voronoiTree.GetChild(childIndex);
          if (child.type == VoronoiTree.Node.NodeType.Internal)
          {
            Tree tree = child as Tree;
            Node nodeById = this.overworldGraph.FindNodeByID(tree.site.id);
            nodeById.tags.Union(tree.tags);
            bool didCreate;
            ProcGen.Map.Cell cell = this.overworldGraph.GetCell(nodeById.position, nodeById.node, true, out didCreate);
            Debug.Assert(didCreate, (object) ("Tried creating a new cell but one already exists. Huh? " + (object) child.site.id));
            cell.tags.Union(tree.tags);
          }
        }
        for (int childIndex = 0; childIndex < this.voronoiTree.ChildCount(); ++childIndex)
        {
          VoronoiTree.Node child = this.voronoiTree.GetChild(childIndex);
          if (child.type == VoronoiTree.Node.NodeType.Internal)
          {
            List<KeyValuePair<VoronoiTree.Node, LineSegment>> neighborsByEdge = (child as Tree).GetNeighborsByEdge();
            for (int index = 0; index < neighborsByEdge.Count; ++index)
            {
              KeyValuePair<VoronoiTree.Node, LineSegment> keyValuePair = neighborsByEdge[index];
              this.overworldGraph.GetCorner(keyValuePair.Value.p0.Value, true);
              this.overworldGraph.GetCorner(keyValuePair.Value.p1.Value, true);
            }
          }
        }
        TagSet others = new TagSet();
        others.Add(WorldGenTags.NearDepths);
        for (int childIndex = 0; childIndex < this.voronoiTree.ChildCount(); ++childIndex)
        {
          VoronoiTree.Node child = this.voronoiTree.GetChild(childIndex);
          if (child.type == VoronoiTree.Node.NodeType.Internal)
          {
            Tree tree = child as Tree;
            Node nodeById1 = this.overworldGraph.FindNodeByID(tree.site.id);
            ProcGen.Map.Cell cell1 = this.overworldGraph.GetCell(nodeById1.node);
            Debug.Assert(cell1 != null, (object) ("cell is null: " + (object) nodeById1.node));
            List<KeyValuePair<VoronoiTree.Node, LineSegment>> neighborsByEdge = tree.GetNeighborsByEdge();
            for (int index = 0; index < neighborsByEdge.Count; ++index)
            {
              KeyValuePair<VoronoiTree.Node, LineSegment> keyValuePair = neighborsByEdge[index];
              Corner corner1 = this.overworldGraph.GetCorner(keyValuePair.Value.p0.Value, false);
              Debug.Assert(corner1 != null, (object) ("corner0 is null: " + (object) keyValuePair.Value.p0));
              Corner corner2 = this.overworldGraph.GetCorner(keyValuePair.Value.p1.Value, false);
              Debug.Assert(corner2 != null, (object) ("corner1 is null: " + (object) keyValuePair.Value.p1));
              VoronoiTree.Node key = keyValuePair.Key;
              ProcGen.Map.Edge edge;
              if (key != null)
              {
                Node nodeById2 = this.overworldGraph.FindNodeByID(key.site.id);
                ProcGen.Map.Cell cell2 = this.overworldGraph.GetCell(nodeById2.node);
                Debug.Assert(cell2 != null, (object) ("otherCell is null: " + (object) nodeById2.node));
                bool didCreate;
                edge = this.overworldGraph.GetEdge(corner1, corner2, cell1, cell2, true, out didCreate);
                SubWorld subWorld1 = this.worldGen.Settings.GetSubWorld(nodeById1.type);
                Debug.Assert(subWorld1 != null, (object) ("SubWorld is null: " + nodeById1.type));
                SubWorld subWorld2 = this.worldGen.Settings.GetSubWorld(nodeById2.type);
                Debug.Assert(subWorld2 != null, (object) ("other SubWorld is null: " + nodeById2.type));
                if (nodeById1.type == nodeById2.type || subWorld1.zoneType == subWorld2.zoneType || subWorld1.zoneType == SubWorld.ZoneType.Space && subWorld2.zoneType == SubWorld.ZoneType.Space || cell1.tags.ContainsOne(others) && cell2.tags.ContainsOne(others))
                  edge.tags.Add(WorldGenTags.EdgeOpen);
                else
                  edge.tags.Add(WorldGenTags.EdgeClosed);
                cell2.Add(edge);
              }
              else
              {
                bool didCreate;
                edge = this.overworldGraph.GetEdge(corner1, corner2, cell1, cell1, true, out didCreate);
                edge.tags.Add(WorldGenTags.EdgeUnpassable);
              }
              cell1.Add(edge);
            }
          }
        }
      }
      catch (Exception ex)
      {
        Debug.LogError((object) ("ex: " + ex.Message + " " + ex.StackTrace));
      }
    }

    public static bool TestEdgeConsistency(ProcGen.Map.Cell cell, out ProcGen.Map.Edge problemEdge)
    {
      for (int index = 0; index < cell.edges.Count; ++index)
      {
        ProcGen.Map.Edge edge = cell.edges[index];
        if (!WorldLayout.IsEdgeConsistent(cell, edge))
        {
          problemEdge = edge;
          return false;
        }
      }
      problemEdge = (ProcGen.Map.Edge) null;
      return true;
    }

    public static bool IsEdgeConsistent(ProcGen.Map.Cell cell, ProcGen.Map.Edge edge1)
    {
      bool flag1 = false;
      bool flag2 = false;
      for (int index = 0; index < cell.edges.Count; ++index)
      {
        ProcGen.Map.Edge edge = cell.edges[index];
        if (edge1 != edge)
        {
          if (edge1.corner0 == edge.corner0 || edge1.corner0 == edge.corner1)
            flag1 = true;
          if (edge1.corner1 == edge.corner0 || edge1.corner1 == edge.corner1)
            flag2 = true;
        }
      }
      return flag1 && flag2;
    }

    public bool IsNodeBorderOpen(VoronoiTree.Node n1, VoronoiTree.Node n2, TagSet edgeOpenTags)
    {
      Debug.Assert(n1 != null, (object) "Border test: n1 was null");
      Debug.Assert(n2 != null, (object) "Border test: n2 was null");
      Node nodeById1 = this.overworldGraph.FindNodeByID(n1.site.id);
      Node nodeById2 = this.overworldGraph.FindNodeByID(n2.site.id);
      Debug.Assert(nodeById1 != null, (object) "Border test: tn1 was null");
      Debug.Assert(nodeById2 != null, (object) "Border test: tn2 was null");
      ProcGen.Map.Cell cell1 = this.overworldGraph.GetCell(nodeById1.node);
      ProcGen.Map.Cell cell2 = this.overworldGraph.GetCell(nodeById2.node);
      Debug.Assert(cell1 != null, (object) "Border test: cell1 was null");
      Debug.Assert(cell2 != null, (object) "Border test: cell2 was null");
      SubWorld subWorld1 = this.worldGen.Settings.GetSubWorld(nodeById1.type);
      SubWorld subWorld2 = this.worldGen.Settings.GetSubWorld(nodeById2.type);
      Debug.Assert(subWorld1 != null, (object) "Border test: sw1 was null");
      Debug.Assert(subWorld2 != null, (object) "Border test: sw2 was null");
      if (nodeById1.type == nodeById2.type || subWorld1.zoneType == subWorld2.zoneType || subWorld1.zoneType == SubWorld.ZoneType.Space && subWorld2.zoneType == SubWorld.ZoneType.Space)
        return true;
      if (cell1.tags.ContainsOne(edgeOpenTags))
        return cell2.tags.ContainsOne(edgeOpenTags);
      return false;
    }

    private void AddSubworldChildren()
    {
      TagSet tagSet1 = new TagSet();
      tagSet1.Add(WorldGenTags.Overworld);
      TagSet tagSet2 = new TagSet((IEnumerable<string>) this.worldGen.Settings.GetDefaultMoveTags());
      VoronoiTree.Node.SplitCommand cmd = new VoronoiTree.Node.SplitCommand();
      cmd.dontCopyTags = tagSet1;
      cmd.moveTags = tagSet2;
      cmd.SplitFunction = new System.Action<Tree, VoronoiTree.Node.SplitCommand>(this.SplitFunction);
      List<Feature> featureList = new List<Feature>();
      foreach (KeyValuePair<string, int> globalFeature in this.worldGen.Settings.world.globalFeatures)
      {
        for (int index = 0; index < globalFeature.Value; ++index)
          featureList.Add(new Feature()
          {
            type = globalFeature.Key
          });
      }
      Dictionary<uint, List<Feature>> dictionary = new Dictionary<uint, List<Feature>>();
      List<VoronoiTree.Node> nodeList = new List<VoronoiTree.Node>();
      this.voronoiTree.GetNodesWithoutTag(WorldGenTags.NoGlobalFeatureSpawning, nodeList);
      nodeList.ShuffleSeeded<VoronoiTree.Node>(this.myRandom.RandomSource());
      foreach (Feature feature in featureList)
      {
        VoronoiTree.Node node = nodeList[0];
        nodeList.RemoveAt(0);
        if (!dictionary.ContainsKey(node.site.id))
          dictionary[node.site.id] = new List<Feature>();
        dictionary[node.site.id].Add(feature);
      }
      for (int childIndex1 = 0; childIndex1 < this.voronoiTree.ChildCount(); ++childIndex1)
      {
        VoronoiTree.Node child = this.voronoiTree.GetChild(childIndex1);
        if (child.type == VoronoiTree.Node.NodeType.Internal)
        {
          Tree node = child as Tree;
          Node nodeById1 = this.overworldGraph.FindNodeByID(node.site.id);
          SubWorld sw = SerializingCloner.Copy<SubWorld>(this.worldGen.Settings.GetSubWorld(nodeById1.type));
          node.AddTag(new Tag(nodeById1.type));
          node.AddTag(new Tag(sw.temperatureRange.ToString()));
          if (dictionary.ContainsKey(child.site.id))
            sw.features.AddRange((IEnumerable<Feature>) dictionary[child.site.id]);
          this.GenerateChildren(sw, node, (Graph) this.localGraph, (float) this.mapHeight, childIndex1 + this.myRandom.seed);
          int num = node.ChildCount();
          if (num < sw.minChildCount)
          {
            node.AddTag(WorldGenTags.DEBUG_SplitForChildCount);
            cmd.dontCopyTags = tagSet1;
            cmd.minChildCount = sw.minChildCount - num;
            node.Split(cmd);
            if (sw.biomes != null && sw.biomes.Count > 0)
            {
              for (int childIndex2 = num; childIndex2 < node.ChildCount(); ++childIndex2)
              {
                WeightedBiome weightedBiome = WeightedRandom.Choose<WeightedBiome>(sw.biomes, this.myRandom);
                Node nodeById2 = this.localGraph.FindNodeByID(node.GetChild(childIndex2).site.id);
                nodeById2.SetType(weightedBiome.name);
                node.GetChild(childIndex2).AddTag(new Tag(nodeById2.type));
              }
            }
            else
            {
              for (int childIndex2 = num; childIndex2 < node.ChildCount(); ++childIndex2)
              {
                Node nodeById2 = this.localGraph.FindNodeByID(node.GetChild(childIndex2).site.id);
                nodeById2.SetType(WorldLayout.GetNodeTypeFromLayers(node.site.position, (float) this.mapHeight, this.myRandom));
                node.GetChild(childIndex2).AddTag(new Tag(nodeById2.type));
              }
            }
          }
          node.RelaxRecursive(0, 10, 1f, this.worldGen.Settings.world.layoutMethod == World.LayoutMethod.PowerTree);
          List<VoronoiTree.Node> nodes = new List<VoronoiTree.Node>();
          node.GetNodesWithTag(WorldGenTags.Feature, nodes);
          cmd.dontCopyTags = new TagSet()
          {
            WorldGenTags.Feature,
            WorldGenTags.SplitOnParentDensity
          };
          for (int index = 0; index < nodes.Count; ++index)
          {
            if (!nodes[index].tags.Contains(WorldGenTags.CenteralFeature))
            {
              if (nodes[index].tags.Contains(WorldGenTags.SplitOnParentDensity))
                nodes[index].Split(cmd);
              if (nodes[index].tags.Contains(WorldGenTags.SplitTwice))
              {
                Tree tree = nodes[index].Split(cmd);
                if (tree.ChildCount() <= 1)
                  Debug.LogError((object) "split did not work.");
                for (int childIndex2 = 0; childIndex2 < tree.ChildCount(); ++childIndex2)
                  tree.GetChild(childIndex2).Split(cmd);
              }
            }
          }
        }
      }
      VoronoiTree.Node.maxDepth = this.voronoiTree.MaxDepth(0);
    }

    private List<Vector2> GetPoints(
      string name,
      LoggerSSF log,
      int minPointCount,
      Polygon boundingArea,
      float density,
      float avoidRadius,
      List<Vector2> avoidPoints,
      PointGenerator.SampleBehaviour sampleBehaviour,
      bool testInsideBounds,
      SeededRandom rnd,
      bool doShuffle = true,
      bool testAvoidPoints = true)
    {
      int num = 0;
      List<Vector2> randomPoints;
      do
      {
        randomPoints = PointGenerator.GetRandomPoints(boundingArea, density, avoidRadius, avoidPoints, sampleBehaviour, testInsideBounds, rnd, doShuffle, testAvoidPoints);
        if (randomPoints.Count < minPointCount)
        {
          density *= 0.8f;
          if (!this.worldGen.isRunningDebugGen)
            ;
        }
        ++num;
      }
      while (randomPoints.Count < minPointCount && num < 10);
      return randomPoints;
    }

    public void GenerateChildren(
      SubWorld sw,
      Tree node,
      Graph graph,
      float worldHeight,
      int seed)
    {
      SeededRandom rnd = new SeededRandom(seed);
      TagSet tagSet1 = new TagSet((IEnumerable<string>) this.worldGen.Settings.GetDefaultMoveTags());
      TagSet tagSet2 = new TagSet();
      if (tagSet1 != null)
      {
        for (int index = 0; index < tagSet1.Count; ++index)
        {
          Tag tag = tagSet1[index];
          if (node.tags.Contains(tag))
          {
            node.tags.Remove(tag);
            tagSet2.Add(tag);
          }
        }
      }
      TagSet newTags = new TagSet(node.tags);
      newTags.Remove(WorldGenTags.Overworld);
      for (int index = 0; index < sw.tags.Count; ++index)
        newTags.Add(new Tag(sw.tags[index]));
      float valueWithinRange1 = sw.density.GetRandomValueWithinRange(rnd);
      List<Vector2> avoidPoints = new List<Vector2>();
      if (sw.centralFeature != null)
      {
        avoidPoints.Add(node.site.poly.Centroid());
        this.CreateTreeNodeWithFeatureAndBiome(this.worldGen.Settings, sw, node, graph, sw.centralFeature, node.site.poly.Centroid(), newTags, -1).AddTag(WorldGenTags.CenteralFeature);
      }
      node.dontRelaxChildren = sw.dontRelaxChildren;
      int minPointCount = sw.features.Count <= 0 ? 2 : sw.features.Count;
      List<Vector2> points = this.GetPoints(sw.name, node.log, minPointCount, node.site.poly, valueWithinRange1, sw.avoidRadius, avoidPoints, sw.sampleBehaviour, true, rnd, true, sw.doAvoidPoints);
      for (int index = 0; index < sw.samplers.Count; ++index)
      {
        avoidPoints.AddRange((IEnumerable<Vector2>) points);
        float valueWithinRange2 = sw.samplers[index].density.GetRandomValueWithinRange(rnd);
        List<Vector2> randomPoints = PointGenerator.GetRandomPoints(node.site.poly, valueWithinRange2, sw.samplers[index].avoidRadius, avoidPoints, sw.samplers[index].sampleBehaviour, true, rnd, true, sw.samplers[index].doAvoidPoints);
        points.AddRange((IEnumerable<Vector2>) randomPoints);
      }
      if (points.Count > 200)
        points.RemoveRange(200, points.Count - 200);
      if (points.Count < minPointCount)
      {
        string str = string.Empty;
        for (int index = 0; index < node.site.poly.Vertices.Count; ++index)
          str = str + (object) node.site.poly.Vertices[index] + ", ";
        if (!this.worldGen.isRunningDebugGen)
          return;
        Debug.Assert((points.Count >= minPointCount ? 1 : 0) != 0, (object) ("Error not enough points " + sw.name + " in node " + (object) node.site.id));
      }
      else
      {
        if (sw.features.Count <= points.Count)
          ;
        for (int i = 0; i < points.Count; ++i)
        {
          Feature feature = (Feature) null;
          if (i < sw.features.Count)
            feature = sw.features[i];
          this.CreateTreeNodeWithFeatureAndBiome(this.worldGen.Settings, sw, node, graph, feature, points[i], newTags, i);
        }
        node.ComputeChildren(rnd.seed + 1, false, false);
        if (node.ChildCount() <= 0)
          return;
        for (int index = 0; index < tagSet2.Count; ++index)
        {
          Debug.Log((object) string.Format("Applying Moved Tag {0} to {1}", (object) tagSet2[index].Name, (object) node.site.id));
          node.GetChild(rnd.RandomSource().Next(node.ChildCount())).AddTag(tagSet2[index]);
        }
      }
    }

    private VoronoiTree.Node CreateTreeNodeWithFeatureAndBiome(
      WorldGenSettings settings,
      SubWorld sw,
      Tree node,
      Graph graph,
      Feature feature,
      Vector2 pos,
      TagSet newTags,
      int i)
    {
      bool flag = false;
      TagSet tagSet1 = new TagSet();
      TagSet tagSet2 = new TagSet();
      if (feature != null)
      {
        FeatureSettings feature1 = settings.GetFeature(feature.type);
        string type = feature.type;
        tagSet2.Union(new TagSet((IEnumerable<string>) feature1.tags));
        if (feature.tags != null && feature.tags.Count > 0)
          tagSet2.Union(new TagSet((IEnumerable<string>) feature.tags));
        if (feature.excludesTags != null && feature.excludesTags.Count > 0)
          tagSet2.Remove(new TagSet((IEnumerable<string>) feature.excludesTags));
        tagSet2.Add(new Tag(feature.type));
        tagSet2.Add(WorldGenTags.Feature);
        if (feature1.forceBiome != null)
        {
          tagSet1.Add((Tag) feature1.forceBiome);
          flag = true;
        }
        if (feature1.biomeTags != null)
          tagSet1.Union(new TagSet((IEnumerable<string>) feature1.biomeTags));
      }
      string type1;
      if (!flag && sw.biomes.Count > 0)
      {
        WeightedBiome weightedBiome = WeightedRandom.Choose<WeightedBiome>(sw.biomes, this.myRandom);
        type1 = weightedBiome.name;
        tagSet1.Add((Tag) weightedBiome.name);
        if (weightedBiome.tags != null && weightedBiome.tags.Count > 0)
          tagSet1.Union(new TagSet((IEnumerable<string>) weightedBiome.tags));
      }
      else
        type1 = "UNKNOWN";
      Node node1 = graph.AddNode(type1);
      node1.biomeSpecificTags = new TagSet(tagSet1);
      node1.featureSpecificTags = new TagSet(tagSet2);
      node1.SetPosition(pos);
      VoronoiTree.Node node2 = node.AddSite(new Diagram.Site((uint) node1.node.Id, node1.position, 1f), VoronoiTree.Node.NodeType.Internal);
      node2.tags = new TagSet(newTags);
      node2.tags.Add((Tag) type1);
      node2.tags.Union(tagSet1);
      node2.tags.Union(tagSet2);
      return node2;
    }

    private void SplitTopAndBottomSites()
    {
      float floatSetting = this.worldGen.Settings.GetFloatSetting("SplitTopAndBottomSitesMaxArea");
      TagSet tagSet1 = new TagSet();
      tagSet1.Add(WorldGenTags.Overworld);
      TagSet tagSet2 = new TagSet((IEnumerable<string>) this.worldGen.Settings.GetDefaultMoveTags());
      List<VoronoiTree.Node> nodes1 = new List<VoronoiTree.Node>();
      this.voronoiTree.GetNodesWithTag(WorldGenTags.NearSurface, nodes1);
      VoronoiTree.Node.SplitCommand cmd = new VoronoiTree.Node.SplitCommand();
      cmd.dontCopyTags = tagSet1;
      cmd.moveTags = tagSet2;
      cmd.SplitFunction = new System.Action<Tree, VoronoiTree.Node.SplitCommand>(this.SplitFunction);
      for (int index = 0; index < nodes1.Count; ++index)
      {
        VoronoiTree.Node node = nodes1[index];
        if ((double) node.site.poly.Area() > (double) floatSetting)
          node.Split(cmd);
      }
      List<VoronoiTree.Node> nodes2 = new List<VoronoiTree.Node>();
      this.voronoiTree.GetNodesWithTag(WorldGenTags.NearDepths, nodes2);
      for (int index = 0; index < nodes2.Count; ++index)
      {
        VoronoiTree.Node node = nodes2[index];
        if ((double) node.site.poly.Area() > (double) floatSetting)
          node.Split(cmd);
      }
      VoronoiTree.Node.maxDepth = this.voronoiTree.MaxDepth(0);
      this.voronoiTree.ForceLowestToLeaf();
      List<VoronoiTree.Node> nodes3 = new List<VoronoiTree.Node>();
      this.voronoiTree.GetNodesWithTag(WorldGenTags.AtSurface, nodes3);
      for (int index = 0; index < nodes3.Count; ++index)
      {
        VoronoiTree.Node node = nodes3[index];
        node.tags.Remove(WorldGenTags.Geode);
        node.tags.Remove(WorldGenTags.Feature);
      }
    }

    private void SplitFunction(Tree tree, VoronoiTree.Node.SplitCommand cmd)
    {
      Node node1 = !tree.tags.Contains(WorldGenTags.Overworld) ? this.worldGen.WorldLayout.localGraph.FindNodeByID(tree.site.id) : this.worldGen.WorldLayout.overworldGraph.FindNodeByID(tree.site.id);
      Debug.Assert(node1 != null, (object) "Null terrain node WTF");
      TagSet originalTags = new TagSet(tree.tags);
      if (cmd.dontCopyTags != null)
      {
        originalTags.Remove(cmd.dontCopyTags);
        if (cmd.moveTags != null)
          originalTags.Remove(cmd.moveTags);
      }
      TagSet tagSet = new TagSet();
      if (cmd.moveTags != null)
      {
        for (int index = 0; index < cmd.moveTags.Count; ++index)
        {
          Tag moveTag = cmd.moveTags[index];
          if (tree.tags.Contains(moveTag))
          {
            tree.tags.Remove(moveTag);
            tagSet.Add(moveTag);
          }
        }
      }
      List<Vector2> avoidPoints = new List<Vector2>();
      if (originalTags.Contains(WorldGenTags.Feature))
      {
        Node node2 = this.worldGen.WorldLayout.localGraph.AddNode(node1.type);
        node2.SetPosition(!originalTags.Contains(WorldGenTags.CenteralFeature) ? tree.site.position : tree.site.poly.Centroid());
        VoronoiTree.Node node3 = tree.AddSite(new Diagram.Site((uint) node2.node.Id, node2.position, 1f), VoronoiTree.Node.NodeType.Leaf);
        if (originalTags != null && originalTags.Count != 0)
          node3.SetTags(originalTags);
        originalTags.Remove(WorldGenTags.Feature);
        originalTags.Remove(new Tag(node1.type));
        avoidPoints.Add(node2.position);
      }
      float floatSetting1 = this.worldGen.Settings.GetFloatSetting("SplitDensityMin");
      float floatSetting2 = this.worldGen.Settings.GetFloatSetting("SplitDensityMax");
      if (tree.tags.Contains(WorldGenTags.UltraHighDensitySplit))
      {
        floatSetting1 = this.worldGen.Settings.GetFloatSetting("UltraHighSplitDensityMin");
        floatSetting2 = this.worldGen.Settings.GetFloatSetting("UltraHighSplitDensityMax");
      }
      else if (tree.tags.Contains(WorldGenTags.VeryHighDensitySplit))
      {
        floatSetting1 = this.worldGen.Settings.GetFloatSetting("VeryHighSplitDensityMin");
        floatSetting2 = this.worldGen.Settings.GetFloatSetting("VeryHighSplitDensityMax");
      }
      else if (tree.tags.Contains(WorldGenTags.HighDensitySplit))
      {
        floatSetting1 = this.worldGen.Settings.GetFloatSetting("HighSplitDensityMin");
        floatSetting2 = this.worldGen.Settings.GetFloatSetting("HighSplitDensityMax");
      }
      else if (tree.tags.Contains(WorldGenTags.MediumDensitySplit))
      {
        floatSetting1 = this.worldGen.Settings.GetFloatSetting("MediumSplitDensityMin");
        floatSetting2 = this.worldGen.Settings.GetFloatSetting("MediumSplitDensityMax");
      }
      float density = tree.myRandom.RandomRange(floatSetting1, floatSetting2);
      List<Vector2> points = this.GetPoints(tree.site.id.ToString(), tree.log, cmd.minChildCount, tree.site.poly, density, 1f, avoidPoints, PointGenerator.SampleBehaviour.PoissonDisk, true, tree.myRandom, true, true);
      if (points.Count < cmd.minChildCount)
      {
        if (this.worldGen.isRunningDebugGen)
          Debug.Assert((points.Count >= cmd.minChildCount ? 1 : 0) != 0, (object) ("Error not enough points [" + (object) cmd.minChildCount + "] for tree split " + tree.site.id.ToString()));
        if (points.Count == 0)
          return;
      }
      for (int index = 0; index < points.Count; ++index)
      {
        Node node2 = this.worldGen.WorldLayout.localGraph.AddNode(cmd.typeOverride != null ? cmd.typeOverride(points[index]) : node1.type);
        node2.SetPosition(points[index]);
        VoronoiTree.Node node3 = tree.AddSite(new Diagram.Site((uint) node2.node.Id, node2.position, 1f), VoronoiTree.Node.NodeType.Leaf);
        if (originalTags != null && originalTags.Count != 0)
          node3.SetTags(originalTags);
      }
      for (int index = 0; index < tagSet.Count; ++index)
      {
        Tag tag = tagSet[index];
        tree.GetChild(tree.myRandom.RandomRange(0, tree.ChildCount())).AddTag(tag);
      }
    }

    private void SprinklePOI(List<TemplateContainer> poi)
    {
      List<VoronoiTree.Node> leafNodesWithTag = this.GetLeafNodesWithTag(WorldGenTags.StartFar);
      leafNodesWithTag.RemoveAll((Predicate<VoronoiTree.Node>) (vn =>
      {
        if (!vn.tags.Contains(WorldGenTags.AtDepths))
          return vn.tags.Contains(WorldGenTags.AtSurface);
        return true;
      }));
      leafNodesWithTag.RemoveAll((Predicate<VoronoiTree.Node>) (vn => vn.tags.Contains(WorldGenTags.AtEdge)));
      leafNodesWithTag.RemoveAll((Predicate<VoronoiTree.Node>) (vn => vn.tags.Contains(WorldGenTags.EdgeOfVoid)));
      for (int index = 0; index < poi.Count; ++index)
      {
        VoronoiTree.Node random1 = leafNodesWithTag.GetRandom<VoronoiTree.Node>(this.myRandom);
        random1.AddTag(new Tag(poi[index].name));
        random1.AddTag(WorldGenTags.POI);
        leafNodesWithTag.Remove(random1);
        VoronoiTree.Node random2 = leafNodesWithTag.GetRandom<VoronoiTree.Node>(this.myRandom);
        random2.AddTag(new Tag(poi[index].name));
        random2.AddTag(WorldGenTags.POI);
        leafNodesWithTag.Remove(random2);
        VoronoiTree.Node random3 = leafNodesWithTag.GetRandom<VoronoiTree.Node>(this.myRandom);
        random3.AddTag(new Tag(poi[index].name));
        random3.AddTag(WorldGenTags.POI);
        leafNodesWithTag.Remove(random3);
      }
    }

    private void TagTopAndBottomSites(Tag topTag, Tag bottomTag)
    {
      List<Diagram.Site> intersectingSites1 = new List<Diagram.Site>();
      List<Diagram.Site> intersectingSites2 = new List<Diagram.Site>();
      this.voronoiTree.GetIntersectingLeafSites(this.topEdge, intersectingSites1);
      this.voronoiTree.GetIntersectingLeafSites(this.bottomEdge, intersectingSites2);
      for (int index = 0; index < intersectingSites1.Count; ++index)
        this.voronoiTree.GetNodeForSite(intersectingSites1[index]).AddTag(topTag);
      for (int index = 0; index < intersectingSites2.Count; ++index)
        this.voronoiTree.GetNodeForSite(intersectingSites2[index]).AddTag(bottomTag);
    }

    private void TagEdgeSites(Tag leftTag, Tag rightTag)
    {
      List<Diagram.Site> intersectingSites1 = new List<Diagram.Site>();
      List<Diagram.Site> intersectingSites2 = new List<Diagram.Site>();
      this.voronoiTree.GetIntersectingLeafSites(this.leftEdge, intersectingSites1);
      this.voronoiTree.GetIntersectingLeafSites(this.rightEdge, intersectingSites2);
      for (int index = 0; index < intersectingSites1.Count; ++index)
        this.voronoiTree.GetNodeForSite(intersectingSites1[index]).AddTag(leftTag);
      for (int index = 0; index < intersectingSites2.Count; ++index)
        this.voronoiTree.GetNodeForSite(intersectingSites2[index]).AddTag(rightTag);
    }

    private bool StartAreaTooLarge(VoronoiTree.Node node)
    {
      if (node.tags.Contains(WorldGenTags.AtStart))
        return (double) node.site.poly.Area() > 2000.0;
      return false;
    }

    private void SplitLargeStartingSites()
    {
      TagSet tagSet1 = new TagSet();
      tagSet1.Add(WorldGenTags.Overworld);
      TagSet tagSet2 = new TagSet((IEnumerable<string>) this.worldGen.Settings.GetDefaultMoveTags());
      List<VoronoiTree.Node> nodes = new List<VoronoiTree.Node>();
      this.voronoiTree.GetLeafNodes(nodes, new Tree.LeafNodeTest(this.StartAreaTooLarge));
      VoronoiTree.Node.SplitCommand cmd = new VoronoiTree.Node.SplitCommand();
      cmd.dontCopyTags = tagSet1;
      cmd.moveTags = tagSet2;
      cmd.SplitFunction = new System.Action<Tree, VoronoiTree.Node.SplitCommand>(this.SplitFunction);
      while (nodes.Count > 0)
      {
        foreach (VoronoiTree.Node node in nodes)
        {
          node.AddTag(WorldGenTags.DEBUG_SplitLargeStartingSites);
          node.Split(cmd);
        }
        nodes.Clear();
        this.voronoiTree.GetLeafNodes(nodes, new Tree.LeafNodeTest(this.StartAreaTooLarge));
      }
    }

    private void PropagateStartTag()
    {
      foreach (VoronoiTree.Node startNode in this.GetStartNodes())
      {
        startNode.AddTagToNeighbors(WorldGenTags.NearStartLocation);
        startNode.AddTag(WorldGenTags.IgnoreCaveOverride);
      }
    }

    public List<VoronoiTree.Node> GetStartNodes()
    {
      return this.GetLeafNodesWithTag(WorldGenTags.StartLocation);
    }

    public List<VoronoiTree.Node> GetLeafNodesWithTag(Tag tag)
    {
      List<VoronoiTree.Node> nodes = new List<VoronoiTree.Node>();
      this.voronoiTree.GetLeafNodes(nodes, (Tree.LeafNodeTest) (node =>
      {
        if (node.tags != null)
          return node.tags.Contains(tag);
        return false;
      }));
      return nodes;
    }

    public List<Node> GetTerrainNodesForTag(Tag tag)
    {
      List<Node> nodeList = new List<Node>();
      foreach (VoronoiTree.Node node in this.GetLeafNodesWithTag(tag))
      {
        Node nodeById = this.localGraph.FindNodeByID(node.site.id);
        if (nodeById != null)
          nodeList.Add(nodeById);
      }
      return nodeList;
    }

    private Node FindFirstNode(string nodeType)
    {
      return this.localGraph.FindNode((Predicate<Node>) (node => node.type == nodeType));
    }

    private Node FindFirstNodeWithTag(Tag tag)
    {
      return this.localGraph.FindNode((Predicate<Node>) (node =>
      {
        if (node.tags != null)
          return node.tags.Contains(tag);
        return false;
      }));
    }

    public Vector2I GetStartLocation()
    {
      Node node1 = this.FindFirstNodeWithTag(WorldGenTags.StartLocation);
      if (node1 == null)
      {
        List<VoronoiTree.Node> nodes = this.GetStartNodes();
        if (nodes == null || nodes.Count == 0)
        {
          Debug.LogWarning((object) "Couldnt find start node");
          return new Vector2I(this.mapWidth / 2, this.mapHeight / 2);
        }
        node1 = this.localGraph.FindNode((Predicate<Node>) (node => (int) (uint) node.node.Id == (int) nodes[0].site.id));
        node1.tags.Add(WorldGenTags.StartLocation);
      }
      if (node1 != null)
        return new Vector2I((int) node1.position.x, (int) node1.position.y);
      Debug.LogWarning((object) "Couldnt find start node");
      return new Vector2I(this.mapWidth / 2, this.mapHeight / 2);
    }

    public List<River> GetRivers()
    {
      List<River> riverList = new List<River>();
      foreach (Satsuma.Arc arc in this.localGraph.baseGraph.Arcs(ArcFilter.All))
      {
        Satsuma.Node n0 = this.localGraph.baseGraph.U(arc);
        Satsuma.Node n1 = this.localGraph.baseGraph.V(arc);
        Node tn0 = this.localGraph.FindNode((Predicate<Node>) (n => n.node == n0));
        Node tn1 = this.localGraph.FindNode((Predicate<Node>) (n => n.node == n1));
        if (tn0 != null && tn1 != null && !(tn0.type != tn1.type) && (tn0.type.Contains(WorldGenTags.River.Name) && riverList.Find((Predicate<River>) (r =>
        {
          if (r.SinkPosition() == tn0.position)
            return r.SourcePosition() == tn1.position;
          return false;
        })) == null))
        {
          River river;
          if (SettingsCache.rivers.ContainsKey(tn0.type))
          {
            river = new River(SettingsCache.rivers[tn0.type], false);
            river.AddSection(tn0, tn1);
          }
          else
            river = new River(tn0, tn1, SimHashes.Water.ToString(), "Granite", 373f, 2000f, 1000f, 100f, 1.5f, 1.5f);
          river.widthCenter = this.myRandom.RandomRange(1f, river.widthCenter + 0.5f);
          river.widthBorder = this.myRandom.RandomRange(1f, river.widthBorder + 0.5f);
          river.Stagger(this.myRandom, (float) this.myRandom.RandomRange(8, 20), (float) this.myRandom.RandomRange(1, 3));
          riverList.Add(river);
        }
      }
      return riverList;
    }

    private List<Diagram.Site> GetIntersectingSites(
      VoronoiTree.Node intersectingSiteSource,
      Tree sitesSource)
    {
      List<Diagram.Site> siteList = new List<Diagram.Site>();
      List<Diagram.Site> intersectingSites = new List<Diagram.Site>();
      for (int index = 1; index < intersectingSiteSource.site.poly.Vertices.Count - 1; ++index)
      {
        LineSegment edge = new LineSegment(new Vector2?(intersectingSiteSource.site.poly.Vertices[index - 1]), new Vector2?(intersectingSiteSource.site.poly.Vertices[index]));
        sitesSource.GetIntersectingLeafSites(edge, intersectingSites);
      }
      LineSegment edge1 = new LineSegment(new Vector2?(intersectingSiteSource.site.poly.Vertices[intersectingSiteSource.site.poly.Vertices.Count - 1]), new Vector2?(intersectingSiteSource.site.poly.Vertices[0]));
      sitesSource.GetIntersectingLeafSites(edge1, intersectingSites);
      return intersectingSites;
    }

    public void GetEdgeOfMapSites(
      Tree vt,
      List<Diagram.Site> topSites,
      List<Diagram.Site> bottomSites,
      List<Diagram.Site> leftSites,
      List<Diagram.Site> rightSites)
    {
      vt.GetIntersectingLeafSites(this.topEdge, topSites);
      vt.GetIntersectingLeafSites(this.bottomEdge, bottomSites);
      vt.GetIntersectingLeafSites(this.leftEdge, leftSites);
      vt.GetIntersectingLeafSites(this.rightEdge, rightSites);
    }

    [OnSerializing]
    internal void OnSerializingMethod()
    {
      try
      {
        this.extra = new WorldLayout.ExtraIO();
        if (this.voronoiTree == null)
          return;
        this.extra.internals.Add(this.voronoiTree);
        this.voronoiTree.GetInternalNodes(this.extra.internals);
        List<VoronoiTree.Node> nodes = new List<VoronoiTree.Node>();
        this.voronoiTree.GetLeafNodes(nodes, (Tree.LeafNodeTest) null);
        foreach (Leaf leaf in nodes)
        {
          Leaf ln = leaf;
          if (ln != null)
          {
            this.extra.leafInternalParent.Add(new KeyValuePair<int, int>(this.extra.leafs.Count, this.extra.internals.FindIndex(0, (Predicate<Tree>) (n => n == ln.parent))));
            this.extra.leafs.Add(ln);
          }
        }
        for (int key = 0; key < this.extra.internals.Count; ++key)
        {
          Tree vt = this.extra.internals[key];
          if (vt.parent != null)
          {
            int index = this.extra.internals.FindIndex(0, (Predicate<Tree>) (n => n == vt.parent));
            if (index >= 0)
              this.extra.internalInternalParent.Add(new KeyValuePair<int, int>(key, index));
          }
        }
      }
      catch (Exception ex)
      {
        WorldGenLogger.LogException(ex.Message, ex.StackTrace);
        Debug.Log((object) ("Error deserialising " + ex.Message));
      }
    }

    [OnSerialized]
    internal void OnSerializedMethod()
    {
      this.extra = (WorldLayout.ExtraIO) null;
    }

    [OnDeserializing]
    internal void OnDeserializingMethod()
    {
      this.extra = new WorldLayout.ExtraIO();
    }

    [OnDeserialized]
    internal void OnDeserializedMethod()
    {
      try
      {
        this.voronoiTree = this.extra.internals[0];
        for (int index = 0; index < this.extra.internalInternalParent.Count; ++index)
        {
          KeyValuePair<int, int> keyValuePair = this.extra.internalInternalParent[index];
          Tree tree = this.extra.internals[keyValuePair.Key];
          this.extra.internals[keyValuePair.Value].AddChild((VoronoiTree.Node) tree);
        }
        for (int index = 0; index < this.extra.leafInternalParent.Count; ++index)
        {
          KeyValuePair<int, int> keyValuePair = this.extra.leafInternalParent[index];
          VoronoiTree.Node leaf = (VoronoiTree.Node) this.extra.leafs[keyValuePair.Key];
          this.extra.internals[keyValuePair.Value].AddChild(leaf);
        }
      }
      catch (Exception ex)
      {
        WorldGenLogger.LogException(ex.Message, ex.StackTrace);
        Debug.Log((object) ("Error deserialising " + ex.Message));
      }
      this.extra = (WorldLayout.ExtraIO) null;
    }

    [System.Flags]
    public enum DebugFlags
    {
      LocalGraph = 1,
      OverworldGraph = 2,
      VoronoiTree = 4,
      PowerDiagram = 8,
    }

    [SerializationConfig(MemberSerialization.OptOut)]
    private class ExtraIO
    {
      public List<Leaf> leafs = new List<Leaf>();
      public List<Tree> internals = new List<Tree>();
      public List<KeyValuePair<int, int>> leafInternalParent = new List<KeyValuePair<int, int>>();
      public List<KeyValuePair<int, int>> internalInternalParent = new List<KeyValuePair<int, int>>();

      [OnDeserializing]
      internal void OnDeserializingMethod()
      {
        this.leafs = new List<Leaf>();
        this.internals = new List<Tree>();
        this.leafInternalParent = new List<KeyValuePair<int, int>>();
        this.internalInternalParent = new List<KeyValuePair<int, int>>();
      }
    }
  }
}
