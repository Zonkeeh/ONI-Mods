// Decompiled with JetBrains decompiler
// Type: MinionSelectPreviewConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using UnityEngine;
using UnityEngine.UI;

public class MinionSelectPreviewConfig : IEntityConfig
{
  public static string ID = "MinionSelectPreview";

  public GameObject CreatePrefab()
  {
    GameObject entity = EntityTemplates.CreateEntity(MinionSelectPreviewConfig.ID, MinionSelectPreviewConfig.ID, true);
    RectTransform rectTransform = entity.AddOrGet<RectTransform>();
    rectTransform.anchorMin = new Vector2(0.0f, 0.0f);
    rectTransform.anchorMax = new Vector2(1f, 1f);
    rectTransform.pivot = new Vector2(0.5f, 0.0f);
    rectTransform.anchoredPosition = new Vector2(0.0f, 0.0f);
    rectTransform.sizeDelta = new Vector2(0.0f, 0.0f);
    LayoutElement layoutElement = entity.AddOrGet<LayoutElement>();
    layoutElement.preferredHeight = 100f;
    layoutElement.preferredWidth = 100f;
    entity.AddOrGet<Effects>();
    entity.AddOrGet<Traits>();
    MinionModifiers minionModifiers = entity.AddOrGet<MinionModifiers>();
    minionModifiers.initialTraits = new string[1]
    {
      MinionConfig.MINION_BASE_TRAIT_ID
    };
    MinionConfig.AddMinionAmounts((Modifiers) minionModifiers);
    entity.AddOrGet<AttributeLevels>();
    entity.AddOrGet<AttributeConverters>();
    entity.AddOrGet<MinionIdentity>().addToIdentityList = false;
    entity.AddOrGet<BoxCollider2D>().size = new Vector2(1f, 1f);
    entity.AddOrGet<FaceGraph>();
    entity.AddOrGet<Accessorizer>();
    KBatchedAnimController kbatchedAnimController = entity.AddOrGet<KBatchedAnimController>();
    kbatchedAnimController.materialType = KAnimBatchGroup.MaterialType.UI;
    kbatchedAnimController.animScale = 0.5f;
    kbatchedAnimController.setScaleFromAnim = false;
    kbatchedAnimController.animOverrideSize = new Vector2(100f, 120f);
    kbatchedAnimController.AnimFiles = new KAnimFile[4]
    {
      Assets.GetAnim((HashedString) "body_comp_default_kanim"),
      Assets.GetAnim((HashedString) "anim_construction_default_kanim"),
      Assets.GetAnim((HashedString) "anim_idles_default_kanim"),
      Assets.GetAnim((HashedString) "anim_cheer_kanim")
    };
    SymbolOverrideControllerUtil.AddToPrefab(entity);
    MinionConfig.ConfigureSymbols(entity);
    return entity;
  }

  public void OnPrefabInit(GameObject go)
  {
  }

  public void OnSpawn(GameObject go)
  {
  }
}
