// Decompiled with JetBrains decompiler
// Type: BaseDreckoConfig
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using System.Collections.Generic;
using UnityEngine;

public static class BaseDreckoConfig
{
  public static GameObject BaseDrecko(
    string id,
    string name,
    string desc,
    string anim_file,
    string trait_id,
    bool is_baby,
    string symbol_override_prefix,
    float warnLowTemp,
    float warnHighTemp)
  {
    GameObject placedEntity = EntityTemplates.CreatePlacedEntity(id, name, desc, 200f, Assets.GetAnim((HashedString) anim_file), "idle_loop", Grid.SceneLayer.Creatures, 1, 1, TUNING.DECOR.BONUS.TIER0, new EffectorValues(), SimHashes.Creature, (List<Tag>) null, (float) (((double) warnLowTemp + (double) warnHighTemp) / 2.0));
    KPrefabID component = placedEntity.GetComponent<KPrefabID>();
    component.AddTag(GameTags.Creatures.Walker, false);
    component.prefabInitFn += (KPrefabID.PrefabFn) (inst => inst.GetAttributes().Add(Db.Get().Attributes.MaxUnderwaterTravelCost));
    string str = "DreckoNavGrid";
    if (is_baby)
      str = "DreckoBabyNavGrid";
    GameObject template = placedEntity;
    FactionManager.FactionID faction = FactionManager.FactionID.Pest;
    string initialTraitID = trait_id;
    string NavGridName = str;
    float moveSpeed = 1f;
    string onDeathDropID = "Meat";
    int onDeathDropCount = 2;
    float warningHighTemperature = warnHighTemp;
    EntityTemplates.ExtendEntityToBasicCreature(template, faction, initialTraitID, NavGridName, NavType.Floor, 32, moveSpeed, onDeathDropID, onDeathDropCount, true, false, warnLowTemp, warningHighTemperature, warnLowTemp - 20f, warnHighTemp + 20f);
    if (!string.IsNullOrEmpty(symbol_override_prefix))
      placedEntity.AddOrGet<SymbolOverrideController>().ApplySymbolOverridesByAffix(Assets.GetAnim((HashedString) anim_file), symbol_override_prefix, (string) null, 0);
    placedEntity.AddOrGet<Trappable>();
    placedEntity.AddOrGetDef<CreatureFallMonitor.Def>();
    placedEntity.AddOrGet<LoopingSounds>();
    placedEntity.AddOrGetDef<ThreatMonitor.Def>().fleethresholdState = Health.HealthState.Dead;
    placedEntity.AddWeapon(1f, 1f, AttackProperties.DamageType.Standard, AttackProperties.TargetType.Single, 1, 0.0f);
    EntityTemplates.CreateAndRegisterBaggedCreature(placedEntity, true, true, false);
    ChoreTable.Builder builder = new ChoreTable.Builder().Add((StateMachine.BaseDef) new DeathStates.Def(), true).Add((StateMachine.BaseDef) new AnimInterruptStates.Def(), true).Add((StateMachine.BaseDef) new GrowUpStates.Def(), true).Add((StateMachine.BaseDef) new TrappedStates.Def(), true).Add((StateMachine.BaseDef) new IncubatingStates.Def(), true).Add((StateMachine.BaseDef) new BaggedStates.Def(), true).Add((StateMachine.BaseDef) new FallStates.Def(), true).Add((StateMachine.BaseDef) new StunnedStates.Def(), true).Add((StateMachine.BaseDef) new DrowningStates.Def(), true).Add((StateMachine.BaseDef) new DebugGoToStates.Def(), true).Add((StateMachine.BaseDef) new FleeStates.Def(), true).Add((StateMachine.BaseDef) new AttackStates.Def(), !is_baby).PushInterruptGroup().Add((StateMachine.BaseDef) new FixedCaptureStates.Def(), true).Add((StateMachine.BaseDef) new RanchedStates.Def(), true).Add((StateMachine.BaseDef) new LayEggStates.Def(), true).Add((StateMachine.BaseDef) new EatStates.Def(), true).Add((StateMachine.BaseDef) new PlayAnimsStates.Def(GameTags.Creatures.Poop, false, "poop", (string) STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.NAME, (string) STRINGS.CREATURES.STATUSITEMS.EXPELLING_SOLID.TOOLTIP), true).Add((StateMachine.BaseDef) new CallAdultStates.Def(), true).PopInterruptGroup();
    IdleStates.Def def1 = new IdleStates.Def();
    IdleStates.Def def2 = def1;
    // ISSUE: reference to a compiler-generated field
    if (BaseDreckoConfig.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      BaseDreckoConfig.\u003C\u003Ef__mg\u0024cache0 = new IdleStates.Def.IdleAnimCallback(BaseDreckoConfig.CustomIdleAnim);
    }
    // ISSUE: reference to a compiler-generated field
    IdleStates.Def.IdleAnimCallback fMgCache0 = BaseDreckoConfig.\u003C\u003Ef__mg\u0024cache0;
    def2.customIdleAnim = fMgCache0;
    IdleStates.Def def3 = def1;
    ChoreTable.Builder chore_table = builder.Add((StateMachine.BaseDef) def3, true);
    EntityTemplates.AddCreatureBrain(placedEntity, chore_table, GameTags.Creatures.Species.DreckoSpecies, symbol_override_prefix);
    return placedEntity;
  }

  private static HashedString CustomIdleAnim(
    IdleStates.Instance smi,
    ref HashedString pre_anim)
  {
    CellOffset offset = new CellOffset(0, -1);
    bool facing = smi.GetComponent<Facing>().GetFacing();
    switch (smi.GetComponent<Navigator>().CurrentNavType)
    {
      case NavType.Floor:
        offset = !facing ? new CellOffset(-1, -1) : new CellOffset(1, -1);
        break;
      case NavType.Ceiling:
        offset = !facing ? new CellOffset(-1, 1) : new CellOffset(1, 1);
        break;
    }
    HashedString hashedString = (HashedString) "idle_loop";
    int cell = Grid.OffsetCell(Grid.PosToCell((StateMachine.Instance) smi), offset);
    if (Grid.IsValidCell(cell) && !Grid.Solid[cell])
    {
      pre_anim = (HashedString) "idle_loop_hang_pre";
      hashedString = (HashedString) "idle_loop_hang";
    }
    return hashedString;
  }
}
