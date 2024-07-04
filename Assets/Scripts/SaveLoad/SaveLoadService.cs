using Cysharp.Threading.Tasks;
using Data;
using UnityEngine;
using Utils.Extensions;

namespace SaveLoad
{
    public class SaveLoadService
    {
        private const string ProgressKey = "Progress";

        public static void SaveProgress(PlayerProgress playerProgress)
        {
            PlayerPrefs.SetString(ProgressKey, playerProgress.ToJson());
            PlayerPrefs.Save();
        }

        public static PlayerProgress LoadProgress()
        {
            var stringProgress = PlayerPrefs.GetString(ProgressKey);
            var progress = stringProgress.ToDeserialized<PlayerProgress>();
            return progress;
        }

        public static void ResetProgress()
        {
            PlayerPrefs.DeleteKey(ProgressKey);
        }
    }
}