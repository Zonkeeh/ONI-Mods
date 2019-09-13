// Decompiled with JetBrains decompiler
// Type: CreditsScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;

public class CreditsScreen : KModalScreen
{
  public GameObject entryPrefab;
  public Transform entryContainer;
  public KButton CloseButton;
  public TextAsset TeamCreditsFile;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.AddCredits(this.TeamCreditsFile);
    foreach (string str in LocString.GetStrings(typeof (UI.CREDITSSCREEN.THIRD_PARTY)))
      Util.KInstantiateUI(this.entryPrefab, this.entryContainer.gameObject, true).GetComponent<LocText>().text = str;
    this.CloseButton.onClick += new System.Action(this.Close);
  }

  public void Close()
  {
    this.Deactivate();
  }

  private void AddCredits(TextAsset csv)
  {
    string[,] strArray = CSVReader.SplitCsvGrid(csv.text, csv.name);
    List<string> list = new List<string>();
    for (int index = 0; index < strArray.GetLength(1); ++index)
    {
      string str = string.Format("{0} {1}", (object) strArray[0, index], (object) strArray[1, index]);
      if (!(str == " "))
        list.Add(str);
    }
    list.Shuffle<string>();
    foreach (string str in list)
      Util.KInstantiateUI(this.entryPrefab, this.entryContainer.gameObject, true).GetComponent<LocText>().text = str;
  }
}
