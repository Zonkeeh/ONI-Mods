// Decompiled with JetBrains decompiler
// Type: OneshotReactableHost
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class OneshotReactableHost : KMonoBehaviour
{
  public float lifetime = 1f;
  private Reactable reactable;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    GameScheduler.Instance.Schedule("CleanupOneshotReactable", this.lifetime, new System.Action<object>(this.OnExpire), (object) null, (SchedulerGroup) null);
  }

  public void SetReactable(Reactable reactable)
  {
    this.reactable = reactable;
  }

  private void OnExpire(object obj)
  {
    if (!this.reactable.IsReacting)
    {
      this.reactable.Cleanup();
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    }
    else
      GameScheduler.Instance.Schedule("CleanupOneshotReactable", 0.5f, new System.Action<object>(this.OnExpire), (object) null, (SchedulerGroup) null);
  }
}
