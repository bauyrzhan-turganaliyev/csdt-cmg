using Configs;
using UnityEngine;
using UnityEngine.Serialization;

public class GridService : MonoBehaviour
{
    [SerializeField] private GameConfig _gameConfig;
    [SerializeField] private GridView _gridView;
    [FormerlySerializedAs("_matchCardPrefab")] [SerializeField] private MatchCardView matchCardViewPrefab;

    public void Init()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        _gridView.SetupFixedWidth(_gameConfig.GridWidth);

        Transform cardsParent = _gridView.GetCardsParent();
        int cardsCount = _gameConfig.GridWidth * _gameConfig.GridHeight;
        
        for (int i = 0; i < cardsCount; i++)
        {
            MatchCardView cardView = Instantiate(matchCardViewPrefab, cardsParent);
            cardView.Init();
        }
    }
}