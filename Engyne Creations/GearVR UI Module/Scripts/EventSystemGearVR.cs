/*
 * EventSystemGearVR.cs
 * Handles all inputs produced by the GearVR Touchpad using delegates.
 * 
 * by Adam Carballo under GPLv3 license.
 * https://github.com/AdamEC/Unity-GearVR-UI-Module
 */

using UnityEngine;

namespace EC_GearVR {
    public class EventSystemGearVR : MonoBehaviour {

        public ButtonGearVR firstSelected;

        public delegate void OnInput(OVRInput.Button button);
        static public event OnInput onInput;


        void Start() {

            if (firstSelected) {
                firstSelected.OnSelect();
            }
        }

        void Update() {

            if (onInput != null) {
#if UNITY_EDITOR
                if (Input.GetKeyDown(KeyCode.Return)) {
                    GetOVRInput();
                }
#endif
                // Return true if any button / Pad is pressed during this frame
                if (OVRInput.Get(OVRInput.Button.Any) || OVRInput.GetUp(OVRInput.Button.Any)) {
                    GetOVRInput();
                }
            }
        }

        /// <summary>
        /// Handles all Gear VR Inputs available on the HMD.
        /// </summary>
        private void GetOVRInput() {

            OVRInput.Button finalInput = OVRInput.Button.None;

#if UNITY_EDITOR
            finalInput = OVRInput.Button.One;
#endif

            // Returns true after a Gear VR touchpad tap
            if (OVRInput.GetDown(OVRInput.Button.One)) {
                finalInput = OVRInput.Button.One;
            }

            // Returns true on the frame when a user’s finger pulled off Gear VR touchpad controller after a swipe
            if (OVRInput.GetUp(OVRInput.Button.DpadUp)) {
                finalInput = OVRInput.Button.DpadUp;
            }
            else if (OVRInput.GetUp(OVRInput.Button.DpadDown)) {
                finalInput = OVRInput.Button.DpadDown;
            }
            else if (OVRInput.GetUp(OVRInput.Button.DpadRight)) {
                finalInput = OVRInput.Button.DpadRight;
            }
            else if (OVRInput.GetUp(OVRInput.Button.DpadLeft)) {
                finalInput = OVRInput.Button.DpadLeft;
            }

            // Returns true if the Gear VR back button is pressed
            if (OVRInput.GetDown(OVRInput.Button.Two)) {
                finalInput = OVRInput.Button.Two;
            }

            if (finalInput == OVRInput.Button.None) {
                Debug.LogError("Button call was outside current configured. Premature exit.");
                return;
            }

            onInput(finalInput);
        }
    }
}