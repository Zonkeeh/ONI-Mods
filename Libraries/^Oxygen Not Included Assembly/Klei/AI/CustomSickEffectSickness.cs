// Decompiled with JetBrains decompiler
// Type: Klei.AI.CustomSickEffectSickness
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace Klei.AI
{
  public class CustomSickEffectSickness : Sickness.SicknessComponent
  {
    private string kanim;
    private string animName;

    public CustomSickEffectSickness(string effect_kanim, string effect_anim_name)
    {
      this.kanim = effect_kanim;
      this.animName = effect_anim_name;
    }

    public override object OnInfect(GameObject go, SicknessInstance diseaseInstance)
    {
      KBatchedAnimController effect = FXHelpers.CreateEffect(this.kanim, go.transform.GetPosition() + new Vector3(0.0f, 0.0f, -0.1f), go.transform, true, Grid.SceneLayer.Front, false);
      effect.Play((HashedString) this.animName, KAnim.PlayMode.Loop, 1f, 0.0f);
      return (object) effect;
    }

    public override void OnCure(GameObject go, object instance_data)
    {
      ((Component) instance_data).gameObject.DeleteObject();
    }
  }
}
