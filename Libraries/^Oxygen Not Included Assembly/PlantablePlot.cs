// Decompiled with JetBrains decompiler
// Type: PlantablePlot
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class PlantablePlot : SingleEntityReceptacle, ISaveLoadable, IEffectDescriptor, IGameObjectEffectDescriptor
{
  private static readonly EventSystem.IntraObjectHandler<PlantablePlot> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<PlantablePlot>((System.Action<PlantablePlot, object>) ((component, data) => component.OnCopySettings(data)));
  private static readonly EventSystem.IntraObjectHandler<PlantablePlot> OnUpdateRoomDelegate = new EventSystem.IntraObjectHandler<PlantablePlot>((System.Action<PlantablePlot, object>) ((component, data) =>
  {
    if (!((UnityEngine.Object) component.plantRef.Get() != (UnityEngine.Object) null))
      return;
    component.plantRef.Get().Trigger(144050788, data);
  }));
  public Vector3 occupyingObjectVisualOffset = Vector3.zero;
  public Grid.SceneLayer plantLayer = Grid.SceneLayer.BuildingBack;
  [SerializeField]
  private bool accepts_irrigation = true;
  [MyCmpAdd]
  private CopyBuildingSettings copyBuildingSettings;
  [Serialize]
  private Ref<KPrefabID> plantRef;
  private EntityPreview plantPreview;
  [SerializeField]
  private bool accepts_fertilizer;
  [SerializeField]
  public bool has_liquid_pipe_input;

  public KPrefabID plant
  {
    get
    {
      return this.plantRef.Get();
    }
    set
    {
      this.plantRef.Set(value);
    }
  }

  public bool ValidPlant
  {
    get
    {
      if (!((UnityEngine.Object) this.plantPreview == (UnityEngine.Object) null))
        return this.plantPreview.Valid;
      return true;
    }
  }

  public bool AcceptsFertilizer
  {
    get
    {
      return this.accepts_fertilizer;
    }
  }

  public bool AcceptsIrrigation
  {
    get
    {
      return this.accepts_irrigation;
    }
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.statusItemNeed = Db.Get().BuildingStatusItems.NeedSeed;
    this.statusItemNoneAvailable = Db.Get().BuildingStatusItems.NoAvailableSeed;
    this.statusItemAwaitingDelivery = Db.Get().BuildingStatusItems.AwaitingSeedDelivery;
    this.plantRef = new Ref<KPrefabID>();
    this.destroyEntityOnDeposit = true;
    this.Subscribe<PlantablePlot>(-905833192, PlantablePlot.OnCopySettingsDelegate);
    this.Subscribe<PlantablePlot>(144050788, PlantablePlot.OnUpdateRoomDelegate);
  }

  private void OnCopySettings(object data)
  {
    PlantablePlot component1 = ((GameObject) data).GetComponent<PlantablePlot>();
    if (!((UnityEngine.Object) component1 != (UnityEngine.Object) null))
      return;
    if ((UnityEngine.Object) this.occupyingObject == (UnityEngine.Object) null && (this.requestedEntityTag != component1.requestedEntityTag || (UnityEngine.Object) component1.occupyingObject != (UnityEngine.Object) null))
    {
      Tag requestedEntityTag = component1.requestedEntityTag;
      if ((UnityEngine.Object) component1.occupyingObject != (UnityEngine.Object) null)
      {
        SeedProducer component2 = component1.occupyingObject.GetComponent<SeedProducer>();
        if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
          requestedEntityTag = TagManager.Create(component2.seedInfo.seedId);
      }
      this.CancelActiveRequest();
      this.CreateOrder(requestedEntityTag);
    }
    if (!((UnityEngine.Object) this.occupyingObject != (UnityEngine.Object) null))
      return;
    Prioritizable component3 = this.GetComponent<Prioritizable>();
    if (!((UnityEngine.Object) component3 != (UnityEngine.Object) null))
      return;
    Prioritizable component4 = this.occupyingObject.GetComponent<Prioritizable>();
    if (!((UnityEngine.Object) component4 != (UnityEngine.Object) null))
      return;
    component4.SetMasterPriority(component3.GetMasterPriority());
  }

  public override void CreateOrder(Tag entityTag)
  {
    this.SetPreview(entityTag, false);
    if (this.ValidPlant)
      base.CreateOrder(entityTag);
    else
      this.SetPreview(Tag.Invalid, false);
  }

  private void SyncPriority(PrioritySetting priority)
  {
    Prioritizable component1 = this.GetComponent<Prioritizable>();
    if (!object.Equals((object) component1.GetMasterPriority(), (object) priority))
      component1.SetMasterPriority(priority);
    if (!((UnityEngine.Object) this.occupyingObject != (UnityEngine.Object) null))
      return;
    Prioritizable component2 = this.occupyingObject.GetComponent<Prioritizable>();
    if (!((UnityEngine.Object) component2 != (UnityEngine.Object) null) || object.Equals((object) component2.GetMasterPriority(), (object) priority))
      return;
    component2.SetMasterPriority(component1.GetMasterPriority());
  }

  protected override void OnSpawn()
  {
    if ((UnityEngine.Object) this.plant != (UnityEngine.Object) null)
      this.RegisterWithPlant(this.plant.gameObject);
    base.OnSpawn();
    this.autoReplaceEntity = false;
    Components.PlantablePlots.Add(this);
    this.GetComponent<Prioritizable>().onPriorityChanged += new System.Action<PrioritySetting>(this.SyncPriority);
  }

  public void SetFertilizationFlags(bool fertilizer, bool liquid_piping)
  {
    this.accepts_fertilizer = fertilizer;
    this.has_liquid_pipe_input = liquid_piping;
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    if ((UnityEngine.Object) this.plantPreview != (UnityEngine.Object) null)
      Util.KDestroyGameObject(this.plantPreview.gameObject);
    if ((bool) ((UnityEngine.Object) this.occupyingObject))
      this.occupyingObject.Trigger(-216549700, (object) null);
    Components.PlantablePlots.Remove(this);
  }

  public override GameObject SpawnOccupyingObject(GameObject depositedEntity)
  {
    PlantableSeed component1 = depositedEntity.GetComponent<PlantableSeed>();
    if ((UnityEngine.Object) component1 == (UnityEngine.Object) null)
    {
      Debug.LogError((object) ("Planted seed " + depositedEntity.gameObject.name + " is missing PlantableSeed component"));
      return (GameObject) null;
    }
    Vector3 posCbc = Grid.CellToPosCBC(Grid.PosToCell((KMonoBehaviour) this), this.plantLayer);
    GameObject plant = GameUtil.KInstantiate(Assets.GetPrefab(component1.PlantID), posCbc, this.plantLayer, (string) null, 0);
    plant.SetActive(true);
    this.plantRef.Set(plant.GetComponent<KPrefabID>());
    this.RegisterWithPlant(plant);
    UprootedMonitor component2 = plant.GetComponent<UprootedMonitor>();
    if ((bool) ((UnityEngine.Object) component2))
      component2.canBeUprooted = false;
    this.autoReplaceEntity = false;
    Prioritizable component3 = this.GetComponent<Prioritizable>();
    if ((UnityEngine.Object) component3 != (UnityEngine.Object) null)
    {
      Prioritizable component4 = plant.GetComponent<Prioritizable>();
      if ((UnityEngine.Object) component4 != (UnityEngine.Object) null)
      {
        component4.SetMasterPriority(component3.GetMasterPriority());
        component4.onPriorityChanged += new System.Action<PrioritySetting>(this.SyncPriority);
      }
    }
    return plant;
  }

  protected override void PositionOccupyingObject()
  {
    base.PositionOccupyingObject();
    KBatchedAnimController component = this.occupyingObject.GetComponent<KBatchedAnimController>();
    component.SetSceneLayer(this.plantLayer);
    this.OffsetAnim(component, this.occupyingObjectVisualOffset);
  }

  private void RegisterWithPlant(GameObject plant)
  {
    this.occupyingObject = plant;
    ReceptacleMonitor component = plant.GetComponent<ReceptacleMonitor>();
    if ((bool) ((UnityEngine.Object) component))
      component.SetReceptacle(this);
    plant.Trigger(1309017699, (object) this.storage);
  }

  protected override void SubscribeToOccupant()
  {
    base.SubscribeToOccupant();
    if (!((UnityEngine.Object) this.occupyingObject != (UnityEngine.Object) null))
      return;
    this.Subscribe(this.occupyingObject, -216549700, new System.Action<object>(this.OnOccupantUprooted));
  }

  protected override void UnsubscribeFromOccupant()
  {
    base.UnsubscribeFromOccupant();
    if (!((UnityEngine.Object) this.occupyingObject != (UnityEngine.Object) null))
      return;
    this.Unsubscribe(this.occupyingObject, -216549700, new System.Action<object>(this.OnOccupantUprooted));
  }

  private void OnOccupantUprooted(object data)
  {
    this.autoReplaceEntity = false;
    this.requestedEntityTag = Tag.Invalid;
  }

  public override void OrderRemoveOccupant()
  {
    if ((UnityEngine.Object) this.Occupant == (UnityEngine.Object) null)
      return;
    Uprootable component = this.Occupant.GetComponent<Uprootable>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
      return;
    component.MarkForUproot(true);
  }

  public override void SetPreview(Tag entityTag, bool solid = false)
  {
    PlantableSeed plantableSeed = (PlantableSeed) null;
    if (entityTag.IsValid)
    {
      GameObject prefab = Assets.GetPrefab(entityTag);
      if ((UnityEngine.Object) prefab == (UnityEngine.Object) null)
      {
        DebugUtil.LogWarningArgs((UnityEngine.Object) this.gameObject, (object) "Planter tried previewing a tag with no asset! If this was the 'Empty' tag, ignore it, that will go away in new save games. Otherwise... Eh? Tag was: ", (object) entityTag);
        return;
      }
      plantableSeed = prefab.GetComponent<PlantableSeed>();
    }
    if ((UnityEngine.Object) this.plantPreview != (UnityEngine.Object) null)
    {
      KPrefabID component = this.plantPreview.GetComponent<KPrefabID>();
      if ((UnityEngine.Object) plantableSeed != (UnityEngine.Object) null && (UnityEngine.Object) component != (UnityEngine.Object) null && component.PrefabTag == plantableSeed.PreviewID)
        return;
      this.plantPreview.gameObject.Unsubscribe(-1820564715, new System.Action<object>(this.OnValidChanged));
      Util.KDestroyGameObject(this.plantPreview.gameObject);
    }
    if (!((UnityEngine.Object) plantableSeed != (UnityEngine.Object) null))
      return;
    GameObject go = GameUtil.KInstantiate(Assets.GetPrefab(plantableSeed.PreviewID), Grid.SceneLayer.Front, (string) null, 0);
    this.plantPreview = go.GetComponent<EntityPreview>();
    go.transform.SetPosition(Vector3.zero);
    go.transform.SetParent(this.gameObject.transform, false);
    go.transform.SetLocalPosition(Vector3.zero);
    if ((UnityEngine.Object) this.rotatable != (UnityEngine.Object) null)
    {
      if (plantableSeed.direction == SingleEntityReceptacle.ReceptacleDirection.Top)
        go.transform.SetLocalPosition(this.occupyingObjectRelativePosition);
      else if (plantableSeed.direction == SingleEntityReceptacle.ReceptacleDirection.Side)
        go.transform.SetLocalPosition(Rotatable.GetRotatedOffset(this.occupyingObjectRelativePosition, Orientation.R90));
      else
        go.transform.SetLocalPosition(Rotatable.GetRotatedOffset(this.occupyingObjectRelativePosition, Orientation.R180));
    }
    else
      go.transform.SetLocalPosition(this.occupyingObjectRelativePosition);
    this.OffsetAnim(go.GetComponent<KBatchedAnimController>(), this.occupyingObjectVisualOffset);
    go.SetActive(true);
    go.Subscribe(-1820564715, new System.Action<object>(this.OnValidChanged));
    if (solid)
      this.plantPreview.SetSolid();
    this.plantPreview.UpdateValidity();
  }

  private void OffsetAnim(KBatchedAnimController kanim, Vector3 offset)
  {
    if ((UnityEngine.Object) this.rotatable != (UnityEngine.Object) null)
      offset = this.rotatable.GetRotatedOffset(offset);
    kanim.Offset = offset;
  }

  private void OnValidChanged(object obj)
  {
    this.Trigger(-1820564715, obj);
    if (this.plantPreview.Valid || this.GetActiveRequest == null)
      return;
    this.CancelActiveRequest();
  }

  public List<Descriptor> GetDescriptors(BuildingDef def)
  {
    return this.GetDescriptors(def.BuildingComplete);
  }

  public override List<Descriptor> GetDescriptors(GameObject go)
  {
    List<Descriptor> descriptorList = new List<Descriptor>();
    Descriptor descriptor = new Descriptor();
    descriptor.SetupDescriptor((string) UI.BUILDINGEFFECTS.ENABLESDOMESTICGROWTH, (string) UI.BUILDINGEFFECTS.TOOLTIPS.ENABLESDOMESTICGROWTH, Descriptor.DescriptorType.Effect);
    descriptorList.Add(descriptor);
    return descriptorList;
  }
}
