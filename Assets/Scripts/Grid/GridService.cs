using System;
using System.Collections.Generic;
using Configs;
using Data;
using Infrastructure;
using Objects;
using UnityEngine;

namespace Grid
{
    public class GridService : MonoBehaviour
    {
        [SerializeField] private GameConfig _gameConfig;
        [SerializeField] private GridView _gridView;
        [SerializeField] private CardView cardViewPrefab;
    
        private List<CardView> _cards;
        private List<int> _contentIndexes;
    
        private PlayerProgress _playerProgress;
        private MessageBus _messageBus;

        public void Init(MessageBus messageBus, PlayerProgress playerProgress)
        {
            _messageBus = messageBus;
            _playerProgress = playerProgress;

            Subscribe();
        
            _cards = new List<CardView>();
            _contentIndexes = new List<int>();
        
            LoadGridData();
        }

        private void Subscribe()
        {
            _messageBus.OnForceSaveGameData += Save;
        }

        private void OnDestroy()
        {
            _messageBus.OnForceSaveGameData -= Save;
        }

        private void GenerateGrid()
        {
            Transform cardsParent = _gridView.GetCardsParent();
            int cardsCount = _playerProgress.GridData.width * _playerProgress.GridData.height;
            switch (_playerProgress.GridData.PoolType)
            {
                case PoolType.Colors:
                    var colorValues = GenerateCardValues(_gameConfig.Pool.Colors, cardsCount);
                    InitCards(colorValues, cardsParent, cardsCount);
                    break;
                case PoolType.Sprites:
                    var spriteValues = GenerateCardValues(_gameConfig.Pool.Sprites, cardsCount);
                    InitCards(spriteValues, cardsParent, cardsCount);
                    break;
                case PoolType.Symbols:
                    var symbolValues = GenerateCardValues(_gameConfig.Pool.Symbols, cardsCount);
                    InitCards(symbolValues, cardsParent, cardsCount);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }       
        
            _gridView.SetupFixedWidth(_playerProgress.GridData.width);
        }

        private T[] GenerateCardValues<T>(T[] pool, int cardsCount)
        {
            if (cardsCount % 2 != 0)
            {
                throw new ArgumentException("Cards count should be even to form pairs.");
            }

            List<T> cardValues = new List<T>();
            int pairsCount = cardsCount / 2;

            for (int i = 0; i < pairsCount; i++)
            {
                int contentIndex = i % pool.Length;
                T value = pool[contentIndex];
            
                _contentIndexes.Add(contentIndex);
                _contentIndexes.Add(contentIndex);
            
                cardValues.Add(value);
                cardValues.Add(value);
            }

            for (int i = 0; i < cardValues.Count; i++)
            {
                int randomIndex = UnityEngine.Random.Range(0, cardValues.Count);
                (cardValues[i], cardValues[randomIndex]) = (cardValues[randomIndex], cardValues[i]);
                (_contentIndexes[i], _contentIndexes[randomIndex]) = (_contentIndexes[randomIndex], _contentIndexes[i]);
            }

            return cardValues.ToArray();
        }

        private void InitCards<T>(T[] values, Transform cardsParent, int cardsCount)
        {
            for (int i = 0; i < cardsCount; i++)
            {
                CardView cardView = Instantiate(cardViewPrefab, cardsParent);
                cardView.Init(values[i]);
                _cards.Add(cardView);
            }
        }
        private void InitCards2<T>(T[] values, Transform cardsParent, int cardsCount)
        {
            for (int i = 0; i < cardsCount; i++)
            {
                CardView cardView = Instantiate(cardViewPrefab, cardsParent);
                cardView.Init(values[_playerProgress.GridData.cards[i].contentIndex]);
                if (_playerProgress.GridData.cards[i].isMatched)
                {
                    cardView.ReverseIsFront();
                    cardView.FlipCard();
                    cardView.DisableCard();
                }
                _cards.Add(cardView);
                _contentIndexes.Add(_playerProgress.GridData.cards[i].contentIndex);
            }
        }
        private void LoadGridData()
        {
            if (!_playerProgress.HasProgress)
            {
                GenerateGrid();
                return;
            }
        
            foreach (Transform child in _gridView.GetCardsParent())
            {
                Destroy(child.gameObject);
            }

            _cards.Clear();
        
            Transform cardsParent = _gridView.GetCardsParent();
            int cardsCount = _playerProgress.GridData.width * _playerProgress.GridData.height;
            switch (_playerProgress.GridData.PoolType)
            {
                case PoolType.Colors:
                    InitCards2(_gameConfig.Pool.Colors, cardsParent, cardsCount);
                    break;
                case PoolType.Sprites:
                    InitCards2(_gameConfig.Pool.Sprites, cardsParent, cardsCount);
                    break;
                case PoolType.Symbols:
                    InitCards2(_gameConfig.Pool.Symbols, cardsParent, cardsCount);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }       

            _gridView.SetupFixedWidth(_playerProgress.GridData.width);
        }
        public List<CardView> GetAllCards()
        {
            return _cards;
        }
    
        public int GetTotalPairs()
        {
            int cardsCount = _playerProgress.GridData.width * _playerProgress.GridData.height;
            return cardsCount / 2;
        }

        private void Save()
        {
            _playerProgress.HasProgress = true;

            _playerProgress.GridData.cards = new List<CardData>();
        
            for (int i = 0; i < _cards.Count; i++)
            {
                _playerProgress.GridData.cards.Add(new CardData()
                {
                    isMatched = _cards[i].IsMatched(),
                    contentIndex = _contentIndexes[i],
                });
            }
        
            _messageBus.OnForceSave?.Invoke();
        }
    }
}