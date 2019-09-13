// Decompiled with JetBrains decompiler
// Type: AstronautTrainingCenter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;

public class AstronautTrainingCenter : Workable
{
  public Chore.Precondition IsNotMarkedForDeconstruction = new Chore.Precondition()
  {
    id = nameof (IsNotMarkedForDeconstruction),
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.IS_MARKED_FOR_DECONSTRUCTION,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      Deconstructable deconstructable = data as Deconstructable;
      if (!((UnityEngine.Object) deconstructable == (UnityEngine.Object) null))
        return !deconstructable.IsMarkedForDeconstruction();
      return true;
    })
  };
  public float daysToMasterRole;
  private Chore chore;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.chore = this.CreateChore();
  }

  private Chore CreateChore()
  {
    return (Chore) new WorkChore<AstronautTrainingCenter>(Db.Get().ChoreTypes.Train, (IStateMachineTarget) this, (ChoreProvider) null, true, (System.Action<Chore>) null, (System.Action<Chore>) null, (System.Action<Chore>) null, false, (ScheduleBlockType) null, false, true, (KAnimFile) null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
  }

  protected override void OnStartWork(Worker worker)
  {
    base.OnStartWork(worker);
    this.GetComponent<Operational>().SetActive(true, false);
  }

  protected override bool OnWorkTick(Worker worker, float dt)
  {
    return (UnityEngine.Object) worker == (UnityEngine.Object) null ? true : true;
  }

  protected override void OnCompleteWork(Worker worker)
  {
    base.OnCompleteWork(worker);
    if (this.chore != null && !this.chore.isComplete)
      this.chore.Cancel("completed but not complete??");
    this.chore = this.CreateChore();
  }

  protected override void OnStopWork(Worker worker)
  {
    base.OnStopWork(worker);
    this.GetComponent<Operational>().SetActive(false, false);
  }

  public override float GetPercentComplete()
  {
    return (UnityEngine.Object) this.worker == (UnityEngine.Object) null ? 0.0f : 0.0f;
  }
}
