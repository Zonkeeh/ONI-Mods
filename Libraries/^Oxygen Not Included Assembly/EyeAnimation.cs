// Decompiled with JetBrains decompiler
// Type: EyeAnimation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class EyeAnimation : IEntityConfig
{
  public static string ID = nameof (EyeAnimation);

  public GameObject CreatePrefab()
  {
    GameObject entity = EntityTemplates.CreateEntity(EyeAnimation.ID, EyeAnimation.ID, false);
    entity.AddOrGet<KBatchedAnimController>().AnimFiles = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_blinks_kanim")
    };
    return entity;
  }

  public void OnPrefabInit(GameObject go)
  {
  }

  public void OnSpawn(GameObject go)
  {
  }
}
