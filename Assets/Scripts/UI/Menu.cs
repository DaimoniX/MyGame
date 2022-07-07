using Player;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class Menu : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI highScore;
        [SerializeField] private TextMeshProUGUI totalScore;
        [SerializeField] private SettingsPanel settings;

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

        public void ToggleSettings()
        {
            settings.TogglePanel();
        }
        
        public void Exit()
        {
            Application.Quit();
        }
    }
}