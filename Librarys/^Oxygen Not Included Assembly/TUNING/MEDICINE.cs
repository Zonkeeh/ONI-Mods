// Decompiled with JetBrains decompiler
// Type: TUNING.MEDICINE
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

namespace TUNING
{
  public class MEDICINE
  {
    public static readonly MedicineInfo BASICBOOSTER = new MedicineInfo("basicbooster", "Medicine_BasicBooster", MedicineInfo.MedicineType.Booster, (string[]) null);
    public static readonly MedicineInfo INTERMEDIATEBOOSTER = new MedicineInfo("mediumbooster", "Medicine_IntermediateBooster", MedicineInfo.MedicineType.Booster, (string[]) null);
    public static readonly MedicineInfo BASICCURE = new MedicineInfo("basiccure", (string) null, MedicineInfo.MedicineType.CureSpecific, new string[1]
    {
      "FoodSickness"
    });
    public static readonly MedicineInfo ANTIHISTAMINE = new MedicineInfo("antihistamine", "HistamineSuppression", MedicineInfo.MedicineType.CureSpecific, new string[1]
    {
      "Allergies"
    });
    public static readonly MedicineInfo INTERMEDIATECURE = new MedicineInfo("mediumcure", (string) null, MedicineInfo.MedicineType.CureSpecific, new string[1]
    {
      "SlimeSickness"
    });
    public static readonly MedicineInfo ADVANCEDCURE = new MedicineInfo("majorcure", (string) null, MedicineInfo.MedicineType.CureSpecific, new string[1]
    {
      "SlimeSickness"
    });
    public const float DEFAULT_MASS = 1f;
    public const float RECUPERATION_DISEASE_MULTIPLIER = 1.1f;
    public const float RECUPERATION_DOCTORED_DISEASE_MULTIPLIER = 1.2f;
    public const float WORK_TIME = 10f;
  }
}
