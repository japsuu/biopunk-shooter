﻿using System;
using Entities.Player;
using JSAM;
using NaughtyAttributes;
using UnityEngine;

namespace Weapons
{
    /// <summary>
    /// Manages a single weapon.
    /// If the heat generated by the weapon exceeds a certain threshold, the weapon will overheat and be unusable for a short time.
    /// </summary>
    public class WeaponObject : MonoBehaviour
    {
        // private const float HEAT_THRESHOLD = 100f;
        
        [Header("References")]
        
        [SerializeField]
        private Transform _projectileSpawnPoint;

        [SerializeField]
        private GameObject _muzzleFlashPrefab;
        
        /*[ReadOnly, SerializeField]
        private float _currentHeat;
        [ReadOnly, SerializeField]
        private float _overheatCooldownLeft;*/
        [ReadOnly, SerializeField]
        private float _fireDelayLeft;
        
        public ProjectileBehaviour DynamicBehaviour { get; private set; } = new(Array.Empty<ProjectileEventData>());
        private IDamageCauser _owner;
        private RuntimeWeaponData _runtimeData;
        private float _overrideDamage = -1;
        
        
        public void Initialize(RuntimeWeaponData data, IDamageCauser owner)
        {
            // Transfer the existing events from _runtimeData to the new data.
            // If there are more events in the old data than what the new data can hold, the remaining events are dropped.
            TransferEvents(data);
            _runtimeData = data;
            _owner = owner;
            DynamicBehaviour = new ProjectileBehaviour(_runtimeData.Events);
            SetEvents(_runtimeData.Events);
        }


        private void TransferEvents(RuntimeWeaponData data)
        {
            if (_runtimeData == null)
                return;
            
            // Transfer the events from the old data to the new data.
            int index = 0;
            while (index < _runtimeData.Events.Length)
            {
                if (index >= data.Events.Length)
                    break;
                
                data.Events[index] = _runtimeData.Events[index];
                _runtimeData.Events[index] = null;
                index++;
            }
            
            // Drop the remaining extra events.
            _runtimeData.DropAllEventItems(_projectileSpawnPoint.position);
        }


        public void SetEvents(ProjectileEventData[] projectileEventData)
        {
            _runtimeData.OverwriteEvents(projectileEventData);
            DynamicBehaviour = new ProjectileBehaviour(_runtimeData.Events);
        }


        private void Update()
        {
            /*// Handle overheating.
            if (_overheatCooldownLeft > 0f)
            {
                _overheatCooldownLeft -= Time.deltaTime;
                _currentHeat = 0f;
            }
            else
            {
                _currentHeat -= _heatRemovedPerSecond * Time.deltaTime;
                if (_currentHeat < 0f)
                {
                    _currentHeat = 0f;
                }
            }*/
            
            // Handle firing delay.
            if (_fireDelayLeft > 0f)
            {
                _fireDelayLeft -= Time.deltaTime;
            }
        }


        public void SetOverrideDamage(float damage)
        {
            _overrideDamage = damage;
        }
        
        
        public void TryFire()
        {
            if (_fireDelayLeft > 0f)
                return;
            
            if (DynamicBehaviour.EventCount <= 0)
                return;
            
            /*if (_overheatCooldownLeft > 0f)
                return;
            
            if (_currentHeat >= HEAT_THRESHOLD)
            {
                _overheatCooldownLeft = _overheatCooldownSeconds;
                return;
            }
            
            _currentHeat += _generatedHeatPerShot;*/
            Fire();
            _fireDelayLeft = 60f / _runtimeData.Weapon.FireRateRpm;
        }


        private void Fire()
        {
            Projectile p = Instantiate(_runtimeData.Weapon.ProjectilePrefab, _projectileSpawnPoint.position, _projectileSpawnPoint.rotation);
            p.Initialize(DynamicBehaviour, _owner, 0);
            
            if (_overrideDamage > 0)
                p.SetImpactDamage(_overrideDamage);
            
            // Generate a muzzle flash.
            if(_muzzleFlashPrefab != null)
                Instantiate(_muzzleFlashPrefab, _projectileSpawnPoint.position, _projectileSpawnPoint.rotation);
            
            AudioManager.PlaySound(_runtimeData.Weapon.FireSound);
        }
    }
}