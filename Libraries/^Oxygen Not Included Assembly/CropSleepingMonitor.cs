// Decompiled with JetBrains decompiler
// Type: CropSleepingMonitor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class CropSleepingMonitor : GameStateMachine<CropSleepingMonitor, CropSleepingMonitor.Instance, IStateMachineTarget, CropSleepingMonitor.Def>
{
  public GameStateMachine<CropSleepingMonitor, CropSleepingMonitor.Instance, IStateMachineTarget, CropSleepingMonitor.Def>.State sleeping;
  public GameStateMachine<CropSleepingMonitor, CropSleepingMonitor.Instance, IStateMachineTarget, CropSleepingMonitor.Def>.State awake;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.awake;
    this.serializable = false;
    this.root.Update("CropSleepingMonitor.root", (System.Action<CropSleepingMonitor.Instance, float>) ((smi, dt) =>
    {
      int cell = Grid.PosToCell(smi.master.gameObject);
      GameStateMachine<CropSleepingMonitor, CropSleepingMonitor.Instance, IStateMachineTarget, CropSleepingMonitor.Def>.State state = !smi.IsCellSafe(cell) ? this.sleeping : this.awake;
      smi.GoTo((StateMachine.BaseState) state);
    }), UpdateRate.SIM_1000ms, false);
    this.sleeping.TriggerOnEnter(GameHashes.CropSleep, (Func<CropSleepingMonitor.Instance, object>) null).ToggleStatusItem(Db.Get().CreatureStatusItems.CropSleeping, (Func<CropSleepingMonitor.Instance, object>) (smi => (object) smi));
    this.awake.TriggerOnEnter(GameHashes.CropWakeUp, (Func<CropSleepingMonitor.Instance, object>) null);
  }

  public class Def : StateMachine.BaseDef, IGameObjectEffectDescriptor
  {
    public float lightIntensityThreshold;
    public bool prefersDarkness;

    public List<Descriptor> GetDescriptors(GameObject obj)
    {
      return (List<Descriptor>) null;
    }
  }

  public class Instance : GameStateMachine<CropSleepingMonitor, CropSleepingMonitor.Instance, IStateMachineTarget, CropSleepingMonitor.Def>.GameInstance
  {
    public Instance(IStateMachineTarget master, CropSleepingMonitor.Def def)
      : base(master, def)
    {
    }

    public bool IsSleeping()
    {
      return this.GetCurrentState() == this.smi.sm.sleeping;
    }

    public bool IsCellSafe(int cell)
    {
      float num = (float) Grid.LightIntensity[cell];
      if (this.def.prefersDarkness)
        return (double) num <= (double) this.def.lightIntensityThreshold;
      return (double) num >= (double) this.def.lightIntensityThreshold;
    }
  }
}
