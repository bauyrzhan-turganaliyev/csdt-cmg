using UnityEngine;

namespace DefaultNamespace
{
    //[CreateAssetMenu(fileName = "PlayerProgress", menuName = "ScriptableObjects/PlayerProgress", order = 1)]
    public class PlayerProgressSO : ScriptableObject
    {
        public ScoreData ScoreData;
        public GridData GridData;

        public bool HasProgress;
    }
}