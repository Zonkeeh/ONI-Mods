// Decompiled with JetBrains decompiler
// Type: Artable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using KSerialization;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class Artable : Workable
{
  [SerializeField]
  public List<Artable.Stage> stages = new List<Artable.Stage>();
  private Dictionary<Artable.Status, StatusItem> statuses;
  [Serialize]
  private string currentStage;
  private WorkChore<Artable> chore;

  protected Artable()
  {
    this.faceTargetWhenWorking = true;
    this.statuses = new Dictionary<Artable.Status, StatusItem>();
  }

  public Artable.Status CurrentStatus
  {
    get
    {
      foreach (Artable.Stage stage in this.stages)
      {
        if (this.CurrentStage == stage.id)
          return stage.statusItem;
      }
      return Artable.Status.Ready;
    }
  }

  public string CurrentStage
  {
    get
    {
      return this.currentStage;
    }
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.statuses[Artable.Status.Ready] = new StatusItem("AwaitingArting", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
    this.statuses[Artable.Status.Ugly] = new StatusItem("LookingUgly", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
    this.statuses[Artable.Status.Okay] = new StatusItem("LookingOkay", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
    this.statuses[Artable.Status.Great] = new StatusItem("LookingGreat", "BUILDING", string.Empty, StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022);
    this.workerStatusItem = Db.Get().DuplicantStatusItems.Arting;
    this.attributeConverter = Db.Get().AttributeConverters.ArtSpeed;
    this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
    this.skillExperienceSkillGroup = Db.Get().SkillGroups.Art.Id;
    this.skillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
    this.requiredSkillPerk = Db.Get().SkillPerks.CanArt.Id;
    this.SetWorkTime(80f);
  }

  protected override void OnSpawn()
  {
    if (string.IsNullOrEmpty(this.currentStage))
      this.currentStage = "Default";
    this.SetStage(this.currentStage, true);
    this.shouldShowSkillPerkStatusItem = false;
    if (this.currentStage == "Default")
    {
      this.shouldShowSkillPerkStatusItem = true;
      Prioritizable.AddRef(this.gameObject);
      this.chore = new WorkChore<Artable>(Db.Get().ChoreTypes.Art, (IStateMachineTarget) this, (ChoreProvider) null, true, (System.Action<Chore>) null, (System.Action<Chore>) null, (System.Action<Chore>) null, true, (ScheduleBlockType) null, false, true, (KAnimFile) null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
      this.chore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, (object) this.requiredSkillPerk);
    }
    base.OnSpawn();
  }

  protected override void OnCompleteWork(Worker worker)
  {
    Artable.Status artist_skill = Artable.Status.Ugly;
    MinionResume component = worker.GetComponent<MinionResume>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
    {
      if (component.HasPerk((HashedString) Db.Get().SkillPerks.CanArtGreat.Id))
        artist_skill = Artable.Status.Great;
      else if (component.HasPerk((HashedString) Db.Get().SkillPerks.CanArtOkay.Id))
        artist_skill = Artable.Status.Okay;
    }
    List<Artable.Stage> potential_stages = new List<Artable.Stage>();
    this.stages.ForEach((System.Action<Artable.Stage>) (item => potential_stages.Add(item)));
    potential_stages.RemoveAll((Predicate<Artable.Stage>) (x =>
    {
      if (x.statusItem <= artist_skill)
        return x.id == "Default";
      return true;
    }));
    potential_stages.Sort((Comparison<Artable.Stage>) ((x, y) => y.statusItem.CompareTo((object) x.statusItem)));
    Artable.Status highest_status = potential_stages[0].statusItem;
    potential_stages.RemoveAll((Predicate<Artable.Stage>) (x => x.statusItem < highest_status));
    potential_stages.Shuffle<Artable.Stage>();
    this.SetStage(potential_stages[0].id, false);
    if (potential_stages[0].cheerOnComplete)
    {
      EmoteChore emoteChore1 = new EmoteChore((IStateMachineTarget) worker.GetComponent<ChoreProvider>(), Db.Get().ChoreTypes.EmoteHighPriority, (HashedString) "anim_cheer_kanim", new HashedString[3]
      {
        (HashedString) "cheer_pre",
        (HashedString) "cheer_loop",
        (HashedString) "cheer_pst"
      }, (Func<StatusItem>) null);
    }
    else
    {
      EmoteChore emoteChore2 = new EmoteChore((IStateMachineTarget) worker.GetComponent<ChoreProvider>(), Db.Get().ChoreTypes.EmoteHighPriority, (HashedString) "anim_disappointed_kanim", new HashedString[3]
      {
        (HashedString) "disappointed_pre",
        (HashedString) "disappointed_loop",
        (HashedString) "disappointed_pst"
      }, (Func<StatusItem>) null);
    }
    this.shouldShowSkillPerkStatusItem = false;
    this.UpdateStatusItem((object) null);
    Prioritizable.RemoveRef(this.gameObject);
  }

  public virtual void SetStage(string stage_id, bool skip_effect)
  {
    Artable.Stage stage = (Artable.Stage) null;
    for (int index = 0; index < this.stages.Count; ++index)
    {
      if (this.stages[index].id == stage_id)
      {
        stage = this.stages[index];
        break;
      }
    }
    if (stage == null)
    {
      Debug.LogError((object) ("Missing stage: " + stage_id));
    }
    else
    {
      this.currentStage = stage.id;
      this.GetComponent<KAnimControllerBase>().Play((HashedString) stage.anim, KAnim.PlayMode.Once, 1f, 0.0f);
      if (stage.decor != 0)
        this.GetAttributes().Add(new AttributeModifier(Db.Get().BuildingAttributes.Decor.Id, (float) stage.decor, "Art Quality", false, false, true));
      KSelectable component = this.GetComponent<KSelectable>();
      component.SetName(stage.name);
      component.SetStatusItem(Db.Get().StatusItemCategories.Main, this.statuses[stage.statusItem], (object) this);
      this.shouldShowSkillPerkStatusItem = false;
      this.UpdateStatusItem((object) null);
    }
  }

  [Serializable]
  public class Stage
  {
    public string id;
    public string name;
    public string anim;
    public int decor;
    public bool cheerOnComplete;
    public Artable.Status statusItem;

    public Stage(
      string id,
      string name,
      string anim,
      int decor_value,
      bool cheer_on_complete,
      Artable.Status status_item)
    {
      this.id = id;
      this.name = name;
      this.anim = anim;
      this.decor = decor_value;
      this.cheerOnComplete = cheer_on_complete;
      this.statusItem = status_item;
    }
  }

  public enum Status
  {
    Ready,
    Ugly,
    Okay,
    Great,
  }
}
