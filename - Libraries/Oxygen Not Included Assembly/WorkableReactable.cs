// Decompiled with JetBrains decompiler
// Type: WorkableReactable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class WorkableReactable : Reactable
{
  protected Workable workable;
  private Worker worker;
  public WorkableReactable.AllowedDirection allowedDirection;

  public WorkableReactable(
    Workable workable,
    HashedString id,
    ChoreType chore_type,
    WorkableReactable.AllowedDirection allowed_direction = WorkableReactable.AllowedDirection.Any)
    : base(workable.gameObject, id, chore_type, 1, 1, false, 0.0f, 0.0f, float.PositiveInfinity)
  {
    this.workable = workable;
    this.allowedDirection = allowed_direction;
  }

  public override bool InternalCanBegin(
    GameObject new_reactor,
    Navigator.ActiveTransition transition)
  {
    if ((Object) this.workable == (Object) null || (Object) this.reactor != (Object) null)
      return false;
    Brain component1 = new_reactor.GetComponent<Brain>();
    if ((Object) component1 == (Object) null || !component1.IsRunning())
      return false;
    Navigator component2 = new_reactor.GetComponent<Navigator>();
    if ((Object) component2 == (Object) null || !component2.IsMoving())
      return false;
    if (this.allowedDirection == WorkableReactable.AllowedDirection.Any)
      return true;
    Facing component3 = new_reactor.GetComponent<Facing>();
    if ((Object) component3 == (Object) null)
      return false;
    bool facing = component3.GetFacing();
    return (!facing || this.allowedDirection != WorkableReactable.AllowedDirection.Right) && (facing || this.allowedDirection != WorkableReactable.AllowedDirection.Left);
  }

  protected override void InternalBegin()
  {
    this.worker = this.reactor.GetComponent<Worker>();
    this.worker.StartWork(new Worker.StartWorkInfo(this.workable));
  }

  public override void Update(float dt)
  {
    if ((Object) this.worker.workable == (Object) null)
    {
      this.End();
    }
    else
    {
      if (this.worker.Work(dt) == Worker.WorkResult.InProgress)
        return;
      this.End();
    }
  }

  protected override void InternalEnd()
  {
    if (!((Object) this.worker != (Object) null))
      return;
    this.worker.StopWork();
  }

  protected override void InternalCleanup()
  {
  }

  public enum AllowedDirection
  {
    Any,
    Left,
    Right,
  }
}
