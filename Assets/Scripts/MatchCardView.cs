using System;
using System.Collections;
using DG.Tweening;
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
    private object _value;
    private bool isFlipping = false;
    private bool isFront = false;

    public void Init<T>(T value)
    {
        _button.onClick.AddListener(OnClicked);
        _value = value;
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
        if (isFlipping) return;
        FlipCard();
        OnClick?.Invoke();
    }

    public object GetValue()
    {
        return _value;
    }

    public void ShowCard()
    {
        if (!isFront)
        {
            FlipCard();
        }
    }

    private void FlipCard()
    {
        isFlipping = true;

        Sequence flipSequence = DOTween.Sequence();
        flipSequence.Append(transform.DORotate(new Vector3(0, 90, 0), 0.25f))
                     .AppendCallback(() =>
                     {
                         isFront = !isFront;
                         _frontSide.SetActive(!isFront);
                         _backSide.SetActive(isFront);
                     })
                     .Append(transform.DORotate(new Vector3(0, 180, 0), 0.25f))
                     .AppendCallback(() => isFlipping = false);
    }

    public void HideCard()
    {
        Sequence flipBackSequence = DOTween.Sequence();
        flipBackSequence.Append(transform.DORotate(new Vector3(0, 90, 0), 0.25f))
                        .AppendCallback(() =>
                        {
                            isFront = !isFront;
                            _frontSide.SetActive(!isFront);
                            _backSide.SetActive(isFront);
                        })
                        .Append(transform.DORotate(new Vector3(0, 0, 0), 0.25f))
                        .AppendCallback(() => isFlipping = false);
    }

    public void DisableCard()
    {
        _button.interactable = false;
        // Можно добавить дополнительную логику для отключения карты, например, сделать её прозрачной
    }

    private void OnDestroy()
    {
        OnClick = null;
    }
}