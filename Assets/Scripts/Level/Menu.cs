using System;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Level
{
    public class Menu : MonoBehaviour
    {
        private const string Key = "highscore";
        [SerializeField] private TextMeshProUGUI highScore;
        [SerializeField] private TextMeshProUGUI totalScore;

        private void Start()
        {
            PlayerData.Load();
            highScore.text = $"HighScore: {PlayerData.HighScore}";
            totalScore.text = $"Total: {PlayerData.TotalScore}";
        }

        public void Play()
        {
            SceneManager.LoadScene(1);
        }
        
        public void Exit()
        {
            Application.Quit();
        }
    }
}