using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "Configs", menuName = "Configs/GameConfig", order = 1)]
    public class GameConfig : ScriptableObject
    {
        public int GridWidth;
        public int GridHeight;
    }
}