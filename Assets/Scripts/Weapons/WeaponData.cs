using System;
using Items;
using UnityEngine;

namespace Weapons
{
    /// <summary>
    /// All weapons have some base stats, and can contain a certain amount of ProjectileEvents.
    /// </summary>
    [CreateAssetMenu(menuName = "Weapons/Create WeaponData", fileName = "WeaponData_", order = 0)]
    public class WeaponData : ItemData
    {
        [Header("Weapon Config")]
        
        [SerializeField]
        [Tooltip("Prefab of the projectile that this weapon fires.")]
        private Projectile _projectilePrefab;
        
        [SerializeField]
        [Tooltip("The rounds per minute of the weapon.")]
        private float _fireRateRpm = 120f;
        
        
        [Header("Event Config")]
        
        [SerializeField]
        private ProjectileEventData[] _defaultProjectileEvents;
        
        [SerializeField]
        [Tooltip("How many projectile events can this weapon contain.")]
        [Range(2, 10)]
        private int _maxProjectileEvents = 6;
        
        /*[SerializeField]  TODO: Reimplement heat system
        [Tooltip("The amount of heat generated per shot. When heat reaches 100, the weapon overheats.")]
        private float _heatGeneratedPerShot = 10f;
        
        [SerializeField]
        [Tooltip("The time it takes for the barrel to cool down, after it has overheated.")]
        private float _overheatCooldownSeconds = 2f;*/
        
        public override ItemType Type => ItemType.Weapon;
        public float FireRateRpm => _fireRateRpm;
        public Projectile ProjectilePrefab => _projectilePrefab;

        
        /// <summary>
        /// Gets the default projectile events array sized to the max projectile events, and preinitialized with the default events.
        /// </summary>
        /// <returns></returns>
        public ProjectileEventData[] GetEventArray()
        {
            ProjectileEventData[] eventArray = new ProjectileEventData[_maxProjectileEvents];
            for (int i = 0; i < _defaultProjectileEvents.Length; i++)
            {
                eventArray[i] = _defaultProjectileEvents[i];
            }

            return eventArray;
        }


        private void OnValidate()
        {
            if (_defaultProjectileEvents.Length <= _maxProjectileEvents)
                return;
            
            Debug.LogWarning("The default projectile events array is longer than the max projectile events. Truncating the array.");
            Array.Resize(ref _defaultProjectileEvents, _maxProjectileEvents);
        }
    }
}