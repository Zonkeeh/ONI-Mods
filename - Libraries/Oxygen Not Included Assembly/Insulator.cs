// Decompiled with JetBrains decompiler
// Type: Insulator
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[SkipSaveFileSerialization]
public class Insulator : KMonoBehaviour
{
  [SerializeField]
  public CellOffset offset = CellOffset.none;
  [MyCmpReq]
  private Building building;

  protected override void OnSpawn()
  {
    SimMessages.SetInsulation(Grid.OffsetCell(Grid.PosToCell(this.transform.GetPosition()), this.offset), this.building.Def.ThermalConductivity);
  }

  protected override void OnCleanUp()
  {
    SimMessages.SetInsulation(Grid.OffsetCell(Grid.PosToCell(this.transform.GetPosition()), this.offset), 1f);
  }
}
