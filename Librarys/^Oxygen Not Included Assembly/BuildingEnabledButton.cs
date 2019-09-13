// Decompiled with JetBrains decompiler
// Type: BuildingEnabledButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class BuildingEnabledButton : KMonoBehaviour, ISaveLoadable, IToggleHandler
{
  public static readonly Operational.Flag EnabledFlag = new Operational.Flag("building_enabled", Operational.Flag.Type.Functional);
  private static readonly EventSystem.IntraObjectHandler<BuildingEnabledButton> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<BuildingEnabledButton>((System.Action<BuildingEnabledButton, object>) ((component, data) => component.OnRefreshUserMenu(data)));
  [Serialize]
  private bool buildingEnabled = true;
  [MyCmpAdd]
  private Toggleable Toggleable;
  [MyCmpReq]
  private Operational Operational;
  private int ToggleIdx;

  public bool IsEnabled
  {
    get
    {
      if ((UnityEngine.Object) this.Operational != (UnityEngine.Object) null)
        return this.Operational.GetFlag(BuildingEnabledButton.EnabledFlag);
      return false;
    }
    set
    {
      this.Operational.SetFlag(BuildingEnabledButton.EnabledFlag, value);
      Game.Instance.userMenu.Refresh(this.gameObject);
      this.buildingEnabled = value;
      this.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.BuildingDisabled, !this.buildingEnabled, (object) null);
      this.Trigger(1088293757, (object) this.buildingEnabled);
    }
  }

  public bool WaitingForDisable
  {
    get
    {
      if (this.IsEnabled)
        return this.Toggleable.IsToggleQueued(this.ToggleIdx);
      return false;
    }
  }

  protected override void OnPrefabInit()
  {
    this.ToggleIdx = this.Toggleable.SetTarget((IToggleHandler) this);
    this.Subscribe<BuildingEnabledButton>(493375141, BuildingEnabledButton.OnRefreshUserMenuDelegate);
  }

  protected override void OnSpawn()
  {
    this.IsEnabled = this.buildingEnabled;
  }

  public void HandleToggle()
  {
    Prioritizable.RemoveRef(this.gameObject);
    this.OnToggle();
  }

  public bool IsHandlerOn()
  {
    return this.IsEnabled;
  }

  private void OnToggle()
  {
    this.IsEnabled = !this.IsEnabled;
    Game.Instance.userMenu.Refresh(this.gameObject);
  }

  private void OnMenuToggle()
  {
    if (!this.Toggleable.IsToggleQueued(this.ToggleIdx))
    {
      if (this.IsEnabled)
        this.Trigger(2108245096, (object) "BuildingDisabled");
      Prioritizable.AddRef(this.gameObject);
    }
    else
      Prioritizable.RemoveRef(this.gameObject);
    this.Toggleable.Toggle(this.ToggleIdx);
    Game.Instance.userMenu.Refresh(this.gameObject);
  }

  private void OnRefreshUserMenu(object data)
  {
    bool isEnabled = this.IsEnabled;
    bool flag = this.Toggleable.IsToggleQueued(this.ToggleIdx);
    Game.Instance.userMenu.AddButton(this.gameObject, isEnabled && !flag || !isEnabled && flag ? new KIconButtonMenu.ButtonInfo("action_building_disabled", (string) UI.USERMENUACTIONS.ENABLEBUILDING.NAME, new System.Action(this.OnMenuToggle), Action.ToggleEnabled, (System.Action<GameObject>) null, (System.Action<KIconButtonMenu.ButtonInfo>) null, (Texture) null, (string) UI.USERMENUACTIONS.ENABLEBUILDING.TOOLTIP, true) : new KIconButtonMenu.ButtonInfo("action_building_disabled", (string) UI.USERMENUACTIONS.ENABLEBUILDING.NAME_OFF, new System.Action(this.OnMenuToggle), Action.ToggleEnabled, (System.Action<GameObject>) null, (System.Action<KIconButtonMenu.ButtonInfo>) null, (Texture) null, (string) UI.USERMENUACTIONS.ENABLEBUILDING.TOOLTIP_OFF, true), 1f);
  }
}
