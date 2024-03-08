using UnityEngine;
using World;

namespace Items
{
    public abstract class ItemData : ScriptableObject
    {
        [SerializeField]
        private Sprite _uiSprite;
        
        public abstract ItemType Type { get; }
        
        public Sprite UiSprite => _uiSprite;
    }
}