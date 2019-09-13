// Decompiled with JetBrains decompiler
// Type: SaveActive
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

public class SaveActive : KScreen
{
  [MyCmpGet]
  private KBatchedAnimController controller;
  private Game.CansaveCB readyForSaveCallback;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    Game.Instance.SetAutoSaveCallbacks(new Game.SavingPreCB(this.ActiveateSaveIndicator), new Game.SavingActiveCB(this.SetActiveSaveIndicator), new Game.SavingPostCB(this.DeactivateSaveIndicator));
  }

  private void DoCallBack(HashedString name)
  {
    this.controller.onAnimComplete -= new KAnimControllerBase.KAnimEvent(this.DoCallBack);
    this.readyForSaveCallback();
    this.readyForSaveCallback = (Game.CansaveCB) null;
  }

  private void ActiveateSaveIndicator(Game.CansaveCB cb)
  {
    this.readyForSaveCallback = cb;
    this.controller.onAnimComplete += new KAnimControllerBase.KAnimEvent(this.DoCallBack);
    this.controller.Play((HashedString) "working_pre", KAnim.PlayMode.Once, 1f, 0.0f);
  }

  private void SetActiveSaveIndicator()
  {
    this.controller.Play((HashedString) "working_loop", KAnim.PlayMode.Once, 1f, 0.0f);
  }

  private void DeactivateSaveIndicator()
  {
    this.controller.Play((HashedString) "working_pst", KAnim.PlayMode.Once, 1f, 0.0f);
  }

  public override void OnKeyDown(KButtonEvent e)
  {
  }
}
