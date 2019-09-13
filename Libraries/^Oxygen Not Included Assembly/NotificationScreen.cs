// Decompiled with JetBrains decompiler
// Type: NotificationScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using FMOD.Studio;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NotificationScreen : KScreen
{
  [SerializeField]
  private List<MessageDialog> dialogPrefabs = new List<MessageDialog>();
  [SerializeField]
  private Color badColor = Color.red;
  [SerializeField]
  private Color normalColor = Color.white;
  private List<Notification> pendingNotifications = new List<Notification>();
  private List<Notification> notifications = new List<Notification>();
  private Dictionary<NotificationType, string> notificationSounds = new Dictionary<NotificationType, string>();
  private Dictionary<string, float> timeOfLastNotification = new Dictionary<string, float>();
  private float soundDecayTime = 10f;
  private List<NotificationScreen.Entry> entries = new List<NotificationScreen.Entry>();
  private Dictionary<string, NotificationScreen.Entry> entriesByMessage = new Dictionary<string, NotificationScreen.Entry>();
  public float lifetime;
  public bool dirty;
  public GameObject LabelPrefab;
  public GameObject LabelsFolder;
  public GameObject MessagesPrefab;
  public GameObject MessagesFolder;
  private MessageDialogFrame messageDialog;
  private float initTime;
  private int notificationIncrement;
  [MyCmpAdd]
  private Notifier notifier;
  [SerializeField]
  private Color badColorBG;
  [SerializeField]
  private Color normalColorBG;
  [SerializeField]
  private Color warningColorBG;
  [SerializeField]
  private Color warningColor;
  [SerializeField]
  private Color messageColorBG;
  [SerializeField]
  private Color messageColor;
  public Sprite icon_normal;
  public Sprite icon_warning;
  public Sprite icon_bad;
  public Sprite icon_message;
  public TextStyleSetting TooltipTextStyle;

  public static NotificationScreen Instance { get; private set; }

  public static void DestroyInstance()
  {
    NotificationScreen.Instance = (NotificationScreen) null;
  }

  private void OnAddNotifier(Notifier notifier)
  {
    notifier.OnAdd += new System.Action<Notification>(this.OnAddNotification);
    notifier.OnRemove += new System.Action<Notification>(this.OnRemoveNotification);
  }

  private void OnRemoveNotifier(Notifier notifier)
  {
    notifier.OnAdd -= new System.Action<Notification>(this.OnAddNotification);
    notifier.OnRemove -= new System.Action<Notification>(this.OnRemoveNotification);
  }

  private void OnAddNotification(Notification notification)
  {
    this.pendingNotifications.Add(notification);
  }

  private void OnRemoveNotification(Notification notification)
  {
    this.dirty = true;
    this.pendingNotifications.Remove(notification);
    NotificationScreen.Entry entry = (NotificationScreen.Entry) null;
    this.entriesByMessage.TryGetValue(notification.titleText, out entry);
    if (entry == null)
      return;
    this.notifications.Remove(notification);
    entry.Remove(notification);
    if (entry.notifications.Count != 0)
      return;
    UnityEngine.Object.Destroy((UnityEngine.Object) entry.label);
    this.entriesByMessage[notification.titleText] = (NotificationScreen.Entry) null;
    this.entries.Remove(entry);
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    NotificationScreen.Instance = this;
    Components.Notifiers.OnAdd += new System.Action<Notifier>(this.OnAddNotifier);
    Components.Notifiers.OnRemove += new System.Action<Notifier>(this.OnRemoveNotifier);
    foreach (Notifier notifier in Components.Notifiers.Items)
      this.OnAddNotifier(notifier);
    this.MessagesPrefab.gameObject.SetActive(false);
    this.LabelPrefab.gameObject.SetActive(false);
    this.InitNotificationSounds();
  }

  private void OnNewMessage(object data)
  {
    this.notifier.Add((Notification) new MessageNotification((Message) data), string.Empty);
  }

  private void ShowMessage(MessageNotification mn)
  {
    mn.message.OnClick();
    if (mn.message.ShowDialog())
    {
      for (int index = 0; index < this.dialogPrefabs.Count; ++index)
      {
        if (this.dialogPrefabs[index].CanDisplay(mn.message))
        {
          if ((UnityEngine.Object) this.messageDialog != (UnityEngine.Object) null)
          {
            UnityEngine.Object.Destroy((UnityEngine.Object) this.messageDialog.gameObject);
            this.messageDialog = (MessageDialogFrame) null;
          }
          this.messageDialog = Util.KInstantiateUI<MessageDialogFrame>(ScreenPrefabs.Instance.MessageDialogFrame.gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject, false);
          this.messageDialog.SetMessage(Util.KInstantiateUI<MessageDialog>(this.dialogPrefabs[index].gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject, false), mn.message);
          this.messageDialog.Show(true);
          break;
        }
      }
    }
    Messenger.Instance.RemoveMessage(mn.message);
    mn.Clear();
  }

  public void OnClickNextMessage()
  {
    this.ShowMessage((MessageNotification) this.notifications.Find((Predicate<Notification>) (notification => notification.Type == NotificationType.Messages)));
  }

  protected override void OnCleanUp()
  {
    Components.Notifiers.OnAdd -= new System.Action<Notifier>(this.OnAddNotifier);
    Components.Notifiers.OnRemove -= new System.Action<Notifier>(this.OnRemoveNotifier);
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.initTime = KTime.Instance.UnscaledGameTime;
    foreach (Graphic componentsInChild in this.LabelPrefab.GetComponentsInChildren<LocText>())
      componentsInChild.color = this.normalColor;
    foreach (Graphic componentsInChild in this.MessagesPrefab.GetComponentsInChildren<LocText>())
      componentsInChild.color = this.normalColor;
    this.Subscribe(Messenger.Instance.gameObject, 1558809273, new System.Action<object>(this.OnNewMessage));
    foreach (Message message in Messenger.Instance.Messages)
    {
      Notification notification = (Notification) new MessageNotification(message);
      notification.playSound = false;
      this.notifier.Add(notification, string.Empty);
    }
  }

  protected override void OnActivate()
  {
    base.OnActivate();
    this.dirty = true;
  }

  private void AddNotification(Notification notification)
  {
    this.notifications.Add(notification);
    notification.Idx = this.notificationIncrement++;
    NotificationScreen.Entry entry = (NotificationScreen.Entry) null;
    this.entriesByMessage.TryGetValue(notification.titleText, out entry);
    if (entry == null)
    {
      GameObject label = notification.Type != NotificationType.Messages ? Util.KInstantiateUI(this.LabelPrefab, this.LabelsFolder, false) : Util.KInstantiateUI(this.MessagesPrefab, this.MessagesFolder, false);
      label.GetComponentInChildren<NotificationAnimator>().Init();
      label.gameObject.SetActive(true);
      KImage componentInChildren1 = label.GetComponentInChildren<KImage>(true);
      UnityEngine.UI.Button[] componentsInChildren = label.gameObject.GetComponentsInChildren<UnityEngine.UI.Button>();
      ColorBlock colors = componentsInChildren[0].colors;
      if (notification.Type == NotificationType.Bad || notification.Type == NotificationType.DuplicantThreatening)
        colors.normalColor = this.badColorBG;
      else if (notification.Type == NotificationType.Messages)
      {
        colors.normalColor = this.messageColorBG;
        Debug.Assert(notification.GetType() == typeof (MessageNotification), (object) string.Format("Notification: \"{0}\" is not of type MessageNotification", (object) notification.titleText));
        componentsInChildren[1].onClick.AddListener((UnityAction) (() =>
        {
          foreach (MessageNotification messageNotification in this.notifications.FindAll((Predicate<Notification>) (n => n.titleText == notification.titleText)))
          {
            Messenger.Instance.RemoveMessage(messageNotification.message);
            messageNotification.Clear();
          }
        }));
      }
      else
        colors.normalColor = notification.Type != NotificationType.Tutorial ? this.normalColorBG : this.warningColorBG;
      componentsInChildren[0].colors = colors;
      componentsInChildren[0].onClick.AddListener((UnityAction) (() => this.OnClick(entry)));
      if (notification.ToolTip != null)
        label.GetComponentInChildren<ToolTip>().OnToolTip = (Func<string>) (() =>
        {
          ToolTip componentInChildren = label.GetComponentInChildren<ToolTip>();
          componentInChildren.ClearMultiStringTooltip();
          componentInChildren.AddMultiStringTooltip(notification.ToolTip(entry.notifications, notification.tooltipData), (ScriptableObject) this.TooltipTextStyle);
          return string.Empty;
        });
      entry = new NotificationScreen.Entry(label);
      this.entriesByMessage[notification.titleText] = entry;
      this.entries.Add(entry);
      foreach (LocText componentsInChild in label.GetComponentsInChildren<LocText>())
      {
        switch (notification.Type)
        {
          case NotificationType.Bad:
            componentsInChild.color = this.badColor;
            componentInChildren1.sprite = this.icon_bad;
            break;
          case NotificationType.Tutorial:
            componentsInChild.color = this.warningColor;
            componentInChildren1.sprite = this.icon_warning;
            break;
          case NotificationType.Messages:
            componentsInChild.color = this.messageColor;
            componentInChildren1.sprite = this.icon_message;
            break;
          case NotificationType.DuplicantThreatening:
            componentsInChild.color = this.badColor;
            componentInChildren1.sprite = this.icon_bad;
            break;
          default:
            componentsInChild.color = this.normalColor;
            componentInChildren1.sprite = this.icon_normal;
            break;
        }
        componentInChildren1.color = componentsInChild.color;
        string str = string.Empty;
        if ((double) KTime.Instance.UnscaledGameTime - (double) this.initTime > 5.0 && notification.playSound)
          this.PlayDingSound(notification, 0);
        else
          str = "too early";
        if (AudioDebug.Get().debugNotificationSounds)
          Debug.Log((object) ("Notification(" + notification.titleText + "):" + str));
      }
    }
    entry.Add(notification);
    entry.UpdateMessage(notification, true);
    this.dirty = true;
    this.SortNotifications();
  }

  private void SortNotifications()
  {
    this.notifications.Sort((Comparison<Notification>) ((n1, n2) =>
    {
      if (n1.Type == n2.Type)
        return n1.Idx - n2.Idx;
      return n1.Type - n2.Type;
    }));
    foreach (Notification notification in this.notifications)
    {
      NotificationScreen.Entry entry = (NotificationScreen.Entry) null;
      this.entriesByMessage.TryGetValue(notification.titleText, out entry);
      entry?.label.GetComponent<RectTransform>().SetAsLastSibling();
    }
  }

  private void PlayDingSound(Notification notification, int count)
  {
    string index;
    if (!this.notificationSounds.TryGetValue(notification.Type, out index))
      index = "Notification";
    float num1;
    if (!this.timeOfLastNotification.TryGetValue(index, out num1))
      num1 = 0.0f;
    float num2 = (Time.time - num1) / this.soundDecayTime;
    this.timeOfLastNotification[index] = Time.time;
    string sound = count <= 1 ? GlobalAssets.GetSound(index, false) : GlobalAssets.GetSound(index + "_AddCount", true) ?? GlobalAssets.GetSound(index, false);
    if (!notification.playSound)
      return;
    EventInstance instance = KFMOD.BeginOneShot(sound, Vector3.zero);
    int num3 = (int) instance.setParameterValue("timeSinceLast", num2);
    KFMOD.EndOneShot(instance);
  }

  private void Update()
  {
    int index1 = 0;
    while (index1 < this.pendingNotifications.Count)
    {
      if (this.pendingNotifications[index1].IsReady())
      {
        this.AddNotification(this.pendingNotifications[index1]);
        this.pendingNotifications.RemoveAt(index1);
      }
      else
        ++index1;
    }
    int num1 = 0;
    int num2 = 0;
    for (int index2 = 0; index2 < this.notifications.Count; ++index2)
    {
      Notification notification = this.notifications[index2];
      if (notification.Type == NotificationType.Messages)
        ++num2;
      else
        ++num1;
      if (notification.expires && (double) KTime.Instance.UnscaledGameTime - (double) notification.Time > (double) this.lifetime)
      {
        this.dirty = true;
        if ((UnityEngine.Object) notification.Notifier == (UnityEngine.Object) null)
          this.OnRemoveNotification(notification);
        else
          notification.Clear();
      }
    }
  }

  private void OnClick(NotificationScreen.Entry entry)
  {
    Notification clickedNotification = entry.NextClickedNotification;
    this.PlaySound3D(GlobalAssets.GetSound("HUD_Click_Open", false));
    if (clickedNotification.customClickCallback != null)
    {
      clickedNotification.customClickCallback(clickedNotification.customClickData);
    }
    else
    {
      if ((UnityEngine.Object) clickedNotification.clickFocus != (UnityEngine.Object) null)
      {
        Vector3 position = clickedNotification.clickFocus.GetPosition();
        position.z = -40f;
        CameraController.Instance.SetTargetPos(position, 8f, true);
        if ((UnityEngine.Object) clickedNotification.clickFocus.GetComponent<KSelectable>() != (UnityEngine.Object) null)
          SelectTool.Instance.Select(clickedNotification.clickFocus.GetComponent<KSelectable>(), false);
      }
      else if ((UnityEngine.Object) clickedNotification.Notifier != (UnityEngine.Object) null)
        SelectTool.Instance.Select(clickedNotification.Notifier.GetComponent<KSelectable>(), false);
      if (clickedNotification.Type != NotificationType.Messages)
        return;
      this.ShowMessage((MessageNotification) clickedNotification);
    }
  }

  private void PositionLocatorIcon()
  {
  }

  private void InitNotificationSounds()
  {
    this.notificationSounds[NotificationType.Good] = "Notification";
    this.notificationSounds[NotificationType.BadMinor] = "Notification";
    this.notificationSounds[NotificationType.Bad] = "Warning";
    this.notificationSounds[NotificationType.Neutral] = "Notification";
    this.notificationSounds[NotificationType.Tutorial] = "Notification";
    this.notificationSounds[NotificationType.Messages] = "Message";
    this.notificationSounds[NotificationType.DuplicantThreatening] = "Warning_DupeThreatening";
  }

  public Color32 BadColorBG
  {
    get
    {
      return (Color32) this.badColorBG;
    }
  }

  private class Entry
  {
    public List<Notification> notifications = new List<Notification>();
    public string message;
    public int clickIdx;
    public GameObject label;

    public Entry(GameObject label)
    {
      this.label = label;
    }

    public void Add(Notification notification)
    {
      this.notifications.Add(notification);
      this.UpdateMessage(notification, true);
    }

    public void Remove(Notification notification)
    {
      this.notifications.Remove(notification);
      this.UpdateMessage(notification, false);
    }

    public void UpdateMessage(Notification notification, bool playSound = true)
    {
      if (Game.IsQuitting())
        return;
      this.message = notification.titleText;
      if (this.notifications.Count > 1)
      {
        if (playSound && (notification.Type == NotificationType.Bad || notification.Type == NotificationType.DuplicantThreatening))
          NotificationScreen.Instance.PlayDingSound(notification, this.notifications.Count);
        NotificationScreen.Entry entry = this;
        entry.message = entry.message + " (" + this.notifications.Count.ToString() + ")";
      }
      if (!((UnityEngine.Object) this.label.gameObject != (UnityEngine.Object) null))
        return;
      this.label.GetComponentInChildren<LocText>().text = this.message;
    }

    public Notification NextClickedNotification
    {
      get
      {
        return this.notifications[this.clickIdx++ % this.notifications.Count];
      }
    }
  }
}
