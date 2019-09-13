// Decompiled with JetBrains decompiler
// Type: RocketModule
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class RocketModule : KMonoBehaviour
{
  private static readonly EventSystem.IntraObjectHandler<RocketModule> OnLaunchDelegate = new EventSystem.IntraObjectHandler<RocketModule>((System.Action<RocketModule, object>) ((component, data) => component.OnLaunch(data)));
  private static readonly EventSystem.IntraObjectHandler<RocketModule> OnLandDelegate = new EventSystem.IntraObjectHandler<RocketModule>((System.Action<RocketModule, object>) ((component, data) => component.OnLand(data)));
  private static readonly EventSystem.IntraObjectHandler<RocketModule> DEBUG_OnDestroyDelegate = new EventSystem.IntraObjectHandler<RocketModule>((System.Action<RocketModule, object>) ((component, data) => component.DEBUG_OnDestroy(data)));
  public List<RocketLaunchCondition> launchConditions = new List<RocketLaunchCondition>();
  public List<RocketFlightCondition> flightConditions = new List<RocketFlightCondition>();
  private string rocket_module_bg_base_string = "{0}{1}";
  private string rocket_module_bg_affix = "BG";
  private string rocket_module_bg_anim = "on";
  protected string parentRocketName = (string) UI.STARMAP.DEFAULT_NAME;
  protected bool isSuspended;
  public LaunchConditionManager conditionManager;
  [SerializeField]
  private KAnimFile bgAnimFile;

  public RocketLaunchCondition AddLaunchCondition(
    RocketLaunchCondition condition)
  {
    if (!this.launchConditions.Contains(condition))
      this.launchConditions.Add(condition);
    return condition;
  }

  public RocketFlightCondition AddFlightCondition(
    RocketFlightCondition condition)
  {
    if (!this.flightConditions.Contains(condition))
      this.flightConditions.Add(condition);
    return condition;
  }

  public void SetBGKAnim(KAnimFile anim_file)
  {
    this.bgAnimFile = anim_file;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.conditionManager = this.FindLaunchConditionManager();
    Spacecraft conditionManager = SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this.conditionManager);
    if (conditionManager != null)
      this.SetParentRocketName(conditionManager.GetRocketName());
    this.RegisterWithConditionManager();
    KSelectable component = this.GetComponent<KSelectable>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      component.AddStatusItem(Db.Get().BuildingStatusItems.RocketName, (object) this);
    if ((UnityEngine.Object) this.conditionManager != (UnityEngine.Object) null && this.conditionManager.GetComponent<KPrefabID>().HasTag(GameTags.RocketNotOnGround))
      this.OnLaunch((object) null);
    this.Subscribe<RocketModule>(-1056989049, RocketModule.OnLaunchDelegate);
    this.Subscribe<RocketModule>(238242047, RocketModule.OnLandDelegate);
    this.Subscribe<RocketModule>(1502190696, RocketModule.DEBUG_OnDestroyDelegate);
    this.FixSorting();
    this.GetComponent<AttachableBuilding>().onAttachmentNetworkChanged += new System.Action<AttachableBuilding>(this.OnAttachmentNetworkChanged);
    if (!((UnityEngine.Object) this.bgAnimFile != (UnityEngine.Object) null))
      return;
    this.AddBGGantry();
  }

  public void FixSorting()
  {
    int num = 0;
    AttachableBuilding component1 = this.GetComponent<AttachableBuilding>();
    while ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
    {
      BuildingAttachPoint attachedTo = component1.GetAttachedTo();
      if ((UnityEngine.Object) attachedTo != (UnityEngine.Object) null)
      {
        component1 = attachedTo.GetComponent<AttachableBuilding>();
        ++num;
      }
      else
        break;
    }
    Vector3 localPosition = this.transform.GetLocalPosition();
    localPosition.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingFront) - (float) num * 0.01f;
    this.transform.SetLocalPosition(localPosition);
    KBatchedAnimController component2 = this.GetComponent<KBatchedAnimController>();
    component2.enabled = false;
    component2.enabled = true;
  }

  private void OnAttachmentNetworkChanged(AttachableBuilding ab)
  {
    this.FixSorting();
  }

  private void AddBGGantry()
  {
    KAnimControllerBase component = this.GetComponent<KAnimControllerBase>();
    GameObject go = new GameObject();
    go.name = string.Format(this.rocket_module_bg_base_string, (object) this.name, (object) this.rocket_module_bg_affix);
    go.SetActive(false);
    Vector3 position = component.transform.GetPosition();
    position.z = Grid.GetLayerZ(Grid.SceneLayer.InteriorWall);
    go.transform.SetPosition(position);
    go.transform.parent = this.transform;
    KBatchedAnimController kbatchedAnimController = go.AddOrGet<KBatchedAnimController>();
    kbatchedAnimController.AnimFiles = new KAnimFile[1]
    {
      this.bgAnimFile
    };
    kbatchedAnimController.initialAnim = this.rocket_module_bg_anim;
    kbatchedAnimController.fgLayer = Grid.SceneLayer.NoLayer;
    kbatchedAnimController.initialMode = KAnim.PlayMode.Paused;
    kbatchedAnimController.FlipX = component.FlipX;
    kbatchedAnimController.FlipY = component.FlipY;
    go.SetActive(true);
  }

  private void DEBUG_OnDestroy(object data)
  {
    if (!((UnityEngine.Object) this.conditionManager != (UnityEngine.Object) null) || App.IsExiting || KMonoBehaviour.isLoadingScene)
      return;
    this.conditionManager.DEBUG_TraceModuleDestruction(this.name, SpacecraftManager.instance.GetSpacecraftFromLaunchConditionManager(this.conditionManager).state.ToString(), new StackTrace(true).ToString());
  }

  public void OnConditionManagerTagsChanged(object data)
  {
    if (!this.conditionManager.GetComponent<KPrefabID>().HasTag(GameTags.RocketNotOnGround))
      return;
    this.OnLaunch((object) null);
  }

  private void OnLaunch(object data)
  {
    KSelectable component1 = this.GetComponent<KSelectable>();
    component1.IsSelectable = false;
    if ((UnityEngine.Object) SelectTool.Instance.selected == (UnityEngine.Object) component1)
      SelectTool.Instance.Select((KSelectable) null, false);
    ConduitConsumer component2 = this.GetComponent<ConduitConsumer>();
    if ((bool) ((UnityEngine.Object) component2))
    {
      switch (component2.conduitType)
      {
        case ConduitType.Gas:
        case ConduitType.Liquid:
          component2.consumptionRate = 0.0f;
          break;
      }
    }
    Deconstructable component3 = this.GetComponent<Deconstructable>();
    if ((UnityEngine.Object) component3 != (UnityEngine.Object) null)
      component3.SetAllowDeconstruction(false);
    HandleVector<int>.Handle handle = GameComps.StructureTemperatures.GetHandle(this.gameObject);
    if (handle.IsValid())
      GameComps.StructureTemperatures.Disable(handle);
    this.ToggleComponent(typeof (ManualDeliveryKG), false);
    this.ToggleComponent(typeof (ElementConsumer), false);
    this.ToggleComponent(typeof (ElementConverter), false);
    this.ToggleComponent(typeof (ConduitDispenser), false);
    this.ToggleComponent(typeof (SolidConduitDispenser), false);
    this.ToggleComponent(typeof (EnergyConsumer), false);
  }

  private void OnLand(object data)
  {
    this.GetComponent<KSelectable>().IsSelectable = true;
    ConduitConsumer component1 = this.GetComponent<ConduitConsumer>();
    if ((bool) ((UnityEngine.Object) component1))
    {
      switch (component1.conduitType)
      {
        case ConduitType.Gas:
          this.GetComponent<ConduitConsumer>().consumptionRate = 1f;
          break;
        case ConduitType.Liquid:
          this.GetComponent<ConduitConsumer>().consumptionRate = 10f;
          break;
      }
    }
    Deconstructable component2 = this.GetComponent<Deconstructable>();
    if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
      component2.SetAllowDeconstruction(true);
    HandleVector<int>.Handle handle = GameComps.StructureTemperatures.GetHandle(this.gameObject);
    if (handle.IsValid())
      GameComps.StructureTemperatures.Enable(handle);
    this.ToggleComponent(typeof (ManualDeliveryKG), true);
    this.ToggleComponent(typeof (ElementConsumer), true);
    this.ToggleComponent(typeof (ElementConverter), true);
    this.ToggleComponent(typeof (ConduitDispenser), true);
    this.ToggleComponent(typeof (SolidConduitDispenser), true);
    this.ToggleComponent(typeof (EnergyConsumer), true);
  }

  private void ToggleComponent(System.Type cmpType, bool enabled)
  {
    MonoBehaviour component = (MonoBehaviour) this.GetComponent(cmpType);
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.enabled = enabled;
  }

  public void RegisterWithConditionManager()
  {
    if ((UnityEngine.Object) this.conditionManager != (UnityEngine.Object) null)
      this.conditionManager.RegisterRocketModule(this);
    else
      Debug.LogWarning((object) "Module conditionManager is null");
  }

  protected override void OnCleanUp()
  {
    if ((UnityEngine.Object) this.conditionManager != (UnityEngine.Object) null)
      this.conditionManager.UnregisterRocketModule(this);
    base.OnCleanUp();
  }

  public virtual void OnSuspend(object data)
  {
    this.isSuspended = true;
  }

  public bool IsSuspended()
  {
    return this.isSuspended;
  }

  public LaunchConditionManager FindLaunchConditionManager()
  {
    foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this.GetComponent<AttachableBuilding>()))
    {
      LaunchConditionManager component = gameObject.GetComponent<LaunchConditionManager>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        return component;
    }
    return (LaunchConditionManager) null;
  }

  public void SetParentRocketName(string newName)
  {
    this.parentRocketName = newName;
    NameDisplayScreen.Instance.UpdateName(this.gameObject);
  }

  public string GetParentRocketName()
  {
    return this.parentRocketName;
  }
}
