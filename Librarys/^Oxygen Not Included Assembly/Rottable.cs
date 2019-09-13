// Decompiled with JetBrains decompiler
// Type: Rottable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Rottable : GameStateMachine<Rottable, Rottable.Instance, IStateMachineTarget, Rottable.Def>
{
  private static readonly Tag[] PRESERVED_TAGS = new Tag[2]
  {
    GameTags.Preserved,
    GameTags.Entombed
  };
  private static readonly Rottable.RotCB rotCB = new Rottable.RotCB();
  public static Dictionary<int, Rottable.RotAtmosphereQuality> AtmosphereModifier = new Dictionary<int, Rottable.RotAtmosphereQuality>()
  {
    {
      721531317,
      Rottable.RotAtmosphereQuality.Contaminating
    },
    {
      1887387588,
      Rottable.RotAtmosphereQuality.Contaminating
    },
    {
      -1528777920,
      Rottable.RotAtmosphereQuality.Normal
    },
    {
      1836671383,
      Rottable.RotAtmosphereQuality.Normal
    },
    {
      1960575215,
      Rottable.RotAtmosphereQuality.Sterilizing
    },
    {
      -899515856,
      Rottable.RotAtmosphereQuality.Sterilizing
    },
    {
      -1554872654,
      Rottable.RotAtmosphereQuality.Sterilizing
    },
    {
      -1858722091,
      Rottable.RotAtmosphereQuality.Sterilizing
    },
    {
      758759285,
      Rottable.RotAtmosphereQuality.Sterilizing
    },
    {
      -1046145888,
      Rottable.RotAtmosphereQuality.Sterilizing
    },
    {
      -1324664829,
      Rottable.RotAtmosphereQuality.Sterilizing
    },
    {
      -1406916018,
      Rottable.RotAtmosphereQuality.Sterilizing
    },
    {
      -432557516,
      Rottable.RotAtmosphereQuality.Sterilizing
    },
    {
      -805366663,
      Rottable.RotAtmosphereQuality.Sterilizing
    },
    {
      1966552544,
      Rottable.RotAtmosphereQuality.Sterilizing
    }
  };
  public StateMachine<Rottable, Rottable.Instance, IStateMachineTarget, Rottable.Def>.FloatParameter rotParameter;
  public GameStateMachine<Rottable, Rottable.Instance, IStateMachineTarget, Rottable.Def>.State Preserved;
  public GameStateMachine<Rottable, Rottable.Instance, IStateMachineTarget, Rottable.Def>.State Fresh;
  public GameStateMachine<Rottable, Rottable.Instance, IStateMachineTarget, Rottable.Def>.State Stale_Pre;
  public GameStateMachine<Rottable, Rottable.Instance, IStateMachineTarget, Rottable.Def>.State Stale;
  public GameStateMachine<Rottable, Rottable.Instance, IStateMachineTarget, Rottable.Def>.State Spoiled;

  public override void InitializeStates(out StateMachine.BaseState default_state)
  {
    default_state = (StateMachine.BaseState) this.Fresh;
    this.serializable = true;
    this.root.TagTransition(GameTags.Preserved, this.Preserved, false).TagTransition(GameTags.Entombed, this.Preserved, false);
    this.Fresh.ToggleStatusItem(Db.Get().CreatureStatusItems.Fresh, (Func<Rottable.Instance, object>) (smi => (object) smi)).ParamTransition<float>((StateMachine<Rottable, Rottable.Instance, IStateMachineTarget, Rottable.Def>.Parameter<float>) this.rotParameter, this.Stale_Pre, (StateMachine<Rottable, Rottable.Instance, IStateMachineTarget, Rottable.Def>.Parameter<float>.Callback) ((smi, p) => (double) p <= (double) smi.def.spoilTime - ((double) smi.def.spoilTime - (double) smi.def.staleTime))).FastUpdate("Rot", (UpdateBucketWithUpdater<Rottable.Instance>.IUpdater) Rottable.rotCB, UpdateRate.SIM_1000ms, true);
    this.Preserved.TagTransition(Rottable.PRESERVED_TAGS, this.Fresh, true).Enter("RefreshModifiers", (StateMachine<Rottable, Rottable.Instance, IStateMachineTarget, Rottable.Def>.State.Callback) (smi => smi.RefreshModifiers(0.0f)));
    this.Stale_Pre.Enter((StateMachine<Rottable, Rottable.Instance, IStateMachineTarget, Rottable.Def>.State.Callback) (smi => smi.GoTo((StateMachine.BaseState) this.Stale)));
    this.Stale.ToggleStatusItem(Db.Get().CreatureStatusItems.Stale, (Func<Rottable.Instance, object>) (smi => (object) smi)).ParamTransition<float>((StateMachine<Rottable, Rottable.Instance, IStateMachineTarget, Rottable.Def>.Parameter<float>) this.rotParameter, this.Fresh, (StateMachine<Rottable, Rottable.Instance, IStateMachineTarget, Rottable.Def>.Parameter<float>.Callback) ((smi, p) => (double) p > (double) smi.def.spoilTime - ((double) smi.def.spoilTime - (double) smi.def.staleTime))).ParamTransition<float>((StateMachine<Rottable, Rottable.Instance, IStateMachineTarget, Rottable.Def>.Parameter<float>) this.rotParameter, this.Spoiled, GameStateMachine<Rottable, Rottable.Instance, IStateMachineTarget, Rottable.Def>.IsLTEZero).FastUpdate("Rot", (UpdateBucketWithUpdater<Rottable.Instance>.IUpdater) Rottable.rotCB, UpdateRate.SIM_1000ms, false);
    this.Spoiled.Enter((StateMachine<Rottable, Rottable.Instance, IStateMachineTarget, Rottable.Def>.State.Callback) (smi =>
    {
      GameObject gameObject = Scenario.SpawnPrefab(Grid.PosToCell(smi.master.gameObject), 0, 0, "RotPile", Grid.SceneLayer.Ore);
      gameObject.gameObject.GetComponent<KSelectable>().SetName((string) UI.GAMEOBJECTEFFECTS.ROTTEN + " " + smi.master.gameObject.GetProperName());
      gameObject.transform.SetPosition(smi.master.transform.GetPosition());
      gameObject.GetComponent<PrimaryElement>().Mass = smi.master.GetComponent<PrimaryElement>().Mass;
      gameObject.GetComponent<PrimaryElement>().Temperature = smi.master.GetComponent<PrimaryElement>().Temperature;
      gameObject.SetActive(true);
      PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, (string) ITEMS.FOOD.ROTPILE.NAME, gameObject.transform, 1.5f, false);
      Edible component1 = smi.GetComponent<Edible>();
      if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
      {
        if ((UnityEngine.Object) component1.worker != (UnityEngine.Object) null)
        {
          ChoreDriver component2 = component1.worker.GetComponent<ChoreDriver>();
          if ((UnityEngine.Object) component2 != (UnityEngine.Object) null && component2.GetCurrentChore() != null)
            component2.GetCurrentChore().Fail("food rotted");
        }
        ReportManager.Instance.ReportValue(ReportManager.ReportType.CaloriesCreated, -component1.Calories, StringFormatter.Replace((string) UI.ENDOFDAYREPORT.NOTES.ROTTED, "{0}", smi.gameObject.GetProperName()), (string) UI.ENDOFDAYREPORT.NOTES.ROTTED_CONTEXT);
      }
      Util.KDestroyGameObject(smi.gameObject);
    }));
  }

  private static string OnStaleTooltip(List<Notification> notifications, object data)
  {
    string str = "\n";
    foreach (Notification notification in notifications)
    {
      if (notification.tooltipData != null)
      {
        GameObject tooltipData = (GameObject) notification.tooltipData;
        if ((UnityEngine.Object) tooltipData != (UnityEngine.Object) null)
          str = str + "\n" + tooltipData.GetProperName();
      }
    }
    return string.Format((string) MISC.NOTIFICATIONS.FOODSTALE.TOOLTIP, (object) str);
  }

  public static void SetStatusItems(
    KSelectable selectable,
    bool refrigerated,
    Rottable.RotAtmosphereQuality atmoshpere)
  {
    selectable.SetStatusItem(Db.Get().StatusItemCategories.PreservationTemperature, !refrigerated ? Db.Get().CreatureStatusItems.Unrefrigerated : Db.Get().CreatureStatusItems.Refrigerated, (object) selectable);
    switch (atmoshpere)
    {
      case Rottable.RotAtmosphereQuality.Normal:
        selectable.SetStatusItem(Db.Get().StatusItemCategories.PreservationAtmosphere, (StatusItem) null, (object) null);
        break;
      case Rottable.RotAtmosphereQuality.Sterilizing:
        selectable.SetStatusItem(Db.Get().StatusItemCategories.PreservationAtmosphere, Db.Get().CreatureStatusItems.SterilizingAtmosphere, (object) null);
        break;
      case Rottable.RotAtmosphereQuality.Contaminating:
        selectable.SetStatusItem(Db.Get().StatusItemCategories.PreservationAtmosphere, Db.Get().CreatureStatusItems.ContaminatedAtmosphere, (object) null);
        break;
    }
  }

  public static bool IsRefrigerated(GameObject gameObject)
  {
    int cell = Grid.PosToCell(gameObject);
    if (Grid.IsValidCell(cell))
    {
      if ((double) Grid.Temperature[cell] < 277.149993896484)
        return true;
      Pickupable component1 = gameObject.GetComponent<Pickupable>();
      if ((UnityEngine.Object) component1 != (UnityEngine.Object) null && (UnityEngine.Object) component1.storage != (UnityEngine.Object) null)
      {
        Refrigerator component2 = component1.storage.GetComponent<Refrigerator>();
        if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
          return component2.IsActive();
        return false;
      }
    }
    return false;
  }

  public static Rottable.RotAtmosphereQuality AtmosphereQuality(GameObject gameObject)
  {
    int cell1 = Grid.PosToCell(gameObject);
    int cell2 = Grid.CellAbove(cell1);
    SimHashes id1 = Grid.Element[cell1].id;
    Rottable.RotAtmosphereQuality atmosphereQuality1 = Rottable.RotAtmosphereQuality.Normal;
    Rottable.AtmosphereModifier.TryGetValue((int) id1, out atmosphereQuality1);
    Rottable.RotAtmosphereQuality atmosphereQuality2 = Rottable.RotAtmosphereQuality.Normal;
    if (Grid.IsValidCell(cell2))
    {
      SimHashes id2 = Grid.Element[cell2].id;
      if (!Rottable.AtmosphereModifier.TryGetValue((int) id2, out atmosphereQuality2))
        atmosphereQuality2 = atmosphereQuality1;
    }
    else
      atmosphereQuality2 = atmosphereQuality1;
    if (atmosphereQuality1 == atmosphereQuality2)
      return atmosphereQuality1;
    if (atmosphereQuality1 == Rottable.RotAtmosphereQuality.Contaminating || atmosphereQuality2 == Rottable.RotAtmosphereQuality.Contaminating)
      return Rottable.RotAtmosphereQuality.Contaminating;
    return atmosphereQuality1 == Rottable.RotAtmosphereQuality.Normal || atmosphereQuality2 == Rottable.RotAtmosphereQuality.Normal ? Rottable.RotAtmosphereQuality.Normal : Rottable.RotAtmosphereQuality.Sterilizing;
  }

  public class Def : StateMachine.BaseDef
  {
    public float rotTemperature = 277.15f;
    public float spoilTime;
    public float staleTime;
  }

  private class RotCB : UpdateBucketWithUpdater<Rottable.Instance>.IUpdater
  {
    public void Update(Rottable.Instance smi, float dt)
    {
      smi.Rot(smi, dt);
    }
  }

  public class Instance : GameStateMachine<Rottable, Rottable.Instance, IStateMachineTarget, Rottable.Def>.GameInstance
  {
    private AmountInstance RotAmountInstance;
    private AttributeModifier UnrefrigeratedModifier;
    private AttributeModifier ContaminatedAtmosphere;
    public PrimaryElement primaryElement;
    public Pickupable pickupable;

    public Instance(IStateMachineTarget master, Rottable.Def def)
      : base(master, def)
    {
      this.pickupable = this.gameObject.RequireComponent<Pickupable>();
      this.master.Subscribe(-2064133523, new System.Action<object>(this.OnAbsorb));
      this.master.Subscribe(1335436905, new System.Action<object>(this.OnSplitFromChunk));
      this.primaryElement = this.gameObject.GetComponent<PrimaryElement>();
      this.RotAmountInstance = master.gameObject.GetAmounts().Add(new AmountInstance(Db.Get().Amounts.Rot, master.gameObject));
      this.RotAmountInstance.maxAttribute.ClearModifiers();
      this.RotAmountInstance.maxAttribute.Add(new AttributeModifier("Rot", def.spoilTime, (string) null, false, false, true));
      double num1 = (double) this.RotAmountInstance.SetValue(def.spoilTime);
      double num2 = (double) this.sm.rotParameter.Set(this.RotAmountInstance.value, this.smi);
      this.UnrefrigeratedModifier = new AttributeModifier("Rot", 0.0f, (string) DUPLICANTS.MODIFIERS.ROTTEMPERATURE.NAME, false, false, false);
      this.ContaminatedAtmosphere = new AttributeModifier("Rot", 0.0f, (string) DUPLICANTS.MODIFIERS.ROTATMOSPHERE.NAME, false, false, false);
      this.RotAmountInstance.deltaAttribute.Add(this.UnrefrigeratedModifier);
      this.RotAmountInstance.deltaAttribute.Add(this.ContaminatedAtmosphere);
      this.RefreshModifiers(0.0f);
    }

    public float RotValue
    {
      get
      {
        return this.RotAmountInstance.value;
      }
      set
      {
        double num1 = (double) this.sm.rotParameter.Set(value, this);
        double num2 = (double) this.RotAmountInstance.SetValue(value);
      }
    }

    public float RotConstitutionPercentage
    {
      get
      {
        return this.RotValue / this.def.spoilTime;
      }
    }

    public string StateString()
    {
      string str = string.Empty;
      if (this.smi.GetCurrentState() == this.sm.Fresh)
        str = Db.Get().CreatureStatusItems.Fresh.resolveStringCallback((string) CREATURES.STATUSITEMS.FRESH.NAME, (object) this);
      if (this.smi.GetCurrentState() == this.sm.Stale)
        str = Db.Get().CreatureStatusItems.Fresh.resolveStringCallback((string) CREATURES.STATUSITEMS.STALE.NAME, (object) this);
      return str;
    }

    public void Rot(Rottable.Instance smi, float deltaTime)
    {
      double num = (double) smi.sm.rotParameter.Set(this.RotAmountInstance.value, smi);
      this.RefreshModifiers(deltaTime);
      if (!((UnityEngine.Object) smi.pickupable.storage != (UnityEngine.Object) null))
        return;
      smi.pickupable.storage.Trigger(-1197125120, (object) null);
    }

    public void RefreshModifiers(float dt)
    {
      if (this.GetMaster().isNull || !Grid.IsValidCell(Grid.PosToCell(this.gameObject)))
        return;
      KSelectable component = this.GetComponent<KSelectable>();
      if (this.GetComponent<KPrefabID>().HasAnyTags(Rottable.PRESERVED_TAGS))
      {
        this.UnrefrigeratedModifier.SetValue(0.0f);
        this.ContaminatedAtmosphere.SetValue(0.0f);
      }
      else
      {
        this.UnrefrigeratedModifier.SetValue(this.rotTemperatureModifier());
        this.ContaminatedAtmosphere.SetValue(this.rotAtmosphereModifier());
      }
      Rottable.RotAtmosphereQuality atmoshpere = (double) this.ContaminatedAtmosphere.Value != 0.0 ? ((double) this.ContaminatedAtmosphere.Value <= 0.0 ? Rottable.RotAtmosphereQuality.Contaminating : Rottable.RotAtmosphereQuality.Sterilizing) : Rottable.RotAtmosphereQuality.Normal;
      Rottable.SetStatusItems(component, (double) this.UnrefrigeratedModifier.Value == 0.0, atmoshpere);
      this.RotAmountInstance.deltaAttribute.ClearModifiers();
      if ((double) this.UnrefrigeratedModifier.Value != 0.0 && (double) this.ContaminatedAtmosphere.Value != 0.5)
        this.RotAmountInstance.deltaAttribute.Add(this.UnrefrigeratedModifier);
      if ((double) this.ContaminatedAtmosphere.Value == 0.0 || (double) this.ContaminatedAtmosphere.Value == 0.5)
        return;
      this.RotAmountInstance.deltaAttribute.Add(this.ContaminatedAtmosphere);
    }

    private float rotTemperatureModifier()
    {
      return Rottable.IsRefrigerated(this.gameObject) ? 0.0f : -0.5f;
    }

    private float rotAtmosphereModifier()
    {
      float num = 1f;
      switch (Rottable.AtmosphereQuality(this.gameObject))
      {
        case Rottable.RotAtmosphereQuality.Normal:
          num = 0.0f;
          break;
        case Rottable.RotAtmosphereQuality.Sterilizing:
          num = 0.5f;
          break;
        case Rottable.RotAtmosphereQuality.Contaminating:
          num = -0.5f;
          break;
      }
      return num;
    }

    private void OnAbsorb(object data)
    {
      Pickupable pickupable = (Pickupable) data;
      if (!((UnityEngine.Object) pickupable != (UnityEngine.Object) null))
        return;
      PrimaryElement component = this.gameObject.GetComponent<PrimaryElement>();
      PrimaryElement primaryElement = pickupable.PrimaryElement;
      Rottable.Instance smi = pickupable.gameObject.GetSMI<Rottable.Instance>();
      if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || !((UnityEngine.Object) primaryElement != (UnityEngine.Object) null) || smi == null)
        return;
      double num = (double) this.sm.rotParameter.Set((float) (((double) (component.Units * this.sm.rotParameter.Get(this.smi)) + (double) (primaryElement.Units * this.sm.rotParameter.Get(smi))) / ((double) component.Units + (double) primaryElement.Units)), this.smi);
    }

    public bool IsRotLevelStackable(Rottable.Instance other)
    {
      return (double) Mathf.Abs(this.RotConstitutionPercentage - other.RotConstitutionPercentage) < 0.100000001490116;
    }

    public string GetToolTip()
    {
      return this.RotAmountInstance.GetTooltip();
    }

    private void OnSplitFromChunk(object data)
    {
      Pickupable cmp = (Pickupable) data;
      if (!((UnityEngine.Object) cmp != (UnityEngine.Object) null))
        return;
      Rottable.Instance smi = cmp.GetSMI<Rottable.Instance>();
      if (smi == null)
        return;
      this.RotValue = smi.RotValue;
    }

    public void OnPreserved(object data)
    {
      if ((bool) data)
        this.smi.GoTo((StateMachine.BaseState) this.sm.Preserved);
      else
        this.smi.GoTo((StateMachine.BaseState) this.sm.Fresh);
    }
  }

  public enum RotAtmosphereQuality
  {
    Normal,
    Sterilizing,
    Contaminating,
  }
}
