// Decompiled with JetBrains decompiler
// Type: SolidConduitDispenser
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System.Collections.Generic;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class SolidConduitDispenser : KMonoBehaviour, ISaveLoadable
{
  private static readonly Operational.Flag outputConduitFlag = new Operational.Flag("output_conduit", Operational.Flag.Type.Functional);
  private int utilityCell = -1;
  [SerializeField]
  public SimHashes[] elementFilter;
  [SerializeField]
  public bool invertElementFilter;
  [SerializeField]
  public bool alwaysDispense;
  [MyCmpReq]
  private Operational operational;
  [MyCmpReq]
  public Storage storage;
  private HandleVector<int>.Handle partitionerEntry;
  private bool dispensing;
  private int round_robin_index;
  private const float MaxMass = 20f;

  public SolidConduitFlow.ConduitContents ConduitContents
  {
    get
    {
      return this.GetConduitFlow().GetContents(this.utilityCell);
    }
  }

  public bool IsDispensing
  {
    get
    {
      return this.dispensing;
    }
  }

  public SolidConduitFlow GetConduitFlow()
  {
    return Game.Instance.solidConduitFlow;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.utilityCell = this.GetComponent<Building>().GetUtilityOutputCell();
    this.partitionerEntry = GameScenePartitioner.Instance.Add("SolidConduitConsumer.OnSpawn", (object) this.gameObject, this.utilityCell, GameScenePartitioner.Instance.objectLayers[20], new System.Action<object>(this.OnConduitConnectionChanged));
    this.GetConduitFlow().AddConduitUpdater(new System.Action<float>(this.ConduitUpdate), ConduitFlowPriority.Last);
    this.OnConduitConnectionChanged((object) null);
  }

  protected override void OnCleanUp()
  {
    this.GetConduitFlow().RemoveConduitUpdater(new System.Action<float>(this.ConduitUpdate));
    GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
    base.OnCleanUp();
  }

  private void OnConduitConnectionChanged(object data)
  {
    this.dispensing = this.dispensing && this.IsConnected;
    this.Trigger(-2094018600, (object) this.IsConnected);
  }

  private void ConduitUpdate(float dt)
  {
    bool flag = false;
    this.operational.SetFlag(SolidConduitDispenser.outputConduitFlag, this.IsConnected);
    if (this.operational.IsOperational || this.alwaysDispense)
    {
      SolidConduitFlow conduitFlow = this.GetConduitFlow();
      if (conduitFlow.HasConduit(this.utilityCell) && conduitFlow.IsConduitEmpty(this.utilityCell))
      {
        Pickupable suitableItem = this.FindSuitableItem();
        if ((bool) ((UnityEngine.Object) suitableItem))
        {
          if ((double) suitableItem.PrimaryElement.Mass > 20.0)
            suitableItem = suitableItem.Take(20f);
          conduitFlow.AddPickupable(this.utilityCell, suitableItem);
          flag = true;
        }
      }
    }
    this.storage.storageNetworkID = this.GetConnectedNetworkID();
    this.dispensing = flag;
  }

  private Pickupable FindSuitableItem()
  {
    List<GameObject> items = this.storage.items;
    if (items.Count < 1)
      return (Pickupable) null;
    this.round_robin_index %= items.Count;
    GameObject gameObject = items[this.round_robin_index];
    ++this.round_robin_index;
    return !(bool) ((UnityEngine.Object) gameObject) ? (Pickupable) null : gameObject.GetComponent<Pickupable>();
  }

  public bool IsConnected
  {
    get
    {
      GameObject gameObject = Grid.Objects[this.utilityCell, 20];
      if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
        return (UnityEngine.Object) gameObject.GetComponent<BuildingComplete>() != (UnityEngine.Object) null;
      return false;
    }
  }

  private int GetConnectedNetworkID()
  {
    GameObject gameObject = Grid.Objects[this.utilityCell, 20];
    SolidConduit solidConduit = !((UnityEngine.Object) gameObject != (UnityEngine.Object) null) ? (SolidConduit) null : gameObject.GetComponent<SolidConduit>();
    UtilityNetwork utilityNetwork = !((UnityEngine.Object) solidConduit != (UnityEngine.Object) null) ? (UtilityNetwork) null : solidConduit.GetNetwork();
    if (utilityNetwork != null)
      return utilityNetwork.id;
    return -1;
  }
}
