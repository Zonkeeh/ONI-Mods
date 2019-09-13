// Decompiled with JetBrains decompiler
// Type: FullPuftTransitionLayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class FullPuftTransitionLayer : TransitionDriver.OverrideLayer
{
  public FullPuftTransitionLayer(Navigator navigator)
    : base(navigator)
  {
  }

  public override void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition)
  {
    base.BeginTransition(navigator, transition);
    CreatureCalorieMonitor.Instance smi = navigator.GetSMI<CreatureCalorieMonitor.Instance>();
    if (smi == null || !smi.stomach.IsReadyToPoop())
      return;
    KBatchedAnimController component = navigator.GetComponent<KBatchedAnimController>();
    string str = HashCache.Get().Get(transition.anim.HashValue) + "_full";
    if (!component.HasAnimation((HashedString) str))
      return;
    transition.anim = (HashedString) str;
  }
}
