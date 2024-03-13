using System;
using Entities.Enemies;
using Singletons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class WaveUIManager : SingletonBehaviour<WaveUIManager>
    {
        [SerializeField]
        private GameObject _enemyImagePrefab;

        [SerializeField]
        private RectTransform _enemyImageRoot;

        [SerializeField]
        private RectTransform _aliveEnemiesDisplayRoot;
        
        [SerializeField]
        private TMP_Text _waveText;
        
        [SerializeField]
        private TMP_Text _timeUntilNextWaveText;
        
        private int _aliveEnemies;
        
        
        public GameObject SpawnEnemyImage(Enemy enemy)
        {
            if (_aliveEnemies == 0)
                _aliveEnemiesDisplayRoot.gameObject.SetActive(true);
            
            GameObject go = Instantiate(_enemyImagePrefab, _enemyImageRoot);
            go.GetComponent<Image>().sprite = enemy.Data.UiImage;
            _aliveEnemies++;
            return go;
        }
        
        
        public void DestroyEnemyImage(GameObject enemyImage)
        {
            _aliveEnemies--;
            Destroy(enemyImage);
            
            if (_aliveEnemies == 0)
                _aliveEnemiesDisplayRoot.gameObject.SetActive(false);
        }


        private void Update()
        {
            _waveText.text = $"Wave {EnemyManager.Instance.CurrentWave}";
            _timeUntilNextWaveText.text = $"Next wave in: {EnemyManager.Instance.TimeUntilNextWave:F1}s";
        }
    }
}