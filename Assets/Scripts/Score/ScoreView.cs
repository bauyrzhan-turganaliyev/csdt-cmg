using TMPro;
using UnityEngine;

namespace Score
{
    public class ScoreView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private TMP_Text _flipText;
        [SerializeField] private TMP_Text _comboText;

        public void SetFlips(int flips)
        {
            _flipText.text = flips.ToString();
        }

        public void SetScore(int score)
        {
            _scoreText.text = score.ToString();
        }

        public void SetCombo(int combo)
        {
            _comboText.text = "x" + combo;
        }
    }
}