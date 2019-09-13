// Decompiled with JetBrains decompiler
// Type: TinkerStation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using Klei.AI;
using STRINGS;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class TinkerStation : Workable, IEffectDescriptor, ISim1000ms
{
  private static readonly EventSystem.IntraObjectHandler<TinkerStation> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<TinkerStation>((System.Action<TinkerStation, object>) ((component, data) => component.OnOperationalChanged(data)));
  public HashedString choreType;
  public HashedString fetchChoreType;
  private Chore chore;
  [MyCmpAdd]
  private Operational operational;
  [MyCmpAdd]
  private Storage storage;
  public bool useFilteredStorage;
  protected FilteredStorage filteredStorage;
  public float massPerTinker;
  public Tag inputMaterial;
  public Tag outputPrefab;
  public float outputTemperature;

  public AttributeConverter AttributeConverter
  {
    set
    {
      this.attributeConverter = value;
    }
  }

  public float AttributeExperienceMultiplier
  {
    set
    {
      this.attributeExperienceMultiplier = value;
    }
  }

  public string SkillExperienceSkillGroup
  {
    set
    {
      this.skillExperienceSkillGroup = value;
    }
  }

  public float SkillExperienceMultiplier
  {
    set
    {
      this.skillExperienceMultiplier = value;
    }
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.attributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
    this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
    this.skillExperienceSkillGroup = Db.Get().SkillGroups.Technicals.Id;
    this.skillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
    if (this.useFilteredStorage)
      this.filteredStorage = new FilteredStorage((KMonoBehaviour) this, (Tag[]) null, (Tag[]) null, (IUserControlledCapacity) null, false, Db.Get().ChoreTypes.GetByHash(this.fetchChoreType));
    this.SetWorkTime(15f);
    this.Subscribe<TinkerStation>(-592767678, TinkerStation.OnOperationalChangedDelegate);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    if (!this.useFilteredStorage || this.filteredStorage == null)
      return;
    this.filteredStorage.FilterChanged();
  }

  protected override void OnCleanUp()
  {
    if (this.filteredStorage != null)
      this.filteredStorage.CleanUp();
    base.OnCleanUp();
  }

  private bool CorrectRolePrecondition(MinionIdentity worker)
  {
    MinionResume component = worker.GetComponent<MinionResume>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      return component.HasPerk((HashedString) this.requiredSkillPerk);
    return false;
  }

  private void OnOperationalChanged(object data)
  {
    RoomTracker component = this.GetComponent<RoomTracker>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.room == null)
      return;
    component.room.RetriggerBuildings();
  }

  protected override void OnStartWork(Worker worker)
  {
    base.OnStartWork(worker);
    if (!this.operational.IsOperational)
      return;
    this.operational.SetActive(true, false);
  }

  protected override void OnStopWork(Worker worker)
  {
    base.OnStopWork(worker);
    this.ShowProgressBar(false);
    this.operational.SetActive(false, false);
  }

  protected override void OnCompleteWork(Worker worker)
  {
    base.OnCompleteWork(worker);
    SimUtil.DiseaseInfo disease_info;
    float aggregate_temperature;
    this.storage.ConsumeAndGetDisease(this.inputMaterial, this.massPerTinker, out disease_info, out aggregate_temperature);
    GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(this.outputPrefab), this.transform.GetPosition(), Grid.SceneLayer.Ore, (string) null, 0);
    gameObject.GetComponent<PrimaryElement>().Temperature = this.outputTemperature;
    gameObject.SetActive(true);
    this.chore = (Chore) null;
  }

  public void Sim1000ms(float dt)
  {
    this.UpdateChore();
  }

  private void UpdateChore()
  {
    if (this.operational.IsOperational && this.ToolsRequested() && this.HasMaterial())
    {
      if (this.chore != null)
        return;
      this.chore = (Chore) new WorkChore<TinkerStation>(Db.Get().ChoreTypes.GetByHash(this.choreType), (IStateMachineTarget) this, (ChoreProvider) null, true, (System.Action<Chore>) null, (System.Action<Chore>) null, (System.Action<Chore>) null, true, (ScheduleBlockType) null, false, true, (KAnimFile) null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
      this.chore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, (object) this.requiredSkillPerk);
      this.SetWorkTime(this.workTime);
    }
    else
    {
      if (this.chore == null)
        return;
      this.chore.Cancel("Can't tinker");
      this.chore = (Chore) null;
    }
  }

  private bool HasMaterial()
  {
    return (double) this.storage.MassStored() > 0.0;
  }

  private bool ToolsRequested()
  {
    if ((double) MaterialNeeds.Instance.GetAmount(this.outputPrefab) > 0.0)
      return (double) WorldInventory.Instance.GetAmount(this.outputPrefab) <= 0.0;
    return false;
  }

  public List<Descriptor> GetDescriptors(BuildingDef def)
  {
    string str = this.inputMaterial.ProperName();
    List<Descriptor> descriptorList = new List<Descriptor>();
    descriptorList.Add(new Descriptor(string.Format((string) UI.BUILDINGEFFECTS.ELEMENTCONSUMEDPERUSE, (object) str, (object) GameUtil.GetFormattedMass(this.massPerTinker, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMEDPERUSE, (object) str, (object) GameUtil.GetFormattedMass(this.massPerTinker, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), Descriptor.DescriptorType.Requirement, false));
    descriptorList.AddRange((IEnumerable<Descriptor>) GameUtil.GetAllDescriptors(Assets.GetPrefab(this.outputPrefab), false));
    List<Tinkerable> tinkerableList = new List<Tinkerable>();
    foreach (GameObject gameObject in Assets.GetPrefabsWithComponent<Tinkerable>())
    {
      Tinkerable component = gameObject.GetComponent<Tinkerable>();
      if (component.tinkerMaterialTag == this.outputPrefab)
        tinkerableList.Add(component);
    }
    if (tinkerableList.Count > 0)
    {
      Effect effect = Db.Get().effects.Get(tinkerableList[0].addedEffect);
      descriptorList.Add(new Descriptor(string.Format((string) UI.BUILDINGEFFECTS.ADDED_EFFECT, (object) effect.Name), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.ADDED_EFFECT, (object) effect.Name, (object) Effect.CreateTooltip(effect, true, "\n")), Descriptor.DescriptorType.Effect, false));
      descriptorList.Add(new Descriptor((string) UI.BUILDINGEFFECTS.IMPROVED_BUILDINGS, (string) UI.BUILDINGEFFECTS.TOOLTIPS.IMPROVED_BUILDINGS, Descriptor.DescriptorType.Effect, false));
      foreach (Tinkerable cmp in tinkerableList)
      {
        Descriptor descriptor = new Descriptor(string.Format((string) UI.BUILDINGEFFECTS.IMPROVED_BUILDINGS_ITEM, (object) cmp.GetProperName()), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.IMPROVED_BUILDINGS_ITEM, (object) cmp.GetProperName()), Descriptor.DescriptorType.Effect, false);
        descriptor.IncreaseIndent();
        descriptorList.Add(descriptor);
      }
    }
    return descriptorList;
  }

  public static TinkerStation AddTinkerStation(
    GameObject go,
    string required_room_type)
  {
    TinkerStation tinkerStation = go.AddOrGet<TinkerStation>();
    go.AddOrGet<RoomTracker>().requiredRoomType = required_room_type;
    return tinkerStation;
  }
}
