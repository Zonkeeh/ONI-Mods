// Decompiled with JetBrains decompiler
// Type: Database.VentXKG
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Database
{
  public class VentXKG : ColonyAchievementRequirement
  {
    private SimHashes element;
    private float kilogramsToVent;

    public VentXKG(SimHashes element, float kilogramsToVent)
    {
      this.element = element;
      this.kilogramsToVent = kilogramsToVent;
    }

    public override bool Success()
    {
      float num = 0.0f;
      foreach (UtilityNetwork network in (IEnumerable<UtilityNetwork>) Conduit.GetNetworkManager(ConduitType.Gas).GetNetworks())
      {
        FlowUtilityNetwork flowUtilityNetwork = network as FlowUtilityNetwork;
        if (flowUtilityNetwork != null)
        {
          foreach (FlowUtilityNetwork.IItem sink in flowUtilityNetwork.sinks)
          {
            Vent component = sink.GameObject.GetComponent<Vent>();
            if ((Object) component != (Object) null)
              num += component.GetVentedMass(this.element);
          }
        }
      }
      return (double) num >= (double) this.kilogramsToVent;
    }

    public override void Serialize(BinaryWriter writer)
    {
      writer.Write((int) this.element);
      writer.Write(this.kilogramsToVent);
    }

    public override void Deserialize(IReader reader)
    {
      this.element = (SimHashes) reader.ReadInt32();
      this.kilogramsToVent = reader.ReadSingle();
    }

    public override string GetProgress(bool complete)
    {
      float num = 0.0f;
      foreach (UtilityNetwork network in (IEnumerable<UtilityNetwork>) Conduit.GetNetworkManager(ConduitType.Gas).GetNetworks())
      {
        FlowUtilityNetwork flowUtilityNetwork = network as FlowUtilityNetwork;
        if (flowUtilityNetwork != null)
        {
          foreach (FlowUtilityNetwork.IItem sink in flowUtilityNetwork.sinks)
          {
            Vent component = sink.GameObject.GetComponent<Vent>();
            if ((Object) component != (Object) null)
              num += component.GetVentedMass(this.element);
          }
        }
      }
      return string.Format((string) COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.VENTED_MASS, (object) GameUtil.GetFormattedMass(!complete ? num : this.kilogramsToVent, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Kilogram, true, "{0:0.#}"), (object) GameUtil.GetFormattedMass(this.kilogramsToVent, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Kilogram, true, "{0:0.#}"));
    }
  }
}
