using UnityEngine;

namespace Items
{
    [CreateAssetMenu(menuName = "Items/Create New Organ", fileName = "Organ_", order = 0)]
    public class OrganData : ItemData
    {
        [Header("Factors")]
        
        [SerializeField]
        [Range(-2f, 2f)]
        private float _maxHealth;
        
        [SerializeField]
        [Range(-2f, 2f)]
        private float _healthRegenPerSec;

        [SerializeField]
        [Range(-2f, 2f)]
        private float _movementSpeed;
        
        [SerializeField]
        [Range(-2f, 2f)]
        private float _fortitude;

        public override ItemType Type => ItemType.Organ;
        
        public float MaxHealth => _maxHealth;
        public float HealthRegenPerSec => _healthRegenPerSec;
        public float MovementSpeed => _movementSpeed;
        public float Fortitude => _fortitude;
    }
}