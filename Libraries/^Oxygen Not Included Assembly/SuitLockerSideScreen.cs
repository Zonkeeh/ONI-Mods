// Decompiled with JetBrains decompiler
// Type: SuitLockerSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class SuitLockerSideScreen : SideScreenContent
{
  [SerializeField]
  private GameObject initialConfigScreen;
  [SerializeField]
  private GameObject regularConfigScreen;
  [SerializeField]
  private LocText initialConfigLabel;
  [SerializeField]
  private KButton initialConfigRequestSuitButton;
  [SerializeField]
  private KButton initialConfigNoSuitButton;
  [SerializeField]
  private LocText regularConfigLabel;
  [SerializeField]
  private KButton regularConfigRequestSuitButton;
  [SerializeField]
  private KButton regularConfigDropSuitButton;
  private SuitLocker suitLocker;

  protected override void OnSpawn()
  {
    base.OnSpawn();
  }

  public override bool IsValidForTarget(GameObject target)
  {
    return (UnityEngine.Object) target.GetComponent<SuitLocker>() != (UnityEngine.Object) null;
  }

  public override void SetTarget(GameObject target)
  {
    this.suitLocker = target.GetComponent<SuitLocker>();
    this.initialConfigRequestSuitButton.GetComponentInChildren<ToolTip>().SetSimpleTooltip((string) UI.UISIDESCREENS.SUIT_SIDE_SCREEN.CONFIG_REQUEST_SUIT_TOOLTIP);
    this.initialConfigNoSuitButton.GetComponentInChildren<ToolTip>().SetSimpleTooltip((string) UI.UISIDESCREENS.SUIT_SIDE_SCREEN.CONFIG_NO_SUIT_TOOLTIP);
    this.initialConfigRequestSuitButton.ClearOnClick();
    this.initialConfigRequestSuitButton.onClick += (System.Action) (() => this.suitLocker.ConfigRequestSuit());
    this.initialConfigNoSuitButton.ClearOnClick();
    this.initialConfigNoSuitButton.onClick += (System.Action) (() => this.suitLocker.ConfigNoSuit());
    this.regularConfigRequestSuitButton.ClearOnClick();
    this.regularConfigRequestSuitButton.onClick += (System.Action) (() =>
    {
      if (this.suitLocker.smi.sm.isWaitingForSuit.Get(this.suitLocker.smi))
        this.suitLocker.ConfigNoSuit();
      else
        this.suitLocker.ConfigRequestSuit();
    });
    this.regularConfigDropSuitButton.ClearOnClick();
    this.regularConfigDropSuitButton.onClick += (System.Action) (() => this.suitLocker.DropSuit());
  }

  private void Update()
  {
    bool flag1 = this.suitLocker.smi.sm.isConfigured.Get(this.suitLocker.smi);
    this.initialConfigScreen.gameObject.SetActive(!flag1);
    this.regularConfigScreen.gameObject.SetActive(flag1);
    bool flag2 = (UnityEngine.Object) this.suitLocker.GetStoredOutfit() != (UnityEngine.Object) null;
    bool flag3 = this.suitLocker.smi.sm.isWaitingForSuit.Get(this.suitLocker.smi);
    this.regularConfigRequestSuitButton.isInteractable = !flag2;
    if (!flag3)
    {
      this.regularConfigRequestSuitButton.GetComponentInChildren<LocText>().text = (string) UI.UISIDESCREENS.SUIT_SIDE_SCREEN.CONFIG_REQUEST_SUIT;
      this.regularConfigRequestSuitButton.GetComponentInChildren<ToolTip>().SetSimpleTooltip((string) UI.UISIDESCREENS.SUIT_SIDE_SCREEN.CONFIG_REQUEST_SUIT_TOOLTIP);
    }
    else
    {
      this.regularConfigRequestSuitButton.GetComponentInChildren<LocText>().text = (string) UI.UISIDESCREENS.SUIT_SIDE_SCREEN.CONFIG_CANCEL_REQUEST;
      this.regularConfigRequestSuitButton.GetComponentInChildren<ToolTip>().SetSimpleTooltip((string) UI.UISIDESCREENS.SUIT_SIDE_SCREEN.CONFIG_CANCEL_REQUEST_TOOLTIP);
    }
    if (flag2)
    {
      this.regularConfigDropSuitButton.isInteractable = true;
      this.regularConfigDropSuitButton.GetComponentInChildren<ToolTip>().SetSimpleTooltip((string) UI.UISIDESCREENS.SUIT_SIDE_SCREEN.CONFIG_DROP_SUIT_TOOLTIP);
    }
    else
    {
      this.regularConfigDropSuitButton.isInteractable = false;
      this.regularConfigDropSuitButton.GetComponentInChildren<ToolTip>().SetSimpleTooltip((string) UI.UISIDESCREENS.SUIT_SIDE_SCREEN.CONFIG_DROP_SUIT_NO_SUIT_TOOLTIP);
    }
    KSelectable component = this.suitLocker.GetComponent<KSelectable>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    StatusItemGroup.Entry statusItem = component.GetStatusItem(Db.Get().StatusItemCategories.Main);
    if (statusItem.item == null)
      return;
    this.regularConfigLabel.text = statusItem.item.GetName(statusItem.data);
    this.regularConfigLabel.GetComponentInChildren<ToolTip>().SetSimpleTooltip(statusItem.item.GetTooltip(statusItem.data));
  }
}
