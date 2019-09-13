// Decompiled with JetBrains decompiler
// Type: StatusItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusItem : Resource
{
  private static Dictionary<HashedString, StatusItem.StatusItemOverlays> overlayBitfieldMap = new Dictionary<HashedString, StatusItem.StatusItemOverlays>()
  {
    {
      OverlayModes.None.ID,
      StatusItem.StatusItemOverlays.None
    },
    {
      OverlayModes.Power.ID,
      StatusItem.StatusItemOverlays.PowerMap
    },
    {
      OverlayModes.Temperature.ID,
      StatusItem.StatusItemOverlays.Temperature
    },
    {
      OverlayModes.ThermalConductivity.ID,
      StatusItem.StatusItemOverlays.ThermalComfort
    },
    {
      OverlayModes.Light.ID,
      StatusItem.StatusItemOverlays.Light
    },
    {
      OverlayModes.LiquidConduits.ID,
      StatusItem.StatusItemOverlays.LiquidPlumbing
    },
    {
      OverlayModes.GasConduits.ID,
      StatusItem.StatusItemOverlays.GasPlumbing
    },
    {
      OverlayModes.SolidConveyor.ID,
      StatusItem.StatusItemOverlays.Conveyor
    },
    {
      OverlayModes.Decor.ID,
      StatusItem.StatusItemOverlays.Decor
    },
    {
      OverlayModes.Disease.ID,
      StatusItem.StatusItemOverlays.Pathogens
    },
    {
      OverlayModes.Crop.ID,
      StatusItem.StatusItemOverlays.Farming
    },
    {
      OverlayModes.Rooms.ID,
      StatusItem.StatusItemOverlays.Rooms
    },
    {
      OverlayModes.Suit.ID,
      StatusItem.StatusItemOverlays.Suits
    },
    {
      OverlayModes.Logic.ID,
      StatusItem.StatusItemOverlays.Logic
    },
    {
      OverlayModes.Oxygen.ID,
      StatusItem.StatusItemOverlays.None
    },
    {
      OverlayModes.TileMode.ID,
      StatusItem.StatusItemOverlays.None
    }
  };
  private bool showShowWorldIcon = true;
  public string tooltipText;
  public string notificationText;
  public string notificationTooltipText;
  public float notificationDelay;
  public string soundPath;
  public string iconName;
  public TintedSprite sprite;
  public bool shouldNotify;
  public StatusItem.IconType iconType;
  public NotificationType notificationType;
  public Notification.ClickCallback notificationClickCallback;
  public Func<string, object, string> resolveStringCallback;
  public Func<string, object, string> resolveTooltipCallback;
  public bool allowMultiples;
  public Func<HashedString, object, bool> conditionalOverlayCallback;
  public HashedString render_overlay;
  public int status_overlays;
  public System.Action<object> statusItemClickCallback;
  private string composedPrefix;
  public const int ALL_OVERLAYS = 129022;

  private StatusItem(string id, string composed_prefix)
    : base(id, (string) Strings.Get(composed_prefix + ".NAME"))
  {
    this.composedPrefix = composed_prefix;
    this.tooltipText = (string) Strings.Get(composed_prefix + ".TOOLTIP");
  }

  public StatusItem(
    string id,
    string prefix,
    string icon,
    StatusItem.IconType icon_type,
    NotificationType notification_type,
    bool allow_multiples,
    HashedString render_overlay,
    bool showWorldIcon = true,
    int status_overlays = 129022)
    : this(id, "STRINGS." + prefix + ".STATUSITEMS." + id.ToUpper())
  {
    switch (icon_type)
    {
      case StatusItem.IconType.Info:
        icon = "dash";
        break;
      case StatusItem.IconType.Exclamation:
        icon = "status_item_exclamation";
        break;
    }
    this.iconName = icon;
    this.notificationType = notification_type;
    this.sprite = Assets.GetTintedSprite(icon);
    this.iconType = icon_type;
    this.allowMultiples = allow_multiples;
    this.render_overlay = render_overlay;
    this.showShowWorldIcon = showWorldIcon;
    this.status_overlays = status_overlays;
    if (this.sprite != null)
      return;
    Debug.LogWarning((object) ("Status item '" + id + "' references a missing icon: " + icon));
  }

  public StatusItem(
    string id,
    string name,
    string tooltip,
    string icon,
    StatusItem.IconType icon_type,
    NotificationType notification_type,
    bool allow_multiples,
    HashedString render_overlay,
    int status_overlays = 129022)
    : base(id, name)
  {
    switch (icon_type)
    {
      case StatusItem.IconType.Info:
        icon = "dash";
        break;
      case StatusItem.IconType.Exclamation:
        icon = "status_item_exclamation";
        break;
    }
    this.iconName = icon;
    this.notificationType = notification_type;
    this.sprite = Assets.GetTintedSprite(icon);
    this.tooltipText = tooltip;
    this.iconType = icon_type;
    this.allowMultiples = allow_multiples;
    this.render_overlay = render_overlay;
    this.status_overlays = status_overlays;
    if (this.sprite != null)
      return;
    Debug.LogWarning((object) ("Status item '" + id + "' references a missing icon: " + icon));
  }

  public void AddNotification(
    string sound_path = null,
    string notification_text = null,
    string notification_tooltip = null,
    float notification_delay = 0.0f)
  {
    this.shouldNotify = true;
    this.notificationDelay = notification_delay;
    this.soundPath = sound_path != null ? sound_path : (this.notificationType == NotificationType.Bad ? "Warning" : "Notification");
    if (notification_text != null)
    {
      this.notificationText = notification_text;
    }
    else
    {
      DebugUtil.Assert(this.composedPrefix != null, "When adding a notification, either set the status prefix or specify strings!");
      this.notificationText = (string) Strings.Get(this.composedPrefix + ".NOTIFICATION_NAME");
    }
    if (notification_tooltip != null)
    {
      this.notificationTooltipText = notification_tooltip;
    }
    else
    {
      DebugUtil.Assert(this.composedPrefix != null, "When adding a notification, either set the status prefix or specify strings!");
      this.notificationTooltipText = (string) Strings.Get(this.composedPrefix + ".NOTIFICATION_TOOLTIP");
    }
  }

  public virtual string GetName(object data)
  {
    return this.ResolveString(this.Name, data);
  }

  public virtual string GetTooltip(object data)
  {
    return this.ResolveTooltip(this.tooltipText, data);
  }

  private string ResolveString(string str, object data)
  {
    if (this.resolveStringCallback != null && data != null)
      return this.resolveStringCallback(str, data);
    return str;
  }

  private string ResolveTooltip(string str, object data)
  {
    if (data != null)
    {
      if (this.resolveTooltipCallback != null)
        return this.resolveTooltipCallback(str, data);
      if (this.resolveStringCallback != null)
        return this.resolveStringCallback(str, data);
    }
    return str;
  }

  public bool ShouldShowIcon()
  {
    if (this.iconType == StatusItem.IconType.Custom)
      return this.showShowWorldIcon;
    return false;
  }

  public virtual void ShowToolTip(
    ToolTip tooltip_widget,
    object data,
    TextStyleSetting property_style)
  {
    tooltip_widget.ClearMultiStringTooltip();
    string tooltip = this.GetTooltip(data);
    tooltip_widget.AddMultiStringTooltip(tooltip, (ScriptableObject) property_style);
  }

  public void SetIcon(Image image, object data)
  {
    if (this.sprite == null)
      return;
    image.color = this.sprite.color;
    image.sprite = this.sprite.sprite;
  }

  public bool UseConditionalCallback(HashedString overlay, Transform transform)
  {
    return overlay != OverlayModes.None.ID && this.conditionalOverlayCallback != null && this.conditionalOverlayCallback(overlay, (object) transform);
  }

  public StatusItem SetResolveStringCallback(Func<string, object, string> cb)
  {
    this.resolveStringCallback = cb;
    return this;
  }

  public void OnClick(object data)
  {
    if (this.statusItemClickCallback == null)
      return;
    this.statusItemClickCallback(data);
  }

  public static StatusItem.StatusItemOverlays GetStatusItemOverlayBySimViewMode(
    HashedString mode)
  {
    StatusItem.StatusItemOverlays statusItemOverlays;
    if (!StatusItem.overlayBitfieldMap.TryGetValue(mode, out statusItemOverlays))
    {
      Debug.LogWarning((object) ("ViewMode " + (object) mode + " has no StatusItemOverlay value"));
      statusItemOverlays = StatusItem.StatusItemOverlays.None;
    }
    return statusItemOverlays;
  }

  public enum IconType
  {
    Info,
    Exclamation,
    Custom,
  }

  [System.Flags]
  public enum StatusItemOverlays
  {
    None = 2,
    PowerMap = 4,
    Temperature = 8,
    ThermalComfort = 16, // 0x00000010
    Light = 32, // 0x00000020
    LiquidPlumbing = 64, // 0x00000040
    GasPlumbing = 128, // 0x00000080
    Decor = 256, // 0x00000100
    Pathogens = 512, // 0x00000200
    Farming = 1024, // 0x00000400
    Rooms = 4096, // 0x00001000
    Suits = 8192, // 0x00002000
    Logic = 16384, // 0x00004000
    Conveyor = 32768, // 0x00008000
    Radiation = 65536, // 0x00010000
  }
}
