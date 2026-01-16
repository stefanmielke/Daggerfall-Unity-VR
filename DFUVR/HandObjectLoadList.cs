using DaggerfallWorkshop;
using DaggerfallWorkshop.Game.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DFUVR
{
    internal class HandObjectLoadList
    {
        public WeaponTypes[] weaponTypes { get; private set; }
        public string assetName { get; private set; }
        public Vector3 sheatedPositionOffset { get; private set; }
        public Quaternion sheatedRotationOffset { get; private set; }
        public Vector3 unsheatedPositionOffset { get; private set; }
        public Quaternion unsheatedRotationOffset { get; private set; }
        public bool renderUnsheated { get; private set; }
        public bool resetPosition { get; private set; }
        public bool isActive { get; private set; }
        public Type collisionType { get; private set; }
        public Type colliderType { get; private set; }
        public Action<GameObject> postAction { get; private set; }

        private HandObjectLoadList(Action<GameObject> postAction, Type collisionType, Type colliderType, WeaponTypes[] weaponTypes, string assetName, Vector3 sheatedPositionOffset, Quaternion sheatedRotationOffset, Vector3 unsheatedPositionOffset, Quaternion unsheatedRotationOffset, bool renderUnsheated = false, bool resetPosition = false, bool isActive = false)
        {
            this.collisionType = collisionType;
            this.colliderType = colliderType;
            this.weaponTypes = weaponTypes;
            this.assetName = assetName;
            this.sheatedPositionOffset = sheatedPositionOffset;
            this.sheatedRotationOffset = sheatedRotationOffset;
            this.unsheatedPositionOffset = unsheatedPositionOffset;
            this.unsheatedRotationOffset = unsheatedRotationOffset;
            this.renderUnsheated = renderUnsheated;
            this.resetPosition = resetPosition;
            this.isActive = isActive;
            this.postAction = postAction;
        }

        public static readonly List<HandObjectLoadList> handObjects = new List<HandObjectLoadList>()
        {
            new HandObjectLoadList(null, typeof(WeaponCollision), typeof(MeshCollider), new[] { WeaponTypes.LongBlade, WeaponTypes.LongBlade_Magic }, "Sword", Vector3.zero, Quaternion.identity, Vector3.zero, Quaternion.identity, true, true, true),
            new HandObjectLoadList(null, typeof(WeaponCollision), typeof(MeshCollider), new[] { WeaponTypes.Dagger, WeaponTypes.Dagger_Magic }, "Steel_Dagger_512", Vector3.zero, Quaternion.Euler(0, 90, 90), Vector3.zero, Quaternion.Euler(0, 90, 90), true),
            new HandObjectLoadList(null, typeof(WeaponCollision), typeof(MeshCollider), new[] { WeaponTypes.Battleaxe, WeaponTypes.Battleaxe_Magic }, "MM_Axe_01_01_lod2", Vector3.zero, Quaternion.Euler(0, -90, 90), Vector3.zero, Quaternion.Euler(0, 180, 180)),
            new HandObjectLoadList(null, null, null, new[] { WeaponTypes.None }, "Sheath", Vector3.zero, Quaternion.identity, Vector3.zero, Quaternion.identity, true),
            new HandObjectLoadList(null, typeof(WeaponCollision), typeof(MeshCollider), new[] { WeaponTypes.Mace, WeaponTypes.Mace_Magic }, "mace", new Vector3(0, 0, 0.1f), Quaternion.Euler(90, 0, 0), new Vector3(0, 0, 0.1f), Quaternion.Euler(90, 0, 0)),
            new HandObjectLoadList(null, typeof(WeaponCollision), typeof(MeshCollider), new[] { WeaponTypes.Flail, WeaponTypes.Flail_Magic }, "mace", new Vector3(0, 0, 0.1f), Quaternion.Euler(90, 0, 0), new Vector3(0, 0, 0.1f), Quaternion.Euler(90, 0, 0)),
            new HandObjectLoadList(null, typeof(WeaponCollision), typeof(BoxCollider), new[] { WeaponTypes.Warhammer, WeaponTypes.Warhammer_Magic }, "Warhammer_1", Vector3.zero, Quaternion.Euler(0, 0, 90), Vector3.zero, Quaternion.identity),
            new HandObjectLoadList(null, typeof(WeaponCollision), typeof(BoxCollider), new[] { WeaponTypes.Staff, WeaponTypes.Staff_Magic }, "Staff_1", Vector3.zero, Quaternion.identity, Vector3.zero, Quaternion.identity),
            new HandObjectLoadList(BowPostAction, null, typeof(SphereCollider), new[] { WeaponTypes.Bow }, "Crossbow", Vector3.zero, Quaternion.Euler(270, 90, 0), Vector3.zero, Quaternion.Euler(270, 90, 0)),
            //new HandObjectLoadList(HandPostAction, typeof(WeaponCollision), typeof(SphereCollider), null, "rHandClosed", new Vector3(0, 0, -0.05f), Quaternion.Euler(20, 10, 270), Vector3.zero, Quaternion.identity)
        };

        private static void BowPostAction(GameObject bow)
        {
            bow.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        }

        //private static void HandPostAction(GameObject meleeHandR)
        //{
        //    try
        //    {
        //        //meleeHandR.transform.GetChild(2).GetComponent<SkinnedMeshRenderer>().material = sword.GetComponent<MeshRenderer>().material;
        //    }
        //    catch (Exception e)
        //    {
        //        Plugin.LoggerInstance.LogError(e);
        //    }
        //}

        public override string ToString()
        {
            return $"HandObjectLoadList(weaponTypes={string.Join(",", weaponTypes)}, assetName={assetName}, sheatedPositionOffset={sheatedPositionOffset}, sheatedRotationOffset={sheatedRotationOffset}, unsheatedPositionOffset={unsheatedPositionOffset}, unsheatedRotationOffset={unsheatedRotationOffset}, renderUnsheated={renderUnsheated}, resetPosition={resetPosition}, isActive={isActive}, collisionType={collisionType}, colliderType={colliderType}, postAction={(postAction != null ? postAction.Method.Name : "null")})";
        }
    }
}
