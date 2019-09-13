// Decompiled with JetBrains decompiler
// Type: TUNING.TRAITS
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace TUNING
{
  public class TRAITS
  {
    public static float EARLYBIRD_MODIFIER = 2f;
    public static int EARLYBIRD_SCHEDULEBLOCK = 5;
    public static float NIGHTOWL_MODIFIER = 3f;
    public static float FLATULENCE_EMIT_INTERVAL_MIN = 10f;
    public static float FLATULENCE_EMIT_INTERVAL_MAX = 40f;
    public static float STINKY_EMIT_INTERVAL_MIN = 10f;
    public static float STINKY_EMIT_INTERVAL_MAX = 30f;
    public static float NARCOLEPSY_INTERVAL_MIN = 300f;
    public static float NARCOLEPSY_INTERVAL_MAX = 600f;
    public static float NARCOLEPSY_SLEEPDURATION_MIN = 15f;
    public static float NARCOLEPSY_SLEEPDURATION_MAX = 30f;
    public static int NO_ATTRIBUTE_BONUS = 0;
    public static int GOOD_ATTRIBUTE_BONUS = 3;
    public static int GREAT_ATTRIBUTE_BONUS = 5;
    public static int BAD_ATTRIBUTE_PENALTY = -3;
    public static int HORRIBLE_ATTRIBUTE_PENALTY = -5;
    public const float FLATULENCE_EMIT_MASS = 0.1f;
    public const float INTERRUPTED_SLEEP_STRESS_DELTA = 10f;
    public const float INTERRUPTED_SLEEP_ATHLETICS_DELTA = -2f;
    public static readonly List<System.Action> TRAIT_CREATORS;

    private static System.Action CreateDisabledTaskTrait(
      string id,
      string name,
      string desc,
      string disabled_chore_group,
      bool is_valid_starter_trait)
    {
      return (System.Action) (() =>
      {
        ChoreGroup[] disabled_chore_groups = new ChoreGroup[1]
        {
          Db.Get().ChoreGroups.Get(disabled_chore_group)
        };
        Db.Get().CreateTrait(id, name, desc, (string) null, true, disabled_chore_groups, false, is_valid_starter_trait);
      });
    }

    private static System.Action CreateTrait(
      string id,
      string name,
      string desc,
      string attributeId,
      float delta,
      string[] chore_groups,
      bool positiveTrait = false)
    {
      return (System.Action) (() =>
      {
        List<ChoreGroup> choreGroupList = new List<ChoreGroup>();
        foreach (string choreGroup in chore_groups)
          choreGroupList.Add(Db.Get().ChoreGroups.Get(choreGroup));
        Db.Get().CreateTrait(id, name, desc, (string) null, true, choreGroupList.ToArray(), positiveTrait, true).Add(new AttributeModifier(attributeId, delta, name, false, false, true));
      });
    }

    private static System.Action CreateAttributeEffectTrait(
      string id,
      string name,
      string desc,
      string attributeId,
      float delta,
      string attributeId2,
      float delta2,
      bool positiveTrait = false)
    {
      return (System.Action) (() =>
      {
        Trait trait = Db.Get().CreateTrait(id, name, desc, (string) null, true, (ChoreGroup[]) null, positiveTrait, true);
        trait.Add(new AttributeModifier(attributeId, delta, name, false, false, true));
        trait.Add(new AttributeModifier(attributeId2, delta2, name, false, false, true));
      });
    }

    private static System.Action CreateAttributeEffectTrait(
      string id,
      string name,
      string desc,
      string attributeId,
      float delta,
      bool positiveTrait = false,
      System.Action<GameObject> on_add = null)
    {
      return (System.Action) (() =>
      {
        Trait trait = Db.Get().CreateTrait(id, name, desc, (string) null, true, (ChoreGroup[]) null, positiveTrait, true);
        trait.Add(new AttributeModifier(attributeId, delta, name, false, false, true));
        trait.OnAddTrait = on_add;
      });
    }

    private static System.Action CreateEffectModifierTrait(
      string id,
      string name,
      string desc,
      string[] ignoredEffects,
      bool positiveTrait = false)
    {
      return (System.Action) (() => Db.Get().CreateTrait(id, name, desc, (string) null, true, (ChoreGroup[]) null, positiveTrait, true).AddIgnoredEffects(ignoredEffects));
    }

    private static System.Action CreateNamedTrait(
      string id,
      string name,
      string desc,
      bool positiveTrait = false)
    {
      return (System.Action) (() => Db.Get().CreateTrait(id, name, desc, (string) null, true, (ChoreGroup[]) null, positiveTrait, true));
    }

    private static System.Action CreateTrait(
      string id,
      string name,
      string desc,
      System.Action<GameObject> on_add,
      ChoreGroup[] disabled_chore_groups = null,
      bool positiveTrait = false,
      Func<string> extendedDescFn = null)
    {
      return (System.Action) (() =>
      {
        Trait trait = Db.Get().CreateTrait(id, name, desc, (string) null, true, disabled_chore_groups, positiveTrait, true);
        trait.OnAddTrait = on_add;
        if (extendedDescFn == null)
          return;
        trait.ExtendedTooltip += extendedDescFn;
      });
    }

    private static System.Action CreateComponentTrait<T>(
      string id,
      string name,
      string desc,
      bool positiveTrait = false,
      Func<string> extendedDescFn = null)
      where T : KMonoBehaviour
    {
      return (System.Action) (() =>
      {
        Trait trait = Db.Get().CreateTrait(id, name, desc, (string) null, true, (ChoreGroup[]) null, positiveTrait, true);
        trait.OnAddTrait = (System.Action<GameObject>) (go => go.FindOrAddUnityComponent<T>());
        if (extendedDescFn == null)
          return;
        trait.ExtendedTooltip += extendedDescFn;
      });
    }

    private static void OnAddStressVomiter(GameObject go)
    {
      Notification notification = new Notification((string) DUPLICANTS.STATUSITEMS.STRESSVOMITING.NOTIFICATION_NAME, NotificationType.Bad, HashedString.Invalid, (Func<List<Notification>, object, string>) ((notificationList, data) => (string) DUPLICANTS.STATUSITEMS.STRESSVOMITING.NOTIFICATION_TOOLTIP + notificationList.ReduceMessages(false)), (object) null, true, 0.0f, (Notification.ClickCallback) null, (object) null, (Transform) null);
      StatusItem tierOneBehaviourStatusItem = new StatusItem("StressSignalVomiter", (string) DUPLICANTS.STATUSITEMS.STRESS_SIGNAL_VOMITER.NAME, (string) DUPLICANTS.STATUSITEMS.STRESS_SIGNAL_VOMITER.TOOLTIP, string.Empty, StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, 129022);
      new StressBehaviourMonitor.Instance((IStateMachineTarget) go.GetComponent<KMonoBehaviour>(), (Func<ChoreProvider, Chore>) (chore_provider => (Chore) new StressEmoteChore((IStateMachineTarget) chore_provider, Db.Get().ChoreTypes.StressEmote, (HashedString) "anim_interrupt_vomiter_kanim", new HashedString[1]
      {
        (HashedString) "interrupt_vomiter"
      }, KAnim.PlayMode.Once, (Func<StatusItem>) (() => tierOneBehaviourStatusItem))), (Func<ChoreProvider, Chore>) (chore_provider => (Chore) new VomitChore(Db.Get().ChoreTypes.StressVomit, (IStateMachineTarget) chore_provider, Db.Get().DuplicantStatusItems.Vomiting, notification, (System.Action<Chore>) null)), "anim_loco_vomiter_kanim", 3f).StartSM();
    }

    private static void OnAddAggressive(GameObject go)
    {
      StatusItem tierOneBehaviourStatusItem = new StatusItem("StressSignalAggresive", (string) DUPLICANTS.STATUSITEMS.STRESS_SIGNAL_AGGRESIVE.NAME, (string) DUPLICANTS.STATUSITEMS.STRESS_SIGNAL_AGGRESIVE.TOOLTIP, string.Empty, StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, 129022);
      new StressBehaviourMonitor.Instance((IStateMachineTarget) go.GetComponent<KMonoBehaviour>(), (Func<ChoreProvider, Chore>) (chore_provider => (Chore) new StressEmoteChore((IStateMachineTarget) chore_provider, Db.Get().ChoreTypes.StressEmote, (HashedString) "anim_interrupt_destructive_kanim", new HashedString[1]
      {
        (HashedString) "interrupt_destruct"
      }, KAnim.PlayMode.Once, (Func<StatusItem>) (() => tierOneBehaviourStatusItem))), (Func<ChoreProvider, Chore>) (chore_provider => (Chore) new AggressiveChore((IStateMachineTarget) chore_provider, (System.Action<Chore>) null)), "anim_loco_destructive_kanim", 3f).StartSM();
    }

    private static void OnAddUglyCrier(GameObject go)
    {
      StatusItem tierOneBehaviourStatusItem = new StatusItem("StressSignalUglyCrier", (string) DUPLICANTS.STATUSITEMS.STRESS_SIGNAL_UGLY_CRIER.NAME, (string) DUPLICANTS.STATUSITEMS.STRESS_SIGNAL_UGLY_CRIER.TOOLTIP, string.Empty, StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, 129022);
      new StressBehaviourMonitor.Instance((IStateMachineTarget) go.GetComponent<KMonoBehaviour>(), (Func<ChoreProvider, Chore>) (chore_provider => (Chore) new StressEmoteChore((IStateMachineTarget) chore_provider, Db.Get().ChoreTypes.StressEmote, (HashedString) "anim_cry_kanim", new HashedString[3]
      {
        (HashedString) "working_pre",
        (HashedString) "working_loop",
        (HashedString) "working_pst"
      }, KAnim.PlayMode.Once, (Func<StatusItem>) (() => tierOneBehaviourStatusItem))), (Func<ChoreProvider, Chore>) (chore_provider => (Chore) new UglyCryChore(Db.Get().ChoreTypes.UglyCry, (IStateMachineTarget) chore_provider, (System.Action<Chore>) null)), "anim_loco_cry_kanim", 3f).StartSM();
    }

    private static void OnAddBingeEater(GameObject go)
    {
      StatusItem tierOneBehaviourStatusItem = new StatusItem("StressSignalBingeEater", (string) DUPLICANTS.STATUSITEMS.STRESS_SIGNAL_BINGE_EAT.NAME, (string) DUPLICANTS.STATUSITEMS.STRESS_SIGNAL_BINGE_EAT.TOOLTIP, string.Empty, StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, 129022);
      new StressBehaviourMonitor.Instance((IStateMachineTarget) go.GetComponent<KMonoBehaviour>(), (Func<ChoreProvider, Chore>) (chore_provider => (Chore) new StressEmoteChore((IStateMachineTarget) chore_provider, Db.Get().ChoreTypes.StressEmote, (HashedString) "anim_interrupt_binge_eat_kanim", new HashedString[1]
      {
        (HashedString) "interrupt_binge_eat"
      }, KAnim.PlayMode.Once, (Func<StatusItem>) (() => tierOneBehaviourStatusItem))), (Func<ChoreProvider, Chore>) (chore_provider => (Chore) new BingeEatChore((IStateMachineTarget) chore_provider, (System.Action<Chore>) null)), "anim_loco_binge_eat_kanim", 8f).StartSM();
    }

    static TRAITS()
    {
      List<System.Action> actionList1 = new List<System.Action>();
      actionList1.Add(TRAITS.CreateAttributeEffectTrait("None", (string) DUPLICANTS.CONGENITALTRAITS.NONE.NAME, (string) DUPLICANTS.CONGENITALTRAITS.NONE.DESC, string.Empty, (float) TRAITS.NO_ATTRIBUTE_BONUS, false, (System.Action<GameObject>) null));
      actionList1.Add(TRAITS.CreateComponentTrait<Stinky>("Stinky", (string) DUPLICANTS.CONGENITALTRAITS.STINKY.NAME, (string) DUPLICANTS.CONGENITALTRAITS.STINKY.DESC, false, (Func<string>) null));
      actionList1.Add(TRAITS.CreateAttributeEffectTrait("Ellie", (string) DUPLICANTS.CONGENITALTRAITS.ELLIE.NAME, (string) DUPLICANTS.CONGENITALTRAITS.ELLIE.DESC, "AirConsumptionRate", -0.045f, "DecorExpectation", -5f, false));
      actionList1.Add(TRAITS.CreateDisabledTaskTrait("Joshua", (string) DUPLICANTS.CONGENITALTRAITS.JOSHUA.NAME, (string) DUPLICANTS.CONGENITALTRAITS.JOSHUA.DESC, "Combat", true));
      actionList1.Add(TRAITS.CreateComponentTrait<Stinky>("Liam", (string) DUPLICANTS.CONGENITALTRAITS.LIAM.NAME, (string) DUPLICANTS.CONGENITALTRAITS.LIAM.DESC, false, (Func<string>) null));
      actionList1.Add(TRAITS.CreateDisabledTaskTrait("CantResearch", (string) DUPLICANTS.TRAITS.CANTRESEARCH.NAME, (string) DUPLICANTS.TRAITS.CANTRESEARCH.DESC, "Research", true));
      actionList1.Add(TRAITS.CreateDisabledTaskTrait("CantBuild", (string) DUPLICANTS.TRAITS.CANTBUILD.NAME, (string) DUPLICANTS.TRAITS.CANTBUILD.DESC, "Build", false));
      actionList1.Add(TRAITS.CreateDisabledTaskTrait("CantCook", (string) DUPLICANTS.TRAITS.CANTCOOK.NAME, (string) DUPLICANTS.TRAITS.CANTCOOK.DESC, "Cook", true));
      actionList1.Add(TRAITS.CreateDisabledTaskTrait("CantDig", (string) DUPLICANTS.TRAITS.CANTDIG.NAME, (string) DUPLICANTS.TRAITS.CANTDIG.DESC, "Dig", false));
      actionList1.Add(TRAITS.CreateDisabledTaskTrait("Hemophobia", (string) DUPLICANTS.TRAITS.HEMOPHOBIA.NAME, (string) DUPLICANTS.TRAITS.HEMOPHOBIA.DESC, "MedicalAid", true));
      actionList1.Add(TRAITS.CreateDisabledTaskTrait("ScaredyCat", (string) DUPLICANTS.TRAITS.SCAREDYCAT.NAME, (string) DUPLICANTS.TRAITS.SCAREDYCAT.DESC, "Combat", true));
      actionList1.Add(TRAITS.CreateNamedTrait("Allergies", (string) DUPLICANTS.TRAITS.ALLERGIES.NAME, (string) DUPLICANTS.TRAITS.ALLERGIES.DESC, false));
      actionList1.Add(TRAITS.CreateAttributeEffectTrait("MouthBreather", (string) DUPLICANTS.TRAITS.MOUTHBREATHER.NAME, (string) DUPLICANTS.TRAITS.MOUTHBREATHER.DESC, "AirConsumptionRate", 0.1f, false, (System.Action<GameObject>) null));
      actionList1.Add(TRAITS.CreateAttributeEffectTrait("CalorieBurner", (string) DUPLICANTS.TRAITS.CALORIEBURNER.NAME, (string) DUPLICANTS.TRAITS.CALORIEBURNER.DESC, "CaloriesDelta", -833.3333f, false, (System.Action<GameObject>) null));
      actionList1.Add(TRAITS.CreateAttributeEffectTrait("SmallBladder", (string) DUPLICANTS.TRAITS.SMALLBLADDER.NAME, (string) DUPLICANTS.TRAITS.SMALLBLADDER.DESC, "BladderDelta", 0.0002777778f, false, (System.Action<GameObject>) null));
      actionList1.Add(TRAITS.CreateAttributeEffectTrait("Anemic", (string) DUPLICANTS.TRAITS.ANEMIC.NAME, (string) DUPLICANTS.TRAITS.ANEMIC.DESC, "Athletics", (float) TRAITS.HORRIBLE_ATTRIBUTE_PENALTY, false, (System.Action<GameObject>) null));
      actionList1.Add(TRAITS.CreateAttributeEffectTrait("SlowLearner", (string) DUPLICANTS.TRAITS.SLOWLEARNER.NAME, (string) DUPLICANTS.TRAITS.SLOWLEARNER.DESC, "Learning", (float) TRAITS.BAD_ATTRIBUTE_PENALTY, false, (System.Action<GameObject>) null));
      actionList1.Add(TRAITS.CreateAttributeEffectTrait("NoodleArms", (string) DUPLICANTS.TRAITS.NOODLEARMS.NAME, (string) DUPLICANTS.TRAITS.NOODLEARMS.DESC, "Strength", (float) TRAITS.BAD_ATTRIBUTE_PENALTY, false, (System.Action<GameObject>) null));
      actionList1.Add(TRAITS.CreateAttributeEffectTrait("InteriorDecorator", (string) DUPLICANTS.TRAITS.INTERIORDECORATOR.NAME, (string) DUPLICANTS.TRAITS.INTERIORDECORATOR.DESC, "Art", (float) TRAITS.GOOD_ATTRIBUTE_BONUS, "DecorExpectation", -5f, true));
      actionList1.Add(TRAITS.CreateAttributeEffectTrait("SimpleTastes", (string) DUPLICANTS.TRAITS.SIMPLETASTES.NAME, (string) DUPLICANTS.TRAITS.SIMPLETASTES.DESC, "FoodExpectation", 1f, true, (System.Action<GameObject>) null));
      actionList1.Add(TRAITS.CreateAttributeEffectTrait("Foodie", (string) DUPLICANTS.TRAITS.FOODIE.NAME, (string) DUPLICANTS.TRAITS.FOODIE.DESC, "Cooking", (float) TRAITS.GOOD_ATTRIBUTE_BONUS, "FoodExpectation", -1f, true));
      actionList1.Add(TRAITS.CreateAttributeEffectTrait("Regeneration", (string) DUPLICANTS.TRAITS.REGENERATION.NAME, (string) DUPLICANTS.TRAITS.REGENERATION.DESC, "HitPointsDelta", 0.03333334f, false, (System.Action<GameObject>) null));
      actionList1.Add(TRAITS.CreateAttributeEffectTrait("DeeperDiversLungs", (string) DUPLICANTS.TRAITS.DEEPERDIVERSLUNGS.NAME, (string) DUPLICANTS.TRAITS.DEEPERDIVERSLUNGS.DESC, "AirConsumptionRate", -0.05f, false, (System.Action<GameObject>) null));
      actionList1.Add(TRAITS.CreateAttributeEffectTrait("SunnyDisposition", (string) DUPLICANTS.TRAITS.SUNNYDISPOSITION.NAME, (string) DUPLICANTS.TRAITS.SUNNYDISPOSITION.DESC, "StressDelta", -0.03333334f, false, (System.Action<GameObject>) (go => go.GetComponent<KBatchedAnimController>().AddAnimOverrides(Assets.GetAnim((HashedString) "anim_loco_happy_kanim"), 0.0f))));
      actionList1.Add(TRAITS.CreateAttributeEffectTrait("RockCrusher", (string) DUPLICANTS.TRAITS.ROCKCRUSHER.NAME, (string) DUPLICANTS.TRAITS.ROCKCRUSHER.DESC, "Strength", 10f, false, (System.Action<GameObject>) null));
      actionList1.Add(TRAITS.CreateTrait("Uncultured", (string) DUPLICANTS.TRAITS.UNCULTURED.NAME, (string) DUPLICANTS.TRAITS.UNCULTURED.DESC, "DecorExpectation", 20f, new string[1]
      {
        "Art"
      }, true));
      actionList1.Add(TRAITS.CreateNamedTrait("Archaeologist", (string) DUPLICANTS.TRAITS.ARCHAEOLOGIST.NAME, (string) DUPLICANTS.TRAITS.ARCHAEOLOGIST.DESC, false));
      actionList1.Add(TRAITS.CreateAttributeEffectTrait("WeakImmuneSystem", (string) DUPLICANTS.TRAITS.WEAKIMMUNESYSTEM.NAME, (string) DUPLICANTS.TRAITS.WEAKIMMUNESYSTEM.DESC, "GermResistance", -1f, false, (System.Action<GameObject>) null));
      actionList1.Add(TRAITS.CreateAttributeEffectTrait("IrritableBowel", (string) DUPLICANTS.TRAITS.IRRITABLEBOWEL.NAME, (string) DUPLICANTS.TRAITS.IRRITABLEBOWEL.DESC, "ToiletEfficiency", -0.5f, false, (System.Action<GameObject>) null));
      actionList1.Add(TRAITS.CreateComponentTrait<Flatulence>("Flatulence", (string) DUPLICANTS.TRAITS.FLATULENCE.NAME, (string) DUPLICANTS.TRAITS.FLATULENCE.DESC, false, (Func<string>) null));
      actionList1.Add(TRAITS.CreateComponentTrait<Snorer>("Snorer", (string) DUPLICANTS.TRAITS.SNORER.NAME, (string) DUPLICANTS.TRAITS.SNORER.DESC, false, (Func<string>) null));
      actionList1.Add(TRAITS.CreateComponentTrait<Narcolepsy>("Narcolepsy", (string) DUPLICANTS.TRAITS.NARCOLEPSY.NAME, (string) DUPLICANTS.TRAITS.NARCOLEPSY.DESC, false, (Func<string>) null));
      actionList1.Add(TRAITS.CreateAttributeEffectTrait("Twinkletoes", (string) DUPLICANTS.TRAITS.TWINKLETOES.NAME, (string) DUPLICANTS.TRAITS.TWINKLETOES.DESC, "Athletics", (float) TRAITS.GOOD_ATTRIBUTE_BONUS, true, (System.Action<GameObject>) null));
      actionList1.Add(TRAITS.CreateAttributeEffectTrait("Greasemonkey", (string) DUPLICANTS.TRAITS.GREASEMONKEY.NAME, (string) DUPLICANTS.TRAITS.GREASEMONKEY.DESC, "Machinery", (float) TRAITS.GOOD_ATTRIBUTE_BONUS, true, (System.Action<GameObject>) null));
      actionList1.Add(TRAITS.CreateAttributeEffectTrait("MoleHands", (string) DUPLICANTS.TRAITS.MOLEHANDS.NAME, (string) DUPLICANTS.TRAITS.MOLEHANDS.DESC, "Digging", (float) TRAITS.GOOD_ATTRIBUTE_BONUS, true, (System.Action<GameObject>) null));
      actionList1.Add(TRAITS.CreateAttributeEffectTrait("FastLearner", (string) DUPLICANTS.TRAITS.FASTLEARNER.NAME, (string) DUPLICANTS.TRAITS.FASTLEARNER.DESC, "Learning", (float) TRAITS.GOOD_ATTRIBUTE_BONUS, true, (System.Action<GameObject>) null));
      actionList1.Add(TRAITS.CreateAttributeEffectTrait("DiversLung", (string) DUPLICANTS.TRAITS.DIVERSLUNG.NAME, (string) DUPLICANTS.TRAITS.DIVERSLUNG.DESC, "AirConsumptionRate", -0.025f, true, (System.Action<GameObject>) null));
      actionList1.Add(TRAITS.CreateAttributeEffectTrait("StrongArm", (string) DUPLICANTS.TRAITS.STRONGARM.NAME, (string) DUPLICANTS.TRAITS.STRONGARM.DESC, "Strength", (float) TRAITS.GOOD_ATTRIBUTE_BONUS, true, (System.Action<GameObject>) null));
      actionList1.Add(TRAITS.CreateNamedTrait("IronGut", (string) DUPLICANTS.TRAITS.IRONGUT.NAME, (string) DUPLICANTS.TRAITS.IRONGUT.DESC, true));
      actionList1.Add(TRAITS.CreateAttributeEffectTrait("StrongImmuneSystem", (string) DUPLICANTS.TRAITS.STRONGIMMUNESYSTEM.NAME, (string) DUPLICANTS.TRAITS.STRONGIMMUNESYSTEM.DESC, "GermResistance", 1f, true, (System.Action<GameObject>) null));
      actionList1.Add(TRAITS.CreateAttributeEffectTrait("BedsideManner", (string) DUPLICANTS.TRAITS.BEDSIDEMANNER.NAME, (string) DUPLICANTS.TRAITS.BEDSIDEMANNER.DESC, "Caring", (float) TRAITS.GOOD_ATTRIBUTE_BONUS, true, (System.Action<GameObject>) null));
      List<System.Action> actionList2 = actionList1;
      string name1 = (string) DUPLICANTS.TRAITS.AGGRESSIVE.NAME;
      string desc1 = (string) DUPLICANTS.TRAITS.AGGRESSIVE.DESC;
      // ISSUE: reference to a compiler-generated field
      if (TRAITS.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        TRAITS.\u003C\u003Ef__mg\u0024cache0 = new System.Action<GameObject>(TRAITS.OnAddAggressive);
      }
      // ISSUE: reference to a compiler-generated field
      System.Action<GameObject> fMgCache0 = TRAITS.\u003C\u003Ef__mg\u0024cache0;
      Func<string> extendedDescFn = (Func<string>) (() => (string) DUPLICANTS.TRAITS.AGGRESSIVE.NOREPAIR);
      System.Action trait1 = TRAITS.CreateTrait("Aggressive", name1, desc1, fMgCache0, (ChoreGroup[]) null, false, extendedDescFn);
      actionList2.Add(trait1);
      List<System.Action> actionList3 = actionList1;
      string name2 = (string) DUPLICANTS.TRAITS.UGLYCRIER.NAME;
      string desc2 = (string) DUPLICANTS.TRAITS.UGLYCRIER.DESC;
      // ISSUE: reference to a compiler-generated field
      if (TRAITS.\u003C\u003Ef__mg\u0024cache1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        TRAITS.\u003C\u003Ef__mg\u0024cache1 = new System.Action<GameObject>(TRAITS.OnAddUglyCrier);
      }
      // ISSUE: reference to a compiler-generated field
      System.Action<GameObject> fMgCache1 = TRAITS.\u003C\u003Ef__mg\u0024cache1;
      System.Action trait2 = TRAITS.CreateTrait("UglyCrier", name2, desc2, fMgCache1, (ChoreGroup[]) null, false, (Func<string>) null);
      actionList3.Add(trait2);
      List<System.Action> actionList4 = actionList1;
      string name3 = (string) DUPLICANTS.TRAITS.BINGEEATER.NAME;
      string desc3 = (string) DUPLICANTS.TRAITS.BINGEEATER.DESC;
      // ISSUE: reference to a compiler-generated field
      if (TRAITS.\u003C\u003Ef__mg\u0024cache2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        TRAITS.\u003C\u003Ef__mg\u0024cache2 = new System.Action<GameObject>(TRAITS.OnAddBingeEater);
      }
      // ISSUE: reference to a compiler-generated field
      System.Action<GameObject> fMgCache2 = TRAITS.\u003C\u003Ef__mg\u0024cache2;
      System.Action trait3 = TRAITS.CreateTrait("BingeEater", name3, desc3, fMgCache2, (ChoreGroup[]) null, false, (Func<string>) null);
      actionList4.Add(trait3);
      List<System.Action> actionList5 = actionList1;
      string name4 = (string) DUPLICANTS.TRAITS.STRESSVOMITER.NAME;
      string desc4 = (string) DUPLICANTS.TRAITS.STRESSVOMITER.DESC;
      // ISSUE: reference to a compiler-generated field
      if (TRAITS.\u003C\u003Ef__mg\u0024cache3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        TRAITS.\u003C\u003Ef__mg\u0024cache3 = new System.Action<GameObject>(TRAITS.OnAddStressVomiter);
      }
      // ISSUE: reference to a compiler-generated field
      System.Action<GameObject> fMgCache3 = TRAITS.\u003C\u003Ef__mg\u0024cache3;
      System.Action trait4 = TRAITS.CreateTrait("StressVomiter", name4, desc4, fMgCache3, (ChoreGroup[]) null, false, (Func<string>) null);
      actionList5.Add(trait4);
      actionList1.Add(TRAITS.CreateComponentTrait<EarlyBird>("EarlyBird", (string) DUPLICANTS.TRAITS.EARLYBIRD.NAME, (string) DUPLICANTS.TRAITS.EARLYBIRD.DESC, true, (Func<string>) (() => string.Format((string) DUPLICANTS.TRAITS.EARLYBIRD.EXTENDED_DESC, (object) GameUtil.AddPositiveSign(TRAITS.EARLYBIRD_MODIFIER.ToString(), true)))));
      actionList1.Add(TRAITS.CreateComponentTrait<NightOwl>("NightOwl", (string) DUPLICANTS.TRAITS.NIGHTOWL.NAME, (string) DUPLICANTS.TRAITS.NIGHTOWL.DESC, true, (Func<string>) (() => string.Format((string) DUPLICANTS.TRAITS.NIGHTOWL.EXTENDED_DESC, (object) GameUtil.AddPositiveSign(TRAITS.NIGHTOWL_MODIFIER.ToString(), true)))));
      actionList1.Add(TRAITS.CreateComponentTrait<Claustrophobic>("Claustrophobic", (string) DUPLICANTS.TRAITS.NEEDS.CLAUSTROPHOBIC.NAME, (string) DUPLICANTS.TRAITS.NEEDS.CLAUSTROPHOBIC.DESC, false, (Func<string>) null));
      actionList1.Add(TRAITS.CreateComponentTrait<PrefersWarmer>("PrefersWarmer", (string) DUPLICANTS.TRAITS.NEEDS.PREFERSWARMER.NAME, (string) DUPLICANTS.TRAITS.NEEDS.PREFERSWARMER.DESC, false, (Func<string>) null));
      actionList1.Add(TRAITS.CreateComponentTrait<PrefersColder>("PrefersColder", (string) DUPLICANTS.TRAITS.NEEDS.PREFERSCOOLER.NAME, (string) DUPLICANTS.TRAITS.NEEDS.PREFERSCOOLER.DESC, false, (Func<string>) null));
      actionList1.Add(TRAITS.CreateComponentTrait<SensitiveFeet>("SensitiveFeet", (string) DUPLICANTS.TRAITS.NEEDS.SENSITIVEFEET.NAME, (string) DUPLICANTS.TRAITS.NEEDS.SENSITIVEFEET.DESC, false, (Func<string>) null));
      actionList1.Add(TRAITS.CreateComponentTrait<Fashionable>("Fashionable", (string) DUPLICANTS.TRAITS.NEEDS.FASHIONABLE.NAME, (string) DUPLICANTS.TRAITS.NEEDS.FASHIONABLE.DESC, false, (Func<string>) null));
      actionList1.Add(TRAITS.CreateComponentTrait<Climacophobic>("Climacophobic", (string) DUPLICANTS.TRAITS.NEEDS.CLIMACOPHOBIC.NAME, (string) DUPLICANTS.TRAITS.NEEDS.CLIMACOPHOBIC.DESC, false, (Func<string>) null));
      actionList1.Add(TRAITS.CreateComponentTrait<SolitarySleeper>("SolitarySleeper", (string) DUPLICANTS.TRAITS.NEEDS.SOLITARYSLEEPER.NAME, (string) DUPLICANTS.TRAITS.NEEDS.SOLITARYSLEEPER.DESC, false, (Func<string>) null));
      actionList1.Add(TRAITS.CreateComponentTrait<Workaholic>("Workaholic", (string) DUPLICANTS.TRAITS.NEEDS.WORKAHOLIC.NAME, (string) DUPLICANTS.TRAITS.NEEDS.WORKAHOLIC.DESC, false, (Func<string>) null));
      TRAITS.TRAIT_CREATORS = actionList1;
    }
  }
}
