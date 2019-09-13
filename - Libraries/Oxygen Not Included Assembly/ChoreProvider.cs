// Decompiled with JetBrains decompiler
// Type: ChoreProvider
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

[SkipSaveFileSerialization]
public class ChoreProvider : KMonoBehaviour
{
  public List<Chore> chores = new List<Chore>();

  public string Name { get; private set; }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Name = this.name;
  }

  public virtual void AddChore(Chore chore)
  {
    chore.provider = this;
    this.chores.Add(chore);
  }

  public virtual void RemoveChore(Chore chore)
  {
    if (chore == null)
      return;
    chore.provider = (ChoreProvider) null;
    this.chores.Remove(chore);
  }

  public virtual void CollectChores(
    ChoreConsumerState consumer_state,
    List<Chore.Precondition.Context> succeeded,
    List<Chore.Precondition.Context> failed_contexts)
  {
    for (int index = 0; index < this.chores.Count; ++index)
      this.chores[index].CollectChores(consumer_state, succeeded, failed_contexts, false);
  }
}
