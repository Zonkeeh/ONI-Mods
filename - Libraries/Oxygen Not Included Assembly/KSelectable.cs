// Decompiled with JetBrains decompiler
// Type: KSelectable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

[SkipSaveFileSerialization]
public class KSelectable : KMonoBehaviour
{
  [SerializeField]
  private bool selectable = true;
  private const float hoverHighlight = 0.25f;
  private const float selectHighlight = 0.2f;
  public string entityName;
  public string entityGender;
  private bool selected;
  [SerializeField]
  private bool disableSelectMarker;
  private StatusItemGroup statusItemGroup;

  public bool IsSelected
  {
    get
    {
      return this.selected;
    }
  }

  public bool IsSelectable
  {
    get
    {
      if (this.selectable)
        return this.isActiveAndEnabled;
      return false;
    }
    set
    {
      this.selectable = value;
    }
  }

  public bool DisableSelectMarker
  {
    get
    {
      return this.disableSelectMarker;
    }
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.statusItemGroup = new StatusItemGroup(this.gameObject);
    if (!((UnityEngine.Object) this.GetComponent<KPrefabID>() != (UnityEngine.Object) null))
      ;
    if (this.entityName == null || this.entityName.Length <= 0)
      this.SetName(this.name);
    if (this.entityGender != null)
      return;
    this.entityGender = "NB";
  }

  public virtual string GetName()
  {
    if (this.entityName != null && !(this.entityName == string.Empty) && this.entityName.Length > 0)
      return this.entityName;
    Debug.Log((object) "Warning Item has blank name!", (UnityEngine.Object) this.gameObject);
    return this.name;
  }

  public void SetStatusIndicatorOffset(Vector3 offset)
  {
    if (this.statusItemGroup == null)
      return;
    this.statusItemGroup.SetOffset(offset);
  }

  public void SetName(string name)
  {
    this.entityName = name;
  }

  public void SetGender(string Gender)
  {
    this.entityGender = Gender;
  }

  public float GetZoom()
  {
    Bounds bounds = Util.GetBounds(this.gameObject);
    return 1.05f * Mathf.Max(bounds.extents.x, bounds.extents.y);
  }

  public Vector3 GetPortraitLocation()
  {
    Vector3 vector3 = new Vector3();
    return Util.GetBounds(this.gameObject).center;
  }

  private void ClearHighlight()
  {
    this.Trigger(-1201923725, (object) false);
    KBatchedAnimController component = this.GetComponent<KBatchedAnimController>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.HighlightColour = (Color32) new Color(0.0f, 0.0f, 0.0f, 0.0f);
  }

  private void ApplyHighlight(float highlight)
  {
    this.Trigger(-1201923725, (object) true);
    KBatchedAnimController component = this.GetComponent<KBatchedAnimController>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.HighlightColour = (Color32) new Color(highlight, highlight, highlight, highlight);
  }

  public void Select()
  {
    this.selected = true;
    this.ClearHighlight();
    this.ApplyHighlight(0.2f);
    this.Trigger(-1503271301, (object) true);
  }

  public void Unselect()
  {
    if (!this.selected)
      return;
    this.selected = false;
    this.ClearHighlight();
    this.Trigger(-1503271301, (object) false);
  }

  public void Hover(bool playAudio)
  {
    this.ClearHighlight();
    if (!DebugHandler.HideUI)
      this.ApplyHighlight(0.25f);
    if (!playAudio)
      return;
    this.PlayHoverSound();
  }

  private void PlayHoverSound()
  {
    if ((UnityEngine.Object) this.GetComponent<CellSelectionObject>() != (UnityEngine.Object) null)
      return;
    UISounds.PlaySound(UISounds.Sound.Object_Mouseover);
  }

  public void Unhover()
  {
    if (this.selected)
      return;
    this.ClearHighlight();
  }

  public Guid ToggleStatusItem(StatusItem status_item, bool on, object data = null)
  {
    if (on)
      return this.AddStatusItem(status_item, data);
    return this.RemoveStatusItem(status_item, false);
  }

  public Guid ToggleStatusItem(StatusItem status_item, Guid guid, bool show, object data = null)
  {
    if (show)
    {
      if (guid != Guid.Empty)
        return guid;
      return this.AddStatusItem(status_item, data);
    }
    if (guid != Guid.Empty)
      return this.RemoveStatusItem(guid, false);
    return guid;
  }

  public Guid SetStatusItem(StatusItemCategory category, StatusItem status_item, object data = null)
  {
    if (this.statusItemGroup == null)
      return Guid.Empty;
    return this.statusItemGroup.SetStatusItem(category, status_item, data);
  }

  public Guid ReplaceStatusItem(Guid guid, StatusItem status_item, object data = null)
  {
    if (this.statusItemGroup == null)
      return Guid.Empty;
    if (guid != Guid.Empty)
      this.statusItemGroup.RemoveStatusItem(guid, false);
    return this.AddStatusItem(status_item, data);
  }

  public Guid AddStatusItem(StatusItem status_item, object data = null)
  {
    if (this.statusItemGroup == null)
      return Guid.Empty;
    return this.statusItemGroup.AddStatusItem(status_item, data, (StatusItemCategory) null);
  }

  public Guid RemoveStatusItem(StatusItem status_item, bool immediate = false)
  {
    if (this.statusItemGroup == null)
      return Guid.Empty;
    this.statusItemGroup.RemoveStatusItem(status_item, immediate);
    return Guid.Empty;
  }

  public Guid RemoveStatusItem(Guid guid, bool immediate = false)
  {
    if (this.statusItemGroup == null)
      return Guid.Empty;
    this.statusItemGroup.RemoveStatusItem(guid, immediate);
    return Guid.Empty;
  }

  public bool HasStatusItem(StatusItem status_item)
  {
    if (this.statusItemGroup == null)
      return false;
    return this.statusItemGroup.HasStatusItem(status_item);
  }

  public StatusItemGroup.Entry GetStatusItem(StatusItemCategory category)
  {
    return this.statusItemGroup.GetStatusItem(category);
  }

  public StatusItemGroup GetStatusItemGroup()
  {
    return this.statusItemGroup;
  }

  protected override void OnLoadLevel()
  {
    this.OnCleanUp();
    base.OnLoadLevel();
  }

  protected override void OnCleanUp()
  {
    if (this.statusItemGroup != null)
    {
      this.statusItemGroup.Destroy();
      this.statusItemGroup = (StatusItemGroup) null;
    }
    if (this.selected && (UnityEngine.Object) SelectTool.Instance != (UnityEngine.Object) null)
    {
      if ((UnityEngine.Object) SelectTool.Instance.selected == (UnityEngine.Object) this)
        SelectTool.Instance.Select((KSelectable) null, true);
      else
        this.Unselect();
    }
    base.OnCleanUp();
  }
}
