// Decompiled with JetBrains decompiler
// Type: GourmetCookingStation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class GourmetCookingStation : ComplexFabricator, IEffectDescriptor
{
  private static readonly Operational.Flag gourmetCookingStationFlag = new Operational.Flag("gourmet_cooking_station", Operational.Flag.Type.Requirement);
  public float GAS_CONVERSION_RATIO = 0.1f;
  [SerializeField]
  private int diseaseCountKillRate = 150;
  public float GAS_CONSUMPTION_RATE;
  public const float START_FUEL_MASS = 5f;
  public Tag fuelTag;
  private GourmetCookingStation.StatesInstance smi;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.keepAdditionalTags.SetTag(this.fuelTag);
    this.choreType = Db.Get().ChoreTypes.Cook;
    this.fetchChoreTypeIdHash = Db.Get().ChoreTypes.CookFetch.IdHash;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.workable.requiredSkillPerk = Db.Get().SkillPerks.CanElectricGrill.Id;
    this.workable.WorkerStatusItem = Db.Get().DuplicantStatusItems.Cooking;
    this.workable.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_cookstation_gourtmet_kanim")
    };
    this.workable.AttributeConverter = Db.Get().AttributeConverters.CookingSpeed;
    this.workable.AttributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
    this.workable.SkillExperienceSkillGroup = Db.Get().SkillGroups.Cooking.Id;
    this.workable.SkillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
    this.workable.OnWorkTickActions += (System.Action<Worker, float>) ((worker, dt) =>
    {
      Debug.Assert((UnityEngine.Object) worker != (UnityEngine.Object) null, (object) "How did we get a null worker?");
      if (this.diseaseCountKillRate <= 0)
        return;
      this.GetComponent<PrimaryElement>().ModifyDiseaseCount(-Math.Max(1, (int) ((double) this.diseaseCountKillRate * (double) dt)), nameof (GourmetCookingStation));
    });
    this.smi = new GourmetCookingStation.StatesInstance(this);
    this.smi.StartSM();
  }

  public float GetAvailableFuel()
  {
    return this.inStorage.GetAmountAvailable(this.fuelTag);
  }

  protected override List<GameObject> SpawnOrderProduct(ComplexRecipe recipe)
  {
    List<GameObject> gameObjectList = base.SpawnOrderProduct(recipe);
    foreach (GameObject gameObject in gameObjectList)
    {
      PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
      component.ModifyDiseaseCount(-component.DiseaseCount, "GourmetCookingStation.CompleteOrder");
    }
    this.GetComponent<Operational>().SetActive(false, false);
    return gameObjectList;
  }

  public override List<Descriptor> GetDescriptors(BuildingDef def)
  {
    List<Descriptor> descriptors = base.GetDescriptors(def);
    descriptors.Add(new Descriptor((string) UI.BUILDINGEFFECTS.REMOVES_DISEASE, (string) UI.BUILDINGEFFECTS.TOOLTIPS.REMOVES_DISEASE, Descriptor.DescriptorType.Effect, false));
    return descriptors;
  }

  public class StatesInstance : GameStateMachine<GourmetCookingStation.States, GourmetCookingStation.StatesInstance, GourmetCookingStation, object>.GameInstance
  {
    public StatesInstance(GourmetCookingStation smi)
      : base(smi)
    {
    }
  }

  public class States : GameStateMachine<GourmetCookingStation.States, GourmetCookingStation.StatesInstance, GourmetCookingStation>
  {
    public static StatusItem waitingForFuelStatus;
    public GameStateMachine<GourmetCookingStation.States, GourmetCookingStation.StatesInstance, GourmetCookingStation, object>.State waitingForFuel;
    public GameStateMachine<GourmetCookingStation.States, GourmetCookingStation.StatesInstance, GourmetCookingStation, object>.State ready;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      if (GourmetCookingStation.States.waitingForFuelStatus == null)
      {
        GourmetCookingStation.States.waitingForFuelStatus = new StatusItem("waitingForFuelStatus", (string) BUILDING.STATUSITEMS.ENOUGH_FUEL.NAME, (string) BUILDING.STATUSITEMS.ENOUGH_FUEL.TOOLTIP, "status_item_no_gas_to_pump", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, 129022);
        GourmetCookingStation.States.waitingForFuelStatus.resolveStringCallback = (Func<string, object, string>) ((str, obj) =>
        {
          GourmetCookingStation gourmetCookingStation = (GourmetCookingStation) obj;
          return string.Format(str, (object) gourmetCookingStation.fuelTag.ProperName(), (object) GameUtil.GetFormattedMass(5f, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
        });
      }
      default_state = (StateMachine.BaseState) this.waitingForFuel;
      this.waitingForFuel.Enter((StateMachine<GourmetCookingStation.States, GourmetCookingStation.StatesInstance, GourmetCookingStation, object>.State.Callback) (smi => smi.master.operational.SetFlag(GourmetCookingStation.gourmetCookingStationFlag, false))).ToggleStatusItem(GourmetCookingStation.States.waitingForFuelStatus, (Func<GourmetCookingStation.StatesInstance, object>) (smi => (object) smi.master)).EventTransition(GameHashes.OnStorageChange, this.ready, (StateMachine<GourmetCookingStation.States, GourmetCookingStation.StatesInstance, GourmetCookingStation, object>.Transition.ConditionCallback) (smi => (double) smi.master.GetAvailableFuel() >= 5.0));
      this.ready.Enter((StateMachine<GourmetCookingStation.States, GourmetCookingStation.StatesInstance, GourmetCookingStation, object>.State.Callback) (smi =>
      {
        smi.master.SetQueueDirty();
        smi.master.operational.SetFlag(GourmetCookingStation.gourmetCookingStationFlag, true);
      })).EventTransition(GameHashes.OnStorageChange, this.waitingForFuel, (StateMachine<GourmetCookingStation.States, GourmetCookingStation.StatesInstance, GourmetCookingStation, object>.Transition.ConditionCallback) (smi => (double) smi.master.GetAvailableFuel() <= 0.0));
    }
  }
}
