using Entities.Enemies;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "Create EnemyDataDatabase", fileName = "Enemies_", order = 0)]
    public class EnemyDataDatabase : Database<EnemyData> { }
}