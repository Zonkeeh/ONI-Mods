// Decompiled with JetBrains decompiler
// Type: SimFailedLoadScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Events;

internal class SimFailedLoadScreen : KScreen
{
  [SerializeField]
  private UnityEngine.UI.Button okButton;
  [SerializeField]
  private LocText bodyText;

  private bool IsRuntimeInstalled()
  {
    return true;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.bodyText.key = "STRINGS.UI.FRONTEND.MINSPECSCREEN.SIMFAILEDTOLOAD";
    this.okButton.onClick.AddListener(new UnityAction(this.OnClickQuit));
    if (!this.IsRuntimeInstalled())
      return;
    this.Deactivate();
  }

  private void OnClickQuit()
  {
    this.Deactivate();
  }

  protected override void OnActivate()
  {
    if (!this.IsRuntimeInstalled())
      return;
    this.Deactivate();
  }
}
