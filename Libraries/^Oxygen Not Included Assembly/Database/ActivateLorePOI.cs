// Decompiled with JetBrains decompiler
// Type: Database.ActivateLorePOI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.IO;
using UnityEngine;

namespace Database
{
  public class ActivateLorePOI : ColonyAchievementRequirement
  {
    public override void Deserialize(IReader reader)
    {
    }

    public override void Serialize(BinaryWriter writer)
    {
    }

    public override bool Success()
    {
      foreach (BuildingComplete buildingComplete in Components.BuildingCompletes.Items)
      {
        if (buildingComplete.GetComponent<KPrefabID>().HasTag(GameTags.TemplateBuilding))
        {
          Unsealable component = buildingComplete.GetComponent<Unsealable>();
          if ((Object) component != (Object) null && component.unsealed)
            return true;
        }
      }
      return false;
    }

    public override string GetProgress(bool complete)
    {
      return (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.INVESTIGATE_A_POI;
    }
  }
}
