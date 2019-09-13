// Decompiled with JetBrains decompiler
// Type: Butcherable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class Butcherable : Workable, ISaveLoadable
{
  private static readonly EventSystem.IntraObjectHandler<Butcherable> SetReadyToButcherDelegate = new EventSystem.IntraObjectHandler<Butcherable>((System.Action<Butcherable, object>) ((component, data) => component.SetReadyToButcher(data)));
  private static readonly EventSystem.IntraObjectHandler<Butcherable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Butcherable>((System.Action<Butcherable, object>) ((component, data) => component.OnRefreshUserMenu(data)));
  [MyCmpGet]
  private KAnimControllerBase controller;
  [MyCmpGet]
  private Harvestable harvestable;
  private bool readyToButcher;
  private bool butchered;
  public string[] Drops;
  private Chore chore;

  public void SetDrops(string[] drops)
  {
    this.Drops = drops;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<Butcherable>(1272413801, Butcherable.SetReadyToButcherDelegate);
    this.Subscribe<Butcherable>(493375141, Butcherable.OnRefreshUserMenuDelegate);
    this.workTime = 3f;
    this.multitoolContext = (HashedString) "harvest";
    this.multitoolHitEffectTag = (Tag) "fx_harvest_splash";
  }

  public void SetReadyToButcher(object param)
  {
    this.readyToButcher = true;
  }

  public void SetReadyToButcher(bool ready)
  {
    this.readyToButcher = ready;
  }

  public void ActivateChore(object param)
  {
    if (this.chore != null)
      return;
    this.chore = (Chore) new WorkChore<Butcherable>(Db.Get().ChoreTypes.Harvest, (IStateMachineTarget) this, (ChoreProvider) null, true, (System.Action<Chore>) null, (System.Action<Chore>) null, (System.Action<Chore>) null, true, (ScheduleBlockType) null, false, true, (KAnimFile) null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
    this.OnRefreshUserMenu((object) null);
  }

  public void CancelChore(object param)
  {
    if (this.chore == null)
      return;
    this.chore.Cancel("User cancelled");
    this.chore = (Chore) null;
  }

  private void OnClickCancel()
  {
    this.CancelChore((object) null);
  }

  private void OnClickButcher()
  {
    if (DebugHandler.InstantBuildMode)
      this.OnButcherComplete();
    else
      this.ActivateChore((object) null);
  }

  private void OnRefreshUserMenu(object data)
  {
    if (!this.readyToButcher)
      return;
    Game.Instance.userMenu.AddButton(this.gameObject, this.chore == null ? new KIconButtonMenu.ButtonInfo("action_harvest", "Meatify", new System.Action(this.OnClickButcher), Action.NumActions, (System.Action<GameObject>) null, (System.Action<KIconButtonMenu.ButtonInfo>) null, (Texture) null, string.Empty, true) : new KIconButtonMenu.ButtonInfo("action_harvest", "Cancel Meatify", new System.Action(this.OnClickCancel), Action.NumActions, (System.Action<GameObject>) null, (System.Action<KIconButtonMenu.ButtonInfo>) null, (Texture) null, string.Empty, true), 1f);
  }

  protected override void OnCompleteWork(Worker worker)
  {
    this.OnButcherComplete();
  }

  public void OnButcherComplete()
  {
    if (this.butchered)
      return;
    KSelectable component1 = this.GetComponent<KSelectable>();
    if ((bool) ((UnityEngine.Object) component1) && component1.IsSelected)
      SelectTool.Instance.Select((KSelectable) null, false);
    for (int index = 0; index < this.Drops.Length; ++index)
    {
      GameObject go = Scenario.SpawnPrefab(this.GetDropSpawnLocation(), 0, 0, this.Drops[index], Grid.SceneLayer.Ore);
      go.SetActive(true);
      Edible component2 = go.GetComponent<Edible>();
      if ((bool) ((UnityEngine.Object) component2))
        ReportManager.Instance.ReportValue(ReportManager.ReportType.CaloriesCreated, component2.Calories, StringFormatter.Replace((string) UI.ENDOFDAYREPORT.NOTES.BUTCHERED, "{0}", go.GetProperName()), (string) UI.ENDOFDAYREPORT.NOTES.BUTCHERED_CONTEXT);
    }
    this.chore = (Chore) null;
    this.butchered = true;
    this.readyToButcher = false;
    Game.Instance.userMenu.Refresh(this.gameObject);
    this.Trigger(395373363, (object) null);
  }

  private int GetDropSpawnLocation()
  {
    int cell1 = Grid.PosToCell(this.gameObject);
    int cell2 = Grid.CellAbove(cell1);
    if (Grid.IsValidCell(cell2) && !Grid.Solid[cell2])
      return cell2;
    return cell1;
  }
}
