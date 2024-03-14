using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using World;

namespace Weapons
{
    /// <summary>
    /// Raycast projectile that flies through the air.
    /// Projectiles are a hybrid between simulated rigidbody bullets, and full-on raycasts that travel instantly.
    /// These projectiles do not travel instantly and have a calculated mass.
    /// 
    /// Projectiles raycast forward a certain amount each frame, and travel there if no hit is detected.
    /// If a hit is detected, the projectile teleports to the hit position and triggers a hit.
    /// Most of the actual logic is delegated to a <see cref="ProjectileBehaviour"/>.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class Projectile : MonoBehaviour
    {
        private const int MAX_RAYCAST_RESULTS = 64;

        [SerializeField]
        private bool _debugDraw;

        [SerializeField]
        private LayerMask _projectileHitLayers;

        [SerializeField]
        private float _defaultImpactDamage = 20f;

        private Coroutine _behaviourCoroutine;
        private bool _awaitingDestruction;
        private RaycastHit2D[] _raycastResults;

        public ProjectileBehaviour Behaviour { get; private set; }
        public IDamageCauser Origin { get; private set; }
        public float ImpactDamage { get; private set; }
        [ShowNativeProperty]
        public float ForwardVelocity { get; private set; }


        private void Awake()
        {
            if (GetComponent<DelayedDestroy>() == null)
                gameObject.AddComponent<DelayedDestroy>().Initialize(30f, null);
        }


        public void Initialize(ProjectileBehaviour behaviour, IDamageCauser origin, int behaviourOffset)
        {
            Behaviour = behaviour;
            Origin = origin;
            _raycastResults = new RaycastHit2D[MAX_RAYCAST_RESULTS];

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
            ForwardVelocity = speed;
        }


        public void DestroySelf()
        {
            if (_behaviourCoroutine != null)
                StopCoroutine(_behaviourCoroutine);
            Destroy(gameObject);
            _awaitingDestruction = true;
        }


        private void FixedUpdate()
        {
            // Safe clause. Might trigger since Destroy(gameObject) does not execute instantly.
            if (_awaitingDestruction)
                return;

            // Calculate how much to move the GameObject along the forward axis (based on muzzleVelocity).
            float distanceToMove = ForwardVelocity * Time.fixedDeltaTime;
            Vector3 debugDrawCachedPos = transform.position;

            // Raycast that distance in front of the bullet before moving: If no collision - move to position, if collision - move to collided point and handle hit.
            int size = Physics2D.RaycastNonAlloc(transform.position, transform.right, _raycastResults, distanceToMove, _projectileHitLayers);

            if (size == MAX_RAYCAST_RESULTS)
                Debug.LogWarning("MAX_RAYCAST_RESULTS hit, the value probably needs to be raised for hit detection to work correctly!");

            for (int i = 0; i < size; i++)
            {
                RaycastHit2D hit = _raycastResults[i];

                if (hit.collider.isTrigger)
                    continue;

                transform.position = hit.point;
                
                if (Behaviour.HandleHit(this, hit))
                    return;

                // Damage the hit object if it's damageable
                if (hit.transform.gameObject.TryGetComponent(out IDamageable damageable))
                {
                    float damage = ImpactDamage + ForwardVelocity / 2;
                    damageable.Damage(damage, Origin);
                }

                // Notify the projectile renderer that a hit has occurred.
                RaycastProjectileRenderer projectileRenderer = GetComponentInChildren<RaycastProjectileRenderer>();
                if (projectileRenderer != null)
                    projectileRenderer.OnProjectileHit(hit, transform.right);
                
                DestroySelf();
                
                return;
            }

            transform.position += transform.right * distanceToMove;

#if UNITY_EDITOR
            if (_debugDraw)
            {
                Debug.DrawLine(debugDrawCachedPos, transform.position, Color.red, 2f);
            }
#endif
        }


        private IEnumerator StartEvents(int behaviourOffset)
        {
            yield return new WaitForEndOfFrame();
            yield return StartCoroutine(Behaviour.ApplyEvents(this, behaviourOffset));
            DestroySelf();
        }
    }
}