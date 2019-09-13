// Decompiled with JetBrains decompiler
// Type: ConduitPreferentialFlow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class ConduitPreferentialFlow : KMonoBehaviour, ISecondaryInput
{
  [SerializeField]
  public ConduitPortInfo portInfo;
  private int inputCell;
  private int outputCell;
  private FlowUtilityNetwork.NetworkItem secondaryInput;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    Building component = this.GetComponent<Building>();
    this.inputCell = component.GetUtilityInputCell();
    this.outputCell = component.GetUtilityOutputCell();
    int cell = Grid.OffsetCell(Grid.PosToCell(this.transform.GetPosition()), component.GetRotatedOffset(this.portInfo.offset));
    Conduit.GetFlowManager(this.portInfo.conduitType).AddConduitUpdater(new System.Action<float>(this.ConduitUpdate), ConduitFlowPriority.Default);
    IUtilityNetworkMgr networkManager = Conduit.GetNetworkManager(this.portInfo.conduitType);
    this.secondaryInput = new FlowUtilityNetwork.NetworkItem(this.portInfo.conduitType, Endpoint.Sink, cell, this.gameObject);
    networkManager.AddToNetworks(this.secondaryInput.Cell, (object) this.secondaryInput, true);
  }

  protected override void OnCleanUp()
  {
    Conduit.GetNetworkManager(this.portInfo.conduitType).RemoveFromNetworks(this.secondaryInput.Cell, (object) this.secondaryInput, true);
    Conduit.GetFlowManager(this.portInfo.conduitType).RemoveConduitUpdater(new System.Action<float>(this.ConduitUpdate));
    base.OnCleanUp();
  }

  private void ConduitUpdate(float dt)
  {
    ConduitFlow flowManager = Conduit.GetFlowManager(this.portInfo.conduitType);
    if (!flowManager.HasConduit(this.outputCell))
      return;
    int cell = this.inputCell;
    ConduitFlow.ConduitContents contents = flowManager.GetContents(cell);
    if ((double) contents.mass <= 0.0)
    {
      cell = this.secondaryInput.Cell;
      contents = flowManager.GetContents(cell);
    }
    if ((double) contents.mass <= 0.0)
      return;
    float delta = flowManager.AddElement(this.outputCell, contents.element, contents.mass, contents.temperature, contents.diseaseIdx, contents.diseaseCount);
    if ((double) delta <= 0.0)
      return;
    flowManager.RemoveElement(cell, delta);
  }

  public ConduitType GetSecondaryConduitType()
  {
    return this.portInfo.conduitType;
  }

  public CellOffset GetSecondaryConduitOffset()
  {
    return this.portInfo.offset;
  }
}
