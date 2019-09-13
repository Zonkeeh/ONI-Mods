// Decompiled with JetBrains decompiler
// Type: BuildingDef
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using Klei.AI;
using ProcGen;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

[Serializable]
public class BuildingDef : Def
{
  public static CellOffset[] ConstructionOffsetFilter_OneDown = new CellOffset[1]
  {
    new CellOffset(0, -1)
  };
  private static Dictionary<CellOffset, CellOffset[]> placementOffsetsCache = new Dictionary<CellOffset, CellOffset[]>();
  public float ThermalConductivity = 1f;
  public bool Floodable = true;
  public bool Disinfectable = true;
  public bool Entombable = true;
  public bool Replaceable = true;
  public bool Overheatable = true;
  public bool Repairable = true;
  public float OverheatTemperature = 348.15f;
  public float FatalHot = 533.15f;
  public bool UseStructureTemperature = true;
  public Action HotKey = Action.NumActions;
  public CellOffset attachablePosition = new CellOffset(0, 0);
  [HashedEnum]
  [NonSerialized]
  public HashedString ViewMode = OverlayModes.None.ID;
  public ObjectLayer ObjectLayer = ObjectLayer.Building;
  public ObjectLayer TileLayer = ObjectLayer.NumLayers;
  public ObjectLayer ReplacementLayer = ObjectLayer.NumLayers;
  public string AudioCategory = "Metal";
  public string AudioSize = "medium";
  public float BaseTimeUntilRepair = 600f;
  public bool ShowInBuildMenu = true;
  public CellOffset UtilityInputOffset = new CellOffset(0, 1);
  public CellOffset UtilityOutputOffset = new CellOffset(1, 0);
  public Grid.SceneLayer SceneLayer = Grid.SceneLayer.Building;
  public Grid.SceneLayer ForegroundLayer = Grid.SceneLayer.BuildingFront;
  public string RequiredAttribute = string.Empty;
  public string DefaultAnimState = "off";
  public List<Klei.AI.Attribute> attributes = new List<Klei.AI.Attribute>();
  public List<AttributeModifier> attributeModifiers = new List<AttributeModifier>();
  public float EnergyConsumptionWhenActive;
  public float GeneratorWattageRating;
  public float GeneratorBaseCapacity;
  public float MassForTemperatureModification;
  public float ExhaustKilowattsWhenActive;
  public float SelfHeatKilowattsWhenActive;
  public float BaseMeltingPoint;
  public float ConstructionTime;
  public float WorkTime;
  public int WidthInCells;
  public int HeightInCells;
  public int HitPoints;
  public bool RequiresPowerInput;
  public bool RequiresPowerOutput;
  public bool UseWhitePowerOutputConnectorColour;
  public CellOffset ElectricalArrowOffset;
  public ConduitType InputConduitType;
  public ConduitType OutputConduitType;
  public bool ModifiesTemperature;
  public bool Invincible;
  public bool Breakable;
  public bool ContinuouslyCheckFoundation;
  public bool IsFoundation;
  public bool DragBuild;
  public bool CanMove;
  public List<Tag> ReplacementTags;
  public List<ObjectLayer> ReplacementCandidateLayers;
  public List<ObjectLayer> EquivalentReplacementLayers;
  public BuildLocationRule BuildLocationRule;
  public Vector3 placementPivot;
  public string DiseaseCellVisName;
  public string[] MaterialCategory;
  public float[] Mass;
  public bool Upgradeable;
  public PermittedRotations PermittedRotations;
  public bool Deprecated;
  public CellOffset PowerInputOffset;
  public CellOffset PowerOutputOffset;
  public int RequiredAttributeLevel;
  public List<Descriptor> EffectDescription;
  public float MassTier;
  public float HeatTier;
  public float ConstructionTimeTier;
  public string PrimaryUse;
  public string SecondaryUse;
  public string PrimarySideEffect;
  public string SecondarySideEffect;
  public Recipe CraftRecipe;
  public Sprite UISprite;
  public bool isKAnimTile;
  public bool isUtility;
  public bool isSolidTile;
  public KAnimFile[] AnimFiles;
  public bool BlockTileIsTransparent;
  public TextureAtlas BlockTileAtlas;
  public TextureAtlas BlockTilePlaceAtlas;
  public TextureAtlas BlockTileShineAtlas;
  public Material BlockTileMaterial;
  public BlockTileDecorInfo DecorBlockTileInfo;
  public BlockTileDecorInfo DecorPlaceBlockTileInfo;
  public Tag AttachmentSlotTag;
  public bool PreventIdleTraversalPastBuilding;
  public GameObject BuildingComplete;
  public GameObject BuildingPreview;
  public GameObject BuildingUnderConstruction;
  public CellOffset[] PlacementOffsets;
  public CellOffset[] ConstructionOffsetFilter;
  public float BaseDecor;
  public float BaseDecorRadius;
  public int BaseNoisePollution;
  public int BaseNoisePollutionRadius;
  public BuildingDef[] Enables;

  public override string Name
  {
    get
    {
      return (string) Strings.Get("STRINGS.BUILDINGS.PREFABS." + this.PrefabID.ToUpper() + ".NAME");
    }
  }

  public string Desc
  {
    get
    {
      return (string) Strings.Get("STRINGS.BUILDINGS.PREFABS." + this.PrefabID.ToUpper() + ".DESC");
    }
  }

  public string Flavor
  {
    get
    {
      return "\"" + (string) Strings.Get("STRINGS.BUILDINGS.PREFABS." + this.PrefabID.ToUpper() + ".FLAVOR") + "\"";
    }
  }

  public string Effect
  {
    get
    {
      return (string) Strings.Get("STRINGS.BUILDINGS.PREFABS." + this.PrefabID.ToUpper() + ".EFFECT");
    }
  }

  public bool IsTilePiece
  {
    get
    {
      return this.TileLayer != ObjectLayer.NumLayers;
    }
  }

  public bool CanReplace(GameObject go)
  {
    if (this.ReplacementTags == null)
      return false;
    foreach (Tag replacementTag in this.ReplacementTags)
    {
      if (go.GetComponent<KPrefabID>().HasTag(replacementTag))
        return true;
    }
    return false;
  }

  public bool IsReplacementLayerOccupied(int cell)
  {
    if ((UnityEngine.Object) Grid.Objects[cell, (int) this.ReplacementLayer] != (UnityEngine.Object) null)
      return true;
    if (this.EquivalentReplacementLayers != null)
    {
      foreach (ObjectLayer replacementLayer in this.EquivalentReplacementLayers)
      {
        if ((UnityEngine.Object) Grid.Objects[cell, (int) replacementLayer] != (UnityEngine.Object) null)
          return true;
      }
    }
    return false;
  }

  public GameObject GetReplacementCandidate(int cell)
  {
    if (this.ReplacementCandidateLayers != null)
    {
      foreach (ObjectLayer replacementCandidateLayer in this.ReplacementCandidateLayers)
      {
        if (Grid.ObjectLayers[(int) replacementCandidateLayer].ContainsKey(cell) && (UnityEngine.Object) Grid.ObjectLayers[(int) replacementCandidateLayer][cell].GetComponent<global::BuildingComplete>() != (UnityEngine.Object) null)
          return Grid.ObjectLayers[(int) replacementCandidateLayer][cell];
      }
    }
    else if (Grid.ObjectLayers[(int) this.TileLayer].ContainsKey(cell))
      return Grid.ObjectLayers[(int) this.TileLayer][cell];
    return (GameObject) null;
  }

  public GameObject Create(
    Vector3 pos,
    Storage resource_storage,
    IList<Tag> selected_elements,
    Recipe recipe,
    float temperature,
    GameObject obj)
  {
    SimUtil.DiseaseInfo a = SimUtil.DiseaseInfo.Invalid;
    if ((UnityEngine.Object) resource_storage != (UnityEngine.Object) null)
    {
      Recipe.Ingredient[] allIngredients = recipe.GetAllIngredients(selected_elements);
      if (allIngredients != null)
      {
        foreach (Recipe.Ingredient ingredient in allIngredients)
        {
          SimUtil.DiseaseInfo disease_info;
          float aggregate_temperature;
          resource_storage.ConsumeAndGetDisease(ingredient.tag, ingredient.amount, out disease_info, out aggregate_temperature);
          a = SimUtil.CalculateFinalDiseaseInfo(a, disease_info);
        }
      }
    }
    GameObject gameObject = GameUtil.KInstantiate(obj, pos, this.SceneLayer, (string) null, 0);
    Element element = ElementLoader.GetElement(selected_elements[0]);
    Debug.Assert(element != null);
    PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
    component.ElementID = element.id;
    component.Temperature = temperature;
    component.AddDisease(a.idx, a.count, "BuildingDef.Create");
    gameObject.name = obj.name;
    gameObject.SetActive(true);
    return gameObject;
  }

  public List<Tag> DefaultElements()
  {
    List<Tag> tagList = new List<Tag>();
    foreach (string str in this.MaterialCategory)
    {
      foreach (Element element in ElementLoader.elements)
      {
        if (element.IsSolid && (element.tag.Name == str || element.HasTag((Tag) str)))
        {
          tagList.Add(element.tag);
          break;
        }
      }
    }
    return tagList;
  }

  public GameObject Build(
    int cell,
    Orientation orientation,
    Storage resource_storage,
    IList<Tag> selected_elements,
    float temperature,
    bool playsound = true,
    float timeBuilt = -1f)
  {
    GameObject go = this.Create(Grid.CellToPosCBC(cell, this.SceneLayer), resource_storage, selected_elements, this.CraftRecipe, temperature, this.BuildingComplete);
    Rotatable component1 = go.GetComponent<Rotatable>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
      component1.SetOrientation(orientation);
    this.MarkArea(cell, orientation, this.ObjectLayer, go);
    if (this.IsTilePiece)
    {
      this.MarkArea(cell, orientation, this.TileLayer, go);
      this.RunOnArea(cell, orientation, (System.Action<int>) (c => TileVisualizer.RefreshCell(c, this.TileLayer, this.ReplacementLayer)));
    }
    string sound = GlobalAssets.GetSound("Finish_Building_" + this.AudioSize, false);
    if (playsound && sound != null)
      KMonoBehaviour.PlaySound3DAtLocation(sound, go.transform.GetPosition());
    Deconstructable component2 = go.GetComponent<Deconstructable>();
    if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
    {
      component2.constructionElements = new Tag[selected_elements.Count];
      for (int index = 0; index < selected_elements.Count; ++index)
        component2.constructionElements[index] = selected_elements[index];
    }
    global::BuildingComplete component3 = go.GetComponent<global::BuildingComplete>();
    if ((bool) ((UnityEngine.Object) component3))
      component3.SetCreationTime(timeBuilt);
    Game.Instance.Trigger(-1661515756, (object) go);
    go.Trigger(-1661515756, (object) go);
    return go;
  }

  public GameObject TryPlace(
    GameObject src_go,
    Vector3 pos,
    Orientation orientation,
    IList<Tag> selected_elements,
    int layer = 0)
  {
    GameObject gameObject = (GameObject) null;
    string fail_reason;
    if (this.IsValidPlaceLocation(src_go, pos, orientation, false, out fail_reason))
      gameObject = this.Instantiate(pos, orientation, selected_elements, layer);
    return gameObject;
  }

  public GameObject TryReplaceTile(
    GameObject src_go,
    Vector3 pos,
    Orientation orientation,
    IList<Tag> selected_elements,
    int layer = 0)
  {
    GameObject gameObject = (GameObject) null;
    string fail_reason;
    if (this.IsValidPlaceLocation(src_go, pos, orientation, true, out fail_reason))
    {
      Constructable component = this.BuildingUnderConstruction.GetComponent<Constructable>();
      component.IsReplacementTile = true;
      gameObject = this.Instantiate(pos, orientation, selected_elements, layer);
      component.IsReplacementTile = false;
    }
    return gameObject;
  }

  public GameObject Instantiate(
    Vector3 pos,
    Orientation orientation,
    IList<Tag> selected_elements,
    int layer = 0)
  {
    float num = -0.15f;
    pos.z += num;
    GameObject gameObject = GameUtil.KInstantiate(this.BuildingUnderConstruction, pos, Grid.SceneLayer.Front, (string) null, layer);
    Element element = ElementLoader.GetElement(selected_elements[0]);
    Debug.Assert(element != null, (object) "Missing primary element for BuildingDef");
    gameObject.GetComponent<PrimaryElement>().ElementID = element.id;
    gameObject.GetComponent<Constructable>().SelectedElementsTags = selected_elements;
    gameObject.SetActive(true);
    return gameObject;
  }

  private bool IsAreaClear(
    GameObject source_go,
    int cell,
    Orientation orientation,
    ObjectLayer layer,
    ObjectLayer tile_layer,
    bool replace_tile,
    out string fail_reason)
  {
    bool flag1 = true;
    fail_reason = (string) null;
    for (int index1 = 0; index1 < this.PlacementOffsets.Length; ++index1)
    {
      CellOffset rotatedCellOffset1 = Rotatable.GetRotatedCellOffset(this.PlacementOffsets[index1], orientation);
      if (!Grid.IsCellOffsetValid(cell, rotatedCellOffset1))
      {
        fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_INVALID_CELL;
        flag1 = false;
        break;
      }
      int cell1 = Grid.OffsetCell(cell, rotatedCellOffset1);
      if (!Grid.IsValidBuildingCell(cell1))
      {
        fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_INVALID_CELL;
        flag1 = false;
        break;
      }
      if (Grid.Element[cell1].id == SimHashes.Unobtanium)
      {
        fail_reason = (string) null;
        flag1 = false;
        break;
      }
      bool flag2 = this.BuildLocationRule == BuildLocationRule.LogicBridge || this.BuildLocationRule == BuildLocationRule.Conduit || this.BuildLocationRule == BuildLocationRule.WireBridge;
      if (!replace_tile && !flag2)
      {
        GameObject gameObject = Grid.Objects[cell1, (int) layer];
        if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
        {
          if ((UnityEngine.Object) gameObject.GetComponent<Wire>() == (UnityEngine.Object) null || (UnityEngine.Object) this.BuildingComplete.GetComponent<Wire>() == (UnityEngine.Object) null)
          {
            fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_OCCUPIED;
            flag1 = false;
            break;
          }
          break;
        }
        if (tile_layer != ObjectLayer.NumLayers && (UnityEngine.Object) Grid.Objects[cell1, (int) tile_layer] != (UnityEngine.Object) null && (UnityEngine.Object) Grid.Objects[cell1, (int) tile_layer].GetComponent<global::BuildingPreview>() == (UnityEngine.Object) null)
        {
          fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_OCCUPIED;
          flag1 = false;
          break;
        }
      }
      if (layer == ObjectLayer.Building && this.AttachmentSlotTag != GameTags.Rocket && (UnityEngine.Object) Grid.Objects[cell1, 38] != (UnityEngine.Object) null)
      {
        if ((UnityEngine.Object) this.BuildingComplete.GetComponent<Wire>() == (UnityEngine.Object) null)
        {
          fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_OCCUPIED;
          flag1 = false;
          break;
        }
        break;
      }
      if (layer == ObjectLayer.Gantry)
      {
        bool flag3 = false;
        for (int index2 = 0; index2 < Gantry.TileOffsets.Length; ++index2)
        {
          CellOffset rotatedCellOffset2 = Rotatable.GetRotatedCellOffset(Gantry.TileOffsets[index2], orientation);
          flag3 |= rotatedCellOffset2 == rotatedCellOffset1;
        }
        if (flag3 && !this.IsValidTileLocation(source_go, cell1, ref fail_reason))
        {
          flag1 = false;
          break;
        }
        GameObject gameObject = Grid.Objects[cell1, 1];
        if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null && (UnityEngine.Object) gameObject.GetComponent<global::BuildingPreview>() == (UnityEngine.Object) null)
        {
          Building component = gameObject.GetComponent<Building>();
          if (flag3 || (UnityEngine.Object) component == (UnityEngine.Object) null || component.Def.AttachmentSlotTag != GameTags.Rocket)
          {
            fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_OCCUPIED;
            flag1 = false;
            break;
          }
        }
      }
      if (this.BuildLocationRule == BuildLocationRule.Tile)
      {
        if (!this.IsValidTileLocation(source_go, cell1, ref fail_reason))
        {
          flag1 = false;
          break;
        }
      }
      else if (this.BuildLocationRule == BuildLocationRule.OnFloorOverSpace && World.Instance.zoneRenderData.GetSubWorldZoneType(cell1) != SubWorld.ZoneType.Space)
      {
        fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_SPACE;
        flag1 = false;
        break;
      }
    }
    if (!flag1)
      return false;
    switch (this.BuildLocationRule)
    {
      case BuildLocationRule.NotInTiles:
        GameObject gameObject1 = Grid.Objects[cell, 9];
        if ((UnityEngine.Object) gameObject1 != (UnityEngine.Object) null && (UnityEngine.Object) gameObject1 != (UnityEngine.Object) source_go)
          flag1 = false;
        else if (Grid.HasDoor[cell])
        {
          flag1 = false;
        }
        else
        {
          GameObject gameObject2 = Grid.Objects[cell, (int) this.ObjectLayer];
          if ((UnityEngine.Object) gameObject2 != (UnityEngine.Object) null)
          {
            if (this.ReplacementLayer == ObjectLayer.NumLayers)
            {
              if ((UnityEngine.Object) gameObject2 != (UnityEngine.Object) source_go)
                flag1 = false;
            }
            else
            {
              Building component = gameObject2.GetComponent<Building>();
              if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.Def.ReplacementLayer != this.ReplacementLayer)
                flag1 = false;
            }
          }
        }
        if (!flag1)
        {
          fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_NOT_IN_TILES;
          break;
        }
        break;
      case BuildLocationRule.WireBridge:
        return this.IsValidWireBridgeLocation(source_go, cell, orientation, out fail_reason);
      case BuildLocationRule.HighWattBridgeTile:
        flag1 = this.IsValidTileLocation(source_go, cell, ref fail_reason) && this.IsValidHighWattBridgeLocation(source_go, cell, orientation, out fail_reason);
        break;
      case BuildLocationRule.BuildingAttachPoint:
        flag1 = false;
        for (int index1 = 0; index1 < Components.BuildingAttachPoints.Count && !flag1; ++index1)
        {
          for (int index2 = 0; index2 < Components.BuildingAttachPoints[index1].points.Length; ++index2)
          {
            if (Components.BuildingAttachPoints[index1].AcceptsAttachment(this.AttachmentSlotTag, Grid.OffsetCell(cell, this.attachablePosition)))
            {
              flag1 = true;
              break;
            }
          }
        }
        if (!flag1)
        {
          fail_reason = string.Format((string) UI.TOOLTIPS.HELP_BUILDLOCATION_ATTACHPOINT, (object) this.AttachmentSlotTag);
          break;
        }
        break;
    }
    return flag1 && this.ArePowerPortsInValidPositions(source_go, cell, orientation, out fail_reason) && this.AreConduitPortsInValidPositions(source_go, cell, orientation, out fail_reason) && this.AreLogicPortsInValidPositions(source_go, cell, out fail_reason);
  }

  private bool IsValidTileLocation(GameObject source_go, int cell, ref string fail_reason)
  {
    GameObject gameObject1 = Grid.Objects[cell, 27];
    if ((UnityEngine.Object) gameObject1 != (UnityEngine.Object) null && (UnityEngine.Object) gameObject1 != (UnityEngine.Object) source_go && gameObject1.GetComponent<Building>().Def.BuildLocationRule == BuildLocationRule.NotInTiles)
    {
      fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_WIRE_OBSTRUCTION;
      return false;
    }
    GameObject gameObject2 = Grid.Objects[cell, 29];
    if ((UnityEngine.Object) gameObject2 != (UnityEngine.Object) null && (UnityEngine.Object) gameObject2 != (UnityEngine.Object) source_go && gameObject2.GetComponent<Building>().Def.BuildLocationRule == BuildLocationRule.HighWattBridgeTile)
    {
      fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_WIRE_OBSTRUCTION;
      return false;
    }
    GameObject gameObject3 = Grid.Objects[cell, 2];
    if ((UnityEngine.Object) gameObject3 != (UnityEngine.Object) null && (UnityEngine.Object) gameObject3 != (UnityEngine.Object) source_go)
    {
      Building component = gameObject3.GetComponent<Building>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.Def.BuildLocationRule == BuildLocationRule.NotInTiles)
      {
        fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_BACK_WALL;
        return false;
      }
    }
    return true;
  }

  public void RunOnArea(int cell, Orientation orientation, System.Action<int> callback)
  {
    for (int index = 0; index < this.PlacementOffsets.Length; ++index)
    {
      CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.PlacementOffsets[index], orientation);
      int num = Grid.OffsetCell(cell, rotatedCellOffset);
      callback(num);
    }
  }

  public void MarkArea(int cell, Orientation orientation, ObjectLayer layer, GameObject go)
  {
    if (this.BuildLocationRule != BuildLocationRule.Conduit && this.BuildLocationRule != BuildLocationRule.WireBridge && this.BuildLocationRule != BuildLocationRule.LogicBridge)
    {
      for (int index1 = 0; index1 < this.PlacementOffsets.Length; ++index1)
      {
        CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.PlacementOffsets[index1], orientation);
        int index2 = Grid.OffsetCell(cell, rotatedCellOffset);
        Grid.Objects[index2, (int) layer] = go;
      }
    }
    if (this.InputConduitType != ConduitType.None)
    {
      CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.UtilityInputOffset, orientation);
      int index = Grid.OffsetCell(cell, rotatedCellOffset);
      ObjectLayer layerForConduitType = Grid.GetObjectLayerForConduitType(this.InputConduitType);
      this.MarkOverlappingPorts(Grid.Objects[index, (int) layerForConduitType], go);
      Grid.Objects[index, (int) layerForConduitType] = go;
    }
    if (this.OutputConduitType != ConduitType.None)
    {
      CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.UtilityOutputOffset, orientation);
      int index = Grid.OffsetCell(cell, rotatedCellOffset);
      ObjectLayer layerForConduitType = Grid.GetObjectLayerForConduitType(this.OutputConduitType);
      this.MarkOverlappingPorts(Grid.Objects[index, (int) layerForConduitType], go);
      Grid.Objects[index, (int) layerForConduitType] = go;
    }
    if (this.RequiresPowerInput)
    {
      CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.PowerInputOffset, orientation);
      int index = Grid.OffsetCell(cell, rotatedCellOffset);
      this.MarkOverlappingPorts(Grid.Objects[index, 29], go);
      Grid.Objects[index, 29] = go;
    }
    if (this.RequiresPowerOutput || (double) this.GeneratorWattageRating > 0.0)
    {
      CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.PowerOutputOffset, orientation);
      int index = Grid.OffsetCell(cell, rotatedCellOffset);
      this.MarkOverlappingPorts(Grid.Objects[index, 29], go);
      Grid.Objects[index, 29] = go;
    }
    if (this.BuildLocationRule == BuildLocationRule.WireBridge || this.BuildLocationRule == BuildLocationRule.HighWattBridgeTile)
    {
      int linked_cell1;
      int linked_cell2;
      go.GetComponent<UtilityNetworkLink>().GetCells(cell, orientation, out linked_cell1, out linked_cell2);
      this.MarkOverlappingPorts(Grid.Objects[linked_cell1, 29], go);
      this.MarkOverlappingPorts(Grid.Objects[linked_cell2, 29], go);
      Grid.Objects[linked_cell1, 29] = go;
      Grid.Objects[linked_cell2, 29] = go;
    }
    if (this.BuildLocationRule == BuildLocationRule.LogicBridge)
    {
      LogicPorts component = go.GetComponent<LogicPorts>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.inputPortInfo != null)
      {
        foreach (LogicPorts.Port port in component.inputPortInfo)
        {
          CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(port.cellOffset, orientation);
          int index = Grid.OffsetCell(cell, rotatedCellOffset);
          this.MarkOverlappingPorts(Grid.Objects[index, (int) layer], go);
          Grid.Objects[index, (int) layer] = go;
        }
      }
    }
    ISecondaryInput component1 = this.BuildingComplete.GetComponent<ISecondaryInput>();
    if (component1 != null)
    {
      ObjectLayer layerForConduitType = Grid.GetObjectLayerForConduitType(component1.GetSecondaryConduitType());
      CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(component1.GetSecondaryConduitOffset(), orientation);
      int index = Grid.OffsetCell(cell, rotatedCellOffset);
      this.MarkOverlappingPorts(Grid.Objects[index, (int) layerForConduitType], go);
      Grid.Objects[index, (int) layerForConduitType] = go;
    }
    ISecondaryOutput component2 = this.BuildingComplete.GetComponent<ISecondaryOutput>();
    if (component2 == null)
      return;
    ObjectLayer layerForConduitType1 = Grid.GetObjectLayerForConduitType(component2.GetSecondaryConduitType());
    CellOffset rotatedCellOffset1 = Rotatable.GetRotatedCellOffset(component2.GetSecondaryConduitOffset(), orientation);
    int index3 = Grid.OffsetCell(cell, rotatedCellOffset1);
    this.MarkOverlappingPorts(Grid.Objects[index3, (int) layerForConduitType1], go);
    Grid.Objects[index3, (int) layerForConduitType1] = go;
  }

  public void MarkOverlappingPorts(GameObject existing, GameObject replaced)
  {
    if ((UnityEngine.Object) existing == (UnityEngine.Object) null || !((UnityEngine.Object) existing != (UnityEngine.Object) replaced))
      return;
    existing.AddTag(GameTags.HasInvalidPorts);
  }

  public void UnmarkArea(int cell, Orientation orientation, ObjectLayer layer, GameObject go)
  {
    for (int index1 = 0; index1 < this.PlacementOffsets.Length; ++index1)
    {
      CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.PlacementOffsets[index1], orientation);
      int index2 = Grid.OffsetCell(cell, rotatedCellOffset);
      if ((UnityEngine.Object) Grid.Objects[index2, (int) layer] == (UnityEngine.Object) go)
        Grid.Objects[index2, (int) layer] = (GameObject) null;
    }
    if (this.InputConduitType != ConduitType.None)
    {
      CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.UtilityInputOffset, orientation);
      int index = Grid.OffsetCell(cell, rotatedCellOffset);
      ObjectLayer layerForConduitType = Grid.GetObjectLayerForConduitType(this.InputConduitType);
      if ((UnityEngine.Object) Grid.Objects[index, (int) layerForConduitType] == (UnityEngine.Object) go)
        Grid.Objects[index, (int) layerForConduitType] = (GameObject) null;
    }
    if (this.OutputConduitType != ConduitType.None)
    {
      CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.UtilityOutputOffset, orientation);
      int index = Grid.OffsetCell(cell, rotatedCellOffset);
      ObjectLayer layerForConduitType = Grid.GetObjectLayerForConduitType(this.OutputConduitType);
      if ((UnityEngine.Object) Grid.Objects[index, (int) layerForConduitType] == (UnityEngine.Object) go)
        Grid.Objects[index, (int) layerForConduitType] = (GameObject) null;
    }
    if (this.RequiresPowerInput)
    {
      CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.PowerInputOffset, orientation);
      int index = Grid.OffsetCell(cell, rotatedCellOffset);
      if ((UnityEngine.Object) Grid.Objects[index, 29] == (UnityEngine.Object) go)
        Grid.Objects[index, 29] = (GameObject) null;
    }
    if (this.RequiresPowerOutput || (double) this.GeneratorWattageRating > 0.0)
    {
      CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.PowerOutputOffset, orientation);
      int index = Grid.OffsetCell(cell, rotatedCellOffset);
      if ((UnityEngine.Object) Grid.Objects[index, 29] == (UnityEngine.Object) go)
        Grid.Objects[index, 29] = (GameObject) null;
    }
    if (this.BuildLocationRule == BuildLocationRule.HighWattBridgeTile)
    {
      int linked_cell1;
      int linked_cell2;
      go.GetComponent<UtilityNetworkLink>().GetCells(cell, orientation, out linked_cell1, out linked_cell2);
      if ((UnityEngine.Object) Grid.Objects[linked_cell1, 29] == (UnityEngine.Object) go)
        Grid.Objects[linked_cell1, 29] = (GameObject) null;
      if ((UnityEngine.Object) Grid.Objects[linked_cell2, 29] == (UnityEngine.Object) go)
        Grid.Objects[linked_cell2, 29] = (GameObject) null;
    }
    ISecondaryInput component1 = this.BuildingComplete.GetComponent<ISecondaryInput>();
    if (component1 != null)
    {
      ObjectLayer layerForConduitType = Grid.GetObjectLayerForConduitType(component1.GetSecondaryConduitType());
      CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(component1.GetSecondaryConduitOffset(), orientation);
      int index = Grid.OffsetCell(cell, rotatedCellOffset);
      if ((UnityEngine.Object) Grid.Objects[index, (int) layerForConduitType] == (UnityEngine.Object) go)
        Grid.Objects[index, (int) layerForConduitType] = (GameObject) null;
    }
    ISecondaryOutput component2 = this.BuildingComplete.GetComponent<ISecondaryOutput>();
    if (component2 == null)
      return;
    ObjectLayer layerForConduitType1 = Grid.GetObjectLayerForConduitType(component2.GetSecondaryConduitType());
    CellOffset rotatedCellOffset1 = Rotatable.GetRotatedCellOffset(component2.GetSecondaryConduitOffset(), orientation);
    int index3 = Grid.OffsetCell(cell, rotatedCellOffset1);
    if (!((UnityEngine.Object) Grid.Objects[index3, (int) layerForConduitType1] == (UnityEngine.Object) go))
      return;
    Grid.Objects[index3, (int) layerForConduitType1] = (GameObject) null;
  }

  public int GetBuildingCell(int cell)
  {
    return cell + (this.WidthInCells - 1) / 2;
  }

  public Vector3 GetVisualizerOffset()
  {
    return Vector3.right * (0.5f * (float) ((this.WidthInCells + 1) % 2));
  }

  public bool IsValidPlaceLocation(
    GameObject go,
    Vector3 pos,
    Orientation orientation,
    out string fail_reason)
  {
    int cell = Grid.PosToCell(pos);
    return this.IsValidPlaceLocation(go, cell, orientation, false, out fail_reason);
  }

  public bool IsValidPlaceLocation(
    GameObject go,
    Vector3 pos,
    Orientation orientation,
    bool replace_tile,
    out string fail_reason)
  {
    int cell = Grid.PosToCell(pos);
    return this.IsValidPlaceLocation(go, cell, orientation, replace_tile, out fail_reason);
  }

  public bool IsValidPlaceLocation(
    GameObject go,
    int cell,
    Orientation orientation,
    out string fail_reason)
  {
    if (!Grid.IsValidBuildingCell(cell))
    {
      fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_INVALID_CELL;
      return false;
    }
    if (this.BuildLocationRule == BuildLocationRule.OnWall)
    {
      if (!BuildingDef.CheckFoundation(cell, orientation, this.BuildLocationRule, this.WidthInCells, this.HeightInCells))
      {
        fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_WALL;
        return false;
      }
    }
    else if (this.BuildLocationRule == BuildLocationRule.InCorner && !BuildingDef.CheckFoundation(cell, orientation, this.BuildLocationRule, this.WidthInCells, this.HeightInCells))
    {
      fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_CORNER;
      return false;
    }
    return this.IsAreaClear(go, cell, orientation, this.ObjectLayer, this.TileLayer, false, out fail_reason);
  }

  public bool IsValidPlaceLocation(
    GameObject go,
    int cell,
    Orientation orientation,
    bool replace_tile,
    out string fail_reason)
  {
    if (!Grid.IsValidBuildingCell(cell))
    {
      fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_INVALID_CELL;
      return false;
    }
    if (this.BuildLocationRule == BuildLocationRule.OnWall)
    {
      if (!BuildingDef.CheckFoundation(cell, orientation, this.BuildLocationRule, this.WidthInCells, this.HeightInCells))
      {
        fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_WALL;
        return false;
      }
    }
    else if (this.BuildLocationRule == BuildLocationRule.InCorner && !BuildingDef.CheckFoundation(cell, orientation, this.BuildLocationRule, this.WidthInCells, this.HeightInCells))
    {
      fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_CORNER;
      return false;
    }
    return this.IsAreaClear(go, cell, orientation, this.ObjectLayer, this.TileLayer, replace_tile, out fail_reason);
  }

  public bool IsValidReplaceLocation(
    Vector3 pos,
    Orientation orientation,
    ObjectLayer replace_layer,
    ObjectLayer obj_layer)
  {
    if (replace_layer == ObjectLayer.NumLayers)
      return false;
    bool flag = true;
    int cell1 = Grid.PosToCell(pos);
    for (int index = 0; index < this.PlacementOffsets.Length; ++index)
    {
      CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.PlacementOffsets[index], orientation);
      int cell2 = Grid.OffsetCell(cell1, rotatedCellOffset);
      if (!Grid.IsValidBuildingCell(cell2))
        return false;
      if ((UnityEngine.Object) Grid.Objects[cell2, (int) obj_layer] == (UnityEngine.Object) null || (UnityEngine.Object) Grid.Objects[cell2, (int) replace_layer] != (UnityEngine.Object) null)
      {
        flag = false;
        break;
      }
    }
    return flag;
  }

  public bool IsValidBuildLocation(GameObject source_go, Vector3 pos, Orientation orientation)
  {
    string reason = string.Empty;
    return this.IsValidBuildLocation(source_go, pos, orientation, out reason);
  }

  public bool IsValidBuildLocation(
    GameObject source_go,
    Vector3 pos,
    Orientation orientation,
    out string reason)
  {
    int cell = Grid.PosToCell(pos);
    return this.IsValidBuildLocation(source_go, cell, orientation, out reason);
  }

  public bool IsValidBuildLocation(
    GameObject source_go,
    int cell,
    Orientation orientation,
    out string fail_reason)
  {
    if (!Grid.IsValidBuildingCell(cell))
    {
      fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_INVALID_CELL;
      return false;
    }
    if (!this.IsAreaValid(cell, orientation, out fail_reason))
      return false;
    bool flag = true;
    fail_reason = (string) null;
    switch (this.BuildLocationRule)
    {
      case BuildLocationRule.Anywhere:
      case BuildLocationRule.Conduit:
        flag = true;
        break;
      case BuildLocationRule.OnFloor:
      case BuildLocationRule.OnCeiling:
      case BuildLocationRule.OnFoundationRotatable:
        if (!BuildingDef.CheckFoundation(cell, orientation, this.BuildLocationRule, this.WidthInCells, this.HeightInCells))
        {
          flag = false;
          fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_FLOOR;
          break;
        }
        break;
      case BuildLocationRule.OnFloorOverSpace:
        if (!BuildingDef.CheckFoundation(cell, orientation, this.BuildLocationRule, this.WidthInCells, this.HeightInCells))
        {
          flag = false;
          fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_FLOOR;
          break;
        }
        if (!BuildingDef.AreAllCellsValid(cell, orientation, this.WidthInCells, this.HeightInCells, (Func<int, bool>) (check_cell => World.Instance.zoneRenderData.GetSubWorldZoneType(check_cell) == SubWorld.ZoneType.Space)))
        {
          flag = false;
          fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_SPACE;
          break;
        }
        break;
      case BuildLocationRule.OnWall:
        if (!BuildingDef.CheckFoundation(cell, orientation, this.BuildLocationRule, this.WidthInCells, this.HeightInCells))
        {
          flag = false;
          fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_WALL;
          break;
        }
        break;
      case BuildLocationRule.InCorner:
        if (!BuildingDef.CheckFoundation(cell, orientation, this.BuildLocationRule, this.WidthInCells, this.HeightInCells))
        {
          flag = false;
          fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_CORNER;
          break;
        }
        break;
      case BuildLocationRule.Tile:
        flag = true;
        GameObject gameObject1 = Grid.Objects[cell, 27];
        if ((UnityEngine.Object) gameObject1 != (UnityEngine.Object) null)
        {
          Building component = gameObject1.GetComponent<Building>();
          if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.Def.BuildLocationRule == BuildLocationRule.NotInTiles)
            flag = false;
        }
        GameObject gameObject2 = Grid.Objects[cell, 2];
        if ((UnityEngine.Object) gameObject2 != (UnityEngine.Object) null)
        {
          Building component = gameObject2.GetComponent<Building>();
          if ((UnityEngine.Object) component != (UnityEngine.Object) null && component.Def.BuildLocationRule == BuildLocationRule.NotInTiles)
          {
            flag = false;
            break;
          }
          break;
        }
        break;
      case BuildLocationRule.NotInTiles:
        GameObject gameObject3 = Grid.Objects[cell, 9];
        flag = ((UnityEngine.Object) gameObject3 == (UnityEngine.Object) null || (UnityEngine.Object) gameObject3 == (UnityEngine.Object) source_go) && !Grid.HasDoor[cell];
        if (flag)
        {
          GameObject gameObject4 = Grid.Objects[cell, (int) this.ObjectLayer];
          if ((UnityEngine.Object) gameObject4 != (UnityEngine.Object) null)
          {
            if (this.ReplacementLayer == ObjectLayer.NumLayers)
            {
              flag = flag && ((UnityEngine.Object) gameObject4 == (UnityEngine.Object) null || (UnityEngine.Object) gameObject4 == (UnityEngine.Object) source_go);
            }
            else
            {
              Building component = gameObject4.GetComponent<Building>();
              flag = (UnityEngine.Object) component == (UnityEngine.Object) null || component.Def.ReplacementLayer == this.ReplacementLayer;
            }
          }
        }
        fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_NOT_IN_TILES;
        break;
      case BuildLocationRule.BuildingAttachPoint:
        flag = false;
        for (int index1 = 0; index1 < Components.BuildingAttachPoints.Count && !flag; ++index1)
        {
          for (int index2 = 0; index2 < Components.BuildingAttachPoints[index1].points.Length; ++index2)
          {
            if (Components.BuildingAttachPoints[index1].AcceptsAttachment(this.AttachmentSlotTag, Grid.OffsetCell(cell, this.attachablePosition)))
            {
              flag = true;
              break;
            }
          }
        }
        fail_reason = string.Format((string) UI.TOOLTIPS.HELP_BUILDLOCATION_ATTACHPOINT, (object) this.AttachmentSlotTag);
        break;
      case BuildLocationRule.OnFloorOrBuildingAttachPoint:
        if (!BuildingDef.CheckFoundation(cell, orientation, BuildLocationRule.OnFloor, this.WidthInCells, this.HeightInCells))
        {
          flag = false;
          fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_FLOOR_OR_ATTACHPOINT;
          if (!flag)
          {
            for (int index1 = 0; index1 < Components.BuildingAttachPoints.Count && !flag; ++index1)
            {
              for (int index2 = 0; index2 < Components.BuildingAttachPoints[index1].points.Length; ++index2)
              {
                if (Components.BuildingAttachPoints[index1].AcceptsAttachment(this.AttachmentSlotTag, Grid.OffsetCell(cell, this.attachablePosition)))
                {
                  flag = true;
                  break;
                }
              }
            }
            fail_reason = string.Format((string) UI.TOOLTIPS.HELP_BUILDLOCATION_FLOOR_OR_ATTACHPOINT, (object) this.AttachmentSlotTag);
            break;
          }
          break;
        }
        flag = true;
        break;
    }
    return flag && this.ArePowerPortsInValidPositions(source_go, cell, orientation, out fail_reason) && this.AreConduitPortsInValidPositions(source_go, cell, orientation, out fail_reason);
  }

  private bool IsAreaValid(int cell, Orientation orientation, out string fail_reason)
  {
    bool flag = true;
    fail_reason = (string) null;
    for (int index = 0; index < this.PlacementOffsets.Length; ++index)
    {
      CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.PlacementOffsets[index], orientation);
      if (!Grid.IsCellOffsetValid(cell, rotatedCellOffset))
      {
        fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_INVALID_CELL;
        flag = false;
        break;
      }
      int cell1 = Grid.OffsetCell(cell, rotatedCellOffset);
      if (!Grid.IsValidBuildingCell(cell1))
      {
        fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_INVALID_CELL;
        flag = false;
        break;
      }
      if (Grid.Element[cell1].id == SimHashes.Unobtanium)
      {
        fail_reason = (string) null;
        flag = false;
        break;
      }
    }
    return flag;
  }

  private bool ArePowerPortsInValidPositions(
    GameObject source_go,
    int cell,
    Orientation orientation,
    out string fail_reason)
  {
    fail_reason = (string) null;
    if ((UnityEngine.Object) source_go == (UnityEngine.Object) null)
      return true;
    if (this.RequiresPowerInput)
    {
      CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.PowerInputOffset, orientation);
      int index = Grid.OffsetCell(cell, rotatedCellOffset);
      GameObject gameObject = Grid.Objects[index, 29];
      if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null && (UnityEngine.Object) gameObject != (UnityEngine.Object) source_go)
      {
        fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_WIRECONNECTORS_OVERLAP;
        return false;
      }
    }
    if (this.RequiresPowerOutput || (double) this.GeneratorWattageRating > 0.0)
    {
      CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.PowerOutputOffset, orientation);
      int index = Grid.OffsetCell(cell, rotatedCellOffset);
      GameObject gameObject = Grid.Objects[index, 29];
      if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null && (UnityEngine.Object) gameObject != (UnityEngine.Object) source_go)
      {
        fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_WIRECONNECTORS_OVERLAP;
        return false;
      }
    }
    return true;
  }

  private bool AreConduitPortsInValidPositions(
    GameObject source_go,
    int cell,
    Orientation orientation,
    out string fail_reason)
  {
    fail_reason = (string) null;
    if ((UnityEngine.Object) source_go == (UnityEngine.Object) null)
      return true;
    bool flag = true;
    if (this.InputConduitType != ConduitType.None)
    {
      CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.UtilityInputOffset, orientation);
      int utility_cell = Grid.OffsetCell(cell, rotatedCellOffset);
      flag = this.IsValidConduitConnection(source_go, this.InputConduitType, utility_cell, ref fail_reason);
    }
    if (flag && this.OutputConduitType != ConduitType.None)
    {
      CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(this.UtilityOutputOffset, orientation);
      int utility_cell = Grid.OffsetCell(cell, rotatedCellOffset);
      flag = this.IsValidConduitConnection(source_go, this.OutputConduitType, utility_cell, ref fail_reason);
    }
    Building component1 = source_go.GetComponent<Building>();
    if (flag && (bool) ((UnityEngine.Object) component1))
    {
      ISecondaryInput component2 = component1.Def.BuildingComplete.GetComponent<ISecondaryInput>();
      if (component2 != null)
      {
        ConduitType secondaryConduitType = component2.GetSecondaryConduitType();
        CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(component2.GetSecondaryConduitOffset(), orientation);
        int utility_cell = Grid.OffsetCell(cell, rotatedCellOffset);
        flag = this.IsValidConduitConnection(source_go, secondaryConduitType, utility_cell, ref fail_reason);
      }
    }
    if (flag)
    {
      ISecondaryOutput component2 = component1.Def.BuildingComplete.GetComponent<ISecondaryOutput>();
      if (component2 != null)
      {
        ConduitType secondaryConduitType = component2.GetSecondaryConduitType();
        CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(component2.GetSecondaryConduitOffset(), orientation);
        int utility_cell = Grid.OffsetCell(cell, rotatedCellOffset);
        flag = this.IsValidConduitConnection(source_go, secondaryConduitType, utility_cell, ref fail_reason);
      }
    }
    return flag;
  }

  private bool IsValidWireBridgeLocation(
    GameObject source_go,
    int cell,
    Orientation orientation,
    out string fail_reason)
  {
    UtilityNetworkLink component = source_go.GetComponent<UtilityNetworkLink>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
    {
      int linked_cell1;
      int linked_cell2;
      component.GetCells(out linked_cell1, out linked_cell2);
      if ((UnityEngine.Object) Grid.Objects[linked_cell1, 29] != (UnityEngine.Object) null || (UnityEngine.Object) Grid.Objects[linked_cell2, 29] != (UnityEngine.Object) null)
      {
        fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_WIRECONNECTORS_OVERLAP;
        return false;
      }
    }
    fail_reason = (string) null;
    return true;
  }

  private bool IsValidHighWattBridgeLocation(
    GameObject source_go,
    int cell,
    Orientation orientation,
    out string fail_reason)
  {
    UtilityNetworkLink component = source_go.GetComponent<UtilityNetworkLink>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
    {
      if (!component.AreCellsValid(cell, orientation))
      {
        fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_INVALID_CELL;
        return false;
      }
      int linked_cell1;
      int linked_cell2;
      component.GetCells(out linked_cell1, out linked_cell2);
      if ((UnityEngine.Object) Grid.Objects[linked_cell1, 29] != (UnityEngine.Object) null || (UnityEngine.Object) Grid.Objects[linked_cell2, 29] != (UnityEngine.Object) null)
      {
        fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_WIRECONNECTORS_OVERLAP;
        return false;
      }
      if ((UnityEngine.Object) Grid.Objects[linked_cell1, 9] != (UnityEngine.Object) null || (UnityEngine.Object) Grid.Objects[linked_cell2, 9] != (UnityEngine.Object) null)
      {
        fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_HIGHWATT_NOT_IN_TILE;
        return false;
      }
      if (Grid.HasDoor[linked_cell1] || Grid.HasDoor[linked_cell2])
      {
        fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_HIGHWATT_NOT_IN_TILE;
        return false;
      }
      GameObject gameObject1 = Grid.Objects[linked_cell1, 1];
      GameObject gameObject2 = Grid.Objects[linked_cell2, 1];
      if ((UnityEngine.Object) gameObject1 != (UnityEngine.Object) null || (UnityEngine.Object) gameObject2 != (UnityEngine.Object) null)
      {
        global::BuildingUnderConstruction underConstruction1 = !(bool) ((UnityEngine.Object) gameObject1) ? (global::BuildingUnderConstruction) null : gameObject1.GetComponent<global::BuildingUnderConstruction>();
        global::BuildingUnderConstruction underConstruction2 = !(bool) ((UnityEngine.Object) gameObject2) ? (global::BuildingUnderConstruction) null : gameObject2.GetComponent<global::BuildingUnderConstruction>();
        if ((bool) ((UnityEngine.Object) underConstruction1) && (bool) ((UnityEngine.Object) underConstruction1.Def.BuildingComplete.GetComponent<Door>()) || (bool) ((UnityEngine.Object) underConstruction2) && (bool) ((UnityEngine.Object) underConstruction2.Def.BuildingComplete.GetComponent<Door>()))
        {
          fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_HIGHWATT_NOT_IN_TILE;
          return false;
        }
      }
    }
    fail_reason = (string) null;
    return true;
  }

  private bool AreLogicPortsInValidPositions(
    GameObject source_go,
    int cell,
    out string fail_reason)
  {
    fail_reason = (string) null;
    if ((UnityEngine.Object) source_go == (UnityEngine.Object) null)
      return true;
    ReadOnlyCollection<ILogicUIElement> visElements = Game.Instance.logicCircuitManager.GetVisElements();
    LogicPorts component1 = source_go.GetComponent<LogicPorts>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
    {
      component1.HackRefreshVisualizers();
      if (this.DoLogicPortsConflict((IList<ILogicUIElement>) component1.inputPorts, (IList<ILogicUIElement>) visElements) || this.DoLogicPortsConflict((IList<ILogicUIElement>) component1.outputPorts, (IList<ILogicUIElement>) visElements))
      {
        fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_LOGIC_PORTS_OBSTRUCTED;
        return false;
      }
    }
    else
    {
      LogicGateBase component2 = source_go.GetComponent<LogicGateBase>();
      if ((UnityEngine.Object) component2 != (UnityEngine.Object) null && (this.IsLogicPortObstructed(component2.InputCellOne, (IList<ILogicUIElement>) visElements) || this.IsLogicPortObstructed(component2.OutputCell, (IList<ILogicUIElement>) visElements) || component2.RequiresTwoInputs && this.IsLogicPortObstructed(component2.InputCellTwo, (IList<ILogicUIElement>) visElements)))
      {
        fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_LOGIC_PORTS_OBSTRUCTED;
        return false;
      }
    }
    return true;
  }

  private bool DoLogicPortsConflict(IList<ILogicUIElement> ports_a, IList<ILogicUIElement> ports_b)
  {
    if (ports_a == null || ports_b == null)
      return false;
    foreach (ILogicUIElement logicUiElement1 in (IEnumerable<ILogicUIElement>) ports_a)
    {
      int logicUiCell = logicUiElement1.GetLogicUICell();
      foreach (ILogicUIElement logicUiElement2 in (IEnumerable<ILogicUIElement>) ports_b)
      {
        if (logicUiElement1 != logicUiElement2 && logicUiCell == logicUiElement2.GetLogicUICell())
          return true;
      }
    }
    return false;
  }

  private bool IsLogicPortObstructed(int cell, IList<ILogicUIElement> ports)
  {
    int num = 0;
    foreach (ILogicUIElement port in (IEnumerable<ILogicUIElement>) ports)
    {
      if (port.GetLogicUICell() == cell)
        ++num;
    }
    return num > 0;
  }

  private bool IsValidConduitConnection(
    GameObject source_go,
    ConduitType conduit_type,
    int utility_cell,
    ref string fail_reason)
  {
    bool flag = true;
    switch (conduit_type)
    {
      case ConduitType.Gas:
        GameObject gameObject1 = Grid.Objects[utility_cell, 15];
        if ((UnityEngine.Object) gameObject1 != (UnityEngine.Object) null && (UnityEngine.Object) gameObject1 != (UnityEngine.Object) source_go)
        {
          flag = false;
          fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_GASPORTS_OVERLAP;
          break;
        }
        break;
      case ConduitType.Liquid:
        GameObject gameObject2 = Grid.Objects[utility_cell, 19];
        if ((UnityEngine.Object) gameObject2 != (UnityEngine.Object) null && (UnityEngine.Object) gameObject2 != (UnityEngine.Object) source_go)
        {
          flag = false;
          fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_LIQUIDPORTS_OVERLAP;
          break;
        }
        break;
      case ConduitType.Solid:
        GameObject gameObject3 = Grid.Objects[utility_cell, 23];
        if ((UnityEngine.Object) gameObject3 != (UnityEngine.Object) null && (UnityEngine.Object) gameObject3 != (UnityEngine.Object) source_go)
        {
          flag = false;
          fail_reason = (string) UI.TOOLTIPS.HELP_BUILDLOCATION_SOLIDPORTS_OVERLAP;
          break;
        }
        break;
    }
    return flag;
  }

  public static int GetXOffset(int width)
  {
    return -(width - 1) / 2;
  }

  public static bool CheckFoundation(
    int cell,
    Orientation orientation,
    BuildLocationRule location_rule,
    int width,
    int height)
  {
    switch (location_rule)
    {
      case BuildLocationRule.OnWall:
        return BuildingDef.CheckWallFoundation(cell, width, height, orientation != Orientation.FlipH);
      case BuildLocationRule.InCorner:
        if (BuildingDef.CheckBaseFoundation(cell, orientation, BuildLocationRule.OnCeiling, width, height))
          return BuildingDef.CheckWallFoundation(cell, width, height, orientation != Orientation.FlipH);
        return false;
      default:
        return BuildingDef.CheckBaseFoundation(cell, orientation, location_rule, width, height);
    }
  }

  public static bool CheckBaseFoundation(
    int cell,
    Orientation orientation,
    BuildLocationRule location_rule,
    int width,
    int height)
  {
    int num1 = -(width - 1) / 2;
    int num2 = width / 2;
    for (int x = num1; x <= num2; ++x)
    {
      CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(location_rule != BuildLocationRule.OnCeiling ? new CellOffset(x, -1) : new CellOffset(x, height), orientation);
      int cell1 = Grid.OffsetCell(cell, rotatedCellOffset);
      if (!Grid.IsValidBuildingCell(cell1) || !Grid.Solid[cell1])
        return false;
    }
    return true;
  }

  public static bool CheckWallFoundation(int cell, int width, int height, bool leftWall)
  {
    int num1 = 0;
    int num2 = height;
    for (int y = num1; y <= num2; ++y)
    {
      CellOffset offset = new CellOffset(!leftWall ? width / 2 + 1 : -(width - 1) / 2 - 1, y);
      int cell1 = Grid.OffsetCell(cell, offset);
      if (!Grid.IsValidBuildingCell(cell1) || !Grid.Solid[cell1])
        return false;
    }
    return true;
  }

  public static bool AreAllCellsValid(
    int base_cell,
    Orientation orientation,
    int width,
    int height,
    Func<int, bool> valid_cell_check)
  {
    int num1 = -(width - 1) / 2;
    int num2 = width / 2;
    if (orientation == Orientation.FlipH)
    {
      int num3 = num1;
      num1 = -num2;
      num2 = -num3;
    }
    for (int y = 0; y < height; ++y)
    {
      for (int x = num1; x <= num2; ++x)
      {
        int num3 = Grid.OffsetCell(base_cell, x, y);
        if (!valid_cell_check(num3))
          return false;
      }
    }
    return true;
  }

  public Sprite GetUISprite(string animName = "ui", bool centered = false)
  {
    return Def.GetUISpriteFromMultiObjectAnim(this.AnimFiles[0], animName, centered, string.Empty);
  }

  public void GenerateOffsets()
  {
    this.GenerateOffsets(this.WidthInCells, this.HeightInCells);
  }

  public void GenerateOffsets(int width, int height)
  {
    if (BuildingDef.placementOffsetsCache.TryGetValue(new CellOffset(width, height), out this.PlacementOffsets))
      return;
    int num1 = width / 2 - width + 1;
    this.PlacementOffsets = new CellOffset[width * height];
    for (int index1 = 0; index1 != height; ++index1)
    {
      int num2 = index1 * width;
      for (int index2 = 0; index2 != width; ++index2)
      {
        int index3 = num2 + index2;
        this.PlacementOffsets[index3].x = index2 + num1;
        this.PlacementOffsets[index3].y = index1;
      }
    }
    BuildingDef.placementOffsetsCache.Add(new CellOffset(width, height), this.PlacementOffsets);
  }

  public void PostProcess()
  {
    this.CraftRecipe = new Recipe(this.BuildingComplete.PrefabID().Name, 1f, (SimHashes) 0, this.Name, (string) null, 0);
    this.CraftRecipe.Icon = this.UISprite;
    for (int index = 0; index < this.MaterialCategory.Length; ++index)
      this.CraftRecipe.Ingredients.Add(new Recipe.Ingredient(this.MaterialCategory[index], (float) (int) this.Mass[index]));
    if ((UnityEngine.Object) this.DecorBlockTileInfo != (UnityEngine.Object) null)
      this.DecorBlockTileInfo.PostProcess();
    if ((UnityEngine.Object) this.DecorPlaceBlockTileInfo != (UnityEngine.Object) null)
      this.DecorPlaceBlockTileInfo.PostProcess();
    if (this.Deprecated)
      return;
    Db.Get().TechItems.AddTechItem(this.PrefabID, this.Name, this.Effect, new Func<string, bool, Sprite>(this.GetUISprite));
  }

  public bool MaterialsAvailable(IList<Tag> selected_elements)
  {
    bool flag = true;
    foreach (Recipe.Ingredient allIngredient in this.CraftRecipe.GetAllIngredients(selected_elements))
    {
      if ((double) WorldInventory.Instance.GetAmount(allIngredient.tag) < (double) allIngredient.amount)
      {
        flag = false;
        break;
      }
    }
    return flag;
  }

  public bool CheckRequiresBuildingCellVisualizer()
  {
    if (!this.CheckRequiresPowerInput() && !this.CheckRequiresPowerOutput() && (!this.CheckRequiresGasInput() && !this.CheckRequiresGasOutput()) && (!this.CheckRequiresLiquidInput() && !this.CheckRequiresLiquidOutput() && (!this.CheckRequiresSolidInput() && !this.CheckRequiresSolidOutput())))
      return this.DiseaseCellVisName != null;
    return true;
  }

  public bool CheckRequiresPowerInput()
  {
    return this.RequiresPowerInput;
  }

  public bool CheckRequiresPowerOutput()
  {
    if ((double) this.GeneratorWattageRating <= 0.0)
      return this.RequiresPowerOutput;
    return true;
  }

  public bool CheckRequiresGasInput()
  {
    return this.InputConduitType == ConduitType.Gas;
  }

  public bool CheckRequiresGasOutput()
  {
    return this.OutputConduitType == ConduitType.Gas;
  }

  public bool CheckRequiresLiquidInput()
  {
    return this.InputConduitType == ConduitType.Liquid;
  }

  public bool CheckRequiresLiquidOutput()
  {
    return this.OutputConduitType == ConduitType.Liquid;
  }

  public bool CheckRequiresSolidInput()
  {
    return this.InputConduitType == ConduitType.Solid;
  }

  public bool CheckRequiresSolidOutput()
  {
    return this.OutputConduitType == ConduitType.Solid;
  }
}
