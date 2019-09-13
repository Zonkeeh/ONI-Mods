// Decompiled with JetBrains decompiler
// Type: Klei.AI.SicknessInstance
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Klei.AI
{
  [SerializationConfig(MemberSerialization.OptIn)]
  public class SicknessInstance : ModifierInstance<Sickness>, ISaveLoadable
  {
    [Serialize]
    private SicknessExposureInfo exposureInfo;
    private SicknessInstance.StatesInstance smi;
    private StatusItem statusItem;
    private Notification notification;

    public SicknessInstance(GameObject game_object, Sickness disease)
      : base(game_object, disease)
    {
    }

    public Sickness Sickness
    {
      get
      {
        return this.modifier;
      }
    }

    public float TotalCureSpeedMultiplier
    {
      get
      {
        AttributeInstance attributeInstance1 = Db.Get().Attributes.DiseaseCureSpeed.Lookup(this.smi.master.gameObject);
        AttributeInstance attributeInstance2 = this.modifier.cureSpeedBase.Lookup(this.smi.master.gameObject);
        float num = 1f;
        if (attributeInstance1 != null)
          num *= attributeInstance1.GetTotalValue();
        if (attributeInstance2 != null)
          num *= attributeInstance2.GetTotalValue();
        return num;
      }
    }

    public bool IsDoctored
    {
      get
      {
        if ((UnityEngine.Object) this.gameObject == (UnityEngine.Object) null)
          return false;
        AttributeInstance attributeInstance = Db.Get().Attributes.DoctoredLevel.Lookup(this.gameObject);
        return attributeInstance != null && (double) attributeInstance.GetTotalValue() > 0.0;
      }
    }

    [OnDeserialized]
    private void OnDeserialized()
    {
      this.InitializeAndStart();
    }

    public SicknessExposureInfo ExposureInfo
    {
      get
      {
        return this.exposureInfo;
      }
      set
      {
        this.exposureInfo = value;
        this.InitializeAndStart();
      }
    }

    private void InitializeAndStart()
    {
      Sickness disease = this.modifier;
      Func<List<Notification>, object, string> tooltip = (Func<List<Notification>, object, string>) ((notificationList, data) =>
      {
        string empty = string.Empty;
        for (int index = 0; index < notificationList.Count; ++index)
        {
          Notification notification = notificationList[index];
          string tooltipData = (string) notification.tooltipData;
          empty += string.Format((string) DUPLICANTS.DISEASES.NOTIFICATION_TOOLTIP, (object) notification.NotifierName, (object) disease.Name, (object) tooltipData);
          if (index < notificationList.Count - 1)
            empty += "\n";
        }
        return empty;
      });
      string name = disease.Name;
      NotificationType type = disease.severity > Sickness.Severity.Minor ? NotificationType.Bad : NotificationType.BadMinor;
      HashedString invalid = HashedString.Invalid;
      string sourceInfo = this.exposureInfo.sourceInfo;
      this.notification = new Notification(name, type, invalid, tooltip, (object) sourceInfo, true, 0.0f, (Notification.ClickCallback) null, (object) null, (Transform) null);
      this.statusItem = new StatusItem(disease.Id, disease.Name, (string) DUPLICANTS.DISEASES.STATUS_ITEM_TOOLTIP.TEMPLATE, string.Empty, disease.severity > Sickness.Severity.Minor ? StatusItem.IconType.Exclamation : StatusItem.IconType.Info, disease.severity > Sickness.Severity.Minor ? NotificationType.Bad : NotificationType.BadMinor, false, OverlayModes.None.ID, 129022);
      this.statusItem.resolveTooltipCallback = new Func<string, object, string>(this.ResolveString);
      if (this.smi != null)
        this.smi.StopSM("refresh");
      this.smi = new SicknessInstance.StatesInstance(this);
      this.smi.StartSM();
    }

    private string ResolveString(string str, object data)
    {
      if (this.smi == null)
      {
        Debug.LogWarning((object) "Attempting to resolve string when smi is null");
        return str;
      }
      KSelectable component = this.gameObject.GetComponent<KSelectable>();
      str = str.Replace("{Descriptor}", string.Format((string) DUPLICANTS.DISEASES.STATUS_ITEM_TOOLTIP.DESCRIPTOR, (object) Strings.Get("STRINGS.DUPLICANTS.DISEASES.SEVERITY." + this.modifier.severity.ToString().ToUpper()), (object) Strings.Get("STRINGS.DUPLICANTS.DISEASES.TYPE." + this.modifier.sicknessType.ToString().ToUpper())));
      str = str.Replace("{Infectee}", component.GetProperName());
      str = str.Replace("{InfectionSource}", string.Format((string) DUPLICANTS.DISEASES.STATUS_ITEM_TOOLTIP.INFECTION_SOURCE, (object) this.exposureInfo.sourceInfo));
      if (this.modifier.severity <= Sickness.Severity.Minor)
        str = str.Replace("{Duration}", string.Format((string) DUPLICANTS.DISEASES.STATUS_ITEM_TOOLTIP.DURATION, (object) GameUtil.GetFormattedCycles(this.GetInfectedTimeRemaining(), "F1")));
      else if (this.modifier.severity == Sickness.Severity.Major)
      {
        str = str.Replace("{Duration}", string.Format((string) DUPLICANTS.DISEASES.STATUS_ITEM_TOOLTIP.DURATION, (object) GameUtil.GetFormattedCycles(this.GetInfectedTimeRemaining(), "F1")));
        str = this.IsDoctored ? str.Replace("{Doctor}", (string) DUPLICANTS.DISEASES.STATUS_ITEM_TOOLTIP.DOCTORED) : str.Replace("{Doctor}", (string) DUPLICANTS.DISEASES.STATUS_ITEM_TOOLTIP.BEDREST);
      }
      else if (this.modifier.severity >= Sickness.Severity.Critical)
      {
        if (!this.IsDoctored)
        {
          str = str.Replace("{Duration}", string.Format((string) DUPLICANTS.DISEASES.STATUS_ITEM_TOOLTIP.FATALITY, (object) GameUtil.GetFormattedCycles(this.GetFatalityTimeRemaining(), "F1")));
          str = str.Replace("{Doctor}", (string) DUPLICANTS.DISEASES.STATUS_ITEM_TOOLTIP.DOCTOR_REQUIRED);
        }
        else
        {
          str = str.Replace("{Duration}", string.Format((string) DUPLICANTS.DISEASES.STATUS_ITEM_TOOLTIP.DURATION, (object) GameUtil.GetFormattedCycles(this.GetInfectedTimeRemaining(), "F1")));
          str = str.Replace("{Doctor}", (string) DUPLICANTS.DISEASES.STATUS_ITEM_TOOLTIP.DOCTORED);
        }
      }
      List<Descriptor> symptoms = this.modifier.GetSymptoms();
      string newValue = string.Empty;
      foreach (Descriptor descriptor in symptoms)
      {
        if (!string.IsNullOrEmpty(newValue))
          newValue += "\n";
        newValue = newValue + "    • " + descriptor.text;
      }
      str = str.Replace("{Symptoms}", newValue);
      str = Regex.Replace(str, "{[^}]*}", string.Empty);
      return str;
    }

    public float GetInfectedTimeRemaining()
    {
      return this.modifier.SicknessDuration * (1f - this.smi.sm.percentRecovered.Get(this.smi)) / this.TotalCureSpeedMultiplier;
    }

    public float GetFatalityTimeRemaining()
    {
      return this.modifier.fatalityDuration * (1f - this.smi.sm.percentDied.Get(this.smi));
    }

    public float GetPercentCured()
    {
      if (this.smi != null)
        return this.smi.sm.percentRecovered.Get(this.smi);
      return 0.0f;
    }

    public void SetPercentCured(float pct)
    {
      double num = (double) this.smi.sm.percentRecovered.Set(pct, this.smi);
    }

    public void Cure()
    {
      this.smi.Cure();
    }

    public override void OnCleanUp()
    {
      if (this.smi == null)
        return;
      this.smi.StopSM("DiseaseInstance.OnCleanUp");
      this.smi = (SicknessInstance.StatesInstance) null;
    }

    public StatusItem GetStatusItem()
    {
      return this.statusItem;
    }

    public List<Descriptor> GetDescriptors()
    {
      return this.modifier.GetSicknessSourceDescriptors();
    }

    private struct CureInfo
    {
      public string name;
      public float multiplier;
    }

    public class StatesInstance : GameStateMachine<SicknessInstance.States, SicknessInstance.StatesInstance, SicknessInstance, object>.GameInstance
    {
      private object[] componentData;

      public StatesInstance(SicknessInstance master)
        : base(master)
      {
      }

      public void UpdateProgress(float dt)
      {
        double num1 = (double) this.sm.percentRecovered.Delta(dt * this.master.TotalCureSpeedMultiplier / this.master.modifier.SicknessDuration, this.smi);
        if ((double) this.master.modifier.fatalityDuration <= 0.0)
          return;
        if (!this.master.IsDoctored)
        {
          double num2 = (double) this.sm.percentDied.Delta(dt / this.master.modifier.fatalityDuration, this.smi);
        }
        else
        {
          double num3 = (double) this.sm.percentDied.Set(0.0f, this.smi);
        }
      }

      public void Infect()
      {
        Sickness modifier = this.master.modifier;
        this.componentData = modifier.Infect(this.gameObject, this.master, this.master.exposureInfo);
        if (!((UnityEngine.Object) PopFXManager.Instance != (UnityEngine.Object) null))
          return;
        PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, string.Format((string) DUPLICANTS.DISEASES.INFECTED_POPUP, (object) modifier.Name), this.gameObject.transform, 1.5f, true);
      }

      public void Cure()
      {
        Sickness modifier = this.master.modifier;
        this.gameObject.GetComponent<Modifiers>().sicknesses.Cure(modifier);
        modifier.Cure(this.gameObject, this.componentData);
        if ((UnityEngine.Object) PopFXManager.Instance != (UnityEngine.Object) null)
          PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, string.Format((string) DUPLICANTS.DISEASES.CURED_POPUP, (object) modifier.Name), this.gameObject.transform, 1.5f, true);
        if (string.IsNullOrEmpty(modifier.recoveryEffect))
          return;
        Effects component = this.gameObject.GetComponent<Effects>();
        if (!(bool) ((UnityEngine.Object) component))
          return;
        component.Add(modifier.recoveryEffect, true);
      }

      public SicknessExposureInfo GetExposureInfo()
      {
        return this.master.ExposureInfo;
      }
    }

    public class States : GameStateMachine<SicknessInstance.States, SicknessInstance.StatesInstance, SicknessInstance>
    {
      public StateMachine<SicknessInstance.States, SicknessInstance.StatesInstance, SicknessInstance, object>.FloatParameter percentRecovered;
      public StateMachine<SicknessInstance.States, SicknessInstance.StatesInstance, SicknessInstance, object>.FloatParameter percentDied;
      public GameStateMachine<SicknessInstance.States, SicknessInstance.StatesInstance, SicknessInstance, object>.State infected;
      public GameStateMachine<SicknessInstance.States, SicknessInstance.StatesInstance, SicknessInstance, object>.State cured;
      public GameStateMachine<SicknessInstance.States, SicknessInstance.StatesInstance, SicknessInstance, object>.State fatality_pre;
      public GameStateMachine<SicknessInstance.States, SicknessInstance.StatesInstance, SicknessInstance, object>.State fatality;

      public override void InitializeStates(out StateMachine.BaseState default_state)
      {
        default_state = (StateMachine.BaseState) this.infected;
        this.serializable = true;
        this.infected.Enter("Infect", (StateMachine<SicknessInstance.States, SicknessInstance.StatesInstance, SicknessInstance, object>.State.Callback) (smi => smi.Infect())).DoNotification((Func<SicknessInstance.StatesInstance, Notification>) (smi => smi.master.notification)).Update("UpdateProgress", (System.Action<SicknessInstance.StatesInstance, float>) ((smi, dt) => smi.UpdateProgress(dt)), UpdateRate.SIM_200ms, false).ToggleStatusItem((Func<SicknessInstance.StatesInstance, StatusItem>) (smi => smi.master.GetStatusItem()), (Func<SicknessInstance.StatesInstance, object>) (smi => (object) smi)).ParamTransition<float>((StateMachine<SicknessInstance.States, SicknessInstance.StatesInstance, SicknessInstance, object>.Parameter<float>) this.percentRecovered, this.cured, GameStateMachine<SicknessInstance.States, SicknessInstance.StatesInstance, SicknessInstance, object>.IsGTOne).ParamTransition<float>((StateMachine<SicknessInstance.States, SicknessInstance.StatesInstance, SicknessInstance, object>.Parameter<float>) this.percentDied, this.fatality_pre, GameStateMachine<SicknessInstance.States, SicknessInstance.StatesInstance, SicknessInstance, object>.IsGTOne);
        this.cured.Enter("Cure", (StateMachine<SicknessInstance.States, SicknessInstance.StatesInstance, SicknessInstance, object>.State.Callback) (smi => smi.master.Cure()));
        this.fatality_pre.Update("DeathByDisease", (System.Action<SicknessInstance.StatesInstance, float>) ((smi, dt) =>
        {
          DeathMonitor.Instance smi1 = smi.master.gameObject.GetSMI<DeathMonitor.Instance>();
          if (smi1 == null)
            return;
          smi1.Kill(Db.Get().Deaths.FatalDisease);
          smi.GoTo((StateMachine.BaseState) this.fatality);
        }), UpdateRate.SIM_200ms, false);
        this.fatality.DoNothing();
      }
    }
  }
}
