// Decompiled with JetBrains decompiler
// Type: CreatureCalorieMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CreatureCalorieMonitor : GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>
{
  public GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State normal;
  private CreatureCalorieMonitor.HungryStates hungry;
  private Effect outOfCaloriesTame;
  public StateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.FloatParameter starvationStartTime;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.normal;
    this.serializable = true;
    GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State state1 = this.root.EventHandler(GameHashes.CaloriesConsumed, (GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.GameEvent.Callback) ((smi, data) => smi.OnCaloriesConsumed(data)));
    Tag poop = GameTags.Creatures.Poop;
    // ISSUE: reference to a compiler-generated field
    if (CreatureCalorieMonitor.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      CreatureCalorieMonitor.\u003C\u003Ef__mg\u0024cache0 = new StateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.Transition.ConditionCallback(CreatureCalorieMonitor.ReadyToPoop);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.Transition.ConditionCallback fMgCache0 = CreatureCalorieMonitor.\u003C\u003Ef__mg\u0024cache0;
    System.Action<CreatureCalorieMonitor.Instance> on_complete = (System.Action<CreatureCalorieMonitor.Instance>) (smi => smi.Poop());
    GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State state2 = state1.ToggleBehaviour(poop, fMgCache0, on_complete);
    // ISSUE: reference to a compiler-generated field
    if (CreatureCalorieMonitor.\u003C\u003Ef__mg\u0024cache1 == null)
    {
      // ISSUE: reference to a compiler-generated field
      CreatureCalorieMonitor.\u003C\u003Ef__mg\u0024cache1 = new System.Action<CreatureCalorieMonitor.Instance, float>(CreatureCalorieMonitor.UpdateMetabolismCalorieModifier);
    }
    // ISSUE: reference to a compiler-generated field
    System.Action<CreatureCalorieMonitor.Instance, float> fMgCache1 = CreatureCalorieMonitor.\u003C\u003Ef__mg\u0024cache1;
    state2.Update(fMgCache1, UpdateRate.SIM_200ms, false);
    this.normal.Transition((GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State) this.hungry, (StateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.Transition.ConditionCallback) (smi => smi.IsHungry()), UpdateRate.SIM_1000ms);
    this.hungry.DefaultState(this.hungry.hungry).ToggleTag(GameTags.Creatures.Hungry).EventTransition(GameHashes.CaloriesConsumed, this.normal, (StateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.Transition.ConditionCallback) (smi => !smi.IsHungry()));
    this.hungry.hungry.Transition(this.normal, (StateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.Transition.ConditionCallback) (smi => !smi.IsHungry()), UpdateRate.SIM_1000ms).Transition((GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State) this.hungry.outofcalories, (StateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.Transition.ConditionCallback) (smi => smi.IsOutOfCalories()), UpdateRate.SIM_1000ms).ToggleStatusItem(Db.Get().CreatureStatusItems.Hungry, (object) null);
    this.hungry.outofcalories.DefaultState(this.hungry.outofcalories.wild).Transition(this.hungry.hungry, (StateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.Transition.ConditionCallback) (smi => !smi.IsOutOfCalories()), UpdateRate.SIM_1000ms);
    this.hungry.outofcalories.wild.TagTransition(GameTags.Creatures.Wild, this.hungry.outofcalories.tame, true).ToggleStatusItem(Db.Get().CreatureStatusItems.Hungry, (object) null);
    GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State tame = this.hungry.outofcalories.tame;
    // ISSUE: reference to a compiler-generated field
    if (CreatureCalorieMonitor.\u003C\u003Ef__mg\u0024cache2 == null)
    {
      // ISSUE: reference to a compiler-generated field
      CreatureCalorieMonitor.\u003C\u003Ef__mg\u0024cache2 = new StateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State.Callback(CreatureCalorieMonitor.StarvationStartTime);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State.Callback fMgCache2 = CreatureCalorieMonitor.\u003C\u003Ef__mg\u0024cache2;
    double num;
    tame.Enter("StarvationStartTime", fMgCache2).Exit("ClearStarvationTime", (StateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State.Callback) (smi => num = (double) this.starvationStartTime.Set(0.0f, smi))).Transition(this.hungry.outofcalories.starvedtodeath, (StateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.Transition.ConditionCallback) (smi => (double) smi.GetDeathTimeRemaining() <= 0.0), UpdateRate.SIM_1000ms).TagTransition(GameTags.Creatures.Wild, this.hungry.outofcalories.wild, false).ToggleStatusItem((string) STRINGS.CREATURES.STATUSITEMS.STARVING.NAME, (string) STRINGS.CREATURES.STATUSITEMS.STARVING.TOOLTIP, string.Empty, StatusItem.IconType.Info, NotificationType.BadMinor, false, new HashedString(), 0, (Func<string, CreatureCalorieMonitor.Instance, string>) ((str, smi) => str.Replace("{TimeUntilDeath}", GameUtil.GetFormattedCycles(smi.GetDeathTimeRemaining(), "F1"))), (Func<string, CreatureCalorieMonitor.Instance, string>) null, (StatusItemCategory) null).ToggleNotification((Func<CreatureCalorieMonitor.Instance, Notification>) (smi => new Notification((string) STRINGS.CREATURES.STATUSITEMS.STARVING.NOTIFICATION_NAME, NotificationType.BadMinor, HashedString.Invalid, (Func<List<Notification>, object, string>) ((notifications, data) => (string) STRINGS.CREATURES.STATUSITEMS.STARVING.NOTIFICATION_TOOLTIP + notifications.ReduceMessages(false)), (object) null, true, 0.0f, (Notification.ClickCallback) null, (object) null, (Transform) null))).ToggleEffect((Func<CreatureCalorieMonitor.Instance, Effect>) (smi => this.outOfCaloriesTame));
    this.hungry.outofcalories.starvedtodeath.Enter((StateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State.Callback) (smi => smi.GetSMI<DeathMonitor.Instance>().Kill(Db.Get().Deaths.Starvation)));
    this.outOfCaloriesTame = new Effect("OutOfCaloriesTame", (string) STRINGS.CREATURES.MODIFIERS.OUT_OF_CALORIES.NAME, (string) STRINGS.CREATURES.MODIFIERS.OUT_OF_CALORIES.TOOLTIP, 0.0f, false, false, false, (string) null, 0.0f, (string) null);
    this.outOfCaloriesTame.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, -10f, (string) STRINGS.CREATURES.MODIFIERS.OUT_OF_CALORIES.NAME, false, false, true));
  }

  private static bool ReadyToPoop(CreatureCalorieMonitor.Instance smi)
  {
    return smi.stomach.IsReadyToPoop() && (double) Time.time - (double) smi.lastMealOrPoopTime >= (double) smi.def.minimumTimeBeforePooping;
  }

  private static void UpdateMetabolismCalorieModifier(CreatureCalorieMonitor.Instance smi, float dt)
  {
    smi.deltaCalorieMetabolismModifier.SetValue((float) (1.0 - (double) smi.metabolism.GetTotalValue() / 100.0));
  }

  private static void StarvationStartTime(CreatureCalorieMonitor.Instance smi)
  {
    if ((double) smi.sm.starvationStartTime.Get(smi) != 0.0)
      return;
    double num = (double) smi.sm.starvationStartTime.Set(GameClock.Instance.GetTime(), smi);
  }

  public struct CaloriesConsumedEvent
  {
    public Tag tag;
    public float calories;
  }

  public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
  {
    public float minPoopSizeInCalories = 100f;
    public float minimumTimeBeforePooping = 10f;
    public float deathTimer = 6000f;
    public Diet diet;

    public override void Configure(GameObject prefab)
    {
      prefab.GetComponent<Modifiers>().initialAmounts.Add(Db.Get().Amounts.Calories.Id);
    }

    public List<Descriptor> GetDescriptors(GameObject obj)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      CreatureCalorieMonitor.Def.\u003CGetDescriptors\u003Ec__AnonStorey1 descriptorsCAnonStorey1_1 = new CreatureCalorieMonitor.Def.\u003CGetDescriptors\u003Ec__AnonStorey1();
      List<Descriptor> descriptorList = new List<Descriptor>();
      descriptorList.Add(new Descriptor((string) UI.BUILDINGEFFECTS.DIET_HEADER, (string) UI.BUILDINGEFFECTS.TOOLTIPS.DIET_HEADER, Descriptor.DescriptorType.Effect, false));
      // ISSUE: reference to a compiler-generated field
      descriptorsCAnonStorey1_1.dailyPlantGrowthConsumption = 1f;
      if (this.diet.consumedTags.Count > 0)
      {
        float calorie_loss_per_second = 0.0f;
        foreach (AttributeModifier selfModifier in Db.Get().traits.Get(obj.GetComponent<Modifiers>().initialTraits[0]).SelfModifiers)
        {
          if (selfModifier.AttributeId == Db.Get().Amounts.Calories.deltaAttribute.Id)
            calorie_loss_per_second = selfModifier.Value;
        }
        string newValue1 = string.Join(", ", this.diet.consumedTags.Select<KeyValuePair<Tag, float>, string>((Func<KeyValuePair<Tag, float>, string>) (t => t.Key.ProperName())).ToArray<string>());
        string empty = string.Empty;
        string newValue2 = !this.diet.eatsPlantsDirectly ? string.Join("\n", this.diet.consumedTags.Select<KeyValuePair<Tag, float>, string>((Func<KeyValuePair<Tag, float>, string>) (t => UI.BUILDINGEFFECTS.DIET_CONSUMED_ITEM.text.Replace("{Food}", t.Key.ProperName()).Replace("{Amount}", GameUtil.GetFormattedMass(-calorie_loss_per_second / t.Value, GameUtil.TimeSlice.PerCycle, GameUtil.MetricMassFormat.Kilogram, true, "{0:0.#}")))).ToArray<string>()) : string.Join("\n", this.diet.consumedTags.Select<KeyValuePair<Tag, float>, string>((Func<KeyValuePair<Tag, float>, string>) (t =>
        {
          // ISSUE: variable of a compiler-generated type
          CreatureCalorieMonitor.Def.\u003CGetDescriptors\u003Ec__AnonStorey1 descriptorsCAnonStorey1 = descriptorsCAnonStorey1_1;
          // ISSUE: variable of a compiler-generated type
          CreatureCalorieMonitor.Def.\u003CGetDescriptors\u003Ec__AnonStorey0 descriptorsCAnonStorey0 = this;
          dailyPlantGrowthConsumption = -calorie_loss_per_second / t.Value;
          Crop crop = Assets.GetPrefab((Tag) t.Key.ToString()).GetComponent<Crop>();
          float num = 1f / (TUNING.CROPS.CROP_TYPES.Find((Predicate<Crop.CropVal>) (m => m.cropId == crop.cropId)).cropDuration / 600f);
          return UI.BUILDINGEFFECTS.DIET_CONSUMED_ITEM.text.Replace("{Food}", t.Key.ProperName()).Replace("{Amount}", GameUtil.GetFormattedPlantGrowth((float) (-(double) calorie_loss_per_second / (double) t.Value * (double) num * 100.0), GameUtil.TimeSlice.PerCycle));
        })).ToArray<string>());
        descriptorList.Add(new Descriptor(UI.BUILDINGEFFECTS.DIET_CONSUMED.text.Replace("{Foodlist}", newValue1), UI.BUILDINGEFFECTS.TOOLTIPS.DIET_CONSUMED.text.Replace("{Foodlist}", newValue2), Descriptor.DescriptorType.Effect, false));
      }
      if (this.diet.producedTags.Count > 0)
      {
        string newValue1 = string.Join(", ", this.diet.producedTags.Select<KeyValuePair<Tag, float>, string>((Func<KeyValuePair<Tag, float>, string>) (t => t.Key.ProperName())).ToArray<string>());
        string empty = string.Empty;
        // ISSUE: reference to a compiler-generated method
        string newValue2 = !this.diet.eatsPlantsDirectly ? string.Join("\n", this.diet.producedTags.Select<KeyValuePair<Tag, float>, string>((Func<KeyValuePair<Tag, float>, string>) (t => UI.BUILDINGEFFECTS.DIET_PRODUCED_ITEM.text.Replace("{Item}", t.Key.ProperName()).Replace("{Percent}", GameUtil.GetFormattedPercent(t.Value * 100f, GameUtil.TimeSlice.None)))).ToArray<string>()) : string.Join("\n", this.diet.producedTags.Select<KeyValuePair<Tag, float>, string>(new Func<KeyValuePair<Tag, float>, string>(descriptorsCAnonStorey1_1.\u003C\u003Em__0)).ToArray<string>());
        descriptorList.Add(new Descriptor(UI.BUILDINGEFFECTS.DIET_PRODUCED.text.Replace("{Items}", newValue1), UI.BUILDINGEFFECTS.TOOLTIPS.DIET_PRODUCED.text.Replace("{Items}", newValue2), Descriptor.DescriptorType.Effect, false));
      }
      return descriptorList;
    }
  }

  public class HungryStates : GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State
  {
    public GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State hungry;
    public CreatureCalorieMonitor.HungryStates.OutOfCaloriesState outofcalories;

    public class OutOfCaloriesState : GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State
    {
      public GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State wild;
      public GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State tame;
      public GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.State starvedtodeath;
    }
  }

  [SerializationConfig(MemberSerialization.OptIn)]
  public class Stomach
  {
    [Serialize]
    private List<CreatureCalorieMonitor.Stomach.CaloriesConsumedEntry> caloriesConsumed = new List<CreatureCalorieMonitor.Stomach.CaloriesConsumedEntry>();
    private float minPoopSizeInCalories;
    private GameObject owner;

    public Stomach(Diet diet, GameObject owner, float min_poop_size_in_calories)
    {
      this.diet = diet;
      this.owner = owner;
      this.minPoopSizeInCalories = min_poop_size_in_calories;
    }

    public Diet diet { get; private set; }

    public void Poop()
    {
      float num1 = 0.0f;
      Tag tag = Tag.Invalid;
      byte disease_idx = byte.MaxValue;
      int num2 = 0;
      bool flag = false;
      for (int index = 0; index < this.caloriesConsumed.Count; ++index)
      {
        CreatureCalorieMonitor.Stomach.CaloriesConsumedEntry caloriesConsumedEntry = this.caloriesConsumed[index];
        if ((double) caloriesConsumedEntry.calories > 0.0)
        {
          Diet.Info dietInfo = this.diet.GetDietInfo(caloriesConsumedEntry.tag);
          if (dietInfo != null && (!(tag != Tag.Invalid) || !(tag != dietInfo.producedElement)))
          {
            num1 += dietInfo.ConvertConsumptionMassToProducedMass(dietInfo.ConvertCaloriesToConsumptionMass(caloriesConsumedEntry.calories));
            tag = dietInfo.producedElement;
            disease_idx = dietInfo.diseaseIdx;
            num2 = (int) ((double) dietInfo.diseasePerKgProduced * (double) num1);
            caloriesConsumedEntry.calories = 0.0f;
            this.caloriesConsumed[index] = caloriesConsumedEntry;
            flag = flag || dietInfo.produceSolidTile;
          }
        }
      }
      if ((double) num1 <= 0.0 || tag == Tag.Invalid)
        return;
      Element element = ElementLoader.GetElement(tag);
      Debug.Assert(element != null, (object) "TODO: implement non-element tag spawning");
      int cell = Grid.PosToCell(this.owner.transform.GetPosition());
      float temperature = this.owner.GetComponent<PrimaryElement>().Temperature;
      if (element.IsLiquid)
        FallingWater.instance.AddParticle(cell, element.idx, num1, temperature, disease_idx, num2, true, false, false, false);
      else if (element.IsGas)
        SimMessages.AddRemoveSubstance(cell, (int) element.idx, CellEventLogger.Instance.ElementConsumerSimUpdate, num1, temperature, disease_idx, num2, true, -1);
      else if (flag)
      {
        int num3 = this.owner.GetComponent<Facing>().GetFrontCell();
        if (!Grid.IsValidCell(num3))
        {
          Debug.LogWarningFormat("{0} attemping to Poop {1} on invalid cell {2} from cell {3}", (object) this.owner, (object) element.name, (object) num3, (object) cell);
          num3 = cell;
        }
        SimMessages.AddRemoveSubstance(num3, (int) element.idx, CellEventLogger.Instance.ElementConsumerSimUpdate, num1, temperature, disease_idx, num2, true, -1);
      }
      else
        element.substance.SpawnResource(Grid.CellToPosCCC(cell, Grid.SceneLayer.Ore), num1, temperature, disease_idx, num2, false, false, false);
      KPrefabID component = this.owner.GetComponent<KPrefabID>();
      if (!Game.Instance.savedInfo.creaturePoopAmount.ContainsKey(component.PrefabTag))
        Game.Instance.savedInfo.creaturePoopAmount.Add(component.PrefabTag, 0.0f);
      Dictionary<Tag, float> creaturePoopAmount;
      Tag prefabTag;
      (creaturePoopAmount = Game.Instance.savedInfo.creaturePoopAmount)[prefabTag = component.PrefabTag] = creaturePoopAmount[prefabTag] + num1;
      PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, element.name, this.owner.transform, 1.5f, false);
    }

    public List<CreatureCalorieMonitor.Stomach.CaloriesConsumedEntry> GetCalorieEntries()
    {
      return this.caloriesConsumed;
    }

    public float GetTotalConsumedCalories()
    {
      float num = 0.0f;
      foreach (CreatureCalorieMonitor.Stomach.CaloriesConsumedEntry caloriesConsumedEntry in this.caloriesConsumed)
      {
        if ((double) caloriesConsumedEntry.calories > 0.0)
        {
          Diet.Info dietInfo = this.diet.GetDietInfo(caloriesConsumedEntry.tag);
          if (dietInfo != null && !(dietInfo.producedElement == Tag.Invalid))
            num += caloriesConsumedEntry.calories;
        }
      }
      return num;
    }

    public float GetFullness()
    {
      return this.GetTotalConsumedCalories() / this.minPoopSizeInCalories;
    }

    public bool IsReadyToPoop()
    {
      float consumedCalories = this.GetTotalConsumedCalories();
      if ((double) consumedCalories > 0.0)
        return (double) consumedCalories >= (double) this.minPoopSizeInCalories;
      return false;
    }

    public void Consume(Tag tag, float calories)
    {
      for (int index = 0; index < this.caloriesConsumed.Count; ++index)
      {
        CreatureCalorieMonitor.Stomach.CaloriesConsumedEntry caloriesConsumedEntry = this.caloriesConsumed[index];
        if (caloriesConsumedEntry.tag == tag)
        {
          caloriesConsumedEntry.calories += calories;
          this.caloriesConsumed[index] = caloriesConsumedEntry;
          return;
        }
      }
      this.caloriesConsumed.Add(new CreatureCalorieMonitor.Stomach.CaloriesConsumedEntry()
      {
        tag = tag,
        calories = calories
      });
    }

    public Tag GetNextPoopEntry()
    {
      for (int index = 0; index < this.caloriesConsumed.Count; ++index)
      {
        CreatureCalorieMonitor.Stomach.CaloriesConsumedEntry caloriesConsumedEntry = this.caloriesConsumed[index];
        if ((double) caloriesConsumedEntry.calories > 0.0)
        {
          Diet.Info dietInfo = this.diet.GetDietInfo(caloriesConsumedEntry.tag);
          if (dietInfo != null && !(dietInfo.producedElement == Tag.Invalid))
            return dietInfo.producedElement;
        }
      }
      return Tag.Invalid;
    }

    [Serializable]
    public struct CaloriesConsumedEntry
    {
      public Tag tag;
      public float calories;
    }
  }

  public class Instance : GameStateMachine<CreatureCalorieMonitor, CreatureCalorieMonitor.Instance, IStateMachineTarget, CreatureCalorieMonitor.Def>.GameInstance
  {
    public const float HUNGRY_RATIO = 0.9f;
    public AmountInstance calories;
    [Serialize]
    public CreatureCalorieMonitor.Stomach stomach;
    public float lastMealOrPoopTime;
    public AttributeInstance metabolism;
    public AttributeModifier deltaCalorieMetabolismModifier;

    public Instance(IStateMachineTarget master, CreatureCalorieMonitor.Def def)
      : base(master, def)
    {
      this.calories = Db.Get().Amounts.Calories.Lookup(this.gameObject);
      this.calories.value = this.calories.GetMax() * 0.9f;
      this.stomach = new CreatureCalorieMonitor.Stomach(def.diet, master.gameObject, def.minPoopSizeInCalories);
      this.metabolism = this.gameObject.GetAttributes().Add(Db.Get().CritterAttributes.Metabolism);
      this.deltaCalorieMetabolismModifier = new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, 1f, (string) DUPLICANTS.MODIFIERS.METABOLISM_CALORIE_MODIFIER.NAME, true, false, false);
      this.calories.deltaAttribute.Add(this.deltaCalorieMetabolismModifier);
    }

    public void OnCaloriesConsumed(object data)
    {
      CreatureCalorieMonitor.CaloriesConsumedEvent caloriesConsumedEvent = (CreatureCalorieMonitor.CaloriesConsumedEvent) data;
      this.calories.value += caloriesConsumedEvent.calories;
      this.stomach.Consume(caloriesConsumedEvent.tag, caloriesConsumedEvent.calories);
      this.lastMealOrPoopTime = Time.time;
    }

    public float GetDeathTimeRemaining()
    {
      return this.smi.def.deathTimer - (GameClock.Instance.GetTime() - this.sm.starvationStartTime.Get(this.smi));
    }

    public void Poop()
    {
      this.lastMealOrPoopTime = Time.time;
      this.stomach.Poop();
    }

    private float GetCalories0to1()
    {
      return this.calories.value / this.calories.GetMax();
    }

    public bool IsHungry()
    {
      return (double) this.GetCalories0to1() < 0.899999976158142;
    }

    public bool IsOutOfCalories()
    {
      return (double) this.GetCalories0to1() <= 0.0;
    }
  }
}
