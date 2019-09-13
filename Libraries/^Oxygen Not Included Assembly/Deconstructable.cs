// Decompiled with JetBrains decompiler
// Type: Deconstructable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

public class Deconstructable : Workable
{
  private static readonly EventSystem.IntraObjectHandler<Deconstructable> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Deconstructable>((System.Action<Deconstructable, object>) ((component, data) => component.OnRefreshUserMenu(data)));
  private static readonly EventSystem.IntraObjectHandler<Deconstructable> OnCancelDelegate = new EventSystem.IntraObjectHandler<Deconstructable>((System.Action<Deconstructable, object>) ((component, data) => component.OnCancel(data)));
  private static readonly EventSystem.IntraObjectHandler<Deconstructable> OnDeconstructDelegate = new EventSystem.IntraObjectHandler<Deconstructable>((System.Action<Deconstructable, object>) ((component, data) => component.OnDeconstruct(data)));
  private static readonly Vector2 INITIAL_VELOCITY_RANGE = new Vector2(0.5f, 4f);
  public bool allowDeconstruction = true;
  private Chore chore;
  [Serialize]
  private bool isMarkedForDeconstruction;
  [Serialize]
  public Tag[] constructionElements;
  private bool destroyed;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.faceTargetWhenWorking = true;
    this.synchronizeAnims = false;
    this.workerStatusItem = Db.Get().DuplicantStatusItems.Deconstructing;
    this.attributeConverter = Db.Get().AttributeConverters.ConstructionSpeed;
    this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
    this.minimumAttributeMultiplier = 0.75f;
    this.skillExperienceSkillGroup = Db.Get().SkillGroups.Building.Id;
    this.skillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
    this.multitoolContext = (HashedString) "build";
    this.multitoolHitEffectTag = (Tag) EffectConfigs.BuildSplashId;
    this.workingPstComplete = HashedString.Invalid;
    this.workingPstFailed = HashedString.Invalid;
    Building component = this.GetComponent<Building>();
    CellOffset[][] table = OffsetGroups.InvertedStandardTable;
    if (component.Def.IsTilePiece)
      table = OffsetGroups.InvertedStandardTableWithCorners;
    this.SetOffsetTable(OffsetGroups.BuildReachabilityTable(component.Def.PlacementOffsets, table, component.Def.ConstructionOffsetFilter));
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Subscribe<Deconstructable>(493375141, Deconstructable.OnRefreshUserMenuDelegate);
    this.Subscribe<Deconstructable>(-111137758, Deconstructable.OnRefreshUserMenuDelegate);
    this.Subscribe<Deconstructable>(2127324410, Deconstructable.OnCancelDelegate);
    this.Subscribe<Deconstructable>(-790448070, Deconstructable.OnDeconstructDelegate);
    if (this.constructionElements == null || this.constructionElements.Length == 0)
    {
      this.constructionElements = new Tag[1];
      this.constructionElements[0] = this.GetComponent<PrimaryElement>().Element.tag;
    }
    if (!this.isMarkedForDeconstruction)
      return;
    this.QueueDeconstruction();
  }

  public override float GetWorkTime()
  {
    return this.GetComponent<Building>().Def.ConstructionTime * 0.5f;
  }

  protected override void OnStartWork(Worker worker)
  {
    this.progressBar.barColor = ProgressBarsConfig.Instance.GetBarColor("DeconstructBar");
    this.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.PendingDeconstruction, false);
  }

  protected override void OnCompleteWork(Worker worker)
  {
    PrimaryElement component1 = this.GetComponent<PrimaryElement>();
    Building building = this.GetComponent<Building>();
    SimCellOccupier component2 = this.GetComponent<SimCellOccupier>();
    if ((UnityEngine.Object) DetailsScreen.Instance != (UnityEngine.Object) null && DetailsScreen.Instance.CompareTargetWith(this.gameObject))
      DetailsScreen.Instance.Show(false);
    float temperature = component1.Temperature;
    byte disease_idx = component1.DiseaseIdx;
    int disease_count = component1.DiseaseCount;
    if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
    {
      if (building.Def.TileLayer != ObjectLayer.NumLayers)
      {
        int cell = Grid.PosToCell(this.transform.GetPosition());
        if ((UnityEngine.Object) Grid.Objects[cell, (int) building.Def.TileLayer] == (UnityEngine.Object) this.gameObject)
        {
          Grid.Objects[cell, (int) building.Def.ObjectLayer] = (GameObject) null;
          Grid.Objects[cell, (int) building.Def.TileLayer] = (GameObject) null;
          Grid.Foundation[cell] = false;
          TileVisualizer.RefreshCell(cell, building.Def.TileLayer, building.Def.ReplacementLayer);
        }
      }
      component2.DestroySelf((System.Action) (() => this.TriggerDestroy(building, temperature, disease_idx, disease_count)));
    }
    else
      this.TriggerDestroy(building, temperature, disease_idx, disease_count);
    string sound = GlobalAssets.GetSound("Finish_Deconstruction_" + building.Def.AudioSize, false);
    if (sound != null)
      KMonoBehaviour.PlaySound3DAtLocation(sound, this.gameObject.transform.GetPosition());
    this.Trigger(-702296337, (object) this);
  }

  private void TriggerDestroy(
    Building building,
    float temperature,
    byte disease_idx,
    int disease_count)
  {
    if ((UnityEngine.Object) this == (UnityEngine.Object) null || this.destroyed)
      return;
    for (int index = 0; index < this.constructionElements.Length && building.Def.Mass.Length > index; ++index)
    {
      GameObject go = Deconstructable.SpawnItem(this.transform.GetPosition(), building.Def, this.constructionElements[index], building.Def.Mass[index], temperature, disease_idx, disease_count);
      go.transform.SetPosition(go.transform.GetPosition() + Vector3.up * 0.5f);
      int cell1 = Grid.PosToCell(go.transform.GetPosition());
      int cell2 = Grid.CellAbove(cell1);
      Vector2 initial_velocity = Grid.IsValidCell(cell1) && Grid.Solid[cell1] || Grid.IsValidCell(cell2) && Grid.Solid[cell2] ? Vector2.zero : new Vector2(UnityEngine.Random.Range(-1f, 1f) * Deconstructable.INITIAL_VELOCITY_RANGE.x, Deconstructable.INITIAL_VELOCITY_RANGE.y);
      if (GameComps.Fallers.Has((object) go))
        GameComps.Fallers.Remove(go);
      GameComps.Fallers.Add(go, initial_velocity);
    }
    this.destroyed = true;
    this.gameObject.DeleteObject();
  }

  private void QueueDeconstruction()
  {
    if (this.chore != null)
      return;
    if (DebugHandler.InstantBuildMode)
    {
      this.OnCompleteWork((Worker) null);
    }
    else
    {
      Prioritizable.AddRef(this.gameObject);
      this.chore = (Chore) new WorkChore<Deconstructable>(Db.Get().ChoreTypes.Deconstruct, (IStateMachineTarget) this, (ChoreProvider) null, true, (System.Action<Chore>) null, (System.Action<Chore>) null, (System.Action<Chore>) null, true, (ScheduleBlockType) null, false, false, (KAnimFile) null, true, true, true, PriorityScreen.PriorityClass.basic, 5, true, true);
      this.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.PendingDeconstruction, (object) this);
      this.isMarkedForDeconstruction = true;
      this.Trigger(2108245096, (object) "Deconstruct");
    }
  }

  private void OnDeconstruct()
  {
    if (this.chore == null)
      this.QueueDeconstruction();
    else
      this.CancelDeconstruction();
  }

  public bool IsMarkedForDeconstruction()
  {
    return this.chore != null;
  }

  public void SetAllowDeconstruction(bool allow)
  {
    this.allowDeconstruction = allow;
    if (this.allowDeconstruction)
      return;
    this.CancelDeconstruction();
  }

  public static GameObject SpawnItem(
    Vector3 position,
    BuildingDef def,
    Tag src_element,
    float src_mass,
    float src_temperature,
    byte disease_idx,
    int disease_count)
  {
    GameObject gameObject = (GameObject) null;
    int cell1 = Grid.PosToCell(position);
    CellOffset[] placementOffsets = def.PlacementOffsets;
    Element element = ElementLoader.GetElement(src_element);
    if (element != null)
    {
      float num = src_mass;
      for (int index1 = 0; (double) index1 < (double) src_mass / 400.0; ++index1)
      {
        int index2 = index1 % def.PlacementOffsets.Length;
        int cell2 = Grid.OffsetCell(cell1, placementOffsets[index2]);
        float mass = num;
        if ((double) num > 400.0)
        {
          mass = 400f;
          num -= 400f;
        }
        gameObject = element.substance.SpawnResource(Grid.CellToPosCBC(cell2, Grid.SceneLayer.Ore), mass, src_temperature, disease_idx, disease_count, false, false, false);
      }
    }
    else
    {
      for (int index1 = 0; (double) index1 < (double) src_mass; ++index1)
      {
        int index2 = index1 % def.PlacementOffsets.Length;
        int cell2 = Grid.OffsetCell(cell1, placementOffsets[index2]);
        gameObject = GameUtil.KInstantiate(Assets.GetPrefab(src_element), Grid.CellToPosCBC(cell2, Grid.SceneLayer.Ore), Grid.SceneLayer.Ore, (string) null, 0);
        gameObject.SetActive(true);
      }
    }
    return gameObject;
  }

  private void OnRefreshUserMenu(object data)
  {
    if (!this.allowDeconstruction)
      return;
    Game.Instance.userMenu.AddButton(this.gameObject, this.chore != null ? new KIconButtonMenu.ButtonInfo("action_deconstruct", (string) UI.USERMENUACTIONS.DEMOLISH.NAME_OFF, new System.Action(this.OnDeconstruct), Action.NumActions, (System.Action<GameObject>) null, (System.Action<KIconButtonMenu.ButtonInfo>) null, (Texture) null, (string) UI.USERMENUACTIONS.DEMOLISH.TOOLTIP_OFF, true) : new KIconButtonMenu.ButtonInfo("action_deconstruct", (string) UI.USERMENUACTIONS.DEMOLISH.NAME, new System.Action(this.OnDeconstruct), Action.NumActions, (System.Action<GameObject>) null, (System.Action<KIconButtonMenu.ButtonInfo>) null, (Texture) null, (string) UI.USERMENUACTIONS.DEMOLISH.TOOLTIP, true), 1f);
  }

  private void CancelDeconstruction()
  {
    if (this.chore == null)
      return;
    this.chore.Cancel("Cancelled deconstruction");
    this.chore = (Chore) null;
    this.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.PendingDeconstruction, false);
    this.ShowProgressBar(false);
    this.isMarkedForDeconstruction = false;
    Prioritizable.RemoveRef(this.gameObject);
  }

  private void OnCancel(object data)
  {
    this.CancelDeconstruction();
  }

  private void OnDeconstruct(object data)
  {
    if (!this.allowDeconstruction)
      return;
    this.QueueDeconstruction();
  }
}
