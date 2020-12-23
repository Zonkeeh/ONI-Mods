using KSerialization;
using System.Collections.Generic;
using UnityEngine;

namespace ConfigurableSweepy
{

    [SerializationConfig(MemberSerialization.OptIn)]
    [AddComponentMenu("KMonoBehaviour/scripts/SolidConduitDispenser")]
    public class SolidConduitSweepStationDispenser : KMonoBehaviour, ISaveLoadable
    {
        private int utilityCell = -1;
        [SerializeField]
        public SimHashes[] elementFilter;
        [SerializeField]
        public bool invertElementFilter;
        [SerializeField]
        public bool alwaysDispense;
        [MyCmpReq]
        private Operational operational;
        [MyCmpReq]
        public Storage storage;
        private HandleVector<int>.Handle partitionerEntry;
        private bool dispensing;
        private int round_robin_index;
        private const float MaxMass = 10f;

        public SolidConduitFlow.ConduitContents ConduitContents => this.GetConduitFlow().GetContents(this.utilityCell);

        public bool IsDispensing => this.dispensing;

        public SolidConduitFlow GetConduitFlow() => Game.Instance.solidConduitFlow;

        protected override void OnSpawn()
        {
            base.OnSpawn();
            this.utilityCell = this.GetComponent<Building>().GetUtilityOutputCell();
            this.partitionerEntry = GameScenePartitioner.Instance.Add("SolidConduitConsumer.OnSpawn", (object)this.gameObject, this.utilityCell, GameScenePartitioner.Instance.objectLayers[20], new System.Action<object>(this.OnConduitConnectionChanged));
            this.GetConduitFlow().AddConduitUpdater(new System.Action<float>(this.ConduitUpdate), ConduitFlowPriority.Dispense);
            this.OnConduitConnectionChanged((object)null);
        }

        protected override void OnCleanUp()
        {
            this.GetConduitFlow().RemoveConduitUpdater(new System.Action<float>(this.ConduitUpdate));
            GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
            base.OnCleanUp();
        }

        private void OnConduitConnectionChanged(object data)
        {
            this.dispensing = this.dispensing && this.IsConnected;
            this.Trigger(-2094018600, (object)this.IsConnected);
        }

        private void ConduitUpdate(float dt)
        {
            bool flag = false;

            if ((this.operational.GetFlag(LogicOperationalController.LogicOperationalFlag) && this.IsConnected) || this.alwaysDispense)
            {
                SolidConduitFlow conduitFlow = this.GetConduitFlow();
                if (conduitFlow.HasConduit(this.utilityCell) && conduitFlow.IsConduitEmpty(this.utilityCell))
                {
                    Pickupable suitableItem = this.FindSuitableItem();
                    if ((bool)((UnityEngine.Object)suitableItem))
                    {
                        if ((double)suitableItem.PrimaryElement.Mass > MaxMass)
                            suitableItem = suitableItem.Take(MaxMass);
                        conduitFlow.AddPickupable(this.utilityCell, suitableItem);
                        flag = true;
                    }
                }
            }
            this.storage.storageNetworkID = this.GetConnectedNetworkID();
            this.dispensing = flag;
        }

        private Pickupable FindSuitableItem()
        {
            List<GameObject> items = this.storage.items;
            if (items.Count < 1)
                return (Pickupable)null;

            this.round_robin_index %= items.Count;
            GameObject gameObject = items[this.round_robin_index];
            ++this.round_robin_index;
            if (!(bool)((UnityEngine.Object)gameObject))
                return (Pickupable)null;
            return gameObject.GetComponent<Pickupable>();
        }

        public bool IsConnected
        {
            get
            {
                GameObject gameObject = Grid.Objects[this.utilityCell, 20];
                return (UnityEngine.Object)gameObject != (UnityEngine.Object)null && (UnityEngine.Object)gameObject.GetComponent<BuildingComplete>() != (UnityEngine.Object)null;
            }
        }

        private int GetConnectedNetworkID()
        {
            GameObject gameObject = Grid.Objects[this.utilityCell, 20];
            SolidConduit solidConduit = (UnityEngine.Object)gameObject != (UnityEngine.Object)null ? gameObject.GetComponent<SolidConduit>() : (SolidConduit)null;
            UtilityNetwork utilityNetwork = (UnityEngine.Object)solidConduit != (UnityEngine.Object)null ? solidConduit.GetNetwork() : (UtilityNetwork)null;
            return utilityNetwork == null ? -1 : utilityNetwork.id;
        }
    }

}
