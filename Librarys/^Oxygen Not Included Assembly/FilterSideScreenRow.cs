// Decompiled with JetBrains decompiler
// Type: FilterSideScreenRow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

public class FilterSideScreenRow : KMonoBehaviour
{
  [SerializeField]
  private Color outlineHighLightColor = (Color) new Color32((byte) 168, (byte) 74, (byte) 121, byte.MaxValue);
  [SerializeField]
  private Color BGHighLightColor = (Color) new Color32((byte) 168, (byte) 74, (byte) 121, (byte) 80);
  [SerializeField]
  private Color outlineDefaultColor = (Color) new Color32((byte) 204, (byte) 204, (byte) 204, byte.MaxValue);
  private Color regularColor = Color.white;
  [SerializeField]
  private LocText labelText;
  [SerializeField]
  private Image BG;
  [SerializeField]
  private Image outline;
  [SerializeField]
  public KButton button;

  public Element element { get; private set; }

  public bool isSelected { get; private set; }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.regularColor = this.outline.color;
    if (!((UnityEngine.Object) this.button != (UnityEngine.Object) null))
      return;
    this.button.onPointerEnter += (System.Action) (() =>
    {
      if (this.isSelected)
        return;
      this.outline.color = this.outlineHighLightColor;
    });
    this.button.onPointerExit += (System.Action) (() =>
    {
      if (this.isSelected)
        return;
      this.outline.color = this.regularColor;
    });
  }

  public void SetElement(Element elem)
  {
    this.element = elem;
    this.SetText(elem.id != SimHashes.Void ? elem.name : STRINGS.UI.UISIDESCREENS.FILTERSIDESCREEN.NO_SELECTION.text);
  }

  private void SetText(string assignmentStr)
  {
    this.labelText.text = string.IsNullOrEmpty(assignmentStr) ? "-" : assignmentStr;
  }

  public void SetSelected(bool selected)
  {
    this.isSelected = selected;
    this.outline.color = !selected ? this.outlineDefaultColor : this.outlineHighLightColor;
    this.BG.color = !selected ? Color.white : this.BGHighLightColor;
  }
}
