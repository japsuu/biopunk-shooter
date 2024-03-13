using System.Collections.Generic;
using Entities.Player;
using Items;
using NaughtyAttributes;
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
        private Animator _leftWeaponAnimator;
        
        [SerializeField]
        private Animator _rightWeaponAnimator;

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
        [SerializeField, ReadOnly]
        private WeaponData _currentLeftWeapon;
        [SerializeField, ReadOnly]
        private WeaponData _currentRightWeapon;
        private bool _leftPlayingIdle;
        private bool _rightPlayingIdle;


        public void PlayIdleAnimation(bool isRight, bool force = false)
        {
            if (isRight)
            {
                if (!force && _rightPlayingIdle)
                    return;
                _rightWeaponAnimator.Play(_currentRightWeapon.IdleAnimationName);
                _rightPlayingIdle = true;
            }
            else
            {
                if (!force && _leftPlayingIdle)
                    return;
                _leftWeaponAnimator.Play(_currentLeftWeapon.IdleAnimationName);
                _leftPlayingIdle = true;
            }
        }
        
        
        public void PlayFiringAnimation(bool isRight)
        {
            if (isRight)
            {
                if (!_rightPlayingIdle)
                    return;
                _rightWeaponAnimator.Play(_currentRightWeapon.FiringAnimationName);
                _rightPlayingIdle = false;
            }
            else
            {
                if (!_leftPlayingIdle)
                    return;
                _leftWeaponAnimator.Play(_currentLeftWeapon.FiringAnimationName);
                _leftPlayingIdle = false;
            }
        }
        
        
        private void Awake()
        {
            foreach (Slot slot in _bodySlots)
            {
                slot.Initialize(SlotType.Body, OnOrganSlotContentsChanged);
            }
            _leftWeaponSlot.Initialize(SlotType.Weapon, () =>
            {
                PlayerController.Instance.OnWeaponSlotChanged((WeaponData)_leftWeaponSlot.AssignedItem, false);
            });
            _rightWeaponSlot.Initialize(SlotType.Weapon, () =>
            {
                PlayerController.Instance.OnWeaponSlotChanged((WeaponData)_rightWeaponSlot.AssignedItem, true);
            });
            
            PlayerWeaponManager.OnWeaponChanged += OnWeaponChanged;
        }


        private void OnWeaponChanged((RuntimeWeaponData newWeapon, bool isRightWeapon) obj)
        {
            (RuntimeWeaponData newWeapon, bool isRightWeapon) = obj;
            if (isRightWeapon)
            {
                _currentRightWeapon = newWeapon.Weapon;
                _rightEventSlots = GenerateEventSlots(newWeapon, true);
                _rightWeaponSlot.AssignItem(newWeapon.Weapon, false);
                PlayIdleAnimation(true, true);  // Force the animation to update.
            }
            else
            {
                _currentLeftWeapon = newWeapon.Weapon;
                _leftEventSlots = GenerateEventSlots(newWeapon, false);
                _leftWeaponSlot.AssignItem(newWeapon.Weapon, false);
                PlayIdleAnimation(false, true);  // Force the animation to update.
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