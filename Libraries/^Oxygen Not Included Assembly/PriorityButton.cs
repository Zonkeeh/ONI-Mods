// Decompiled with JetBrains decompiler
// Type: PriorityButton
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class PriorityButton : KMonoBehaviour
{
  public bool playSelectionSound = true;
  public KToggle toggle;
  public LocText text;
  public ToolTip tooltip;
  [MyCmpGet]
  private ImageToggleState its;
  public ColorStyleSetting normalStyle;
  public ColorStyleSetting highStyle;
  public System.Action<PrioritySetting> onClick;
  private PrioritySetting _priority;

  public PrioritySetting priority
  {
    get
    {
      return this._priority;
    }
    set
    {
      this._priority = value;
      if (!((UnityEngine.Object) this.its != (UnityEngine.Object) null))
        return;
      this.its.colorStyleSetting = this.priority.priority_class != PriorityScreen.PriorityClass.high ? this.normalStyle : this.highStyle;
      this.its.RefreshColorStyle();
      this.its.ResetColor();
    }
  }

  protected override void OnPrefabInit()
  {
    this.toggle.onClick += new System.Action(this.OnClick);
  }

  private void OnClick()
  {
    if (this.playSelectionSound)
      PriorityScreen.PlayPriorityConfirmSound(this.priority);
    if (this.onClick == null)
      return;
    this.onClick(this.priority);
  }
}
