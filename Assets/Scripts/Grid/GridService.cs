using System;
using System.Collections.Generic;
using Configs;
using UnityEngine;

public class GridService : MonoBehaviour
{
    [SerializeField] private GameConfig _gameConfig;
    [SerializeField] private GridView _gridView;
    [SerializeField] private MatchCardView matchCardViewPrefab;
    private List<MatchCardView> _cards;

    public void Init()
    {
        _cards = new List<MatchCardView>();
        GenerateGrid();
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
            T value = pool[i % pool.Length];
            cardValues.Add(value);
            cardValues.Add(value);
        }

        for (int i = 0; i < cardValues.Count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, cardValues.Count);
            (cardValues[i], cardValues[randomIndex]) = (cardValues[randomIndex], cardValues[i]);
        }

        return cardValues.ToArray();
    }

    private void InitCards<T>(T[] values, Transform cardsParent, int cardsCount)
    {
        for (int i = 0; i < cardsCount; i++)
        {
            MatchCardView cardView = Instantiate(matchCardViewPrefab, cardsParent);
            cardView.Init(values[i]);
            _cards.Add(cardView);
        }
    }
    
    public List<MatchCardView> GetAllCards()
    {
        return _cards;
    }
    
    public int GetTotalPairs()
    {
        int cardsCount = _gameConfig.GridWidth * _gameConfig.GridHeight;
        return cardsCount / 2;
    }
}