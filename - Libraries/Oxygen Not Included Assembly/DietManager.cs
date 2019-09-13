// Decompiled with JetBrains decompiler
// Type: DietManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public class DietManager : KMonoBehaviour
{
  private Dictionary<Tag, Diet> diets;
  public static DietManager Instance;

  public static void DestroyInstance()
  {
    DietManager.Instance = (DietManager) null;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.diets = DietManager.CollectDiets((Tag[]) null);
    DietManager.Instance = this;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    foreach (Tag tag in WorldInventory.Instance.GetDiscovered())
      this.Discover(tag);
    foreach (KeyValuePair<Tag, Diet> diet in this.diets)
    {
      foreach (Diet.Info info in diet.Value.infos)
      {
        foreach (Tag consumedTag in info.consumedTags)
        {
          if ((UnityEngine.Object) Assets.GetPrefab(consumedTag) == (UnityEngine.Object) null)
            Debug.LogError((object) ("Could not find prefab: " + (object) consumedTag));
        }
      }
    }
    WorldInventory.Instance.OnDiscover += new System.Action<Tag, Tag>(this.OnWorldInventoryDiscover);
  }

  private void Discover(Tag tag)
  {
    foreach (KeyValuePair<Tag, Diet> diet in this.diets)
    {
      if (diet.Value.GetDietInfo(tag) != null)
        WorldInventory.Instance.Discover(tag, diet.Key);
    }
  }

  private void OnWorldInventoryDiscover(Tag category_tag, Tag tag)
  {
    this.Discover(tag);
  }

  public static Dictionary<Tag, Diet> CollectDiets(Tag[] target_species)
  {
    Dictionary<Tag, Diet> dictionary = new Dictionary<Tag, Diet>();
    foreach (KPrefabID prefab in Assets.Prefabs)
    {
      CreatureCalorieMonitor.Def def = prefab.GetDef<CreatureCalorieMonitor.Def>();
      if (def != null && (target_species == null || Array.IndexOf<Tag>(target_species, prefab.GetComponent<CreatureBrain>().species) >= 0))
        dictionary[prefab.PrefabTag] = def.diet;
    }
    return dictionary;
  }
}
