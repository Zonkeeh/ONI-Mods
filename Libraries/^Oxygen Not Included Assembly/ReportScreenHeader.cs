// Decompiled with JetBrains decompiler
// Type: ReportScreenHeader
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class ReportScreenHeader : KMonoBehaviour
{
  [SerializeField]
  private ReportScreenHeaderRow rowTemplate;
  private ReportScreenHeaderRow mainRow;

  public void SetMainEntry(ReportManager.ReportGroup reportGroup)
  {
    if ((Object) this.mainRow == (Object) null)
      this.mainRow = Util.KInstantiateUI(this.rowTemplate.gameObject, this.gameObject, true).GetComponent<ReportScreenHeaderRow>();
    this.mainRow.SetLine(reportGroup);
  }
}
