// Decompiled with JetBrains decompiler
// Type: ClosestEdibleSensor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class ClosestEdibleSensor : Sensor
{
  private static TagBits edibleTagBits = new TagBits(GameTags.Edible);
  private Edible edible;
  private bool hasEdible;
  public bool edibleInReachButNotPermitted;

  public ClosestEdibleSensor(Sensors sensors)
    : base(sensors)
  {
  }

  public override void Update()
  {
    TagBits forbid_tags = new TagBits(this.GetComponent<ConsumableConsumer>().forbiddenTags);
    Pickupable edibleFetchTarget = Game.Instance.fetchManager.FindEdibleFetchTarget(this.GetComponent<Storage>(), ref ClosestEdibleSensor.edibleTagBits, ref TagBits.None, ref forbid_tags, 0.0f);
    bool reachButNotPermitted = this.edibleInReachButNotPermitted;
    Edible edible = (Edible) null;
    bool flag1 = false;
    bool flag2;
    if ((Object) edibleFetchTarget != (Object) null)
    {
      edible = edibleFetchTarget.GetComponent<Edible>();
      flag1 = true;
      flag2 = false;
    }
    else
      flag2 = (Object) Game.Instance.fetchManager.FindFetchTarget(this.GetComponent<Storage>(), ref ClosestEdibleSensor.edibleTagBits, ref TagBits.None, ref TagBits.None, 0.0f) != (Object) null;
    if (!((Object) edible != (Object) this.edible) && this.hasEdible == flag1)
      return;
    this.edible = edible;
    this.hasEdible = flag1;
    this.edibleInReachButNotPermitted = flag2;
    this.Trigger(86328522, (object) this.edible);
  }

  public Edible GetEdible()
  {
    return this.edible;
  }
}
