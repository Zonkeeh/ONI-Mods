// Decompiled with JetBrains decompiler
// Type: Studyable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using TUNING;

public class Studyable : Workable, ISidescreenButtonControl
{
  public string meterTrackerSymbol;
  public string meterAnim;
  private Chore chore;
  private const float STUDY_WORK_TIME = 3600f;
  [Serialize]
  private bool studied;
  [Serialize]
  private bool markedForStudy;
  private Guid statusItemGuid;
  private Guid additionalStatusItemGuid;
  private MeterController studiedIndicator;

  public bool Studied
  {
    get
    {
      return this.studied;
    }
  }

  public string SidescreenTitleKey
  {
    get
    {
      return "STRINGS.UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.TITLE";
    }
  }

  public string SidescreenStatusMessage
  {
    get
    {
      if (this.studied)
        return (string) UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.STUDIED_STATUS;
      if (this.markedForStudy)
        return (string) UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.PENDING_STATUS;
      return (string) UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.SEND_STATUS;
    }
  }

  public string SidescreenButtonText
  {
    get
    {
      if (this.studied)
        return (string) UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.STUDIED_BUTTON;
      if (this.markedForStudy)
        return (string) UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.PENDING_BUTTON;
      return (string) UI.UISIDESCREENS.STUDYABLE_SIDE_SCREEN.SEND_BUTTON;
    }
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_use_machine_kanim")
    };
    this.faceTargetWhenWorking = true;
    this.synchronizeAnims = false;
    this.workerStatusItem = Db.Get().DuplicantStatusItems.Studying;
    this.resetProgressOnStop = false;
    this.requiredSkillPerk = Db.Get().SkillPerks.CanStudyWorldObjects.Id;
    this.attributeConverter = Db.Get().AttributeConverters.ResearchSpeed;
    this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
    this.skillExperienceSkillGroup = Db.Get().SkillGroups.Research.Id;
    this.skillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
    this.SetWorkTime(3600f);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.studiedIndicator = new MeterController((KAnimControllerBase) this.GetComponent<KBatchedAnimController>(), this.meterTrackerSymbol, this.meterAnim, Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[1]
    {
      this.meterTrackerSymbol
    });
    this.Refresh();
  }

  public void CancelChore()
  {
    if (this.chore == null)
      return;
    this.chore.Cancel("Studyable.CancelChore");
    this.chore = (Chore) null;
  }

  public void Refresh()
  {
    if (KMonoBehaviour.isLoadingScene)
      return;
    KSelectable component = this.GetComponent<KSelectable>();
    if (this.studied)
    {
      this.statusItemGuid = component.ReplaceStatusItem(this.statusItemGuid, Db.Get().MiscStatusItems.Studied, (object) null);
      this.studiedIndicator.gameObject.SetActive(true);
      this.studiedIndicator.meterController.Play((HashedString) this.meterAnim, KAnim.PlayMode.Loop, 1f, 0.0f);
      this.requiredSkillPerk = (string) null;
      this.UpdateStatusItem((object) null);
    }
    else
    {
      if (this.markedForStudy)
      {
        if (this.chore == null)
          this.chore = (Chore) new WorkChore<Studyable>(Db.Get().ChoreTypes.Research, (IStateMachineTarget) this, (ChoreProvider) null, true, (System.Action<Chore>) null, (System.Action<Chore>) null, (System.Action<Chore>) null, true, (ScheduleBlockType) null, false, false, (KAnimFile) null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
        this.statusItemGuid = component.ReplaceStatusItem(this.statusItemGuid, Db.Get().MiscStatusItems.AwaitingStudy, (object) null);
      }
      else
      {
        this.CancelChore();
        this.statusItemGuid = component.RemoveStatusItem(this.statusItemGuid, false);
      }
      this.studiedIndicator.gameObject.SetActive(false);
    }
  }

  private void ToggleStudyChore()
  {
    if (DebugHandler.InstantBuildMode)
    {
      this.studied = true;
      if (this.chore != null)
      {
        this.chore.Cancel("debug");
        this.chore = (Chore) null;
      }
    }
    else
      this.markedForStudy = !this.markedForStudy;
    this.Refresh();
  }

  protected override void OnCompleteWork(Worker worker)
  {
    base.OnCompleteWork(worker);
    this.studied = true;
    this.chore = (Chore) null;
    this.Refresh();
  }

  public void OnSidescreenButtonPressed()
  {
    this.ToggleStudyChore();
  }
}
