// Decompiled with JetBrains decompiler
// Type: OreSizeVisualizerData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public struct OreSizeVisualizerData
{
  public PrimaryElement primaryElement;
  public System.Action<object> onMassChangedCB;

  public OreSizeVisualizerData(GameObject go)
  {
    this.primaryElement = go.GetComponent<PrimaryElement>();
    this.onMassChangedCB = (System.Action<object>) null;
  }
}
