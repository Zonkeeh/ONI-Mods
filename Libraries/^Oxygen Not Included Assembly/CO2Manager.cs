// Decompiled with JetBrains decompiler
// Type: CO2Manager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class CO2Manager : KMonoBehaviour, ISim33ms
{
  private List<CO2> co2Items = new List<CO2>();
  private const float CO2Lifetime = 3f;
  [SerializeField]
  private Vector3 acceleration;
  [SerializeField]
  private CO2 prefab;
  [SerializeField]
  private GameObject breathPrefab;
  [SerializeField]
  private Color tintColour;
  private ObjectPool breathPool;
  private ObjectPool co2Pool;
  public static CO2Manager instance;

  public static void DestroyInstance()
  {
    CO2Manager.instance = (CO2Manager) null;
  }

  protected override void OnPrefabInit()
  {
    CO2Manager.instance = this;
    this.prefab.gameObject.SetActive(false);
    this.breathPrefab.SetActive(false);
    this.co2Pool = new ObjectPool(new Func<GameObject>(this.InstantiateCO2), 16);
    this.breathPool = new ObjectPool(new Func<GameObject>(this.InstantiateBreath), 16);
  }

  private GameObject InstantiateCO2()
  {
    GameObject gameObject = GameUtil.KInstantiate((Component) this.prefab, Grid.SceneLayer.Front, (string) null, 0);
    gameObject.SetActive(false);
    return gameObject;
  }

  private GameObject InstantiateBreath()
  {
    GameObject gameObject = GameUtil.KInstantiate(this.breathPrefab, Grid.SceneLayer.Front, (string) null, 0);
    gameObject.SetActive(false);
    return gameObject;
  }

  public void Sim33ms(float dt)
  {
    Vector2I xy1 = new Vector2I();
    Vector2I xy2 = new Vector2I();
    Vector3 vector3 = this.acceleration * dt;
    int count = this.co2Items.Count;
    for (int index1 = 0; index1 < count; ++index1)
    {
      CO2 co2Item = this.co2Items[index1];
      co2Item.velocity += vector3;
      co2Item.lifetimeRemaining -= dt;
      Grid.PosToXY(co2Item.transform.GetPosition(), out xy1);
      co2Item.transform.SetPosition(co2Item.transform.GetPosition() + co2Item.velocity * dt);
      Grid.PosToXY(co2Item.transform.GetPosition(), out xy2);
      int num = Grid.XYToCell(xy1.x, xy1.y);
      for (int y = xy1.y; y >= xy2.y; --y)
      {
        int cell = Grid.XYToCell(xy1.x, y);
        bool flag1 = !Grid.IsValidCell(cell) || (double) co2Item.lifetimeRemaining <= 0.0;
        if (!flag1)
        {
          Element element = Grid.Element[cell];
          flag1 = element.IsLiquid || element.IsSolid;
        }
        if (flag1)
        {
          bool flag2 = false;
          int index2;
          if (num != cell)
          {
            index2 = num;
            flag2 = true;
          }
          else
          {
            for (index2 = cell; Grid.IsValidCell(index2); index2 = Grid.CellAbove(index2))
            {
              Element element = Grid.Element[index2];
              if (!element.IsLiquid && !element.IsSolid)
              {
                flag2 = true;
                break;
              }
            }
          }
          co2Item.TriggerDestroy();
          if (flag2)
          {
            SimMessages.ModifyMass(index2, co2Item.mass, byte.MaxValue, 0, CellEventLogger.Instance.CO2ManagerFixedUpdate, co2Item.temperature, SimHashes.CarbonDioxide);
            --count;
            this.co2Items[index1] = this.co2Items[count];
            this.co2Items.RemoveAt(count);
            break;
          }
          DebugUtil.LogWarningArgs((object) "Couldn't emit CO2");
          break;
        }
        num = cell;
      }
    }
  }

  public void SpawnCO2(Vector3 position, float mass, float temperature)
  {
    position.z = Grid.GetLayerZ(Grid.SceneLayer.Front);
    GameObject instance = this.co2Pool.GetInstance();
    instance.transform.SetPosition(position);
    instance.SetActive(true);
    CO2 component1 = instance.GetComponent<CO2>();
    component1.mass = mass;
    component1.temperature = temperature;
    component1.velocity = Vector3.zero;
    component1.lifetimeRemaining = 3f;
    KBatchedAnimController component2 = component1.GetComponent<KBatchedAnimController>();
    component2.TintColour = (Color32) this.tintColour;
    component2.onDestroySelf = new System.Action<GameObject>(this.OnDestroyCO2);
    component1.StartLoop();
    this.co2Items.Add(component1);
  }

  public void SpawnBreath(Vector3 position, float mass, float temperature)
  {
    position.z = Grid.GetLayerZ(Grid.SceneLayer.Front);
    this.SpawnCO2(position, mass, temperature);
    GameObject instance = this.breathPool.GetInstance();
    instance.transform.SetPosition(position);
    instance.SetActive(true);
    KBatchedAnimController component = instance.GetComponent<KBatchedAnimController>();
    component.TintColour = (Color32) this.tintColour;
    component.onDestroySelf = new System.Action<GameObject>(this.OnDestroyBreath);
    component.Play((HashedString) "breath", KAnim.PlayMode.Once, 1f, 0.0f);
  }

  private void OnDestroyCO2(GameObject co2_go)
  {
    co2_go.SetActive(false);
    this.co2Pool.ReleaseInstance(co2_go);
  }

  private void OnDestroyBreath(GameObject breath_go)
  {
    breath_go.SetActive(false);
    this.breathPool.ReleaseInstance(breath_go);
  }
}
