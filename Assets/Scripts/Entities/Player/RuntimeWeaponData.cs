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
            {
                if (eventData != null)
                    WorldItemSpawner.SpawnWorldItem(eventData, position + Random.insideUnitCircle);
            }
        }
        
        
        public void OverwriteEvents(ProjectileEventData[] newEvents)
        {
            Events = newEvents;
        }
        
        
        public void CopyEvents(ProjectileEventData[] newEvents)
        {
            for (int i = 0; i < newEvents.Length; i++)
            {
                if (newEvents[i] != null)
                    Events[i] = newEvents[i];
            }
        }
    }
}