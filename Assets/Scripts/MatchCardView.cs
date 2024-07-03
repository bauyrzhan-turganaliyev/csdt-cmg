using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MatchCardView : MonoBehaviour
{
    [SerializeField] private Button _button;
    
    [SerializeField] private GameObject _backSide;
    [SerializeField] private GameObject _frontSide;

    [SerializeField] private Image _image;
    [SerializeField] private TMP_Text _text;

    public Action OnClick;
    
    public void Init<T>(T value)
    {
        _button.onClick.AddListener(OnClicked);

        SetupContent(value);
    }

    private void SetupContent<T>(T value)
    {
        switch (value)
        {
            case Color color:
                _image.color = color;
                break;
            case Sprite sprite:
                _image.sprite = sprite;
                break;
            case string symbols:
                _text.text = symbols;
                break;
        }
    }

    private void OnClicked()
    {
        print("card clicked");
        _frontSide.gameObject.SetActive(true);
        OnClick?.Invoke();
    }

    private void OnDestroy()
    {
        OnClick = null;
    }


}