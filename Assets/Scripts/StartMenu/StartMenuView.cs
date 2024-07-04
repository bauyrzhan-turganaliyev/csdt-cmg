using System;
using Configs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Infrastructure
{
    public class StartMenuView : MonoBehaviour
    {
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _newGameButton;

        [SerializeField] private GameObject _buttons;
        [SerializeField] private GameObject _newGameSettingsPanel;

        [SerializeField] private Slider _widthSlider;
        [SerializeField] private Slider _heightSlider;

        [SerializeField] private TMP_Text _widthText;
        [SerializeField] private TMP_Text _heightText;

        [SerializeField] private AdvancedButton[] _poolButtons;
        [SerializeField] private Button _startButton;

        private PlayerProgress _playerProgress;
        private AdvancedButton _selectedPoolButton;
          
        public Action<PlayerProgress> OnStartGame;

        public void Init(PlayerProgress playerProgress)
        {
            _playerProgress = playerProgress;

            Subscribe();
        }

        private void OnPoolSelected(AdvancedButton button)
        {
            if (_selectedPoolButton != null)
            {
                _selectedPoolButton.Unselect();
            }

            _selectedPoolButton = button;
            _selectedPoolButton.Select();
        }

        private void StartGame()
        {
            _playerProgress.GridData = new GridData()
            {
                width = (int)_widthSlider.value,
                height = (int)_heightSlider.value,
                PoolType = (PoolType)_selectedPoolButton.Value,
            };
               
            OnStartGame?.Invoke(_playerProgress);
        }

        private void OnNewGameClicked()
        {
            _playerProgress = new PlayerProgress();
               
            _buttons.SetActive(false);
            _newGameSettingsPanel.SetActive(true);
        }

        private void OnWidthSliderChanged(float value)
        {
            int width = Mathf.RoundToInt(value);
            int height = Mathf.RoundToInt(_heightSlider.value);

            if ((width * height) % 2 != 0)
            {
                height += 1;
                _heightSlider.value = height;
            }

            _widthText.text = _widthSlider.value.ToString();
            _heightText.text = _heightSlider.value.ToString();
        }

        private void OnHeightSliderChanged(float value)
        {
            int width = Mathf.RoundToInt(_widthSlider.value);
            int height = Mathf.RoundToInt(value);

            if ((width * height) % 2 != 0)
            {
                width += 1;
                _widthSlider.value = width;
            }
               
            _widthText.text = _widthSlider.value.ToString();
            _heightText.text = _heightSlider.value.ToString();
        }
          
        private void Subscribe()
        {
            if (_playerProgress.HasProgress)
            {
                _continueButton.onClick.AddListener((() => OnStartGame?.Invoke(_playerProgress)));
                _continueButton.gameObject.SetActive(true);
            }

            _newGameButton.onClick.AddListener(OnNewGameClicked);
            _startButton.onClick.AddListener(StartGame);
            for (int i = 0; i < _poolButtons.Length; i++)
            {
                _poolButtons[i].SetValue(i);
                    
                var i1 = i;
                _poolButtons[i].Button.onClick.AddListener(() => OnPoolSelected(_poolButtons[i1]));
            }
               
            _widthSlider.onValueChanged.AddListener(OnWidthSliderChanged);
            _heightSlider.onValueChanged.AddListener(OnHeightSliderChanged);
        }
    }
}