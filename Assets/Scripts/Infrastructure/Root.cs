using System;
using UnityEngine;

namespace Infrastructure
{
    public class Root : MonoBehaviour
    {
        [SerializeField] private GameController _gameController;
        
        private void Awake()
        {
            _gameController.Init();
        }
    }
}
