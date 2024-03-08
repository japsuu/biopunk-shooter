using Items;
using JetBrains.Annotations;

namespace Weapons
{
    public class DynamicWeaponData
    {
        private const float BASE_PROJ_SPEED = 20f;
        private const float BASE_PROJ_DAMAGE = 20f;
        
        [CanBeNull]
        private readonly WeaponPartData[] _parts;
        private readonly float _projectileSpeed;
        private readonly float _projectileDamage;


        public DynamicWeaponData([CanBeNull] WeaponPartData[] parts)   //TODO: calculate speed modifier from parts.
        {
            _parts = parts;
            _projectileSpeed = BASE_PROJ_SPEED * 1f;
            _projectileDamage = BASE_PROJ_DAMAGE * 1f;
        }


        public float ProjectileSpeed => _projectileSpeed;
        public float ProjectileDamage => _projectileDamage;
    }
}