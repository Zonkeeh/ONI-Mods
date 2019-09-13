// Decompiled with JetBrains decompiler
// Type: ConduitThresholdSensor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public abstract class ConduitThresholdSensor : ConduitSensor
{
  private static readonly EventSystem.IntraObjectHandler<ConduitThresholdSensor> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<ConduitThresholdSensor>((System.Action<ConduitThresholdSensor, object>) ((component, data) => component.OnCopySettings(data)));
  [SerializeField]
  [Serialize]
  protected bool activateAboveThreshold = true;
  [SerializeField]
  [Serialize]
  protected float threshold;
  [MyCmpAdd]
  private CopyBuildingSettings copyBuildingSettings;

  public abstract float CurrentValue { get; }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<ConduitThresholdSensor>(-905833192, ConduitThresholdSensor.OnCopySettingsDelegate);
  }

  private void OnCopySettings(object data)
  {
    ConduitThresholdSensor component = ((GameObject) data).GetComponent<ConduitThresholdSensor>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    this.Threshold = component.Threshold;
    this.ActivateAboveThreshold = component.ActivateAboveThreshold;
  }

  protected override void ConduitUpdate(float dt)
  {
    if ((double) this.GetContainedMass() <= 0.0)
      return;
    float currentValue = this.CurrentValue;
    if (this.activateAboveThreshold)
    {
      if (((double) currentValue <= (double) this.threshold || this.IsSwitchedOn) && ((double) currentValue > (double) this.threshold || !this.IsSwitchedOn))
        return;
      this.Toggle();
    }
    else
    {
      if (((double) currentValue <= (double) this.threshold || !this.IsSwitchedOn) && ((double) currentValue > (double) this.threshold || this.IsSwitchedOn))
        return;
      this.Toggle();
    }
  }

  private float GetContainedMass()
  {
    return Conduit.GetFlowManager(this.conduitType).GetContents(Grid.PosToCell((KMonoBehaviour) this)).mass;
  }

  public float Threshold
  {
    get
    {
      return this.threshold;
    }
    set
    {
      this.threshold = value;
    }
  }

  public bool ActivateAboveThreshold
  {
    get
    {
      return this.activateAboveThreshold;
    }
    set
    {
      this.activateAboveThreshold = value;
    }
  }
}
