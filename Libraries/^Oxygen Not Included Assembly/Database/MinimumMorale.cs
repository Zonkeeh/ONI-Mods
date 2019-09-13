// Decompiled with JetBrains decompiler
// Type: Database.MinimumMorale
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections;
using System.IO;
using UnityEngine;

namespace Database
{
  public class MinimumMorale : VictoryColonyAchievementRequirement
  {
    public int minimumMorale;

    public MinimumMorale(int minimumMorale = 16)
    {
      this.minimumMorale = minimumMorale;
    }

    public override string Name()
    {
      return string.Format((string) COLONY_ACHIEVEMENTS.THRIVING.REQUIREMENTS.MINIMUM_MORALE, (object) this.minimumMorale);
    }

    public override string Description()
    {
      return string.Format((string) COLONY_ACHIEVEMENTS.THRIVING.REQUIREMENTS.MINIMUM_MORALE_DESCRIPTION, (object) this.minimumMorale);
    }

    public override bool Success()
    {
      bool flag = true;
      IEnumerator enumerator = Components.MinionAssignablesProxy.GetEnumerator();
      try
      {
        while (enumerator.MoveNext())
        {
          GameObject targetGameObject = ((MinionAssignablesProxy) enumerator.Current).GetTargetGameObject();
          if ((UnityEngine.Object) targetGameObject != (UnityEngine.Object) null && !targetGameObject.HasTag(GameTags.Dead))
          {
            AttributeInstance attributeInstance = Db.Get().Attributes.QualityOfLife.Lookup((Component) targetGameObject.GetComponent<MinionModifiers>());
            flag = attributeInstance != null && (double) attributeInstance.GetTotalValue() >= (double) this.minimumMorale && flag;
          }
        }
      }
      finally
      {
        if (enumerator is IDisposable disposable)
          disposable.Dispose();
      }
      return flag;
    }

    public override void Serialize(BinaryWriter writer)
    {
      writer.Write(this.minimumMorale);
    }

    public override void Deserialize(IReader reader)
    {
      this.minimumMorale = reader.ReadInt32();
    }

    public override string GetProgress(bool complete)
    {
      return this.Description();
    }
  }
}
