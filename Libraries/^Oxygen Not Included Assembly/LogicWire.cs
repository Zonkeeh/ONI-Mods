// Decompiled with JetBrains decompiler
// Type: LogicWire
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[SkipSaveFileSerialization]
public class LogicWire : KMonoBehaviour, IFirstFrameCallback, IHaveUtilityNetworkMgr, IBridgedNetworkItem
{
  public static readonly KAnimHashedString OutlineSymbol = new KAnimHashedString("outline");
  private static readonly EventSystem.IntraObjectHandler<LogicWire> OnBuildingBrokenDelegate = new EventSystem.IntraObjectHandler<LogicWire>((System.Action<LogicWire, object>) ((component, data) => component.OnBuildingBroken(data)));
  private static readonly EventSystem.IntraObjectHandler<LogicWire> OnBuildingFullyRepairedDelegate = new EventSystem.IntraObjectHandler<LogicWire>((System.Action<LogicWire, object>) ((component, data) => component.OnBuildingFullyRepaired(data)));
  [SerializeField]
  private bool disconnected = true;
  private System.Action firstFrameCallback;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    Game.Instance.logicCircuitSystem.AddToNetworks(Grid.PosToCell(this.transform.GetPosition()), (object) this, false);
    this.Subscribe<LogicWire>(774203113, LogicWire.OnBuildingBrokenDelegate);
    this.Subscribe<LogicWire>(-1735440190, LogicWire.OnBuildingFullyRepairedDelegate);
    this.GetComponent<KBatchedAnimController>().SetSymbolVisiblity(LogicWire.OutlineSymbol, false);
  }

  protected override void OnCleanUp()
  {
    int cell = Grid.PosToCell(this.transform.GetPosition());
    BuildingComplete component = this.GetComponent<BuildingComplete>();
    if (component.Def.ReplacementLayer == ObjectLayer.NumLayers || (UnityEngine.Object) Grid.Objects[cell, (int) component.Def.ReplacementLayer] == (UnityEngine.Object) null)
      Game.Instance.logicCircuitSystem.RemoveFromNetworks(cell, (object) this, false);
    this.Unsubscribe<LogicWire>(774203113, LogicWire.OnBuildingBrokenDelegate, false);
    this.Unsubscribe<LogicWire>(-1735440190, LogicWire.OnBuildingFullyRepairedDelegate, false);
    base.OnCleanUp();
  }

  public bool IsConnected
  {
    get
    {
      return Game.Instance.logicCircuitSystem.GetNetworkForCell(Grid.PosToCell(this.transform.GetPosition())) is LogicCircuitNetwork;
    }
  }

  public bool Connect()
  {
    BuildingHP component = this.GetComponent<BuildingHP>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null || component.HitPoints > 0)
    {
      this.disconnected = false;
      Game.Instance.logicCircuitSystem.ForceRebuildNetworks();
    }
    return !this.disconnected;
  }

  public void Disconnect()
  {
    this.disconnected = true;
    this.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, Db.Get().BuildingStatusItems.WireDisconnected, (object) null);
    Game.Instance.logicCircuitSystem.ForceRebuildNetworks();
  }

  public UtilityConnections GetWireConnections()
  {
    return Game.Instance.logicCircuitSystem.GetConnections(Grid.PosToCell(this.transform.GetPosition()), true);
  }

  public string GetWireConnectionsString()
  {
    return Game.Instance.logicCircuitSystem.GetVisualizerString(this.GetWireConnections());
  }

  private void OnBuildingBroken(object data)
  {
    this.Disconnect();
  }

  private void OnBuildingFullyRepaired(object data)
  {
    this.Connect();
  }

  public void SetFirstFrameCallback(System.Action ffCb)
  {
    this.firstFrameCallback = ffCb;
    this.StartCoroutine(this.RunCallback());
  }

  [DebuggerHidden]
  private IEnumerator RunCallback()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new LogicWire.\u003CRunCallback\u003Ec__Iterator0()
    {
      \u0024this = this
    };
  }

  public IUtilityNetworkMgr GetNetworkManager()
  {
    return (IUtilityNetworkMgr) Game.Instance.logicCircuitSystem;
  }

  public void AddNetworks(ICollection<UtilityNetwork> networks)
  {
    UtilityNetwork networkForCell = Game.Instance.logicCircuitSystem.GetNetworkForCell(Grid.PosToCell(this.transform.GetPosition()));
    if (networkForCell == null)
      return;
    networks.Add(networkForCell);
  }

  public bool IsConnectedToNetworks(ICollection<UtilityNetwork> networks)
  {
    UtilityNetwork networkForCell = Game.Instance.logicCircuitSystem.GetNetworkForCell(Grid.PosToCell(this.transform.GetPosition()));
    return networks.Contains(networkForCell);
  }

  public int GetNetworkCell()
  {
    return Grid.PosToCell((KMonoBehaviour) this);
  }
}
