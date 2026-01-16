using UnityEngine;

namespace DFUVR
{
    public class HandObject
    {
        public GameObject gameObject { get; private set; }
        public Vector3 sheatedPositionOffset { get; private set; }
        public Quaternion sheatedRotationOffset { get; private set; }
        public Vector3 unsheatedPositionOffset { get; private set; }
        public Quaternion unsheatedRotationOffset { get; private set; }
        public bool renderUnsheated { get; private set; }

        public HandObject(GameObject gameObject, Vector3 sheatedPositionOffset, Quaternion sheatedRotationOffset, Vector3 unsheatedPositionOffset, Quaternion unsheatedRotationOffset, bool renderUnsheated)
        {
            this.gameObject = gameObject;
            this.sheatedPositionOffset = sheatedPositionOffset;
            this.sheatedRotationOffset = sheatedRotationOffset;
            this.unsheatedPositionOffset = unsheatedPositionOffset;
            this.unsheatedRotationOffset = unsheatedRotationOffset;
            this.renderUnsheated = renderUnsheated;
        }
    }
}
