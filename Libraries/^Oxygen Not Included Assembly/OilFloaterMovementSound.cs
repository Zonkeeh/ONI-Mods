// Decompiled with JetBrains decompiler
// Type: OilFloaterMovementSound
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

internal class OilFloaterMovementSound : KMonoBehaviour
{
  private static readonly EventSystem.IntraObjectHandler<OilFloaterMovementSound> OnObjectMovementStateChangedDelegate = new EventSystem.IntraObjectHandler<OilFloaterMovementSound>((System.Action<OilFloaterMovementSound, object>) ((component, data) => component.OnObjectMovementStateChanged(data)));
  public string sound;
  public bool isPlayingSound;
  public bool isMoving;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.sound = GlobalAssets.GetSound(this.sound, false);
    this.Subscribe<OilFloaterMovementSound>(1027377649, OilFloaterMovementSound.OnObjectMovementStateChangedDelegate);
    Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(this.transform, new System.Action(this.OnCellChanged), nameof (OilFloaterMovementSound));
  }

  private void OnObjectMovementStateChanged(object data)
  {
    this.isMoving = (GameHashes) data == GameHashes.ObjectMovementWakeUp;
    this.UpdateSound();
  }

  private void OnCellChanged()
  {
    this.UpdateSound();
  }

  private void UpdateSound()
  {
    bool flag = this.isMoving && this.GetComponent<Navigator>().CurrentNavType != NavType.Swim;
    if (flag == this.isPlayingSound)
      return;
    LoopingSounds component = this.GetComponent<LoopingSounds>();
    if (flag)
      component.StartSound(this.sound);
    else
      component.StopSound(this.sound);
    this.isPlayingSound = flag;
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(this.transform, new System.Action(this.OnCellChanged));
  }
}
