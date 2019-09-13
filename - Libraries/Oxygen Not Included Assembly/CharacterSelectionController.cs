// Decompiled with JetBrains decompiler
// Type: CharacterSelectionController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.CustomSettings;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectionController : KModalScreen
{
  protected int numberOfDuplicantOptions = 3;
  [SerializeField]
  private CharacterContainer containerPrefab;
  [SerializeField]
  private CarePackageContainer carePackageContainerPrefab;
  [SerializeField]
  private GameObject containerParent;
  [SerializeField]
  protected KButton proceedButton;
  protected int numberOfCarePackageOptions;
  [SerializeField]
  protected int selectableCount;
  [SerializeField]
  private bool allowsReplacing;
  protected List<ITelepadDeliverable> selectedDeliverables;
  protected List<ITelepadDeliverableContainer> containers;
  public System.Action OnLimitReachedEvent;
  public System.Action OnLimitUnreachedEvent;
  public System.Action<bool> OnReshuffleEvent;
  public System.Action<ITelepadDeliverable> OnReplacedEvent;
  public System.Action OnProceedEvent;

  public bool IsStarterMinion { get; set; }

  public bool AllowsReplacing
  {
    get
    {
      return this.allowsReplacing;
    }
  }

  protected virtual void OnProceed()
  {
  }

  protected virtual void OnDeliverableAdded()
  {
  }

  protected virtual void OnDeliverableRemoved()
  {
  }

  protected virtual void OnLimitReached()
  {
  }

  protected virtual void OnLimitUnreached()
  {
  }

  protected virtual void InitializeContainers()
  {
    this.DisableProceedButton();
    if (this.containers != null && this.containers.Count > 0)
      return;
    this.OnReplacedEvent = (System.Action<ITelepadDeliverable>) null;
    this.containers = new List<ITelepadDeliverableContainer>();
    if (this.IsStarterMinion || CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.CarePackages).id != "Enabled")
    {
      this.numberOfDuplicantOptions = 3;
      this.numberOfCarePackageOptions = 0;
    }
    else
    {
      this.numberOfCarePackageOptions = UnityEngine.Random.Range(0, 101) <= 70 ? 1 : 2;
      this.numberOfDuplicantOptions = 4 - this.numberOfCarePackageOptions;
    }
    for (int index = 0; index < this.numberOfDuplicantOptions; ++index)
    {
      CharacterContainer characterContainer = Util.KInstantiateUI<CharacterContainer>(this.containerPrefab.gameObject, this.containerParent, false);
      characterContainer.SetController(this);
      this.containers.Add((ITelepadDeliverableContainer) characterContainer);
    }
    for (int index = 0; index < this.numberOfCarePackageOptions; ++index)
    {
      CarePackageContainer packageContainer = Util.KInstantiateUI<CarePackageContainer>(this.carePackageContainerPrefab.gameObject, this.containerParent, false);
      packageContainer.SetController(this);
      this.containers.Add((ITelepadDeliverableContainer) packageContainer);
      packageContainer.gameObject.transform.SetSiblingIndex(UnityEngine.Random.Range(0, packageContainer.transform.parent.childCount));
    }
    this.selectedDeliverables = new List<ITelepadDeliverable>();
  }

  public virtual void OnPressBack()
  {
    foreach (ITelepadDeliverableContainer container in this.containers)
    {
      CharacterContainer characterContainer = container as CharacterContainer;
      if ((UnityEngine.Object) characterContainer != (UnityEngine.Object) null)
        characterContainer.ForceStopEditingTitle();
    }
    this.Show(false);
  }

  public void RemoveLast()
  {
    if (this.selectedDeliverables == null || this.selectedDeliverables.Count == 0)
      return;
    ITelepadDeliverable selectedDeliverable = this.selectedDeliverables[this.selectedDeliverables.Count - 1];
    if (this.OnReplacedEvent == null)
      return;
    this.OnReplacedEvent(selectedDeliverable);
  }

  public void AddDeliverable(ITelepadDeliverable deliverable)
  {
    if (this.selectedDeliverables.Contains(deliverable))
      Debug.Log((object) "Tried to add the same minion twice.");
    else if (this.selectedDeliverables.Count >= this.selectableCount)
    {
      Debug.LogError((object) "Tried to add minions beyond the allowed limit");
    }
    else
    {
      this.selectedDeliverables.Add(deliverable);
      this.OnDeliverableAdded();
      if (this.selectedDeliverables.Count != this.selectableCount)
        return;
      this.EnableProceedButton();
      if (this.OnLimitReachedEvent != null)
        this.OnLimitReachedEvent();
      this.OnLimitReached();
    }
  }

  public void RemoveDeliverable(ITelepadDeliverable deliverable)
  {
    bool flag = this.selectedDeliverables.Count >= this.selectableCount;
    this.selectedDeliverables.Remove(deliverable);
    this.OnDeliverableRemoved();
    if (!flag || this.selectedDeliverables.Count >= this.selectableCount)
      return;
    this.DisableProceedButton();
    if (this.OnLimitUnreachedEvent != null)
      this.OnLimitUnreachedEvent();
    this.OnLimitUnreached();
  }

  public bool IsSelected(ITelepadDeliverable deliverable)
  {
    return this.selectedDeliverables.Contains(deliverable);
  }

  protected void EnableProceedButton()
  {
    this.proceedButton.isInteractable = true;
    this.proceedButton.ClearOnClick();
    this.proceedButton.onClick += (System.Action) (() => this.OnProceed());
  }

  protected void DisableProceedButton()
  {
    this.proceedButton.ClearOnClick();
    this.proceedButton.isInteractable = false;
    this.proceedButton.onClick += (System.Action) (() => KMonoBehaviour.PlaySound(GlobalAssets.GetSound("Negative", false)));
  }
}
