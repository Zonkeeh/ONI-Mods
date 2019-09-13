// Decompiled with JetBrains decompiler
// Type: ScaleGrowthMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ScaleGrowthMonitor : GameStateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>
{
  private static HashedString[] SCALE_SYMBOL_NAMES = new HashedString[5]
  {
    (HashedString) "scale_0",
    (HashedString) "scale_1",
    (HashedString) "scale_2",
    (HashedString) "scale_3",
    (HashedString) "scale_4"
  };
  public ScaleGrowthMonitor.GrowingState growing;
  public GameStateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.State fullyGrown;
  private AttributeModifier scaleGrowthModifier;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.growing;
    GameStateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.State state1 = this.root.Enter((StateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.State.Callback) (smi => ScaleGrowthMonitor.UpdateScales(smi, 0.0f)));
    // ISSUE: reference to a compiler-generated field
    if (ScaleGrowthMonitor.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      ScaleGrowthMonitor.\u003C\u003Ef__mg\u0024cache0 = new System.Action<ScaleGrowthMonitor.Instance, float>(ScaleGrowthMonitor.UpdateScales);
    }
    // ISSUE: reference to a compiler-generated field
    System.Action<ScaleGrowthMonitor.Instance, float> fMgCache0 = ScaleGrowthMonitor.\u003C\u003Ef__mg\u0024cache0;
    state1.Update(fMgCache0, UpdateRate.SIM_1000ms, false);
    GameStateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.State state2 = this.growing.DefaultState(this.growing.growing);
    GameStateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.State fullyGrown = this.fullyGrown;
    // ISSUE: reference to a compiler-generated field
    if (ScaleGrowthMonitor.\u003C\u003Ef__mg\u0024cache1 == null)
    {
      // ISSUE: reference to a compiler-generated field
      ScaleGrowthMonitor.\u003C\u003Ef__mg\u0024cache1 = new StateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.Transition.ConditionCallback(ScaleGrowthMonitor.AreScalesFullyGrown);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.Transition.ConditionCallback fMgCache1 = ScaleGrowthMonitor.\u003C\u003Ef__mg\u0024cache1;
    state2.Transition(fullyGrown, fMgCache1, UpdateRate.SIM_1000ms);
    GameStateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.State growing1 = this.growing.growing;
    GameStateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.State stunted1 = this.growing.stunted;
    // ISSUE: reference to a compiler-generated field
    if (ScaleGrowthMonitor.\u003C\u003Ef__mg\u0024cache2 == null)
    {
      // ISSUE: reference to a compiler-generated field
      ScaleGrowthMonitor.\u003C\u003Ef__mg\u0024cache2 = new StateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.Transition.ConditionCallback(ScaleGrowthMonitor.IsInCorrectAtmosphere);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.Transition.ConditionCallback condition1 = GameStateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.Not(ScaleGrowthMonitor.\u003C\u003Ef__mg\u0024cache2);
    GameStateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.State state3 = growing1.Transition(stunted1, condition1, UpdateRate.SIM_1000ms);
    // ISSUE: reference to a compiler-generated field
    if (ScaleGrowthMonitor.\u003C\u003Ef__mg\u0024cache3 == null)
    {
      // ISSUE: reference to a compiler-generated field
      ScaleGrowthMonitor.\u003C\u003Ef__mg\u0024cache3 = new StateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.State.Callback(ScaleGrowthMonitor.ApplyModifier);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.State.Callback fMgCache3 = ScaleGrowthMonitor.\u003C\u003Ef__mg\u0024cache3;
    GameStateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.State state4 = state3.Enter(fMgCache3);
    // ISSUE: reference to a compiler-generated field
    if (ScaleGrowthMonitor.\u003C\u003Ef__mg\u0024cache4 == null)
    {
      // ISSUE: reference to a compiler-generated field
      ScaleGrowthMonitor.\u003C\u003Ef__mg\u0024cache4 = new StateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.State.Callback(ScaleGrowthMonitor.RemoveModifier);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.State.Callback fMgCache4 = ScaleGrowthMonitor.\u003C\u003Ef__mg\u0024cache4;
    state4.Exit(fMgCache4);
    GameStateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.State stunted2 = this.growing.stunted;
    GameStateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.State growing2 = this.growing.growing;
    // ISSUE: reference to a compiler-generated field
    if (ScaleGrowthMonitor.\u003C\u003Ef__mg\u0024cache5 == null)
    {
      // ISSUE: reference to a compiler-generated field
      ScaleGrowthMonitor.\u003C\u003Ef__mg\u0024cache5 = new StateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.Transition.ConditionCallback(ScaleGrowthMonitor.IsInCorrectAtmosphere);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.Transition.ConditionCallback fMgCache5 = ScaleGrowthMonitor.\u003C\u003Ef__mg\u0024cache5;
    GameStateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.State state5 = stunted2.Transition(growing2, fMgCache5, UpdateRate.SIM_1000ms);
    string name1 = (string) CREATURES.STATUSITEMS.STUNTED_SCALE_GROWTH.NAME;
    string tooltip1 = (string) CREATURES.STATUSITEMS.STUNTED_SCALE_GROWTH.TOOLTIP;
    StatusItemCategory main = Db.Get().StatusItemCategories.Main;
    string name2 = name1;
    string tooltip2 = tooltip1;
    string empty = string.Empty;
    HashedString render_overlay = new HashedString();
    StatusItemCategory category = main;
    state5.ToggleStatusItem(name2, tooltip2, empty, StatusItem.IconType.Info, (NotificationType) 0, false, render_overlay, 0, (Func<string, ScaleGrowthMonitor.Instance, string>) null, (Func<string, ScaleGrowthMonitor.Instance, string>) null, category);
    GameStateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.State state6 = this.fullyGrown.ToggleTag(GameTags.Creatures.ScalesGrown).ToggleBehaviour(GameTags.Creatures.ScalesGrown, (StateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.Transition.ConditionCallback) (smi => smi.HasTag(GameTags.Creatures.CanMolt)), (System.Action<ScaleGrowthMonitor.Instance>) null);
    ScaleGrowthMonitor.GrowingState growing3 = this.growing;
    // ISSUE: reference to a compiler-generated field
    if (ScaleGrowthMonitor.\u003C\u003Ef__mg\u0024cache6 == null)
    {
      // ISSUE: reference to a compiler-generated field
      ScaleGrowthMonitor.\u003C\u003Ef__mg\u0024cache6 = new StateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.Transition.ConditionCallback(ScaleGrowthMonitor.AreScalesFullyGrown);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.Transition.ConditionCallback condition2 = GameStateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.Not(ScaleGrowthMonitor.\u003C\u003Ef__mg\u0024cache6);
    state6.Transition((GameStateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.State) growing3, condition2, UpdateRate.SIM_1000ms);
  }

  private static bool IsInCorrectAtmosphere(ScaleGrowthMonitor.Instance smi)
  {
    int cell = Grid.PosToCell((StateMachine.Instance) smi);
    return Grid.Element[cell].id == smi.def.targetAtmosphere;
  }

  private static bool AreScalesFullyGrown(ScaleGrowthMonitor.Instance smi)
  {
    return (double) smi.scaleGrowth.value >= (double) smi.scaleGrowth.GetMax();
  }

  private static void ApplyModifier(ScaleGrowthMonitor.Instance smi)
  {
    smi.scaleGrowth.deltaAttribute.Add(smi.scaleGrowthModifier);
  }

  private static void RemoveModifier(ScaleGrowthMonitor.Instance smi)
  {
    smi.scaleGrowth.deltaAttribute.Remove(smi.scaleGrowthModifier);
  }

  private static void UpdateScales(ScaleGrowthMonitor.Instance smi, float dt)
  {
    int num = (int) ((double) smi.def.levelCount * (double) smi.scaleGrowth.value / 100.0);
    if (smi.currentScaleLevel == num)
      return;
    KBatchedAnimController component = smi.GetComponent<KBatchedAnimController>();
    for (int index = 0; index < ScaleGrowthMonitor.SCALE_SYMBOL_NAMES.Length; ++index)
    {
      bool is_visible = index <= num - 1;
      component.SetSymbolVisiblity((KAnimHashedString) ScaleGrowthMonitor.SCALE_SYMBOL_NAMES[index], is_visible);
    }
    smi.currentScaleLevel = num;
  }

  public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
  {
    public int levelCount;
    public float defaultGrowthRate;
    public SimHashes targetAtmosphere;
    public Tag itemDroppedOnShear;
    public float dropMass;

    public override void Configure(GameObject prefab)
    {
      prefab.GetComponent<Modifiers>().initialAmounts.Add(Db.Get().Amounts.ScaleGrowth.Id);
    }

    public List<Descriptor> GetDescriptors(GameObject obj)
    {
      List<Descriptor> descriptorList = new List<Descriptor>();
      if (this.targetAtmosphere == (SimHashes) 0)
        descriptorList.Add(new Descriptor(UI.BUILDINGEFFECTS.SCALE_GROWTH.Replace("{Item}", this.itemDroppedOnShear.ProperName()).Replace("{Amount}", GameUtil.GetFormattedMass(this.dropMass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")).Replace("{Time}", GameUtil.GetFormattedCycles(1f / this.defaultGrowthRate, "F1")), UI.BUILDINGEFFECTS.TOOLTIPS.SCALE_GROWTH.Replace("{Item}", this.itemDroppedOnShear.ProperName()).Replace("{Amount}", GameUtil.GetFormattedMass(this.dropMass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")).Replace("{Time}", GameUtil.GetFormattedCycles(1f / this.defaultGrowthRate, "F1")), Descriptor.DescriptorType.Effect, false));
      else
        descriptorList.Add(new Descriptor(UI.BUILDINGEFFECTS.SCALE_GROWTH_ATMO.Replace("{Item}", this.itemDroppedOnShear.ProperName()).Replace("{Amount}", GameUtil.GetFormattedMass(this.dropMass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")).Replace("{Time}", GameUtil.GetFormattedCycles(1f / this.defaultGrowthRate, "F1")).Replace("{Atmosphere}", this.targetAtmosphere.CreateTag().ProperName()), UI.BUILDINGEFFECTS.TOOLTIPS.SCALE_GROWTH_ATMO.Replace("{Item}", this.itemDroppedOnShear.ProperName()).Replace("{Amount}", GameUtil.GetFormattedMass(this.dropMass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")).Replace("{Time}", GameUtil.GetFormattedCycles(1f / this.defaultGrowthRate, "F1")).Replace("{Atmosphere}", this.targetAtmosphere.CreateTag().ProperName()), Descriptor.DescriptorType.Effect, false));
      return descriptorList;
    }
  }

  public class GrowingState : GameStateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.State
  {
    public GameStateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.State growing;
    public GameStateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.State stunted;
  }

  public class Instance : GameStateMachine<ScaleGrowthMonitor, ScaleGrowthMonitor.Instance, IStateMachineTarget, ScaleGrowthMonitor.Def>.GameInstance
  {
    public int currentScaleLevel = -1;
    public AmountInstance scaleGrowth;
    public AttributeModifier scaleGrowthModifier;

    public Instance(IStateMachineTarget master, ScaleGrowthMonitor.Def def)
      : base(master, def)
    {
      this.scaleGrowth = Db.Get().Amounts.ScaleGrowth.Lookup(this.gameObject);
      this.scaleGrowth.value = this.scaleGrowth.GetMax();
      this.scaleGrowthModifier = new AttributeModifier(this.scaleGrowth.amount.deltaAttribute.Id, def.defaultGrowthRate * 100f, (string) CREATURES.MODIFIERS.SCALE_GROWTH_RATE.NAME, false, false, true);
    }

    public bool IsFullyGrown()
    {
      return this.currentScaleLevel == this.def.levelCount;
    }

    public void Shear()
    {
      PrimaryElement component1 = this.smi.GetComponent<PrimaryElement>();
      GameObject go = Util.KInstantiate(Assets.GetPrefab(this.def.itemDroppedOnShear), (GameObject) null, (string) null);
      go.transform.SetPosition(Grid.CellToPosCCC(Grid.CellLeft(Grid.PosToCell((StateMachine.Instance) this)), Grid.SceneLayer.Ore));
      PrimaryElement component2 = go.GetComponent<PrimaryElement>();
      component2.Temperature = component1.Temperature;
      component2.Mass = this.def.dropMass;
      component2.AddDisease(component1.DiseaseIdx, component1.DiseaseCount, "Shearing");
      go.SetActive(true);
      Vector2 initial_velocity = new Vector2(UnityEngine.Random.Range(-1f, 1f) * 1f, (float) ((double) UnityEngine.Random.value * 2.0 + 2.0));
      if (GameComps.Fallers.Has((object) go))
        GameComps.Fallers.Remove(go);
      GameComps.Fallers.Add(go, initial_velocity);
      this.scaleGrowth.value = 0.0f;
      ScaleGrowthMonitor.UpdateScales(this, 0.0f);
    }
  }
}
