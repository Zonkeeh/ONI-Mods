// Decompiled with JetBrains decompiler
// Type: TargetScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public abstract class TargetScreen : KScreen
{
  protected GameObject selectedTarget;

  public abstract bool IsValidForTarget(GameObject target);

  public void SetTarget(GameObject target)
  {
    if (!((UnityEngine.Object) this.selectedTarget != (UnityEngine.Object) target))
      return;
    if ((UnityEngine.Object) this.selectedTarget != (UnityEngine.Object) null)
      this.OnDeselectTarget(this.selectedTarget);
    this.selectedTarget = target;
    if (!((UnityEngine.Object) this.selectedTarget != (UnityEngine.Object) null))
      return;
    this.OnSelectTarget(this.selectedTarget);
  }

  protected override void OnDeactivate()
  {
    base.OnDeactivate();
    this.SetTarget((GameObject) null);
  }

  public virtual void OnSelectTarget(GameObject target)
  {
    target.Subscribe(1502190696, new System.Action<object>(this.OnTargetDestroyed));
  }

  public virtual void OnDeselectTarget(GameObject target)
  {
    target.Unsubscribe(1502190696, new System.Action<object>(this.OnTargetDestroyed));
  }

  private void OnTargetDestroyed(object data)
  {
    DetailsScreen.Instance.Show(false);
    this.SetTarget((GameObject) null);
  }
}
