using System.Collections;
using UnityEngine;

namespace Weapons
{
    public class ProjectileBehaviour
    {
        private readonly ProjectileEventData[] _events;
        private ProjectileEventData _currentEvent;
        
        public int CurrentEventIndex { get; private set; }
        public int EventCount { get; private set; }


        public ProjectileBehaviour(ProjectileEventData[] events)
        {
            _events = events;
            foreach (ProjectileEventData data in events)
            {
                if (data != null)
                    EventCount++;
            }
        }
        
        
        public ProjectileBehaviour(ProjectileBehaviour other)
        {
            _events = new ProjectileEventData[other._events.Length];
            for (int i = 0; i < _events.Length; i++)
            {
                if (other._events[i] != null)
                    _events[i] = other._events[i];
            }
            EventCount = other.EventCount;
        }
        
        
        public IEnumerator ApplyEvents(Projectile projectile, int startOffset)
        {
            for (int i = startOffset; i < _events.Length; i++)
            {
                CurrentEventIndex = i;
                _currentEvent = _events[i];
                if (_currentEvent == null)
                    continue;
                yield return _currentEvent.ApplyToProjectile(projectile);
            }
        }
        
        
        public bool HandleHit(Projectile projectile, RaycastHit2D hit)
        {
            if (_currentEvent == null)
                return false;
            
            return _currentEvent.HandleHit(projectile, hit);
        }
    }
}