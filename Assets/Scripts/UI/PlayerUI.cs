using Level;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PlayerUI : MonoBehaviour
    {
        private LevelManager _level;
        private Player2D _player;
        [SerializeField] private TextMeshProUGUI score;
        [SerializeField] private TextMeshProUGUI health;
        [SerializeField] private TextMeshProUGUI info;
        [SerializeField] private Slider healthSlider;
        [SerializeField] private Slider abilitySlider;

        public void Setup(LevelManager level, Player2D player)
        {
            _level = level;
            _player = player;
            _player.GetHealth().onDeath.AddListener(() => enabled = false);
        }

        private void Update()
        {
            score.text = $"Score: {_level.Score}";
            health.text = $"{_player.GetHealth().CurrentHealth}/{_player.GetHealth().MaxHealth}";
            info.text = _level.Status;
            healthSlider.value = _player.GetHealth().HealthPercent;
            abilitySlider.value = 1 - _player.Ability.CooldownPercent;
        }
    }
}