// Decompiled with JetBrains decompiler
// Type: DiseaseContainer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public struct DiseaseContainer
{
  public AutoDisinfectable autoDisinfectable;
  public byte elemIdx;
  public bool isContainer;
  public ConduitType conduitType;
  public KBatchedAnimController controller;
  public GameObject visualDiseaseProvider;
  public int overpopulationCount;
  public float instanceGrowthRate;
  public float accumulatedError;

  public DiseaseContainer(GameObject go, byte elemIdx)
  {
    this.elemIdx = elemIdx;
    this.isContainer = go.GetComponent<IUserControlledCapacity>() != null;
    Conduit component = go.GetComponent<Conduit>();
    this.conduitType = !((Object) component != (Object) null) ? ConduitType.None : component.type;
    this.controller = go.GetComponent<KBatchedAnimController>();
    this.overpopulationCount = 1;
    this.instanceGrowthRate = 1f;
    this.accumulatedError = 0.0f;
    this.visualDiseaseProvider = (GameObject) null;
    this.autoDisinfectable = go.GetComponent<AutoDisinfectable>();
    if (!((Object) this.autoDisinfectable != (Object) null))
      return;
    AutoDisinfectableManager.Instance.AddAutoDisinfectable(this.autoDisinfectable);
  }

  public void Clear()
  {
    this.controller = (KBatchedAnimController) null;
  }
}
