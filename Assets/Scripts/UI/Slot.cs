﻿using System;
using Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// Slot that can contain a single item.
    /// Allows the player to:
    /// - drag and drop an item from the world into a slot.
    /// - drag and drop an item from a slot to some other slot, to move it between slots.
    /// - drop the assigned item back into the world, if dragged and dropped outside of any slots.
    /// </summary>
    public class Slot : MonoBehaviour, IDragHandler, IBeginDragHandler
    {
        [SerializeField]
        private Image _assignedItemImage;
        
        [SerializeField]
        private Image _highlightItemCompatibilityImage;
        
        [SerializeField]
        private bool _canRemoveItem = true;

        public SlotType SlotType { get; private set; }
        public ItemData AssignedItem { get; private set; }

        private Action _onContentsChangeCallback;
        
        
        public bool CanAcceptItem(ItemData item)
        {
            return item.Type switch
            {
                ItemType.Organ => SlotType == SlotType.Body,
                ItemType.ProjectileEvent => SlotType == SlotType.ProjectileEvent,
                ItemType.Weapon => SlotType == SlotType.Weapon,
                _ => throw new ArgumentOutOfRangeException()
            };
        }


        public bool HasItem => AssignedItem != null;


        /// <summary>
        /// Called by the SlotManager on startup.
        /// </summary>
        public void Initialize(SlotType type, Action onContentsChangeCallback)
        {
            SlotType = type;
            _onContentsChangeCallback = onContentsChangeCallback;
            DraggableItem.OnDraggedItemChanged += OnDraggedItemChanged;
        }


        private void OnDraggedItemChanged(ItemData obj)
        {
            // Change the highlight image color based on whether the dragged item can be assigned to this slot.
            if (obj == null)
            {
                _highlightItemCompatibilityImage.color = Color.clear;
                return;
            }
            _highlightItemCompatibilityImage.color = CanAcceptItem(obj) ? new Color(0f, 1f, 0f, 0.1f) : new Color(1f, 0f, 0f, 0.1f);
        }


        public void AssignItem(ItemData item, bool callCallback = true)
        {
            OnAssignItem(item, callCallback);
        }


        private void OnAssignItem(ItemData item, bool callCallback)
        {
            AssignedItem = item;
            _assignedItemImage.sprite = item.UiSprite;
            _assignedItemImage.enabled = true;
            if (callCallback)
                _onContentsChangeCallback?.Invoke();
        }
        
        
        private void OnRemoveItem()
        {
            AssignedItem = null;
            _assignedItemImage.sprite = null;
            _assignedItemImage.enabled = false;
            _onContentsChangeCallback?.Invoke();
        }


        public void OnDrag(PointerEventData eventData)
        {
            // Unused.
        }


        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!_canRemoveItem)
                return;
            if (AssignedItem == null)
                return;
            DraggableItem.Instance.Initialize(AssignedItem, this);
            OnRemoveItem();
        }
    }
}