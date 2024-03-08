using Singletons;
using UnityEngine;
using World;

namespace DefaultNamespace
{
    public class WorldItemSpawner : SingletonBehaviour<WorldItemSpawner>
    {
        [SerializeField]
        private WorldItem _worldItemPrefab;
    }
}