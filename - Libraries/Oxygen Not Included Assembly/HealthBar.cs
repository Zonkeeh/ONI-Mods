// Decompiled with JetBrains decompiler
// Type: HealthBar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class HealthBar : ProgressBar
{
  private float maxShowTime = 10f;
  private float alwaysShowThreshold = 0.8f;
  private float showTimer;

  private bool ShouldShow
  {
    get
    {
      if ((double) this.showTimer <= 0.0)
        return (double) this.PercentFull < (double) this.alwaysShowThreshold;
      return true;
    }
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.barColor = ProgressBarsConfig.Instance.GetBarColor(nameof (HealthBar));
    this.gameObject.SetActive(this.ShouldShow);
  }

  public void OnChange()
  {
    this.enabled = true;
    this.showTimer = this.maxShowTime;
  }

  public override void Update()
  {
    base.Update();
    if ((double) Time.timeScale > 0.0)
      this.showTimer = Mathf.Max(0.0f, this.showTimer - Time.unscaledDeltaTime);
    if (this.ShouldShow)
      return;
    this.gameObject.SetActive(false);
  }

  private void OnBecameInvisible()
  {
    this.enabled = false;
  }

  private void OnBecameVisible()
  {
    this.enabled = true;
  }

  public override void OnOverlayChanged(object data = null)
  {
    if (!this.autoHide)
      return;
    if ((HashedString) data == OverlayModes.None.ID)
    {
      if (this.gameObject.activeSelf || !this.ShouldShow)
        return;
      this.enabled = true;
      this.gameObject.SetActive(true);
    }
    else
    {
      if (!this.gameObject.activeSelf)
        return;
      this.enabled = false;
      this.gameObject.SetActive(false);
    }
  }
}
