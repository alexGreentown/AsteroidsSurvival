using System;
using System.Collections.Generic;
using AsteroidsSurvival.Gameplay.Enemy;
using AsteroidsSurvival.Gameplay.Player;
using AsteroidsSurvival.Gameplay.Shot;
using AsteroidsSurvival.Interfaces;
using AsteroidsSurvival.ServiceLocator;
using AsteroidsSurvival.Services;
using AsteroidsSurvival.View.Gameplay.Enemy;
using UnityEngine;

namespace AsteroidsSurvival.Gameplay
{
    public class GameplayObjectsFactory : ILogic
    {
        #region Fields
        
        private PrefabsData _prefabsData;

        // Container where fight objects, ships, asteroids are instantiated into
        private Transform _fightContainer;

        private List<BulletController> _bulletsPool = new();
        #endregion
        
        
        
        #region Methods
        
        public void InitializeData(Transform fightContainer)
        {
            PrefabsDataService prefabsDataService = MyServiceLocator.Get<PrefabsDataService>();
            _prefabsData = prefabsDataService.PrefabsData;

            _fightContainer = fightContainer;
        }
        
        public PlayerController CreatePlayer()
        {
            GameObject playerPrefab = _prefabsData.PlayerController;
            GameObject newObject = GameObject.Instantiate(playerPrefab, _fightContainer);
            PlayerController player = newObject.GetComponent<PlayerController>();
            
            InputControllerService inputControllerService = MyServiceLocator.Get<InputControllerService>();
            player.InputController = inputControllerService;
            
            return player;
        }

        public BulletController CreateBullet()
        {
            int lastBullet = _bulletsPool.Count - 1;
            if (_bulletsPool.Count > 0 && _bulletsPool[lastBullet].IsDestroyed)
            {
                BulletController bulletFromPool = _bulletsPool[lastBullet];
                _bulletsPool.RemoveAt(lastBullet);
                bulletFromPool.gameObject.SetActive(true);
                return bulletFromPool;
            }
            
            GameObject bulletPrefab = _prefabsData.BulletController;
            GameObject newObject = GameObject.Instantiate(bulletPrefab, _fightContainer);
            BulletController bullet = newObject.GetComponent<BulletController>();
            
            return bullet;
        }

        public AsteroidController CreateAsteroid()
        {
            GameObject asteroidPrefab = _prefabsData.AsteroidController;
            GameObject newObject = GameObject.Instantiate(asteroidPrefab, _fightContainer);
            AsteroidController asteroid = newObject.GetComponent<AsteroidController>();
            asteroid.Initialize();
            return asteroid;
        }

        public GameObject CreateExplosion()
        {
            GameObject explosionPrefab = _prefabsData.Explosion;
            GameObject newObject = GameObject.Instantiate(explosionPrefab, _fightContainer);
            return newObject;
        }
        
        public UFOController CreateUFO()
        {
            GameObject UFOPrefab = _prefabsData.UFOController;
            GameObject newObject = GameObject.Instantiate(UFOPrefab, _fightContainer);
            UFOController UFO = newObject.GetComponent<UFOController>();
            return UFO;
        }
        
        public float GetAsteroidsDelayValue()
        {
            float asteroidsDelayValue = _prefabsData.AsteroidsCreateDelayValue;
            return asteroidsDelayValue;
        }
        
        public float GetUFODelayValue()
        {
            float asteroidsDelayValue = _prefabsData.UFOCreateDelayValue;
            return asteroidsDelayValue;
        }

        public void AddBulletToBulletsPool(BulletController targetBullet)
        {
            _bulletsPool.Add(targetBullet);
        }

        public Sprite GetBulletSprite(BulletType bulletType)
        {
            switch (bulletType)
            {
                case BulletType.PLAYER:
                    return _prefabsData.GraphicsPlayerBullet;
                    break;
                case BulletType.ENEMY:
                    return _prefabsData.GraphicsEnemyBullet;
                    break;
                default:
                    return _prefabsData.GraphicsEnemyBullet;
            }
        }
        
        #endregion

    }
}
