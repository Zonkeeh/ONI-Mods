// Decompiled with JetBrains decompiler
// Type: OneshotReactableLocator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class OneshotReactableLocator : IEntityConfig
{
  public static readonly string ID = nameof (OneshotReactableLocator);

  public static EmoteReactable CreateOneshotReactable(
    GameObject source,
    float lifetime,
    string id,
    ChoreType chore_type,
    HashedString animset,
    int range_width = 15,
    int range_height = 15,
    float min_reactor_time = 20f)
  {
    GameObject gameObject = Util.KInstantiate(Assets.GetPrefab((Tag) OneshotReactableLocator.ID), source.transform.GetPosition());
    EmoteReactable emoteReactable = new EmoteReactable(gameObject, (HashedString) id, chore_type, animset, range_width, range_height, 100000f, min_reactor_time, float.PositiveInfinity);
    emoteReactable.AddPrecondition(OneshotReactableLocator.ReactorIsNotSource(source));
    OneshotReactableHost component = gameObject.GetComponent<OneshotReactableHost>();
    component.lifetime = lifetime;
    component.SetReactable((Reactable) emoteReactable);
    gameObject.SetActive(true);
    return emoteReactable;
  }

  private static Reactable.ReactablePrecondition ReactorIsNotSource(GameObject source)
  {
    return (Reactable.ReactablePrecondition) ((reactor, transition) => (Object) reactor != (Object) source);
  }

  public GameObject CreatePrefab()
  {
    GameObject entity = EntityTemplates.CreateEntity(OneshotReactableLocator.ID, OneshotReactableLocator.ID, false);
    entity.AddTag(GameTags.NotConversationTopic);
    entity.AddOrGet<OneshotReactableHost>();
    return entity;
  }

  public void OnPrefabInit(GameObject go)
  {
  }

  public void OnSpawn(GameObject go)
  {
  }
}
