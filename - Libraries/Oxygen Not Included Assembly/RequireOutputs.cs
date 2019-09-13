// Decompiled with JetBrains decompiler
// Type: RequireOutputs
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

[SkipSaveFileSerialization]
public class RequireOutputs : KMonoBehaviour
{
  private static readonly Operational.Flag outputConnectedFlag = new Operational.Flag("output_connected", Operational.Flag.Type.Requirement);
  private static readonly Operational.Flag pipesHaveRoomFlag = new Operational.Flag("pipesHaveRoom", Operational.Flag.Type.Requirement);
  private bool previouslyConnected = true;
  private bool previouslyHadRoom = true;
  [MyCmpReq]
  private KSelectable selectable;
  [MyCmpReq]
  private Operational operational;
  public bool ignoreFullPipe;
  private int utilityCell;
  private ConduitType conduitType;
  private bool connected;
  private Guid hasPipeGuid;
  private Guid pipeBlockedGuid;
  private HandleVector<int>.Handle partitionerEntry;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    ScenePartitionerLayer layer = (ScenePartitionerLayer) null;
    Building component = this.GetComponent<Building>();
    this.utilityCell = component.GetUtilityOutputCell();
    this.conduitType = component.Def.OutputConduitType;
    switch (component.Def.OutputConduitType)
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
    this.UpdateConnectionState(true);
    this.UpdatePipeRoomState(true);
    if (layer != null)
      this.partitionerEntry = GameScenePartitioner.Instance.Add(nameof (RequireOutputs), (object) this.gameObject, this.utilityCell, layer, (System.Action<object>) (data => this.UpdateConnectionState(false)));
    this.GetConduitFlow().AddConduitUpdater(new System.Action<float>(this.UpdatePipeState), ConduitFlowPriority.First);
  }

  protected override void OnCleanUp()
  {
    GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
    this.GetConduitFlow()?.RemoveConduitUpdater(new System.Action<float>(this.UpdatePipeState));
    base.OnCleanUp();
  }

  private void UpdateConnectionState(bool force_update = false)
  {
    this.connected = this.IsConnected(this.utilityCell);
    if (this.connected == this.previouslyConnected && !force_update)
      return;
    this.operational.SetFlag(RequireOutputs.outputConnectedFlag, this.connected);
    this.previouslyConnected = this.connected;
    StatusItem status_item = (StatusItem) null;
    switch (this.conduitType)
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
    }
    this.hasPipeGuid = this.selectable.ToggleStatusItem(status_item, this.hasPipeGuid, !this.connected, (object) this);
  }

  private bool OutputPipeIsEmpty()
  {
    if (this.ignoreFullPipe)
      return true;
    bool flag = true;
    if (this.connected)
      flag = this.GetConduitFlow().IsConduitEmpty(this.utilityCell);
    return flag;
  }

  private void UpdatePipeState(float dt)
  {
    this.UpdatePipeRoomState(false);
  }

  private void UpdatePipeRoomState(bool force_update = false)
  {
    bool flag = this.OutputPipeIsEmpty();
    if (flag == this.previouslyHadRoom && !force_update)
      return;
    this.operational.SetFlag(RequireOutputs.pipesHaveRoomFlag, flag);
    this.previouslyHadRoom = flag;
    this.pipeBlockedGuid = this.selectable.ToggleStatusItem(Db.Get().BuildingStatusItems.ConduitBlockedMultiples, this.pipeBlockedGuid, !flag, (object) null);
  }

  private IConduitFlow GetConduitFlow()
  {
    switch (this.conduitType)
    {
      case ConduitType.Gas:
        return (IConduitFlow) Game.Instance.gasConduitFlow;
      case ConduitType.Liquid:
        return (IConduitFlow) Game.Instance.liquidConduitFlow;
      case ConduitType.Solid:
        return (IConduitFlow) Game.Instance.solidConduitFlow;
      default:
        Debug.LogWarning((object) ("GetConduitFlow() called with unexpected conduitType: " + this.conduitType.ToString()));
        return (IConduitFlow) null;
    }
  }

  private bool IsConnected(int cell)
  {
    return RequireOutputs.IsConnected(cell, this.conduitType);
  }

  public static bool IsConnected(int cell, ConduitType conduitType)
  {
    ObjectLayer objectLayer = ObjectLayer.NumLayers;
    switch (conduitType)
    {
      case ConduitType.Gas:
        objectLayer = ObjectLayer.GasConduit;
        break;
      case ConduitType.Liquid:
        objectLayer = ObjectLayer.LiquidConduit;
        break;
      case ConduitType.Solid:
        objectLayer = ObjectLayer.SolidConduit;
        break;
    }
    GameObject gameObject = Grid.Objects[cell, (int) objectLayer];
    if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
      return (UnityEngine.Object) gameObject.GetComponent<BuildingComplete>() != (UnityEngine.Object) null;
    return false;
  }
}
