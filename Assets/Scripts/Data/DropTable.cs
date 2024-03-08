using Items;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "Create Drop Table", fileName = "DropTable_", order = 0)]
    public class DropTable : Database<ItemData> { }
}