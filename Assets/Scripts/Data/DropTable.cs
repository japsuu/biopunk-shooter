using System.Collections.Generic;
using Items;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "Create Drop Table", fileName = "DropTable_", order = 0)]
    public class DropTable : Database<ItemData>
    {
        public void AddItems(ItemData[] items)
        {
            List<Entry<ItemData>> datas = new(_entries.Length + items.Length);
            datas.AddRange(_entries);
            foreach (ItemData item in items)
            {
                datas.Add(new Entry<ItemData> {Object = item, Weight = 1});
            }
            
            _entries = datas.ToArray();
        }
    }
}