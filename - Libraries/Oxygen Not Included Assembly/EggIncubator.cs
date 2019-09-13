// Decompiled with JetBrains decompiler
// Type: EggIncubator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using KSerialization;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class EggIncubator : SingleEntityReceptacle, ISaveLoadable, ISim1000ms
{
  private static readonly EventSystem.IntraObjectHandler<EggIncubator> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<EggIncubator>((System.Action<EggIncubator, object>) ((component, data) => component.OnOperationalChanged(data)));
  private static readonly EventSystem.IntraObjectHandler<EggIncubator> OnOccupantChangedDelegate = new EventSystem.IntraObjectHandler<EggIncubator>((System.Action<EggIncubator, object>) ((component, data) => component.OnOccupantChanged(data)));
  private static readonly EventSystem.IntraObjectHandler<EggIncubator> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<EggIncubator>((System.Action<EggIncubator, object>) ((component, data) => component.OnStorageChange(data)));
  [MyCmpAdd]
  private EggIncubatorWorkable workable;
  private Chore chore;
  private EggIncubatorStates.Instance smi;
  private KBatchedAnimTracker tracker;
  private MeterController meter;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.autoReplaceEntity = true;
    this.statusItemNeed = Db.Get().BuildingStatusItems.NeedEgg;
    this.statusItemNoneAvailable = Db.Get().BuildingStatusItems.NoAvailableEgg;
    this.statusItemAwaitingDelivery = Db.Get().BuildingStatusItems.AwaitingEggDelivery;
    this.requiredSkillPerk = Db.Get().SkillPerks.CanWrangleCreatures.Id;
    this.occupyingObjectRelativePosition = new Vector3(0.5f, 1f, -1f);
    this.synchronizeAnims = false;
    this.GetComponent<KBatchedAnimController>().SetSymbolVisiblity((KAnimHashedString) "egg_target", false);
    this.meter = new MeterController((KMonoBehaviour) this, Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[0]);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    if ((bool) ((UnityEngine.Object) this.occupyingObject))
    {
      if (this.occupyingObject.HasTag(GameTags.Creature))
        this.storage.allowItemRemoval = true;
      this.storage.RenotifyAll();
      this.PositionOccupyingObject();
    }
    this.Subscribe<EggIncubator>(-592767678, EggIncubator.OnOperationalChangedDelegate);
    this.Subscribe<EggIncubator>(-731304873, EggIncubator.OnOccupantChangedDelegate);
    this.Subscribe<EggIncubator>(-1697596308, EggIncubator.OnStorageChangeDelegate);
    this.smi = new EggIncubatorStates.Instance((IStateMachineTarget) this);
    this.smi.StartSM();
  }

  protected override void OnCleanUp()
  {
    this.smi.StopSM("cleanup");
    base.OnCleanUp();
  }

  protected override void SubscribeToOccupant()
  {
    base.SubscribeToOccupant();
    if ((UnityEngine.Object) this.occupyingObject != (UnityEngine.Object) null)
    {
      this.tracker = this.occupyingObject.AddComponent<KBatchedAnimTracker>();
      this.tracker.symbol = (HashedString) "egg_target";
      this.tracker.forceAlwaysVisible = true;
    }
    this.UpdateProgress();
  }

  protected override void UnsubscribeFromOccupant()
  {
    base.UnsubscribeFromOccupant();
    UnityEngine.Object.Destroy((UnityEngine.Object) this.tracker);
    this.tracker = (KBatchedAnimTracker) null;
    this.UpdateProgress();
  }

  private void OnOperationalChanged(object data = null)
  {
    if ((bool) ((UnityEngine.Object) this.occupyingObject))
      return;
    this.storage.DropAll(false, false, new Vector3(), true);
  }

  private void OnOccupantChanged(object data = null)
  {
    if ((bool) ((UnityEngine.Object) this.occupyingObject))
      return;
    this.storage.allowItemRemoval = false;
  }

  private void OnStorageChange(object data = null)
  {
    if (!(bool) ((UnityEngine.Object) this.occupyingObject) || this.storage.items.Contains(this.occupyingObject))
      return;
    this.UnsubscribeFromOccupant();
    this.ClearOccupant();
  }

  protected override void ClearOccupant()
  {
    bool flag = false;
    if ((UnityEngine.Object) this.occupyingObject != (UnityEngine.Object) null)
      flag = !this.occupyingObject.HasTag(GameTags.Egg);
    base.ClearOccupant();
    if (!this.autoReplaceEntity || !flag || !this.requestedEntityTag.IsValid)
      return;
    this.CreateOrder(this.requestedEntityTag);
  }

  protected override void PositionOccupyingObject()
  {
    base.PositionOccupyingObject();
    this.occupyingObject.GetComponent<KBatchedAnimController>().SetSceneLayer(Grid.SceneLayer.BuildingUse);
    KSelectable component = this.occupyingObject.GetComponent<KSelectable>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.IsSelectable = true;
  }

  public override void OrderRemoveOccupant()
  {
    UnityEngine.Object.Destroy((UnityEngine.Object) this.tracker);
    this.tracker = (KBatchedAnimTracker) null;
    this.storage.DropAll(false, false, new Vector3(), true);
    this.occupyingObject = (GameObject) null;
    this.ClearOccupant();
  }

  public float GetProgress()
  {
    float num = 0.0f;
    if ((bool) ((UnityEngine.Object) this.occupyingObject))
    {
      AmountInstance amountInstance = this.occupyingObject.GetAmounts().Get(Db.Get().Amounts.Incubation);
      num = amountInstance == null ? 1f : amountInstance.value / amountInstance.GetMax();
    }
    return num;
  }

  private void UpdateProgress()
  {
    this.meter.SetPositionPercent(this.GetProgress());
  }

  public void Sim1000ms(float dt)
  {
    this.UpdateProgress();
    this.UpdateChore();
  }

  public void StoreBaby(GameObject baby)
  {
    this.UnsubscribeFromOccupant();
    this.storage.DropAll(false, false, new Vector3(), true);
    this.storage.allowItemRemoval = true;
    this.storage.Store(baby, false, false, true, false);
    this.occupyingObject = baby;
    this.SubscribeToOccupant();
    this.Trigger(-731304873, (object) this.occupyingObject);
  }

  private void UpdateChore()
  {
    if (this.operational.IsOperational && this.EggNeedsAttention())
    {
      if (this.chore != null)
        return;
      this.chore = (Chore) new WorkChore<EggIncubatorWorkable>(Db.Get().ChoreTypes.EggSing, (IStateMachineTarget) this.workable, (ChoreProvider) null, true, (System.Action<Chore>) null, (System.Action<Chore>) null, (System.Action<Chore>) null, true, (ScheduleBlockType) null, false, true, (KAnimFile) null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
    }
    else
    {
      if (this.chore == null)
        return;
      this.chore.Cancel("now is not the time for song");
      this.chore = (Chore) null;
    }
  }

  private bool EggNeedsAttention()
  {
    if (!(bool) ((UnityEngine.Object) this.Occupant))
      return false;
    IncubationMonitor.Instance smi = this.Occupant.GetSMI<IncubationMonitor.Instance>();
    if (smi == null)
      return false;
    return !smi.HasSongBuff();
  }
}
