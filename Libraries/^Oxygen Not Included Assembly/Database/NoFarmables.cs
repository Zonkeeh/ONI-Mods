// Decompiled with JetBrains decompiler
// Type: Database.NoFarmables
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.IO;
using UnityEngine;

namespace Database
{
  public class NoFarmables : ColonyAchievementRequirement
  {
    public override bool Success()
    {
      foreach (PlantablePlot plantablePlot in Components.PlantablePlots.Items)
      {
        if ((Object) plantablePlot.Occupant != (Object) null)
        {
          foreach (Tag depositObjectTag in plantablePlot.possibleDepositObjectTags)
          {
            if (depositObjectTag != GameTags.DecorSeed)
              return false;
          }
        }
      }
      return true;
    }

    public override bool Fail()
    {
      return !this.Success();
    }

    public override void Deserialize(IReader reader)
    {
    }

    public override void Serialize(BinaryWriter writer)
    {
    }

    public override string GetProgress(bool complete)
    {
      return (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.NO_FARM_TILES;
    }
  }
}
