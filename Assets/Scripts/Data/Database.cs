using System;
using System.Collections.Generic;
using Thirdparty.WeightedRandomSelector;
using Thirdparty.WeightedRandomSelector.Interfaces;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "db_", menuName = "GameObject Database", order = 0)]
    public abstract class Database<T> : ScriptableObject
    {
        [Serializable]
        public class Entry<TE>
        {
            public TE Object;
            public float Weight;
        }

        [SerializeField]
        private Entry<T>[] _entries;
        
        public IEnumerable<Entry<T>> Entries => _entries;
        public IEnumerable<T> Objects => Array.ConvertAll(_entries, entry => entry.Object);


        /// <summary>
        /// Creates a random selector for the specified type.
        /// Only includes objects that are of the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IRandomSelector<T> CreateRandomSelector()
        {
            DynamicRandomSelector<T> selector = new();
            
            foreach (Entry<T> entry in _entries)
            {
                T component = entry.Object;
                if (component != null)
                    selector.Add(component, entry.Weight);
            }

            return selector.Build();
        }
    }
}