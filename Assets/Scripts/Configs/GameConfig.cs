using UnityEditor;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "Configs", menuName = "Configs/GameConfig", order = 1)]
    public class GameConfig : ScriptableObject
    {
        public int GridWidth;
        public int GridHeight;
        public PoolType PoolType;
        
        public MatchCardPool Pool;
    }

    public enum PoolType
    {
        Colors,
        Sprites,
        Symbols,
    }

    [System.Serializable]
    public class MatchCardPool
    {
        public Color[] Colors;
        public Sprite[] Sprites;
        public string[] Symbols;
    }
    
#if UNITY_EDITOR
    [CustomEditor(typeof(GameConfig))]
    public class GameConfigEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            GameConfig gameConfig = (GameConfig)target;

            EditorGUILayout.LabelField("Grid Settings", EditorStyles.boldLabel);
            gameConfig.GridWidth = EditorGUILayout.IntSlider("Grid Width", gameConfig.GridWidth, 1, 6);
            gameConfig.GridHeight = EditorGUILayout.IntSlider("Grid Height", gameConfig.GridHeight, 1, 6);

            if ((gameConfig.GridWidth * gameConfig.GridHeight) % 2 != 0)
            {
                EditorGUILayout.HelpBox("The product of Grid Width and Grid Height must be even.", MessageType.Error);
            }

            gameConfig.PoolType = (PoolType)EditorGUILayout.EnumPopup("Pool Type", gameConfig.PoolType);

            EditorGUILayout.LabelField("Card Pool", EditorStyles.boldLabel);
            SerializedProperty poolProperty = serializedObject.FindProperty("Pool");
            EditorGUILayout.PropertyField(poolProperty, true);

            serializedObject.ApplyModifiedProperties();
            
            if ((gameConfig.GridWidth * gameConfig.GridHeight) % 2 != 0)
            {
                if (gameConfig.GridWidth % 2 != 0)
                {
                    gameConfig.GridWidth += 1;
                }
                else
                {
                    gameConfig.GridHeight += 1;
                }
            }
        }
    }
#endif
}