using Harmony;
using KSerialization;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;
using Zolibrary.Logging;

namespace Trashcans
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class TrashcanAutotrashSideScreen : SideScreen, ISaveLoadable, ICheckboxControl, IIntSliderControl, ISim1000ms
    {
        private Trashcan trashcan;

        public string CheckboxTitleKey => (LocString) TrashcansStrings.CheckboxKey + ".TITLE";

        public string CheckboxLabel => TrashcansStrings.CheckboxLabel;

        public string CheckboxTooltip => TrashcansStrings.CheckboxTooltip;

        public string SliderTitleKey => (LocString)TrashcansStrings.SliderKey + ".TITLE";

        public string GetSliderTooltipKey(int index) => (LocString) TrashcansStrings.SliderKey + ".TOOLTIP";

        public string GetSliderTooltip() => TrashcansStrings.SliderTooltip;

        public string SliderUnits => TrashcansStrings.SliderUnits;

        protected override void OnSpawn() 
        {
            base.OnSpawn();
            this.trashcan = this.FindComponent<Trashcan>();
        }

        public bool GetCheckboxValue()
        {
            if (this.trashcan == null)
                return false;

            return this.trashcan.AutoTrash;
        }

        public void SetCheckboxValue(bool value)
        {
            if (this.trashcan == null)
                return;

            this.trashcan.ChangeAutoTrash(value);
        }

        public int SliderDecimalPlaces(int index) => 0;

        public float GetSliderMin(int index) => 1f;

        public float GetSliderMax(int index) => TrashcansConfigChecker.MaxAutoTrashInterval;

        public float GetSliderValue(int index)
        {
            if (this.trashcan == null)
                return 60f;

            return this.trashcan.WaitTime;
        }

        public void SetSliderValue(float percent, int index)
        {
            if (this.trashcan == null)
                return;

            this.trashcan.ChangeAutoTrashTime(Mathf.RoundToInt(percent));
        }

        public void Sim1000ms(float dt)
        {
            if (!this.trashcan.AutoTrash || !this.GetComponent<Operational>().IsOperational)
                return;

            this.trashcan.IncrementTime(false);

            if (this.trashcan.CurrentTime < this.trashcan.WaitTime)
                return;
            else
            {
                this.trashcan.IncrementTime(true);
                this.trashcan.EmptyTrash();
            }
        }

    }
}
