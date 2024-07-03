using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Infrastructure;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GridService _gridService;
    private List<MatchCardView> _openCards = new List<MatchCardView>();
    private int pairsFound = 0;
    private int totalPairs;
    private MessageBus _messageBus;

    public void Init(MessageBus messageBus)
    {
        _messageBus = messageBus;
        
        _gridService.Init();
        SubscribeToCardClicks();
        totalPairs = _gridService.GetTotalPairs();
    }

    private void SubscribeToCardClicks()
    {
        foreach (MatchCardView card in _gridService.GetAllCards())
        {
            card.OnClick += () => OnCardClicked(card);
        }
    }

    private void OnCardClicked(MatchCardView clickedCard)
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
            pairsFound++;
            CheckForWin();
            _messageBus.OnCheckMatch?.Invoke(true);
        }
        else
        {
            _messageBus.OnCheckMatch?.Invoke(false);
            yield return new WaitForSeconds(2); // Дождитесь завершения анимации несоответствия
            CloseOpenCards();
        }

        _openCards.Clear();
    }

    private void CheckForWin()
    {
        if (pairsFound == totalPairs)
        {
            Debug.Log("You win!");
            // Здесь можно добавить любую логику, которая должна выполняться при победе
        }
    }
}