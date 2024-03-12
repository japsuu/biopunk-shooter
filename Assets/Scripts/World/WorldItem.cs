using System;
using Items;
using Thirdparty.UnityTooltips.Scripts.UI;
using UI;
using UnityEngine;

namespace World
{
    /// <summary>
    /// An item drop in the world, that can be dragged into a slot.
    /// Gets destroyed when assigned to a slot.
    /// </summary>
    public class WorldItem : MonoBehaviour
    {
        [SerializeField]
        private ReactToPointer _tooltip;

        [SerializeField]
        private SpriteRenderer _spriteRendererBackground;
        [SerializeField]
        private SpriteRenderer _spriteRendererItem;
        [SerializeField]
        private SpriteRenderer _spriteRendererOverlay;

        [SerializeField]
        private GameObject _highlighter;

        private ItemData _itemData;


        public void Initialize(ItemData itemData)
        {
            _itemData = itemData;
            _spriteRendererBackground.color = itemData.UiBackgroundColor;
            _spriteRendererBackground.sprite = itemData.UiBackgroundSprite;
            _spriteRendererItem.sprite = itemData.UiSprite;
            _spriteRendererOverlay.sprite = itemData.UiOverlaySprite;
            
            _tooltip.TooltipMessage = itemData.GetTooltipText();
            
            _highlighter.SetActive(false);
        }


        private void OnMouseDown()
        {
            DraggableItem.Instance.Initialize(_itemData, transform.position);
            Destroy(_highlighter);
            Destroy(gameObject);
        }


        private void OnMouseEnter()
        {
            _highlighter.SetActive(true);
        }


        private void OnMouseExit()
        {
            _highlighter.SetActive(false);
        }
    }
}