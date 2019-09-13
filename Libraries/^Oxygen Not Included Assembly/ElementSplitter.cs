// Decompiled with JetBrains decompiler
// Type: ElementSplitter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

public struct ElementSplitter
{
  public PrimaryElement primaryElement;
  public Func<float, Pickupable> onTakeCB;
  public Func<Pickupable, bool> canAbsorbCB;

  public ElementSplitter(GameObject go)
  {
    this.primaryElement = go.GetComponent<PrimaryElement>();
    this.onTakeCB = (Func<float, Pickupable>) null;
    this.canAbsorbCB = (Func<Pickupable, bool>) null;
  }
}
