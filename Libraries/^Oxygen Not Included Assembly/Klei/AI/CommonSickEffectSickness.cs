// Decompiled with JetBrains decompiler
// Type: Klei.AI.CommonSickEffectSickness
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace Klei.AI
{
  public class CommonSickEffectSickness : Sickness.SicknessComponent
  {
    public override object OnInfect(GameObject go, SicknessInstance diseaseInstance)
    {
      KBatchedAnimController effect = FXHelpers.CreateEffect("contaminated_crew_fx_kanim", go.transform.GetPosition() + new Vector3(0.0f, 0.0f, -0.1f), go.transform, true, Grid.SceneLayer.Front, false);
      effect.Play((HashedString) "fx_loop", KAnim.PlayMode.Loop, 1f, 0.0f);
      return (object) effect;
    }

    public override void OnCure(GameObject go, object instance_data)
    {
      ((Component) instance_data).gameObject.DeleteObject();
    }
  }
}
