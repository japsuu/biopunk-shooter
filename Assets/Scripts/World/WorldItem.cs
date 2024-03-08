using Items;
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
        private SpriteRenderer _spriteRenderer;

        [SerializeField]
        private GameObject _highlighter;
        
        private ItemData _itemData;
        
        
        public void Initialize(ItemData item)
        {
            _itemData = item;
            _spriteRenderer.sprite = item.UiSprite;
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