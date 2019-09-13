// Decompiled with JetBrains decompiler
// Type: ElementFilter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class ElementFilter : KMonoBehaviour, ISaveLoadable, ISecondaryOutput
{
  private SimHashes filteredElem = SimHashes.Void;
  private int inputCell = -1;
  private int outputCell = -1;
  private int filteredCell = -1;
  [SerializeField]
  public ConduitPortInfo portInfo;
  [MyCmpReq]
  private Operational operational;
  [MyCmpReq]
  private Building building;
  [MyCmpReq]
  private KSelectable selectable;
  public Filterable filterable;
  private Guid needsConduitStatusItemGuid;
  private Guid conduitBlockedStatusItemGuid;
  private FlowUtilityNetwork.NetworkItem itemFilter;
  private HandleVector<int>.Handle partitionerEntry;
  private static StatusItem filterStatusItem;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.filterable = this.GetComponent<Filterable>();
    this.InitializeStatusItems();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.inputCell = this.building.GetUtilityInputCell();
    this.outputCell = this.building.GetUtilityOutputCell();
    this.filteredCell = Grid.OffsetCell(Grid.PosToCell(this.transform.GetPosition()), this.building.GetRotatedOffset(this.portInfo.offset));
    IUtilityNetworkMgr networkManager = Conduit.GetNetworkManager(this.portInfo.conduitType);
    this.itemFilter = new FlowUtilityNetwork.NetworkItem(this.portInfo.conduitType, Endpoint.Source, this.filteredCell, this.gameObject);
    networkManager.AddToNetworks(this.filteredCell, (object) this.itemFilter, true);
    this.GetComponent<ConduitConsumer>().isConsuming = false;
    this.OnFilterChanged(this.filterable.SelectedTag);
    this.filterable.onFilterChanged += new System.Action<Tag>(this.OnFilterChanged);
    Conduit.GetFlowManager(this.portInfo.conduitType).AddConduitUpdater(new System.Action<float>(this.OnConduitTick), ConduitFlowPriority.Default);
    this.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, ElementFilter.filterStatusItem, (object) this);
    this.UpdateConduitExistsStatus();
    this.UpdateConduitBlockedStatus();
    ScenePartitionerLayer layer = (ScenePartitionerLayer) null;
    switch (this.portInfo.conduitType)
    {
      case ConduitType.Gas:
        layer = GameScenePartitioner.Instance.gasConduitsLayer;
        break;
      case ConduitType.Liquid:
        layer = GameScenePartitioner.Instance.liquidConduitsLayer;
        break;
      case ConduitType.Solid:
        layer = GameScenePartitioner.Instance.solidConduitsLayer;
        break;
    }
    if (layer == null)
      return;
    this.partitionerEntry = GameScenePartitioner.Instance.Add("ElementFilterConduitExists", (object) this.gameObject, this.filteredCell, layer, (System.Action<object>) (data => this.UpdateConduitExistsStatus()));
  }

  protected override void OnCleanUp()
  {
    Conduit.GetNetworkManager(this.portInfo.conduitType).RemoveFromNetworks(this.filteredCell, (object) this.itemFilter, true);
    Conduit.GetFlowManager(this.portInfo.conduitType).RemoveConduitUpdater(new System.Action<float>(this.OnConduitTick));
    if (this.partitionerEntry.IsValid() && (UnityEngine.Object) GameScenePartitioner.Instance != (UnityEngine.Object) null)
      GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
    base.OnCleanUp();
  }

  private void OnConduitTick(float dt)
  {
    bool flag = false;
    this.UpdateConduitBlockedStatus();
    if (this.operational.IsOperational)
    {
      ConduitFlow flowManager = Conduit.GetFlowManager(this.portInfo.conduitType);
      ConduitFlow.ConduitContents contents1 = flowManager.GetContents(this.inputCell);
      int num = contents1.element != this.filteredElem ? this.outputCell : this.filteredCell;
      ConduitFlow.ConduitContents contents2 = flowManager.GetContents(num);
      if ((double) contents1.mass > 0.0 && (double) contents2.mass <= 0.0)
      {
        flag = true;
        float delta = flowManager.AddElement(num, contents1.element, contents1.mass, contents1.temperature, contents1.diseaseIdx, contents1.diseaseCount);
        if ((double) delta > 0.0)
          flowManager.RemoveElement(this.inputCell, delta);
      }
    }
    this.operational.SetActive(flag, false);
  }

  private void UpdateConduitExistsStatus()
  {
    bool flag1 = RequireOutputs.IsConnected(this.filteredCell, this.portInfo.conduitType);
    StatusItem status_item;
    switch (this.portInfo.conduitType)
    {
      case ConduitType.Gas:
        status_item = Db.Get().BuildingStatusItems.NeedGasOut;
        break;
      case ConduitType.Liquid:
        status_item = Db.Get().BuildingStatusItems.NeedLiquidOut;
        break;
      case ConduitType.Solid:
        status_item = Db.Get().BuildingStatusItems.NeedSolidOut;
        break;
      default:
        throw new ArgumentOutOfRangeException();
    }
    bool flag2 = this.needsConduitStatusItemGuid != Guid.Empty;
    if (flag1 != flag2)
      return;
    this.needsConduitStatusItemGuid = this.selectable.ToggleStatusItem(status_item, this.needsConduitStatusItemGuid, !flag1, (object) null);
  }

  private void UpdateConduitBlockedStatus()
  {
    bool flag1 = Conduit.GetFlowManager(this.portInfo.conduitType).IsConduitEmpty(this.filteredCell);
    StatusItem blockedMultiples = Db.Get().BuildingStatusItems.ConduitBlockedMultiples;
    bool flag2 = this.conduitBlockedStatusItemGuid != Guid.Empty;
    if (flag1 != flag2)
      return;
    this.conduitBlockedStatusItemGuid = this.selectable.ToggleStatusItem(blockedMultiples, this.conduitBlockedStatusItemGuid, !flag1, (object) null);
  }

  private void OnFilterChanged(Tag tag)
  {
    bool on = true;
    Element element = ElementLoader.GetElement(tag);
    if (element != null)
    {
      this.filteredElem = element.id;
      on = this.filteredElem == SimHashes.Void || this.filteredElem == SimHashes.Vacuum;
    }
    this.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.NoFilterElementSelected, on, (object) null);
  }

  private void InitializeStatusItems()
  {
    if (ElementFilter.filterStatusItem != null)
      return;
    ElementFilter.filterStatusItem = new StatusItem("Filter", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.LiquidConduits.ID, true, 129022);
    ElementFilter.filterStatusItem.resolveStringCallback = (Func<string, object, string>) ((str, data) =>
    {
      ElementFilter elementFilter = (ElementFilter) data;
      if (elementFilter.filteredElem == SimHashes.Void)
      {
        str = string.Format((string) BUILDINGS.PREFABS.GASFILTER.STATUS_ITEM, (object) BUILDINGS.PREFABS.GASFILTER.ELEMENT_NOT_SPECIFIED);
      }
      else
      {
        Element elementByHash = ElementLoader.FindElementByHash(elementFilter.filteredElem);
        str = string.Format((string) BUILDINGS.PREFABS.GASFILTER.STATUS_ITEM, (object) elementByHash.name);
      }
      return str;
    });
    ElementFilter.filterStatusItem.conditionalOverlayCallback = new Func<HashedString, object, bool>(this.ShowInUtilityOverlay);
  }

  private bool ShowInUtilityOverlay(HashedString mode, object data)
  {
    bool flag = false;
    switch (((ElementFilter) data).portInfo.conduitType)
    {
      case ConduitType.Gas:
        flag = mode == OverlayModes.GasConduits.ID;
        break;
      case ConduitType.Liquid:
        flag = mode == OverlayModes.LiquidConduits.ID;
        break;
    }
    return flag;
  }

  public ConduitType GetSecondaryConduitType()
  {
    return this.portInfo.conduitType;
  }

  public CellOffset GetSecondaryConduitOffset()
  {
    return this.portInfo.offset;
  }

  public int GetFilteredCell()
  {
    return this.filteredCell;
  }
}
