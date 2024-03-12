using UnityEngine;

namespace Items
{
    public abstract class ItemData : ScriptableObject
    {
        [SerializeField]
        private string _name;
        
        [SerializeField]
        private string _tooltipDescription;
        
        [SerializeField]
        private Color _uiBackgroundColor = Color.white;
        
        [SerializeField]
        private Sprite _uiBackgroundSprite;
        
        [SerializeField]
        private Sprite _uiSprite;
        
        [SerializeField]
        private Sprite _uiOverlaySprite;

        public abstract ItemType Type { get; }
        
        public Color UiBackgroundColor => _uiBackgroundColor;
        public Sprite UiSprite => _uiSprite;
        public Sprite UiBackgroundSprite => _uiBackgroundSprite;
        public Sprite UiOverlaySprite => _uiOverlaySprite;
        
        
        public string GetTooltipText()
        {
            return $"<b>{_name}</b>\n\n{_tooltipDescription}";
        }
    }
}