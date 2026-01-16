
using DaggerfallWorkshop.Game;
using DaggerfallWorkshop.Game.Utility;
using HarmonyLib;
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SpatialTracking;
using UnityEngine.UI;
using uWindowCapture;

namespace DFUVR
{
    public class SPC : MonoBehaviour
    {
        private LineRenderer lineRenderer;
        private TrackedPoseDriver trackedPoseDriver;
        private GraphicRaycaster graphicRaycaster;

        public GraphicRaycaster raycaster;
        public float raycastDistance = 20f;

        // Window properties
        public int windowPosX;
        public int windowPosY;
        public int windowWidth;
        public int windowHeight;

        private UwcWindowTexture wTexture;

        [DllImport("user32.dll")]
        private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, int dwExtraInfo);

        private const uint MOUSEEVENTF_LEFTDOWN = 0x02;
        private const uint MOUSEEVENTF_LEFTUP = 0x04;

        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int X, int Y);

        void Start()
        {

        }
        public void Initialize(LineRenderer line, GraphicRaycaster graphicRaycaster, TrackedPoseDriver tracked)
        {
            if (!Var.fStartMenu)
            {
                if (Var.isFirst)
                {
                    wTexture = GameObject.Find("VRUI").GetComponent<UwcWindowTexture>();
                    wTexture.type = WindowTextureType.Window;
                    wTexture.partialWindowTitle = "Daggerfall";
                }
                else
                {
                    //wTexture = GameObject.Find("VRUI").GetComponent<UwcWindowTexture>();
                    windowHeight = Var.windowHeight;
                    windowWidth = Var.windowWidth;

                }
            }
            lineRenderer = line;
            trackedPoseDriver = tracked;
            this.graphicRaycaster = graphicRaycaster;
        }

        void Update()
        {

            //foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
            //{
            //    if (Input.GetKey(kcode))
            //        Plugin.LoggerInstance.LogInfo("KeyCode down: " + kcode);
            //}
            //float input = Input.GetAxis("Axis" + Var.debugInt2);

            //Plugin.LoggerInstance.LogInfo("Input:" + input);
            //Plugin.LoggerInstance.LogInfo("Axis:" + Var.debugInt2);
            //Debug.Log(Var.heightOffset);

            //if (Input.GetKeyDown(Var.acceptButton)) { Var.debugInt2 += 1; }
            //Plugin.LoggerInstance.LogInfo(wTexture);
            //DaggerfallUI daggerfallUI = GameObject.Find("DaggerfallUI").GetComponent<DaggerfallUI>();
            //FieldInfo directionField = AccessTools.Field(typeof(DaggerfallUI), "customRenderTarget");
            //Plugin.LoggerInstance.LogInfo(directionField.GetValue(daggerfallUI));
            //if (Input.GetKeyDown(KeyCode.K))
            //{
            //    Plugin.LoggerInstance.LogInfo("Pressed");
            //    StartCoroutine(SpawnUI.Calibrate());
            //}
            //wTexture = GameObject.Find("VRUI").GetComponent<UwcWindowTexture>();
            //if (wTexture != null)
            //{
            //    windowPosX = wTexture.window.x;
            //    windowPosY = wTexture.window.y;
            //    windowWidth = wTexture.window.width;
            //    windowHeight = wTexture.window.height;
            //    Plugin.LoggerInstance.LogInfo(windowWidth+", "+windowHeight);
            //}

            TriggerProvider.CheckPressedRight();
            if (trackedPoseDriver != null && lineRenderer != null)
            {
                Vector3 controllerPosition = trackedPoseDriver.transform.localPosition;
                Quaternion controllerRotation = trackedPoseDriver.transform.localRotation;

                Vector3 lineStart = trackedPoseDriver.transform.position;
                Vector3 lineEnd = trackedPoseDriver.transform.position + trackedPoseDriver.transform.forward * 10f;

                lineRenderer.SetPosition(0, lineStart);
                lineRenderer.SetPosition(1, lineEnd);
                if (Input.GetKeyDown(Var.acceptButton) || Var.rTriggerDone || Var.lTriggerDone)
                {
                    if (!Var.fStartMenu)
                    {
                        if (!Var.isCalibrated)
                        {
                            var result = UwcWindowTexture.RayCast(lineStart, trackedPoseDriver.transform.forward, raycastDistance, -1);
                            if (result.hit)
                            {
                                //uWindowCapture really needs a proper documentation. I wish I knew sooner that it had a precise raycast function built in before making one myself...
                                //windowCoord = result.windowCoord;
                                Vector2 desktopCoord = result.desktopCoord;
                                SetCursorPos((int)desktopCoord.x, (int)desktopCoord.y);
                                mouse_event(MOUSEEVENTF_LEFTDOWN, (uint)desktopCoord.x, (uint)desktopCoord.y, 0, 0);
                                mouse_event(MOUSEEVENTF_LEFTUP, (uint)desktopCoord.x, (uint)desktopCoord.y, 0, 0);
                            }
                        }
                        else
                        {
                            //RaycastHit hit;

                            //if (Physics.Raycast(lineStart, trackedPoseDriver.transform.forward, out hit, raycastDistance))
                            //{
                            //    if (hit.collider.gameObject.name == "VRUI")
                            //    {
                            //        if (Input.GetKeyDown(Var.acceptButton))
                            //        {
                            //            Plugin.LoggerInstance.LogInfo("Raycast Hit: " + hit.point);
                            //            SimulateMouseClick(hit.point);
                            //        }
                            //    }
                            //    if (Input.GetKeyDown(Var.acceptButton))
                            //    {
                            //        if (hit.collider.gameObject.name != "VRUI")
                            //        { Plugin.LoggerInstance.LogInfo("Hit " + hit.collider.gameObject.name); }
                            //    }
                            //}
                            RaycastHit[] hits = Physics.RaycastAll(lineStart, trackedPoseDriver.transform.forward, raycastDistance);

                            foreach (var hit in hits)
                            {
                                Plugin.LoggerInstance.LogInfo("clicked");
                                if (hit.collider.gameObject.name == "VRUI")
                                {
                                    Vector3 localPoint = hit.collider.gameObject.transform.InverseTransformPoint(hit.point);
                                    MeshFilter mFilter = hit.collider.gameObject.GetComponent<MeshFilter>();

                                    var bounds = mFilter.mesh.bounds;

                                    float u = Mathf.InverseLerp(bounds.min.x, bounds.max.x, localPoint.x);
                                    float v = Mathf.InverseLerp(bounds.min.y, bounds.max.y, localPoint.y);

                                    float screenX = u * Display.main.systemWidth;
                                    float screenY = Display.main.systemHeight - (v * Display.main.systemHeight);

                                    float windowAspect = (float)Screen.width / (float)Screen.height;
                                    float displayAspect = (float)Display.main.systemWidth / (float)Display.main.systemHeight;
                                    float aspectRatio = windowAspect / displayAspect;

                                    float offsetX = (Display.main.systemWidth - (Display.main.systemWidth * aspectRatio)) / 2;

                                    screenX = (screenX * aspectRatio) + offsetX;

                                    SimulateMouseClick(new Vector2(screenX, screenY));
                                    break;
                                }

                                if (hit.collider.gameObject.GetComponent<Button>() != null)
                                {
                                    hit.collider.gameObject.GetComponent<Button>().onClick.Invoke();
                                }

                            }
                        }
                    }
                    else
                    {
                        RaycastHit[] hits = Physics.RaycastAll(lineStart, trackedPoseDriver.transform.forward, raycastDistance);

                        foreach (var hit in hits)
                        {
                            Plugin.LoggerInstance.LogInfo("clicked");
                            if (hit.collider.gameObject.GetComponent<Button>() != null)
                            {
                                hit.collider.gameObject.GetComponent<Button>().onClick.Invoke();
                            }
                            else if (hit.collider.gameObject.GetComponent<Dropdown>() != null)
                            {
                                Dropdown dropdown = hit.collider.gameObject.GetComponent<Dropdown>();


                                dropdown.value = (dropdown.value + 1) % dropdown.options.Count;
                                dropdown.RefreshShownValue();
                            }
                        }
                    }
                }
            }


        }
        //void LateUpdate()
        //{
        //    string[] joystickNames = Input.GetJoystickNames();
        //    foreach (string name in joystickNames)
        //    {
        //        Plugin.LoggerInstance.LogInfo("Joystick: " + name);
        //    }
        //}



        //deprecated
        private void SimulateMouseClick(Vector3 hitPoint)
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN, (uint)hitPoint.x, (uint)hitPoint.y, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, (uint)hitPoint.x, (uint)hitPoint.y, 0, 0);
            SetCursorPos((int)hitPoint.x, (int)hitPoint.y);

            return;
        }
    }
}



