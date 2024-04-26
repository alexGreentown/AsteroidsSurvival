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
        [SerializeField] private FightField _fightField;
        
        [SerializeField] private GameOverUI _gameOverController;
        
        private PrefabsData _prefabsData;
        #endregion
        
        
        
        #region Unity Lifecycle

        private void OnEnable()
        {
            _fightField.OnPlayerDataUpdate += _interfaceController.UpdatePlayerData;
            _fightField.OnGameOver += OnGameOver;
        }
        
        private void OnDisable()
        {
            _fightField.OnPlayerDataUpdate -= _interfaceController.UpdatePlayerData;
            _fightField.OnGameOver -= OnGameOver;
        }

        #endregion
        
        
        
        #region Methods
        
        public void Initialize()
        {
            _gameOverController.gameObject.SetActive(false);

            _fightField.Initialize();
        }
        
        public void StartGameplay()
        {
            _fightField.StartFight();
        }

        private void OnGameOver()
        {
            _gameOverController.gameObject.SetActive(true);
            _gameOverController.SetScoreText(_fightField.PlayerController.EnemiesKilled);

            if (OnGameExit == null)
            {
                throw new NotImplementedException();
            }
            OnGameExit.Invoke();
        }

        public void UpdateGame()
        {
            _fightField.UpdateFight();
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
