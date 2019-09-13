// Decompiled with JetBrains decompiler
// Type: Equippable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using KSerialization;
using System.Collections.Generic;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class Equippable : Assignable, ISaveLoadable, IGameObjectEffectDescriptor, IQuality
{
  private static readonly EventSystem.IntraObjectHandler<Equippable> SetDestroyedTrueDelegate = new EventSystem.IntraObjectHandler<Equippable>((System.Action<Equippable, object>) ((component, data) => component.destroyed = true));
  private QualityLevel quality;
  [MyCmpAdd]
  private EquippableWorkable equippableWorkable;
  [MyCmpReq]
  private KSelectable selectable;
  public DefHandle defHandle;
  [Serialize]
  public bool isEquipped;
  private bool destroyed;

  public QualityLevel GetQuality()
  {
    return this.quality;
  }

  public void SetQuality(QualityLevel level)
  {
    this.quality = level;
  }

  public EquipmentDef def
  {
    get
    {
      return this.defHandle.Get<EquipmentDef>();
    }
    set
    {
      this.defHandle.Set<EquipmentDef>(value);
    }
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    if (this.def.AdditionalTags == null)
      return;
    foreach (Tag additionalTag in this.def.AdditionalTags)
      this.GetComponent<KPrefabID>().AddTag(additionalTag, false);
  }

  protected override void OnSpawn()
  {
    if (this.isEquipped)
    {
      if (this.assignee != null && this.assignee is MinionIdentity)
      {
        this.assignee = (IAssignableIdentity) (this.assignee as MinionIdentity).assignableProxy.Get();
        this.assignee_identityRef.Set(this.assignee as KMonoBehaviour);
      }
      if (this.assignee == null && (UnityEngine.Object) this.assignee_identityRef.Get() != (UnityEngine.Object) null)
        this.assignee = this.assignee_identityRef.Get().GetComponent<IAssignableIdentity>();
      if (this.assignee != null)
      {
        this.assignee.GetSoleOwner().GetComponent<Equipment>().Equip(this);
      }
      else
      {
        Debug.LogWarning((object) "Equippable trying to be equipped to missing prefab");
        this.isEquipped = false;
      }
    }
    this.Subscribe<Equippable>(1969584890, Equippable.SetDestroyedTrueDelegate);
  }

  public override void Assign(IAssignableIdentity new_assignee)
  {
    if (new_assignee == this.assignee)
      return;
    if (this.slot != null && new_assignee is MinionIdentity)
      new_assignee = (IAssignableIdentity) (new_assignee as MinionIdentity).assignableProxy.Get();
    if (this.slot != null && new_assignee is StoredMinionIdentity)
      new_assignee = (IAssignableIdentity) (new_assignee as StoredMinionIdentity).assignableProxy.Get();
    if (new_assignee is MinionAssignablesProxy)
    {
      AssignableSlotInstance slot = new_assignee.GetSoleOwner().GetComponent<Equipment>().GetSlot(this.slot);
      if (slot != null)
      {
        Assignable assignable = slot.assignable;
        if ((UnityEngine.Object) assignable != (UnityEngine.Object) null)
          assignable.Unassign();
      }
    }
    base.Assign(new_assignee);
  }

  public override void Unassign()
  {
    if (this.isEquipped)
    {
      (!(this.assignee is MinionIdentity) ? ((Component) this.assignee).GetComponent<Equipment>() : ((MinionIdentity) this.assignee).assignableProxy.Get().GetComponent<Equipment>()).Unequip(this);
      this.OnUnequip();
    }
    base.Unassign();
  }

  public void OnEquip(AssignableSlotInstance slot)
  {
    this.isEquipped = true;
    if ((UnityEngine.Object) SelectTool.Instance.selected == (UnityEngine.Object) this.selectable)
      SelectTool.Instance.Select((KSelectable) null, false);
    this.GetComponent<KBatchedAnimController>().enabled = false;
    this.GetComponent<KSelectable>().IsSelectable = false;
    Effects component = slot.gameObject.GetComponent<MinionAssignablesProxy>().GetTargetGameObject().GetComponent<Effects>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
    {
      foreach (Effect effectImmunite in this.def.EffectImmunites)
        component.AddImmunity(effectImmunite);
    }
    if (this.def.OnEquipCallBack != null)
      this.def.OnEquipCallBack(this);
    this.GetComponent<KPrefabID>().AddTag(GameTags.Equipped, false);
  }

  public void OnUnequip()
  {
    this.isEquipped = false;
    if (this.destroyed)
      return;
    this.GetComponent<KPrefabID>().RemoveTag(GameTags.Equipped);
    this.GetComponent<KBatchedAnimController>().enabled = true;
    this.GetComponent<KSelectable>().IsSelectable = true;
    if (this.assignee != null)
    {
      Ownables soleOwner = this.assignee.GetSoleOwner();
      if ((bool) ((UnityEngine.Object) soleOwner))
      {
        GameObject targetGameObject = soleOwner.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
        if ((bool) ((UnityEngine.Object) targetGameObject))
        {
          Effects component = targetGameObject.GetComponent<Effects>();
          if ((UnityEngine.Object) component != (UnityEngine.Object) null)
          {
            foreach (Effect effectImmunite in this.def.EffectImmunites)
              component.RemoveImmunity(effectImmunite);
          }
        }
      }
    }
    if (this.def.OnUnequipCallBack == null)
      return;
    this.def.OnUnequipCallBack(this);
  }

  public List<Descriptor> GetDescriptors(GameObject go)
  {
    if (!((UnityEngine.Object) this.def != (UnityEngine.Object) null))
      return new List<Descriptor>();
    List<Descriptor> equipmentEffects = GameUtil.GetEquipmentEffects(this.def);
    if (this.def.additionalDescriptors != null)
    {
      foreach (Descriptor additionalDescriptor in this.def.additionalDescriptors)
        equipmentEffects.Add(additionalDescriptor);
    }
    return equipmentEffects;
  }
}
