using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Harmony;
using static DetailsScreen;

namespace AdvancedSpaceScanner
{
    public class AdvancedSpaceScannerSideScreen : SideScreenContent
    {
        public Dictionary<object, GameObject> rows = new Dictionary<object, GameObject>();
        private AdvancedSpaceScannerController.Instance detector;
        public GameObject rowPrefab;
        public RectTransform rowContainer;

        protected override void OnPrefabInit() 
        {
            Traverse detailsScreen = Traverse.Create(DetailsScreen.Instance);
            List<SideScreenRef> screens = detailsScreen.Field("sideScreens").GetValue<List<SideScreenRef>>();
            SideScreenContent cometDetector_screen = screens.Find(s => s.name == "CometDetectorSideScreen").screenPrefab;
            CometDetectorSideScreen cometDetector = cometDetector_screen.GetComponent<CometDetectorSideScreen>();

            this.rows = cometDetector.rows;
            this.rowPrefab = cometDetector.rowPrefab;
            this.rowContainer = cometDetector.rowContainer;
            base.OnPrefabInit();
        }

        protected override void OnShow(bool show)
        {
            base.OnShow(show);
            if (!show)
                return;
   
            this.RefreshOptions();
        }

        private void RefreshOptions()
        {
            int idx = 0;
            int num = idx + 1;
            this.SetRow(idx, (string)STRINGS.UI.UISIDESCREENS.COMETDETECTORSIDESCREEN.COMETS, Assets.GetSprite((HashedString)"asteroid"), (LaunchConditionManager)null);
            foreach (Spacecraft spacecraft in SpacecraftManager.instance.GetSpacecraft())
                this.SetRow(num++, spacecraft.GetRocketName(), Assets.GetSprite((HashedString)"icon_category_rocketry"), spacecraft.launchConditions);
            for (int index = num; index < this.rowContainer.childCount; ++index)
                this.rowContainer.GetChild(index).gameObject.SetActive(false);
        }

        private void ClearRows()
        {
            for (int index = this.rowContainer.childCount - 1; index >= 0; --index)
                Util.KDestroyGameObject((Component)this.rowContainer.GetChild(index));
            this.rows.Clear();
        }

        public override void SetTarget(GameObject target)
        {
            this.detector = target.GetSMI<AdvancedSpaceScannerController.Instance>();
            this.RefreshOptions();
        }

        private void SetRow(int idx, string name, Sprite icon, LaunchConditionManager target)
        {
            GameObject gameObject = idx >= this.rowContainer.childCount ? Util.KInstantiateUI(this.rowPrefab, this.rowContainer.gameObject, true) : this.rowContainer.GetChild(idx).gameObject;
            HierarchyReferences component1 = gameObject.GetComponent<HierarchyReferences>();
            component1.GetReference<LocText>("label").text = name;
            component1.GetReference<Image>(nameof(icon)).sprite = icon;
            MultiToggle component2 = gameObject.GetComponent<MultiToggle>();
            component2.ChangeState((UnityEngine.Object)this.detector.GetTargetCraft() == (UnityEngine.Object)target ? 1 : 0);
            LaunchConditionManager _target = target;
            component2.onClick = (System.Action)(() =>
            {
                this.detector.SetTargetCraft(_target);
                this.RefreshOptions();
            });
        }

        public override bool IsValidForTarget(GameObject target)
        {
            return target.GetSMI<AdvancedSpaceScannerController.Instance>() != null;
        }
    }

}
