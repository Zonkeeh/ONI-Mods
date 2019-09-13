// Decompiled with JetBrains decompiler
// Type: Fashionable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

[SkipSaveFileSerialization]
public class Fashionable : StateMachineComponent<Fashionable.StatesInstance>
{
  protected override void OnSpawn()
  {
    this.smi.StartSM();
  }

  protected bool IsUncomfortable()
  {
    ClothingWearer component = this.GetComponent<ClothingWearer>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      return component.currentClothing.decorMod <= 0;
    return false;
  }

  public class StatesInstance : GameStateMachine<Fashionable.States, Fashionable.StatesInstance, Fashionable, object>.GameInstance
  {
    public StatesInstance(Fashionable master)
      : base(master)
    {
    }
  }

  public class States : GameStateMachine<Fashionable.States, Fashionable.StatesInstance, Fashionable>
  {
    public GameStateMachine<Fashionable.States, Fashionable.StatesInstance, Fashionable, object>.State satisfied;
    public GameStateMachine<Fashionable.States, Fashionable.StatesInstance, Fashionable, object>.State suffering;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.satisfied;
      this.root.EventHandler(GameHashes.EquippedItemEquipper, (StateMachine<Fashionable.States, Fashionable.StatesInstance, Fashionable, object>.State.Callback) (smi =>
      {
        if (smi.master.IsUncomfortable())
          smi.GoTo((StateMachine.BaseState) this.suffering);
        else
          smi.GoTo((StateMachine.BaseState) this.satisfied);
      })).EventHandler(GameHashes.UnequippedItemEquipper, (StateMachine<Fashionable.States, Fashionable.StatesInstance, Fashionable, object>.State.Callback) (smi =>
      {
        if (smi.master.IsUncomfortable())
          smi.GoTo((StateMachine.BaseState) this.suffering);
        else
          smi.GoTo((StateMachine.BaseState) this.satisfied);
      }));
      this.suffering.AddEffect("UnfashionableClothing").ToggleExpression(Db.Get().Expressions.Uncomfortable, (Func<Fashionable.StatesInstance, bool>) null);
      this.satisfied.DoNothing();
    }
  }
}
