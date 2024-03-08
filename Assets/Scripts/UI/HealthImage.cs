using Entities.Player;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Image))]
    public class HealthImage : MonoBehaviour
    {
        private Image _image;


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
        }


        private void Update()
        {
            _image.fillAmount = PlayerController.Instance.Vitals.CurrentHealth / PlayerController.Instance.Vitals.MaxHealth;
        }
    }
}