// Decompiled with JetBrains decompiler
// Type: ModsScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KMod;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ModsScreen : KModalScreen
{
  private List<ModsScreen.DisplayedMod> displayedMods = new List<ModsScreen.DisplayedMod>();
  private List<KMod.Label> mod_footprint = new List<KMod.Label>();
  [SerializeField]
  private KButton closeButtonTitle;
  [SerializeField]
  private KButton closeButton;
  [SerializeField]
  private KButton workshopButton;
  [SerializeField]
  private GameObject entryPrefab;
  [SerializeField]
  private Transform entryParent;

  protected override void OnActivate()
  {
    base.OnActivate();
    this.closeButtonTitle.onClick += new System.Action(this.Exit);
    this.closeButton.onClick += new System.Action(this.Exit);
    this.workshopButton.onClick += (System.Action) (() => Application.OpenURL("http://steamcommunity.com/workshop/browse/?appid=457140"));
    Global.Instance.modManager.Sanitize(this.gameObject);
    this.mod_footprint.Clear();
    foreach (KMod.Mod mod in Global.Instance.modManager.mods)
    {
      if (mod.enabled)
      {
        this.mod_footprint.Add(mod.label);
        if ((mod.loaded_content & (Content.Strings | Content.DLL | Content.Translation | Content.Animation)) == (mod.available_content & (Content.Strings | Content.DLL | Content.Translation | Content.Animation)))
          mod.Uncrash();
      }
    }
    this.BuildDisplay();
    Global.Instance.modManager.on_update += new Manager.OnUpdate(this.RebuildDisplay);
  }

  protected override void OnDeactivate()
  {
    Global.Instance.modManager.on_update -= new Manager.OnUpdate(this.RebuildDisplay);
    base.OnDeactivate();
  }

  private void Exit()
  {
    Global.Instance.modManager.Save();
    if (!Global.Instance.modManager.MatchFootprint(this.mod_footprint, Content.Strings | Content.DLL | Content.Translation | Content.Animation))
      Global.Instance.modManager.RestartDialog((string) UI.FRONTEND.MOD_DIALOGS.MODS_SCREEN_CHANGES.TITLE, (string) UI.FRONTEND.MOD_DIALOGS.MODS_SCREEN_CHANGES.MESSAGE, new System.Action(((KScreen) this).Deactivate), true, this.gameObject, (string) null);
    else
      this.Deactivate();
    Global.Instance.modManager.events.Clear();
  }

  private void RebuildDisplay(object change_source)
  {
    if (object.ReferenceEquals(change_source, (object) this))
      return;
    this.BuildDisplay();
  }

  private void BuildDisplay()
  {
    foreach (ModsScreen.DisplayedMod displayedMod in this.displayedMods)
    {
      if ((UnityEngine.Object) displayedMod.rect_transform != (UnityEngine.Object) null)
        UnityEngine.Object.Destroy((UnityEngine.Object) displayedMod.rect_transform.gameObject);
    }
    this.displayedMods.Clear();
    ModsScreen.ModOrderingDragListener orderingDragListener = new ModsScreen.ModOrderingDragListener(this, this.displayedMods);
    for (int index = 0; index != Global.Instance.modManager.mods.Count; ++index)
    {
      KMod.Mod mod = Global.Instance.modManager.mods[index];
      if (mod.status != KMod.Mod.Status.NotInstalled && mod.status != KMod.Mod.Status.UninstallPending && mod.HasAnyContent(Content.LayerableFiles | Content.Strings | Content.DLL | Content.Animation))
      {
        HierarchyReferences hierarchyReferences = Util.KInstantiateUI<HierarchyReferences>(this.entryPrefab, this.entryParent.gameObject, false);
        this.displayedMods.Add(new ModsScreen.DisplayedMod()
        {
          rect_transform = hierarchyReferences.gameObject.GetComponent<RectTransform>(),
          mod_index = index
        });
        hierarchyReferences.GetComponent<DragMe>().listener = (DragMe.IDragListener) orderingDragListener;
        LocText reference1 = hierarchyReferences.GetReference<LocText>("Title");
        reference1.text = mod.title;
        hierarchyReferences.GetReference<ToolTip>("Description").toolTip = mod.description;
        if (mod.crash_count != 0)
          reference1.color = Color.Lerp(Color.white, Color.red, (float) mod.crash_count / 3f);
        KButton reference2 = hierarchyReferences.GetReference<KButton>("ManageButton");
        reference2.isInteractable = mod.is_managed;
        if (reference2.isInteractable)
        {
          reference2.GetComponent<ToolTip>().toolTip = (string) mod.manage_tooltip;
          reference2.onClick += mod.on_managed;
        }
        MultiToggle toggle = hierarchyReferences.GetReference<MultiToggle>("EnabledToggle");
        toggle.ChangeState(!mod.enabled ? 0 : 1);
        toggle.onClick += (System.Action) (() => this.OnToggleClicked(toggle, mod.label));
        toggle.GetComponent<ToolTip>().OnToolTip = (Func<string>) (() => (string) (!mod.enabled ? UI.FRONTEND.MODS.TOOLTIPS.DISABLED : UI.FRONTEND.MODS.TOOLTIPS.ENABLED));
        hierarchyReferences.gameObject.SetActive(true);
      }
    }
    foreach (ModsScreen.DisplayedMod displayedMod in this.displayedMods)
      displayedMod.rect_transform.gameObject.SetActive(true);
    if (this.displayedMods.Count != 0)
      ;
  }

  private void OnToggleClicked(MultiToggle toggle, KMod.Label mod)
  {
    Manager modManager = Global.Instance.modManager;
    bool enabled = !modManager.IsModEnabled(mod);
    toggle.ChangeState(!enabled ? 0 : 1);
    modManager.EnableMod(mod, enabled, (object) this);
  }

  private struct DisplayedMod
  {
    public RectTransform rect_transform;
    public int mod_index;
  }

  private class ModOrderingDragListener : DragMe.IDragListener
  {
    private int startDragIdx = -1;
    private List<ModsScreen.DisplayedMod> mods;
    private ModsScreen screen;

    public ModOrderingDragListener(ModsScreen screen, List<ModsScreen.DisplayedMod> mods)
    {
      this.screen = screen;
      this.mods = mods;
    }

    public void OnBeginDrag(Vector2 pos)
    {
      this.startDragIdx = this.GetDragIdx(pos);
    }

    public void OnEndDrag(Vector2 pos)
    {
      if (this.startDragIdx < 0)
        return;
      int dragIdx = this.GetDragIdx(pos);
      Global.Instance.modManager.Reinsert(this.mods[this.startDragIdx].mod_index, dragIdx < 0 || dragIdx == this.startDragIdx ? Global.Instance.modManager.mods.Count : this.mods[dragIdx].mod_index, (object) this);
      this.screen.BuildDisplay();
    }

    private int GetDragIdx(Vector2 pos)
    {
      int num = -1;
      for (int index = 0; index < this.mods.Count; ++index)
      {
        if (RectTransformUtility.RectangleContainsScreenPoint(this.mods[index].rect_transform, pos))
        {
          num = index;
          break;
        }
      }
      return num;
    }
  }
}
