using Cysharp.Threading.Tasks;
using UnityEngine;
using Utils.Extensions;

namespace SaveLoad
{
    public class SaveLoadService
    {
        private const string ProgressKey = "Progress";

        public static async UniTaskVoid SaveProgress(PlayerProgress playerProgress)
        {
            PlayerPrefs.SetString(ProgressKey, playerProgress.ToJson());
            PlayerPrefs.Save();
            await UniTask.CompletedTask;
        }

        public static PlayerProgress LoadProgress()
        {
            var stringProgress = PlayerPrefs.GetString(ProgressKey);
            var progress = stringProgress.ToDeserialized<PlayerProgress>();
            return progress;
        }
    }
}