// Decompiled with JetBrains decompiler
// Type: DropDown
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropDown : KMonoBehaviour
{
  public bool addEmptyRow = true;
  private List<IListableOption> entries = new List<IListableOption>();
  public Dictionary<IListableOption, GameObject> rowLookup = new Dictionary<IListableOption, GameObject>();
  private bool displaySelectedValueWhenClosed = true;
  public RectTransform targetDropDownContainer;
  public IListableOption selectedEntry;
  public LocText selectedLabel;
  public KButton openButton;
  public Transform contentContainer;
  public GameObject scrollRect;
  public RectTransform dropdownAlignmentTarget;
  public GameObject rowEntryPrefab;
  public object targetData;
  private System.Action<IListableOption, object> onEntrySelectedAction;
  private System.Action<DropDownEntry, object> rowRefreshAction;
  private Func<IListableOption, IListableOption, object, int> sortFunction;
  private GameObject emptyRow;
  private string emptyRowLabel;
  private Sprite emptyRowSprite;
  private bool built;
  private const int ROWS_BEFORE_SCROLL = 8;
  private KCanvasScaler canvasScaler;

  public bool open { get; private set; }

  public List<IListableOption> Entries
  {
    get
    {
      return this.entries;
    }
  }

  public void Initialize(
    IEnumerable<IListableOption> contentKeys,
    System.Action<IListableOption, object> onEntrySelectedAction,
    Func<IListableOption, IListableOption, object, int> sortFunction = null,
    System.Action<DropDownEntry, object> refreshAction = null,
    bool displaySelectedValueWhenClosed = true,
    object targetData = null)
  {
    this.targetData = targetData;
    this.sortFunction = sortFunction;
    this.onEntrySelectedAction = onEntrySelectedAction;
    this.displaySelectedValueWhenClosed = displaySelectedValueWhenClosed;
    this.rowRefreshAction = refreshAction;
    this.ChangeContent(contentKeys);
    this.openButton.ClearOnClick();
    this.openButton.onClick += (System.Action) (() => this.OnClick());
    this.canvasScaler = GameScreenManager.Instance.ssOverlayCanvas.GetComponent<KCanvasScaler>();
  }

  public void CustomizeEmptyRow(string txt, Sprite icon)
  {
    this.emptyRowLabel = txt;
    this.emptyRowSprite = icon;
  }

  public void OnClick()
  {
    if (!this.open)
      this.Open();
    else
      this.Close();
  }

  public void ChangeContent(IEnumerable<IListableOption> contentKeys)
  {
    this.entries.Clear();
    foreach (IListableOption contentKey in contentKeys)
      this.entries.Add(contentKey);
    this.built = false;
  }

  private void Update()
  {
    if (!this.open || !Input.GetMouseButtonDown(0) && (double) Input.GetAxis("Mouse ScrollWheel") == 0.0)
      return;
    float canvasScale = this.canvasScaler.GetCanvasScale();
    if ((double) this.scrollRect.rectTransform().GetPosition().x + (double) this.scrollRect.rectTransform().sizeDelta.x * (double) canvasScale >= (double) KInputManager.GetMousePos().x && (double) this.scrollRect.rectTransform().GetPosition().x <= (double) KInputManager.GetMousePos().x && ((double) this.scrollRect.rectTransform().GetPosition().y - (double) this.scrollRect.rectTransform().sizeDelta.y * (double) canvasScale <= (double) KInputManager.GetMousePos().y && (double) this.scrollRect.rectTransform().GetPosition().y >= (double) KInputManager.GetMousePos().y))
      return;
    this.Close();
  }

  private void Build(List<IListableOption> contentKeys)
  {
    this.built = true;
    for (int index = this.contentContainer.childCount - 1; index >= 0; --index)
      Util.KDestroyGameObject((Component) this.contentContainer.GetChild(index));
    this.rowLookup.Clear();
    if (this.addEmptyRow)
    {
      this.emptyRow = Util.KInstantiateUI(this.rowEntryPrefab, this.contentContainer.gameObject, true);
      this.emptyRow.GetComponent<KButton>().onClick += (System.Action) (() =>
      {
        this.onEntrySelectedAction((IListableOption) null, this.targetData);
        this.Close();
      });
      this.emptyRow.GetComponent<DropDownEntry>().label.text = this.emptyRowLabel == null ? (string) STRINGS.UI.DROPDOWN.NONE : this.emptyRowLabel;
      if ((UnityEngine.Object) this.emptyRowSprite != (UnityEngine.Object) null)
        this.emptyRow.GetComponent<DropDownEntry>().image.sprite = this.emptyRowSprite;
    }
    for (int index = 0; index < contentKeys.Count; ++index)
    {
      GameObject gameObject = Util.KInstantiateUI(this.rowEntryPrefab, this.contentContainer.gameObject, true);
      IListableOption id = contentKeys[index];
      gameObject.GetComponent<DropDownEntry>().entryData = (object) id;
      gameObject.GetComponent<KButton>().onClick += (System.Action) (() =>
      {
        this.onEntrySelectedAction(id, this.targetData);
        if (this.displaySelectedValueWhenClosed)
          this.selectedLabel.text = id.GetProperName();
        this.Close();
      });
      this.rowLookup.Add(id, gameObject);
    }
    this.RefreshEntries();
    this.Close();
    this.scrollRect.gameObject.transform.SetParent(this.targetDropDownContainer.transform);
    this.scrollRect.gameObject.SetActive(false);
  }

  private void RefreshEntries()
  {
    foreach (KeyValuePair<IListableOption, GameObject> keyValuePair in this.rowLookup)
    {
      DropDownEntry component = keyValuePair.Value.GetComponent<DropDownEntry>();
      component.label.text = keyValuePair.Key.GetProperName();
      if ((UnityEngine.Object) component.portrait != (UnityEngine.Object) null && keyValuePair.Key is IAssignableIdentity)
        component.portrait.SetIdentityObject(keyValuePair.Key as IAssignableIdentity, true);
    }
    if (this.sortFunction != null)
    {
      this.entries.Sort((Comparison<IListableOption>) ((a, b) => this.sortFunction(a, b, this.targetData)));
      for (int index = 0; index < this.entries.Count; ++index)
        this.rowLookup[this.entries[index]].transform.SetAsFirstSibling();
      if ((UnityEngine.Object) this.emptyRow != (UnityEngine.Object) null)
        this.emptyRow.transform.SetAsFirstSibling();
    }
    foreach (KeyValuePair<IListableOption, GameObject> keyValuePair in this.rowLookup)
      this.rowRefreshAction(keyValuePair.Value.GetComponent<DropDownEntry>(), this.targetData);
    if (!((UnityEngine.Object) this.emptyRow != (UnityEngine.Object) null))
      return;
    this.rowRefreshAction(this.emptyRow.GetComponent<DropDownEntry>(), this.targetData);
  }

  protected override void OnCleanUp()
  {
    Util.KDestroyGameObject(this.scrollRect);
    base.OnCleanUp();
  }

  public void Open()
  {
    if (!this.built)
      this.Build(this.entries);
    else
      this.RefreshEntries();
    this.open = true;
    this.scrollRect.gameObject.SetActive(true);
    this.scrollRect.rectTransform().localScale = Vector3.one;
    foreach (KeyValuePair<IListableOption, GameObject> keyValuePair in this.rowLookup)
      keyValuePair.Value.SetActive(true);
    this.scrollRect.rectTransform().sizeDelta = new Vector2(this.scrollRect.rectTransform().sizeDelta.x, 32f * (float) Mathf.Min(this.contentContainer.childCount, 8));
    Vector3 position = this.dropdownAlignmentTarget.TransformPoint(this.dropdownAlignmentTarget.rect.x, this.dropdownAlignmentTarget.rect.y, 0.0f);
    Vector2 vector2 = new Vector2(Mathf.Min(0.0f, (float) Screen.width - (position.x + this.rowEntryPrefab.GetComponent<LayoutElement>().minWidth)), -Mathf.Min(0.0f, position.y - this.scrollRect.rectTransform().sizeDelta.y));
    position += (Vector3) vector2;
    this.scrollRect.rectTransform().SetPosition(position);
  }

  public void Close()
  {
    this.open = false;
    foreach (KeyValuePair<IListableOption, GameObject> keyValuePair in this.rowLookup)
      keyValuePair.Value.SetActive(false);
    this.scrollRect.SetActive(false);
  }
}
