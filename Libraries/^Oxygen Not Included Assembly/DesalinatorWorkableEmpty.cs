// Decompiled with JetBrains decompiler
// Type: DesalinatorWorkableEmpty
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using TUNING;

public class DesalinatorWorkableEmpty : Workable
{
  private static readonly HashedString[] WORK_ANIMS = new HashedString[2]
  {
    (HashedString) "salt_pre",
    (HashedString) "salt_loop"
  };
  private static readonly HashedString PST_ANIM = new HashedString("salt_pst");
  [Serialize]
  public int timesCleaned;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.workerStatusItem = Db.Get().DuplicantStatusItems.Cleaning;
    this.workingStatusItem = Db.Get().MiscStatusItems.Cleaning;
    this.attributeConverter = Db.Get().AttributeConverters.TidyingSpeed;
    this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
    this.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_desalinator_kanim")
    };
    this.workAnims = DesalinatorWorkableEmpty.WORK_ANIMS;
    this.workingPstComplete = DesalinatorWorkableEmpty.PST_ANIM;
    this.workingPstFailed = DesalinatorWorkableEmpty.PST_ANIM;
    this.synchronizeAnims = false;
  }

  protected override void OnCompleteWork(Worker worker)
  {
    ++this.timesCleaned;
    base.OnCompleteWork(worker);
  }
}
