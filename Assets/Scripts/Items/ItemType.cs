namespace Items
{
    public enum ItemType
    {
        /// <summary>
        /// Organs apply bonuses to the player.
        /// </summary>
        Organ,
        
        /// <summary>
        /// Events change the behaviour of projectiles.
        /// </summary>
        ProjectileEvent,
        
        /// <summary>
        /// Weapons create projectiles and contain the events that modify them.
        /// </summary>
        Weapon
    }
}