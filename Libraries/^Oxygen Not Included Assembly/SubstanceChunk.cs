// Decompiled with JetBrains decompiler
// Type: SubstanceChunk
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using UnityEngine;

[SkipSaveFileSerialization]
[SerializationConfig(MemberSerialization.OptIn)]
public class SubstanceChunk : KMonoBehaviour, ISaveLoadable
{
  private static readonly KAnimHashedString symbolToTint = new KAnimHashedString("substance_tinter");

  protected override void OnSpawn()
  {
    base.OnSpawn();
    Color colour = (Color) this.GetComponent<PrimaryElement>().Element.substance.colour;
    colour.a = 1f;
    this.GetComponent<KBatchedAnimController>().SetSymbolTint(SubstanceChunk.symbolToTint, colour);
  }

  private void OnRefreshUserMenu(object data)
  {
    Game.Instance.userMenu.AddButton(this.gameObject, new KIconButtonMenu.ButtonInfo("action_deconstruct", (string) UI.USERMENUACTIONS.RELEASEELEMENT.NAME, new System.Action(this.OnRelease), Action.NumActions, (System.Action<GameObject>) null, (System.Action<KIconButtonMenu.ButtonInfo>) null, (Texture) null, (string) UI.USERMENUACTIONS.RELEASEELEMENT.TOOLTIP, true), 1f);
  }

  private void OnRelease()
  {
    int cell = Grid.PosToCell(this.transform.GetPosition());
    PrimaryElement component = this.GetComponent<PrimaryElement>();
    if ((double) component.Mass > 0.0)
      SimMessages.AddRemoveSubstance(cell, component.ElementID, CellEventLogger.Instance.ExhaustSimUpdate, component.Mass, component.Temperature, component.DiseaseIdx, component.DiseaseCount, true, -1);
    this.gameObject.DeleteObject();
  }
}
