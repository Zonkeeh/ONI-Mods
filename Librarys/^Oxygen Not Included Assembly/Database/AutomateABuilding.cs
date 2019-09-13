// Decompiled with JetBrains decompiler
// Type: Database.AutomateABuilding
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Database
{
  public class AutomateABuilding : ColonyAchievementRequirement
  {
    public override bool Success()
    {
      foreach (LogicCircuitNetwork network in (IEnumerable<UtilityNetwork>) Game.Instance.logicCircuitSystem.GetNetworks())
      {
        if (network.Receivers.Count > 0 && network.Senders.Count > 0)
        {
          bool flag1 = false;
          foreach (ILogicEventReceiver receiver in network.Receivers)
          {
            GameObject gameObject = Grid.Objects[receiver.GetLogicCell(), 1];
            if ((Object) gameObject != (Object) null && !gameObject.GetComponent<KPrefabID>().HasTag(GameTags.TemplateBuilding))
            {
              flag1 = true;
              break;
            }
          }
          bool flag2 = false;
          foreach (ILogicEventSender sender in network.Senders)
          {
            GameObject gameObject = Grid.Objects[sender.GetLogicCell(), 1];
            if ((Object) gameObject != (Object) null && !gameObject.GetComponent<KPrefabID>().HasTag(GameTags.TemplateBuilding))
            {
              flag2 = true;
              break;
            }
          }
          if (flag1 && flag2)
            return true;
        }
      }
      return false;
    }

    public override void Serialize(BinaryWriter writer)
    {
    }

    public override void Deserialize(IReader reader)
    {
    }

    public override string GetProgress(bool complete)
    {
      return (string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.AUTOMATE_A_BUILDING;
    }
  }
}
