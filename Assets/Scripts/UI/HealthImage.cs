using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Entities.Player;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Image))]
    public class HealthImage : MonoBehaviour
    {
        [SerializeField]
        private float _damageFlashLength = 0.3f;
        
        private Image _image;
        private TweenerCore<Color, Color, ColorOptions> _damageTweener;


        private void Awake()
        {
            _image = GetComponent<Image>();
        }


        private void Start()
        {
            // Configure the image
            _image.type = Image.Type.Filled;
            _image.fillMethod = Image.FillMethod.Vertical;
            _image.fillOrigin = (int)Image.OriginVertical.Bottom;
            
            PlayerController.Instance.Vitals.OnHealthDecreased += FlashRed;
        }


        private void FlashRed()
        {
            _damageTweener?.Kill();

            _image.color = Color.red;
            _damageTweener = _image.DOColor(Color.white, _damageFlashLength);
        }


        private void Update()
        {
            _image.fillAmount = PlayerController.Instance.Vitals.CurrentHealth / PlayerController.Instance.Vitals.MaxHealth;
        }


        private void OnDestroy()
        {
            _damageTweener?.Kill();
            if (PlayerController.Instance != null)
                PlayerController.Instance.Vitals.OnHealthDecreased -= FlashRed;
        }
    }
}