// Decompiled with JetBrains decompiler
// Type: Toggleable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

public class Toggleable : Workable
{
  private List<KeyValuePair<IToggleHandler, Chore>> targets;

  private Toggleable()
  {
    this.SetOffsetTable(OffsetGroups.InvertedStandardTable);
  }

  protected override void OnPrefabInit()
  {
    this.faceTargetWhenWorking = true;
    base.OnPrefabInit();
    this.targets = new List<KeyValuePair<IToggleHandler, Chore>>();
    this.SetWorkTime(3f);
    this.workerStatusItem = Db.Get().DuplicantStatusItems.Toggling;
    this.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_use_remote_kanim")
    };
    this.synchronizeAnims = false;
  }

  public int SetTarget(IToggleHandler handler)
  {
    this.targets.Add(new KeyValuePair<IToggleHandler, Chore>(handler, (Chore) null));
    return this.targets.Count - 1;
  }

  public IToggleHandler GetToggleHandlerForWorker(Worker worker)
  {
    int targetForWorker = this.GetTargetForWorker(worker);
    if (targetForWorker != -1)
      return this.targets[targetForWorker].Key;
    return (IToggleHandler) null;
  }

  private int GetTargetForWorker(Worker worker)
  {
    for (int index = 0; index < this.targets.Count; ++index)
    {
      if (this.targets[index].Value != null && (UnityEngine.Object) this.targets[index].Value.driver != (UnityEngine.Object) null && (UnityEngine.Object) this.targets[index].Value.driver.gameObject == (UnityEngine.Object) worker.gameObject)
        return index;
    }
    return -1;
  }

  protected override void OnCompleteWork(Worker worker)
  {
    int targetForWorker = this.GetTargetForWorker(worker);
    if (targetForWorker != -1 && this.targets[targetForWorker].Key != null)
    {
      this.targets[targetForWorker] = new KeyValuePair<IToggleHandler, Chore>(this.targets[targetForWorker].Key, (Chore) null);
      this.targets[targetForWorker].Key.HandleToggle();
    }
    this.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.PendingSwitchToggle, false);
  }

  private void QueueToggle(int targetIdx)
  {
    if (this.targets[targetIdx].Value != null)
      return;
    if (DebugHandler.InstantBuildMode)
    {
      this.targets[targetIdx].Key.HandleToggle();
    }
    else
    {
      this.targets[targetIdx] = new KeyValuePair<IToggleHandler, Chore>(this.targets[targetIdx].Key, (Chore) new WorkChore<Toggleable>(Db.Get().ChoreTypes.Toggle, (IStateMachineTarget) this, (ChoreProvider) null, true, (System.Action<Chore>) null, (System.Action<Chore>) null, (System.Action<Chore>) null, true, (ScheduleBlockType) null, false, false, (KAnimFile) null, false, true, true, PriorityScreen.PriorityClass.basic, 5, true, true));
      this.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.PendingSwitchToggle, (object) null);
    }
  }

  public void Toggle(int targetIdx)
  {
    if (targetIdx >= this.targets.Count)
      return;
    if (this.targets[targetIdx].Value == null)
      this.QueueToggle(targetIdx);
    else
      this.CancelToggle(targetIdx);
  }

  private void CancelToggle(int targetIdx)
  {
    if (this.targets[targetIdx].Value == null)
      return;
    this.targets[targetIdx].Value.Cancel("Toggle cancelled");
    this.targets[targetIdx] = new KeyValuePair<IToggleHandler, Chore>(this.targets[targetIdx].Key, (Chore) null);
    this.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.PendingSwitchToggle, false);
  }

  public bool IsToggleQueued(int targetIdx)
  {
    return this.targets[targetIdx].Value != null;
  }
}
