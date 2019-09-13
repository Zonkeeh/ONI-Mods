// Decompiled with JetBrains decompiler
// Type: NewGameFlowScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public abstract class NewGameFlowScreen : KModalScreen
{
  public event System.Action OnNavigateForward;

  public event System.Action OnNavigateBackward;

  protected void NavigateBackward()
  {
    this.OnNavigateBackward();
  }

  protected void NavigateForward()
  {
    this.OnNavigateForward();
  }
}
