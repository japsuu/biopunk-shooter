using System.Collections.Generic;
using System.Linq;
using Cameras;
using Singletons;
using Thirdparty.UnityTooltips.Scripts.UI;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Thirdparty.UnityTooltips.Scripts.Systems
{
    public class ObjectPrimaryClicked
    {
        public GameObject Object { get; set; }
    }

    public delegate void PointerEvent<T>(T data);

    public delegate void PointerAction();

    public class TooltipControlSystem : SingletonBehaviour<TooltipControlSystem>
    {
        public GraphicRaycaster Raycaster;
        public EventSystem EventSystem;
        private PointerEventData _pointerEventData;

        public Camera Camera;
        public PointerViewController Pointer;

        public List<GameObject> HoveredObjects { get; private set; }
        public GameObject PrimaryHoveredObject => HoveredObjects.FirstOrDefault();
        public List<GameObject> HoveredUiObjects { get; private set; }
        public GameObject PrimaryHoveredUiObject => HoveredUiObjects.FirstOrDefault();

        public List<GameObject> DebugList;

        public PointerEvent<string> PointerTooltipUpdated;
        public PointerEvent<PointerDisplayModeChanged> PointerDisplayModeChanged;
        public PointerAction PointerDisplayModeReset;
        public PointerAction PrimaryMouseClicked;
        public PointerAction SecondaryMouseClicked;

        public PointerEvent<ObjectPrimaryClicked> ObjectPrimaryClicked;


        // Start is called before the first frame update
        private void Start()
        {
            Assert.IsNotNull(Pointer);
            Assert.IsNotNull(Camera);
            Assert.IsNotNull(Raycaster);
            Assert.IsNotNull(EventSystem);

            HoveredObjects = new List<GameObject>();
            HoveredUiObjects = new List<GameObject>();

            PrimaryMouseClicked += () =>
            {
                RaycastHit hit;
                Ray ray = CameraController.Instance.Camera.ScreenPointToRay(Pointer.Position);

                if (Physics.Raycast(ray, out hit, 100))
                {
                    Transform objectHit = hit.transform;

                    ObjectPrimaryClicked?.Invoke(
                        new ObjectPrimaryClicked()
                        {
                            Object = objectHit.gameObject
                        });
                }
            };
        }


        public void OnPointerTooltipUpdated(string msg)
        {
            PointerTooltipUpdated?.Invoke(msg);
        }


        public void OnPointerDisplayModeChanged(PointerDisplayModeChanged e)
        {
            PointerDisplayModeChanged?.Invoke(e);
        }


        public void OnPointerDisplayModeReset()
        {
            PointerDisplayModeReset?.Invoke();
        }


        private void FixedUpdate()
        {
            HoveredObjects.Clear();
            HoveredUiObjects.Clear();

            // Scene objects

            RaycastHit2D hit = Physics2D.Raycast(CameraController.Instance.Camera.ScreenToWorldPoint(Pointer.Position), Vector2.zero);

            if (hit.collider != null)
                HoveredObjects.Add(hit.collider.gameObject);

            // UI objects

            _pointerEventData = new PointerEventData(EventSystem)
            {
                position = Pointer.Position
            };

            //Create a list of Raycast Results
            List<RaycastResult> results = new();

            //Raycast using the Graphics Raycaster and mouse click position
            Raycaster.Raycast(_pointerEventData, results);

            //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
            foreach (RaycastResult result in results)
                HoveredUiObjects.Add(result.gameObject);

            DebugList = HoveredUiObjects;
        }


        // Update is called once per frame
        private void Update()
        {
            if (Input.GetMouseButtonUp(0))
                PrimaryMouseClicked?.Invoke();

            if (Input.GetMouseButtonUp(1))
                SecondaryMouseClicked?.Invoke();
        }
    }
}