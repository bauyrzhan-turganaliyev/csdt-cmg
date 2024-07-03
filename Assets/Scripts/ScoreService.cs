using Infrastructure;
using UnityEngine;

public class ScoreService : MonoBehaviour
{
    [SerializeField] private ScoreView _scoreView;
    private MessageBus _messageBus;

    private ScoreData _scoreData;
    private PlayerProgress _playerProgress;

    public void Init(MessageBus messageBus, PlayerProgress playerProgress)
    {
        _messageBus = messageBus;
        _playerProgress = playerProgress;
        
        Subscribe();
    }

    private void Subscribe()
    {
        _messageBus.OnCardFlip += AddCardFlip;
        _messageBus.OnCheckMatch += CheckMatch;
    }

    private void CheckMatch(bool isMatch)
    {
        if (isMatch)
        {
            _playerProgress.ScoreData.Score += 1 * _playerProgress.ScoreData.Combo;
            _playerProgress.ScoreData.Combo++;
            _scoreView.SetScore(_playerProgress.ScoreData.Score);
        }
        else
        {
            _playerProgress.ScoreData.Combo = 1;
        }
        
        _scoreView.SetCombo(_playerProgress.ScoreData.Combo);
    }

    private void AddCardFlip()
    {
        if (_playerProgress != null && _playerProgress.ScoreData != null)
        {
            _playerProgress.ScoreData.Flips++;
            _scoreView.SetFlips(_playerProgress.ScoreData.Flips);
        }
    }
}