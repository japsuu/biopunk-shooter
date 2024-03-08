using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace World
{
    /// <summary>
    /// Shows an image on top of some other object.
    /// Smoothly fades overlay image in and out.
    /// </summary>
    public class Highlighter : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer _spriteRenderer;
        
        [SerializeField]
        private float _fadeDuration = 1f;

        private TweenerCore<Color, Color, ColorOptions> _tweener;


        private void OnEnable()
        {
            _tweener?.Kill();
            _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, 1f);
            _tweener = _spriteRenderer.DOFade(0f, _fadeDuration).SetLoops(-1, LoopType.Yoyo);
        }
        
        
        private void OnDisable()
        {
            _tweener?.Kill();
        }


        private void OnDestroy()
        {
            _tweener?.Kill();
        }
    }
}