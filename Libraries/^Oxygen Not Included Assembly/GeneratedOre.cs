// Decompiled with JetBrains decompiler
// Type: GeneratedOre
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class GeneratedOre
{
  public static void LoadGeneratedOre(List<System.Type> types)
  {
    System.Type type1 = typeof (IOreConfig);
    HashSet<SimHashes> simHashesSet = new HashSet<SimHashes>();
    foreach (System.Type type2 in types)
    {
      if (type1.IsAssignableFrom(type2) && !type2.IsAbstract && !type2.IsInterface)
      {
        IOreConfig instance = Activator.CreateInstance(type2) as IOreConfig;
        SimHashes elementId = instance.ElementID;
        if (elementId != SimHashes.Void)
          simHashesSet.Add(elementId);
        Assets.AddPrefab(instance.CreatePrefab().GetComponent<KPrefabID>());
      }
    }
    foreach (Element element in ElementLoader.elements)
    {
      if (element != null && !simHashesSet.Contains(element.id))
      {
        if (element.substance != null && (UnityEngine.Object) element.substance.anim != (UnityEngine.Object) null)
        {
          GameObject gameObject = (GameObject) null;
          if (element.IsSolid)
            gameObject = EntityTemplates.CreateSolidOreEntity(element.id, (List<Tag>) null);
          else if (element.IsLiquid)
            gameObject = EntityTemplates.CreateLiquidOreEntity(element.id, (List<Tag>) null);
          else if (element.IsGas)
            gameObject = EntityTemplates.CreateGasOreEntity(element.id, (List<Tag>) null);
          if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
            Assets.AddPrefab(gameObject.GetComponent<KPrefabID>());
        }
        else
          Debug.LogError((object) ("Missing substance or anim for element [" + element.name + "]"));
      }
    }
  }

  public static SubstanceChunk CreateChunk(
    Element element,
    float mass,
    float temperature,
    byte diseaseIdx,
    int diseaseCount,
    Vector3 position)
  {
    if ((double) temperature <= 0.0)
      DebugUtil.LogWarningArgs((object) "GeneratedOre.CreateChunk tried to create a chunk with a temperature <= 0");
    GameObject prefab = Assets.GetPrefab(element.tag);
    if ((UnityEngine.Object) prefab == (UnityEngine.Object) null)
      Debug.LogError((object) ("Could not find prefab for element " + element.id.ToString()));
    SubstanceChunk component1 = GameUtil.KInstantiate(prefab, Grid.SceneLayer.Ore, (string) null, 0).GetComponent<SubstanceChunk>();
    component1.transform.SetPosition(position);
    component1.gameObject.SetActive(true);
    PrimaryElement component2 = component1.GetComponent<PrimaryElement>();
    component2.Mass = mass;
    component2.Temperature = temperature;
    component2.AddDisease(diseaseIdx, diseaseCount, "GeneratedOre.CreateChunk");
    component1.GetComponent<KPrefabID>().InitializeTags();
    return component1;
  }
}
