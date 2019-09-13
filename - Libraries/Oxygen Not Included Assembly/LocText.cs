// Decompiled with JetBrains decompiler
// Type: LocText
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class LocText : TextMeshProUGUI
{
  public string key;
  public TextStyleSetting textStyleSetting;
  public bool allowOverride;
  public bool staticLayout;
  private TextLinkHandler textLinkHandler;
  [SerializeField]
  private bool allowLinksInternal;
  private static string[] splits;

  protected override void OnEnable()
  {
    base.OnEnable();
  }

  public bool AllowLinks
  {
    get
    {
      return this.allowLinksInternal;
    }
    set
    {
      this.allowLinksInternal = value;
      this.RefreshLinkHandler();
      this.raycastTarget = this.raycastTarget || this.allowLinksInternal;
    }
  }

  [ContextMenu("Apply Settings")]
  public void ApplySettings()
  {
    if (this.key != string.Empty && Application.isPlaying)
      this.text = (string) Strings.Get(new StringKey(this.key));
    if (!((Object) this.textStyleSetting != (Object) null))
      return;
    SetTextStyleSetting.ApplyStyle((TextMeshProUGUI) this, this.textStyleSetting);
  }

  private new void Awake()
  {
    base.Awake();
    if (!Application.isPlaying)
      return;
    if (this.key != string.Empty)
      this.text = Strings.Get(new StringKey(this.key)).String;
    this.text = Localization.Fixup(this.text);
    this.isRightToLeftText = Localization.IsRightToLeft;
    SetTextStyleSetting textStyleSetting = this.gameObject.GetComponent<SetTextStyleSetting>();
    if ((Object) textStyleSetting == (Object) null)
      textStyleSetting = this.gameObject.AddComponent<SetTextStyleSetting>();
    if (!this.allowOverride)
      textStyleSetting.SetStyle(this.textStyleSetting);
    this.textLinkHandler = this.GetComponent<TextLinkHandler>();
  }

  private new void Start()
  {
    base.Start();
    this.RefreshLinkHandler();
  }

  public override void SetLayoutDirty()
  {
    if (this.staticLayout)
      return;
    base.SetLayoutDirty();
  }

  public override string text
  {
    get
    {
      return base.text;
    }
    set
    {
      base.text = this.FilterInput(value);
    }
  }

  public override void SetText(string text)
  {
    text = this.FilterInput(text);
    base.SetText(text);
  }

  private string FilterInput(string input)
  {
    if (this.AllowLinks)
      return LocText.ModifyLinkStrings(input);
    return input;
  }

  protected override void GenerateTextMesh()
  {
    base.GenerateTextMesh();
  }

  internal void SwapFont(TMP_FontAsset font, bool isRightToLeft)
  {
    this.font = font;
    if (this.key != string.Empty)
      this.text = Strings.Get(new StringKey(this.key)).String;
    this.text = Localization.Fixup(this.text);
    this.isRightToLeftText = isRightToLeft;
  }

  private static string ModifyLinkStrings(string input)
  {
    string pattern1 = "<link=\"";
    string pattern2 = "</link>";
    string str1 = "<b><style=\"KLink\">";
    string str2 = "</style></b>";
    string pattern3 = str1 + pattern1;
    if (input == null || Regex.Split(input, pattern3).Length > 1)
      return input;
    LocText.splits = Regex.Split(input, pattern1);
    if (LocText.splits.Length > 1)
    {
      for (int index = 1; index < LocText.splits.Length; ++index)
      {
        if (!(LocText.splits[index] == string.Empty))
        {
          int num = input.IndexOf(LocText.splits[index]);
          input = input.Insert(num - pattern1.Length, str1);
        }
      }
    }
    LocText.splits = Regex.Split(input, pattern2);
    if (LocText.splits.Length > 1)
    {
      for (int index = 0; index < LocText.splits.Length; ++index)
      {
        if (!(LocText.splits[index] == string.Empty))
        {
          int startIndex = input.IndexOf(LocText.splits[index]);
          if (startIndex != 0)
            input = input.Insert(startIndex, str2);
        }
      }
    }
    return input;
  }

  private void RefreshLinkHandler()
  {
    if ((Object) this.textLinkHandler == (Object) null && this.allowLinksInternal)
    {
      this.textLinkHandler = this.GetComponent<TextLinkHandler>();
      if ((Object) this.textLinkHandler == (Object) null)
        this.textLinkHandler = this.gameObject.AddComponent<TextLinkHandler>();
    }
    else if (!this.allowLinksInternal && (Object) this.textLinkHandler != (Object) null)
    {
      Object.Destroy((Object) this.textLinkHandler);
      this.textLinkHandler = (TextLinkHandler) null;
    }
    if (!((Object) this.textLinkHandler != (Object) null))
      return;
    this.textLinkHandler.CheckMouseOver();
  }
}
