﻿// Decompiled with JetBrains decompiler
// Type: Expectations
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System.Collections.Generic;

public static class Expectations
{
  public static List<Expectation[]> ExpectationsByTier = new List<Expectation[]>()
  {
    new Expectation[1]
    {
      (Expectation) Expectations.QOLExpectation(1, (string) UI.ROLES_SCREEN.EXPECTATIONS.QUALITYOFLIFE.TIER0.NAME, (string) UI.ROLES_SCREEN.EXPECTATIONS.QUALITYOFLIFE.TIER0.DESCRIPTION)
    },
    new Expectation[1]
    {
      (Expectation) Expectations.QOLExpectation(2, (string) UI.ROLES_SCREEN.EXPECTATIONS.QUALITYOFLIFE.TIER1.NAME, (string) UI.ROLES_SCREEN.EXPECTATIONS.QUALITYOFLIFE.TIER1.DESCRIPTION)
    },
    new Expectation[1]
    {
      (Expectation) Expectations.QOLExpectation(4, (string) UI.ROLES_SCREEN.EXPECTATIONS.QUALITYOFLIFE.TIER2.NAME, (string) UI.ROLES_SCREEN.EXPECTATIONS.QUALITYOFLIFE.TIER2.DESCRIPTION)
    },
    new Expectation[1]
    {
      (Expectation) Expectations.QOLExpectation(8, (string) UI.ROLES_SCREEN.EXPECTATIONS.QUALITYOFLIFE.TIER3.NAME, (string) UI.ROLES_SCREEN.EXPECTATIONS.QUALITYOFLIFE.TIER3.DESCRIPTION)
    },
    new Expectation[1]
    {
      (Expectation) Expectations.QOLExpectation(12, (string) UI.ROLES_SCREEN.EXPECTATIONS.QUALITYOFLIFE.TIER4.NAME, (string) UI.ROLES_SCREEN.EXPECTATIONS.QUALITYOFLIFE.TIER4.DESCRIPTION)
    },
    new Expectation[1]
    {
      (Expectation) Expectations.QOLExpectation(16, (string) UI.ROLES_SCREEN.EXPECTATIONS.QUALITYOFLIFE.TIER5.NAME, (string) UI.ROLES_SCREEN.EXPECTATIONS.QUALITYOFLIFE.TIER5.DESCRIPTION)
    },
    new Expectation[1]
    {
      (Expectation) Expectations.QOLExpectation(20, (string) UI.ROLES_SCREEN.EXPECTATIONS.QUALITYOFLIFE.TIER6.NAME, (string) UI.ROLES_SCREEN.EXPECTATIONS.QUALITYOFLIFE.TIER6.DESCRIPTION)
    },
    new Expectation[1]
    {
      (Expectation) Expectations.QOLExpectation(25, (string) UI.ROLES_SCREEN.EXPECTATIONS.QUALITYOFLIFE.TIER7.NAME, (string) UI.ROLES_SCREEN.EXPECTATIONS.QUALITYOFLIFE.TIER7.DESCRIPTION)
    },
    new Expectation[1]
    {
      (Expectation) Expectations.QOLExpectation(30, (string) UI.ROLES_SCREEN.EXPECTATIONS.QUALITYOFLIFE.TIER8.NAME, (string) UI.ROLES_SCREEN.EXPECTATIONS.QUALITYOFLIFE.TIER8.DESCRIPTION)
    }
  };

  private static AttributeModifier QOLModifier(int level)
  {
    return new AttributeModifier(Db.Get().Attributes.QualityOfLifeExpectation.Id, (float) level, (string) DUPLICANTS.NEEDS.QUALITYOFLIFE.EXPECTATION_MOD_NAME, false, false, true);
  }

  private static AttributeModifierExpectation QOLExpectation(
    int level,
    string name,
    string description)
  {
    return new AttributeModifierExpectation("QOL_" + level.ToString(), name, description, Expectations.QOLModifier(level), Assets.GetSprite((HashedString) "icon_category_morale"));
  }
}