#define UsesDLC
using KSerialization;
using System;
using TUNING;
using UnityEngine;
using Zolibrary.Logging;

namespace DestructibleFeatures
{
    [AddComponentMenu("KMonoBehaviour/Workable/Destructible")]
    public class DestructibleWorkable : Workable, ISidescreenButtonControl
    {
        private Chore chore;
        [Serialize]
        private const float DESTROY_WORK_TIME = 1800f;
        [Serialize]
        private bool markedForDestruction;
        private Guid statusItemGuid;

        public string SidescreenTitleKey => DestructibleStrings.TitleKey;

        public string SidescreenButtonTooltip => DestructibleStrings.Tooltip;

        public string SidescreenStatusMessage
        {
            get
            {
                if (this.markedForDestruction)
                    return DestructibleStrings.PendingStatus;
                return DestructibleStrings.Status;
            }
        }

        public string SidescreenButtonText
        {
            get
            {
                if (this.markedForDestruction)
                    return DestructibleStrings.PendingButton;
                return DestructibleStrings.Button;
            }
        }

        public bool SidescreenEnabled() => true;

        public bool SidescreenButtonInteractable() => true;

        public int ButtonSideScreenSortOrder() => 20;

        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            this.overrideAnims = new KAnimFile[1] { Assets.GetAnim((HashedString)"anim_use_machine_kanim") };
            this.faceTargetWhenWorking = true;
            this.synchronizeAnims = false;
            this.workerStatusItem = DestructibleStrings.DuplicantStatus;
            this.resetProgressOnStop = false;
#if UsesDLC
            this.requiredSkillPerk = Db.Get().SkillPerks.CanDigSuperDuperHard.Id;
#else
            this.requiredSkillPerk = Db.Get().SkillPerks.CanDigSupersuperhard.Id;
#endif

            this.attributeConverter = Db.Get().AttributeConverters.DiggingSpeed;
            this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
            this.skillExperienceSkillGroup = Db.Get().SkillGroups.Mining.Id;
            this.skillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
            this.SetWorkTime(DestructibleWorkable.DESTROY_WORK_TIME);
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            this.Refresh();
        }

        public void OnSidescreenButtonPressed()
        {
            this.ToggleDeconstructChore();
        }

        public void CancelChore()
        {
            if (this.chore == null)
                return;
            this.chore.Cancel("DestructibleWorkable.CancelChore");
            this.chore = null;
        }

        public void Refresh()
        {
            if (KMonoBehaviour.isLoadingScene)
                return;

            KSelectable component = this.GetComponent<KSelectable>();

            if (this.markedForDestruction)
            {
                if (this.chore == null)
                    this.chore = new WorkChore<DestructibleWorkable>(
                        chore_type: Db.Get().ChoreTypes.Dig,
                        target: this,
                        chore_provider: null,
                        run_until_complete: true,
                        on_complete: null,
                        on_begin: null,
                        on_end: null,
                        allow_in_red_alert: true,
                        schedule_block: null,
                        ignore_schedule_block: false,
                        only_when_operational: false,
                        override_anims: null,
                        is_preemptable: false,
                        allow_in_context_menu: true,
                        allow_prioritization: true, 
                        priority_class: PriorityScreen.PriorityClass.basic, 
                        priority_class_value: 5, 
                        ignore_building_assignment: false, 
                        add_to_daily_report: true
                        );

                if(component!= null)
                    this.statusItemGuid = component.ReplaceStatusItem(this.statusItemGuid, DestructibleStrings.AwaitingDeconstruct, null);
            }
            else
            {
                this.CancelChore();
                if (component != null)
                    this.statusItemGuid = component.RemoveStatusItem(this.statusItemGuid, false);
            }
        }

        private void ToggleDeconstructChore()
        {
            this.markedForDestruction = !this.markedForDestruction;
            this.Refresh();
        }

        protected override void OnCompleteWork(Worker worker)
        {
            base.OnCompleteWork(worker);
            this.chore = (Chore) null;

            if (DetailsScreen.Instance != null && DetailsScreen.Instance.CompareTargetWith(this.gameObject))
                DetailsScreen.Instance.Show(false);

            this.DestroyNeutroniumAndGeyser();
        }

        private void DestroyNeutroniumAndGeyser()
        {
            if (!FeaturePatches.config.RemoveNeutronium)
            {
                Util.KDestroyGameObject(this.gameObject);
                return;
            }

            int cell = this.NaturalBuildingCell();
            int[] cells = new[]
            {
                Grid.CellDownLeft(cell),
                Grid.CellBelow(cell),
                Grid.CellDownRight(cell),
                Grid.CellRight(Grid.CellDownRight(cell))
            };

            foreach (int x in cells)
            {
                if (Grid.Element.Length < x || Grid.Element[x] == null)
                {
                    LogManager.LogException("Element list does not contain a valid element for the cell:" + x, 
                        new IndexOutOfRangeException());
                    return;
                }

                Element e = Grid.Element[x];
                if (!e.IsSolid && !e.id.ToString().ToUpperInvariant().Equals("UNOBTANIUM"))
                {
                    LogManager.LogException("Element is expected to be neutronium & solid for the cell:" + x, 
                        new ArgumentException());
                    return;
                }

                SimHashes replaceElement = SimHashes.Obsidian;

                if (!FeaturePatches.config.ReplaceNeutroniumWithObsidian)
                    replaceElement = SimHashes.Vacuum;
                
                SimMessages.ReplaceElement(
                    gameCell: x, 
                    new_element: replaceElement, 
                    ev: CellEventLogger.Instance.DebugTool, 
                    mass: 100f, 
                    temperature: 293f, 
                    diseaseIdx: byte.MaxValue, 
                    diseaseCount: 0, 
                    callbackIdx: -1
                    );

                Util.KDestroyGameObject(this.gameObject);
            }
        }
	}
}
