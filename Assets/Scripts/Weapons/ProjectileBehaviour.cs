using System.Collections;
using UnityEngine;

namespace Weapons
{
    public class ProjectileBehaviour
    {
        private readonly ProjectileEventData[] _events;
        private ProjectileEventData _currentEvent;
        
        public int CurrentEventIndex { get; private set; }
        public int EventCount { get; private set; }


        public ProjectileBehaviour(ProjectileEventData[] events)
        {
            _events = events;
            foreach (ProjectileEventData data in events)
            {
                if (data != null)
                    EventCount++;
            }
        }
        
        
        public IEnumerator ApplyEvents(Projectile projectile, int startOffset)
        {
            for (int i = startOffset; i < _events.Length; i++)
            {
                CurrentEventIndex = i;
                _currentEvent = _events[i];
                if (_currentEvent == null)
                    continue;
                yield return _currentEvent.ApplyToProjectile(projectile);
            }
        }
        
        
        public void HandleHit(Projectile projectile, Collider2D other)
        {
            if (_currentEvent != null)
                _currentEvent.HandleHit(projectile, other);
            else
            {
                if (other.TryGetComponent(out IDamageable damageable))
                    damageable.Damage(projectile.ImpactDamage, projectile.Origin);
            
                // Spawn a hit prefab opposite to the velocity of the projectile. Remember this is a top-down 2D project.
                Vector3 position = projectile.transform.position;
                Quaternion rotation = Quaternion.LookRotation(-projectile.transform.right, Vector3.up);
                projectile.SpawnHitPrefab(position, rotation);
                
                projectile.DestroySelf();
            }
        }
    }
}