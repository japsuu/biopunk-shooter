using System.Collections;
using NaughtyAttributes;
using UnityEngine;

namespace Weapons.Events
{
    [CreateAssetMenu(menuName = "Events/Create RandomizedEvent", fileName = "RandomizedEvent_", order = 0)]
    public class RandomizedEvent : ProjectileEventData
    {
        [InfoBox("This event delegates the ApplyToProjectile method to a random event.")]
        [SerializeField]
        private ProjectileEventData[] _events;
        
        
        public override IEnumerator ApplyToProjectile(Projectile projectile)
        {
            yield return _events[Random.Range(0, _events.Length)].ApplyToProjectile(projectile);
        }
    }
}