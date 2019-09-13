// Decompiled with JetBrains decompiler
// Type: Database.MonumentBuilt
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections;
using System.IO;

namespace Database
{
  public class MonumentBuilt : VictoryColonyAchievementRequirement
  {
    public override string Name()
    {
      return (string) COLONY_ACHIEVEMENTS.THRIVING.REQUIREMENTS.BUILT_MONUMENT;
    }

    public override string Description()
    {
      return (string) COLONY_ACHIEVEMENTS.THRIVING.REQUIREMENTS.BUILT_MONUMENT_DESCRIPTION;
    }

    public override bool Success()
    {
      IEnumerator enumerator = Components.MonumentParts.GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
        {
          if (((MonumentPart) enumerator.Current).IsMonumentCompleted())
          {
            Game.Instance.unlocks.Unlock("thriving");
            return true;
          }
        }
      }
      finally
      {
        if (enumerator is IDisposable disposable)
          disposable.Dispose();
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
      return this.Name();
    }
  }
}
