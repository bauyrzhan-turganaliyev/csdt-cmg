using System;
using System.Collections.Generic;
using Configs;
using Cysharp.Threading.Tasks;
using Infrastructure;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.Serialization;

public class GridService : MonoBehaviour
{
    [SerializeField] private GameConfig _gameConfig;
    [SerializeField] private GridView _gridView;
    [SerializeField] private CardView cardViewPrefab;
    
    private List<CardView> _cards;
    private List<int> _contentIndexes;
    
    private PlayerProgress _playerProgress;
    private MessageBus _messageBus;

    public async void Init(MessageBus messageBus, PlayerProgress playerProgress)
    {
        _messageBus = messageBus;
        _playerProgress = playerProgress;
        
        _cards = new List<CardView>();
        _contentIndexes = new List<int>();

        //await UniTask.Delay(2000);
        
        LoadGridData();
        //GenerateGrid();
    }

    private void GenerateGrid()
    {
        Transform cardsParent = _gridView.GetCardsParent();
        int cardsCount = _gameConfig.GridWidth * _gameConfig.GridHeight;
        switch (_gameConfig.PoolType)
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
        
        _gridView.SetupFixedWidth(_gameConfig.GridWidth);
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
    public void LoadGridData()
    {
        if (_playerProgress.ScoreData.Flips == 0)
        {
            Debug.LogError("Save data not found!");
            GenerateGrid();
            return;
        }
        _gameConfig.GridWidth = _playerProgress.GridData.width;
        _gameConfig.GridHeight = _playerProgress.GridData.height;

        foreach (Transform child in _gridView.GetCardsParent())
        {
            Destroy(child.gameObject);
        }

        _cards.Clear();
        
        Transform cardsParent = _gridView.GetCardsParent();
        int cardsCount = _gameConfig.GridWidth * _gameConfig.GridHeight;
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

        _gridView.SetupFixedWidth(_gameConfig.GridWidth);
        Debug.Log("Grid data loaded.");
    }
    public List<CardView> GetAllCards()
    {
        return _cards;
    }
    
    public int GetTotalPairs()
    {
        int cardsCount = _gameConfig.GridWidth * _gameConfig.GridHeight;
        return cardsCount / 2;
    }

    private void OnApplicationQuit()
    {
        _playerProgress.GridData = new GridData()
        {
            width = _gameConfig.GridWidth,
            height = _gameConfig.GridHeight,
            PoolType = _gameConfig.PoolType,
            cards = new List<CardData>()
        };

        for (int i = 0; i < _cards.Count; i++)
        {
            _playerProgress.GridData.cards.Add(new CardData()
            {
                isMatched = _cards[i].IsMatched(),
                contentIndex = _contentIndexes[i],
            });
        }
        
        _messageBus.OnForceQuit?.Invoke();
        
        /*
        string json = JsonUtility.ToJson(_playerProgress.GridData);
        PlayerPrefs.SetString("GridData", json);
        PlayerPrefs.Save();*/
    }
}