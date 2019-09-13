// Decompiled with JetBrains decompiler
// Type: WorldDamage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMODUnity;
using STRINGS;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class WorldDamage : KMonoBehaviour
{
  private List<int> queuedDigCallbackCells = new List<int>();
  private float damageAmount = 0.0008333334f;
  private Dictionary<int, float> spawnTimes = new Dictionary<int, float>();
  private List<int> expiredCells = new List<int>();
  public KBatchedAnimController leakEffect;
  [SerializeField]
  private FMODAsset leakSound;
  [SerializeField]
  [EventRef]
  private string leakSoundMigrated;
  private const float SPAWN_DELAY = 1f;

  public static WorldDamage Instance { get; private set; }

  public static void DestroyInstance()
  {
    WorldDamage.Instance = (WorldDamage) null;
  }

  protected override void OnPrefabInit()
  {
    WorldDamage.Instance = this;
  }

  public void RestoreDamageToValue(int cell, float amount)
  {
    if ((double) Grid.Damage[cell] <= (double) amount)
      return;
    Grid.Damage[cell] = amount;
  }

  public float ApplyDamage(Sim.WorldDamageInfo damage_info)
  {
    return this.ApplyDamage(damage_info.gameCell, this.damageAmount, damage_info.damageSourceOffset, -1, (string) BUILDINGS.DAMAGESOURCES.LIQUID_PRESSURE, (string) UI.GAMEOBJECTEFFECTS.DAMAGE_POPS.LIQUID_PRESSURE);
  }

  public float ApplyDamage(
    int cell,
    float amount,
    int src_cell,
    int destroy_cb_index = -1,
    string source_name = null,
    string pop_text = null)
  {
    float num1 = 0.0f;
    if (Grid.Solid[cell])
    {
      float num2 = Grid.Damage[cell];
      num1 = Mathf.Min(amount, 1f - num2);
      float b = num2 + amount;
      bool flag = (double) b > 0.150000005960464;
      if (flag)
      {
        GameObject go = Grid.Objects[cell, 9];
        if ((Object) go != (Object) null)
        {
          BuildingHP component = go.GetComponent<BuildingHP>();
          if ((Object) component != (Object) null)
          {
            int num3 = Mathf.RoundToInt(Mathf.Max((float) component.HitPoints - (1f - b) * (float) component.MaxHitPoints, 0.0f));
            go.Trigger(-794517298, (object) new BuildingHP.DamageSourceInfo()
            {
              damage = num3,
              source = source_name,
              popString = pop_text
            });
          }
        }
      }
      Grid.Damage[cell] = Mathf.Min(1f, b);
      if ((double) Grid.Damage[cell] >= 1.0)
        this.DestroyCell(cell, destroy_cb_index);
      else if (Grid.IsValidCell(src_cell) && flag)
      {
        Element elem = Grid.Element[src_cell];
        if (elem.IsLiquid && (double) Grid.Mass[src_cell] > 1.0)
        {
          int offset = cell - src_cell;
          switch (offset)
          {
            case -1:
            case 1:
              int index = cell + offset;
              if (Grid.IsValidCell(index))
              {
                Element element = Grid.Element[index];
                if (!element.IsSolid && (!element.IsLiquid || element.id == elem.id && (double) Grid.Mass[index] <= 100.0) && (((int) Grid.Properties[index] & 2) == 0 && !this.spawnTimes.ContainsKey(index)))
                {
                  this.spawnTimes[index] = Time.realtimeSinceStartup;
                  int idx = (int) elem.idx;
                  float temperature = Grid.Temperature[src_cell];
                  this.StartCoroutine(this.DelayedSpawnFX(src_cell, index, offset, elem, idx, temperature));
                  break;
                }
                break;
              }
              break;
            default:
              if (offset == Grid.WidthInCells || offset == -Grid.WidthInCells)
                goto case -1;
              else
                break;
          }
        }
      }
    }
    return num1;
  }

  private void ReleaseGO(GameObject go)
  {
    go.DeleteObject();
  }

  [DebuggerHidden]
  private IEnumerator DelayedSpawnFX(
    int src_cell,
    int dest_cell,
    int offset,
    Element elem,
    int idx,
    float temperature)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new WorldDamage.\u003CDelayedSpawnFX\u003Ec__Iterator0()
    {
      dest_cell = dest_cell,
      elem = elem,
      src_cell = src_cell,
      idx = idx,
      temperature = temperature,
      offset = offset,
      \u0024this = this
    };
  }

  private void Update()
  {
    this.expiredCells.Clear();
    float realtimeSinceStartup = Time.realtimeSinceStartup;
    foreach (KeyValuePair<int, float> spawnTime in this.spawnTimes)
    {
      if ((double) realtimeSinceStartup - (double) spawnTime.Value > 1.0)
        this.expiredCells.Add(spawnTime.Key);
    }
    foreach (int expiredCell in this.expiredCells)
      this.spawnTimes.Remove(expiredCell);
    this.expiredCells.Clear();
  }

  public void DestroyCell(int cell, int cb_index = -1)
  {
    if (!Grid.Solid[cell])
      return;
    if (cb_index == -1)
    {
      if (this.queuedDigCallbackCells.Contains(cell))
        return;
      this.queuedDigCallbackCells.Add(cell);
      SimMessages.Dig(cell, -1);
    }
    else
      SimMessages.Dig(cell, cb_index);
  }

  public void OnSolidStateChanged(int cell)
  {
    Grid.Damage[cell] = 0.0f;
  }

  public void OnDigComplete(
    int cell,
    float mass,
    float temperature,
    byte element_idx,
    byte disease_idx,
    int disease_count)
  {
    if (!this.queuedDigCallbackCells.Contains(cell))
      return;
    this.queuedDigCallbackCells.Remove(cell);
    Vector3 pos = Grid.CellToPos(cell, CellAlignment.RandomInternal, Grid.SceneLayer.Ore);
    Element element = ElementLoader.elements[(int) element_idx];
    Grid.Damage[cell] = 0.0f;
    WorldDamage.Instance.PlaySoundForSubstance(element, pos);
    float num = mass * 0.5f;
    if ((double) num <= 0.0)
      return;
    GameObject gameObject = element.substance.SpawnResource(pos, num, temperature, disease_idx, disease_count, false, false, false);
    if (!((Object) gameObject.GetComponent<Pickupable>() != (Object) null) || !WorldInventory.Instance.IsReachable(gameObject.GetComponent<Pickupable>()))
      return;
    PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, Mathf.RoundToInt(num).ToString() + " " + element.name, gameObject.transform, 1.5f, false);
  }

  private void PlaySoundForSubstance(Element element, Vector3 pos)
  {
    string sound = GlobalAssets.GetSound("Break_" + (element.substance.GetMiningBreakSound() ?? (!element.HasTag(GameTags.RefinedMetal) ? (!element.HasTag(GameTags.Metal) ? "Rock" : "RawMetal") : "RefinedMetal")), false);
    if (!(bool) ((Object) CameraController.Instance) || !CameraController.Instance.IsAudibleSound(pos, sound))
      return;
    KFMOD.PlayOneShot(sound, CameraController.Instance.GetVerticallyScaledPosition((Vector2) pos));
  }
}
