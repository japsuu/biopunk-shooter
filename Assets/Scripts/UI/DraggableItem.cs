using System;
using System.Collections.Generic;
using Entities.Player;
using Items;
using Singletons;
using Thirdparty.UnityTooltips.Scripts.Systems;
using Thirdparty.UnityTooltips.Scripts.UI;
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
        
        public bool IsDragging => _itemData != null;
        
        [SerializeField]
        private Image _assignedItemBackgroundImage;
        [SerializeField]
        private Image _assignedItemImage;
        [SerializeField]
        private Image _assignedItemOverlayImage;

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
            _assignedItemBackgroundImage.color = itemData.UiBackgroundColor;
            _assignedItemBackgroundImage.sprite = itemData.UiBackgroundSprite;
            _assignedItemImage.sprite = itemData.UiSprite;
            _assignedItemOverlayImage.sprite = itemData.UiOverlaySprite;
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
            
            TooltipControlSystem.Instance.OnPointerDisplayModeChanged(
                new PointerDisplayModeChanged
                {
                    Mode = PointerDisplayMode.Grabbing
                });
            
            _rectTransform.position = Input.mousePosition;
            
            Slot newSlot = TryFindSlot();

            if (!Input.GetMouseButtonUp(0))
                return;
            
            if (newSlot != null)
            {
                if (newSlot.CanAcceptItem(_itemData))
                {
                    if (_oldSlot != null)
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
                    
                    // The item was dragged from the world, but the new slot already has an item.
                    // Drop the existing item back into the world, and assign the new item to the slot.
                    if (newSlot.HasItem)
                    {
                        WorldItemSpawner.SpawnWorldItem(newSlot.AssignedItem, _oldPosition);
                        newSlot.AssignItem(_itemData);
                        DeInit();
                        return;
                    }

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

                WorldItemSpawner.SpawnWorldItem(_itemData, _oldPosition);
                DeInit();
                return;
            }
            
            // If no suitable slot was found, move back to old slot or drop the item back into the world.
            if (_oldSlot != null)
            {
                WorldItemSpawner.SpawnWorldItem(_itemData, PlayerController.Instance.transform.position);
                DeInit();
                return;
            }
            WorldItemSpawner.SpawnWorldItem(_itemData, _oldPosition);
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