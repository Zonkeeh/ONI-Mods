// Decompiled with JetBrains decompiler
// Type: Klei.AI.TraitGroup
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

namespace Klei.AI
{
  public class TraitGroup : ModifierGroup<Trait>
  {
    public bool IsSpawnTrait;

    public TraitGroup(string id, string name, bool is_spawn_trait)
      : base(id, name)
    {
      this.IsSpawnTrait = is_spawn_trait;
    }
  }
}
