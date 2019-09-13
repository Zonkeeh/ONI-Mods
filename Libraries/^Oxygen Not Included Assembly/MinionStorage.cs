// Decompiled with JetBrains decompiler
// Type: MinionStorage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using KSerialization;
using System;
using System.Collections.Generic;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class MinionStorage : KMonoBehaviour
{
  [Serialize]
  private List<MinionStorage.Info> serializedMinions = new List<MinionStorage.Info>();

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    Components.MinionStorages.Add(this);
  }

  protected override void OnCleanUp()
  {
    Components.MinionStorages.Remove(this);
    base.OnCleanUp();
  }

  private KPrefabID CreateSerializedMinion(GameObject src_minion)
  {
    GameObject gameObject = Util.KInstantiate(SaveLoader.Instance.saveManager.GetPrefab((Tag) StoredMinionConfig.ID), Vector3.zero);
    gameObject.SetActive(true);
    MinionIdentity component1 = src_minion.GetComponent<MinionIdentity>();
    StoredMinionIdentity component2 = gameObject.GetComponent<StoredMinionIdentity>();
    this.CopyMinion(component1, component2);
    MinionStorage.RedirectInstanceTracker(src_minion, gameObject);
    component1.assignableProxy.Get().SetTarget((IAssignableIdentity) component2, gameObject);
    Util.KDestroyGameObject(src_minion);
    return gameObject.GetComponent<KPrefabID>();
  }

  private void CopyMinion(MinionIdentity src_id, StoredMinionIdentity dest_id)
  {
    dest_id.storedName = src_id.name;
    dest_id.nameStringKey = src_id.nameStringKey;
    dest_id.gender = src_id.gender;
    dest_id.genderStringKey = src_id.genderStringKey;
    dest_id.arrivalTime = src_id.arrivalTime;
    dest_id.voiceIdx = src_id.voiceIdx;
    dest_id.bodyData = src_id.bodyData;
    Traits component1 = src_id.GetComponent<Traits>();
    dest_id.traitIDs = new List<string>((IEnumerable<string>) component1.GetTraitIds());
    dest_id.assignableProxy.Set(src_id.assignableProxy.Get());
    dest_id.assignableProxy.Get().SetTarget((IAssignableIdentity) dest_id, dest_id.gameObject);
    Accessorizer component2 = src_id.GetComponent<Accessorizer>();
    dest_id.accessories = component2.GetAccessories();
    ConsumableConsumer component3 = src_id.GetComponent<ConsumableConsumer>();
    if (component3.forbiddenTags != null)
      dest_id.forbiddenTags = new List<Tag>((IEnumerable<Tag>) component3.forbiddenTags);
    MinionResume component4 = src_id.GetComponent<MinionResume>();
    dest_id.MasteryBySkillID = component4.MasteryBySkillID;
    dest_id.AptitudeBySkillGroup = component4.AptitudeBySkillGroup;
    dest_id.TotalExperienceGained = component4.TotalExperienceGained;
    dest_id.currentHat = component4.CurrentHat;
    dest_id.targetHat = component4.TargetHat;
    ChoreConsumer component5 = src_id.GetComponent<ChoreConsumer>();
    dest_id.choreGroupPriorities = component5.GetChoreGroupPriorities();
    AttributeLevels component6 = src_id.GetComponent<AttributeLevels>();
    component6.OnSerializing();
    dest_id.attributeLevels = new List<AttributeLevels.LevelSaveLoad>((IEnumerable<AttributeLevels.LevelSaveLoad>) component6.SaveLoadLevels);
    MinionStorage.StoreModifiers(src_id, dest_id);
    Schedulable component7 = src_id.GetComponent<Schedulable>();
    Schedule schedule = component7.GetSchedule();
    if (schedule == null)
      return;
    schedule.Unassign(component7);
    Schedulable component8 = dest_id.GetComponent<Schedulable>();
    schedule.Assign(component8);
  }

  private static void StoreModifiers(MinionIdentity src_id, StoredMinionIdentity dest_id)
  {
    foreach (AttributeInstance attribute in src_id.GetComponent<MinionModifiers>().attributes)
    {
      if (dest_id.minionModifiers.attributes.Get(attribute.Attribute.Id) == null)
        dest_id.minionModifiers.attributes.Add(attribute.Attribute);
      for (int index = 0; index < attribute.Modifiers.Count; ++index)
        dest_id.minionModifiers.attributes.Get(attribute.Id).Add(attribute.Modifiers[index]);
    }
  }

  private static void CopyMinion(StoredMinionIdentity src_id, MinionIdentity dest_id)
  {
    dest_id.SetName(src_id.storedName);
    dest_id.nameStringKey = src_id.nameStringKey;
    dest_id.gender = src_id.gender;
    dest_id.genderStringKey = src_id.genderStringKey;
    dest_id.arrivalTime = src_id.arrivalTime;
    dest_id.voiceIdx = src_id.voiceIdx;
    dest_id.bodyData = src_id.bodyData;
    if (src_id.traitIDs != null)
      dest_id.GetComponent<Traits>().SetTraitIds(src_id.traitIDs);
    if (src_id.accessories != null)
      dest_id.GetComponent<Accessorizer>().SetAccessories(src_id.accessories);
    ConsumableConsumer component1 = dest_id.GetComponent<ConsumableConsumer>();
    if (src_id.forbiddenTags != null)
      component1.forbiddenTags = src_id.forbiddenTags.ToArray();
    if (src_id.MasteryBySkillID != null)
    {
      MinionResume component2 = dest_id.GetComponent<MinionResume>();
      component2.RestoreResume(src_id.MasteryBySkillID, src_id.AptitudeBySkillGroup, src_id.TotalExperienceGained);
      component2.SetHats(src_id.currentHat, src_id.targetHat);
    }
    if (src_id.choreGroupPriorities != null)
      dest_id.GetComponent<ChoreConsumer>().SetChoreGroupPriorities(src_id.choreGroupPriorities);
    AttributeLevels component3 = dest_id.GetComponent<AttributeLevels>();
    if (src_id.attributeLevels != null)
    {
      component3.SaveLoadLevels = src_id.attributeLevels.ToArray();
      component3.OnDeserialized();
    }
    dest_id.GetComponent<Accessorizer>().ApplyAccessories();
    dest_id.assignableProxy = new Ref<MinionAssignablesProxy>();
    dest_id.assignableProxy.Set(src_id.assignableProxy.Get());
    dest_id.assignableProxy.Get().SetTarget((IAssignableIdentity) dest_id, dest_id.gameObject);
    Equipment equipment = dest_id.GetEquipment();
    foreach (AssignableSlotInstance slot in equipment.Slots)
    {
      Equippable assignable = slot.assignable as Equippable;
      if ((UnityEngine.Object) assignable != (UnityEngine.Object) null)
        equipment.Equip(assignable);
    }
    Schedulable component4 = src_id.GetComponent<Schedulable>();
    Schedule schedule = component4.GetSchedule();
    if (schedule == null)
      return;
    schedule.Unassign(component4);
    Schedulable component5 = dest_id.GetComponent<Schedulable>();
    schedule.Assign(component5);
  }

  public static void RedirectInstanceTracker(GameObject src_minion, GameObject dest_minion)
  {
    KPrefabID component = src_minion.GetComponent<KPrefabID>();
    dest_minion.GetComponent<KPrefabID>().InstanceID = component.InstanceID;
    component.InstanceID = -1;
  }

  public void SerializeMinion(GameObject minion)
  {
    this.CleanupBadReferences();
    KPrefabID serializedMinion = this.CreateSerializedMinion(minion);
    this.serializedMinions.Add(new MinionStorage.Info(serializedMinion.GetComponent<StoredMinionIdentity>().storedName, new Ref<KPrefabID>(serializedMinion)));
  }

  private void CleanupBadReferences()
  {
    for (int index = this.serializedMinions.Count - 1; index >= 0; --index)
    {
      if (this.serializedMinions[index].serializedMinion == null || (UnityEngine.Object) this.serializedMinions[index].serializedMinion.Get() == (UnityEngine.Object) null)
        this.serializedMinions.RemoveAt(index);
    }
  }

  private int GetMinionIndex(Guid id)
  {
    int num = -1;
    for (int index = 0; index < this.serializedMinions.Count; ++index)
    {
      if (this.serializedMinions[index].id == id)
      {
        num = index;
        break;
      }
    }
    return num;
  }

  public GameObject DeserializeMinion(Guid id, Vector3 pos)
  {
    int minionIndex = this.GetMinionIndex(id);
    if (minionIndex < 0 || minionIndex >= this.serializedMinions.Count)
      return (GameObject) null;
    KPrefabID kprefabId = this.serializedMinions[minionIndex].serializedMinion.Get();
    this.serializedMinions.RemoveAt(minionIndex);
    if ((UnityEngine.Object) kprefabId == (UnityEngine.Object) null)
      return (GameObject) null;
    return MinionStorage.DeserializeMinion(kprefabId.gameObject, pos);
  }

  public static GameObject DeserializeMinion(GameObject sourceMinion, Vector3 pos)
  {
    GameObject gameObject = Util.KInstantiate(SaveLoader.Instance.saveManager.GetPrefab((Tag) MinionConfig.ID), pos);
    StoredMinionIdentity component1 = sourceMinion.GetComponent<StoredMinionIdentity>();
    MinionIdentity component2 = gameObject.GetComponent<MinionIdentity>();
    MinionStorage.RedirectInstanceTracker(sourceMinion, gameObject);
    gameObject.SetActive(true);
    MinionStorage.CopyMinion(component1, component2);
    component1.assignableProxy.Get().SetTarget((IAssignableIdentity) component2, gameObject);
    Util.KDestroyGameObject(sourceMinion);
    return gameObject;
  }

  public void DeleteStoredMinion(Guid id)
  {
    int minionIndex = this.GetMinionIndex(id);
    if (minionIndex < 0)
      return;
    if (this.serializedMinions[minionIndex].serializedMinion != null)
      Util.KDestroyGameObject(this.serializedMinions[minionIndex].serializedMinion.Get().gameObject);
    this.serializedMinions.RemoveAt(minionIndex);
  }

  public List<MinionStorage.Info> GetStoredMinionInfo()
  {
    return this.serializedMinions;
  }

  public struct Info
  {
    public Guid id;
    public string name;
    public Ref<KPrefabID> serializedMinion;

    public Info(string name, Ref<KPrefabID> ref_obj)
    {
      this.id = Guid.NewGuid();
      this.name = name;
      this.serializedMinion = ref_obj;
    }

    public static MinionStorage.Info CreateEmpty()
    {
      return new MinionStorage.Info()
      {
        id = Guid.Empty,
        name = (string) null,
        serializedMinion = (Ref<KPrefabID>) null
      };
    }
  }
}
