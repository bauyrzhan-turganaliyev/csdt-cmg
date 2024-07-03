using System;
using System.Collections;
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
        StartCoroutine(FlipCard());
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
            StartCoroutine(FlipCard());
        }
    }

    private IEnumerator FlipCard()
    {
        isFlipping = true;

        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(0, 180, 0);

        float time = 0;
        while (time < 1)
        {
            time += Time.deltaTime * 2; // Скорость переворота
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, time);
            if (time >= 0.5f && time < 0.55f) // Переключение сторон на полпути переворота
            {
                isFront = !isFront;
                _frontSide.SetActive(!isFront);
                _backSide.SetActive(isFront);
            }
            yield return null;
        }

        transform.rotation = endRotation; // Убедитесь, что карта полностью перевернута

        isFlipping = false;
    }

    public void HideCard()
    {
        if (isFlipping) return;
        StartCoroutine(FlipCardBack());
    }

    private IEnumerator FlipCardBack()
    {
        isFlipping = true;

        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(0, -180, 0);

        float time = 0;
        while (time < 1)
        {
            time += Time.deltaTime * 2; // Скорость переворота
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, time);
            if (time >= 0.5f && time < 0.55f) // Переключение сторон на полпути переворота
            {
                isFront = !isFront;
                _frontSide.SetActive(!isFront);
                _backSide.SetActive(isFront);
            }
            yield return null;
        }

        transform.rotation = endRotation; // Убедитесь, что карта полностью перевернута

        isFlipping = false;
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