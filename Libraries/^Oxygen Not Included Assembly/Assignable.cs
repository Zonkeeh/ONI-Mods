// Decompiled with JetBrains decompiler
// Type: Assignable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

public abstract class Assignable : KMonoBehaviour, ISaveLoadable
{
  [Serialize]
  protected Ref<KMonoBehaviour> assignee_identityRef = new Ref<KMonoBehaviour>();
  [Serialize]
  private string assignee_groupID = string.Empty;
  [Serialize]
  private bool canBeAssigned = true;
  private List<Func<MinionAssignablesProxy, bool>> autoassignmentPreconditions = new List<Func<MinionAssignablesProxy, bool>>();
  private List<Func<MinionAssignablesProxy, bool>> assignmentPreconditions = new List<Func<MinionAssignablesProxy, bool>>();
  public string slotID;
  private AssignableSlot _slot;
  public IAssignableIdentity assignee;
  public AssignableSlot[] subSlots;
  public bool canBePublic;

  public AssignableSlot slot
  {
    get
    {
      if (this._slot == null)
        this._slot = Db.Get().AssignableSlots.Get(this.slotID);
      return this._slot;
    }
  }

  public bool CanBeAssigned
  {
    get
    {
      return this.canBeAssigned;
    }
  }

  public event System.Action<IAssignableIdentity> OnAssign;

  [OnDeserialized]
  internal void OnDeserialized()
  {
  }

  private void RestoreAssignee()
  {
    IAssignableIdentity savedAssignee = this.GetSavedAssignee();
    if (savedAssignee == null)
      return;
    this.Assign(savedAssignee);
  }

  private IAssignableIdentity GetSavedAssignee()
  {
    if ((UnityEngine.Object) this.assignee_identityRef.Get() != (UnityEngine.Object) null)
      return this.assignee_identityRef.Get().GetComponent<IAssignableIdentity>();
    if (this.assignee_groupID != string.Empty)
      return (IAssignableIdentity) Game.Instance.assignmentManager.assignment_groups[this.assignee_groupID];
    return (IAssignableIdentity) null;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.RestoreAssignee();
    Game.Instance.assignmentManager.Add(this);
    if (this.assignee != null || !this.canBePublic)
      return;
    this.Assign((IAssignableIdentity) Game.Instance.assignmentManager.assignment_groups["public"]);
  }

  protected override void OnCleanUp()
  {
    this.Unassign();
    Game.Instance.assignmentManager.Remove(this);
    base.OnCleanUp();
  }

  public bool CanAutoAssignTo(IAssignableIdentity identity)
  {
    MinionAssignablesProxy assignablesProxy = identity as MinionAssignablesProxy;
    if ((UnityEngine.Object) assignablesProxy == (UnityEngine.Object) null)
      return true;
    if (!this.CanAssignTo((IAssignableIdentity) assignablesProxy))
      return false;
    foreach (Func<MinionAssignablesProxy, bool> autoassignmentPrecondition in this.autoassignmentPreconditions)
    {
      if (!autoassignmentPrecondition(assignablesProxy))
        return false;
    }
    return true;
  }

  public bool CanAssignTo(IAssignableIdentity identity)
  {
    MinionAssignablesProxy assignablesProxy = identity as MinionAssignablesProxy;
    if ((UnityEngine.Object) assignablesProxy == (UnityEngine.Object) null)
      return true;
    foreach (Func<MinionAssignablesProxy, bool> assignmentPrecondition in this.assignmentPreconditions)
    {
      if (!assignmentPrecondition(assignablesProxy))
        return false;
    }
    return true;
  }

  public bool IsAssigned()
  {
    return this.assignee != null;
  }

  public bool IsAssignedTo(IAssignableIdentity identity)
  {
    Debug.Assert(identity != null, (object) "IsAssignedTo identity is null");
    Ownables soleOwner = identity.GetSoleOwner();
    Debug.Assert((UnityEngine.Object) soleOwner != (UnityEngine.Object) null, (object) "IsAssignedTo identity sole owner is null");
    if (this.assignee != null)
    {
      foreach (Ownables owner in this.assignee.GetOwners())
      {
        Debug.Assert((bool) ((UnityEngine.Object) owner), (object) "Assignable owners list contained null");
        if ((UnityEngine.Object) owner.gameObject == (UnityEngine.Object) soleOwner.gameObject)
          return true;
      }
    }
    return false;
  }

  public virtual void Assign(IAssignableIdentity new_assignee)
  {
    if (new_assignee == this.assignee)
      return;
    if (new_assignee is KMonoBehaviour)
    {
      if (!this.CanAssignTo(new_assignee))
        return;
      this.assignee_identityRef.Set((KMonoBehaviour) new_assignee);
      this.assignee_groupID = string.Empty;
    }
    else if (new_assignee is AssignmentGroup)
    {
      this.assignee_identityRef.Set((KMonoBehaviour) null);
      this.assignee_groupID = ((AssignmentGroup) new_assignee).id;
    }
    this.GetComponent<KPrefabID>().AddTag(GameTags.Assigned, false);
    this.assignee = new_assignee;
    if (this.slot != null && (new_assignee is MinionIdentity || new_assignee is StoredMinionIdentity || new_assignee is MinionAssignablesProxy))
    {
      Ownables soleOwner = new_assignee.GetSoleOwner();
      if ((UnityEngine.Object) soleOwner != (UnityEngine.Object) null)
        soleOwner.GetSlot(this.slot)?.Assign(this);
      Equipment component = soleOwner.GetComponent<Equipment>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        component.GetSlot(this.slot)?.Assign(this);
    }
    if (this.OnAssign != null)
      this.OnAssign(new_assignee);
    this.Trigger(684616645, (object) new_assignee);
  }

  public virtual void Unassign()
  {
    if (this.assignee == null)
      return;
    this.GetComponent<KPrefabID>().RemoveTag(GameTags.Assigned);
    if (this.slot != null)
    {
      Ownables soleOwner = this.assignee.GetSoleOwner();
      if ((bool) ((UnityEngine.Object) soleOwner))
      {
        soleOwner.GetSlot(this.slot)?.Unassign(true);
        Equipment component = soleOwner.GetComponent<Equipment>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
          component.GetSlot(this.slot)?.Unassign(true);
      }
    }
    this.assignee = (IAssignableIdentity) null;
    if (this.canBePublic)
      this.Assign((IAssignableIdentity) Game.Instance.assignmentManager.assignment_groups["public"]);
    this.assignee_identityRef.Set((KMonoBehaviour) null);
    this.assignee_groupID = string.Empty;
    if (this.OnAssign != null)
      this.OnAssign((IAssignableIdentity) null);
    this.Trigger(684616645, (object) null);
  }

  public void SetCanBeAssigned(bool state)
  {
    this.canBeAssigned = state;
  }

  public void AddAssignPrecondition(Func<MinionAssignablesProxy, bool> precondition)
  {
    this.assignmentPreconditions.Add(precondition);
  }

  public void AddAutoassignPrecondition(Func<MinionAssignablesProxy, bool> precondition)
  {
    this.autoassignmentPreconditions.Add(precondition);
  }

  public int GetNavigationCost(Navigator navigator)
  {
    int num = -1;
    int cell1 = Grid.PosToCell((KMonoBehaviour) this);
    IApproachable component = this.GetComponent<IApproachable>();
    CellOffset[] cellOffsetArray;
    if (component != null)
      cellOffsetArray = component.GetOffsets();
    else
      cellOffsetArray = new CellOffset[1]
      {
        new CellOffset()
      };
    foreach (CellOffset offset in cellOffsetArray)
    {
      int cell2 = Grid.OffsetCell(cell1, offset);
      int navigationCost = navigator.GetNavigationCost(cell2);
      if (navigationCost != -1 && (num == -1 || navigationCost < num))
        num = navigationCost;
    }
    return num;
  }
}
