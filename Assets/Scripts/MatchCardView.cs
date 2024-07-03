using System;
using UnityEngine;
using UnityEngine.UI;

public class MatchCardView : MonoBehaviour
{
    [SerializeField] private Button _button;
    
    [SerializeField] private GameObject _backSide;
    [SerializeField] private GameObject _frontSide;

    public Action OnClick;
    
    public void Init()
    {
        _button.onClick.AddListener(OnClicked);
    }
    
    private void OnClicked()
    {
        print("card clicked");
        OnClick?.Invoke();
    }

    private void OnDestroy()
    {
        OnClick = null;
    }
}