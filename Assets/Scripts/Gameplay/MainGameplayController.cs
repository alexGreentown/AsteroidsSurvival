using System;
using System.Collections.Generic;
using AsteroidsSurvival.Interfaces;
using AsteroidsSurvival.Services;
using UnityEngine;

namespace AsteroidsSurvival.Gameplay
{
    public class MainGameplayController : MonoBehaviour, IObservable
    {
        #region Fields
        [SerializeField] private GameplayInterfaceController _interfaceController;
        [SerializeField] private FightField _fightField;
        
        [SerializeField] private GameOverUI _gameOverController;
        
        private PrefabsData _prefabsData;
        private List<IObserver> _observers = new();
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

            NotifyObservers();
        }

        public void UpdateGame()
        {
            _fightField.UpdateFight();
        }
        
        #endregion



        #region IObservable implementation
        public void AddObserver(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void RemoveObserver(IObserver observer)
        {
            if (_observers.Contains(observer))
            {
                _observers.Remove(observer);
            }
            else
            {
                throw new NotImplementedException("_observers doesnt contain observer!");
            }
        }

        public void NotifyObservers()
        {
            foreach (IObserver observer in _observers)
            {
                observer.GetNotification();
            }
        }
        #endregion
    }

    public class PlayerDataSet
    {
        public Vector2 Coordinates = new();
        public float Angle;
        public float Speed;
        public int BulletsCount;
        public float LaserFill;
        public int Score;
    }
}
