using System;
using System.Collections.Generic;
using Items;
using Singletons;
using UI;
using UnityEngine;
using World;
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

        private PlayerRotation _playerRotation; //TODO: Rotation speed modifier
        private PlayerWeaponManager _weaponManager;

        public PlayerMovement PlayerMovement { get; private set; }
        
        public PlayerVitals Vitals { get; private set; }
        public PlayerStats Stats { get; private set; }


        private void Awake()
        {
            _playerRotation = GetComponent<PlayerRotation>();
            PlayerMovement = GetComponent<PlayerMovement>();
            Vitals = GetComponent<PlayerVitals>();
            Stats = GetComponent<PlayerStats>();
            _weaponManager = GetComponent<PlayerWeaponManager>();
            SlotManager.OnEquippedItemsChanged += OnEquippedItemsChanged;
        }


        private void Update()
        {
            DoDebugStuff();
            
            PlayerMovement.MovementSpeed = Vitals.MovementSpeed;
        }


        private void DoDebugStuff()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                ItemData item = _testItemData[Random.Range(0, _testItemData.Count)];
                WorldItem i = Instantiate(item.WorldItemPrefab, transform.position + new Vector3(2f, 2f, 0), Quaternion.identity);
                i.Initialize(item);
            }
        }


        private void OnEquippedItemsChanged((SlotType slot, List<ItemData> newItems) obj)
        {
            switch (obj.slot)
            {
                case SlotType.LeftHand:
                    _weaponManager.SetLeftHandWeaponParts(obj.newItems);
                    break;
                case SlotType.RightHand:
                    _weaponManager.SetRightHandWeaponParts(obj.newItems);
                    break;
                case SlotType.Body:
                    Vitals.SetBodyParts(obj.newItems);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}