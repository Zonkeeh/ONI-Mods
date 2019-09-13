// Decompiled with JetBrains decompiler
// Type: Toilet
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Toilet : StateMachineComponent<Toilet.StatesInstance>, ISaveLoadable, IUsable, IEffectDescriptor, IGameObjectEffectDescriptor
{
  private static readonly EventSystem.IntraObjectHandler<Toilet> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Toilet>((System.Action<Toilet, object>) ((component, data) => component.OnRefreshUserMenu(data)));
  [SerializeField]
  public int maxFlushes = 15;
  [SerializeField]
  public Toilet.SpawnInfo solidWastePerUse;
  [SerializeField]
  public float solidWasteTemperature;
  [SerializeField]
  public Toilet.SpawnInfo gasWasteWhenFull;
  [SerializeField]
  public string diseaseId;
  [SerializeField]
  public int diseasePerFlush;
  [SerializeField]
  public int diseaseOnDupePerFlush;
  [Serialize]
  public int _flushesUsed;
  private MeterController meter;
  [MyCmpReq]
  private Storage storage;

  public int FlushesUsed
  {
    get
    {
      return this._flushesUsed;
    }
    set
    {
      this._flushesUsed = value;
      this.smi.sm.flushes.Set(value, this.smi);
    }
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    Components.Toilets.Add((IUsable) this);
    this.smi.StartSM();
    this.GetComponent<ToiletWorkableUse>().trackUses = true;
    this.meter = new MeterController((KAnimControllerBase) this.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Behind, Grid.SceneLayer.NoLayer, new string[3]
    {
      "meter_target",
      "meter_arrow",
      "meter_scale"
    });
    this.meter.SetPositionPercent((float) this.FlushesUsed / (float) this.maxFlushes);
    this.FlushesUsed = this._flushesUsed;
    this.Subscribe<Toilet>(493375141, Toilet.OnRefreshUserMenuDelegate);
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    Components.Toilets.Remove((IUsable) this);
  }

  public bool IsUsable()
  {
    return this.smi.HasTag(GameTags.Usable);
  }

  public void Flush(Worker worker)
  {
    Element elementByHash = ElementLoader.FindElementByHash(this.solidWastePerUse.elementID);
    byte index = Db.Get().Diseases.GetIndex((HashedString) this.diseaseId);
    this.storage.Store(elementByHash.substance.SpawnResource(this.transform.GetPosition(), this.smi.MassPerFlush(), this.solidWasteTemperature, index, this.diseasePerFlush, true, false, false), false, false, true, false);
    worker.GetComponent<PrimaryElement>().AddDisease(index, this.diseaseOnDupePerFlush, "Toilet.Flush");
    PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, string.Format((string) DUPLICANTS.DISEASES.ADDED_POPFX, (object) Db.Get().Diseases[(int) index].Name, (object) (this.diseasePerFlush + this.diseaseOnDupePerFlush)), this.transform, Vector3.up, 1.5f, false, false);
    ++this.FlushesUsed;
    this.meter.SetPositionPercent((float) this.FlushesUsed / (float) this.maxFlushes);
    Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_LotsOfGerms, true);
  }

  private void OnRefreshUserMenu(object data)
  {
    if (this.smi.GetCurrentState() == this.smi.sm.full || !this.smi.IsSoiled || this.smi.cleanChore != null)
      return;
    Game.Instance.userMenu.AddButton(this.gameObject, new KIconButtonMenu.ButtonInfo("status_item_toilet_needs_emptying", (string) UI.USERMENUACTIONS.CLEANTOILET.NAME, (System.Action) (() => this.smi.GoTo((StateMachine.BaseState) this.smi.sm.earlyclean)), Action.NumActions, (System.Action<GameObject>) null, (System.Action<KIconButtonMenu.ButtonInfo>) null, (Texture) null, (string) UI.USERMENUACTIONS.CLEANTOILET.TOOLTIP, true), 1f);
  }

  private void SpawnMonster()
  {
    GameUtil.KInstantiate(Assets.GetPrefab(new Tag("Glom")), this.smi.transform.GetPosition(), Grid.SceneLayer.Creatures, (string) null, 0).SetActive(true);
  }

  public List<Descriptor> RequirementDescriptors()
  {
    List<Descriptor> descriptorList = new List<Descriptor>();
    string str = this.GetComponent<ManualDeliveryKG>().requestedItemTag.ProperName();
    Descriptor descriptor = new Descriptor();
    descriptor.SetupDescriptor(string.Format((string) UI.BUILDINGEFFECTS.ELEMENTCONSUMEDPERUSE, (object) str, (object) GameUtil.GetFormattedMass(this.smi.MassPerFlush(), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMEDPERUSE, (object) str, (object) GameUtil.GetFormattedMass(this.smi.MassPerFlush(), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}")), Descriptor.DescriptorType.Requirement);
    descriptorList.Add(descriptor);
    return descriptorList;
  }

  public List<Descriptor> EffectDescriptors()
  {
    List<Descriptor> descriptorList = new List<Descriptor>();
    string str = ElementLoader.FindElementByHash(this.solidWastePerUse.elementID).tag.ProperName();
    descriptorList.Add(new Descriptor(string.Format((string) UI.BUILDINGEFFECTS.ELEMENTEMITTED_TOILET, (object) str, (object) GameUtil.GetFormattedMass(this.smi.MassPerFlush(), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}"), (object) GameUtil.GetFormattedTemperature(this.solidWasteTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTEMITTED_TOILET, (object) str, (object) GameUtil.GetFormattedMass(this.smi.MassPerFlush(), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.##}"), (object) GameUtil.GetFormattedTemperature(this.solidWasteTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), Descriptor.DescriptorType.Effect, false));
    Klei.AI.Disease disease = Db.Get().Diseases.Get(this.diseaseId);
    int units = this.diseasePerFlush + this.diseaseOnDupePerFlush;
    descriptorList.Add(new Descriptor(string.Format((string) UI.BUILDINGEFFECTS.DISEASEEMITTEDPERUSE, (object) disease.Name, (object) GameUtil.GetFormattedDiseaseAmount(units)), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.DISEASEEMITTEDPERUSE, (object) disease.Name, (object) GameUtil.GetFormattedDiseaseAmount(units)), Descriptor.DescriptorType.DiseaseSource, false));
    return descriptorList;
  }

  public List<Descriptor> GetDescriptors(BuildingDef def)
  {
    List<Descriptor> descriptorList = new List<Descriptor>();
    descriptorList.AddRange((IEnumerable<Descriptor>) this.RequirementDescriptors());
    descriptorList.AddRange((IEnumerable<Descriptor>) this.EffectDescriptors());
    return descriptorList;
  }

  public List<Descriptor> GetDescriptors(GameObject go)
  {
    List<Descriptor> descriptorList = new List<Descriptor>();
    foreach (Descriptor requirementDescriptor in this.RequirementDescriptors())
      descriptorList.Add(requirementDescriptor);
    foreach (Descriptor effectDescriptor in this.EffectDescriptors())
      descriptorList.Add(effectDescriptor);
    return descriptorList;
  }

  Transform IUsable.get_transform()
  {
    return this.transform;
  }

  [Serializable]
  public struct SpawnInfo
  {
    [HashedEnum]
    public SimHashes elementID;
    public float mass;
    public float interval;

    public SpawnInfo(SimHashes element_id, float mass, float interval)
    {
      this.elementID = element_id;
      this.mass = mass;
      this.interval = interval;
    }
  }

  public class StatesInstance : GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.GameInstance
  {
    public float monsterSpawnTime = 1200f;
    public Chore cleanChore;
    public List<Chore> activeUseChores;

    public StatesInstance(Toilet master)
      : base(master)
    {
      this.activeUseChores = new List<Chore>();
    }

    public bool IsSoiled
    {
      get
      {
        return this.master.FlushesUsed > 0;
      }
    }

    public int GetFlushesRemaining()
    {
      return this.master.maxFlushes - this.master.FlushesUsed;
    }

    public bool HasDirt()
    {
      if (this.master.storage.IsEmpty())
        return false;
      return this.master.storage.Has(ElementLoader.FindElementByHash(SimHashes.Dirt).tag);
    }

    public float MassPerFlush()
    {
      return this.master.solidWastePerUse.mass;
    }

    public bool IsToxicSandRemoved()
    {
      return (UnityEngine.Object) this.master.storage.FindFirst(GameTagExtensions.Create(this.master.solidWastePerUse.elementID)) == (UnityEngine.Object) null;
    }

    public void CreateCleanChore()
    {
      if (this.cleanChore != null)
        this.cleanChore.Cancel("dupe");
      this.cleanChore = (Chore) new WorkChore<ToiletWorkableClean>(Db.Get().ChoreTypes.CleanToilet, (IStateMachineTarget) this.master.GetComponent<ToiletWorkableClean>(), (ChoreProvider) null, true, new System.Action<Chore>(this.OnCleanComplete), (System.Action<Chore>) null, (System.Action<Chore>) null, true, (ScheduleBlockType) null, false, true, (KAnimFile) null, false, true, true, PriorityScreen.PriorityClass.basic, 5, true, true);
    }

    public void CancelCleanChore()
    {
      if (this.cleanChore == null)
        return;
      this.cleanChore.Cancel("Cancelled");
      this.cleanChore = (Chore) null;
    }

    private void OnCleanComplete(Chore chore)
    {
      this.cleanChore = (Chore) null;
      Tag tag = GameTagExtensions.Create(this.master.solidWastePerUse.elementID);
      ListPool<GameObject, Toilet>.PooledList pooledList = ListPool<GameObject, Toilet>.Allocate();
      this.master.storage.Find(tag, (List<GameObject>) pooledList);
      foreach (GameObject go in (List<GameObject>) pooledList)
        this.master.storage.Drop(go, true);
      pooledList.Recycle();
      this.master.meter.SetPositionPercent((float) this.master.FlushesUsed / (float) this.master.maxFlushes);
    }

    public void Flush()
    {
      this.master.Flush(this.master.GetComponent<ToiletWorkableUse>().worker);
    }
  }

  public class States : GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet>
  {
    private static readonly HashedString[] FULL_ANIMS = new HashedString[2]
    {
      (HashedString) "full_pre",
      (HashedString) nameof (full)
    };
    public StateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.IntParameter flushes = new StateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.IntParameter(0);
    public GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State needsdirt;
    public GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State empty;
    public GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State notoperational;
    public GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State ready;
    public GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State earlyclean;
    public GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State earlyWaitingForClean;
    public GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State full;
    public GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State fullWaitingForClean;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.needsdirt;
      this.root.PlayAnim("off").EventTransition(GameHashes.OnStorageChange, this.needsdirt, (StateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.Transition.ConditionCallback) (smi => !smi.HasDirt())).EventTransition(GameHashes.OperationalChanged, this.notoperational, (StateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.Transition.ConditionCallback) (smi => !smi.Get<Operational>().IsOperational));
      this.needsdirt.ToggleMainStatusItem(Db.Get().BuildingStatusItems.Unusable).EventTransition(GameHashes.OnStorageChange, this.ready, (StateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.Transition.ConditionCallback) (smi => smi.HasDirt()));
      this.ready.ParamTransition<int>((StateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.Parameter<int>) this.flushes, this.full, (StateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.Parameter<int>.Callback) ((smi, p) => smi.GetFlushesRemaining() <= 0)).ToggleMainStatusItem(Db.Get().BuildingStatusItems.Toilet).ToggleRecurringChore(new Func<Toilet.StatesInstance, Chore>(this.CreateUrgentUseChore), (Func<Toilet.StatesInstance, bool>) null).ToggleRecurringChore(new Func<Toilet.StatesInstance, Chore>(this.CreateBreakUseChore), (Func<Toilet.StatesInstance, bool>) null).ToggleTag(GameTags.Usable).EventHandler(GameHashes.Flush, (GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.GameEvent.Callback) ((smi, data) => smi.Flush()));
      this.earlyclean.PlayAnims((Func<Toilet.StatesInstance, HashedString[]>) (smi => Toilet.States.FULL_ANIMS), KAnim.PlayMode.Once).OnAnimQueueComplete(this.earlyWaitingForClean);
      this.earlyWaitingForClean.Enter((StateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State.Callback) (smi => smi.CreateCleanChore())).Exit((StateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State.Callback) (smi => smi.CancelCleanChore())).ToggleStatusItem(Db.Get().BuildingStatusItems.ToiletNeedsEmptying, (object) null).ToggleMainStatusItem(Db.Get().BuildingStatusItems.Unusable).EventTransition(GameHashes.OnStorageChange, this.empty, (StateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.Transition.ConditionCallback) (smi => smi.IsToxicSandRemoved()));
      this.full.PlayAnims((Func<Toilet.StatesInstance, HashedString[]>) (smi => Toilet.States.FULL_ANIMS), KAnim.PlayMode.Once).OnAnimQueueComplete(this.fullWaitingForClean);
      this.fullWaitingForClean.Enter((StateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State.Callback) (smi => smi.CreateCleanChore())).Exit((StateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State.Callback) (smi => smi.CancelCleanChore())).ToggleStatusItem(Db.Get().BuildingStatusItems.ToiletNeedsEmptying, (object) null).ToggleMainStatusItem(Db.Get().BuildingStatusItems.Unusable).EventTransition(GameHashes.OnStorageChange, this.empty, (StateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.Transition.ConditionCallback) (smi => smi.IsToxicSandRemoved())).Enter((StateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State.Callback) (smi => smi.Schedule(smi.monsterSpawnTime, (System.Action<object>) (_param1 => smi.master.SpawnMonster()), (object) null)));
      this.empty.PlayAnim("off").Enter("ClearFlushes", (StateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State.Callback) (smi => smi.master.FlushesUsed = 0)).Enter("ClearDirt", (StateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State.Callback) (smi => smi.master.storage.ConsumeAllIgnoringDisease())).GoTo(this.needsdirt);
      this.notoperational.EventTransition(GameHashes.OperationalChanged, this.needsdirt, (StateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.Transition.ConditionCallback) (smi => smi.Get<Operational>().IsOperational)).ToggleMainStatusItem(Db.Get().BuildingStatusItems.Unusable);
    }

    private Chore CreateUrgentUseChore(Toilet.StatesInstance smi)
    {
      Chore useChore = this.CreateUseChore(smi, Db.Get().ChoreTypes.Pee);
      useChore.AddPrecondition(ChorePreconditions.instance.IsBladderFull, (object) null);
      useChore.AddPrecondition(ChorePreconditions.instance.NotCurrentlyPeeing, (object) null);
      return useChore;
    }

    private Chore CreateBreakUseChore(Toilet.StatesInstance smi)
    {
      Chore useChore = this.CreateUseChore(smi, Db.Get().ChoreTypes.BreakPee);
      useChore.AddPrecondition(ChorePreconditions.instance.IsBladderNotFull, (object) null);
      useChore.AddPrecondition(ChorePreconditions.instance.IsScheduledTime, (object) Db.Get().ScheduleBlockTypes.Hygiene);
      return useChore;
    }

    private Chore CreateUseChore(Toilet.StatesInstance smi, ChoreType choreType)
    {
      WorkChore<ToiletWorkableUse> workChore1 = new WorkChore<ToiletWorkableUse>(choreType, (IStateMachineTarget) smi.master, (ChoreProvider) null, true, (System.Action<Chore>) null, (System.Action<Chore>) null, (System.Action<Chore>) null, false, (ScheduleBlockType) null, true, true, (KAnimFile) null, false, true, false, PriorityScreen.PriorityClass.personalNeeds, 5, false, false);
      smi.activeUseChores.Add((Chore) workChore1);
      WorkChore<ToiletWorkableUse> workChore2 = workChore1;
      workChore2.onExit = workChore2.onExit + (System.Action<Chore>) (exiting_chore => smi.activeUseChores.Remove(exiting_chore));
      workChore1.AddPrecondition(ChorePreconditions.instance.IsPreferredAssignableOrUrgentBladder, (object) smi.master.GetComponent<Assignable>());
      workChore1.AddPrecondition(ChorePreconditions.instance.IsExclusivelyAvailableWithOtherChores, (object) smi.activeUseChores);
      return (Chore) workChore1;
    }

    public class ReadyStates : GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State
    {
      public GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State idle;
      public GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State inuse;
      public GameStateMachine<Toilet.States, Toilet.StatesInstance, Toilet, object>.State flush;
    }
  }
}
