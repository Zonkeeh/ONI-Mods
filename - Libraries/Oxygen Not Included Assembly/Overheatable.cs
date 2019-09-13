// Decompiled with JetBrains decompiler
// Type: Overheatable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

[SkipSaveFileSerialization]
public class Overheatable : StateMachineComponent<Overheatable.StatesInstance>, IEffectDescriptor, IGameObjectEffectDescriptor
{
  private bool modifiersInitialized;
  private AttributeInstance overheatTemp;
  private AttributeInstance fatalTemp;
  public float baseOverheatTemp;
  public float baseFatalTemp;

  public void ResetTemperature()
  {
    this.GetComponent<PrimaryElement>().Temperature = 293.15f;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.overheatTemp = this.GetAttributes().Add(Db.Get().BuildingAttributes.OverheatTemperature);
    this.fatalTemp = this.GetAttributes().Add(Db.Get().BuildingAttributes.FatalTemperature);
  }

  private void InitializeModifiers()
  {
    if (this.modifiersInitialized)
      return;
    this.modifiersInitialized = true;
    AttributeModifier modifier1 = new AttributeModifier(this.overheatTemp.Id, this.baseOverheatTemp, (string) UI.TOOLTIPS.BASE_VALUE, false, false, true);
    AttributeModifier modifier2 = new AttributeModifier(this.fatalTemp.Id, this.baseFatalTemp, (string) UI.TOOLTIPS.BASE_VALUE, false, false, true);
    this.GetAttributes().Add(modifier1);
    this.GetAttributes().Add(modifier2);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.InitializeModifiers();
    HandleVector<int>.Handle handle = GameComps.StructureTemperatures.GetHandle(this.gameObject);
    if (handle.IsValid() && GameComps.StructureTemperatures.IsEnabled(handle))
    {
      GameComps.StructureTemperatures.Disable(handle);
      GameComps.StructureTemperatures.Enable(handle);
    }
    this.smi.StartSM();
  }

  public float OverheatTemperature
  {
    get
    {
      this.InitializeModifiers();
      if (this.overheatTemp != null)
        return this.overheatTemp.GetTotalValue();
      return 10000f;
    }
  }

  public Notification CreateOverheatedNotification()
  {
    KSelectable component = this.GetComponent<KSelectable>();
    return new Notification((string) MISC.NOTIFICATIONS.BUILDINGOVERHEATED.NAME, NotificationType.BadMinor, HashedString.Invalid, (Func<List<Notification>, object, string>) ((notificationList, data) => (string) MISC.NOTIFICATIONS.BUILDINGOVERHEATED.TOOLTIP + notificationList.ReduceMessages(false)), (object) ("/t• " + component.GetProperName()), false, 0.0f, (Notification.ClickCallback) null, (object) null, (Transform) null);
  }

  private static string ToolTipResolver(List<Notification> notificationList, object data)
  {
    string empty = string.Empty;
    for (int index = 0; index < notificationList.Count; ++index)
    {
      Notification notification = notificationList[index];
      empty += (string) notification.tooltipData;
      if (index < notificationList.Count - 1)
        empty += "\n";
    }
    return string.Format((string) MISC.NOTIFICATIONS.BUILDINGOVERHEATED.TOOLTIP, (object) empty);
  }

  public List<Descriptor> GetDescriptors(BuildingDef def)
  {
    return this.GetDescriptors(def.BuildingComplete);
  }

  public List<Descriptor> GetDescriptors(GameObject go)
  {
    List<Descriptor> descriptorList = new List<Descriptor>();
    if (this.overheatTemp != null && this.fatalTemp != null)
    {
      string formattedValue1 = this.overheatTemp.GetFormattedValue();
      string formattedValue2 = this.fatalTemp.GetFormattedValue();
      string format = (string) UI.BUILDINGEFFECTS.TOOLTIPS.OVERHEAT_TEMP + "\n\n" + this.overheatTemp.GetAttributeValueTooltip();
      Descriptor descriptor = new Descriptor(string.Format((string) UI.BUILDINGEFFECTS.OVERHEAT_TEMP, (object) formattedValue1, (object) formattedValue2), string.Format(format, (object) formattedValue1, (object) formattedValue2), Descriptor.DescriptorType.Effect, false);
      descriptorList.Add(descriptor);
    }
    else if ((double) this.baseOverheatTemp != 0.0)
    {
      string formattedTemperature1 = GameUtil.GetFormattedTemperature(this.baseOverheatTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false);
      string formattedTemperature2 = GameUtil.GetFormattedTemperature(this.baseFatalTemp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false);
      string overheatTemp = (string) UI.BUILDINGEFFECTS.TOOLTIPS.OVERHEAT_TEMP;
      Descriptor descriptor = new Descriptor(string.Format((string) UI.BUILDINGEFFECTS.OVERHEAT_TEMP, (object) formattedTemperature1, (object) formattedTemperature2), string.Format(overheatTemp, (object) formattedTemperature1, (object) formattedTemperature2), Descriptor.DescriptorType.Effect, false);
      descriptorList.Add(descriptor);
    }
    return descriptorList;
  }

  public class StatesInstance : GameStateMachine<Overheatable.States, Overheatable.StatesInstance, Overheatable, object>.GameInstance
  {
    public float lastOverheatDamageTime;

    public StatesInstance(Overheatable smi)
      : base(smi)
    {
    }

    public void TryDoOverheatDamage()
    {
      if ((double) Time.time - (double) this.lastOverheatDamageTime < 7.5)
        return;
      this.lastOverheatDamageTime += 7.5f;
      this.master.Trigger(-794517298, (object) new BuildingHP.DamageSourceInfo()
      {
        damage = 1,
        source = (string) BUILDINGS.DAMAGESOURCES.BUILDING_OVERHEATED,
        popString = (string) UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.OVERHEAT,
        fullDamageEffectName = "smoke_damage_kanim"
      });
    }
  }

  public class States : GameStateMachine<Overheatable.States, Overheatable.StatesInstance, Overheatable>
  {
    public GameStateMachine<Overheatable.States, Overheatable.StatesInstance, Overheatable, object>.State invulnerable;
    public GameStateMachine<Overheatable.States, Overheatable.StatesInstance, Overheatable, object>.State safeTemperature;
    public GameStateMachine<Overheatable.States, Overheatable.StatesInstance, Overheatable, object>.State overheated;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.safeTemperature;
      this.root.EventTransition(GameHashes.BuildingBroken, this.invulnerable, (StateMachine<Overheatable.States, Overheatable.StatesInstance, Overheatable, object>.Transition.ConditionCallback) null);
      this.invulnerable.EventHandler(GameHashes.BuildingPartiallyRepaired, (StateMachine<Overheatable.States, Overheatable.StatesInstance, Overheatable, object>.State.Callback) (smi => smi.master.ResetTemperature())).EventTransition(GameHashes.BuildingPartiallyRepaired, this.safeTemperature, (StateMachine<Overheatable.States, Overheatable.StatesInstance, Overheatable, object>.Transition.ConditionCallback) null);
      this.safeTemperature.TriggerOnEnter(GameHashes.OptimalTemperatureAchieved, (Func<Overheatable.StatesInstance, object>) null).EventTransition(GameHashes.BuildingOverheated, this.overheated, (StateMachine<Overheatable.States, Overheatable.StatesInstance, Overheatable, object>.Transition.ConditionCallback) null);
      this.overheated.Enter((StateMachine<Overheatable.States, Overheatable.StatesInstance, Overheatable, object>.State.Callback) (smi => Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_OverheatingBuildings, true))).EventTransition(GameHashes.BuildingNoLongerOverheated, this.safeTemperature, (StateMachine<Overheatable.States, Overheatable.StatesInstance, Overheatable, object>.Transition.ConditionCallback) null).ToggleStatusItem(Db.Get().BuildingStatusItems.Overheated, (object) null).ToggleNotification((Func<Overheatable.StatesInstance, Notification>) (smi => smi.master.CreateOverheatedNotification())).TriggerOnEnter(GameHashes.TooHotWarning, (Func<Overheatable.StatesInstance, object>) null).Enter("InitOverheatTime", (StateMachine<Overheatable.States, Overheatable.StatesInstance, Overheatable, object>.State.Callback) (smi => smi.lastOverheatDamageTime = Time.time)).Update("OverheatDamage", (System.Action<Overheatable.StatesInstance, float>) ((smi, dt) => smi.TryDoOverheatDamage()), UpdateRate.SIM_4000ms, false);
    }
  }
}
