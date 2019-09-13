// Decompiled with JetBrains decompiler
// Type: MonumentSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class MonumentSideScreen : SideScreenContent
{
  private List<GameObject> buttons = new List<GameObject>();
  private MonumentPart target;
  public KButton debugVictoryButton;
  public KButton flipButton;
  public GameObject stateButtonPrefab;
  [SerializeField]
  private RectTransform buttonContainer;

  public override bool IsValidForTarget(GameObject target)
  {
    return (UnityEngine.Object) target.GetComponent<MonumentPart>() != (UnityEngine.Object) null;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.debugVictoryButton.onClick += (System.Action) (() =>
    {
      SaveGame.Instance.GetComponent<ColonyAchievementTracker>().DebugTriggerAchievement(Db.Get().ColonyAchievements.Thriving.Id);
      SaveGame.Instance.GetComponent<ColonyAchievementTracker>().DebugTriggerAchievement(Db.Get().ColonyAchievements.Clothe8Dupes.Id);
      SaveGame.Instance.GetComponent<ColonyAchievementTracker>().DebugTriggerAchievement(Db.Get().ColonyAchievements.Build4NatureReserves.Id);
      SaveGame.Instance.GetComponent<ColonyAchievementTracker>().DebugTriggerAchievement(Db.Get().ColonyAchievements.ReachedSpace.Id);
      GameScheduler.Instance.Schedule("ForceCheckAchievements", 0.1f, (System.Action<object>) (data => Game.Instance.Trigger(395452326, (object) null)), (object) null, (SchedulerGroup) null);
    });
    this.debugVictoryButton.gameObject.SetActive(DebugHandler.InstantBuildMode && this.target.part == MonumentPart.Part.Top);
    int num;
    this.flipButton.onClick += (System.Action) (() => num = (int) this.target.GetComponent<Rotatable>().Rotate());
  }

  public override void SetTarget(GameObject target)
  {
    base.SetTarget(target);
    this.target = target.GetComponent<MonumentPart>();
    this.debugVictoryButton.gameObject.SetActive(DebugHandler.InstantBuildMode && this.target.part == MonumentPart.Part.Top);
    this.GenerateStateButtons();
  }

  public void GenerateStateButtons()
  {
    for (int index = this.buttons.Count - 1; index >= 0; --index)
      Util.KDestroyGameObject(this.buttons[index]);
    this.buttons.Clear();
    foreach (Tuple<string, string> selectableStatesAndSymbol in this.target.selectableStatesAndSymbols)
    {
      GameObject gameObject = Util.KInstantiateUI(this.stateButtonPrefab, this.buttonContainer.gameObject, true);
      string targetState = selectableStatesAndSymbol.first;
      string second = selectableStatesAndSymbol.second;
      gameObject.GetComponent<KButton>().onClick += (System.Action) (() => this.target.SetState(targetState));
      this.buttons.Add(gameObject);
      KAnimFile animFile = this.target.GetComponent<KBatchedAnimController>().AnimFiles[0];
      gameObject.GetComponent<KButton>().fgImage.sprite = Def.GetUISpriteFromMultiObjectAnim(animFile, targetState, false, second);
    }
  }
}
