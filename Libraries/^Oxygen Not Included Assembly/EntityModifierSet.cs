// Decompiled with JetBrains decompiler
// Type: EntityModifierSet
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;

public class EntityModifierSet : ModifierSet
{
  public DuplicantStatusItems DuplicantStatusItems;
  public ChoreGroups ChoreGroups;

  public override void Initialize()
  {
    base.Initialize();
    this.DuplicantStatusItems = new DuplicantStatusItems(this.Root);
    this.ChoreGroups = new ChoreGroups(this.Root);
    this.LoadTraits();
  }
}
