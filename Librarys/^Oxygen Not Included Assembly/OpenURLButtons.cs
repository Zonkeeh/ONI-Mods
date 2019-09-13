// Decompiled with JetBrains decompiler
// Type: OpenURLButtons
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class OpenURLButtons : KMonoBehaviour
{
  public GameObject buttonPrefab;
  public List<OpenURLButtons.URLButtonData> buttonData;
  [SerializeField]
  private GameObject patchNotesScreen;

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    for (int index = 0; index < this.buttonData.Count; ++index)
    {
      OpenURLButtons.URLButtonData data = this.buttonData[index];
      GameObject gameObject = Util.KInstantiateUI(this.buttonPrefab, this.gameObject, true);
      string text = (string) Strings.Get(data.stringKey);
      gameObject.GetComponentInChildren<LocText>().SetText(text);
      switch (data.urlType)
      {
        case OpenURLButtons.URLButtonType.url:
          gameObject.GetComponent<KButton>().onClick += (System.Action) (() => this.OpenURL(data.url));
          break;
        case OpenURLButtons.URLButtonType.patchNotes:
          gameObject.GetComponent<KButton>().onClick += (System.Action) (() => this.OpenPatchNotes());
          break;
      }
    }
  }

  public void OpenPatchNotes()
  {
    this.patchNotesScreen.SetActive(true);
  }

  public void OpenURL(string URL)
  {
    Application.OpenURL(URL);
  }

  public enum URLButtonType
  {
    url,
    patchNotes,
  }

  [Serializable]
  public class URLButtonData
  {
    public string stringKey;
    public OpenURLButtons.URLButtonType urlType;
    public string url;
  }
}
