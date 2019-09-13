// Decompiled with JetBrains decompiler
// Type: Capturable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class Capturable : Workable, IGameObjectEffectDescriptor
{
  private static readonly EventSystem.IntraObjectHandler<Capturable> OnDeathDelegate = new EventSystem.IntraObjectHandler<Capturable>((System.Action<Capturable, object>) ((component, data) => component.OnDeath(data)));
  private static readonly EventSystem.IntraObjectHandler<Capturable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Capturable>((System.Action<Capturable, object>) ((component, data) => component.OnRefreshUserMenu(data)));
  private static readonly EventSystem.IntraObjectHandler<Capturable> OnTagsChangedDelegate = new EventSystem.IntraObjectHandler<Capturable>((System.Action<Capturable, object>) ((component, data) => component.OnTagsChanged(data)));
  public bool allowCapture = true;
  [MyCmpAdd]
  private Baggable baggable;
  [MyCmpAdd]
  private Prioritizable prioritizable;
  [Serialize]
  private bool markedForCapture;
  private Chore chore;

  public bool IsMarkedForCapture
  {
    get
    {
      return this.markedForCapture;
    }
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    Components.Capturables.Add(this);
    this.SetOffsetTable(OffsetGroups.InvertedStandardTable);
    this.requiredSkillPerk = Db.Get().SkillPerks.CanWrangleCreatures.Id;
    this.resetProgressOnStop = true;
    this.faceTargetWhenWorking = true;
    this.synchronizeAnims = false;
    this.multitoolContext = (HashedString) "capture";
    this.multitoolHitEffectTag = (Tag) "fx_capture_splash";
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Subscribe<Capturable>(1623392196, Capturable.OnDeathDelegate);
    this.Subscribe<Capturable>(493375141, Capturable.OnRefreshUserMenuDelegate);
    this.Subscribe<Capturable>(-1582839653, Capturable.OnTagsChangedDelegate);
    if (this.markedForCapture)
      Prioritizable.AddRef(this.gameObject);
    this.UpdateStatusItem();
    this.UpdateChore();
    this.SetWorkTime(10f);
  }

  protected override void OnCleanUp()
  {
    Components.Capturables.Remove(this);
    base.OnCleanUp();
  }

  private void OnDeath(object data)
  {
    this.allowCapture = false;
    this.markedForCapture = false;
  }

  private void OnTagsChanged(object data)
  {
    this.MarkForCapture(this.markedForCapture);
  }

  public void MarkForCapture(bool mark)
  {
    PrioritySetting priority = new PrioritySetting(PriorityScreen.PriorityClass.basic, 5);
    this.MarkForCapture(mark, priority);
  }

  public void MarkForCapture(bool mark, PrioritySetting priority)
  {
    mark = mark && this.IsCapturable();
    if (this.markedForCapture && !mark)
      Prioritizable.RemoveRef(this.gameObject);
    else if (!this.markedForCapture && mark)
    {
      Prioritizable.AddRef(this.gameObject);
      Prioritizable component = this.GetComponent<Prioritizable>();
      if ((bool) ((UnityEngine.Object) component))
        component.SetMasterPriority(priority);
    }
    this.markedForCapture = mark;
    this.UpdateStatusItem();
    this.UpdateChore();
  }

  public bool IsCapturable()
  {
    return this.allowCapture && !this.gameObject.HasTag(GameTags.Trapped) && (!this.gameObject.HasTag(GameTags.Stored) && !this.gameObject.HasTag(GameTags.Creatures.Bagged));
  }

  private void OnRefreshUserMenu(object data)
  {
    if (!this.IsCapturable())
      return;
    Game.Instance.userMenu.AddButton(this.gameObject, this.markedForCapture ? new KIconButtonMenu.ButtonInfo("action_capture", (string) UI.USERMENUACTIONS.CANCELCAPTURE.NAME, (System.Action) (() => this.MarkForCapture(false)), Action.NumActions, (System.Action<GameObject>) null, (System.Action<KIconButtonMenu.ButtonInfo>) null, (Texture) null, (string) UI.USERMENUACTIONS.CANCELCAPTURE.TOOLTIP, true) : new KIconButtonMenu.ButtonInfo("action_capture", (string) UI.USERMENUACTIONS.CAPTURE.NAME, (System.Action) (() => this.MarkForCapture(true)), Action.NumActions, (System.Action<GameObject>) null, (System.Action<KIconButtonMenu.ButtonInfo>) null, (Texture) null, (string) UI.USERMENUACTIONS.CAPTURE.TOOLTIP, true), 1f);
  }

  private void UpdateStatusItem()
  {
    this.shouldShowSkillPerkStatusItem = this.markedForCapture;
    this.UpdateStatusItem((object) null);
    if (this.markedForCapture)
      this.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.OrderCapture, (object) this);
    else
      this.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.OrderCapture, false);
  }

  private void UpdateChore()
  {
    if (this.markedForCapture && this.chore == null)
    {
      this.chore = (Chore) new WorkChore<Capturable>(Db.Get().ChoreTypes.Capture, (IStateMachineTarget) this, (ChoreProvider) null, true, (System.Action<Chore>) null, (System.Action<Chore>) null, (System.Action<Chore>) null, true, (ScheduleBlockType) null, false, true, (KAnimFile) null, true, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
    }
    else
    {
      if (this.markedForCapture || this.chore == null)
        return;
      this.chore.Cancel("not marked for capture");
      this.chore = (Chore) null;
    }
  }

  protected override void OnStartWork(Worker worker)
  {
    this.GetComponent<KPrefabID>().AddTag(GameTags.Creatures.Stunned, false);
  }

  protected override void OnStopWork(Worker worker)
  {
    this.GetComponent<KPrefabID>().RemoveTag(GameTags.Creatures.Stunned);
  }

  protected override void OnCompleteWork(Worker worker)
  {
    int cell1 = this.NaturalBuildingCell();
    if (Grid.Solid[cell1])
    {
      int cell2 = Grid.CellAbove(cell1);
      if (Grid.IsValidCell(cell2) && !Grid.Solid[cell2])
        cell1 = cell2;
    }
    this.MarkForCapture(false);
    this.baggable.SetWrangled();
    this.baggable.transform.SetPosition(Grid.CellToPosCCC(cell1, Grid.SceneLayer.Ore));
  }

  public override List<Descriptor> GetDescriptors(GameObject go)
  {
    List<Descriptor> descriptors = base.GetDescriptors(go);
    if (this.allowCapture)
      descriptors.Add(new Descriptor((string) UI.BUILDINGEFFECTS.CAPTURE_METHOD_WRANGLE, (string) UI.BUILDINGEFFECTS.TOOLTIPS.CAPTURE_METHOD_WRANGLE, Descriptor.DescriptorType.Effect, false));
    return descriptors;
  }
}
