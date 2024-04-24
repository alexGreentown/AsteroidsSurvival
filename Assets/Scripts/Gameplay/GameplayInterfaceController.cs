using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AsteroidsSurvival.Gameplay
{
    public class GameplayInterfaceController : MonoBehaviour
    {
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private TMP_Text _coordinatesText;
        [SerializeField] private TMP_Text _angleText;
        [SerializeField] private TMP_Text _speedText;
        [SerializeField] private TMP_Text _laserCountText;
        [SerializeField] private TMP_Text _laserFillText;
        [SerializeField] private Image _laserFillImage;

        public void UpdatePlayerData(PlayerDataSet playerDataSet)
        {
            _scoreText.text = playerDataSet.Score.ToString();
            
            string stringX = String.Format("{0:0}", playerDataSet.Coordinates.x);
            string stringY = String.Format("{0:0}", playerDataSet.Coordinates.y);
            string coordinatesString = $"x:{stringX} y:{stringY}";
            _coordinatesText.text = coordinatesString;
            
            string stringAngle = String.Format("{0:0}", playerDataSet.Angle);
            _angleText.text = stringAngle;
            
            _speedText.text = playerDataSet.Speed.ToString();
            _laserCountText.text = playerDataSet.LaserCount.ToString();
            _laserFillText.text = playerDataSet.LaserFill.ToString();
        }
    }
}