using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Animator))]
    public class SettingsPanel : MonoBehaviour
    {
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider fxSlider;
        [SerializeField] private TMP_Dropdown shipSelector;
        private Animator _animator;
        private static readonly int Toggle = Animator.StringToHash("Toggle");

        private void Start()
        {
            musicSlider.value = PlayerData.MusicVolume;
            fxSlider.value = PlayerData.FXVolume;
            shipSelector.value = PlayerData.SelectedShip;
            _animator = transform.GetComponent<Animator>();
        }

        public void TogglePanel()
        {
            _animator.SetTrigger(Toggle);
        }

        public void SelectShip(int shipID)
        {
            PlayerData.SelectShip(shipID);
        }
        
        public void SetMusicVolume(float value)
        {
            PlayerData.SetMusicVolume(value);
        }

        public void SetFXVolume(float value)
        {
            PlayerData.SetFXVolume(value);
        }
    }
}