// Decompiled with JetBrains decompiler
// Type: TubeTransitionLayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class TubeTransitionLayer : TransitionDriver.OverrideLayer
{
  private TubeTraveller.Instance tube_traveller;
  private TravelTubeEntrance entrance;

  public TubeTransitionLayer(Navigator navigator)
    : base(navigator)
  {
    this.tube_traveller = navigator.GetSMI<TubeTraveller.Instance>();
  }

  public override void BeginTransition(Navigator navigator, Navigator.ActiveTransition transition)
  {
    base.BeginTransition(navigator, transition);
    this.tube_traveller.OnPathAdvanced((object) null);
    if (transition.start != NavType.Tube && transition.end == NavType.Tube)
      this.entrance = this.GetEntrance(Grid.PosToCell((KMonoBehaviour) navigator));
    else
      this.entrance = (TravelTubeEntrance) null;
  }

  public override void EndTransition(Navigator navigator, Navigator.ActiveTransition transition)
  {
    base.EndTransition(navigator, transition);
    if (transition.start != NavType.Tube && transition.end == NavType.Tube && (bool) ((Object) this.entrance))
    {
      this.entrance.ConsumeCharge(navigator.gameObject);
      this.entrance = (TravelTubeEntrance) null;
    }
    this.tube_traveller.OnTubeTransition(transition.end == NavType.Tube);
  }

  private TravelTubeEntrance GetEntrance(int cell)
  {
    if (!Grid.HasUsableTubeEntrance(cell, this.tube_traveller.prefabInstanceID))
      return (TravelTubeEntrance) null;
    GameObject gameObject = Grid.Objects[cell, 1];
    if ((Object) gameObject != (Object) null)
    {
      TravelTubeEntrance component = gameObject.GetComponent<TravelTubeEntrance>();
      if ((Object) component != (Object) null && component.isSpawned)
        return component;
    }
    return (TravelTubeEntrance) null;
  }
}
