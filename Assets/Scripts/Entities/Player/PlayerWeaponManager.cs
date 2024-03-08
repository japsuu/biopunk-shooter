using System.Collections.Generic;
using Items;
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
        [SerializeField]
        private Projectile _projectilePrefab;
        
        [SerializeField]
        private WeaponObject _leftWeapon;
        
        [SerializeField]
        private WeaponObject _rightWeapon;
        
        [SerializeField]
        private bool _allowFiring = true;


        public void SetLeftHandWeaponParts(List<ItemData> objNewItems)
        {
            UpdateWeaponParts(objNewItems, _leftWeapon);
        }


        public void SetRightHandWeaponParts(List<ItemData> objNewItems)
        {
            UpdateWeaponParts(objNewItems, _rightWeapon);
        }


        private void Start()
        {
            _leftWeapon.Initialize(_projectilePrefab, this);
            _rightWeapon.Initialize(_projectilePrefab, this);
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


        private static void UpdateWeaponParts(IReadOnlyList<ItemData> objNewItems, WeaponObject weapon)
        {
            WeaponPartData[] parts = new WeaponPartData[objNewItems.Count];
            for (int i = 0; i < objNewItems.Count; i++)
            {
                parts[i] = (WeaponPartData)objNewItems[i];
            }
            weapon.SetParts(parts);
        }


        public void NotifyOfKill(IDamageable killed)
        {
            float rewardXp = killed.KillRewardXp;
            PlayerStats.Instance.AddExperience(rewardXp);
        }
    }
}