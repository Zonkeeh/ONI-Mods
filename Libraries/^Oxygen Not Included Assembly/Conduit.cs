// Decompiled with JetBrains decompiler
// Type: Conduit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[SkipSaveFileSerialization]
public class Conduit : KMonoBehaviour, IFirstFrameCallback, IHaveUtilityNetworkMgr, IBridgedNetworkItem, IDisconnectable, FlowUtilityNetwork.IItem
{
  private static readonly EventSystem.IntraObjectHandler<Conduit> OnHighlightedDelegate = new EventSystem.IntraObjectHandler<Conduit>((System.Action<Conduit, object>) ((component, data) => component.OnHighlighted(data)));
  private static readonly EventSystem.IntraObjectHandler<Conduit> OnConduitFrozenDelegate = new EventSystem.IntraObjectHandler<Conduit>((System.Action<Conduit, object>) ((component, data) => component.OnConduitFrozen(data)));
  private static readonly EventSystem.IntraObjectHandler<Conduit> OnConduitBoilingDelegate = new EventSystem.IntraObjectHandler<Conduit>((System.Action<Conduit, object>) ((component, data) => component.OnConduitBoiling(data)));
  private static readonly EventSystem.IntraObjectHandler<Conduit> OnStructureTemperatureRegisteredDelegate = new EventSystem.IntraObjectHandler<Conduit>((System.Action<Conduit, object>) ((component, data) => component.OnStructureTemperatureRegistered(data)));
  private static readonly EventSystem.IntraObjectHandler<Conduit> OnBuildingBrokenDelegate = new EventSystem.IntraObjectHandler<Conduit>((System.Action<Conduit, object>) ((component, data) => component.OnBuildingBroken(data)));
  private static readonly EventSystem.IntraObjectHandler<Conduit> OnBuildingFullyRepairedDelegate = new EventSystem.IntraObjectHandler<Conduit>((System.Action<Conduit, object>) ((component, data) => component.OnBuildingFullyRepaired(data)));
  [SerializeField]
  private bool disconnected = true;
  [MyCmpReq]
  private KAnimGraphTileVisualizer graphTileDependency;
  public ConduitType type;
  private System.Action firstFrameCallback;

  public void SetFirstFrameCallback(System.Action ffCb)
  {
    this.firstFrameCallback = ffCb;
    this.StartCoroutine(this.RunCallback());
  }

  [DebuggerHidden]
  private IEnumerator RunCallback()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new Conduit.\u003CRunCallback\u003Ec__Iterator0()
    {
      \u0024this = this
    };
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<Conduit>(-1201923725, Conduit.OnHighlightedDelegate);
    this.Subscribe<Conduit>(-700727624, Conduit.OnConduitFrozenDelegate);
    this.Subscribe<Conduit>(-1152799878, Conduit.OnConduitBoilingDelegate);
    this.Subscribe<Conduit>(-1555603773, Conduit.OnStructureTemperatureRegisteredDelegate);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Subscribe<Conduit>(774203113, Conduit.OnBuildingBrokenDelegate);
    this.Subscribe<Conduit>(-1735440190, Conduit.OnBuildingFullyRepairedDelegate);
  }

  private void OnStructureTemperatureRegistered(object data)
  {
    this.GetNetworkManager().AddToNetworks(Grid.PosToCell((KMonoBehaviour) this), (object) this, false);
    this.Connect();
    this.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.Pipe, (object) this);
    BuildingDef def = this.GetComponent<Building>().Def;
    if (!((UnityEngine.Object) def != (UnityEngine.Object) null) || (double) def.ThermalConductivity == 1.0)
      return;
    this.GetFlowVisualizer().AddThermalConductivity(Grid.PosToCell(this.transform.GetPosition()), def.ThermalConductivity);
  }

  protected override void OnCleanUp()
  {
    this.Unsubscribe<Conduit>(774203113, Conduit.OnBuildingBrokenDelegate, false);
    this.Unsubscribe<Conduit>(-1735440190, Conduit.OnBuildingFullyRepairedDelegate, false);
    BuildingDef def = this.GetComponent<Building>().Def;
    if ((UnityEngine.Object) def != (UnityEngine.Object) null && (double) def.ThermalConductivity != 1.0)
      this.GetFlowVisualizer().RemoveThermalConductivity(Grid.PosToCell(this.transform.GetPosition()), def.ThermalConductivity);
    int cell = Grid.PosToCell(this.transform.GetPosition());
    this.GetNetworkManager().RemoveFromNetworks(cell, (object) this, false);
    BuildingComplete component = this.GetComponent<BuildingComplete>();
    if (component.Def.ReplacementLayer == ObjectLayer.NumLayers || (UnityEngine.Object) Grid.Objects[cell, (int) component.Def.ReplacementLayer] == (UnityEngine.Object) null)
    {
      this.GetNetworkManager().RemoveFromNetworks(cell, (object) this, false);
      this.GetFlowManager().EmptyConduit(Grid.PosToCell(this.transform.GetPosition()));
    }
    base.OnCleanUp();
  }

  private ConduitFlowVisualizer GetFlowVisualizer()
  {
    return this.type != ConduitType.Gas ? Game.Instance.liquidFlowVisualizer : Game.Instance.gasFlowVisualizer;
  }

  public IUtilityNetworkMgr GetNetworkManager()
  {
    return this.type != ConduitType.Gas ? (IUtilityNetworkMgr) Game.Instance.liquidConduitSystem : (IUtilityNetworkMgr) Game.Instance.gasConduitSystem;
  }

  public ConduitFlow GetFlowManager()
  {
    return this.type != ConduitType.Gas ? Game.Instance.liquidConduitFlow : Game.Instance.gasConduitFlow;
  }

  public static ConduitFlow GetFlowManager(ConduitType type)
  {
    return type != ConduitType.Gas ? Game.Instance.liquidConduitFlow : Game.Instance.gasConduitFlow;
  }

  public static IUtilityNetworkMgr GetNetworkManager(ConduitType type)
  {
    return type != ConduitType.Gas ? (IUtilityNetworkMgr) Game.Instance.liquidConduitSystem : (IUtilityNetworkMgr) Game.Instance.gasConduitSystem;
  }

  public void AddNetworks(ICollection<UtilityNetwork> networks)
  {
    UtilityNetwork networkForCell = this.GetNetworkManager().GetNetworkForCell(Grid.PosToCell((KMonoBehaviour) this));
    if (networkForCell == null)
      return;
    networks.Add(networkForCell);
  }

  public bool IsConnectedToNetworks(ICollection<UtilityNetwork> networks)
  {
    UtilityNetwork networkForCell = this.GetNetworkManager().GetNetworkForCell(Grid.PosToCell((KMonoBehaviour) this));
    return networks.Contains(networkForCell);
  }

  public int GetNetworkCell()
  {
    return Grid.PosToCell((KMonoBehaviour) this);
  }

  private void OnHighlighted(object data)
  {
    this.GetFlowVisualizer().SetHighlightedCell(!(bool) data ? -1 : Grid.PosToCell(this.transform.GetPosition()));
  }

  private void OnConduitFrozen(object data)
  {
    this.Trigger(-794517298, (object) new BuildingHP.DamageSourceInfo()
    {
      damage = 1,
      source = (string) BUILDINGS.DAMAGESOURCES.CONDUIT_CONTENTS_FROZE,
      popString = (string) UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.CONDUIT_CONTENTS_FROZE,
      takeDamageEffect = (this.ConduitType != ConduitType.Gas ? SpawnFXHashes.BuildingFreeze : SpawnFXHashes.BuildingLeakLiquid),
      fullDamageEffectName = (this.ConduitType != ConduitType.Gas ? "ice_damage_kanim" : "water_damage_kanim")
    });
    this.GetFlowManager().EmptyConduit(Grid.PosToCell(this.transform.GetPosition()));
  }

  private void OnConduitBoiling(object data)
  {
    this.Trigger(-794517298, (object) new BuildingHP.DamageSourceInfo()
    {
      damage = 1,
      source = (string) BUILDINGS.DAMAGESOURCES.CONDUIT_CONTENTS_BOILED,
      popString = (string) UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.CONDUIT_CONTENTS_BOILED,
      takeDamageEffect = SpawnFXHashes.BuildingLeakGas,
      fullDamageEffectName = "gas_damage_kanim"
    });
    this.GetFlowManager().EmptyConduit(Grid.PosToCell(this.transform.GetPosition()));
  }

  private void OnBuildingBroken(object data)
  {
    this.Disconnect();
  }

  private void OnBuildingFullyRepaired(object data)
  {
    this.Connect();
  }

  public bool IsDisconnected()
  {
    return this.disconnected;
  }

  public bool Connect()
  {
    BuildingHP component = this.GetComponent<BuildingHP>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null || component.HitPoints > 0)
    {
      this.disconnected = false;
      this.GetNetworkManager().ForceRebuildNetworks();
    }
    return !this.disconnected;
  }

  public void Disconnect()
  {
    this.disconnected = true;
    this.GetNetworkManager().ForceRebuildNetworks();
  }

  public FlowUtilityNetwork Network
  {
    set
    {
    }
  }

  public int Cell
  {
    get
    {
      return Grid.PosToCell((KMonoBehaviour) this);
    }
  }

  public Endpoint EndpointType
  {
    get
    {
      return Endpoint.Conduit;
    }
  }

  public ConduitType ConduitType
  {
    get
    {
      return this.type;
    }
  }

  public GameObject GameObject
  {
    get
    {
      return this.gameObject;
    }
  }
}
