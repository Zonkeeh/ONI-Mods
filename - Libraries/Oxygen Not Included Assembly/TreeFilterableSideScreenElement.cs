// Decompiled with JetBrains decompiler
// Type: TreeFilterableSideScreenElement
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class TreeFilterableSideScreenElement : KMonoBehaviour
{
  [SerializeField]
  private LocText elementName;
  [SerializeField]
  private KToggle checkBox;
  [SerializeField]
  private KImage elementImg;
  private KImage checkBoxImg;
  private Tag elementTag;
  private TreeFilterableSideScreen parent;
  private bool initialized;

  public Tag GetElementTag()
  {
    return this.elementTag;
  }

  public bool IsSelected
  {
    get
    {
      return this.checkBox.isOn;
    }
  }

  public event System.Action<Tag, bool> OnSelectionChanged;

  public KToggle GetCheckboxToggle()
  {
    return this.checkBox;
  }

  public TreeFilterableSideScreen Parent
  {
    get
    {
      return this.parent;
    }
    set
    {
      this.parent = value;
    }
  }

  private void Initialize()
  {
    if (this.initialized)
      return;
    this.checkBoxImg = this.checkBox.gameObject.GetComponentInChildrenOnly<KImage>();
    this.checkBox.onClick += new System.Action(this.CheckBoxClicked);
    this.initialized = true;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.Initialize();
  }

  public Sprite GetStorageObjectSprite(Tag t)
  {
    Sprite sprite = (Sprite) null;
    GameObject prefab = Assets.GetPrefab(t);
    if ((UnityEngine.Object) prefab != (UnityEngine.Object) null)
    {
      KBatchedAnimController component = prefab.GetComponent<KBatchedAnimController>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        sprite = Def.GetUISpriteFromMultiObjectAnim(component.AnimFiles[0], "ui", false, string.Empty);
    }
    return sprite;
  }

  public void SetSprite(Tag t)
  {
    Element element = ElementLoader.GetElement(t);
    Sprite sprite = element == null ? this.GetStorageObjectSprite(t) : Def.GetUISpriteFromMultiObjectAnim(element.substance.anim, "ui", false, string.Empty);
    this.elementImg.sprite = sprite;
    this.elementImg.enabled = (UnityEngine.Object) sprite != (UnityEngine.Object) null;
  }

  public void SetTag(Tag newTag)
  {
    this.Initialize();
    this.elementTag = newTag;
    string str = this.elementTag.ProperName();
    if (this.parent.IsStorage)
    {
      float amountInStorage = this.parent.GetAmountInStorage(this.elementTag);
      str = str + ": " + GameUtil.GetFormattedMass(amountInStorage, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}");
    }
    this.elementName.text = str;
  }

  private void CheckBoxClicked()
  {
    this.SetCheckBox(!this.parent.IsTagAllowed(this.GetElementTag()));
  }

  public void SetCheckBox(bool checkBoxState)
  {
    this.checkBox.isOn = checkBoxState;
    this.checkBoxImg.enabled = checkBoxState;
    if (this.OnSelectionChanged == null)
      return;
    this.OnSelectionChanged(this.GetElementTag(), checkBoxState);
  }
}
