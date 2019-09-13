// Decompiled with JetBrains decompiler
// Type: RotPile
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

public class RotPile : StateMachineComponent<RotPile.StatesInstance>
{
  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi.StartSM();
  }

  protected void ConvertToElement()
  {
    PrimaryElement component = this.smi.master.GetComponent<PrimaryElement>();
    float mass = component.Mass;
    float temperature = component.Temperature;
    if ((double) mass <= 0.0)
    {
      Util.KDestroyGameObject(this.gameObject);
    }
    else
    {
      SimHashes hash = SimHashes.ToxicSand;
      GameObject gameObject = ElementLoader.FindElementByHash(hash).substance.SpawnResource(this.smi.master.transform.GetPosition(), mass, temperature, byte.MaxValue, 0, false, false, false);
      PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, ElementLoader.FindElementByHash(hash).name, gameObject.transform, 1.5f, false);
      Util.KDestroyGameObject(this.smi.gameObject);
    }
  }

  public class StatesInstance : GameStateMachine<RotPile.States, RotPile.StatesInstance, RotPile, object>.GameInstance
  {
    public AttributeModifier baseDecomposeRate;

    public StatesInstance(RotPile master)
      : base(master)
    {
      if (!WorldInventory.Instance.IsReachable(this.smi.master.gameObject.GetComponent<Pickupable>()))
        return;
      string name = (string) MISC.NOTIFICATIONS.FOODROT.NAME;
      HashedString invalid = HashedString.Invalid;
      // ISSUE: reference to a compiler-generated field
      if (RotPile.StatesInstance.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        RotPile.StatesInstance.\u003C\u003Ef__mg\u0024cache0 = new Func<List<Notification>, object, string>(RotPile.StatesInstance.OnRottenTooltip);
      }
      // ISSUE: reference to a compiler-generated field
      Func<List<Notification>, object, string> fMgCache0 = RotPile.StatesInstance.\u003C\u003Ef__mg\u0024cache0;
      this.gameObject.AddOrGet<Notifier>().Add(new Notification(name, NotificationType.BadMinor, invalid, fMgCache0, (object) null, true, 0.0f, (Notification.ClickCallback) null, (object) null, (Transform) null)
      {
        tooltipData = (object) master.gameObject.GetProperName()
      }, string.Empty);
    }

    private static string OnRottenTooltip(List<Notification> notifications, object data)
    {
      string str = "\n";
      foreach (Notification notification in notifications)
      {
        if (notification.tooltipData != null)
          str = str + "\n" + (string) notification.tooltipData;
      }
      return string.Format((string) MISC.NOTIFICATIONS.FOODROT.TOOLTIP, (object) str);
    }
  }

  public class States : GameStateMachine<RotPile.States, RotPile.StatesInstance, RotPile>
  {
    public GameStateMachine<RotPile.States, RotPile.StatesInstance, RotPile, object>.State decomposing;
    public GameStateMachine<RotPile.States, RotPile.StatesInstance, RotPile, object>.State convertDestroy;
    public StateMachine<RotPile.States, RotPile.StatesInstance, RotPile, object>.FloatParameter decompositionAmount;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.decomposing;
      this.serializable = true;
      double num;
      this.decomposing.ParamTransition<float>((StateMachine<RotPile.States, RotPile.StatesInstance, RotPile, object>.Parameter<float>) this.decompositionAmount, this.convertDestroy, (StateMachine<RotPile.States, RotPile.StatesInstance, RotPile, object>.Parameter<float>.Callback) ((smi, p) => (double) p >= 600.0)).Update("Decomposing", (System.Action<RotPile.StatesInstance, float>) ((smi, dt) => num = (double) this.decompositionAmount.Delta(dt, smi)), UpdateRate.SIM_200ms, false);
      this.convertDestroy.Enter((StateMachine<RotPile.States, RotPile.StatesInstance, RotPile, object>.State.Callback) (smi => smi.master.ConvertToElement()));
    }
  }
}
