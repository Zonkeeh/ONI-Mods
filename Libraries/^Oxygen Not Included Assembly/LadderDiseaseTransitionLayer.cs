// Decompiled with JetBrains decompiler
// Type: LadderDiseaseTransitionLayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using UnityEngine;

public class LadderDiseaseTransitionLayer : TransitionDriver.OverrideLayer
{
  public LadderDiseaseTransitionLayer(Navigator navigator)
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
  }

  public override void UpdateTransition(Navigator navigator, Navigator.ActiveTransition transition)
  {
    base.UpdateTransition(navigator, transition);
  }

  public override void EndTransition(Navigator navigator, Navigator.ActiveTransition transition)
  {
    base.EndTransition(navigator, transition);
    if (transition.end != NavType.Ladder)
      return;
    int cell = Grid.PosToCell((KMonoBehaviour) navigator);
    GameObject gameObject = Grid.Objects[cell, 1];
    if (!((Object) gameObject != (Object) null))
      return;
    PrimaryElement component1 = gameObject.GetComponent<PrimaryElement>();
    if (!((Object) component1 != (Object) null))
      return;
    PrimaryElement component2 = navigator.GetComponent<PrimaryElement>();
    if (!((Object) component2 != (Object) null))
      return;
    SimUtil.DiseaseInfo invalid1 = SimUtil.DiseaseInfo.Invalid;
    invalid1.idx = component2.DiseaseIdx;
    invalid1.count = (int) ((double) component2.DiseaseCount * 0.00499999988824129);
    SimUtil.DiseaseInfo invalid2 = SimUtil.DiseaseInfo.Invalid;
    invalid2.idx = component1.DiseaseIdx;
    invalid2.count = (int) ((double) component1.DiseaseCount * 0.00499999988824129);
    component2.ModifyDiseaseCount(-invalid1.count, "Navigator.EndTransition");
    component1.ModifyDiseaseCount(-invalid2.count, "Navigator.EndTransition");
    if (invalid1.count > 0)
      component1.AddDisease(invalid1.idx, invalid1.count, "TransitionDriver.EndTransition");
    if (invalid2.count <= 0)
      return;
    component2.AddDisease(invalid2.idx, invalid2.count, "TransitionDriver.EndTransition");
  }
}
