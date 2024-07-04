using Infrastructure;
using UnityEngine;

namespace Audio
{
    public class AudioService : MonoBehaviour
    {
        [SerializeField] private AudioClip _matchSound;
        [SerializeField] private AudioClip _missMatchSound;
        [SerializeField] private AudioClip _flipSound;
        [SerializeField] private AudioClip _winSound;

        [SerializeField] private AudioSource _audioSource;
        private MessageBus _messageBus;

        public void Init(MessageBus messageBus)
        {
            _messageBus = messageBus;
            Subscribe();
        }

        private void Subscribe()
        {
            _messageBus.OnCardFlip += PlayFlipSound;
            _messageBus.OnCheckMatch += CheckMatch;
            _messageBus.OnGameOver += OnGameOver;
        }
        private void OnDestroy()
        {
            _messageBus.OnCardFlip -= PlayFlipSound;
            _messageBus.OnCheckMatch -= CheckMatch;
            _messageBus.OnGameOver -= OnGameOver;
        }
        private void OnGameOver(bool isWin)
        {
            if (isWin)
            {
                _audioSource.PlayOneShot(_winSound);
            }
        }

        private void CheckMatch(bool isMatch)
        {
            _audioSource.PlayOneShot(isMatch ? _matchSound : _missMatchSound);
        }

        private void PlayFlipSound()
        {
            _audioSource.PlayOneShot(_flipSound);
        }
    }
}