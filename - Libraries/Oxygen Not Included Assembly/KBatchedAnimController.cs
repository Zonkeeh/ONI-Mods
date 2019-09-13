// Decompiled with JetBrains decompiler
// Type: KBatchedAnimController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

[DebuggerDisplay("{name} visible={visible} suspendUpdates={suspendUpdates} moving={moving}")]
public class KBatchedAnimController : KAnimControllerBase, KAnimConverter.IAnimConverter
{
  private Vector3 lastPos = Vector3.zero;
  private Vector2I lastChunkXY = KBatchedAnimUpdater.INVALID_CHUNK_ID;
  public float animScale = 0.005f;
  [NonSerialized]
  public KBatchedAnimUpdater.RegistrationState updateRegistrationState = KBatchedAnimUpdater.RegistrationState.Unregistered;
  private Vector3 screenOffset = new Vector3(0.0f, 0.0f, 0.0f);
  public Matrix2x3 navMatrix = Matrix2x3.identity;
  public bool setScaleFromAnim = true;
  public Vector2 animOverrideSize = Vector2.one;
  [NonSerialized]
  protected bool _forceRebuild;
  private KAnimBatch batch;
  private bool suspendUpdates;
  private bool visibilityListenerRegistered;
  private bool moving;
  private SymbolOverrideController symbolOverrideController;
  private int symbolOverrideControllerVersion;
  public Grid.SceneLayer sceneLayer;
  private RectTransform rt;
  private CanvasScaler scaler;
  private Canvas rootCanvas;
  public bool isMovable;

  public KBatchedAnimController()
  {
    this.batchInstanceData = new KBatchedAnimInstanceData((KAnimConverter.IAnimConverter) this);
  }

  public int GetCurrentFrameIndex()
  {
    return this.curAnimFrameIdx;
  }

  public KBatchedAnimInstanceData GetBatchInstanceData()
  {
    return this.batchInstanceData;
  }

  protected bool forceRebuild
  {
    get
    {
      return this._forceRebuild;
    }
    set
    {
      this._forceRebuild = value;
    }
  }

  public bool IsActive()
  {
    if (this.isActiveAndEnabled)
      return this._enabled;
    return false;
  }

  public bool IsVisible()
  {
    return this.isVisible;
  }

  public void SetSymbolScale(KAnimHashedString symbol_name, float scale)
  {
    KAnim.Build.Symbol symbol = KAnimBatchManager.Instance().GetBatchGroupData(this.GetBatchGroupID(false)).GetSymbol(symbol_name);
    if (symbol == null)
      return;
    this.symbolInstanceGpuData.SetSymbolScale(symbol.symbolIndexInSourceBuild, scale);
    this.SuspendUpdates(false);
    this.SetDirty();
  }

  public void SetSymbolTint(KAnimHashedString symbol_name, Color color)
  {
    KAnim.Build.Symbol symbol = KAnimBatchManager.Instance().GetBatchGroupData(this.GetBatchGroupID(false)).GetSymbol(symbol_name);
    if (symbol == null)
      return;
    this.symbolInstanceGpuData.SetSymbolTint(symbol.symbolIndexInSourceBuild, color);
    this.SuspendUpdates(false);
    this.SetDirty();
  }

  public Vector2I GetCellXY()
  {
    Vector3 positionIncludingOffset = this.PositionIncludingOffset;
    if ((double) Grid.CellSizeInMeters == 0.0)
      return new Vector2I((int) positionIncludingOffset.x, (int) positionIncludingOffset.y);
    return Grid.PosToXY(positionIncludingOffset);
  }

  public float GetZ()
  {
    return this.transform.GetPosition().z;
  }

  public string GetName()
  {
    return this.name;
  }

  public override KAnim.Anim GetAnim(int index)
  {
    if (!this.batchGroupID.IsValid || !(this.batchGroupID != KAnimBatchManager.NO_BATCH))
      Debug.LogError((object) (this.name + " batch not ready"));
    KBatchGroupData batchGroupData = KAnimBatchManager.Instance().GetBatchGroupData(this.batchGroupID);
    Debug.Assert(batchGroupData != null);
    return batchGroupData.GetAnim(index);
  }

  private void Initialize()
  {
    if (!this.batchGroupID.IsValid || !(this.batchGroupID != KAnimBatchManager.NO_BATCH))
      return;
    this.DeRegister();
    this.Register();
  }

  private void OnMovementStateChanged(bool is_moving)
  {
    if (is_moving == this.moving)
      return;
    this.moving = is_moving;
    this.SetDirty();
    this.ConfigureUpdateListener();
  }

  private static void OnMovementStateChanged(Transform transform, bool is_moving)
  {
    transform.GetComponent<KBatchedAnimController>().OnMovementStateChanged(is_moving);
  }

  private void SetBatchGroup(KAnimFileData kafd)
  {
    DebugUtil.Assert(!this.batchGroupID.IsValid, "Should only be setting the batch group once.");
    DebugUtil.Assert(kafd != null, "Null anim data!! For", this.name);
    this.curBuild = kafd.build;
    DebugUtil.Assert(this.curBuild != null, "Null build for anim!! ", this.name, kafd.name);
    KAnimGroupFile.Group group = KAnimGroupFile.GetGroup(this.curBuild.batchTag);
    HashedString hashedString = kafd.build.batchTag;
    if (group.renderType == KAnimBatchGroup.RendererType.DontRender || group.renderType == KAnimBatchGroup.RendererType.AnimOnly)
    {
      Debug.Assert(group.swapTarget.IsValid, (object) ("Invalid swap target fro group [" + (object) group.id + "]"));
      hashedString = group.swapTarget;
    }
    this.batchGroupID = hashedString;
    this.symbolInstanceGpuData = new SymbolInstanceGpuData(KAnimBatchManager.instance.GetBatchGroupData(this.batchGroupID).maxSymbolsPerBuild);
    this.symbolOverrideInfoGpuData = new SymbolOverrideInfoGpuData(KAnimBatchManager.instance.GetBatchGroupData(this.batchGroupID).symbolFrameInstances.Count);
    if (this.batchGroupID.IsValid && !(this.batchGroupID == KAnimBatchManager.NO_BATCH))
      return;
    Debug.LogError((object) ("Batch is not ready: " + this.name));
  }

  public void LoadAnims()
  {
    if (!KAnimBatchManager.Instance().isReady)
      Debug.LogError((object) ("KAnimBatchManager is not ready when loading anim:" + this.name));
    if (this.animFiles.Length <= 0)
      DebugUtil.Assert(false, "KBatchedAnimController has no anim files:" + this.name);
    if (this.animFiles[0].buildBytes == null)
      DebugUtil.LogErrorArgs((UnityEngine.Object) this.gameObject, (object) string.Format("First anim file needs to be the build file but {0} doesn't have an associated build", (object) this.animFiles[0].GetData().name));
    this.overrideAnims.Clear();
    this.anims.Clear();
    this.SetBatchGroup(this.animFiles[0].GetData());
    for (int index = 0; index < this.animFiles.Length; ++index)
      this.AddAnims(this.animFiles[index]);
    this.forceRebuild = true;
    if (this.layering != null)
      this.layering.HideSymbols();
    if (!this.usingNewSymbolOverrideSystem)
      return;
    DebugUtil.Assert((UnityEngine.Object) this.GetComponent<SymbolOverrideController>() != (UnityEngine.Object) null);
  }

  public void UpdateAnim(float dt)
  {
    if (this.batch != null && this.transform.hasChanged)
    {
      this.transform.hasChanged = false;
      if (this.batch != null && this.batch.group.maxGroupSize == 1 && (double) this.lastPos.z != (double) this.transform.GetPosition().z)
        this.batch.OverrideZ(this.transform.GetPosition().z);
      this.lastPos = this.PositionIncludingOffset;
      if (this.visibilityType != KAnimControllerBase.VisibilityType.Always && KAnimBatchManager.ControllerToChunkXY((KAnimConverter.IAnimConverter) this) != this.lastChunkXY && this.lastChunkXY != KBatchedAnimUpdater.INVALID_CHUNK_ID)
      {
        this.DeRegister();
        this.Register();
      }
      this.SetDirty();
    }
    if (this.batchGroupID == KAnimBatchManager.NO_BATCH || !this.IsActive())
      return;
    if (!this.forceRebuild && (this.mode == KAnim.PlayMode.Paused || this.stopped || this.curAnim == null || this.mode == KAnim.PlayMode.Once && this.curAnim != null && ((double) this.elapsedTime > (double) this.curAnim.totalTime || (double) this.curAnim.totalTime <= 0.0) && this.animQueue.Count == 0))
      this.SuspendUpdates(true);
    if (!this.isVisible && !this.forceRebuild)
    {
      if (this.visibilityType != KAnimControllerBase.VisibilityType.OffscreenUpdate || this.stopped || this.mode == KAnim.PlayMode.Paused)
        return;
      this.SetElapsedTime(this.elapsedTime + dt * this.playSpeed);
    }
    else
    {
      this.curAnimFrameIdx = this.GetFrameIdx(this.elapsedTime, true);
      if (this.eventManagerHandle.IsValid() && this.aem != null && (int) (((double) this.elapsedTime - (double) this.aem.GetElapsedTime(this.eventManagerHandle)) * 100.0) != 0)
        this.UpdateAnimEventSequenceTime();
      this.UpdateFrame(this.elapsedTime);
      if (!this.stopped && this.mode != KAnim.PlayMode.Paused)
        this.SetElapsedTime(this.elapsedTime + dt * this.playSpeed);
      this.forceRebuild = false;
    }
  }

  protected override void UpdateFrame(float t)
  {
    this.previousFrame = this.currentFrame;
    if (!this.stopped || this.forceRebuild)
    {
      if (this.curAnim != null && (this.mode == KAnim.PlayMode.Loop || (double) this.elapsedTime <= (double) this.GetDuration() || this.forceRebuild))
      {
        this.currentFrame = this.curAnim.GetFrameIdx(this.mode, this.elapsedTime);
        if (this.currentFrame != this.previousFrame || this.forceRebuild)
          this.SetDirty();
      }
      else
        this.TriggerStop();
      if (!this.stopped && this.mode == KAnim.PlayMode.Loop && this.currentFrame == 0)
        this.AnimEnter(this.curAnim.hash);
    }
    if (this.synchronizer == null)
      return;
    this.synchronizer.SyncTime();
  }

  public override void TriggerStop()
  {
    if (this.animQueue.Count > 0)
    {
      this.StartQueuedAnim();
    }
    else
    {
      if (this.curAnim == null || this.mode != KAnim.PlayMode.Once)
        return;
      this.currentFrame = this.curAnim.numFrames - 1;
      this.Stop();
      this.gameObject.Trigger(-1061186183, (object) null);
      if (!this.destroyOnAnimComplete)
        return;
      this.DestroySelf();
    }
  }

  public override void UpdateHidden()
  {
    for (int symbol_idx = 0; symbol_idx < this.curBuild.symbols.Length; ++symbol_idx)
    {
      bool is_visible = !this.hiddenSymbols.Contains(this.curBuild.symbols[symbol_idx].hash);
      this.symbolInstanceGpuData.SetVisible(symbol_idx, is_visible);
    }
    this.SetDirty();
  }

  public int GetMaxVisible()
  {
    return this.maxSymbols;
  }

  public HashedString batchGroupID { get; private set; }

  public HashedString GetBatchGroupID(bool isEditorWindow = false)
  {
    Debug.Assert(isEditorWindow || (this.animFiles == null || this.animFiles.Length == 0 || this.batchGroupID.IsValid && this.batchGroupID != KAnimBatchManager.NO_BATCH));
    return this.batchGroupID;
  }

  public int GetLayer()
  {
    return this.gameObject.layer;
  }

  public KAnimBatch GetBatch()
  {
    return this.batch;
  }

  public void SetBatch(KAnimBatch new_batch)
  {
    this.batch = new_batch;
    if (this.materialType != KAnimBatchGroup.MaterialType.UI)
      return;
    KBatchedAnimCanvasRenderer animCanvasRenderer = this.GetComponent<KBatchedAnimCanvasRenderer>();
    if ((UnityEngine.Object) animCanvasRenderer == (UnityEngine.Object) null && new_batch != null)
      animCanvasRenderer = this.gameObject.AddComponent<KBatchedAnimCanvasRenderer>();
    if (!((UnityEngine.Object) animCanvasRenderer != (UnityEngine.Object) null))
      return;
    animCanvasRenderer.SetBatch((KAnimConverter.IAnimConverter) this);
  }

  public int GetCurrentNumFrames()
  {
    if (this.curAnim != null)
      return this.curAnim.numFrames;
    return 0;
  }

  public int GetFirstFrameIndex()
  {
    if (this.curAnim != null)
      return this.curAnim.firstFrameIdx;
    return -1;
  }

  private Canvas GetRootCanvas()
  {
    if ((UnityEngine.Object) this.rt == (UnityEngine.Object) null)
      return (Canvas) null;
    for (RectTransform component1 = this.rt.parent.GetComponent<RectTransform>(); (UnityEngine.Object) component1 != (UnityEngine.Object) null; component1 = component1.parent.GetComponent<RectTransform>())
    {
      Canvas component2 = component1.GetComponent<Canvas>();
      if ((UnityEngine.Object) component2 != (UnityEngine.Object) null && component2.isRootCanvas)
        return component2;
    }
    return (Canvas) null;
  }

  public override Matrix2x3 GetTransformMatrix()
  {
    Vector3 vector3 = this.PositionIncludingOffset;
    vector3.z = 0.0f;
    Vector2 scale = new Vector2(this.animScale * this.animWidth, -this.animScale * this.animHeight);
    if (this.materialType == KAnimBatchGroup.MaterialType.UI)
    {
      this.rt = this.GetComponent<RectTransform>();
      if ((UnityEngine.Object) this.rootCanvas == (UnityEngine.Object) null)
        this.rootCanvas = this.GetRootCanvas();
      if ((UnityEngine.Object) this.scaler == (UnityEngine.Object) null && (UnityEngine.Object) this.rootCanvas != (UnityEngine.Object) null)
        this.scaler = this.rootCanvas.GetComponent<CanvasScaler>();
      if ((UnityEngine.Object) this.rootCanvas == (UnityEngine.Object) null)
      {
        this.screenOffset.x = (float) (Screen.width / 2);
        this.screenOffset.y = (float) (Screen.height / 2);
      }
      else
      {
        this.screenOffset.x = this.rootCanvas.rectTransform().rect.width / 2f;
        this.screenOffset.y = this.rootCanvas.rectTransform().rect.height / 2f;
      }
      float num1 = 1f;
      if ((UnityEngine.Object) this.scaler != (UnityEngine.Object) null)
        num1 = 1f / this.scaler.scaleFactor;
      vector3 = (this.rt.localToWorldMatrix.MultiplyPoint((Vector3) this.rt.pivot) + this.offset) * num1 - this.screenOffset;
      float num2 = this.animWidth * this.animScale;
      float num3 = this.animHeight * this.animScale;
      float num4;
      float num5;
      if (this.setScaleFromAnim && this.curAnim != null)
      {
        num4 = num2 * (this.rt.rect.size.x / this.curAnim.unScaledSize.x);
        num5 = num3 * (this.rt.rect.size.y / this.curAnim.unScaledSize.y);
      }
      else
      {
        num4 = num2 * (this.rt.rect.size.x / this.animOverrideSize.x);
        num5 = num3 * (this.rt.rect.size.y / this.animOverrideSize.y);
      }
      scale = (Vector2) new Vector3(this.rt.lossyScale.x * num4, -this.rt.lossyScale.y * num5, this.rt.lossyScale.z);
      this.pivot = (Vector3) this.rt.pivot;
    }
    Matrix2x3 matrix2x3_1 = Matrix2x3.Scale(scale);
    Matrix2x3 matrix2x3_2 = Matrix2x3.Scale(new Vector2(!this.flipX ? 1f : -1f, !this.flipY ? 1f : -1f));
    Matrix2x3 matrix2x3_3;
    if ((double) this.rotation != 0.0)
    {
      Matrix2x3 matrix2x3_4 = Matrix2x3.Translate((Vector2) this.pivot) * Matrix2x3.Rotate(this.rotation * ((float) Math.PI / 180f)) * Matrix2x3.Translate((Vector2) (-this.pivot));
      matrix2x3_3 = Matrix2x3.TRS((Vector2) vector3, this.transform.rotation, (Vector2) this.transform.localScale) * matrix2x3_4 * matrix2x3_1 * this.navMatrix * matrix2x3_2;
    }
    else
      matrix2x3_3 = Matrix2x3.TRS((Vector2) vector3, this.transform.rotation, (Vector2) this.transform.localScale) * matrix2x3_1 * this.navMatrix * matrix2x3_2;
    return matrix2x3_3;
  }

  public override Matrix4x4 GetSymbolTransform(HashedString symbol, out bool symbolVisible)
  {
    if (this.curAnimFrameIdx != -1 && this.batch != null)
    {
      Matrix2x3 symbolLocalTransform = this.GetSymbolLocalTransform(symbol, out symbolVisible);
      if (symbolVisible)
        return (Matrix4x4) this.GetTransformMatrix() * (Matrix4x4) symbolLocalTransform;
    }
    symbolVisible = false;
    return new Matrix4x4();
  }

  public override Matrix2x3 GetSymbolLocalTransform(
    HashedString symbol,
    out bool symbolVisible)
  {
    if (this.curAnimFrameIdx != -1 && this.batch != null)
    {
      KAnim.Anim.Frame frame = this.batch.group.data.GetFrame(this.curAnimFrameIdx);
      if (frame != KAnim.Anim.Frame.InvalidFrame)
      {
        for (int index1 = 0; index1 < frame.numElements; ++index1)
        {
          int index2 = frame.firstElementIdx + index1;
          if (index2 < this.batch.group.data.frameElements.Count)
          {
            KAnim.Anim.FrameElement frameElement = this.batch.group.data.frameElements[index2];
            if (frameElement.symbol == symbol)
            {
              symbolVisible = true;
              return frameElement.transform;
            }
          }
        }
      }
    }
    symbolVisible = false;
    return Matrix2x3.identity;
  }

  public override void SetLayer(int layer)
  {
    if (layer == this.gameObject.layer)
      return;
    base.SetLayer(layer);
    this.DeRegister();
    this.gameObject.layer = layer;
    this.Register();
  }

  public override void SetDirty()
  {
    if (this.batch == null)
      return;
    this.batch.SetDirty((KAnimConverter.IAnimConverter) this);
  }

  protected override void OnStartQueuedAnim()
  {
    this.SuspendUpdates(false);
  }

  protected override void OnAwake()
  {
    this.LoadAnims();
    if (this.visibilityType == KAnimControllerBase.VisibilityType.Default)
      this.visibilityType = this.materialType != KAnimBatchGroup.MaterialType.UI ? this.visibilityType : KAnimControllerBase.VisibilityType.Always;
    this.symbolOverrideController = this.GetComponent<SymbolOverrideController>();
    this.UpdateHidden();
    this.hasEnableRun = false;
  }

  protected override void OnStart()
  {
    if (this.batch == null)
      this.Initialize();
    if (this.visibilityType == KAnimControllerBase.VisibilityType.Always || this.visibilityType == KAnimControllerBase.VisibilityType.OffscreenUpdate)
      this.ConfigureUpdateListener();
    CellChangeMonitor instance = Singleton<CellChangeMonitor>.Instance;
    if (instance != null)
    {
      CellChangeMonitor cellChangeMonitor = instance;
      Transform transform = this.transform;
      // ISSUE: reference to a compiler-generated field
      if (KBatchedAnimController.\u003C\u003Ef__mg\u0024cache0 == null)
      {
        // ISSUE: reference to a compiler-generated field
        KBatchedAnimController.\u003C\u003Ef__mg\u0024cache0 = new System.Action<Transform, bool>(KBatchedAnimController.OnMovementStateChanged);
      }
      // ISSUE: reference to a compiler-generated field
      System.Action<Transform, bool> fMgCache0 = KBatchedAnimController.\u003C\u003Ef__mg\u0024cache0;
      cellChangeMonitor.RegisterMovementStateChanged(transform, fMgCache0);
      this.moving = instance.IsMoving(this.transform);
    }
    this.symbolOverrideController = this.GetComponent<SymbolOverrideController>();
    this.SetDirty();
  }

  protected override void OnStop()
  {
    this.SetDirty();
  }

  private void OnEnable()
  {
    if (!this._enabled)
      return;
    this.Enable();
  }

  protected override void Enable()
  {
    if (this.hasEnableRun)
      return;
    this.hasEnableRun = true;
    if (this.batch == null)
      this.Initialize();
    this.SetDirty();
    this.SuspendUpdates(false);
    this.ConfigureVisibilityListener(true);
    if (this.stopped || this.curAnim == null || (this.mode == KAnim.PlayMode.Paused || this.eventManagerHandle.IsValid()))
      return;
    this.StartAnimEventSequence();
  }

  private void OnDisable()
  {
    this.Disable();
  }

  protected override void Disable()
  {
    if (App.IsExiting || KMonoBehaviour.isLoadingScene || !this.hasEnableRun)
      return;
    this.hasEnableRun = false;
    this.SuspendUpdates(true);
    if (this.batch != null)
      this.DeRegister();
    this.ConfigureVisibilityListener(false);
    this.StopAnimEventSequence();
  }

  protected override void OnDestroy()
  {
    if (App.IsExiting)
      return;
    CellChangeMonitor instance = Singleton<CellChangeMonitor>.Instance;
    if (instance != null)
    {
      CellChangeMonitor cellChangeMonitor = instance;
      Transform transform = this.transform;
      // ISSUE: reference to a compiler-generated field
      if (KBatchedAnimController.\u003C\u003Ef__mg\u0024cache1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        KBatchedAnimController.\u003C\u003Ef__mg\u0024cache1 = new System.Action<Transform, bool>(KBatchedAnimController.OnMovementStateChanged);
      }
      // ISSUE: reference to a compiler-generated field
      System.Action<Transform, bool> fMgCache1 = KBatchedAnimController.\u003C\u003Ef__mg\u0024cache1;
      cellChangeMonitor.UnregisterMovementStateChanged(transform, fMgCache1);
    }
    Singleton<KBatchedAnimUpdater>.Instance?.UpdateUnregister(this);
    this.isVisible = false;
    this.DeRegister();
    this.stopped = true;
    this.StopAnimEventSequence();
    this.batchInstanceData = (KBatchedAnimInstanceData) null;
    this.batch = (KAnimBatch) null;
    base.OnDestroy();
  }

  public void SetBlendValue(float value)
  {
    this.batchInstanceData.SetBlend(value);
    this.SetDirty();
  }

  public bool ApplySymbolOverrides()
  {
    if (!((UnityEngine.Object) this.symbolOverrideController != (UnityEngine.Object) null))
      return false;
    if (this.symbolOverrideControllerVersion != this.symbolOverrideController.version || this.symbolOverrideController.applySymbolOverridesEveryFrame)
    {
      this.symbolOverrideControllerVersion = this.symbolOverrideController.version;
      this.symbolOverrideController.ApplyOverrides();
    }
    this.symbolOverrideController.ApplyAtlases();
    return true;
  }

  public void SetSymbolOverride(
    int symbol_idx,
    KAnim.Build.SymbolFrameInstance symbol_frame_instance)
  {
    DebugUtil.Assert(this.usingNewSymbolOverrideSystem, "KBatchedAnimController requires usingNewSymbolOverrideSystem to bet to true to enable symbol overrides.");
    this.symbolOverrideInfoGpuData.SetSymbolOverrideInfo(symbol_idx, symbol_frame_instance);
  }

  protected override void Register()
  {
    if (!this.IsActive() || this.batch != null || (!this.batchGroupID.IsValid || !(this.batchGroupID != KAnimBatchManager.NO_BATCH)))
      return;
    this.lastChunkXY = KAnimBatchManager.ControllerToChunkXY((KAnimConverter.IAnimConverter) this);
    KAnimBatchManager.Instance().Register((KAnimConverter.IAnimConverter) this);
    this.forceRebuild = true;
    this.SetDirty();
  }

  protected override void DeRegister()
  {
    if (this.batch == null)
      return;
    this.batch.Deregister((KAnimConverter.IAnimConverter) this);
  }

  private void ConfigureUpdateListener()
  {
    if (this.IsActive() && !this.suspendUpdates && this.isVisible || (this.moving || this.visibilityType == KAnimControllerBase.VisibilityType.OffscreenUpdate) || this.visibilityType == KAnimControllerBase.VisibilityType.Always)
      Singleton<KBatchedAnimUpdater>.Instance.UpdateRegister(this);
    else
      Singleton<KBatchedAnimUpdater>.Instance.UpdateUnregister(this);
  }

  protected override void SuspendUpdates(bool suspend)
  {
    this.suspendUpdates = suspend;
    this.ConfigureUpdateListener();
  }

  public void SetVisiblity(bool is_visible)
  {
    if (is_visible == this.isVisible)
      return;
    this.isVisible = is_visible;
    if (is_visible)
    {
      this.SuspendUpdates(false);
      this.SetDirty();
      this.UpdateAnimEventSequenceTime();
    }
    else
    {
      this.SuspendUpdates(true);
      this.SetDirty();
    }
  }

  private void ConfigureVisibilityListener(bool enabled)
  {
    if (this.visibilityType == KAnimControllerBase.VisibilityType.Always || this.visibilityType == KAnimControllerBase.VisibilityType.OffscreenUpdate)
      return;
    if (enabled)
      this.RegisterVisibilityListener();
    else
      this.UnregisterVisibilityListener();
  }

  protected override void RefreshVisibilityListener()
  {
    if (!this.visibilityListenerRegistered)
      return;
    this.ConfigureVisibilityListener(false);
    this.ConfigureVisibilityListener(true);
  }

  private void RegisterVisibilityListener()
  {
    DebugUtil.Assert(!this.visibilityListenerRegistered);
    Singleton<KBatchedAnimUpdater>.Instance.VisibilityRegister(this);
    this.visibilityListenerRegistered = true;
  }

  private void UnregisterVisibilityListener()
  {
    DebugUtil.Assert(this.visibilityListenerRegistered);
    Singleton<KBatchedAnimUpdater>.Instance.VisibilityUnregister(this);
    this.visibilityListenerRegistered = false;
  }

  public void SetSceneLayer(Grid.SceneLayer layer)
  {
    float layerZ = Grid.GetLayerZ(layer);
    this.sceneLayer = layer;
    Vector3 position = this.transform.GetPosition();
    position.z = layerZ;
    this.transform.SetPosition(position);
    this.DeRegister();
    this.Register();
  }
}
