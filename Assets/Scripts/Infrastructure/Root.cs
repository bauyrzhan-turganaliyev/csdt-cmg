using System;
using UnityEngine;

namespace Infrastructure
{
    public class Root : MonoBehaviour
    {
        [SerializeField] private GridService _gridService;
        
        private void Awake()
        {
            _gridService.Init();
        }
    }
}
