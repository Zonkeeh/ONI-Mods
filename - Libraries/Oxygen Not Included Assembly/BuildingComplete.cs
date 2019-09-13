// Decompiled with JetBrains decompiler
// Type: BuildingComplete
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using KSerialization;
using System.Collections.Generic;
using UnityEngine;

public class BuildingComplete : Building
{
  private static readonly EventSystem.IntraObjectHandler<BuildingComplete> OnObjectReplacedDelegate = new EventSystem.IntraObjectHandler<BuildingComplete>((System.Action<BuildingComplete, object>) ((component, data) => component.OnObjectReplaced(data)));
  [Serialize]
  public float creationTime = -1f;
  private ObjectLayer replacingTileLayer = ObjectLayer.NumLayers;
  public List<AttributeModifier> regionModifiers = new List<AttributeModifier>();
  [MyCmpReq]
  private Modifiers modifiers;
  [MyCmpGet]
  public Assignable assignable;
  [MyCmpGet]
  public KPrefabID prefabid;
  public bool isManuallyOperated;
  public bool isArtable;
  private bool hasSpawnedKComponents;
  private HandleVector<int>.Handle scenePartitionerEntry;

  private bool WasReplaced()
  {
    return this.replacingTileLayer != ObjectLayer.NumLayers;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    Vector3 position = this.transform.GetPosition();
    position.z = Grid.GetLayerZ(this.Def.SceneLayer);
    this.transform.SetPosition(position);
    this.gameObject.SetLayerRecursively(LayerMask.NameToLayer("Default"));
    Attributes attributes = this.GetAttributes();
    foreach (Klei.AI.Attribute attribute in this.Def.attributes)
      attributes.Add(attribute);
    foreach (AttributeModifier attributeModifier in this.Def.attributeModifiers)
    {
      Klei.AI.Attribute attribute = Db.Get().BuildingAttributes.Get(attributeModifier.AttributeId);
      if (attributes.Get(attribute) == null)
        attributes.Add(attribute);
      attributes.Add(attributeModifier);
    }
    foreach (AttributeInstance attributeInstance in attributes)
      this.regionModifiers.Add(new AttributeModifier(attributeInstance.Id, attributeInstance.GetTotalValue(), (string) null, false, false, true));
    if (this.Def.UseStructureTemperature)
      GameComps.StructureTemperatures.Add(this.gameObject);
    this.Subscribe<BuildingComplete>(1606648047, BuildingComplete.OnObjectReplacedDelegate);
  }

  private void OnObjectReplaced(object data)
  {
    this.replacingTileLayer = (ObjectLayer) data;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    KBatchedAnimController component1 = this.GetComponent<KBatchedAnimController>();
    Rotatable component2 = this.GetComponent<Rotatable>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null && (UnityEngine.Object) component2 == (UnityEngine.Object) null)
      component1.Offset = this.Def.GetVisualizerOffset() + this.Def.placementPivot;
    KBoxCollider2D component3 = this.GetComponent<KBoxCollider2D>();
    if ((UnityEngine.Object) component3 != (UnityEngine.Object) null)
    {
      Vector3 visualizerOffset = this.Def.GetVisualizerOffset();
      component3.offset = component3.offset + new Vector2(visualizerOffset.x, visualizerOffset.y);
    }
    int cell = Grid.PosToCell(this.transform.GetPosition());
    if (this.Def.IsFoundation)
    {
      foreach (int placementCell in this.PlacementCells)
      {
        Grid.Foundation[placementCell] = true;
        Game.Instance.roomProber.SolidChangedEvent(placementCell, false);
      }
    }
    this.transform.SetPosition(Grid.CellToPosCBC(cell, this.Def.SceneLayer));
    PrimaryElement component4 = this.GetComponent<PrimaryElement>();
    if ((UnityEngine.Object) component4 != (UnityEngine.Object) null && (double) component4.Mass == 0.0)
      component4.Mass = this.Def.Mass[0];
    this.Def.MarkArea(cell, this.Orientation, this.Def.ObjectLayer, this.gameObject);
    if (this.Def.IsTilePiece)
    {
      this.Def.MarkArea(cell, this.Orientation, this.Def.TileLayer, this.gameObject);
      this.Def.RunOnArea(cell, this.Orientation, (System.Action<int>) (c => TileVisualizer.RefreshCell(c, this.Def.TileLayer, this.Def.ReplacementLayer)));
    }
    this.RegisterBlockTileRenderer();
    if (this.Def.PreventIdleTraversalPastBuilding)
    {
      for (int index = 0; index < this.PlacementCells.Length; ++index)
        Grid.PreventIdleTraversal[this.PlacementCells[index]] = true;
    }
    KSelectable component5 = this.GetComponent<KSelectable>();
    if ((UnityEngine.Object) component5 != (UnityEngine.Object) null)
      component5.SetStatusIndicatorOffset(this.Def.placementPivot);
    Components.BuildingCompletes.Add(this);
    BuildingConfigManager.Instance.AddBuildingCompleteKComponents(this.gameObject, this.Def.Tag);
    this.hasSpawnedKComponents = true;
    this.scenePartitionerEntry = GameScenePartitioner.Instance.Add(this.name, (object) this, this.GetExtents(), GameScenePartitioner.Instance.completeBuildings, (System.Action<object>) null);
    Attributes attributes = this.GetAttributes();
    if (attributes == null)
      return;
    Deconstructable component6 = this.GetComponent<Deconstructable>();
    if (!((UnityEngine.Object) component6 != (UnityEngine.Object) null))
      return;
    for (int index = 1; index < component6.constructionElements.Length; ++index)
    {
      Tag constructionElement = component6.constructionElements[index];
      Element element = ElementLoader.GetElement(constructionElement);
      if (element != null)
      {
        foreach (AttributeModifier attributeModifier in element.attributeModifiers)
          attributes.Add(attributeModifier);
      }
      else
      {
        GameObject prefab = Assets.TryGetPrefab(constructionElement);
        if ((UnityEngine.Object) prefab != (UnityEngine.Object) null)
        {
          PrefabAttributeModifiers component7 = prefab.GetComponent<PrefabAttributeModifiers>();
          if ((UnityEngine.Object) component7 != (UnityEngine.Object) null)
          {
            foreach (AttributeModifier descriptor in component7.descriptors)
              attributes.Add(descriptor);
          }
        }
      }
    }
  }

  public void SetCreationTime(float time)
  {
    this.creationTime = time;
  }

  private string GetInspectSound()
  {
    return GlobalAssets.GetSound("AI_Inspect_" + this.GetComponent<KPrefabID>().PrefabTag.Name, false);
  }

  protected override void OnCleanUp()
  {
    if (Game.quitting)
      return;
    GameScenePartitioner.Instance.Free(ref this.scenePartitionerEntry);
    if (this.hasSpawnedKComponents)
      BuildingConfigManager.Instance.DestroyBuildingCompleteKComponents(this.gameObject, this.Def.Tag);
    if (this.Def.UseStructureTemperature)
      GameComps.StructureTemperatures.Remove(this.gameObject);
    base.OnCleanUp();
    if (!this.WasReplaced())
    {
      int cell = Grid.PosToCell((KMonoBehaviour) this);
      this.Def.UnmarkArea(cell, this.Orientation, this.Def.ObjectLayer, this.gameObject);
      if (this.Def.IsTilePiece)
      {
        this.Def.UnmarkArea(cell, this.Orientation, this.Def.TileLayer, this.gameObject);
        this.Def.RunOnArea(cell, this.Orientation, (System.Action<int>) (c => TileVisualizer.RefreshCell(c, this.Def.TileLayer, this.Def.ReplacementLayer)));
      }
      if (this.Def.IsFoundation)
      {
        foreach (int placementCell in this.PlacementCells)
        {
          Grid.Foundation[placementCell] = false;
          Game.Instance.roomProber.SolidChangedEvent(placementCell, false);
        }
      }
      if (this.Def.PreventIdleTraversalPastBuilding)
      {
        for (int index = 0; index < this.PlacementCells.Length; ++index)
          Grid.PreventIdleTraversal[this.PlacementCells[index]] = false;
      }
    }
    if (this.WasReplaced() && this.Def.IsTilePiece && this.replacingTileLayer != this.Def.TileLayer)
    {
      int cell = Grid.PosToCell((KMonoBehaviour) this);
      this.Def.UnmarkArea(cell, this.Orientation, this.Def.TileLayer, this.gameObject);
      this.Def.RunOnArea(cell, this.Orientation, (System.Action<int>) (c => TileVisualizer.RefreshCell(c, this.Def.TileLayer, this.Def.ReplacementLayer)));
    }
    Components.BuildingCompletes.Remove(this);
    this.UnregisterBlockTileRenderer();
    this.Trigger(-21016276, (object) this);
  }
}
