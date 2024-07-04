using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmationView : MonoBehaviour
{
    [SerializeField] private TMP_Text _questionText;
    [SerializeField] private Button _yesButton;
    [SerializeField] private Button _noButton;

    public Action<bool> OnTryConfirm;
        
    public void Init()
    {
        _yesButton.onClick.AddListener((() => OnTryConfirm?.Invoke(true)));
        _noButton.onClick.AddListener((() => OnTryConfirm?.Invoke(false)));
    }
        
    public void SetTitleText(string text = "Are you sure?")
    {
        _questionText.text = text;
    }
}