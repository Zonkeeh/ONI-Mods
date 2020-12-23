using Harmony;
using Klei.AI;
using KSerialization;

namespace ConfigurableSweepy
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class SweepyConfigurator : KMonoBehaviour, ISaveLoadable
    {
        [Serialize]
        private bool hasRun;
        
        public float MoveSpeed { get { return this.moveSpeed; } }
        [Serialize]
        private float moveSpeed;

        public int ProbingRadius { get { return this.probingRadius; } }
        [Serialize]
        private int probingRadius;

        [Serialize]
        private float storageCapacity;

        protected override void OnSpawn()
        {
            base.OnSpawn();
            this.InitStats();
        }

        private void InitStats() 
        {
            Storage storage = this.gameObject.GetComponents<Storage>()[1];

            if (SweepyConfigChecker.SweepyUsesPower)
                this.GetComponent<KSelectable>().AddStatusItem(SweepyStrings.BatteryLevelStatus, (object)this.gameObject);

            if (storage != null)
            {
                this.storageCapacity = SweepyConfigChecker.StorageCapacity;
                storage.capacityKg = this.storageCapacity;
                storage.showDescriptor = true;
                storage.showInUI = true;
                this.GetComponent<KSelectable>().AddStatusItem(SweepyStrings.StorageStatus, (object)storage);
            }

            this.InitMoveSpeed();
            this.InitProbingRadius();

            if (SweepyConfigChecker.DebugMode)
                this.GetComponent<KSelectable>().AddStatusItem(SweepyStrings.CustomDebugStatus, (object)this.gameObject);

            this.hasRun = true;
        }

        private void InitMoveSpeed()
        {
            if (this.hasRun)
            {
                Navigator nav = this.GetComponent<Navigator>();

                if (nav == null)
                    return;

                nav.defaultSpeed = this.moveSpeed;

                if (!SweepyConfigChecker.BatteryDrainBasedOnSpeed)
                    return;

                this.ChangeBatteryDelta();
            }
            else
                this.ChangeMoveSpeed(SweepyConfigChecker.BaseMovementSpeed);
        }

        public bool ChangeMoveSpeed(float moveSpeed)
        {
            if (moveSpeed == this.moveSpeed || moveSpeed>SweepyConfigChecker.MaxSpeedSliderValue || moveSpeed<SweepyConfigChecker.MinSpeedSliderValue)
                return false;

            Navigator nav = this.GetComponent<Navigator>();

            if (nav == null)
                return false;

            this.moveSpeed = moveSpeed;
            nav.defaultSpeed = this.moveSpeed;

            if (!SweepyConfigChecker.BatteryDrainBasedOnSpeed || !SweepyConfigChecker.SweepyUsesPower)
                return true;

            this.ChangeBatteryDelta();

            return true;
        }

        private void ChangeBatteryDelta() 
        {
            Modifiers modifiers = this.GetComponent<Modifiers>();

            if (modifiers == null)
                return;

            AttributeInstance deltaMod = modifiers.attributes.Get(Db.Get().Amounts.InternalBattery.deltaAttribute.Id);
            deltaMod.modifier.BaseValue = -SweepyConfigChecker.BatteryDepletionRate*this.moveSpeed*SweepyConfigChecker.DrainSpeedMultiplier;
        }

        private void InitProbingRadius()
        {
            if (this.hasRun) 
            {
                Navigator nav = this.GetComponent<Navigator>();

                if (nav == null)
                    return;

                nav.maxProbingRadius = this.probingRadius;
            }
            else
                this.ChangeProbingRadius(SweepyConfigChecker.BaseProbingRadius);
        }

        public bool ChangeProbingRadius(int radius)
        {
            if (radius == this.probingRadius || radius>SweepyConfigChecker.MaxProbingSliderValue || radius<SweepyConfigChecker.MinProbingSliderValue)
                return false;

            Navigator nav = this.GetComponent<Navigator>();

            if (nav == null)
                return false;

            this.probingRadius = radius;
            nav.maxProbingRadius = this.probingRadius;
            return true;
        }

        public void FindSweepy() 
        {
            Navigator nav = this.GetComponent<Navigator>();

            if (nav == null)
                return;

            Traverse.Create(nav).Method("OnFollowCam").GetValue();
        }

        public int ResetSweepy()
        {
            bool move_changed = this.ChangeMoveSpeed(SweepyConfigChecker.BaseMovementSpeed);
            bool range_changed = this.ChangeProbingRadius(SweepyConfigChecker.BaseProbingRadius);

            if (move_changed && range_changed)
                return 3;
            else if (range_changed)
                return 2;
            else if (move_changed)
                return 1;
            else
                return 0;
        }
    }
}
