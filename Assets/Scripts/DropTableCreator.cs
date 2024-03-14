using Data;
using Items;
using NaughtyAttributes;
using UnityEngine;

public class DropTableCreator : MonoBehaviour
{
    public DropTable DropTable;
    public ItemData[] Items;
    
    
    [Button("Create Drop Table")]
    public void CreateDropTable()
    {
        print("Creating drop table.");
        DropTable.AddItems(Items);
    }
}