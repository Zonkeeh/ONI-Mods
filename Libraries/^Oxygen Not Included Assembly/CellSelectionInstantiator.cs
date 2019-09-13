// Decompiled with JetBrains decompiler
// Type: CellSelectionInstantiator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class CellSelectionInstantiator : MonoBehaviour
{
  public GameObject CellSelectionPrefab;

  private void Awake()
  {
    GameObject gameObject1 = Util.KInstantiate(this.CellSelectionPrefab, (GameObject) null, "WorldSelectionCollider");
    GameObject gameObject2 = Util.KInstantiate(this.CellSelectionPrefab, (GameObject) null, "WorldSelectionCollider");
    CellSelectionObject component1 = gameObject1.GetComponent<CellSelectionObject>();
    CellSelectionObject component2 = gameObject2.GetComponent<CellSelectionObject>();
    component1.alternateSelectionObject = component2;
    component2.alternateSelectionObject = component1;
  }
}
