// Decompiled with JetBrains decompiler
// Type: BuildWatermark
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class BuildWatermark : KScreen
{
  public LocText textDisplay;
  public static BuildWatermark Instance;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    BuildWatermark.Instance = this;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    string str = "LU-" + (!Application.isEditor ? 366134U.ToString() : "<EDITOR>");
    this.textDisplay.SetText(string.Format((string) UI.DEVELOPMENTBUILDS.WATERMARK, (object) str));
  }

  private void Update()
  {
    if (this.transform.GetSiblingIndex() == this.transform.parent.childCount - 1)
      return;
    this.transform.SetAsLastSibling();
  }
}
