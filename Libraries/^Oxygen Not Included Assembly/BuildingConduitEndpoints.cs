// Decompiled with JetBrains decompiler
// Type: BuildingConduitEndpoints
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class BuildingConduitEndpoints : KMonoBehaviour
{
  private FlowUtilityNetwork.NetworkItem itemInput;
  private FlowUtilityNetwork.NetworkItem itemOutput;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    Building component = this.GetComponent<Building>();
    BuildingDef def = component.Def;
    if (def.InputConduitType != ConduitType.None)
    {
      int utilityInputCell = component.GetUtilityInputCell();
      this.itemInput = new FlowUtilityNetwork.NetworkItem(def.InputConduitType, Endpoint.Sink, utilityInputCell, this.gameObject);
      if (def.InputConduitType == ConduitType.Solid)
        Game.Instance.solidConduitSystem.AddToNetworks(utilityInputCell, (object) this.itemInput, true);
      else
        Conduit.GetNetworkManager(def.InputConduitType).AddToNetworks(utilityInputCell, (object) this.itemInput, true);
    }
    if (def.OutputConduitType == ConduitType.None)
      return;
    int utilityOutputCell = component.GetUtilityOutputCell();
    this.itemOutput = new FlowUtilityNetwork.NetworkItem(def.OutputConduitType, Endpoint.Source, utilityOutputCell, this.gameObject);
    if (def.OutputConduitType == ConduitType.Solid)
      Game.Instance.solidConduitSystem.AddToNetworks(utilityOutputCell, (object) this.itemOutput, true);
    else
      Conduit.GetNetworkManager(def.OutputConduitType).AddToNetworks(utilityOutputCell, (object) this.itemOutput, true);
  }

  protected override void OnCleanUp()
  {
    if (this.itemInput != null)
    {
      if (this.itemInput.ConduitType == ConduitType.Solid)
        Game.Instance.solidConduitSystem.RemoveFromNetworks(this.itemInput.Cell, (object) this.itemInput, true);
      else
        Conduit.GetNetworkManager(this.itemInput.ConduitType).RemoveFromNetworks(this.itemInput.Cell, (object) this.itemInput, true);
    }
    if (this.itemOutput != null)
    {
      if (this.itemOutput.ConduitType == ConduitType.Solid)
        Game.Instance.solidConduitSystem.RemoveFromNetworks(this.itemOutput.Cell, (object) this.itemOutput, true);
      else
        Conduit.GetNetworkManager(this.itemOutput.ConduitType).RemoveFromNetworks(this.itemOutput.Cell, (object) this.itemOutput, true);
    }
    base.OnCleanUp();
  }
}
