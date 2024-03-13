using System;
using Cameras;
using Entities.Player;
using Items;
using UI;
using UnityEngine;
using Weapons;
using Random = UnityEngine.Random;

namespace Entities.Enemies
{
    [RequireComponent(typeof(EnemyMovement))]
    [RequireComponent(typeof(EnemyRotation))]
    public class Enemy : MonoBehaviour, IDamageable, IDamageCauser
    {
        public static event Action<Enemy> EnemyCreated;
        public static event Action<Enemy> EnemyDestroyed;
        
        [SerializeField]
        private WeaponData _defaultWeapon;  //TODO: Randomize weapon.
        
        [SerializeField]
        private ProjectileEventData[] _defaultEvents;
        
        [SerializeField]
        private SpriteRenderer _renderer;
        
        [SerializeField]
        private WeaponObject _weapon;

        private EnemyMovement _movement;
        private EnemyRotation _rotation;
        private float _health;
        private GameObject _uiImageHandle;

        public EnemyData Data { get; private set; }

        public WeaponObject Weapon => _weapon;
        public float KillRewardXp => Data.KillRewardXp;


        private void Awake()
        {
            _rotation = GetComponent<EnemyRotation>();
            _movement = GetComponent<EnemyMovement>();
        }


        public void Initialize(EnemyData data)
        {
            Data = data;
            _movement.MovementSpeed = Data.MovementSpeed;
            _health = Data.Health;
            RuntimeWeaponData weaponData = new RuntimeWeaponData(_defaultWeapon);
            
            if (_defaultEvents != null)
                weaponData.CopyEvents(_defaultEvents);
            
            _weapon.Initialize(weaponData, this);
            
            _rotation.Initialize(this);
            //TODO: Set weapon parts.
            
            _uiImageHandle = WaveUIManager.Instance.SpawnEnemyImage(this);
            
            EnemyCreated?.Invoke(this);
        }


        public void Damage(float amount, IDamageCauser notifyOfKill)
        {
            _health -= amount;
            if (_health <= 0)
                Die(notifyOfKill);
        }


        public void NotifyOfKill(IDamageable killed)
        {
            //TODO: Upgrade weapon.
        }


        private void Update()
        {
            //if (Vector2.Distance(PlayerController.Instance.transform.position, transform.position) < 18)
            if (IsVisible())
                _weapon.TryFire();
        }


        private void Die(IDamageCauser notifyOfKill)
        {
            SpawnLoot();
            notifyOfKill?.NotifyOfKill(this);
            DestroySelf();
        }


        public void DestroySelf()
        {
            WaveUIManager.Instance.DestroyEnemyImage(_uiImageHandle);
            EnemyDestroyed?.Invoke(this);
            Destroy(gameObject);
        }


        private void SpawnLoot()
        {
            ItemData item = Data.DropTable.CreateRandomSelector().SelectRandomItem(); 
            WorldItemSpawner.SpawnWorldItem(item, (Vector2)transform.position + Random.insideUnitCircle * 2);
        }
        
        
        private bool IsVisible()
        {
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(CameraController.Instance.Camera);

            return GeometryUtility.TestPlanesAABB(planes , _renderer.bounds);
        }
    }
}