using System.Collections.Generic;
using Items;
using UnityEngine;
using Weapons;

namespace Entities.Player
{
    public class PlayerVitals : MonoBehaviour, IDamageable
    {
        [Header("Config")]
        
        [Tooltip("Changes how much health the player can have.")]
        [SerializeField]
        [Range(1f, 200f)]
        private float _baseMaxHealth = 100f;
        
        [Tooltip("Changes how fast the player regenerates health.")]
        [SerializeField]
        [Range(1f, 40f)]
        private float _baseRegenPerSec = 5f;
        
        [Tooltip("Changes how fast the player moves.")]
        [SerializeField]
        [Range(1f, 20f)]
        private float _baseMovementSpeed = 5f;

        [Tooltip("Changes how much damage the player takes.")]
        [SerializeField]
        [Range(0.1f, 2f)]
        private float _baseFortitudeFactor = 1f;
        
        // Runtime stats
        private readonly List<float> _maxHealthModifiers = new();        // Percentage-modifiers to the base value.
        private readonly List<float> _healthRegenModifiers = new();
        private readonly List<float> _movementSpeedModifiers = new();
        private readonly List<float> _fortitudeFactorModifiers = new();

        public float MaxHealth => _baseMaxHealth * GetTotalModifier(_maxHealthModifiers, 0.01f);
        public float Regen => _baseRegenPerSec * GetTotalModifier(_healthRegenModifiers, 0);
        public float MovementSpeed => _baseMovementSpeed * GetTotalModifier(_movementSpeedModifiers, 0.1f);
        public float FortitudeFactor => _baseFortitudeFactor * GetTotalModifier(_fortitudeFactorModifiers, 0);
        public float CurrentHealth { get; private set; }
        public bool IsAlive => CurrentHealth > 0f;
        
        public float KillRewardXp => 20f;


        private void Awake()
        {
            CurrentHealth = MaxHealth;
        }


        private void Update()
        {
            Heal(Time.deltaTime * Regen);
        }


        public void TakeDamage(float damage)
        {
            CurrentHealth -= damage * FortitudeFactor;
            if (!IsAlive)
            {
                Die();
            }
        }
        
        
        public void Heal(float amount)
        {
            CurrentHealth = Mathf.Min(CurrentHealth + amount, MaxHealth);
        }


        public void SetOrgans(List<OrganData> newBodyParts)
        {
            _maxHealthModifiers.Clear();
            _healthRegenModifiers.Clear();
            _movementSpeedModifiers.Clear();
            _fortitudeFactorModifiers.Clear();
            
            foreach (OrganData organ in newBodyParts)
            {
                _maxHealthModifiers.Add(organ.MaxHealth);
                _healthRegenModifiers.Add(organ.HealthRegenPerSec);
                _movementSpeedModifiers.Add(organ.MovementSpeed);
                _fortitudeFactorModifiers.Add(organ.Fortitude);
            }
        }
        
        
        private float GetTotalModifier(List<float> modifiers, float minimum)
        {
            float total = 1f;
            foreach (float modifier in modifiers)
            {
                total += modifier;
            }
            return Mathf.Max(total, minimum);
        }
        
        
        private void Die()
        {
            Debug.LogWarning("Player died!");
            PlayerController.Instance.OnPlayerDied();
        }


        public void Damage(float amount, IDamageCauser causer)
        {
            TakeDamage(amount);
        }
    }
}