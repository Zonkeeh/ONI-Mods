// Decompiled with JetBrains decompiler
// Type: ResearchTreeTitle
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

public class ResearchTreeTitle : MonoBehaviour
{
  [Header("References")]
  [SerializeField]
  private LocText treeLabel;
  [SerializeField]
  private Image BG;

  public void SetLabel(string txt)
  {
    this.treeLabel.text = txt;
  }

  public void SetColor(int id)
  {
    this.BG.enabled = id % 2 != 0;
  }
}
