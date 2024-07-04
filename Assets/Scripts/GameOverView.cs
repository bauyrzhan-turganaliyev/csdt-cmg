using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverView : MonoBehaviour
{
    [SerializeField] private TMP_Text _titleText;
    [SerializeField] private Button _toMenuButton;

    public Action OnBackMenu;
        
    public void Init()
    {
        _toMenuButton.onClick.AddListener((() => OnBackMenu?.Invoke()));
    }

    public void SetTitleText(string titleText)
    {
        _titleText.text = titleText;
    }
}