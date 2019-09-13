// Decompiled with JetBrains decompiler
// Type: ItemPedestal
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using UnityEngine;

public class ItemPedestal : KMonoBehaviour
{
  private static readonly EventSystem.IntraObjectHandler<ItemPedestal> OnOccupantChangedDelegate = new EventSystem.IntraObjectHandler<ItemPedestal>((System.Action<ItemPedestal, object>) ((component, data) => component.OnOccupantChanged(data)));
  [MyCmpReq]
  private SingleEntityReceptacle receptacle;
  [MyCmpReq]
  private DecorProvider decorProvider;
  private const float MINIMUM_DECOR = 5f;
  private const float STORED_DECOR_MODIFIER = 2f;
  private const int RADIUS_BONUS = 2;
  private AttributeModifier decorModifier;
  private AttributeModifier decorRadiusModifier;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Subscribe<ItemPedestal>(-731304873, ItemPedestal.OnOccupantChangedDelegate);
    if (!(bool) ((UnityEngine.Object) this.receptacle.Occupant))
      return;
    KBatchedAnimController component = this.receptacle.Occupant.GetComponent<KBatchedAnimController>();
    if ((bool) ((UnityEngine.Object) component))
      component.enabled = true;
    this.OnOccupantChanged((object) this.receptacle.Occupant);
  }

  private void OnOccupantChanged(object data)
  {
    Attributes attributes = this.GetAttributes();
    if (this.decorModifier != null)
    {
      attributes.Remove(this.decorModifier);
      attributes.Remove(this.decorRadiusModifier);
      this.decorModifier = (AttributeModifier) null;
      this.decorRadiusModifier = (AttributeModifier) null;
    }
    if (data == null)
      return;
    GameObject go = (GameObject) data;
    DecorProvider component = go.GetComponent<DecorProvider>();
    float num1 = 5f;
    float num2 = 3f;
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
    {
      num1 = Mathf.Max(Db.Get().BuildingAttributes.Decor.Lookup(go).GetTotalValue() * 2f, 5f);
      num2 = Db.Get().BuildingAttributes.DecorRadius.Lookup(go).GetTotalValue() + 2f;
    }
    string description = string.Format((string) BUILDINGS.PREFABS.ITEMPEDESTAL.DISPLAYED_ITEM_FMT, (object) go.GetComponent<KPrefabID>().PrefabTag.ProperName());
    this.decorModifier = new AttributeModifier(Db.Get().BuildingAttributes.Decor.Id, num1, description, false, false, true);
    this.decorRadiusModifier = new AttributeModifier(Db.Get().BuildingAttributes.DecorRadius.Id, num2, description, false, false, true);
    attributes.Add(this.decorModifier);
    attributes.Add(this.decorRadiusModifier);
  }
}
