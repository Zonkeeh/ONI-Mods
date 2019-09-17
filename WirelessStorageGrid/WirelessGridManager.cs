// Decompiled with JetBrains decompiler
// Type: WirelessAutomation.WirelessAutomationManager
// Assembly: WirelessAutomation-merged, Version=2019.9.5.0, Culture=neutral, PublicKeyToken=null
// MVID: C4EBA218-73FA-4D36-8F30-4D91E9958487
// Assembly location: C:\Users\Isaac\Documents\Klei\OxygenNotIncluded\mods\Steam\1718226085\WirelessAutomation.dll

using KSerialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WirelessStorageGrid
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public static class WirelessGridManager
    {
        public static string SliderTooltipKey = "STRINGS.UI.UISIDESCREENS.WIRELESS_AUTOMATION_SIDE_SCREEN.TOOLTIP";
        public static string SliderTooltip = "Select channel to tune in the device";
        public static string SliderTitleKey = "STRINGS.UI.UISIDESCREENS.WIRELESS_AUTOMATION_SIDE_SCREEN.TITLE";
        public static string SliderTitle = "Channel";

        private static List<MachineChannelInfo> RegisteredMachines { get; } = new List<MachineChannelInfo>();
        private static Dictionary<int, Storage> StorageMap { get; } = new Dictionary<int, Storage>();

        public static void ResetMachines()
        {
            WirelessGridManager.RegisteredMachines.Clear();
        }

        public static int RegisterEmitter(SignalEmitter emitter)
        {
            int num = 0;
            if (WirelessGridManager.Emitters.Count > 0)
                num = WirelessGridManager.Emitters.Max<SignalEmitter>((Func<SignalEmitter, int>)(e => e.Id)) + 1;
            emitter.Id = num;
            WirelessGridManager.Emitters.Add(emitter);
            return emitter.Id;
        }

        public static void UnregisterEmitter(int emitterId)
        {
            WirelessGridManager.Emitters.Remove(WirelessGridManager.Emitters.FirstOrDefault<SignalEmitter>((Func<SignalEmitter, bool>)(e => e.Id == emitterId)));
        }

        public static bool GetSignalForChannel(int channel)
        {
            SignalEmitter signalEmitter = WirelessGridManager.Emitters.FirstOrDefault<SignalEmitter>((Func<SignalEmitter, bool>)(e => e.EmitChannel == channel));
            if (signalEmitter == null)
                return false;
            return signalEmitter.Signal;
        }

        public static void SetEmitterSignal(int emitterId, bool signal)
        {
            SignalEmitter signalEmitter = WirelessGridManager.Emitters.FirstOrDefault<SignalEmitter>((Func<SignalEmitter, bool>)(e => e.Id == emitterId));
            if (signalEmitter == null)
                return;
            signalEmitter.Signal = signal;
        }

        public static void ChangeMachineChannel(int emitterId, int channel)
        {
            SignalEmitter signalEmitter = WirelessGridManager.Emitters.FirstOrDefault<SignalEmitter>((Func<SignalEmitter, bool>)(e => e.Id == emitterId));
            if (signalEmitter == null)
                return;
            signalEmitter.EmitChannel = channel;
        }
    }
}
