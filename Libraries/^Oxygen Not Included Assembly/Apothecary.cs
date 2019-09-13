// Decompiled with JetBrains decompiler
// Type: Apothecary
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class Apothecary : ComplexFabricator
{
  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.choreType = Db.Get().ChoreTypes.Compound;
    this.fetchChoreTypeIdHash = Db.Get().ChoreTypes.DoctorFetch.IdHash;
    this.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.workable.WorkerStatusItem = Db.Get().DuplicantStatusItems.Fabricating;
    this.workable.AttributeConverter = Db.Get().AttributeConverters.CompoundingSpeed;
    this.workable.SkillExperienceSkillGroup = Db.Get().SkillGroups.MedicalAid.Id;
    this.workable.SkillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
    this.workable.requiredSkillPerk = Db.Get().SkillPerks.CanCompound.Id;
    this.workable.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_apothecary_kanim")
    };
    this.workable.AnimOffset = new Vector3(-1f, 0.0f, 0.0f);
  }
}
