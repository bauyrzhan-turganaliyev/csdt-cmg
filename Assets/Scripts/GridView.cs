using UnityEngine;
using UnityEngine.UI;

public class GridView : MonoBehaviour
{
    [SerializeField] private GridLayoutGroup _gridLayoutGroup;
    [SerializeField] private Transform _cardsParent;

    public void SetupFixedWidth(int width)
    {
        _gridLayoutGroup.constraintCount = width;
    }

    public Transform GetCardsParent()
    {
        return _cardsParent;
    }
}