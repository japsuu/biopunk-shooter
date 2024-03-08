using System;
using System.Collections.Generic;
using Entities.Player;
using Items;
using Singletons;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// Initialized when the player starts dragging an item from the world, or from a slot.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class DraggableItem : SingletonBehaviour<DraggableItem>
    {
        public static event Action<ItemData> OnDraggedItemChanged;
        
        [SerializeField]
        private Image _assignedItemImage;

        private RectTransform _rectTransform;
        
        private Vector2 _oldPosition;
        private Slot _oldSlot;
        private ItemData _itemData;


        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            GetComponentInParent<GraphicRaycaster>();
        }


        public void Initialize(ItemData itemData, Vector2 oldPos)
        {
            _itemData = itemData;
            _oldPosition = oldPos;
            Init(itemData);
        }


        public void Initialize(ItemData itemData, Slot oldSlot)
        {
            _itemData = itemData;
            _oldSlot = oldSlot;
            Init(itemData);
        }


        private void Init(ItemData itemData)
        {
            _assignedItemImage.sprite = itemData.UiSprite;
            OnDraggedItemChanged?.Invoke(itemData);
        }
        
        
        private void DeInit()
        {
            _itemData = null;
            _oldSlot = null;
            _assignedItemImage.sprite = null;
            _rectTransform.position = new Vector3(-10000, -10000);  // Hacky!
            OnDraggedItemChanged?.Invoke(null);
        }


        private void Update()
        {
            if (_itemData == null)
                return;
            
            _rectTransform.position = Input.mousePosition;
            
            Slot newSlot = TryFindSlot();

            if (!Input.GetMouseButtonUp(0))
                return;
            
            if (newSlot != null)
            {
                if (newSlot.CanAcceptItem(_itemData))
                {
                    // Swap items.
                    if (newSlot.HasItem)
                    {
                        if (_oldSlot.CanAcceptItem(newSlot.AssignedItem))
                        {
                            ItemData oldItem = newSlot.AssignedItem;
                            newSlot.AssignItem(_itemData);
                            _oldSlot.AssignItem(oldItem);
                            DeInit();
                            return;
                        }

                        _oldSlot.AssignItem(_itemData);
                        DeInit();
                        return;
                    }

                    // Assign item.
                    newSlot.AssignItem(_itemData);
                    DeInit();
                    return;
                }
                
                // If cannot move to slot, move back to old slot or drop the item back into the world.
                if (_oldSlot != null)
                {
                    _oldSlot.AssignItem(_itemData);
                    DeInit();
                    return;
                }

                Instantiate(_itemData.WorldItemPrefab, _oldPosition, Quaternion.identity).Initialize(_itemData);
                DeInit();
                return;
            }
            
            // If no suitable slot was found, move back to old slot or drop the item back into the world.
            if (_oldSlot != null)
            {
                Instantiate(_itemData.WorldItemPrefab, PlayerController.Instance.transform.position, Quaternion.identity).Initialize(_itemData);
                DeInit();
                return;
            }
            Instantiate(_itemData.WorldItemPrefab, _oldPosition, Quaternion.identity).Initialize(_itemData);
            DeInit();
        }


        private static Slot TryFindSlot()
        {
            List<RaycastResult> results = new();
            EventSystem.current.RaycastAll(new PointerEventData(EventSystem.current) { position = Input.mousePosition }, results);
            
            foreach (RaycastResult result in results)
            {
                Slot slot = result.gameObject.GetComponent<Slot>();
                if (slot == null)
                    continue;
                
                return slot;
            }

            return null;
        }
    }
}