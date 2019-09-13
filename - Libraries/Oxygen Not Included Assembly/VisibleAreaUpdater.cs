// Decompiled with JetBrains decompiler
// Type: VisibleAreaUpdater
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class VisibleAreaUpdater
{
  private GridVisibleArea VisibleArea;
  private System.Action<int> OutsideViewFirstTimeCallback;
  private System.Action<int> InsideViewFirstTimeCallback;
  private System.Action<int> InsideViewSecondTimeCallback;
  private System.Action<int> InsideViewRepeatCallback;
  private System.Action<int> UpdateCallback;
  private string Name;

  public VisibleAreaUpdater(
    System.Action<int> outside_view_first_time_cb,
    System.Action<int> inside_view_first_time_cb,
    System.Action<int> inside_view_second_time_cb,
    System.Action<int> inside_view_repeat_cb,
    string name)
  {
    this.OutsideViewFirstTimeCallback = outside_view_first_time_cb;
    this.InsideViewFirstTimeCallback = inside_view_first_time_cb;
    this.InsideViewSecondTimeCallback = inside_view_second_time_cb;
    this.UpdateCallback = new System.Action<int>(this.InternalUpdateCell);
    this.Name = name;
  }

  public void Update()
  {
    if (!((UnityEngine.Object) CameraController.Instance != (UnityEngine.Object) null) || this.VisibleArea != null)
      return;
    this.VisibleArea = CameraController.Instance.VisibleArea;
    this.VisibleArea.AddCallback(this.Name, new System.Action(this.OnVisibleAreaUpdate));
    this.VisibleArea.Run(this.InsideViewFirstTimeCallback);
    this.VisibleArea.Run(this.InsideViewRepeatCallback);
  }

  private void OnVisibleAreaUpdate()
  {
    if (this.VisibleArea == null)
      return;
    this.VisibleArea.Run(this.OutsideViewFirstTimeCallback, this.InsideViewFirstTimeCallback, this.InsideViewSecondTimeCallback);
  }

  private void InternalUpdateCell(int cell)
  {
    this.OutsideViewFirstTimeCallback(cell);
    this.InsideViewFirstTimeCallback(cell);
  }

  public void UpdateCell(int cell)
  {
    if (this.VisibleArea == null)
      return;
    this.VisibleArea.RunIfVisible(cell, this.UpdateCallback);
  }
}
