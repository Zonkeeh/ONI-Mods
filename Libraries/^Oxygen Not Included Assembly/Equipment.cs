// Decompiled with JetBrains decompiler
// Type: Equipment
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using KSerialization;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class Equipment : Assignables
{
  private static readonly EventSystem.IntraObjectHandler<Equipment> SetDestroyedTrueDelegate = new EventSystem.IntraObjectHandler<Equipment>((System.Action<Equipment, object>) ((component, data) => component.destroyed = true));
  private SchedulerHandle refreshHandle;

  public bool destroyed { get; private set; }

  private GameObject GetTargetGameObject()
  {
    MinionAssignablesProxy assignableIdentity = (MinionAssignablesProxy) this.GetAssignableIdentity();
    if ((bool) ((UnityEngine.Object) assignableIdentity))
      return assignableIdentity.GetTargetGameObject();
    return (GameObject) null;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    Components.Equipment.Add(this);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Subscribe<Equipment>(1502190696, Equipment.SetDestroyedTrueDelegate);
    this.Subscribe<Equipment>(1969584890, Equipment.SetDestroyedTrueDelegate);
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    this.refreshHandle.ClearScheduler();
    Components.Equipment.Remove(this);
  }

  public void Equip(Equippable equippable)
  {
    AssignableSlotInstance slot = this.GetSlot(equippable.slot);
    slot.Assign((Assignable) equippable);
    GameObject targetGameObject = this.GetTargetGameObject();
    Debug.Assert((bool) ((UnityEngine.Object) targetGameObject), (object) "GetTargetGameObject returned null in Equip");
    targetGameObject.Trigger(-448952673, (object) equippable.GetComponent<KPrefabID>());
    equippable.Trigger(-1617557748, (object) this);
    Attributes attributes = targetGameObject.GetAttributes();
    if (attributes != null)
    {
      foreach (AttributeModifier attributeModifier in equippable.def.AttributeModifiers)
        attributes.Add(attributeModifier);
    }
    SnapOn component1 = targetGameObject.GetComponent<SnapOn>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
    {
      component1.AttachSnapOnByName(equippable.def.SnapOn);
      if (equippable.def.SnapOn1 != null)
        component1.AttachSnapOnByName(equippable.def.SnapOn1);
    }
    KBatchedAnimController component2 = targetGameObject.GetComponent<KBatchedAnimController>();
    if ((UnityEngine.Object) component2 != (UnityEngine.Object) null && (UnityEngine.Object) equippable.def.BuildOverride != (UnityEngine.Object) null)
      component2.GetComponent<SymbolOverrideController>().AddBuildOverride(equippable.def.BuildOverride.GetData(), equippable.def.BuildOverridePriority);
    if ((bool) ((UnityEngine.Object) equippable.transform.parent))
    {
      Storage component3 = equippable.transform.parent.GetComponent<Storage>();
      if ((bool) ((UnityEngine.Object) component3))
        component3.Drop(equippable.gameObject, true);
    }
    equippable.transform.parent = slot.gameObject.transform;
    equippable.transform.SetLocalPosition(Vector3.zero);
    this.SetEquippableStoredModifiers(equippable, true);
    equippable.OnEquip(slot);
    if ((double) this.refreshHandle.TimeRemaining > 0.0)
    {
      Debug.LogWarning((object) (targetGameObject.GetProperName() + " is already in the process of changing equipment (equip)"));
      this.refreshHandle.ClearScheduler();
    }
    CreatureSimTemperatureTransfer transferer = targetGameObject.GetComponent<CreatureSimTemperatureTransfer>();
    if (!((UnityEngine.Object) component2 == (UnityEngine.Object) null))
      this.refreshHandle = GameScheduler.Instance.Schedule("ChangeEquipment", 2f, (System.Action<object>) (obj =>
      {
        if (!((UnityEngine.Object) transferer != (UnityEngine.Object) null))
          return;
        transferer.RefreshRegistration();
      }), (object) null, (SchedulerGroup) null);
    Game.Instance.Trigger(-2146166042, (object) null);
  }

  public void Unequip(Equippable equippable)
  {
    this.GetSlot(equippable.slot).Unassign(true);
    equippable.Trigger(-170173755, (object) this);
    GameObject targetGameObject = this.GetTargetGameObject();
    if (!(bool) ((UnityEngine.Object) targetGameObject))
      return;
    targetGameObject.Trigger(-1285462312, (object) equippable.GetComponent<KPrefabID>());
    KBatchedAnimController component1 = targetGameObject.GetComponent<KBatchedAnimController>();
    if (!this.destroyed)
    {
      if ((UnityEngine.Object) equippable.def.BuildOverride != (UnityEngine.Object) null && (UnityEngine.Object) component1 != (UnityEngine.Object) null)
        component1.GetComponent<SymbolOverrideController>().TryRemoveBuildOverride(equippable.def.BuildOverride.GetData(), equippable.def.BuildOverridePriority);
      Attributes attributes = targetGameObject.GetAttributes();
      if (attributes != null)
      {
        foreach (AttributeModifier attributeModifier in equippable.def.AttributeModifiers)
          attributes.Remove(attributeModifier);
      }
      if (!equippable.def.IsBody)
      {
        SnapOn component2 = targetGameObject.GetComponent<SnapOn>();
        component2.DetachSnapOnByName(equippable.def.SnapOn);
        if (equippable.def.SnapOn1 != null)
          component2.DetachSnapOnByName(equippable.def.SnapOn1);
      }
      if ((bool) ((UnityEngine.Object) equippable.transform.parent))
      {
        Storage component2 = equippable.transform.parent.GetComponent<Storage>();
        if ((bool) ((UnityEngine.Object) component2))
          component2.Drop(equippable.gameObject, true);
      }
      this.SetEquippableStoredModifiers(equippable, false);
      equippable.transform.parent = (Transform) null;
      equippable.transform.SetPosition(targetGameObject.transform.GetPosition() + Vector3.up / 2f);
      KBatchedAnimController component3 = equippable.GetComponent<KBatchedAnimController>();
      if ((bool) ((UnityEngine.Object) component3))
        component3.SetSceneLayer(Grid.SceneLayer.Ore);
      if (!((UnityEngine.Object) component1 == (UnityEngine.Object) null))
      {
        if ((double) this.refreshHandle.TimeRemaining > 0.0)
          this.refreshHandle.ClearScheduler();
        Equipment instance = this;
        this.refreshHandle = GameScheduler.Instance.Schedule("ChangeEquipment", 1f, (System.Action<object>) (obj =>
        {
          GameObject gameObject = !((UnityEngine.Object) instance != (UnityEngine.Object) null) ? (GameObject) null : instance.GetTargetGameObject();
          if (!(bool) ((UnityEngine.Object) gameObject))
            return;
          CreatureSimTemperatureTransfer component2 = gameObject.GetComponent<CreatureSimTemperatureTransfer>();
          if (!((UnityEngine.Object) component2 != (UnityEngine.Object) null))
            return;
          component2.RefreshRegistration();
        }), (object) null, (SchedulerGroup) null);
      }
    }
    Game.Instance.Trigger(-2146166042, (object) null);
  }

  public bool IsEquipped(Equippable equippable)
  {
    if (equippable.assignee is Equipment && (UnityEngine.Object) equippable.assignee == (UnityEngine.Object) this)
      return equippable.isEquipped;
    return false;
  }

  public bool IsSlotOccupied(AssignableSlot slot)
  {
    EquipmentSlotInstance slot1 = this.GetSlot(slot) as EquipmentSlotInstance;
    if (slot1.IsAssigned())
      return (slot1.assignable as Equippable).isEquipped;
    return false;
  }

  public void UnequipAll()
  {
    foreach (AssignableSlotInstance slot in this.slots)
    {
      if ((UnityEngine.Object) slot.assignable != (UnityEngine.Object) null)
        slot.assignable.Unassign();
    }
  }

  private void SetEquippableStoredModifiers(Equippable equippable, bool isStoring)
  {
    GameObject gameObject = equippable.gameObject;
    Storage.MakeItemTemperatureInsulated(gameObject, isStoring, false);
    Storage.MakeItemInvisible(gameObject, isStoring, false);
  }
}
