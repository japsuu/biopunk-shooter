using System.Collections.Generic;
using Items;
using Saving;
using Scenes;
using Singletons;
using UnityEngine;
using Weapons;

namespace Entities.Player
{
    [RequireComponent(typeof(PlayerStats))]
    [RequireComponent(typeof(PlayerVitals))]
    [RequireComponent(typeof(PlayerMovement))]
    [RequireComponent(typeof(PlayerRotation))]
    [RequireComponent(typeof(PlayerWeaponManager))]
    public class PlayerController : SingletonBehaviour<PlayerController>
    {
        private PlayerRotation _playerRotation;

        public PlayerMovement PlayerMovement { get; private set; }
        
        public PlayerVitals Vitals { get; private set; }
        public PlayerStats Stats { get; private set; }
        public PlayerWeaponManager WeaponManager { get; private set; }


        private void Awake()
        {
            _playerRotation = GetComponent<PlayerRotation>();
            PlayerMovement = GetComponent<PlayerMovement>();
            Vitals = GetComponent<PlayerVitals>();
            Stats = GetComponent<PlayerStats>();
            WeaponManager = GetComponent<PlayerWeaponManager>();
        }


        private void Update()
        {
            PlayerMovement.MovementSpeed = Vitals.MovementSpeed;
        }
        
        
        public void OnOrganSlotsChanged(List<OrganData> slotsContents)
        {
            Vitals.SetOrgans(slotsContents);
        }


        public void OnWeaponSlotChanged(WeaponData newWeapon, bool isRight)
        {
            WeaponManager.SetWeapon(newWeapon, isRight);
        }


        public void OnPlayerDied()
        {
            PlayerMovement.enabled = false;
            _playerRotation.enabled = false;
            WeaponManager.enabled = false;
            
            HighScores.SaveHighScore(Mathf.RoundToInt(Stats.TotalExperience));
            
            SceneChanger.GoToMainMenuScene();
        }
    }
}