// Decompiled with JetBrains decompiler
// Type: Database.CreateMasterPainting
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.IO;
using UnityEngine;

namespace Database
{
  public class CreateMasterPainting : ColonyAchievementRequirement
  {
    public override bool Success()
    {
      foreach (BuildingComplete buildingComplete in Components.BuildingCompletes.Items)
      {
        if (buildingComplete.isArtable)
        {
          Painting component = buildingComplete.GetComponent<Painting>();
          if ((Object) component != (Object) null && component.CurrentStatus == Artable.Status.Great)
            return true;
        }
      }
      return false;
    }

    public override void Deserialize(IReader reader)
    {
    }

    public override void Serialize(BinaryWriter writer)
    {
    }

    public override string GetProgress(bool complete)
    {
      return (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.CREATE_A_PAINTING;
    }
  }
}
