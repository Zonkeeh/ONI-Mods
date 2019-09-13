// Decompiled with JetBrains decompiler
// Type: GasBreatherFromWorldProvider
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class GasBreatherFromWorldProvider : OxygenBreather.IGasProvider
{
  private SuffocationMonitor.Instance suffocationMonitor;
  private SafeCellMonitor.Instance safeCellMonitor;
  private OxygenBreather oxygenBreather;

  public void OnSetOxygenBreather(OxygenBreather oxygen_breather)
  {
    this.suffocationMonitor = new SuffocationMonitor.Instance(oxygen_breather);
    this.suffocationMonitor.StartSM();
    this.safeCellMonitor = new SafeCellMonitor.Instance((IStateMachineTarget) oxygen_breather);
    this.safeCellMonitor.StartSM();
    this.oxygenBreather = oxygen_breather;
  }

  public void OnClearOxygenBreather(OxygenBreather oxygen_breather)
  {
    this.suffocationMonitor.StopSM("Removed gas provider");
    this.safeCellMonitor.StopSM("Removed gas provider");
  }

  public bool ShouldEmitCO2()
  {
    return true;
  }

  public bool ShouldStoreCO2()
  {
    return false;
  }

  public bool ConsumeGas(OxygenBreather oxygen_breather, float gas_consumed)
  {
    SimHashes breathableElement = oxygen_breather.GetBreathableElement;
    if (breathableElement == SimHashes.Vacuum)
      return false;
    Game.ComplexCallbackHandleVector<Sim.MassConsumedCallback> consumedCallbackManager = Game.Instance.massConsumedCallbackManager;
    // ISSUE: reference to a compiler-generated field
    if (GasBreatherFromWorldProvider.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      GasBreatherFromWorldProvider.\u003C\u003Ef__mg\u0024cache0 = new System.Action<Sim.MassConsumedCallback, object>(GasBreatherFromWorldProvider.OnSimConsumeCallback);
    }
    // ISSUE: reference to a compiler-generated field
    System.Action<Sim.MassConsumedCallback, object> fMgCache0 = GasBreatherFromWorldProvider.\u003C\u003Ef__mg\u0024cache0;
    HandleVector<Game.ComplexCallbackInfo<Sim.MassConsumedCallback>>.Handle handle = consumedCallbackManager.Add(fMgCache0, (object) this, nameof (GasBreatherFromWorldProvider));
    SimMessages.ConsumeMass(oxygen_breather.mouthCell, breathableElement, gas_consumed, (byte) 3, handle.index);
    return true;
  }

  private static void OnSimConsumeCallback(Sim.MassConsumedCallback mass_cb_info, object data)
  {
    ((GasBreatherFromWorldProvider) data).OnSimConsume(mass_cb_info);
  }

  private void OnSimConsume(Sim.MassConsumedCallback mass_cb_info)
  {
    if ((UnityEngine.Object) this.oxygenBreather == (UnityEngine.Object) null || this.oxygenBreather.GetComponent<KPrefabID>().HasTag(GameTags.Dead))
      return;
    Game.Instance.accumulators.Accumulate(this.oxygenBreather.O2Accumulator, mass_cb_info.mass);
    ReportManager.Instance.ReportValue(ReportManager.ReportType.OxygenCreated, -mass_cb_info.mass, this.oxygenBreather.GetProperName(), (string) null);
    this.oxygenBreather.Consume(mass_cb_info);
  }
}
