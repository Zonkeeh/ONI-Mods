// Decompiled with JetBrains decompiler
// Type: OperationalValve
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;

[SerializationConfig(MemberSerialization.OptIn)]
public class OperationalValve : ValveBase
{
  private static readonly EventSystem.IntraObjectHandler<OperationalValve> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<OperationalValve>((System.Action<OperationalValve, object>) ((component, data) => component.OnOperationalChanged(data)));
  [MyCmpReq]
  private Operational operational;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<OperationalValve>(-592767678, OperationalValve.OnOperationalChangedDelegate);
  }

  protected override void OnSpawn()
  {
    this.OnOperationalChanged((object) this.operational.IsOperational);
    base.OnSpawn();
  }

  protected override void OnCleanUp()
  {
    this.Unsubscribe<OperationalValve>(-592767678, OperationalValve.OnOperationalChangedDelegate, false);
    base.OnCleanUp();
  }

  private void OnOperationalChanged(object data)
  {
    if ((bool) data)
      this.CurrentFlow = this.MaxFlow;
    else
      this.CurrentFlow = 0.0f;
  }

  public override void UpdateAnim()
  {
    float averageRate = Game.Instance.accumulators.GetAverageRate(this.flowAccumulator);
    if (this.operational.IsOperational)
    {
      if ((double) averageRate > 0.0)
        this.controller.Queue((HashedString) "on_flow", KAnim.PlayMode.Loop, 1f, 0.0f);
      else
        this.controller.Queue((HashedString) "on", KAnim.PlayMode.Once, 1f, 0.0f);
    }
    else if ((double) averageRate > 0.0)
      this.controller.Queue((HashedString) "off_flow", KAnim.PlayMode.Loop, 1f, 0.0f);
    else
      this.controller.Queue((HashedString) "off", KAnim.PlayMode.Once, 1f, 0.0f);
  }
}
