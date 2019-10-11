using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace WirelessStorageGrid
{
    [SkipSaveFileSerialization]
    public class DataWire : KMonoBehaviour, IFirstFrameCallback, IHaveUtilityNetworkMgr, IBridgedNetworkItem
    {
        public static readonly KAnimHashedString OutlineSymbol = new KAnimHashedString("outline");
        private static readonly EventSystem.IntraObjectHandler<DataWire> OnBuildingBrokenDelegate = new EventSystem.IntraObjectHandler<DataWire>((System.Action<DataWire, object>)((component, data) => component.OnBuildingBroken(data)));
        private static readonly EventSystem.IntraObjectHandler<DataWire> OnBuildingFullyRepairedDelegate = new EventSystem.IntraObjectHandler<DataWire>((System.Action<DataWire, object>)((component, data) => component.OnBuildingFullyRepaired(data)));
        [SerializeField]
        private bool disconnected = true;
        private System.Action firstFrameCallback;

        protected override void OnSpawn()
        {
            base.OnSpawn();
            Game.Instance.logicCircuitSystem.AddToNetworks(Grid.PosToCell(this.transform.GetPosition()), (object)this, false);
            this.Subscribe<DataWire>(774203113, DataWire.OnBuildingBrokenDelegate);
            this.Subscribe<DataWire>(-1735440190, DataWire.OnBuildingFullyRepairedDelegate);
            this.GetComponent<KBatchedAnimController>().SetSymbolVisiblity(DataWire.OutlineSymbol, false);
        }

        protected override void OnCleanUp()
        {
            int cell = Grid.PosToCell(this.transform.GetPosition());
            BuildingComplete component = this.GetComponent<BuildingComplete>();
            if (component.Def.ReplacementLayer == ObjectLayer.NumLayers || (UnityEngine.Object)Grid.Objects[cell, (int)component.Def.ReplacementLayer] == (UnityEngine.Object)null)
                WirelessGridManager.dataCircuitSystem.RemoveFromNetworks(cell, (object)this, false);
            this.Unsubscribe<DataWire>(774203113, DataWire.OnBuildingBrokenDelegate, false);
            this.Unsubscribe<DataWire>(-1735440190, DataWire.OnBuildingFullyRepairedDelegate, false);
            base.OnCleanUp();
        }

        public bool IsConnected
        {
            get
            {
                return WirelessGridManager.dataCircuitSystem.GetNetworkForCell(Grid.PosToCell(this.transform.GetPosition())) is DataCircuitNetwork;
            }
        }

        public bool Connect()
        {
            BuildingHP component = this.GetComponent<BuildingHP>();
            if ((UnityEngine.Object)component == (UnityEngine.Object)null || component.HitPoints > 0)
            {
                this.disconnected = false;
                WirelessGridManager.dataCircuitSystem.ForceRebuildNetworks();
            }
            return !this.disconnected;
        }

        public void Disconnect()
        {
            this.disconnected = true;
            this.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, Db.Get().BuildingStatusItems.WireDisconnected, (object)null);
            WirelessGridManager.dataCircuitSystem.ForceRebuildNetworks();
        }

        public UtilityConnections GetWireConnections()
        {
            return WirelessGridManager.dataCircuitSystem.GetConnections(Grid.PosToCell(this.transform.GetPosition()), true);
        }

        public string GetWireConnectionsString()
        {
            return WirelessGridManager.dataCircuitSystem.GetVisualizerString(this.GetWireConnections());
        }

        private void OnBuildingBroken(object data)
        {
            this.Disconnect();
        }

        private void OnBuildingFullyRepaired(object data)
        {
            this.Connect();
        }

        
        public void SetFirstFrameCallback(System.Action ffCb)
        {
            this.firstFrameCallback = ffCb;
           //this.StartCoroutine(this.RunCallback());
        }

        /*
        [DebuggerHidden]
        private IEnumerator RunCallback()
        {
            return (IEnumerator)new DataWire.<RunCallback>c__Iterator0()
            { 
                $this=this
            };
        }
        */

        public IUtilityNetworkMgr GetNetworkManager()
        {
            return (IUtilityNetworkMgr) WirelessGridManager.dataCircuitSystem;
        }

        public void AddNetworks(ICollection<UtilityNetwork> networks)
        {
            UtilityNetwork networkForCell = WirelessGridManager.dataCircuitSystem.GetNetworkForCell(Grid.PosToCell(this.transform.GetPosition()));
            if (networkForCell == null)
                return;
            networks.Add(networkForCell);
        }

        public bool IsConnectedToNetworks(ICollection<UtilityNetwork> networks)
        {
            UtilityNetwork networkForCell = WirelessGridManager.dataCircuitSystem.GetNetworkForCell(Grid.PosToCell(this.transform.GetPosition()));
            return networks.Contains(networkForCell);
        }

        public int GetNetworkCell()
        {
            return Grid.PosToCell((KMonoBehaviour)this);
        }
    }

}
