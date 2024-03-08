using Items;
using UnityEngine;
using World;

namespace Weapons
{
    /// <summary>
    /// A physics-based projectile.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class Projectile : MonoBehaviour
    {
        private Rigidbody2D _rb;
        private DynamicWeaponData _data;
        private IDamageCauser _shooter;


        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            gameObject.AddComponent<DelayedDestroy>().Initialize(10f, null);
        }


        public void Initialize(DynamicWeaponData data, IDamageCauser shooter)
        {
            _data = data;
            _shooter = shooter;
            _rb.velocity = transform.right * _data.ProjectileSpeed;
        }
        
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent(out IDamageable damageable))
                return;
            
            damageable.Damage(_data.ProjectileDamage, _shooter);
            Destroy(gameObject);
        }
        
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.collider.TryGetComponent(out IDamageable damageable))
                damageable.Damage(_data.ProjectileDamage, _shooter);
            
            Destroy(gameObject);
        }
    }
}