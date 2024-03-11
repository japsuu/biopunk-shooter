using System.Collections;
using NaughtyAttributes;
using UnityEngine;

namespace Weapons.Events
{
    [CreateAssetMenu(menuName = "Events/Create WaitEvent", fileName = "WaitEvent_", order = 0)]
    public class WaitEvent : ProjectileEventData
    {
        [InfoBox("This event does nothing, but waits for a certain amount of time.")]
        [SerializeField]
        private float _duration = 1f;
        
        
        public override IEnumerator ApplyToProjectile(Projectile projectile)
        {
            yield return new WaitForSeconds(_duration);
        }
    }
}