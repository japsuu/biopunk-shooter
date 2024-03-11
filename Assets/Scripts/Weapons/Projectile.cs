using System;
using System.Collections;
using UnityEngine;
using World;

namespace Weapons
{
    /// <summary>
    /// A physics-based projectile.
    /// Most of the actual logic is delegated to a <see cref="ProjectileBehaviour"/>.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class Projectile : MonoBehaviour
    {
        [SerializeField]
        private GameObject _impactEffectPrefab;
        
        [SerializeField]
        private float _defaultImpactDamage = 20f;
        
        private Coroutine _behaviourCoroutine;
        private Rigidbody2D _rigidbody;

        public ProjectileBehaviour Behaviour { get; private set; }
        public IDamageCauser Origin { get; private set; }
        public float ImpactDamage { get; private set; }
        public float ForwardVelocity => _rigidbody.velocity.x;


        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            gameObject.AddComponent<DelayedDestroy>().Initialize(30f, null);
        }


        public void Initialize(ProjectileBehaviour behaviour, IDamageCauser origin, int behaviourOffset)
        {
            Behaviour = behaviour;
            Origin = origin;
            
            SetImpactDamage(_defaultImpactDamage);
            
            // Start applying the projectile behaviour
            _behaviourCoroutine = StartCoroutine(StartEvents(behaviourOffset));
        }
        
        
        public void SetImpactDamage(float damage)
        {
            ImpactDamage = damage;
        }
        
        
        public void SetForwardVelocity(float speed)
        {
            _rigidbody.velocity = transform.right * speed;
        }
        
        
        public void SpawnHitPrefab(Vector2 position, Quaternion rotation)
        {
            if (_impactEffectPrefab != null)
                Instantiate(_impactEffectPrefab, position, rotation);
        }
        
        
        public void DestroySelf()
        {
            if (_behaviourCoroutine != null)
                StopCoroutine(_behaviourCoroutine);
            Destroy(gameObject);
        }
        
        
        private IEnumerator StartEvents(int behaviourOffset)
        {
            yield return StartCoroutine(Behaviour.ApplyEvents(this, behaviourOffset));
            DestroySelf();
        }
        
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            Behaviour.HandleHit(this, other);
        }
    }
}