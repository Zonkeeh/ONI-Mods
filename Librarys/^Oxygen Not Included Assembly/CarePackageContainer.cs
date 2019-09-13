// Decompiled with JetBrains decompiler
// Type: CarePackageContainer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class CarePackageContainer : KScreen, ITelepadDeliverableContainer
{
  public float baseCharacterScale = 0.38f;
  private List<GameObject> entryIcons = new List<GameObject>();
  [Header("UI References")]
  [SerializeField]
  private GameObject contentBody;
  [SerializeField]
  private LocText characterName;
  public GameObject selectedBorder;
  [SerializeField]
  private Image titleBar;
  [SerializeField]
  private Color selectedTitleColor;
  [SerializeField]
  private Color deselectedTitleColor;
  [SerializeField]
  private KButton reshuffleButton;
  private KBatchedAnimController animController;
  [SerializeField]
  private LocText itemName;
  [SerializeField]
  private LocText quantity;
  [SerializeField]
  private LocText currentQuantity;
  [SerializeField]
  private LocText description;
  [SerializeField]
  private KToggle selectButton;
  private CarePackageInfo info;
  private CharacterSelectionController controller;
  private static List<ITelepadDeliverableContainer> containers;
  [SerializeField]
  private Sprite enabledSpr;
  [SerializeField]
  private List<CarePackageContainer.ProfessionIcon> professionIcons;
  private Dictionary<string, Sprite> professionIconMap;

  public GameObject GetGameObject()
  {
    return this.gameObject;
  }

  public CarePackageInfo Info
  {
    get
    {
      return this.info;
    }
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Initialize();
    this.reshuffleButton.onClick += (System.Action) (() => this.Reshuffle(true));
    this.StartCoroutine(this.DelayedGeneration());
  }

  public override float GetSortKey()
  {
    return 100f;
  }

  [DebuggerHidden]
  private IEnumerator DelayedGeneration()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new CarePackageContainer.\u003CDelayedGeneration\u003Ec__Iterator0()
    {
      \u0024this = this
    };
  }

  protected override void OnCmpDisable()
  {
    base.OnCmpDisable();
    if (!((UnityEngine.Object) this.animController != (UnityEngine.Object) null))
      return;
    ScreenResize.Instance.OnResize -= new System.Action(this.OnResize);
    this.animController.gameObject.DeleteObject();
    this.animController = (KBatchedAnimController) null;
  }

  protected override void OnCleanUp()
  {
    base.OnCleanUp();
    if ((UnityEngine.Object) this.controller != (UnityEngine.Object) null)
    {
      this.controller.OnLimitReachedEvent -= new System.Action(this.OnCharacterSelectionLimitReached);
      this.controller.OnLimitUnreachedEvent -= new System.Action(this.OnCharacterSelectionLimitUnReached);
      this.controller.OnReshuffleEvent -= new System.Action<bool>(this.Reshuffle);
    }
    if (!((UnityEngine.Object) this.animController != (UnityEngine.Object) null))
      return;
    ScreenResize.Instance.OnResize -= new System.Action(this.OnResize);
  }

  private void Initialize()
  {
    this.professionIconMap = new Dictionary<string, Sprite>();
    this.professionIcons.ForEach((System.Action<CarePackageContainer.ProfessionIcon>) (ic => this.professionIconMap.Add(ic.professionName, ic.iconImg)));
    if (CarePackageContainer.containers == null)
      CarePackageContainer.containers = new List<ITelepadDeliverableContainer>();
    CarePackageContainer.containers.Add((ITelepadDeliverableContainer) this);
  }

  private void GenerateCharacter(bool is_starter)
  {
    int num = 0;
    do
    {
      this.info = Immigration.Instance.RandomCarePackage();
      ++num;
    }
    while (this.IsCharacterRedundant() && num < 20);
    if ((UnityEngine.Object) this.animController != (UnityEngine.Object) null)
    {
      ScreenResize.Instance.OnResize -= new System.Action(this.OnResize);
      UnityEngine.Object.Destroy((UnityEngine.Object) this.animController.gameObject);
      this.animController = (KBatchedAnimController) null;
    }
    this.SetAnimator();
    this.SetInfoText();
    this.selectButton.ClearOnClick();
    if (this.controller.IsStarterMinion)
      return;
    this.selectButton.onClick += (System.Action) (() => this.SelectDeliverable());
  }

  private void OnResize()
  {
    this.animController.animScale = this.baseCharacterScale * (1f / UnityEngine.Object.FindObjectOfType<KCanvasScaler>().GetCanvasScale());
  }

  private void SetAnimator()
  {
    GameObject prefab = Assets.GetPrefab(this.info.id.ToTag());
    EdiblesManager.FoodInfo foodInfo = Game.Instance.ediblesManager.GetFoodInfo(this.info.id);
    int num1 = ElementLoader.FindElementByName(this.info.id) == null ? (foodInfo == null ? (int) this.info.quantity : (int) ((double) this.info.quantity % (double) foodInfo.CaloriesPerUnit)) : 1;
    if ((UnityEngine.Object) prefab != (UnityEngine.Object) null)
    {
      for (int index = 0; index < num1; ++index)
      {
        GameObject gameObject = Util.KInstantiateUI(this.contentBody, this.contentBody.transform.parent.gameObject, false);
        gameObject.SetActive(true);
        Image component = gameObject.GetComponent<Image>();
        Tuple<Sprite, Color> uiSprite = Def.GetUISprite((object) prefab, "ui", false);
        component.sprite = uiSprite.first;
        component.color = uiSprite.second;
        this.entryIcons.Add(gameObject);
        if (num1 > 1)
        {
          int num2;
          int num3;
          int num4;
          if (num1 % 2 == 1)
          {
            num2 = Mathf.CeilToInt((float) (num1 / 2));
            int num5 = num2 - index;
            num3 = num5 <= 0 ? -1 : 1;
            num4 = Mathf.Abs(num5);
          }
          else
          {
            num2 = num1 / 2 - 1;
            if (index <= num2)
            {
              num4 = Mathf.Abs(num2 - index);
              num3 = -1;
            }
            else
            {
              num4 = Mathf.Abs(num2 + 1 - index);
              num3 = 1;
            }
          }
          int num6 = 0;
          if (num1 % 2 == 0)
          {
            num6 = index > num2 ? 6 : -6;
            gameObject.transform.SetPosition(gameObject.transform.position += new Vector3((float) num6, 0.0f, 0.0f));
          }
          gameObject.transform.localScale = new Vector3((float) (1.0 - (double) num4 * 0.100000001490116), (float) (1.0 - (double) num4 * 0.100000001490116), 1f);
          gameObject.transform.Rotate(0.0f, 0.0f, 3f * (float) num4 * (float) num3);
          gameObject.transform.SetPosition(gameObject.transform.position + new Vector3(25f * (float) num4 * (float) num3, 5f * (float) num4) + new Vector3((float) num6, 0.0f, 0.0f));
          gameObject.GetComponent<Canvas>().sortingOrder = num1 - num4;
        }
      }
    }
    else
    {
      GameObject gameObject = Util.KInstantiateUI(this.contentBody, this.contentBody.transform.parent.gameObject, false);
      gameObject.SetActive(true);
      Image component = gameObject.GetComponent<Image>();
      component.sprite = Def.GetUISpriteFromMultiObjectAnim(ElementLoader.GetElement(this.info.id.ToTag()).substance.anim, "ui", false, string.Empty);
      component.color = (Color) ElementLoader.GetElement(this.info.id.ToTag()).substance.uiColour;
      this.entryIcons.Add(gameObject);
    }
  }

  private string GetSpawnableName()
  {
    GameObject prefab = Assets.GetPrefab((Tag) this.info.id);
    if (!((UnityEngine.Object) prefab == (UnityEngine.Object) null))
      return prefab.GetProperName();
    Element elementByName = ElementLoader.FindElementByName(this.info.id);
    if (elementByName != null)
      return elementByName.substance.name;
    return string.Empty;
  }

  private string GetSpawnableQuantityOnly()
  {
    if (ElementLoader.GetElement(this.info.id.ToTag()) != null)
      return string.Format((string) STRINGS.UI.IMMIGRANTSCREEN.CARE_PACKAGE_ELEMENT_COUNT_ONLY, (object) GameUtil.GetFormattedMass(this.info.quantity, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
    if (Game.Instance.ediblesManager.GetFoodInfo(this.info.id) != null)
      return string.Format((string) STRINGS.UI.IMMIGRANTSCREEN.CARE_PACKAGE_ELEMENT_COUNT_ONLY, (object) GameUtil.GetFormattedCaloriesForItem((Tag) this.info.id, this.info.quantity, GameUtil.TimeSlice.None, true));
    return string.Format((string) STRINGS.UI.IMMIGRANTSCREEN.CARE_PACKAGE_ELEMENT_COUNT_ONLY, (object) this.info.quantity.ToString());
  }

  private string GetCurrentQuantity()
  {
    if (ElementLoader.GetElement(this.info.id.ToTag()) != null)
    {
      float amount = WorldInventory.Instance.GetAmount(this.info.id.ToTag());
      return string.Format((string) STRINGS.UI.IMMIGRANTSCREEN.CARE_PACKAGE_CURRENT_AMOUNT, (object) GameUtil.GetFormattedMass(amount, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
    }
    if (Game.Instance.ediblesManager.GetFoodInfo(this.info.id) != null)
    {
      float calories = RationTracker.Get().CountRationsByFoodType(this.info.id, true);
      return string.Format((string) STRINGS.UI.IMMIGRANTSCREEN.CARE_PACKAGE_CURRENT_AMOUNT, (object) GameUtil.GetFormattedCalories(calories, GameUtil.TimeSlice.None, true));
    }
    float amount1 = WorldInventory.Instance.GetAmount(this.info.id.ToTag());
    return string.Format((string) STRINGS.UI.IMMIGRANTSCREEN.CARE_PACKAGE_CURRENT_AMOUNT, (object) amount1.ToString());
  }

  private string GetSpawnableQuantity()
  {
    if (ElementLoader.GetElement(this.info.id.ToTag()) != null)
      return string.Format((string) STRINGS.UI.IMMIGRANTSCREEN.CARE_PACKAGE_ELEMENT_QUANTITY, (object) GameUtil.GetFormattedMass(this.info.quantity, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), (object) Assets.GetPrefab((Tag) this.info.id).GetProperName());
    if (Game.Instance.ediblesManager.GetFoodInfo(this.info.id) != null)
      return string.Format((string) STRINGS.UI.IMMIGRANTSCREEN.CARE_PACKAGE_ELEMENT_QUANTITY, (object) GameUtil.GetFormattedCaloriesForItem((Tag) this.info.id, this.info.quantity, GameUtil.TimeSlice.None, true), (object) Assets.GetPrefab((Tag) this.info.id).GetProperName());
    return string.Format((string) STRINGS.UI.IMMIGRANTSCREEN.CARE_PACKAGE_ELEMENT_COUNT, (object) Assets.GetPrefab((Tag) this.info.id).GetProperName(), (object) this.info.quantity.ToString());
  }

  private string GetSpawnableDescription()
  {
    Element element = ElementLoader.GetElement(this.info.id.ToTag());
    if (element != null)
      return element.Description();
    GameObject prefab = Assets.GetPrefab((Tag) this.info.id);
    if ((UnityEngine.Object) prefab == (UnityEngine.Object) null)
      return string.Empty;
    InfoDescription component = prefab.GetComponent<InfoDescription>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      return component.description;
    return prefab.GetProperName();
  }

  private void SetInfoText()
  {
    this.characterName.SetText(this.GetSpawnableName());
    this.description.SetText(this.GetSpawnableDescription());
    this.itemName.SetText(this.GetSpawnableName());
    this.quantity.SetText(this.GetSpawnableQuantityOnly());
    this.currentQuantity.SetText(this.GetCurrentQuantity());
  }

  public void SelectDeliverable()
  {
    if ((UnityEngine.Object) this.controller != (UnityEngine.Object) null)
      this.controller.AddDeliverable((ITelepadDeliverable) this.info);
    if (MusicManager.instance.SongIsPlaying("Music_SelectDuplicant"))
      MusicManager.instance.SetSongParameter("Music_SelectDuplicant", "songSection", 1f, true);
    this.selectButton.GetComponent<ImageToggleState>().SetActive();
    this.selectButton.ClearOnClick();
    this.selectButton.onClick += (System.Action) (() =>
    {
      this.DeselectDeliverable();
      if (!MusicManager.instance.SongIsPlaying("Music_SelectDuplicant"))
        return;
      MusicManager.instance.SetSongParameter("Music_SelectDuplicant", "songSection", 0.0f, true);
    });
    this.selectedBorder.SetActive(true);
    this.titleBar.color = this.selectedTitleColor;
  }

  public void DeselectDeliverable()
  {
    if ((UnityEngine.Object) this.controller != (UnityEngine.Object) null)
      this.controller.RemoveDeliverable((ITelepadDeliverable) this.info);
    this.selectButton.GetComponent<ImageToggleState>().SetInactive();
    this.selectButton.Deselect();
    this.selectButton.ClearOnClick();
    this.selectButton.onClick += (System.Action) (() => this.SelectDeliverable());
    this.selectedBorder.SetActive(false);
    this.titleBar.color = this.deselectedTitleColor;
  }

  private void OnReplacedEvent(ITelepadDeliverable stats)
  {
    if (stats != this.info)
      return;
    this.DeselectDeliverable();
  }

  private void OnCharacterSelectionLimitReached()
  {
    if ((UnityEngine.Object) this.controller != (UnityEngine.Object) null && this.controller.IsSelected((ITelepadDeliverable) this.info))
      return;
    this.selectButton.ClearOnClick();
    if (this.controller.AllowsReplacing)
      this.selectButton.onClick += new System.Action(this.ReplaceCharacterSelection);
    else
      this.selectButton.onClick += new System.Action(this.CantSelectCharacter);
  }

  private void CantSelectCharacter()
  {
    KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative", false));
  }

  private void ReplaceCharacterSelection()
  {
    if ((UnityEngine.Object) this.controller == (UnityEngine.Object) null)
      return;
    this.controller.RemoveLast();
    this.SelectDeliverable();
  }

  private void OnCharacterSelectionLimitUnReached()
  {
    if ((UnityEngine.Object) this.controller != (UnityEngine.Object) null && this.controller.IsSelected((ITelepadDeliverable) this.info))
      return;
    this.selectButton.ClearOnClick();
    this.selectButton.onClick += (System.Action) (() => this.SelectDeliverable());
  }

  public void SetReshufflingState(bool enable)
  {
    this.reshuffleButton.gameObject.SetActive(enable);
  }

  private void Reshuffle(bool is_starter)
  {
    if ((UnityEngine.Object) this.controller != (UnityEngine.Object) null && this.controller.IsSelected((ITelepadDeliverable) this.info))
      this.DeselectDeliverable();
    this.ClearEntryIcons();
    this.GenerateCharacter(is_starter);
  }

  public void SetController(CharacterSelectionController csc)
  {
    if ((UnityEngine.Object) csc == (UnityEngine.Object) this.controller)
      return;
    this.controller = csc;
    this.controller.OnLimitReachedEvent += new System.Action(this.OnCharacterSelectionLimitReached);
    this.controller.OnLimitUnreachedEvent += new System.Action(this.OnCharacterSelectionLimitUnReached);
    this.controller.OnReshuffleEvent += new System.Action<bool>(this.Reshuffle);
    this.controller.OnReplacedEvent += new System.Action<ITelepadDeliverable>(this.OnReplacedEvent);
  }

  public void DisableSelectButton()
  {
    this.selectButton.soundPlayer.AcceptClickCondition = (Func<bool>) (() => false);
    this.selectButton.GetComponent<ImageToggleState>().SetDisabled();
    this.selectButton.soundPlayer.Enabled = false;
  }

  private bool IsCharacterRedundant()
  {
    foreach (ITelepadDeliverableContainer container in CarePackageContainer.containers)
    {
      if (!object.ReferenceEquals((object) container, (object) this))
      {
        CarePackageContainer packageContainer = container as CarePackageContainer;
        if ((UnityEngine.Object) packageContainer != (UnityEngine.Object) null && packageContainer.info == this.info)
          return true;
      }
    }
    return false;
  }

  public string GetValueColor(bool isPositive)
  {
    return isPositive ? "<color=green>" : "<color=#ff2222ff>";
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (e.IsAction(Action.Escape))
      this.controller.OnPressBack();
    e.Consumed = true;
  }

  public override void OnKeyUp(KButtonEvent e)
  {
    e.Consumed = true;
  }

  protected override void OnCmpEnable()
  {
    this.OnActivate();
    if (this.info == null)
      return;
    this.ClearEntryIcons();
    this.SetAnimator();
    this.SetInfoText();
  }

  private void ClearEntryIcons()
  {
    for (int index = 0; index < this.entryIcons.Count; ++index)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.entryIcons[index]);
  }

  [Serializable]
  public struct ProfessionIcon
  {
    public string professionName;
    public Sprite iconImg;
  }
}
