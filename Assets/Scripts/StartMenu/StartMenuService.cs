using DefaultNamespace;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure
{
    public class StartMenuService : MonoBehaviour
    {
        private const int GAME_SCENE_INDEX = 1;

        [SerializeField] private PlayerProgressSO _playerProgressSo;
        [SerializeField] private StartMenuView _startMenuView;

        private PlayerProgress _playerProgress;

        public void Init(PlayerProgress playerProgress)
        {
            _playerProgress = playerProgress;

            _startMenuView.OnStartGame += StartGame;
            _startMenuView.Init(_playerProgress);
        }

        private void StartGame(PlayerProgress playerProgress)
        {
            _playerProgressSo.ScoreData = playerProgress.ScoreData;
            _playerProgressSo.GridData = playerProgress.GridData;
            _playerProgressSo.HasProgress = playerProgress.HasProgress;
            
            SceneManager.LoadScene(sceneBuildIndex: GAME_SCENE_INDEX);
        }
    }
}