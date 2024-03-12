using System.Collections;
using Items;
using UnityEngine;

namespace Weapons
{
    /// <summary>
    /// The lifetime of a projectile consists of multiple "events", each of which can modify the projectile in some way.
    /// Projectiles internally keep an array of these events, and apply them in order to themselves.
    /// Projectile is destroyed, when all the events have been applied.
    ///
    /// Initially the player's weapon will consist of "set velocity" and "delay" events, to make the projectile move and destroy it after a certain time.
    /// </summary>
    public abstract class ProjectileEventData : ItemData
    {
        public override ItemType Type => ItemType.ProjectileEvent;


        /// <summary>
        /// Applies this event to the projectile.
        /// May choose to destroy the <see cref="projectile"/>.
        /// </summary>
        /// <param name="projectile">The projectile to apply this event to.</param>
        public abstract IEnumerator ApplyToProjectile(Projectile projectile);


        /// <summary>
        /// Called when the projectile hits something.
        /// Returns true if the event handled the hit, and native hit handling should be skipped.
        /// </summary>
        public virtual bool HandleHit(Projectile projectile, RaycastHit2D hit)
        {
            return false;
        }
    }
}