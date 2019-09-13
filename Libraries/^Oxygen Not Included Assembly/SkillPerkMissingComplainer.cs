// Decompiled with JetBrains decompiler
// Type: SkillPerkMissingComplainer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;

public class SkillPerkMissingComplainer : KMonoBehaviour
{
  private int skillUpdateHandle = -1;
  public string requiredSkillPerk;
  private Guid workStatusItemHandle;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    if (!string.IsNullOrEmpty(this.requiredSkillPerk))
      this.skillUpdateHandle = Game.Instance.Subscribe(-1523247426, new System.Action<object>(this.UpdateStatusItem));
    this.UpdateStatusItem((object) null);
  }

  protected override void OnCleanUp()
  {
    if (this.skillUpdateHandle != -1)
      Game.Instance.Unsubscribe(this.skillUpdateHandle);
    base.OnCleanUp();
  }

  protected virtual void UpdateStatusItem(object data = null)
  {
    KSelectable component = this.GetComponent<KSelectable>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null || string.IsNullOrEmpty(this.requiredSkillPerk))
      return;
    bool flag = MinionResume.AnyMinionHasPerk(this.requiredSkillPerk);
    if (!flag && this.workStatusItemHandle == Guid.Empty)
    {
      this.workStatusItemHandle = component.AddStatusItem(Db.Get().BuildingStatusItems.ColonyLacksRequiredSkillPerk, (object) this.requiredSkillPerk);
    }
    else
    {
      if (!flag || !(this.workStatusItemHandle != Guid.Empty))
        return;
      component.RemoveStatusItem(this.workStatusItemHandle, false);
      this.workStatusItemHandle = Guid.Empty;
    }
  }
}
