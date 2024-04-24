using System;
using AsteroidsSurvival.Services;
using UnityEngine;

namespace AsteroidsSurvival.Gameplay
{
    public class MainGameplayController : MonoBehaviour
    {
        #region Events
        public event Action OnGameExit;
        #endregion
        
        
        
        #region Fields
        [SerializeField] private GameplayInterfaceController _interfaceController;
        [SerializeField] private GameplayFightController _fightController;
        
        [SerializeField] private GameOverUI _gameOverController;
        
        private PrefabsData _prefabsData;
        #endregion
        
        
        
        #region Unity Lifecycle

        private void OnEnable()
        {
            _fightController.OnPlayerDataUpdate += _interfaceController.UpdatePlayerData;
            _fightController.OnGameOver += OnGameOver;
        }
        
        private void OnDisable()
        {
            _fightController.OnPlayerDataUpdate -= _interfaceController.UpdatePlayerData;
            _fightController.OnGameOver -= OnGameOver;
        }

        #endregion
        
        
        
        #region Methods
        
        public void Initialize()
        {
            _gameOverController.gameObject.SetActive(false);

            _fightController.Initialize();
        }
        
        public void StartGameplay()
        {
            _fightController.StartFight();
        }

        private void OnGameOver()
        {
            _gameOverController.gameObject.SetActive(true);
            _gameOverController.SetScoreText(_fightController.PlayerController.EnemiesKilled);
            
            OnGameExit();
        }

        public void UpdateGame()
        {
            _fightController.UpdateFight();
        }
        
        #endregion
        
    }

    public class PlayerDataSet
    {
        public Vector2 Coordinates = new();
        public float Angle;
        public float Speed;
        public int LaserCount;
        public float LaserFill;
        public int Score;
    }
}
