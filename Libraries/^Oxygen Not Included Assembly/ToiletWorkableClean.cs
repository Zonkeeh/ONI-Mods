// Decompiled with JetBrains decompiler
// Type: ToiletWorkableClean
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using TUNING;

public class ToiletWorkableClean : Workable
{
  private static readonly HashedString[] CLEAN_ANIMS = new HashedString[2]
  {
    (HashedString) "unclog_pre",
    (HashedString) "unclog_loop"
  };
  private static readonly HashedString PST_ANIM = new HashedString("unclog_pst");
  [Serialize]
  public int timesCleaned;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.workerStatusItem = Db.Get().DuplicantStatusItems.Cleaning;
    this.workingStatusItem = Db.Get().MiscStatusItems.Cleaning;
    this.attributeConverter = Db.Get().AttributeConverters.TidyingSpeed;
    this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
    this.skillExperienceSkillGroup = Db.Get().SkillGroups.Basekeeping.Id;
    this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
    this.workAnims = ToiletWorkableClean.CLEAN_ANIMS;
    this.workingPstComplete = ToiletWorkableClean.PST_ANIM;
    this.workingPstFailed = ToiletWorkableClean.PST_ANIM;
  }

  protected override void OnCompleteWork(Worker worker)
  {
    ++this.timesCleaned;
    base.OnCompleteWork(worker);
  }
}
