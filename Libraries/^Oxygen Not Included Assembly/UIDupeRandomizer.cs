// Decompiled with JetBrains decompiler
// Type: UIDupeRandomizer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UIDupeRandomizer : MonoBehaviour
{
  public bool applyHat = true;
  public bool applySuit = true;
  public KAnimFile head_default_anim;
  public KAnimFile head_swap_anim;
  public KAnimFile body_swap_anim;
  public UIDupeRandomizer.AnimChoice[] anims;
  private AccessorySlots slots;

  protected virtual void Start()
  {
    this.slots = new AccessorySlots((ResourceSet) null, this.head_default_anim, this.head_swap_anim, this.body_swap_anim);
    for (int minion_idx = 0; minion_idx < this.anims.Length; ++minion_idx)
    {
      this.anims[minion_idx].curBody = (KAnimFile) null;
      this.GetNewBody(minion_idx);
    }
  }

  protected void GetNewBody(int minion_idx)
  {
    Personality personality = Db.Get().Personalities[UnityEngine.Random.Range(0, Db.Get().Personalities.Count)];
    foreach (KBatchedAnimController minion in this.anims[minion_idx].minions)
      this.Apply(minion, personality);
  }

  private void Apply(KBatchedAnimController dupe, Personality personality)
  {
    KCompBuilder.BodyData bodyData = MinionStartingStats.CreateBodyData(personality);
    SymbolOverrideController component = dupe.GetComponent<SymbolOverrideController>();
    component.RemoveAllSymbolOverrides(0);
    UIDupeRandomizer.AddAccessory(dupe, this.slots.Hair.Lookup(bodyData.hair));
    UIDupeRandomizer.AddAccessory(dupe, this.slots.HatHair.Lookup("hat_" + HashCache.Get().Get(bodyData.hair)));
    UIDupeRandomizer.AddAccessory(dupe, this.slots.Eyes.Lookup(bodyData.eyes));
    UIDupeRandomizer.AddAccessory(dupe, this.slots.HeadShape.Lookup(bodyData.headShape));
    UIDupeRandomizer.AddAccessory(dupe, this.slots.Mouth.Lookup(bodyData.mouth));
    UIDupeRandomizer.AddAccessory(dupe, this.slots.Body.Lookup(bodyData.body));
    UIDupeRandomizer.AddAccessory(dupe, this.slots.Arm.Lookup(bodyData.arms));
    if (this.applySuit && (double) UnityEngine.Random.value < 0.150000005960464)
    {
      component.AddBuildOverride(Assets.GetAnim((HashedString) "body_oxygen_kanim").GetData(), 6);
      component.AddBuildOverride(Assets.GetAnim((HashedString) "helm_oxygen_kanim").GetData(), 6);
      dupe.SetSymbolVisiblity((KAnimHashedString) "snapto_neck", true);
    }
    else
      dupe.SetSymbolVisiblity((KAnimHashedString) "snapto_neck", false);
    if (this.applyHat && (double) UnityEngine.Random.value < 0.5)
    {
      List<string> stringList = new List<string>();
      foreach (Skill resource in Db.Get().Skills.resources)
        stringList.Add(resource.hat);
      string id = stringList[UnityEngine.Random.Range(0, stringList.Count)];
      UIDupeRandomizer.AddAccessory(dupe, this.slots.Hat.Lookup(id));
      dupe.SetSymbolVisiblity(Db.Get().AccessorySlots.Hair.targetSymbolId, false);
      dupe.SetSymbolVisiblity(Db.Get().AccessorySlots.HatHair.targetSymbolId, true);
    }
    else
    {
      dupe.SetSymbolVisiblity(Db.Get().AccessorySlots.Hair.targetSymbolId, true);
      dupe.SetSymbolVisiblity(Db.Get().AccessorySlots.HatHair.targetSymbolId, false);
      dupe.SetSymbolVisiblity(Db.Get().AccessorySlots.Hat.targetSymbolId, false);
    }
  }

  public static KAnimHashedString AddAccessory(
    KBatchedAnimController minion,
    Accessory accessory)
  {
    if (accessory == null)
      return (KAnimHashedString) HashedString.Invalid;
    SymbolOverrideController component = minion.GetComponent<SymbolOverrideController>();
    DebugUtil.Assert((UnityEngine.Object) component != (UnityEngine.Object) null, minion.name + " is missing symbol override controller");
    component.TryRemoveSymbolOverride((HashedString) accessory.slot.targetSymbolId, 0);
    component.AddSymbolOverride((HashedString) accessory.slot.targetSymbolId, accessory.symbol, 0);
    minion.SetSymbolVisiblity(accessory.slot.targetSymbolId, true);
    return accessory.slot.targetSymbolId;
  }

  public KAnimHashedString AddRandomAccessory(
    KBatchedAnimController minion,
    List<Accessory> choices)
  {
    Accessory choice = choices[UnityEngine.Random.Range(1, choices.Count)];
    return UIDupeRandomizer.AddAccessory(minion, choice);
  }

  protected virtual void Update()
  {
  }

  [Serializable]
  public struct AnimChoice
  {
    public string anim_name;
    public List<KBatchedAnimController> minions;
    public float minSecondsBetweenAction;
    public float maxSecondsBetweenAction;
    public float lastWaitTime;
    public KAnimFile curBody;
  }
}
