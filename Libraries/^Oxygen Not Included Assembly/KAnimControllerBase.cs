// Decompiled with JetBrains decompiler
// Type: KAnimControllerBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class KAnimControllerBase : MonoBehaviour
{
  private static HashedString snaptoPivot = new HashedString("snapTo_pivot");
  [SerializeField]
  public KAnim.PlayMode initialMode = KAnim.PlayMode.Once;
  [SerializeField]
  protected KAnimFile[] animFiles = new KAnimFile[0];
  protected int curAnimFrameIdx = KAnim.Anim.Frame.InvalidFrame.idx;
  protected int prevAnimFrame = KAnim.Anim.Frame.InvalidFrame.idx;
  protected HandleVector<int>.Handle eventManagerHandle = HandleVector<int>.InvalidHandle;
  protected List<KAnimControllerBase.OverrideAnimFileData> overrideAnimFiles = new List<KAnimControllerBase.OverrideAnimFileData>();
  protected DeepProfiler DeepProfiler = new DeepProfiler(false);
  protected float playSpeed = 1f;
  protected KAnim.PlayMode mode = KAnim.PlayMode.Once;
  protected bool stopped = true;
  public float animHeight = 1f;
  public float animWidth = 1f;
  [SerializeField]
  protected bool _enabled = true;
  [SerializeField]
  protected List<KAnimHashedString> hiddenSymbols = new List<KAnimHashedString>();
  protected Dictionary<HashedString, KAnimControllerBase.AnimLookupData> anims = new Dictionary<HashedString, KAnimControllerBase.AnimLookupData>();
  protected Dictionary<HashedString, KAnimControllerBase.AnimLookupData> overrideAnims = new Dictionary<HashedString, KAnimControllerBase.AnimLookupData>();
  protected Queue<KAnimControllerBase.AnimData> animQueue = new Queue<KAnimControllerBase.AnimData>();
  public Grid.SceneLayer fgLayer = Grid.SceneLayer.NoLayer;
  [NonSerialized]
  public GameObject showWhenMissing;
  [SerializeField]
  public KAnimBatchGroup.MaterialType materialType;
  [SerializeField]
  public string initialAnim;
  [SerializeField]
  protected Vector3 offset;
  [SerializeField]
  protected Vector3 pivot;
  [SerializeField]
  protected float rotation;
  [SerializeField]
  public bool destroyOnAnimComplete;
  [SerializeField]
  public bool inactiveDisable;
  [SerializeField]
  protected bool flipX;
  [SerializeField]
  protected bool flipY;
  protected KAnim.Anim curAnim;
  public bool usingNewSymbolOverrideSystem;
  public bool randomiseLoopedOffset;
  protected float elapsedTime;
  protected bool isVisible;
  protected Bounds bounds;
  public System.Action<Bounds> OnUpdateBounds;
  public System.Action<Color> OnTintChanged;
  public System.Action<Color> OnHighlightChanged;
  protected KAnimSynchronizer synchronizer;
  protected KAnimLayering layering;
  protected bool hasEnableRun;
  protected bool hasAwakeRun;
  protected KBatchedAnimInstanceData batchInstanceData;
  public KAnimControllerBase.VisibilityType visibilityType;
  public System.Action<GameObject> onDestroySelf;
  protected int maxSymbols;
  protected AnimEventManager aem;

  protected KAnimControllerBase()
  {
    this.previousFrame = -1;
    this.currentFrame = -1;
    this.PlaySpeedMultiplier = 1f;
    this.synchronizer = new KAnimSynchronizer(this);
    this.layering = new KAnimLayering(this, this.fgLayer);
    this.isVisible = true;
  }

  public abstract KAnim.Anim GetAnim(int index);

  public string debugName { get; private set; }

  public KAnim.Build curBuild { get; protected set; }

  public event System.Action<Color32> OnOverlayColourChanged;

  public new bool enabled
  {
    get
    {
      return this._enabled;
    }
    set
    {
      this._enabled = value;
      if (!this.hasAwakeRun)
        return;
      if (this._enabled)
        this.Enable();
      else
        this.Disable();
    }
  }

  public bool HasBatchInstanceData
  {
    get
    {
      return this.batchInstanceData != null;
    }
  }

  public SymbolInstanceGpuData symbolInstanceGpuData { get; protected set; }

  public SymbolOverrideInfoGpuData symbolOverrideInfoGpuData { get; protected set; }

  public Color32 TintColour
  {
    get
    {
      return (Color32) this.batchInstanceData.GetTintColour();
    }
    set
    {
      if (this.batchInstanceData == null || !this.batchInstanceData.SetTintColour((Color) value))
        return;
      this.SetDirty();
      this.SuspendUpdates(false);
      if (this.OnTintChanged == null)
        return;
      this.OnTintChanged((Color) value);
    }
  }

  public Color32 HighlightColour
  {
    get
    {
      return (Color32) this.batchInstanceData.GetHighlightcolour();
    }
    set
    {
      if (!this.batchInstanceData.SetHighlightColour((Color) value))
        return;
      this.SetDirty();
      this.SuspendUpdates(false);
      if (this.OnHighlightChanged == null)
        return;
      this.OnHighlightChanged((Color) value);
    }
  }

  public Color OverlayColour
  {
    get
    {
      return this.batchInstanceData.GetOverlayColour();
    }
    set
    {
      if (!this.batchInstanceData.SetOverlayColour(value))
        return;
      this.SetDirty();
      this.SuspendUpdates(false);
      if (this.OnOverlayColourChanged == null)
        return;
      this.OnOverlayColourChanged((Color32) value);
    }
  }

  public event KAnimControllerBase.KAnimEvent onAnimEnter;

  public event KAnimControllerBase.KAnimEvent onAnimComplete;

  public event System.Action<int> onLayerChanged;

  public int previousFrame { get; protected set; }

  public int currentFrame { get; protected set; }

  public string currentAnim { get; protected set; }

  public string currentAnimFile { get; protected set; }

  public KAnimHashedString currentAnimFileHash { get; protected set; }

  public float PlaySpeedMultiplier { set; get; }

  public void SetFGLayer(Grid.SceneLayer layer)
  {
    this.fgLayer = layer;
    this.GetLayering();
    if (this.layering == null)
      return;
    this.layering.SetLayer(this.fgLayer);
  }

  public KAnim.PlayMode PlayMode
  {
    get
    {
      return this.mode;
    }
    set
    {
      this.mode = value;
    }
  }

  public bool FlipX
  {
    get
    {
      return this.flipX;
    }
    set
    {
      this.flipX = value;
    }
  }

  public bool FlipY
  {
    get
    {
      return this.flipY;
    }
    set
    {
      this.flipY = value;
    }
  }

  public Vector3 Offset
  {
    get
    {
      return this.offset;
    }
    set
    {
      this.offset = value;
      if (this.layering != null)
        this.layering.Dirty();
      this.DeRegister();
      this.Register();
      this.RefreshVisibilityListener();
      this.SetDirty();
    }
  }

  public float Rotation
  {
    get
    {
      return this.rotation;
    }
    set
    {
      this.rotation = value;
      if (this.layering != null)
        this.layering.Dirty();
      this.SetDirty();
    }
  }

  public Vector3 Pivot
  {
    get
    {
      return this.pivot;
    }
    set
    {
      this.pivot = value;
      if (this.layering != null)
        this.layering.Dirty();
      this.SetDirty();
    }
  }

  public Vector3 PositionIncludingOffset
  {
    get
    {
      return this.transform.GetPosition() + this.Offset;
    }
  }

  public KAnimBatchGroup.MaterialType GetMaterialType()
  {
    return this.materialType;
  }

  public Vector3 GetWorldPivot()
  {
    Vector3 position = this.transform.GetPosition();
    KBoxCollider2D component = this.GetComponent<KBoxCollider2D>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
    {
      position.x += component.offset.x;
      position.y += component.offset.y - component.size.y / 2f;
    }
    return position;
  }

  public KAnim.Anim GetCurrentAnim()
  {
    return this.curAnim;
  }

  public KAnimHashedString GetBuildHash()
  {
    if (this.curBuild == null)
      return (KAnimHashedString) KAnimBatchManager.NO_BATCH;
    return this.curBuild.fileHash;
  }

  protected float GetDuration()
  {
    if (this.curAnim != null)
      return (float) this.curAnim.numFrames / this.curAnim.frameRate;
    return 0.0f;
  }

  protected int GetFrameIdxFromOffset(int offset)
  {
    int num = -1;
    if (this.curAnim != null)
      num = offset + this.curAnim.firstFrameIdx;
    return num;
  }

  public int GetFrameIdx(float time, bool absolute)
  {
    int num = -1;
    if (this.curAnim != null)
      num = this.curAnim.GetFrameIdx(this.mode, time) + (!absolute ? 0 : this.curAnim.firstFrameIdx);
    return num;
  }

  public bool IsStopped()
  {
    return this.stopped;
  }

  public KAnim.Anim CurrentAnim
  {
    get
    {
      return this.curAnim;
    }
  }

  public KAnimSynchronizer GetSynchronizer()
  {
    return this.synchronizer;
  }

  public KAnimLayering GetLayering()
  {
    if (this.layering == null && this.fgLayer != Grid.SceneLayer.NoLayer)
      this.layering = new KAnimLayering(this, this.fgLayer);
    return this.layering;
  }

  public KAnim.PlayMode GetMode()
  {
    return this.mode;
  }

  public static string GetModeString(KAnim.PlayMode mode)
  {
    switch (mode)
    {
      case KAnim.PlayMode.Loop:
        return "Loop";
      case KAnim.PlayMode.Once:
        return "Once";
      case KAnim.PlayMode.Paused:
        return "Paused";
      default:
        return "Unknown";
    }
  }

  public float GetPlaySpeed()
  {
    return this.playSpeed;
  }

  public void SetElapsedTime(float value)
  {
    this.elapsedTime = value;
  }

  public float GetElapsedTime()
  {
    return this.elapsedTime;
  }

  protected abstract void SuspendUpdates(bool suspend);

  protected abstract void OnStartQueuedAnim();

  public abstract void SetDirty();

  protected abstract void RefreshVisibilityListener();

  protected abstract void DeRegister();

  protected abstract void Register();

  protected abstract void OnAwake();

  protected abstract void OnStart();

  protected abstract void OnStop();

  protected abstract void Enable();

  protected abstract void Disable();

  protected abstract void UpdateFrame(float t);

  public abstract Matrix2x3 GetTransformMatrix();

  public abstract Matrix2x3 GetSymbolLocalTransform(
    HashedString symbol,
    out bool symbolVisible);

  public abstract void UpdateHidden();

  public abstract void TriggerStop();

  public virtual void SetLayer(int layer)
  {
    if (this.onLayerChanged == null)
      return;
    this.onLayerChanged(layer);
  }

  public Vector3 GetPivotSymbolPosition()
  {
    bool symbolVisible = false;
    Matrix4x4 symbolTransform = this.GetSymbolTransform(KAnimControllerBase.snaptoPivot, out symbolVisible);
    Vector3 vector3 = this.transform.GetPosition();
    if (symbolVisible)
      vector3 = new Vector3(symbolTransform[0, 3], symbolTransform[1, 3], symbolTransform[2, 3]);
    return vector3;
  }

  public virtual Matrix4x4 GetSymbolTransform(HashedString symbol, out bool symbolVisible)
  {
    symbolVisible = false;
    return Matrix4x4.identity;
  }

  private void Awake()
  {
    if ((UnityEngine.Object) Global.Instance != (UnityEngine.Object) null)
      this.aem = Global.Instance.GetAnimEventManager();
    this.debugName = this.name;
    this.SetFGLayer(this.fgLayer);
    this.OnAwake();
    if (!string.IsNullOrEmpty(this.initialAnim))
    {
      this.SetDirty();
      this.Play((HashedString) this.initialAnim, this.initialMode, 1f, 0.0f);
    }
    this.hasAwakeRun = true;
  }

  private void Start()
  {
    this.OnStart();
  }

  protected virtual void OnDestroy()
  {
    this.animFiles = (KAnimFile[]) null;
    this.curAnim = (KAnim.Anim) null;
    this.curBuild = (KAnim.Build) null;
    this.synchronizer = (KAnimSynchronizer) null;
    this.layering = (KAnimLayering) null;
    this.animQueue = (Queue<KAnimControllerBase.AnimData>) null;
    this.overrideAnims = (Dictionary<HashedString, KAnimControllerBase.AnimLookupData>) null;
    this.anims = (Dictionary<HashedString, KAnimControllerBase.AnimLookupData>) null;
    this.synchronizer = (KAnimSynchronizer) null;
    this.layering = (KAnimLayering) null;
    this.overrideAnimFiles = (List<KAnimControllerBase.OverrideAnimFileData>) null;
  }

  protected void AnimEnter(HashedString hashed_name)
  {
    if (this.onAnimEnter == null)
      return;
    this.onAnimEnter(hashed_name);
  }

  public void Play(HashedString anim_name, KAnim.PlayMode mode = KAnim.PlayMode.Once, float speed = 1f, float time_offset = 0.0f)
  {
    if (!this.stopped)
      this.Stop();
    this.Queue(anim_name, mode, speed, time_offset);
  }

  public void Play(HashedString[] anim_names, KAnim.PlayMode mode = KAnim.PlayMode.Once)
  {
    if (!this.stopped)
      this.Stop();
    for (int index = 0; index < anim_names.Length - 1; ++index)
      this.Queue(anim_names[index], KAnim.PlayMode.Once, 1f, 0.0f);
    this.Queue(anim_names[anim_names.Length - 1], mode, 1f, 0.0f);
  }

  public void Queue(HashedString anim_name, KAnim.PlayMode mode = KAnim.PlayMode.Once, float speed = 1f, float time_offset = 0.0f)
  {
    this.animQueue.Enqueue(new KAnimControllerBase.AnimData()
    {
      anim = anim_name,
      mode = mode,
      speed = speed,
      timeOffset = time_offset
    });
    this.mode = mode != KAnim.PlayMode.Paused ? KAnim.PlayMode.Once : KAnim.PlayMode.Paused;
    if (this.aem != null)
      this.aem.SetMode(this.eventManagerHandle, this.mode);
    if (this.animQueue.Count != 1 || !this.stopped)
      return;
    this.StartQueuedAnim();
  }

  public void ClearQueue()
  {
    this.animQueue.Clear();
  }

  private void Restart(
    HashedString anim_name,
    KAnim.PlayMode mode = KAnim.PlayMode.Once,
    float speed = 1f,
    float time_offset = 0.0f)
  {
    if (this.curBuild == null)
    {
      Debug.LogWarning((object) ("[" + this.gameObject.name + "] Missing build while trying to play anim [" + (object) anim_name + "]"), (UnityEngine.Object) this.gameObject);
    }
    else
    {
      Queue<KAnimControllerBase.AnimData> animDataQueue = new Queue<KAnimControllerBase.AnimData>();
      animDataQueue.Enqueue(new KAnimControllerBase.AnimData()
      {
        anim = anim_name,
        mode = mode,
        speed = speed,
        timeOffset = time_offset
      });
      while (this.animQueue.Count > 0)
        animDataQueue.Enqueue(this.animQueue.Dequeue());
      this.animQueue = animDataQueue;
      if (this.animQueue.Count != 1 || !this.stopped)
        return;
      this.StartQueuedAnim();
    }
  }

  protected void StartQueuedAnim()
  {
    this.StopAnimEventSequence();
    this.previousFrame = -1;
    this.currentFrame = -1;
    this.SuspendUpdates(false);
    this.stopped = false;
    this.OnStartQueuedAnim();
    KAnimControllerBase.AnimData animData = this.animQueue.Dequeue();
    while (animData.mode == KAnim.PlayMode.Loop && this.animQueue.Count > 0)
      animData = this.animQueue.Dequeue();
    KAnimControllerBase.AnimLookupData animLookupData;
    if (this.overrideAnims == null || !this.overrideAnims.TryGetValue(animData.anim, out animLookupData))
    {
      if (!this.anims.TryGetValue(animData.anim, out animLookupData))
      {
        bool flag = true;
        if ((UnityEngine.Object) this.showWhenMissing != (UnityEngine.Object) null)
          this.showWhenMissing.SetActive(true);
        if (flag)
        {
          this.TriggerStop();
          return;
        }
      }
      else if ((UnityEngine.Object) this.showWhenMissing != (UnityEngine.Object) null)
        this.showWhenMissing.SetActive(false);
    }
    this.curAnim = this.GetAnim(animLookupData.animIndex);
    int offset = 0;
    if (animData.mode == KAnim.PlayMode.Loop && this.randomiseLoopedOffset)
      offset = UnityEngine.Random.Range(0, this.curAnim.numFrames - 1);
    this.prevAnimFrame = -1;
    this.curAnimFrameIdx = this.GetFrameIdxFromOffset(offset);
    this.currentFrame = this.curAnimFrameIdx;
    this.mode = animData.mode;
    this.playSpeed = animData.speed * this.PlaySpeedMultiplier;
    this.SetElapsedTime((float) offset / this.curAnim.frameRate + animData.timeOffset);
    this.synchronizer.Sync();
    this.StartAnimEventSequence();
    this.AnimEnter(animData.anim);
  }

  public bool GetSymbolVisiblity(KAnimHashedString symbol)
  {
    return !this.hiddenSymbols.Contains(symbol);
  }

  public void SetSymbolVisiblity(KAnimHashedString symbol, bool is_visible)
  {
    if (is_visible)
      this.hiddenSymbols.Remove(symbol);
    else if (!this.hiddenSymbols.Contains(symbol))
      this.hiddenSymbols.Add(symbol);
    if (this.curBuild == null)
      return;
    this.UpdateHidden();
  }

  public void AddAnimOverrides(KAnimFile kanim_file, float priority = 0.0f)
  {
    Debug.Assert((UnityEngine.Object) kanim_file != (UnityEngine.Object) null);
    if (kanim_file.GetData().build != null && kanim_file.GetData().build.symbols.Length > 0)
    {
      SymbolOverrideController component = this.GetComponent<SymbolOverrideController>();
      DebugUtil.Assert((UnityEngine.Object) component != (UnityEngine.Object) null, "Anim overrides containing additional symbols require a symbol override controller.");
      component.AddBuildOverride(kanim_file.GetData(), 0);
    }
    this.overrideAnimFiles.Add(new KAnimControllerBase.OverrideAnimFileData()
    {
      priority = priority,
      file = kanim_file
    });
    this.overrideAnimFiles.Sort((Comparison<KAnimControllerBase.OverrideAnimFileData>) ((a, b) => b.priority.CompareTo(a.priority)));
    this.RebuildOverrides(kanim_file);
  }

  public void RemoveAnimOverrides(KAnimFile kanim_file)
  {
    Debug.Assert((UnityEngine.Object) kanim_file != (UnityEngine.Object) null);
    if (kanim_file.GetData().build != null && kanim_file.GetData().build.symbols.Length > 0)
    {
      SymbolOverrideController component = this.GetComponent<SymbolOverrideController>();
      DebugUtil.Assert((UnityEngine.Object) component != (UnityEngine.Object) null, "Anim overrides containing additional symbols require a symbol override controller.");
      component.TryRemoveBuildOverride(kanim_file.GetData(), 0);
    }
    for (int index = 0; index < this.overrideAnimFiles.Count; ++index)
    {
      if ((UnityEngine.Object) this.overrideAnimFiles[index].file == (UnityEngine.Object) kanim_file)
      {
        this.overrideAnimFiles.RemoveAt(index);
        break;
      }
    }
    this.RebuildOverrides(kanim_file);
  }

  private void RebuildOverrides(KAnimFile kanim_file)
  {
    bool flag = false;
    this.overrideAnims.Clear();
    for (int index1 = 0; index1 < this.overrideAnimFiles.Count; ++index1)
    {
      KAnimControllerBase.OverrideAnimFileData overrideAnimFile = this.overrideAnimFiles[index1];
      KAnimFileData data = overrideAnimFile.file.GetData();
      for (int index2 = 0; index2 < data.animCount; ++index2)
      {
        KAnim.Anim anim = data.GetAnim(index2);
        if (anim.animFile.hashName != data.hashName)
          Debug.LogError((object) string.Format("How did we get an anim from another file? [{0}] != [{1}] for anim [{2}]", (object) data.name, (object) anim.animFile.name, (object) index2));
        KAnimControllerBase.AnimLookupData animLookupData = new KAnimControllerBase.AnimLookupData();
        animLookupData.animIndex = anim.index;
        HashedString key = new HashedString(anim.name);
        if (!this.overrideAnims.ContainsKey(key))
          this.overrideAnims[key] = animLookupData;
        if (this.curAnim != null && this.curAnim.hash == key && (UnityEngine.Object) overrideAnimFile.file == (UnityEngine.Object) kanim_file)
          flag = true;
      }
    }
    if (!flag)
      return;
    this.Restart((HashedString) this.curAnim.name, this.mode, this.playSpeed, 0.0f);
  }

  public bool HasAnimation(HashedString anim_name)
  {
    return this.anims.ContainsKey(anim_name);
  }

  public void AddAnims(KAnimFile anim_file)
  {
    KAnimFileData data = anim_file.GetData();
    if (data == null)
    {
      Debug.LogError((object) "AddAnims() Null animfile data");
    }
    else
    {
      this.maxSymbols = Mathf.Max(this.maxSymbols, data.maxVisSymbolFrames);
      for (int index = 0; index < data.animCount; ++index)
      {
        KAnim.Anim anim = data.GetAnim(index);
        if (anim.animFile.hashName != data.hashName)
          Debug.LogErrorFormat("How did we get an anim from another file? [{0}] != [{1}] for anim [{2}]", (object) data.name, (object) anim.animFile.name, (object) index);
        this.anims[anim.hash] = new KAnimControllerBase.AnimLookupData()
        {
          animIndex = anim.index
        };
      }
      if (!this.usingNewSymbolOverrideSystem || data.buildIndex == -1 || (data.build.symbols == null || data.build.symbols.Length <= 0))
        return;
      this.GetComponent<SymbolOverrideController>().AddBuildOverride(anim_file.GetData(), -1);
    }
  }

  public KAnimFile[] AnimFiles
  {
    get
    {
      return this.animFiles;
    }
    set
    {
      DebugUtil.Assert(value.Length > 0, "Controller has no anim files.");
      DebugUtil.Assert(value[0].buildBytes != null, "First anim file needs to be the build file.");
      for (int index = 0; index < value.Length; ++index)
        DebugUtil.Assert((UnityEngine.Object) value[index] != (UnityEngine.Object) null, "Anim file is null");
      this.animFiles = new KAnimFile[value.Length];
      for (int index = 0; index < value.Length; ++index)
        this.animFiles[index] = value[index];
    }
  }

  public void Stop()
  {
    if (this.curAnim != null)
      this.StopAnimEventSequence();
    this.animQueue.Clear();
    this.stopped = true;
    if (this.onAnimComplete != null)
      this.onAnimComplete(this.curAnim != null ? this.curAnim.hash : HashedString.Invalid);
    this.OnStop();
  }

  public void StopAndClear()
  {
    if (!this.stopped)
      this.Stop();
    this.bounds.center = Vector3.zero;
    this.bounds.extents = Vector3.zero;
    if (this.OnUpdateBounds == null)
      return;
    this.OnUpdateBounds(this.bounds);
  }

  public float GetPositionPercent()
  {
    return this.GetElapsedTime() / this.GetDuration();
  }

  public void SetPositionPercent(float percent)
  {
    if (this.curAnim == null)
      return;
    this.SetElapsedTime((float) this.curAnim.numFrames / this.curAnim.frameRate * percent);
    if (this.currentFrame == this.curAnim.GetFrameIdx(this.mode, this.elapsedTime))
      return;
    this.SetDirty();
    this.UpdateAnimEventSequenceTime();
    this.SuspendUpdates(false);
  }

  protected void StartAnimEventSequence()
  {
    if (this.layering.GetIsForeground() || this.aem == null)
      return;
    this.eventManagerHandle = this.aem.PlayAnim(this, this.curAnim, this.mode, this.elapsedTime, this.visibilityType == KAnimControllerBase.VisibilityType.Always);
  }

  protected void UpdateAnimEventSequenceTime()
  {
    if (!this.eventManagerHandle.IsValid() || this.aem == null)
      return;
    this.aem.SetElapsedTime(this.eventManagerHandle, this.elapsedTime);
  }

  protected void StopAnimEventSequence()
  {
    if (!this.eventManagerHandle.IsValid() || this.aem == null)
      return;
    if (!this.stopped && this.mode != KAnim.PlayMode.Paused)
      this.SetElapsedTime(this.aem.GetElapsedTime(this.eventManagerHandle));
    this.aem.StopAnim(this.eventManagerHandle);
    this.eventManagerHandle = HandleVector<int>.InvalidHandle;
  }

  protected void DestroySelf()
  {
    if (this.onDestroySelf != null)
      this.onDestroySelf(this.gameObject);
    else
      Util.KDestroyGameObject(this.gameObject);
  }

  public struct OverrideAnimFileData
  {
    public float priority;
    public KAnimFile file;
  }

  public struct AnimLookupData
  {
    public int animIndex;
  }

  public struct AnimData
  {
    public HashedString anim;
    public KAnim.PlayMode mode;
    public float speed;
    public float timeOffset;
  }

  public enum VisibilityType
  {
    Default,
    OffscreenUpdate,
    Always,
  }

  public delegate void KAnimEvent(HashedString name);
}
