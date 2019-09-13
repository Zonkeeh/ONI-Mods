// Decompiled with JetBrains decompiler
// Type: Klei.AI.HeatRash
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;

namespace Klei.AI
{
  public class HeatRash : Sickness
  {
    public const string ID = "HeatSickness";

    public HeatRash()
      : base("HeatSickness", Sickness.SicknessType.Ailment, Sickness.Severity.Minor, 0.005f, new List<Sickness.InfectionVector>()
      {
        Sickness.InfectionVector.Inhalation
      }, 180f, (string) null)
    {
      this.AddSicknessComponent((Sickness.SicknessComponent) new CommonSickEffectSickness());
      this.AddSicknessComponent((Sickness.SicknessComponent) new AttributeModifierSickness(new AttributeModifier[4]
      {
        new AttributeModifier("Learning", -5f, (string) DUPLICANTS.DISEASES.HEATSICKNESS.NAME, false, false, true),
        new AttributeModifier("Machinery", -5f, (string) DUPLICANTS.DISEASES.HEATSICKNESS.NAME, false, false, true),
        new AttributeModifier("Construction", -5f, (string) DUPLICANTS.DISEASES.HEATSICKNESS.NAME, false, false, true),
        new AttributeModifier("Cooking", -5f, (string) DUPLICANTS.DISEASES.HEATSICKNESS.NAME, false, false, true)
      }));
      this.AddSicknessComponent((Sickness.SicknessComponent) new AnimatedSickness(new HashedString[3]
      {
        (HashedString) "anim_idle_hot_kanim",
        (HashedString) "anim_loco_run_hot_kanim",
        (HashedString) "anim_loco_walk_hot_kanim"
      }, Db.Get().Expressions.SickFierySkin));
      this.AddSicknessComponent((Sickness.SicknessComponent) new PeriodicEmoteSickness((HashedString) "anim_idle_hot_kanim", new HashedString[3]
      {
        (HashedString) "idle_pre",
        (HashedString) "idle_default",
        (HashedString) "idle_pst"
      }, 15f));
    }
  }
}
