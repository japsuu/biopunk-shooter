using System;
using UI;
using UnityEngine;
using Weapons;

namespace Entities.Player
{
    /// <summary>
    /// Provides weapon functionality for the player.
    /// The player has two weapons, one in each hand.
    /// LMB fires the left weapon, RMB fires the right weapon.
    /// </summary>
    public class PlayerWeaponManager : MonoBehaviour, IDamageCauser
    {
        public static event Action<(RuntimeWeaponData newWeaponData, bool isRightWeapon)> OnWeaponChanged;
        
        [SerializeField]
        private WeaponData _defaultLeftData;
        
        [SerializeField]
        private WeaponData _defaultRightData;
        
        [SerializeField]
        private ProjectileEventData[] _defaultEventsLeft;
        
        [SerializeField]
        private ProjectileEventData[] _defaultEventsRight;
        
        [SerializeField]
        private WeaponObject _leftWeapon;
        
        [SerializeField]
        private WeaponObject _rightWeapon;
        
        [SerializeField]
        private bool _allowFiring = true;


        private void Start()
        {
            ChangeLeftWeapon(_defaultLeftData, _defaultEventsLeft);
            ChangeRightWeapon(_defaultRightData, _defaultEventsRight);
        }
        
        
        public void ChangeLeftWeapon(WeaponData weaponData, ProjectileEventData[] defaultEvents = null)
        {
            RuntimeWeaponData d = new(weaponData);
            
            if (defaultEvents != null)
                d.CopyEvents(defaultEvents);

            _leftWeapon.Initialize(d, this);
            OnWeaponChanged?.Invoke((d, false));
        }


        public void ChangeRightWeapon(WeaponData weaponData, ProjectileEventData[] defaultEvents = null)
        {
            RuntimeWeaponData d = new(weaponData);
            
            if (defaultEvents != null)
                d.CopyEvents(defaultEvents);
            
            _rightWeapon.Initialize(d, this);
            OnWeaponChanged?.Invoke((d, true));
        }


        public void SetLeftWeaponEvents(ProjectileEventData[] e)
        {
            _leftWeapon.SetEvents(e);
        }


        public void SetRightWeaponEvents(ProjectileEventData[] e)
        {
            _rightWeapon.SetEvents(e);
        }


        private void Update()
        {
            if (!_allowFiring)
                return;
            
            // Handle firing.
            if (Input.GetMouseButton(0) && !DraggableItem.Instance.IsDragging)
            {
                _leftWeapon.TryFire();
                SlotManager.Instance.PlayFiringAnimation(false);
            }
            else
            {
                SlotManager.Instance.PlayIdleAnimation(false);
            }
            if (Input.GetMouseButton(1) && !DraggableItem.Instance.IsDragging)
            {
                _rightWeapon.TryFire();
                SlotManager.Instance.PlayFiringAnimation(true);
            }
            else
            {
                SlotManager.Instance.PlayIdleAnimation(true);
            }
        }


        private void OnDisable()
        {
            if (SlotManager.Instance != null)
            {
                SlotManager.Instance.PlayIdleAnimation(false);
                SlotManager.Instance.PlayIdleAnimation(true);
            }
        }


        public void NotifyOfKill(IDamageable killed)
        {
            float rewardXp = killed.KillRewardXp;
            PlayerStats.Instance.AddExperience(rewardXp);
        }


        public void SetWeapon(WeaponData newWeapon, bool isRight)
        {
            if (isRight)
                ChangeRightWeapon(newWeapon);
            else
                ChangeLeftWeapon(newWeapon);
        }
    }
}