using System;
using Thirdparty.UnityTooltips.Scripts.Systems;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

namespace Thirdparty.UnityTooltips.Scripts.UI
{
    public enum PointerMode
    {
        Mouse,
        Controller
    }

    public enum PointerDisplayMode
    {
        Crosshair,
        Point,
        Grab,
        Grabbing
    }

    public class PointerTooltipUpdated
    {
        public string Message { get; set; }
    }

    public class PointerDisplayModeChanged
    {
        public PointerDisplayMode Mode { get; set; }
    }

    public class PointerViewController : MonoBehaviour
    {
        public PointerMode Mode;

        public PointerDisplayMode DefaultDisplayMode = PointerDisplayMode.Crosshair;
        public GameObject CrosshairPointerDisplay;
        public GameObject PointPointerDisplay;
        public GameObject GrabPointerDisplay;
        public GameObject GrabbingPointerDisplay;
        public float ControllerPointerSpeed = 512;
        public Vector3 Offset;
        public string TooltipMessage;
        public TMP_Text TooltipText;
        public GameObject TooltipDisplay;

        private Vector3 _pointerPosition;

        public Vector2 Position => _pointerPosition;

        private PointerDisplayModeChanged _defaultPointerConfig;


        // Start is called before the first frame update
        private void Start()
        {
            Assert.IsNotNull(TooltipControlSystem.Instance);
            Assert.IsNotNull(TooltipText);
            Assert.IsNotNull(TooltipDisplay);
            Assert.IsNotNull(CrosshairPointerDisplay);
            Assert.IsNotNull(PointPointerDisplay);
            Assert.IsNotNull(GrabPointerDisplay);
            Assert.IsNotNull(GrabbingPointerDisplay);

            _defaultPointerConfig = new PointerDisplayModeChanged
            {
                Mode = DefaultDisplayMode
            };
            
            TooltipControlSystem.Instance.PointerTooltipUpdated += msg => { TooltipMessage = msg; };

            TooltipControlSystem.Instance.PointerDisplayModeChanged += ChangeDisplayMode;
            TooltipControlSystem.Instance.PointerDisplayModeReset += () => { ChangeDisplayMode(_defaultPointerConfig); };

            ChangeDisplayMode(_defaultPointerConfig);
        }


        private void ChangeDisplayMode(PointerDisplayModeChanged change)
        {
            View.Hide(CrosshairPointerDisplay);
            View.Hide(PointPointerDisplay);
            View.Hide(GrabPointerDisplay);
            View.Hide(GrabbingPointerDisplay);

            switch (change.Mode)
            {
                case PointerDisplayMode.Crosshair:
                    View.Show(CrosshairPointerDisplay);
                    break;
                case PointerDisplayMode.Point:
                    View.Show(PointPointerDisplay);
                    break;
                case PointerDisplayMode.Grab:
                    View.Show(GrabPointerDisplay);
                    break;
                case PointerDisplayMode.Grabbing:
                    View.Show(GrabbingPointerDisplay);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        // Update is called once per frame
        private void LateUpdate()
        {
            switch (Mode)
            {
                case PointerMode.Mouse:
                    _pointerPosition = Input.mousePosition;
                    Cursor.visible = false;
                    break;
                case PointerMode.Controller:
                    _pointerPosition += Time.deltaTime * ControllerPointerSpeed * new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
                    Cursor.visible = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            transform.position = _pointerPosition + Offset; // TODO: consider  * _settings.Settings.UiScale here

            if (!string.IsNullOrEmpty(TooltipMessage))
            {
                View.Show(TooltipDisplay);
                TooltipText.text = TooltipMessage;
            }
            else
            {
                View.Hide(TooltipDisplay);
            }
        }
    }
}