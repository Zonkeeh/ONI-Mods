// Decompiled with JetBrains decompiler
// Type: GameOverScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class GameOverScreen : KModalScreen
{
  public KButton DismissButton;
  public KButton QuitButton;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Init();
  }

  private void Init()
  {
    if ((bool) ((UnityEngine.Object) this.QuitButton))
      this.QuitButton.onClick += (System.Action) (() => this.Quit());
    if (!(bool) ((UnityEngine.Object) this.DismissButton))
      return;
    this.DismissButton.onClick += (System.Action) (() => this.Dismiss());
  }

  private void Quit()
  {
    PauseScreen.TriggerQuitGame();
  }

  private void Dismiss()
  {
    this.Show(false);
  }
}
