using System;
using SaveLoad;
using UnityEngine;

namespace Infrastructure
{
    public class Root : MonoBehaviour
    {
        [SerializeField] private GameController _gameController;
        [SerializeField] private ScoreService _scoreService;
        private PlayerProgress _playerProgress;
        private MessageBus _messageBus;

        private void Awake()
        {
            _playerProgress = SaveLoadService.LoadProgress() ?? new PlayerProgress();
            _messageBus = new MessageBus();
            _messageBus.OnForceQuit += SaveProgress;
            
            _gameController.Init(_messageBus, _playerProgress);
            _scoreService.Init(_messageBus, _playerProgress);
        }

        private void SaveProgress()
        {
            SaveLoadService.SaveProgress(_playerProgress);
        }
    }
}
