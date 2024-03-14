using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using Entities.Player;
using NaughtyAttributes;
using Singletons;
using Thirdparty.WeightedRandomSelector.Interfaces;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Entities.Enemies
{
    public class EnemyManager : SingletonBehaviour<EnemyManager>
    {
        [SerializeField]
        private bool _spawnEnemies = true;
        
        [SerializeField]
        private Wormhole _wormholePrefab;

        [SerializeField]
        private EnemyDataDatabase _enemyDatabase;

        [SerializeField]
        [Tooltip("How long it takes for a wormhole to spawn an enemy.")]
        [MinMaxSlider(1.0f, 10.0f)]
        private Vector2 _enemySpawnDelayRange = new(3.0f, 6.0f);

        [SerializeField]
        [Tooltip("How often a wormhole is spawned.")]
        [MinMaxSlider(1.0f, 10.0f)]
        private Vector2 _wormholeSpawnIntervalRange = new(3.0f, 6.0f);

        [SerializeField]
        [Tooltip("How far from this object a wormhole group origin can spawn.")]
        private float _wormholeGroupCenterMaxRange = 30.0f;

        [SerializeField]
        [Tooltip("When a wormhole group is spawned, how far the wormholes may spawn from the group origin.")]
        private float _wormholeGroupMaxSize = 10.0f;
        
        [SerializeField]
        [Tooltip("How long is the cooldown between waves, in seconds.")]
        private int _waveCooldown = 10;
        
        [SerializeField]
        [Tooltip("How long a wave can last for, in seconds.")]
        private float _maxWaveLength = 30f;
        
        [SerializeField]
        [Tooltip("How many enemies are spawned on the first wave.")]
        private int _initialWaveEnemyCount = 3;
        
        private IRandomSelector<EnemyData> _enemyDataSelector;
        private readonly List<Wormhole> _aliveWormholes = new();
        private readonly List<Enemy> _aliveEnemies = new();
        
        public int TimeUntilNextWave { get; private set; }
        public int CurrentWave { get; private set; }
        
        private bool HasAliveEnemies => _aliveEnemies.Count > 0 || _aliveWormholes.Count > 0;
        private bool IsPlayerAlive => PlayerController.Instance.Vitals.IsAlive;


        private void Awake()
        {
            Wormhole.WormholeDestroyed += OnWormholeDestroyed;
            Enemy.EnemyCreated += OnEnemyCreated;
            Enemy.EnemyDestroyed += OnEnemyDestroyed;
        }


        private void OnDisable()
        {
            Wormhole.WormholeDestroyed -= OnWormholeDestroyed;
            Enemy.EnemyCreated -= OnEnemyCreated;
            Enemy.EnemyDestroyed -= OnEnemyDestroyed;
        }


        private void OnWormholeDestroyed(Wormhole obj)
        {
            _aliveWormholes.Remove(obj);
        }


        private void OnEnemyCreated(Enemy obj)
        {
            _aliveEnemies.Add(obj);
        }


        private void OnEnemyDestroyed(Enemy obj)
        {
            _aliveEnemies.Remove(obj);
        }


        private void Start()
        {
            if (!_spawnEnemies)
                return;
            _enemyDataSelector = _enemyDatabase.CreateRandomSelector();

            StartCoroutine(MainWaveLoop());
        }


        private IEnumerator MainWaveLoop()
        {
            CurrentWave = 0;
            Coroutine waveCoroutine = null;
            while (IsPlayerAlive)
            {
                CurrentWave++;
                TimeUntilNextWave = 0;
                float waveStartTime = Time.time;
                int playerLevel = PlayerController.Instance.Stats.Level;
                int wormholeCount = _initialWaveEnemyCount + Mathf.RoundToInt(playerLevel * 2.4f);
                Debug.Log($"Starting wave for player level {playerLevel} with {wormholeCount} wormholes.");
                _aliveEnemies.Clear();
                waveCoroutine = StartCoroutine(SpawnWormholes(wormholeCount));
                
                // Wait until all enemies are spawned and dead, or the player is dead, or the wave has lasted too long.
                while (HasAliveEnemies && IsPlayerAlive && !HasWaveLastedTooLong(waveStartTime))
                {
                    TimeUntilNextWave = Mathf.RoundToInt(_maxWaveLength - (Time.time - waveStartTime));
                    yield return null;
                }
                
                if (IsPlayerAlive)
                {
                    // Wait for _waveCooldown seconds, while updating the TimeUntilNextWave property.
                    for (int i = 0; i < _waveCooldown; i++)
                    {
                        TimeUntilNextWave = _waveCooldown - i;
                        yield return new WaitForSeconds(1);
                    }
                }
            }
            
            if (waveCoroutine != null)
                StopCoroutine(waveCoroutine);

            for (int i = _aliveWormholes.Count - 1; i >= 0; i--)
            {
                Wormhole wormhole = _aliveWormholes[i];
                wormhole.DestroySelf();
            }

            for (int i = _aliveEnemies.Count - 1; i >= 0; i--)
            {
                Enemy enemy = _aliveEnemies[i];
                enemy.DestroySelf();
            }

            _aliveWormholes.Clear();
            _aliveEnemies.Clear();
            
            Debug.LogWarning("Game has ended. No more waves will be spawned.");
        }


        private bool HasWaveLastedTooLong(float waveStartTime) => Time.time - waveStartTime >= _maxWaveLength;


        private IEnumerator SpawnWormholes(int count)
        {
            // Select a random center point for the group of wormholes.
            Vector2 groupOrigin = (Vector3)Random.insideUnitCircle * _wormholeGroupCenterMaxRange;
            for (int i = 0; i < count; i++)
            {
                SpawnWormhole(groupOrigin);
                yield return new WaitForSeconds(Random.Range(_wormholeSpawnIntervalRange.x, _wormholeSpawnIntervalRange.y));
            }
        }
        
        
        private void SpawnWormhole(Vector2 groupOrigin)
        {
            Vector2 targetPos = groupOrigin + Random.insideUnitCircle * _wormholeGroupMaxSize;
            Wormhole wormhole = Instantiate(_wormholePrefab, transform.position + (Vector3)targetPos, Quaternion.identity);
            wormhole.transform.parent = transform;
            wormhole.gameObject.SetActive(true);
            wormhole.gameObject.name = "Wormhole";
            wormhole.Initialize(_enemyDataSelector.SelectRandomItem(), Random.Range(_enemySpawnDelayRange.x, _enemySpawnDelayRange.y));
            _aliveWormholes.Add(wormhole);
        }


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _wormholeGroupCenterMaxRange);
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position + new Vector3(_wormholeGroupCenterMaxRange, 0, 0), _wormholeGroupMaxSize);
        }
    }
}