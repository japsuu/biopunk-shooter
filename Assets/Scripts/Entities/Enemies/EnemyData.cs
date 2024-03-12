﻿using Data;
using UnityEngine;

namespace Entities.Enemies
{
    [CreateAssetMenu(fileName = "Enemy_", menuName = "Enemy Data", order = 0)]
    public class EnemyData : ScriptableObject
    {
        public float Health;
        public float Damage;
        public float KillRewardXp;
        
        [SerializeField]
        private DropTable _dropTable;
        
        [SerializeField]
        private Enemy _prefab;
        
        public DropTable DropTable => _dropTable;
        public Enemy Prefab => _prefab;
    }
}