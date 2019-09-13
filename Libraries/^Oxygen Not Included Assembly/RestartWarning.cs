// Decompiled with JetBrains decompiler
// Type: RestartWarning
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.UI;

public class RestartWarning : MonoBehaviour
{
  public static bool ShouldWarn;
  public LocText text;
  public Image image;

  private void Update()
  {
    if (!RestartWarning.ShouldWarn)
      return;
    this.text.enabled = true;
    this.image.enabled = true;
  }
}
