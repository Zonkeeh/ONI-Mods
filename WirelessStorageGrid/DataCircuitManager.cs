using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace WirelessStorageGrid
{
    public class DataCircuitManager
    {
        public static float ClockTickInterval = 0.1f;
        private List<ILogicUIElement> uiVisElements = new List<ILogicUIElement>();
        private float elapsedTime;
        private UtilityNetworkManager<DataCircuitNetwork, DataWire> conduitSystem;
        public System.Action<ILogicUIElement> onElemAdded;
        public System.Action<ILogicUIElement> onElemRemoved;

        public DataCircuitManager(
          UtilityNetworkManager<DataCircuitNetwork, DataWire> conduit_system)
        {
            this.conduitSystem = conduit_system;
            this.elapsedTime = 0.0f;
        }

        public void Sim200ms(float dt)
        {
            this.Refresh(dt);
        }

        private void Refresh(float dt)
        {
            if (this.conduitSystem.IsDirty)
            {
                this.conduitSystem.Update();
                this.PropagateSignals(true);
                this.elapsedTime = 0.0f;
            }
            else
            {
                if (this.conduitSystem.GetNetworks().Count <= 0 || !((UnityEngine.Object)SpeedControlScreen.Instance != (UnityEngine.Object)null) || SpeedControlScreen.Instance.IsPaused)
                    return;
                this.elapsedTime += dt;
                while ((double)this.elapsedTime > (double)DataCircuitManager.ClockTickInterval)
                {
                    this.elapsedTime -= DataCircuitManager.ClockTickInterval;
                    this.PropagateSignals(false);
                }
            }
        }

        private void PropagateSignals(bool force_send_events)
        {
            IList<UtilityNetwork> networks = WirelessGridManager.dataCircuitSystem.GetNetworks();
            foreach (DataCircuitNetwork dataCircuitNetwork in (IEnumerable<UtilityNetwork>)networks)
                dataCircuitNetwork.UpdateDataValue();
            foreach (DataCircuitNetwork dataCircuitNetwork in (IEnumerable<UtilityNetwork>)networks)
                dataCircuitNetwork.SendDataEvents(force_send_events);
        }

        public DataCircuitNetwork GetNetworkForCell(int cell)
        {
            return this.conduitSystem.GetNetworkForCell(cell) as DataCircuitNetwork;
        }

        public void AddVisElem(ILogicUIElement elem)
        {
            this.uiVisElements.Add(elem);
            if (this.onElemAdded == null)
                return;
            this.onElemAdded(elem);
        }

        public void RemoveVisElem(ILogicUIElement elem)
        {
            if (this.onElemRemoved != null)
                this.onElemRemoved(elem);
            this.uiVisElements.Remove(elem);
        }

        public ReadOnlyCollection<ILogicUIElement> GetVisElements()
        {
            return this.uiVisElements.AsReadOnly();
        }

        public static void ToggleNoWireConnected(bool show_missing_wire, GameObject go)
        {
            go.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.NoLogicWireConnected, show_missing_wire, (object)null);
        }

        private struct Signal
        {
            public int cell;
            public int value;

            public Signal(int cell, int value)
            {
                this.cell = cell;
                this.value = value;
            }
        }
    }
}
