// Decompiled with JetBrains decompiler
// Type: DecorProvider
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

[SkipSaveFileSerialization]
public class DecorProvider : KMonoBehaviour, IEffectDescriptor, IGameObjectEffectDescriptor
{
  private int[] cells = new int[512];
  public const string ID = "DecorProvider";
  private int width;
  private int height;
  private int previousDecor;
  public float baseRadius;
  public float baseDecor;
  public string overrideName;
  private HandleVector<int>.Handle partitionerEntry;
  public System.Action refreshCallback;
  public System.Action<object> refreshPartionerCallback;
  public System.Action<object> onCollectDecorProvidersCallback;
  public AttributeInstance decor;
  public AttributeInstance decorRadius;
  private AttributeModifier baseDecorModifier;
  private AttributeModifier baseDecorRadiusModifier;
  public bool isMovable;
  [MyCmpReq]
  public OccupyArea occupyArea;
  [MyCmpGet]
  public Pickupable pickupable;
  [MyCmpGet]
  public Rotatable rotatable;
  [MyCmpGet]
  public SimCellOccupier simCellOccupier;
  private int cellCount;
  [MyCmpReq]
  private Modifiers modifiers;
  private DecorProvider.Splat splat;

  public void Refresh()
  {
    this.splat.Clear();
    this.splat = new DecorProvider.Splat(this);
    KPrefabID component = this.GetComponent<KPrefabID>();
    bool flag1 = component.HasTag(RoomConstraints.ConstraintTags.Decor20);
    bool flag2 = (double) this.decor.GetTotalValue() >= 20.0;
    if (flag1 == flag2)
      return;
    if (flag2)
      component.AddTag(RoomConstraints.ConstraintTags.Decor20, false);
    else
      component.RemoveTag(RoomConstraints.ConstraintTags.Decor20);
    Game.Instance.roomProber.SolidChangedEvent(Grid.PosToCell((KMonoBehaviour) this), true);
  }

  public float GetDecorForCell(int cell)
  {
    for (int index = 0; index < this.cellCount; ++index)
    {
      if (this.cells[index] == cell)
        return this.splat.decor;
    }
    return 0.0f;
  }

  public void SetValues(EffectorValues values)
  {
    this.baseDecor = (float) values.amount;
    this.baseRadius = (float) values.radius;
    if (!this.IsInitialized())
      return;
    this.UpdateBaseDecorModifiers();
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.decor = this.GetAttributes().Add(Db.Get().BuildingAttributes.Decor);
    this.decorRadius = this.GetAttributes().Add(Db.Get().BuildingAttributes.DecorRadius);
    this.UpdateBaseDecorModifiers();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.refreshCallback = new System.Action(this.Refresh);
    this.refreshPartionerCallback = (System.Action<object>) (data => this.Refresh());
    this.onCollectDecorProvidersCallback = new System.Action<object>(this.OnCollectDecorProviders);
    KBatchedAnimController component = this.GetComponent<KBatchedAnimController>();
    this.isMovable = (UnityEngine.Object) component != (UnityEngine.Object) null && component.isMovable;
    Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(this.transform, new System.Action(this.OnCellChange), "DecorProvider.OnSpawn");
    this.decor.OnDirty += this.refreshCallback;
    this.decorRadius.OnDirty += this.refreshCallback;
    this.Refresh();
  }

  private void UpdateBaseDecorModifiers()
  {
    Attributes attributes = this.GetAttributes();
    if (this.baseDecorModifier != null)
    {
      attributes.Remove(this.baseDecorModifier);
      attributes.Remove(this.baseDecorRadiusModifier);
      this.baseDecorModifier = (AttributeModifier) null;
      this.baseDecorRadiusModifier = (AttributeModifier) null;
    }
    if ((double) this.baseDecor == 0.0)
      return;
    this.baseDecorModifier = new AttributeModifier(Db.Get().BuildingAttributes.Decor.Id, this.baseDecor, (string) UI.TOOLTIPS.BASE_VALUE, false, false, true);
    this.baseDecorRadiusModifier = new AttributeModifier(Db.Get().BuildingAttributes.DecorRadius.Id, this.baseRadius, (string) UI.TOOLTIPS.BASE_VALUE, false, false, true);
    attributes.Add(this.baseDecorModifier);
    attributes.Add(this.baseDecorRadiusModifier);
  }

  private void OnCellChange()
  {
    this.Refresh();
  }

  private void OnCollectDecorProviders(object data)
  {
    ((List<DecorProvider>) data).Add(this);
  }

  public string GetName()
  {
    if (string.IsNullOrEmpty(this.overrideName))
      return this.GetComponent<KSelectable>().GetName();
    return this.overrideName;
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    if (this.isSpawned)
    {
      this.decor.OnDirty -= this.refreshCallback;
      this.decorRadius.OnDirty -= this.refreshCallback;
      Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(this.transform, new System.Action(this.OnCellChange));
    }
    this.splat.Clear();
  }

  public List<Descriptor> GetEffectDescriptions()
  {
    List<Descriptor> descriptorList = new List<Descriptor>();
    if (this.decor != null && this.decorRadius != null)
    {
      float totalValue1 = this.decor.GetTotalValue();
      float totalValue2 = this.decorRadius.GetTotalValue();
      string str1 = (double) this.baseDecor <= 0.0 ? "consumed" : "produced";
      string format = (string) ((double) this.baseDecor <= 0.0 ? UI.BUILDINGEFFECTS.TOOLTIPS.DECORDECREASED : UI.BUILDINGEFFECTS.TOOLTIPS.DECORPROVIDED) + "\n\n" + this.decor.GetAttributeValueTooltip();
      string str2 = GameUtil.AddPositiveSign(totalValue1.ToString(), (double) totalValue1 > 0.0);
      Descriptor descriptor = new Descriptor(string.Format((string) UI.BUILDINGEFFECTS.DECORPROVIDED, (object) str1, (object) str2, (object) totalValue2), string.Format(format, (object) str2, (object) totalValue2), Descriptor.DescriptorType.Effect, false);
      descriptorList.Add(descriptor);
    }
    else if ((double) this.baseDecor != 0.0)
    {
      string str1 = (double) this.baseDecor < 0.0 ? "consumed" : "produced";
      string format = (string) ((double) this.baseDecor < 0.0 ? UI.BUILDINGEFFECTS.TOOLTIPS.DECORDECREASED : UI.BUILDINGEFFECTS.TOOLTIPS.DECORPROVIDED);
      string str2 = GameUtil.AddPositiveSign(this.baseDecor.ToString(), (double) this.baseDecor > 0.0);
      Descriptor descriptor = new Descriptor(string.Format((string) UI.BUILDINGEFFECTS.DECORPROVIDED, (object) str1, (object) str2, (object) this.baseRadius), string.Format(format, (object) str2, (object) this.baseRadius), Descriptor.DescriptorType.Effect, false);
      descriptorList.Add(descriptor);
    }
    return descriptorList;
  }

  public static int GetLightDecorBonus(int cell)
  {
    if (Grid.LightIntensity[cell] > 0)
      return TUNING.DECOR.LIT_BONUS;
    return 0;
  }

  public List<Descriptor> GetDescriptors(BuildingDef def)
  {
    return this.GetEffectDescriptions();
  }

  public List<Descriptor> GetDescriptors(GameObject go)
  {
    return this.GetEffectDescriptions();
  }

  private struct Splat
  {
    private DecorProvider provider;
    private Extents extents;
    private HandleVector<int>.Handle partitionerEntry;
    private HandleVector<int>.Handle solidChangedPartitionerEntry;

    public unsafe Splat(DecorProvider provider)
    {
      *(DecorProvider.Splat*) ref this = new DecorProvider.Splat();
      AttributeInstance decor = provider.decor;
      this.decor = 0.0f;
      if (decor != null)
        this.decor = decor.GetTotalValue();
      if (provider.HasTag(GameTags.Stored))
        this.decor = 0.0f;
      int cell = Grid.PosToCell(provider.gameObject);
      if (!Grid.IsValidCell(cell))
        return;
      if (!Grid.Transparent[cell] && Grid.Solid[cell] && (UnityEngine.Object) provider.simCellOccupier == (UnityEngine.Object) null)
        this.decor = 0.0f;
      if ((double) this.decor == 0.0)
        return;
      provider.cellCount = 0;
      this.provider = provider;
      int num = 5;
      AttributeInstance decorRadius = provider.decorRadius;
      if (decorRadius != null)
        num = (int) decorRadius.GetTotalValue();
      Orientation orientation = Orientation.Neutral;
      if ((bool) ((UnityEngine.Object) provider.rotatable))
        orientation = provider.rotatable.GetOrientation();
      this.extents = provider.occupyArea.GetExtents(orientation);
      this.extents.x = Mathf.Max(this.extents.x - num, 0);
      this.extents.y = Mathf.Max(this.extents.y - num, 0);
      this.extents.width = Mathf.Min(this.extents.width + num * 2, Grid.WidthInCells - 1);
      this.extents.height = Mathf.Min(this.extents.height + num * 2, Grid.HeightInCells - 1);
      this.partitionerEntry = GameScenePartitioner.Instance.Add("DecorProvider.SplatCollectDecorProviders", (object) provider.gameObject, this.extents, GameScenePartitioner.Instance.decorProviderLayer, provider.onCollectDecorProvidersCallback);
      this.solidChangedPartitionerEntry = GameScenePartitioner.Instance.Add("DecorProvider.SplatSolidCheck", (object) provider.gameObject, this.extents, GameScenePartitioner.Instance.solidChangedLayer, provider.refreshPartionerCallback);
      this.AddDecor();
    }

    public float decor { get; private set; }

    public void Clear()
    {
      if ((double) this.decor == 0.0)
        return;
      this.RemoveDecor();
      GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
      GameScenePartitioner.Instance.Free(ref this.solidChangedPartitionerEntry);
    }

    private void AddDecor()
    {
      int cell1 = Grid.PosToCell((KMonoBehaviour) this.provider);
      int val1_1 = this.extents.x + this.extents.width;
      int val1_2 = this.extents.y + this.extents.height;
      int x1 = this.extents.x;
      int y1 = this.extents.y;
      int x2 = 0;
      int y2 = 0;
      Grid.CellToXY(cell1, out x2, out y2);
      int num1 = Math.Min(val1_1, Grid.WidthInCells);
      int num2 = Math.Min(val1_2, Grid.HeightInCells);
      int num3 = Math.Max(0, x1);
      int num4 = Math.Max(0, y1);
      for (int index1 = num3; index1 < num1; ++index1)
      {
        for (int index2 = num4; index2 < num2; ++index2)
        {
          if (Grid.VisibilityTest(x2, y2, index1, index2, false))
          {
            int cell2 = Grid.XYToCell(index1, index2);
            if (Grid.IsValidCell(cell2))
            {
              Grid.Decor[cell2] += this.decor;
              if (this.provider.cellCount >= 0 && this.provider.cellCount < this.provider.cells.Length)
                this.provider.cells[this.provider.cellCount++] = cell2;
            }
          }
        }
      }
    }

    private void RemoveDecor()
    {
      if ((double) this.decor == 0.0 || (UnityEngine.Object) this.provider == (UnityEngine.Object) null)
        return;
      for (int index = 0; index < this.provider.cellCount; ++index)
      {
        int cell = this.provider.cells[index];
        if (Grid.IsValidCell(cell))
          Grid.Decor[cell] -= this.decor;
      }
    }
  }
}
