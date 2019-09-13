// Decompiled with JetBrains decompiler
// Type: DiscoveredSpaceMessage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using UnityEngine;

public class DiscoveredSpaceMessage : Message
{
  [Serialize]
  private Vector3 cameraFocusPos;
  private const string MUSIC_STINGER = "Stinger_Surface";

  public DiscoveredSpaceMessage()
  {
  }

  public DiscoveredSpaceMessage(Vector3 pos)
  {
    this.cameraFocusPos = pos;
    this.cameraFocusPos.z = -40f;
  }

  public override string GetSound()
  {
    return "Discover_Space";
  }

  public override string GetMessageBody()
  {
    return (string) MISC.NOTIFICATIONS.DISCOVERED_SPACE.TOOLTIP;
  }

  public override string GetTitle()
  {
    return (string) MISC.NOTIFICATIONS.DISCOVERED_SPACE.NAME;
  }

  public override string GetTooltip()
  {
    return (string) null;
  }

  public override bool IsValid()
  {
    return true;
  }

  public override void OnClick()
  {
    this.OnDiscoveredSpaceClicked();
  }

  private void OnDiscoveredSpaceClicked()
  {
    KFMOD.PlayOneShot(GlobalAssets.GetSound(this.GetSound(), false));
    MusicManager.instance.PlaySong("Stinger_Surface", false);
    CameraController.Instance.SetTargetPos(this.cameraFocusPos, 8f, true);
    SaveGame.Instance.GetComponent<SeasonManager>().ForceBeginMeteorSeasonWithShower();
  }
}
