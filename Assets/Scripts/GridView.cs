using UnityEngine;
using UnityEngine.UI;

public class GridView : MonoBehaviour
{
    [SerializeField] private GridLayoutGroup _gridLayoutGroup;
    [SerializeField] private RectTransform _cardsParent;
    //[SerializeField] private float _maxCellSize = 150f;

    private void Start()
    {
        UpdateCellSize();
    }

    public void SetupFixedWidth(int width)
    {
        _gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        _gridLayoutGroup.constraintCount = width;
        UpdateCellSize();
    }

    public Transform GetCardsParent()
    {
        return _cardsParent;
    }

    private void UpdateCellSize()
    {
        float parentWidth = _cardsParent.rect.width;
        float parentHeight = _cardsParent.rect.height;

        int width = _gridLayoutGroup.constraintCount;

        float totalSpacingWidth = _gridLayoutGroup.spacing.x * (width - 1);
        float totalPaddingWidth = _gridLayoutGroup.padding.left + _gridLayoutGroup.padding.right;

        float totalSpacingHeight = _gridLayoutGroup.spacing.y * (_cardsParent.childCount / width - 1);
        float totalPaddingHeight = _gridLayoutGroup.padding.top + _gridLayoutGroup.padding.bottom;

        float availableWidth = parentWidth - totalSpacingWidth - totalPaddingWidth;
        float availableHeight = parentHeight - totalSpacingHeight - totalPaddingHeight;

        float cellSizeWidth = availableWidth / width;
        float cellSizeHeight = availableHeight / (_cardsParent.childCount / width);

        float cellSize = Mathf.Min(cellSizeWidth, cellSizeHeight);
        
        //cellSize = Mathf.Min(cellSize, _maxCellSize);

        _gridLayoutGroup.cellSize = new Vector2(cellSize, cellSize);
    }
}