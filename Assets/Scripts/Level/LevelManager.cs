using System;
using System.Collections.Generic;
using System.Linq;
using Enemies;
using Player;
using UnityEngine;
using UnityEngine.Events;
using Utils;
using Random = UnityEngine.Random;

namespace Level
{
    public class LevelManager : MonoBehaviour
    {
        private PlayerUI _ui;
        private Player2D _player;
        public int Score { get; private set; } = 0;
        [SerializeField] private float maxDistanceToPlayer = 40;
        [SerializeField] private Player2D[] playerShips;
        [SerializeField] private Enemy[] enemies;
        private HashSet<Enemy> _aliveEnemies;
        public UnityEvent<int> onNextWave = new();
        
        private int _enemiesLeft = 0;
        private const int MaxEnemiesAlive = 6;
        private int _wave = 0;
        private int _maxEnemyLevel = 0;

        private float _timer = 0;
        private const float SpawnDelay = 4;

        public string Status => $"Wave: {_wave} Enemies left: {_enemiesLeft}";

        private void Awake()
        {
            Score = 0;
            _aliveEnemies = new HashSet<Enemy>();
            _ui = FindObjectOfType<PlayerUI>();
            FollowCamera followCamera = FindObjectOfType<FollowCamera>();
            _player = CreatePlayer();
            _player.GetHealth().onDeath.AddListener(UpdateData);
            _ui.Setup(this, _player);
            followCamera.SetTarget(_player.transform);
            _timer = 0;
            
#if UNITY_EDITOR
            onNextWave.AddListener((wave) => Debug.Log($"NEW WAVE {wave}"));
#endif
            NextWave();
        }

        private void NextWave()
        {
            _wave++;
            onNextWave.Invoke(_wave);
            _maxEnemyLevel++;
            _enemiesLeft = 4 + 6 * _wave + 10 * (_wave / 5);
        }

        private void Update()
        {
            if(!_player) return;
            _aliveEnemies.RemoveWhere(enemy => !enemy);
            foreach (var enemy in _aliveEnemies.Where(enemy => enemy.DestroyOnOutOfBounds && DistanceToPlayer(enemy.transform.position) > maxDistanceToPlayer))
            {
                _aliveEnemies.Remove(enemy);
                Destroy(enemy.gameObject);
                break;
            }

            if (_enemiesLeft - _aliveEnemies.Count > 0 && _timer <= 0 && _aliveEnemies.Count < MaxEnemiesAlive)
            {
                SpawnEnemy();
                _timer = SpawnDelay;
            }

            if (_enemiesLeft <= 0)
                NextWave();

            if (_aliveEnemies.Count <= MaxEnemiesAlive / 3)
                _timer = 0;
            _timer -= Time.deltaTime * (1 + _wave / 10f);
        }

        private Player2D CreatePlayer()
        {
            int shipID = Math.Clamp(PlayerData.SelectedShip, 0, playerShips.Length);
            return Instantiate(playerShips[shipID]);
        }

        private Vector2 GetEnemySpawnPosition()
        {
            return (Vector2)transform.position + maxDistanceToPlayer * 0.8f * Random.insideUnitCircle.normalized;
        }

        private void SpawnEnemy()
        {
            int enemyID = Random.Range(0, Math.Min(_maxEnemyLevel, enemies.Length));
            Enemy enemy = Instantiate(enemies[enemyID], GetEnemySpawnPosition(), transform.rotation);
            enemy.GetHealth().onDeath.AddListener(() => IncreaseScore(enemy.ScoreValue));
            enemy.SetTarget(_player.transform);
            _aliveEnemies.Add(enemy);
        }

        private void IncreaseScore(int value)
        {
            Score += value;
            _enemiesLeft--;
            PlayerData.IncreaseScore();
        }

        private float DistanceToPlayer(Vector2 point)
        {
            return Vector2.Distance(_player.transform.position, point);
        }

        private void UpdateData()
        {
            enabled = false;
            // SceneManager.LoadScene(0);
            PlayerData.SetHighScore(Score);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            if (!_player)
            {
                var position = transform.position;
                Gizmos.DrawWireSphere(position, maxDistanceToPlayer);
                Gizmos.DrawWireSphere(position, maxDistanceToPlayer * 0.8f);
                return;
            }
            Gizmos.DrawWireSphere(_player.transform.position, maxDistanceToPlayer);
            UnityEditor.Handles.color = GUI.color = Color.yellow;
            UnityEditor.Handles.Label(transform.position,
                $"Spawn timer: {_timer}\n" +
                $"Alive enemies: {_aliveEnemies.Count}");
        }
#endif
    }
}