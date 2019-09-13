// Decompiled with JetBrains decompiler
// Type: NameDisplayScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameDisplayScreen : KScreen
{
  public int fontsize_min = 14;
  public int fontsize_max = 32;
  public float cameraDistance_fontsize_min = 6f;
  public float cameraDistance_fontsize_max = 4f;
  public List<NameDisplayScreen.Entry> entries = new List<NameDisplayScreen.Entry>();
  public List<NameDisplayScreen.TextEntry> textEntries = new List<NameDisplayScreen.TextEntry>();
  public bool worldSpace = true;
  private List<KCollider2D> workingList = new List<KCollider2D>();
  [SerializeField]
  private float HideDistance;
  public static NameDisplayScreen Instance;
  public GameObject nameAndBarsPrefab;
  public GameObject barsPrefab;
  public TextStyleSetting ToolTipStyle_Property;
  [SerializeField]
  private Color selectedColor;
  [SerializeField]
  private Color defaultColor;

  public static void DestroyInstance()
  {
    NameDisplayScreen.Instance = (NameDisplayScreen) null;
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    NameDisplayScreen.Instance = this;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    UIRegistry.nameDisplayScreen = this;
    Components.Health.Register(new System.Action<Health>(this.OnHealthAdded), (System.Action<Health>) null);
    Components.Equipment.Register(new System.Action<Equipment>(this.OnEquipmentAdded), (System.Action<Equipment>) null);
  }

  private void OnHealthAdded(Health health)
  {
    this.RegisterComponent(health.gameObject, (object) health, false);
  }

  private void OnEquipmentAdded(Equipment equipment)
  {
    MinionAssignablesProxy component = equipment.GetComponent<MinionAssignablesProxy>();
    GameObject targetGameObject = component.GetTargetGameObject();
    if ((bool) ((UnityEngine.Object) targetGameObject))
      this.RegisterComponent(targetGameObject, (object) equipment, false);
    else
      Debug.LogWarningFormat("OnEquipmentAdded proxy target {0} was null.", (object) component.TargetInstanceID);
  }

  private bool ShouldShowName(GameObject representedObject)
  {
    bool flag1 = (UnityEngine.Object) representedObject.GetComponent<MinionBrain>() != (UnityEngine.Object) null;
    bool flag2 = (UnityEngine.Object) representedObject.GetComponent<CommandModule>() != (UnityEngine.Object) null;
    if (!flag1)
      return flag2;
    return true;
  }

  public Guid AddWorldText(string initialText, GameObject prefab)
  {
    NameDisplayScreen.TextEntry textEntry = new NameDisplayScreen.TextEntry();
    textEntry.guid = Guid.NewGuid();
    textEntry.display_go = Util.KInstantiateUI(prefab, this.gameObject, true);
    textEntry.display_go.GetComponentInChildren<LocText>().text = initialText;
    this.textEntries.Add(textEntry);
    return textEntry.guid;
  }

  public GameObject GetWorldText(Guid guid)
  {
    GameObject gameObject = (GameObject) null;
    foreach (NameDisplayScreen.TextEntry textEntry in this.textEntries)
    {
      if (textEntry.guid == guid)
      {
        gameObject = textEntry.display_go;
        break;
      }
    }
    return gameObject;
  }

  public void RemoveWorldText(Guid guid)
  {
    int index1 = -1;
    for (int index2 = 0; index2 < this.textEntries.Count; ++index2)
    {
      if (this.textEntries[index2].guid == guid)
      {
        index1 = index2;
        break;
      }
    }
    if (index1 < 0)
      return;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.textEntries[index1].display_go);
    this.textEntries.RemoveAt(index1);
  }

  public void AddNewEntry(GameObject representedObject)
  {
    NameDisplayScreen.Entry entry = new NameDisplayScreen.Entry();
    entry.world_go = representedObject;
    GameObject gameObject = Util.KInstantiateUI(!this.ShouldShowName(representedObject) ? this.barsPrefab : this.nameAndBarsPrefab, this.gameObject, true);
    entry.display_go = gameObject;
    if (this.worldSpace)
      entry.display_go.transform.localScale = Vector3.one * 0.01f;
    gameObject.name = representedObject.name + " character overlay";
    entry.Name = representedObject.name;
    entry.refs = gameObject.GetComponent<HierarchyReferences>();
    this.entries.Add(entry);
    KSelectable component1 = representedObject.GetComponent<KSelectable>();
    FactionAlignment component2 = representedObject.GetComponent<FactionAlignment>();
    if (!((UnityEngine.Object) component1 != (UnityEngine.Object) null))
      return;
    if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
    {
      if (component2.Alignment != FactionManager.FactionID.Friendly && component2.Alignment != FactionManager.FactionID.Duplicant)
        return;
      this.UpdateName(representedObject);
    }
    else
      this.UpdateName(representedObject);
  }

  public void RegisterComponent(
    GameObject representedObject,
    object component,
    bool force_new_entry = false)
  {
    NameDisplayScreen.Entry entry = !force_new_entry ? this.GetEntry(representedObject) : (NameDisplayScreen.Entry) null;
    if (entry == null)
    {
      CharacterOverlay component1 = representedObject.GetComponent<CharacterOverlay>();
      if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
      {
        component1.Register();
        entry = this.GetEntry(representedObject);
      }
    }
    if (entry == null)
      return;
    Transform reference = entry.refs.GetReference<Transform>("Bars");
    entry.bars_go = reference.gameObject;
    if (component is Health)
    {
      if (!(bool) ((UnityEngine.Object) entry.healthBar))
      {
        Health health = (Health) component;
        GameObject gameObject = Util.KInstantiateUI(ProgressBarsConfig.Instance.healthBarPrefab, reference.gameObject, false);
        gameObject.name = "Health Bar";
        health.healthBar = gameObject.GetComponent<HealthBar>();
        health.healthBar.GetComponent<KSelectable>().entityName = (string) STRINGS.UI.METERS.HEALTH.TOOLTIP;
        health.healthBar.GetComponent<KSelectableHealthBar>().IsSelectable = (UnityEngine.Object) representedObject.GetComponent<MinionBrain>() != (UnityEngine.Object) null;
        entry.healthBar = health.healthBar;
        entry.healthBar.autoHide = false;
        gameObject.transform.Find("Bar").GetComponent<Image>().color = ProgressBarsConfig.Instance.GetBarColor("HealthBar");
      }
      else
        Debug.LogWarningFormat("Health added twice {0}", component);
    }
    else if (component is OxygenBreather)
    {
      if (!(bool) ((UnityEngine.Object) entry.breathBar))
      {
        GameObject gameObject = Util.KInstantiateUI(ProgressBarsConfig.Instance.progressBarUIPrefab, reference.gameObject, false);
        entry.breathBar = gameObject.GetComponent<ProgressBar>();
        entry.breathBar.autoHide = false;
        gameObject.gameObject.GetComponent<ToolTip>().AddMultiStringTooltip("Breath", (ScriptableObject) this.ToolTipStyle_Property);
        gameObject.name = "Breath Bar";
        gameObject.transform.Find("Bar").GetComponent<Image>().color = ProgressBarsConfig.Instance.GetBarColor("BreathBar");
        gameObject.GetComponent<KSelectable>().entityName = (string) STRINGS.UI.METERS.BREATH.TOOLTIP;
      }
      else
        Debug.LogWarningFormat("OxygenBreather added twice {0}", component);
    }
    else if (component is Equipment)
    {
      if (!(bool) ((UnityEngine.Object) entry.suitBar))
      {
        GameObject gameObject = Util.KInstantiateUI(ProgressBarsConfig.Instance.progressBarUIPrefab, reference.gameObject, false);
        entry.suitBar = gameObject.GetComponent<ProgressBar>();
        entry.suitBar.autoHide = false;
        gameObject.name = "Suit Tank Bar";
        gameObject.transform.Find("Bar").GetComponent<Image>().color = ProgressBarsConfig.Instance.GetBarColor("OxygenTankBar");
        gameObject.GetComponent<KSelectable>().entityName = (string) STRINGS.UI.METERS.BREATH.TOOLTIP;
      }
      else
        Debug.LogWarningFormat("SuitBar added twice {0}", component);
      if (!(bool) ((UnityEngine.Object) entry.suitFuelBar))
      {
        GameObject gameObject = Util.KInstantiateUI(ProgressBarsConfig.Instance.progressBarUIPrefab, reference.gameObject, false);
        entry.suitFuelBar = gameObject.GetComponent<ProgressBar>();
        entry.suitFuelBar.autoHide = false;
        gameObject.name = "Suit Fuel Bar";
        gameObject.transform.Find("Bar").GetComponent<Image>().color = ProgressBarsConfig.Instance.GetBarColor("FuelTankBar");
        gameObject.GetComponent<KSelectable>().entityName = (string) STRINGS.UI.METERS.FUEL.TOOLTIP;
      }
      else
        Debug.LogWarningFormat("FuelBar added twice {0}", component);
    }
    else
    {
      if (!(component is ThoughtGraph.Instance))
        return;
      if (!(bool) ((UnityEngine.Object) entry.thoughtBubble))
      {
        GameObject gameObject1 = Util.KInstantiateUI(EffectPrefabs.Instance.ThoughtBubble, entry.display_go, false);
        entry.thoughtBubble = gameObject1.GetComponent<HierarchyReferences>();
        gameObject1.name = "Thought Bubble";
        GameObject gameObject2 = Util.KInstantiateUI(EffectPrefabs.Instance.ThoughtBubbleConvo, entry.display_go, false);
        entry.thoughtBubbleConvo = gameObject2.GetComponent<HierarchyReferences>();
        gameObject2.name = "Thought Bubble Convo";
      }
      else
        Debug.LogWarningFormat("ThoughtGraph added twice {0}", component);
    }
  }

  private void LateUpdate()
  {
    if (App.isLoading || App.IsExiting)
      return;
    HashedString hashedString = OverlayModes.None.ID;
    if ((UnityEngine.Object) OverlayScreen.Instance != (UnityEngine.Object) null)
      hashedString = OverlayScreen.Instance.GetMode();
    bool flag = !((UnityEngine.Object) Camera.main == (UnityEngine.Object) null) && ((double) Camera.main.orthographicSize < (double) this.HideDistance && hashedString == OverlayModes.None.ID);
    int count = this.entries.Count;
    int index = 0;
    while (index < count)
    {
      if ((UnityEngine.Object) this.entries[index].world_go != (UnityEngine.Object) null)
      {
        Vector3 pos = this.entries[index].world_go.transform.GetPosition();
        if (flag && CameraController.Instance.IsVisiblePos(pos))
        {
          RectTransform component1 = this.entries[index].display_go.GetComponent<RectTransform>();
          if ((UnityEngine.Object) CameraController.Instance != (UnityEngine.Object) null && (UnityEngine.Object) CameraController.Instance.followTarget == (UnityEngine.Object) this.entries[index].world_go.transform)
          {
            pos = CameraController.Instance.followTargetPos;
          }
          else
          {
            KAnimControllerBase component2 = this.entries[index].world_go.GetComponent<KAnimControllerBase>();
            if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
              pos = component2.GetWorldPivot();
          }
          component1.anchoredPosition = (Vector2) (!this.worldSpace ? this.WorldToScreen(pos) : pos);
          this.entries[index].display_go.SetActive(true);
        }
        else if (this.entries[index].display_go.activeSelf)
          this.entries[index].display_go.SetActive(false);
        if (this.entries[index].world_go.HasTag(GameTags.Dead))
          this.entries[index].bars_go.SetActive(false);
        if ((UnityEngine.Object) this.entries[index].bars_go != (UnityEngine.Object) null)
        {
          this.entries[index].bars_go.GetComponentsInChildren<KCollider2D>(false, this.workingList);
          foreach (KCollider2D working in this.workingList)
            working.MarkDirty(false);
        }
        ++index;
      }
      else
      {
        UnityEngine.Object.Destroy((UnityEngine.Object) this.entries[index].display_go);
        --count;
        this.entries[index] = this.entries[count];
      }
    }
    this.entries.RemoveRange(count, this.entries.Count - count);
  }

  public void UpdateName(GameObject representedObject)
  {
    NameDisplayScreen.Entry entry = this.GetEntry(representedObject);
    if (entry == null)
      return;
    KSelectable component = representedObject.GetComponent<KSelectable>();
    entry.display_go.name = component.GetProperName() + " character overlay";
    LocText componentInChildren = entry.display_go.GetComponentInChildren<LocText>();
    if (!((UnityEngine.Object) componentInChildren != (UnityEngine.Object) null))
      return;
    componentInChildren.text = component.GetProperName();
    if (!((UnityEngine.Object) representedObject.GetComponent<RocketModule>() != (UnityEngine.Object) null))
      return;
    componentInChildren.text = representedObject.GetComponent<RocketModule>().GetParentRocketName();
  }

  public void SetThoughtBubbleDisplay(
    GameObject minion_go,
    bool bVisible,
    string hover_text,
    Sprite bubble_sprite,
    Sprite topic_sprite)
  {
    NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
    if (entry == null || (UnityEngine.Object) entry.thoughtBubble == (UnityEngine.Object) null)
      return;
    this.ApplyThoughtSprite(entry.thoughtBubble, bubble_sprite, nameof (bubble_sprite));
    this.ApplyThoughtSprite(entry.thoughtBubble, topic_sprite, "icon_sprite");
    entry.thoughtBubble.GetComponent<KSelectable>().entityName = hover_text;
    entry.thoughtBubble.gameObject.SetActive(bVisible);
  }

  public void SetThoughtBubbleConvoDisplay(
    GameObject minion_go,
    bool bVisible,
    string hover_text,
    Sprite bubble_sprite,
    Sprite topic_sprite,
    Sprite mode_sprite)
  {
    NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
    if (entry == null || (UnityEngine.Object) entry.thoughtBubble == (UnityEngine.Object) null)
      return;
    this.ApplyThoughtSprite(entry.thoughtBubbleConvo, bubble_sprite, nameof (bubble_sprite));
    this.ApplyThoughtSprite(entry.thoughtBubbleConvo, topic_sprite, "icon_sprite");
    this.ApplyThoughtSprite(entry.thoughtBubbleConvo, mode_sprite, "icon_sprite_mode");
    entry.thoughtBubbleConvo.GetComponent<KSelectable>().entityName = hover_text;
    entry.thoughtBubbleConvo.gameObject.SetActive(bVisible);
  }

  private void ApplyThoughtSprite(HierarchyReferences active_bubble, Sprite sprite, string target)
  {
    active_bubble.GetReference<Image>(target).sprite = sprite;
  }

  public void SetBreathDisplay(GameObject minion_go, Func<float> updatePercentFull, bool bVisible)
  {
    NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
    if (entry == null || (UnityEngine.Object) entry.breathBar == (UnityEngine.Object) null)
      return;
    entry.breathBar.SetUpdateFunc(updatePercentFull);
    entry.breathBar.gameObject.SetActive(bVisible);
  }

  public void SetHealthDisplay(GameObject minion_go, Func<float> updatePercentFull, bool bVisible)
  {
    NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
    if (entry == null || (UnityEngine.Object) entry.healthBar == (UnityEngine.Object) null)
      return;
    entry.healthBar.OnChange();
    entry.healthBar.SetUpdateFunc(updatePercentFull);
    if (entry.healthBar.gameObject.activeSelf == bVisible)
      return;
    entry.healthBar.gameObject.SetActive(bVisible);
  }

  public void SetSuitTankDisplay(
    GameObject minion_go,
    Func<float> updatePercentFull,
    bool bVisible)
  {
    NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
    if (entry == null || (UnityEngine.Object) entry.suitBar == (UnityEngine.Object) null)
      return;
    entry.suitBar.SetUpdateFunc(updatePercentFull);
    entry.suitBar.gameObject.SetActive(bVisible);
  }

  public void SetSuitFuelDisplay(
    GameObject minion_go,
    Func<float> updatePercentFull,
    bool bVisible)
  {
    NameDisplayScreen.Entry entry = this.GetEntry(minion_go);
    if (entry == null || (UnityEngine.Object) entry.suitFuelBar == (UnityEngine.Object) null)
      return;
    entry.suitFuelBar.SetUpdateFunc(updatePercentFull);
    entry.suitFuelBar.gameObject.SetActive(bVisible);
  }

  private NameDisplayScreen.Entry GetEntry(GameObject worldObject)
  {
    return this.entries.Find((Predicate<NameDisplayScreen.Entry>) (entry => (UnityEngine.Object) entry.world_go == (UnityEngine.Object) worldObject));
  }

  [Serializable]
  public class Entry
  {
    public string Name;
    public GameObject world_go;
    public GameObject display_go;
    public GameObject bars_go;
    public HealthBar healthBar;
    public ProgressBar breathBar;
    public ProgressBar suitBar;
    public ProgressBar suitFuelBar;
    public HierarchyReferences thoughtBubble;
    public HierarchyReferences thoughtBubbleConvo;
    public HierarchyReferences refs;
  }

  public class TextEntry
  {
    public Guid guid;
    public GameObject display_go;
  }
}
