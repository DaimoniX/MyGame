using System;
using Level;
using TMPro;
using UnityEngine;

namespace Player
{
    public class PlayerUI : MonoBehaviour
    {
        private LevelManager _level;
        private Player2D _player;
        [SerializeField] private TextMeshProUGUI score;
        [SerializeField] private TextMeshProUGUI ability;
        [SerializeField] private TextMeshProUGUI health;
        [SerializeField] private TextMeshProUGUI enemies;

        public void Setup(LevelManager level, Player2D player)
        {
            _level = level;
            _player = player;
        }

        private void Update()
        {
            score.text = $"Score: {_level.Score}";
            ability.text = $"Ability: {100 - (int)(_player.Ability.CooldownPercent * 100)}%";
            health.text = $"Health: {_player.GetHealth().CurrentHealth}/{_player.GetHealth().MaxHealth}";
            enemies.text = $"Wave: {_level.Wave} Enemies left: {_level.EnemiesLeft}";
        }
    }
}