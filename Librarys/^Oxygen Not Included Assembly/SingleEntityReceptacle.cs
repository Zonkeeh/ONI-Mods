// Decompiled with JetBrains decompiler
// Type: SingleEntityReceptacle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System.Collections.Generic;
using UnityEngine;

public class SingleEntityReceptacle : Workable, IRender1000ms
{
  private static readonly EventSystem.IntraObjectHandler<SingleEntityReceptacle> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<SingleEntityReceptacle>((System.Action<SingleEntityReceptacle, object>) ((component, data) => component.OnOperationalChanged(data)));
  [Serialize]
  private Ref<KSelectable> occupyObjectRef = new Ref<KSelectable>();
  [SerializeField]
  private List<Tag> possibleDepositTagsList = new List<Tag>();
  public Vector3 occupyingObjectRelativePosition = new Vector3(0.0f, 1f, 3f);
  [MyCmpReq]
  protected Operational operational;
  [MyCmpReq]
  protected Storage storage;
  [MyCmpGet]
  public Rotatable rotatable;
  protected FetchChore fetchChore;
  [Serialize]
  public bool autoReplaceEntity;
  [Serialize]
  public Tag requestedEntityTag;
  [SerializeField]
  protected bool destroyEntityOnDeposit;
  [SerializeField]
  protected SingleEntityReceptacle.ReceptacleDirection direction;
  protected StatusItem statusItemAwaitingDelivery;
  protected StatusItem statusItemNeed;
  protected StatusItem statusItemNoneAvailable;

  public FetchChore GetActiveRequest
  {
    get
    {
      return this.fetchChore;
    }
  }

  protected GameObject occupyingObject
  {
    get
    {
      if ((UnityEngine.Object) this.occupyObjectRef.Get() != (UnityEngine.Object) null)
        return this.occupyObjectRef.Get().gameObject;
      return (GameObject) null;
    }
    set
    {
      if ((UnityEngine.Object) value == (UnityEngine.Object) null)
        this.occupyObjectRef.Set((KSelectable) null);
      else
        this.occupyObjectRef.Set(value.GetComponent<KSelectable>());
    }
  }

  public GameObject Occupant
  {
    get
    {
      return this.occupyingObject;
    }
  }

  public Tag[] possibleDepositObjectTags
  {
    get
    {
      return this.possibleDepositTagsList.ToArray();
    }
  }

  public SingleEntityReceptacle.ReceptacleDirection Direction
  {
    get
    {
      return this.direction;
    }
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    if ((UnityEngine.Object) this.occupyingObject != (UnityEngine.Object) null)
    {
      this.PositionOccupyingObject();
      this.SubscribeToOccupant();
    }
    this.UpdateStatusItem();
    if ((UnityEngine.Object) this.occupyingObject == (UnityEngine.Object) null && this.requestedEntityTag.IsValid)
      this.CreateOrder(this.requestedEntityTag);
    this.Subscribe<SingleEntityReceptacle>(-592767678, SingleEntityReceptacle.OnOperationalChangedDelegate);
  }

  public void AddDepositTag(Tag t)
  {
    this.possibleDepositTagsList.Add(t);
  }

  public void SetReceptacleDirection(SingleEntityReceptacle.ReceptacleDirection d)
  {
    this.direction = d;
  }

  public virtual void SetPreview(Tag entityTag, bool solid = false)
  {
  }

  public virtual void CreateOrder(Tag entityTag)
  {
    this.requestedEntityTag = entityTag;
    this.CreateFetchChore(this.requestedEntityTag);
    this.SetPreview(entityTag, true);
    this.UpdateStatusItem();
  }

  public void Render1000ms(float dt)
  {
    this.UpdateStatusItem();
  }

  protected void UpdateStatusItem()
  {
    KSelectable component = this.GetComponent<KSelectable>();
    if ((UnityEngine.Object) this.Occupant != (UnityEngine.Object) null)
      component.SetStatusItem(Db.Get().StatusItemCategories.EntityReceptacle, (StatusItem) null, (object) null);
    else if (this.fetchChore != null)
    {
      bool flag = (UnityEngine.Object) this.fetchChore.fetcher != (UnityEngine.Object) null;
      if (!flag)
      {
        foreach (Tag tag in this.fetchChore.tags)
        {
          if ((double) WorldInventory.Instance.GetTotalAmount(tag) > 0.0)
          {
            flag = true;
            break;
          }
        }
      }
      if (flag)
        component.SetStatusItem(Db.Get().StatusItemCategories.EntityReceptacle, this.statusItemAwaitingDelivery, (object) null);
      else
        component.SetStatusItem(Db.Get().StatusItemCategories.EntityReceptacle, this.statusItemNoneAvailable, (object) null);
    }
    else
      component.SetStatusItem(Db.Get().StatusItemCategories.EntityReceptacle, this.statusItemNeed, (object) null);
  }

  protected void CreateFetchChore(Tag entityTag)
  {
    if (this.fetchChore != null || !entityTag.IsValid || !(entityTag != GameTags.Empty))
      return;
    this.fetchChore = new FetchChore(Db.Get().ChoreTypes.FarmFetch, this.storage, 1f, new Tag[1]
    {
      entityTag
    }, (Tag[]) null, (Tag[]) null, (ChoreProvider) null, true, new System.Action<Chore>(this.OnFetchComplete), (System.Action<Chore>) (chore => this.UpdateStatusItem()), (System.Action<Chore>) (chore => this.UpdateStatusItem()), FetchOrder2.OperationalRequirement.Functional, 0);
    MaterialNeeds.Instance.UpdateNeed(this.requestedEntityTag, 1f);
    this.UpdateStatusItem();
  }

  public virtual void OrderRemoveOccupant()
  {
    this.ClearOccupant();
  }

  protected virtual void ClearOccupant()
  {
    if ((bool) ((UnityEngine.Object) this.occupyingObject))
      this.storage.DropAll(false, false, new Vector3(), true);
    this.occupyingObject = (GameObject) null;
    this.UpdateActive();
    this.UpdateStatusItem();
    this.Trigger(-731304873, (object) this.occupyingObject);
  }

  public void CancelActiveRequest()
  {
    if (this.fetchChore != null)
    {
      MaterialNeeds.Instance.UpdateNeed(this.requestedEntityTag, -1f);
      this.fetchChore.Cancel("User canceled");
      this.fetchChore = (FetchChore) null;
    }
    this.requestedEntityTag = Tag.Invalid;
    this.UpdateStatusItem();
    this.SetPreview(Tag.Invalid, false);
  }

  private void ClearOccupantEventHandler(object data)
  {
    this.ClearOccupant();
    if (!this.autoReplaceEntity || !this.requestedEntityTag.IsValid || !(this.requestedEntityTag != GameTags.Empty))
      return;
    this.CreateOrder(this.requestedEntityTag);
  }

  protected virtual void SubscribeToOccupant()
  {
    if (!((UnityEngine.Object) this.occupyingObject != (UnityEngine.Object) null))
      return;
    this.Subscribe(this.occupyingObject, 1969584890, new System.Action<object>(this.ClearOccupantEventHandler));
  }

  protected virtual void UnsubscribeFromOccupant()
  {
    if (!((UnityEngine.Object) this.occupyingObject != (UnityEngine.Object) null))
      return;
    this.Unsubscribe(this.occupyingObject, 1969584890, new System.Action<object>(this.ClearOccupantEventHandler));
  }

  private void OnFetchComplete(Chore chore)
  {
    if (this.fetchChore == null)
      Debug.LogWarningFormat((UnityEngine.Object) this.gameObject, "{0} OnFetchComplete fetchChore null", (object) this.gameObject);
    else if ((UnityEngine.Object) this.fetchChore.fetchTarget == (UnityEngine.Object) null)
      Debug.LogWarningFormat((UnityEngine.Object) this.gameObject, "{0} OnFetchComplete fetchChore.fetchTarget null", (object) this.gameObject);
    else
      this.OnDepositObject(this.fetchChore.fetchTarget.GetComponent<Pickupable>());
  }

  public void ForceDepositPickupable(Pickupable pickupable)
  {
    this.OnDepositObject(pickupable);
  }

  private void OnDepositObject(Pickupable pickupable)
  {
    this.SetPreview(Tag.Invalid, false);
    MaterialNeeds.Instance.UpdateNeed(this.requestedEntityTag, -1f);
    KBatchedAnimController component = pickupable.GetComponent<KBatchedAnimController>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      component.GetBatchInstanceData().ClearOverrideTransformMatrix();
    this.occupyingObject = this.SpawnOccupyingObject(pickupable.gameObject);
    if ((UnityEngine.Object) this.occupyingObject != (UnityEngine.Object) null)
    {
      this.occupyingObject.SetActive(true);
      this.PositionOccupyingObject();
      this.SubscribeToOccupant();
    }
    else
      Debug.LogWarning((object) (this.gameObject.name + " EntityReceptacle did not spawn occupying entity."));
    if (this.fetchChore != null)
    {
      this.fetchChore.Cancel("receptacle filled");
      this.fetchChore = (FetchChore) null;
    }
    if (!this.autoReplaceEntity)
      this.requestedEntityTag = Tag.Invalid;
    this.UpdateActive();
    this.UpdateStatusItem();
    if (this.destroyEntityOnDeposit)
      Util.KDestroyGameObject(pickupable.gameObject);
    this.Trigger(-731304873, (object) this.occupyingObject);
  }

  public virtual GameObject SpawnOccupyingObject(GameObject depositedEntity)
  {
    return depositedEntity;
  }

  protected virtual void PositionOccupyingObject()
  {
    if ((UnityEngine.Object) this.rotatable != (UnityEngine.Object) null)
      this.occupyingObject.transform.SetPosition(this.gameObject.transform.GetPosition() + this.rotatable.GetRotatedOffset(this.occupyingObjectRelativePosition));
    else
      this.occupyingObject.transform.SetPosition(this.gameObject.transform.GetPosition() + this.occupyingObjectRelativePosition);
  }

  private void UpdateActive()
  {
    if (this.Equals((object) null) || (UnityEngine.Object) this == (UnityEngine.Object) null || (this.gameObject.Equals((object) null) || (UnityEngine.Object) this.gameObject == (UnityEngine.Object) null))
      return;
    this.operational.SetActive(this.operational.IsOperational && (UnityEngine.Object) this.occupyingObject != (UnityEngine.Object) null, false);
  }

  protected override void OnCleanUp()
  {
    this.CancelActiveRequest();
    this.UnsubscribeFromOccupant();
    base.OnCleanUp();
  }

  private void OnOperationalChanged(object data)
  {
    this.UpdateActive();
    if (!(bool) ((UnityEngine.Object) this.occupyingObject))
      return;
    this.occupyingObject.Trigger(!this.operational.IsOperational ? 960378201 : 1628751838, (object) null);
  }

  public enum ReceptacleDirection
  {
    Top,
    Side,
    Bottom,
  }
}
