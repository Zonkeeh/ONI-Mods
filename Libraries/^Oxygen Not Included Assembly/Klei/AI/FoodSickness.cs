// Decompiled with JetBrains decompiler
// Type: Klei.AI.FoodSickness
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;

namespace Klei.AI
{
  public class FoodSickness : Sickness
  {
    public const string ID = "FoodSickness";
    public const string RECOVERY_ID = "FoodSicknessRecovery";
    private const float VOMIT_FREQUENCY = 200f;

    public FoodSickness()
      : base(nameof (FoodSickness), Sickness.SicknessType.Pathogen, Sickness.Severity.Minor, 0.005f, new List<Sickness.InfectionVector>()
      {
        Sickness.InfectionVector.Digestion
      }, 1020f, "FoodSicknessRecovery")
    {
      this.AddSicknessComponent((Sickness.SicknessComponent) new CommonSickEffectSickness());
      this.AddSicknessComponent((Sickness.SicknessComponent) new AttributeModifierSickness(new AttributeModifier[3]
      {
        new AttributeModifier("BladderDelta", 0.3333333f, (string) DUPLICANTS.DISEASES.FOODSICKNESS.NAME, false, false, true),
        new AttributeModifier("ToiletEfficiency", -0.2f, (string) DUPLICANTS.DISEASES.FOODSICKNESS.NAME, false, false, true),
        new AttributeModifier("StaminaDelta", -0.05f, (string) DUPLICANTS.DISEASES.FOODSICKNESS.NAME, false, false, true)
      }));
      this.AddSicknessComponent((Sickness.SicknessComponent) new AnimatedSickness(new HashedString[1]
      {
        (HashedString) "anim_idle_sick_kanim"
      }, Db.Get().Expressions.Sick));
      this.AddSicknessComponent((Sickness.SicknessComponent) new PeriodicEmoteSickness((HashedString) "anim_idle_sick_kanim", new HashedString[3]
      {
        (HashedString) "idle_pre",
        (HashedString) "idle_default",
        (HashedString) "idle_pst"
      }, 10f));
    }
  }
}
