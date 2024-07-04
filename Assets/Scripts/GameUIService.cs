using System;
using Infrastructure;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class GameUIService : MonoBehaviour
    {
        private const int MENU_SCENE_INDEX = 0;
        
        [SerializeField] private GameOverView _gameOverView;
        [SerializeField] private ConfirmationView _confirmationView;

        [SerializeField] private Button _backButton;
        private MessageBus _messageBus;

        public void Init(MessageBus messageBus)
        {
            _messageBus = messageBus;
            
            Subscribe();

            _gameOverView.Init();
            _confirmationView.Init();
        }

        private void Subscribe()
        {
            _messageBus.OnGameOver += OnGameOver;
            
            _backButton.onClick.AddListener(OnBackButtonClicked);

            _gameOverView.OnBackMenu += BackMenu;
            _confirmationView.OnTryConfirm += TryBackMenu;
        }

        private void OnDestroy()
        {
            _messageBus.OnGameOver -= OnGameOver;
            
            _backButton.onClick.RemoveAllListeners();

            _gameOverView.OnBackMenu -= BackMenu;
            _confirmationView.OnTryConfirm -= TryBackMenu;
        }

        private void BackMenu()
        {
            SceneManager.LoadScene(MENU_SCENE_INDEX);
        }

        private void OnGameOver(bool isWin)
        {
            _gameOverView.SetTitleText("Congratulations!");
            _gameOverView.gameObject.SetActive(true);
        }

        private void TryBackMenu(bool isBack)
        {
            _confirmationView.gameObject.SetActive(false);
            if (isBack)
            {
                _messageBus.OnForceSaveGameData?.Invoke();
                BackMenu();
            }
        }

        private void OnBackButtonClicked()
        {
            _confirmationView.gameObject.SetActive(true);
            _confirmationView.SetTitleText();
        }
    }
}