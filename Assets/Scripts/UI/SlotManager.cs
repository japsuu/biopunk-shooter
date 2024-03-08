using System;
using System.Collections.Generic;
using Items;
using Singletons;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// Initializes the available slots, and listens for changes in the equipped items.
    /// </summary>
    public class SlotManager : SingletonBehaviour<SlotManager>
    {
        /// <summary>
        /// Called when the equipped items of a certain slot type have changed.
        /// </summary>
        public static event Action<(SlotType slot, List<ItemData> newItems)> OnEquippedItemsChanged;
        
        [SerializeField]
        private Canvas _canvas;
        
        [SerializeField]
        private List<Slot> _leftHandSlots;      // Affects the behaviour of weapon 1.
        
        [SerializeField]
        private List<Slot> _rightHandSlots;     // Affects the behaviour of weapon 2.
        
        [SerializeField]
        private List<Slot> _bodySlots;          // Affects the player's stats.
        
        private readonly Dictionary<SlotType, List<Slot>> _slotsMap = new();

        public Canvas Canvas => _canvas;
        
        
        private void Awake()
        {
            PrepareSlots(_leftHandSlots, SlotType.LeftHand);
            PrepareSlots(_rightHandSlots, SlotType.RightHand);
            PrepareSlots(_bodySlots, SlotType.Body);
            Slot.OnSlotContentsChanged += OnSlotContentsChanged;
        }


        private void PrepareSlots(List<Slot> slots, SlotType type)
        {
            foreach (Slot slot in slots)
                slot.Initialize(type);
            _slotsMap.Add(type, slots);
        }
        
        
        /// <summary>
        /// Called by a slot when its contents have changed.
        /// </summary>
        private void OnSlotContentsChanged(Slot changedSlot)
        {
            List<ItemData> newItems = new();
            foreach (Slot slot in _slotsMap[changedSlot.SlotType])
            {
                if (slot.AssignedItem != null)
                    newItems.Add(slot.AssignedItem);
            }
            OnEquippedItemsChanged?.Invoke((changedSlot.SlotType, newItems));
        }
    }
}