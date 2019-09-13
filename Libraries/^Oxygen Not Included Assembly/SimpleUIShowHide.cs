// Decompiled with JetBrains decompiler
// Type: SimpleUIShowHide
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class SimpleUIShowHide : KMonoBehaviour
{
  [MyCmpReq]
  private MultiToggle toggle;
  [SerializeField]
  public GameObject content;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.toggle.onClick += new System.Action(this.OnClick);
  }

  private void OnClick()
  {
    this.toggle.NextState();
    this.content.SetActive(this.toggle.CurrentState == 0);
  }
}
