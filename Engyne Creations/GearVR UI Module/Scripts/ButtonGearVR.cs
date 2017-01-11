/*
 * ButtonGearVR.cs
 * Resembles a Unity UI basic Button, but works with the GearVR Touchpad.
 * 
 * by Adam Carballo under GPLv3 license.
 * https://github.com/AdamEC/Unity-GearVR-UI-Module
 */

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace EC_GearVR {
    [RequireComponent(typeof(Image))]
    public class ButtonGearVR : MonoBehaviour {

        [System.Serializable]
        public class Settings {
            public Image targetGraphic;
            public Color highlightedColor = new Color32(245, 245, 245, 255);
            public Color pressedColor = new Color32(200, 200, 200, 200);
            public Color disabledColor = new Color32(200, 200, 200, 128);
            [System.NonSerialized]
            public Color normalColor;
        }

        [System.Serializable]
        public class Navigation {
            public ButtonGearVR selectOnUp;
            public ButtonGearVR selectOnDown;
            public ButtonGearVR selectOnLeft;
            public ButtonGearVR selectOnRight;
            [Range(0.01f, 10f)]
            public float arrowHeadSize = 0.01f;
        }

        [Tooltip("Interactable only works on Awake()")]
        public bool interactable = true;
        public Settings settings;
        public Navigation navigation;

        [System.Serializable]
        public class EventType : UnityEvent { }
        [Space]
        public EventType OnPress;
        public EventType OnSwipeIn;
        public EventType OnSwipeOut;

        /// <summary>
        /// Try to find the attached image if the reference is null.
        /// </summary>
        private void OnValidate() {

            if (GetComponent<Image>() && settings.targetGraphic == null) {
                settings.targetGraphic = GetComponent<Image>();
            }
        }

        /// <summary>
        /// Store Image color as normalColor and if interactable is false apply disabled color.
        /// </summary>
        void Awake() {

            settings.normalColor = settings.targetGraphic.color;

            if (!interactable) {
                settings.targetGraphic.color = settings.disabledColor;
            }
        }

        /// <summary>
        /// Called when the button gets selected. Starts listening to inputs,
        /// changes color to highlightedColor and invokes OnSwipeIn event.
        /// </summary>
        public void OnSelect() {

            EventSystemGearVR.onInput -= InputReceived;
            EventSystemGearVR.onInput += InputReceived;

            settings.targetGraphic.color = settings.highlightedColor;
            OnSwipeIn.Invoke();
        }

        /// <summary>
        /// Called when the button gets deselected. Stops listening to inputs,
        /// changes color to normalColor and invokes OnSwipeOut event.
        /// </summary>
        public void OnDeselect() {

            EventSystemGearVR.onInput -= InputReceived;

            settings.targetGraphic.color = settings.normalColor;
            OnSwipeOut.Invoke();
        }

        /// <summary>
        /// Handles the input received from the HMD.
        /// </summary>
        /// <param name="type">Type of input.</param>
        private void InputReceived(OVRInput.Button type) {

            if (type == OVRInput.Button.One) {
                if (!interactable) return;
                settings.targetGraphic.color = settings.pressedColor;
                OnPress.Invoke();
            }
            else if (type == OVRInput.Button.DpadUp) {
                if (navigation.selectOnUp) {
                    OnDeselect();
                    navigation.selectOnUp.OnSelect();
                }
            }
            else if (type == OVRInput.Button.DpadDown) {
                if (navigation.selectOnDown) {
                    OnDeselect();
                    navigation.selectOnDown.OnSelect();
                }
            }
            else if (type == OVRInput.Button.DpadRight) {
                if (navigation.selectOnRight) {
                    OnDeselect();
                    navigation.selectOnRight.OnSelect();
                }
            }
            else if (type == OVRInput.Button.DpadLeft) {
                if (navigation.selectOnLeft) {
                    OnDeselect();
                    navigation.selectOnLeft.OnSelect();
                }
            }
        }

        /// <summary>
        /// Draws arrows to visualize the UI flow.
        /// </summary>
        void OnDrawGizmos() {

            Color color = new Color32(255, 140, 0, 255);

            if (navigation.selectOnUp) {
                DrawArrow.GearVR(transform.position, navigation.selectOnUp.transform.position, color, navigation.arrowHeadSize, 15f);
            }
            if (navigation.selectOnDown) {
                DrawArrow.GearVR(transform.position, navigation.selectOnDown.transform.position, color, navigation.arrowHeadSize, 15f);
            }
            if (navigation.selectOnRight) {
                DrawArrow.GearVR(transform.position, navigation.selectOnRight.transform.position, color, navigation.arrowHeadSize, 15f);
            }
            if (navigation.selectOnLeft) {
                DrawArrow.GearVR(transform.position, navigation.selectOnLeft.transform.position, color, navigation.arrowHeadSize, 15f);
            }
        }
    }
}