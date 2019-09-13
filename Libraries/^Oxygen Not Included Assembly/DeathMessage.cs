// Decompiled with JetBrains decompiler
// Type: DeathMessage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using UnityEngine;

public class DeathMessage : TargetMessage
{
  [Serialize]
  private ResourceRef<Death> death = new ResourceRef<Death>();

  public DeathMessage()
  {
  }

  public DeathMessage(GameObject go, Death death)
    : base(go.GetComponent<KPrefabID>())
  {
    this.death.Set(death);
  }

  public override string GetSound()
  {
    return string.Empty;
  }

  public override bool PlayNotificationSound()
  {
    return false;
  }

  public override string GetTitle()
  {
    return (string) MISC.NOTIFICATIONS.DUPLICANTDIED.NAME;
  }

  public override string GetTooltip()
  {
    return this.GetMessageBody();
  }

  public override string GetMessageBody()
  {
    return this.death.Get().description.Replace("{Target}", this.GetTarget().GetName());
  }
}
