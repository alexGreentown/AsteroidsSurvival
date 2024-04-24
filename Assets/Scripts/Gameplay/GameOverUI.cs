using TMPro;
using UnityEngine;

namespace AsteroidsSurvival.Gameplay
{
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _scoreText;

        public void SetScoreText(int score)
        {
            _scoreText.text = score.ToString();
        }
    }
}
