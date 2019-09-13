// Decompiled with JetBrains decompiler
// Type: CameraController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

public class CameraController : KMonoBehaviour, IInputHandler
{
  public float MAX_Y_SCALE = 1.1f;
  public GridVisibleArea VisibleArea = new GridVisibleArea();
  private float maxOrthographicSize = 20f;
  public List<Camera> cameras = new List<Camera>();
  private int cinemaZoomSpeed = 10;
  private float cinemaEasing = 0.05f;
  public const float DEFAULT_MAX_ORTHO_SIZE = 20f;
  public LocText infoText;
  private const float FIXED_Z = -100f;
  public bool FreeCameraEnabled;
  public float zoomSpeed;
  public float minOrthographicSize;
  public float zoomFactor;
  public float keyPanningSpeed;
  public float keyPanningEasing;
  public Texture2D dayColourCube;
  public Texture2D nightColourCube;
  public Material LightBufferMaterial;
  public Material LightCircleOverlay;
  public Material LightConeOverlay;
  public Transform followTarget;
  public Vector3 followTargetPos;
  private float overrideZoomSpeed;
  private bool panning;
  private Vector3 keyPanDelta;
  [SerializeField]
  private LayerMask timelapseCameraCullingMask;
  [SerializeField]
  private LayerMask timelapseOverlayCameraCullingMask;
  private bool userCameraControlDisabled;
  private bool panLeft;
  private bool panRight;
  private bool panUp;
  private bool panDown;
  [NonSerialized]
  public Camera baseCamera;
  [NonSerialized]
  public Camera overlayCamera;
  [NonSerialized]
  public Camera overlayNoDepthCamera;
  [NonSerialized]
  public Camera uiCamera;
  [NonSerialized]
  public Camera lightBufferCamera;
  [NonSerialized]
  public Camera simOverlayCamera;
  [NonSerialized]
  public Camera infraredCamera;
  [NonSerialized]
  public Camera timelapseFreezeCamera;
  private MultipleRenderTarget mrt;
  public SoundCuller soundCuller;
  private bool cinemaCamEnabled;
  private bool cinemaToggleLock;
  private bool cinemaToggleEasing;
  private bool cinemaPanLeft;
  private bool cinemaPanRight;
  private bool cinemaPanUp;
  private bool cinemaPanDown;
  private bool cinemaZoomIn;
  private bool cinemaZoomOut;
  private float cinemaZoomVelocity;
  private Coroutine activeFadeRoutine;

  public string handlerName
  {
    get
    {
      return this.gameObject.name;
    }
  }

  public KInputHandler inputHandler { get; set; }

  public float targetOrthographicSize { get; private set; }

  public bool isTargetPosSet { get; set; }

  public Vector3 targetPos { get; private set; }

  public bool DisableUserCameraControl
  {
    get
    {
      return this.userCameraControlDisabled;
    }
    set
    {
      this.userCameraControlDisabled = value;
      if (!this.userCameraControlDisabled)
        return;
      this.panning = false;
      this.panLeft = false;
      this.panRight = false;
      this.panUp = false;
      this.panDown = false;
    }
  }

  public static CameraController Instance { get; private set; }

  public static void DestroyInstance()
  {
    CameraController.Instance = (CameraController) null;
  }

  public void ToggleColouredOverlayView(bool enabled)
  {
    this.mrt.ToggleColouredOverlayView(enabled);
  }

  protected override void OnPrefabInit()
  {
    Util.Reset(this.transform);
    this.transform.SetLocalPosition(new Vector3(Grid.WidthInMeters / 2f, Grid.HeightInMeters / 2f, -100f));
    this.targetOrthographicSize = this.maxOrthographicSize;
    CameraController.Instance = this;
    this.DisableUserCameraControl = false;
    this.baseCamera = this.CopyCamera(Camera.main, "baseCamera");
    this.mrt = this.baseCamera.gameObject.AddComponent<MultipleRenderTarget>();
    this.mrt.onSetupComplete += new System.Action<Camera>(this.OnMRTSetupComplete);
    this.baseCamera.gameObject.AddComponent<LightBufferCompositor>();
    this.baseCamera.transparencySortMode = TransparencySortMode.Orthographic;
    this.baseCamera.transform.parent = this.transform;
    Util.Reset(this.baseCamera.transform);
    int mask1 = LayerMask.GetMask("PlaceWithDepth", "Overlay");
    int mask2 = LayerMask.GetMask("Construction");
    this.cameras.Add(this.baseCamera);
    this.baseCamera.cullingMask &= ~mask1;
    this.baseCamera.cullingMask |= mask2;
    this.baseCamera.tag = "Untagged";
    this.baseCamera.gameObject.AddComponent<CameraRenderTexture>().TextureName = "_LitTex";
    this.infraredCamera = this.CopyCamera(this.baseCamera, "Infrared");
    this.infraredCamera.cullingMask = 0;
    this.infraredCamera.clearFlags = CameraClearFlags.Color;
    this.infraredCamera.depth = this.baseCamera.depth - 1f;
    this.infraredCamera.transform.parent = this.transform;
    this.infraredCamera.gameObject.AddComponent<Infrared>();
    this.simOverlayCamera = this.CopyCamera(this.baseCamera, "SimOverlayCamera");
    this.simOverlayCamera.cullingMask = LayerMask.GetMask("SimDebugView");
    this.simOverlayCamera.clearFlags = CameraClearFlags.Color;
    this.simOverlayCamera.depth = this.baseCamera.depth + 1f;
    this.simOverlayCamera.transform.parent = this.transform;
    this.simOverlayCamera.gameObject.AddComponent<CameraRenderTexture>().TextureName = "_SimDebugViewTex";
    this.overlayCamera = Camera.main;
    this.overlayCamera.name = "Overlay";
    this.overlayCamera.cullingMask = mask1 | mask2;
    this.overlayCamera.clearFlags = CameraClearFlags.Nothing;
    this.overlayCamera.transform.parent = this.transform;
    this.overlayCamera.depth = this.baseCamera.depth + 3f;
    this.overlayCamera.transform.SetLocalPosition(Vector3.zero);
    this.overlayCamera.transform.localRotation = Quaternion.identity;
    this.overlayCamera.renderingPath = RenderingPath.Forward;
    this.overlayCamera.allowHDR = false;
    this.overlayCamera.tag = "Untagged";
    this.overlayCamera.gameObject.AddComponent<CameraReferenceTexture>().referenceCamera = this.baseCamera;
    ColorCorrectionLookup component = this.overlayCamera.GetComponent<ColorCorrectionLookup>();
    component.Convert(this.dayColourCube, string.Empty);
    component.Convert2(this.nightColourCube, string.Empty);
    this.cameras.Add(this.overlayCamera);
    this.lightBufferCamera = this.CopyCamera(this.overlayCamera, "Light Buffer");
    this.lightBufferCamera.clearFlags = CameraClearFlags.Color;
    this.lightBufferCamera.cullingMask = LayerMask.GetMask("Lights");
    this.lightBufferCamera.depth = this.baseCamera.depth - 1f;
    this.lightBufferCamera.transform.parent = this.transform;
    this.lightBufferCamera.transform.SetLocalPosition(Vector3.zero);
    this.lightBufferCamera.rect = new Rect(0.0f, 0.0f, 1f, 1f);
    LightBuffer lightBuffer = this.lightBufferCamera.gameObject.AddComponent<LightBuffer>();
    lightBuffer.Material = this.LightBufferMaterial;
    lightBuffer.CircleMaterial = this.LightCircleOverlay;
    lightBuffer.ConeMaterial = this.LightConeOverlay;
    this.overlayNoDepthCamera = this.CopyCamera(this.overlayCamera, "overlayNoDepth");
    int mask3 = LayerMask.GetMask("Overlay", "Place");
    this.baseCamera.cullingMask &= ~mask3;
    this.overlayNoDepthCamera.clearFlags = CameraClearFlags.Depth;
    this.overlayNoDepthCamera.cullingMask = mask3;
    this.overlayNoDepthCamera.transform.parent = this.transform;
    this.overlayNoDepthCamera.transform.SetLocalPosition(Vector3.zero);
    this.overlayNoDepthCamera.depth = this.baseCamera.depth + 4f;
    this.overlayNoDepthCamera.tag = "MainCamera";
    this.overlayNoDepthCamera.gameObject.AddComponent<NavPathDrawer>();
    this.uiCamera = this.CopyCamera(this.overlayCamera, "uiCamera");
    this.uiCamera.clearFlags = CameraClearFlags.Depth;
    this.uiCamera.cullingMask = LayerMask.GetMask("UI");
    this.uiCamera.transform.parent = this.transform;
    this.uiCamera.transform.SetLocalPosition(Vector3.zero);
    this.uiCamera.depth = this.baseCamera.depth + 5f;
    this.timelapseFreezeCamera = this.CopyCamera(this.uiCamera, "timelapseFreezeCamera");
    this.timelapseFreezeCamera.depth = this.uiCamera.depth + 3f;
    this.timelapseFreezeCamera.gameObject.AddComponent<FillRenderTargetEffect>();
    this.timelapseFreezeCamera.enabled = false;
    Camera camera = CameraController.CloneCamera(this.overlayCamera, "timelapseCamera");
    Timelapser timelapser = camera.gameObject.AddComponent<Timelapser>();
    camera.transparencySortMode = TransparencySortMode.Orthographic;
    Game.Instance.timelapser = timelapser;
    GameScreenManager.Instance.SetCamera(GameScreenManager.UIRenderTarget.ScreenSpaceCamera, this.uiCamera);
    GameScreenManager.Instance.SetCamera(GameScreenManager.UIRenderTarget.WorldSpace, this.uiCamera);
    GameScreenManager.Instance.SetCamera(GameScreenManager.UIRenderTarget.ScreenshotModeCamera, this.uiCamera);
    this.infoText = GameScreenManager.Instance.screenshotModeCanvas.GetComponentInChildren<LocText>();
  }

  public static Camera CloneCamera(Camera camera, string name)
  {
    GameObject gameObject = new GameObject();
    gameObject.name = name;
    Camera camera1 = gameObject.AddComponent<Camera>();
    camera1.CopyFrom(camera);
    return camera1;
  }

  private Camera CopyCamera(Camera camera, string name)
  {
    Camera camera1 = CameraController.CloneCamera(camera, name);
    this.cameras.Add(camera1);
    return camera1;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Restore();
  }

  public void FadeOut(float targetPercentage = 1f, float speed = 1f)
  {
    if (this.activeFadeRoutine != null)
      this.StopCoroutine(this.activeFadeRoutine);
    this.activeFadeRoutine = this.StartCoroutine(this.FadeToBlack(targetPercentage, speed));
  }

  public void FadeIn(float targetPercentage = 0.0f, float speed = 1f)
  {
    if (this.activeFadeRoutine != null)
      this.StopCoroutine(this.activeFadeRoutine);
    this.activeFadeRoutine = this.StartCoroutine(this.FadeInFromBlack(targetPercentage, speed));
  }

  public void SetWorldInteractive(bool state)
  {
    GameScreenManager.Instance.fadePlane.raycastTarget = !state;
  }

  [DebuggerHidden]
  private IEnumerator FadeToBlack(float targetBlackPercent = 1f, float speed = 1f)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new CameraController.\u003CFadeToBlack\u003Ec__Iterator0()
    {
      targetBlackPercent = targetBlackPercent,
      speed = speed,
      \u0024this = this
    };
  }

  [DebuggerHidden]
  private IEnumerator FadeInFromBlack(float targetBlackPercent = 0.0f, float speed = 1f)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new CameraController.\u003CFadeInFromBlack\u003Ec__Iterator1()
    {
      targetBlackPercent = targetBlackPercent,
      speed = speed,
      \u0024this = this
    };
  }

  public void EnableFreeCamera(bool enable)
  {
    this.FreeCameraEnabled = enable;
    this.SetInfoText("Screenshot Mode (ESC to exit)");
  }

  private static bool WithinInputField()
  {
    UnityEngine.EventSystems.EventSystem current = UnityEngine.EventSystems.EventSystem.current;
    if ((UnityEngine.Object) current == (UnityEngine.Object) null)
      return false;
    bool flag = false;
    if ((UnityEngine.Object) current.currentSelectedGameObject != (UnityEngine.Object) null && ((UnityEngine.Object) current.currentSelectedGameObject.GetComponent<TMP_InputField>() != (UnityEngine.Object) null || (UnityEngine.Object) current.currentSelectedGameObject.GetComponent<InputField>() != (UnityEngine.Object) null))
      flag = true;
    return flag;
  }

  private void SetInfoText(string text)
  {
    this.infoText.text = text;
    Color color = this.infoText.color;
    color.a = 0.5f;
    this.infoText.color = color;
  }

  public void OnKeyDown(KButtonEvent e)
  {
    if (e.Consumed || this.DisableUserCameraControl || CameraController.WithinInputField() || (UnityEngine.Object) SaveGame.Instance != (UnityEngine.Object) null && SaveGame.Instance.GetComponent<UserNavigation>().Handle(e))
      return;
    if (e.TryConsume(Action.ZoomIn))
    {
      this.targetOrthographicSize = Mathf.Max(this.targetOrthographicSize - this.zoomFactor * this.targetOrthographicSize, this.minOrthographicSize);
      this.overrideZoomSpeed = 0.0f;
      this.isTargetPosSet = false;
    }
    else if (e.TryConsume(Action.ZoomOut))
    {
      this.targetOrthographicSize = Mathf.Min(this.targetOrthographicSize + this.zoomFactor * this.targetOrthographicSize, !this.FreeCameraEnabled ? this.maxOrthographicSize : TuningData<CameraController.Tuning>.Get().maxOrthographicSizeDebug);
      this.overrideZoomSpeed = 0.0f;
      this.isTargetPosSet = false;
    }
    else if (e.TryConsume(Action.MouseMiddle) || e.IsAction(Action.MouseRight))
    {
      this.panning = true;
      this.overrideZoomSpeed = 0.0f;
      this.isTargetPosSet = false;
    }
    else if (this.FreeCameraEnabled && e.TryConsume(Action.CinemaCamEnable))
    {
      this.cinemaCamEnabled = !this.cinemaCamEnabled;
      DebugUtil.LogArgs((object) "Cinema Cam Enabled ", (object) this.cinemaCamEnabled);
      this.SetInfoText(!this.cinemaCamEnabled ? "Cinema Cam Disabled" : "Cinema Cam Enabled");
    }
    else if (this.FreeCameraEnabled && this.cinemaCamEnabled)
    {
      if (e.TryConsume(Action.CinemaToggleLock))
      {
        this.cinemaToggleLock = !this.cinemaToggleLock;
        DebugUtil.LogArgs((object) "Cinema Toggle Lock ", (object) this.cinemaToggleLock);
        this.SetInfoText(!this.cinemaToggleLock ? "Cinema Input Lock OFF" : "Cinema Input Lock ON");
      }
      else if (e.TryConsume(Action.CinemaToggleEasing))
      {
        this.cinemaToggleEasing = !this.cinemaToggleEasing;
        DebugUtil.LogArgs((object) "Cinema Toggle Easing ", (object) this.cinemaToggleEasing);
        this.SetInfoText(!this.cinemaToggleEasing ? "Cinema Easing OFF" : "Cinema Easing ON");
      }
      else if (e.TryConsume(Action.CinemaPanLeft))
      {
        this.cinemaPanLeft = !this.cinemaToggleLock || !this.cinemaPanLeft;
        this.cinemaPanRight = false;
      }
      else if (e.TryConsume(Action.CinemaPanRight))
      {
        this.cinemaPanRight = !this.cinemaToggleLock || !this.cinemaPanRight;
        this.cinemaPanLeft = false;
      }
      else if (e.TryConsume(Action.CinemaPanUp))
      {
        this.cinemaPanUp = !this.cinemaToggleLock || !this.cinemaPanUp;
        this.cinemaPanDown = false;
      }
      else if (e.TryConsume(Action.CinemaPanDown))
      {
        this.cinemaPanDown = !this.cinemaToggleLock || !this.cinemaPanDown;
        this.cinemaPanUp = false;
      }
      else if (e.TryConsume(Action.CinemaZoomIn))
      {
        this.cinemaZoomIn = !this.cinemaToggleLock || !this.cinemaZoomIn;
        this.cinemaZoomOut = false;
      }
      else if (e.TryConsume(Action.CinemaZoomOut))
      {
        this.cinemaZoomOut = !this.cinemaToggleLock || !this.cinemaZoomOut;
        this.cinemaZoomIn = false;
      }
      else if (e.TryConsume(Action.CinemaZoomSpeedPlus))
      {
        ++this.cinemaZoomSpeed;
        DebugUtil.LogArgs((object) "Cinema Zoom Speed ", (object) this.cinemaZoomSpeed);
        this.SetInfoText("Cinema Zoom Speed: " + (object) this.cinemaZoomSpeed);
      }
      else if (e.TryConsume(Action.CinemaZoomSpeedMinus))
      {
        --this.cinemaZoomSpeed;
        DebugUtil.LogArgs((object) "Cinema Zoom Speed ", (object) this.cinemaZoomSpeed);
        this.SetInfoText("Cinema Zoom Speed: " + (object) this.cinemaZoomSpeed);
      }
    }
    else if (e.TryConsume(Action.PanLeft))
      this.panLeft = true;
    else if (e.TryConsume(Action.PanRight))
      this.panRight = true;
    else if (e.TryConsume(Action.PanUp))
      this.panUp = true;
    else if (e.TryConsume(Action.PanDown))
      this.panDown = true;
    if (e.Consumed || !((UnityEngine.Object) OverlayMenu.Instance != (UnityEngine.Object) null))
      return;
    OverlayMenu.Instance.OnKeyDown(e);
  }

  public void OnKeyUp(KButtonEvent e)
  {
    if (this.DisableUserCameraControl || CameraController.WithinInputField())
      return;
    if (e.TryConsume(Action.MouseMiddle) || e.IsAction(Action.MouseRight))
      this.panning = false;
    else if (this.FreeCameraEnabled && this.cinemaCamEnabled)
    {
      if (e.TryConsume(Action.CinemaPanLeft))
        this.cinemaPanLeft = this.cinemaToggleLock && this.cinemaPanLeft;
      else if (e.TryConsume(Action.CinemaPanRight))
        this.cinemaPanRight = this.cinemaToggleLock && this.cinemaPanRight;
      else if (e.TryConsume(Action.CinemaPanUp))
        this.cinemaPanUp = this.cinemaToggleLock && this.cinemaPanUp;
      else if (e.TryConsume(Action.CinemaPanDown))
        this.cinemaPanDown = this.cinemaToggleLock && this.cinemaPanDown;
      else if (e.TryConsume(Action.CinemaZoomIn))
      {
        this.cinemaZoomIn = this.cinemaToggleLock && this.cinemaZoomIn;
      }
      else
      {
        if (!e.TryConsume(Action.CinemaZoomOut))
          return;
        this.cinemaZoomOut = this.cinemaToggleLock && this.cinemaZoomOut;
      }
    }
    else if (e.TryConsume(Action.CameraHome))
      this.CameraGoHome(2f);
    else if (e.TryConsume(Action.PanLeft))
      this.panLeft = false;
    else if (e.TryConsume(Action.PanRight))
      this.panRight = false;
    else if (e.TryConsume(Action.PanUp))
    {
      this.panUp = false;
    }
    else
    {
      if (!e.TryConsume(Action.PanDown))
        return;
      this.panDown = false;
    }
  }

  public void ForcePanningState(bool state)
  {
    this.panning = false;
  }

  public void CameraGoHome(float speed = 2f)
  {
    GameObject telepad = GameUtil.GetTelepad();
    if (!((UnityEngine.Object) telepad != (UnityEngine.Object) null))
      return;
    this.SetTargetPos(new Vector3(telepad.transform.GetPosition().x, telepad.transform.GetPosition().y + 1f, this.transform.GetPosition().z), 10f, true);
    this.SetOverrideZoomSpeed(speed);
  }

  public void CameraGoTo(Vector3 pos, float speed = 2f, bool playSound = true)
  {
    pos.z = this.transform.GetPosition().z;
    this.SetTargetPos(pos, 10f, playSound);
    this.SetOverrideZoomSpeed(speed);
  }

  public void SnapTo(Vector3 pos)
  {
    this.ClearFollowTarget();
    pos.z = -100f;
    this.transform.SetPosition(pos);
    this.keyPanDelta = Vector3.zero;
    this.SetOrthographicsSize(this.targetOrthographicSize);
  }

  public void SetOverrideZoomSpeed(float tempZoomSpeed)
  {
    this.overrideZoomSpeed = tempZoomSpeed;
  }

  public void SetTargetPos(Vector3 pos, float orthographic_size, bool playSound)
  {
    this.ClearFollowTarget();
    if (playSound && !this.isTargetPosSet)
      KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Click_Notification", false));
    this.isTargetPosSet = true;
    pos.z = -100f;
    this.targetPos = pos;
    this.targetOrthographicSize = orthographic_size;
  }

  public void SetMaxOrthographicSize(float size)
  {
    this.maxOrthographicSize = size;
  }

  public void SetOrthographicsSize(float size)
  {
    for (int index = 0; index < this.cameras.Count; ++index)
      this.cameras[index].orthographicSize = size;
  }

  public void SetPosition(Vector3 pos)
  {
    this.transform.SetPosition(pos);
  }

  private Vector3 PointUnderCursor(Vector3 mousePos, Camera cam)
  {
    Ray ray = cam.ScreenPointToRay(mousePos);
    Vector3 direction = ray.direction;
    Vector3 vector3 = direction * Mathf.Abs(cam.transform.GetPosition().z / direction.z);
    return ray.origin + vector3;
  }

  private void CinemaCamUpdate()
  {
    float unscaledDeltaTime = Time.unscaledDeltaTime;
    Camera main = Camera.main;
    Vector3 localPosition = this.transform.GetLocalPosition();
    float num1 = Mathf.Pow((float) this.cinemaZoomSpeed, 3f);
    if (this.cinemaZoomIn)
    {
      this.overrideZoomSpeed = -num1 / TuningData<CameraController.Tuning>.Get().cinemaZoomFactor;
      this.isTargetPosSet = false;
    }
    else if (this.cinemaZoomOut)
    {
      this.overrideZoomSpeed = num1 / TuningData<CameraController.Tuning>.Get().cinemaZoomFactor;
      this.isTargetPosSet = false;
    }
    else
      this.overrideZoomSpeed = 0.0f;
    if (this.cinemaToggleEasing)
      this.cinemaZoomVelocity += (this.overrideZoomSpeed - this.cinemaZoomVelocity) * this.cinemaEasing;
    else
      this.cinemaZoomVelocity = this.overrideZoomSpeed;
    if ((double) this.cinemaZoomVelocity != 0.0)
    {
      this.SetOrthographicsSize(main.orthographicSize + (float) ((double) this.cinemaZoomVelocity * (double) unscaledDeltaTime * ((double) main.orthographicSize / 20.0)));
      this.targetOrthographicSize = main.orthographicSize;
    }
    float num2 = num1 / TuningData<CameraController.Tuning>.Get().cinemaZoomToFactor;
    float num3 = this.keyPanningSpeed / 20f * main.orthographicSize;
    float num4 = num3 * (num1 / TuningData<CameraController.Tuning>.Get().cinemaPanToFactor);
    if (!this.isTargetPosSet && (double) this.targetOrthographicSize != (double) main.orthographicSize)
    {
      float t = Mathf.Min(num2 * unscaledDeltaTime, 0.1f);
      this.SetOrthographicsSize(Mathf.Lerp(main.orthographicSize, this.targetOrthographicSize, t));
    }
    Vector3 vector3_1 = Vector3.zero;
    if (this.isTargetPosSet)
    {
      float num5 = this.cinemaEasing * TuningData<CameraController.Tuning>.Get().targetZoomEasingFactor;
      float num6 = this.cinemaEasing * TuningData<CameraController.Tuning>.Get().targetPanEasingFactor;
      float f1 = this.targetOrthographicSize - main.orthographicSize;
      Vector3 vector3_2 = this.targetPos - localPosition;
      float num7;
      float num8;
      if (!this.cinemaToggleEasing)
      {
        num7 = num2 * unscaledDeltaTime;
        num8 = num4 * unscaledDeltaTime;
      }
      else
      {
        DebugUtil.LogArgs((object) "Min zoom of:", (object) (float) ((double) num2 * (double) unscaledDeltaTime), (object) (float) ((double) Mathf.Abs(f1) * (double) num5 * (double) unscaledDeltaTime));
        num7 = Mathf.Min(num2 * unscaledDeltaTime, Mathf.Abs(f1) * num5 * unscaledDeltaTime);
        DebugUtil.LogArgs((object) "Min pan of:", (object) (float) ((double) num4 * (double) unscaledDeltaTime), (object) (float) ((double) vector3_2.magnitude * (double) num6 * (double) unscaledDeltaTime));
        num8 = Mathf.Min(num4 * unscaledDeltaTime, vector3_2.magnitude * num6 * unscaledDeltaTime);
      }
      float f2 = (double) Mathf.Abs(f1) >= (double) num7 ? Mathf.Sign(f1) * num7 : f1;
      vector3_1 = (double) vector3_2.magnitude >= (double) num8 ? vector3_2.normalized * num8 : vector3_2;
      if ((double) Mathf.Abs(f2) < 1.0 / 1000.0 && (double) vector3_1.magnitude < 1.0 / 1000.0)
      {
        this.isTargetPosSet = false;
        f2 = f1;
        vector3_1 = vector3_2;
      }
      this.SetOrthographicsSize(main.orthographicSize + f2 * (main.orthographicSize / 20f));
    }
    if (!PlayerController.Instance.IsDragging())
      this.panning = false;
    Vector3 vector3_3 = Vector3.zero;
    if (this.panning)
    {
      vector3_3 = -PlayerController.Instance.GetWorldDragDelta();
      this.isTargetPosSet = false;
      if ((double) vector3_3.magnitude > 0.0)
        this.ClearFollowTarget();
      this.keyPanDelta = Vector3.zero;
    }
    else
    {
      float num5 = num1 / TuningData<CameraController.Tuning>.Get().cinemaPanFactor;
      Vector3 zero = Vector3.zero;
      if (this.cinemaPanLeft)
      {
        this.ClearFollowTarget();
        zero.x = -num3 * num5;
        this.isTargetPosSet = false;
      }
      if (this.cinemaPanRight)
      {
        this.ClearFollowTarget();
        zero.x = num3 * num5;
        this.isTargetPosSet = false;
      }
      if (this.cinemaPanUp)
      {
        this.ClearFollowTarget();
        zero.y = num3 * num5;
        this.isTargetPosSet = false;
      }
      if (this.cinemaPanDown)
      {
        this.ClearFollowTarget();
        zero.y = -num3 * num5;
        this.isTargetPosSet = false;
      }
      if (this.cinemaToggleEasing)
        this.keyPanDelta += (zero - this.keyPanDelta) * this.cinemaEasing;
      else
        this.keyPanDelta = zero;
    }
    Vector3 position = localPosition + vector3_1 + vector3_3 + this.keyPanDelta * unscaledDeltaTime;
    if ((UnityEngine.Object) this.followTarget != (UnityEngine.Object) null)
    {
      position.x = this.followTargetPos.x;
      position.y = this.followTargetPos.y;
    }
    position.z = -100f;
    if ((double) (position - this.transform.GetLocalPosition()).magnitude <= 0.001)
      return;
    this.transform.SetLocalPosition(position);
  }

  private void NormalCamUpdate()
  {
    float unscaledDeltaTime = Time.unscaledDeltaTime;
    Camera main = Camera.main;
    float num1 = (double) this.overrideZoomSpeed == 0.0 ? this.zoomSpeed : this.overrideZoomSpeed;
    Vector3 localPosition = this.transform.GetLocalPosition();
    Vector3 vector3_1 = (double) this.overrideZoomSpeed == 0.0 ? KInputManager.GetMousePos() : new Vector3((float) Screen.width / 2f, (float) Screen.height / 2f, 0.0f);
    Vector3 position1 = this.PointUnderCursor(vector3_1, main);
    Vector3 viewportPoint1 = main.ScreenToViewportPoint(vector3_1);
    float num2 = this.keyPanningSpeed / 20f * main.orthographicSize;
    float t = Mathf.Min(num1 * unscaledDeltaTime, 0.1f);
    this.SetOrthographicsSize(Mathf.Lerp(main.orthographicSize, this.targetOrthographicSize, t));
    this.transform.SetLocalPosition(localPosition);
    Vector3 viewportPoint2 = main.WorldToViewportPoint(position1);
    viewportPoint1.z = viewportPoint2.z;
    Vector3 vector3_2 = main.ViewportToWorldPoint(viewportPoint2) - main.ViewportToWorldPoint(viewportPoint1);
    if (this.isTargetPosSet)
    {
      vector3_2 = Vector3.Lerp(localPosition, this.targetPos, num1 * unscaledDeltaTime) - localPosition;
      if ((double) vector3_2.magnitude < 1.0 / 1000.0)
      {
        this.isTargetPosSet = false;
        vector3_2 = this.targetPos - localPosition;
      }
    }
    if (!PlayerController.Instance.IsDragging())
      this.panning = false;
    Vector3 vector3_3 = Vector3.zero;
    if (this.panning)
    {
      vector3_3 = -PlayerController.Instance.GetWorldDragDelta();
      this.isTargetPosSet = false;
    }
    Vector3 position2 = localPosition + vector3_2 + vector3_3;
    if (this.panning)
    {
      if ((double) vector3_3.magnitude > 0.0)
        this.ClearFollowTarget();
      this.keyPanDelta = Vector3.zero;
    }
    else if (!this.DisableUserCameraControl)
    {
      if (this.panLeft)
      {
        this.ClearFollowTarget();
        this.keyPanDelta.x -= num2;
        this.isTargetPosSet = false;
        this.overrideZoomSpeed = 0.0f;
      }
      if (this.panRight)
      {
        this.ClearFollowTarget();
        this.keyPanDelta.x += num2;
        this.isTargetPosSet = false;
        this.overrideZoomSpeed = 0.0f;
      }
      if (this.panUp)
      {
        this.ClearFollowTarget();
        this.keyPanDelta.y += num2;
        this.isTargetPosSet = false;
        this.overrideZoomSpeed = 0.0f;
      }
      if (this.panDown)
      {
        this.ClearFollowTarget();
        this.keyPanDelta.y -= num2;
        this.isTargetPosSet = false;
        this.overrideZoomSpeed = 0.0f;
      }
      Vector3 vector3_4 = new Vector3(Mathf.Lerp(0.0f, this.keyPanDelta.x, unscaledDeltaTime * this.keyPanningEasing), Mathf.Lerp(0.0f, this.keyPanDelta.y, unscaledDeltaTime * this.keyPanningEasing), 0.0f);
      this.keyPanDelta -= vector3_4;
      position2.x += vector3_4.x;
      position2.y += vector3_4.y;
    }
    if ((UnityEngine.Object) this.followTarget != (UnityEngine.Object) null)
    {
      position2.x = this.followTargetPos.x;
      position2.y = this.followTargetPos.y;
    }
    position2.z = -100f;
    if ((double) (position2 - this.transform.GetLocalPosition()).magnitude <= 0.001)
      return;
    this.transform.SetLocalPosition(position2);
  }

  private void Update()
  {
    if (!Game.Instance.timelapser.CapturingTimelapseScreenshot)
    {
      if (this.FreeCameraEnabled && this.cinemaCamEnabled)
        this.CinemaCamUpdate();
      else
        this.NormalCamUpdate();
    }
    if ((double) this.infoText.color.a > 0.0)
    {
      Color color = this.infoText.color;
      color.a = Mathf.Max(0.0f, this.infoText.color.a - Time.unscaledDeltaTime * 0.5f);
      this.infoText.color = color;
    }
    this.ConstrainToWorld();
    Vector3 vector3 = this.PointUnderCursor(KInputManager.GetMousePos(), Camera.main);
    Shader.SetGlobalVector("_WorldCameraPos", new Vector4(this.transform.GetPosition().x, this.transform.GetPosition().y, this.transform.GetPosition().z, Camera.main.orthographicSize));
    Shader.SetGlobalVector("_WorldCursorPos", new Vector4(vector3.x, vector3.y, 0.0f, 0.0f));
    this.VisibleArea.Update();
    this.soundCuller = SoundCuller.CreateCuller();
  }

  private Vector3 GetFollowPos()
  {
    if (!((UnityEngine.Object) this.followTarget != (UnityEngine.Object) null))
      return Vector3.zero;
    Vector3 vector3 = this.followTarget.transform.GetPosition();
    KAnimControllerBase component = this.followTarget.GetComponent<KAnimControllerBase>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      vector3 = component.GetWorldPivot();
    return vector3;
  }

  private void ConstrainToWorld()
  {
    if (Game.Instance.IsLoading() || this.FreeCameraEnabled)
      return;
    Camera main = Camera.main;
    Ray ray1 = main.ViewportPointToRay(Vector3.zero);
    Ray ray2 = main.ViewportPointToRay(Vector3.one);
    float distance1 = Mathf.Abs(ray1.origin.z / ray1.direction.z);
    float distance2 = Mathf.Abs(ray2.origin.z / ray2.direction.z);
    Vector3 point1 = ray1.GetPoint(distance1);
    Vector3 point2 = ray2.GetPoint(distance2);
    if ((double) point2.x - (double) point1.x > (double) Grid.WidthInMeters || (double) point2.y - (double) point1.y > (double) Grid.HeightInMeters)
      return;
    Vector3 vector3_1 = this.transform.GetPosition() - ray1.origin;
    Vector3 vector3_2 = point1;
    vector3_2.x = Mathf.Max(0.0f, vector3_2.x);
    vector3_2.y = Mathf.Max(0.0f, vector3_2.y);
    ray1.origin = vector3_2;
    ray1.direction = -ray1.direction;
    vector3_2 = ray1.GetPoint(distance1);
    this.transform.SetPosition(vector3_2 + vector3_1);
    Vector3 vector3_3 = this.transform.GetPosition() - ray2.origin;
    vector3_2 = point2;
    vector3_2.x = Mathf.Min(Grid.WidthInMeters, vector3_2.x);
    vector3_2.y = Mathf.Min(Grid.HeightInMeters * this.MAX_Y_SCALE, vector3_2.y);
    ray2.origin = vector3_2;
    ray2.direction = -ray2.direction;
    vector3_2 = ray2.GetPoint(distance2);
    Vector3 position = vector3_2 + vector3_3;
    position.z = -100f;
    this.transform.SetPosition(position);
  }

  public void Save(BinaryWriter writer)
  {
    writer.Write(this.transform.GetPosition());
    writer.Write(this.transform.localScale);
    writer.Write(this.transform.rotation);
    writer.Write(this.targetOrthographicSize);
    CameraSaveData.position = this.transform.GetPosition();
    CameraSaveData.localScale = this.transform.localScale;
    CameraSaveData.rotation = this.transform.rotation;
  }

  private void Restore()
  {
    if (!CameraSaveData.valid)
      return;
    int cell = Grid.PosToCell(CameraSaveData.position);
    if (Grid.IsValidCell(cell) && !Grid.IsVisible(cell))
    {
      Debug.LogWarning((object) "Resetting Camera Position... camera was saved in an undiscovered area of the map.");
      this.CameraGoHome(2f);
    }
    else
    {
      this.transform.SetPosition(CameraSaveData.position);
      this.transform.localScale = CameraSaveData.localScale;
      this.transform.rotation = CameraSaveData.rotation;
      this.targetOrthographicSize = Mathf.Clamp(CameraSaveData.orthographicsSize, this.minOrthographicSize, !this.FreeCameraEnabled ? this.maxOrthographicSize : TuningData<CameraController.Tuning>.Get().maxOrthographicSizeDebug);
      this.SnapTo(this.transform.GetPosition());
    }
  }

  private void OnMRTSetupComplete(Camera cam)
  {
    this.cameras.Add(cam);
  }

  public bool IsAudibleSound(Vector2 pos)
  {
    return this.soundCuller.IsAudible(pos);
  }

  public bool IsAudibleSound(Vector3 pos, string sound_path)
  {
    return this.soundCuller.IsAudible((Vector2) pos, sound_path);
  }

  public Vector3 GetVerticallyScaledPosition(Vector2 pos)
  {
    return this.soundCuller.GetVerticallyScaledPosition(pos);
  }

  public bool IsVisiblePos(Vector3 pos)
  {
    GridArea visibleArea = GridVisibleArea.GetVisibleArea();
    if (visibleArea.Min <= (Vector2) pos)
      return (Vector2) pos <= visibleArea.Max;
    return false;
  }

  protected override void OnCleanUp()
  {
    CameraController.Instance = (CameraController) null;
  }

  public void SetFollowTarget(Transform follow_target)
  {
    this.ClearFollowTarget();
    if ((UnityEngine.Object) follow_target == (UnityEngine.Object) null)
      return;
    this.followTarget = follow_target;
    this.SetOrthographicsSize(6f);
    this.targetOrthographicSize = 6f;
    Vector3 followPos = this.GetFollowPos();
    this.followTargetPos = new Vector3(followPos.x, followPos.y, this.transform.GetPosition().z);
    this.transform.SetPosition(this.followTargetPos);
    this.followTarget.GetComponent<KMonoBehaviour>().Trigger(-1506069671, (object) null);
  }

  public void ClearFollowTarget()
  {
    if ((UnityEngine.Object) this.followTarget == (UnityEngine.Object) null)
      return;
    this.followTarget.GetComponent<KMonoBehaviour>().Trigger(-485480405, (object) null);
    this.followTarget = (Transform) null;
  }

  public void UpdateFollowTarget()
  {
    if (!((UnityEngine.Object) this.followTarget != (UnityEngine.Object) null))
      return;
    Vector2 vector2 = Vector2.Lerp(new Vector2(this.transform.GetLocalPosition().x, this.transform.GetLocalPosition().y), (Vector2) this.GetFollowPos(), Time.unscaledDeltaTime * 25f);
    this.followTargetPos = new Vector3(vector2.x, vector2.y, this.transform.GetLocalPosition().z);
  }

  public void RenderForTimelapser(ref RenderTexture tex)
  {
    this.RenderCameraForTimelapse(this.baseCamera, ref tex, this.timelapseCameraCullingMask, -1f);
    CameraClearFlags clearFlags = this.overlayCamera.clearFlags;
    this.overlayCamera.clearFlags = CameraClearFlags.Nothing;
    this.RenderCameraForTimelapse(this.overlayCamera, ref tex, this.timelapseOverlayCameraCullingMask, -1f);
    this.overlayCamera.clearFlags = clearFlags;
  }

  private void RenderCameraForTimelapse(
    Camera cam,
    ref RenderTexture tex,
    LayerMask mask,
    float overrideAspect = -1f)
  {
    int cullingMask = cam.cullingMask;
    RenderTexture targetTexture = cam.targetTexture;
    cam.targetTexture = tex;
    cam.aspect = (float) tex.width / (float) tex.height;
    if ((double) overrideAspect != -1.0)
      cam.aspect = overrideAspect;
    if ((int) mask != -1)
      cam.cullingMask = (int) mask;
    cam.Render();
    cam.ResetAspect();
    cam.cullingMask = cullingMask;
    cam.targetTexture = targetTexture;
  }

  public class Tuning : TuningData<CameraController.Tuning>
  {
    public float cinemaZoomFactor = 100f;
    public float cinemaPanFactor = 50f;
    public float cinemaZoomToFactor = 100f;
    public float cinemaPanToFactor = 50f;
    public float targetZoomEasingFactor = 400f;
    public float targetPanEasingFactor = 100f;
    public float maxOrthographicSizeDebug;
  }
}
