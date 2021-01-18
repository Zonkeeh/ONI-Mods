using Zolibrary.Logging;
using Zolibrary.Config;

namespace WirelessPower.Configuration
{
    public static class WirelessPowerConfigChecker
    {
        private static WirelessPowerConfig config;

        private static int _maxNumberOfChannels = 10;
        public static int MaxNumberOfChannels => _maxNumberOfChannels;

        private static bool _useColourInStatusItems = false;
        public static bool UseColourInStatusItems => _useColourInStatusItems;

        private static bool _useEnergyFalloff = true;
        public static bool UseEnergyFalloff => _useEnergyFalloff;

        private static float _customEnergyFalloffPercentage = 0.05f;
        public static float CustomEnergyFalloffPercentage => _customEnergyFalloffPercentage;

        private static float _batteryCapacity = 40000f;
        public static float BatteryCapacity => _batteryCapacity;

        private static float _batteryJoulesLostPerSecond = 0.75f;
        public static float BatteryJoulesLostPerSecond => _batteryJoulesLostPerSecond;

        private static float _defaultTransfer = 200f;
        public static float DefaultTransfer => _defaultTransfer;

        private static float _minTransfer = 10f;
        public static float MinTransfer => _minTransfer;

        private static float _maxTransfer = 5000f;
        public static float MaxTransfer => _maxTransfer;

        private static bool _buildUsesOnlySteel = true;
        public static bool BuildUsesOnlySteel => _buildUsesOnlySteel;

        private static float _batteryBuildTime = 60f;
        public static float BatteryBuildTime => _batteryBuildTime;

        private static float _batteryMaterialCost = 400f;
        public static float BatteryMaterialCost => _batteryMaterialCost;

        private static float _senderReceiverBuildTime = 30f;
        public static float SenderReceiverBuildTime => _senderReceiverBuildTime;

        private static float _senderReceiverMaterialCost = 200f;
        public static float SenderReceiverMaterialCost => _senderReceiverMaterialCost;

        public static class Mod_OnLoad
        {
            public static void OnLoad()
            {
                ConfigManager cm = new ConfigManager();
                WirelessPowerConfigChecker.config = cm.LoadConfig<WirelessPowerConfig>(new WirelessPowerConfig());
                CheckConfigVariables();
            }

            private static bool FloatSanityCheck(float num, float min, float max) => num >= min && num <= max;

            private static bool IntegerSanityCheck(int num, int min, int max) => num >= min && num <= max;

            private static void CheckConfigVariables()
            {
                if (IntegerSanityCheck(config.MaxNumberOfChannels, 1, 999))
                    WirelessPowerConfigChecker._maxNumberOfChannels = config.MaxNumberOfChannels;
                else
                    LogManager.LogError("Config file argument (MaxNumberOfChannels) was invalid, therefore the default of '" + WirelessPowerConfigChecker._maxNumberOfChannels + "' was used. \n\tValue was not a positive integer (between 1 and 9999): " + config.MaxNumberOfChannels);

                if (config.UseColourInStatusItems != WirelessPowerConfigChecker._useColourInStatusItems)
                    WirelessPowerConfigChecker._useColourInStatusItems = config.UseColourInStatusItems;



                if (config.UseEnergyFalloff != WirelessPowerConfigChecker._useEnergyFalloff)
                    WirelessPowerConfigChecker._useEnergyFalloff = config.UseEnergyFalloff;

                if (FloatSanityCheck(config.CustomEnergyFalloffPercentage, 0f, 1f))
                    WirelessPowerConfigChecker._customEnergyFalloffPercentage = config.CustomEnergyFalloffPercentage;
                else
                    LogManager.LogError("Config file argument (CustomEnergyFalloffPercentage) was invalid, therefore the default of '" + WirelessPowerConfigChecker._customEnergyFalloffPercentage + "' was used. \n\tValue was not a positive float (between 0 and 1): " + config.CustomEnergyFalloffPercentage);

                if (FloatSanityCheck(config.BatteryCapacity, 1f, 1000000f))
                    WirelessPowerConfigChecker._batteryCapacity = config.BatteryCapacity;
                else
                    LogManager.LogError("Config file argument (BatteryCapacity) was invalid, therefore the default of '" + WirelessPowerConfigChecker._batteryCapacity + "' was used. \n\tValue was not a positive float (between 1 and 1,000,000): " + config.BatteryCapacity);

                if (FloatSanityCheck(config.BatteryJoulesLostPerSecond, 0f, 10f))
                    WirelessPowerConfigChecker._batteryJoulesLostPerSecond = config.BatteryJoulesLostPerSecond;
                else
                    LogManager.LogError("Config file argument (BatteryJoulesLostPerSecond) was invalid, therefore the default of '" + WirelessPowerConfigChecker._batteryJoulesLostPerSecond + "' was used. \n\tValue was not a positive float (between 0 and 10): " + config.BatteryJoulesLostPerSecond);
                


                if (FloatSanityCheck(config.MinTransfer, 1f, 100000f))
                    WirelessPowerConfigChecker._minTransfer = config.MinTransfer;
                else
                    LogManager.LogError("Config file argument (MinTransfer) was invalid, therefore the default of '" + WirelessPowerConfigChecker._minTransfer + "' was used. \n\tValue was not a positive float (between 1 and 100,000): " + config.MinTransfer);

                if (FloatSanityCheck(config.MaxTransfer, 1f, 100000f) && config.MaxTransfer > WirelessPowerConfigChecker._minTransfer)
                    WirelessPowerConfigChecker._maxTransfer = config.MaxTransfer;
                else if (config.MaxTransfer <= WirelessPowerConfigChecker._minTransfer)
                    LogManager.LogError("Config file argument (MaxTransfer) was invalid, therefore the default of '" + WirelessPowerConfigChecker._maxTransfer + "' was used. \n\tValue was not greater than the minimum transfer amount. \n\t\tMinTransfer:" + config.MinTransfer + "\n\t\tMaxTransfer:" + config.MaxTransfer);
                else
                    LogManager.LogError("Config file argument (MaxTransfer) was invalid, therefore the default of '" + WirelessPowerConfigChecker._maxTransfer + "' was used. \n\tValue was not a positive float (between 1 and 100,000): " + config.MaxTransfer);

                if (FloatSanityCheck(config.DefaultTransfer, 1f, 100000f) && config.DefaultTransfer >= WirelessPowerConfigChecker._minTransfer && config.DefaultTransfer <= WirelessPowerConfigChecker._maxTransfer)
                    WirelessPowerConfigChecker._defaultTransfer = config.DefaultTransfer;
                else
                    LogManager.LogError("Config file argument (DefaultTransfer) was invalid, therefore the default of '" + WirelessPowerConfigChecker._defaultTransfer + "' was used. \n\tValue was not a positive float (between 1 and 100,000) or between the set values for minimum and maximum transfer (between" + WirelessPowerConfigChecker._minTransfer + " and" + WirelessPowerConfigChecker._maxTransfer + "): " + config.DefaultTransfer);



                if (config.BuildUsesOnlySteel != WirelessPowerConfigChecker._buildUsesOnlySteel)
                    WirelessPowerConfigChecker._buildUsesOnlySteel = config.BuildUsesOnlySteel;

                if (IntegerSanityCheck(config.BatteryBuildTime, 1, 600))
                    WirelessPowerConfigChecker._batteryBuildTime = config.BatteryBuildTime;
                else
                    LogManager.LogError("Config file argument (BatteryBuildTime) was invalid, therefore the default of '" + WirelessPowerConfigChecker._batteryBuildTime + "' was used. \n\tValue was not a positive integer (between 1 and 600): " + config.BatteryBuildTime);


                if (FloatSanityCheck(config.BatteryMaterialCost, 1f, 10000f))
                    WirelessPowerConfigChecker._batteryMaterialCost = config.BatteryMaterialCost;
                else
                    LogManager.LogError("Config file argument (BatteryMaterialCost) was invalid, therefore the default of '" + WirelessPowerConfigChecker._batteryMaterialCost + "' was used. \n\tValue was not a positive float (between 1 and 10,000): " + config.BatteryMaterialCost);

                if (IntegerSanityCheck(config.SenderReceiverBuildTime, 1, 600))
                    WirelessPowerConfigChecker._senderReceiverBuildTime = config.SenderReceiverBuildTime;
                else
                    LogManager.LogError("Config file argument (SenderReceiverBuildTime) was invalid, therefore the default of '" + WirelessPowerConfigChecker._senderReceiverBuildTime + "' was used. \n\tValue was not a positive integer (between 1 and 600): " + config.SenderReceiverBuildTime);


                if (FloatSanityCheck(config.SenderReceiverMaterialCost, 1f, 10000f))
                    WirelessPowerConfigChecker._senderReceiverMaterialCost = config.SenderReceiverMaterialCost;
                else
                    LogManager.LogError("Config file argument (SenderReceiverMaterialCost) was invalid, therefore the default of '" + WirelessPowerConfigChecker._senderReceiverMaterialCost + "' was used. \n\tValue was not a positive float (between 1 and 10,000): " + config.SenderReceiverMaterialCost);


#if DEBUG
                LogManager.LogDebug("CheckConfigVariables()\n" +
                    "UseColourInStatusItems: " + WirelessPowerConfigChecker._useColourInStatusItems + "\n" +
                    "MaxNumberOfChannels: " + WirelessPowerConfigChecker.MaxNumberOfChannels + "\n" +
                    "UseEnergyFalloff: " + WirelessPowerConfigChecker._useEnergyFalloff + "\n" +
                    "CustomEnergyFalloffPercentage: " + WirelessPowerConfigChecker._customEnergyFalloffPercentage + "\n" +
                    "BatteryCapacity: " + WirelessPowerConfigChecker._batteryCapacity + "\n" +
                    "BatteryJoulesLostPerSecond: " + WirelessPowerConfigChecker._batteryJoulesLostPerSecond + "\n" +
                    "DefaultTransfer: " + WirelessPowerConfigChecker._defaultTransfer + "\n" +
                    "MinTransfer: " + WirelessPowerConfigChecker._minTransfer + "\n" +
                    "MaxTransfer: " + WirelessPowerConfigChecker._maxTransfer + "\n" +
                    "BuildUsesOnlySteel: " + WirelessPowerConfigChecker._buildUsesOnlySteel + "\n" +
                    "BatteryBuildTime: " + WirelessPowerConfigChecker._batteryBuildTime + "\n" +
                    "BatteryMaterialCost: " + WirelessPowerConfigChecker._batteryMaterialCost + "\n" +
                    "SenderReceiverBuildTime: " + WirelessPowerConfigChecker._senderReceiverBuildTime + "\n" +
                    "SenderReceiverMaterialCost: " + WirelessPowerConfigChecker._senderReceiverMaterialCost + "\n"
                );
#endif
            }
        }
    }
}
