// Decompiled with JetBrains decompiler
// Type: MinionIdentity
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using Klei.AI;
using Klei.CustomSettings;
using KSerialization;
using STRINGS;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

[SerializationConfig(MemberSerialization.OptIn)]
public class MinionIdentity : KMonoBehaviour, ISaveLoadable, IAssignableIdentity, IListableOption, ISim1000ms
{
  private static readonly EventSystem.IntraObjectHandler<MinionIdentity> OnDiedDelegate = new EventSystem.IntraObjectHandler<MinionIdentity>((System.Action<MinionIdentity, object>) ((component, data) => component.OnDied(data)));
  public bool addToIdentityList = true;
  [MyCmpReq]
  private KSelectable selectable;
  public int femaleVoiceCount;
  public int maleVoiceCount;
  [Serialize]
  private string name;
  [Serialize]
  public string gender;
  [Serialize]
  [ReadOnly]
  public float arrivalTime;
  [Serialize]
  public int voiceIdx;
  [Serialize]
  public KCompBuilder.BodyData bodyData;
  [Serialize]
  public Ref<MinionAssignablesProxy> assignableProxy;
  private Navigator navigator;
  private ChoreDriver choreDriver;
  public float timeLastSpoke;
  private string voiceId;
  private KAnimHashedString overrideExpression;
  private KAnimHashedString expression;
  private static MinionIdentity.NameList maleNameList;
  private static MinionIdentity.NameList femaleNameList;

  [Serialize]
  public string genderStringKey { get; set; }

  [Serialize]
  public string nameStringKey { get; set; }

  public static void DestroyStatics()
  {
    MinionIdentity.maleNameList = (MinionIdentity.NameList) null;
    MinionIdentity.femaleNameList = (MinionIdentity.NameList) null;
  }

  protected override void OnPrefabInit()
  {
    if (this.name == null)
      this.name = MinionIdentity.ChooseRandomName();
    if ((UnityEngine.Object) GameClock.Instance != (UnityEngine.Object) null)
      this.arrivalTime = (float) GameClock.Instance.GetCycle();
    KAnimControllerBase component = this.GetComponent<KAnimControllerBase>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      component.OnUpdateBounds += new System.Action<Bounds>(this.OnUpdateBounds);
    this.Subscribe<MinionIdentity>(1623392196, MinionIdentity.OnDiedDelegate);
  }

  protected override void OnSpawn()
  {
    if (this.addToIdentityList)
    {
      this.ValidateProxy();
      this.CleanupLimboMinions();
    }
    PathProber component1 = this.GetComponent<PathProber>();
    if ((UnityEngine.Object) component1 != (UnityEngine.Object) null)
      component1.SetGroupProber((IGroupProber) MinionGroupProber.Get());
    this.SetName(this.name);
    if (this.nameStringKey == null)
      this.nameStringKey = this.name;
    this.SetGender(this.gender);
    if (this.genderStringKey == null)
      this.genderStringKey = "NB";
    if (this.addToIdentityList)
    {
      Components.MinionIdentities.Add(this);
      if (!this.gameObject.HasTag(GameTags.Dead))
        Components.LiveMinionIdentities.Add(this);
    }
    SymbolOverrideController component2 = this.GetComponent<SymbolOverrideController>();
    if ((UnityEngine.Object) component2 != (UnityEngine.Object) null)
    {
      Accessorizer component3 = this.gameObject.GetComponent<Accessorizer>();
      if ((UnityEngine.Object) component3 != (UnityEngine.Object) null)
      {
        this.bodyData = new KCompBuilder.BodyData();
        component3.GetBodySlots(ref this.bodyData);
        string str = HashCache.Get().Get(component3.GetAccessory(Db.Get().AccessorySlots.HeadShape).symbol.hash).Replace("headshape", "cheek");
        component2.AddSymbolOverride((HashedString) "snapto_cheek", Assets.GetAnim((HashedString) "head_swap_kanim").GetData().build.GetSymbol((KAnimHashedString) str), 1);
        component2.AddSymbolOverride((HashedString) Db.Get().AccessorySlots.HairAlways.targetSymbolId, component3.GetAccessory(Db.Get().AccessorySlots.Hair).symbol, 1);
        component2.AddSymbolOverride((HashedString) Db.Get().AccessorySlots.HatHair.targetSymbolId, Db.Get().AccessorySlots.HatHair.Lookup("hat_" + HashCache.Get().Get(component3.GetAccessory(Db.Get().AccessorySlots.Hair).symbol.hash)).symbol, 1);
      }
    }
    this.voiceId = "0";
    this.voiceId += (this.voiceIdx + 1).ToString();
    Prioritizable component4 = this.GetComponent<Prioritizable>();
    if ((UnityEngine.Object) component4 != (UnityEngine.Object) null)
      component4.showIcon = false;
    Pickupable component5 = this.GetComponent<Pickupable>();
    if ((UnityEngine.Object) component5 != (UnityEngine.Object) null)
      component5.carryAnimOverride = Assets.GetAnim((HashedString) "anim_incapacitated_carrier_kanim");
    this.ApplyCustomGameSettings();
  }

  public void ValidateProxy()
  {
    this.assignableProxy = MinionAssignablesProxy.InitAssignableProxy(this.assignableProxy, (IAssignableIdentity) this);
  }

  private void CleanupLimboMinions()
  {
    KPrefabID component = this.GetComponent<KPrefabID>();
    if (component.InstanceID == -1)
    {
      DebugUtil.LogWarningArgs((object) "Minion with an invalid kpid! Attempting to recover...", (object) this.name);
      if ((UnityEngine.Object) KPrefabIDTracker.Get().GetInstance(component.InstanceID) != (UnityEngine.Object) null)
        KPrefabIDTracker.Get().Unregister(component);
      component.InstanceID = KPrefabID.GetUniqueID();
      KPrefabIDTracker.Get().Register(component);
      DebugUtil.LogWarningArgs((object) "Restored as:", (object) component.InstanceID);
    }
    if (component.conflicted)
    {
      DebugUtil.LogWarningArgs((object) "Minion with a conflicted kpid! Attempting to recover... ", (object) component.InstanceID, (object) this.name);
      if ((UnityEngine.Object) KPrefabIDTracker.Get().GetInstance(component.InstanceID) != (UnityEngine.Object) null)
        KPrefabIDTracker.Get().Unregister(component);
      component.InstanceID = KPrefabID.GetUniqueID();
      KPrefabIDTracker.Get().Register(component);
      DebugUtil.LogWarningArgs((object) "Restored as:", (object) component.InstanceID);
    }
    this.assignableProxy.Get().SetTarget((IAssignableIdentity) this, this.gameObject);
  }

  public string GetProperName()
  {
    return this.gameObject.GetProperName();
  }

  public string GetVoiceId()
  {
    return this.voiceId;
  }

  public void SetName(string name)
  {
    this.name = name;
    if ((UnityEngine.Object) this.selectable != (UnityEngine.Object) null)
      this.selectable.SetName(name);
    this.gameObject.name = name;
    NameDisplayScreen.Instance.UpdateName(this.gameObject);
  }

  public bool IsNull()
  {
    return (UnityEngine.Object) this == (UnityEngine.Object) null;
  }

  public void SetGender(string gender)
  {
    this.gender = gender;
    this.selectable.SetGender(gender);
  }

  public static string ChooseRandomName()
  {
    if (MinionIdentity.femaleNameList == null)
    {
      MinionIdentity.maleNameList = new MinionIdentity.NameList(Game.Instance.maleNamesFile);
      MinionIdentity.femaleNameList = new MinionIdentity.NameList(Game.Instance.femaleNamesFile);
    }
    if ((double) UnityEngine.Random.value > 0.5)
      return MinionIdentity.maleNameList.Next();
    return MinionIdentity.femaleNameList.Next();
  }

  protected override void OnCleanUp()
  {
    if (this.assignableProxy != null)
    {
      MinionAssignablesProxy assignablesProxy = this.assignableProxy.Get();
      if ((bool) ((UnityEngine.Object) assignablesProxy) && assignablesProxy.target == this)
        Util.KDestroyGameObject(assignablesProxy.gameObject);
    }
    Components.MinionIdentities.Remove(this);
    Components.LiveMinionIdentities.Remove(this);
  }

  private void OnUpdateBounds(Bounds bounds)
  {
    KBoxCollider2D component = this.GetComponent<KBoxCollider2D>();
    component.offset = (Vector2) bounds.center;
    component.size = (Vector2) bounds.extents;
  }

  private void OnDied(object data)
  {
    this.GetSoleOwner().UnassignAll();
    this.GetEquipment().UnequipAll();
    Components.LiveMinionIdentities.Remove(this);
  }

  public List<Ownables> GetOwners()
  {
    return this.assignableProxy.Get().ownables;
  }

  public Ownables GetSoleOwner()
  {
    return this.assignableProxy.Get().GetComponent<Ownables>();
  }

  public Equipment GetEquipment()
  {
    return this.assignableProxy.Get().GetComponent<Equipment>();
  }

  public void Sim1000ms(float dt)
  {
    if ((UnityEngine.Object) this == (UnityEngine.Object) null)
      return;
    if ((UnityEngine.Object) this.navigator == (UnityEngine.Object) null)
      this.navigator = this.GetComponent<Navigator>();
    if ((UnityEngine.Object) this.navigator != (UnityEngine.Object) null && !this.navigator.IsMoving())
      return;
    if ((UnityEngine.Object) this.choreDriver == (UnityEngine.Object) null)
      this.choreDriver = this.GetComponent<ChoreDriver>();
    if (!((UnityEngine.Object) this.choreDriver != (UnityEngine.Object) null))
      return;
    switch (this.choreDriver.GetCurrentChore())
    {
      case FetchAreaChore _:
        MinionResume component = this.GetComponent<MinionResume>();
        if (!((UnityEngine.Object) component != (UnityEngine.Object) null))
          break;
        component.AddExperienceWithAptitude(Db.Get().SkillGroups.Hauling.Id, dt, SKILLS.ALL_DAY_EXPERIENCE);
        break;
    }
  }

  private void ApplyCustomGameSettings()
  {
    SettingLevel currentQualitySetting1 = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.ImmuneSystem);
    if (currentQualitySetting1.id == "Compromised")
    {
      Db.Get().Attributes.DiseaseCureSpeed.Lookup((Component) this).Add(new AttributeModifier(Db.Get().Attributes.DiseaseCureSpeed.Id, -0.3333f, (string) UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.IMMUNESYSTEM.LEVELS.COMPROMISED.ATTRIBUTE_MODIFIER_NAME, false, false, true));
      Db.Get().Attributes.GermResistance.Lookup((Component) this).Add(new AttributeModifier(Db.Get().Attributes.GermResistance.Id, -2f, (string) UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.IMMUNESYSTEM.LEVELS.COMPROMISED.ATTRIBUTE_MODIFIER_NAME, false, false, true));
    }
    else if (currentQualitySetting1.id == "Weak")
      Db.Get().Attributes.GermResistance.Lookup((Component) this).Add(new AttributeModifier(Db.Get().Attributes.GermResistance.Id, -1f, (string) UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.IMMUNESYSTEM.LEVELS.WEAK.ATTRIBUTE_MODIFIER_NAME, false, false, true));
    else if (currentQualitySetting1.id == "Strong")
    {
      Db.Get().Attributes.DiseaseCureSpeed.Lookup((Component) this).Add(new AttributeModifier(Db.Get().Attributes.DiseaseCureSpeed.Id, 2f, (string) UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.IMMUNESYSTEM.LEVELS.STRONG.ATTRIBUTE_MODIFIER_NAME, false, false, true));
      Db.Get().Attributes.GermResistance.Lookup((Component) this).Add(new AttributeModifier(Db.Get().Attributes.GermResistance.Id, 2f, (string) UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.IMMUNESYSTEM.LEVELS.STRONG.ATTRIBUTE_MODIFIER_NAME, false, false, true));
    }
    else if (currentQualitySetting1.id == "Invincible")
    {
      Db.Get().Attributes.DiseaseCureSpeed.Lookup((Component) this).Add(new AttributeModifier(Db.Get().Attributes.DiseaseCureSpeed.Id, 1E+08f, (string) UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.IMMUNESYSTEM.LEVELS.INVINCIBLE.ATTRIBUTE_MODIFIER_NAME, false, false, true));
      Db.Get().Attributes.GermResistance.Lookup((Component) this).Add(new AttributeModifier(Db.Get().Attributes.GermResistance.Id, 200f, (string) UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.IMMUNESYSTEM.LEVELS.INVINCIBLE.ATTRIBUTE_MODIFIER_NAME, false, false, true));
    }
    SettingLevel currentQualitySetting2 = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.Stress);
    if (currentQualitySetting2.id == "Doomed")
      Db.Get().Amounts.Stress.deltaAttribute.Lookup((Component) this).Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, 0.03333334f, (string) UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.STRESS.LEVELS.DOOMED.ATTRIBUTE_MODIFIER_NAME, false, false, true));
    else if (currentQualitySetting2.id == "Pessimistic")
      Db.Get().Amounts.Stress.deltaAttribute.Lookup((Component) this).Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, 0.01666667f, (string) UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.STRESS.LEVELS.PESSIMISTIC.ATTRIBUTE_MODIFIER_NAME, false, false, true));
    else if (currentQualitySetting2.id == "Optimistic")
      Db.Get().Amounts.Stress.deltaAttribute.Lookup((Component) this).Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, -0.01666667f, (string) UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.STRESS.LEVELS.OPTIMISTIC.ATTRIBUTE_MODIFIER_NAME, false, false, true));
    else if (currentQualitySetting2.id == "Indomitable")
      Db.Get().Amounts.Stress.deltaAttribute.Lookup((Component) this).Add(new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, float.NegativeInfinity, (string) UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.STRESS.LEVELS.INDOMITABLE.ATTRIBUTE_MODIFIER_NAME, false, false, true));
    SettingLevel currentQualitySetting3 = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.CalorieBurn);
    if (currentQualitySetting3.id == "VeryHard")
      Db.Get().Amounts.Calories.deltaAttribute.Lookup((Component) this).Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -1666.667f, (string) UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.CALORIE_BURN.LEVELS.VERYHARD.ATTRIBUTE_MODIFIER_NAME, false, false, true));
    else if (currentQualitySetting3.id == "Hard")
      Db.Get().Amounts.Calories.deltaAttribute.Lookup((Component) this).Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, -833.3333f, (string) UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.CALORIE_BURN.LEVELS.HARD.ATTRIBUTE_MODIFIER_NAME, false, false, true));
    else if (currentQualitySetting3.id == "Easy")
    {
      Db.Get().Amounts.Calories.deltaAttribute.Lookup((Component) this).Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, 833.3333f, (string) UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.CALORIE_BURN.LEVELS.EASY.ATTRIBUTE_MODIFIER_NAME, false, false, true));
    }
    else
    {
      if (!(currentQualitySetting3.id == "Disabled"))
        return;
      Db.Get().Amounts.Calories.deltaAttribute.Lookup((Component) this).Add(new AttributeModifier(Db.Get().Amounts.Calories.deltaAttribute.Id, float.PositiveInfinity, (string) UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.CALORIE_BURN.LEVELS.DISABLED.ATTRIBUTE_MODIFIER_NAME, false, false, true));
    }
  }

  private class NameList
  {
    private List<string> names = new List<string>();
    private int idx;

    public NameList(TextAsset file)
    {
      string str1 = file.text.Replace("  ", " ").Replace("\r\n", "\n");
      char[] chArray1 = new char[1]{ '\n' };
      foreach (string str2 in str1.Split(chArray1))
      {
        char[] chArray2 = new char[1]{ ' ' };
        string[] strArray = str2.Split(chArray2);
        if (strArray[strArray.Length - 1] != string.Empty && strArray[strArray.Length - 1] != null)
          this.names.Add(strArray[strArray.Length - 1]);
      }
      this.names.Shuffle<string>();
    }

    public string Next()
    {
      return this.names[this.idx++ % this.names.Count];
    }
  }
}
