// Decompiled with JetBrains decompiler
// Type: EggCracker
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using TUNING;
using UnityEngine;

public class EggCracker : KMonoBehaviour
{
  [MyCmpReq]
  private ComplexFabricator refinery;
  [MyCmpReq]
  private ComplexFabricatorWorkable workable;
  private KBatchedAnimTracker tracker;
  private GameObject display_egg;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.refinery.choreType = Db.Get().ChoreTypes.Cook;
    this.refinery.fetchChoreTypeIdHash = Db.Get().ChoreTypes.CookFetch.IdHash;
    this.workable.WorkerStatusItem = Db.Get().DuplicantStatusItems.Processing;
    this.workable.AttributeConverter = Db.Get().AttributeConverters.CookingSpeed;
    this.workable.AttributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
    this.workable.SkillExperienceSkillGroup = Db.Get().SkillGroups.Cooking.Id;
    this.workable.SkillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
    ComplexFabricatorWorkable workable = this.workable;
    workable.OnWorkableEventCB = workable.OnWorkableEventCB + new System.Action<Workable.WorkableEvent>(this.OnWorkableEvent);
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    UnityEngine.Object.Destroy((UnityEngine.Object) this.tracker);
    this.tracker = (KBatchedAnimTracker) null;
  }

  private void OnWorkableEvent(Workable.WorkableEvent e)
  {
    switch (e)
    {
      case Workable.WorkableEvent.WorkStarted:
        ComplexRecipe currentWorkingOrder = this.refinery.CurrentWorkingOrder;
        if (currentWorkingOrder == null)
          break;
        ComplexRecipe.RecipeElement[] ingredients = currentWorkingOrder.ingredients;
        if (ingredients.Length <= 0)
          break;
        this.display_egg = this.refinery.buildStorage.FindFirst(ingredients[0].material);
        this.PositionActiveEgg();
        break;
      case Workable.WorkableEvent.WorkCompleted:
        if (!(bool) ((UnityEngine.Object) this.display_egg))
          break;
        this.display_egg.GetComponent<KBatchedAnimController>().Play((HashedString) "hatching_pst", KAnim.PlayMode.Once, 1f, 0.0f);
        break;
      case Workable.WorkableEvent.WorkStopped:
        UnityEngine.Object.Destroy((UnityEngine.Object) this.tracker);
        this.tracker = (KBatchedAnimTracker) null;
        this.display_egg = (GameObject) null;
        break;
    }
  }

  private void PositionActiveEgg()
  {
    if (!(bool) ((UnityEngine.Object) this.display_egg))
      return;
    KBatchedAnimController component1 = this.display_egg.GetComponent<KBatchedAnimController>();
    component1.enabled = true;
    component1.SetSceneLayer(Grid.SceneLayer.BuildingUse);
    KSelectable component2 = this.display_egg.GetComponent<KSelectable>();
    if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
      component2.enabled = true;
    this.tracker = this.display_egg.AddComponent<KBatchedAnimTracker>();
    this.tracker.symbol = (HashedString) "snapto_egg";
  }
}
