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

namespace DuplicantLifecycle
{
    public static class DuplicantLifecycleConfigChecker
    {
        private static Config config;

        private static float maxAge;
        public static float MaxAge { get { return maxAge; } }
        private static float middleAge;
        public static float MiddleAge { get { return middleAge; } }
        private static float elderlyAge;
        public static float ElderlyAge { get { return elderlyAge; } }
        private static float dyingAge;
        public static float DyingAge { get { return dyingAge; } }

        private static bool usePercentage;
        public static bool UsePercentage { get { return usePercentage; } }
        private static float customBase;
        public static float CustomBase { get { return customBase; } }
        private static float probabilityDecrease;
        public static float ProbabilityDecrease { get { return probabilityDecrease; } }

        private static float youthAttributeMultiplier;
        public static float YouthAttributeMultiplier { get { return youthAttributeMultiplier; } }
        private static float middleAttributeMultiplier;
        public static float MiddleAttributeMultiplier { get { return middleAttributeMultiplier; } }
        private static float elderlyAttributeMultiplier;
        public static float ElderlyAttributeMultiplier { get { return elderlyAttributeMultiplier; } }
        private static float dyingAttributeMultiplier;
        public static float DyingAttributeMultiplier { get { return dyingAttributeMultiplier; } }
        private static float immortalAttributeMultiplier;
        public static float ImmortalAttributeMultiplier { get { return immortalAttributeMultiplier; } }

        public static class Mod_OnLoad
        {
            public static void OnLoad()
            {
                ConfigManager cm = new ConfigManager();
                DuplicantLifecycleConfigChecker.config = cm.LoadConfig<Config>(new Config());
                CheckConfigVariables();
            }

            private static bool MultiplierSanityCheck(float multiplier) => (Math.Abs(multiplier) <= 10f);
            private static bool AgeSanityCheck(float cycle) => (cycle > 0);


            private static void CheckConfigVariables()
            {
                float temp_maxAge = 200f;
                float temp_youthCutoff = 30f;
                float temp_middleCutoff = 140f;
                float temp_elderlyCutoff = 180f;
                bool temp_usePercentage = true;
                float temp_customBase = 4f;
                float temp_probabilityDecrease = 0f;
                float temp_youthMultiplier = 1f;
                float temp_middleAgedMultiplier = .35f;
                float temp_elderlyMultiplier = -.35f;
                float temp_dyingMultiplier = -1f;
                float temp_immortalMultiplier = .1f;

                if (AgeSanityCheck(config.MaxDuplicantAge))
                    temp_maxAge = config.MaxDuplicantAge;
                else
                    LogManager.LogError("Config file argument (MaxDuplicantAge) was invalid." +
                            "\nValue was not a postive floating point number: " + config.MaxDuplicantAge);

                if (AgeSanityCheck(config.YouthfulAgeCutoff))
                    temp_youthCutoff = config.YouthfulAgeCutoff;
                else
                    LogManager.LogError("Config file argument (YouthfulAgeCutoff) was invalid." +
                            "\nValue was not a postive floating point number: " + config.YouthfulAgeCutoff);

                if (AgeSanityCheck(config.MiddleAgedAgeCutoff))
                    temp_middleCutoff = config.MiddleAgedAgeCutoff;
                else
                    LogManager.LogError("Config file argument (MiddleAgedAgeCutoff) was invalid." +
                             "\nValue was not a postive floating point number: " + config.MiddleAgedAgeCutoff);

                if (AgeSanityCheck(config.ElderlyAgeCutoff))
                    temp_elderlyCutoff = config.ElderlyAgeCutoff;
                else
                    LogManager.LogError("Config file argument (ElderlyAgeCutoff) was invalid." +
                            "\nValue was not a postive floating point number: " + config.ElderlyAgeCutoff);


                if (!config.UsePercentageOfTotalStats)
                    temp_usePercentage = false;

                if (!temp_usePercentage && MultiplierSanityCheck(config.CustomStatBaseValue))
                    temp_customBase = config.CustomStatBaseValue;
                else if (!temp_usePercentage)
                    LogManager.LogError("Config file argument (CustomStatBaseValue) was invalid." +
                            "\nValue was not a floating point number between -10 and 10: " + config.CustomStatBaseValue);

                if (AgeSanityCheck(config.TimeToDieProbabilityDecrease))
                    temp_probabilityDecrease = config.TimeToDieProbabilityDecrease;
                else
                    LogManager.LogError("Config file argument (TimeToDieProbabilityIncrease) was invalid." +
                            "\nValue was not a postive floating point number: " + config.TimeToDieProbabilityDecrease);

                if (MultiplierSanityCheck(config.YouthAttributeMultiplier))
                    temp_youthMultiplier = config.YouthAttributeMultiplier;
                else
                    LogManager.LogError("Config file argument (YouthAttributeMultiplier) was invalid." +
                            "\nValue was not a floating point number between -10 and 10: " + config.YouthAttributeMultiplier);

                if (MultiplierSanityCheck(config.MiddleAgedAttributeMultiplier))
                    temp_middleAgedMultiplier = config.MiddleAgedAttributeMultiplier;
                else
                    LogManager.LogError("Config file argument (MiddleAgedAttributeMultiplier) was invalid." +
                            "\nValue was not a floating point number between -10 and 10: " + config.MiddleAgedAttributeMultiplier);

                if (MultiplierSanityCheck(config.ElderlyAttributeMultiplier))
                    temp_elderlyMultiplier = config.ElderlyAttributeMultiplier;
                else
                    LogManager.LogError("Config file argument (ElderlyAttributeMultiplier) was invalid." +
                            "\nValue was not a floating point number between -10 and 10: " + config.ElderlyAttributeMultiplier);

                if (MultiplierSanityCheck(config.DyingAttributeMultiplier))
                    temp_dyingMultiplier = config.DyingAttributeMultiplier;
                else
                    LogManager.LogError("Config file argument (DyingAttributeMultiplier) was invalid." +
                            "\nValue was not a floating point number between -10 and 10: " + config.DyingAttributeMultiplier);

                if (MultiplierSanityCheck(config.ImmortalAttributeMultiplier))
                    temp_immortalMultiplier = config.ImmortalAttributeMultiplier;
                else
                    LogManager.LogError("Config file argument (ImmortalAttributeMultiplier) was invalid." +
                            "\nValue was not a floating point number between -10 and 10: " + config.ImmortalAttributeMultiplier);


                bool useDefaultCutoffs = false;

                if (temp_middleCutoff <= temp_youthCutoff)
                {
                    useDefaultCutoffs = true;
                    LogManager.LogError("Config file argument (MiddleAgedAgeCutoff) was less than YouthfulAgeCutoff: " + 
                        config.MiddleAgedAgeCutoff + " < " + config.YouthfulAgeCutoff +
                        "\nTherefore default cutoffs will be used.");
                }

                if (temp_elderlyCutoff <= temp_middleCutoff)
                {
                    useDefaultCutoffs = true;
                    LogManager.LogError("Config file argument (ElderlyAgeCutoff) was less than MiddleAgedAgeCutoff: " +
                        config.ElderlyAgeCutoff + " < " + config.MiddleAgedAgeCutoff +
                        "\nTherefore default cutoffs will be used.");
                }

                if (temp_maxAge <= temp_elderlyCutoff)
                {
                    useDefaultCutoffs = true;
                    LogManager.LogError("Config file argument (MaxDuplicantAge) was less than ElderlyAgeCutoff: " +
                        config.MaxDuplicantAge + " < " + config.ElderlyAgeCutoff +
                        "\nTherefore default cutoffs will be used.");
                }

                if (useDefaultCutoffs)
                {
                    DuplicantLifecycleConfigChecker.maxAge = 200f;
                    DuplicantLifecycleConfigChecker.middleAge = 30f;
                    DuplicantLifecycleConfigChecker.elderlyAge = 140f;
                    DuplicantLifecycleConfigChecker.dyingAge = 180f;
                }
                else
                {
                    DuplicantLifecycleConfigChecker.maxAge = temp_maxAge;
                    DuplicantLifecycleConfigChecker.middleAge = temp_youthCutoff;
                    DuplicantLifecycleConfigChecker.elderlyAge = temp_middleCutoff;
                    DuplicantLifecycleConfigChecker.dyingAge = temp_elderlyCutoff;
                }


                DuplicantLifecycleConfigChecker.usePercentage = temp_usePercentage;
                DuplicantLifecycleConfigChecker.customBase = temp_customBase;
                DuplicantLifecycleConfigChecker.probabilityDecrease = temp_probabilityDecrease;
                DuplicantLifecycleConfigChecker.youthAttributeMultiplier = temp_youthMultiplier;
                DuplicantLifecycleConfigChecker.middleAttributeMultiplier = temp_middleAgedMultiplier;
                DuplicantLifecycleConfigChecker.elderlyAttributeMultiplier = temp_elderlyMultiplier;
                DuplicantLifecycleConfigChecker.dyingAttributeMultiplier = temp_dyingMultiplier;
                DuplicantLifecycleConfigChecker.immortalAttributeMultiplier = temp_immortalMultiplier;

                #if DEBUG
                    LogManager.LogDebug("CheckConfigVariables()\n" +
                        "MaxDuplicantAge: " + DuplicantLifecycleConfigChecker.maxAge + "\n" +
                        "YouthfulAgeCutoff: " + DuplicantLifecycleConfigChecker.middleAge + "\n" +
                        "MiddleAgedAgeCutoff: " + DuplicantLifecycleConfigChecker.elderlyAge + "\n" +
                        "ElderlyAgeCutoff: " + DuplicantLifecycleConfigChecker.elderlyAge + "\n" +
                        "UsePercentageOfTotalStats: " + DuplicantLifecycleConfigChecker.usePercentage + "\n" +
                        "CustomStatBaseValue: " + DuplicantLifecycleConfigChecker.customBase + "\n" +
                        "TimeToDieProbabilityIncrease: " + DuplicantLifecycleConfigChecker.probabilityDecrease + "\n" +
                        "YouthAttributeMultiplier: " + DuplicantLifecycleConfigChecker.youthAttributeMultiplier + "\n" +
                        "MiddleAgedAttributeMultiplier: " + DuplicantLifecycleConfigChecker.middleAttributeMultiplier + "\n" +
                        "ElderlyAttributeMultiplier: " + DuplicantLifecycleConfigChecker.elderlyAttributeMultiplier + "\n" +
                        "DyingAttributeMultiplier: " + DuplicantLifecycleConfigChecker.dyingAttributeMultiplier + "\n" +
                        "ImmortalAttributeMultiplier: " + DuplicantLifecycleConfigChecker.immortalAttributeMultiplier + "\n"
                    );
                #endif
            }
        }
    }
}
