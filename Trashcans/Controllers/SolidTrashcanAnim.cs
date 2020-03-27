using KSerialization;
using UnityEngine;

namespace Trashcans
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class SolidTrashcanAnim : TrashcanAnim
    {
        [MyCmpReq]
        private SolidConduitConsumer solidConsumer;

        protected override bool IsConsuming()
        {
            return this.solidConsumer.IsConsuming;
        }
    }
}
