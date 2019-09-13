// Decompiled with JetBrains decompiler
// Type: Database.Deaths
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;

namespace Database
{
  public class Deaths : ResourceSet<Death>
  {
    public Death Generic;
    public Death Frozen;
    public Death Suffocation;
    public Death Starvation;
    public Death Slain;
    public Death Overheating;
    public Death Drowned;
    public Death Explosion;
    public Death FatalDisease;

    public Deaths(ResourceSet parent)
      : base(nameof (Deaths), parent)
    {
      this.Generic = new Death(nameof (Generic), (ResourceSet) this, (string) DUPLICANTS.DEATHS.GENERIC.NAME, (string) DUPLICANTS.DEATHS.GENERIC.DESCRIPTION, "dead_on_back", "dead_on_back");
      this.Frozen = new Death(nameof (Frozen), (ResourceSet) this, (string) DUPLICANTS.DEATHS.FROZEN.NAME, (string) DUPLICANTS.DEATHS.FROZEN.DESCRIPTION, "death_freeze_trans", "death_freeze_solid");
      this.Suffocation = new Death(nameof (Suffocation), (ResourceSet) this, (string) DUPLICANTS.DEATHS.SUFFOCATION.NAME, (string) DUPLICANTS.DEATHS.SUFFOCATION.DESCRIPTION, "death_suffocation", "dead_on_back");
      this.Starvation = new Death(nameof (Starvation), (ResourceSet) this, (string) DUPLICANTS.DEATHS.STARVATION.NAME, (string) DUPLICANTS.DEATHS.STARVATION.DESCRIPTION, "dead_on_back", "dead_on_back");
      this.Overheating = new Death(nameof (Overheating), (ResourceSet) this, (string) DUPLICANTS.DEATHS.OVERHEATING.NAME, (string) DUPLICANTS.DEATHS.OVERHEATING.DESCRIPTION, "dead_on_back", "dead_on_back");
      this.Drowned = new Death(nameof (Drowned), (ResourceSet) this, (string) DUPLICANTS.DEATHS.DROWNED.NAME, (string) DUPLICANTS.DEATHS.DROWNED.DESCRIPTION, "death_suffocation", "dead_on_back");
      this.Explosion = new Death(nameof (Explosion), (ResourceSet) this, (string) DUPLICANTS.DEATHS.EXPLOSION.NAME, (string) DUPLICANTS.DEATHS.EXPLOSION.DESCRIPTION, "dead_on_back", "dead_on_back");
      this.Slain = new Death("Combat", (ResourceSet) this, (string) DUPLICANTS.DEATHS.COMBAT.NAME, (string) DUPLICANTS.DEATHS.COMBAT.DESCRIPTION, "dead_on_back", "dead_on_back");
      this.FatalDisease = new Death(nameof (FatalDisease), (ResourceSet) this, (string) DUPLICANTS.DEATHS.FATALDISEASE.NAME, (string) DUPLICANTS.DEATHS.FATALDISEASE.DESCRIPTION, "dead_on_back", "dead_on_back");
    }
  }
}
