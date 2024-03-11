﻿using System;
using System.Collections.Generic;
using Entities.Player;
using Items;
using Singletons;
using UnityEngine;
using Weapons;

namespace UI
{
    /// <summary>
    /// Initializes the available slots, and listens for changes in the equipped items.
    /// </summary>
    public class SlotManager : SingletonBehaviour<SlotManager>
    {
        [SerializeField]
        private List<Slot> _bodySlots;
        
        [SerializeField]
        private Slot _leftWeaponSlot;
        
        [SerializeField]
        private Slot _rightWeaponSlot;
        
        [SerializeField]
        private Slot _eventSlotPrefab;

        [Tooltip("The transform under which the left weapon\'s effect slots will be created.")]
        [SerializeField]
        private RectTransform _leftWeaponEventSlotsParent;

        [Tooltip("The transform under which the right weapon\'s effect slots will be created.")]
        [SerializeField]
        private RectTransform _rightWeaponEventSlotsParent;
        
        private List<Slot> _leftEventSlots = new();
        private List<Slot> _rightEventSlots = new();
        
        
        private void Awake()
        {
            foreach (Slot slot in _bodySlots)
            {
                slot.Initialize(SlotType.Body, OnOrganSlotContentsChanged);
            }
            _leftWeaponSlot.Initialize(SlotType.Weapon, () => PlayerController.Instance.OnWeaponSlotChanged((WeaponData)_leftWeaponSlot.AssignedItem, false));
            _rightWeaponSlot.Initialize(SlotType.Weapon, () => PlayerController.Instance.OnWeaponSlotChanged((WeaponData)_rightWeaponSlot.AssignedItem, true));
            
            PlayerWeaponManager.OnWeaponChanged += OnWeaponChanged;
        }


        private void OnWeaponChanged((RuntimeWeaponData newWeapon, bool isRightWeapon) obj)
        {
            (RuntimeWeaponData newWeapon, bool isRightWeapon) = obj;
            
            if (isRightWeapon)
            {
                _rightEventSlots = GenerateEventSlots(newWeapon, true);
                _rightWeaponSlot.AssignItem(newWeapon.Weapon, false);
            }
            else
            {
                _leftEventSlots = GenerateEventSlots(newWeapon, false);
                _leftWeaponSlot.AssignItem(newWeapon.Weapon, false);
            }
        }


        private List<Slot> GenerateEventSlots(RuntimeWeaponData newWeapon, bool isRightWeapon)
        {
            List<Slot> eventSlots = new();
            
            // Regenerate the event slots for the weapon.
            RectTransform parent = isRightWeapon ? _rightWeaponEventSlotsParent : _leftWeaponEventSlotsParent;
            foreach (Transform child in parent)
            {
                Destroy(child.gameObject);
            }

            foreach (ProjectileEventData eventData in newWeapon.Events)
            {
                Slot eventSlot = Instantiate(_eventSlotPrefab, parent);
                eventSlot.Initialize(SlotType.ProjectileEvent, () => OnProjectileEventSlotChanged(isRightWeapon));
                eventSlots.Add(eventSlot);

                if (eventData != null)
                    eventSlot.AssignItem(eventData, false);
            }
            
            return eventSlots;
        }


        private void OnProjectileEventSlotChanged(bool isRightWeapon)
        {
            if (isRightWeapon)
            {
                List<ProjectileEventData> rightEvents = new();
                
                foreach (Slot slot in _rightEventSlots)
                {
                    if (slot.AssignedItem != null)
                        rightEvents.Add((ProjectileEventData)slot.AssignedItem);
                }
                PlayerController.Instance.WeaponManager.SetRightWeaponEvents(rightEvents.ToArray());
            }
            else
            {
                List<ProjectileEventData> leftEvents = new();
                foreach (Slot slot in _leftEventSlots)
                {
                    if (slot.AssignedItem != null)
                        leftEvents.Add((ProjectileEventData)slot.AssignedItem);
                }
                PlayerController.Instance.WeaponManager.SetLeftWeaponEvents(leftEvents.ToArray());
            }
        }


        private void OnOrganSlotContentsChanged()
        {
            List<OrganData> currentData = new();
            foreach (Slot slot in _bodySlots)
            {
                if (slot.AssignedItem != null)
                    currentData.Add((OrganData)slot.AssignedItem);
            }
            PlayerController.Instance.OnOrganSlotsChanged(currentData);
        }
    }
}