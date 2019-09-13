// Decompiled with JetBrains decompiler
// Type: SimCellOccupier
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

[SkipSaveFileSerialization]
public class SimCellOccupier : KMonoBehaviour, IEffectDescriptor
{
  private static readonly EventSystem.IntraObjectHandler<SimCellOccupier> OnBuildingRepairedDelegate = new EventSystem.IntraObjectHandler<SimCellOccupier>((System.Action<SimCellOccupier, object>) ((component, data) => component.OnBuildingRepaired(data)));
  [SerializeField]
  public bool doReplaceElement = true;
  [SerializeField]
  public float strengthMultiplier = 1f;
  [SerializeField]
  public float movementSpeedMultiplier = 1f;
  private bool callDestroy = true;
  [MyCmpReq]
  private Building building;
  [MyCmpReq]
  private PrimaryElement primaryElement;
  [SerializeField]
  public bool setGasImpermeable;
  [SerializeField]
  public bool setLiquidImpermeable;
  [SerializeField]
  public bool setTransparent;
  [SerializeField]
  public bool setOpaque;
  [SerializeField]
  public bool notifyOnMelt;
  private bool isReady;

  public bool IsVisuallySolid
  {
    get
    {
      return this.doReplaceElement;
    }
  }

  protected override void OnPrefabInit()
  {
    this.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.Normal, (object) null);
  }

  protected override void OnSpawn()
  {
    HandleVector<Game.CallbackInfo>.Handle callbackHandle = Game.Instance.callbackManager.Add(new Game.CallbackInfo(new System.Action(this.OnModifyComplete), false));
    float mass_per_cell = this.primaryElement.Mass / (float) this.building.Def.PlacementOffsets.Length;
    this.building.RunOnArea((System.Action<int>) (offset_cell =>
    {
      if (this.doReplaceElement)
      {
        SimMessages.ReplaceAndDisplaceElement(offset_cell, this.primaryElement.ElementID, CellEventLogger.Instance.SimCellOccupierOnSpawn, mass_per_cell, this.primaryElement.Temperature, this.primaryElement.DiseaseIdx, this.primaryElement.DiseaseCount, callbackHandle.index);
        callbackHandle = HandleVector<Game.CallbackInfo>.InvalidHandle;
        SimMessages.SetStrength(offset_cell, 0, this.strengthMultiplier);
        Game.Instance.RemoveSolidChangedFilter(offset_cell);
      }
      else
      {
        this.ForceSetGameCellData(offset_cell);
        Game.Instance.AddSolidChangedFilter(offset_cell);
      }
      Sim.Cell.Properties simCellProperties = this.GetSimCellProperties();
      SimMessages.SetCellProperties(offset_cell, (byte) simCellProperties);
      Grid.RenderedByWorld[offset_cell] = false;
      Game.Instance.GetComponent<EntombedItemVisualizer>().ForceClear(offset_cell);
    }));
    this.Subscribe<SimCellOccupier>(-1699355994, SimCellOccupier.OnBuildingRepairedDelegate);
  }

  protected override void OnCleanUp()
  {
    if (!this.callDestroy)
      return;
    this.DestroySelf((System.Action) null);
  }

  private Sim.Cell.Properties GetSimCellProperties()
  {
    Sim.Cell.Properties properties = Sim.Cell.Properties.SolidImpermeable;
    if (this.setGasImpermeable)
      properties |= Sim.Cell.Properties.GasImpermeable;
    if (this.setLiquidImpermeable)
      properties |= Sim.Cell.Properties.LiquidImpermeable;
    if (this.setTransparent)
      properties |= Sim.Cell.Properties.Transparent;
    if (this.setOpaque)
      properties |= Sim.Cell.Properties.Opaque;
    if (this.notifyOnMelt)
      properties |= Sim.Cell.Properties.NotifyOnMelt;
    return properties;
  }

  public void DestroySelf(System.Action onComplete)
  {
    this.callDestroy = false;
    for (int index = 0; index < this.building.PlacementCells.Length; ++index)
    {
      int placementCell = this.building.PlacementCells[index];
      Game.Instance.RemoveSolidChangedFilter(placementCell);
      Sim.Cell.Properties simCellProperties = this.GetSimCellProperties();
      SimMessages.ClearCellProperties(placementCell, (byte) simCellProperties);
      if (this.doReplaceElement && Grid.Element[placementCell].id == this.primaryElement.ElementID)
      {
        HandleVector<int>.Handle handle1 = GameComps.DiseaseContainers.GetHandle(this.gameObject);
        if (handle1.IsValid())
        {
          DiseaseHeader header = GameComps.DiseaseContainers.GetHeader(handle1);
          header.diseaseIdx = Grid.DiseaseIdx[placementCell];
          header.diseaseCount = Grid.DiseaseCount[placementCell];
          GameComps.DiseaseContainers.SetHeader(handle1, header);
        }
        if (onComplete != null)
        {
          HandleVector<Game.CallbackInfo>.Handle handle2 = Game.Instance.callbackManager.Add(new Game.CallbackInfo(onComplete, false));
          SimMessages.ReplaceElement(placementCell, SimHashes.Vacuum, CellEventLogger.Instance.SimCellOccupierDestroySelf, 0.0f, -1f, byte.MaxValue, 0, handle2.index);
        }
        else
          SimMessages.ReplaceElement(placementCell, SimHashes.Vacuum, CellEventLogger.Instance.SimCellOccupierDestroySelf, 0.0f, -1f, byte.MaxValue, 0, -1);
        SimMessages.SetStrength(placementCell, 1, 1f);
      }
      else
      {
        Grid.SetSolid(placementCell, false, CellEventLogger.Instance.SimCellOccupierDestroy);
        onComplete.Signal();
        World.Instance.OnSolidChanged(placementCell);
        GameScenePartitioner.Instance.TriggerEvent(placementCell, GameScenePartitioner.Instance.solidChangedLayer, (object) null);
      }
    }
  }

  public bool IsReady()
  {
    return this.isReady;
  }

  private void OnModifyComplete()
  {
    if ((UnityEngine.Object) this == (UnityEngine.Object) null || (UnityEngine.Object) this.gameObject == (UnityEngine.Object) null)
      return;
    this.isReady = true;
    this.GetComponent<PrimaryElement>().SetUseSimDiseaseInfo(true);
    Vector2I xy = Grid.PosToXY(this.transform.GetPosition());
    GameScenePartitioner.Instance.TriggerEvent(xy.x, xy.y, 1, 1, GameScenePartitioner.Instance.solidChangedLayer, (object) null);
  }

  private void ForceSetGameCellData(int cell)
  {
    bool solid = !Grid.DupePassable[cell];
    Grid.SetSolid(cell, solid, CellEventLogger.Instance.SimCellOccupierForceSolid);
    Pathfinding.Instance.AddDirtyNavGridCell(cell);
    GameScenePartitioner.Instance.TriggerEvent(cell, GameScenePartitioner.Instance.solidChangedLayer, (object) null);
    Grid.Damage[cell] = 0.0f;
  }

  public List<Descriptor> GetDescriptors(BuildingDef def)
  {
    List<Descriptor> descriptorList = (List<Descriptor>) null;
    if ((double) this.movementSpeedMultiplier != 1.0)
    {
      descriptorList = new List<Descriptor>();
      Descriptor descriptor = new Descriptor();
      descriptor.SetupDescriptor(string.Format((string) UI.BUILDINGEFFECTS.DUPLICANTMOVEMENTBOOST, (object) GameUtil.AddPositiveSign(GameUtil.GetFormattedPercent((float) ((double) this.movementSpeedMultiplier * 100.0 - 100.0), GameUtil.TimeSlice.None), (double) this.movementSpeedMultiplier - 1.0 >= 0.0)), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.DUPLICANTMOVEMENTBOOST, (object) GameUtil.GetFormattedPercent((float) ((double) this.movementSpeedMultiplier * 100.0 - 100.0), GameUtil.TimeSlice.None)), Descriptor.DescriptorType.Effect);
      descriptorList.Add(descriptor);
    }
    return descriptorList;
  }

  private void OnBuildingRepaired(object data)
  {
    BuildingHP buildingHp = (BuildingHP) data;
    float damage = (float) (1.0 - (double) buildingHp.HitPoints / (double) buildingHp.MaxHitPoints);
    this.building.RunOnArea((System.Action<int>) (offset_cell => WorldDamage.Instance.RestoreDamageToValue(offset_cell, damage)));
  }
}
