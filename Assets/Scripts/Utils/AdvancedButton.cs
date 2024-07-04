using UnityEngine;
using UnityEngine.UI;

namespace Infrastructure
{
    public class AdvancedButton : MonoBehaviour
    {
        public Button Button;
        public Image Image;

        public int Value;

        private Color _initColor;

        private void Start()
        {
            _initColor = Image.color;
        }

        public void SetValue(int value)
        {
            Value = value;
        }

        public void Unselect()
        {
            Image.color = _initColor;
        }

        public void Select()
        {
            Image.color = Color.green;
        }
    }
}