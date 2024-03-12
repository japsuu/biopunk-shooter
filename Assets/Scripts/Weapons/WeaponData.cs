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
        [Header("Animation Config")]
        
        [SerializeField]
        private AnimationClip _idleClip;
        
        [SerializeField]
        private AnimationClip _firingClip;
        
        [Header("Weapon Config")]
        
        [SerializeField]
        [Tooltip("Prefab of the projectile that this weapon fires.")]
        private Projectile _projectilePrefab;
        
        [SerializeField]
        [Tooltip("The rounds per minute of the weapon.")]
        private float _fireRateRpm = 120f;
        
        
        [Header("Event Config")]
        
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
        
        public string IdleAnimationName => _idleClip.name;
        public string FiringAnimationName => _firingClip.name;
        public override ItemType Type => ItemType.Weapon;
        public float FireRateRpm => _fireRateRpm;
        public Projectile ProjectilePrefab => _projectilePrefab;

        
        /// <summary>
        /// Gets the default projectile events array sized to the max projectile events.
        /// </summary>
        /// <returns></returns>
        public ProjectileEventData[] GetEventArray()
        {
            return new ProjectileEventData[_maxProjectileEvents];
        }
    }
}