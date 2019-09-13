// Decompiled with JetBrains decompiler
// Type: SimpleVent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class SimpleVent : KMonoBehaviour
{
  private static readonly EventSystem.IntraObjectHandler<SimpleVent> OnChangedDelegate = new EventSystem.IntraObjectHandler<SimpleVent>((System.Action<SimpleVent, object>) ((component, data) => component.OnChanged(data)));
  [MyCmpGet]
  private Operational operational;

  protected override void OnPrefabInit()
  {
    this.Subscribe<SimpleVent>(-592767678, SimpleVent.OnChangedDelegate);
    this.Subscribe<SimpleVent>(-111137758, SimpleVent.OnChangedDelegate);
  }

  protected override void OnSpawn()
  {
    this.OnChanged((object) null);
  }

  private void OnChanged(object data)
  {
    if (this.operational.IsFunctional)
      this.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.Normal, (object) this);
    else
      this.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, (StatusItem) null, (object) null);
  }
}
