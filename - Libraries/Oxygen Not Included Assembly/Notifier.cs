// Decompiled with JetBrains decompiler
// Type: Notifier
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

[SkipSaveFileSerialization]
public class Notifier : KMonoBehaviour
{
  public bool AutoClickFocus = true;
  [MyCmpGet]
  private KSelectable Selectable;
  public System.Action<Notification> OnAdd;
  public System.Action<Notification> OnRemove;
  public bool DisableNotifications;
  private Dictionary<HashedString, Notification> NotificationGroups;

  protected override void OnPrefabInit()
  {
    Components.Notifiers.Add(this);
  }

  protected override void OnCleanUp()
  {
    this.ClearNotifications();
    Components.Notifiers.Remove(this);
  }

  public void Add(Notification notification, string suffix = "")
  {
    if ((UnityEngine.Object) KScreenManager.Instance == (UnityEngine.Object) null || this.DisableNotifications)
      return;
    if ((UnityEngine.Object) notification.Notifier == (UnityEngine.Object) null)
    {
      notification.NotifierName = !((UnityEngine.Object) this.Selectable != (UnityEngine.Object) null) ? "• " + this.name + suffix : "• " + this.Selectable.GetName() + suffix;
      notification.Notifier = this;
      if (this.AutoClickFocus && (UnityEngine.Object) notification.clickFocus == (UnityEngine.Object) null)
        notification.clickFocus = this.transform;
      if (notification.Group.IsValid && notification.Group != (HashedString) string.Empty)
      {
        if (this.NotificationGroups == null)
          this.NotificationGroups = new Dictionary<HashedString, Notification>();
        Notification notification1;
        this.NotificationGroups.TryGetValue(notification.Group, out notification1);
        if (notification1 != null)
          this.Remove(notification1);
        this.NotificationGroups[notification.Group] = notification;
      }
      if (this.OnAdd != null)
        this.OnAdd(notification);
      notification.GameTime = Time.time;
    }
    else
      DebugUtil.Assert((UnityEngine.Object) notification.Notifier == (UnityEngine.Object) this);
    notification.Time = KTime.Instance.UnscaledGameTime;
  }

  public void Remove(Notification notification)
  {
    if (!((UnityEngine.Object) notification.Notifier != (UnityEngine.Object) null))
      return;
    notification.Notifier = (Notifier) null;
    if (this.NotificationGroups != null && notification.Group.IsValid && notification.Group != (HashedString) string.Empty)
      this.NotificationGroups.Remove(notification.Group);
    if (this.OnRemove == null)
      return;
    this.OnRemove(notification);
  }

  public void ClearNotifications()
  {
    if (this.NotificationGroups == null)
      return;
    foreach (HashedString index in new List<HashedString>((IEnumerable<HashedString>) this.NotificationGroups.Keys))
      this.Remove(this.NotificationGroups[index]);
  }
}
