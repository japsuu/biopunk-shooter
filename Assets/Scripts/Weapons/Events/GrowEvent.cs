using System.Collections;
using NaughtyAttributes;
using UnityEngine;

namespace Weapons.Events
{
    [CreateAssetMenu(menuName = "Events/Create GrowEvent", fileName = "GrowEvent_", order = 0)]
    public class GrowEvent : ProjectileEventData
    {
        [InfoBox("This event grows the size of the projectile.")]
        [SerializeField]
        private float _growAmount = 1f;
        
        
        public override IEnumerator ApplyToProjectile(Projectile projectile)
        {
            projectile.transform.localScale += Vector3.one * _growAmount;
            yield return null;
        }
    }
}