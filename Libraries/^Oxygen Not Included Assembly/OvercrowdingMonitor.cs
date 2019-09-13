// Decompiled with JetBrains decompiler
// Type: OvercrowdingMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System.Collections.Generic;

public class OvercrowdingMonitor : GameStateMachine<OvercrowdingMonitor, OvercrowdingMonitor.Instance, IStateMachineTarget, OvercrowdingMonitor.Def>
{
  public const float OVERCROWDED_FERTILITY_DEBUFF = -1f;
  public static Effect futureOvercrowdedEffect;
  public static Effect overcrowdedEffect;
  public static Effect stuckEffect;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.root;
    GameStateMachine<OvercrowdingMonitor, OvercrowdingMonitor.Instance, IStateMachineTarget, OvercrowdingMonitor.Def>.State root = this.root;
    // ISSUE: reference to a compiler-generated field
    if (OvercrowdingMonitor.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      OvercrowdingMonitor.\u003C\u003Ef__mg\u0024cache0 = new System.Action<OvercrowdingMonitor.Instance, float>(OvercrowdingMonitor.UpdateState);
    }
    // ISSUE: reference to a compiler-generated field
    System.Action<OvercrowdingMonitor.Instance, float> fMgCache0 = OvercrowdingMonitor.\u003C\u003Ef__mg\u0024cache0;
    root.Update(fMgCache0, UpdateRate.SIM_1000ms, true);
    OvercrowdingMonitor.futureOvercrowdedEffect = new Effect("FutureOvercrowded", (string) CREATURES.MODIFIERS.FUTURE_OVERCROWDED.NAME, (string) CREATURES.MODIFIERS.FUTURE_OVERCROWDED.TOOLTIP, 0.0f, true, false, true, (string) null, 0.0f, (string) null);
    OvercrowdingMonitor.futureOvercrowdedEffect.Add(new AttributeModifier(Db.Get().Amounts.Fertility.deltaAttribute.Id, -1f, (string) CREATURES.MODIFIERS.FUTURE_OVERCROWDED.NAME, true, false, true));
    OvercrowdingMonitor.overcrowdedEffect = new Effect("Overcrowded", (string) CREATURES.MODIFIERS.OVERCROWDED.NAME, (string) CREATURES.MODIFIERS.OVERCROWDED.TOOLTIP, 0.0f, true, false, true, (string) null, 0.0f, (string) null);
    OvercrowdingMonitor.overcrowdedEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, -5f, (string) CREATURES.MODIFIERS.OVERCROWDED.NAME, false, false, true));
    OvercrowdingMonitor.stuckEffect = new Effect("Confined", (string) CREATURES.MODIFIERS.CONFINED.NAME, (string) CREATURES.MODIFIERS.CONFINED.TOOLTIP, 0.0f, true, false, true, (string) null, 0.0f, (string) null);
    OvercrowdingMonitor.stuckEffect.Add(new AttributeModifier(Db.Get().CritterAttributes.Happiness.Id, -10f, (string) CREATURES.MODIFIERS.CONFINED.NAME, false, false, true));
  }

  private static bool IsConfined(OvercrowdingMonitor.Instance smi)
  {
    return !smi.HasTag(GameTags.Creatures.Burrowed) && !smi.HasTag(GameTags.Creatures.Digger) && (smi.cavity == null || smi.cavity.numCells < smi.def.spaceRequiredPerCreature);
  }

  private static bool IsFutureOvercrowded(OvercrowdingMonitor.Instance smi)
  {
    if (smi.def.spaceRequiredPerCreature == 0 || smi.cavity == null)
      return false;
    int num = smi.cavity.creatures.Count + smi.cavity.eggs.Count;
    if (num == 0 || smi.cavity.eggs.Count == 0)
      return false;
    return smi.cavity.numCells / num < smi.def.spaceRequiredPerCreature;
  }

  private static bool IsOvercrowded(OvercrowdingMonitor.Instance smi)
  {
    if (smi.def.spaceRequiredPerCreature == 0)
      return false;
    FishOvercrowdingMonitor.Instance smi1 = smi.GetSMI<FishOvercrowdingMonitor.Instance>();
    if (smi1 != null)
    {
      int fishCount = smi1.fishCount;
      if (fishCount > 0)
        return smi1.cellCount / fishCount < smi.def.spaceRequiredPerCreature;
      return false;
    }
    if (smi.cavity != null && smi.cavity.creatures.Count > 1)
      return smi.cavity.numCells / smi.cavity.creatures.Count < smi.def.spaceRequiredPerCreature;
    return false;
  }

  private static void UpdateState(OvercrowdingMonitor.Instance smi, float dt)
  {
    OvercrowdingMonitor.UpdateCavity(smi, dt);
    bool set1 = OvercrowdingMonitor.IsConfined(smi);
    bool set2 = OvercrowdingMonitor.IsOvercrowded(smi);
    bool set3 = !smi.isBaby && OvercrowdingMonitor.IsFutureOvercrowded(smi);
    KPrefabID component = smi.gameObject.GetComponent<KPrefabID>();
    component.SetTag(GameTags.Creatures.Confined, set1);
    component.SetTag(GameTags.Creatures.Overcrowded, set2);
    component.SetTag(GameTags.Creatures.Expecting, set3);
    OvercrowdingMonitor.SetEffect(smi, OvercrowdingMonitor.stuckEffect, set1);
    OvercrowdingMonitor.SetEffect(smi, OvercrowdingMonitor.overcrowdedEffect, !set1 && set2);
    OvercrowdingMonitor.SetEffect(smi, OvercrowdingMonitor.futureOvercrowdedEffect, !set1 && set3);
  }

  private static void SetEffect(OvercrowdingMonitor.Instance smi, Effect effect, bool set)
  {
    Effects component = smi.GetComponent<Effects>();
    if (set)
      component.Add(effect, false);
    else
      component.Remove(effect);
  }

  private static List<KPrefabID> GetCreatureCollection(
    OvercrowdingMonitor.Instance smi,
    CavityInfo cavity_info)
  {
    if (smi.HasTag(GameTags.Egg))
      return cavity_info.eggs;
    return cavity_info.creatures;
  }

  private static void UpdateCavity(OvercrowdingMonitor.Instance smi, float dt)
  {
    CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(Grid.PosToCell((StateMachine.Instance) smi));
    if (cavityForCell == smi.cavity)
      return;
    KPrefabID component = smi.GetComponent<KPrefabID>();
    if (smi.cavity != null)
    {
      OvercrowdingMonitor.GetCreatureCollection(smi, smi.cavity).Remove(component);
      Game.Instance.roomProber.UpdateRoom(cavityForCell);
    }
    smi.cavity = cavityForCell;
    if (smi.cavity == null)
      return;
    OvercrowdingMonitor.GetCreatureCollection(smi, smi.cavity).Add(component);
    Game.Instance.roomProber.UpdateRoom(smi.cavity);
  }

  public class Def : StateMachine.BaseDef
  {
    public int spaceRequiredPerCreature;
  }

  public class Instance : GameStateMachine<OvercrowdingMonitor, OvercrowdingMonitor.Instance, IStateMachineTarget, OvercrowdingMonitor.Def>.GameInstance
  {
    public CavityInfo cavity;
    public bool isBaby;

    public Instance(IStateMachineTarget master, OvercrowdingMonitor.Def def)
      : base(master, def)
    {
      this.isBaby = master.gameObject.GetDef<BabyMonitor.Def>() != null;
    }

    protected override void OnCleanUp()
    {
      KPrefabID component = this.master.GetComponent<KPrefabID>();
      if (this.cavity == null)
        return;
      OvercrowdingMonitor.GetCreatureCollection(this, this.cavity).Remove(component);
    }
  }
}
