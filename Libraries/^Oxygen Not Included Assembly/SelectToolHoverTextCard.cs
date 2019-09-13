// Decompiled with JetBrains decompiler
// Type: SelectToolHoverTextCard
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SelectToolHoverTextCard : HoverTextConfiguration
{
  public static int maxNumberOfDisplayedSelectableWarnings = 10;
  public static List<GameObject> highlightedObjects = new List<GameObject>();
  private static readonly List<System.Type> hiddenChoreConsumerTypes = new List<System.Type>()
  {
    typeof (KSelectableHealthBar)
  };
  private Dictionary<HashedString, Func<bool>> overlayFilterMap;
  public int recentNumberOfDisplayedSelectables;
  public int currentSelectedSelectableIndex;
  [NonSerialized]
  public Sprite iconWarning;
  [NonSerialized]
  public Sprite iconDash;
  [NonSerialized]
  public Sprite iconHighlighted;
  [NonSerialized]
  public Sprite iconActiveAutomationPort;
  public HoverTextConfiguration.TextStylePair Styles_LogicActive;
  public HoverTextConfiguration.TextStylePair Styles_LogicStandby;
  public TextStyleSetting Styles_LogicSignalInactive;
  private int maskOverlay;
  private string cachedTemperatureString;
  private float cachedTemperature;
  private List<KSelectable> overlayValidHoverObjects;
  private Dictionary<HashedString, Func<KSelectable, bool>> modeFilters;

  public SelectToolHoverTextCard()
  {
    Dictionary<HashedString, Func<KSelectable, bool>> dictionary1 = new Dictionary<HashedString, Func<KSelectable, bool>>();
    Dictionary<HashedString, Func<KSelectable, bool>> dictionary2 = dictionary1;
    HashedString id1 = OverlayModes.Oxygen.ID;
    // ISSUE: reference to a compiler-generated field
    if (SelectToolHoverTextCard.\u003C\u003Ef__mg\u0024cache0 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SelectToolHoverTextCard.\u003C\u003Ef__mg\u0024cache0 = new Func<KSelectable, bool>(SelectToolHoverTextCard.ShouldShowOxygenOverlay);
    }
    // ISSUE: reference to a compiler-generated field
    Func<KSelectable, bool> fMgCache0 = SelectToolHoverTextCard.\u003C\u003Ef__mg\u0024cache0;
    dictionary2.Add(id1, fMgCache0);
    Dictionary<HashedString, Func<KSelectable, bool>> dictionary3 = dictionary1;
    HashedString id2 = OverlayModes.Light.ID;
    // ISSUE: reference to a compiler-generated field
    if (SelectToolHoverTextCard.\u003C\u003Ef__mg\u0024cache1 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SelectToolHoverTextCard.\u003C\u003Ef__mg\u0024cache1 = new Func<KSelectable, bool>(SelectToolHoverTextCard.ShouldShowLightOverlay);
    }
    // ISSUE: reference to a compiler-generated field
    Func<KSelectable, bool> fMgCache1 = SelectToolHoverTextCard.\u003C\u003Ef__mg\u0024cache1;
    dictionary3.Add(id2, fMgCache1);
    Dictionary<HashedString, Func<KSelectable, bool>> dictionary4 = dictionary1;
    HashedString id3 = OverlayModes.Radiation.ID;
    // ISSUE: reference to a compiler-generated field
    if (SelectToolHoverTextCard.\u003C\u003Ef__mg\u0024cache2 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SelectToolHoverTextCard.\u003C\u003Ef__mg\u0024cache2 = new Func<KSelectable, bool>(SelectToolHoverTextCard.ShouldShowRadiationOverlay);
    }
    // ISSUE: reference to a compiler-generated field
    Func<KSelectable, bool> fMgCache2 = SelectToolHoverTextCard.\u003C\u003Ef__mg\u0024cache2;
    dictionary4.Add(id3, fMgCache2);
    Dictionary<HashedString, Func<KSelectable, bool>> dictionary5 = dictionary1;
    HashedString id4 = OverlayModes.GasConduits.ID;
    // ISSUE: reference to a compiler-generated field
    if (SelectToolHoverTextCard.\u003C\u003Ef__mg\u0024cache3 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SelectToolHoverTextCard.\u003C\u003Ef__mg\u0024cache3 = new Func<KSelectable, bool>(SelectToolHoverTextCard.ShouldShowGasConduitOverlay);
    }
    // ISSUE: reference to a compiler-generated field
    Func<KSelectable, bool> fMgCache3 = SelectToolHoverTextCard.\u003C\u003Ef__mg\u0024cache3;
    dictionary5.Add(id4, fMgCache3);
    Dictionary<HashedString, Func<KSelectable, bool>> dictionary6 = dictionary1;
    HashedString id5 = OverlayModes.LiquidConduits.ID;
    // ISSUE: reference to a compiler-generated field
    if (SelectToolHoverTextCard.\u003C\u003Ef__mg\u0024cache4 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SelectToolHoverTextCard.\u003C\u003Ef__mg\u0024cache4 = new Func<KSelectable, bool>(SelectToolHoverTextCard.ShouldShowLiquidConduitOverlay);
    }
    // ISSUE: reference to a compiler-generated field
    Func<KSelectable, bool> fMgCache4 = SelectToolHoverTextCard.\u003C\u003Ef__mg\u0024cache4;
    dictionary6.Add(id5, fMgCache4);
    Dictionary<HashedString, Func<KSelectable, bool>> dictionary7 = dictionary1;
    HashedString id6 = OverlayModes.SolidConveyor.ID;
    // ISSUE: reference to a compiler-generated field
    if (SelectToolHoverTextCard.\u003C\u003Ef__mg\u0024cache5 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SelectToolHoverTextCard.\u003C\u003Ef__mg\u0024cache5 = new Func<KSelectable, bool>(SelectToolHoverTextCard.ShouldShowSolidConveyorOverlay);
    }
    // ISSUE: reference to a compiler-generated field
    Func<KSelectable, bool> fMgCache5 = SelectToolHoverTextCard.\u003C\u003Ef__mg\u0024cache5;
    dictionary7.Add(id6, fMgCache5);
    Dictionary<HashedString, Func<KSelectable, bool>> dictionary8 = dictionary1;
    HashedString id7 = OverlayModes.Power.ID;
    // ISSUE: reference to a compiler-generated field
    if (SelectToolHoverTextCard.\u003C\u003Ef__mg\u0024cache6 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SelectToolHoverTextCard.\u003C\u003Ef__mg\u0024cache6 = new Func<KSelectable, bool>(SelectToolHoverTextCard.ShouldShowPowerOverlay);
    }
    // ISSUE: reference to a compiler-generated field
    Func<KSelectable, bool> fMgCache6 = SelectToolHoverTextCard.\u003C\u003Ef__mg\u0024cache6;
    dictionary8.Add(id7, fMgCache6);
    Dictionary<HashedString, Func<KSelectable, bool>> dictionary9 = dictionary1;
    HashedString id8 = OverlayModes.Logic.ID;
    // ISSUE: reference to a compiler-generated field
    if (SelectToolHoverTextCard.\u003C\u003Ef__mg\u0024cache7 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SelectToolHoverTextCard.\u003C\u003Ef__mg\u0024cache7 = new Func<KSelectable, bool>(SelectToolHoverTextCard.ShouldShowLogicOverlay);
    }
    // ISSUE: reference to a compiler-generated field
    Func<KSelectable, bool> fMgCache7 = SelectToolHoverTextCard.\u003C\u003Ef__mg\u0024cache7;
    dictionary9.Add(id8, fMgCache7);
    Dictionary<HashedString, Func<KSelectable, bool>> dictionary10 = dictionary1;
    HashedString id9 = OverlayModes.TileMode.ID;
    // ISSUE: reference to a compiler-generated field
    if (SelectToolHoverTextCard.\u003C\u003Ef__mg\u0024cache8 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SelectToolHoverTextCard.\u003C\u003Ef__mg\u0024cache8 = new Func<KSelectable, bool>(SelectToolHoverTextCard.ShouldShowTileOverlay);
    }
    // ISSUE: reference to a compiler-generated field
    Func<KSelectable, bool> fMgCache8 = SelectToolHoverTextCard.\u003C\u003Ef__mg\u0024cache8;
    dictionary10.Add(id9, fMgCache8);
    Dictionary<HashedString, Func<KSelectable, bool>> dictionary11 = dictionary1;
    HashedString id10 = OverlayModes.Disease.ID;
    // ISSUE: reference to a compiler-generated field
    if (SelectToolHoverTextCard.\u003C\u003Ef__mg\u0024cache9 == null)
    {
      // ISSUE: reference to a compiler-generated field
      SelectToolHoverTextCard.\u003C\u003Ef__mg\u0024cache9 = new Func<KSelectable, bool>(SelectToolHoverTextCard.ShowOverlayIfHasComponent<PrimaryElement>);
    }
    // ISSUE: reference to a compiler-generated field
    Func<KSelectable, bool> fMgCache9 = SelectToolHoverTextCard.\u003C\u003Ef__mg\u0024cache9;
    dictionary11.Add(id10, fMgCache9);
    Dictionary<HashedString, Func<KSelectable, bool>> dictionary12 = dictionary1;
    HashedString id11 = OverlayModes.Decor.ID;
    // ISSUE: reference to a compiler-generated field
    if (SelectToolHoverTextCard.\u003C\u003Ef__mg\u0024cacheA == null)
    {
      // ISSUE: reference to a compiler-generated field
      SelectToolHoverTextCard.\u003C\u003Ef__mg\u0024cacheA = new Func<KSelectable, bool>(SelectToolHoverTextCard.ShowOverlayIfHasComponent<DecorProvider>);
    }
    // ISSUE: reference to a compiler-generated field
    Func<KSelectable, bool> fMgCacheA = SelectToolHoverTextCard.\u003C\u003Ef__mg\u0024cacheA;
    dictionary12.Add(id11, fMgCacheA);
    Dictionary<HashedString, Func<KSelectable, bool>> dictionary13 = dictionary1;
    HashedString id12 = OverlayModes.Crop.ID;
    // ISSUE: reference to a compiler-generated field
    if (SelectToolHoverTextCard.\u003C\u003Ef__mg\u0024cacheB == null)
    {
      // ISSUE: reference to a compiler-generated field
      SelectToolHoverTextCard.\u003C\u003Ef__mg\u0024cacheB = new Func<KSelectable, bool>(SelectToolHoverTextCard.ShouldShowCropOverlay);
    }
    // ISSUE: reference to a compiler-generated field
    Func<KSelectable, bool> fMgCacheB = SelectToolHoverTextCard.\u003C\u003Ef__mg\u0024cacheB;
    dictionary13.Add(id12, fMgCacheB);
    Dictionary<HashedString, Func<KSelectable, bool>> dictionary14 = dictionary1;
    HashedString id13 = OverlayModes.Temperature.ID;
    // ISSUE: reference to a compiler-generated field
    if (SelectToolHoverTextCard.\u003C\u003Ef__mg\u0024cacheC == null)
    {
      // ISSUE: reference to a compiler-generated field
      SelectToolHoverTextCard.\u003C\u003Ef__mg\u0024cacheC = new Func<KSelectable, bool>(SelectToolHoverTextCard.ShouldShowTemperatureOverlay);
    }
    // ISSUE: reference to a compiler-generated field
    Func<KSelectable, bool> fMgCacheC = SelectToolHoverTextCard.\u003C\u003Ef__mg\u0024cacheC;
    dictionary14.Add(id13, fMgCacheC);
    this.modeFilters = dictionary1;
    // ISSUE: explicit constructor call
    base.\u002Ector();
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.overlayFilterMap.Add(OverlayModes.Oxygen.ID, (Func<bool>) (() =>
    {
      int cell = Grid.PosToCell(CameraController.Instance.baseCamera.ScreenToWorldPoint(KInputManager.GetMousePos()));
      return Grid.Element[cell].IsGas;
    }));
    this.overlayFilterMap.Add(OverlayModes.GasConduits.ID, (Func<bool>) (() =>
    {
      int cell = Grid.PosToCell(CameraController.Instance.baseCamera.ScreenToWorldPoint(KInputManager.GetMousePos()));
      return Grid.Element[cell].IsGas;
    }));
    this.overlayFilterMap.Add(OverlayModes.LiquidConduits.ID, (Func<bool>) (() =>
    {
      int cell = Grid.PosToCell(CameraController.Instance.baseCamera.ScreenToWorldPoint(KInputManager.GetMousePos()));
      return Grid.Element[cell].IsLiquid;
    }));
    this.overlayFilterMap.Add(OverlayModes.Decor.ID, (Func<bool>) (() => false));
    this.overlayFilterMap.Add(OverlayModes.Rooms.ID, (Func<bool>) (() => false));
    this.overlayFilterMap.Add(OverlayModes.Logic.ID, (Func<bool>) (() => false));
    this.overlayFilterMap.Add(OverlayModes.TileMode.ID, (Func<bool>) (() =>
    {
      int cell = Grid.PosToCell(CameraController.Instance.baseCamera.ScreenToWorldPoint(KInputManager.GetMousePos()));
      Element element = Grid.Element[cell];
      foreach (Tag tileOverlayFilter in Game.Instance.tileOverlayFilters)
      {
        if (element.HasTag(tileOverlayFilter))
          return true;
      }
      return false;
    }));
  }

  public override void ConfigureHoverScreen()
  {
    base.ConfigureHoverScreen();
    HoverTextScreen instance = HoverTextScreen.Instance;
    this.iconWarning = instance.GetSprite("iconWarning");
    this.iconDash = instance.GetSprite("dash");
    this.iconHighlighted = instance.GetSprite("dash_arrow");
    this.iconActiveAutomationPort = instance.GetSprite("current_automation_state_arrow");
    this.maskOverlay = LayerMask.GetMask("MaskedOverlay", "MaskedOverlayBG");
  }

  private bool IsStatusItemWarning(StatusItemGroup.Entry item)
  {
    return item.item.notificationType == NotificationType.Bad || item.item.notificationType == NotificationType.BadMinor || item.item.notificationType == NotificationType.DuplicantThreatening;
  }

  public override void UpdateHoverElements(List<KSelectable> hoverObjects)
  {
    if ((UnityEngine.Object) this.iconWarning == (UnityEngine.Object) null)
      this.ConfigureHoverScreen();
    int cell = Grid.PosToCell(Camera.main.ScreenToWorldPoint(KInputManager.GetMousePos()));
    if ((UnityEngine.Object) OverlayScreen.Instance == (UnityEngine.Object) null || !Grid.IsValidCell(cell))
      return;
    HoverTextDrawer drawer = HoverTextScreen.Instance.BeginDrawing();
    this.overlayValidHoverObjects.Clear();
    foreach (KSelectable hoverObject in hoverObjects)
    {
      if (this.ShouldShowSelectableInCurrentOverlay(hoverObject))
        this.overlayValidHoverObjects.Add(hoverObject);
    }
    this.currentSelectedSelectableIndex = -1;
    if (SelectToolHoverTextCard.highlightedObjects.Count > 0)
      SelectToolHoverTextCard.highlightedObjects.Clear();
    HashedString mode = SimDebugView.Instance.GetMode();
    bool flag1 = mode == OverlayModes.Disease.ID;
    bool flag2 = true;
    if (Grid.DupePassable[cell])
      flag2 = false;
    bool flag3 = Grid.IsVisible(cell);
    if (!flag3)
      flag2 = false;
    foreach (KeyValuePair<HashedString, Func<bool>> overlayFilter in this.overlayFilterMap)
    {
      if (OverlayScreen.Instance.GetMode() == overlayFilter.Key)
      {
        if (!overlayFilter.Value())
        {
          flag2 = false;
          break;
        }
        break;
      }
    }
    string str1 = string.Empty;
    string empty1 = string.Empty;
    if (mode == OverlayModes.Temperature.ID && Game.Instance.temperatureOverlayMode == Game.TemperatureOverlayModes.HeatFlow)
    {
      if (!Grid.Solid[cell] && flag3)
      {
        float thermalComfort1 = GameUtil.GetThermalComfort(cell, 0.0f);
        float thermalComfort2 = GameUtil.GetThermalComfort(cell, -0.08368f);
        float num = 0.0f;
        if ((double) thermalComfort2 * (1.0 / 1000.0) > -0.278933346271515 - (double) num && (double) thermalComfort2 * (1.0 / 1000.0) < 0.278933346271515 + (double) num)
          str1 = (string) UI.OVERLAYS.HEATFLOW.NEUTRAL;
        else if ((double) thermalComfort2 <= (double) ExternalTemperatureMonitor.GetExternalColdThreshold((Attributes) null))
          str1 = (string) UI.OVERLAYS.HEATFLOW.COOLING;
        else if ((double) thermalComfort2 >= (double) ExternalTemperatureMonitor.GetExternalWarmThreshold((Attributes) null))
          str1 = (string) UI.OVERLAYS.HEATFLOW.HEATING;
        float dtu_s = 1f * thermalComfort1;
        string text = str1 + " (" + GameUtil.GetFormattedHeatEnergyRate(dtu_s, GameUtil.HeatEnergyFormatterUnit.Automatic) + ")";
        drawer.BeginShadowBar(false);
        drawer.DrawText((string) UI.OVERLAYS.HEATFLOW.HOVERTITLE, this.Styles_Title.Standard);
        drawer.NewLine(26);
        drawer.DrawText(text, this.Styles_BodyText.Standard);
        drawer.EndShadowBar();
      }
    }
    else if (mode == OverlayModes.Decor.ID)
    {
      List<DecorProvider> decorProviderList = new List<DecorProvider>();
      GameScenePartitioner.Instance.TriggerEvent(cell, GameScenePartitioner.Instance.decorProviderLayer, (object) decorProviderList);
      float decorAtCell = GameUtil.GetDecorAtCell(cell);
      drawer.BeginShadowBar(false);
      drawer.DrawText((string) UI.OVERLAYS.DECOR.HOVERTITLE, this.Styles_Title.Standard);
      drawer.NewLine(26);
      drawer.DrawText((string) UI.OVERLAYS.DECOR.TOTAL + GameUtil.GetFormattedDecor(decorAtCell, true), this.Styles_BodyText.Standard);
      if (!Grid.Solid[cell] && flag3)
      {
        List<EffectorEntry> effectorEntryList1 = new List<EffectorEntry>();
        List<EffectorEntry> effectorEntryList2 = new List<EffectorEntry>();
        foreach (DecorProvider decorProvider in decorProviderList)
        {
          float decorForCell = decorProvider.GetDecorForCell(cell);
          if ((double) decorForCell != 0.0)
          {
            string name = decorProvider.GetName();
            KMonoBehaviour component = decorProvider.GetComponent<KMonoBehaviour>();
            if ((UnityEngine.Object) component != (UnityEngine.Object) null && (UnityEngine.Object) component.gameObject != (UnityEngine.Object) null)
            {
              SelectToolHoverTextCard.highlightedObjects.Add(component.gameObject);
              if ((UnityEngine.Object) component.GetComponent<MonumentPart>() != (UnityEngine.Object) null && component.GetComponent<MonumentPart>().IsMonumentCompleted())
              {
                name = (string) MISC.MONUMENT_COMPLETE.NAME;
                foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(component.GetComponent<AttachableBuilding>()))
                  SelectToolHoverTextCard.highlightedObjects.Add(gameObject);
              }
            }
            bool flag4 = false;
            if ((double) decorForCell > 0.0)
            {
              for (int index = 0; index < effectorEntryList1.Count; ++index)
              {
                if (effectorEntryList1[index].name == name)
                {
                  EffectorEntry effectorEntry = effectorEntryList1[index];
                  ++effectorEntry.count;
                  effectorEntry.value += decorForCell;
                  effectorEntryList1[index] = effectorEntry;
                  flag4 = true;
                  break;
                }
              }
              if (!flag4)
                effectorEntryList1.Add(new EffectorEntry(name, decorForCell));
            }
            else
            {
              for (int index = 0; index < effectorEntryList2.Count; ++index)
              {
                if (effectorEntryList2[index].name == name)
                {
                  EffectorEntry effectorEntry = effectorEntryList2[index];
                  ++effectorEntry.count;
                  effectorEntry.value += decorForCell;
                  effectorEntryList2[index] = effectorEntry;
                  flag4 = true;
                  break;
                }
              }
              if (!flag4)
                effectorEntryList2.Add(new EffectorEntry(name, decorForCell));
            }
          }
        }
        int lightDecorBonus = DecorProvider.GetLightDecorBonus(cell);
        if (lightDecorBonus > 0)
          effectorEntryList1.Add(new EffectorEntry((string) UI.OVERLAYS.DECOR.LIGHTING, (float) lightDecorBonus));
        effectorEntryList1.Sort((Comparison<EffectorEntry>) ((x, y) => y.value.CompareTo(x.value)));
        if (effectorEntryList1.Count > 0)
        {
          drawer.NewLine(26);
          drawer.DrawText((string) UI.OVERLAYS.DECOR.HEADER_POSITIVE, this.Styles_BodyText.Standard);
        }
        foreach (EffectorEntry effectorEntry in effectorEntryList1)
        {
          drawer.NewLine(18);
          drawer.DrawIcon(this.iconDash, 18);
          drawer.DrawText(effectorEntry.ToString(), this.Styles_BodyText.Standard);
        }
        effectorEntryList2.Sort((Comparison<EffectorEntry>) ((x, y) => Mathf.Abs(y.value).CompareTo(Mathf.Abs(x.value))));
        if (effectorEntryList2.Count > 0)
        {
          drawer.NewLine(26);
          drawer.DrawText((string) UI.OVERLAYS.DECOR.HEADER_NEGATIVE, this.Styles_BodyText.Standard);
        }
        foreach (EffectorEntry effectorEntry in effectorEntryList2)
        {
          drawer.NewLine(18);
          drawer.DrawIcon(this.iconDash, 18);
          drawer.DrawText(effectorEntry.ToString(), this.Styles_BodyText.Standard);
        }
      }
      drawer.EndShadowBar();
    }
    else if (mode == OverlayModes.Rooms.ID)
    {
      CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(cell);
      if (cavityForCell != null)
      {
        Room room = cavityForCell.room;
        RoomType roomType = (RoomType) null;
        string text1;
        if (room != null)
        {
          roomType = room.roomType;
          text1 = roomType.Name;
        }
        else
          text1 = (string) UI.OVERLAYS.ROOMS.NOROOM.HEADER;
        drawer.BeginShadowBar(false);
        drawer.DrawText(text1, this.Styles_Title.Standard);
        string empty2 = string.Empty;
        if (room != null)
        {
          string empty3 = string.Empty;
          string text2 = RoomDetails.EFFECT.resolve_string_function(room);
          string empty4 = string.Empty;
          string text3 = RoomDetails.ASSIGNED_TO.resolve_string_function(room);
          string empty5 = string.Empty;
          string text4 = RoomConstraints.RoomCriteriaString(room);
          string empty6 = string.Empty;
          string text5 = RoomDetails.EFFECTS.resolve_string_function(room);
          if (text2 != string.Empty)
          {
            drawer.NewLine(26);
            drawer.DrawText(text2, this.Styles_BodyText.Standard);
          }
          if (text3 != string.Empty && roomType != Db.Get().RoomTypes.Neutral)
          {
            drawer.NewLine(26);
            drawer.DrawText(text3, this.Styles_BodyText.Standard);
          }
          drawer.NewLine(22);
          drawer.DrawText(RoomDetails.RoomDetailString(room), this.Styles_BodyText.Standard);
          if (text4 != string.Empty)
          {
            drawer.NewLine(26);
            drawer.DrawText(text4, this.Styles_BodyText.Standard);
          }
          if (text5 != string.Empty)
          {
            drawer.NewLine(26);
            drawer.DrawText(text5, this.Styles_BodyText.Standard);
          }
        }
        else
        {
          string text2 = (string) UI.OVERLAYS.ROOMS.NOROOM.DESC;
          int maxRoomSize = TuningData<RoomProber.Tuning>.Get().maxRoomSize;
          if (cavityForCell.numCells > maxRoomSize)
            text2 = text2 + "\n" + string.Format((string) UI.OVERLAYS.ROOMS.NOROOM.TOO_BIG, (object) cavityForCell.numCells, (object) maxRoomSize);
          drawer.NewLine(26);
          drawer.DrawText(text2, this.Styles_BodyText.Standard);
        }
        drawer.EndShadowBar();
      }
    }
    else if (mode == OverlayModes.Light.ID)
    {
      if (flag3)
      {
        string text = str1 + string.Format((string) UI.OVERLAYS.LIGHTING.DESC, (object) Grid.LightIntensity[cell]) + " (" + GameUtil.GetLightDescription(Grid.LightIntensity[cell]) + ")";
        drawer.BeginShadowBar(false);
        drawer.DrawText((string) UI.OVERLAYS.LIGHTING.HOVERTITLE, this.Styles_Title.Standard);
        drawer.NewLine(26);
        drawer.DrawText(text, this.Styles_BodyText.Standard);
        drawer.EndShadowBar();
      }
    }
    else if (mode == OverlayModes.Logic.ID)
    {
      foreach (KSelectable hoverObject in hoverObjects)
      {
        LogicPorts component1 = hoverObject.GetComponent<LogicPorts>();
        LogicPorts.Port port1;
        bool isInput;
        if ((UnityEngine.Object) component1 != (UnityEngine.Object) null && component1.TryGetPortAtCell(cell, out port1, out isInput))
        {
          bool flag4 = component1.IsPortConnected(port1.id);
          drawer.BeginShadowBar(false);
          int num;
          if (isInput)
          {
            string str2 = !port1.displayCustomName ? UI.LOGIC_PORTS.PORT_INPUT_DEFAULT_NAME.text : port1.description;
            num = component1.GetInputValue(port1.id);
            drawer.DrawText(UI.TOOLS.GENERIC.LOGIC_INPUT_HOVER_FMT.Replace("{Port}", str2.ToUpper()).Replace("{Name}", hoverObject.GetProperName().ToUpper()), this.Styles_Title.Standard);
          }
          else
          {
            string str2 = !port1.displayCustomName ? UI.LOGIC_PORTS.PORT_OUTPUT_DEFAULT_NAME.text : port1.description;
            num = component1.GetOutputValue(port1.id);
            drawer.DrawText(UI.TOOLS.GENERIC.LOGIC_OUTPUT_HOVER_FMT.Replace("{Port}", str2.ToUpper()).Replace("{Name}", hoverObject.GetProperName().ToUpper()), this.Styles_Title.Standard);
          }
          drawer.NewLine(26);
          TextStyleSetting style1 = !flag4 ? this.Styles_LogicActive.Standard : (num != 1 ? this.Styles_LogicSignalInactive : this.Styles_LogicActive.Selected);
          drawer.DrawIcon(num != 1 || !flag4 ? this.iconDash : this.iconActiveAutomationPort, style1.textColor, 18, 2);
          drawer.DrawText(port1.activeDescription, style1);
          drawer.NewLine(26);
          TextStyleSetting style2 = !flag4 ? this.Styles_LogicStandby.Standard : (num != 0 ? this.Styles_LogicSignalInactive : this.Styles_LogicStandby.Selected);
          drawer.DrawIcon(num != 0 || !flag4 ? this.iconDash : this.iconActiveAutomationPort, style2.textColor, 18, 2);
          drawer.DrawText(port1.inactiveDescription, style2);
          drawer.EndShadowBar();
        }
        LogicGate component2 = hoverObject.GetComponent<LogicGate>();
        LogicGateBase.PortId port2;
        if ((UnityEngine.Object) component2 != (UnityEngine.Object) null && component2.TryGetPortAtCell(cell, out port2))
        {
          int portValue = component2.GetPortValue(port2);
          bool portConnected = component2.GetPortConnected(port2);
          LogicGate.LogicGateDescriptions.Description portDescription = component2.GetPortDescription(port2);
          drawer.BeginShadowBar(false);
          if (port2 == LogicGateBase.PortId.Output)
            drawer.DrawText(UI.TOOLS.GENERIC.LOGIC_MULTI_OUTPUT_HOVER_FMT.Replace("{Port}", portDescription.name.ToUpper()).Replace("{Name}", hoverObject.GetProperName().ToUpper()), this.Styles_Title.Standard);
          else
            drawer.DrawText(UI.TOOLS.GENERIC.LOGIC_MULTI_INPUT_HOVER_FMT.Replace("{Port}", portDescription.name.ToUpper()).Replace("{Name}", hoverObject.GetProperName().ToUpper()), this.Styles_Title.Standard);
          drawer.NewLine(26);
          TextStyleSetting style1 = !portConnected ? this.Styles_LogicActive.Standard : (portValue != 1 ? this.Styles_LogicSignalInactive : this.Styles_LogicActive.Selected);
          drawer.DrawIcon(portValue != 1 || !portConnected ? this.iconDash : this.iconActiveAutomationPort, style1.textColor, 18, 2);
          drawer.DrawText(portDescription.active, style1);
          drawer.NewLine(26);
          TextStyleSetting style2 = !portConnected ? this.Styles_LogicStandby.Standard : (portValue != 0 ? this.Styles_LogicSignalInactive : this.Styles_LogicStandby.Selected);
          drawer.DrawIcon(portValue != 0 || !portConnected ? this.iconDash : this.iconActiveAutomationPort, style2.textColor, 18, 2);
          drawer.DrawText(portDescription.inactive, style2);
          drawer.EndShadowBar();
        }
      }
    }
    int num1 = 0;
    ChoreConsumer choreConsumer = (ChoreConsumer) null;
    if ((UnityEngine.Object) SelectTool.Instance.selected != (UnityEngine.Object) null)
      choreConsumer = SelectTool.Instance.selected.GetComponent<ChoreConsumer>();
    for (int index1 = 0; index1 < this.overlayValidHoverObjects.Count; ++index1)
    {
      if ((UnityEngine.Object) this.overlayValidHoverObjects[index1] != (UnityEngine.Object) null && (UnityEngine.Object) this.overlayValidHoverObjects[index1].GetComponent<CellSelectionObject>() == (UnityEngine.Object) null)
      {
        KSelectable validHoverObject = this.overlayValidHoverObjects[index1];
        if ((!((UnityEngine.Object) OverlayScreen.Instance != (UnityEngine.Object) null) || !(OverlayScreen.Instance.mode != OverlayModes.None.ID) || (validHoverObject.gameObject.layer & this.maskOverlay) == 0) && flag3)
        {
          PrimaryElement component1 = validHoverObject.GetComponent<PrimaryElement>();
          bool selected = (UnityEngine.Object) SelectTool.Instance.selected == (UnityEngine.Object) this.overlayValidHoverObjects[index1];
          if (selected)
            this.currentSelectedSelectableIndex = index1;
          ++num1;
          drawer.BeginShadowBar(selected);
          string str2 = GameUtil.GetUnitFormattedName(this.overlayValidHoverObjects[index1].gameObject, true);
          if ((UnityEngine.Object) component1 != (UnityEngine.Object) null && (UnityEngine.Object) validHoverObject.GetComponent<Building>() != (UnityEngine.Object) null)
            str2 = StringFormatter.Replace(StringFormatter.Replace((string) UI.TOOLS.GENERIC.BUILDING_HOVER_NAME_FMT, "{Name}", str2), "{Element}", component1.Element.nameUpperCase);
          drawer.DrawText(str2, this.Styles_Title.Standard);
          bool flag4 = false;
          string text = (string) UI.OVERLAYS.DISEASE.NO_DISEASE;
          if (flag1)
          {
            if ((UnityEngine.Object) component1 != (UnityEngine.Object) null && component1.DiseaseIdx != byte.MaxValue)
              text = GameUtil.GetFormattedDisease(component1.DiseaseIdx, component1.DiseaseCount, true);
            flag4 = true;
            Storage component2 = validHoverObject.GetComponent<Storage>();
            if ((UnityEngine.Object) component2 != (UnityEngine.Object) null && component2.showInUI)
            {
              List<GameObject> items = component2.items;
              for (int index2 = 0; index2 < items.Count; ++index2)
              {
                GameObject gameObject = items[index2];
                if ((UnityEngine.Object) gameObject != (UnityEngine.Object) null)
                {
                  PrimaryElement component3 = gameObject.GetComponent<PrimaryElement>();
                  if (component3.DiseaseIdx != byte.MaxValue)
                    text += string.Format((string) UI.OVERLAYS.DISEASE.CONTAINER_FORMAT, (object) gameObject.GetComponent<KSelectable>().GetProperName(), (object) GameUtil.GetFormattedDisease(component3.DiseaseIdx, component3.DiseaseCount, true));
                }
              }
            }
          }
          if (flag4)
          {
            drawer.NewLine(26);
            drawer.DrawIcon(this.iconDash, 18);
            drawer.DrawText(text, this.Styles_Values.Property.Standard);
          }
          int num2 = 0;
          foreach (StatusItemGroup.Entry entry in this.overlayValidHoverObjects[index1].GetStatusItemGroup())
          {
            if (this.ShowStatusItemInCurrentOverlay(entry.item))
            {
              if (num2 < SelectToolHoverTextCard.maxNumberOfDisplayedSelectableWarnings)
              {
                if (entry.category != null && entry.category.Id == "Main" && num2 < SelectToolHoverTextCard.maxNumberOfDisplayedSelectableWarnings)
                {
                  TextStyleSetting style = !this.IsStatusItemWarning(entry) ? this.Styles_BodyText.Standard : this.HoverTextStyleSettings[1];
                  Sprite icon = entry.item.sprite == null ? this.iconWarning : entry.item.sprite.sprite;
                  Color color = !this.IsStatusItemWarning(entry) ? this.Styles_BodyText.Standard.textColor : this.HoverTextStyleSettings[1].textColor;
                  drawer.NewLine(26);
                  drawer.DrawIcon(icon, color, 18, 2);
                  drawer.DrawText(entry.GetName(), style);
                  ++num2;
                }
              }
              else
                break;
            }
          }
          foreach (StatusItemGroup.Entry entry in this.overlayValidHoverObjects[index1].GetStatusItemGroup())
          {
            if (this.ShowStatusItemInCurrentOverlay(entry.item))
            {
              if (num2 < SelectToolHoverTextCard.maxNumberOfDisplayedSelectableWarnings)
              {
                if ((entry.category == null || entry.category.Id != "Main") && num2 < SelectToolHoverTextCard.maxNumberOfDisplayedSelectableWarnings)
                {
                  TextStyleSetting style = !this.IsStatusItemWarning(entry) ? this.Styles_BodyText.Standard : this.HoverTextStyleSettings[1];
                  Sprite icon = entry.item.sprite == null ? this.iconWarning : entry.item.sprite.sprite;
                  Color color = !this.IsStatusItemWarning(entry) ? this.Styles_BodyText.Standard.textColor : this.HoverTextStyleSettings[1].textColor;
                  drawer.NewLine(26);
                  drawer.DrawIcon(icon, color, 18, 2);
                  drawer.DrawText(entry.GetName(), style);
                  ++num2;
                }
              }
              else
                break;
            }
          }
          float temp = 0.0f;
          bool flag5 = true;
          bool flag6 = OverlayModes.Temperature.ID == SimDebugView.Instance.GetMode() && Game.Instance.temperatureOverlayMode != Game.TemperatureOverlayModes.HeatFlow;
          if ((bool) ((UnityEngine.Object) validHoverObject.GetComponent<Constructable>()))
            flag5 = false;
          else if (flag6 && (bool) ((UnityEngine.Object) component1))
            temp = component1.Temperature;
          else if ((bool) ((UnityEngine.Object) validHoverObject.GetComponent<Building>()) && (bool) ((UnityEngine.Object) component1))
            temp = component1.Temperature;
          else if ((UnityEngine.Object) validHoverObject.GetComponent<CellSelectionObject>() != (UnityEngine.Object) null)
            temp = validHoverObject.GetComponent<CellSelectionObject>().temperature;
          else
            flag5 = false;
          if (mode != OverlayModes.None.ID && mode != OverlayModes.Temperature.ID)
            flag5 = false;
          if (flag5)
          {
            drawer.NewLine(26);
            drawer.DrawIcon(this.iconDash, 18);
            drawer.DrawText(GameUtil.GetFormattedTemperature(temp, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false), this.Styles_BodyText.Standard);
          }
          BuildingComplete component4 = validHoverObject.GetComponent<BuildingComplete>();
          if ((UnityEngine.Object) component4 != (UnityEngine.Object) null && component4.Def.IsFoundation)
            flag2 = false;
          if (mode == OverlayModes.Light.ID && (UnityEngine.Object) choreConsumer != (UnityEngine.Object) null)
          {
            bool flag7 = false;
            foreach (System.Type choreConsumerType in SelectToolHoverTextCard.hiddenChoreConsumerTypes)
            {
              if ((UnityEngine.Object) choreConsumer.gameObject.GetComponent(choreConsumerType) != (UnityEngine.Object) null)
              {
                flag7 = true;
                break;
              }
            }
            if (!flag7)
              choreConsumer.ShowHoverTextOnHoveredItem(validHoverObject, drawer, this);
          }
          drawer.EndShadowBar();
        }
      }
    }
    if (flag2)
    {
      CellSelectionObject cellSelectionObject = (CellSelectionObject) null;
      if ((UnityEngine.Object) SelectTool.Instance.selected != (UnityEngine.Object) null)
        cellSelectionObject = SelectTool.Instance.selected.GetComponent<CellSelectionObject>();
      bool selected = (UnityEngine.Object) cellSelectionObject != (UnityEngine.Object) null && cellSelectionObject.mouseCell == cellSelectionObject.alternateSelectionObject.mouseCell;
      if (selected)
        this.currentSelectedSelectableIndex = this.recentNumberOfDisplayedSelectables - 1;
      Element element1 = Grid.Element[cell];
      drawer.BeginShadowBar(selected);
      drawer.DrawText(element1.nameUpperCase, this.Styles_Title.Standard);
      if (Grid.DiseaseCount[cell] > 0 || flag1)
      {
        drawer.NewLine(26);
        drawer.DrawIcon(this.iconDash, 18);
        drawer.DrawText(GameUtil.GetFormattedDisease(Grid.DiseaseIdx[cell], Grid.DiseaseCount[cell], true), this.Styles_Values.Property.Standard);
      }
      if (!element1.IsVacuum)
      {
        drawer.NewLine(26);
        drawer.DrawIcon(this.iconDash, 18);
        drawer.DrawText(ElementLoader.elements[(int) Grid.ElementIdx[cell]].GetMaterialCategoryTag().ProperName(), this.Styles_BodyText.Standard);
      }
      string[] strArray = WorldInspector.MassStringsReadOnly(cell);
      drawer.NewLine(26);
      drawer.DrawIcon(this.iconDash, 18);
      for (int index = 0; index < strArray.Length; ++index)
      {
        if (index >= 3 || !element1.IsVacuum)
          drawer.DrawText(strArray[index], this.Styles_BodyText.Standard);
      }
      if (!element1.IsVacuum)
      {
        drawer.NewLine(26);
        drawer.DrawIcon(this.iconDash, 18);
        Element element2 = Grid.Element[cell];
        string str2 = this.cachedTemperatureString;
        float num2 = Grid.Temperature[cell];
        if ((double) num2 != (double) this.cachedTemperature)
        {
          this.cachedTemperature = num2;
          str2 = GameUtil.GetFormattedTemperature(Grid.Temperature[cell], GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false);
          this.cachedTemperatureString = str2;
        }
        string text = (double) element2.specificHeatCapacity != 0.0 ? str2 : "N/A";
        drawer.DrawText(text, this.Styles_BodyText.Standard);
      }
      if (CellSelectionObject.IsExposedToSpace(cell))
      {
        drawer.NewLine(26);
        drawer.DrawIcon(this.iconDash, 18);
        drawer.DrawText((string) MISC.STATUSITEMS.SPACE.NAME, this.Styles_BodyText.Standard);
      }
      if (Game.Instance.GetComponent<EntombedItemVisualizer>().IsEntombedItem(cell))
      {
        drawer.NewLine(26);
        drawer.DrawIcon(this.iconDash, 18);
        drawer.DrawText((string) MISC.STATUSITEMS.BURIEDITEM.NAME, this.Styles_BodyText.Standard);
      }
      if (element1.id == SimHashes.OxyRock)
      {
        float mass = Grid.AccumulatedFlow[cell] / 3f;
        string text1 = (string) BUILDING.STATUSITEMS.EMITTINGOXYGENAVG.NAME.Replace("{FlowRate}", GameUtil.GetFormattedMass(mass, GameUtil.TimeSlice.PerSecond, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
        drawer.NewLine(26);
        drawer.DrawIcon(this.iconDash, 18);
        drawer.DrawText(text1, this.Styles_BodyText.Standard);
        if ((double) mass <= 0.0)
        {
          bool all_not_gaseous;
          bool all_over_pressure;
          GameUtil.IsEmissionBlocked(cell, out all_not_gaseous, out all_over_pressure);
          string text2 = (string) null;
          if (all_not_gaseous)
            text2 = (string) MISC.STATUSITEMS.OXYROCK.NEIGHBORSBLOCKED.NAME;
          else if (all_over_pressure)
            text2 = (string) MISC.STATUSITEMS.OXYROCK.OVERPRESSURE.NAME;
          if (text2 != null)
          {
            drawer.NewLine(26);
            drawer.DrawIcon(this.iconDash, 18);
            drawer.DrawText(text2, this.Styles_BodyText.Standard);
          }
        }
      }
      drawer.EndShadowBar();
    }
    else if (!flag3)
    {
      drawer.BeginShadowBar(false);
      drawer.DrawIcon(this.iconWarning, 18);
      drawer.DrawText((string) UI.TOOLS.GENERIC.UNKNOWN, this.Styles_BodyText.Standard);
      drawer.EndShadowBar();
    }
    this.recentNumberOfDisplayedSelectables = num1 + 1;
    drawer.EndDrawing();
  }

  private bool ShowStatusItemInCurrentOverlay(StatusItem status)
  {
    if ((UnityEngine.Object) OverlayScreen.Instance == (UnityEngine.Object) null)
      return false;
    return ((StatusItem.StatusItemOverlays) status.status_overlays & StatusItem.GetStatusItemOverlayBySimViewMode(OverlayScreen.Instance.GetMode())) == StatusItem.GetStatusItemOverlayBySimViewMode(OverlayScreen.Instance.GetMode());
  }

  private bool ShouldShowSelectableInCurrentOverlay(KSelectable selectable)
  {
    bool flag = true;
    if ((UnityEngine.Object) OverlayScreen.Instance == (UnityEngine.Object) null)
      return flag;
    if ((UnityEngine.Object) selectable == (UnityEngine.Object) null)
      return false;
    Func<KSelectable, bool> func;
    if ((UnityEngine.Object) selectable.GetComponent<KPrefabID>() == (UnityEngine.Object) null || !this.modeFilters.TryGetValue(OverlayScreen.Instance.GetMode(), out func))
      return flag;
    flag = func(selectable);
    return flag;
  }

  private static bool ShouldShowOxygenOverlay(KSelectable selectable)
  {
    return (UnityEngine.Object) selectable.GetComponent<AlgaeHabitat>() != (UnityEngine.Object) null || (UnityEngine.Object) selectable.GetComponent<Electrolyzer>() != (UnityEngine.Object) null || (UnityEngine.Object) selectable.GetComponent<AirFilter>() != (UnityEngine.Object) null;
  }

  private static bool ShouldShowLightOverlay(KSelectable selectable)
  {
    return (UnityEngine.Object) selectable.GetComponent<Light2D>() != (UnityEngine.Object) null;
  }

  private static bool ShouldShowRadiationOverlay(KSelectable selectable)
  {
    return (UnityEngine.Object) selectable.GetComponent<Light2D>() != (UnityEngine.Object) null;
  }

  private static bool ShouldShowGasConduitOverlay(KSelectable selectable)
  {
    return (UnityEngine.Object) selectable.GetComponent<Conduit>() != (UnityEngine.Object) null && selectable.GetComponent<Conduit>().type == ConduitType.Gas || (UnityEngine.Object) selectable.GetComponent<Filterable>() != (UnityEngine.Object) null && selectable.GetComponent<Filterable>().filterElementState == Filterable.ElementState.Gas || (UnityEngine.Object) selectable.GetComponent<Vent>() != (UnityEngine.Object) null && selectable.GetComponent<Vent>().conduitType == ConduitType.Gas || (UnityEngine.Object) selectable.GetComponent<Pump>() != (UnityEngine.Object) null && selectable.GetComponent<Pump>().conduitType == ConduitType.Gas || (UnityEngine.Object) selectable.GetComponent<ValveBase>() != (UnityEngine.Object) null && selectable.GetComponent<ValveBase>().conduitType == ConduitType.Gas;
  }

  private static bool ShouldShowLiquidConduitOverlay(KSelectable selectable)
  {
    return (UnityEngine.Object) selectable.GetComponent<Conduit>() != (UnityEngine.Object) null && selectable.GetComponent<Conduit>().type == ConduitType.Liquid || (UnityEngine.Object) selectable.GetComponent<Filterable>() != (UnityEngine.Object) null && selectable.GetComponent<Filterable>().filterElementState == Filterable.ElementState.Liquid || (UnityEngine.Object) selectable.GetComponent<Vent>() != (UnityEngine.Object) null && selectable.GetComponent<Vent>().conduitType == ConduitType.Liquid || (UnityEngine.Object) selectable.GetComponent<Pump>() != (UnityEngine.Object) null && selectable.GetComponent<Pump>().conduitType == ConduitType.Liquid || (UnityEngine.Object) selectable.GetComponent<ValveBase>() != (UnityEngine.Object) null && selectable.GetComponent<ValveBase>().conduitType == ConduitType.Liquid;
  }

  private static bool ShouldShowPowerOverlay(KSelectable selectable)
  {
    Tag prefabTag = selectable.GetComponent<KPrefabID>().PrefabTag;
    return OverlayScreen.WireIDs.Contains(prefabTag) || (UnityEngine.Object) selectable.GetComponent<Battery>() != (UnityEngine.Object) null || (UnityEngine.Object) selectable.GetComponent<PowerTransformer>() != (UnityEngine.Object) null || (UnityEngine.Object) selectable.GetComponent<EnergyConsumer>() != (UnityEngine.Object) null || (UnityEngine.Object) selectable.GetComponent<EnergyGenerator>() != (UnityEngine.Object) null;
  }

  private static bool ShouldShowTileOverlay(KSelectable selectable)
  {
    bool flag = false;
    PrimaryElement component = selectable.GetComponent<PrimaryElement>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
    {
      Element element = component.Element;
      foreach (Tag tileOverlayFilter in Game.Instance.tileOverlayFilters)
      {
        if (element.HasTag(tileOverlayFilter))
        {
          flag = true;
          break;
        }
      }
    }
    return flag;
  }

  private static bool ShouldShowTemperatureOverlay(KSelectable selectable)
  {
    return (UnityEngine.Object) selectable.GetComponent<PrimaryElement>() != (UnityEngine.Object) null;
  }

  private static bool ShouldShowLogicOverlay(KSelectable selectable)
  {
    Tag prefabTag = selectable.GetComponent<KPrefabID>().PrefabTag;
    return OverlayModes.Logic.HighlightItemIDs.Contains(prefabTag) || (UnityEngine.Object) selectable.GetComponent<LogicPorts>() != (UnityEngine.Object) null;
  }

  private static bool ShouldShowSolidConveyorOverlay(KSelectable selectable)
  {
    Tag prefabTag = selectable.GetComponent<KPrefabID>().PrefabTag;
    return OverlayScreen.SolidConveyorIDs.Contains(prefabTag);
  }

  private static bool HideInOverlay(KSelectable selectable)
  {
    return false;
  }

  private static bool ShowOverlayIfHasComponent<T>(KSelectable selectable)
  {
    return (object) selectable.GetComponent<T>() != null;
  }

  private static bool ShouldShowCropOverlay(KSelectable selectable)
  {
    return (UnityEngine.Object) selectable.GetComponent<Uprootable>() != (UnityEngine.Object) null || (UnityEngine.Object) selectable.GetComponent<PlanterBox>() != (UnityEngine.Object) null;
  }
}
