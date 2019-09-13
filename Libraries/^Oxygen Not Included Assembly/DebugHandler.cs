// Decompiled with JetBrains decompiler
// Type: DebugHandler
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using STRINGS;
using System;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public class DebugHandler : IInputHandler
{
  public static bool InstantBuildMode;
  public static bool InvincibleMode;
  public static bool SelectInEditor;
  public static bool DebugPathFinding;
  public static bool HideUI;
  public static bool DebugCellInfo;
  public static bool DebugNextCall;
  private bool superTestMode;
  private bool ultraTestMode;
  private bool slowTestMode;

  public DebugHandler()
  {
    DebugHandler.enabled = File.Exists(System.IO.Path.Combine(Application.dataPath, "debug_enable.txt"));
    DebugHandler.enabled = DebugHandler.enabled || File.Exists(System.IO.Path.Combine(Application.dataPath, "../debug_enable.txt"));
    DebugHandler.enabled = DebugHandler.enabled || GenericGameSettings.instance.debugEnable;
  }

  public static bool enabled { get; private set; }

  public string handlerName
  {
    get
    {
      return nameof (DebugHandler);
    }
  }

  public KInputHandler inputHandler { get; set; }

  public static int GetMouseCell()
  {
    Vector3 mousePos = KInputManager.GetMousePos();
    mousePos.z = -Camera.main.transform.GetPosition().z - Grid.CellSizeInMeters;
    return Grid.PosToCell(Camera.main.ScreenToWorldPoint(mousePos));
  }

  public static Vector3 GetMousePos()
  {
    Vector3 mousePos = KInputManager.GetMousePos();
    mousePos.z = -Camera.main.transform.GetPosition().z - Grid.CellSizeInMeters;
    return Camera.main.ScreenToWorldPoint(mousePos);
  }

  private void SpawnMinion()
  {
    if ((UnityEngine.Object) Immigration.Instance == (UnityEngine.Object) null)
      return;
    if (!Grid.IsValidBuildingCell(DebugHandler.GetMouseCell()))
    {
      PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, (string) UI.DEBUG_TOOLS.INVALID_LOCATION, (Transform) null, DebugHandler.GetMousePos(), 1.5f, false, true);
    }
    else
    {
      GameObject gameObject = Util.KInstantiate(Assets.GetPrefab((Tag) MinionConfig.ID), (GameObject) null, (string) null);
      gameObject.name = Assets.GetPrefab((Tag) MinionConfig.ID).name;
      Immigration.Instance.ApplyDefaultPersonalPriorities(gameObject);
      Vector3 posCbc = Grid.CellToPosCBC(DebugHandler.GetMouseCell(), Grid.SceneLayer.Move);
      gameObject.transform.SetLocalPosition(posCbc);
      gameObject.SetActive(true);
      new MinionStartingStats(false, (string) null).Apply(gameObject);
    }
  }

  public unsafe void OnKeyDown(KButtonEvent e)
  {
    if (!DebugHandler.enabled)
      return;
    if (e.TryConsume(Action.DebugSpawnMinion))
      this.SpawnMinion();
    else if (e.TryConsume(Action.DebugSpawnStressTest))
    {
      for (int index = 0; index < 60; ++index)
        this.SpawnMinion();
    }
    else if (e.TryConsume(Action.DebugSuperTestMode))
    {
      if (!this.superTestMode)
      {
        Time.timeScale = 15f;
        this.superTestMode = true;
      }
      else
      {
        Time.timeScale = 1f;
        this.superTestMode = false;
      }
    }
    else if (e.TryConsume(Action.DebugUltraTestMode))
    {
      if (!this.ultraTestMode)
      {
        Time.timeScale = 30f;
        this.ultraTestMode = true;
      }
      else
      {
        Time.timeScale = 1f;
        this.ultraTestMode = false;
      }
    }
    else if (e.TryConsume(Action.DebugSlowTestMode))
    {
      if (!this.slowTestMode)
      {
        Time.timeScale = 0.06f;
        this.slowTestMode = true;
      }
      else
      {
        Time.timeScale = 1f;
        this.slowTestMode = false;
      }
    }
    else if (e.TryConsume(Action.DebugDig))
      SimMessages.Dig(DebugHandler.GetMouseCell(), -1);
    else if (e.TryConsume(Action.DebugInstantBuildMode))
    {
      DebugHandler.InstantBuildMode = !DebugHandler.InstantBuildMode;
      if ((UnityEngine.Object) Game.Instance == (UnityEngine.Object) null)
        return;
      if ((UnityEngine.Object) PlanScreen.Instance != (UnityEngine.Object) null)
        PlanScreen.Instance.Refresh();
      if ((UnityEngine.Object) BuildMenu.Instance != (UnityEngine.Object) null)
        BuildMenu.Instance.Refresh();
      if ((UnityEngine.Object) OverlayMenu.Instance != (UnityEngine.Object) null)
        OverlayMenu.Instance.Refresh();
      if ((UnityEngine.Object) ConsumerManager.instance != (UnityEngine.Object) null)
        ConsumerManager.instance.RefreshDiscovered((object) null);
      if ((UnityEngine.Object) ManagementMenu.Instance != (UnityEngine.Object) null)
      {
        ManagementMenu.Instance.CheckResearch((object) null);
        ManagementMenu.Instance.CheckSkills((object) null);
        ManagementMenu.Instance.CheckStarmap((object) null);
      }
      Game.Instance.Trigger(1594320620, (object) "all_the_things");
    }
    else if (e.TryConsume(Action.DebugExplosion))
    {
      Vector3 mousePos = KInputManager.GetMousePos();
      mousePos.z = -Camera.main.transform.GetPosition().z - Grid.CellSizeInMeters;
      GameUtil.CreateExplosion(Camera.main.ScreenToWorldPoint(mousePos));
    }
    else if (e.TryConsume(Action.DebugLockCursor))
    {
      if (GenericGameSettings.instance.developerDebugEnable)
      {
        KInputManager.isMousePosLocked = !KInputManager.isMousePosLocked;
        KInputManager.lockedMousePos = KInputManager.GetMousePos();
      }
    }
    else if (e.TryConsume(Action.DebugDiscoverAllElements))
    {
      if ((UnityEngine.Object) WorldInventory.Instance != (UnityEngine.Object) null)
      {
        foreach (Element element in ElementLoader.elements)
          WorldInventory.Instance.Discover(element.tag, element.GetMaterialCategoryTag());
      }
    }
    else if (e.TryConsume(Action.DebugToggleUI))
      DebugHandler.ToggleScreenshotMode();
    else if (e.TryConsume(Action.SreenShot1x))
      ScreenCapture.CaptureScreenshot(System.IO.Path.ChangeExtension(SaveLoader.GetActiveSaveFilePath(), ".png"), 1);
    else if (e.TryConsume(Action.SreenShot2x))
      ScreenCapture.CaptureScreenshot(System.IO.Path.ChangeExtension(SaveLoader.GetActiveSaveFilePath(), ".png"), 2);
    else if (e.TryConsume(Action.SreenShot8x))
      ScreenCapture.CaptureScreenshot(System.IO.Path.ChangeExtension(SaveLoader.GetActiveSaveFilePath(), ".png"), 8);
    else if (e.TryConsume(Action.SreenShot32x))
      ScreenCapture.CaptureScreenshot(System.IO.Path.ChangeExtension(SaveLoader.GetActiveSaveFilePath(), ".png"), 32);
    else if (e.TryConsume(Action.DebugCellInfo))
      DebugHandler.DebugCellInfo = !DebugHandler.DebugCellInfo;
    else if (e.TryConsume(Action.DebugToggle))
    {
      if ((UnityEngine.Object) Game.Instance != (UnityEngine.Object) null)
      {
        Game.Instance.UpdateGameActiveRegion(0, 0, Grid.WidthInCells, Grid.HeightInCells);
        SaveGame.Instance.worldGenSpawner.SpawnEverything();
      }
      if ((UnityEngine.Object) DebugPaintElementScreen.Instance != (UnityEngine.Object) null)
      {
        bool activeSelf = DebugPaintElementScreen.Instance.gameObject.activeSelf;
        DebugPaintElementScreen.Instance.gameObject.SetActive(!activeSelf);
        if ((bool) ((UnityEngine.Object) DebugElementMenu.Instance) && DebugElementMenu.Instance.root.activeSelf)
          DebugElementMenu.Instance.root.SetActive(false);
        DebugBaseTemplateButton.Instance.gameObject.SetActive(!activeSelf);
        PropertyTextures.FogOfWarScale = activeSelf ? 0.0f : 1f;
        if ((UnityEngine.Object) CameraController.Instance != (UnityEngine.Object) null)
          CameraController.Instance.EnableFreeCamera(!activeSelf);
      }
    }
    else if (e.TryConsume(Action.DebugCollectGarbage))
      GC.Collect();
    else if (e.TryConsume(Action.DebugInvincible))
      DebugHandler.InvincibleMode = !DebugHandler.InvincibleMode;
    else if (e.TryConsume(Action.DebugVisualTest))
      Scenario.Instance.SetupVisualTest();
    else if (e.TryConsume(Action.DebugGameplayTest))
      Scenario.Instance.SetupGameplayTest();
    else if (e.TryConsume(Action.DebugElementTest))
      Scenario.Instance.SetupElementTest();
    else if (e.TryConsume(Action.ToggleProfiler))
      Sim.SIM_HandleMessage(-409964931, 0, (byte*) null);
    else if (e.TryConsume(Action.DebugRefreshNavCell))
      Pathfinding.Instance.RefreshNavCell(DebugHandler.GetMouseCell());
    else if (e.TryConsume(Action.DebugToggleSelectInEditor))
      DebugHandler.SetSelectInEditor(!DebugHandler.SelectInEditor);
    else if (e.TryConsume(Action.DebugGotoTarget))
    {
      Debug.Log((object) "Debug GoTo");
      Game.Instance.Trigger(775300118, (object) null);
      foreach (Brain cmp in Components.Brains.Items)
      {
        cmp.GetSMI<DebugGoToMonitor.Instance>()?.GoToCursor();
        cmp.GetSMI<CreatureDebugGoToMonitor.Instance>()?.GoToCursor();
      }
    }
    else if (e.TryConsume(Action.DebugTeleport))
    {
      if ((UnityEngine.Object) SelectTool.Instance == (UnityEngine.Object) null)
        return;
      KSelectable selected = SelectTool.Instance.selected;
      if ((UnityEngine.Object) selected != (UnityEngine.Object) null)
      {
        int mouseCell = DebugHandler.GetMouseCell();
        if (!Grid.IsValidBuildingCell(mouseCell))
        {
          PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, (string) UI.DEBUG_TOOLS.INVALID_LOCATION, (Transform) null, DebugHandler.GetMousePos(), 1.5f, false, true);
          return;
        }
        selected.transform.SetPosition(Grid.CellToPosCBC(mouseCell, Grid.SceneLayer.Move));
      }
    }
    else if (!e.TryConsume(Action.DebugPlace) && !e.TryConsume(Action.DebugSelectMaterial))
    {
      if (e.TryConsume(Action.DebugNotification))
      {
        if (GenericGameSettings.instance.developerDebugEnable)
          Tutorial.Instance.DebugNotification();
      }
      else if (e.TryConsume(Action.DebugNotificationMessage))
      {
        if (GenericGameSettings.instance.developerDebugEnable)
          Tutorial.Instance.DebugNotificationMessage();
      }
      else if (e.TryConsume(Action.DebugSuperSpeed))
      {
        if ((UnityEngine.Object) SpeedControlScreen.Instance != (UnityEngine.Object) null)
          SpeedControlScreen.Instance.ToggleRidiculousSpeed();
      }
      else if (e.TryConsume(Action.DebugGameStep))
      {
        if ((UnityEngine.Object) SpeedControlScreen.Instance != (UnityEngine.Object) null)
          SpeedControlScreen.Instance.DebugStepFrame();
      }
      else if (e.TryConsume(Action.DebugSimStep))
        Game.Instance.ForceSimStep();
      else if (e.TryConsume(Action.DebugToggleMusic))
        AudioDebug.Get().ToggleMusic();
      else if (e.TryConsume(Action.DebugRiverTest))
        Scenario.Instance.SetupRiverTest();
      else if (e.TryConsume(Action.DebugTileTest))
        Scenario.Instance.SetupTileTest();
      else if (e.TryConsume(Action.DebugForceLightEverywhere))
        PropertyTextures.instance.ForceLightEverywhere = !PropertyTextures.instance.ForceLightEverywhere;
      else if (e.TryConsume(Action.DebugPathFinding))
      {
        DebugHandler.DebugPathFinding = !DebugHandler.DebugPathFinding;
        Debug.Log((object) ("DebugPathFinding=" + (object) DebugHandler.DebugPathFinding));
      }
      else if (!e.TryConsume(Action.DebugFocus))
      {
        if (e.TryConsume(Action.DebugReportBug))
        {
          if (GenericGameSettings.instance.developerDebugEnable)
          {
            int num = 0;
            string validSaveFilename;
            while (true)
            {
              validSaveFilename = SaveScreen.GetValidSaveFilename("bug_report_savefile_" + num.ToString());
              if (File.Exists(validSaveFilename))
                ++num;
              else
                break;
            }
            string save_file = "No save file (front end)";
            if ((UnityEngine.Object) SaveLoader.Instance != (UnityEngine.Object) null)
              save_file = SaveLoader.Instance.Save(validSaveFilename, false, false);
            KCrashReporter.ReportBug("Bug Report", save_file);
          }
          else
            Debug.Log((object) "Debug crash keys are not enabled.");
        }
        else if (e.TryConsume(Action.DebugTriggerException))
        {
          if (GenericGameSettings.instance.developerDebugEnable)
            KCrashReporter.ReportError("Debug crash with random stack", Guid.NewGuid().ToString() + "\n" + new StackTrace(1, true).ToString(), (string) null, ScreenPrefabs.Instance.ConfirmDialogScreen, string.Empty);
        }
        else if (e.TryConsume(Action.DebugTriggerError))
        {
          if (GenericGameSettings.instance.developerDebugEnable)
            Debug.LogError((object) "Oooops! Testing error!");
        }
        else if (e.TryConsume(Action.DebugDumpGCRoots))
          GarbageProfiler.DebugDumpRootItems();
        else if (e.TryConsume(Action.DebugDumpGarbageReferences))
          GarbageProfiler.DebugDumpGarbageStats();
        else if (e.TryConsume(Action.DebugDumpEventData))
        {
          if (GenericGameSettings.instance.developerDebugEnable)
            KObjectManager.Instance.DumpEventData();
        }
        else if (e.TryConsume(Action.DebugDumpSceneParitionerLeakData))
        {
          if (!GenericGameSettings.instance.developerDebugEnable)
            ;
        }
        else if (e.TryConsume(Action.DebugCrashSim))
        {
          if (GenericGameSettings.instance.developerDebugEnable)
            Sim.SIM_DebugCrash();
        }
        else if (e.TryConsume(Action.DebugNextCall))
          DebugHandler.DebugNextCall = true;
        else if (e.TryConsume(Action.DebugTogglePersonalPriorityComparison))
          Chore.ENABLE_PERSONAL_PRIORITIES = !Chore.ENABLE_PERSONAL_PRIORITIES;
      }
    }
    if (!e.Consumed || !((UnityEngine.Object) Game.Instance != (UnityEngine.Object) null))
      return;
    Game.Instance.debugWasUsed = true;
    KCrashReporter.debugWasUsed = true;
  }

  public static void SetSelectInEditor(bool select_in_editor)
  {
  }

  public static void ToggleScreenshotMode()
  {
    DebugHandler.SetHideUI(!DebugHandler.HideUI);
    if ((UnityEngine.Object) CameraController.Instance != (UnityEngine.Object) null)
      CameraController.Instance.EnableFreeCamera(DebugHandler.HideUI);
    if (!((UnityEngine.Object) KScreenManager.Instance != (UnityEngine.Object) null))
      return;
    KScreenManager.Instance.DisableInput(DebugHandler.HideUI);
  }

  public static void SetHideUI(bool hide)
  {
    DebugHandler.HideUI = hide;
    float num = !DebugHandler.HideUI ? 1f : 0.0f;
    GameScreenManager.Instance.ssHoverTextCanvas.GetComponent<CanvasGroup>().alpha = num;
    GameScreenManager.Instance.ssCameraCanvas.GetComponent<CanvasGroup>().alpha = num;
    GameScreenManager.Instance.ssOverlayCanvas.GetComponent<CanvasGroup>().alpha = num;
    GameScreenManager.Instance.worldSpaceCanvas.GetComponent<CanvasGroup>().alpha = num;
    GameScreenManager.Instance.screenshotModeCanvas.GetComponent<CanvasGroup>().alpha = 1f - num;
  }

  public enum PaintMode
  {
    None,
    Element,
    Hot,
    Cold,
  }
}
