// Decompiled with JetBrains decompiler
// Type: Database.CoolBuildingToXKelvin
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.IO;
using UnityEngine;

namespace Database
{
  public class CoolBuildingToXKelvin : ColonyAchievementRequirement
  {
    private int kelvinToCoolTo;

    public CoolBuildingToXKelvin(int kelvinToCoolTo)
    {
      this.kelvinToCoolTo = kelvinToCoolTo;
    }

    public override bool Success()
    {
      foreach (Component component in Components.BuildingCompletes.Items)
      {
        if ((double) component.GetComponent<PrimaryElement>().Temperature <= (double) this.kelvinToCoolTo)
          return true;
      }
      return false;
    }

    public override void Deserialize(IReader reader)
    {
      this.kelvinToCoolTo = reader.ReadInt32();
    }

    public override void Serialize(BinaryWriter writer)
    {
      writer.Write(this.kelvinToCoolTo);
    }

    public override string GetProgress(bool complete)
    {
      float val1 = float.MaxValue;
      foreach (BuildingComplete buildingComplete in Components.BuildingCompletes.Items)
        val1 = Math.Min(val1, buildingComplete.GetComponent<PrimaryElement>().Temperature);
      return string.Format((string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.KELVIN_COOLING, (object) val1);
    }
  }
}
