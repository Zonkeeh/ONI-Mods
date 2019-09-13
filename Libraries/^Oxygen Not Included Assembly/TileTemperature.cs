// Decompiled with JetBrains decompiler
// Type: TileTemperature
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

[SkipSaveFileSerialization]
public class TileTemperature : KMonoBehaviour
{
  [MyCmpReq]
  private PrimaryElement primaryElement;
  [MyCmpReq]
  private KSelectable selectable;

  protected override void OnPrefabInit()
  {
    PrimaryElement primaryElement1 = this.primaryElement;
    // ISSUE: reference to a compiler-generated field
    if (TileTemperature.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      TileTemperature.\u003C\u003Ef__mg\u0024cache0 = new PrimaryElement.GetTemperatureCallback(TileTemperature.OnGetTemperature);
    }
    // ISSUE: reference to a compiler-generated field
    PrimaryElement.GetTemperatureCallback fMgCache0 = TileTemperature.\u003C\u003Ef__mg\u0024cache0;
    primaryElement1.getTemperatureCallback = fMgCache0;
    PrimaryElement primaryElement2 = this.primaryElement;
    // ISSUE: reference to a compiler-generated field
    if (TileTemperature.\u003C\u003Ef__mg\u0024cache1 == null)
    {
      // ISSUE: reference to a compiler-generated field
      TileTemperature.\u003C\u003Ef__mg\u0024cache1 = new PrimaryElement.SetTemperatureCallback(TileTemperature.OnSetTemperature);
    }
    // ISSUE: reference to a compiler-generated field
    PrimaryElement.SetTemperatureCallback fMgCache1 = TileTemperature.\u003C\u003Ef__mg\u0024cache1;
    primaryElement2.setTemperatureCallback = fMgCache1;
    base.OnPrefabInit();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
  }

  private static float OnGetTemperature(PrimaryElement primary_element)
  {
    SimCellOccupier component = primary_element.GetComponent<SimCellOccupier>();
    if (!((Object) component != (Object) null) || !component.IsReady())
      return primary_element.InternalTemperature;
    int cell = Grid.PosToCell(primary_element.transform.GetPosition());
    return Grid.Temperature[cell];
  }

  private static void OnSetTemperature(PrimaryElement primary_element, float temperature)
  {
    SimCellOccupier component = primary_element.GetComponent<SimCellOccupier>();
    if ((Object) component != (Object) null && component.IsReady())
      Debug.LogWarning((object) "Only set a tile's temperature during initialization. Otherwise you should be modifying the cell via the sim!");
    else
      primary_element.InternalTemperature = temperature;
  }
}
