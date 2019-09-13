// Decompiled with JetBrains decompiler
// Type: Def
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Def : ScriptableObject
{
  private static Dictionary<Tuple<KAnimFile, string, bool>, Sprite> knownUISprites = new Dictionary<Tuple<KAnimFile, string, bool>, Sprite>();
  public string PrefabID;
  public Tag Tag;

  public virtual void InitDef()
  {
    this.Tag = TagManager.Create(this.PrefabID);
  }

  public virtual string Name
  {
    get
    {
      return (string) null;
    }
  }

  public static Tuple<Sprite, Color> GetUISprite(
    object item,
    string animName = "ui",
    bool centered = false)
  {
    if (item is Substance)
      return Def.GetUISprite((object) ElementLoader.FindElementByHash((item as Substance).elementID), animName, centered);
    if (item is Element)
    {
      if ((item as Element).IsSolid)
        return new Tuple<Sprite, Color>(Def.GetUISpriteFromMultiObjectAnim((item as Element).substance.anim, animName, centered, string.Empty), Color.white);
      if ((item as Element).IsLiquid)
        return new Tuple<Sprite, Color>(Assets.GetSprite((HashedString) "element_liquid"), (Color) (item as Element).substance.uiColour);
      if ((item as Element).IsGas)
        return new Tuple<Sprite, Color>(Assets.GetSprite((HashedString) "element_gas"), (Color) (item as Element).substance.uiColour);
      return new Tuple<Sprite, Color>((Sprite) null, Color.clear);
    }
    if (item is GameObject)
    {
      GameObject go = item as GameObject;
      if (ElementLoader.GetElement(go.PrefabID()) != null)
        return Def.GetUISprite((object) ElementLoader.GetElement(go.PrefabID()), animName, centered);
      CreatureBrain component1 = go.GetComponent<CreatureBrain>();
      if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
        animName = component1.symbolPrefix + "ui";
      SpaceArtifact component2 = go.GetComponent<SpaceArtifact>();
      if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
        animName = component2.GetUIAnim();
      if (go.HasTag(GameTags.Egg))
      {
        IncubationMonitor.Def def = go.GetDef<IncubationMonitor.Def>();
        if (def != null)
        {
          GameObject prefab = Assets.GetPrefab(def.spawnedCreature);
          if ((bool) ((UnityEngine.Object) prefab))
          {
            CreatureBrain component3 = prefab.GetComponent<CreatureBrain>();
            if ((bool) ((UnityEngine.Object) component3) && !string.IsNullOrEmpty(component3.symbolPrefix))
              animName = component3.symbolPrefix + animName;
          }
        }
      }
      KBatchedAnimController component4 = go.GetComponent<KBatchedAnimController>();
      if ((bool) ((UnityEngine.Object) component4))
      {
        Sprite fromMultiObjectAnim = Def.GetUISpriteFromMultiObjectAnim(component4.AnimFiles[0], animName, centered, string.Empty);
        return new Tuple<Sprite, Color>(fromMultiObjectAnim, !((UnityEngine.Object) fromMultiObjectAnim != (UnityEngine.Object) null) ? Color.clear : Color.white);
      }
      if ((UnityEngine.Object) go.GetComponent<Building>() != (UnityEngine.Object) null)
      {
        Sprite uiSprite = go.GetComponent<Building>().Def.GetUISprite(animName, centered);
        return new Tuple<Sprite, Color>(uiSprite, !((UnityEngine.Object) uiSprite != (UnityEngine.Object) null) ? Color.clear : Color.white);
      }
      Debug.LogWarningFormat("Can't get sprite for type {0} (no KBatchedAnimController)", (object) item.ToString());
      return (Tuple<Sprite, Color>) null;
    }
    if (item is string)
    {
      if (Db.Get().Amounts.Exists(item as string))
        return new Tuple<Sprite, Color>(Assets.GetSprite((HashedString) Db.Get().Amounts.Get(item as string).uiSprite), Color.white);
      if (Db.Get().Attributes.Exists(item as string))
        return new Tuple<Sprite, Color>(Assets.GetSprite((HashedString) Db.Get().Attributes.Get(item as string).uiSprite), Color.white);
      return Def.GetUISprite((object) (item as string).ToTag(), animName, centered);
    }
    if (item is Tag)
    {
      if (ElementLoader.GetElement((Tag) item) != null)
        return Def.GetUISprite((object) ElementLoader.GetElement((Tag) item), animName, centered);
      if ((UnityEngine.Object) Assets.GetPrefab((Tag) item) != (UnityEngine.Object) null)
        return Def.GetUISprite((object) Assets.GetPrefab((Tag) item), animName, centered);
      if ((UnityEngine.Object) Assets.GetSprite((HashedString) ((Tag) item).Name) != (UnityEngine.Object) null)
        return new Tuple<Sprite, Color>(Assets.GetSprite((HashedString) ((Tag) item).Name), Color.white);
    }
    DebugUtil.DevAssertArgs(false, (object) "Can't get sprite for type ", (object) item.ToString());
    return (Tuple<Sprite, Color>) null;
  }

  public static Sprite GetUISpriteFromMultiObjectAnim(
    KAnimFile animFile,
    string animName = "ui",
    bool centered = false,
    string symbolName = "")
  {
    Tuple<KAnimFile, string, bool> key = new Tuple<KAnimFile, string, bool>(animFile, animName, centered);
    if (Def.knownUISprites.ContainsKey(key))
      return Def.knownUISprites[key];
    if ((UnityEngine.Object) animFile == (UnityEngine.Object) null)
    {
      DebugUtil.LogWarningArgs((object) animName, (object) "missing Anim File");
      return (Sprite) null;
    }
    KAnimFileData data = animFile.GetData();
    if (data == null)
    {
      DebugUtil.LogWarningArgs((object) animName, (object) "KAnimFileData is null");
      return (Sprite) null;
    }
    if (data.build == null)
      return (Sprite) null;
    KAnim.Anim.Frame frame1 = KAnim.Anim.Frame.InvalidFrame;
    for (int index = 0; index < data.animCount; ++index)
    {
      KAnim.Anim anim = data.GetAnim(index);
      if (anim.name == animName)
        frame1 = anim.GetFrame(data.batchTag, 0);
    }
    if (!frame1.IsValid())
    {
      DebugUtil.LogWarningArgs((object) string.Format("missing '{0}' anim in '{1}'", (object) animName, (object) animFile));
      return (Sprite) null;
    }
    if (data.elementCount == 0)
      return (Sprite) null;
    KAnim.Anim.FrameElement frameElement = new KAnim.Anim.FrameElement();
    if (string.IsNullOrEmpty(symbolName))
      symbolName = animName;
    KAnim.Anim.FrameElement animFrameElement = data.FindAnimFrameElement((KAnimHashedString) symbolName);
    KAnim.Build.Symbol symbol = data.build.GetSymbol(animFrameElement.symbol);
    if (symbol == null)
    {
      DebugUtil.LogWarningArgs((object) animFile.name, (object) animName, (object) "placeSymbol [", (object) animFrameElement.symbol, (object) "] is missing");
      return (Sprite) null;
    }
    int frame2 = animFrameElement.frame;
    KAnim.Build.SymbolFrame symbolFrame = symbol.GetFrame(frame2).symbolFrame;
    if (symbolFrame == null)
    {
      DebugUtil.LogWarningArgs((object) animName, (object) "SymbolFrame [", (object) animFrameElement.frame, (object) "] is missing");
      return (Sprite) null;
    }
    Texture2D texture = data.build.GetTexture(0);
    float x1 = symbolFrame.uvMin.x;
    float x2 = symbolFrame.uvMax.x;
    float y1 = symbolFrame.uvMax.y;
    float y2 = symbolFrame.uvMin.y;
    int num1 = (int) ((double) texture.width * (double) Mathf.Abs(x2 - x1));
    int num2 = (int) ((double) texture.height * (double) Mathf.Abs(y2 - y1));
    float num3 = Mathf.Abs(symbolFrame.bboxMax.x - symbolFrame.bboxMin.x);
    Rect rect = new Rect();
    rect.width = (float) num1;
    rect.height = (float) num2;
    rect.x = (float) (int) ((double) texture.width * (double) x1);
    rect.y = (float) (int) ((double) texture.height * (double) y1);
    float pixelsPerUnit = 100f;
    if (num1 != 0)
      pixelsPerUnit = (float) (100.0 / ((double) num3 / (double) num1));
    Sprite sprite = Sprite.Create(texture, rect, !centered ? Vector2.zero : new Vector2(0.5f, 0.5f), pixelsPerUnit, 0U, SpriteMeshType.FullRect);
    sprite.name = string.Format("{0}:{1}:{2}:{3}", (object) texture.name, (object) animName, (object) animFrameElement.frame.ToString(), (object) centered);
    Def.knownUISprites[key] = sprite;
    return sprite;
  }
}
