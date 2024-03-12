using System.Collections.Generic;
using Items;
using Singletons;
using UnityEngine;
using Weapons;
using Random = UnityEngine.Random;

namespace Entities.Player
{
    [RequireComponent(typeof(PlayerStats))]
    [RequireComponent(typeof(PlayerVitals))]
    [RequireComponent(typeof(PlayerMovement))]
    [RequireComponent(typeof(PlayerRotation))]
    [RequireComponent(typeof(PlayerWeaponManager))]
    public class PlayerController : SingletonBehaviour<PlayerController>
    {
        [SerializeField]
        private List<ItemData> _testItemData;

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
            DoDebugStuff();
            
            PlayerMovement.MovementSpeed = Vitals.MovementSpeed;
        }


        private void DoDebugStuff() //TODO: Remove before release.
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                ItemData item = _testItemData[Random.Range(0, _testItemData.Count)];
                Vector2 position = (Vector2)transform.position + Random.insideUnitCircle * 3;
                WorldItemSpawner.SpawnWorldItem(item, position);
            }
        }
        
        
        public void OnOrganSlotsChanged(List<OrganData> slotsContents)
        {
            Vitals.SetOrgans(slotsContents);
        }


        public void OnWeaponSlotChanged(WeaponData newWeapon, bool isRight)
        {
            WeaponManager.SetWeapon(newWeapon, isRight);
        }
    }
}