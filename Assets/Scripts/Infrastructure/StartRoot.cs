using SaveLoad;
using UnityEngine;

namespace Infrastructure
{
     public class StartRoot : MonoBehaviour
     {
          [SerializeField] private StartMenuService _startMenuService;
          private PlayerProgress _playerProgress;

          private void Awake()
          {
               _playerProgress = SaveLoadService.LoadProgress() ?? new PlayerProgress();
               
               _startMenuService.Init(_playerProgress);
          }
     }
}
