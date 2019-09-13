// Decompiled with JetBrains decompiler
// Type: BuildingHP
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[SerializationConfig(MemberSerialization.OptIn)]
public class BuildingHP : Workable
{
  private static readonly EventSystem.IntraObjectHandler<BuildingHP> OnDoBuildingDamageDelegate = new EventSystem.IntraObjectHandler<BuildingHP>((System.Action<BuildingHP, object>) ((component, data) => component.OnDoBuildingDamage(data)));
  private static readonly EventSystem.IntraObjectHandler<BuildingHP> DestroyOnDamagedDelegate = new EventSystem.IntraObjectHandler<BuildingHP>((System.Action<BuildingHP, object>) ((component, data) => component.DestroyOnDamaged(data)));
  public static List<Meter> kbacQueryList = new List<Meter>();
  private float minDamagePopInterval = 4f;
  [Serialize]
  [SerializeField]
  private int hitpoints;
  [Serialize]
  private BuildingHP.DamageSourceInfo damageSourceInfo;
  public bool destroyOnDamaged;
  public bool invincible;
  [MyCmpGet]
  private Building building;
  private BuildingHP.SMInstance smi;
  private float lastPopTime;

  public int HitPoints
  {
    get
    {
      return this.hitpoints;
    }
  }

  public void SetHitPoints(int hp)
  {
    this.hitpoints = hp;
  }

  public int MaxHitPoints
  {
    get
    {
      return this.building.Def.HitPoints;
    }
  }

  public BuildingHP.DamageSourceInfo GetDamageSourceInfo()
  {
    return this.damageSourceInfo;
  }

  protected override void OnLoadLevel()
  {
    this.smi = (BuildingHP.SMInstance) null;
    base.OnLoadLevel();
  }

  public void DoDamage(int damage)
  {
    if (this.invincible)
      return;
    damage = Math.Max(0, damage);
    this.hitpoints = Math.Max(0, this.hitpoints - damage);
    this.Trigger(-1964935036, (object) this);
  }

  public void Repair(int repair_amount)
  {
    this.hitpoints = this.hitpoints + repair_amount >= this.hitpoints ? Math.Min(this.hitpoints + repair_amount, this.building.Def.HitPoints) : this.building.Def.HitPoints;
    this.Trigger(-1699355994, (object) this);
    if (this.hitpoints < this.building.Def.HitPoints)
      return;
    this.Trigger(-1735440190, (object) this);
  }

  protected override void OnPrefabInit()
  {
    base.OnPrefabInit();
    this.SetWorkTime(10f);
    this.multitoolContext = (HashedString) "build";
    this.multitoolHitEffectTag = (Tag) EffectConfigs.BuildSplashId;
  }

  protected override void OnSpawn()
  {
    base.OnSpawn();
    this.smi = new BuildingHP.SMInstance(this);
    this.smi.StartSM();
    this.Subscribe<BuildingHP>(-794517298, BuildingHP.OnDoBuildingDamageDelegate);
    if (this.destroyOnDamaged)
      this.Subscribe<BuildingHP>(774203113, BuildingHP.DestroyOnDamagedDelegate);
    if (this.hitpoints > 0)
      return;
    this.Trigger(774203113, (object) this);
  }

  private void DestroyOnDamaged(object data)
  {
    Util.KDestroyGameObject(this.gameObject);
  }

  protected override void OnCompleteWork(Worker worker)
  {
    this.Repair(10 + Math.Max(0, (int) Db.Get().Attributes.Machinery.Lookup((Component) worker).GetTotalValue() * 10));
  }

  private void OnDoBuildingDamage(object data)
  {
    if (this.invincible)
      return;
    this.damageSourceInfo = (BuildingHP.DamageSourceInfo) data;
    this.DoDamage(this.damageSourceInfo.damage);
    this.DoDamagePopFX(this.damageSourceInfo);
    this.DoTakeDamageFX(this.damageSourceInfo);
  }

  private void DoTakeDamageFX(BuildingHP.DamageSourceInfo info)
  {
    if (info.takeDamageEffect == SpawnFXHashes.None)
      return;
    int cell = Grid.OffsetCell(Grid.PosToCell((KMonoBehaviour) this), 0, this.GetComponent<BuildingComplete>().Def.HeightInCells - 1);
    Game.Instance.SpawnFX(info.takeDamageEffect, cell, 0.0f);
  }

  private void DoDamagePopFX(BuildingHP.DamageSourceInfo info)
  {
    if (info.popString == null || (double) Time.time <= (double) this.lastPopTime + (double) this.minDamagePopInterval)
      return;
    PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Building, info.popString, this.gameObject.transform, 1.5f, false);
    this.lastPopTime = Time.time;
  }

  public bool IsBroken
  {
    get
    {
      return this.hitpoints == 0;
    }
  }

  public bool NeedsRepairs
  {
    get
    {
      return this.HitPoints < this.building.Def.HitPoints;
    }
  }

  public struct DamageSourceInfo
  {
    public int damage;
    public string source;
    public string popString;
    public SpawnFXHashes takeDamageEffect;
    public string fullDamageEffectName;
    public string statusItemID;

    public override string ToString()
    {
      return this.source;
    }
  }

  public class SMInstance : GameStateMachine<BuildingHP.States, BuildingHP.SMInstance, BuildingHP, object>.GameInstance
  {
    private ProgressBar progressBar;

    public SMInstance(BuildingHP master)
      : base(master)
    {
    }

    public Notification CreateBrokenMachineNotification()
    {
      return new Notification((string) MISC.NOTIFICATIONS.BROKENMACHINE.NAME, NotificationType.BadMinor, HashedString.Invalid, (Func<List<Notification>, object, string>) ((notificationList, data) => (string) MISC.NOTIFICATIONS.BROKENMACHINE.TOOLTIP + notificationList.ReduceMessages(false)), (object) ("/t• " + this.master.damageSourceInfo.source), false, 0.0f, (Notification.ClickCallback) null, (object) null, (Transform) null);
    }

    public void ShowProgressBar(bool show)
    {
      if (show && Grid.IsValidCell(Grid.PosToCell(this.gameObject)) && Grid.IsVisible(Grid.PosToCell(this.gameObject)))
      {
        this.CreateProgressBar();
      }
      else
      {
        if (!((UnityEngine.Object) this.progressBar != (UnityEngine.Object) null))
          return;
        this.progressBar.gameObject.DeleteObject();
        this.progressBar = (ProgressBar) null;
      }
    }

    public void UpdateMeter()
    {
      if ((UnityEngine.Object) this.progressBar == (UnityEngine.Object) null)
        this.ShowProgressBar(true);
      if (!(bool) ((UnityEngine.Object) this.progressBar))
        return;
      this.progressBar.Update();
    }

    private float HealthPercent()
    {
      return (float) this.smi.master.HitPoints / (float) this.smi.master.building.Def.HitPoints;
    }

    private void CreateProgressBar()
    {
      if ((UnityEngine.Object) this.progressBar != (UnityEngine.Object) null)
        return;
      this.progressBar = Util.KInstantiateUI<ProgressBar>(ProgressBarsConfig.Instance.progressBarPrefab, (GameObject) null, false);
      this.progressBar.transform.SetParent(GameScreenManager.Instance.worldSpaceCanvas.transform);
      this.progressBar.name = this.smi.master.name + "." + this.smi.master.GetType().Name + " ProgressBar";
      this.progressBar.transform.Find("Bar").GetComponent<Image>().color = ProgressBarsConfig.Instance.GetBarColor("ProgressBar");
      this.progressBar.SetUpdateFunc(new Func<float>(this.HealthPercent));
      this.progressBar.barColor = ProgressBarsConfig.Instance.GetBarColor("HealthBar");
      CanvasGroup component1 = this.progressBar.GetComponent<CanvasGroup>();
      component1.interactable = false;
      component1.blocksRaycasts = false;
      this.progressBar.Update();
      Vector3 vector3 = this.gameObject.transform.GetPosition() + Vector3.down * 0.15f;
      vector3.z += 0.05f;
      Rotatable component2 = this.GetComponent<Rotatable>();
      this.progressBar.transform.SetPosition((UnityEngine.Object) component2 == (UnityEngine.Object) null || component2.GetOrientation() == Orientation.Neutral || (this.smi.master.building.Def.WidthInCells < 2 || this.smi.master.building.Def.HeightInCells < 2) ? vector3 - Vector3.right * 0.5f * (float) (this.smi.master.building.Def.WidthInCells % 2) : vector3 + Vector3.left * (float) (1.0 + 0.5 * (double) (this.smi.master.building.Def.WidthInCells % 2)));
      this.progressBar.gameObject.SetActive(true);
    }

    private static string ToolTipResolver(List<Notification> notificationList, object data)
    {
      string empty = string.Empty;
      for (int index = 0; index < notificationList.Count; ++index)
      {
        Notification notification = notificationList[index];
        empty += string.Format((string) BUILDINGS.DAMAGESOURCES.NOTIFICATION_TOOLTIP, (object) notification.NotifierName, (object) (string) notification.tooltipData);
        if (index < notificationList.Count - 1)
          empty += "\n";
      }
      return empty;
    }

    public void ShowDamagedEffect()
    {
      if (this.master.damageSourceInfo.takeDamageEffect == SpawnFXHashes.None)
        return;
      Game.Instance.SpawnFX(this.master.damageSourceInfo.takeDamageEffect, Grid.OffsetCell(Grid.PosToCell((KMonoBehaviour) this.master), 0, this.master.GetComponent<BuildingComplete>().Def.HeightInCells - 1), 0.0f);
    }

    public FXAnim.Instance InstantiateDamageFX()
    {
      if (this.master.damageSourceInfo.fullDamageEffectName == null)
        return (FXAnim.Instance) null;
      BuildingDef def = this.master.GetComponent<BuildingComplete>().Def;
      Vector3 offset = Vector3.zero;
      offset = def.HeightInCells <= 1 ? new Vector3(0.0f, 0.5f, 0.0f) : new Vector3(0.0f, (float) (def.HeightInCells - 1), 0.0f);
      return new FXAnim.Instance((IStateMachineTarget) this.smi.master, this.master.damageSourceInfo.fullDamageEffectName, "idle", KAnim.PlayMode.Loop, offset, (Color32) Color.white);
    }

    public void SetCrackOverlayValue(float value)
    {
      KBatchedAnimController component = this.master.GetComponent<KBatchedAnimController>();
      if ((UnityEngine.Object) component == (UnityEngine.Object) null)
        return;
      component.SetBlendValue(value);
      BuildingHP.kbacQueryList.Clear();
      this.master.GetComponentsInChildren<Meter>(BuildingHP.kbacQueryList);
      for (int index = 0; index < BuildingHP.kbacQueryList.Count; ++index)
        BuildingHP.kbacQueryList[index].GetComponent<KBatchedAnimController>().SetBlendValue(value);
    }
  }

  public class States : GameStateMachine<BuildingHP.States, BuildingHP.SMInstance, BuildingHP>
  {
    private static readonly Operational.Flag healthyFlag = new Operational.Flag(nameof (healthy), Operational.Flag.Type.Functional);
    public GameStateMachine<BuildingHP.States, BuildingHP.SMInstance, BuildingHP, object>.State damaged;
    public BuildingHP.States.Healthy healthy;

    public override void InitializeStates(out StateMachine.BaseState default_state)
    {
      this.serializable = true;
      default_state = (StateMachine.BaseState) this.healthy;
      this.healthy.DefaultState((GameStateMachine<BuildingHP.States, BuildingHP.SMInstance, BuildingHP, object>.State) this.healthy.imperfect).EventTransition(GameHashes.BuildingReceivedDamage, this.damaged, (StateMachine<BuildingHP.States, BuildingHP.SMInstance, BuildingHP, object>.Transition.ConditionCallback) (smi => smi.master.HitPoints <= 0));
      this.healthy.imperfect.Enter((StateMachine<BuildingHP.States, BuildingHP.SMInstance, BuildingHP, object>.State.Callback) (smi => smi.ShowProgressBar(true))).DefaultState(this.healthy.imperfect.playEffect).EventTransition(GameHashes.BuildingPartiallyRepaired, this.healthy.perfect, (StateMachine<BuildingHP.States, BuildingHP.SMInstance, BuildingHP, object>.Transition.ConditionCallback) (smi => smi.master.HitPoints == smi.master.building.Def.HitPoints)).EventHandler(GameHashes.BuildingPartiallyRepaired, (StateMachine<BuildingHP.States, BuildingHP.SMInstance, BuildingHP, object>.State.Callback) (smi => smi.UpdateMeter())).ToggleStatusItem((Func<BuildingHP.SMInstance, StatusItem>) (smi =>
      {
        if (smi.master.damageSourceInfo.statusItemID != null)
          return Db.Get().BuildingStatusItems.Get(smi.master.damageSourceInfo.statusItemID);
        return (StatusItem) null;
      }), (Func<BuildingHP.SMInstance, object>) null).Exit((StateMachine<BuildingHP.States, BuildingHP.SMInstance, BuildingHP, object>.State.Callback) (smi => smi.ShowProgressBar(false)));
      this.healthy.imperfect.playEffect.Transition(this.healthy.imperfect.waiting, (StateMachine<BuildingHP.States, BuildingHP.SMInstance, BuildingHP, object>.Transition.ConditionCallback) (smi => true), UpdateRate.SIM_200ms);
      this.healthy.imperfect.waiting.ScheduleGoTo((Func<BuildingHP.SMInstance, float>) (smi => UnityEngine.Random.Range(15f, 30f)), (StateMachine.BaseState) this.healthy.imperfect.playEffect);
      this.healthy.perfect.EventTransition(GameHashes.BuildingReceivedDamage, (GameStateMachine<BuildingHP.States, BuildingHP.SMInstance, BuildingHP, object>.State) this.healthy.imperfect, (StateMachine<BuildingHP.States, BuildingHP.SMInstance, BuildingHP, object>.Transition.ConditionCallback) (smi => smi.master.HitPoints < smi.master.building.Def.HitPoints));
      this.damaged.Enter((StateMachine<BuildingHP.States, BuildingHP.SMInstance, BuildingHP, object>.State.Callback) (smi =>
      {
        Operational component = smi.GetComponent<Operational>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
          component.SetFlag(BuildingHP.States.healthyFlag, false);
        smi.ShowProgressBar(true);
        smi.master.Trigger(774203113, (object) smi.master);
        smi.SetCrackOverlayValue(1f);
      })).ToggleNotification((Func<BuildingHP.SMInstance, Notification>) (smi => smi.CreateBrokenMachineNotification())).ToggleStatusItem(Db.Get().BuildingStatusItems.Broken, (object) null).ToggleFX((Func<BuildingHP.SMInstance, StateMachine.Instance>) (smi => (StateMachine.Instance) smi.InstantiateDamageFX())).EventTransition(GameHashes.BuildingPartiallyRepaired, this.healthy.perfect, (StateMachine<BuildingHP.States, BuildingHP.SMInstance, BuildingHP, object>.Transition.ConditionCallback) (smi => smi.master.HitPoints == smi.master.building.Def.HitPoints)).EventHandler(GameHashes.BuildingPartiallyRepaired, (StateMachine<BuildingHP.States, BuildingHP.SMInstance, BuildingHP, object>.State.Callback) (smi => smi.UpdateMeter())).Exit((StateMachine<BuildingHP.States, BuildingHP.SMInstance, BuildingHP, object>.State.Callback) (smi =>
      {
        Operational component = smi.GetComponent<Operational>();
        if ((UnityEngine.Object) component != (UnityEngine.Object) null)
          component.SetFlag(BuildingHP.States.healthyFlag, true);
        smi.ShowProgressBar(false);
        smi.SetCrackOverlayValue(0.0f);
      }));
    }

    private Chore CreateRepairChore(BuildingHP.SMInstance smi)
    {
      return (Chore) new WorkChore<BuildingHP>(Db.Get().ChoreTypes.Repair, (IStateMachineTarget) smi.master, (ChoreProvider) null, true, (System.Action<Chore>) null, (System.Action<Chore>) null, (System.Action<Chore>) null, true, (ScheduleBlockType) null, false, false, (KAnimFile) null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
    }

    public class Healthy : GameStateMachine<BuildingHP.States, BuildingHP.SMInstance, BuildingHP, object>.State
    {
      public BuildingHP.States.ImperfectStates imperfect;
      public GameStateMachine<BuildingHP.States, BuildingHP.SMInstance, BuildingHP, object>.State perfect;
    }

    public class ImperfectStates : GameStateMachine<BuildingHP.States, BuildingHP.SMInstance, BuildingHP, object>.State
    {
      public GameStateMachine<BuildingHP.States, BuildingHP.SMInstance, BuildingHP, object>.State playEffect;
      public GameStateMachine<BuildingHP.States, BuildingHP.SMInstance, BuildingHP, object>.State waiting;
    }
  }
}
