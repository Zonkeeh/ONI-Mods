// Decompiled with JetBrains decompiler
// Type: CargoBay
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System.Collections.Generic;
using UnityEngine;

public class CargoBay : KMonoBehaviour
{
  private static readonly EventSystem.IntraObjectHandler<CargoBay> OnLaunchDelegate = new EventSystem.IntraObjectHandler<CargoBay>((System.Action<CargoBay, object>) ((component, data) => component.OnLaunch(data)));
  private static readonly EventSystem.IntraObjectHandler<CargoBay> OnLandDelegate = new EventSystem.IntraObjectHandler<CargoBay>((System.Action<CargoBay, object>) ((component, data) => component.OnLand(data)));
  private static readonly EventSystem.IntraObjectHandler<CargoBay> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<CargoBay>((System.Action<CargoBay, object>) ((component, data) => component.OnRefreshUserMenu(data)));
  public Storage storage;
  private MeterController meter;
  public CargoBay.CargoType storageType;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.GetComponent<KBatchedAnimController>().Play((HashedString) "grounded", KAnim.PlayMode.Loop, 1f, 0.0f);
    this.Subscribe<CargoBay>(-1056989049, CargoBay.OnLaunchDelegate);
    this.Subscribe<CargoBay>(238242047, CargoBay.OnLandDelegate);
    this.Subscribe<CargoBay>(493375141, CargoBay.OnRefreshUserMenuDelegate);
    this.meter = new MeterController((KAnimControllerBase) this.GetComponent<KBatchedAnimController>(), "meter_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, new string[4]
    {
      "meter_target",
      "meter_fill",
      "meter_frame",
      "meter_OL"
    });
    this.Subscribe(-1697596308, (System.Action<object>) (data => this.meter.SetPositionPercent(this.storage.MassStored() / this.storage.Capacity())));
  }

  private void OnRefreshUserMenu(object data)
  {
    Game.Instance.userMenu.AddButton(this.gameObject, new KIconButtonMenu.ButtonInfo("action_empty_contents", (string) UI.USERMENUACTIONS.EMPTYSTORAGE.NAME, (System.Action) (() => this.storage.DropAll(false, false, new Vector3(), true)), Action.NumActions, (System.Action<GameObject>) null, (System.Action<KIconButtonMenu.ButtonInfo>) null, (Texture) null, (string) UI.USERMENUACTIONS.EMPTYSTORAGE.TOOLTIP, true), 1f);
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
  }

  public void SpawnResources(object data)
  {
    SpaceDestination spacecraftDestination = SpacecraftManager.instance.GetSpacecraftDestination(SpacecraftManager.instance.GetSpacecraftID(this.GetComponent<RocketModule>().conditionManager.GetComponent<LaunchableRocket>()));
    int cell = Grid.PosToCell(this.gameObject);
    foreach (KeyValuePair<SimHashes, float> keyValuePair in spacecraftDestination.GetMissionResourceResult(this.storage.RemainingCapacity(), this.storageType == CargoBay.CargoType.solids, this.storageType == CargoBay.CargoType.liquids, this.storageType == CargoBay.CargoType.gasses))
    {
      Element elementByHash = ElementLoader.FindElementByHash(keyValuePair.Key);
      if (this.storageType == CargoBay.CargoType.solids && elementByHash.IsSolid)
      {
        GameObject go = Scenario.SpawnPrefab(cell, 0, 0, elementByHash.tag.Name, Grid.SceneLayer.Ore);
        go.GetComponent<PrimaryElement>().Mass = keyValuePair.Value;
        go.GetComponent<PrimaryElement>().Temperature = ElementLoader.FindElementByHash(keyValuePair.Key).defaultValues.temperature;
        go.SetActive(true);
        this.storage.Store(go, false, false, true, false);
      }
      else if (this.storageType == CargoBay.CargoType.liquids && elementByHash.IsLiquid)
        this.storage.AddLiquid(keyValuePair.Key, keyValuePair.Value, ElementLoader.FindElementByHash(keyValuePair.Key).defaultValues.temperature, byte.MaxValue, 0, false, true);
      else if (this.storageType == CargoBay.CargoType.gasses && elementByHash.IsGas)
        this.storage.AddGasChunk(keyValuePair.Key, keyValuePair.Value, ElementLoader.FindElementByHash(keyValuePair.Key).defaultValues.temperature, byte.MaxValue, 0, false, true);
    }
    if (this.storageType != CargoBay.CargoType.entities)
      return;
    foreach (KeyValuePair<Tag, int> keyValuePair in spacecraftDestination.GetMissionEntityResult())
    {
      GameObject prefab = Assets.GetPrefab(keyValuePair.Key);
      if ((UnityEngine.Object) prefab == (UnityEngine.Object) null)
      {
        KCrashReporter.Assert(false, "Missing prefab: " + keyValuePair.Key.Name);
      }
      else
      {
        for (int index = 0; index < keyValuePair.Value; ++index)
        {
          GameObject go = Util.KInstantiate(prefab, this.transform.position);
          go.SetActive(true);
          this.storage.Store(go, false, false, true, false);
          Baggable component = go.GetComponent<Baggable>();
          if ((UnityEngine.Object) component != (UnityEngine.Object) null)
            component.SetWrangled();
        }
      }
    }
  }

  public void OnLaunch(object data)
  {
    this.ReserveResources();
    ConduitDispenser component = this.GetComponent<ConduitDispenser>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    component.conduitType = ConduitType.None;
  }

  private void ReserveResources()
  {
    int spacecraftId = SpacecraftManager.instance.GetSpacecraftID(this.GetComponent<RocketModule>().conditionManager.GetComponent<LaunchableRocket>());
    SpacecraftManager.instance.GetSpacecraftDestination(spacecraftId).UpdateRemainingResources(this);
  }

  public void OnLand(object data)
  {
    this.SpawnResources(data);
    ConduitDispenser component = this.GetComponent<ConduitDispenser>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
      return;
    switch (this.storageType)
    {
      case CargoBay.CargoType.liquids:
        component.conduitType = ConduitType.Liquid;
        break;
      case CargoBay.CargoType.gasses:
        component.conduitType = ConduitType.Gas;
        break;
      default:
        component.conduitType = ConduitType.None;
        break;
    }
  }

  public enum CargoType
  {
    solids,
    liquids,
    gasses,
    entities,
  }
}
