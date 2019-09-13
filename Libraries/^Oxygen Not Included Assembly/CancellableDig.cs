// Decompiled with JetBrains decompiler
// Type: CancellableDig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

[SkipSaveFileSerialization]
public class CancellableDig : Cancellable
{
  protected override void OnCancel(object data)
  {
    EasingAnimations componentInChildren = this.GetComponentInChildren<EasingAnimations>();
    componentInChildren.OnAnimationDone += new System.Action<string>(this.OnAnimationDone);
    componentInChildren.PlayAnimation("ScaleDown", 0.1f);
  }

  private void OnAnimationDone(string animationName)
  {
    if (animationName != "ScaleDown")
      return;
    this.GetComponentInChildren<EasingAnimations>().OnAnimationDone -= new System.Action<string>(this.OnAnimationDone);
    this.DeleteObject();
  }
}
