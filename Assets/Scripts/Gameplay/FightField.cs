using System;
using System.Collections;
using System.Collections.Generic;
using AsteroidsSurvival.Gameplay.Enemy;
using AsteroidsSurvival.View;
using UnityEngine;
using AsteroidsSurvival.Gameplay.Player;
using AsteroidsSurvival.Gameplay.Shot;
using AsteroidsSurvival.Interfaces;
using AsteroidsSurvival.View.Gameplay.Asteroid;

namespace AsteroidsSurvival.Gameplay
{
    public class FightField : MonoBehaviour, IView
    {
        #region Events
        public event Action<PlayerDataSet> OnPlayerDataUpdate;
        public event Action OnGameOver;
        #endregion
        
        
        
        #region Fields and Properties
        
        private PlayerController _playerController;
        private List<IEnemy> _enemiesList = new();
        private List<BulletController> _bulletsList = new();

        private GameplayObjectsFactory _objectsFactory = new();

        private Coroutine CreateAsteroidsCoroutine;
        private Coroutine CreateUFOCoroutine;
        
        private float _asteroidsCreateDelay = 5f;
        private float _UFOCreateDelay = 8f;

        private PlayerDataSet _playerDataSet = new();

        private GameplayFightLogic _gameplayFightLogic = new();

        public PlayerController PlayerController => _playerController;

        public List<BulletController> BulletsList => _bulletsList;

        public List<IEnemy> EnemiesList => _enemiesList;

        #endregion
        
        
        
        #region Unity LifeCycle

        private void Awake()
        {
            _gameplayFightLogic.Initialize(this);
        }

        #endregion
        
        

        #region Methods

        public void UpdateFight()
        {
            _playerController.GetPlayerData(_playerDataSet);
            OnPlayerDataUpdate(_playerDataSet);

            _gameplayFightLogic.MyUpdate();
            
            for (int i = _enemiesList.Count; i-- > 0;)
            {
                if (_enemiesList[i] is AsteroidController asteroid)
                {
                    if (asteroid.IsDivided)
                    {
                        asteroid.IsDivided = false;
                        float randomDirection = UnityEngine.Random.Range(0f, 360f);
                        asteroid.AsteroidStrategy = new AsteroidStrategySmall();
                        asteroid.Initialize(randomDirection + 25f);
                        
                        AsteroidController secondAsteroid = _objectsFactory.CreateAsteroid();
                        secondAsteroid.AsteroidStrategy = new AsteroidStrategySmall();
                        secondAsteroid.Initialize(randomDirection - 25f);
                        secondAsteroid.Transform.position = asteroid.Transform.position;
                        
                        EnemiesList.Add(secondAsteroid);
                        return;
                    }
                }
            }
        }
        
        public void StartFight()
        {
            InitializePlayer();
            InitializeUFO();
            InitializeAsteroids();
        }

        public void Initialize()
        {
            _objectsFactory.InitializeData(this.transform);

            _asteroidsCreateDelay = _objectsFactory.GetAsteroidsDelayValue();
            _UFOCreateDelay = _objectsFactory.GetUFODelayValue();
        }

        private void PlaceOnEmptyCell(IEnemy newEnemy)
        {
            // place newEnemy on a random position
            Vector2 position;
            position.x = UnityEngine.Random.Range(0f, Screen.width);
            position.y = UnityEngine.Random.Range(0f, Screen.height);
            // make sure there is no player or other enemy on that point
            while (!IsEmptyCell(position, newEnemy))
            {
                position.x = UnityEngine.Random.Range(0f, Screen.width);
                position.y = UnityEngine.Random.Range(0f, Screen.height);
            }
            
            newEnemy.Transform.position = position;
        }

        /// <summary>
        /// return true if newEnemy position is not occupied by another enemy or player
        /// </summary>
        private bool IsEmptyCell(Vector2 position, IEnemy newEnemy)
        {
            Vector2 positionDifference;
            foreach (var enemy in _enemiesList)
            {
                positionDifference.x = position.x - enemy.Transform.position.x;
                positionDifference.y = position.y - enemy.Transform.position.y;
                if (positionDifference.magnitude <= newEnemy.Radius + enemy.Radius)
                {
                    return false;
                }
            }
            
            positionDifference.x = position.x - _playerController.transform.position.x;
            positionDifference.y = position.y - _playerController.transform.position.y;
            if (positionDifference.magnitude <= newEnemy.Radius + _playerController.Radius)
            {
                return false;
            }

            return true;
        }
        #endregion
        
        
        
        #region Player methods
        private void InitializePlayer()
        {
            _playerController = _objectsFactory.CreatePlayer();
            
            _playerController.OnMakeShot += InitializeBullet;
            _playerController.OnPlayerKilled += KillPlayer;

            _gameplayFightLogic.SetPlayerTransform();
        }

        private void KillPlayer()
        {
            _playerController.OnMakeShot -= InitializeBullet;
            _playerController.OnPlayerKilled -= KillPlayer;
            
            StopCoroutine(CreateUFOCoroutine);
            StopCoroutine(CreateAsteroidsCoroutine);
            
            if (OnGameOver == null)
            {
                throw new NotImplementedException("OnGameOver event is missing");
            }
            OnGameOver();
        }

        #endregion



        #region UFO Methods
        private void InitializeUFO()
        {
            // Each 8 seconds 1 UFO is being added
            CreateUFOCoroutine = StartCoroutine(CreateUFOEnumerator());
        }

        private IEnumerator CreateUFOEnumerator()
        {
            while (true)
            {
                yield return new WaitForSeconds(_UFOCreateDelay);
                UFOController newUFO = _objectsFactory.CreateUFO();
                newUFO.Initialize(_playerController);

                PlaceOnEmptyCell(newUFO);
                
                newUFO.OnMakeShot += InitializeEnemyBullet;
                
                EnemiesList.Add(newUFO);
            }
        }

        #endregion



        #region Asteroids Methods
        private void InitializeAsteroids()
        {
            // Initially there are 3  Asteroids in scene and next each 5 seconds 1 Asteroid is being added
            int initialAsteroidsNum = 3;
            for (int i = 0; i < initialAsteroidsNum; i++)
            {
                AsteroidController newAsteroid = _objectsFactory.CreateAsteroid();
                PlaceOnEmptyCell(newAsteroid);
                EnemiesList.Add(newAsteroid);
            }

            CreateAsteroidsCoroutine = StartCoroutine(CreateAsteroidsEnumerator());
        }
        
        private IEnumerator CreateAsteroidsEnumerator()
        {
            while (true)
            {
                yield return new WaitForSeconds(_asteroidsCreateDelay);
                AsteroidController newAsteroid = _objectsFactory.CreateAsteroid();
                PlaceOnEmptyCell(newAsteroid);
                EnemiesList.Add(newAsteroid);
            }
        }
        #endregion



        #region Bullets Methods
        private void InitializeBullet(float rotation, Vector2 position)
        {
            BulletController newBullet = _objectsFactory.CreateBullet();
            
            newBullet.Initialize(rotation, position);
            newBullet.BulletType = BulletType.PLAYER; 
            Sprite bulletSprite = _objectsFactory.GetBulletSprite(BulletType.PLAYER);
            newBullet.SetSprite(bulletSprite);
            
            newBullet.OnBulletDestroy += DestroyBullet;
            
            BulletsList.Add(newBullet);
        }

        private void DestroyBullet(BulletController targetBullet)
        {
            targetBullet.gameObject.SetActive(false);
            _objectsFactory.AddToBulletsPool(targetBullet);
            StartCoroutine(RemoveFromBulletsListInNextFrame(targetBullet));
        }

        private IEnumerator RemoveFromBulletsListInNextFrame(BulletController targetBullet)
        {
            yield return null;
            _bulletsList.Remove(targetBullet);
        }

        private void InitializeEnemyBullet(float rotation, Vector2 position)
        {
            BulletController newBullet = _objectsFactory.CreateBullet();
            
            newBullet.Initialize(rotation, position);
            newBullet.BulletType = BulletType.ENEMY;
            Sprite bulletSprite = _objectsFactory.GetBulletSprite(BulletType.ENEMY);
            newBullet.SetSprite(bulletSprite);
            
            newBullet.OnBulletDestroy += DestroyBullet;
            
            BulletsList.Add(newBullet);
        }

        public void EnemyKilled(IEnemy enemy)
        {
            if (enemy is AsteroidController asteroid)
            {
                if (asteroid.AsteroidStrategy is AsteroidStrategyBig)
                {
                    asteroid.IsDivided = true;
                    return;
                }
                else
                {
                    _objectsFactory.AddToAsteroidsPool(asteroid);
                }
            }
            
            GameObject explosion = _objectsFactory.CreateExplosion();
            MonoBehaviour enemyTransform = enemy as MonoBehaviour; 
            explosion.transform.position = enemyTransform.transform.position;
            enemyTransform.gameObject.SetActive(false);

            StartCoroutine(RemoveFromEnemyListInNextFrame(enemy));

            if (enemy is UFOController ufo)
            {
                ufo.OnMakeShot -= InitializeEnemyBullet;
            }

            _playerController.EnemiesKilled++;
        }

        private IEnumerator RemoveFromEnemyListInNextFrame(IEnemy enemy)
        {
            yield return null;
            _enemiesList.Remove(enemy);
        }

        #endregion
        
    }
}
