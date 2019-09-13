// Decompiled with JetBrains decompiler
// Type: RadiationGermSpawner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class RadiationGermSpawner : KMonoBehaviour
{
  private const float GERM_SCALE = 100f;
  private const int CELLS_PER_UPDATE = 1024;
  private int nextEvaluatedCell;
  private float cellRatio;
  private byte disease_idx;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.cellRatio = (float) (Grid.CellCount / 1024);
    this.disease_idx = byte.MaxValue;
  }

  private void Update()
  {
  }

  private void EvaluateRadiation()
  {
    for (int index = 0; index < 1024; ++index)
    {
      int gameCell = (this.nextEvaluatedCell + index) % Grid.CellCount;
      if (Grid.RadiationCount[gameCell] >= 0)
      {
        int disease_delta1 = Mathf.RoundToInt((float) ((double) Grid.RadiationCount[gameCell] * 100.0 * ((double) Time.deltaTime * (double) this.cellRatio)));
        if ((int) Grid.DiseaseIdx[gameCell] == (int) this.disease_idx)
        {
          SimMessages.ModifyDiseaseOnCell(gameCell, this.disease_idx, disease_delta1);
        }
        else
        {
          int disease_delta2 = Grid.DiseaseCount[gameCell] - disease_delta1;
          if (disease_delta2 < 0)
            SimMessages.ModifyDiseaseOnCell(gameCell, this.disease_idx, disease_delta2);
          else
            SimMessages.ModifyDiseaseOnCell(gameCell, Grid.DiseaseIdx[gameCell], -disease_delta1);
        }
      }
    }
    this.nextEvaluatedCell = (this.nextEvaluatedCell + 1024) % Grid.CellCount;
  }
}
