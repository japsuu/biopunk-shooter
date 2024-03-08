using System;
using Entities.Player;
using TMPro;
using UnityEngine;

namespace UI
{
    public class PlayerStatsDisplay : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _healthText;
        
        [SerializeField]
        private TMP_Text _regenText;
        
        [SerializeField]
        private TMP_Text _fortitudeText;
        
        [SerializeField]
        private TMP_Text _xpText;
        
        [SerializeField]
        private TMP_Text _levelText;


        private void Update()
        {
            _healthText.text =      $"Health : {Math.Round(PlayerController.Instance.Vitals.CurrentHealth, 1)} / {Math.Round(PlayerController.Instance.Vitals.MaxHealth, 1)}";
            _regenText.text =       $"Regen / sec : {Math.Round(PlayerController.Instance.Vitals.Regen, 1)}";
            _fortitudeText.text =   $"Fortitude : {Math.Round(PlayerController.Instance.Vitals.FortitudeFactor, 1)}";
            _xpText.text =          $"XP : {Math.Round(PlayerController.Instance.Stats.Experience, 1)} / {Math.Round(PlayerController.Instance.Stats.ExperienceToNextLevel, 1)}";
            _levelText.text =       $"Level : {PlayerController.Instance.Stats.Level}";
        }
    }
}