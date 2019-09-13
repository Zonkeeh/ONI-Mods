// Decompiled with JetBrains decompiler
// Type: Ownable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class Ownable : Assignable, ISaveLoadable, IEffectDescriptor
{
  private Color unownedTint = Color.gray;
  private Color ownedTint = Color.white;

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
      AssignableSlotInstance slot = new_assignee.GetSoleOwner().GetComponent<Ownables>().GetSlot(this.slot);
      if (slot != null)
      {
        Assignable assignable = slot.assignable;
        if ((UnityEngine.Object) assignable != (UnityEngine.Object) null)
          assignable.Unassign();
      }
    }
    base.Assign(new_assignee);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.UpdateTint();
    this.UpdateStatusString();
    this.OnAssign += new System.Action<IAssignableIdentity>(this.OnNewAssignment);
    if (this.assignee != null)
      return;
    MinionStorage component1 = this.GetComponent<MinionStorage>();
    if (!(bool) ((UnityEngine.Object) component1))
      return;
    List<MinionStorage.Info> storedMinionInfo = component1.GetStoredMinionInfo();
    if (storedMinionInfo.Count <= 0)
      return;
    Ref<KPrefabID> serializedMinion = storedMinionInfo[0].serializedMinion;
    if (serializedMinion == null || serializedMinion.GetId() == -1)
      return;
    StoredMinionIdentity component2 = serializedMinion.Get().GetComponent<StoredMinionIdentity>();
    component2.ValidateProxy();
    this.Assign((IAssignableIdentity) component2);
  }

  private void OnNewAssignment(IAssignableIdentity assignables)
  {
    this.UpdateTint();
    this.UpdateStatusString();
  }

  private void UpdateTint()
  {
    KAnimControllerBase component1 = this.GetComponent<KAnimControllerBase>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null && component1.HasBatchInstanceData)
    {
      component1.TintColour = (Color32) (this.assignee != null ? this.ownedTint : this.unownedTint);
    }
    else
    {
      KBatchedAnimController component2 = this.GetComponent<KBatchedAnimController>();
      if (!((UnityEngine.Object) component2 != (UnityEngine.Object) null) || !component2.HasBatchInstanceData)
        return;
      component2.TintColour = (Color32) (this.assignee != null ? this.ownedTint : this.unownedTint);
    }
  }

  private void UpdateStatusString()
  {
    KSelectable component = this.GetComponent<KSelectable>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      return;
    StatusItem status_item = this.assignee == null ? Db.Get().BuildingStatusItems.Unassigned : (!(this.assignee is MinionIdentity) ? (!(this.assignee is Room) ? Db.Get().BuildingStatusItems.AssignedTo : Db.Get().BuildingStatusItems.AssignedTo) : Db.Get().BuildingStatusItems.AssignedTo);
    component.SetStatusItem(Db.Get().StatusItemCategories.Main, status_item, (object) this);
  }

  public List<Descriptor> GetDescriptors(BuildingDef def)
  {
    List<Descriptor> descriptorList = new List<Descriptor>();
    Descriptor descriptor = new Descriptor();
    descriptor.SetupDescriptor((string) UI.BUILDINGEFFECTS.ASSIGNEDDUPLICANT, (string) UI.BUILDINGEFFECTS.TOOLTIPS.ASSIGNEDDUPLICANT, Descriptor.DescriptorType.Requirement);
    descriptorList.Add(descriptor);
    return descriptorList;
  }
}
