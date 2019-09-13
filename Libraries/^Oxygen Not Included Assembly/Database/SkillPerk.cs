// Decompiled with JetBrains decompiler
// Type: Database.SkillPerk
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

namespace Database
{
  public class SkillPerk : Resource
  {
    public SkillPerk(
      string id_str,
      string description,
      System.Action<MinionResume> OnApply,
      System.Action<MinionResume> OnRemove,
      System.Action<MinionResume> OnMinionsChanged,
      bool affectAll = false)
      : base(id_str, description)
    {
      this.OnApply = OnApply;
      this.OnRemove = OnRemove;
      this.OnMinionsChanged = OnMinionsChanged;
      this.affectAll = affectAll;
    }

    public System.Action<MinionResume> OnApply { get; protected set; }

    public System.Action<MinionResume> OnRemove { get; protected set; }

    public System.Action<MinionResume> OnMinionsChanged { get; protected set; }

    public bool affectAll { get; protected set; }
  }
}
