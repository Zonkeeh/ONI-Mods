// Decompiled with JetBrains decompiler
// Type: CharacterContainer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Database;
using Klei.AI;
using STRINGS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TUNING;
using UnityEngine;
using UnityEngine.UI;

public class CharacterContainer : KScreen, ITelepadDeliverableContainer
{
  private static readonly HashedString[] idleAnims = new HashedString[7]
  {
    (HashedString) "anim_idle_healthy_kanim",
    (HashedString) "anim_idle_susceptible_kanim",
    (HashedString) "anim_idle_keener_kanim",
    (HashedString) "anim_idle_coaster_kanim",
    (HashedString) "anim_idle_fastfeet_kanim",
    (HashedString) "anim_idle_breatherdeep_kanim",
    (HashedString) "anim_idle_breathershallow_kanim"
  };
  [HideInInspector]
  public bool addMinionToIdentityList = true;
  public float baseCharacterScale = 0.38f;
  [SerializeField]
  private GameObject contentBody;
  [SerializeField]
  private LocText characterName;
  [SerializeField]
  private EditableTitleBar characterNameTitle;
  [SerializeField]
  private LocText characterJob;
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
  private GameObject iconGroup;
  private List<GameObject> iconGroups;
  [SerializeField]
  private LocText goodTrait;
  [SerializeField]
  private LocText badTrait;
  [SerializeField]
  private GameObject aptitudeEntry;
  [SerializeField]
  private Transform aptitudeLabel;
  [SerializeField]
  private Transform attributeLabelAptitude;
  [SerializeField]
  private Transform attributeLabelTrait;
  [SerializeField]
  private LocText expectationRight;
  private List<LocText> expectationLabels;
  [SerializeField]
  private DropDown archetypeDropDown;
  [SerializeField]
  private Image selectedArchetypeIcon;
  [SerializeField]
  private Sprite noArchetypeIcon;
  [SerializeField]
  private Sprite dropdownArrowIcon;
  private string guaranteedAptitudeID;
  private List<GameObject> aptitudeEntries;
  private List<GameObject> traitEntries;
  [SerializeField]
  private LocText description;
  [SerializeField]
  private KToggle selectButton;
  [SerializeField]
  private KBatchedAnimController fxAnim;
  private MinionStartingStats stats;
  private CharacterSelectionController controller;
  private static List<CharacterContainer> containers;
  private KAnimFile idle_anim;
  [SerializeField]
  private Sprite enabledSpr;

  public GameObject GetGameObject()
  {
    return this.gameObject;
  }

  public MinionStartingStats Stats
  {
    get
    {
      return this.stats;
    }
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Initialize();
    this.characterNameTitle.OnStartedEditing += new System.Action(this.OnStartedEditing);
    this.characterNameTitle.OnNameChanged += new System.Action<string>(this.OnNameChanged);
    this.reshuffleButton.onClick += (System.Action) (() => this.Reshuffle(true));
    List<IListableOption> listableOptionList = new List<IListableOption>();
    foreach (SkillGroup skillGroup in new List<SkillGroup>((IEnumerable<SkillGroup>) Db.Get().SkillGroups.resources))
      listableOptionList.Add((IListableOption) skillGroup);
    this.archetypeDropDown.Initialize((IEnumerable<IListableOption>) listableOptionList, new System.Action<IListableOption, object>(this.OnArchetypeEntryClick), new Func<IListableOption, IListableOption, object, int>(this.archetypeDropDownSort), new System.Action<DropDownEntry, object>(this.archetypeDropEntryRefreshAction), false, (object) null);
    this.archetypeDropDown.CustomizeEmptyRow((string) Strings.Get("STRINGS.UI.CHARACTERCONTAINER_NOARCHETYPESELECTED"), this.noArchetypeIcon);
    this.StartCoroutine(this.DelayedGeneration());
  }

  public void ForceStopEditingTitle()
  {
    this.characterNameTitle.ForceStopEditing();
  }

  public override float GetSortKey()
  {
    return 100f;
  }

  [DebuggerHidden]
  private IEnumerator DelayedGeneration()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new CharacterContainer.\u003CDelayedGeneration\u003Ec__Iterator0()
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
    this.iconGroups = new List<GameObject>();
    this.traitEntries = new List<GameObject>();
    this.expectationLabels = new List<LocText>();
    this.aptitudeEntries = new List<GameObject>();
    if (CharacterContainer.containers == null)
      CharacterContainer.containers = new List<CharacterContainer>();
    CharacterContainer.containers.Add(this);
  }

  private void OnNameChanged(string newName)
  {
    this.stats.Name = newName;
    this.stats.personality.Name = newName;
    this.description.text = this.stats.personality.description;
  }

  private void OnStartedEditing()
  {
    KScreenManager.Instance.RefreshStack();
  }

  private void GenerateCharacter(bool is_starter, string guaranteedAptitudeID = null)
  {
    int num = 0;
    do
    {
      this.stats = new MinionStartingStats(is_starter, guaranteedAptitudeID);
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
    this.StartCoroutine(this.SetAttributes());
    this.selectButton.ClearOnClick();
    if (this.controller.IsStarterMinion)
      return;
    this.selectButton.enabled = true;
    this.selectButton.onClick += (System.Action) (() => this.SelectDeliverable());
  }

  private void OnResize()
  {
    KCanvasScaler objectOfType = UnityEngine.Object.FindObjectOfType<KCanvasScaler>();
    this.animController.animScale = this.baseCharacterScale * (1f / objectOfType.GetCanvasScale());
    Transform transform = this.animController.transform.parent.gameObject.transform.Find("BG");
    KBatchedAnimController kbatchedAnimController = !((UnityEngine.Object) transform != (UnityEngine.Object) null) ? (KBatchedAnimController) null : transform.gameObject.GetComponent<KBatchedAnimController>();
    if (!((UnityEngine.Object) kbatchedAnimController != (UnityEngine.Object) null))
      return;
    kbatchedAnimController.animScale = this.baseCharacterScale * (1f / objectOfType.GetCanvasScale());
  }

  private void SetAnimator()
  {
    if ((UnityEngine.Object) this.animController == (UnityEngine.Object) null)
    {
      this.animController = Util.KInstantiateUI(Assets.GetPrefab(new Tag("MinionSelectPreview")), this.contentBody.gameObject, false).GetComponent<KBatchedAnimController>();
      this.animController.gameObject.SetActive(true);
      KCanvasScaler objectOfType = UnityEngine.Object.FindObjectOfType<KCanvasScaler>();
      this.animController.animScale = this.baseCharacterScale * (1f / objectOfType.GetCanvasScale());
      ScreenResize.Instance.OnResize += new System.Action(this.OnResize);
      Transform transform = this.animController.transform.parent.gameObject.transform.Find("BG");
      KBatchedAnimController kbatchedAnimController = !((UnityEngine.Object) transform != (UnityEngine.Object) null) ? (KBatchedAnimController) null : transform.gameObject.GetComponent<KBatchedAnimController>();
      if ((UnityEngine.Object) kbatchedAnimController != (UnityEngine.Object) null)
        kbatchedAnimController.animScale = this.baseCharacterScale * (1f / objectOfType.GetCanvasScale());
    }
    this.stats.ApplyTraits(this.animController.gameObject);
    this.stats.ApplyRace(this.animController.gameObject);
    this.stats.ApplyAccessories(this.animController.gameObject);
    this.stats.ApplyExperience(this.animController.gameObject);
    this.idle_anim = Assets.GetAnim(CharacterContainer.idleAnims[UnityEngine.Random.Range(0, CharacterContainer.idleAnims.Length)]);
    if ((UnityEngine.Object) this.idle_anim != (UnityEngine.Object) null)
      this.animController.AddAnimOverrides(this.idle_anim, 0.0f);
    KAnimFile anim = Assets.GetAnim(new HashedString("crewSelect_fx_kanim"));
    if ((UnityEngine.Object) anim != (UnityEngine.Object) null)
      this.animController.AddAnimOverrides(anim, 0.0f);
    this.animController.Queue((HashedString) "idle_default", KAnim.PlayMode.Loop, 1f, 0.0f);
  }

  private void SetInfoText()
  {
    this.traitEntries.ForEach((System.Action<GameObject>) (tl => UnityEngine.Object.Destroy((UnityEngine.Object) tl.gameObject)));
    this.traitEntries.Clear();
    this.characterNameTitle.SetTitle(this.stats.Name);
    for (int index1 = 1; index1 < this.stats.Traits.Count; ++index1)
    {
      Trait trait = this.stats.Traits[index1];
      LocText locText1 = !trait.PositiveTrait ? this.badTrait : this.goodTrait;
      LocText locText2 = Util.KInstantiateUI<LocText>(locText1.gameObject, locText1.transform.parent.gameObject, false);
      locText2.gameObject.SetActive(true);
      locText2.text = this.stats.Traits[index1].Name;
      locText2.color = !trait.PositiveTrait ? Constants.NEGATIVE_COLOR : Constants.POSITIVE_COLOR;
      locText2.GetComponent<ToolTip>().SetSimpleTooltip(trait.description);
      for (int index2 = 0; index2 < trait.SelfModifiers.Count; ++index2)
      {
        GameObject gameObject = Util.KInstantiateUI(this.attributeLabelTrait.gameObject, locText1.transform.parent.gameObject, false);
        gameObject.SetActive(true);
        LocText componentInChildren = gameObject.GetComponentInChildren<LocText>();
        string format = (string) ((double) trait.SelfModifiers[index2].Value <= 0.0 ? STRINGS.UI.CHARACTERCONTAINER_ATTRIBUTEMODIFIER_DECREASED : STRINGS.UI.CHARACTERCONTAINER_ATTRIBUTEMODIFIER_INCREASED);
        componentInChildren.text = string.Format(format, (object) Strings.Get("STRINGS.DUPLICANTS.ATTRIBUTES." + trait.SelfModifiers[index2].AttributeId.ToUpper() + ".NAME"));
        if (!(trait.SelfModifiers[index2].AttributeId == "GermResistance"))
          ;
        Klei.AI.Attribute attrib = Db.Get().Attributes.Get(trait.SelfModifiers[index2].AttributeId);
        string message = attrib.Description + "\n\n" + (string) Strings.Get("STRINGS.DUPLICANTS.ATTRIBUTES." + trait.SelfModifiers[index2].AttributeId.ToUpper() + ".NAME") + ": " + trait.SelfModifiers[index2].GetFormattedString((GameObject) null);
        List<AttributeConverter> convertersForAttribute = Db.Get().AttributeConverters.GetConvertersForAttribute(attrib);
        for (int index3 = 0; index3 < convertersForAttribute.Count; ++index3)
        {
          string str = convertersForAttribute[index3].DescriptionFromAttribute(convertersForAttribute[index3].multiplier * trait.SelfModifiers[index2].Value, (GameObject) null);
          if (str != string.Empty)
            message = message + "\n    • " + str;
        }
        componentInChildren.GetComponent<ToolTip>().SetSimpleTooltip(message);
        this.traitEntries.Add(gameObject);
      }
      if (trait.disabledChoreGroups != null)
      {
        GameObject gameObject = Util.KInstantiateUI(this.attributeLabelTrait.gameObject, locText1.transform.parent.gameObject, false);
        gameObject.SetActive(true);
        LocText componentInChildren = gameObject.GetComponentInChildren<LocText>();
        componentInChildren.text = trait.GetDisabledChoresString(false);
        string empty1 = string.Empty;
        string empty2 = string.Empty;
        for (int index2 = 0; index2 < trait.disabledChoreGroups.Length; ++index2)
        {
          if (index2 > 0)
          {
            empty1 += ", ";
            empty2 += "\n";
          }
          empty1 += trait.disabledChoreGroups[index2].Name;
          empty2 += trait.disabledChoreGroups[index2].description;
        }
        componentInChildren.GetComponent<ToolTip>().SetSimpleTooltip(string.Format((string) DUPLICANTS.TRAITS.CANNOT_DO_TASK_TOOLTIP, (object) empty1, (object) empty2));
        this.traitEntries.Add(gameObject);
      }
      if (trait.ignoredEffects != null && trait.ignoredEffects.Length > 0)
      {
        GameObject gameObject = Util.KInstantiateUI(this.attributeLabelTrait.gameObject, locText1.transform.parent.gameObject, false);
        gameObject.SetActive(true);
        LocText componentInChildren = gameObject.GetComponentInChildren<LocText>();
        componentInChildren.text = trait.GetIgnoredEffectsString(false);
        string empty1 = string.Empty;
        string empty2 = string.Empty;
        for (int index2 = 0; index2 < trait.ignoredEffects.Length; ++index2)
        {
          if (index2 > 0)
          {
            empty1 += ", ";
            empty2 += "\n";
          }
          empty1 += (string) Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + trait.ignoredEffects[index2].ToUpper() + ".NAME");
          empty2 += (string) Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + trait.ignoredEffects[index2].ToUpper() + ".CAUSE");
        }
        componentInChildren.GetComponent<ToolTip>().SetSimpleTooltip(string.Format((string) DUPLICANTS.TRAITS.IGNORED_EFFECTS_TOOLTIP, (object) empty1, (object) empty2));
        this.traitEntries.Add(gameObject);
      }
      StringEntry result;
      if (Strings.TryGet("STRINGS.DUPLICANTS.TRAITS." + trait.Id.ToUpper() + ".SHORT_DESC", out result))
      {
        GameObject gameObject = Util.KInstantiateUI(this.attributeLabelTrait.gameObject, locText1.transform.parent.gameObject, false);
        gameObject.SetActive(true);
        LocText componentInChildren = gameObject.GetComponentInChildren<LocText>();
        componentInChildren.text = result.String;
        componentInChildren.GetComponent<ToolTip>().SetSimpleTooltip((string) Strings.Get("STRINGS.DUPLICANTS.TRAITS." + trait.Id.ToUpper() + ".SHORT_DESC_TOOLTIP"));
        this.traitEntries.Add(gameObject);
      }
      this.traitEntries.Add(locText2.gameObject);
    }
    this.aptitudeEntries.ForEach((System.Action<GameObject>) (al => UnityEngine.Object.Destroy((UnityEngine.Object) al.gameObject)));
    this.aptitudeEntries.Clear();
    this.expectationLabels.ForEach((System.Action<LocText>) (el => UnityEngine.Object.Destroy((UnityEngine.Object) el.gameObject)));
    this.expectationLabels.Clear();
    foreach (KeyValuePair<SkillGroup, float> skillAptitude in this.stats.skillAptitudes)
    {
      if ((double) skillAptitude.Value != 0.0)
      {
        SkillGroup skillGroup = Db.Get().SkillGroups.Get(skillAptitude.Key.IdHash);
        if (skillGroup == null)
        {
          Debug.LogWarningFormat("Role group not found for aptitude: {0}", (object) skillAptitude.Key);
        }
        else
        {
          GameObject parent = Util.KInstantiateUI(this.aptitudeEntry.gameObject, this.aptitudeEntry.transform.parent.gameObject, false);
          LocText locText1 = Util.KInstantiateUI<LocText>(this.aptitudeLabel.gameObject, parent, false);
          locText1.gameObject.SetActive(true);
          locText1.text = skillGroup.Name;
          string empty = string.Empty;
          string message1;
          if (skillGroup.choreGroupID != string.Empty)
          {
            ChoreGroup choreGroup = Db.Get().ChoreGroups.Get(skillGroup.choreGroupID);
            message1 = string.Format((string) DUPLICANTS.ROLES.GROUPS.APTITUDE_DESCRIPTION_CHOREGROUP, (object) skillGroup.Name, (object) DUPLICANTSTATS.APTITUDE_BONUS, (object) choreGroup.description);
          }
          else
            message1 = string.Format((string) DUPLICANTS.ROLES.GROUPS.APTITUDE_DESCRIPTION, (object) skillGroup.Name, (object) DUPLICANTSTATS.APTITUDE_BONUS);
          locText1.GetComponent<ToolTip>().SetSimpleTooltip(message1);
          float num = (float) DUPLICANTSTATS.APTITUDE_ATTRIBUTE_BONUSES[this.stats.skillAptitudes.Count - 1];
          LocText locText2 = Util.KInstantiateUI<LocText>(this.attributeLabelAptitude.gameObject, parent, false);
          locText2.gameObject.SetActive(true);
          locText2.text = "+" + (object) num + " " + skillAptitude.Key.relevantAttributes[0].Name;
          string message2 = skillAptitude.Key.relevantAttributes[0].Description + "\n\n" + skillAptitude.Key.relevantAttributes[0].Name + ": +" + (object) DUPLICANTSTATS.APTITUDE_ATTRIBUTE_BONUSES[this.stats.skillAptitudes.Count - 1];
          List<AttributeConverter> convertersForAttribute = Db.Get().AttributeConverters.GetConvertersForAttribute(skillAptitude.Key.relevantAttributes[0]);
          for (int index = 0; index < convertersForAttribute.Count; ++index)
            message2 = message2 + "\n    • " + convertersForAttribute[index].DescriptionFromAttribute(convertersForAttribute[index].multiplier * num, (GameObject) null);
          locText2.GetComponent<ToolTip>().SetSimpleTooltip(message2);
          parent.gameObject.SetActive(true);
          this.aptitudeEntries.Add(parent);
        }
      }
    }
    if (this.stats.stressTrait != null)
    {
      LocText locText = Util.KInstantiateUI<LocText>(this.expectationRight.gameObject, this.expectationRight.transform.parent.gameObject, false);
      locText.gameObject.SetActive(true);
      locText.text = string.Format((string) STRINGS.UI.CHARACTERCONTAINER_STRESSTRAIT, (object) this.stats.stressTrait.Name);
      locText.GetComponent<ToolTip>().SetSimpleTooltip(this.stats.stressTrait.GetTooltip());
      this.expectationLabels.Add(locText);
    }
    if (this.stats.congenitaltrait != null)
    {
      LocText locText = Util.KInstantiateUI<LocText>(this.expectationRight.gameObject, this.expectationRight.transform.parent.gameObject, false);
      locText.gameObject.SetActive(true);
      locText.text = string.Format((string) STRINGS.UI.CHARACTERCONTAINER_CONGENITALTRAIT, (object) this.stats.congenitaltrait.Name);
      locText.GetComponent<ToolTip>().SetSimpleTooltip(this.stats.congenitaltrait.GetTooltip());
      this.expectationLabels.Add(locText);
    }
    this.description.text = this.stats.personality.description;
  }

  [DebuggerHidden]
  private IEnumerator SetAttributes()
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new CharacterContainer.\u003CSetAttributes\u003Ec__Iterator1()
    {
      \u0024this = this
    };
  }

  public void SelectDeliverable()
  {
    if ((UnityEngine.Object) this.controller != (UnityEngine.Object) null)
      this.controller.AddDeliverable((ITelepadDeliverable) this.stats);
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
    this.animController.Play((HashedString) "cheer_pre", KAnim.PlayMode.Once, 1f, 0.0f);
    this.animController.Play((HashedString) "cheer_loop", KAnim.PlayMode.Loop, 1f, 0.0f);
  }

  public void DeselectDeliverable()
  {
    if ((UnityEngine.Object) this.controller != (UnityEngine.Object) null)
      this.controller.RemoveDeliverable((ITelepadDeliverable) this.stats);
    this.selectButton.GetComponent<ImageToggleState>().SetInactive();
    this.selectButton.Deselect();
    this.selectButton.ClearOnClick();
    this.selectButton.onClick += (System.Action) (() => this.SelectDeliverable());
    this.selectedBorder.SetActive(false);
    this.titleBar.color = this.deselectedTitleColor;
    this.animController.Queue((HashedString) "cheer_pst", KAnim.PlayMode.Once, 1f, 0.0f);
    this.animController.Queue((HashedString) "idle_default", KAnim.PlayMode.Loop, 1f, 0.0f);
  }

  private void OnReplacedEvent(ITelepadDeliverable deliverable)
  {
    if (deliverable != this.stats)
      return;
    this.DeselectDeliverable();
  }

  private void OnCharacterSelectionLimitReached()
  {
    if ((UnityEngine.Object) this.controller != (UnityEngine.Object) null && this.controller.IsSelected((ITelepadDeliverable) this.stats))
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
    if ((UnityEngine.Object) this.controller != (UnityEngine.Object) null && this.controller.IsSelected((ITelepadDeliverable) this.stats))
      return;
    this.selectButton.ClearOnClick();
    this.selectButton.onClick += (System.Action) (() => this.SelectDeliverable());
  }

  public void SetReshufflingState(bool enable)
  {
    this.reshuffleButton.gameObject.SetActive(enable);
    this.archetypeDropDown.gameObject.SetActive(enable);
  }

  private void Reshuffle(bool is_starter)
  {
    if ((UnityEngine.Object) this.controller != (UnityEngine.Object) null && this.controller.IsSelected((ITelepadDeliverable) this.stats))
      this.DeselectDeliverable();
    if ((UnityEngine.Object) this.fxAnim != (UnityEngine.Object) null)
      this.fxAnim.Play((HashedString) "loop", KAnim.PlayMode.Once, 1f, 0.0f);
    this.GenerateCharacter(is_starter, this.guaranteedAptitudeID);
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
    if (!((UnityEngine.Object) CharacterContainer.containers.Find((Predicate<CharacterContainer>) (c =>
    {
      if ((UnityEngine.Object) c != (UnityEngine.Object) null && c.stats != null && (UnityEngine.Object) c != (UnityEngine.Object) this)
        return c.stats.Name == this.stats.Name;
      return false;
    })) != (UnityEngine.Object) null))
      return Components.LiveMinionIdentities.Items.Any<MinionIdentity>((Func<MinionIdentity, bool>) (id => id.GetProperName() == this.stats.Name));
    return true;
  }

  public string GetValueColor(bool isPositive)
  {
    return isPositive ? "<color=green>" : "<color=#ff2222ff>";
  }

  public override void OnKeyDown(KButtonEvent e)
  {
    if (e.IsAction(Action.Escape))
    {
      this.characterNameTitle.ForceStopEditing();
      this.controller.OnPressBack();
    }
    e.Consumed = true;
  }

  public override void OnKeyUp(KButtonEvent e)
  {
    e.Consumed = true;
  }

  protected override void OnCmpEnable()
  {
    this.OnActivate();
    if (this.stats == null)
      return;
    this.SetAnimator();
  }

  protected override void OnShow(bool show)
  {
    base.OnShow(show);
    this.characterNameTitle.ForceStopEditing();
  }

  private void OnArchetypeEntryClick(IListableOption skill, object data)
  {
    if (skill != null)
    {
      SkillGroup skillGroup = skill as SkillGroup;
      this.guaranteedAptitudeID = skillGroup.Id;
      this.selectedArchetypeIcon.sprite = Assets.GetSprite((HashedString) skillGroup.archetypeIcon);
      this.Reshuffle(true);
    }
    else
    {
      this.guaranteedAptitudeID = (string) null;
      this.selectedArchetypeIcon.sprite = this.dropdownArrowIcon;
      this.Reshuffle(true);
    }
  }

  private int archetypeDropDownSort(IListableOption a, IListableOption b, object targetData)
  {
    if (b.Equals((object) "Random"))
      return -1;
    return b.GetProperName().CompareTo(a.GetProperName());
  }

  private void archetypeDropEntryRefreshAction(DropDownEntry entry, object targetData)
  {
    if (entry.entryData == null)
      return;
    SkillGroup entryData = entry.entryData as SkillGroup;
    entry.image.sprite = Assets.GetSprite((HashedString) entryData.archetypeIcon);
  }

  [Serializable]
  public struct ProfessionIcon
  {
    public string professionName;
    public Sprite iconImg;
  }
}
