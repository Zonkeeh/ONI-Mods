// Decompiled with JetBrains decompiler
// Type: FXHelpers
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public static class FXHelpers
{
  public static KBatchedAnimController CreateEffect(
    string anim_file_name,
    Vector3 position,
    Transform parent = null,
    bool update_looping_sounds_position = false,
    Grid.SceneLayer layer = Grid.SceneLayer.Front,
    bool set_inactive = false)
  {
    KBatchedAnimController component = GameUtil.KInstantiate(Assets.GetPrefab((Tag) EffectConfigs.EffectTemplateId), position, layer, (string) null, 0).GetComponent<KBatchedAnimController>();
    component.GetComponent<KPrefabID>().PrefabTag = TagManager.Create(anim_file_name);
    component.name = anim_file_name;
    if ((Object) parent != (Object) null)
      component.transform.SetParent(parent, false);
    component.transform.SetPosition(position);
    if (update_looping_sounds_position)
      component.FindOrAddComponent<LoopingSounds>().updatePosition = true;
    KAnimFile anim = Assets.GetAnim((HashedString) anim_file_name);
    if ((Object) anim == (Object) null)
      Debug.LogWarning((object) ("Missing effect anim: " + anim_file_name));
    else
      component.AnimFiles = new KAnimFile[1]{ anim };
    if (!set_inactive)
      component.gameObject.SetActive(true);
    return component;
  }
}
