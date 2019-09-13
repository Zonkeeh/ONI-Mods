// Decompiled with JetBrains decompiler
// Type: SeedProducer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SeedProducer : KMonoBehaviour, IGameObjectEffectDescriptor
{
  private static readonly EventSystem.IntraObjectHandler<SeedProducer> DropSeedDelegate = new EventSystem.IntraObjectHandler<SeedProducer>((System.Action<SeedProducer, object>) ((component, data) => component.DropSeed(data)));
  private static readonly EventSystem.IntraObjectHandler<SeedProducer> CropPickedDelegate = new EventSystem.IntraObjectHandler<SeedProducer>((System.Action<SeedProducer, object>) ((component, data) => component.CropPicked(data)));
  public SeedProducer.SeedInfo seedInfo;
  private bool droppedSeedAlready;

  public void Configure(
    string SeedID,
    SeedProducer.ProductionType productionType,
    int newSeedsProduced = 1)
  {
    this.seedInfo.seedId = SeedID;
    this.seedInfo.productionType = productionType;
    this.seedInfo.newSeedsProduced = newSeedsProduced;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Subscribe<SeedProducer>(-216549700, SeedProducer.DropSeedDelegate);
    this.Subscribe<SeedProducer>(1623392196, SeedProducer.DropSeedDelegate);
    this.Subscribe<SeedProducer>(-1072826864, SeedProducer.CropPickedDelegate);
  }

  public GameObject ProduceSeed(string seedId, int units = 1)
  {
    if (seedId == null || units <= 0)
      return (GameObject) null;
    Vector3 position = this.gameObject.transform.GetPosition() + new Vector3(0.0f, 0.5f, 0.0f);
    GameObject go = GameUtil.KInstantiate(Assets.GetPrefab(new Tag(seedId)), position, Grid.SceneLayer.Ore, (string) null, 0);
    PrimaryElement component1 = this.gameObject.GetComponent<PrimaryElement>();
    PrimaryElement component2 = go.GetComponent<PrimaryElement>();
    component2.Temperature = component1.Temperature;
    component2.Units = (float) units;
    this.Trigger(472291861, (object) go.GetComponent<PlantableSeed>());
    go.SetActive(true);
    PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, go.GetProperName(), go.transform, 1.5f, false);
    return go;
  }

  public void DropSeed(object data = null)
  {
    if (this.droppedSeedAlready)
      return;
    this.Trigger(-1736624145, (object) this.ProduceSeed(this.seedInfo.seedId, 1).GetComponent<PlantableSeed>());
    this.droppedSeedAlready = true;
  }

  public void CropDepleted(object data)
  {
    this.DropSeed((object) null);
  }

  public void CropPicked(object data)
  {
    if (this.seedInfo.productionType != SeedProducer.ProductionType.Harvest)
      return;
    Worker completedBy = this.GetComponent<Harvestable>().completed_by;
    float num = 10f;
    if ((UnityEngine.Object) completedBy != (UnityEngine.Object) null)
      num += completedBy.GetAttributes().Get(Db.Get().Attributes.Botanist).GetTotalValue() * Db.Get().AttributeConverters.SeedHarvestChance.multiplier;
    this.ProduceSeed(this.seedInfo.seedId, (double) UnityEngine.Random.Range(0, 100) > (double) num ? 0 : 1);
  }

  public List<Descriptor> GetDescriptors(GameObject go)
  {
    List<Descriptor> descriptorList = new List<Descriptor>();
    if (!((UnityEngine.Object) Assets.GetPrefab(new Tag(this.seedInfo.seedId)) != (UnityEngine.Object) null))
      ;
    switch (this.seedInfo.productionType)
    {
      case SeedProducer.ProductionType.DigOnly:
        return (List<Descriptor>) null;
      case SeedProducer.ProductionType.Harvest:
        descriptorList.Add(new Descriptor((string) UI.GAMEOBJECTEFFECTS.SEED_PRODUCTION_HARVEST, (string) UI.GAMEOBJECTEFFECTS.TOOLTIPS.SEED_PRODUCTION_HARVEST, Descriptor.DescriptorType.Lifecycle, true));
        break;
      case SeedProducer.ProductionType.Fruit:
        descriptorList.Add(new Descriptor((string) UI.GAMEOBJECTEFFECTS.SEED_PRODUCTION_FRUIT, (string) UI.GAMEOBJECTEFFECTS.TOOLTIPS.SEED_PRODUCTION_DIG_ONLY, Descriptor.DescriptorType.Lifecycle, true));
        break;
      default:
        return (List<Descriptor>) null;
    }
    return descriptorList;
  }

  [Serializable]
  public struct SeedInfo
  {
    public string seedId;
    public SeedProducer.ProductionType productionType;
    public int newSeedsProduced;
  }

  public enum ProductionType
  {
    Hidden,
    DigOnly,
    Harvest,
    Fruit,
  }
}
