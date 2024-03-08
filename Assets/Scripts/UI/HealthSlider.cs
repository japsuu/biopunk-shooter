using Entities.Player;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Slider))]
    public class HealthSlider : MonoBehaviour
    {
        private Slider _slider;


        private void Awake()
        {
            _slider = GetComponent<Slider>();
        }


        private void Start()
        {
            // Configure the slider
            _slider.minValue = 0;
            _slider.maxValue = 1;
        }


        private void Update()
        {
            _slider.value = PlayerController.Instance.Vitals.CurrentHealth / PlayerController.Instance.Vitals.MaxHealth;
        }
    }
}