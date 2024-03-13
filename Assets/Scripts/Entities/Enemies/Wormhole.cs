using System;
using DG.Tweening;
using UnityEngine;

namespace Entities.Enemies
{
    /// <summary>
    /// Spawns randomly on the map, and spawns a single enemy after a certain amount of time.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class Wormhole : MonoBehaviour
    {
        public static event Action<Wormhole> WormholeDestroyed;
        
        private SpriteRenderer _spriteRenderer;
        
        private Enemy _enemyPrefab;
        private EnemyData _enemyData;
        private float _enemySpawnTimer;
        private bool _destroying;


        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }


        public void Initialize(EnemyData enemyData, float enemySpawnDelay)
        {
            _enemyPrefab = enemyData.Prefab;
            _enemyData = enemyData;
            _enemySpawnTimer = enemySpawnDelay;
        }
        
        
        private void Update()
        {
            if (_destroying)
                return;
            
            if (_enemySpawnTimer <= 0)
            {
                SpawnEnemy();
                DestroySelf();
            }
            else
            {
                _enemySpawnTimer -= Time.deltaTime;
            }
        }
        
        
        private void SpawnEnemy()
        {
            Enemy enemy = Instantiate(_enemyPrefab, transform.position, Quaternion.identity);
            enemy.Initialize(_enemyData);
            enemy.gameObject.SetActive(true);
            enemy.gameObject.name = "Enemy";
        }


        public void DestroySelf()
        {
            _destroying = true;
            WormholeDestroyed?.Invoke(this);
            _spriteRenderer.DOFade(0, 1.0f).OnComplete(() => Destroy(gameObject, 1f));
        }
    }
}
