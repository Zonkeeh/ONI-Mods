// Decompiled with JetBrains decompiler
// Type: Klei.AI.Allergies
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;

namespace Klei.AI
{
  public class Allergies : Sickness
  {
    public const string ID = "Allergies";
    public const float STRESS_PER_CYCLE = 15f;

    public Allergies()
      : base(nameof (Allergies), Sickness.SicknessType.Pathogen, Sickness.Severity.Minor, 0.00025f, new List<Sickness.InfectionVector>()
      {
        Sickness.InfectionVector.Inhalation
      }, 60f, (string) null)
    {
      float num = 0.025f;
      this.AddSicknessComponent((Sickness.SicknessComponent) new CommonSickEffectSickness());
      this.AddSicknessComponent((Sickness.SicknessComponent) new AnimatedSickness(new HashedString[1]
      {
        (HashedString) "anim_idle_allergies_kanim"
      }, Db.Get().Expressions.Uncomfortable));
      this.AddSicknessComponent((Sickness.SicknessComponent) new AttributeModifierSickness(new AttributeModifier[2]
      {
        new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, num, (string) DUPLICANTS.DISEASES.ALLERGIES.NAME, false, false, true),
        new AttributeModifier(Db.Get().Attributes.Sneezyness.Id, 10f, (string) DUPLICANTS.DISEASES.ALLERGIES.NAME, false, false, true)
      }));
    }
  }
}
