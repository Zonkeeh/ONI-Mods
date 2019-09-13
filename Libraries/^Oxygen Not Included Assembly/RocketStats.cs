// Decompiled with JetBrains decompiler
// Type: RocketStats
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class RocketStats
{
  private CommandModule commandModule;
  private static Dictionary<Tag, float> oxidizerEfficiencies;

  public RocketStats(CommandModule commandModule)
  {
    this.commandModule = commandModule;
    if (RocketStats.oxidizerEfficiencies != null)
      return;
    RocketStats.oxidizerEfficiencies = new Dictionary<Tag, float>()
    {
      {
        SimHashes.OxyRock.CreateTag(),
        ROCKETRY.OXIDIZER_EFFICIENCY.LOW
      },
      {
        SimHashes.LiquidOxygen.CreateTag(),
        ROCKETRY.OXIDIZER_EFFICIENCY.HIGH
      }
    };
  }

  public float GetRocketMaxDistance()
  {
    return Mathf.Max(0.0f, this.GetTotalThrust() - ROCKETRY.CalculateMassWithPenalty(this.GetTotalMass()));
  }

  public float GetTotalMass()
  {
    return this.GetDryMass() + this.GetWetMass();
  }

  public float GetDryMass()
  {
    float num = 0.0f;
    foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this.commandModule.GetComponent<AttachableBuilding>()))
    {
      RocketModule component = gameObject.GetComponent<RocketModule>();
      if ((Object) component != (Object) null)
        num += component.GetComponent<PrimaryElement>().Mass;
    }
    return num;
  }

  public float GetWetMass()
  {
    float num = 0.0f;
    foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this.commandModule.GetComponent<AttachableBuilding>()))
    {
      RocketModule component1 = gameObject.GetComponent<RocketModule>();
      if ((Object) component1 != (Object) null)
      {
        FuelTank component2 = component1.GetComponent<FuelTank>();
        OxidizerTank component3 = component1.GetComponent<OxidizerTank>();
        SolidBooster component4 = component1.GetComponent<SolidBooster>();
        if ((Object) component2 != (Object) null)
          num += component2.MassStored();
        if ((Object) component3 != (Object) null)
          num += component3.MassStored();
        if ((Object) component4 != (Object) null)
          num += component4.fuelStorage.MassStored();
      }
    }
    return num;
  }

  public Tag GetEngineFuelTag()
  {
    RocketEngine mainEngine = this.GetMainEngine();
    if ((Object) mainEngine != (Object) null)
      return mainEngine.fuelTag;
    return (Tag) ((string) null);
  }

  public float GetTotalFuel(bool includeBoosters = false)
  {
    float num = 0.0f;
    foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this.commandModule.GetComponent<AttachableBuilding>()))
    {
      FuelTank component1 = gameObject.GetComponent<FuelTank>();
      Tag engineFuelTag = this.GetEngineFuelTag();
      if ((Object) component1 != (Object) null)
        num += component1.GetAmountAvailable(engineFuelTag);
      if (includeBoosters)
      {
        SolidBooster component2 = gameObject.GetComponent<SolidBooster>();
        if ((Object) component2 != (Object) null)
          num += component2.fuelStorage.GetAmountAvailable(component2.fuelTag);
      }
    }
    return num;
  }

  public float GetTotalOxidizer(bool includeBoosters = false)
  {
    float num = 0.0f;
    foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this.commandModule.GetComponent<AttachableBuilding>()))
    {
      OxidizerTank component1 = gameObject.GetComponent<OxidizerTank>();
      if ((Object) component1 != (Object) null)
        num += component1.GetTotalOxidizerAvailable();
      if (includeBoosters)
      {
        SolidBooster component2 = gameObject.GetComponent<SolidBooster>();
        if ((Object) component2 != (Object) null)
          num += component2.fuelStorage.GetAmountAvailable(GameTags.OxyRock);
      }
    }
    return num;
  }

  public float GetAverageOxidizerEfficiency()
  {
    Dictionary<Tag, float> dictionary1 = new Dictionary<Tag, float>();
    dictionary1[SimHashes.LiquidOxygen.CreateTag()] = 0.0f;
    dictionary1[SimHashes.OxyRock.CreateTag()] = 0.0f;
    foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this.commandModule.GetComponent<AttachableBuilding>()))
    {
      OxidizerTank component = gameObject.GetComponent<OxidizerTank>();
      if ((Object) component != (Object) null)
      {
        foreach (KeyValuePair<Tag, float> keyValuePair in component.GetOxidizersAvailable())
        {
          if (dictionary1.ContainsKey(keyValuePair.Key))
          {
            Dictionary<Tag, float> dictionary2;
            Tag key;
            (dictionary2 = dictionary1)[key = keyValuePair.Key] = dictionary2[key] + keyValuePair.Value;
          }
        }
      }
    }
    float num1 = 0.0f;
    float num2 = 0.0f;
    foreach (KeyValuePair<Tag, float> keyValuePair in dictionary1)
    {
      num1 += keyValuePair.Value * RocketStats.oxidizerEfficiencies[keyValuePair.Key];
      num2 += keyValuePair.Value;
    }
    if ((double) num2 == 0.0)
      return 0.0f;
    return num1 / num2 * 100f;
  }

  public float GetTotalThrust()
  {
    float totalFuel = this.GetTotalFuel(false);
    float totalOxidizer = this.GetTotalOxidizer(false);
    float oxidizerEfficiency = this.GetAverageOxidizerEfficiency();
    RocketEngine mainEngine = this.GetMainEngine();
    if ((Object) mainEngine == (Object) null)
      return 0.0f;
    return (!mainEngine.requireOxidizer ? totalFuel * mainEngine.efficiency : Mathf.Min(totalFuel, totalOxidizer) * (mainEngine.efficiency * (oxidizerEfficiency / 100f))) + this.GetBoosterThrust();
  }

  public float GetBoosterThrust()
  {
    float num = 0.0f;
    foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this.commandModule.GetComponent<AttachableBuilding>()))
    {
      SolidBooster component = gameObject.GetComponent<SolidBooster>();
      if ((Object) component != (Object) null)
      {
        float amountAvailable1 = component.fuelStorage.GetAmountAvailable(ElementLoader.FindElementByHash(SimHashes.OxyRock).tag);
        float amountAvailable2 = component.fuelStorage.GetAmountAvailable(ElementLoader.FindElementByHash(SimHashes.Iron).tag);
        num += component.efficiency * Mathf.Min(amountAvailable1, amountAvailable2);
      }
    }
    return num;
  }

  public float GetEngineEfficiency()
  {
    RocketEngine mainEngine = this.GetMainEngine();
    if ((Object) mainEngine != (Object) null)
      return mainEngine.efficiency;
    return 0.0f;
  }

  public RocketEngine GetMainEngine()
  {
    RocketEngine rocketEngine = (RocketEngine) null;
    foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(this.commandModule.GetComponent<AttachableBuilding>()))
    {
      rocketEngine = gameObject.GetComponent<RocketEngine>();
      if ((Object) rocketEngine != (Object) null)
      {
        if (rocketEngine.mainEngine)
          break;
      }
    }
    return rocketEngine;
  }

  public float GetTotalOxidizableFuel()
  {
    return Mathf.Min(this.GetTotalFuel(false), this.GetTotalOxidizer(false));
  }
}
