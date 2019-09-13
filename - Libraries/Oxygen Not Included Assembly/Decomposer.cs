// Decompiled with JetBrains decompiler
// Type: Decomposer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class Decomposer : KMonoBehaviour
{
  protected override void OnSpawn()
  {
    base.OnSpawn();
    StateMachineController component = this.GetComponent<StateMachineController>();
    if ((Object) component == (Object) null)
      return;
    DecompositionMonitor.Instance instance = new DecompositionMonitor.Instance((IStateMachineTarget) this, (Klei.AI.Disease) null, 1f, false);
    component.AddStateMachineInstance((StateMachine.Instance) instance);
    instance.StartSM();
    instance.dirtyWaterMaxRange = 3;
  }
}
