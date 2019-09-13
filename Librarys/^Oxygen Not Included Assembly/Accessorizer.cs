// Decompiled with JetBrains decompiler
// Type: Accessorizer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

public class Accessorizer : KMonoBehaviour
{
  [Serialize]
  private List<ResourceRef<Accessory>> accessories = new List<ResourceRef<Accessory>>();
  [MyCmpReq]
  private KAnimControllerBase animController;

  public List<ResourceRef<Accessory>> GetAccessories()
  {
    return this.accessories;
  }

  public void SetAccessories(List<ResourceRef<Accessory>> data)
  {
    this.accessories = data;
  }

  [OnDeserialized]
  private void OnDeserialized()
  {
    this.ApplyAccessories();
  }

  public void AddAccessory(Accessory accessory)
  {
    if (accessory == null)
      return;
    this.animController.GetComponent<SymbolOverrideController>().AddSymbolOverride((HashedString) accessory.slot.targetSymbolId, accessory.symbol, 0);
    if (this.HasAccessory(accessory))
      return;
    ResourceRef<Accessory> resourceRef = new ResourceRef<Accessory>(accessory);
    if (resourceRef == null)
      return;
    this.accessories.Add(resourceRef);
  }

  public void RemoveAccessory(Accessory accessory)
  {
    this.accessories.RemoveAll((Predicate<ResourceRef<Accessory>>) (x => x.Get() == accessory));
    this.animController.GetComponent<SymbolOverrideController>().TryRemoveSymbolOverride((HashedString) accessory.slot.targetSymbolId, 0);
  }

  public void ApplyAccessories()
  {
    foreach (ResourceRef<Accessory> accessory1 in this.accessories)
    {
      Accessory accessory2 = accessory1.Get();
      if (accessory2 != null)
        this.AddAccessory(accessory2);
    }
  }

  public bool HasAccessory(Accessory accessory)
  {
    return this.accessories.Exists((Predicate<ResourceRef<Accessory>>) (x => x.Get() == accessory));
  }

  public Accessory GetAccessory(AccessorySlot slot)
  {
    for (int index = 0; index < this.accessories.Count; ++index)
    {
      if (this.accessories[index].Get() != null && this.accessories[index].Get().slot == slot)
        return this.accessories[index].Get();
    }
    return (Accessory) null;
  }

  public void GetBodySlots(ref KCompBuilder.BodyData fd)
  {
    fd.eyes = HashedString.Invalid;
    fd.hair = HashedString.Invalid;
    fd.headShape = HashedString.Invalid;
    fd.mouth = HashedString.Invalid;
    fd.neck = HashedString.Invalid;
    fd.body = HashedString.Invalid;
    fd.arms = HashedString.Invalid;
    fd.hat = HashedString.Invalid;
    for (int index = 0; index < this.accessories.Count; ++index)
    {
      Accessory accessory = this.accessories[index].Get();
      if (accessory != null)
      {
        if (accessory.slot.Id == "Eyes")
          fd.eyes = accessory.IdHash;
        else if (accessory.slot.Id == "Hair")
          fd.hair = accessory.IdHash;
        else if (accessory.slot.Id == "HeadShape")
          fd.headShape = accessory.IdHash;
        else if (accessory.slot.Id == "Mouth")
          fd.mouth = accessory.IdHash;
        else if (accessory.slot.Id == "Neck")
          fd.neck = accessory.IdHash;
        else if (accessory.slot.Id == "Body")
          fd.body = accessory.IdHash;
        else if (accessory.slot.Id == "Arm")
          fd.arms = accessory.IdHash;
        else if (accessory.slot.Id == "Hat")
          fd.hat = HashedString.Invalid;
      }
    }
  }
}
