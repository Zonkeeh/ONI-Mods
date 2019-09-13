// Decompiled with JetBrains decompiler
// Type: NewGameFlow
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;

public class NewGameFlow : KMonoBehaviour
{
  private int currentScreenIndex = -1;
  public List<NewGameFlowScreen> newGameFlowScreens;
  private NewGameFlowScreen currentScreen;

  public void BeginFlow()
  {
    this.currentScreenIndex = -1;
    this.Next();
  }

  private void Next()
  {
    this.ClearCurrentScreen();
    ++this.currentScreenIndex;
    this.ActivateCurrentScreen();
  }

  private void Previous()
  {
    this.ClearCurrentScreen();
    --this.currentScreenIndex;
    this.ActivateCurrentScreen();
  }

  private void ClearCurrentScreen()
  {
    if (!((UnityEngine.Object) this.currentScreen != (UnityEngine.Object) null))
      return;
    this.currentScreen.Deactivate();
    this.currentScreen = (NewGameFlowScreen) null;
  }

  private void ActivateCurrentScreen()
  {
    if (this.currentScreenIndex < 0 || this.currentScreenIndex >= this.newGameFlowScreens.Count)
      return;
    NewGameFlowScreen newGameFlowScreen = Util.KInstantiateUI<NewGameFlowScreen>(this.newGameFlowScreens[this.currentScreenIndex].gameObject, this.transform.parent.gameObject, true);
    newGameFlowScreen.OnNavigateForward += new System.Action(this.Next);
    newGameFlowScreen.OnNavigateBackward += new System.Action(this.Previous);
    if (!newGameFlowScreen.IsActive() && !newGameFlowScreen.activateOnSpawn)
      newGameFlowScreen.Activate();
    this.currentScreen = newGameFlowScreen;
  }
}
