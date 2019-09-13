// Decompiled with JetBrains decompiler
// Type: Sculpture
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class Sculpture : Artable
{
  private static KAnimFile[] sculptureOverrides;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    if (Sculpture.sculptureOverrides == null)
      Sculpture.sculptureOverrides = new KAnimFile[1]
      {
        Assets.GetAnim((HashedString) "anim_interacts_sculpture_kanim")
      };
    this.overrideAnims = Sculpture.sculptureOverrides;
    this.synchronizeAnims = false;
  }

  public override void SetStage(string stage_id, bool skip_effect)
  {
    base.SetStage(stage_id, skip_effect);
    if (skip_effect || !(this.CurrentStage != "Default"))
      return;
    KBatchedAnimController effect = FXHelpers.CreateEffect("sculpture_fx_kanim", this.transform.GetPosition(), this.transform, false, Grid.SceneLayer.Front, false);
    effect.destroyOnAnimComplete = true;
    effect.transform.SetLocalPosition(Vector3.zero);
    effect.Play((HashedString) "poof", KAnim.PlayMode.Once, 1f, 0.0f);
  }
}
