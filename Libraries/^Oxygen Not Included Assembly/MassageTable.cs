// Decompiled with JetBrains decompiler
// Type: MassageTable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using KSerialization;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;

public class MassageTable : RelaxationPoint, IEffectDescriptor, IActivationRangeTarget
{
  private static readonly string[] EffectsRemoved = new string[1]
  {
    "SoreBack"
  };
  private static readonly EventSystem.IntraObjectHandler<MassageTable> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<MassageTable>((System.Action<MassageTable, object>) ((component, data) => component.OnCopySettings(data)));
  private static readonly Chore.Precondition IsStressAboveActivationRange = new Chore.Precondition()
  {
    id = nameof (IsStressAboveActivationRange),
    description = (string) DUPLICANTS.CHORES.PRECONDITIONS.IS_STRESS_ABOVE_ACTIVATION_RANGE,
    fn = (Chore.PreconditionFn) ((ref Chore.Precondition.Context context, object data) =>
    {
      IActivationRangeTarget activationRangeTarget = (IActivationRangeTarget) data;
      return (double) Db.Get().Amounts.Stress.Lookup(context.consumerState.gameObject).value >= (double) activationRangeTarget.ActivateValue;
    })
  };
  [Serialize]
  private float activateValue = 50f;

  public string ActivateTooltip
  {
    get
    {
      return (string) BUILDINGS.PREFABS.MASSAGETABLE.ACTIVATE_TOOLTIP;
    }
  }

  public string DeactivateTooltip
  {
    get
    {
      return (string) BUILDINGS.PREFABS.MASSAGETABLE.DEACTIVATE_TOOLTIP;
    }
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<MassageTable>(-905833192, MassageTable.OnCopySettingsDelegate);
  }

  private void OnCopySettings(object data)
  {
    MassageTable component = ((GameObject) data).GetComponent<MassageTable>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    this.ActivateValue = component.ActivateValue;
    this.DeactivateValue = component.DeactivateValue;
  }

  protected override void OnCompleteWork(Worker worker)
  {
    base.OnCompleteWork(worker);
    Effects component = worker.GetComponent<Effects>();
    for (int index = 0; index < MassageTable.EffectsRemoved.Length; ++index)
    {
      string effect_id = MassageTable.EffectsRemoved[index];
      component.Remove(effect_id);
    }
  }

  public new List<Descriptor> GetDescriptors(BuildingDef def)
  {
    List<Descriptor> descriptorList = new List<Descriptor>();
    Descriptor descriptor1 = new Descriptor();
    descriptor1.SetupDescriptor(string.Format((string) UI.BUILDINGEFFECTS.STRESSREDUCEDPERMINUTE, (object) GameUtil.GetFormattedPercent((float) ((double) this.stressModificationValue / 600.0 * 60.0), GameUtil.TimeSlice.None)), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.STRESSREDUCEDPERMINUTE, (object) GameUtil.GetFormattedPercent((float) ((double) this.stressModificationValue / 600.0 * 60.0), GameUtil.TimeSlice.None)), Descriptor.DescriptorType.Effect);
    descriptorList.Add(descriptor1);
    if (MassageTable.EffectsRemoved.Length > 0)
    {
      Descriptor descriptor2 = new Descriptor();
      descriptor2.SetupDescriptor((string) UI.BUILDINGEFFECTS.REMOVESEFFECTSUBTITLE, (string) UI.BUILDINGEFFECTS.TOOLTIPS.REMOVESEFFECTSUBTITLE, Descriptor.DescriptorType.Effect);
      descriptorList.Add(descriptor2);
      for (int index = 0; index < MassageTable.EffectsRemoved.Length; ++index)
      {
        string str1 = MassageTable.EffectsRemoved[index];
        string str2 = (string) Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + str1.ToUpper() + ".NAME");
        string str3 = (string) Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + str1.ToUpper() + ".CAUSE");
        Descriptor descriptor3 = new Descriptor();
        descriptor3.IncreaseIndent();
        descriptor3.SetupDescriptor("• " + string.Format((string) UI.BUILDINGEFFECTS.REMOVEDEFFECT, (object) str2), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.REMOVEDEFFECT, (object) str3), Descriptor.DescriptorType.Effect);
        descriptorList.Add(descriptor3);
      }
    }
    return descriptorList;
  }

  protected override WorkChore<RelaxationPoint> CreateWorkChore()
  {
    WorkChore<RelaxationPoint> workChore = new WorkChore<RelaxationPoint>(Db.Get().ChoreTypes.StressHeal, (IStateMachineTarget) this, (ChoreProvider) null, true, (System.Action<Chore>) null, (System.Action<Chore>) null, (System.Action<Chore>) null, false, (ScheduleBlockType) null, true, true, (KAnimFile) null, false, true, false, PriorityScreen.PriorityClass.high, 5, false, true);
    workChore.AddPrecondition(MassageTable.IsStressAboveActivationRange, (object) this);
    return workChore;
  }

  public float ActivateValue
  {
    get
    {
      return this.activateValue;
    }
    set
    {
      this.activateValue = value;
    }
  }

  public float DeactivateValue
  {
    get
    {
      return this.stopStressingValue;
    }
    set
    {
      this.stopStressingValue = value;
    }
  }

  public bool UseWholeNumbers
  {
    get
    {
      return true;
    }
  }

  public float MinValue
  {
    get
    {
      return 0.0f;
    }
  }

  public float MaxValue
  {
    get
    {
      return 100f;
    }
  }

  public string ActivationRangeTitleText
  {
    get
    {
      return (string) UI.UISIDESCREENS.ACTIVATION_RANGE_SIDE_SCREEN.NAME;
    }
  }

  public string ActivateSliderLabelText
  {
    get
    {
      return (string) UI.UISIDESCREENS.ACTIVATION_RANGE_SIDE_SCREEN.ACTIVATE;
    }
  }

  public string DeactivateSliderLabelText
  {
    get
    {
      return (string) UI.UISIDESCREENS.ACTIVATION_RANGE_SIDE_SCREEN.DEACTIVATE;
    }
  }
}
