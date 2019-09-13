// Decompiled with JetBrains decompiler
// Type: CodexImage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CodexImage : CodexWidget<CodexImage>
{
  public CodexImage()
  {
  }

  public CodexImage(int preferredWidth, int preferredHeight, Sprite sprite, Color color)
    : base(preferredWidth, preferredHeight)
  {
    this.sprite = sprite;
    this.color = color;
  }

  public CodexImage(int preferredWidth, int preferredHeight, Sprite sprite)
    : this(preferredWidth, preferredHeight, sprite, Color.white)
  {
  }

  public CodexImage(int preferredWidth, int preferredHeight, Tuple<Sprite, Color> coloredSprite)
    : this(preferredWidth, preferredHeight, coloredSprite.first, coloredSprite.second)
  {
  }

  public CodexImage(Tuple<Sprite, Color> coloredSprite)
    : this(-1, -1, coloredSprite)
  {
  }

  public Sprite sprite { get; set; }

  public Color color { get; set; }

  public string spriteName
  {
    set
    {
      this.sprite = Assets.GetSprite((HashedString) value);
    }
    get
    {
      return "--> " + (!((Object) this.sprite == (Object) null) ? this.sprite.ToString() : "NULL");
    }
  }

  public string batchedAnimPrefabSourceID
  {
    set
    {
      GameObject prefab = Assets.GetPrefab((Tag) value);
      KBatchedAnimController kbatchedAnimController = !((Object) prefab != (Object) null) ? (KBatchedAnimController) null : prefab.GetComponent<KBatchedAnimController>();
      KAnimFile animFile = !((Object) kbatchedAnimController != (Object) null) ? (KAnimFile) null : kbatchedAnimController.AnimFiles[0];
      this.sprite = !((Object) animFile != (Object) null) ? (Sprite) null : Def.GetUISpriteFromMultiObjectAnim(animFile, "ui", false, string.Empty);
    }
    get
    {
      return "--> " + (!((Object) this.sprite == (Object) null) ? this.sprite.ToString() : "NULL");
    }
  }

  public void ConfigureImage(Image image)
  {
    image.sprite = this.sprite;
    image.color = this.color;
  }

  public override void Configure(
    GameObject contentGameObject,
    Transform displayPane,
    Dictionary<CodexTextStyle, TextStyleSetting> textStyles)
  {
    this.ConfigureImage(contentGameObject.GetComponent<Image>());
    this.ConfigurePreferredLayout(contentGameObject);
  }
}
