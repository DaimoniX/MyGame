using System;
using System.Collections.Generic;
using Enemies;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;
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

        public int EnemiesLeft => _enemiesLeft;
        public int Wave => _wave;
        private int _enemiesLeft = 0;
        private int _enemiesAlive = 0;
        private const int MaxEnemiesAlive = 6;
        private int _wave = 0;
        private int _maxEnemyLevel = 0;

        private float _timer = 0;
        private float _spawnDelay = 4;
        
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
            NextWave();
        }

        public void NextWave()
        {
            Debug.Log("NEXT WAVE");
            _wave++;
            _maxEnemyLevel++;
            _enemiesLeft = 6 * _wave;
        }

        private void Update()
        {
            _aliveEnemies.RemoveWhere(enemy => !enemy);
            foreach (var enemy in _aliveEnemies)
            {
                if (enemy.DestroyOnOutOfBounds && DistanceToPlayer(enemy.transform.position) > maxDistanceToPlayer)
                {
                    _aliveEnemies.Remove(enemy);
                    _enemiesAlive--;
                    Destroy(enemy.gameObject);
                    break;
                }
            }

            if (_enemiesLeft > 0 && _timer <= 0 && _enemiesAlive < MaxEnemiesAlive)
            {
                SpawnEnemy();
                _timer = _spawnDelay;
            }

            if(_enemiesLeft <= 0)
                NextWave();

            if (_aliveEnemies.Count <= 1)
                _timer = -1;
            _timer -= Time.deltaTime;
        }

        private Player2D CreatePlayer()
        {
            int shipID = Math.Clamp(PlayerData.SelectedShip, 0, playerShips.Length);
            return Instantiate(playerShips[shipID]);
        }

        private Vector2 GetEnemySpawnPosition()
        {
            return maxDistanceToPlayer * 0.8f * Random.insideUnitCircle.normalized;
        }

        private void SpawnEnemy()
        {
            _enemiesAlive++;
            int enemyID = Random.Range(0, Math.Min(_maxEnemyLevel, enemies.Length + 1));
            Vector2 position = GetEnemySpawnPosition();
            Enemy enemy = Instantiate(enemies[enemyID], position, transform.rotation);
            enemy.GetHealth().onDeath.AddListener(() => IncreaseScore(enemy.ScoreValue));
            enemy.SetTarget(_player.transform);
            _aliveEnemies.Add(enemy);
        }

        private void IncreaseScore(int value)
        {
            Score++;
            _enemiesAlive--;
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
            SceneManager.LoadScene(0);
            PlayerData.SetHighScore(Score);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if(!_player) return;
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(_player.transform.position, maxDistanceToPlayer);
        }
#endif
    }
}