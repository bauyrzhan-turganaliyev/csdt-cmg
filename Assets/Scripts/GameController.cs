using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data;
using DG.Tweening;
using Grid;
using Infrastructure;
using Objects;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GridService _gridService;
    [SerializeField] private float _waitSecondsBeforeClose = 2f;
    
    private readonly List<CardView> _openCards = new List<CardView>();
    
    private int _pairsFound;
    private int _totalPairs;
    
    private MessageBus _messageBus;
    private PlayerProgress _playerProgress;

    public void Init(MessageBus messageBus, PlayerProgress playerProgress)
    {
        _messageBus = messageBus;
        _playerProgress = playerProgress;

        _gridService.Init(_messageBus, _playerProgress);
        
        SetupGameData();
    }

    private void SetupGameData()
    {
        SubscribeToCardClicks();

        _totalPairs = _gridService.GetTotalPairs();

        if (_playerProgress.HasProgress)
        {
            var allMatchedCardsCount = _playerProgress.GridData.cards.Count(t => t.isMatched);
            _pairsFound = allMatchedCardsCount / 2;
        }
    }

    private void SubscribeToCardClicks()
    {
        foreach (CardView card in _gridService.GetAllCards())
        {
            card.OnClick += () => OnCardClicked(card);
        }
    }

    private void OnCardClicked(CardView clickedCard)
    {
        if (_openCards.Contains(clickedCard))
            return;

        _messageBus.OnCardFlip?.Invoke();
        
        if (_openCards.Count == 2)
        {
            CloseOpenCards();
            StopAllCoroutines();
        }

        clickedCard.ShowCard();
        _openCards.Add(clickedCard);

        if (_openCards.Count == 2)
        {
            StartCoroutine(CheckForMatch());
        }
    }

    private void CloseOpenCards()
    {
        foreach (var card in _openCards)
        {
            card.HideCard();
        }
        _openCards.Clear();
    }

    private IEnumerator CheckForMatch()
    {
        if (_openCards[0].GetValue().Equals(_openCards[1].GetValue()))
        {
            _openCards[0].DisableCard();
            _openCards[1].DisableCard();
            _pairsFound++;
            CheckForWin();
            
            _messageBus.OnCheckMatch?.Invoke(true);
        }
        else
        {
            _messageBus.OnCheckMatch?.Invoke(false);
            yield return new WaitForSeconds(_waitSecondsBeforeClose);
            CloseOpenCards();
        }

        _openCards.Clear();
    }

    private void CheckForWin()
    {
        if (_pairsFound == _totalPairs)
        {
            FlagsStorage.IsAbleToSave = false;
            
            _messageBus.OnClearProgress?.Invoke();
            _messageBus.OnGameOver?.Invoke(true);
        }
    }
}