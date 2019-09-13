// Decompiled with JetBrains decompiler
// Type: CommonPlacerConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;
using UnityEngine.Rendering;

public class CommonPlacerConfig
{
  public GameObject CreatePrefab(string id, string name, Material default_material)
  {
    GameObject entity = EntityTemplates.CreateEntity(id, name, true);
    entity.layer = LayerMask.NameToLayer("PlaceWithDepth");
    entity.AddOrGet<SaveLoadRoot>();
    entity.AddOrGet<StateMachineController>();
    entity.AddOrGet<Prioritizable>().iconOffset = new Vector2(0.3f, 0.32f);
    KBoxCollider2D kboxCollider2D = entity.AddOrGet<KBoxCollider2D>();
    kboxCollider2D.offset = new Vector2(0.0f, 0.5f);
    kboxCollider2D.size = new Vector2(1f, 1f);
    GameObject gameObject = new GameObject("Mask");
    gameObject.layer = LayerMask.NameToLayer("PlaceWithDepth");
    gameObject.transform.parent = entity.transform;
    gameObject.transform.SetLocalPosition(new Vector3(0.0f, 0.5f, -3.537f));
    gameObject.transform.eulerAngles = new Vector3(0.0f, 180f, 0.0f);
    gameObject.AddComponent<MeshFilter>().sharedMesh = Assets.instance.commonPlacerAssets.mesh;
    MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
    meshRenderer.lightProbeUsage = LightProbeUsage.Off;
    meshRenderer.reflectionProbeUsage = ReflectionProbeUsage.Off;
    meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
    meshRenderer.receiveShadows = false;
    meshRenderer.sharedMaterial = default_material;
    gameObject.AddComponent<EasingAnimations>().scales = new EasingAnimations.AnimationScales[2]
    {
      new EasingAnimations.AnimationScales()
      {
        name = "ScaleUp",
        startScale = 0.0f,
        endScale = 1f,
        type = EasingAnimations.AnimationScales.AnimationType.EaseInOutBack,
        easingMultiplier = 5f
      },
      new EasingAnimations.AnimationScales()
      {
        name = "ScaleDown",
        startScale = 1f,
        endScale = 0.0f,
        type = EasingAnimations.AnimationScales.AnimationType.EaseOutBack,
        easingMultiplier = 1f
      }
    };
    return entity;
  }

  [Serializable]
  public class CommonPlacerAssets
  {
    public Mesh mesh;
  }
}
