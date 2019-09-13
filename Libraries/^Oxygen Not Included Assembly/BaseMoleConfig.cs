// Decompiled with JetBrains decompiler
// Type: BaseMoleConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public static class BaseMoleConfig
{
  private static readonly string[] SolidIdleAnims = new string[4]
  {
    "idle1",
    "idle2",
    "idle3",
    "idle4"
  };

  public static GameObject BaseMole(
    string id,
    string name,
    string desc,
    string traitId,
    string anim_file,
    bool is_baby)
  {
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity(id, name, desc, 25f, Assets.GetAnim((HashedString) anim_file), "idle_loop", Grid.SceneLayer.Creatures, 1, 1, TUNING.BUILDINGS.DECOR.NONE, new EffectorValues(), SimHashes.Creature, (List<Tag>) null, 293f);
    EntityTemplates.ExtendEntityToBasicCreature(placedEntity, FactionManager.FactionID.Pest, traitId, "DiggerNavGrid", NavType.Floor, 32, 2f, "Meat", 10, true, false, 123.15f, 673.15f, 73.14999f, 773.15f);
    placedEntity.AddOrGetDef<CreatureFallMonitor.Def>();
    placedEntity.AddOrGet<Trappable>();
    placedEntity.AddOrGetDef<DiggerMonitor.Def>().depthToDig = MoleTuning.DEPTH_TO_HIDE;
    EntityTemplates.CreateAndRegisterBaggedCreature(placedEntity, true, true, false);
    placedEntity.GetComponent<KPrefabID>().AddTag(GameTags.Creatures.Walker, false);
    ChoreTable.Builder builder = new ChoreTable.Builder().Add((StateMachine.BaseDef) new DeathStates.Def(), true).Add((StateMachine.BaseDef) new AnimInterruptStates.Def(), true).Add((StateMachine.BaseDef) new FallStates.Def(), true).Add((StateMachine.BaseDef) new StunnedStates.Def(), true).Add((StateMachine.BaseDef) new DrowningStates.Def(), true).Add((StateMachine.BaseDef) new DiggerStates.Def(), true).Add((StateMachine.BaseDef) new GrowUpStates.Def(), true).Add((StateMachine.BaseDef) new TrappedStates.Def(), true).Add((StateMachine.BaseDef) new IncubatingStates.Def(), true).Add((StateMachine.BaseDef) new BaggedStates.Def(), true).Add((StateMachine.BaseDef) new DebugGoToStates.Def(), true).Add((StateMachine.BaseDef) new FleeStates.Def(), true).Add((StateMachine.BaseDef) new AttackStates.Def(), !is_baby).PushInterruptGroup().Add((StateMachine.BaseDef) new FixedCaptureStates.Def(), true).Add((StateMachine.BaseDef) new RanchedStates.Def(), true).Add((StateMachine.BaseDef) new LayEggStates.Def(), true).Add((StateMachine.BaseDef) new CreatureSleepStates.Def(), true).Add((StateMachine.BaseDef) new EatStates.Def(), true).Add((StateMachine.BaseDef) new NestingPoopState.Def(!is_baby ? SimHashes.Regolith.CreateTag() : Tag.Invalid), true).Add((StateMachine.BaseDef) new PlayAnimsStates.Def(GameTags.Creatures.Poop, false, "poop", (string) STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.NAME, (string) STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.TOOLTIP), true).PopInterruptGroup();
    IdleStates.Def def1 = new IdleStates.Def();
    IdleStates.Def def2 = def1;
    // ISSUE: reference to a compiler-generated field
    if (BaseMoleConfig.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      BaseMoleConfig.\u003C\u003Ef__mg\u0024cache0 = new IdleStates.Def.IdleAnimCallback(BaseMoleConfig.CustomIdleAnim);
    }
    // ISSUE: reference to a compiler-generated field
    IdleStates.Def.IdleAnimCallback fMgCache0 = BaseMoleConfig.\u003C\u003Ef__mg\u0024cache0;
    def2.customIdleAnim = fMgCache0;
    IdleStates.Def def3 = def1;
    ChoreTable.Builder chore_table = builder.Add((StateMachine.BaseDef) def3, true);
    EntityTemplates.AddCreatureBrain(placedEntity, chore_table, GameTags.Creatures.Species.MoleSpecies, (string) null);
    return placedEntity;
  }

  public static List<Diet.Info> SimpleOreDiet(
    List<Tag> elementTags,
    float caloriesPerKg,
    float producedConversionRate)
  {
    List<Diet.Info> infoList = new List<Diet.Info>();
    foreach (Tag elementTag in elementTags)
      infoList.Add(new Diet.Info(new HashSet<Tag>()
      {
        elementTag
      }, elementTag, caloriesPerKg, producedConversionRate, (string) null, 0.0f, true, false));
    return infoList;
  }

  private static HashedString CustomIdleAnim(
    IdleStates.Instance smi,
    ref HashedString pre_anim)
  {
    if (smi.gameObject.GetComponent<Navigator>().CurrentNavType == NavType.Solid)
    {
      int index = Random.Range(0, BaseMoleConfig.SolidIdleAnims.Length);
      return (HashedString) BaseMoleConfig.SolidIdleAnims[index];
    }
    if (smi.gameObject.GetDef<BabyMonitor.Def>() != null && Random.Range(0, 100) >= 90)
      return (HashedString) "drill_fail";
    return (HashedString) "idle_loop";
  }
}
