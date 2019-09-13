// Decompiled with JetBrains decompiler
// Type: MessageNotification
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class MessageNotification : Notification
{
  public Message message;

  public MessageNotification(Message m)
    : base(m.GetTitle(), NotificationType.Messages, HashedString.Invalid, (Func<List<Notification>, object, string>) null, (object) null, false, 0.0f, (Notification.ClickCallback) null, (object) null, (Transform) null)
  {
    MessageNotification messageNotification = this;
    this.message = m;
    if (!this.message.PlayNotificationSound())
      this.playSound = false;
    this.ToolTip = (Func<List<Notification>, object, string>) ((notifications, data) => messageNotification.OnToolTip(notifications, m.GetTooltip()));
    this.clickFocus = (Transform) null;
  }

  private string OnToolTip(List<Notification> notifications, string tooltipText)
  {
    return tooltipText;
  }
}
