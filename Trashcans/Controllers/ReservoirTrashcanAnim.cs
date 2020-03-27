using KSerialization;
using UnityEngine;

namespace Trashcans
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class ReservoirTrashcanAnim : TrashcanAnim
    {
        [MyCmpReq]
        private ConduitConsumer reservoirConsumer;

        protected override bool IsConsuming()
        {
            return this.reservoirConsumer.isConsuming;
        }
    }
}
