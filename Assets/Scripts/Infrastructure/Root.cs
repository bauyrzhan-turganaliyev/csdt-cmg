using System;
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
            _playerProgress = new PlayerProgress();
            _messageBus = new MessageBus();
            
            _gameController.Init(_messageBus);
            _scoreService.Init(_messageBus, _playerProgress);
        }
    }
}
