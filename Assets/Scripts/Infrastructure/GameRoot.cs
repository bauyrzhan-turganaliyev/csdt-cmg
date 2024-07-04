using System;
using DefaultNamespace;
using SaveLoad;
using UnityEngine;

namespace Infrastructure
{
    public class GameRoot : MonoBehaviour
    {
        [SerializeField] private PlayerProgressSO _playerProgressSO;
        [SerializeField] private GameController _gameController;
        [SerializeField] private ScoreService _scoreService;
        [SerializeField] private GameUIService _gameUIService;
        
        private PlayerProgress _playerProgress;
        private MessageBus _messageBus;

        private void Awake()
        {
            SetupPlayerProgress();

            _messageBus = new MessageBus();
            Subscribe();

            _gameController.Init(_messageBus, _playerProgress);
            _scoreService.Init(_messageBus, _playerProgress);
            _gameUIService.Init(_messageBus);
        }

        private void Subscribe()
        {
            _messageBus.OnForceSave += SaveProgress;
            _messageBus.OnClearProgress += ClearProgress;
        }
        private void OnDestroy()
        {
            _messageBus.OnForceSave -= SaveProgress;
            _messageBus.OnClearProgress -= ClearProgress;
        }
        private void SetupPlayerProgress()
        {
            _playerProgress = new PlayerProgress
            {
                GridData = _playerProgressSO.GridData,
                ScoreData = _playerProgressSO.ScoreData,
                HasProgress = _playerProgressSO.HasProgress
            };
        }
        private void ClearProgress()
        {
            _playerProgress = new PlayerProgress();
            _playerProgressSO.GridData = new GridData();
            _playerProgressSO.ScoreData = new ScoreData();
            _playerProgressSO.HasProgress = false;
            SaveLoadService.ResetProgress();
        }

        private void SaveProgress()
        {
            SaveLoadService.SaveProgress(_playerProgress);
        }

        private void OnApplicationQuit()
        {
            if (FlagsStorage.IsAbleToSave)
            {
                _messageBus.OnForceSaveGameData?.Invoke();
                SaveProgress();
            }
        }
    }
}
