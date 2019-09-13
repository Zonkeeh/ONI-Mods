// Decompiled with JetBrains decompiler
// Type: DiseaseDropper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class DiseaseDropper : GameStateMachine<DiseaseDropper, DiseaseDropper.Instance, IStateMachineTarget, DiseaseDropper.Def>
{
  public GameStateMachine<DiseaseDropper, DiseaseDropper.Instance, IStateMachineTarget, DiseaseDropper.Def>.State working;
  public GameStateMachine<DiseaseDropper, DiseaseDropper.Instance, IStateMachineTarget, DiseaseDropper.Def>.State stopped;
  public StateMachine<DiseaseDropper, DiseaseDropper.Instance, IStateMachineTarget, DiseaseDropper.Def>.Signal cellChangedSignal;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.stopped;
    this.root.EventHandler(GameHashes.BurstEmitDisease, (StateMachine<DiseaseDropper, DiseaseDropper.Instance, IStateMachineTarget, DiseaseDropper.Def>.State.Callback) (smi => smi.DropSingleEmit()));
    this.working.TagTransition(GameTags.PreventEmittingDisease, this.stopped, false).Update((System.Action<DiseaseDropper.Instance, float>) ((smi, dt) => smi.DropPeriodic(dt)), UpdateRate.SIM_200ms, false);
    this.stopped.TagTransition(GameTags.PreventEmittingDisease, this.working, true);
  }

  public class Def : StateMachine.BaseDef
  {
    public byte diseaseIdx = byte.MaxValue;
    public float emitFrequency = 1f;
    public int singleEmitQuantity;
    public int averageEmitPerSecond;
  }

  public class Instance : GameStateMachine<DiseaseDropper, DiseaseDropper.Instance, IStateMachineTarget, DiseaseDropper.Def>.GameInstance
  {
    private float timeSinceLastDrop;

    public Instance(IStateMachineTarget master, DiseaseDropper.Def def)
      : base(master, def)
    {
    }

    public bool ShouldDropDisease()
    {
      return true;
    }

    public void DropSingleEmit()
    {
      this.DropDisease(this.def.diseaseIdx, this.def.singleEmitQuantity);
    }

    public void DropPeriodic(float dt)
    {
      this.timeSinceLastDrop += dt;
      if (this.def.averageEmitPerSecond <= 0 || (double) this.def.emitFrequency <= 0.0)
        return;
      for (; (double) this.timeSinceLastDrop > (double) this.def.emitFrequency; this.timeSinceLastDrop -= this.def.emitFrequency)
        this.DropDisease(this.def.diseaseIdx, (int) ((double) this.def.averageEmitPerSecond * (double) this.def.emitFrequency));
    }

    public void DropDisease(byte disease_idx, int disease_count)
    {
      if (disease_count <= 0 || disease_idx == byte.MaxValue)
        return;
      int cell = Grid.PosToCell(this.transform.GetPosition());
      if (!Grid.IsValidCell(cell))
        return;
      SimMessages.ModifyDiseaseOnCell(cell, disease_idx, disease_count);
    }

    public bool IsValidDropCell()
    {
      int cell = Grid.PosToCell(this.transform.GetPosition());
      return Grid.IsValidCell(cell) && Grid.IsGas(cell) && (double) Grid.Mass[cell] <= 1.0;
    }
  }
}
