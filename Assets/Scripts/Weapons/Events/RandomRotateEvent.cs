using System.Collections;
using NaughtyAttributes;
using UnityEngine;

namespace Weapons.Events
{
    [CreateAssetMenu(menuName = "Events/Create RandomRotateEvent", fileName = "RandomRotateEvent_", order = 0)]
    public class RandomRotateEvent : ProjectileEventData
    {
        [InfoBox("This event rotates the projectile by a random angle.")]
        [SerializeField, MinMaxSlider(-180f, 180f)]
        private Vector2 _randomAngleRange = new(-15f, 15f);
        
        
        public override IEnumerator ApplyToProjectile(Projectile projectile)
        {
            projectile.transform.rotation = Quaternion.Euler(0, 0, projectile.transform.eulerAngles.z + Random.Range(_randomAngleRange.x, _randomAngleRange.y));
            yield return null;
        }
    }
}