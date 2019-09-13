// Decompiled with JetBrains decompiler
// Type: TreeBud
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using UnityEngine;

public class TreeBud : KMonoBehaviour, IWiltCause
{
  private static StandardCropPlant.AnimSet[] animSets = new StandardCropPlant.AnimSet[7]
  {
    new StandardCropPlant.AnimSet()
    {
      grow = "branch_a_grow",
      grow_pst = "branch_a_grow_pst",
      idle_full = "branch_a_idle_full",
      wilt_base = "branch_a_wilt",
      harvest = "branch_a_harvest"
    },
    new StandardCropPlant.AnimSet()
    {
      grow = "branch_b_grow",
      grow_pst = "branch_b_grow_pst",
      idle_full = "branch_b_idle_full",
      wilt_base = "branch_b_wilt",
      harvest = "branch_b_harvest"
    },
    new StandardCropPlant.AnimSet()
    {
      grow = "branch_c_grow",
      grow_pst = "branch_c_grow_pst",
      idle_full = "branch_c_idle_full",
      wilt_base = "branch_c_wilt",
      harvest = "branch_c_harvest"
    },
    new StandardCropPlant.AnimSet()
    {
      grow = "branch_d_grow",
      grow_pst = "branch_d_grow_pst",
      idle_full = "branch_d_idle_full",
      wilt_base = "branch_d_wilt",
      harvest = "branch_d_harvest"
    },
    new StandardCropPlant.AnimSet()
    {
      grow = "branch_e_grow",
      grow_pst = "branch_e_grow_pst",
      idle_full = "branch_e_idle_full",
      wilt_base = "branch_e_wilt",
      harvest = "branch_e_harvest"
    },
    new StandardCropPlant.AnimSet()
    {
      grow = "branch_f_grow",
      grow_pst = "branch_f_grow_pst",
      idle_full = "branch_f_idle_full",
      wilt_base = "branch_f_wilt",
      harvest = "branch_f_harvest"
    },
    new StandardCropPlant.AnimSet()
    {
      grow = "branch_g_grow",
      grow_pst = "branch_g_grow_pst",
      idle_full = "branch_g_idle_full",
      wilt_base = "branch_g_wilt",
      harvest = "branch_g_harvest"
    }
  };
  private static Vector3[] animOffset = new Vector3[7]
  {
    new Vector3(1f, 0.0f, 0.0f),
    new Vector3(1f, -1f, 0.0f),
    new Vector3(1f, -2f, 0.0f),
    new Vector3(0.0f, -2f, 0.0f),
    new Vector3(-1f, -2f, 0.0f),
    new Vector3(-1f, -1f, 0.0f),
    new Vector3(-1f, 0.0f, 0.0f)
  };
  private static readonly EventSystem.IntraObjectHandler<TreeBud> OnHarvestDelegate = new EventSystem.IntraObjectHandler<TreeBud>((System.Action<TreeBud, object>) ((component, data) => component.OnHarvest(data)));
  private int trunkWiltHandle = -1;
  private int trunkWiltRecoverHandle = -1;
  [MyCmpReq]
  private Growing growing;
  [MyCmpReq]
  private StandardCropPlant crop;
  [Serialize]
  public Ref<BuddingTrunk> buddingTrunk;
  [Serialize]
  private int trunkPosition;
  [Serialize]
  public int growingPos;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.simRenderLoadBalance = true;
    int cell = Grid.PosToCell(this.gameObject);
    GameObject gameObject = Grid.Objects[cell, 5];
    if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null && (UnityEngine.Object) gameObject != (UnityEngine.Object) this.gameObject)
      Util.KDestroyGameObject(this.gameObject);
    else
      this.SetOccupyGridSpace(true);
    this.Subscribe<TreeBud>(1272413801, TreeBud.OnHarvestDelegate);
  }

  private void OnHarvest(object data)
  {
    if (!((UnityEngine.Object) this.buddingTrunk.Get() != (UnityEngine.Object) null))
      return;
    this.buddingTrunk.Get().TryRollNewSeed();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    if (this.buddingTrunk != null && (UnityEngine.Object) this.buddingTrunk.Get() != (UnityEngine.Object) null)
    {
      this.SubscribeToTrunk();
      this.UpdateAnimationSet();
    }
    else
    {
      Debug.LogWarning((object) "TreeBud loaded with missing trunk reference. Destroying...");
      Util.KDestroyGameObject(this.gameObject);
    }
  }

  protected override void OnCleanUp()
  {
    this.UnsubscribeToTrunk();
    this.SetOccupyGridSpace(false);
    base.OnCleanUp();
  }

  private void SetOccupyGridSpace(bool active)
  {
    int cell = Grid.PosToCell(this.gameObject);
    if (active)
    {
      GameObject gameObject = Grid.Objects[cell, 5];
      if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null && (UnityEngine.Object) gameObject != (UnityEngine.Object) this.gameObject)
        Debug.LogWarningFormat((UnityEngine.Object) this.gameObject, "TreeBud.SetOccupyGridSpace already occupied by {0}", (object) gameObject);
      Grid.Objects[cell, 5] = this.gameObject;
    }
    else
    {
      if (!((UnityEngine.Object) Grid.Objects[cell, 5] == (UnityEngine.Object) this.gameObject))
        return;
      Grid.Objects[cell, 5] = (GameObject) null;
    }
  }

  private void SubscribeToTrunk()
  {
    if (this.trunkWiltHandle != -1 || this.trunkWiltRecoverHandle != -1)
      return;
    Debug.Assert(this.buddingTrunk != null, (object) "buddingTrunk null");
    BuddingTrunk buddingTrunk = this.buddingTrunk.Get();
    Debug.Assert((UnityEngine.Object) buddingTrunk != (UnityEngine.Object) null, (object) "tree_trunk null");
    this.trunkWiltHandle = buddingTrunk.Subscribe(-724860998, new System.Action<object>(this.OnTrunkWilt));
    this.trunkWiltRecoverHandle = buddingTrunk.Subscribe(712767498, new System.Action<object>(this.OnTrunkRecover));
    this.Trigger(912965142, (object) !buddingTrunk.GetComponent<WiltCondition>().IsWilting());
    this.GetComponent<ReceptacleMonitor>().SetReceptacle(buddingTrunk.GetComponent<ReceptacleMonitor>().GetReceptacle());
    Vector3 position = this.gameObject.transform.position;
    position.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingFront) - 0.1f * (float) this.trunkPosition;
    this.gameObject.transform.SetPosition(position);
    this.GetComponent<BudUprootedMonitor>().SetParentObject(buddingTrunk.GetComponent<KPrefabID>());
  }

  private void UnsubscribeToTrunk()
  {
    if (this.buddingTrunk == null)
    {
      Debug.LogWarning((object) "buddingTrunk null", (UnityEngine.Object) this.gameObject);
    }
    else
    {
      BuddingTrunk buddingTrunk = this.buddingTrunk.Get();
      if ((UnityEngine.Object) buddingTrunk == (UnityEngine.Object) null)
      {
        Debug.LogWarning((object) "tree_trunk null", (UnityEngine.Object) this.gameObject);
      }
      else
      {
        buddingTrunk.Unsubscribe(this.trunkWiltHandle);
        buddingTrunk.Unsubscribe(this.trunkWiltRecoverHandle);
        buddingTrunk.OnBranchRemoved(this.trunkPosition, this);
      }
    }
  }

  public void SetTrunkPosition(BuddingTrunk budding_trunk, int idx)
  {
    this.buddingTrunk = new Ref<BuddingTrunk>(budding_trunk);
    this.trunkPosition = idx;
    this.SubscribeToTrunk();
    this.UpdateAnimationSet();
  }

  private void OnTrunkWilt(object data = null)
  {
    this.Trigger(912965142, (object) false);
  }

  private void OnTrunkRecover(object data = null)
  {
    this.Trigger(912965142, (object) true);
  }

  private void UpdateAnimationSet()
  {
    this.crop.anims = TreeBud.animSets[this.trunkPosition];
    KBatchedAnimController component = this.GetComponent<KBatchedAnimController>();
    component.Offset = TreeBud.animOffset[this.trunkPosition];
    component.Play((HashedString) this.crop.anims.grow, KAnim.PlayMode.Paused, 1f, 0.0f);
    this.crop.RefreshPositionPercent();
  }

  public string WiltStateString
  {
    get
    {
      return "    • " + (string) DUPLICANTS.STATS.TRUNKHEALTH.NAME;
    }
  }

  public WiltCondition.Condition[] Conditions
  {
    get
    {
      return new WiltCondition.Condition[1]
      {
        WiltCondition.Condition.UnhealthyRoot
      };
    }
  }
}
