// Decompiled with JetBrains decompiler
// Type: Database.ScheduleBlockTypes
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

namespace Database
{
  public class ScheduleBlockTypes : ResourceSet<ScheduleBlockType>
  {
    public ScheduleBlockType Sleep;
    public ScheduleBlockType Eat;
    public ScheduleBlockType Work;
    public ScheduleBlockType Hygiene;
    public ScheduleBlockType Recreation;

    public ScheduleBlockTypes(ResourceSet parent)
      : base(nameof (ScheduleBlockTypes), parent)
    {
      this.Sleep = this.Add(new ScheduleBlockType(nameof (Sleep), (ResourceSet) this, (string) UI.SCHEDULEBLOCKTYPES.SLEEP.NAME, (string) UI.SCHEDULEBLOCKTYPES.SLEEP.DESCRIPTION, new Color(0.9843137f, 0.9921569f, 0.2705882f)));
      this.Eat = this.Add(new ScheduleBlockType(nameof (Eat), (ResourceSet) this, (string) UI.SCHEDULEBLOCKTYPES.EAT.NAME, (string) UI.SCHEDULEBLOCKTYPES.EAT.DESCRIPTION, new Color(0.8078431f, 0.5294118f, 0.1137255f)));
      this.Work = this.Add(new ScheduleBlockType(nameof (Work), (ResourceSet) this, (string) UI.SCHEDULEBLOCKTYPES.WORK.NAME, (string) UI.SCHEDULEBLOCKTYPES.WORK.DESCRIPTION, new Color(0.9372549f, 0.1294118f, 0.1294118f)));
      this.Hygiene = this.Add(new ScheduleBlockType(nameof (Hygiene), (ResourceSet) this, (string) UI.SCHEDULEBLOCKTYPES.HYGIENE.NAME, (string) UI.SCHEDULEBLOCKTYPES.HYGIENE.DESCRIPTION, new Color(0.4588235f, 0.1764706f, 0.345098f)));
      this.Recreation = this.Add(new ScheduleBlockType(nameof (Recreation), (ResourceSet) this, (string) UI.SCHEDULEBLOCKTYPES.RECREATION.NAME, (string) UI.SCHEDULEBLOCKTYPES.RECREATION.DESCRIPTION, new Color(0.4588235f, 0.372549f, 0.1882353f)));
    }
  }
}
