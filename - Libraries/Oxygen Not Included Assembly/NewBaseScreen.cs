// Decompiled with JetBrains decompiler
// Type: NewBaseScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMODUnity;
using Klei.AI;
using UnityEngine;

public class NewBaseScreen : KScreen
{
  public static NewBaseScreen Instance;
  [SerializeField]
  private CanvasGroup[] disabledUIElements;
  [EventRef]
  public string ScanSoundMigrated;
  [EventRef]
  public string BuildBaseSoundMigrated;
  private ITelepadDeliverable[] minionStartingStats;

  public override float GetSortKey()
  {
    return 1f;
  }

  protected override void OnPrefabInit()
  {
    NewBaseScreen.Instance = this;
    base.OnPrefabInit();
    TimeOfDay.Instance.SetScale(0.0f);
  }

  public static Vector2I SetInitialCamera()
  {
    Vector2I baseStartPos = SaveLoader.Instance.cachedGSD.baseStartPos;
    Vector3 posCcc = Grid.CellToPosCCC(Grid.OffsetCell(Grid.OffsetCell(0, baseStartPos.x, baseStartPos.y), 0, -2), Grid.SceneLayer.Background);
    CameraController.Instance.SetMaxOrthographicSize(40f);
    CameraController.Instance.SnapTo(posCcc);
    CameraController.Instance.SetTargetPos(posCcc, 20f, false);
    CameraController.Instance.SetOrthographicsSize(40f);
    CameraSaveData.valid = false;
    return baseStartPos;
  }

  protected override void OnActivate()
  {
    if (this.disabledUIElements != null)
    {
      foreach (CanvasGroup disabledUiElement in this.disabledUIElements)
      {
        if ((UnityEngine.Object) disabledUiElement != (UnityEngine.Object) null)
          disabledUiElement.interactable = false;
      }
    }
    NewBaseScreen.SetInitialCamera();
    if (SpeedControlScreen.Instance.IsPaused)
      SpeedControlScreen.Instance.Unpause(false);
    this.Final();
  }

  public void SetStartingMinionStats(ITelepadDeliverable[] stats)
  {
    this.minionStartingStats = stats;
  }

  protected override void OnDeactivate()
  {
    Game.Instance.Trigger(-122303817, (object) null);
    if (this.disabledUIElements == null)
      return;
    foreach (CanvasGroup disabledUiElement in this.disabledUIElements)
    {
      if ((UnityEngine.Object) disabledUiElement != (UnityEngine.Object) null)
        disabledUiElement.interactable = true;
    }
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    Action[] actionArray = new Action[4]
    {
      Action.SpeedUp,
      Action.SlowDown,
      Action.TogglePause,
      Action.CycleSpeed
    };
    if (e.Consumed)
      return;
    int index = 0;
    while (index < actionArray.Length && !e.TryConsume(actionArray[index]))
      ++index;
  }

  private void Final()
  {
    SpeedControlScreen.Instance.Unpause(false);
    Telepad objectOfType = UnityEngine.Object.FindObjectOfType<Telepad>();
    if ((bool) ((UnityEngine.Object) objectOfType))
      this.SpawnMinions(Grid.PosToCell(objectOfType.gameObject));
    Game.Instance.baseAlreadyCreated = true;
    Game.Instance.StartDelayedInitialSave();
    this.Deactivate();
  }

  private void SpawnMinions(int headquartersCell)
  {
    if (headquartersCell == -1)
    {
      Debug.LogWarning((object) "No headquarters in saved base template. Cannot place minions. Confirm there is a headquarters saved to the base template, or consider creating a new one.");
    }
    else
    {
      int x;
      int y;
      Grid.CellToXY(headquartersCell, out x, out y);
      if (Grid.WidthInCells < 64)
        return;
      int baseLeft = SaveGame.Instance.worldGen.BaseLeft;
      int baseRight = SaveGame.Instance.worldGen.BaseRight;
      Effect a_new_hope = Db.Get().effects.Get("AnewHope");
      for (int index = 0; index < this.minionStartingStats.Length; ++index)
      {
        int cell = Grid.XYToCell(x + index % (baseRight - baseLeft) + 1, y);
        GameObject gameObject1 = Util.KInstantiate(Assets.GetPrefab((Tag) MinionConfig.ID), (GameObject) null, (string) null);
        Immigration.Instance.ApplyDefaultPersonalPriorities(gameObject1);
        gameObject1.transform.SetLocalPosition(Grid.CellToPosCBC(cell, Grid.SceneLayer.Move));
        gameObject1.SetActive(true);
        ((MinionStartingStats) this.minionStartingStats[index]).Apply(gameObject1);
        GameScheduler.Instance.Schedule("ANewHope", (float) (3.0 + 0.5 * (double) index), (System.Action<object>) (m =>
        {
          GameObject gameObject = m as GameObject;
          if ((UnityEngine.Object) gameObject == (UnityEngine.Object) null)
            return;
          gameObject.GetComponent<Effects>().Add(a_new_hope, true);
        }), (object) gameObject1, (SchedulerGroup) null);
      }
    }
  }
}
