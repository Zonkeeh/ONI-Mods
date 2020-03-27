using System;
using System.Collections.Generic;
using Database;
using Harmony;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;
using Zolibrary.Logging;
using Zolibrary.Config;
using Zolibrary.Utilities;

namespace Trashcans
{
    public static class TrashcansConfigChecker
    {
        private static Config config;

        public static bool RequiresPower { get { return requiresPower; } }
        private static bool requiresPower;

        public static bool CanOverheat { get { return canOverheat; } }
        private static bool canOverheat;

        public static bool CanFlood { get { return canFlood; } }
        private static bool canFlood;

        public static float EnergyConsumptionWhenActive { get { return energyConsumptionWhenActive; } }
        private static float energyConsumptionWhenActive;

        public static bool EnableAutoTrash { get { return enableAutoTrash; } }
        private static bool enableAutoTrash;

        public static float MaxAutoTrashInterval { get { return maxAutoTrashInterval; } }
        private static float maxAutoTrashInterval;

        public static float GasTrashCapicityKG { get { return gasTrashCapicityKG; } }
        private static float gasTrashCapicityKG;

        public static float LiquidTrashCapicityKG { get { return liquidTrashCapicityKG; } }
        private static float liquidTrashCapicityKG;

        public static float SolidTrashCapicityKG { get { return solidTrashCapicityKG; } }
        private static float solidTrashCapicityKG;

        public static class Mod_OnLoad
        {
            public static void OnLoad()
            {
                ConfigManager cm = new ConfigManager();
                TrashcansConfigChecker.config = cm.LoadConfig<Config>(new Config());
                CheckConfigVariables();
            }

            private static bool FloatSanityCheck(float num) => num>0 && num<3601;

            private static bool CapacitySanityCheck(float cap) => cap>99 && cap<100001;

            private static void CheckConfigVariables()
            {
                bool temp_requiresPower = true;
                bool temp_canOverheat = true;
                bool temp_canFlood = false;
                float temp_energyConsumptionWhenActive = 60f;
                bool temp_enableAutoTrash = true;
                float temp_maxAutoTrashInterval = 3600f;
                float temp_gasTrashCapicityKG = 100f;
                float temp_liquidTrashCapicityKG = 1000f;
                float temp_solidTrashCapicityKG = 1000f;

                if (!config.RequiresPower)
                    temp_requiresPower = false;

                if (!config.CanOverheat)
                    temp_canOverheat = false;

                if (config.CanFlood)
                    temp_canFlood = true;

                if (temp_requiresPower && FloatSanityCheck(config.EnergyConsumptionWhenActive))
                    temp_energyConsumptionWhenActive = config.EnergyConsumptionWhenActive;
                else if (temp_requiresPower)
                    LogManager.LogError("Config file argument (EnergyConsumptionWhenActive) was invalid, default was used. Value was not a positive floating point number (between 1 and 3600): " + config.EnergyConsumptionWhenActive);
                else
                    LogManager.Log("Config file argument (EnergyConsumptionWhenActive) was ignored. RequiresPower is false therefore unused: " + config.EnergyConsumptionWhenActive);

                if (!config.EnableAutoTrash)
                    temp_enableAutoTrash = false;

                if (temp_enableAutoTrash && FloatSanityCheck(config.MaxAutoTrashInterval))
                    temp_maxAutoTrashInterval = config.MaxAutoTrashInterval;
                else if (temp_enableAutoTrash)
                    LogManager.LogError("Config file argument (MaxAutoTrashInterval) was invalid, default was used. Value was not a positive floating point number (between 1 and 3600): " + config.MaxAutoTrashInterval);
                else
                    LogManager.Log("Config file argument (MaxAutoTrashInterval) was ignored. EnableAutoTrash is false therefore unused: " + config.MaxAutoTrashInterval);

                if (CapacitySanityCheck(config.GasTrashCapicityKG))
                    temp_gasTrashCapicityKG = config.GasTrashCapicityKG;
                else
                    LogManager.LogError("Config file argument (GasTrashCapicityKG) was invalid, default was used. Value was not a positive floating point number (between 100 and 100,000): " + config.GasTrashCapicityKG);

                if (CapacitySanityCheck(config.LiquidTrashCapicityKG))
                    temp_liquidTrashCapicityKG = config.LiquidTrashCapicityKG;
                else
                    LogManager.LogError("Config file argument (LiquidTrashCapicityKG) was invalid, default was used. Value was not a positive floating point number (between 100 and 100,000): " + config.LiquidTrashCapicityKG);

                if (CapacitySanityCheck(config.SolidTrashCapicityKG))
                    temp_solidTrashCapicityKG = config.SolidTrashCapicityKG;
                else
                    LogManager.LogError("Config file argument (SolidTrashCapicityKG) was invalid, default was used. Value was not a positive floating point number (between 100 and 100,000): " + config.SolidTrashCapicityKG);

                requiresPower = temp_requiresPower;
                canOverheat = temp_canOverheat;
                canFlood = temp_canFlood;
                energyConsumptionWhenActive = temp_energyConsumptionWhenActive;
                enableAutoTrash = temp_enableAutoTrash;
                maxAutoTrashInterval = temp_maxAutoTrashInterval;
                gasTrashCapicityKG = temp_gasTrashCapicityKG;
                liquidTrashCapicityKG = temp_liquidTrashCapicityKG;
                solidTrashCapicityKG = temp_solidTrashCapicityKG;


#if DEBUG
                LogManager.LogDebug("CheckConfigVariables()\n" +
                    "RequiresPower: " + RequiresPower + "\n" +
                    "CanOverheat: " + CanOverheat + "\n" +
                    "CanFlood: " + CanFlood + "\n" +
                    "EnergyConsumptionWhenActive: " + EnergyConsumptionWhenActive + "\n" +
                    "EnableAutoTrash: " + EnableAutoTrash + "\n" +
                    "MaxAutoTrashInterval: " + MaxAutoTrashInterval + "\n" +
                    "GasTrashCapicityKG: " + GasTrashCapicityKG + "\n" +
                    "LiquidTrashCapicityKG: " + LiquidTrashCapicityKG + "\n" +
                    "SolidTrashCapicityKG: " + SolidTrashCapicityKG + "\n"
                );
#endif
            }
        }
    }
}
