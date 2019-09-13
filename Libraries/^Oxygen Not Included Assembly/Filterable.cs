// Decompiled with JetBrains decompiler
// Type: Filterable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System.Collections.Generic;
using UnityEngine;

public class Filterable : KMonoBehaviour
{
  private static readonly Operational.Flag filterSelected = new Operational.Flag(nameof (filterSelected), Operational.Flag.Type.Requirement);
  private static readonly EventSystem.IntraObjectHandler<Filterable> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<Filterable>((System.Action<Filterable, object>) ((component, data) => component.OnCopySettings(data)));
  [Serialize]
  private Tag selectedTag = GameTags.Void;
  [MyCmpAdd]
  private CopyBuildingSettings copyBuildingSettings;
  [Serialize]
  public Filterable.ElementState filterElementState;

  public event System.Action<Tag> onFilterChanged;

  public Tag SelectedTag
  {
    get
    {
      return this.selectedTag;
    }
    set
    {
      this.selectedTag = value;
      this.OnFilterChanged();
    }
  }

  public virtual IList<Tag> GetTagOptions()
  {
    List<Tag> tagList = new List<Tag>();
    tagList.Add(GameTags.Void);
    foreach (Element element in ElementLoader.elements)
    {
      if (!element.disabled)
      {
        bool flag = true;
        if (this.filterElementState != Filterable.ElementState.None)
        {
          switch (this.filterElementState)
          {
            case Filterable.ElementState.Solid:
              flag = element.IsSolid;
              break;
            case Filterable.ElementState.Liquid:
              flag = element.IsLiquid;
              break;
            case Filterable.ElementState.Gas:
              flag = element.IsGas;
              break;
          }
        }
        if (flag)
        {
          Tag tag = GameTagExtensions.Create(element.id);
          tagList.Add(tag);
        }
      }
    }
    return (IList<Tag>) tagList;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.Subscribe<Filterable>(-905833192, Filterable.OnCopySettingsDelegate);
  }

  private void OnCopySettings(object data)
  {
    Filterable component = ((GameObject) data).GetComponent<Filterable>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    this.SelectedTag = component.SelectedTag;
  }

  protected override void OnSpawn()
  {
    this.OnFilterChanged();
  }

  private void OnFilterChanged()
  {
    if (this.onFilterChanged != null)
      this.onFilterChanged(this.selectedTag);
    Operational component = this.GetComponent<Operational>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.SetFlag(Filterable.filterSelected, this.selectedTag.IsValid);
  }

  public enum ElementState
  {
    None,
    Solid,
    Liquid,
    Gas,
  }
}
