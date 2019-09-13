// Decompiled with JetBrains decompiler
// Type: CreatureBait
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;

[SerializationConfig(MemberSerialization.OptIn)]
public class CreatureBait : StateMachineComponent<CreatureBait.StatesInstance>
{
  [Serialize]
  public Tag baitElement;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.baitElement = this.GetComponent<Deconstructable>().constructionElements[1];
    this.gameObject.GetSMI<Lure.Instance>().SetActiveLures(new Tag[1]
    {
      this.baitElement
    });
    this.smi.StartSM();
  }

  public class StatesInstance : GameStateMachine<CreatureBait.States, CreatureBait.StatesInstance, CreatureBait, object>.GameInstance
  {
    public StatesInstance(CreatureBait master)
      : base(master)
    {
    }
  }

  public class States : GameStateMachine<CreatureBait.States, CreatureBait.StatesInstance, CreatureBait>
  {
    public GameStateMachine<CreatureBait.States, CreatureBait.StatesInstance, CreatureBait, object>.State idle;
    public GameStateMachine<CreatureBait.States, CreatureBait.StatesInstance, CreatureBait, object>.State destroy;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.idle;
      this.idle.ToggleMainStatusItem(Db.Get().BuildingStatusItems.Baited).Enter((StateMachine<CreatureBait.States, CreatureBait.StatesInstance, CreatureBait, object>.State.Callback) (smi =>
      {
        KAnim.Build build = ElementLoader.FindElementByName(smi.master.baitElement.ToString()).substance.anim.GetData().build;
        KAnim.Build.Symbol symbol = build.GetSymbol(new KAnimHashedString(build.name));
        HashedString target_symbol = (HashedString) "snapTo_bait";
        smi.GetComponent<SymbolOverrideController>().AddSymbolOverride(target_symbol, symbol, 0);
      })).TagTransition(GameTags.LureUsed, this.destroy, false);
      this.destroy.PlayAnim("use").EventHandler(GameHashes.AnimQueueComplete, (StateMachine<CreatureBait.States, CreatureBait.StatesInstance, CreatureBait, object>.State.Callback) (smi => Util.KDestroyGameObject(smi.master.gameObject)));
    }
  }
}
