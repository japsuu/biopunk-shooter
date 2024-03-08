using Data;
using NaughtyAttributes;
using Thirdparty.WeightedRandomSelector.Interfaces;
using UnityEngine;

namespace Entities.Enemies
{
    public class WormholeManager : MonoBehaviour
    {
        [SerializeField]
        private Wormhole _wormholePrefab;

        [SerializeField]
        private EnemyDataDatabase _enemyDatabase;

        [SerializeField]
        private Enemy _enemyPrefab;

        [SerializeField]
        [Tooltip("How long it takes for a wormhole to spawn an enemy.")]
        [MinMaxSlider(1.0f, 10.0f)]
        private Vector2 _enemySpawnDelayRange = new(3.0f, 6.0f);

        [SerializeField]
        [Tooltip("How often a wormhole is spawned.")]
        [MinMaxSlider(1.0f, 10.0f)]
        private Vector2 _wormholeSpawnIntervalRange = new(3.0f, 6.0f);

        [SerializeField]
        [Tooltip("How far from this object a wormhole can spawn.")]
        private float _wormholeSpawnRange = 10.0f;
        
        private IRandomSelector<EnemyData> _enemyDataSelector;
        private float _nextWormholeSpawnTimer;
        
        
        private void Start()
        {
            _enemyDataSelector = _enemyDatabase.CreateRandomSelector();
        }
        
        
        private void Update()
        {
            if (_nextWormholeSpawnTimer <= 0)
            {
                SpawnWormhole();
                _nextWormholeSpawnTimer = Random.Range(_wormholeSpawnIntervalRange.x, _wormholeSpawnIntervalRange.y);
            }
            else
            {
                _nextWormholeSpawnTimer -= Time.deltaTime;
            }
        }
        
        
        private void SpawnWormhole()
        {
            Wormhole wormhole = Instantiate(_wormholePrefab, transform.position + (Vector3)Random.insideUnitCircle * _wormholeSpawnRange, Quaternion.identity);
            wormhole.transform.parent = transform;
            wormhole.gameObject.SetActive(true);
            wormhole.gameObject.name = "Wormhole";
            wormhole.Initialize(_enemyPrefab, _enemyDataSelector.SelectRandomItem(), Random.Range(_enemySpawnDelayRange.x, _enemySpawnDelayRange.y));
        }


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _wormholeSpawnRange);
        }
    }
}