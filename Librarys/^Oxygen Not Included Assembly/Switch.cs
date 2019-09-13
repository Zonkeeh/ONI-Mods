// Decompiled with JetBrains decompiler
// Type: Switch
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class Switch : KMonoBehaviour, ISaveLoadable, IToggleHandler
{
  private static readonly EventSystem.IntraObjectHandler<Switch> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Switch>((System.Action<Switch, object>) ((component, data) => component.OnRefreshUserMenu(data)));
  [SerializeField]
  public bool manuallyControlled = true;
  [SerializeField]
  public bool defaultState = true;
  [Serialize]
  protected bool switchedOn = true;
  [MyCmpAdd]
  private Toggleable openSwitch;
  private int openToggleIndex;

  public bool IsSwitchedOn
  {
    get
    {
      return this.switchedOn;
    }
  }

  public event System.Action<bool> OnToggle;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.switchedOn = this.defaultState;
  }

  protected override void OnSpawn()
  {
    this.openToggleIndex = this.openSwitch.SetTarget((IToggleHandler) this);
    if (this.OnToggle != null)
      this.OnToggle(this.switchedOn);
    if (this.manuallyControlled)
      this.Subscribe<Switch>(493375141, Switch.OnRefreshUserMenuDelegate);
    this.UpdateSwitchStatus();
  }

  public void HandleToggle()
  {
    this.Toggle();
  }

  public bool IsHandlerOn()
  {
    return this.switchedOn;
  }

  private void OnMinionToggle()
  {
    if (!DebugHandler.InstantBuildMode)
      this.openSwitch.Toggle(this.openToggleIndex);
    else
      this.Toggle();
  }

  protected virtual void Toggle()
  {
    this.SetState(!this.switchedOn);
  }

  protected virtual void SetState(bool on)
  {
    if (this.switchedOn == on)
      return;
    this.switchedOn = on;
    this.UpdateSwitchStatus();
    if (this.OnToggle != null)
      this.OnToggle(this.switchedOn);
    if (!this.manuallyControlled)
      return;
    Game.Instance.userMenu.Refresh(this.gameObject);
  }

  protected virtual void OnRefreshUserMenu(object data)
  {
    Game.Instance.userMenu.AddButton(this.gameObject, new KIconButtonMenu.ButtonInfo("action_power", (string) (!this.switchedOn ? BUILDINGS.PREFABS.SWITCH.TURN_ON : BUILDINGS.PREFABS.SWITCH.TURN_OFF), new System.Action(this.OnMinionToggle), Action.ToggleEnabled, (System.Action<GameObject>) null, (System.Action<KIconButtonMenu.ButtonInfo>) null, (Texture) null, (string) (!this.switchedOn ? BUILDINGS.PREFABS.SWITCH.TURN_ON_TOOLTIP : BUILDINGS.PREFABS.SWITCH.TURN_OFF_TOOLTIP), true), 1f);
  }

  protected virtual void UpdateSwitchStatus()
  {
    this.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, !this.switchedOn ? Db.Get().BuildingStatusItems.SwitchStatusInactive : Db.Get().BuildingStatusItems.SwitchStatusActive, (object) null);
  }
}
