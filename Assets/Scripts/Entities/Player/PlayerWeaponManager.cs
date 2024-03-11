using System;
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
        private WeaponObject _leftWeapon;
        
        [SerializeField]
        private WeaponObject _rightWeapon;
        
        [SerializeField]
        private bool _allowFiring = true;


        private void Start()
        {
            ChangeLeftWeapon(_defaultLeftData);
            ChangeRightWeapon(_defaultRightData);
        }
        
        
        public void ChangeLeftWeapon(WeaponData weaponData)
        {
            RuntimeWeaponData d = new(weaponData);

            _leftWeapon.Initialize(d, this);
            OnWeaponChanged?.Invoke((d, false));
        }


        public void ChangeRightWeapon(WeaponData weaponData)
        {
            RuntimeWeaponData d = new(weaponData);
            
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
            if (Input.GetMouseButton(0))
            {
                _leftWeapon.TryFire();
            }
            if (Input.GetMouseButton(1))
            {
                _rightWeapon.TryFire();
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