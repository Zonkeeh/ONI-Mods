// Decompiled with JetBrains decompiler
// Type: Dumpable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using UnityEngine;

public class Dumpable : Workable
{
  private static readonly EventSystem.IntraObjectHandler<Dumpable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Dumpable>((System.Action<Dumpable, object>) ((component, data) => component.OnRefreshUserMenu(data)));
  private Chore chore;
  [Serialize]
  private bool isMarkedForDumping;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<Dumpable>(493375141, Dumpable.OnRefreshUserMenuDelegate);
    this.workerStatusItem = Db.Get().DuplicantStatusItems.Emptying;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    if (this.isMarkedForDumping)
      this.chore = (Chore) new WorkChore<Dumpable>(Db.Get().ChoreTypes.EmptyStorage, (IStateMachineTarget) this, (ChoreProvider) null, true, (System.Action<Chore>) null, (System.Action<Chore>) null, (System.Action<Chore>) null, true, (ScheduleBlockType) null, false, true, (KAnimFile) null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
    this.SetWorkTime(0.1f);
  }

  public void ToggleDumping()
  {
    if (DebugHandler.InstantBuildMode)
      this.OnCompleteWork((Worker) null);
    else if (this.isMarkedForDumping)
    {
      this.isMarkedForDumping = false;
      this.chore.Cancel("Cancel Dumping!");
      this.chore = (Chore) null;
      this.ShowProgressBar(false);
    }
    else
    {
      this.isMarkedForDumping = true;
      this.chore = (Chore) new WorkChore<Dumpable>(Db.Get().ChoreTypes.EmptyStorage, (IStateMachineTarget) this, (ChoreProvider) null, true, (System.Action<Chore>) null, (System.Action<Chore>) null, (System.Action<Chore>) null, true, (ScheduleBlockType) null, false, true, (KAnimFile) null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
    }
  }

  protected override void OnCompleteWork(Worker worker)
  {
    this.isMarkedForDumping = false;
    this.chore = (Chore) null;
    this.Dump();
  }

  public void Dump()
  {
    PrimaryElement component = this.GetComponent<PrimaryElement>();
    if ((double) component.Mass > 0.0)
      SimMessages.AddRemoveSubstance(Grid.PosToCell((KMonoBehaviour) this), component.ElementID, CellEventLogger.Instance.Dumpable, component.Mass, component.Temperature, component.DiseaseIdx, component.DiseaseCount, true, -1);
    Util.KDestroyGameObject(this.gameObject);
  }

  public void Dump(Vector3 pos)
  {
    PrimaryElement component = this.GetComponent<PrimaryElement>();
    if ((double) component.Mass > 0.0)
      SimMessages.AddRemoveSubstance(Grid.PosToCell(pos), component.ElementID, CellEventLogger.Instance.Dumpable, component.Mass, component.Temperature, component.DiseaseIdx, component.DiseaseCount, true, -1);
    Util.KDestroyGameObject(this.gameObject);
  }

  private void OnRefreshUserMenu(object data)
  {
    if (this.HasTag(GameTags.Stored))
      return;
    Game.Instance.userMenu.AddButton(this.gameObject, !this.isMarkedForDumping ? new KIconButtonMenu.ButtonInfo("action_empty_contents", (string) UI.USERMENUACTIONS.DUMP.NAME, new System.Action(this.ToggleDumping), Action.NumActions, (System.Action<GameObject>) null, (System.Action<KIconButtonMenu.ButtonInfo>) null, (Texture) null, (string) UI.USERMENUACTIONS.DUMP.TOOLTIP, true) : new KIconButtonMenu.ButtonInfo("action_empty_contents", (string) UI.USERMENUACTIONS.DUMP.NAME_OFF, new System.Action(this.ToggleDumping), Action.NumActions, (System.Action<GameObject>) null, (System.Action<KIconButtonMenu.ButtonInfo>) null, (Texture) null, (string) UI.USERMENUACTIONS.DUMP.TOOLTIP_OFF, true), 1f);
  }
}
