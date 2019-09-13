// Decompiled with JetBrains decompiler
// Type: ScreenPrefabs
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class ScreenPrefabs : KMonoBehaviour
{
  public ControlsScreen ControlsScreen;
  public Hud HudScreen;
  public HoverTextScreen HoverTextScreen;
  public OverlayScreen OverlayScreen;
  public TileScreen TileScreen;
  public SpeedControlScreen SpeedControlScreen;
  public OverviewScreen ManagementScreen;
  public ManagementMenu ManagementMenu;
  public ToolTipScreen ToolTipScreen;
  public DebugPaintElementScreen DebugPaintElementScreen;
  public UserMenuScreen UserMenuScreen;
  public KButtonMenu OwnerScreen;
  public EnergyInfoScreen EnergyInfoScreen;
  public KButtonMenu ButtonGrid;
  public NameDisplayScreen NameDisplayScreen;
  public ConfirmDialogScreen ConfirmDialogScreen;
  public CustomizableDialogScreen CustomizableDialogScreen;
  public InfoDialogScreen InfoDialogScreen;
  public StoryMessageScreen StoryMessageScreen;
  public FileNameDialog FileNameDialog;
  public TagFilterScreen TagFilterScreen;
  public ResearchScreen ResearchScreen;
  public MessageDialogFrame MessageDialogFrame;
  public ResourceCategoryScreen ResourceCategoryScreen;
  public LanguageOptionsScreen languageOptionsScreen;
  public ModsScreen modsMenu;
  public GameObject GameOverScreen;
  public GameObject VictoryScreen;
  public GameObject StatusItemIndicatorScreen;
  public GameObject CollapsableContentPanel;
  public GameObject DescriptionLabel;
  public LoadingOverlay loadingOverlay;
  public LoadScreen LoadScreen;
  public InspectSaveScreen InspectSaveScreen;
  public OptionsMenuScreen OptionsScreen;
  public WorldGenScreen WorldGenScreen;
  public ModeSelectScreen ModeSelectScreen;
  public NewGameSettingsScreen NewGameSettingsScreen;
  public ColonyDestinationSelectScreen ColonyDestinationSelectScreen;
  public RetiredColonyInfoScreen RetiredColonyInfoScreen;
  public VideoScreen VideoScreen;
  public ComicViewer ComicViewer;

  public static ScreenPrefabs Instance { get; private set; }

  protected override void OnPrefabInit()
  {
    ScreenPrefabs.Instance = this;
  }

  public void ConfirmDoAction(string message, System.Action action, Transform parent)
  {
    ((ConfirmDialogScreen) KScreenManager.Instance.StartScreen(ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject, parent.gameObject)).PopupConfirmDialog(message, action, (System.Action) (() => {}), (string) null, (System.Action) null, (string) null, (string) null, (string) null, (Sprite) null, true);
  }
}
