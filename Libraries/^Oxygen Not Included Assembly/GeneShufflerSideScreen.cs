// Decompiled with JetBrains decompiler
// Type: GeneShufflerSideScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using UnityEngine;

public class GeneShufflerSideScreen : SideScreenContent
{
  [SerializeField]
  private LocText label;
  [SerializeField]
  private KButton button;
  [SerializeField]
  private LocText buttonLabel;
  [SerializeField]
  private GeneShuffler target;
  [SerializeField]
  private GameObject contents;

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.button.onClick += new System.Action(this.OnButtonClick);
    this.Refresh();
  }

  public override bool IsValidForTarget(GameObject target)
  {
    return (UnityEngine.Object) target.GetComponent<GeneShuffler>() != (UnityEngine.Object) null;
  }

  public override void SetTarget(GameObject target)
  {
    GeneShuffler component = target.GetComponent<GeneShuffler>();
    if ((UnityEngine.Object) component == (UnityEngine.Object) null)
    {
      Debug.LogError((object) "Target doesn't have a GeneShuffler associated with it.");
    }
    else
    {
      this.target = component;
      this.Refresh();
    }
  }

  private void OnButtonClick()
  {
    if (this.target.WorkComplete)
    {
      this.target.SetWorkTime(0.0f);
    }
    else
    {
      if (!this.target.IsConsumed)
        return;
      this.target.RequestRecharge(!this.target.RechargeRequested);
      this.Refresh();
    }
  }

  private void Refresh()
  {
    if ((UnityEngine.Object) this.target != (UnityEngine.Object) null)
    {
      if (this.target.WorkComplete)
      {
        this.contents.SetActive(true);
        this.label.text = (string) UI.UISIDESCREENS.GENESHUFFLERSIDESREEN.COMPLETE;
        this.button.gameObject.SetActive(true);
        this.buttonLabel.text = (string) UI.UISIDESCREENS.GENESHUFFLERSIDESREEN.BUTTON;
      }
      else if (this.target.IsConsumed)
      {
        this.contents.SetActive(true);
        this.button.gameObject.SetActive(true);
        if (this.target.RechargeRequested)
        {
          this.label.text = (string) UI.UISIDESCREENS.GENESHUFFLERSIDESREEN.CONSUMED_WAITING;
          this.buttonLabel.text = (string) UI.UISIDESCREENS.GENESHUFFLERSIDESREEN.BUTTON_RECHARGE_CANCEL;
        }
        else
        {
          this.label.text = (string) UI.UISIDESCREENS.GENESHUFFLERSIDESREEN.CONSUMED;
          this.buttonLabel.text = (string) UI.UISIDESCREENS.GENESHUFFLERSIDESREEN.BUTTON_RECHARGE;
        }
      }
      else if (this.target.IsWorking)
      {
        this.contents.SetActive(true);
        this.label.text = (string) UI.UISIDESCREENS.GENESHUFFLERSIDESREEN.UNDERWAY;
        this.button.gameObject.SetActive(false);
      }
      else
        this.contents.SetActive(false);
    }
    else
      this.contents.SetActive(false);
  }
}
