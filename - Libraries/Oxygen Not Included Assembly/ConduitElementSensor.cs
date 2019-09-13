// Decompiled with JetBrains decompiler
// Type: ConduitElementSensor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;

[SerializationConfig(MemberSerialization.OptIn)]
public class ConduitElementSensor : ConduitSensor
{
  private SimHashes desiredElement = SimHashes.Void;
  [MyCmpGet]
  private Filterable filterable;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.filterable.onFilterChanged += new System.Action<Tag>(this.OnFilterChanged);
    this.OnFilterChanged(this.filterable.SelectedTag);
  }

  private void OnFilterChanged(Tag tag)
  {
    this.desiredElement = SimHashes.Void;
    if (!tag.IsValid)
      return;
    Element element = ElementLoader.GetElement(tag);
    bool on = true;
    if (element != null)
    {
      this.desiredElement = element.id;
      on = this.desiredElement == SimHashes.Void || this.desiredElement == SimHashes.Vacuum;
    }
    this.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.NoFilterElementSelected, on, (object) null);
  }

  protected override void ConduitUpdate(float dt)
  {
    ConduitFlow.ConduitContents contents = Conduit.GetFlowManager(this.conduitType).GetContents(Grid.PosToCell(this.transform.GetPosition()));
    if (this.IsSwitchedOn)
    {
      if (contents.element == this.desiredElement)
        return;
      this.Toggle();
    }
    else
    {
      if (contents.element != this.desiredElement)
        return;
      this.Toggle();
    }
  }
}
