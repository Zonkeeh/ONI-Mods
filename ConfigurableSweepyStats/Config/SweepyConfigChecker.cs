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

namespace ConfigurableSweepyStats
{
    public static class SweepyConfigChecker
    {
        private static Config config;

        public static bool DebugMode { get { return debugMode; } }
        private static bool debugMode;

        public static float StorageCapacity { get { return storageCapacity; } }
        private static float storageCapacity;

        public static bool SweepyUsesPower { get { return sweepyUsesPower; } }
        private static bool sweepyUsesPower;

        public static float BatteryCapacity { get { return batteryCapacity; } }
        private static float batteryCapacity;

        public static float BatteryDepletionRate { get { return batteryDepletionRate; } }
        private static float batteryDepletionRate;

        public static float BaseMovementSpeed { get { return baseMovementSpeed; } }
        private static float baseMovementSpeed;

        public static int BaseProbingRadius { get { return baseProbingRadius; } }
        private static int baseProbingRadius;

        public static bool UseCustomSliders { get { return useCustomSliders; } }
        private static bool useCustomSliders;

        public static bool BatteryDrainBasedOnSpeed { get { return batteryDrainBasedOnSpeed; } }
        private static bool batteryDrainBasedOnSpeed;

        public static float DrainSpeedMultiplier { get { return drainSpeedMultiplier; } }
        private static float drainSpeedMultiplier;

        public static float MinSpeedSliderValue { get { return minSpeedSliderValue; } }
        private static float minSpeedSliderValue;

        public static float MaxSpeedSliderValue { get { return maxSpeedSliderValue; } }
        private static float maxSpeedSliderValue;

        public static float MinProbingSliderValue { get { return minProbingSliderValue; } }
        private static float minProbingSliderValue;

        public static float MaxProbingSliderValue { get { return maxProbingSliderValue; } }
        private static float maxProbingSliderValue;

        public static bool StationUsesPower { get { return stationUsesPower; } }
        private static bool stationUsesPower;

        public static bool StationCanOverheat { get { return stationCanOverheat; } }
        private static bool stationCanOverheat;

        public static bool StationCanFlood { get { return stationCanFlood; } }
        private static bool stationCanFlood;

        public static float StationEnergyConsumption { get { return stationEnergyConsumption; } }
        private static float stationEnergyConsumption;

        public static float StationStorageCapacity { get { return stationStorageCapacity; } }
        private static float stationStorageCapacity;

        public static class Mod_OnLoad
        {
            public static void OnLoad()
            {
                ConfigManager cm = new ConfigManager();
                SweepyConfigChecker.config = cm.LoadConfig<Config>(new Config());
                CheckConfigVariables();
            }

            private static bool FloatSanityCheck(float num) => num>0 && num<100001;

            private static void CheckConfigVariables()
            {
                bool _DebugMode = false;
                float _StorageCapacity = 500f;
                bool _SweepyUsesPower = true;
                float _BatteryDepletionRate = 40f;           
                float _BatteryCapacity = 21000f;                  
                float _BaseMovementSpeed = 1f;          
                int _BaseProbingRadius = 32;   
                bool _UseCustomSliders = true;          
                bool _BatteryDrainBasedOnSpeed = true;
                float _DrainSpeedMultiplier = 1f;
                float _MinSpeedSliderValue = 1f;          
                float _MaxSpeedSliderValue = 5f;           
                float _MinProbingSliderValue = 1f;            
                float _MaxProbingSliderValue = 64f;
                bool _StationUsesPower = true;
                bool _StationCanOverheat = false;
                bool _StationCanFlood = false;
                float _StationEnergyConsumption = 240f;
                float _StationStorageCapacity = 1000f;

                _DebugMode = config.DebugMode;

                if (FloatSanityCheck(config.StorageCapacity))
                    _StorageCapacity = config.StorageCapacity;
                else
                    LogManager.LogError("Config file argument (StorageCapacity) was invalid, default was used. Value was not a positive floating point number (between 1 and 100,000): " + config.StorageCapacity);

                _SweepyUsesPower = config.SweepyUsesPower;

                if (_SweepyUsesPower && FloatSanityCheck(config.BatteryDepletionRate))
                    _BatteryDepletionRate = config.BatteryDepletionRate;
                else if (_SweepyUsesPower)
                    LogManager.LogError("Config file argument (BatteryDepletionRate) was invalid, default was used. Value was not a positive floating point number (between 1 and 100,000): " + config.BatteryDepletionRate);

                if (_SweepyUsesPower && FloatSanityCheck(config.BatteryCapacity))
                    _BatteryCapacity = config.BatteryCapacity;
                else if (_SweepyUsesPower)
                    LogManager.LogError("Config file argument (BatteryCapacity) was invalid, default was used. Value was not a positive floating point number (between 1 and 100,000): " + config.BatteryCapacity);

                if (FloatSanityCheck(config.BaseMovementSpeed))
                    _BaseMovementSpeed = config.BaseMovementSpeed;
                else
                    LogManager.LogError("Config file argument (BaseMovementSpeed) was invalid, default was used. Value was not a positive floating point number (between 1 and 100,000): " + config.BaseMovementSpeed);

                if (FloatSanityCheck(config.BaseProbingRadius))
                    _BaseProbingRadius = config.BaseProbingRadius;
                else
                    LogManager.LogError("Config file argument (BaseProbingRadius) was invalid, default was used. Value was not a positive Integer (between 1 and 100,000): " + config.BaseProbingRadius);

                _UseCustomSliders = config.UseCustomSliders;

                if (_UseCustomSliders) 
                {
                    _BatteryDrainBasedOnSpeed = !_SweepyUsesPower ? false : config.BatteryDrainBasedOnSpeed;

                    if (_BatteryDrainBasedOnSpeed && FloatSanityCheck(config.DrainSpeedMultiplier))
                        _DrainSpeedMultiplier = config.DrainSpeedMultiplier;
                    else if(_BatteryDrainBasedOnSpeed)
                        LogManager.LogError("Config file argument (DrainSpeedMultiplier) was invalid, default was used. Value was not a positive floating point number (between 1 and 100,000): " + config.DrainSpeedMultiplier);

                    if (FloatSanityCheck(config.MinSpeedSliderValue))
                        _MinSpeedSliderValue = config.MinSpeedSliderValue<_BaseMovementSpeed ? config.MinSpeedSliderValue : _BaseMovementSpeed;
                    else
                        LogManager.LogError("Config file argument (MinSpeedSliderValue) was invalid, default was used. Value was not a positive floating point number (between 1 and 100,000): " + config.MinSpeedSliderValue);

                    if (FloatSanityCheck(config.MaxSpeedSliderValue))
                        _MaxSpeedSliderValue = config.MaxSpeedSliderValue>_BaseMovementSpeed ? config.MaxSpeedSliderValue : _BaseMovementSpeed;
                    else
                        LogManager.LogError("Config file argument (MaxSpeedSliderValue) was invalid, default was used. Value was not a positive floating point number (between 1 and 100,000): " + config.MaxSpeedSliderValue);

                    if (FloatSanityCheck(config.MinProbingSliderValue))
                        _MinProbingSliderValue = config.MinProbingSliderValue<_BaseProbingRadius ? config.MinProbingSliderValue : _BaseProbingRadius;
                    else
                        LogManager.LogError("Config file argument (MinProbingSliderValue) was invalid, default was used. Value was not a positive floating point number (between 1 and 100,000): " + config.MinProbingSliderValue);

                    if (FloatSanityCheck(config.MaxProbingSliderValue))
                        _MaxProbingSliderValue = config.MaxProbingSliderValue>_BaseProbingRadius ? config.MaxProbingSliderValue : _BaseProbingRadius;
                    else
                        LogManager.LogError("Config file argument (MaxProbingSliderValue) was invalid, default was used. Value was not a positive floating point number (between 1 and 100,000): " + config.MaxProbingSliderValue);
                }

                _StationUsesPower = config.StationUsesPower;
                _StationCanOverheat = config.StationCanOverheat;
                _StationCanFlood = config.StationCanFlood;

                if (_StationUsesPower && FloatSanityCheck(config.StationEnergyConsumption))
                    _StationEnergyConsumption = config.StationEnergyConsumption;
                else if (_StationUsesPower)
                    LogManager.LogError("Config file argument (StationEnergyConsumption) was invalid, default was used. Value was not a positive floating point number (between 1 and 100,000): " + config.StationEnergyConsumption);

                if (FloatSanityCheck(config.StationStorageCapacity))
                    _StationStorageCapacity = config.StationStorageCapacity;
                else
                    LogManager.LogError("Config file argument (StationStorageCapacity) was invalid, default was used. Value was not a positive Integer (between 1 and 100,000): " + config.StationStorageCapacity);

                debugMode = _DebugMode;
                storageCapacity = _StorageCapacity;
                sweepyUsesPower = _SweepyUsesPower;
                batteryDepletionRate = _BatteryDepletionRate;
                batteryCapacity = _BatteryCapacity;
                baseMovementSpeed = _BaseMovementSpeed;
                baseProbingRadius = _BaseProbingRadius;
                useCustomSliders = _UseCustomSliders;
                batteryDrainBasedOnSpeed = _BatteryDrainBasedOnSpeed;
                drainSpeedMultiplier = _DrainSpeedMultiplier;
                minSpeedSliderValue = _MinSpeedSliderValue;
                maxSpeedSliderValue = _MaxSpeedSliderValue;
                minProbingSliderValue = _MinProbingSliderValue;
                maxProbingSliderValue = _MaxProbingSliderValue;
                stationUsesPower = _StationUsesPower;
                stationCanOverheat = _StationCanOverheat;
                stationCanFlood = _StationCanFlood;
                stationEnergyConsumption = _StationEnergyConsumption;
                stationStorageCapacity = _StationStorageCapacity;



                if(_DebugMode)
                LogManager.LogDebug("CheckConfigVariables()\n" +
                    "debugMode: " + debugMode + "\n" +
                    "storageCapacity: " + storageCapacity + "\n" +
                    "sweepyUsesPower: " + sweepyUsesPower + "\n" +
                    "batteryDepletionRate: " + batteryDepletionRate + "\n" +
                    "batteryCapacity: " + batteryCapacity + "\n" +
                    "baseMovementSpeed: " + baseMovementSpeed + "\n" +
                    "baseProbingRadius: " + baseProbingRadius + "\n" +
                    "useCustomSliders: " + useCustomSliders + "\n" +
                    "batteryDrainBasedOnSpeed: " + batteryDrainBasedOnSpeed + "\n" +
                    "drainSpeedMultiplier: " + drainSpeedMultiplier + "\n" +
                    "minSpeedSliderValue: " + minSpeedSliderValue + "\n" +
                    "maxSpeedSliderValue: " + maxSpeedSliderValue + "\n" +
                    "minProbingSliderValue: " + minProbingSliderValue + "\n" +
                    "maxProbingSliderValue: " + maxProbingSliderValue + "\n" +
                    "stationUsesPower: " + stationUsesPower + "\n" +
                    "stationCanOverheat: " + stationCanOverheat + "\n" +
                    "stationCanFlood: " + stationCanFlood + "\n" +
                    "stationEnergyConsumption: " + stationEnergyConsumption + "\n" +
                    "stationStorageCapacity: " + stationStorageCapacity + "\n"
                );
            }
        }
    }
}
