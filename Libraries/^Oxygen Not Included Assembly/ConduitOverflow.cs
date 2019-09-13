// Decompiled with JetBrains decompiler
// Type: ConduitOverflow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class ConduitOverflow : KMonoBehaviour, ISecondaryOutput
{
  [SerializeField]
  public ConduitPortInfo portInfo;
  private int inputCell;
  private int outputCell;
  private FlowUtilityNetwork.NetworkItem secondaryOutput;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    Building component = this.GetComponent<Building>();
    this.inputCell = component.GetUtilityInputCell();
    this.outputCell = component.GetUtilityOutputCell();
    int cell = Grid.OffsetCell(Grid.PosToCell(this.transform.GetPosition()), component.GetRotatedOffset(this.portInfo.offset));
    Conduit.GetFlowManager(this.portInfo.conduitType).AddConduitUpdater(new System.Action<float>(this.ConduitUpdate), ConduitFlowPriority.Default);
    IUtilityNetworkMgr networkManager = Conduit.GetNetworkManager(this.portInfo.conduitType);
    this.secondaryOutput = new FlowUtilityNetwork.NetworkItem(this.portInfo.conduitType, Endpoint.Sink, cell, this.gameObject);
    networkManager.AddToNetworks(this.secondaryOutput.Cell, (object) this.secondaryOutput, true);
  }

  protected override void OnCleanUp()
  {
    Conduit.GetNetworkManager(this.portInfo.conduitType).RemoveFromNetworks(this.secondaryOutput.Cell, (object) this.secondaryOutput, true);
    Conduit.GetFlowManager(this.portInfo.conduitType).RemoveConduitUpdater(new System.Action<float>(this.ConduitUpdate));
    base.OnCleanUp();
  }

  private void ConduitUpdate(float dt)
  {
    ConduitFlow flowManager = Conduit.GetFlowManager(this.portInfo.conduitType);
    if (!flowManager.HasConduit(this.inputCell))
      return;
    ConduitFlow.ConduitContents contents1 = flowManager.GetContents(this.inputCell);
    if ((double) contents1.mass <= 0.0)
      return;
    int num = this.outputCell;
    ConduitFlow.ConduitContents contents2 = flowManager.GetContents(num);
    if ((double) contents2.mass > 0.0)
    {
      num = this.secondaryOutput.Cell;
      contents2 = flowManager.GetContents(num);
    }
    if ((double) contents2.mass > 0.0)
      return;
    float delta = flowManager.AddElement(num, contents1.element, contents1.mass, contents1.temperature, contents1.diseaseIdx, contents1.diseaseCount);
    if ((double) delta <= 0.0)
      return;
    flowManager.RemoveElement(this.inputCell, delta);
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
