// Decompiled with JetBrains decompiler
// Type: Assignables
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class Assignables : KMonoBehaviour
{
  private static readonly EventSystem.IntraObjectHandler<Assignables> OnDeathDelegate = new EventSystem.IntraObjectHandler<Assignables>((System.Action<Assignables, object>) ((component, data) => component.OnDeath(data)));
  protected List<AssignableSlotInstance> slots = new List<AssignableSlotInstance>();

  public List<AssignableSlotInstance> Slots
  {
    get
    {
      return this.slots;
    }
  }

  protected IAssignableIdentity GetAssignableIdentity()
  {
    MinionIdentity component = this.GetComponent<MinionIdentity>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      return (IAssignableIdentity) component.assignableProxy.Get();
    return (IAssignableIdentity) this.GetComponent<MinionAssignablesProxy>();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Subscribe<Assignables>(1623392196, Assignables.OnDeathDelegate);
  }

  private void OnDeath(object data)
  {
    foreach (AssignableSlotInstance slot in this.slots)
      slot.Unassign(true);
  }

  public void Add(AssignableSlotInstance slot_instance)
  {
    this.slots.Add(slot_instance);
  }

  public Assignable GetAssignable(AssignableSlot slot)
  {
    return this.GetSlot(slot)?.assignable;
  }

  public AssignableSlotInstance GetSlot(AssignableSlot slot)
  {
    Debug.Assert(this.slots.Count > 0, (object) "GetSlot called with no slots configured");
    if (slot == null)
      return (AssignableSlotInstance) null;
    foreach (AssignableSlotInstance slot1 in this.slots)
    {
      if (slot1.slot == slot)
        return slot1;
    }
    return (AssignableSlotInstance) null;
  }

  public Assignable AutoAssignSlot(AssignableSlot slot)
  {
    Assignable assignable1 = this.GetAssignable(slot);
    if ((UnityEngine.Object) assignable1 != (UnityEngine.Object) null)
      return assignable1;
    GameObject targetGameObject = this.GetComponent<MinionAssignablesProxy>().GetTargetGameObject();
    if ((UnityEngine.Object) targetGameObject == (UnityEngine.Object) null)
    {
      Debug.LogWarning((object) "AutoAssignSlot failed, proxy game object was null.");
      return (Assignable) null;
    }
    Navigator component = targetGameObject.GetComponent<Navigator>();
    IAssignableIdentity assignableIdentity = this.GetAssignableIdentity();
    int num = int.MaxValue;
    foreach (Assignable assignable2 in Game.Instance.assignmentManager)
    {
      if (!((UnityEngine.Object) assignable2 == (UnityEngine.Object) null) && !assignable2.IsAssigned() && (assignable2.slot == slot && assignable2.CanAutoAssignTo(assignableIdentity)))
      {
        int navigationCost = assignable2.GetNavigationCost(component);
        if (navigationCost != -1 && navigationCost < num)
        {
          num = navigationCost;
          assignable1 = assignable2;
        }
      }
    }
    if ((UnityEngine.Object) assignable1 != (UnityEngine.Object) null)
      assignable1.Assign(assignableIdentity);
    return assignable1;
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    foreach (AssignableSlotInstance slot in this.slots)
      slot.Unassign(true);
  }
}
