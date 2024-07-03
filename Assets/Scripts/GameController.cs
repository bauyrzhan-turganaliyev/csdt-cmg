using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GridService _gridService;
    private List<MatchCardView> _openCards = new List<MatchCardView>();
    private int _totalPairs;
    private int _pairsFound;

    public void Init()
    {
        _gridService.Init();
        SubscribeToCardClicks();
        _totalPairs = _gridService.GetTotalPairs();
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
        }
        else
        {
            yield return new WaitForSeconds(2);
            CloseOpenCards();
        }

        _openCards.Clear();
    }
    
    private void CheckForWin()
    {
        if (_pairsFound == _totalPairs)
        {
            Debug.Log("You win!");
            // Здесь можно добавить любую логику, которая должна выполняться при победе
        }
    }
}