using Data;
using UnityEngine;
using Weapons;

namespace Entities.Enemies
{
    [CreateAssetMenu(fileName = "Enemy_", menuName = "Enemy Data", order = 0)]
    public class EnemyData : ScriptableObject
    {
        public float Health;
        public float Damage;
        public float KillRewardXp;
        
        [Header("Weapon config")]
        
        [SerializeField]
        private WeaponData _equippedWeapon;
        
        [SerializeField]
        private ProjectileEventData[] _defaultEvents;
        
        [SerializeField]
        private ProjectileEventData[] _randomizedEvents;
        
        [SerializeField]
        private int _selectedRandomizedEventCount = 2;
        
        [SerializeField]
        private float _selectRandomizedEventChance = 0.5f;
        
        [Header("Drops")]
        
        [SerializeField]
        private DropTable _dropTable;
        
        [Header("Prefab")]
        
        [SerializeField]
        private Enemy _prefab;
        
        [Header("UI")]
        
        [SerializeField]
        private Sprite _uiImage;
        
        public WeaponData EquippedWeapon => _equippedWeapon;
        public DropTable DropTable => _dropTable;
        public Enemy Prefab => _prefab;
        public Sprite UiImage => _uiImage;


        public ProjectileEventData[] GetEvents()
        {
            if (_randomizedEvents.Length == 0)
            {
                return _defaultEvents;
            }

            int defaultCount = _defaultEvents.Length;
            ProjectileEventData[] events = new ProjectileEventData[defaultCount + _selectedRandomizedEventCount];
            _defaultEvents.CopyTo(events, 0);
            
            if (Random.value < _selectRandomizedEventChance)
            {
                for (int i = 0; i < _selectedRandomizedEventCount; i++)
                {
                    int randomIndex = Random.Range(0, _randomizedEvents.Length);
                    events[defaultCount + i] = _randomizedEvents[randomIndex];
                }
            }

            return events;
        }
    }
}