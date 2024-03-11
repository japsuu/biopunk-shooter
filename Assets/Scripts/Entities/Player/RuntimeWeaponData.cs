using UnityEngine;
using Weapons;

namespace Entities.Player
{
    public class RuntimeWeaponData
    {
        public readonly WeaponData Weapon;
        public ProjectileEventData[] Events;


        public RuntimeWeaponData(WeaponData weapon)
        {
            Weapon = weapon;
            Events = weapon.GetEventArray();
        }
        
        
        public void DropAllEventItems(Vector2 position)
        {
            foreach (ProjectileEventData eventData in Events)
                WorldItemSpawner.SpawnWorldItem(eventData, position);
        }
        
        
        public void OverwriteEvents(ProjectileEventData[] newEvents)
        {
            Events = newEvents;
        }
    }
}