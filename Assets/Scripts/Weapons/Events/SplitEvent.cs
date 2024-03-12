using System.Collections;
using NaughtyAttributes;
using UnityEngine;

namespace Weapons.Events
{
    [CreateAssetMenu(menuName = "Events/Create SplitEvent", fileName = "SplitEvent_", order = 0)]
    public class SplitEvent : ProjectileEventData
    {
        [InfoBox("This event destroys the original projectile, and splits the projectile into multiple projectiles.")]
        [SerializeField]
        private int _splitCount = 2;
        
        [Tooltip("The angle offset for the first split projectile.")]
        [SerializeField]
        private float _splitAngleOffset = -15f;
        
        [Tooltip("The angle increment for the split projectiles.")]
        [SerializeField]
        private float _splitAngleIncrement = 30f;
        
        
        public override IEnumerator ApplyToProjectile(Projectile projectile)
        {
            for (int i = 0; i < _splitCount; i++)
            {
                Projectile splitProjectile = Instantiate(projectile, projectile.transform.position, Quaternion.identity);
                
                // Initialize the split projectile with the next event index.
                splitProjectile.Initialize(new ProjectileBehaviour(projectile.Behaviour), projectile.Origin, projectile.Behaviour.CurrentEventIndex + 1);
                
                // Rotate the split projectile by the split angle offset and the split angle increment.
                splitProjectile.transform.rotation = Quaternion.Euler(0, 0, projectile.transform.eulerAngles.z + _splitAngleOffset + i * _splitAngleIncrement);
                
                // Copy the velocity from the original projectile.
                splitProjectile.SetForwardVelocity(projectile.ForwardVelocity);
            }
            projectile.DestroySelf();
            
            yield return null;
        }
    }
}