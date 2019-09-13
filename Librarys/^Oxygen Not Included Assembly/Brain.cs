// Decompiled with JetBrains decompiler
// Type: Brain
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class Brain : KMonoBehaviour
{
  private bool running;
  private bool suspend;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
  }

  protected override void OnSpawn()
  {
    this.running = true;
    Components.Brains.Add(this);
  }

  public event System.Action onPreUpdate;

  public virtual void UpdateBrain()
  {
    if (this.onPreUpdate != null)
      this.onPreUpdate();
    if (!this.IsRunning())
      return;
    this.UpdateChores();
  }

  private bool FindBetterChore(ref Chore.Precondition.Context context)
  {
    return this.GetComponent<ChoreConsumer>().FindNextChore(ref context);
  }

  private void UpdateChores()
  {
    if (this.GetComponent<KPrefabID>().HasTag(GameTags.PreventChoreInterruption))
      return;
    Chore.Precondition.Context context = new Chore.Precondition.Context();
    if (!this.FindBetterChore(ref context))
      return;
    if (this.HasTag(GameTags.PerformingWorkRequest))
      this.Trigger(1485595942, (object) null);
    else
      this.GetComponent<ChoreDriver>().SetChore(context);
  }

  public bool IsRunning()
  {
    if (this.running)
      return !this.suspend;
    return false;
  }

  public void Reset(string reason)
  {
    this.Stop(nameof (Reset));
    this.running = true;
  }

  public void Stop(string reason)
  {
    this.GetComponent<ChoreDriver>().StopChore();
    this.running = false;
  }

  public void Resume(string caller)
  {
    this.suspend = false;
  }

  public void Suspend(string caller)
  {
    this.suspend = true;
  }

  protected override void OnCmpDisable()
  {
    base.OnCmpDisable();
    this.Stop(nameof (OnCmpDisable));
  }

  protected override void OnCleanUp()
  {
    this.Stop(nameof (OnCleanUp));
    Components.Brains.Remove(this);
  }
}
