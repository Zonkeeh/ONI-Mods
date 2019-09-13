// Decompiled with JetBrains decompiler
// Type: Components
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;

public class Components
{
  public static Components.Cmps<MinionIdentity> LiveMinionIdentities = new Components.Cmps<MinionIdentity>();
  public static Components.Cmps<MinionIdentity> MinionIdentities = new Components.Cmps<MinionIdentity>();
  public static Components.Cmps<StoredMinionIdentity> StoredMinionIdentities = new Components.Cmps<StoredMinionIdentity>();
  public static Components.Cmps<MinionStorage> MinionStorages = new Components.Cmps<MinionStorage>();
  public static Components.Cmps<MinionResume> MinionResumes = new Components.Cmps<MinionResume>();
  public static Components.Cmps<Sleepable> Sleepables = new Components.Cmps<Sleepable>();
  public static Components.Cmps<IUsable> Toilets = new Components.Cmps<IUsable>();
  public static Components.Cmps<Pickupable> Pickupables = new Components.Cmps<Pickupable>();
  public static Components.Cmps<Brain> Brains = new Components.Cmps<Brain>();
  public static Components.Cmps<BuildingComplete> BuildingCompletes = new Components.Cmps<BuildingComplete>();
  public static Components.Cmps<Notifier> Notifiers = new Components.Cmps<Notifier>();
  public static Components.Cmps<Fabricator> Fabricators = new Components.Cmps<Fabricator>();
  public static Components.Cmps<Refinery> Refineries = new Components.Cmps<Refinery>();
  public static Components.Cmps<PlantablePlot> PlantablePlots = new Components.Cmps<PlantablePlot>();
  public static Components.Cmps<Ladder> Ladders = new Components.Cmps<Ladder>();
  public static Components.Cmps<ITravelTubePiece> ITravelTubePieces = new Components.Cmps<ITravelTubePiece>();
  public static Components.Cmps<CreatureFeeder> CreatureFeeders = new Components.Cmps<CreatureFeeder>();
  public static Components.Cmps<Light2D> Light2Ds = new Components.Cmps<Light2D>();
  public static Components.Cmps<Radiator> Radiators = new Components.Cmps<Radiator>();
  public static Components.Cmps<Edible> Edibles = new Components.Cmps<Edible>();
  public static Components.Cmps<Diggable> Diggables = new Components.Cmps<Diggable>();
  public static Components.Cmps<ResearchCenter> ResearchCenters = new Components.Cmps<ResearchCenter>();
  public static Components.Cmps<Harvestable> Harvestables = new Components.Cmps<Harvestable>();
  public static Components.Cmps<HarvestDesignatable> HarvestDesignatables = new Components.Cmps<HarvestDesignatable>();
  public static Components.Cmps<Uprootable> Uprootables = new Components.Cmps<Uprootable>();
  public static Components.Cmps<global::Health> Health = new Components.Cmps<global::Health>();
  public static Components.Cmps<global::Equipment> Equipment = new Components.Cmps<global::Equipment>();
  public static Components.Cmps<FactionAlignment> FactionAlignments = new Components.Cmps<FactionAlignment>();
  public static Components.Cmps<Telepad> Telepads = new Components.Cmps<Telepad>();
  public static Components.Cmps<Generator> Generators = new Components.Cmps<Generator>();
  public static Components.Cmps<EnergyConsumer> EnergyConsumers = new Components.Cmps<EnergyConsumer>();
  public static Components.Cmps<Battery> Batteries = new Components.Cmps<Battery>();
  public static Components.Cmps<Breakable> Breakables = new Components.Cmps<Breakable>();
  public static Components.Cmps<Crop> Crops = new Components.Cmps<Crop>();
  public static Components.Cmps<Prioritizable> Prioritizables = new Components.Cmps<Prioritizable>();
  public static Components.Cmps<Clinic> Clinics = new Components.Cmps<Clinic>();
  public static Components.Cmps<HandSanitizer> HandSanitizers = new Components.Cmps<HandSanitizer>();
  public static Components.Cmps<BuildingCellVisualizer> BuildingCellVisualizers = new Components.Cmps<BuildingCellVisualizer>();
  public static Components.Cmps<RoleStation> RoleStations = new Components.Cmps<RoleStation>();
  public static Components.Cmps<Telescope> Telescopes = new Components.Cmps<Telescope>();
  public static Components.Cmps<Capturable> Capturables = new Components.Cmps<Capturable>();
  public static Components.Cmps<NotCapturable> NotCapturables = new Components.Cmps<NotCapturable>();
  public static Components.Cmps<DiseaseSourceVisualizer> DiseaseSourceVisualizers = new Components.Cmps<DiseaseSourceVisualizer>();
  public static Components.Cmps<DetectorNetwork.Instance> DetectorNetworks = new Components.Cmps<DetectorNetwork.Instance>();
  public static Components.Cmps<Grave> Graves = new Components.Cmps<Grave>();
  public static Components.Cmps<AttachableBuilding> AttachableBuildings = new Components.Cmps<AttachableBuilding>();
  public static Components.Cmps<BuildingAttachPoint> BuildingAttachPoints = new Components.Cmps<BuildingAttachPoint>();
  public static Components.Cmps<global::MinionAssignablesProxy> MinionAssignablesProxy = new Components.Cmps<global::MinionAssignablesProxy>();
  public static Components.Cmps<ComplexFabricator> ComplexFabricators = new Components.Cmps<ComplexFabricator>();
  public static Components.Cmps<MonumentPart> MonumentParts = new Components.Cmps<MonumentPart>();

  public class Cmps<T> : ICollection, IEnumerable
  {
    private Dictionary<T, HandleVector<int>.Handle> table;
    private KCompactedVector<T> items;

    public Cmps()
    {
      App.OnPreLoadScene += new System.Action(this.Clear);
      this.items = new KCompactedVector<T>(0);
      this.table = new Dictionary<T, HandleVector<int>.Handle>();
    }

    public List<T> Items
    {
      get
      {
        return this.items.GetDataList();
      }
    }

    public int Count
    {
      get
      {
        return this.items.Count;
      }
    }

    public T this[int idx]
    {
      get
      {
        return this.Items[idx];
      }
    }

    private void Clear()
    {
      this.items.Clear();
      this.table.Clear();
      this.OnAdd = (System.Action<T>) null;
      this.OnRemove = (System.Action<T>) null;
    }

    public void Add(T cmp)
    {
      HandleVector<int>.Handle handle = this.items.Allocate(cmp);
      this.table[cmp] = handle;
      if (this.OnAdd == null)
        return;
      this.OnAdd(cmp);
    }

    public void Remove(T cmp)
    {
      HandleVector<int>.Handle invalidHandle = HandleVector<int>.InvalidHandle;
      if (!this.table.TryGetValue(cmp, out invalidHandle))
        return;
      this.table.Remove(cmp);
      this.items.Free(invalidHandle);
      if (this.OnRemove == null)
        return;
      this.OnRemove(cmp);
    }

    public void Register(System.Action<T> on_add, System.Action<T> on_remove)
    {
      this.OnAdd += on_add;
      this.OnRemove += on_remove;
      foreach (T obj in this.Items)
        this.OnAdd(obj);
    }

    public void Unregister(System.Action<T> on_add, System.Action<T> on_remove)
    {
      this.OnAdd -= on_add;
      this.OnRemove -= on_remove;
    }

    public event System.Action<T> OnAdd;

    public event System.Action<T> OnRemove;

    public bool IsSynchronized
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public object SyncRoot
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    public void CopyTo(Array array, int index)
    {
      throw new NotImplementedException();
    }

    public IEnumerator GetEnumerator()
    {
      return this.items.GetEnumerator();
    }
  }
}
