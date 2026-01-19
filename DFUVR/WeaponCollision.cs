using UnityEngine;
using DaggerfallWorkshop;
using DaggerfallWorkshop.Game;
using DaggerfallWorkshop.Game.Entity;
using HarmonyLib;
using DaggerfallWorkshop.Game.Items;
namespace DFUVR
{
    public class WeaponCollision : MonoBehaviour
    {
        public static float minTimeToDamageAgain = 0.3f;
        public static float timeToNextDamage = 0.0f;
        //public float velocityThreshold = 2.0f;

        //private Rigidbody rb;

        void Start()
        {
            //rb = GetComponent<Rigidbody>();
        }
        //private void Update()
        //{
        //    if (Input.GetKeyDown(KeyCode.Y))
        //    {

        //        WeaponManager weaponManager=GameObject.Find("PlayerAdvanced").GetComponent<WeaponManager>();
        //        weaponManager.ToggleSheath();

        //    }
        //}

        private void OnTriggerEnter(Collider other)
        {
            if (Var.weaponManager == null)
            {
                Plugin.LoggerInstance.LogError("WeaponManager is null");
                return;
            }

            if (timeToNextDamage > Time.time)
                return;

            //Plugin.LoggerInstance.LogInfo("Hit something");

            bool hitSomething = false;

            if (other.GetComponent<DaggerfallEntityBehaviour>())
            {
                //Debug.Log("hit");
                //Vector3 relativeVelocity = rb.velocity;
                //if (relativeVelocity.magnitude > velocityThreshold)
                //{

                //Plugin.LoggerInstance.LogInfo("Hit Entity");

                //    Vector3 hitDirection = other.transform.position - transform.position;
                //    hitDirection = hitDirection.normalized;
                //    Debug.Log("Hit Direction: " + hitDirection);
                //}
                Vector3 hitDirection = other.transform.position - transform.position;
                Transform hitTransform = other.transform;
                DaggerfallUnityItem currentRightHandWeapon = (DaggerfallUnityItem)AccessTools.Field(typeof(WeaponManager), "currentRightHandWeapon").GetValue(Var.weaponManager);
                hitDirection = hitDirection.normalized;
                //Debug.Log("Hit Direction: " + hitDirection);

                GameObject player = (GameObject)AccessTools.Field(typeof(WeaponManager), "player").GetValue(Var.weaponManager);
                if (player.gameObject.GetInstanceID() != other.gameObject.GetInstanceID())
                {
                    Var.weaponManager.WeaponDamage(currentRightHandWeapon, false, false, hitTransform, hitTransform.localPosition, hitDirection);
                    hitSomething = true;
                }
            }
            else if (other.GetComponent<DaggerfallAction>())
            {
                Plugin.LoggerInstance.LogInfo("Hit action");
                DaggerfallAction action = other.GetComponent<DaggerfallAction>();
                GameObject player = (GameObject)AccessTools.Field(typeof(WeaponManager), "player").GetValue(Var.weaponManager);
                action.Receive(player, DaggerfallAction.TriggerTypes.Attack);
                hitSomething = true;
            }
            else if (other.GetComponent<DaggerfallActionDoor>())
            {
                //Plugin.LoggerInstance.LogInfo("Hit door");
                DaggerfallActionDoor actionDoor = other.GetComponent<DaggerfallActionDoor>();
                if (actionDoor)
                {
                    actionDoor.AttemptBash(true);
                    hitSomething = true;
                }
            }

            if (hitSomething)
            {
                Haptics.TriggerHapticFeedback(UnityEngine.XR.XRNode.RightHand, 0.6f);
                timeToNextDamage = Time.time + minTimeToDamageAgain;
            }
        }
    }
}
