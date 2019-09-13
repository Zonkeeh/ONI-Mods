// Decompiled with JetBrains decompiler
// Type: SolidConduitConsumer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[SkipSaveFileSerialization]
public class SolidConduitConsumer : KMonoBehaviour
{
  [SerializeField]
  public Tag capacityTag = GameTags.Any;
  [SerializeField]
  public float capacityKG = float.PositiveInfinity;
  private int utilityCell = -1;
  [SerializeField]
  public bool alwaysConsume;
  [MyCmpReq]
  private Operational operational;
  [MyCmpReq]
  private Building building;
  [MyCmpGet]
  public Storage storage;
  private HandleVector<int>.Handle partitionerEntry;
  private bool consuming;

  public bool IsConsuming
  {
    get
    {
      return this.consuming;
    }
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

  private SolidConduitFlow GetConduitFlow()
  {
    return Game.Instance.solidConduitFlow;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.utilityCell = this.building.GetUtilityInputCell();
    this.partitionerEntry = GameScenePartitioner.Instance.Add("SolidConduitConsumer.OnSpawn", (object) this.gameObject, this.utilityCell, GameScenePartitioner.Instance.objectLayers[20], new System.Action<object>(this.OnConduitConnectionChanged));
    this.GetConduitFlow().AddConduitUpdater(new System.Action<float>(this.ConduitUpdate), ConduitFlowPriority.Default);
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
    this.consuming = this.consuming && this.IsConnected;
    this.Trigger(-2094018600, (object) this.IsConnected);
  }

  private void ConduitUpdate(float dt)
  {
    bool flag = false;
    SolidConduitFlow conduitFlow = this.GetConduitFlow();
    if (this.IsConnected)
    {
      SolidConduitFlow.ConduitContents contents = conduitFlow.GetContents(this.utilityCell);
      if (contents.pickupableHandle.IsValid() && (this.alwaysConsume || this.operational.IsOperational))
      {
        float num1 = !(this.capacityTag != GameTags.Any) ? this.storage.MassStored() : this.storage.GetMassAvailable(this.capacityTag);
        float num2 = Mathf.Min(this.storage.capacityKg, this.capacityKG);
        float num3 = Mathf.Max(0.0f, num2 - num1);
        if ((double) num3 > 0.0)
        {
          Pickupable pickupable1 = conduitFlow.GetPickupable(contents.pickupableHandle);
          if ((double) pickupable1.PrimaryElement.Mass <= (double) num3 || (double) pickupable1.PrimaryElement.Mass > (double) num2)
          {
            Pickupable pickupable2 = conduitFlow.RemovePickupable(this.utilityCell);
            if ((bool) ((UnityEngine.Object) pickupable2))
            {
              this.storage.Store(pickupable2.gameObject, true, false, true, false);
              flag = true;
            }
          }
        }
      }
    }
    this.storage.storageNetworkID = this.GetConnectedNetworkID();
    this.consuming = flag;
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
