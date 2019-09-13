// Decompiled with JetBrains decompiler
// Type: Database.ChoreTypes
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using STRINGS;
using System;
using System.Collections.Generic;

namespace Database
{
  public class ChoreTypes : ResourceSet<ChoreType>
  {
    private int nextImplicitPriority = 10000;
    public ChoreType Attack;
    public ChoreType Capture;
    public ChoreType Flee;
    public ChoreType BeIncapacitated;
    public ChoreType DebugGoTo;
    public ChoreType DeliverFood;
    public ChoreType Die;
    public ChoreType GeneShuffle;
    public ChoreType Doctor;
    public ChoreType WashHands;
    public ChoreType Shower;
    public ChoreType Eat;
    public ChoreType Entombed;
    public ChoreType Idle;
    public ChoreType MoveToQuarantine;
    public ChoreType RescueIncapacitated;
    public ChoreType RecoverBreath;
    public ChoreType Sigh;
    public ChoreType Sleep;
    public ChoreType Narcolepsy;
    public ChoreType Vomit;
    public ChoreType Cough;
    public ChoreType Pee;
    public ChoreType BreakPee;
    public ChoreType TakeMedicine;
    public ChoreType GetDoctored;
    public ChoreType RestDueToDisease;
    public ChoreType SleepDueToDisease;
    public ChoreType Heal;
    public ChoreType HealCritical;
    public ChoreType EmoteIdle;
    public ChoreType Emote;
    public ChoreType EmoteHighPriority;
    public ChoreType StressEmote;
    public ChoreType StressActingOut;
    public ChoreType Relax;
    public ChoreType StressHeal;
    public ChoreType MoveToSafety;
    public ChoreType Equip;
    public ChoreType Recharge;
    public ChoreType Unequip;
    public ChoreType Warmup;
    public ChoreType Cooldown;
    public ChoreType Mop;
    public ChoreType Relocate;
    public ChoreType Toggle;
    public ChoreType Mourn;
    public ChoreType Fetch;
    public ChoreType FetchCritical;
    public ChoreType StorageFetch;
    public ChoreType Transport;
    public ChoreType RepairFetch;
    public ChoreType MachineFetch;
    public ChoreType ResearchFetch;
    public ChoreType FarmFetch;
    public ChoreType FabricateFetch;
    public ChoreType CookFetch;
    public ChoreType PowerFetch;
    public ChoreType BuildFetch;
    public ChoreType CreatureFetch;
    public ChoreType FoodFetch;
    public ChoreType DoctorFetch;
    public ChoreType Disinfect;
    public ChoreType Repair;
    public ChoreType EmptyStorage;
    public ChoreType Deconstruct;
    public ChoreType Art;
    public ChoreType Research;
    public ChoreType GeneratePower;
    public ChoreType Harvest;
    public ChoreType Uproot;
    public ChoreType CleanToilet;
    public ChoreType EmptyDesalinator;
    public ChoreType LiquidCooledFan;
    public ChoreType IceCooledFan;
    public ChoreType CompostWorkable;
    public ChoreType Fabricate;
    public ChoreType FarmingFabricate;
    public ChoreType PowerFabricate;
    public ChoreType Compound;
    public ChoreType Cook;
    public ChoreType Train;
    public ChoreType Ranch;
    public ChoreType Build;
    public ChoreType BuildDig;
    public ChoreType Dig;
    public ChoreType FlipCompost;
    public ChoreType PowerTinker;
    public ChoreType MachineTinker;
    public ChoreType CropTend;
    public ChoreType Depressurize;
    public ChoreType DropUnusedInventory;
    public ChoreType StressVomit;
    public ChoreType MoveTo;
    public ChoreType UglyCry;
    public ChoreType BingeEat;
    public ChoreType StressIdle;
    public ChoreType ScrubOre;
    public ChoreType SuitMarker;
    public ChoreType ReturnSuitUrgent;
    public ChoreType ReturnSuitIdle;
    public ChoreType Checkpoint;
    public ChoreType TravelTubeEntrance;
    public ChoreType LearnSkill;
    public ChoreType SwitchHat;
    public ChoreType EggSing;
    public ChoreType Astronaut;
    public ChoreType TopPriority;
    private const int INVALID_PRIORITY = -1;

    public ChoreTypes(ResourceSet parent)
      : base(nameof (ChoreTypes), parent)
    {
      this.Die = this.Add(nameof (Die), new string[0], string.Empty, new string[0], (string) DUPLICANTS.CHORES.DIE.NAME, (string) DUPLICANTS.CHORES.DIE.STATUS, (string) DUPLICANTS.CHORES.DIE.TOOLTIP, false, -1, (string) null);
      this.Entombed = this.Add(nameof (Entombed), new string[0], string.Empty, new string[0], (string) DUPLICANTS.CHORES.ENTOMBED.NAME, (string) DUPLICANTS.CHORES.ENTOMBED.STATUS, (string) DUPLICANTS.CHORES.ENTOMBED.TOOLTIP, false, -1, (string) null);
      this.SuitMarker = this.Add(nameof (SuitMarker), new string[0], string.Empty, new string[0], (string) DUPLICANTS.CHORES.WASHHANDS.NAME, (string) DUPLICANTS.CHORES.WASHHANDS.STATUS, (string) DUPLICANTS.CHORES.WASHHANDS.TOOLTIP, false, -1, (string) null);
      this.Checkpoint = this.Add(nameof (Checkpoint), new string[0], string.Empty, new string[0], (string) DUPLICANTS.CHORES.CHECKPOINT.NAME, (string) DUPLICANTS.CHORES.CHECKPOINT.STATUS, (string) DUPLICANTS.CHORES.CHECKPOINT.TOOLTIP, false, -1, (string) null);
      this.TravelTubeEntrance = this.Add(nameof (TravelTubeEntrance), new string[0], string.Empty, new string[0], (string) DUPLICANTS.CHORES.TRAVELTUBEENTRANCE.NAME, (string) DUPLICANTS.CHORES.TRAVELTUBEENTRANCE.STATUS, (string) DUPLICANTS.CHORES.TRAVELTUBEENTRANCE.TOOLTIP, false, -1, (string) null);
      this.WashHands = this.Add(nameof (WashHands), new string[0], string.Empty, new string[0], (string) DUPLICANTS.CHORES.WASHHANDS.NAME, (string) DUPLICANTS.CHORES.WASHHANDS.STATUS, (string) DUPLICANTS.CHORES.WASHHANDS.TOOLTIP, false, -1, (string) null);
      this.HealCritical = this.Add(nameof (HealCritical), new string[0], nameof (HealCritical), new string[3]
      {
        nameof (Vomit),
        nameof (Cough),
        nameof (EmoteHighPriority)
      }, (string) DUPLICANTS.CHORES.HEAL.NAME, (string) DUPLICANTS.CHORES.HEAL.STATUS, (string) DUPLICANTS.CHORES.HEAL.TOOLTIP, false, -1, (string) null);
      this.BeIncapacitated = this.Add(nameof (BeIncapacitated), new string[0], nameof (BeIncapacitated), new string[0], (string) DUPLICANTS.CHORES.BEINCAPACITATED.NAME, (string) DUPLICANTS.CHORES.BEINCAPACITATED.STATUS, (string) DUPLICANTS.CHORES.BEINCAPACITATED.TOOLTIP, false, -1, (string) null);
      this.GeneShuffle = this.Add(nameof (GeneShuffle), new string[0], string.Empty, new string[0], (string) DUPLICANTS.CHORES.GENESHUFFLE.NAME, (string) DUPLICANTS.CHORES.GENESHUFFLE.STATUS, (string) DUPLICANTS.CHORES.GENESHUFFLE.TOOLTIP, false, -1, (string) null);
      this.DebugGoTo = this.Add(nameof (DebugGoTo), new string[0], string.Empty, new string[0], (string) DUPLICANTS.CHORES.DEBUGGOTO.NAME, (string) DUPLICANTS.CHORES.DEBUGGOTO.STATUS, (string) DUPLICANTS.CHORES.MOVETO.TOOLTIP, false, -1, (string) null);
      this.MoveTo = this.Add(nameof (MoveTo), new string[0], string.Empty, new string[0], (string) DUPLICANTS.CHORES.MOVETO.NAME, (string) DUPLICANTS.CHORES.MOVETO.STATUS, (string) DUPLICANTS.CHORES.MOVETO.TOOLTIP, false, -1, (string) null);
      this.DropUnusedInventory = this.Add(nameof (DropUnusedInventory), new string[0], string.Empty, new string[0], (string) DUPLICANTS.CHORES.DROPUNUSEDINVENTORY.NAME, (string) DUPLICANTS.CHORES.DROPUNUSEDINVENTORY.STATUS, (string) DUPLICANTS.CHORES.DROPUNUSEDINVENTORY.TOOLTIP, false, -1, (string) null);
      this.Pee = this.Add(nameof (Pee), new string[0], nameof (Pee), new string[0], (string) DUPLICANTS.CHORES.PEE.NAME, (string) DUPLICANTS.CHORES.PEE.STATUS, (string) DUPLICANTS.CHORES.PEE.TOOLTIP, false, -1, (string) null);
      this.RecoverBreath = this.Add(nameof (RecoverBreath), new string[0], nameof (RecoverBreath), new string[0], (string) DUPLICANTS.CHORES.RECOVERBREATH.NAME, (string) DUPLICANTS.CHORES.RECOVERBREATH.STATUS, (string) DUPLICANTS.CHORES.RECOVERBREATH.TOOLTIP, false, -1, (string) null);
      this.Flee = this.Add(nameof (Flee), new string[0], string.Empty, new string[0], (string) DUPLICANTS.CHORES.FLEE.NAME, (string) DUPLICANTS.CHORES.FLEE.STATUS, (string) DUPLICANTS.CHORES.FLEE.TOOLTIP, false, -1, (string) null);
      this.MoveToQuarantine = this.Add(nameof (MoveToQuarantine), new string[0], nameof (MoveToQuarantine), new string[0], (string) DUPLICANTS.CHORES.MOVETOQUARANTINE.NAME, (string) DUPLICANTS.CHORES.MOVETOQUARANTINE.STATUS, (string) DUPLICANTS.CHORES.MOVETOQUARANTINE.TOOLTIP, false, -1, (string) null);
      this.EmoteIdle = this.Add(nameof (EmoteIdle), new string[0], nameof (EmoteIdle), new string[0], (string) DUPLICANTS.CHORES.EMOTEHIGHPRIORITY.NAME, (string) DUPLICANTS.CHORES.EMOTEHIGHPRIORITY.STATUS, (string) DUPLICANTS.CHORES.EMOTEHIGHPRIORITY.TOOLTIP, false, -1, (string) null);
      this.Emote = this.Add(nameof (Emote), new string[0], nameof (Emote), new string[0], (string) DUPLICANTS.CHORES.EMOTEHIGHPRIORITY.NAME, (string) DUPLICANTS.CHORES.EMOTEHIGHPRIORITY.STATUS, (string) DUPLICANTS.CHORES.EMOTEHIGHPRIORITY.TOOLTIP, false, -1, (string) null);
      this.EmoteHighPriority = this.Add(nameof (EmoteHighPriority), new string[0], nameof (EmoteHighPriority), new string[0], (string) DUPLICANTS.CHORES.EMOTEHIGHPRIORITY.NAME, (string) DUPLICANTS.CHORES.EMOTEHIGHPRIORITY.STATUS, (string) DUPLICANTS.CHORES.EMOTEHIGHPRIORITY.TOOLTIP, false, -1, (string) null);
      this.StressEmote = this.Add(nameof (StressEmote), new string[0], nameof (EmoteHighPriority), new string[0], (string) DUPLICANTS.CHORES.EMOTEHIGHPRIORITY.NAME, (string) DUPLICANTS.CHORES.EMOTEHIGHPRIORITY.STATUS, (string) DUPLICANTS.CHORES.EMOTEHIGHPRIORITY.TOOLTIP, false, -1, (string) null);
      this.StressVomit = this.Add(nameof (StressVomit), new string[0], string.Empty, new string[0], (string) DUPLICANTS.CHORES.STRESSVOMIT.NAME, (string) DUPLICANTS.CHORES.STRESSVOMIT.STATUS, (string) DUPLICANTS.CHORES.STRESSVOMIT.TOOLTIP, false, -1, (string) null);
      this.UglyCry = this.Add(nameof (UglyCry), new string[0], string.Empty, new string[1]
      {
        nameof (MoveTo)
      }, (string) DUPLICANTS.CHORES.UGLY_CRY.NAME, (string) DUPLICANTS.CHORES.UGLY_CRY.STATUS, (string) DUPLICANTS.CHORES.UGLY_CRY.TOOLTIP, false, -1, (string) null);
      this.BingeEat = this.Add(nameof (BingeEat), new string[0], string.Empty, new string[1]
      {
        nameof (MoveTo)
      }, (string) DUPLICANTS.CHORES.BINGE_EAT.NAME, (string) DUPLICANTS.CHORES.BINGE_EAT.STATUS, (string) DUPLICANTS.CHORES.BINGE_EAT.TOOLTIP, false, -1, (string) null);
      this.StressActingOut = this.Add(nameof (StressActingOut), new string[0], string.Empty, new string[1]
      {
        nameof (MoveTo)
      }, (string) DUPLICANTS.CHORES.STRESSACTINGOUT.NAME, (string) DUPLICANTS.CHORES.STRESSACTINGOUT.STATUS, (string) DUPLICANTS.CHORES.STRESSACTINGOUT.TOOLTIP, false, -1, (string) null);
      this.Vomit = this.Add(nameof (Vomit), new string[0], nameof (EmoteHighPriority), new string[0], (string) DUPLICANTS.CHORES.VOMIT.NAME, (string) DUPLICANTS.CHORES.VOMIT.STATUS, (string) DUPLICANTS.CHORES.VOMIT.TOOLTIP, false, -1, (string) null);
      this.Cough = this.Add(nameof (Cough), new string[0], nameof (EmoteHighPriority), new string[0], (string) DUPLICANTS.CHORES.COUGH.NAME, (string) DUPLICANTS.CHORES.COUGH.STATUS, (string) DUPLICANTS.CHORES.COUGH.TOOLTIP, false, -1, (string) null);
      this.SwitchHat = this.Add(nameof (SwitchHat), new string[0], string.Empty, new string[0], (string) DUPLICANTS.CHORES.LEARNSKILL.NAME, (string) DUPLICANTS.CHORES.LEARNSKILL.STATUS, (string) DUPLICANTS.CHORES.LEARNSKILL.TOOLTIP, false, -1, (string) null);
      this.StressIdle = this.Add(nameof (StressIdle), new string[0], string.Empty, new string[0], (string) DUPLICANTS.CHORES.STRESSIDLE.NAME, (string) DUPLICANTS.CHORES.STRESSIDLE.STATUS, (string) DUPLICANTS.CHORES.STRESSIDLE.TOOLTIP, false, -1, (string) null);
      this.RescueIncapacitated = this.Add(nameof (RescueIncapacitated), new string[0], string.Empty, new string[0], (string) DUPLICANTS.CHORES.RESCUEINCAPACITATED.NAME, (string) DUPLICANTS.CHORES.RESCUEINCAPACITATED.STATUS, (string) DUPLICANTS.CHORES.RESCUEINCAPACITATED.TOOLTIP, false, -1, (string) null);
      this.BreakPee = this.Add(nameof (BreakPee), new string[0], nameof (Pee), new string[0], (string) DUPLICANTS.CHORES.BREAK_PEE.NAME, (string) DUPLICANTS.CHORES.BREAK_PEE.STATUS, (string) DUPLICANTS.CHORES.BREAK_PEE.TOOLTIP, false, -1, (string) null);
      this.Eat = this.Add(nameof (Eat), new string[0], nameof (Eat), new string[0], (string) DUPLICANTS.CHORES.EAT.NAME, (string) DUPLICANTS.CHORES.EAT.STATUS, (string) DUPLICANTS.CHORES.EAT.TOOLTIP, false, -1, (string) null);
      this.Narcolepsy = this.Add(nameof (Narcolepsy), new string[0], nameof (Narcolepsy), new string[0], (string) DUPLICANTS.CHORES.NARCOLEPSY.NAME, (string) DUPLICANTS.CHORES.NARCOLEPSY.STATUS, (string) DUPLICANTS.CHORES.NARCOLEPSY.TOOLTIP, false, -1, (string) null);
      this.ReturnSuitUrgent = this.Add(nameof (ReturnSuitUrgent), new string[0], string.Empty, new string[0], (string) DUPLICANTS.CHORES.RETURNSUIT.NAME, (string) DUPLICANTS.CHORES.RETURNSUIT.STATUS, (string) DUPLICANTS.CHORES.RETURNSUIT.TOOLTIP, false, -1, (string) null);
      this.SleepDueToDisease = this.Add(nameof (SleepDueToDisease), new string[0], nameof (Sleep), new string[3]
      {
        nameof (Vomit),
        nameof (Cough),
        nameof (EmoteHighPriority)
      }, (string) DUPLICANTS.CHORES.RESTDUETODISEASE.NAME, (string) DUPLICANTS.CHORES.RESTDUETODISEASE.STATUS, (string) DUPLICANTS.CHORES.RESTDUETODISEASE.TOOLTIP, false, -1, (string) null);
      this.Sleep = this.Add(nameof (Sleep), new string[0], nameof (Sleep), new string[0], (string) DUPLICANTS.CHORES.SLEEP.NAME, (string) DUPLICANTS.CHORES.SLEEP.STATUS, (string) DUPLICANTS.CHORES.SLEEP.TOOLTIP, false, -1, (string) null);
      this.TakeMedicine = this.Add(nameof (TakeMedicine), new string[0], string.Empty, new string[0], (string) DUPLICANTS.CHORES.TAKEMEDICINE.NAME, (string) DUPLICANTS.CHORES.TAKEMEDICINE.STATUS, (string) DUPLICANTS.CHORES.TAKEMEDICINE.TOOLTIP, false, -1, (string) null);
      this.GetDoctored = this.Add(nameof (GetDoctored), new string[0], string.Empty, new string[0], (string) DUPLICANTS.CHORES.GETDOCTORED.NAME, (string) DUPLICANTS.CHORES.GETDOCTORED.STATUS, (string) DUPLICANTS.CHORES.GETDOCTORED.TOOLTIP, false, -1, (string) null);
      this.RestDueToDisease = this.Add(nameof (RestDueToDisease), new string[0], nameof (RestDueToDisease), new string[3]
      {
        nameof (Vomit),
        nameof (Cough),
        nameof (EmoteHighPriority)
      }, (string) DUPLICANTS.CHORES.RESTDUETODISEASE.NAME, (string) DUPLICANTS.CHORES.RESTDUETODISEASE.STATUS, (string) DUPLICANTS.CHORES.RESTDUETODISEASE.TOOLTIP, false, -1, (string) null);
      this.ScrubOre = this.Add(nameof (ScrubOre), new string[0], string.Empty, new string[0], (string) DUPLICANTS.CHORES.SCRUBORE.NAME, (string) DUPLICANTS.CHORES.SCRUBORE.STATUS, (string) DUPLICANTS.CHORES.SCRUBORE.TOOLTIP, false, -1, (string) null);
      this.DeliverFood = this.Add(nameof (DeliverFood), new string[0], string.Empty, new string[0], (string) DUPLICANTS.CHORES.DELIVERFOOD.NAME, (string) DUPLICANTS.CHORES.DELIVERFOOD.STATUS, (string) DUPLICANTS.CHORES.DELIVERFOOD.TOOLTIP, false, -1, (string) null);
      this.Sigh = this.Add(nameof (Sigh), new string[0], nameof (Emote), new string[0], (string) DUPLICANTS.CHORES.SIGH.NAME, (string) DUPLICANTS.CHORES.SIGH.STATUS, (string) DUPLICANTS.CHORES.SIGH.TOOLTIP, false, -1, (string) null);
      this.Heal = this.Add(nameof (Heal), new string[0], nameof (Heal), new string[3]
      {
        nameof (Vomit),
        nameof (Cough),
        nameof (EmoteHighPriority)
      }, (string) DUPLICANTS.CHORES.HEAL.NAME, (string) DUPLICANTS.CHORES.HEAL.STATUS, (string) DUPLICANTS.CHORES.HEAL.TOOLTIP, false, -1, (string) null);
      this.Shower = this.Add(nameof (Shower), new string[0], nameof (Shower), new string[0], (string) DUPLICANTS.CHORES.SHOWER.NAME, (string) DUPLICANTS.CHORES.SHOWER.STATUS, (string) DUPLICANTS.CHORES.SHOWER.TOOLTIP, false, -1, (string) null);
      this.LearnSkill = this.Add(nameof (LearnSkill), new string[0], nameof (LearnSkill), new string[0], (string) DUPLICANTS.CHORES.LEARNSKILL.NAME, (string) DUPLICANTS.CHORES.LEARNSKILL.STATUS, (string) DUPLICANTS.CHORES.LEARNSKILL.TOOLTIP, false, -1, (string) null);
      this.Equip = this.Add(nameof (Equip), new string[0], string.Empty, new string[0], (string) DUPLICANTS.CHORES.EQUIP.NAME, (string) DUPLICANTS.CHORES.EQUIP.STATUS, (string) DUPLICANTS.CHORES.EQUIP.TOOLTIP, false, -1, (string) null);
      this.StressHeal = this.Add(nameof (StressHeal), new string[0], string.Empty, new string[1]
      {
        string.Empty
      }, (string) DUPLICANTS.CHORES.STRESSHEAL.NAME, (string) DUPLICANTS.CHORES.STRESSHEAL.STATUS, (string) DUPLICANTS.CHORES.STRESSHEAL.TOOLTIP, false, -1, (string) null);
      this.Relax = this.Add(nameof (Relax), new string[0], string.Empty, new string[1]
      {
        nameof (Sleep)
      }, (string) DUPLICANTS.CHORES.RELAX.NAME, (string) DUPLICANTS.CHORES.RELAX.STATUS, (string) DUPLICANTS.CHORES.RELAX.TOOLTIP, false, -1, (string) null);
      this.Recharge = this.Add(nameof (Recharge), new string[0], string.Empty, new string[0], (string) DUPLICANTS.CHORES.RECHARGE.NAME, (string) DUPLICANTS.CHORES.RECHARGE.STATUS, (string) DUPLICANTS.CHORES.RECHARGE.TOOLTIP, false, -1, (string) null);
      this.Unequip = this.Add(nameof (Unequip), new string[0], string.Empty, new string[0], (string) DUPLICANTS.CHORES.UNEQUIP.NAME, (string) DUPLICANTS.CHORES.UNEQUIP.STATUS, (string) DUPLICANTS.CHORES.UNEQUIP.TOOLTIP, false, -1, (string) null);
      this.Mourn = this.Add(nameof (Mourn), new string[0], string.Empty, new string[0], (string) DUPLICANTS.CHORES.MOURN.NAME, (string) DUPLICANTS.CHORES.MOURN.STATUS, (string) DUPLICANTS.CHORES.MOURN.TOOLTIP, false, -1, (string) null);
      this.TopPriority = this.Add(nameof (TopPriority), new string[0], string.Empty, new string[0], string.Empty, string.Empty, string.Empty, false, -1, (string) null);
      this.Attack = this.Add(nameof (Attack), new string[1]
      {
        "Combat"
      }, string.Empty, new string[0], (string) DUPLICANTS.CHORES.ATTACK.NAME, (string) DUPLICANTS.CHORES.ATTACK.STATUS, (string) DUPLICANTS.CHORES.ATTACK.TOOLTIP, false, 5000, (string) null);
      this.Doctor = this.Add("DoctorChore", new string[1]
      {
        "MedicalAid"
      }, nameof (Doctor), new string[0], (string) DUPLICANTS.CHORES.DOCTOR.NAME, (string) DUPLICANTS.CHORES.DOCTOR.STATUS, (string) DUPLICANTS.CHORES.DOCTOR.TOOLTIP, false, 5000, (string) null);
      this.Toggle = this.Add(nameof (Toggle), new string[1]
      {
        nameof (Toggle)
      }, string.Empty, new string[0], (string) DUPLICANTS.CHORES.TOGGLE.NAME, (string) DUPLICANTS.CHORES.TOGGLE.STATUS, (string) DUPLICANTS.CHORES.TOGGLE.TOOLTIP, true, 5000, (string) null);
      this.Capture = this.Add(nameof (Capture), new string[1]
      {
        "Ranching"
      }, string.Empty, new string[0], (string) DUPLICANTS.CHORES.CAPTURE.NAME, (string) DUPLICANTS.CHORES.CAPTURE.STATUS, (string) DUPLICANTS.CHORES.CAPTURE.TOOLTIP, false, 5000, (string) null);
      this.CreatureFetch = this.Add(nameof (CreatureFetch), new string[1]
      {
        "Ranching"
      }, string.Empty, new string[0], (string) DUPLICANTS.CHORES.FETCHCREATURE.NAME, (string) DUPLICANTS.CHORES.FETCHCREATURE.STATUS, (string) DUPLICANTS.CHORES.FETCHCREATURE.TOOLTIP, false, 5000, (string) null);
      this.EggSing = this.Add(nameof (EggSing), new string[1]
      {
        "Ranching"
      }, string.Empty, new string[0], (string) DUPLICANTS.CHORES.SINGTOEGG.NAME, (string) DUPLICANTS.CHORES.SINGTOEGG.STATUS, (string) DUPLICANTS.CHORES.SINGTOEGG.TOOLTIP, false, 5000, (string) null);
      this.Astronaut = this.Add(nameof (Astronaut), new string[1]
      {
        "MachineOperating"
      }, string.Empty, new string[0], (string) DUPLICANTS.CHORES.ASTRONAUT.NAME, (string) DUPLICANTS.CHORES.ASTRONAUT.STATUS, (string) DUPLICANTS.CHORES.ASTRONAUT.TOOLTIP, false, 5000, (string) null);
      this.FetchCritical = this.Add(nameof (FetchCritical), new string[2]
      {
        "Hauling",
        "LifeSupport"
      }, string.Empty, new string[0], (string) DUPLICANTS.CHORES.FETCHCRITICAL.NAME, (string) DUPLICANTS.CHORES.FETCHCRITICAL.STATUS, (string) DUPLICANTS.CHORES.FETCHCRITICAL.TOOLTIP, false, 5000, (string) DUPLICANTS.CHORES.FETCHCRITICAL.REPORT_NAME);
      this.Art = this.Add(nameof (Art), new string[1]
      {
        nameof (Art)
      }, string.Empty, new string[0], (string) DUPLICANTS.CHORES.ART.NAME, (string) DUPLICANTS.CHORES.ART.STATUS, (string) DUPLICANTS.CHORES.ART.TOOLTIP, false, 5000, (string) null);
      this.EmptyStorage = this.Add(nameof (EmptyStorage), new string[2]
      {
        "Basekeeping",
        "Hauling"
      }, string.Empty, new string[0], (string) DUPLICANTS.CHORES.EMPTYSTORAGE.NAME, (string) DUPLICANTS.CHORES.EMPTYSTORAGE.STATUS, (string) DUPLICANTS.CHORES.EMPTYSTORAGE.TOOLTIP, false, 5000, (string) null);
      this.Mop = this.Add(nameof (Mop), new string[1]
      {
        "Basekeeping"
      }, string.Empty, new string[0], (string) DUPLICANTS.CHORES.MOP.NAME, (string) DUPLICANTS.CHORES.MOP.STATUS, (string) DUPLICANTS.CHORES.MOP.TOOLTIP, true, 5000, (string) null);
      this.Relocate = this.Add(nameof (Relocate), new string[0], string.Empty, new string[0], (string) DUPLICANTS.CHORES.RELOCATE.NAME, (string) DUPLICANTS.CHORES.RELOCATE.STATUS, (string) DUPLICANTS.CHORES.RELOCATE.TOOLTIP, true, 5000, (string) null);
      this.Disinfect = this.Add(nameof (Disinfect), new string[1]
      {
        "Basekeeping"
      }, string.Empty, new string[0], (string) DUPLICANTS.CHORES.DISINFECT.NAME, (string) DUPLICANTS.CHORES.DISINFECT.STATUS, (string) DUPLICANTS.CHORES.DISINFECT.TOOLTIP, true, 5000, (string) null);
      this.Repair = this.Add(nameof (Repair), new string[1]
      {
        "Basekeeping"
      }, string.Empty, new string[0], (string) DUPLICANTS.CHORES.REPAIR.NAME, (string) DUPLICANTS.CHORES.REPAIR.STATUS, (string) DUPLICANTS.CHORES.REPAIR.TOOLTIP, false, 5000, (string) null);
      this.RepairFetch = this.Add(nameof (RepairFetch), new string[2]
      {
        "Basekeeping",
        "Hauling"
      }, string.Empty, new string[0], (string) DUPLICANTS.CHORES.REPAIRFETCH.NAME, (string) DUPLICANTS.CHORES.REPAIRFETCH.STATUS, (string) DUPLICANTS.CHORES.REPAIRFETCH.TOOLTIP, false, 5000, (string) null);
      this.Deconstruct = this.Add(nameof (Deconstruct), new string[1]
      {
        nameof (Build)
      }, string.Empty, new string[0], (string) DUPLICANTS.CHORES.DECONSTRUCT.NAME, (string) DUPLICANTS.CHORES.DECONSTRUCT.STATUS, (string) DUPLICANTS.CHORES.DECONSTRUCT.TOOLTIP, false, 5000, (string) null);
      this.Research = this.Add(nameof (Research), new string[1]
      {
        nameof (Research)
      }, string.Empty, new string[0], (string) DUPLICANTS.CHORES.RESEARCH.NAME, (string) DUPLICANTS.CHORES.RESEARCH.STATUS, (string) DUPLICANTS.CHORES.RESEARCH.TOOLTIP, false, 5000, (string) null);
      this.ResearchFetch = this.Add(nameof (ResearchFetch), new string[2]
      {
        nameof (Research),
        "Hauling"
      }, string.Empty, new string[0], (string) DUPLICANTS.CHORES.RESEARCHFETCH.NAME, (string) DUPLICANTS.CHORES.RESEARCHFETCH.STATUS, (string) DUPLICANTS.CHORES.RESEARCHFETCH.TOOLTIP, false, 5000, (string) null);
      this.GeneratePower = this.Add(nameof (GeneratePower), new string[1]
      {
        "MachineOperating"
      }, string.Empty, new string[1]{ nameof (StressHeal) }, (string) DUPLICANTS.CHORES.GENERATEPOWER.NAME, (string) DUPLICANTS.CHORES.GENERATEPOWER.STATUS, (string) DUPLICANTS.CHORES.GENERATEPOWER.TOOLTIP, false, 5000, (string) null);
      this.CropTend = this.Add(nameof (CropTend), new string[1]
      {
        "Farming"
      }, string.Empty, new string[0], (string) DUPLICANTS.CHORES.CROP_TEND.NAME, (string) DUPLICANTS.CHORES.CROP_TEND.STATUS, (string) DUPLICANTS.CHORES.CROP_TEND.TOOLTIP, false, 5000, (string) null);
      this.PowerTinker = this.Add(nameof (PowerTinker), new string[1]
      {
        "MachineOperating"
      }, string.Empty, new string[0], (string) DUPLICANTS.CHORES.POWER_TINKER.NAME, (string) DUPLICANTS.CHORES.POWER_TINKER.STATUS, (string) DUPLICANTS.CHORES.POWER_TINKER.TOOLTIP, false, 5000, (string) null);
      this.MachineTinker = this.Add(nameof (MachineTinker), new string[1]
      {
        "MachineOperating"
      }, string.Empty, new string[0], (string) DUPLICANTS.CHORES.POWER_TINKER.NAME, (string) DUPLICANTS.CHORES.POWER_TINKER.STATUS, (string) DUPLICANTS.CHORES.POWER_TINKER.TOOLTIP, false, 5000, (string) null);
      this.MachineFetch = this.Add(nameof (MachineFetch), new string[2]
      {
        "MachineOperating",
        "Hauling"
      }, string.Empty, new string[0], (string) DUPLICANTS.CHORES.MACHINEFETCH.NAME, (string) DUPLICANTS.CHORES.MACHINEFETCH.STATUS, (string) DUPLICANTS.CHORES.MACHINEFETCH.TOOLTIP, false, 5000, (string) DUPLICANTS.CHORES.MACHINEFETCH.REPORT_NAME);
      this.Harvest = this.Add(nameof (Harvest), new string[1]
      {
        "Farming"
      }, string.Empty, new string[0], (string) DUPLICANTS.CHORES.HARVEST.NAME, (string) DUPLICANTS.CHORES.HARVEST.STATUS, (string) DUPLICANTS.CHORES.HARVEST.TOOLTIP, false, 5000, (string) null);
      this.FarmFetch = this.Add(nameof (FarmFetch), new string[2]
      {
        "Farming",
        "Hauling"
      }, string.Empty, new string[0], (string) DUPLICANTS.CHORES.FARMFETCH.NAME, (string) DUPLICANTS.CHORES.FARMFETCH.STATUS, (string) DUPLICANTS.CHORES.FARMFETCH.TOOLTIP, false, 5000, (string) null);
      this.Uproot = this.Add(nameof (Uproot), new string[1]
      {
        "Farming"
      }, string.Empty, new string[0], (string) DUPLICANTS.CHORES.UPROOT.NAME, (string) DUPLICANTS.CHORES.UPROOT.STATUS, (string) DUPLICANTS.CHORES.UPROOT.TOOLTIP, false, 5000, (string) null);
      this.CleanToilet = this.Add(nameof (CleanToilet), new string[1]
      {
        "Basekeeping"
      }, string.Empty, new string[0], (string) DUPLICANTS.CHORES.CLEANTOILET.NAME, (string) DUPLICANTS.CHORES.CLEANTOILET.STATUS, (string) DUPLICANTS.CHORES.CLEANTOILET.TOOLTIP, false, 5000, (string) null);
      this.EmptyDesalinator = this.Add(nameof (EmptyDesalinator), new string[1]
      {
        "Basekeeping"
      }, string.Empty, new string[0], (string) DUPLICANTS.CHORES.EMPTYDESALINATOR.NAME, (string) DUPLICANTS.CHORES.EMPTYDESALINATOR.STATUS, (string) DUPLICANTS.CHORES.EMPTYDESALINATOR.TOOLTIP, false, 5000, (string) null);
      this.LiquidCooledFan = this.Add(nameof (LiquidCooledFan), new string[1]
      {
        "MachineOperating"
      }, string.Empty, new string[0], (string) DUPLICANTS.CHORES.LIQUIDCOOLEDFAN.NAME, (string) DUPLICANTS.CHORES.LIQUIDCOOLEDFAN.STATUS, (string) DUPLICANTS.CHORES.LIQUIDCOOLEDFAN.TOOLTIP, false, 5000, (string) null);
      this.IceCooledFan = this.Add(nameof (IceCooledFan), new string[1]
      {
        "MachineOperating"
      }, string.Empty, new string[0], (string) DUPLICANTS.CHORES.ICECOOLEDFAN.NAME, (string) DUPLICANTS.CHORES.ICECOOLEDFAN.STATUS, (string) DUPLICANTS.CHORES.ICECOOLEDFAN.TOOLTIP, false, 5000, (string) null);
      this.Train = this.Add(nameof (Train), new string[1]
      {
        "MachineOperating"
      }, string.Empty, new string[0], (string) DUPLICANTS.CHORES.TRAIN.NAME, (string) DUPLICANTS.CHORES.TRAIN.STATUS, (string) DUPLICANTS.CHORES.TRAIN.TOOLTIP, false, 5000, (string) null);
      this.Cook = this.Add(nameof (Cook), new string[1]
      {
        nameof (Cook)
      }, string.Empty, new string[0], (string) DUPLICANTS.CHORES.COOK.NAME, (string) DUPLICANTS.CHORES.COOK.STATUS, (string) DUPLICANTS.CHORES.COOK.TOOLTIP, false, 5000, (string) null);
      this.CookFetch = this.Add(nameof (CookFetch), new string[2]
      {
        nameof (Cook),
        "Hauling"
      }, string.Empty, new string[0], (string) DUPLICANTS.CHORES.COOKFETCH.NAME, (string) DUPLICANTS.CHORES.COOKFETCH.STATUS, (string) DUPLICANTS.CHORES.COOKFETCH.TOOLTIP, false, 5000, (string) null);
      this.DoctorFetch = this.Add(nameof (DoctorFetch), new string[2]
      {
        "MedicalAid",
        "Hauling"
      }, string.Empty, new string[0], (string) DUPLICANTS.CHORES.DOCTORFETCH.NAME, (string) DUPLICANTS.CHORES.DOCTORFETCH.STATUS, (string) DUPLICANTS.CHORES.DOCTORFETCH.TOOLTIP, false, 5000, (string) DUPLICANTS.CHORES.DOCTORFETCH.REPORT_NAME);
      this.Ranch = this.Add(nameof (Ranch), new string[1]
      {
        "Ranching"
      }, string.Empty, new string[0], (string) DUPLICANTS.CHORES.RANCH.NAME, (string) DUPLICANTS.CHORES.RANCH.STATUS, (string) DUPLICANTS.CHORES.RANCH.TOOLTIP, false, 5000, (string) null);
      this.PowerFetch = this.Add(nameof (PowerFetch), new string[2]
      {
        "MachineOperating",
        "Hauling"
      }, string.Empty, new string[0], (string) DUPLICANTS.CHORES.POWERFETCH.NAME, (string) DUPLICANTS.CHORES.POWERFETCH.STATUS, (string) DUPLICANTS.CHORES.POWERFETCH.TOOLTIP, false, 5000, (string) DUPLICANTS.CHORES.POWERFETCH.REPORT_NAME);
      this.FlipCompost = this.Add(nameof (FlipCompost), new string[1]
      {
        "Farming"
      }, string.Empty, new string[0], (string) DUPLICANTS.CHORES.FLIPCOMPOST.NAME, (string) DUPLICANTS.CHORES.FLIPCOMPOST.STATUS, (string) DUPLICANTS.CHORES.FLIPCOMPOST.TOOLTIP, false, 5000, (string) null);
      this.Depressurize = this.Add(nameof (Depressurize), new string[1]
      {
        "MachineOperating"
      }, string.Empty, new string[0], (string) DUPLICANTS.CHORES.DEPRESSURIZE.NAME, (string) DUPLICANTS.CHORES.DEPRESSURIZE.STATUS, (string) DUPLICANTS.CHORES.DEPRESSURIZE.TOOLTIP, false, 5000, (string) null);
      this.FarmingFabricate = this.Add(nameof (FarmingFabricate), new string[1]
      {
        "Farming"
      }, string.Empty, new string[0], (string) DUPLICANTS.CHORES.FABRICATE.NAME, (string) DUPLICANTS.CHORES.FABRICATE.STATUS, (string) DUPLICANTS.CHORES.FABRICATE.TOOLTIP, false, 5000, (string) null);
      this.PowerFabricate = this.Add(nameof (PowerFabricate), new string[1]
      {
        "MachineOperating"
      }, string.Empty, new string[0], (string) DUPLICANTS.CHORES.FABRICATE.NAME, (string) DUPLICANTS.CHORES.FABRICATE.STATUS, (string) DUPLICANTS.CHORES.FABRICATE.TOOLTIP, false, 5000, (string) null);
      this.Compound = this.Add(nameof (Compound), new string[1]
      {
        "MedicalAid"
      }, string.Empty, new string[0], (string) DUPLICANTS.CHORES.COMPOUND.NAME, (string) DUPLICANTS.CHORES.COMPOUND.STATUS, (string) DUPLICANTS.CHORES.COMPOUND.TOOLTIP, false, 5000, (string) null);
      this.Fabricate = this.Add(nameof (Fabricate), new string[1]
      {
        "MachineOperating"
      }, string.Empty, new string[0], (string) DUPLICANTS.CHORES.FABRICATE.NAME, (string) DUPLICANTS.CHORES.FABRICATE.STATUS, (string) DUPLICANTS.CHORES.FABRICATE.TOOLTIP, false, 5000, (string) null);
      this.FabricateFetch = this.Add(nameof (FabricateFetch), new string[2]
      {
        "MachineOperating",
        "Hauling"
      }, string.Empty, new string[0], (string) DUPLICANTS.CHORES.FABRICATEFETCH.NAME, (string) DUPLICANTS.CHORES.FABRICATEFETCH.STATUS, (string) DUPLICANTS.CHORES.FABRICATEFETCH.TOOLTIP, false, 5000, (string) DUPLICANTS.CHORES.FABRICATEFETCH.REPORT_NAME);
      this.FoodFetch = this.Add(nameof (FoodFetch), new string[1]
      {
        "Hauling"
      }, string.Empty, new string[0], (string) DUPLICANTS.CHORES.FOODFETCH.NAME, (string) DUPLICANTS.CHORES.FOODFETCH.STATUS, (string) DUPLICANTS.CHORES.FOODFETCH.TOOLTIP, false, 5000, (string) DUPLICANTS.CHORES.FOODFETCH.REPORT_NAME);
      this.Transport = this.Add(nameof (Transport), new string[2]
      {
        "Hauling",
        "Basekeeping"
      }, string.Empty, new string[0], (string) DUPLICANTS.CHORES.TRANSPORT.NAME, (string) DUPLICANTS.CHORES.TRANSPORT.STATUS, (string) DUPLICANTS.CHORES.TRANSPORT.TOOLTIP, true, 5000, (string) null);
      this.Build = this.Add(nameof (Build), new string[1]
      {
        nameof (Build)
      }, string.Empty, new string[0], (string) DUPLICANTS.CHORES.BUILD.NAME, (string) DUPLICANTS.CHORES.BUILD.STATUS, (string) DUPLICANTS.CHORES.BUILD.TOOLTIP, true, 5000, (string) null);
      this.BuildDig = this.Add(nameof (BuildDig), new string[2]
      {
        nameof (Build),
        nameof (Dig)
      }, string.Empty, new string[0], (string) DUPLICANTS.CHORES.BUILDDIG.NAME, (string) DUPLICANTS.CHORES.BUILDDIG.STATUS, (string) DUPLICANTS.CHORES.BUILDDIG.TOOLTIP, true, 5000, (string) null);
      this.BuildFetch = this.Add(nameof (BuildFetch), new string[2]
      {
        nameof (Build),
        "Hauling"
      }, string.Empty, new string[0], (string) DUPLICANTS.CHORES.BUILDFETCH.NAME, (string) DUPLICANTS.CHORES.BUILDFETCH.STATUS, (string) DUPLICANTS.CHORES.BUILDFETCH.TOOLTIP, true, 5000, (string) null);
      this.Dig = this.Add(nameof (Dig), new string[1]
      {
        nameof (Dig)
      }, string.Empty, new string[0], (string) DUPLICANTS.CHORES.DIG.NAME, (string) DUPLICANTS.CHORES.DIG.STATUS, (string) DUPLICANTS.CHORES.DIG.TOOLTIP, false, 5000, (string) null);
      this.Fetch = this.Add(nameof (Fetch), new string[1]
      {
        "Storage"
      }, string.Empty, new string[0], (string) DUPLICANTS.CHORES.FETCH.NAME, (string) DUPLICANTS.CHORES.FETCH.STATUS, (string) DUPLICANTS.CHORES.FETCH.TOOLTIP, false, 5000, (string) DUPLICANTS.CHORES.FETCH.REPORT_NAME);
      this.StorageFetch = this.Add(nameof (StorageFetch), new string[1]
      {
        "Storage"
      }, string.Empty, new string[0], (string) DUPLICANTS.CHORES.STORAGEFETCH.NAME, (string) DUPLICANTS.CHORES.STORAGEFETCH.STATUS, (string) DUPLICANTS.CHORES.STORAGEFETCH.TOOLTIP, true, 5000, (string) DUPLICANTS.CHORES.STORAGEFETCH.REPORT_NAME);
      this.MoveToSafety = this.Add(nameof (MoveToSafety), new string[0], nameof (MoveToSafety), new string[0], (string) DUPLICANTS.CHORES.MOVETOSAFETY.NAME, (string) DUPLICANTS.CHORES.MOVETOSAFETY.STATUS, (string) DUPLICANTS.CHORES.MOVETOSAFETY.TOOLTIP, false, -1, (string) null);
      this.ReturnSuitIdle = this.Add(nameof (ReturnSuitIdle), new string[0], string.Empty, new string[0], (string) DUPLICANTS.CHORES.RETURNSUIT.NAME, (string) DUPLICANTS.CHORES.RETURNSUIT.STATUS, (string) DUPLICANTS.CHORES.RETURNSUIT.TOOLTIP, false, -1, (string) null);
      this.Idle = this.Add("IdleChore", new string[0], string.Empty, new string[0], (string) DUPLICANTS.CHORES.IDLE.NAME, (string) DUPLICANTS.CHORES.IDLE.STATUS, (string) DUPLICANTS.CHORES.IDLE.TOOLTIP, false, -1, (string) null);
      ChoreType[][] choreTypeArray1 = new ChoreType[29][]
      {
        new ChoreType[1]{ this.Die },
        new ChoreType[1]{ this.Entombed },
        new ChoreType[1]{ this.HealCritical },
        new ChoreType[2]{ this.BeIncapacitated, this.GeneShuffle },
        new ChoreType[1]{ this.DebugGoTo },
        new ChoreType[1]{ this.StressVomit },
        new ChoreType[1]{ this.MoveTo },
        new ChoreType[1]{ this.RecoverBreath },
        new ChoreType[1]{ this.ReturnSuitUrgent },
        new ChoreType[1]{ this.UglyCry },
        new ChoreType[1]{ this.BingeEat },
        new ChoreType[8]
        {
          this.EmoteHighPriority,
          this.StressActingOut,
          this.Vomit,
          this.Cough,
          this.Pee,
          this.StressIdle,
          this.RescueIncapacitated,
          this.SwitchHat
        },
        new ChoreType[1]{ this.MoveToQuarantine },
        new ChoreType[1]{ this.TopPriority },
        new ChoreType[1]{ this.Attack },
        new ChoreType[1]{ this.Flee },
        new ChoreType[3]{ this.LearnSkill, this.Eat, this.BreakPee },
        new ChoreType[1]{ this.TakeMedicine },
        new ChoreType[3]
        {
          this.Heal,
          this.SleepDueToDisease,
          this.RestDueToDisease
        },
        new ChoreType[2]{ this.Sleep, this.Narcolepsy },
        new ChoreType[2]{ this.Doctor, this.GetDoctored },
        new ChoreType[1]{ this.Emote },
        new ChoreType[1]{ this.Mourn },
        new ChoreType[1]{ this.StressHeal },
        new ChoreType[1]{ this.Relax },
        new ChoreType[2]{ this.Equip, this.Unequip },
        new ChoreType[61]
        {
          this.DeliverFood,
          this.Sigh,
          this.EmptyStorage,
          this.Repair,
          this.Disinfect,
          this.Shower,
          this.CleanToilet,
          this.LiquidCooledFan,
          this.IceCooledFan,
          this.SuitMarker,
          this.Checkpoint,
          this.TravelTubeEntrance,
          this.WashHands,
          this.Recharge,
          this.ScrubOre,
          this.Ranch,
          this.MoveToSafety,
          this.Relocate,
          this.Research,
          this.Mop,
          this.Toggle,
          this.Deconstruct,
          this.Capture,
          this.EggSing,
          this.Art,
          this.GeneratePower,
          this.CropTend,
          this.PowerTinker,
          this.MachineTinker,
          this.DropUnusedInventory,
          this.Harvest,
          this.Uproot,
          this.FarmingFabricate,
          this.PowerFabricate,
          this.Compound,
          this.Fabricate,
          this.Train,
          this.Cook,
          this.Build,
          this.Dig,
          this.BuildDig,
          this.FlipCompost,
          this.Depressurize,
          this.StressEmote,
          this.Astronaut,
          this.EmptyDesalinator,
          this.FetchCritical,
          this.ResearchFetch,
          this.CreatureFetch,
          this.Fetch,
          this.Transport,
          this.FarmFetch,
          this.BuildFetch,
          this.CookFetch,
          this.DoctorFetch,
          this.MachineFetch,
          this.PowerFetch,
          this.FabricateFetch,
          this.FoodFetch,
          this.StorageFetch,
          this.RepairFetch
        },
        new ChoreType[2]{ this.ReturnSuitIdle, this.EmoteIdle },
        new ChoreType[1]{ this.Idle }
      };
      string str1 = string.Empty;
      int num = 100000;
      foreach (ChoreType[] choreTypeArray2 in choreTypeArray1)
      {
        foreach (ChoreType choreType in choreTypeArray2)
        {
          if (choreType.interruptPriority != 0)
            str1 = str1 + "Interrupt priority set more than once: " + choreType.Id;
          choreType.interruptPriority = num;
        }
        num -= 100;
      }
      if (!string.IsNullOrEmpty(str1))
        Debug.LogError((object) str1);
      string str2 = string.Empty;
      foreach (ChoreType resource in this.resources)
      {
        if (resource.interruptPriority == 0)
          str2 = str2 + "Interrupt priority missing for: " + resource.Id + "\n";
      }
      if (string.IsNullOrEmpty(str2))
        return;
      Debug.LogError((object) str2);
    }

    public ChoreType GetByHash(HashedString id_hash)
    {
      int index = this.resources.FindIndex((Predicate<ChoreType>) (item => item.IdHash == id_hash));
      if (index != -1)
        return this.resources[index];
      return (ChoreType) null;
    }

    private ChoreType Add(
      string id,
      string[] chore_groups,
      string urge,
      string[] interrupt_exclusion,
      string name,
      string status_message,
      string tooltip,
      bool skip_implicit_priority_change,
      int explicit_priority = -1,
      string report_name = null)
    {
      ListPool<Tag, ChoreTypes>.PooledList pooledList = ListPool<Tag, ChoreTypes>.Allocate();
      for (int index = 0; index < interrupt_exclusion.Length; ++index)
        pooledList.Add(TagManager.Create(interrupt_exclusion[index]));
      if (explicit_priority == -1)
        explicit_priority = this.nextImplicitPriority;
      ChoreType choreType = new ChoreType(id, (ResourceSet) this, chore_groups, urge, name, status_message, tooltip, (IEnumerable<Tag>) pooledList, this.nextImplicitPriority, explicit_priority);
      pooledList.Recycle();
      if (!skip_implicit_priority_change)
        this.nextImplicitPriority -= 100;
      if (report_name != null)
        choreType.reportName = report_name;
      return choreType;
    }
  }
}
