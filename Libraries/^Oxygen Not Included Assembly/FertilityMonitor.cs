// Decompiled with JetBrains decompiler
// Type: FertilityMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using Klei.AI;
using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class FertilityMonitor : GameStateMachine<FertilityMonitor, FertilityMonitor.Instance, IStateMachineTarget, FertilityMonitor.Def>
{
  private GameStateMachine<FertilityMonitor, FertilityMonitor.Instance, IStateMachineTarget, FertilityMonitor.Def>.State fertile;
  private GameStateMachine<FertilityMonitor, FertilityMonitor.Instance, IStateMachineTarget, FertilityMonitor.Def>.State infertile;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.fertile;
    this.serializable = true;
    this.root.DefaultState(this.fertile);
    GameStateMachine<FertilityMonitor, FertilityMonitor.Instance, IStateMachineTarget, FertilityMonitor.Def>.State state = this.fertile.ToggleBehaviour(GameTags.Creatures.Fertile, (StateMachine<FertilityMonitor, FertilityMonitor.Instance, IStateMachineTarget, FertilityMonitor.Def>.Transition.ConditionCallback) (smi => smi.IsReadyToLayEgg()), (System.Action<FertilityMonitor.Instance>) null).ToggleEffect((Func<FertilityMonitor.Instance, Effect>) (smi => smi.fertileEffect));
    GameStateMachine<FertilityMonitor, FertilityMonitor.Instance, IStateMachineTarget, FertilityMonitor.Def>.State infertile1 = this.infertile;
    // ISSUE: reference to a compiler-generated field
    if (FertilityMonitor.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      FertilityMonitor.\u003C\u003Ef__mg\u0024cache0 = new StateMachine<FertilityMonitor, FertilityMonitor.Instance, IStateMachineTarget, FertilityMonitor.Def>.Transition.ConditionCallback(FertilityMonitor.IsFertile);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<FertilityMonitor, FertilityMonitor.Instance, IStateMachineTarget, FertilityMonitor.Def>.Transition.ConditionCallback condition = GameStateMachine<FertilityMonitor, FertilityMonitor.Instance, IStateMachineTarget, FertilityMonitor.Def>.Not(FertilityMonitor.\u003C\u003Ef__mg\u0024cache0);
    state.Transition(infertile1, condition, UpdateRate.SIM_1000ms);
    GameStateMachine<FertilityMonitor, FertilityMonitor.Instance, IStateMachineTarget, FertilityMonitor.Def>.State infertile2 = this.infertile;
    GameStateMachine<FertilityMonitor, FertilityMonitor.Instance, IStateMachineTarget, FertilityMonitor.Def>.State fertile = this.fertile;
    // ISSUE: reference to a compiler-generated field
    if (FertilityMonitor.\u003C\u003Ef__mg\u0024cache1 == null)
    {
      // ISSUE: reference to a compiler-generated field
      FertilityMonitor.\u003C\u003Ef__mg\u0024cache1 = new StateMachine<FertilityMonitor, FertilityMonitor.Instance, IStateMachineTarget, FertilityMonitor.Def>.Transition.ConditionCallback(FertilityMonitor.IsFertile);
    }
    // ISSUE: reference to a compiler-generated field
    StateMachine<FertilityMonitor, FertilityMonitor.Instance, IStateMachineTarget, FertilityMonitor.Def>.Transition.ConditionCallback fMgCache1 = FertilityMonitor.\u003C\u003Ef__mg\u0024cache1;
    infertile2.Transition(fertile, fMgCache1, UpdateRate.SIM_1000ms);
  }

  public static bool IsFertile(FertilityMonitor.Instance smi)
  {
    return !smi.HasTag(GameTags.Creatures.Confined) && !smi.HasTag(GameTags.Creatures.Expecting);
  }

  [Serializable]
  public class BreedingChance
  {
    public Tag egg;
    public float weight;
  }

  public class Def : StateMachine.BaseDef
  {
    public Tag eggPrefab;
    public List<FertilityMonitor.BreedingChance> initialBreedingWeights;
    public float baseFertileCycles;

    public override void Configure(GameObject prefab)
    {
      prefab.AddOrGet<Modifiers>().initialAmounts.Add(Db.Get().Amounts.Fertility.Id);
    }
  }

  public class Instance : GameStateMachine<FertilityMonitor, FertilityMonitor.Instance, IStateMachineTarget, FertilityMonitor.Def>.GameInstance
  {
    private static HashedString targetEggSymbol = (HashedString) "snapto_egg";
    public AmountInstance fertility;
    private GameObject egg;
    [Serialize]
    public List<FertilityMonitor.BreedingChance> breedingChances;
    public Effect fertileEffect;

    public Instance(IStateMachineTarget master, FertilityMonitor.Def def)
      : base(master, def)
    {
      this.fertility = Db.Get().Amounts.Fertility.Lookup(this.gameObject);
      if (GenericGameSettings.instance.acceleratedLifecycle)
        this.fertility.deltaAttribute.Add(new AttributeModifier(this.fertility.deltaAttribute.Id, 33.33333f, "Accelerated Lifecycle", false, false, true));
      float num = (float) (100.0 / ((double) def.baseFertileCycles * 600.0));
      this.fertileEffect = new Effect("Fertile", (string) CREATURES.MODIFIERS.BASE_FERTILITY.NAME, (string) CREATURES.MODIFIERS.BASE_FERTILITY.TOOLTIP, 0.0f, false, false, false, (string) null, 0.0f, (string) null);
      this.fertileEffect.Add(new AttributeModifier(Db.Get().Amounts.Fertility.deltaAttribute.Id, num, (string) CREATURES.MODIFIERS.BASE_FERTILITY.NAME, false, false, true));
      this.InitializeBreedingChances();
    }

    [OnDeserialized]
    private void OnDeserialized()
    {
      if (this.breedingChances.Count != 0)
        return;
      this.InitializeBreedingChances();
    }

    private void InitializeBreedingChances()
    {
      this.breedingChances = new List<FertilityMonitor.BreedingChance>();
      if (this.def.initialBreedingWeights == null)
        return;
      foreach (FertilityMonitor.BreedingChance initialBreedingWeight in this.def.initialBreedingWeights)
      {
        this.breedingChances.Add(new FertilityMonitor.BreedingChance()
        {
          egg = initialBreedingWeight.egg,
          weight = initialBreedingWeight.weight
        });
        foreach (FertilityModifier fertilityModifier in Db.Get().FertilityModifiers.GetForTag(initialBreedingWeight.egg))
          fertilityModifier.ApplyFunction(this, initialBreedingWeight.egg);
      }
      this.NormalizeBreedingChances();
    }

    public void ShowEgg()
    {
      if (!((UnityEngine.Object) this.egg != (UnityEngine.Object) null))
        return;
      bool symbolVisible;
      Vector3 vector3 = this.GetComponent<KBatchedAnimController>().GetSymbolTransform(FertilityMonitor.Instance.targetEggSymbol, out symbolVisible).MultiplyPoint3x4(Vector3.zero);
      if (symbolVisible)
      {
        vector3.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
        int cell = Grid.PosToCell(vector3);
        if (Grid.IsValidCell(cell) && !Grid.Solid[cell])
          this.egg.transform.SetPosition(vector3);
      }
      this.egg.SetActive(true);
      Db.Get().Amounts.Wildness.Copy(this.egg, this.gameObject);
      this.egg = (GameObject) null;
    }

    public void LayEgg()
    {
      this.fertility.value = 0.0f;
      Vector3 position = this.smi.transform.GetPosition();
      position.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
      float num1 = UnityEngine.Random.value;
      Tag tag = Tag.Invalid;
      foreach (FertilityMonitor.BreedingChance breedingChance in this.breedingChances)
      {
        num1 -= breedingChance.weight;
        if ((double) num1 <= 0.0)
        {
          tag = breedingChance.egg;
          break;
        }
      }
      if (GenericGameSettings.instance.acceleratedLifecycle)
      {
        float num2 = 0.0f;
        foreach (FertilityMonitor.BreedingChance breedingChance in this.breedingChances)
        {
          if ((double) breedingChance.weight > (double) num2)
          {
            num2 = breedingChance.weight;
            tag = breedingChance.egg;
          }
        }
      }
      Debug.Assert(tag != Tag.Invalid, (object) "Didn't pick an egg to lay. Weights weren't normalized?");
      GameObject prefab = Assets.GetPrefab(tag);
      this.egg = Util.KInstantiate(prefab, position);
      SymbolOverrideController component1 = this.GetComponent<SymbolOverrideController>();
      string str = "egg01";
      CreatureBrain component2 = Assets.GetPrefab(prefab.GetDef<IncubationMonitor.Def>().spawnedCreature).GetComponent<CreatureBrain>();
      if (!string.IsNullOrEmpty(component2.symbolPrefix))
        str = component2.symbolPrefix + "egg01";
      KAnim.Build.Symbol symbol = this.egg.GetComponent<KBatchedAnimController>().AnimFiles[0].GetData().build.GetSymbol((KAnimHashedString) str);
      if (symbol != null)
        component1.AddSymbolOverride(FertilityMonitor.Instance.targetEggSymbol, symbol, 0);
      this.Trigger(1193600993, (object) this.egg);
    }

    public bool IsReadyToLayEgg()
    {
      return (double) this.smi.fertility.value >= (double) this.smi.fertility.GetMax();
    }

    public void AddBreedingChance(Tag type, float addedPercentChance)
    {
      foreach (FertilityMonitor.BreedingChance breedingChance in this.breedingChances)
      {
        if (breedingChance.egg == type)
        {
          float num = Mathf.Min(1f - breedingChance.weight, Mathf.Max(0.0f - breedingChance.weight, addedPercentChance));
          breedingChance.weight += num;
        }
      }
      this.NormalizeBreedingChances();
      this.master.Trigger(1059811075, (object) this.breedingChances);
    }

    private void NormalizeBreedingChances()
    {
      float num = 0.0f;
      foreach (FertilityMonitor.BreedingChance breedingChance in this.breedingChances)
        num += breedingChance.weight;
      foreach (FertilityMonitor.BreedingChance breedingChance in this.breedingChances)
        breedingChance.weight /= num;
    }
  }
}
