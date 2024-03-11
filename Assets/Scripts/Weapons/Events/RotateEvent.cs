using System.Collections;
using NaughtyAttributes;
using UnityEngine;

namespace Weapons.Events
{
    [CreateAssetMenu(menuName = "Events/Create RotateEvent", fileName = "RotateEvent_", order = 0)]
    public class RotateEvent : ProjectileEventData
    {
        [InfoBox("This event rotates the projectile by an angle.")]
        [SerializeField, Range(-180f, 180f)]
        private float _angle = 90f;
        
        
        public override IEnumerator ApplyToProjectile(Projectile projectile)
        {
            projectile.transform.rotation = Quaternion.Euler(0, 0, projectile.transform.eulerAngles.z + _angle);
            yield return null;
        }
    }
}