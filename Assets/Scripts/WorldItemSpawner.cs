using Items;
using Singletons;
using UnityEngine;
using World;

public class WorldItemSpawner : SingletonBehaviour<WorldItemSpawner>
{
    [SerializeField]
    private WorldItem _worldItemPrefab;
        
        
    public static ItemData SpawnWorldItem(ItemData itemData, Vector3 position)
    {
        WorldItem worldItem = Instantiate(Instance._worldItemPrefab, position, Quaternion.identity);
        worldItem.Initialize(itemData);
        return itemData;
    }
}