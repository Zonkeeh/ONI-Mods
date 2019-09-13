// Decompiled with JetBrains decompiler
// Type: Global
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using KMod;
using Steamworks;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using UnityEngine;
using UnityEngine.U2D;

public class Global : MonoBehaviour
{
  public static readonly string LanguagePackKey = "LanguagePack";
  public static readonly string LanguageCodeKey = "LanguageCode";
  public SpriteAtlas[] forcedAtlasInitializationList;
  public GameObject modErrorsPrefab;
  public GameObject globalCanvas;
  private GameInputManager mInputManager;
  private AnimEventManager mAnimEventManager;
  public KMod.Manager modManager;
  private bool gotKleiUserID;
  public Thread mainThread;
  private bool updated_with_initialized_distribution_platform;

  public static Global Instance { get; private set; }

  public static BindingEntry[] GenerateDefaultBindings()
  {
    List<BindingEntry> bindings = new List<BindingEntry>()
    {
      new BindingEntry((string) null, GamepadButton.NumButtons, KKeyCode.Escape, Modifier.None, Action.Escape, false, false),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.W, Modifier.None, Action.PanUp, true, false),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.S, Modifier.None, Action.PanDown, true, false),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.A, Modifier.None, Action.PanLeft, true, false),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.D, Modifier.None, Action.PanRight, true, false),
      new BindingEntry("Tool", GamepadButton.NumButtons, KKeyCode.O, Modifier.None, Action.RotateBuilding, true, false),
      new BindingEntry("Management", GamepadButton.NumButtons, KKeyCode.L, Modifier.None, Action.ManagePriorities, true, false),
      new BindingEntry("Management", GamepadButton.NumButtons, KKeyCode.F, Modifier.None, Action.ManageConsumables, true, false),
      new BindingEntry("Management", GamepadButton.NumButtons, KKeyCode.V, Modifier.None, Action.ManageVitals, true, false),
      new BindingEntry("Management", GamepadButton.NumButtons, KKeyCode.R, Modifier.None, Action.ManageResearch, true, false),
      new BindingEntry("Management", GamepadButton.NumButtons, KKeyCode.E, Modifier.None, Action.ManageReport, true, false),
      new BindingEntry("Management", GamepadButton.NumButtons, KKeyCode.U, Modifier.None, Action.ManageDatabase, true, false),
      new BindingEntry("Management", GamepadButton.NumButtons, KKeyCode.J, Modifier.None, Action.ManageSkills, true, false),
      new BindingEntry("Management", GamepadButton.NumButtons, KKeyCode.Period, Modifier.None, Action.ManageSchedule, true, false),
      new BindingEntry("Management", GamepadButton.NumButtons, KKeyCode.Z, Modifier.None, Action.ManageStarmap, true, false),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.G, Modifier.None, Action.Dig, true, false),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.M, Modifier.None, Action.Mop, true, false),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.K, Modifier.None, Action.Clear, true, false),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.I, Modifier.None, Action.Disinfect, true, false),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.T, Modifier.None, Action.Attack, true, false),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.N, Modifier.None, Action.Capture, true, false),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.Y, Modifier.None, Action.Harvest, true, false),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.Insert, Modifier.None, Action.EmptyPipe, true, false),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.P, Modifier.None, Action.Prioritize, true, false),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.S, Modifier.Alt, Action.ToggleScreenshotMode, true, false),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.C, Modifier.None, Action.BuildingCancel, true, false),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.X, Modifier.None, Action.BuildingDeconstruct, true, false),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.Tab, Modifier.None, Action.CycleSpeed, true, false),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.H, Modifier.None, Action.CameraHome, true, false),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.Mouse0, Modifier.None, Action.MouseLeft, false, false),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.Mouse0, Modifier.Shift, Action.ShiftMouseLeft, false, false),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.Mouse1, Modifier.None, Action.MouseRight, false, false),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.Mouse2, Modifier.None, Action.MouseMiddle, false, false),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.Alpha1, Modifier.None, Action.Plan1, true, false),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.Alpha2, Modifier.None, Action.Plan2, true, false),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.Alpha3, Modifier.None, Action.Plan3, true, false),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.Alpha4, Modifier.None, Action.Plan4, true, false),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.Alpha5, Modifier.None, Action.Plan5, true, false),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.Alpha6, Modifier.None, Action.Plan6, true, false),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.Alpha7, Modifier.None, Action.Plan7, true, false),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.Alpha8, Modifier.None, Action.Plan8, true, false),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.Alpha9, Modifier.None, Action.Plan9, true, false),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.Alpha0, Modifier.None, Action.Plan10, true, false),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.Minus, Modifier.None, Action.Plan11, true, false),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.Equals, Modifier.None, Action.Plan12, true, false),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.B, Modifier.None, Action.CopyBuilding, true, false),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.MouseScrollUp, Modifier.None, Action.ZoomIn, true, false),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.MouseScrollDown, Modifier.None, Action.ZoomOut, true, false),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.F1, Modifier.None, Action.Overlay1, true, false),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.F2, Modifier.None, Action.Overlay2, true, false),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.F3, Modifier.None, Action.Overlay3, true, false),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.F4, Modifier.None, Action.Overlay4, true, false),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.F5, Modifier.None, Action.Overlay5, true, false),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.F6, Modifier.None, Action.Overlay6, true, false),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.F7, Modifier.None, Action.Overlay7, true, false),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.F8, Modifier.None, Action.Overlay8, true, false),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.F9, Modifier.None, Action.Overlay9, true, false),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.F10, Modifier.None, Action.Overlay10, true, false),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.F11, Modifier.None, Action.Overlay11, true, false),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.F1, Modifier.Shift, Action.Overlay12, true, false),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.F2, Modifier.Shift, Action.Overlay13, true, false),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.F3, Modifier.Shift, Action.Overlay14, true, false),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.F4, Modifier.Shift, Action.Overlay15, true, false),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.KeypadPlus, Modifier.None, Action.SpeedUp, true, false),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.KeypadMinus, Modifier.None, Action.SlowDown, true, false),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.Space, Modifier.None, Action.TogglePause, true, false),
      new BindingEntry("Navigation", GamepadButton.NumButtons, KKeyCode.Alpha1, Modifier.Ctrl, Action.SetUserNav1, true, false),
      new BindingEntry("Navigation", GamepadButton.NumButtons, KKeyCode.Alpha2, Modifier.Ctrl, Action.SetUserNav2, true, false),
      new BindingEntry("Navigation", GamepadButton.NumButtons, KKeyCode.Alpha3, Modifier.Ctrl, Action.SetUserNav3, true, false),
      new BindingEntry("Navigation", GamepadButton.NumButtons, KKeyCode.Alpha4, Modifier.Ctrl, Action.SetUserNav4, true, false),
      new BindingEntry("Navigation", GamepadButton.NumButtons, KKeyCode.Alpha5, Modifier.Ctrl, Action.SetUserNav5, true, false),
      new BindingEntry("Navigation", GamepadButton.NumButtons, KKeyCode.Alpha6, Modifier.Ctrl, Action.SetUserNav6, true, false),
      new BindingEntry("Navigation", GamepadButton.NumButtons, KKeyCode.Alpha7, Modifier.Ctrl, Action.SetUserNav7, true, false),
      new BindingEntry("Navigation", GamepadButton.NumButtons, KKeyCode.Alpha8, Modifier.Ctrl, Action.SetUserNav8, true, false),
      new BindingEntry("Navigation", GamepadButton.NumButtons, KKeyCode.Alpha9, Modifier.Ctrl, Action.SetUserNav9, true, false),
      new BindingEntry("Navigation", GamepadButton.NumButtons, KKeyCode.Alpha0, Modifier.Ctrl, Action.SetUserNav10, true, false),
      new BindingEntry("Navigation", GamepadButton.NumButtons, KKeyCode.Alpha1, Modifier.Shift, Action.GotoUserNav1, true, false),
      new BindingEntry("Navigation", GamepadButton.NumButtons, KKeyCode.Alpha2, Modifier.Shift, Action.GotoUserNav2, true, false),
      new BindingEntry("Navigation", GamepadButton.NumButtons, KKeyCode.Alpha3, Modifier.Shift, Action.GotoUserNav3, true, false),
      new BindingEntry("Navigation", GamepadButton.NumButtons, KKeyCode.Alpha4, Modifier.Shift, Action.GotoUserNav4, true, false),
      new BindingEntry("Navigation", GamepadButton.NumButtons, KKeyCode.Alpha5, Modifier.Shift, Action.GotoUserNav5, true, false),
      new BindingEntry("Navigation", GamepadButton.NumButtons, KKeyCode.Alpha6, Modifier.Shift, Action.GotoUserNav6, true, false),
      new BindingEntry("Navigation", GamepadButton.NumButtons, KKeyCode.Alpha7, Modifier.Shift, Action.GotoUserNav7, true, false),
      new BindingEntry("Navigation", GamepadButton.NumButtons, KKeyCode.Alpha8, Modifier.Shift, Action.GotoUserNav8, true, false),
      new BindingEntry("Navigation", GamepadButton.NumButtons, KKeyCode.Alpha9, Modifier.Shift, Action.GotoUserNav9, true, false),
      new BindingEntry("Navigation", GamepadButton.NumButtons, KKeyCode.Alpha0, Modifier.Shift, Action.GotoUserNav10, true, false),
      new BindingEntry("CinematicCamera", GamepadButton.NumButtons, KKeyCode.C, Modifier.None, Action.CinemaCamEnable, true, true),
      new BindingEntry("CinematicCamera", GamepadButton.NumButtons, KKeyCode.A, Modifier.None, Action.CinemaPanLeft, true, true),
      new BindingEntry("CinematicCamera", GamepadButton.NumButtons, KKeyCode.D, Modifier.None, Action.CinemaPanRight, true, true),
      new BindingEntry("CinematicCamera", GamepadButton.NumButtons, KKeyCode.W, Modifier.None, Action.CinemaPanUp, true, true),
      new BindingEntry("CinematicCamera", GamepadButton.NumButtons, KKeyCode.S, Modifier.None, Action.CinemaPanDown, true, true),
      new BindingEntry("CinematicCamera", GamepadButton.NumButtons, KKeyCode.I, Modifier.None, Action.CinemaZoomIn, true, true),
      new BindingEntry("CinematicCamera", GamepadButton.NumButtons, KKeyCode.O, Modifier.None, Action.CinemaZoomOut, true, true),
      new BindingEntry("CinematicCamera", GamepadButton.NumButtons, KKeyCode.Z, Modifier.None, Action.CinemaZoomSpeedPlus, true, true),
      new BindingEntry("CinematicCamera", GamepadButton.NumButtons, KKeyCode.Z, Modifier.Shift, Action.CinemaZoomSpeedMinus, true, true),
      new BindingEntry("CinematicCamera", GamepadButton.NumButtons, KKeyCode.T, Modifier.None, Action.CinemaToggleLock, true, true),
      new BindingEntry("CinematicCamera", GamepadButton.NumButtons, KKeyCode.E, Modifier.None, Action.CinemaToggleEasing, true, true),
      new BindingEntry("Building", GamepadButton.NumButtons, KKeyCode.Slash, Modifier.None, Action.ToggleOpen, true, false),
      new BindingEntry("Building", GamepadButton.NumButtons, KKeyCode.Return, Modifier.None, Action.ToggleEnabled, true, false),
      new BindingEntry("Building", GamepadButton.NumButtons, KKeyCode.Backslash, Modifier.None, Action.BuildingUtility1, true, false),
      new BindingEntry("Building", GamepadButton.NumButtons, KKeyCode.LeftBracket, Modifier.None, Action.BuildingUtility2, true, false),
      new BindingEntry("Building", GamepadButton.NumButtons, KKeyCode.RightBracket, Modifier.None, Action.BuildingUtility3, true, false),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.LeftAlt, Modifier.Alt, Action.AlternateView, true, false),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.RightAlt, Modifier.Alt, Action.AlternateView, true, false),
      new BindingEntry("Tool", GamepadButton.NumButtons, KKeyCode.LeftShift, Modifier.Shift, Action.DragStraight, true, false),
      new BindingEntry("Tool", GamepadButton.NumButtons, KKeyCode.RightShift, Modifier.Shift, Action.DragStraight, true, false),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.T, Modifier.Ctrl, Action.DebugFocus, true, false),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.U, Modifier.Ctrl, Action.DebugUltraTestMode, true, false),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.F1, Modifier.Alt, Action.DebugToggleUI, true, false),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.F3, Modifier.Alt, Action.DebugCollectGarbage, true, false),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.F7, Modifier.Alt, Action.DebugInvincible, true, false),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.F10, Modifier.Alt, Action.DebugForceLightEverywhere, true, false),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.F10, Modifier.Shift, Action.DebugElementTest, true, false),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.F11, Modifier.Shift, Action.DebugRiverTest, true, false),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.F12, Modifier.Shift, Action.DebugTileTest, true, false),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.N, Modifier.Alt, Action.DebugRefreshNavCell, true, false),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.Q, Modifier.Ctrl, Action.DebugGotoTarget, true, false),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.S, Modifier.Ctrl, Action.DebugSelectMaterial, true, false),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.M, Modifier.Ctrl, Action.DebugToggleMusic, true, false),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.Backspace, Modifier.None, Action.DebugToggle, true, false),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.Q, Modifier.Alt, Action.DebugTeleport, true, false),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.F2, Modifier.Ctrl, Action.DebugSpawnMinion, true, false),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.F3, Modifier.Ctrl, Action.DebugPlace, true, false),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.F4, Modifier.Ctrl, Action.DebugInstantBuildMode, true, false),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.F5, Modifier.Ctrl, Action.DebugSlowTestMode, true, false),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.F6, Modifier.Ctrl, Action.DebugDig, true, false),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.F8, Modifier.Ctrl, Action.DebugExplosion, true, false),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.F9, Modifier.Ctrl, Action.DebugDiscoverAllElements, true, false),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.T, Modifier.Alt, Action.DebugToggleSelectInEditor, true, false),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.P, Modifier.Alt, Action.DebugPathFinding, true, false),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.Z, Modifier.Alt, Action.DebugSuperSpeed, true, false),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.Equals, Modifier.Alt, Action.DebugGameStep, true, false),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.Minus, Modifier.Alt, Action.DebugSimStep, true, false),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.X, Modifier.Alt, Action.DebugNotification, true, false),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.C, Modifier.Alt, Action.DebugNotificationMessage, true, false),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.BackQuote, Modifier.None, Action.ToggleProfiler, true, false),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.BackQuote, Modifier.Alt, Action.ToggleChromeProfiler, true, false),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.F1, Modifier.Ctrl, Action.DebugDumpSceneParitionerLeakData, true, false),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.F12, Modifier.Ctrl, Action.DebugTriggerException, true, false),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.F12, Modifier.Ctrl | Modifier.Shift, Action.DebugTriggerError, true, false),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.F10, Modifier.Ctrl, Action.DebugDumpGCRoots, true, false),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.F10, Modifier.Alt | Modifier.Ctrl, Action.DebugDumpGarbageReferences, true, false),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.F11, Modifier.Ctrl, Action.DebugDumpEventData, true, false),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.F7, Modifier.Alt | Modifier.Ctrl, Action.DebugCrashSim, true, false),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.Alpha9, Modifier.Alt, Action.DebugNextCall, true, false),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.Alpha1, Modifier.Alt, Action.SreenShot1x, true, false),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.Alpha2, Modifier.Alt, Action.SreenShot2x, true, false),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.Alpha3, Modifier.Alt, Action.SreenShot8x, true, false),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.Alpha4, Modifier.Alt, Action.SreenShot32x, true, false),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.Alpha5, Modifier.Alt, Action.DebugLockCursor, true, false),
      new BindingEntry("Debug", GamepadButton.NumButtons, KKeyCode.Alpha0, Modifier.Alt, Action.DebugTogglePersonalPriorityComparison, true, false),
      new BindingEntry("Root", GamepadButton.NumButtons, KKeyCode.Return, Modifier.None, Action.DialogSubmit, false, false),
      new BindingEntry("BuildingsMenu", GamepadButton.NumButtons, KKeyCode.A, Modifier.None, Action.BuildMenuKeyA, false, true),
      new BindingEntry("BuildingsMenu", GamepadButton.NumButtons, KKeyCode.B, Modifier.None, Action.BuildMenuKeyB, false, true),
      new BindingEntry("BuildingsMenu", GamepadButton.NumButtons, KKeyCode.C, Modifier.None, Action.BuildMenuKeyC, false, true),
      new BindingEntry("BuildingsMenu", GamepadButton.NumButtons, KKeyCode.D, Modifier.None, Action.BuildMenuKeyD, false, true),
      new BindingEntry("BuildingsMenu", GamepadButton.NumButtons, KKeyCode.E, Modifier.None, Action.BuildMenuKeyE, false, true),
      new BindingEntry("BuildingsMenu", GamepadButton.NumButtons, KKeyCode.F, Modifier.None, Action.BuildMenuKeyF, false, true),
      new BindingEntry("BuildingsMenu", GamepadButton.NumButtons, KKeyCode.G, Modifier.None, Action.BuildMenuKeyG, false, true),
      new BindingEntry("BuildingsMenu", GamepadButton.NumButtons, KKeyCode.H, Modifier.None, Action.BuildMenuKeyH, false, true),
      new BindingEntry("BuildingsMenu", GamepadButton.NumButtons, KKeyCode.I, Modifier.None, Action.BuildMenuKeyI, false, true),
      new BindingEntry("BuildingsMenu", GamepadButton.NumButtons, KKeyCode.J, Modifier.None, Action.BuildMenuKeyJ, false, true),
      new BindingEntry("BuildingsMenu", GamepadButton.NumButtons, KKeyCode.K, Modifier.None, Action.BuildMenuKeyK, false, true),
      new BindingEntry("BuildingsMenu", GamepadButton.NumButtons, KKeyCode.L, Modifier.None, Action.BuildMenuKeyL, false, true),
      new BindingEntry("BuildingsMenu", GamepadButton.NumButtons, KKeyCode.M, Modifier.None, Action.BuildMenuKeyM, false, true),
      new BindingEntry("BuildingsMenu", GamepadButton.NumButtons, KKeyCode.N, Modifier.None, Action.BuildMenuKeyN, false, true),
      new BindingEntry("BuildingsMenu", GamepadButton.NumButtons, KKeyCode.O, Modifier.None, Action.BuildMenuKeyO, false, true),
      new BindingEntry("BuildingsMenu", GamepadButton.NumButtons, KKeyCode.P, Modifier.None, Action.BuildMenuKeyP, false, true),
      new BindingEntry("BuildingsMenu", GamepadButton.NumButtons, KKeyCode.Q, Modifier.None, Action.BuildMenuKeyQ, false, true),
      new BindingEntry("BuildingsMenu", GamepadButton.NumButtons, KKeyCode.R, Modifier.None, Action.BuildMenuKeyR, false, true),
      new BindingEntry("BuildingsMenu", GamepadButton.NumButtons, KKeyCode.S, Modifier.None, Action.BuildMenuKeyS, false, true),
      new BindingEntry("BuildingsMenu", GamepadButton.NumButtons, KKeyCode.T, Modifier.None, Action.BuildMenuKeyT, false, true),
      new BindingEntry("BuildingsMenu", GamepadButton.NumButtons, KKeyCode.U, Modifier.None, Action.BuildMenuKeyU, false, true),
      new BindingEntry("BuildingsMenu", GamepadButton.NumButtons, KKeyCode.V, Modifier.None, Action.BuildMenuKeyV, false, true),
      new BindingEntry("BuildingsMenu", GamepadButton.NumButtons, KKeyCode.W, Modifier.None, Action.BuildMenuKeyW, false, true),
      new BindingEntry("BuildingsMenu", GamepadButton.NumButtons, KKeyCode.X, Modifier.None, Action.BuildMenuKeyX, false, true),
      new BindingEntry("BuildingsMenu", GamepadButton.NumButtons, KKeyCode.Y, Modifier.None, Action.BuildMenuKeyY, false, true),
      new BindingEntry("BuildingsMenu", GamepadButton.NumButtons, KKeyCode.Z, Modifier.None, Action.BuildMenuKeyZ, false, true),
      new BindingEntry("Sandbox", GamepadButton.NumButtons, KKeyCode.B, Modifier.Shift, Action.SandboxBrush, true, false),
      new BindingEntry("Sandbox", GamepadButton.NumButtons, KKeyCode.N, Modifier.Shift, Action.SandboxSprinkle, true, false),
      new BindingEntry("Sandbox", GamepadButton.NumButtons, KKeyCode.F, Modifier.Shift, Action.SandboxFlood, true, false),
      new BindingEntry("Sandbox", GamepadButton.NumButtons, KKeyCode.K, Modifier.Shift, Action.SandboxSample, true, false),
      new BindingEntry("Sandbox", GamepadButton.NumButtons, KKeyCode.H, Modifier.Shift, Action.SandboxHeatGun, true, false),
      new BindingEntry("Sandbox", GamepadButton.NumButtons, KKeyCode.C, Modifier.Shift, Action.SandboxClearFloor, true, false),
      new BindingEntry("Sandbox", GamepadButton.NumButtons, KKeyCode.X, Modifier.Shift, Action.SandboxDestroy, true, false),
      new BindingEntry("Sandbox", GamepadButton.NumButtons, KKeyCode.E, Modifier.Shift, Action.SandboxSpawnEntity, true, false),
      new BindingEntry("Sandbox", GamepadButton.NumButtons, KKeyCode.S, Modifier.Shift, Action.ToggleSandboxTools, true, false),
      new BindingEntry("Sandbox", GamepadButton.NumButtons, KKeyCode.R, Modifier.Shift, Action.SandboxReveal, true, false)
    };
    IList<BuildMenu.DisplayInfo> data = (IList<BuildMenu.DisplayInfo>) BuildMenu.OrderedBuildings.data;
    if (BuildMenu.UseHotkeyBuildMenu())
    {
      foreach (BuildMenu.DisplayInfo display_info in (IEnumerable<BuildMenu.DisplayInfo>) data)
        Global.AddBindings(HashedString.Invalid, display_info, bindings);
    }
    return bindings.ToArray();
  }

  private static void AddBindings(
    HashedString parent_category,
    BuildMenu.DisplayInfo display_info,
    List<BindingEntry> bindings)
  {
    if (display_info.data == null)
      return;
    System.Type type = display_info.data.GetType();
    if (typeof (IList<BuildMenu.DisplayInfo>).IsAssignableFrom(type))
    {
      foreach (BuildMenu.DisplayInfo display_info1 in (IEnumerable<BuildMenu.DisplayInfo>) display_info.data)
        Global.AddBindings(display_info.category, display_info1, bindings);
    }
    else
    {
      if (!typeof (IList<BuildMenu.BuildingInfo>).IsAssignableFrom(type))
        return;
      BindingEntry bindingEntry = new BindingEntry(new CultureInfo("en-US", false).TextInfo.ToTitleCase(HashCache.Get().Get(parent_category)) + " Menu", GamepadButton.NumButtons, display_info.keyCode, Modifier.None, display_info.hotkey, true, true);
      bindings.Add(bindingEntry);
    }
  }

  private void Awake()
  {
    KCrashReporter crash_reporter = this.GetComponent<KCrashReporter>();
    if ((UnityEngine.Object) crash_reporter != (UnityEngine.Object) null & SceneInitializerLoader.ReportDeferredError == null)
      SceneInitializerLoader.ReportDeferredError = (SceneInitializerLoader.DeferredErrorDelegate) (deferred_error => crash_reporter.ShowDialog(deferred_error.msg, deferred_error.stack_trace));
    this.globalCanvas = GameObject.Find("Canvas");
    UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object) this.globalCanvas.gameObject);
    this.OutputSystemInfo();
    Debug.Assert((UnityEngine.Object) Global.Instance == (UnityEngine.Object) null);
    Global.Instance = this;
    Debug.Log((object) ("Initializing at " + System.DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff")));
    Debug.Log((object) ("Save path: " + Util.RootFolder()));
    if (this.forcedAtlasInitializationList != null)
    {
      foreach (SpriteAtlas atlasInitialization in this.forcedAtlasInitializationList)
      {
        Sprite[] sprites = new Sprite[atlasInitialization.spriteCount];
        atlasInitialization.GetSprites(sprites);
        foreach (Sprite sprite in sprites)
        {
          Texture2D texture = sprite.texture;
          if ((UnityEngine.Object) texture != (UnityEngine.Object) null)
          {
            texture.filterMode = FilterMode.Bilinear;
            texture.anisoLevel = 4;
            texture.mipMapBias = 0.0f;
          }
        }
      }
    }
    FileSystem.Initialize();
    Singleton<StateMachineUpdater>.CreateInstance();
    Singleton<StateMachineManager>.CreateInstance();
    Localization.RegisterForTranslation(typeof (UI));
    this.modManager = new KMod.Manager();
    this.modManager.Load(Content.DLL);
    this.modManager.Load(Content.Strings);
    KSerialization.Manager.Initialize();
    this.mInputManager = new GameInputManager(Global.GenerateDefaultBindings());
    Audio.Get();
    KAnimBatchManager.CreateInstance();
    Singleton<SoundEventVolumeCache>.CreateInstance();
    this.mAnimEventManager = new AnimEventManager();
    Singleton<KBatchedAnimUpdater>.CreateInstance();
    DistributionPlatform.Initialize();
    Localization.Initialize(false);
    this.modManager.Load(Content.Translation);
    this.modManager.distribution_platforms.Add((IDistributionPlatform) new Local("Local", KMod.Label.DistributionPlatform.Local));
    this.modManager.distribution_platforms.Add((IDistributionPlatform) new Local("Dev", KMod.Label.DistributionPlatform.Dev));
    this.mainThread = Thread.CurrentThread;
    KProfiler.main_thread = Thread.CurrentThread;
    this.RestoreLegacyMetricsSetting();
    if (DistributionPlatform.Initialized)
    {
      if (!KPrivacyPrefs.instance.disableDataCollection)
      {
        Debug.Log((object) ("Logged into " + DistributionPlatform.Inst.Name + " with ID:" + (object) DistributionPlatform.Inst.LocalUser.Id + ", NAME:" + DistributionPlatform.Inst.LocalUser.Name));
        ThreadedHttps<KleiAccount>.Instance.AuthenticateUser(new KleiAccount.GetUserIDdelegate(this.OnGetUserIdKey));
      }
    }
    else
    {
      Debug.LogWarning((object) ("Can't init " + DistributionPlatform.Inst.Name + " distribution platform..."));
      this.OnGetUserIdKey();
    }
    this.modManager.Load(Content.LayerableFiles);
    GlobalResources.Instance();
  }

  private void RestoreLegacyMetricsSetting()
  {
    if (KPlayerPrefs.GetInt("ENABLE_METRICS", 1) != 0)
      return;
    KPlayerPrefs.DeleteKey("ENABLE_METRICS");
    KPlayerPrefs.Save();
    KPrivacyPrefs.instance.disableDataCollection = true;
    KPrivacyPrefs.Save();
  }

  public GameInputManager GetInputManager()
  {
    return this.mInputManager;
  }

  public AnimEventManager GetAnimEventManager()
  {
    if (App.IsExiting)
      return (AnimEventManager) null;
    return this.mAnimEventManager;
  }

  private void OnApplicationFocus(bool focus)
  {
    if (this.mInputManager == null)
      return;
    this.mInputManager.OnApplicationFocus(focus);
  }

  private void OnGetUserIdKey()
  {
    this.gotKleiUserID = true;
  }

  private void Update()
  {
    this.mInputManager.Update();
    if (this.mAnimEventManager != null)
      this.mAnimEventManager.Update();
    if (DistributionPlatform.Initialized && !this.updated_with_initialized_distribution_platform)
    {
      this.updated_with_initialized_distribution_platform = true;
      SteamUGCService.Initialize();
      Steam steam = new Steam();
      SteamUGCService.Instance.AddClient((SteamUGCService.IClient) steam);
      this.modManager.distribution_platforms.Add((IDistributionPlatform) steam);
      SteamAchievementService.Initialize();
    }
    if (this.gotKleiUserID)
    {
      this.gotKleiUserID = false;
      ThreadedHttps<KleiMetrics>.Instance.SetCallBacks(new System.Action(this.SetONIStaticSessionVariables), new System.Action<Dictionary<string, object>>(this.SetONIDynamicSessionVariables));
      ThreadedHttps<KleiMetrics>.Instance.StartSession();
    }
    ThreadedHttps<KleiMetrics>.Instance.SetLastUserAction(KInputManager.lastUserActionTicks);
    Localization.VerifyTranslationModSubscription(this.globalCanvas);
  }

  private void SetONIStaticSessionVariables()
  {
    ThreadedHttps<KleiMetrics>.Instance.SetStaticSessionVariable("Branch", (object) "release");
    ThreadedHttps<KleiMetrics>.Instance.SetStaticSessionVariable("Build", (object) 366134U);
    if (KPlayerPrefs.HasKey(UnitConfigurationScreen.MassUnitKey))
      ThreadedHttps<KleiMetrics>.Instance.SetStaticSessionVariable(UnitConfigurationScreen.MassUnitKey, (object) ((GameUtil.MassUnit) KPlayerPrefs.GetInt(UnitConfigurationScreen.MassUnitKey)).ToString());
    if (KPlayerPrefs.HasKey(UnitConfigurationScreen.TemperatureUnitKey))
      ThreadedHttps<KleiMetrics>.Instance.SetStaticSessionVariable(UnitConfigurationScreen.TemperatureUnitKey, (object) ((GameUtil.TemperatureUnit) KPlayerPrefs.GetInt(UnitConfigurationScreen.TemperatureUnitKey)).ToString());
    if (!SteamManager.Initialized)
      return;
    PublishedFileId_t installed;
    string installedLanguageCode = LanguageOptionsScreen.GetInstalledLanguageCode(out installed);
    if (installed != PublishedFileId_t.Invalid)
      ThreadedHttps<KleiMetrics>.Instance.SetStaticSessionVariable(Global.LanguagePackKey, (object) installed.m_PublishedFileId);
    if (string.IsNullOrEmpty(installedLanguageCode))
      return;
    ThreadedHttps<KleiMetrics>.Instance.SetStaticSessionVariable(Global.LanguageCodeKey, (object) installedLanguageCode);
  }

  private void SetONIDynamicSessionVariables(Dictionary<string, object> data)
  {
    if (!((UnityEngine.Object) Game.Instance != (UnityEngine.Object) null) || !((UnityEngine.Object) GameClock.Instance != (UnityEngine.Object) null))
      return;
    data.Add("GameTimeSeconds", (object) (int) GameClock.Instance.GetTime());
  }

  private void LateUpdate()
  {
    Singleton<KBatchedAnimUpdater>.Instance.LateUpdate();
  }

  private void OnDestroy()
  {
    if (this.modManager != null)
      this.modManager.Shutdown();
    Global.Instance = (Global) null;
    if (this.mAnimEventManager != null)
      this.mAnimEventManager.FreeResources();
    Singleton<KBatchedAnimUpdater>.DestroyInstance();
  }

  private void OnApplicationQuit()
  {
    KGlobalAnimParser.DestroyInstance();
    ThreadedHttps<KleiMetrics>.Instance.EndSession(false);
  }

  private void OutputSystemInfo()
  {
    try
    {
      Console.WriteLine("SYSTEM INFO:");
      foreach (KeyValuePair<string, object> hardwareStat in KleiMetrics.GetHardwareStats())
      {
        try
        {
          Console.WriteLine(string.Format("    {0}={1}", (object) hardwareStat.Key.ToString(), (object) hardwareStat.Value.ToString()));
        }
        catch
        {
        }
      }
      Console.WriteLine(string.Format("    {0}={1}", (object) "System Language", (object) Application.systemLanguage.ToString()));
    }
    catch
    {
    }
  }
}
