// Decompiled with JetBrains decompiler
// Type: SafeCellSensor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class SafeCellSensor : Sensor
{
  private int cell = Grid.InvalidCell;
  private MinionBrain brain;
  private Navigator navigator;
  private KPrefabID prefabid;

  public SafeCellSensor(Sensors sensors)
    : base(sensors)
  {
    this.navigator = this.GetComponent<Navigator>();
    this.brain = this.GetComponent<MinionBrain>();
    this.prefabid = this.GetComponent<KPrefabID>();
  }

  public override void Update()
  {
    if (!this.prefabid.HasTag(GameTags.Idle))
    {
      this.cell = Grid.InvalidCell;
    }
    else
    {
      bool flag1 = this.HasSafeCell();
      this.RunSafeCellQuery(false);
      bool flag2 = this.HasSafeCell();
      if (flag2 == flag1)
        return;
      if (flag2)
        this.sensors.Trigger(982561777, (object) null);
      else
        this.sensors.Trigger(506919987, (object) null);
    }
  }

  public void RunSafeCellQuery(bool avoid_light)
  {
    MinionPathFinderAbilities currentAbilities = (MinionPathFinderAbilities) this.navigator.GetCurrentAbilities();
    currentAbilities.SetIdleNavMaskEnabled(true);
    SafeCellQuery safeCellQuery = PathFinderQueries.safeCellQuery.Reset(this.brain, avoid_light);
    this.navigator.RunQuery((PathFinderQuery) safeCellQuery);
    currentAbilities.SetIdleNavMaskEnabled(false);
    this.cell = safeCellQuery.GetResultCell();
    if (this.cell != Grid.PosToCell((KMonoBehaviour) this.navigator))
      return;
    this.cell = Grid.InvalidCell;
  }

  public int GetSensorCell()
  {
    return this.cell;
  }

  public int GetCellQuery()
  {
    if (this.cell == Grid.InvalidCell)
      this.RunSafeCellQuery(false);
    return this.cell;
  }

  public int GetSleepCellQuery()
  {
    if (this.cell == Grid.InvalidCell)
      this.RunSafeCellQuery(true);
    return this.cell;
  }

  public bool HasSafeCell()
  {
    if (this.cell != Grid.InvalidCell)
      return this.cell != Grid.PosToCell((KMonoBehaviour) this.sensors);
    return false;
  }
}
