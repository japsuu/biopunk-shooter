using System.Collections;
using NaughtyAttributes;
using UnityEngine;

namespace Weapons.Events
{
    [CreateAssetMenu(menuName = "Events/Create AddSpeedEvent", fileName = "SpeedEvent_", order = 0)]
    public class AddSpeedEvent : ProjectileEventData
    {
        [InfoBox("This event adds to the velocity of the projectile.")]
        [SerializeField]
        private float _forwardVelocity = 10f;
        
        
        public override IEnumerator ApplyToProjectile(Projectile projectile)
        {
            projectile.SetForwardVelocity(projectile.ForwardVelocity + _forwardVelocity);
            yield return null;
        }
    }
}