// Decompiled with JetBrains decompiler
// Type: PlantableSeed
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;

public class PlantableSeed : KMonoBehaviour, IReceptacleDirection, IGameObjectEffectDescriptor
{
  private static readonly EventSystem.IntraObjectHandler<PlantableSeed> OnAbsorbDelegate = new EventSystem.IntraObjectHandler<PlantableSeed>((System.Action<PlantableSeed, object>) ((component, data) => component.OnAbsorb(data)));
  private static readonly EventSystem.IntraObjectHandler<PlantableSeed> OnSplitDelegate = new EventSystem.IntraObjectHandler<PlantableSeed>((System.Action<PlantableSeed, object>) ((component, data) => component.OnSplit(data)));
  public Tag PlantID;
  public Tag PreviewID;
  [Serialize]
  public float timeUntilSelfPlant;
  public Tag replantGroundTag;
  public string domesticatedDescription;
  public SingleEntityReceptacle.ReceptacleDirection direction;

  public SingleEntityReceptacle.ReceptacleDirection Direction
  {
    get
    {
      return this.direction;
    }
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<PlantableSeed>(-2064133523, PlantableSeed.OnAbsorbDelegate);
    this.Subscribe<PlantableSeed>(1335436905, PlantableSeed.OnSplitDelegate);
    this.timeUntilSelfPlant = Util.RandomVariance(2400f, 600f);
  }

  private void OnAbsorb(object data)
  {
  }

  private void OnSplit(object data)
  {
  }

  public void TryPlant(bool allow_plant_from_storage = false)
  {
    this.timeUntilSelfPlant = Util.RandomVariance(2400f, 600f);
    if (!allow_plant_from_storage && this.gameObject.HasTag(GameTags.Stored))
      return;
    int cell = Grid.PosToCell(this.gameObject);
    if (!this.TestSuitableGround(cell))
      return;
    GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(this.PlantID), Grid.CellToPosCBC(cell, Grid.SceneLayer.BuildingFront), Grid.SceneLayer.BuildingFront, (string) null, 0);
    gameObject.SetActive(true);
    Pickupable pickupable = this.GetComponent<Pickupable>().Take(1f);
    if ((UnityEngine.Object) pickupable != (UnityEngine.Object) null)
    {
      if (!((UnityEngine.Object) gameObject.GetComponent<Crop>() != (UnityEngine.Object) null))
        ;
      Util.KDestroyGameObject(pickupable.gameObject);
    }
    else
      KCrashReporter.Assert(false, "Seed has fractional total amount < 1f");
  }

  public bool TestSuitableGround(int cell)
  {
    if (!Grid.IsValidCell(cell))
      return false;
    int cell1 = this.Direction != SingleEntityReceptacle.ReceptacleDirection.Bottom ? Grid.CellBelow(cell) : Grid.CellAbove(cell);
    if (!Grid.IsValidCell(cell1) || Grid.Foundation[cell1] || Grid.Element[cell1].hardness >= (byte) 150 || this.replantGroundTag.IsValid && !Grid.Element[cell1].HasTag(this.replantGroundTag))
      return false;
    GameObject prefab = Assets.GetPrefab(this.PlantID);
    EntombVulnerable component1 = prefab.GetComponent<EntombVulnerable>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null && !component1.IsCellSafe(cell))
      return false;
    DrowningMonitor component2 = prefab.GetComponent<DrowningMonitor>();
    if ((UnityEngine.Object) component2 != (UnityEngine.Object) null && !component2.IsCellSafe(cell))
      return false;
    TemperatureVulnerable component3 = prefab.GetComponent<TemperatureVulnerable>();
    if ((UnityEngine.Object) component3 != (UnityEngine.Object) null && !component3.IsCellSafe(cell))
      return false;
    UprootedMonitor component4 = prefab.GetComponent<UprootedMonitor>();
    if ((UnityEngine.Object) component4 != (UnityEngine.Object) null && !component4.IsCellSafe(cell))
      return false;
    OccupyArea component5 = prefab.GetComponent<OccupyArea>();
    return !((UnityEngine.Object) component5 != (UnityEngine.Object) null) || component5.CanOccupyArea(cell, ObjectLayer.Building);
  }

  public List<Descriptor> GetDescriptors(GameObject go)
  {
    List<Descriptor> descriptorList = new List<Descriptor>();
    if (this.direction == SingleEntityReceptacle.ReceptacleDirection.Bottom)
    {
      Descriptor descriptor = new Descriptor((string) UI.GAMEOBJECTEFFECTS.SEED_REQUIREMENT_CEILING, (string) UI.GAMEOBJECTEFFECTS.TOOLTIPS.SEED_REQUIREMENT_CEILING, Descriptor.DescriptorType.Requirement, false);
      descriptorList.Add(descriptor);
    }
    else if (this.direction == SingleEntityReceptacle.ReceptacleDirection.Side)
    {
      Descriptor descriptor = new Descriptor((string) UI.GAMEOBJECTEFFECTS.SEED_REQUIREMENT_WALL, (string) UI.GAMEOBJECTEFFECTS.TOOLTIPS.SEED_REQUIREMENT_WALL, Descriptor.DescriptorType.Requirement, false);
      descriptorList.Add(descriptor);
    }
    return descriptorList;
  }
}
