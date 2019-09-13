// Decompiled with JetBrains decompiler
// Type: RootMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class RootMenu : KScreen
{
  private List<KScreen> subMenus = new List<KScreen>();
  public bool canTogglePauseScreen = true;
  private DetailsScreen detailsScreen;
  private UserMenuScreen userMenu;
  [SerializeField]
  private GameObject detailsScreenPrefab;
  [SerializeField]
  private UserMenuScreen userMenuPrefab;
  private GameObject userMenuParent;
  [SerializeField]
  private TileScreen tileScreen;
  public KScreen buildMenu;
  private TileScreen tileScreenInst;
  public GameObject selectedGO;

  public static void DestroyInstance()
  {
    RootMenu.Instance = (RootMenu) null;
  }

  public static RootMenu Instance { get; private set; }

  public override float GetSortKey()
  {
    return -1f;
  }

  protected override void OnPrefabInit()
  {
    RootMenu.Instance = this;
    UIRegistry.rootMenu = this;
    this.Subscribe(Game.Instance.gameObject, -1503271301, new System.Action<object>(this.OnSelectObject));
    this.Subscribe(Game.Instance.gameObject, 288942073, new System.Action<object>(this.OnUIClear));
    this.Subscribe(Game.Instance.gameObject, -809948329, new System.Action<object>(this.OnBuildingStatechanged));
    base.OnPrefabInit();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.detailsScreen = Util.KInstantiateUI(this.detailsScreenPrefab, this.gameObject, true).GetComponent<DetailsScreen>();
    this.detailsScreen.gameObject.SetActive(true);
    this.userMenuParent = this.detailsScreen.UserMenuPanel.gameObject;
    this.userMenu = Util.KInstantiateUI(this.userMenuPrefab.gameObject, this.userMenuParent, false).GetComponent<UserMenuScreen>();
    this.detailsScreen.gameObject.SetActive(false);
    this.userMenu.gameObject.SetActive(false);
  }

  private void OnClickCommon()
  {
    this.CloseSubMenus();
  }

  public void AddSubMenu(KScreen sub_menu)
  {
    if (sub_menu.activateOnSpawn)
      sub_menu.Show(true);
    this.subMenus.Add(sub_menu);
  }

  public void RemoveSubMenu(KScreen sub_menu)
  {
    this.subMenus.Remove(sub_menu);
  }

  private void CloseSubMenus()
  {
    foreach (KScreen subMenu in this.subMenus)
    {
      if ((UnityEngine.Object) subMenu != (UnityEngine.Object) null)
      {
        if (subMenu.activateOnSpawn)
          subMenu.gameObject.SetActive(false);
        else
          subMenu.Deactivate();
      }
    }
    this.subMenus.Clear();
  }

  private void OnSelectObject(object data)
  {
    GameObject gameObject = (GameObject) data;
    if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
    {
      KPrefabID component = gameObject.GetComponent<KPrefabID>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null && !component.IsInitialized())
        return;
    }
    if (!((UnityEngine.Object) gameObject != (UnityEngine.Object) this.selectedGO))
      return;
    this.selectedGO = gameObject;
    this.CloseSubMenus();
    if ((UnityEngine.Object) this.selectedGO != (UnityEngine.Object) null && ((UnityEngine.Object) this.selectedGO.GetComponent<KPrefabID>() != (UnityEngine.Object) null || (bool) ((UnityEngine.Object) this.selectedGO.GetComponent<CellSelectionObject>())))
    {
      this.AddSubMenu((KScreen) this.detailsScreen);
      this.detailsScreen.Refresh(this.selectedGO);
      this.AddSubMenu((KScreen) this.userMenu);
      this.userMenu.SetSelected(this.selectedGO);
      this.userMenu.Refresh(this.selectedGO);
    }
    else
      this.userMenu.SetSelected((GameObject) null);
  }

  private void OnBuildingStatechanged(object data)
  {
    GameObject gameObject = (GameObject) data;
    if (!((UnityEngine.Object) gameObject == (UnityEngine.Object) this.selectedGO))
      return;
    this.OnSelectObject((object) gameObject);
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (!e.Consumed && e.TryConsume(Action.Escape) && SelectTool.Instance.enabled)
    {
      if (!this.canTogglePauseScreen)
        return;
      if (this.AreSubMenusOpen())
      {
        KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Back", false));
        this.CloseSubMenus();
        SelectTool.Instance.Select((KSelectable) null, false);
      }
      else if (e.IsAction(Action.Escape))
      {
        if (!SelectTool.Instance.enabled)
          KMonoBehaviour.PlaySound(GlobalAssets.GetSound("HUD_Click_Close", false));
        if (PlayerController.Instance.IsUsingDefaultTool())
        {
          if ((UnityEngine.Object) SelectTool.Instance.selected != (UnityEngine.Object) null)
          {
            SelectTool.Instance.Select((KSelectable) null, false);
          }
          else
          {
            CameraController.Instance.ForcePanningState(false);
            this.TogglePauseScreen();
          }
        }
        else
          Game.Instance.Trigger(288942073, (object) null);
        ToolMenu.Instance.ClearSelection();
        SelectTool.Instance.Activate();
      }
    }
    base.OnKeyDown(e);
  }

  public override void OnKeyUp(KButtonEvent e)
  {
    base.OnKeyUp(e);
    if (e.Consumed || !e.TryConsume(Action.AlternateView) || !((UnityEngine.Object) this.tileScreenInst != (UnityEngine.Object) null))
      return;
    this.tileScreenInst.Deactivate();
    this.tileScreenInst = (TileScreen) null;
  }

  public void TogglePauseScreen()
  {
    PauseScreen.Instance.Show(true);
  }

  public void ExternalClose()
  {
    this.OnClickCommon();
  }

  private void OnUIClear(object data)
  {
    this.CloseSubMenus();
    if ((UnityEngine.Object) UnityEngine.EventSystems.EventSystem.current != (UnityEngine.Object) null)
      UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject((GameObject) null);
    else
      Debug.LogWarning((object) "OnUIClear() Event system is null");
  }

  protected override void OnActivate()
  {
    base.OnActivate();
  }

  private bool AreSubMenusOpen()
  {
    return this.subMenus.Count > 0;
  }

  private KToggleMenu.ToggleInfo[] GetFillers()
  {
    HashSet<Tag> tagSet = new HashSet<Tag>();
    List<KToggleMenu.ToggleInfo> toggleInfoList = new List<KToggleMenu.ToggleInfo>();
    foreach (Pickupable pickupable in Components.Pickupables.Items)
    {
      KPrefabID kprefabId = pickupable.KPrefabID;
      if (kprefabId.HasTag(GameTags.Filler) && tagSet.Add(kprefabId.PrefabTag))
      {
        string text = kprefabId.GetComponent<PrimaryElement>().Element.id.ToString();
        toggleInfoList.Add(new KToggleMenu.ToggleInfo(text, (object) null, Action.NumActions));
      }
    }
    return toggleInfoList.ToArray();
  }

  public bool IsBuildingChorePanelActive()
  {
    if ((UnityEngine.Object) this.detailsScreen != (UnityEngine.Object) null)
      return this.detailsScreen.GetActiveTab() is BuildingChoresPanel;
    return false;
  }
}
