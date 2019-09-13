// Decompiled with JetBrains decompiler
// Type: FactionAlignment
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using UnityEngine;

public class FactionAlignment : KMonoBehaviour
{
  private static readonly EventSystem.IntraObjectHandler<FactionAlignment> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<FactionAlignment>((System.Action<FactionAlignment, object>) ((component, data) => component.OnRefreshUserMenu(data)));
  private static readonly EventSystem.IntraObjectHandler<FactionAlignment> OnDeathDelegate = new EventSystem.IntraObjectHandler<FactionAlignment>((System.Action<FactionAlignment, object>) ((component, data) => component.OnDeath(data)));
  private static readonly EventSystem.IntraObjectHandler<FactionAlignment> SetPlayerTargetedFalseDelegate = new EventSystem.IntraObjectHandler<FactionAlignment>((System.Action<FactionAlignment, object>) ((component, data) => component.SetPlayerTargeted(false)));
  [Serialize]
  private bool alignmentActive = true;
  [Serialize]
  public bool targetable = true;
  public FactionManager.FactionID Alignment;
  [Serialize]
  public bool targeted;

  [MyCmpAdd]
  public Health health { get; private set; }

  public AttackableBase attackable { get; private set; }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.health = this.GetComponent<Health>();
    this.attackable = this.GetComponent<AttackableBase>();
    Components.FactionAlignments.Add(this);
    this.Subscribe<FactionAlignment>(493375141, FactionAlignment.OnRefreshUserMenuDelegate);
    this.Subscribe<FactionAlignment>(2127324410, FactionAlignment.SetPlayerTargetedFalseDelegate);
    if (this.alignmentActive)
      FactionManager.Instance.GetFaction(this.Alignment).Members.Add(this);
    this.Subscribe<FactionAlignment>(1623392196, FactionAlignment.OnDeathDelegate);
    this.UpdateStatusItem();
  }

  private void OnDeath(object data)
  {
    this.SetAlignmentActive(false);
  }

  public void SetAlignmentActive(bool active)
  {
    this.SetPlayerTargetable(active);
    this.alignmentActive = active;
    if (active)
      FactionManager.Instance.GetFaction(this.Alignment).Members.Add(this);
    else
      FactionManager.Instance.GetFaction(this.Alignment).Members.Remove(this);
  }

  public bool IsAlignmentActive()
  {
    return FactionManager.Instance.GetFaction(this.Alignment).Members.Contains(this);
  }

  public void SetPlayerTargetable(bool state)
  {
    this.targetable = state;
    if (state)
      return;
    this.SetPlayerTargeted(false);
  }

  public void SetPlayerTargeted(bool state)
  {
    this.targeted = state && this.targetable;
    this.UpdateStatusItem();
  }

  private void UpdateStatusItem()
  {
    if (this.targeted)
      this.GetComponent<KSelectable>().AddStatusItem(Db.Get().MiscStatusItems.OrderAttack, (object) null);
    else
      this.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.OrderAttack, false);
  }

  public void SwitchAlignment(FactionManager.FactionID newAlignment)
  {
    this.SetAlignmentActive(false);
    this.Alignment = newAlignment;
    this.SetAlignmentActive(true);
  }

  protected override void OnCleanUp()
  {
    Components.FactionAlignments.Remove(this);
    FactionManager.Instance.GetFaction(this.Alignment).Members.Remove(this);
    base.OnCleanUp();
  }

  private void OnRefreshUserMenu(object data)
  {
    if (this.Alignment == FactionManager.FactionID.Duplicant || !this.IsAlignmentActive())
      return;
    Game.Instance.userMenu.AddButton(this.gameObject, this.targeted ? new KIconButtonMenu.ButtonInfo("action_attack", (string) UI.USERMENUACTIONS.CANCELATTACK.NAME, (System.Action) (() => this.SetPlayerTargeted(false)), Action.NumActions, (System.Action<GameObject>) null, (System.Action<KIconButtonMenu.ButtonInfo>) null, (Texture) null, (string) UI.USERMENUACTIONS.CANCELATTACK.TOOLTIP, true) : new KIconButtonMenu.ButtonInfo("action_attack", (string) UI.USERMENUACTIONS.ATTACK.NAME, (System.Action) (() => this.SetPlayerTargeted(true)), Action.NumActions, (System.Action<GameObject>) null, (System.Action<KIconButtonMenu.ButtonInfo>) null, (Texture) null, (string) UI.USERMENUACTIONS.ATTACK.TOOLTIP, true), 1f);
  }
}
