using AsteroidsSurvival.Gameplay.Enemy;
using AsteroidsSurvival.Gameplay.Player;
using AsteroidsSurvival.Gameplay.Shot;
using AsteroidsSurvival.Interfaces;
using AsteroidsSurvival.ServiceLocator;
using AsteroidsSurvival.Services;
using AsteroidsSurvival.View.Gameplay.Asteroid;
using UnityEngine;
using AsteroidsSurvival.Utils;

namespace AsteroidsSurvival.Gameplay
{
    public class GameplayObjectsFactory : ILogic
    {
        #region Fields
        
        private PrefabsData _prefabsData;

        // Container where fight objects, ships, asteroids are instantiated into
        private Transform _fightContainer;

        private GenericPool<BulletController> _bulletsPool = new();
        private GenericPool<AsteroidController> _asteroidsPool = new();
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
            player.Initialize();
            
            
            
            return player;
        }

        public BulletController CreateBullet()
        {
            BulletController bulletFromPool = _bulletsPool.Get();
            if(bulletFromPool != null)
            {
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
            AsteroidController asteroidFromPool = _asteroidsPool.Get();
            if(asteroidFromPool != null)
            {
                asteroidFromPool.gameObject.SetActive(true);
                return asteroidFromPool;
            }
            
            GameObject asteroidPrefab = _prefabsData.AsteroidController;
            GameObject newObject = GameObject.Instantiate(asteroidPrefab, _fightContainer);
            AsteroidController asteroid = newObject.GetComponent<AsteroidController>();
            asteroid.Initialize();
            return asteroid;
        }

        public void AddToAsteroidsPool(AsteroidController asteroid)
        { 
            _asteroidsPool.Add(asteroid);
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

        public void AddToBulletsPool(BulletController targetBullet)
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
