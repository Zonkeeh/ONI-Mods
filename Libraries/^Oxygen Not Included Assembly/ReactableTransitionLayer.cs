// Decompiled with JetBrains decompiler
// Type: ReactableTransitionLayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

public class ReactableTransitionLayer : TransitionDriver.OverrideLayer
{
  public ReactableTransitionLayer(Navigator navigator)
    : base(navigator)
  {
  }

  public override void Destroy()
  {
    base.Destroy();
  }

  public override void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition)
  {
    base.BeginTransition(navigator, transition);
    ReactionMonitor.Instance reaction_monitor = navigator.GetSMI<ReactionMonitor.Instance>();
    reaction_monitor.PollForReactables(transition);
    if (!reaction_monitor.IsReacting())
      return;
    transition.anim = (HashedString) ((string) null);
    transition.isLooping = false;
    transition.end = transition.start;
    transition.speed = 1f;
    transition.animSpeed = 1f;
    transition.x = 0;
    transition.y = 0;
    transition.isCompleteCB = (Func<bool>) (() => !reaction_monitor.IsReacting());
  }
}
