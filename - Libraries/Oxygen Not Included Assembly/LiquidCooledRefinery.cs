﻿// Decompiled with JetBrains decompiler
// Type: LiquidCooledRefinery
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LiquidCooledRefinery : ComplexFabricator
{
  public static readonly Operational.Flag coolantOutputPipeEmpty = new Operational.Flag(nameof (coolantOutputPipeEmpty), Operational.Flag.Type.Requirement);
  private static readonly EventSystem.IntraObjectHandler<LiquidCooledRefinery> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<LiquidCooledRefinery>((System.Action<LiquidCooledRefinery, object>) ((component, data) => component.OnStorageChange(data)));
  public float minCoolantMass = 100f;
  public float thermalFudge = 0.8f;
  public float outputTemperature = 313.15f;
  [MyCmpReq]
  private ConduitConsumer conduitConsumer;
  private int outputCell;
  public Tag coolantTag;
  private MeterController meter_coolant;
  private MeterController meter_metal;
  private LiquidCooledRefinery.StatesInstance smi;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<LiquidCooledRefinery>(-1697596308, LiquidCooledRefinery.OnStorageChangeDelegate);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    KBatchedAnimController component = this.GetComponent<KBatchedAnimController>();
    this.meter_coolant = new MeterController((KAnimControllerBase) component, "meter_target", "meter_coolant", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Vector3.zero, (string[]) null);
    this.meter_metal = new MeterController((KAnimControllerBase) component, "meter_target_metal", "meter_metal", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, Vector3.zero, (string[]) null);
    this.meter_metal.SetPositionPercent(1f);
    this.smi = new LiquidCooledRefinery.StatesInstance(this);
    this.smi.StartSM();
    Game.Instance.liquidConduitFlow.AddConduitUpdater(new System.Action<float>(this.OnConduitUpdate), ConduitFlowPriority.Default);
    this.outputCell = this.GetComponent<Building>().GetUtilityOutputCell();
    this.workable.OnWorkTickActions = (System.Action<Worker, float>) ((worker, dt) => this.meter_metal.SetPositionPercent(this.workable.GetPercentComplete()));
  }

  protected override void OnCleanUp()
  {
    Game.Instance.liquidConduitFlow.RemoveConduitUpdater(new System.Action<float>(this.OnConduitUpdate));
    base.OnCleanUp();
  }

  private void OnConduitUpdate(float dt)
  {
    bool flag = (double) Game.Instance.liquidConduitFlow.GetContents(this.outputCell).mass > 0.0;
    this.smi.sm.outputBlocked.Set(flag, this.smi);
    this.operational.SetFlag(LiquidCooledRefinery.coolantOutputPipeEmpty, !flag);
  }

  public bool HasEnoughCoolant()
  {
    return (double) (this.inStorage.GetAmountAvailable(this.coolantTag) + this.buildStorage.GetAmountAvailable(this.coolantTag)) >= (double) this.minCoolantMass;
  }

  private void OnStorageChange(object data)
  {
    float percent_full = Mathf.Clamp01(this.inStorage.GetAmountAvailable(this.coolantTag) / this.conduitConsumer.capacityKG);
    if (this.meter_coolant == null)
      return;
    this.meter_coolant.SetPositionPercent(percent_full);
  }

  protected override bool HasIngredients(ComplexRecipe recipe, Storage storage)
  {
    if ((double) storage.GetAmountAvailable(this.coolantTag) >= (double) this.minCoolantMass)
      return base.HasIngredients(recipe, storage);
    return false;
  }

  protected override void TransferCurrentRecipeIngredientsForBuild()
  {
    base.TransferCurrentRecipeIngredientsForBuild();
    float num;
    for (float minCoolantMass = this.minCoolantMass; (double) this.buildStorage.GetAmountAvailable(this.coolantTag) < (double) this.minCoolantMass && (double) this.inStorage.GetAmountAvailable(this.coolantTag) > 0.0 && (double) minCoolantMass > 0.0; minCoolantMass -= num)
      num = this.inStorage.Transfer(this.buildStorage, this.coolantTag, minCoolantMass, false, true);
  }

  protected override List<GameObject> SpawnOrderProduct(ComplexRecipe recipe)
  {
    List<GameObject> gameObjectList = base.SpawnOrderProduct(recipe);
    PrimaryElement component1 = gameObjectList[0].GetComponent<PrimaryElement>();
    component1.Temperature = this.outputTemperature;
    float forElementChange = GameUtil.CalculateEnergyDeltaForElementChange(component1.Element.specificHeatCapacity, component1.Mass, component1.Element.highTemp, this.outputTemperature);
    ListPool<GameObject, LiquidCooledRefinery>.PooledList pooledList = ListPool<GameObject, LiquidCooledRefinery>.Allocate();
    this.buildStorage.Find(this.coolantTag, (List<GameObject>) pooledList);
    float num1 = 0.0f;
    foreach (GameObject gameObject in (List<GameObject>) pooledList)
    {
      PrimaryElement component2 = gameObject.GetComponent<PrimaryElement>();
      if ((double) component2.Mass != 0.0)
        num1 += component2.Mass * component2.Element.specificHeatCapacity;
    }
    foreach (GameObject gameObject in (List<GameObject>) pooledList)
    {
      PrimaryElement component2 = gameObject.GetComponent<PrimaryElement>();
      if ((double) component2.Mass != 0.0)
      {
        float num2 = component2.Mass * component2.Element.specificHeatCapacity / num1;
        float kilowatts = -forElementChange * num2 * this.thermalFudge;
        float temperatureChange = GameUtil.CalculateTemperatureChange(component2.Element.specificHeatCapacity, component2.Mass, kilowatts);
        float temperature = component2.Temperature;
        component2.Temperature += temperatureChange;
      }
    }
    double num3 = (double) this.buildStorage.Transfer(this.outStorage, this.coolantTag, float.MaxValue, false, true);
    pooledList.Recycle();
    return gameObjectList;
  }

  public override List<Descriptor> GetDescriptors(BuildingDef def)
  {
    List<Descriptor> descriptors = base.GetDescriptors(def);
    descriptors.Add(new Descriptor(string.Format((string) UI.BUILDINGEFFECTS.COOLANT, (object) this.coolantTag.ProperName(), (object) GameUtil.GetFormattedMass(this.minCoolantMass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.COOLANT, (object) this.coolantTag.ProperName(), (object) GameUtil.GetFormattedMass(this.minCoolantMass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), Descriptor.DescriptorType.Requirement, false));
    return descriptors;
  }

  public override List<Descriptor> AdditionalEffectsForRecipe(ComplexRecipe recipe)
  {
    List<Descriptor> descriptorList = base.AdditionalEffectsForRecipe(recipe);
    PrimaryElement component = Assets.GetPrefab(recipe.results[0].material).GetComponent<PrimaryElement>();
    PrimaryElement cmp = this.inStorage.FindFirstWithMass(this.coolantTag);
    string format = (string) UI.BUILDINGEFFECTS.TOOLTIPS.REFINEMENT_ENERGY_HAS_COOLANT;
    if ((UnityEngine.Object) cmp == (UnityEngine.Object) null)
    {
      cmp = Assets.GetPrefab(GameTags.Water).GetComponent<PrimaryElement>();
      format = (string) UI.BUILDINGEFFECTS.TOOLTIPS.REFINEMENT_ENERGY_NO_COOLANT;
    }
    float joules = -GameUtil.CalculateEnergyDeltaForElementChange(component.Element.specificHeatCapacity, recipe.results[0].amount, component.Element.highTemp, this.outputTemperature);
    float temperatureChange = GameUtil.CalculateTemperatureChange(cmp.Element.specificHeatCapacity, this.minCoolantMass, joules * this.thermalFudge);
    descriptorList.Add(new Descriptor(string.Format((string) UI.BUILDINGEFFECTS.REFINEMENT_ENERGY, (object) GameUtil.GetFormattedJoules(joules, "F1", GameUtil.TimeSlice.None)), string.Format(format, (object) GameUtil.GetFormattedJoules(joules, "F1", GameUtil.TimeSlice.None), (object) cmp.GetProperName(), (object) GameUtil.GetFormattedTemperature(temperatureChange, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Relative, true, false)), Descriptor.DescriptorType.Effect, false));
    return descriptorList;
  }

  public class StatesInstance : GameStateMachine<LiquidCooledRefinery.States, LiquidCooledRefinery.StatesInstance, LiquidCooledRefinery, object>.GameInstance
  {
    public StatesInstance(LiquidCooledRefinery master)
      : base(master)
    {
    }
  }

  public class States : GameStateMachine<LiquidCooledRefinery.States, LiquidCooledRefinery.StatesInstance, LiquidCooledRefinery>
  {
    public static StatusItem waitingForCoolantStatus;
    public StateMachine<LiquidCooledRefinery.States, LiquidCooledRefinery.StatesInstance, LiquidCooledRefinery, object>.BoolParameter outputBlocked;
    public GameStateMachine<LiquidCooledRefinery.States, LiquidCooledRefinery.StatesInstance, LiquidCooledRefinery, object>.State waiting_for_coolant;
    public GameStateMachine<LiquidCooledRefinery.States, LiquidCooledRefinery.StatesInstance, LiquidCooledRefinery, object>.State ready;
    public GameStateMachine<LiquidCooledRefinery.States, LiquidCooledRefinery.StatesInstance, LiquidCooledRefinery, object>.State output_blocked;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      if (LiquidCooledRefinery.States.waitingForCoolantStatus == null)
      {
        LiquidCooledRefinery.States.waitingForCoolantStatus = new StatusItem("waitingForCoolantStatus", (string) BUILDING.STATUSITEMS.ENOUGH_COOLANT.NAME, (string) BUILDING.STATUSITEMS.ENOUGH_COOLANT.TOOLTIP, "status_item_no_liquid_to_pump", StatusItem.IconType.Custom, NotificationType.BadMinor, false, OverlayModes.None.ID, 129022);
        LiquidCooledRefinery.States.waitingForCoolantStatus.resolveStringCallback = (Func<string, object, string>) ((str, obj) =>
        {
          LiquidCooledRefinery liquidCooledRefinery = (LiquidCooledRefinery) obj;
          return string.Format(str, (object) liquidCooledRefinery.coolantTag.ProperName(), (object) GameUtil.GetFormattedMass(liquidCooledRefinery.minCoolantMass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
        });
      }
      default_state = (StateMachine.BaseState) this.waiting_for_coolant;
      this.waiting_for_coolant.ToggleStatusItem(LiquidCooledRefinery.States.waitingForCoolantStatus, (Func<LiquidCooledRefinery.StatesInstance, object>) (smi => (object) smi.master)).EventTransition(GameHashes.OnStorageChange, this.ready, (StateMachine<LiquidCooledRefinery.States, LiquidCooledRefinery.StatesInstance, LiquidCooledRefinery, object>.Transition.ConditionCallback) (smi => smi.master.HasEnoughCoolant())).ParamTransition<bool>((StateMachine<LiquidCooledRefinery.States, LiquidCooledRefinery.StatesInstance, LiquidCooledRefinery, object>.Parameter<bool>) this.outputBlocked, this.output_blocked, GameStateMachine<LiquidCooledRefinery.States, LiquidCooledRefinery.StatesInstance, LiquidCooledRefinery, object>.IsTrue);
      this.ready.EventTransition(GameHashes.OnStorageChange, this.waiting_for_coolant, (StateMachine<LiquidCooledRefinery.States, LiquidCooledRefinery.StatesInstance, LiquidCooledRefinery, object>.Transition.ConditionCallback) (smi => !smi.master.HasEnoughCoolant())).ParamTransition<bool>((StateMachine<LiquidCooledRefinery.States, LiquidCooledRefinery.StatesInstance, LiquidCooledRefinery, object>.Parameter<bool>) this.outputBlocked, this.output_blocked, GameStateMachine<LiquidCooledRefinery.States, LiquidCooledRefinery.StatesInstance, LiquidCooledRefinery, object>.IsTrue).Enter((StateMachine<LiquidCooledRefinery.States, LiquidCooledRefinery.StatesInstance, LiquidCooledRefinery, object>.State.Callback) (smi => smi.master.SetQueueDirty()));
      this.output_blocked.ToggleStatusItem(Db.Get().BuildingStatusItems.OutputPipeFull, (object) null).ParamTransition<bool>((StateMachine<LiquidCooledRefinery.States, LiquidCooledRefinery.StatesInstance, LiquidCooledRefinery, object>.Parameter<bool>) this.outputBlocked, this.waiting_for_coolant, GameStateMachine<LiquidCooledRefinery.States, LiquidCooledRefinery.StatesInstance, LiquidCooledRefinery, object>.IsFalse);
    }
  }
}