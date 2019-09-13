// Decompiled with JetBrains decompiler
// Type: AutoMiner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMODUnity;
using KSerialization;
using System;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class AutoMiner : StateMachineComponent<AutoMiner.Instance>, ISim1000ms
{
  private static HashedString HASH_ROTATION = (HashedString) "rotation";
  private static readonly EventSystem.IntraObjectHandler<AutoMiner> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<AutoMiner>((System.Action<AutoMiner, object>) ((component, data) => component.OnOperationalChanged(data)));
  [EventRef]
  private string rotateSound = "AutoMiner_rotate";
  private float arm_rot = 45f;
  private float turn_rate = 180f;
  private int dig_cell = Grid.InvalidCell;
  [MyCmpReq]
  private Operational operational;
  [MyCmpGet]
  private KSelectable selectable;
  [MyCmpAdd]
  private Storage storage;
  [MyCmpGet]
  private Rotatable rotatable;
  [MyCmpReq]
  private MiningSounds mining_sounds;
  public int x;
  public int y;
  public int width;
  public int height;
  public CellOffset vision_offset;
  private KBatchedAnimController arm_anim_ctrl;
  private GameObject arm_go;
  private LoopingSounds looping_sounds;
  private KAnimLink link;
  private bool rotation_complete;
  private bool rotate_sound_playing;
  private GameObject hitEffectPrefab;
  private GameObject hitEffect;

  private bool HasDigCell
  {
    get
    {
      return this.dig_cell != Grid.InvalidCell;
    }
  }

  private bool RotationComplete
  {
    get
    {
      if (this.HasDigCell)
        return this.rotation_complete;
      return false;
    }
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.simRenderLoadBalance = true;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.hitEffectPrefab = Assets.GetPrefab((Tag) "fx_dig_splash");
    KBatchedAnimController component = this.GetComponent<KBatchedAnimController>();
    string name = component.name + ".gun";
    this.arm_go = new GameObject(name);
    this.arm_go.SetActive(false);
    this.arm_go.transform.parent = component.transform;
    this.looping_sounds = this.arm_go.AddComponent<LoopingSounds>();
    this.rotateSound = GlobalAssets.GetSound(this.rotateSound, false);
    this.arm_go.AddComponent<KPrefabID>().PrefabTag = new Tag(name);
    this.arm_anim_ctrl = this.arm_go.AddComponent<KBatchedAnimController>();
    this.arm_anim_ctrl.AnimFiles = new KAnimFile[1]
    {
      component.AnimFiles[0]
    };
    this.arm_anim_ctrl.initialAnim = "gun";
    this.arm_anim_ctrl.isMovable = true;
    this.arm_anim_ctrl.sceneLayer = Grid.SceneLayer.TransferArm;
    component.SetSymbolVisiblity((KAnimHashedString) "gun_target", false);
    bool symbolVisible;
    Vector3 column = (Vector3) component.GetSymbolTransform(new HashedString("gun_target"), out symbolVisible).GetColumn(3);
    column.z = Grid.GetLayerZ(Grid.SceneLayer.TransferArm);
    this.arm_go.transform.SetPosition(column);
    this.arm_go.SetActive(true);
    this.link = new KAnimLink((KAnimControllerBase) component, (KAnimControllerBase) this.arm_anim_ctrl);
    this.Subscribe<AutoMiner>(-592767678, AutoMiner.OnOperationalChangedDelegate);
    this.RotateArm(this.rotatable.GetRotatedOffset(Quaternion.Euler(0.0f, 0.0f, -45f) * Vector3.up), true, 0.0f);
    this.StopDig();
    this.smi.StartSM();
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
  }

  public void Sim1000ms(float dt)
  {
    if (!this.operational.IsOperational)
      return;
    this.RefreshDiggableCell();
    this.operational.SetActive(this.HasDigCell, false);
  }

  private void OnOperationalChanged(object data)
  {
    if ((bool) data)
      return;
    this.dig_cell = Grid.InvalidCell;
    this.rotation_complete = false;
  }

  public void UpdateRotation(float dt)
  {
    if (!this.HasDigCell)
      return;
    Vector3 posCcc = Grid.CellToPosCCC(this.dig_cell, Grid.SceneLayer.TileMain);
    posCcc.z = 0.0f;
    Vector3 position = this.arm_go.transform.GetPosition();
    position.z = 0.0f;
    this.RotateArm(Vector3.Normalize(posCcc - position), false, dt);
  }

  private Element GetTargetElement()
  {
    if (this.HasDigCell)
      return Grid.Element[this.dig_cell];
    return (Element) null;
  }

  public void StartDig()
  {
    this.Trigger(-1762453998, (object) this.GetTargetElement());
    this.CreateHitEffect();
    this.arm_anim_ctrl.Play((HashedString) "gun_digging", KAnim.PlayMode.Loop, 1f, 0.0f);
  }

  public void StopDig()
  {
    this.Trigger(939543986, (object) null);
    this.DestroyHitEffect();
    this.arm_anim_ctrl.Play((HashedString) "gun", KAnim.PlayMode.Loop, 1f, 0.0f);
  }

  public void UpdateDig(float dt)
  {
    if (!this.HasDigCell || !this.rotation_complete)
      return;
    Diggable.DoDigTick(this.dig_cell, dt);
    this.mining_sounds.SetPercentComplete(Grid.Damage[this.dig_cell]);
    Vector3 posCcc = Grid.CellToPosCCC(this.dig_cell, Grid.SceneLayer.FXFront2);
    posCcc.z = 0.0f;
    Vector3 position = this.arm_go.transform.GetPosition();
    position.z = 0.0f;
    float sqrMagnitude = (posCcc - position).sqrMagnitude;
    this.arm_anim_ctrl.GetBatchInstanceData().SetClipRadius(position.x, position.y, sqrMagnitude, true);
    if (AutoMiner.ValidDigCell(this.dig_cell))
      return;
    this.dig_cell = Grid.InvalidCell;
    this.rotation_complete = false;
  }

  private void CreateHitEffect()
  {
    if ((UnityEngine.Object) this.hitEffectPrefab == (UnityEngine.Object) null)
      return;
    if ((UnityEngine.Object) this.hitEffect != (UnityEngine.Object) null)
      this.DestroyHitEffect();
    this.hitEffect = GameUtil.KInstantiate(this.hitEffectPrefab, Grid.CellToPosCCC(this.dig_cell, Grid.SceneLayer.FXFront2), Grid.SceneLayer.FXFront2, (string) null, 0);
    this.hitEffect.SetActive(true);
    KBatchedAnimController component = this.hitEffect.GetComponent<KBatchedAnimController>();
    component.sceneLayer = Grid.SceneLayer.FXFront2;
    component.initialMode = KAnim.PlayMode.Loop;
    component.enabled = false;
    component.enabled = true;
  }

  private void DestroyHitEffect()
  {
    if ((UnityEngine.Object) this.hitEffectPrefab == (UnityEngine.Object) null || !((UnityEngine.Object) this.hitEffect != (UnityEngine.Object) null))
      return;
    this.hitEffect.DeleteObject();
    this.hitEffect = (GameObject) null;
  }

  private void RefreshDiggableCell()
  {
    CellOffset offset1 = this.vision_offset;
    if ((bool) ((UnityEngine.Object) this.rotatable))
      offset1 = this.rotatable.GetRotatedCellOffset(this.vision_offset);
    int cell1 = Grid.PosToCell(this.transform.gameObject);
    int cell2 = Grid.OffsetCell(cell1, offset1);
    int x1;
    int y1;
    Grid.CellToXY(cell2, out x1, out y1);
    float num1 = float.MaxValue;
    int num2 = Grid.InvalidCell;
    Vector3 pos1 = Grid.CellToPos(cell2);
    bool flag = false;
    for (int index1 = 0; index1 < this.height; ++index1)
    {
      for (int index2 = 0; index2 < this.width; ++index2)
      {
        CellOffset offset2 = new CellOffset(this.x + index2, this.y + index1);
        if ((bool) ((UnityEngine.Object) this.rotatable))
          offset2 = this.rotatable.GetRotatedCellOffset(offset2);
        int cell3 = Grid.OffsetCell(cell1, offset2);
        if (Grid.IsValidCell(cell3))
        {
          int x2;
          int y2;
          Grid.CellToXY(cell3, out x2, out y2);
          if (Grid.IsValidCell(cell3) && AutoMiner.ValidDigCell(cell3))
          {
            int x3 = x1;
            int y3 = y1;
            int x2_1 = x2;
            int y2_1 = y2;
            // ISSUE: reference to a compiler-generated field
            if (AutoMiner.\u003C\u003Ef__mg\u0024cache0 == null)
            {
              // ISSUE: reference to a compiler-generated field
              AutoMiner.\u003C\u003Ef__mg\u0024cache0 = new Func<int, bool>(AutoMiner.DigBlockingCB);
            }
            // ISSUE: reference to a compiler-generated field
            Func<int, bool> fMgCache0 = AutoMiner.\u003C\u003Ef__mg\u0024cache0;
            if (Grid.TestLineOfSight(x3, y3, x2_1, y2_1, fMgCache0, false))
            {
              if (cell3 == this.dig_cell)
                flag = true;
              Vector3 pos2 = Grid.CellToPos(cell3);
              float num3 = Vector3.Distance(pos1, pos2);
              if ((double) num3 < (double) num1)
              {
                num1 = num3;
                num2 = cell3;
              }
            }
          }
        }
      }
    }
    if (flag || this.dig_cell == num2)
      return;
    this.dig_cell = num2;
    this.rotation_complete = false;
  }

  private static bool ValidDigCell(int cell)
  {
    return Grid.Solid[cell] && !Grid.Foundation[cell] && Grid.Element[cell].hardness < (byte) 150;
  }

  public static bool DigBlockingCB(int cell)
  {
    return Grid.Foundation[cell] || Grid.Element[cell].hardness >= (byte) 150;
  }

  private void RotateArm(Vector3 target_dir, bool warp, float dt)
  {
    if (this.rotation_complete)
      return;
    float a = MathUtil.Wrap(-180f, 180f, MathUtil.AngleSigned(Vector3.up, target_dir, Vector3.forward) - this.arm_rot);
    this.rotation_complete = Mathf.Approximately(a, 0.0f);
    float num = a;
    if (warp)
      this.rotation_complete = true;
    else
      num = Mathf.Clamp(num, -this.turn_rate * dt, this.turn_rate * dt);
    this.arm_rot += num;
    this.arm_rot = MathUtil.Wrap(-180f, 180f, this.arm_rot);
    this.arm_go.transform.rotation = Quaternion.Euler(0.0f, 0.0f, this.arm_rot);
    if (!this.rotation_complete)
    {
      this.StartRotateSound();
      this.looping_sounds.SetParameter(this.rotateSound, AutoMiner.HASH_ROTATION, this.arm_rot);
    }
    else
      this.StopRotateSound();
  }

  private void StartRotateSound()
  {
    if (this.rotate_sound_playing)
      return;
    this.looping_sounds.StartSound(this.rotateSound);
    this.rotate_sound_playing = true;
  }

  private void StopRotateSound()
  {
    if (!this.rotate_sound_playing)
      return;
    this.looping_sounds.StopSound(this.rotateSound);
    this.rotate_sound_playing = false;
  }

  public class Instance : GameStateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.GameInstance
  {
    public Instance(AutoMiner master)
      : base(master)
    {
    }
  }

  public class States : GameStateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner>
  {
    public StateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.BoolParameter transferring;
    public GameStateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.State off;
    public AutoMiner.States.ReadyStates on;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      default_state = (StateMachine.BaseState) this.off;
      this.root.DoNothing();
      this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, (GameStateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.State) this.on, (StateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.Transition.ConditionCallback) (smi => smi.GetComponent<Operational>().IsOperational));
      this.on.DefaultState(this.on.idle).EventTransition(GameHashes.OperationalChanged, this.off, (StateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.Transition.ConditionCallback) (smi => !smi.GetComponent<Operational>().IsOperational));
      this.on.idle.PlayAnim("on").EventTransition(GameHashes.ActiveChanged, this.on.moving, (StateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.Transition.ConditionCallback) (smi => smi.GetComponent<Operational>().IsActive));
      GameStateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.State state1 = this.on.moving.Exit((StateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.State.Callback) (smi => smi.master.StopRotateSound())).PlayAnim("working").EventTransition(GameHashes.ActiveChanged, this.on.idle, (StateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.Transition.ConditionCallback) (smi => !smi.GetComponent<Operational>().IsActive)).Update((System.Action<AutoMiner.Instance, float>) ((smi, dt) => smi.master.UpdateRotation(dt)), UpdateRate.SIM_33ms, false);
      GameStateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.State digging = this.on.digging;
      // ISSUE: reference to a compiler-generated field
      if (AutoMiner.States.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        AutoMiner.States.\u003C\u003Ef__mg\u0024cache0 = new StateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.Transition.ConditionCallback(AutoMiner.States.RotationComplete);
      }
      // ISSUE: reference to a compiler-generated field
      StateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.Transition.ConditionCallback fMgCache0 = AutoMiner.States.\u003C\u003Ef__mg\u0024cache0;
      state1.Transition(digging, fMgCache0, UpdateRate.SIM_200ms);
      GameStateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.State state2 = this.on.digging.Enter((StateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.State.Callback) (smi => smi.master.StartDig())).Exit((StateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.State.Callback) (smi => smi.master.StopDig())).PlayAnim("working").EventTransition(GameHashes.ActiveChanged, this.on.idle, (StateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.Transition.ConditionCallback) (smi => !smi.GetComponent<Operational>().IsActive)).Update((System.Action<AutoMiner.Instance, float>) ((smi, dt) => smi.master.UpdateDig(dt)), UpdateRate.SIM_200ms, false);
      GameStateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.State moving = this.on.moving;
      // ISSUE: reference to a compiler-generated field
      if (AutoMiner.States.\u003C\u003Ef__mg\u0024cache1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        AutoMiner.States.\u003C\u003Ef__mg\u0024cache1 = new StateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.Transition.ConditionCallback(AutoMiner.States.RotationComplete);
      }
      // ISSUE: reference to a compiler-generated field
      StateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.Transition.ConditionCallback condition = GameStateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.Not(AutoMiner.States.\u003C\u003Ef__mg\u0024cache1);
      state2.Transition(moving, condition, UpdateRate.SIM_200ms);
    }

    public static bool RotationComplete(AutoMiner.Instance smi)
    {
      return smi.master.RotationComplete;
    }

    public class ReadyStates : GameStateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.State
    {
      public GameStateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.State idle;
      public GameStateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.State moving;
      public GameStateMachine<AutoMiner.States, AutoMiner.Instance, AutoMiner, object>.State digging;
    }
  }
}
