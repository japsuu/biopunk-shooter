using Camera;
using Entities.Player;
using Items;
using UnityEngine;
using Weapons;
using World;
using Random = UnityEngine.Random;

namespace Entities.Enemies
{
    [RequireComponent(typeof(EnemyMovement))]
    [RequireComponent(typeof(EnemyRotation))]
    public class Enemy : MonoBehaviour, IDamageable, IDamageCauser
    {
        [SerializeField]
        private WeaponData _defaultWeapon;  //TODO: Randomize weapon.
        
        [SerializeField]
        private SpriteRenderer _renderer;
        
        [SerializeField]
        private WeaponObject _weapon;
        
        [SerializeField]
        private Projectile _projectilePrefab; 

        private EnemyMovement _movement;
        private EnemyRotation _rotation;
        private EnemyData _data;
        private float _health;

        public WeaponObject Weapon => _weapon;
        public float KillRewardXp => _data.KillRewardXp;


        private void Awake()
        {
            _rotation = GetComponent<EnemyRotation>();
            _movement = GetComponent<EnemyMovement>();
        }


        public void Initialize(EnemyData data)
        {
            _data = data;
            _health = _data.Health;
            _weapon.Initialize(new RuntimeWeaponData(_defaultWeapon), this);
            _rotation.Initialize(this);
            //TODO: Set weapon parts.
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
            Destroy(gameObject);
        }


        private void SpawnLoot()
        {
            ItemData item = _data.DropTable.CreateRandomSelector().SelectRandomItem(); 
            WorldItemSpawner.SpawnWorldItem(item, (Vector2)transform.position + Random.insideUnitCircle * 2);
        }
        
        
        private bool IsVisible()
        {
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(CameraController.Instance.Camera);

            return GeometryUtility.TestPlanesAABB(planes , _renderer.bounds);
        }
    }
}