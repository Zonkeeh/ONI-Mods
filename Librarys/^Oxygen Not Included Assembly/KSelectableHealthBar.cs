// Decompiled with JetBrains decompiler
// Type: KSelectableHealthBar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class KSelectableHealthBar : KSelectable
{
  private int scaleAmount = 100;
  [MyCmpGet]
  private ProgressBar progressBar;

  public override string GetName()
  {
    return string.Format("{0} {1}/{2}", (object) this.entityName, (object) (int) ((double) this.progressBar.PercentFull * (double) this.scaleAmount), (object) this.scaleAmount);
  }
}
