// Decompiled with JetBrains decompiler
// Type: MicrobeMusher
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class MicrobeMusher : ComplexFabricator
{
  private static readonly KAnimHashedString meterRationHash = new KAnimHashedString("meter_ration");
  private static readonly KAnimHashedString canHash = new KAnimHashedString("can");
  [SerializeField]
  public Vector3 mushbarSpawnOffset = Vector3.right;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.choreType = Db.Get().ChoreTypes.Cook;
    this.fetchChoreTypeIdHash = Db.Get().ChoreTypes.CookFetch.IdHash;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    GameScheduler.Instance.Schedule("WaterFetchingTutorial", 2f, (System.Action<object>) (obj => Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_FetchingWater, true)), (object) null, (SchedulerGroup) null);
    this.workable.WorkerStatusItem = Db.Get().DuplicantStatusItems.Mushing;
    this.workable.AttributeConverter = Db.Get().AttributeConverters.CookingSpeed;
    this.workable.AttributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
    this.workable.SkillExperienceSkillGroup = Db.Get().SkillGroups.Cooking.Id;
    this.workable.SkillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
    this.workable.meter = new MeterController((KAnimControllerBase) this.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Behind, Grid.SceneLayer.NoLayer, new string[2]
    {
      "meter_target",
      "meter_ration"
    });
    this.workable.meter.meterController.SetSymbolVisiblity(MicrobeMusher.canHash, false);
    this.workable.meter.meterController.SetSymbolVisiblity(MicrobeMusher.meterRationHash, false);
    this.workable.meter.meterController.GetComponent<KBatchedAnimTracker>().skipInitialDisable = true;
  }

  protected override List<GameObject> SpawnOrderProduct(ComplexRecipe recipe)
  {
    List<GameObject> gameObjectList = base.SpawnOrderProduct(recipe);
    foreach (GameObject go in gameObjectList)
    {
      PrimaryElement component = go.GetComponent<PrimaryElement>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      {
        if (go.PrefabID() == (Tag) "MushBar")
        {
          byte index = Db.Get().Diseases.GetIndex((HashedString) "FoodPoisoning");
          component.AddDisease(index, 1000, "Made of mud");
        }
        if (go.GetComponent<PrimaryElement>().DiseaseCount > 0)
          Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_DiseaseCooking, true);
      }
    }
    return gameObjectList;
  }
}
