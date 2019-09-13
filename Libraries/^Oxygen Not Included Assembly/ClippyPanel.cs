// Decompiled with JetBrains decompiler
// Type: ClippyPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

public class ClippyPanel : KScreen
{
  public Text title;
  public Text detailText;
  public Text flavorText;
  public Image topicIcon;
  private KButton okButton;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
  }

  protected override void OnActivate()
  {
    base.OnActivate();
    SpeedControlScreen.Instance.Pause(true);
    Game.Instance.Trigger(1634669191, (object) null);
  }

  public void OnOk()
  {
    SpeedControlScreen.Instance.Unpause(true);
    Object.Destroy((Object) this.gameObject);
  }
}
