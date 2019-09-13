// Decompiled with JetBrains decompiler
// Type: SuitEquipper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class SuitEquipper : KMonoBehaviour
{
  private static readonly EventSystem.IntraObjectHandler<SuitEquipper> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<SuitEquipper>((System.Action<SuitEquipper, object>) ((component, data) => component.OnRefreshUserMenu(data)));

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Subscribe<SuitEquipper>(493375141, SuitEquipper.OnRefreshUserMenuDelegate);
  }

  private void OnRefreshUserMenu(object data)
  {
    foreach (EquipmentSlotInstance slot in this.GetComponent<MinionIdentity>().GetEquipment().Slots)
    {
      Equippable equippable = slot.assignable as Equippable;
      if ((bool) ((UnityEngine.Object) equippable))
        Game.Instance.userMenu.AddButton(this.gameObject, new KIconButtonMenu.ButtonInfo("iconDown", string.Format((string) UI.USERMENUACTIONS.UNEQUIP.NAME, (object) equippable.def.GenericName), (System.Action) (() => equippable.Unassign()), Action.NumActions, (System.Action<GameObject>) null, (System.Action<KIconButtonMenu.ButtonInfo>) null, (Texture) null, string.Empty, true), 2f);
    }
  }

  public Equippable IsWearingAirtightSuit()
  {
    Equippable equippable = (Equippable) null;
    foreach (AssignableSlotInstance slot in this.GetComponent<MinionIdentity>().GetEquipment().Slots)
    {
      Equippable assignable = slot.assignable as Equippable;
      if ((bool) ((UnityEngine.Object) assignable) && assignable.GetComponent<KPrefabID>().HasTag(GameTags.AirtightSuit))
      {
        equippable = assignable;
        break;
      }
    }
    return equippable;
  }
}
