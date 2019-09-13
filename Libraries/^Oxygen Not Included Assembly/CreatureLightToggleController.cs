// Decompiled with JetBrains decompiler
// Type: CreatureLightToggleController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class CreatureLightToggleController : GameStateMachine<CreatureLightToggleController, CreatureLightToggleController.Instance, IStateMachineTarget, CreatureLightToggleController.Def>
{
  private GameStateMachine<CreatureLightToggleController, CreatureLightToggleController.Instance, IStateMachineTarget, CreatureLightToggleController.Def>.State light_off;
  private GameStateMachine<CreatureLightToggleController, CreatureLightToggleController.Instance, IStateMachineTarget, CreatureLightToggleController.Def>.State turning_off;
  private GameStateMachine<CreatureLightToggleController, CreatureLightToggleController.Instance, IStateMachineTarget, CreatureLightToggleController.Def>.State light_on;
  private GameStateMachine<CreatureLightToggleController, CreatureLightToggleController.Instance, IStateMachineTarget, CreatureLightToggleController.Def>.State turning_on;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.light_on;
    this.serializable = true;
    this.light_off.Enter((StateMachine<CreatureLightToggleController, CreatureLightToggleController.Instance, IStateMachineTarget, CreatureLightToggleController.Def>.State.Callback) (smi => smi.SwitchLight(false))).TagTransition(GameTags.Creatures.Overcrowded, this.turning_on, true);
    this.turning_off.BatchUpdate((UpdateBucketWithUpdater<CreatureLightToggleController.Instance>.BatchUpdateDelegate) ((instances, time_delta) => CreatureLightToggleController.Instance.ModifyBrightness(instances, CreatureLightToggleController.Instance.dim, time_delta)), UpdateRate.SIM_200ms).Transition(this.light_off, (StateMachine<CreatureLightToggleController, CreatureLightToggleController.Instance, IStateMachineTarget, CreatureLightToggleController.Def>.Transition.ConditionCallback) (smi => smi.IsOff()), UpdateRate.SIM_200ms);
    this.light_on.Enter((StateMachine<CreatureLightToggleController, CreatureLightToggleController.Instance, IStateMachineTarget, CreatureLightToggleController.Def>.State.Callback) (smi => smi.SwitchLight(true))).TagTransition(GameTags.Creatures.Overcrowded, this.turning_off, false);
    this.turning_on.Enter((StateMachine<CreatureLightToggleController, CreatureLightToggleController.Instance, IStateMachineTarget, CreatureLightToggleController.Def>.State.Callback) (smi => smi.SwitchLight(true))).BatchUpdate((UpdateBucketWithUpdater<CreatureLightToggleController.Instance>.BatchUpdateDelegate) ((instances, time_delta) => CreatureLightToggleController.Instance.ModifyBrightness(instances, CreatureLightToggleController.Instance.brighten, time_delta)), UpdateRate.SIM_200ms).Transition(this.light_on, (StateMachine<CreatureLightToggleController, CreatureLightToggleController.Instance, IStateMachineTarget, CreatureLightToggleController.Def>.Transition.ConditionCallback) (smi => smi.IsOn()), UpdateRate.SIM_200ms);
  }

  public class Def : StateMachine.BaseDef
  {
  }

  public class Instance : GameStateMachine<CreatureLightToggleController, CreatureLightToggleController.Instance, IStateMachineTarget, CreatureLightToggleController.Def>.GameInstance
  {
    private static WorkItemCollection<CreatureLightToggleController.Instance.ModifyBrightnessTask, object> modify_brightness_job = new WorkItemCollection<CreatureLightToggleController.Instance.ModifyBrightnessTask, object>();
    public static CreatureLightToggleController.Instance.ModifyLuxDelegate dim = (CreatureLightToggleController.Instance.ModifyLuxDelegate) ((instance, time_delta) =>
    {
      float num = (float) instance.originalLux / 25f;
      instance.light.Lux = Mathf.FloorToInt(Mathf.Max(0.0f, (float) instance.light.Lux - num * time_delta));
    });
    public static CreatureLightToggleController.Instance.ModifyLuxDelegate brighten = (CreatureLightToggleController.Instance.ModifyLuxDelegate) ((instance, time_delta) =>
    {
      float num = (float) instance.originalLux / 15f;
      instance.light.Lux = Mathf.CeilToInt(Mathf.Min((float) instance.originalLux, (float) instance.light.Lux + num * time_delta));
    });
    private const float DIM_TIME = 25f;
    private const float GLOW_TIME = 15f;
    private int originalLux;
    private float originalRange;
    private Light2D light;

    public Instance(IStateMachineTarget master, CreatureLightToggleController.Def def)
      : base(master, def)
    {
      this.light = master.GetComponent<Light2D>();
      this.originalLux = this.light.Lux;
      this.originalRange = this.light.Range;
    }

    public void SwitchLight(bool on)
    {
      this.light.enabled = on;
    }

    public static void ModifyBrightness(
      List<UpdateBucketWithUpdater<CreatureLightToggleController.Instance>.Entry> instances,
      CreatureLightToggleController.Instance.ModifyLuxDelegate modify_lux,
      float time_delta)
    {
      CreatureLightToggleController.Instance.modify_brightness_job.Reset((object) null);
      for (int index = 0; index != instances.Count; ++index)
      {
        UpdateBucketWithUpdater<CreatureLightToggleController.Instance>.Entry instance = instances[index];
        instance.lastUpdateTime = 0.0f;
        instances[index] = instance;
        CreatureLightToggleController.Instance data = instance.data;
        modify_lux(data, time_delta);
        data.light.Range = data.originalRange * (float) data.light.Lux / (float) data.originalLux;
        int num = (int) data.light.RefreshShapeAndPosition();
        if (data.light.RefreshShapeAndPosition() != Light2D.RefreshResult.None)
          CreatureLightToggleController.Instance.modify_brightness_job.Add(new CreatureLightToggleController.Instance.ModifyBrightnessTask(data.light.emitter));
      }
      GlobalJobManager.Run((IWorkItemCollection) CreatureLightToggleController.Instance.modify_brightness_job);
      for (int idx = 0; idx != CreatureLightToggleController.Instance.modify_brightness_job.Count; ++idx)
        CreatureLightToggleController.Instance.modify_brightness_job.GetWorkItem(idx).Finish();
    }

    public bool IsOff()
    {
      return this.light.Lux == 0;
    }

    public bool IsOn()
    {
      return this.light.Lux >= this.originalLux;
    }

    private struct ModifyBrightnessTask : IWorkItem<object>
    {
      private LightGridManager.LightGridEmitter emitter;

      public ModifyBrightnessTask(LightGridManager.LightGridEmitter emitter)
      {
        this.emitter = emitter;
        emitter.RemoveFromGrid();
      }

      public void Run(object context)
      {
        this.emitter.UpdateLitCells();
      }

      public void Finish()
      {
        this.emitter.AddToGrid(false);
      }
    }

    public delegate void ModifyLuxDelegate(
      CreatureLightToggleController.Instance instance,
      float time_delta);
  }
}
