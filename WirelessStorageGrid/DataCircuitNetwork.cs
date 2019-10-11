using FMOD.Studio;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace WirelessStorageGrid
{
    public class DataCircuitNetwork : UtilityNetwork
    {
        private List<DataWire> wires = new List<DataWire>();
        private List<IDataEventReceiver> receivers = new List<IDataEventReceiver>();
        private List<IDataEventSender> senders = new List<IDataEventSender>();
        private int previousValue = -1;
        private int outputValue;
        private bool resetting;

        public override void AddItem(int cell, object item)
        {
            if (item is DataWire)
                this.wires.Add((DataWire)item);
            else if (item is IDataEventReceiver)
                this.receivers.Add((IDataEventReceiver)item);
            else if (item is IDataEventSender)
                this.senders.Add((IDataEventSender)item);
        }

        public override void RemoveItem(int cell, object item)
        {
            if (item is DataWire)
                this.wires.Remove((DataWire)item);
            else if (item is IDataEventReceiver)
                this.receivers.Remove(item as IDataEventReceiver);
            else if (item is IDataEventSender)
                this.senders.Remove((IDataEventSender)item);
        }

        public override void ConnectItem(int cell, object item)
        {
            if (item is IDataEventReceiver)
                ((IDataNetworkConnection)item).OnDataNetworkConnectionChanged(true);
            else if (item is ILogicEventSender)
                ((IDataNetworkConnection)item).OnDataNetworkConnectionChanged(true);
        }

        public override void DisconnectItem(int cell, object item)
        {
            if (item is IDataEventReceiver)
            {
                IDataEventReceiver dataEventReceiver = item as IDataEventReceiver;
                dataEventReceiver.ReceiveDataEvent(0);
                dataEventReceiver.OnDataNetworkConnectionChanged(false);
            }
            else if (item is IDataEventSender)
                (item as IDataEventSender).OnDataNetworkConnectionChanged(false);
        }

        public override void Reset(UtilityNetworkGridNode[] grid)
        {
            this.resetting = true;
            this.previousValue = -1;
            this.outputValue = 0;

            foreach (DataWire wire in this.wires)
            {
                int cell = Grid.PosToCell(wire.transform.GetPosition());
                UtilityNetworkGridNode utilityNetworkGridNode = grid[cell];
                utilityNetworkGridNode.networkIdx = -1;
                grid[cell] = utilityNetworkGridNode;
            }
            this.wires.Clear();
            this.senders.Clear();
            this.receivers.Clear();
            this.resetting = false;
        }

        public void UpdateDataValue()
        {
            if (this.resetting)
                return;

            this.previousValue = this.outputValue;
            this.outputValue = 0;

            foreach (IDataEventSender sender in this.senders)
                sender.DataTick();
            foreach (IDataEventSender sender in this.senders)
                this.outputValue |= sender.GetDataValue();
        }

        public void SendDataEvents(bool force_send)
        {
            if (this.resetting || this.outputValue == this.previousValue && !force_send)
                return;

            foreach (IDataEventReceiver receiver in this.receivers)
                receiver.ReceiveDataEvent(this.outputValue);

            if (force_send)
                return;
            this.TriggerAudio(this.previousValue < 0 ? 0 : this.previousValue);
        }

        private void TriggerAudio(int old_value)
        {
            SpeedControlScreen instance1 = SpeedControlScreen.Instance;

            if (old_value == this.outputValue || !((Object)instance1 != (Object)null) || instance1.IsPaused)
                return;

            GridArea visibleArea = GridVisibleArea.GetVisibleArea();
            List<DataWire> dataWireList = new List<DataWire>();
            foreach (DataWire wire in this.wires)
                if (visibleArea.Min <= (Vector2)wire.transform.GetPosition() && (Vector2)wire.transform.GetPosition() <= visibleArea.Max)
                    dataWireList.Add(wire);

            if (dataWireList.Count <= 0)
                return;

            int index1 = Mathf.CeilToInt((float)(dataWireList.Count / 2));
            if (!((Object)dataWireList[index1] != (Object)null))
                return;

            EventInstance instance2 = KFMOD.BeginOneShot(GlobalAssets.GetSound("Logic_Circuit_Toggle", false), dataWireList[index1].transform.GetPosition());
            int num1 = (int)instance2.setParameterValue("wireCount", (float)(this.wires.Count % 24));
            int num2 = (int)instance2.setParameterValue("enabled", (float)this.outputValue);
            KFMOD.EndOneShot(instance2);
        }

        public int OutputValue
        {
            get
            {
                return this.outputValue;
            }
        }

        public List<DataWire> Wires
        {
            get
            {
                return this.wires;
            }
        }

        public ReadOnlyCollection<IDataEventSender> Senders
        {
            get
            {
                return this.senders.AsReadOnly();
            }
        }

        public ReadOnlyCollection<IDataEventReceiver> Receivers
        {
            get
            {
                return this.receivers.AsReadOnly();
            }
        }
    }

}
