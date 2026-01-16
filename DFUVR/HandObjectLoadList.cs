using BepInEx;
using DaggerfallWorkshop;
using DaggerfallWorkshop.Utility.AssetInjection;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace DFUVR
{
    public class HandObjectLoadList : MonoBehaviour
    {
        public enum AssetType { DaggerfallResource, AssetBundle }

        public AssetType assetType { get; private set; }

        // used when loading from asset bundles
        public string assetBundlePath { get; private set; }
        // used when using asset ids from Daggerfall's resources
        public uint assetId { get; private set; }

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

        private HandObjectLoadList(uint assetId, Action<GameObject> postAction, Type collisionType, Type colliderType, WeaponTypes[] weaponTypes, string assetName, Vector3 sheatedPositionOffset, Quaternion sheatedRotationOffset, Vector3 unsheatedPositionOffset, Quaternion unsheatedRotationOffset, bool renderUnsheated = false, bool resetPosition = false, bool isActive = false)
        {
            this.assetId = assetId;
            this.assetBundlePath = null;
            this.assetType = AssetType.DaggerfallResource;

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

        private HandObjectLoadList(string assetBundlePath, Action<GameObject> postAction, Type collisionType, Type colliderType, WeaponTypes[] weaponTypes, string assetName, Vector3 sheatedPositionOffset, Quaternion sheatedRotationOffset, Vector3 unsheatedPositionOffset, Quaternion unsheatedRotationOffset, bool renderUnsheated = false, bool resetPosition = false, bool isActive = false)
        {
            this.assetBundlePath = assetBundlePath;
            this.assetId = 0;
            this.assetType = AssetType.AssetBundle;

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

        public static readonly List<HandObjectLoadList> handObjectLoadList = new List<HandObjectLoadList>()
        {
            //new HandObjectLoadList(128005, null, typeof(WeaponCollision), typeof(MeshCollider), new[] { WeaponTypes.LongBlade, WeaponTypes.LongBlade_Magic }, "Sword", Vector3.zero, Quaternion.identity, Vector3.zero, Quaternion.identity, true, true, true),
            new HandObjectLoadList("AssetBundles/weapons", null, typeof(WeaponCollision), typeof(MeshCollider), new[] { WeaponTypes.LongBlade, WeaponTypes.LongBlade_Magic }, "Sword", Vector3.zero, Quaternion.identity, Vector3.zero, Quaternion.identity, true, true, true),
            new HandObjectLoadList("AssetBundles/weapons", null, typeof(WeaponCollision), typeof(MeshCollider), new[] { WeaponTypes.Dagger, WeaponTypes.Dagger_Magic }, "Steel_Dagger_512", Vector3.zero, Quaternion.Euler(0, 90, 90), Vector3.zero, Quaternion.Euler(0, 90, 90), true),
            new HandObjectLoadList("AssetBundles/weapons", null, typeof(WeaponCollision), typeof(MeshCollider), new[] { WeaponTypes.Battleaxe, WeaponTypes.Battleaxe_Magic }, "MM_Axe_01_01_lod2", Vector3.zero, Quaternion.Euler(0, -90, 90), Vector3.zero, Quaternion.Euler(0, 180, 180)),
            new HandObjectLoadList("AssetBundles/weapons", null, null, null, new[] { WeaponTypes.None }, "Sheath", Vector3.zero, Quaternion.identity, Vector3.zero, Quaternion.identity, true),
            new HandObjectLoadList("AssetBundles/weapons", null, typeof(WeaponCollision), typeof(MeshCollider), new[] { WeaponTypes.Mace, WeaponTypes.Mace_Magic }, "mace", new Vector3(0, 0, 0.1f), Quaternion.Euler(90, 0, 0), new Vector3(0, 0, 0.1f), Quaternion.Euler(90, 0, 0)),
            new HandObjectLoadList("AssetBundles/weapons", null, typeof(WeaponCollision), typeof(MeshCollider), new[] { WeaponTypes.Flail, WeaponTypes.Flail_Magic }, "mace", new Vector3(0, 0, 0.1f), Quaternion.Euler(90, 0, 0), new Vector3(0, 0, 0.1f), Quaternion.Euler(90, 0, 0)),
            new HandObjectLoadList("AssetBundles/weapons", null, typeof(WeaponCollision), typeof(BoxCollider), new[] { WeaponTypes.Warhammer, WeaponTypes.Warhammer_Magic }, "Warhammer_1", Vector3.zero, Quaternion.Euler(0, 0, 90), Vector3.zero, Quaternion.identity),
            new HandObjectLoadList("AssetBundles/weapons", null, typeof(WeaponCollision), typeof(BoxCollider), new[] { WeaponTypes.Staff, WeaponTypes.Staff_Magic }, "Staff_1", Vector3.zero, Quaternion.identity, Vector3.zero, Quaternion.identity),
            new HandObjectLoadList("AssetBundles/weapons", BowPostAction, null, typeof(SphereCollider), new[] { WeaponTypes.Bow }, "Crossbow", Vector3.zero, Quaternion.Euler(270, 90, 0), Vector3.zero, Quaternion.Euler(270, 90, 0)),
            new HandObjectLoadList("AssetBundles/hands", HandPostAction, typeof(WeaponCollision), typeof(SphereCollider), new[] { WeaponTypes.Melee, WeaponTypes.Werecreature }, "rHandClosed", new Vector3(0, 0, -0.05f), Quaternion.Euler(20, 10, 270), Vector3.zero, Quaternion.identity)
        };

        public static void LoadAllHandObjects(Dictionary<WeaponTypes, HandObject> handObjectDictionary)
        {
            AssetBundle currentBundle = null;
            string currentBundlePath = string.Empty;

            foreach (var handObjectLoad in handObjectLoadList)
            {
                GameObject gameObject = null;
                if (handObjectLoad.assetType == AssetType.DaggerfallResource)
                {
                    gameObject = MeshReplacement.ImportCustomGameobject(handObjectLoad.assetId, null, Matrix4x4.identity);
                    if (gameObject == null)
                    {
                        Plugin.LoggerInstance.LogError($"Failed to load asset '{handObjectLoad.assetName}' from DaggerfallResources.");
                        continue;
                    }
                }
                else
                {
                    if (handObjectLoad.assetBundlePath != currentBundlePath)
                    {
                        currentBundlePath = handObjectLoad.assetBundlePath;
                        currentBundle?.Unload(false);

                        string assetBundlePath = Path.Combine(Paths.PluginPath, currentBundlePath);
                        currentBundle = AssetBundle.LoadFromFile(assetBundlePath);
                        if (currentBundle == null)
                        {
                            Plugin.LoggerInstance.LogError($"Failed to load AssetBundle from path '{handObjectLoad.assetBundlePath}'.");
                            continue;
                        }
                    }

                    gameObject = Instantiate(currentBundle.LoadAsset<GameObject>(handObjectLoad.assetName));
                    if (gameObject == null)
                        Plugin.LoggerInstance.LogError($"Failed to load asset '{handObjectLoad.assetName}' from AssetBundle.");
                }

                if (handObjectLoad.collisionType != null)
                {
                    gameObject.AddComponent(handObjectLoad.collisionType);
                    var collider = gameObject.GetComponent(handObjectLoad.colliderType) as Collider;
                    if (collider == null)
                        collider = gameObject.AddComponent(handObjectLoad.colliderType) as Collider;

                    if (collider == null)
                        Plugin.LoggerInstance.LogError($"Failed to add collider of type '{handObjectLoad.colliderType}' to '{handObjectLoad.assetName}'.");
                    else
                        collider.isTrigger = true;
                }
                else if (handObjectLoad.colliderType != null)
                {
                    var collider = gameObject.AddComponent(handObjectLoad.colliderType) as Collider;
                    if (collider == null)
                        Plugin.LoggerInstance.LogError($"Failed to add collider of type '{handObjectLoad.colliderType}' to '{handObjectLoad.assetName}'.");
                    else
                        collider.isTrigger = true;
                }

                gameObject.SetActive(handObjectLoad.isActive);

                if (handObjectLoad.resetPosition)
                {
                    gameObject.transform.position = Vector3.zero;
                    gameObject.transform.rotation = Quaternion.identity;
                }

                if (handObjectLoad.weaponTypes != null)
                {
                    foreach (var type in handObjectLoad.weaponTypes)
                        handObjectDictionary.Add(type, new HandObject(handObjectLoad, gameObject));
                }

                handObjectLoad.postAction?.Invoke(gameObject);
            }

            currentBundle?.Unload(false);
        }

        private static void BowPostAction(GameObject bow)
        {
            bow.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        }

        private static void HandPostAction(GameObject meleeHandR)
        {
            try
            {
                var sword = Var.handObjects[WeaponTypes.LongBlade].gameObject;
                meleeHandR.transform.GetChild(2).GetComponent<SkinnedMeshRenderer>().material = sword.GetComponent<MeshRenderer>().material;
                sword.SetActive(false);
            }
            catch (Exception e)
            {
                Plugin.LoggerInstance.LogError(e);
            }
        }

        public override string ToString()
        {
            return $"HandObjectLoadList(weaponTypes={string.Join(",", weaponTypes)}, assetName={assetName}, sheatedPositionOffset={sheatedPositionOffset}, sheatedRotationOffset={sheatedRotationOffset}, unsheatedPositionOffset={unsheatedPositionOffset}, unsheatedRotationOffset={unsheatedRotationOffset}, renderUnsheated={renderUnsheated}, resetPosition={resetPosition}, isActive={isActive}, collisionType={collisionType}, colliderType={colliderType}, postAction={(postAction != null ? postAction.Method.Name : "null")})";
        }
    }
}
