using Harmony;
using KSerialization;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;
using Zolibrary.Logging;

namespace Trashcans
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class Trashcan : KMonoBehaviour, ISaveLoadable, ISidescreenButtonControl
    {
        public bool AutoTrash { get { return this.autoTrash; } }
        [Serialize]
        private bool autoTrash;
        public int WaitTime { get{ return this.waitTime; } }
        [Serialize]
        private int waitTime;
        public int CurrentTime { get { return this.currentTime; } }
        [Serialize]
        private int currentTime; 

        public string SidescreenTitleKey => TrashcansStrings.SideButtonKey + ".TITLE";

        public string SidescreenStatusMessage => TrashcansStrings.SideButtonStatus;

        public string SidescreenButtonText => TrashcansStrings.SideButtonText;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            Subscribe((int)GameHashes.RefreshUserMenu, OnRefresh);

            if((UnityEngine.Object)this.GetComponent<SolidConduitConsumer>() == (UnityEngine.Object)null)
                this.GetComponent<KSelectable>().AddStatusItem(TrashcansStrings.ReservoirCapacityStatus, (object)this.GetComponent<Storage>());

            if (TrashcansConfigChecker.EnableAutoTrash)
            {
                this.FindOrAdd<TrashcanAutotrashSideScreen>();
                this.GetComponent<KSelectable>().AddStatusItem(TrashcansStrings.TrashcanStatus, (object)this);
            }   
        }

        public void OnRefresh(object _)
        {
            string buttonTitle = TrashcansStrings.UserMenuButtonTitle;
            string buttonTooltip = TrashcansStrings.UserMenuButtonTooltip;

            KIconButtonMenu.ButtonInfo buttonInfo = new KIconButtonMenu.ButtonInfo(
                "action_building_disabled",
                buttonTitle,
                DropItems,
                Action.NumActions,
                null,
                null,
                null,
                buttonTooltip
            );

            Game.Instance.userMenu.AddButton(gameObject, buttonInfo);
        }

        public void EmptyTrash()
        {
            if (!this.GetComponent<Operational>().IsOperational)
                return;
            Traverse.Create(this.FindComponent<Storage>()).Method("ClearItems").GetValue();
        }

        public void OnSidescreenButtonPressed() => EmptyTrash();

        public void ChangeAutoTrash(bool value) 
        {
            if (value == this.autoTrash)
                return;

            this.currentTime = 0;
            this.autoTrash = value;
        }

        public void ChangeAutoTrashTime(int time)
        {
            if (time == this.waitTime || time < 1)
                return;

            LogManager.Log("Set: " + time + "\nWaitTime: " + this.waitTime);

            this.currentTime = 0;
            this.waitTime = time;
        }

        public void IncrementTime(bool reset)
        {
            if (reset)
                this.currentTime = 0;
            else
                this.currentTime++;
        }

        private void DropItems()
        {
            Storage storage = this.FindComponent<Storage>();

            if (storage != null)
                storage.DropAll();
        }
    }
}
