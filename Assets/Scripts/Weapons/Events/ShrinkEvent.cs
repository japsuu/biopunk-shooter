using System.Collections;
using NaughtyAttributes;
using UnityEngine;

namespace Weapons.Events
{
    [CreateAssetMenu(menuName = "Events/Create ShrinkEvent", fileName = "ShrinkEvent_", order = 0)]
    public class ShrinkEvent : ProjectileEventData
    {
        [InfoBox("This event shrinks the size of the projectile.")]
        [SerializeField]
        private float _shrinkAmount = 1f;
        
        
        public override IEnumerator ApplyToProjectile(Projectile projectile)
        {
            projectile.transform.localScale -= Vector3.one * _shrinkAmount;
            
            if (projectile.transform.localScale.x <= 0.1f)
            {
                projectile.transform.localScale = Vector3.one * 0.1f;
            }
            
            yield return null;
        }
    }
}