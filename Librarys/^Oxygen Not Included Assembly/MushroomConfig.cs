// Decompiled with JetBrains decompiler
// Type: MushroomConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

public class MushroomConfig : IEntityConfig
{
  public static float SEEDS_PER_FRUIT_CHANCE = 0.05f;
  public static string ID = "Mushroom";
  private static readonly EventSystem.IntraObjectHandler<Edible> OnEatCompleteDelegate = new EventSystem.IntraObjectHandler<Edible>((System.Action<Edible, object>) ((component, data) => MushroomConfig.OnEatComplete(component)));

  public GameObject CreatePrefab()
  {
    return EntityTemplates.ExtendEntityToFood(EntityTemplates.CreateLooseEntity(MushroomConfig.ID, (string) ITEMS.FOOD.MUSHROOM.NAME, (string) ITEMS.FOOD.MUSHROOM.DESC, 1f, false, Assets.GetAnim((HashedString) "funguscap_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.77f, 0.48f, true, 0, SimHashes.Creature, (List<Tag>) null), TUNING.FOOD.FOOD_TYPES.MUSHROOM);
  }

  public void OnPrefabInit(GameObject inst)
  {
  }

  public void OnSpawn(GameObject inst)
  {
    inst.Subscribe<Edible>(-10536414, MushroomConfig.OnEatCompleteDelegate);
  }

  private static void OnEatComplete(Edible edible)
  {
    if (!((UnityEngine.Object) edible != (UnityEngine.Object) null))
      return;
    int num1 = 0;
    float unitsConsumed = edible.unitsConsumed;
    int num2 = Mathf.FloorToInt(unitsConsumed);
    if ((double) UnityEngine.Random.value < (double) (unitsConsumed % 1f))
      ++num2;
    for (int index = 0; index < num2; ++index)
    {
      if ((double) UnityEngine.Random.value < (double) MushroomConfig.SEEDS_PER_FRUIT_CHANCE)
        ++num1;
    }
    if (num1 <= 0)
      return;
    GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(new Tag("MushroomSeed")), Grid.CellToPosCCC(Grid.PosToCell(edible.transform.GetPosition() + new Vector3(0.0f, 0.05f, 0.0f)), Grid.SceneLayer.Ore), Grid.SceneLayer.Ore, (string) null, 0);
    PrimaryElement component1 = edible.GetComponent<PrimaryElement>();
    PrimaryElement component2 = gameObject.GetComponent<PrimaryElement>();
    component2.Temperature = component1.Temperature;
    component2.Units = (float) num1;
    gameObject.SetActive(true);
  }
}
