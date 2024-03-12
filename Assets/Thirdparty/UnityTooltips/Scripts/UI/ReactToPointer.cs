using NaughtyAttributes;
using Thirdparty.UnityTooltips.Scripts.Systems;
using UnityEngine;
using UnityEngine.Assertions;

namespace Thirdparty.UnityTooltips.Scripts.UI
{
    public delegate void PointerReactor();

    public class ReactToPointer : MonoBehaviour
    {
        public PointerReactor OnPointerEntered;
        public PointerReactor OnPointerExited;
        public PointerReactor OnPointerSelected;

        public string TooltipMessage;
        public PointerDisplayMode PointerDisplayMode;
        //public PointerDisplayMode DragPointerDisplayMode;

        [SerializeField, ReadOnly]
        public bool _focused;
        public bool HasFocus => _focused;

        private float _hoverDelayTimer;

        public float HoverDelay;
        private bool _delayElapsed;
        private bool _focusTriggered;
        // private PointerDisplayMode _displayMode;
        // private PointerDisplayMode _prevDisplayMode;


        // Start is called before the first frame update
        private void Start()
        {
            Assert.IsNotNull(TooltipControlSystem.Instance);
            
            // _displayMode = PointerDisplayMode;
            // _prevDisplayMode = _displayMode;

            TooltipControlSystem.Instance.PrimaryMouseClicked += ReactToMouseClick;
        }


        private void ReactToMouseClick()
        {
            if (!_focused || !gameObject.activeInHierarchy)
                return;
            OnPointerSelected?.Invoke();
        }


        private void OnDisable()
        {
            OnPointerExited?.Invoke();
            if (TooltipControlSystem.Instance != null)
            {
                LostFocus();
                TooltipControlSystem.Instance.PrimaryMouseClicked -= ReactToMouseClick;
                TooltipControlSystem.Instance.OnPointerTooltipUpdated("");
            }
        }


        // Update is called once per frame
        private void Update()
        {
            // bool displayModeChanged = false;
            /*if (PointerDisplayMode != DragPointerDisplayMode)
            {
                _displayMode = Input.GetMouseButton(0) ? DragPointerDisplayMode : PointerDisplayMode;
                // displayModeChanged = _displayMode != _prevDisplayMode;
                // _prevDisplayMode = _displayMode;
            }*/
            bool focused = TooltipControlSystem.Instance.PrimaryHoveredUiObject == null && TooltipControlSystem.Instance.PrimaryHoveredObject == gameObject;

            if (TooltipControlSystem.Instance.HoveredUiObjects.Contains(gameObject))
                focused = true;

            // Gained focus
            /*if (displayModeChanged)
                print($"displayModeChanged: {displayModeChanged}, focused: {focused}, _focusTriggered: {_focusTriggered}");*/
            if (_delayElapsed && focused && (!_focusTriggered/* || displayModeChanged*/))
            {
                _focusTriggered = true;
                OnPointerEntered?.Invoke();

                if (!string.IsNullOrEmpty(TooltipMessage))
                    TooltipControlSystem.Instance.OnPointerTooltipUpdated(TooltipMessage);

                TooltipControlSystem.Instance.OnPointerDisplayModeChanged(
                    new PointerDisplayModeChanged
                    {
                        Mode = /*_displayMode*/ PointerDisplayMode
                    });
            }


            // Lost focus
            if (_focused && !focused)
            {
                LostFocus();
            }

            if (focused)
            {
                _hoverDelayTimer += Time.deltaTime;

                if (_hoverDelayTimer >= HoverDelay)
                    _delayElapsed = true;
            }
            else
            {
                _hoverDelayTimer = 0;
                _delayElapsed = false;
                _focusTriggered = false;
            }

            _focused = focused;
        }


        private void LostFocus()
        {
            OnPointerExited?.Invoke();

            if (!string.IsNullOrEmpty(TooltipMessage))
                TooltipControlSystem.Instance.OnPointerTooltipUpdated("");

            TooltipControlSystem.Instance.OnPointerDisplayModeReset();
        }
    }
}