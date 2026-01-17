using UnityEngine;
namespace DFUVR
{
    public class SheathCollision : MonoBehaviour
    {
        public static bool canUse = false;
        public static bool rightHand = true;

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<HandLabel>() != null)
            {
                if (other.gameObject.GetComponent<HandLabel>().rightHand == false)
                {
                    Haptics.TriggerHapticFeedback(UnityEngine.XR.XRNode.LeftHand, 0.6f);
                }
                else
                {
                    Haptics.TriggerHapticFeedback(UnityEngine.XR.XRNode.RightHand, 0.6f);
                    canUse = true;
                }
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.gameObject.GetComponent<HandLabel>() != null)
            {
                canUse = false;
            }
        }
    }
}
