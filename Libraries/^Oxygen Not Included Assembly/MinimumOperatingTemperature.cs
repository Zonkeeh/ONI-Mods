// Decompiled with JetBrains decompiler
// Type: MinimumOperatingTemperature
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

[SkipSaveFileSerialization]
public class MinimumOperatingTemperature : KMonoBehaviour, ISim200ms, IGameObjectEffectDescriptor
{
  public static Operational.Flag warmEnoughFlag = new Operational.Flag("warm_enough", Operational.Flag.Type.Functional);
  public float minimumTemperature = 275.15f;
  [MyCmpReq]
  private Building building;
  [MyCmpReq]
  private Operational operational;
  [MyCmpReq]
  private PrimaryElement primaryElement;
  private const float TURN_ON_DELAY = 5f;
  private float lastOffTime;
  private bool isWarm;
  private HandleVector<int>.Handle partitionerEntry;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.TestTemperature(true);
  }

  public void Sim200ms(float dt)
  {
    this.TestTemperature(false);
  }

  private void TestTemperature(bool force)
  {
    bool flag;
    if ((double) this.primaryElement.Temperature < (double) this.minimumTemperature)
    {
      flag = false;
    }
    else
    {
      flag = true;
      for (int index = 0; index < this.building.PlacementCells.Length; ++index)
      {
        int placementCell = this.building.PlacementCells[index];
        float num1 = Grid.Temperature[placementCell];
        float num2 = Grid.Mass[placementCell];
        if (((double) num1 != 0.0 || (double) num2 != 0.0) && (double) num1 < (double) this.minimumTemperature)
        {
          flag = false;
          break;
        }
      }
    }
    if (!flag)
      this.lastOffTime = Time.time;
    if ((flag == this.isWarm || flag) && (flag == this.isWarm || !flag || (double) Time.time <= (double) this.lastOffTime + 5.0) && !force)
      return;
    this.isWarm = flag;
    this.operational.SetFlag(MinimumOperatingTemperature.warmEnoughFlag, this.isWarm);
    this.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.TooCold, !this.isWarm, (object) this);
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
  }

  public List<Descriptor> GetDescriptors(GameObject go)
  {
    return new List<Descriptor>()
    {
      new Descriptor(string.Format((string) UI.BUILDINGEFFECTS.MINIMUM_TEMP, (object) GameUtil.GetFormattedTemperature(this.minimumTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), string.Format((string) UI.BUILDINGEFFECTS.TOOLTIPS.MINIMUM_TEMP, (object) GameUtil.GetFormattedTemperature(this.minimumTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false)), Descriptor.DescriptorType.Effect, false)
    };
  }
}
