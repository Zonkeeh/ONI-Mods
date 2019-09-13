// Decompiled with JetBrains decompiler
// Type: MinionAssignablesProxy
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class MinionAssignablesProxy : KMonoBehaviour, IAssignableIdentity
{
  private static readonly EventSystem.IntraObjectHandler<MinionAssignablesProxy> OnAssignablesChangedDelegate = new EventSystem.IntraObjectHandler<MinionAssignablesProxy>((System.Action<MinionAssignablesProxy, object>) ((component, data) => component.OnAssignablesChanged(data)));
  [Serialize]
  private int target_instance_id = -1;
  public List<Ownables> ownables;
  private bool slotsConfigured;

  public IAssignableIdentity target { get; private set; }

  public bool IsConfigured
  {
    get
    {
      return this.slotsConfigured;
    }
  }

  public int TargetInstanceID
  {
    get
    {
      return this.target_instance_id;
    }
  }

  public GameObject GetTargetGameObject()
  {
    if (this.target == null && this.target_instance_id != -1)
      this.RestoreTargetFromInstanceID();
    KMonoBehaviour target = (KMonoBehaviour) this.target;
    if ((UnityEngine.Object) target != (UnityEngine.Object) null)
      return target.gameObject;
    return (GameObject) null;
  }

  public float GetArrivalTime()
  {
    if ((UnityEngine.Object) this.GetTargetGameObject().GetComponent<MinionIdentity>() != (UnityEngine.Object) null)
      return this.GetTargetGameObject().GetComponent<MinionIdentity>().arrivalTime;
    if ((UnityEngine.Object) this.GetTargetGameObject().GetComponent<StoredMinionIdentity>() != (UnityEngine.Object) null)
      return this.GetTargetGameObject().GetComponent<StoredMinionIdentity>().arrivalTime;
    Debug.LogError((object) "Could not get minion arrival time");
    return -1f;
  }

  public int GetTotalSkillpoints()
  {
    if ((UnityEngine.Object) this.GetTargetGameObject().GetComponent<MinionIdentity>() != (UnityEngine.Object) null)
      return this.GetTargetGameObject().GetComponent<MinionResume>().TotalSkillPointsGained;
    if ((UnityEngine.Object) this.GetTargetGameObject().GetComponent<StoredMinionIdentity>() != (UnityEngine.Object) null)
      return MinionResume.CalculateTotalSkillPointsGained(this.GetTargetGameObject().GetComponent<StoredMinionIdentity>().TotalExperienceGained);
    Debug.LogError((object) "Could not get minion skill points time");
    return -1;
  }

  public void SetTarget(IAssignableIdentity target, GameObject targetGO)
  {
    Debug.Assert(target != null, (object) "target was null");
    if ((UnityEngine.Object) targetGO == (UnityEngine.Object) null)
    {
      Debug.LogWarningFormat("{0} MinionAssignablesProxy.SetTarget {1}, {2}, {3}. DESTROYING", (object) this.GetInstanceID(), (object) this.target_instance_id, (object) target, (object) targetGO);
      Util.KDestroyGameObject(this.gameObject);
    }
    this.target = target;
    this.target_instance_id = targetGO.GetComponent<KPrefabID>().InstanceID;
    this.gameObject.name = "Minion Assignables Proxy : " + targetGO.name;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.ownables = new List<Ownables>()
    {
      this.gameObject.AddOrGet<Ownables>()
    };
    Components.MinionAssignablesProxy.Add(this);
    this.ConfigureAssignableSlots();
  }

  [OnDeserialized]
  private void OnDeserialized()
  {
  }

  public void ConfigureAssignableSlots()
  {
    if (this.slotsConfigured)
      return;
    Ownables component1 = this.GetComponent<Ownables>();
    Equipment component2 = this.GetComponent<Equipment>();
    if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
    {
      foreach (AssignableSlot resource in Db.Get().AssignableSlots.resources)
      {
        if (resource is OwnableSlot)
        {
          OwnableSlotInstance ownableSlotInstance = new OwnableSlotInstance((Assignables) component1, (OwnableSlot) resource);
          component1.Add((AssignableSlotInstance) ownableSlotInstance);
        }
        else if (resource is EquipmentSlot)
        {
          EquipmentSlotInstance equipmentSlotInstance = new EquipmentSlotInstance((Assignables) component2, (EquipmentSlot) resource);
          component2.Add((AssignableSlotInstance) equipmentSlotInstance);
        }
      }
    }
    this.slotsConfigured = true;
  }

  public void RestoreTargetFromInstanceID()
  {
    if (this.target_instance_id == -1 || this.target != null)
      return;
    KPrefabID instance = KPrefabIDTracker.Get().GetInstance(this.target_instance_id);
    if ((bool) ((UnityEngine.Object) instance))
    {
      IAssignableIdentity component = instance.GetComponent<IAssignableIdentity>();
      if (component != null)
      {
        this.SetTarget(component, instance.gameObject);
      }
      else
      {
        Debug.LogWarningFormat("RestoreTargetFromInstanceID target ID {0} was found but it wasn't an IAssignableIdentity, destroying proxy object.", (object) this.target_instance_id);
        Util.KDestroyGameObject(this.gameObject);
      }
    }
    else
    {
      Debug.LogWarningFormat("RestoreTargetFromInstanceID target ID {0} was not found, destroying proxy object.", (object) this.target_instance_id);
      Util.KDestroyGameObject(this.gameObject);
    }
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.RestoreTargetFromInstanceID();
    if (this.target == null)
      return;
    this.Subscribe<MinionAssignablesProxy>(-1585839766, MinionAssignablesProxy.OnAssignablesChangedDelegate);
    Game.Instance.assignmentManager.AddToAssignmentGroup("public", (IAssignableIdentity) this);
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    Game.Instance.assignmentManager.RemoveFromAllGroups((IAssignableIdentity) this);
    this.GetComponent<Ownables>().UnassignAll();
    this.GetComponent<Equipment>().UnequipAll();
    Components.MinionAssignablesProxy.Remove(this);
  }

  private void OnAssignablesChanged(object data)
  {
    if (this.target.IsNull())
      return;
    ((KMonoBehaviour) this.target).Trigger(-1585839766, data);
  }

  private void CheckTarget()
  {
    if (this.target != null)
      return;
    KPrefabID instance = KPrefabIDTracker.Get().GetInstance(this.target_instance_id);
    if (!((UnityEngine.Object) instance != (UnityEngine.Object) null))
      return;
    this.target = instance.GetComponent<IAssignableIdentity>();
    if (this.target == null)
      return;
    MinionIdentity target1 = this.target as MinionIdentity;
    if ((bool) ((UnityEngine.Object) target1))
    {
      target1.ValidateProxy();
    }
    else
    {
      StoredMinionIdentity target2 = this.target as StoredMinionIdentity;
      if (!(bool) ((UnityEngine.Object) target2))
        return;
      target2.ValidateProxy();
    }
  }

  public List<Ownables> GetOwners()
  {
    this.CheckTarget();
    return this.target.GetOwners();
  }

  public string GetProperName()
  {
    this.CheckTarget();
    return this.target.GetProperName();
  }

  public Ownables GetSoleOwner()
  {
    this.CheckTarget();
    return this.target.GetSoleOwner();
  }

  public bool IsNull()
  {
    this.CheckTarget();
    return this.target.IsNull();
  }

  public static Ref<MinionAssignablesProxy> InitAssignableProxy(
    Ref<MinionAssignablesProxy> assignableProxyRef,
    IAssignableIdentity source)
  {
    if (assignableProxyRef == null)
      assignableProxyRef = new Ref<MinionAssignablesProxy>();
    GameObject gameObject1 = ((Component) source).gameObject;
    MinionAssignablesProxy assignablesProxy = assignableProxyRef.Get();
    if ((UnityEngine.Object) assignablesProxy == (UnityEngine.Object) null)
    {
      GameObject gameObject2 = GameUtil.KInstantiate(Assets.GetPrefab((Tag) MinionAssignablesProxyConfig.ID), Grid.SceneLayer.NoLayer, (string) null, 0);
      MinionAssignablesProxy component = gameObject2.GetComponent<MinionAssignablesProxy>();
      component.SetTarget(source, gameObject1);
      gameObject2.SetActive(true);
      assignableProxyRef.Set(component);
    }
    else
      assignablesProxy.SetTarget(source, gameObject1);
    return assignableProxyRef;
  }
}
